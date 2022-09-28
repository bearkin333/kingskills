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

        [HarmonyPatch(typeof(Humanoid), nameof(Humanoid.BlockAttack))]
        [HarmonyPrefix]
        public static bool BlockAttackTakeover(Humanoid __instance, HitData hit, Character attacker, ref bool __result)
        {
            //largely stolen from the game. this was my only option
            //I swear

            if (!__instance.IsPlayer() && __instance != Player.m_localPlayer) return CFG.DontSkipOriginal;
            Player localPlayer = (Player)__instance;

            float blockAngle = Vector3.Dot(hit.m_dir, localPlayer.transform.forward);
            bool asguard = CFG.CheckPlayerActivePerk(localPlayer, PerkMan.PerkType.Asguard);
            if (!asguard && blockAngle > 0f)
            {
                __result = false;
                return CFG.SkipOriginal;
            }
            ItemDrop.ItemData currentBlocker = localPlayer.GetCurrentBlocker();
            if (currentBlocker is null)
            {
                __result = false;
                return CFG.SkipOriginal;
            }

            bool isParry = currentBlocker.m_shared.m_timedBlockBonus > 1f && localPlayer.m_blockTimer != -1f && localPlayer.m_blockTimer < 0.25f;
            if (asguard && blockAngle > 0f) isParry = false;

            float skillFactor = localPlayer.GetSkillFactor(Skills.SkillType.Blocking);
            float blockPower = KSBlockPower(currentBlocker, skillFactor, isParry, localPlayer);

            if (currentBlocker.m_shared.m_damageModifiers.Count > 0)
            {
                HitData.DamageModifiers modifiers = default(HitData.DamageModifiers);
                modifiers.Apply(currentBlocker.m_shared.m_damageModifiers);
                hit.ApplyResistance(modifiers, out var _);
            }

            HitData.DamageTypes damageTypes = hit.m_damage.Clone();
            damageTypes.ApplyArmor(blockPower);

            float totalBlockableDamage;
            float totalDamageAfterBlock;

            if (PerkMan.IsPerkActive(PerkMan.PerkType.BlockExpert))
            {
                totalBlockableDamage = hit.GetTotalDamage();
                totalDamageAfterBlock = damageTypes.GetTotalDamage();
            }
            else
            {
                totalBlockableDamage = hit.GetTotalBlockableDamage();
                totalDamageAfterBlock = damageTypes.GetTotalBlockableDamage();
            }

            float damageBlocked = totalBlockableDamage - totalDamageAfterBlock;
            float blockFactor = Mathf.Clamp01(damageBlocked / blockPower);
            float stamina = (isParry ? localPlayer.m_blockStaminaDrain : (localPlayer.m_blockStaminaDrain * blockFactor));
            UseBlockStamina(localPlayer, stamina);
            float totalStaggerDamage = damageTypes.GetTotalStaggerDamage();
            bool staggered = localPlayer.AddStaggerDamage(totalStaggerDamage, hit.m_dir);
            bool hasStamina = localPlayer.HaveStamina();
            bool succeedBlock = hasStamina && !staggered;
            if (succeedBlock)
            {
                hit.m_statusEffect = "";
                hit.BlockDamage(damageBlocked);
                BlockExpPatch(hit, damageBlocked, localPlayer, isParry);
                DamageText.instance.ShowText(DamageText.TextType.Blocked, hit.m_point + Vector3.up * 0.5f, damageBlocked);
            }
            if (currentBlocker.m_shared.m_useDurability)
            {
                float durabilityDrain = currentBlocker.m_shared.m_useDurabilityDrain * (totalBlockableDamage / blockPower);
                currentBlocker.m_durability -= durabilityDrain;
            }

            currentBlocker.m_shared.m_blockEffect.Create(hit.m_point, Quaternion.identity);
            if (!(attacker is null) && isParry && succeedBlock)
            {
                localPlayer.m_perfectBlockEffect.Create(hit.m_point, Quaternion.identity);
                if (attacker.m_staggerWhenBlocked)
                {
                    attacker.Stagger(-hit.m_dir);
                }
                //t.UseStamina(t.m_blockStaminaDrain);
            }
            if (succeedBlock)
            {
                hit.m_pushForce *= blockFactor;
                if (!(attacker is null) && !hit.m_ranged)
                {
                    float num6 = 1f - Mathf.Clamp01(blockFactor * 0.5f);
                    HitData hitData = new HitData();
                    hitData.m_pushForce = currentBlocker.GetDeflectionForce() * num6;
                    hitData.m_dir = attacker.transform.position - localPlayer.transform.position;
                    hitData.m_dir.y = 0f;
                    hitData.m_dir = Vector3.Normalize(hitData.m_dir);
                    attacker.Damage(hitData);
                }
            }

            __result = true;
            return CFG.SkipOriginal;
        }

        /*
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
        */

        private static void UseBlockStamina(Humanoid player, float stamina)
        {
            if (CFG.IsSkillActive(Skills.SkillType.Blocking))
            {
                float skillFactor = player.GetSkillFactor(Skills.SkillType.Blocking);
                //Jotunn.Logger.LogMessage($"Stamina before redux is {stamina}");
                stamina *= CFG.GetBlockStaminaRedux(skillFactor);
                //Jotunn.Logger.LogMessage($"Stamina after redux is {stamina}");
            }
            player.UseStamina(stamina);
        }


        private static void BlockExpPatch(HitData hit, float damage, Humanoid __instance, bool isParry)
        {
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

        public static float KSBlockPower(
            ItemDrop.ItemData currentBlocker,
            float skillFactor,
            bool isParry,
            Humanoid instance
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
                blockPower *= CFG.GetVanillaBlockMult(skillFactor);


            //Bonuses to power on parry
            if (isParry)
            {
                blockPower *= currentBlocker.m_shared.m_timedBlockBonus; 

                if (CFG.IsSkillActive(Skills.SkillType.Swords))
                    blockPower *=
                        CFG.GetSwordParryMult(instance.GetSkillFactor(Skills.SkillType.Swords));
            }

            

            return blockPower;
        }


    }
}
