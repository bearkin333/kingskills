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
        //These are the base stats already in valheim
        public const float BaseSwimSpeed = 2f;
        public const float BaseSwimAccel = .05f;
        public const float BaseSwimTurn = 100f;

        //These are percents, max and min referencing effects at level 100 and 0
        public const float SwimSpeedMax = 3f;
        public const float SwimSpeedMin = 0f;
        public const float SwimAccelMax = 3f;
        public const float SwimAccelMin = 0f;
        public const float SwimTurnMax = 5f;
        public const float SwimTurnMin = 0f;

        //Swim stamina drain per second in absolute value
        public const float SwimStaminaPSMax = .5f;
        public const float SwimStaminaPSMin = 5f;

        //The maximum and minimum viable weight you can get a bonus for carrying
        public const float AbsoluteWeightMaxWeight = 1800;
        public const float AbsoluteWeightMinWeight = 100;

        //The number to determine the curve of the absolute weight experience
        public const float AbsoluteWeightExponent = 2.2f;
        //And a weighting for the overall bonus. Set very high
        public const float AbsoluteWeightExpMod = 20f;

        //These represent the percentages of the various states of
        //relative encumberance, included with the respective
        //experience bonuses you get for being in them
        public const float RelativeWeightLight = .33f;
            public const float RelativeWeightLightMod = -.25f;
        public const float RelativeWeightMed = .5f;
            public const float RelativeWeightMedMod = 0f;
        public const float RelativeWeightHighMed = .66f;
            public const float RelativeWeightHighMedMod = .25f;
        public const float RelativeWeightHeavy = .8f;
            public const float RelativeWeightHeavyMod = .5f;
        public const float RelativeWeightFull = 1f;
            public const float RelativeWeightFullMod = .8f;
        //This one is used for when you're over encumbered. Still a %
        public const float RelativeWeightOverMod = 2f;

        //This is the weight that Relative EXP will have, for quick changes
        public const float RelativeWeightExpMod = 1f;

        //This is the weight modifier for the bonus experience you get from run speed
        public const float RunSpeedExpMod = 1f;

        //These are percents, max and min referencing effects at level 100 and 0
        public const float RunSpeedMax = 2.5f;
        public const float RunSpeedMin = 0f;
        //How much of a reduction to the movespeed minus you get from your equipment
        public const float RunEquipmentReduxMax = .5f;
        public const float RunEquipmentReduxMin = 0f;
        //How much our encumberance system can decrease your movespeed in percent
        public const float RunEncumberanceMax = .5f;
        public const float RunEncumberanceMin = 0f;
        //How much run speed reduces the effects of encumberance
        public const float RunEncumberanceReduxMax = .5f;
        public const float RunEncumberanceReduxMin = 0f;
        //These are the base run and turn speeds in the game's code
        public const float BaseRunSpeed = 20f;
        public const float BaseRunTurnSpeed = 300f;

        public const float BaseRunStaminaDrain = 10f;
        public const float RunStaminaReduxMax = .8f;
        public const float RunStaminaReduxMin = -.25f;

        //How much stamina run grants per level
        public const float RunStaminaPerLevel = .6f;

        //Increases force you jump with by %
        public const float BaseJumpForce = 8f;
        public const float JumpForceMax = .9f;
        public const float JumpForceMin = -.1f;

        //Reduces stamina cost of jumping by a %
        public const float BaseJumpStaminaUse = 10f;
        public const float JumpStaminaReduxMax = .6f;
        public const float JumpStaminaReduxMin = 0f;

        //Increases forwards force when jumping by %
        public const float BaseJumpForwardForce = 2f;
        public const float JumpForwardForceMax = 1.5f;
        public const float JumpForwardForceMin = 0f;
        //May not actually be used or be different from jump force

        //This absolute % affects your jump force when tired.
        //Base is taken from game code, max is our new value at level 100
        public const float BaseJumpTiredFactor = .7f;
        public const float MaxJumpTiredFactor = .9f;

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
            float skillFactor = __instance.m_skills.GetSkillFactor(Skills.SkillType.Run);

            float runSkillFactor = 1f + Mathf.Lerp(RunSpeedMin, RunSpeedMax, skillFactor);

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

        public static float GetEncumberanceFactor(Player player, float skillFactor)
        {
            float encumberancePercent = Mathf.Clamp01(player.GetInventory().GetTotalWeight() / player.GetMaxCarryWeight());
            float encumberancePercentCurved = Mathf.Lerp(RunEncumberanceMin, RunEncumberanceMax, ShapeFactorSin(encumberancePercent));
            float skillEncumberanceRedux = 1f - Mathf.Lerp(RunEncumberanceReduxMin, RunEncumberanceReduxMax, skillFactor);
            return 1f - encumberancePercentCurved * skillEncumberanceRedux;
        }
        public static float GetEquipmentFactor(Player player, float skillFactor)
        {
            float equipmentMalusRedux = 1f - Mathf.Lerp(RunEquipmentReduxMin, RunEquipmentReduxMax, skillFactor);
            float equipmentFactor = player.m_equipmentMovementModifier;
            if (equipmentFactor < 0f) { equipmentFactor *= equipmentMalusRedux; }

            return equipmentFactor + 1;
        }

        public static float absoluteWeightBonus(Player player)
        {
            float weightPercent = Mathf.Clamp01(
                  (player.GetInventory().GetTotalWeight() - AbsoluteWeightMinWeight) 
                / (AbsoluteWeightMaxWeight - AbsoluteWeightMinWeight));
            return  1 + (AbsoluteWeightExpMod * Mathf.Pow(weightPercent, AbsoluteWeightExponent));
        }

        public static float relativeWeightBonus(Player player)
        {
            float weightPercent = player.GetInventory().GetTotalWeight() / player.GetMaxCarryWeight();
            float modifier = 0f;

            if (weightPercent <= RelativeWeightLight)
            {modifier = RelativeWeightLightMod;}

            else if (weightPercent <= RelativeWeightMed)
            {modifier = RelativeWeightMedMod;}

            else if (weightPercent <= RelativeWeightHighMed)
            {modifier = RelativeWeightHighMedMod;}

            else if (weightPercent <= RelativeWeightHeavy)
            {modifier = RelativeWeightHeavyMod;}

            else if (weightPercent <= RelativeWeightFull)
            {modifier = RelativeWeightFullMod;}

            else 
            {modifier = RelativeWeightOverMod;}

            return RelativeWeightExpMod * (1 + modifier);
        }

        public static float runSpeedExpBonus(Player player)
        {
            float runMod = player.GetRunSpeedFactor();
            player.m_seman.ApplyStatusEffectSpeedMods(ref runMod);

            return runMod * RunSpeedExpMod;
        }
        public static float swimSpeedExpBonus(Player player)
        {
            float swimMod = player.m_swimSpeed;
            player.m_seman.ApplyStatusEffectSpeedMods(ref swimMod);

            return swimMod;
        }

        public static float LastStaminaAdded = 0f;
        public static void RunSpeedUpdate(Player player)
        {
            float skillFactor = player.GetSkillFactor(Skills.SkillType.Run);
            float newRunStaminaDrain = BaseRunStaminaDrain;

            //We want to undo the game's skillfactor Lerp first before we do our own
            //Jotunn.Logger.LogMessage($"According to vanilla, the stamina drain ought to be {newRunStaminaDrain}.");
            newRunStaminaDrain /= Mathf.Lerp(1f, .5f, skillFactor);

            //Jotunn.Logger.LogMessage($"Reversing the effect of the skill, it's now {newRunStaminaDrain}.");
            newRunStaminaDrain *= (1f - Mathf.Lerp(RunStaminaReduxMin, RunStaminaReduxMax, skillFactor));
            player.m_runStaminaDrain = newRunStaminaDrain;

            //Jotunn.Logger.LogMessage($"Adding in our own skill effects, it's now {newRunStaminaDrain}.");
            //This number will not sound right - that's because the game will run it's own Lerp on it later.

            float newRunLevel = skillFactor * 100;
            float totalRunStaminaBonus = newRunLevel * RunStaminaPerLevel;

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
            player.m_swimSpeed = BaseSwimSpeed * (1f + Mathf.Lerp(SwimSpeedMin, SwimSpeedMax, skillFactor));
            player.m_swimAcceleration = BaseSwimAccel * (1f + Mathf.Lerp(SwimAccelMin, SwimAccelMax, skillFactor));
            player.m_swimTurnSpeed = BaseSwimTurn * (1f + Mathf.Lerp(SwimTurnMin, SwimTurnMax, skillFactor));
            player.m_swimStaminaDrainMinSkill = SwimStaminaPSMin;
            player.m_swimStaminaDrainMaxSkill = SwimStaminaPSMax;
        }

        public static void JumpForceUpdate(Player player)
        {
            float skillFactor = player.GetSkillFactor(Skills.SkillType.Jump);

            float vanillaJumpAddition = 1f + skillFactor * .4f;
            float newJumpForce = BaseJumpForce * (1 + Mathf.Lerp(JumpForceMin, JumpForceMax, skillFactor));
            float newStaminaUse = BaseJumpStaminaUse * (1 - Mathf.Lerp(JumpStaminaReduxMin, JumpStaminaReduxMax, skillFactor));
            float newJumpForwardForce = BaseJumpForwardForce * (1 + Mathf.Lerp(JumpForwardForceMin, JumpForwardForceMax, skillFactor));
            float newTiredFactor = Mathf.Lerp(BaseJumpTiredFactor, MaxJumpTiredFactor, skillFactor);

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

        //Written by fritz to create a quick and dirty sin curve.
        //Pass in a number between 0 and 1 and get a curved number between 0 and 1 back
        public static float ShapeFactorSin(float x)
        {
            x = Mathf.Clamp01(x);
            if (x <= 0.5)
            {
                return x;
            }
            return Mathf.Sin(Mathf.Lerp(0f, Mathf.PI / 2, x));
        }
    }
}
