using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using kingskills.Patches;
using kingskills.Perks;

namespace kingskills
{

    [HarmonyPatch(typeof(Humanoid), "BlockAttack")]
    class KSBlock : Humanoid
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            bool sawBlockPower = false;
            bool patchedBlockPower = false;
            bool patchedAsguard = false;
            CodeInstruction storeBlockPower = new CodeInstruction(OpCodes.Nop);
            CodeInstruction loadParryFlag = new CodeInstruction(OpCodes.Nop);

            foreach (var instruction in instructions)
            {
                if (!patchedAsguard)
                {
                    if (instruction.Calls(AccessTools.Method(typeof(Vector3), "Dot", new Type[] { typeof(Vector3), typeof(Vector3)})))
                    {
                        patchedAsguard = true;
                        yield return new CodeInstruction(OpCodes.Ldarg_0); // Humanoid this (blocker)
                        yield return new CodeInstruction(OpCodes.Call, 
                            AccessTools.DeclaredMethod(typeof(P_Asguard), "BlockDirectionPatch"));
                    }
                    else
                    {
                        yield return instruction;
                    }
                }
                else if (!patchedBlockPower)
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
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.DeclaredMethod(typeof(KSBlock), "FixBlockPower"));
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
                    instruction.operand = AccessTools.DeclaredMethod(typeof(KSBlock), nameof(KSBlock.UseBlockStamina));
                    yield return instruction;
                }
                else if (instruction.Calls(AccessTools.DeclaredMethod(typeof(HitData), nameof(HitData.BlockDamage))))
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return loadParryFlag;
                    loadParryFlag = null;
                    instruction.operand = AccessTools.DeclaredMethod(typeof(KSBlock), nameof(KSBlock.BlockDamageExpPatch));
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
            if (CFG.IsSkillActive(Skills.SkillType.Blocking))
            {
                float skillFactor = __instance.GetSkillFactor(Skills.SkillType.Blocking);
                //Jotunn.Logger.LogMessage($"Stamina before redux is {stamina}");
                stamina *= CFG.GetBlockStaminaRedux(skillFactor);
                //Jotunn.Logger.LogMessage($"Stamina after redux is {stamina}");
            }
            __instance.UseStamina(stamina);
        }


        private static void BlockDamageExpPatch(HitData hit, float damage, Humanoid __instance, bool isParry)
        {
            hit.BlockDamage(damage);
            float expValue = 1f;

            //Jotunn.Logger.LogMessage($"Block detected");

            if (CFG.IsSkillActive(Skills.SkillType.Blocking)) 
            {
                expValue *= damage * CFG.GetBlockExpMult();

                if (isParry)
                {
                    expValue *= CFG.GetBlockParryExpMult();
                    P_BlackFlash.BlackFlashExplosion();
                }
            }

            ((Player)__instance).RaiseSkill(Skills.SkillType.Blocking, expValue);

            if (__instance.GetCurrentBlocker() == __instance.m_unarmedWeapon.m_itemData)
                LevelUp.BXP(__instance as Player, Skills.SkillType.Unarmed, CFG.GetFistBXPBlock(damage));
        }

        public static float FixBlockPower(
            ItemDrop.ItemData currentBlocker,
            float skillFactor,
            bool isParry,
            Humanoid instance,
            HitData _ = null,
            Character __ = null
            )
        {
            //Jotunn.Logger.LogMessage($"block power of {currentBlocker.GetBlockPower(skillFactor)} is now mine, also am I parrying? {isParry}");
            float baseBlockPower = currentBlocker.GetBaseBlockPower();

            //The flat block from blocking skill
            if (CFG.IsSkillActive(Skills.SkillType.Blocking))
                baseBlockPower += CFG.GetBlockPowerFlat(skillFactor);

            //The flat block armor bonus from spears
            if (CFG.IsSkillActive(Skills.SkillType.Spears))
                baseBlockPower +=
                    CFG.GetSpearBlockArmor(instance.GetSkillFactor(Skills.SkillType.Spears));

            //The flat bonus for an unarmed block
            if (CFG.IsSkillActive(Skills.SkillType.Unarmed) && 
                currentBlocker == instance.m_unarmedWeapon.m_itemData)
                baseBlockPower += 
                    CFG.GetFistBlockArmor(instance.GetSkillFactor(Skills.SkillType.Unarmed));

            //The flat bonus for a polearm block
            else if (CFG.IsSkillActive(Skills.SkillType.Polearms) && 
                currentBlocker.m_shared.m_skillType == Skills.SkillType.Polearms)
                baseBlockPower +=
                    CFG.GetPolearmBlock(instance.GetSkillFactor(Skills.SkillType.Polearms));

            float blockPower = baseBlockPower;

            //Skill bonus for block level
            if (CFG.IsSkillActive(Skills.SkillType.Blocking))
                blockPower *= CFG.GetBlockPowerMult(skillFactor);
            //Otherwise, we have to use base numbers
            else
            {
                blockPower *= CFG.GetVanillaBlockMult(skillFactor);
            }

            //Here's the additional bonus to sword parry
            if (CFG.IsSkillActive(Skills.SkillType.Swords) && isParry)
            { 
                blockPower *= 
                    CFG.GetSwordParryMult(instance.GetSkillFactor(Skills.SkillType.Swords));
            }

            return blockPower;
        }
    }
}
