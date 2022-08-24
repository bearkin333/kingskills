using BepInEx;
using HarmonyLib;
using Jotunn.Entities;
using Jotunn.Managers;
using kingskills.Commands;
using UnityEngine;

namespace kingskills
{

    [HarmonyPatch(typeof(Player))]
    class MovePatch
    {

        [HarmonyPatch(nameof(Player.OnSkillLevelup))]
        [HarmonyPostfix]
        static void SkillLevelupPatch(Player __instance, Skills.SkillType skill)
        {
            if (skill == Skills.SkillType.Swim)
            {
                SwimSpeedUpdate(__instance);
            }
            else if (skill == Skills.SkillType.Run)
            {
                RunSpeedUpdate(__instance);
            }
            else if (skill == Skills.SkillType.Jump)
            {
                JumpForceUpdate(__instance);
            }

        }

        [HarmonyPatch(nameof(Player.RaiseSkill))]
        [HarmonyPrefix]
        static bool RaiseSkillPatch(Player __instance, Skills.SkillType skill, float value = 1f)
        {
            bool dontSkip = true;

            if (skill == Skills.SkillType.Run)
            {
                float expValue = 1f;
                float x = 0;

                //Allow status effects to modify exp gain rate
                __instance.GetSEMan().ModifyRaiseSkill(skill, ref expValue);

                x = absoluteWeightBonus(__instance);
                expValue *= x;
                //Jotunn.Logger.LogMessage("Absolute weight mod: " + x);

                x = relativeWeightBonus(__instance);
                expValue *= x;
                //Jotunn.Logger.LogMessage("Relative weight mod: " + x);

                x = runSpeedExpBonus(__instance);
                expValue *= x;
                //Jotunn.Logger.LogMessage("Run speed mod: " + x);

                __instance.GetSkills().RaiseSkill(skill, expValue);

                dontSkip = false;
                return dontSkip;
            }
            else if (skill == Skills.SkillType.Swim)
            {
                float expValue = 1f;
                float x = 0;

                //Allow status effects to modify exp gain rate
                __instance.GetSEMan().ModifyRaiseSkill(skill, ref expValue);

                x = absoluteWeightBonus(__instance);
                expValue *= x;
                x = relativeWeightBonus(__instance);
                expValue *= x;
                x = swimSpeedExpBonus(__instance);
                expValue *= x;

                __instance.GetSkills().RaiseSkill(skill, expValue);

                dontSkip = false;
                return dontSkip;
            }

            return dontSkip;
        }


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
        // Updating functions
        //
        public static float LastStaminaAdded = 0f;
        public static void RunSpeedUpdate(Player player)
        {
            float skillFactor = player.GetSkillFactor(Skills.SkillType.Run);
            float newRunStaminaDrain = ConfigManager.BaseRunStaminaDrain;

            //We want to undo the game's skillfactor Lerp first before we do our own
            //Jotunn.Logger.LogMessage($"According to vanilla, the stamina drain ought to be {newRunStaminaDrain}.");
            newRunStaminaDrain /= Mathf.Lerp(1f, .5f, skillFactor);

            //Jotunn.Logger.LogMessage($"Reversing the effect of the skill, it's now {newRunStaminaDrain}.");
            newRunStaminaDrain *= ConfigManager.GetRunStaminaRedux(skillFactor);
            player.m_runStaminaDrain = newRunStaminaDrain;

            //Jotunn.Logger.LogMessage($"Adding in our own skill effects, it's now {newRunStaminaDrain}.");
            //This number will not sound right - that's because the game will run it's own Lerp on it later.

            float newRunLevel = skillFactor * 100;
            float totalRunStaminaBonus = newRunLevel * ConfigManager.RunStaminaPerLevel.Value;

            //First, we remove the last recorded update to the player's base stamina
            //Jotunn.Logger.LogMessage($"The player's base stamina is {player.m_baseStamina}, and last time we added to it we added {LastStaminaAdded}.");
            player.m_baseStamina -= LastStaminaAdded;

            //Jotunn.Logger.LogMessage($"Now it is {player.m_baseStamina}.");
            //Then, we add the new value
            player.m_baseStamina += totalRunStaminaBonus;

            //Jotunn.Logger.LogMessage($"We just added {totalRunStaminaBonus} to it, making it {player.m_baseStamina}.");
            //Then, we record it for next time this function is run
            LastStaminaAdded = totalRunStaminaBonus;

            //Jotunn.Logger.LogMessage($"Now we reset the last stamina added to {LastStaminaAdded}.");
        }
        public static void SwimSpeedUpdate(Player player)
        {
            float skillFactor = player.GetSkillFactor(Skills.SkillType.Swim);
            player.m_swimSpeed = ConfigManager.BaseSwimSpeed * ConfigManager.GetSwimSpeedMod(skillFactor);
            player.m_swimAcceleration = ConfigManager.BaseSwimAccel * ConfigManager.GetSwimAccelMod(skillFactor);
            player.m_swimTurnSpeed = ConfigManager.BaseSwimTurn * ConfigManager.GetSwimTurnMod(skillFactor);
            player.m_swimStaminaDrainMinSkill = ConfigManager.SwimStaminaPerSecMin.Value;
            player.m_swimStaminaDrainMaxSkill = ConfigManager.SwimStaminaPerSecMax.Value;
        }
        public static void JumpForceUpdate(Player player)
        {
            float skillFactor = player.GetSkillFactor(Skills.SkillType.Jump);

            float vanillaJumpAddition = 1f + skillFactor * .4f;
            float newJumpForce = ConfigManager.BaseJumpForce * ConfigManager.GetJumpForceMod(skillFactor);
            float newJumpForwardForce = ConfigManager.BaseJumpForwardForce * ConfigManager.GetJumpForwardForceMod(skillFactor);
            float newStaminaUse = ConfigManager.BaseJumpStaminaUse * ConfigManager.GetJumpStaminaRedux(skillFactor);
            float newTiredFactor = ConfigManager.BaseJumpTiredFactor + ConfigManager.GetJumpTiredRedux(skillFactor);

            newJumpForce /= vanillaJumpAddition; //Removing the vanilla calculations
            newJumpForwardForce /= vanillaJumpAddition; //Removing the vanilla calculations


            player.m_jumpForce = newJumpForce;
            player.m_jumpStaminaUsage = newStaminaUse;
            player.m_jumpForceForward = newJumpForwardForce;
            player.m_jumpForceTiredFactor = newTiredFactor;

            //Jotunn.Logger.LogMessage($"Jump forward force is currently {player.m_jumpForceForward}");
            /*Jotunn.Logger.LogMessage(
                $"New jump force: {player.m_jumpForceForward} \n" +
                $"New stamina use: {player.m_jumpStaminaUsage} \n" +
                $"New forward force: {player.m_jumpForceForward} \n" +
                $"New tired factor: {player.m_jumpForceTiredFactor}");*/
        }



        // 
        // Experience determining functions
        //
        public static float GetEncumberanceFactor(Player player, float skillFactor)
        {
            float encumberancePercent = Mathf.Clamp01(player.GetInventory().GetTotalWeight() / player.GetMaxCarryWeight());
            float encumberancePercentCurved = ConfigManager.GetEncumberanceCurve(encumberancePercent);
            float skillEncumberanceRedux = ConfigManager.GetEncumberanceRedux(skillFactor);
            return 1f - encumberancePercentCurved * skillEncumberanceRedux;
        }
        public static float GetEquipmentFactor(Player player, float skillFactor)
        {
            float equipmentMalusRedux = ConfigManager.GetEquipmentRedux(skillFactor);
            float equipmentFactor = player.m_equipmentMovementModifier;
            if (equipmentFactor < 0f) { equipmentFactor *= equipmentMalusRedux; }

            return equipmentFactor + 1;
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

            return runMod * ConfigManager.RunEXPSpeedMod.Value;
        }
        public static float swimSpeedExpBonus(Player player)
        {
            float swimMod = player.m_swimSpeed;
            player.m_seman.ApplyStatusEffectSpeedMods(ref swimMod);

            return swimMod * ConfigManager.SwimEXPSpeedMod.Value;
        }
    }
}
