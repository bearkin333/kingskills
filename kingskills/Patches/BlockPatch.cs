using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace kingskills
{

    [HarmonyPatch(typeof(Humanoid), "BlockAttack")]
    class BlockPatch : Humanoid
    {

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            bool sawBlockPower = false;
            bool patchedBlockPower = false;
            CodeInstruction storeBlockPower = new CodeInstruction(OpCodes.Nop);
            CodeInstruction loadParryFlag = new CodeInstruction(OpCodes.Nop);

            foreach (var instruction in instructions)
            {
                if (!patchedBlockPower)
                {
                    if (instruction.Calls(AccessTools.Method(typeof(ItemDrop.ItemData), "GetBlockPower", new Type[] { typeof(float) })))
                    {
                        sawBlockPower = true;
                    }
                    else if (sawBlockPower && instruction.IsStloc())
                    {
                        storeBlockPower = instruction.Clone();
                    }
                    else if (sawBlockPower && instruction.IsLdloc()) // load parry flag
                    {
                        loadParryFlag = instruction.Clone();
                        yield return instruction.Clone(); // parry flag
                        yield return new CodeInstruction(OpCodes.Ldarg_0); // Humanoid this (blocker)
                        yield return new CodeInstruction(OpCodes.Ldarg_1); // HitData hit
                        yield return new CodeInstruction(OpCodes.Ldarg_2); // Character attacker
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.DeclaredMethod(typeof(BlockPatch), "FixBlockPower"));
                        yield return storeBlockPower; // store result (block power)
                        yield return instruction;
                        storeBlockPower = null;
                        sawBlockPower = false;
                        patchedBlockPower = true;
                    }
                    else
                    {
                        yield return instruction;
                    }
                }
                else if (instruction.Calls(AccessTools.DeclaredMethod(typeof(Character), nameof(Character.UseStamina))))
                {
                    instruction.operand = AccessTools.DeclaredMethod(typeof(BlockPatch), nameof(BlockPatch.UseBlockStamina));
                    yield return instruction;
                }
                else if (instruction.Calls(AccessTools.DeclaredMethod(typeof(HitData), nameof(HitData.BlockDamage))))
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return loadParryFlag;
                    loadParryFlag = null;
                    instruction.operand = AccessTools.DeclaredMethod(typeof(BlockPatch), nameof(BlockPatch.BlockDamageExpPatch));
                    yield return instruction;
                }
                else if (instruction.Calls(AccessTools.DeclaredMethod(typeof(Character), "RaiseSkill")))
                {
                    instruction.opcode = OpCodes.Pop;
                    instruction.operand = null;
                    yield return instruction;
                    yield return new CodeInstruction(OpCodes.Pop);
                    yield return new CodeInstruction(OpCodes.Pop);
                }
                else
                {
                    yield return instruction;
                }
            }
        }

        private static void UseBlockStamina(Humanoid __instance, float stamina)
        {
            if (ConfigManager.IsSkillActive(Skills.SkillType.Blocking))
            {
                float skillFactor = __instance.GetSkillFactor(Skills.SkillType.Blocking);
                //Jotunn.Logger.LogMessage($"Stamina before redux is {stamina}");
                stamina *= ConfigManager.GetBlockStaminaRedux(skillFactor);
                //Jotunn.Logger.LogMessage($"Stamina after redux is {stamina}");
            }
            __instance.UseStamina(stamina);
        }


        private static void BlockDamageExpPatch(HitData hit, float damage, Humanoid __instance, bool isParry)
        {
            hit.BlockDamage(damage);
            float expValue = 1f;

            if (ConfigManager.IsSkillActive(Skills.SkillType.Blocking)) 
            {
                expValue *= damage * ConfigManager.GetBlockExpMult();

                if (isParry)
                {
                    expValue *= ConfigManager.GetBlockParryExpMult();
                    //Jotunn.Logger.LogMessage($"Parried! Exp Value doubled!");
                }
            }

            __instance.RaiseSkill(Skills.SkillType.Blocking, expValue);

            if (ConfigManager.IsSkillActive(Skills.SkillType.Unarmed) &&
                __instance.GetCurrentBlocker() == __instance.m_unarmedWeapon.m_itemData)
            {
                //Bonus exp for unarmed block!
                __instance.RaiseSkill(Skills.SkillType.Unarmed, ConfigManager.WeaponBXPUnarmedBlock.Value);

                BonusExperienceDamageText.CreateBXPText(
                    BonusExperienceDamageText.GetInFrontOfCharacter(__instance),
                    ConfigManager.WeaponBXPUnarmedBlock.Value);
            }
            //Jotunn.Logger.LogMessage($"Increased blocking skill by {expValue} due to damage");
        }

        private static float FixBlockPower(
            ItemDrop.ItemData currentBlocker,
            float skillFactor,
            bool isParry,
            Humanoid instance,
            HitData hit,
            Character attacker
            )
        {
            //Jotunn.Logger.LogMessage($"block power of {currentBlocker.GetBlockPower(skillFactor)} is now mine, also am I parrying? {isParry}");
            float baseBlockPower = currentBlocker.GetBaseBlockPower();

            //The flat block from blocking skill
            if (ConfigManager.IsSkillActive(Skills.SkillType.Blocking))
                baseBlockPower += ConfigManager.GetBlockPowerFlat(skillFactor);

            //The flat block armor bonus from spears
            if (ConfigManager.IsSkillActive(Skills.SkillType.Spears))
                baseBlockPower +=
                    ConfigManager.GetSpearBlockArmor(instance.GetSkillFactor(Skills.SkillType.Spears));

            //The flat bonus for an unarmed block
            if (ConfigManager.IsSkillActive(Skills.SkillType.Unarmed) && 
                currentBlocker == instance.m_unarmedWeapon.m_itemData)
                baseBlockPower += 
                    ConfigManager.GetFistBlockArmor(instance.GetSkillFactor(Skills.SkillType.Unarmed));

            //The flat bonus for a polearm block
            else if (ConfigManager.IsSkillActive(Skills.SkillType.Polearms) && 
                currentBlocker.m_shared.m_skillType == Skills.SkillType.Polearms)
                baseBlockPower +=
                    ConfigManager.GetPolearmBlock(instance.GetSkillFactor(Skills.SkillType.Polearms));

            float blockPower = baseBlockPower;

            //Skill bonus for block level
            if (ConfigManager.IsSkillActive(Skills.SkillType.Blocking))
                blockPower *= ConfigManager.GetBlockPowerMult(skillFactor);
            //Otherwise, we have to use base numbers
            else
            {
                blockPower *= ConfigManager.GetVanillaBlockMult(skillFactor);
            }

            //Here's the additional bonus to sword parry
            if (ConfigManager.IsSkillActive(Skills.SkillType.Swords) && isParry)
            { 
                blockPower *= 
                    ConfigManager.GetSwordParryMult(instance.GetSkillFactor(Skills.SkillType.Swords));
            }

            return blockPower;
        }
    }
}
