using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace kingskills
{
    [HarmonyPatch(typeof(Player))]
    class StatsPatch
    {
        public static void UpdateStats(Player player, bool selectively = false, Skills.SkillType skill = Skills.SkillType.None)
        {
            if (!selectively)
            {
                RunSpeedUpdate(player);
                SwimSpeedUpdate(player);
                JumpForceUpdate(player);
                SwordUpdate(player);
                SneakUpdate(player);
                CheckMaxLevel(player);
            }
            else if (skill != Skills.SkillType.None)
            {
                switch (skill)
                {
                    case Skills.SkillType.Run:
                        RunSpeedUpdate(player);
                        break;
                    case Skills.SkillType.Swim:
                        SwimSpeedUpdate(player);
                        break;
                    case Skills.SkillType.Jump:
                        JumpForceUpdate(player);
                        break;
                    case Skills.SkillType.Swords:
                        SwordUpdate(player);
                        break;
                    case Skills.SkillType.Sneak:
                        SneakUpdate(player);
                        break;
                }
            }
        }


        public static void RunSpeedUpdate(Player player)
        {
            if (!ConfigManager.IsSkillActive(Skills.SkillType.Run)) return;
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
            //float newRunLevel = skillFactor  * ConfigManager.MaxSkillLevel.Value;
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
            if (!ConfigManager.IsSkillActive(Skills.SkillType.Swim)) return;

            float skillFactor = player.GetSkillFactor(Skills.SkillType.Swim);
            player.m_swimSpeed = ConfigManager.BaseSwimSpeed * ConfigManager.GetSwimSpeedMult(skillFactor);
            player.m_swimAcceleration = ConfigManager.BaseSwimAccel * ConfigManager.GetSwimAccelMult(skillFactor);
            player.m_swimTurnSpeed = ConfigManager.BaseSwimTurn * ConfigManager.GetSwimTurnMult(skillFactor);
            player.m_swimStaminaDrainMinSkill = ConfigManager.SwimStaminaPerSecMin.Value;
            player.m_swimStaminaDrainMaxSkill = ConfigManager.SwimStaminaPerSecMax.Value;
        }
        public static void JumpForceUpdate(Player player)
        {
            if (!ConfigManager.IsSkillActive(Skills.SkillType.Jump)) return;

            float skillFactor = player.GetSkillFactor(Skills.SkillType.Jump);

            float vanillaJumpAddition = 1f + skillFactor * .4f;
            float newJumpForce = ConfigManager.BaseJumpForce * ConfigManager.GetJumpForceMult(skillFactor);
            float newJumpForwardForce = ConfigManager.BaseJumpForwardForce * ConfigManager.GetJumpForwardForceMult(skillFactor);
            float newStaminaUse = ConfigManager.BaseJumpStaminaUse * ConfigManager.GetJumpStaminaRedux(skillFactor);
            float newTiredFactor = ConfigManager.BaseJumpTiredFactor + ConfigManager.GetJumpTiredMod(skillFactor);

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
        public static void SwordUpdate(Player player)
        {
            if (!ConfigManager.IsSkillActive(Skills.SkillType.Swords)) return;

            player.m_dodgeStaminaUsage = ConfigManager.BaseDodgeStaminaUsage *
                ConfigManager.GetSwordDodgeStaminaRedux(player.GetSkillFactor(Skills.SkillType.Swords));
        }
        public static void SneakUpdate(Player player)
        {
            if (!ConfigManager.IsSkillActive(Skills.SkillType.Sneak)) return;

            player.m_crouchSpeed = ConfigManager.BaseCrouchSpeed *
                ConfigManager.GetSneakSpeedMult(player.GetSkillFactor(Skills.SkillType.Sneak));
        }
        public static void CheckMaxLevel(Player player)
        {
            Dictionary<Skills.SkillType, bool> temp = new Dictionary<Skills.SkillType, bool>(Perks.skillAscendedFlags);
            foreach (KeyValuePair<Skills.SkillType, bool> skillAcension in temp)
            {
                if (Perks.skillAscendedFlags[skillAcension.Key])
                {
                    //No need to check if it's already ascended
                }
                else if (player.GetSkills().GetSkillLevel(skillAcension.Key) >= ConfigManager.MaxSkillLevel.Value)
                {
                    Perks.skillAscendedFlags[skillAcension.Key] = true;
                    Jotunn.Logger.LogMessage("Regular update found a maxed skill and set it to be ascended");
                }
            }
        }

        //New max health and stamina

        [HarmonyPatch(nameof(Player.GetMaxCarryWeight))]
        [HarmonyPostfix]
        public static void GetMyMaxCarryWeight(Player __instance, ref float __result)
        {
            if (!ConfigManager.IsSkillActive(Skills.SkillType.Axes)) return;

            __result += ConfigManager.GetAxeCarryCapacity(__instance.GetSkillFactor(Skills.SkillType.Axes));
        }

        [HarmonyPatch(nameof(Player.SetMaxStamina))]
        [HarmonyPrefix]
        public static void SetMyMaxStamina(Player __instance, ref float stamina)
        {
            //Jotunn.Logger.LogMessage($"tried to set max stamina to {stamina}...");

            if (ConfigManager.IsSkillActive(Skills.SkillType.Run))
                stamina += ConfigManager.GetRunStamina(__instance.GetSkillFactor(Skills.SkillType.Run));
            if (ConfigManager.IsSkillActive(Skills.SkillType.Axes))
                stamina += ConfigManager.GetAxeStamina(__instance.GetSkillFactor(Skills.SkillType.Axes));

            //Jotunn.Logger.LogMessage($"Setting max stamina to {stamina} instead!");
        }

        [HarmonyPatch(typeof(Character))]
        [HarmonyPatch(nameof(Character.SetMaxHealth))]
        [HarmonyPrefix]
        public static void SetMyMaxHealth(Character __instance, ref float health)
        {
            if (!ConfigManager.IsSkillActive(Skills.SkillType.Blocking)) return;
            if (!__instance.IsPlayer()) return;

            //Jotunn.Logger.LogMessage($"tried to set max health to {health}...");
            health += ConfigManager.GetBlockHealth(__instance.GetSkillFactor(Skills.SkillType.Blocking));
            //Jotunn.Logger.LogMessage($"Setting max health to {health} instead!");
        }
    }
}
