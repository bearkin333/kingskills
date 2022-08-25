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
            //This function provides a number to multiply the base run speed by
            float skillFactor = __instance.m_skills.GetSkillFactor(Skills.SkillType.Run);

            float runSkillFactor = ConfigManager.GetRunSpeedMod(skillFactor);

            float equipmentFactor = GetEquipmentFactor(__instance, skillFactor);
            float encumberanceFactor = GetEncumberanceFactor(__instance, skillFactor);

            float runSpeed = runSkillFactor * equipmentFactor * encumberanceFactor;
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

        // 
        // Experience determining functions
        //
        public static float GetEncumberanceFactor(Player player, float skillFactor)
        {
            float encumberancePercent = Mathf.Clamp01(player.GetInventory().GetTotalWeight() / player.GetMaxCarryWeight());
            float encumberanceCurved = ConfigManager.GetEncumberanceCurve(encumberancePercent);
            float skillEncumberanceRedux = ConfigManager.GetEncumberanceRedux(skillFactor);
            return encumberanceCurved * skillEncumberanceRedux;
        }
        public static float GetEquipmentFactor(Player player, float skillFactor)
        {
            float equipmentMalusRedux = ConfigManager.GetEquipmentRedux(skillFactor);
            float equipmentFactor = player.m_equipmentMovementModifier;
            if (equipmentFactor < 0f) { equipmentFactor *= equipmentMalusRedux; }

            return equipmentFactor + 1f;
        }
        public static float absoluteWeightBonus(Player player)
        {
            float weightPercent = ConfigManager.GetAbsoluteWeightPercent(player.m_inventory.GetTotalWeight());
            return ConfigManager.GetAbsoluteWeightCurve(weightPercent);
        }
        public static float relativeWeightBonus(Player player)
        {
            float weightPercent = player.GetInventory().GetTotalWeight() / player.GetMaxCarryWeight();
            return ConfigManager.GetRelativeWeightStage(weightPercent);
        }
        public static float runSpeedExpBonus(Player player)
        {
            float runMod = player.GetRunSpeedFactor();
            player.m_seman.ApplyStatusEffectSpeedMods(ref runMod);

            return runMod * ConfigManager.GetRunEXPSpeedMod();
        }
        public static float swimSpeedExpBonus(Player player)
        {
            float swimMod = player.m_swimSpeed;
            player.m_seman.ApplyStatusEffectSpeedMods(ref swimMod);

            return swimMod * ConfigManager.SwimEXPSpeedMod.Value;
        }
    }
       
    class FallPatch : Character
    {   
        public void FallDamageOverride()
            {
            float fallHeight = Mathf.Max(0f, m_maxAirAltitude - this.GetTransform().position.y);
            float skillFactor = GetSkillFactor(Skills.SkillType.Jump);
            float fallThreshold = ConfigManager.GetFallDamageThreshold(skillFactor);
                
            if (IsPlayer() && fallHeight > fallThreshold)
            {
                HitData hitData = new HitData();
                float fallDamage = (fallHeight - fallThreshold) * 8.33f;
                fallDamage *= ConfigManager.GetFallDamageRedux(skillFactor);
                hitData.m_damage.m_damage = fallDamage;
                hitData.m_point = m_lastGroundPoint;
                hitData.m_dir = m_lastGroundNormal;
                    
                Damage(hitData);
                RaiseSkill(Skills.SkillType.Jump, hitData.GetTotalDamage());
                //Jotunn.Logger.LogMessage("Jump exp just increased by " + hitData.GetTotalDamage() + " thanks to fall damage");
                
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
