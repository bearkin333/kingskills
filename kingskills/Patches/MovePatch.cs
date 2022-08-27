using BepInEx;
using HarmonyLib;
using Jotunn.Entities;
using Jotunn.Managers;
using kingskills.Commands;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace kingskills
{
    [HarmonyPatch(typeof(Player))]
    class MovePatch
    {
        [HarmonyPatch(nameof(Player.GetRunSpeedFactor))]
        [HarmonyPrefix]
        public static bool GetRunSpeedPatch(Player __instance, ref float __result)
        {
            if (!ConfigManager.IsSkillActive(Skills.SkillType.Run))
            {
                return true;
            }

            //This function provides a number to multiply the base run speed by
            float runSkillFactor = ConfigManager.GetRunSpeedMult(__instance.m_skills.GetSkillFactor(Skills.SkillType.Run));

            float runSpeed = runSkillFactor * GetGenericMovespeedAdjustments(__instance);
            __result = runSpeed;
            /*
            Jotunn.Logger.LogMessage($"Skill factor was {skillFactor},\n" +
                $"runSkill factor was {runSkillFactor},\n" +
                $"equipment malus redux was {equipmentMalusRedux},\n" +
                $"equipment factor was {equipmentFactor},\n" +
                $"encumberance percent was {encumberancePercent},\n" +
                $"encumberance percent curved was {encumberancePercentCurved},\n" +
                $"skill encumberance redux was {skillEncumberanceRedux},\n" +
                $"encumberance factor was {encumberanceFactor},\n" +
                $"total run speed was was {runSpeed},\n");*/

            //Returning false skips the original implementation of GetRunSpeedFactor
            return false;
        }

        [HarmonyPatch(nameof(Player.GetJogSpeedFactor))]
        [HarmonyPrefix]
        public static bool GetJogSpeedPatch(Player __instance, ref float __result)
        {
            __result = GetGenericMovespeedAdjustments(__instance);
            return false;
        }

        public static float GetGenericMovespeedAdjustments(Player player){
            float mod = 1f;
            mod *= GetSkillMovespeedMod(player) 
                * GetEncumberanceFactor(player) 
                * GetEquipmentFactor(player);
            return mod;
        }

        public static float GetSkillMovespeedMod(Player player)
        {
            float mod = 1f;

            float modAddition = 1f;

            if (ConfigManager.IsSkillActive(Skills.SkillType.Unarmed))
                modAddition += ConfigManager.GetFistMovespeedMod(player.GetSkillFactor(Skills.SkillType.Unarmed));

            if (ConfigManager.IsSkillActive(Skills.SkillType.Run))
                modAddition += ConfigManager.GetKnifeMovespeedMod(player.GetSkillFactor(Skills.SkillType.Knives));

            return mod * modAddition;
        }

        // 
        // Experience determining functions
        //
        public static float GetEncumberanceFactor(Player player)
        {
            if (!ConfigManager.IsSkillActive(Skills.SkillType.Run)) return 1f;

            float skillFactor = player.GetSkillFactor(Skills.SkillType.Run);
            float encumberancePercent = Mathf.Clamp01(player.GetInventory().GetTotalWeight() / player.GetMaxCarryWeight());
            float encumberanceCurved = ConfigManager.GetEncumberanceCurveMult(encumberancePercent);
            float skillEncumberanceRedux = ConfigManager.GetEncumberanceRedux(skillFactor);
            return encumberanceCurved * skillEncumberanceRedux;
        }
        public static float GetEquipmentFactor(Player player)
        {
            if (!ConfigManager.IsSkillActive(Skills.SkillType.Run)) return 1f;

            float skillFactor = player.GetSkillFactor(Skills.SkillType.Run);
            float equipmentMalusRedux = ConfigManager.GetEquipmentRedux(skillFactor);
            float equipmentFactor = player.m_equipmentMovementModifier;
            if (equipmentFactor < 0f) { equipmentFactor *= equipmentMalusRedux; }

            return equipmentFactor + 1f;
        }
        public static float absoluteWeightBonus(Player player)
        {
            float weightPercent = ConfigManager.GetAbsoluteWeightMod(player.m_inventory.GetTotalWeight());
            return ConfigManager.GetAbsoluteWeightCurveMult(weightPercent);
        }
        public static float relativeWeightBonus(Player player)
        {
            float weightPercent = player.GetInventory().GetTotalWeight() / player.GetMaxCarryWeight();
            return ConfigManager.GetRelativeWeightStageMult(weightPercent);
        }
        public static float runSpeedExpBonus(Player player)
        {
            float runMod = player.GetRunSpeedFactor();
            player.m_seman.ApplyStatusEffectSpeedMods(ref runMod);

            return runMod * ConfigManager.GetRunEXPSpeedMult();
        }
        public static float swimSpeedExpBonus(Player player)
        {
            float swimMod = player.m_swimSpeed;
            player.m_seman.ApplyStatusEffectSpeedMods(ref swimMod);

            return swimMod * ConfigManager.GetSwimXPSpeedMult();
        }
    }
       
    class FallPatch : Character
    {   
        public void FallDamageOverride()
            {
            float fallHeight = Mathf.Max(0f, m_maxAirAltitude - this.GetTransform().position.y);
            float skillFactor = GetSkillFactor(Skills.SkillType.Jump);

            float fallThreshold = 4;
            if (ConfigManager.IsSkillActive(Skills.SkillType.Jump))
                fallThreshold = ConfigManager.GetFallDamageThreshold(skillFactor);
                
            if (IsPlayer() && fallHeight > fallThreshold)
            {
                HitData hitData = new HitData();

                float fallDamage = (fallHeight - fallThreshold) * 8.33f;
                float expGained = fallDamage;

                if (ConfigManager.IsSkillActive(Skills.SkillType.Jump))
                    fallDamage *= ConfigManager.GetFallDamageRedux(skillFactor);

                hitData.m_damage.m_damage = fallDamage;
                hitData.m_point = m_lastGroundPoint;
                hitData.m_dir = m_lastGroundNormal;
                    
                Damage(hitData);

                //Jotunn.Logger.LogMessage("Jump exp just increased by " + expGained + " thanks to fall damage");
                if (ConfigManager.IsSkillActive(Skills.SkillType.Jump))
                   RaiseSkill(Skills.SkillType.Jump, expGained);
            }
        }
    }

    [HarmonyPatch(typeof(Character), "UpdateGroundContact")]
    class JumpExp
    { 
        static ConstructorInfo hitDataConstructor = AccessTools.Constructor(typeof(HitData));
        static MethodInfo resetGroundContactMTD = AccessTools.Method(typeof(Character), "ResetGroundContact");
        static MethodInfo fallDamageOverrideMTD = AccessTools.DeclaredMethod(typeof(FallPatch), nameof(FallPatch.FallDamageOverride));

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instruct)
        {    
            var newInstructions = new List<CodeInstruction>(instruct);
            bool flaggedNop = false;
                
            for (int i = 0; i < newInstructions.Count(); i++)
            {
                var instruction = newInstructions[i];
                if (flaggedNop)
                {
                    if (newInstructions[i + 1].Calls(resetGroundContactMTD))
                    {
                        newInstructions.Insert(i + 1, new CodeInstruction(OpCodes.Call, fallDamageOverrideMTD));
                       newInstructions.Insert(i + 2, new CodeInstruction(OpCodes.Ldarg_0));
                        break;
                    }
                    newInstructions[i].opcode = OpCodes.Nop;
                    newInstructions[i].operand = null;
                }
                else if (instruction.opcode == OpCodes.Newobj && Equals(instruction.operand, hitDataConstructor))
                {
                    newInstructions[i].opcode = OpCodes.Nop;
                    newInstructions[i].operand = null;
                    flaggedNop = true;
                }
            }
            return newInstructions.AsEnumerable();
        }
    }
}
