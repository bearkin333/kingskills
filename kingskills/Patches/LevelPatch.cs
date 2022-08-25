﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace kingskills
{
    [HarmonyPatch(typeof(Player))]
    class LevelPatch
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
            else if (skill == Skills.SkillType.Swords)
            {
                SwordUpdate(__instance);
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

                x = MovePatch.absoluteWeightBonus(__instance);
                expValue *= x;
                //Jotunn.Logger.LogMessage("Absolute weight mod: " + x);

                x = MovePatch.relativeWeightBonus(__instance);
                expValue *= x;
                //Jotunn.Logger.LogMessage("Relative weight mod: " + x);

                x = MovePatch.runSpeedExpBonus(__instance);
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

                x = MovePatch.absoluteWeightBonus(__instance);
                expValue *= x;
                x = MovePatch.relativeWeightBonus(__instance);
                expValue *= x;
                x = MovePatch.swimSpeedExpBonus(__instance);
                expValue *= x;

                __instance.GetSkills().RaiseSkill(skill, expValue);

                dontSkip = false;
                return dontSkip;
            }

            return dontSkip;
        }
        public static float LastStaminaAdded = 0f;


        //
        // Level Updating functions
        //

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


            //Then a whole bunch of deprecated nonsense to try to increase the stamina.
            //Now I simply tack my number on to the "GetMaxStamina" call by player
            //float newRunLevel = skillFactor * 100;
            //float totalRunStaminaBonus = newRunLevel * ConfigManager.RunStaminaPerLevel.Value;

            //First, we remove the last recorded update to the player's base stamina
            //Jotunn.Logger.LogMessage($"The player's base stamina is {player.m_baseStamina}, and last time we added to it we added {LastStaminaAdded}.");
            // player.m_baseStamina -= LastStaminaAdded;

            //Jotunn.Logger.LogMessage($"Now it is {player.m_baseStamina}.");
            //Then, we add the new value
            //player.m_baseStamina += totalRunStaminaBonus;

            //Jotunn.Logger.LogMessage($"We just added {totalRunStaminaBonus} to it, making it {player.m_baseStamina}.");
            //Then, we record it for next time this function is run
            //LastStaminaAdded = totalRunStaminaBonus;

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
        public static void SwordUpdate(Player player){
            player.m_dodgeStaminaUsage = ConfigManager.BaseDodgeStaminaUsage *
                ConfigManager.GetSwordDodgeStaminaRedux(player.GetSkillFactor(Skills.SkillType.Swords));
        }

        //New max health and stamina

        [HarmonyPatch(nameof(Player.SetMaxStamina))]
        [HarmonyPostfix]
        public static void SetMyMaxStamina(Player __instance)
        {
            __instance.m_maxStamina += ConfigManager.RunStaminaPerLevel.Value * __instance.GetSkillFactor(Skills.SkillType.Run) +
                ConfigManager.AxeStaminaPerLevel.Value * __instance.GetSkillFactor(Skills.SkillType.Axes);
        }

        [HarmonyPatch(nameof(Player.GetMaxCarryWeight))]
        [HarmonyPostfix]
        public static void GetMyMaxCarryWeight(Player __instance, ref float __result)
        {
            __result += ConfigManager.GetAxeCarryCapacity(__instance.GetSkillFactor(Skills.SkillType.Axes));
        }

        [HarmonyPatch(nameof(Player.SetMaxHealth))]
        [HarmonyPostfix]
        public static void SetMyMaxHealth(Player __instance, float health)
        {
            health += ConfigManager.BlockHealthPerLevel.Value * __instance.GetSkillFactor(Skills.SkillType.Blocking);
            __instance.SetMaxHealth(health);
        }
    }
}
