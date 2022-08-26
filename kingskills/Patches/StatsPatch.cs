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
        public static void SneakUpdate(Player player)
        {
            player.m_crouchSpeed = ConfigManager.BaseCrouchSpeed *
                ConfigManager.GetSneakSpeedMod(player.GetSkillFactor(Skills.SkillType.Sneak));
        }

        //New max health and stamina

        [HarmonyPatch(nameof(Player.GetMaxCarryWeight))]
        [HarmonyPostfix]
        public static void GetMyMaxCarryWeight(Player __instance, ref float __result)
        {
            __result += ConfigManager.GetAxeCarryCapacity(__instance.GetSkillFactor(Skills.SkillType.Axes));
        }

        [HarmonyPatch(nameof(Player.SetMaxStamina))]
        [HarmonyPrefix]
        public static void SetMyMaxStamina(Player __instance, ref float stamina)
        {
            //Jotunn.Logger.LogMessage($"tried to set max stamina to {stamina}...");
            stamina += ConfigManager.GetRunStamina(__instance.GetSkillFactor(Skills.SkillType.Run)) +
                ConfigManager.GetAxeStamina(__instance.GetSkillFactor(Skills.SkillType.Axes));
            //Jotunn.Logger.LogMessage($"Setting max stamina to {stamina} instead!");
        }

        [HarmonyPatch(typeof(Character))]
        [HarmonyPatch(nameof(Character.SetMaxHealth))]
        [HarmonyPrefix]
        public static void SetMyMaxHealth(Character __instance, ref float health)
        {
            if (!__instance.IsPlayer()) return;

            //Jotunn.Logger.LogMessage($"tried to set max health to {health}...");
            health += ConfigManager.GetBlockHealth(__instance.GetSkillFactor(Skills.SkillType.Blocking));
            //Jotunn.Logger.LogMessage($"Setting max health to {health} instead!");
        }
    }
}
