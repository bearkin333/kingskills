using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Configuration;
using UnityEngine;

namespace kingskills
{
    /*
     * 
        
    */

    class ConfigManager
    {
        /* Template
        public static ConfigEntry<float> ;
         */

        public static ConfigEntry<float> WeaponEXPSwing;
        public static ConfigEntry<float> WeaponEXPHoldPerTick;
        public static ConfigEntry<float> WeaponEXPHoldTickLength;
        public static ConfigEntry<float> WeaponEXPHoldUnarmedMod;
        public static ConfigEntry<float> WeaponEXPStrikeDamageMod;
        public static ConfigEntry<float> WeaponEXPStrikeDamageFactor;
        public static ConfigEntry<float> WeaponEXPStrikeDestructibleMod;
        public static ConfigEntry<float> WeaponBXPUnarmedBlock;
        public static ConfigEntry<float> WeaponBXPClubStagger; 
        public static ConfigEntry<float> WeaponBXPSwordStagger; 
        public static ConfigEntry<float> WeaponBXPKnifeBackstab; 
        public static ConfigEntry<float> WeaponBXPAxeRange;
        public static ConfigEntry<float> WeaponBXPAxeTreeAmount;
        public static ConfigEntry<float> WeaponBXPSpearThrown; 
        public static ConfigEntry<float> WeaponBXPBowDistanceMod; 
        public static ConfigEntry<float> ToolEXPStrikeDamageMod;
        public static ConfigEntry<float> ToolEXPStrikeDamageFactor;
        public static ConfigEntry<float> BlockExpMod;
        public static ConfigEntry<float> BlockParryExpMod;
        public static ConfigEntry<float> BlockFlatPowerMax;
        public static ConfigEntry<float> BlockFlatPowerMin;
        public static ConfigEntry<float> BlockPowerModMax;
        public static ConfigEntry<float> BlockPowerModMin;
        public static ConfigEntry<float> BlockStaminaReduxMax;
        public static ConfigEntry<float> BlockStaminaReduxMin;
        public static ConfigEntry<float> BlockStaggerLimitModMax;
        public static ConfigEntry<float> BlockStaggerLimitModMin;
        public static ConfigEntry<float> JumpFallDamageThresholdMax;
        public static ConfigEntry<float> JumpFallDamageThresholdMin;
        public static ConfigEntry<float> JumpFallDamageReduxMax;
        public static ConfigEntry<float> JumpFallDamageReduxMin;
        public static ConfigEntry<float> SwimEXPSpeedMod;
        public static ConfigEntry<float> SwimSpeedModMax;
        public static ConfigEntry<float> SwimSpeedModMin;
        public static ConfigEntry<float> SwimAccelModMax;
        public static ConfigEntry<float> SwimAccelModMin;
        public static ConfigEntry<float> SwimTurnModMax;
        public static ConfigEntry<float> SwimTurnModMin;
        public static ConfigEntry<float> SwimStaminaPerSecMax;
        public static ConfigEntry<float> SwimStaminaPerSecMin;
        public static ConfigEntry<float> RunAbsoluteWeightMaxWeight;
        public static ConfigEntry<float> RunAbsoluteWeightMinWeight;
        public static ConfigEntry<float> RunAbsoluteWeightFactor;
        public static ConfigEntry<float> RunAbsoluteWeightExpMod;
        public static ConfigEntry<float> RunRelativeWeightLight;
        public static ConfigEntry<float> RunRelativeWeightLightMod;
        public static ConfigEntry<float> RunRelativeWeightMed;
        public static ConfigEntry<float> RunRelativeWeightMedMod;
        public static ConfigEntry<float> RunRelativeWeightHighMed;
        public static ConfigEntry<float> RunRelativeWeightHighMedMod;
        public static ConfigEntry<float> RunRelativeWeightHeavy;
        public static ConfigEntry<float> RunRelativeWeightHeavyMod;
        public static ConfigEntry<float> RunRelativeWeightFull;
        public static ConfigEntry<float> RunRelativeWeightFullMod;
        public static ConfigEntry<float> RunRelativeWeightOverMod;
        public static ConfigEntry<float> RunRelativeWeightExpMod;
        public static ConfigEntry<float> RunEXPSpeedMod;
        public static ConfigEntry<float> RunSpeedModMax;
        public static ConfigEntry<float> RunSpeedModMin;
        public static ConfigEntry<float> RunEquipmentReduxMax;
        public static ConfigEntry<float> RunEquipmentReduxMin;
        public static ConfigEntry<float> RunEncumberanceModMax;
        public static ConfigEntry<float> RunEncumberanceModMin;
        public static ConfigEntry<float> RunEncumberanceReduxMax;
        public static ConfigEntry<float> RunEncumberanceReduxMin;
        public static ConfigEntry<float> RunStaminaReduxMax;
        public static ConfigEntry<float> RunStaminaReduxMin;
        public static ConfigEntry<float> RunStaminaPerLevel;
        public static ConfigEntry<float> JumpForceModMax;
        public static ConfigEntry<float> JumpForceModMin;
        public static ConfigEntry<float> JumpStaminaReduxMax;
        public static ConfigEntry<float> JumpStaminaReduxMin;
        public static ConfigEntry<float> JumpForwardForceModMax;
        public static ConfigEntry<float> JumpForwardForceModMin;
        public static ConfigEntry<float> JumpTiredReduxMax;
        public static ConfigEntry<float> JumpTiredReduxMin;

        //These are the base stats already in valheim
        public const float BaseSwimSpeed = 2f;
        public const float BaseSwimAccel = .05f;
        public const float BaseSwimTurn = 100f;
        public const float BaseRunSpeed = 20f;
        public const float BaseRunTurnSpeed = 300f;
        public const float BaseRunStaminaDrain = 10f;
        public const float BaseJumpForce = 8f;
        public const float BaseJumpStaminaUse = 10f;
        public const float BaseJumpForwardForce = 2f;
        public const float BaseJumpTiredFactor = .7f;

        /* flavor curve numbers
        const float TotalXp = 20553.6f;
        const float MasteryTime = 5f * 60 * 60;  // seconds
        const float XpPerSec = TotalXp / MasteryTime; // xp/s needed to master skill in target_mastery_time
        */

        public static void Init(ConfigFile cfg)
        {
            /* Template
            x = cfg.Bind("section.section", "Title", 0f, 
                "Description");
             */
            //Weapon Swing Experience
            WeaponEXPSwing = cfg.Bind("Experience.Weapons", "XP Per Swing", 1f,
                "Flat experience to be gained on each swing, regardless of hit.");


            //Weapon Hold Experience
            WeaponEXPHoldPerTick = cfg.Bind("Experience.Weapons", "XP Per Second", .05f,
                "Flat experience to be gained every second holding a particular weapon");
            WeaponEXPHoldTickLength = cfg.Bind("Experience.Weapons", "Hold Timer", 1.0f,
                "Seconds between ticks of hold experience");
            WeaponEXPHoldUnarmedMod = cfg.Bind("Experience.Weapons", "Unarmed Mod", 20f,
                "% of normal hold experience gained for unarmed when holding nothing");


            //Weapon Strike Experience
            WeaponEXPStrikeDamageMod = cfg.Bind("Experience.Weapons", "XP Per Damage", 40f,
                "% of damage dealt that becomes experience earned");
            WeaponEXPStrikeDamageFactor = cfg.Bind("Experience.Weapons", "XP Per Damage Curve", .42f,
                "Factor to define the slope of the damage to xp curve");
            WeaponEXPStrikeDestructibleMod = cfg.Bind("Experience.Weapons", "Destructible Mod", 10f,
                "% of experience gained when you hit a non living thing");
            

            //Weapon Bonus experience (BXP)
            WeaponBXPUnarmedBlock = cfg.Bind("Weapon.BonusExperience", "Unarmed Block", 20f,
                "Flat BXP gained every time you perform an unarmed block");
            WeaponBXPClubStagger = cfg.Bind("Weapon.BonusExperience", "Club Stagger", 20f,
                "Flat BXP gained every time you stagger an enemy with clubs");
            WeaponBXPSwordStagger = cfg.Bind("Weapon.BonusExperience", "Sword Parry Hit", 20f,
                "Flat BXP gained every time you hit a staggered enemy with a sword");
            WeaponBXPKnifeBackstab = cfg.Bind("Weapon.BonusExperience", "Knife Backstab", 20f,
                "Flat BXP gained every time you get a sneak attack using the knife.");
            WeaponBXPAxeTreeAmount = cfg.Bind("Weapon.BonusExperience", "Axe Log", 20f,
                "Flat BXP gained every time you break down a log");
            WeaponBXPAxeRange = cfg.Bind("Weapon.BonusExperience", "Axe Felling Range", 100f,
                "Distance to check for axe BXP gain");
            WeaponBXPSpearThrown = cfg.Bind("Weapon.BonusExperience", "Spear Throw", 20f,
                "Flat BXP gained every time you hit with a thrown spear");
            WeaponBXPBowDistanceMod = cfg.Bind("Weapon.BonusExperience", "Bow Distance Mod", 50f,
                "% of distance that becomes bow experience on hit.");


            //Tool experience
            ToolEXPStrikeDamageMod = cfg.Bind("Experience.Tools", "Tool Damage Mod", 150f,
                "% of damage done to resources that becomes experience for gathering skills " +
                "(Woodcutting, Mining)");
            ToolEXPStrikeDamageFactor = cfg.Bind("Experience.Tools", "Tool Damage Factor", .22f,
                "Factor to define the slope of the damage to xp curve");


            //Block experience
            BlockExpMod = cfg.Bind("Block.Experience", "Experience Mod", 22f,
                "% of damage blocked that turns into experience");
            BlockParryExpMod = cfg.Bind("Block.Experience", "Parry Bonus", 200f,
                "% extra experience earned when parrying an attack");


            //Block effects
            BlockFlatPowerMin = cfg.Bind("Block.Effect", "Flat Armor Bonus Min", 0f,
                "This flat value is added to block armor at level 0");
            BlockFlatPowerMax = cfg.Bind("Block.Effect", "Flat Armor Bonus Max", 50f,
                "This flat value is added to block armor at level 100");
            BlockPowerModMin = cfg.Bind("Block.Effect", "% Bonus Block Armor Min", -25f,
                "% change in total block armor at level 0");
            BlockPowerModMax = cfg.Bind("Block.Effect", "% Bonus Block Armor Max", 100f,
                "% change in total block armor at level 100");
            BlockStaminaReduxMin = cfg.Bind("Block.Effect", "Stamina Cost Reduction Min", -10f,
                "% less stamina to block at level 0");
            BlockStaminaReduxMax = cfg.Bind("Block.Effect", "Stamina Cost Reduction Max", 50f,
                "% less stamina to block at level 100");
            BlockStaggerLimitModMin = cfg.Bind("Block.Effect", "Stagger Limit Bonus Min", 0f,
                "% of max health to add to player's stagger limit at level 0");
            BlockStaggerLimitModMax = cfg.Bind("Block.Effect", "Stagger Limit Bonus Max", 30f,
                "% of max health to add to player's stagger limit at level 100");

            //Jump Effects
            JumpFallDamageThresholdMin = cfg.Bind("Jump.Effect", "Fall Damage Threshold Min", 4f,
                "meters to fall before you start calculating fall damage at level 0");
            JumpFallDamageThresholdMax = cfg.Bind("Jump.Effect", "Fall Damage Threshold Max", 12f,
                "meters to fall before you start calculating fall damage at level 100");
            JumpFallDamageReduxMin = cfg.Bind("Jump.Effect", "Fall Damage Reduction Min", -15f,
                "% less fall damage to take at level 0");
            JumpFallDamageReduxMax = cfg.Bind("Jump.Effect", "Fall Damage Reduction Max", 60f,
                "% less fall damage to take at level 100");


            //Swim Experience
            SwimEXPSpeedMod = cfg.Bind("Swim.Experience", "Swim Experience Mod", 100f,
                "% of swim speed that becomes bonus experience gain");


            //Swim effects
            SwimSpeedModMin = cfg.Bind("Swim.Effect", "Speed Min", 0f,
                "% to increase swim speed at level 0");
            SwimSpeedModMax = cfg.Bind("Swim.Effect", "Speed Max", 300f,
                "% to increase swim speed at level 100");
            SwimAccelModMin = cfg.Bind("Swim.Effect", "Acceleration Min", 0f,
                "% to increase swim acceleration at level 0");
            SwimAccelModMax = cfg.Bind("Swim.Effect", "Acceleration Max", 300f,
                "% to increase swim acceleration at level 100");
            SwimTurnModMin = cfg.Bind("Swim.Effect", "Turn Speed Min", 0f,
                "% to increase swim turn speed at level 0");
            SwimTurnModMax = cfg.Bind("Swim.Effect", "Turn Speed Max", 500f,
                "% to increase swim turn speed at level 100");
            SwimStaminaPerSecMin = cfg.Bind("Swim.Effect", "Stamina cost min", 5f,
                "How much stamina swimming will take per second at level 0");
            SwimStaminaPerSecMax = cfg.Bind("Swim.Effect", "Stamina cost max", .5f,
                "How much stamina swimming will take per second at level 100");


            //Run Absolute Weight Experience
            RunAbsoluteWeightMinWeight = cfg.Bind("Run.Experience.AbsoluteWeight", "Min Weight", 100f,
                "The lowest weight you will get an experience bonus for carrying");
            RunAbsoluteWeightMaxWeight = cfg.Bind("Run.Experience.AbsoluteWeight", "Max Weight", 1800f,
                "The heighest weight you will get an experience bonus for carrying");
            RunAbsoluteWeightFactor = cfg.Bind("Run.Experience.AbsoluteWeight", "Factor ", 2.2f,
                "Factor to define the slope of the absolute weight curve");
            RunAbsoluteWeightExpMod = cfg.Bind("Run.Experience.AbsoluteWeight", "Mod", 2000f, 
                "% modifier for how much experience you get from absolute weight");


            //Run Relative Weight Experience
            RunRelativeWeightLight = cfg.Bind("Run.Experience.RelativeWeight", "Light Threshold", 33f,
                "Threshold for being at 'light' encumberance");
            RunRelativeWeightLightMod = cfg.Bind("Run.Experience.RelativeWeight", "Light Modifier", -25f,
                "Experience Bonus for being at 'light' encumberance");
            RunRelativeWeightMed = cfg.Bind("Run.Experience.RelativeWeight", "Medium Threshold", 50f,
                "Threshold for being at 'medium' encumberance");
            RunRelativeWeightMedMod = cfg.Bind("Run.Experience.RelativeWeight", "Medium Modifier", 0f,
                "Experience Bonus for being at 'medium' encumberance");
            RunRelativeWeightHighMed = cfg.Bind("Run.Experience.RelativeWeight", "High Threshold", 66f,
                "Threshold for being at 'high' encumberance");
            RunRelativeWeightHighMedMod = cfg.Bind("Run.Experience.RelativeWeight", "High Modifier", 25f,
                "Experience Bonus for being at 'high' encumberance");
            RunRelativeWeightHeavy = cfg.Bind("Run.Experience.RelativeWeight", "Heavy Threshold", 80f,
                "Threshold for being at 'heavy' encumberance");
            RunRelativeWeightHeavyMod = cfg.Bind("Run.Experience.RelativeWeight", "Heavy Modifier", 50f,
                "Experience Bonus for being at 'heavy' encumberance");
            RunRelativeWeightFull = cfg.Bind("Run.Experience.RelativeWeight", "Full Threshold", 100f,
                "Threshold for being at 'full' encumberance");
            RunRelativeWeightFullMod = cfg.Bind("Run.Experience.RelativeWeight", "Full Modifier", 80f,
                "Experience Bonus for being at 'full' encumberance");
            RunRelativeWeightOverMod = cfg.Bind("Run.Experience.RelativeWeight", "Overweight Modifier", 200f,
                "Experience Bonus for being overencumbered");
            RunRelativeWeightExpMod = cfg.Bind("Run.Experience.RelativeWeight", "Overall Modifier", 100f,
                "% modifier for how much experience you get from relative weight");


            //Run Experience
            RunEXPSpeedMod = cfg.Bind("Run.Experience", "Run Experience Mod", 100f,
                "% of run speed that becomes bonus experience gain");


            //Run Effects
            RunSpeedModMin = cfg.Bind("Run.Effect", "Speed Min", 0f,
                "% extra run speed at level 0");
            RunSpeedModMax = cfg.Bind("Run.Effect", "Speed Max", 250f,
                "% extra run speed at level 100");
            RunEquipmentReduxMin = cfg.Bind("Run.Effect", "Equipment Reduction Min", 0f,
                "% less movespeed reduction from equipment at level 0");
            RunEquipmentReduxMax = cfg.Bind("Run.Effect", "Equipment Reduction Max", 50f,
                "% less movespeed reduction from equipment at level 100");
            RunEncumberanceModMin = cfg.Bind("Run.Effect", "Encumberance Min", 0f,
                "% less run speed when your inventory is empty");
            RunEncumberanceModMax = cfg.Bind("Run.Effect", "Encumberance Max", 50f,
                "% less run speed when your inventory is full");
            RunEncumberanceReduxMin = cfg.Bind("Run.Effect", "Encumberance Reduction Min", 0f,
                "% less effect from encumberance at level 0");
            RunEncumberanceReduxMax = cfg.Bind("Run.Effect", "Encumberance Reduction Max", 50f, 
                "% less effect from encumberance at level 100");
            RunStaminaReduxMin = cfg.Bind("Run.Effect", "Stamina Reduction Min", -25f,
                "% less stamina cost to run at level 100");
            RunStaminaReduxMax = cfg.Bind("Run.Effect", "Stamina Reduction Max", 80f,
                "% less stamina cost to run at level 100");
            RunStaminaPerLevel = cfg.Bind("Run.Effect", "Base Stamina Bonus", .6f,
                "How much base stamina is added per level of run");


            //Jump Effects
            JumpForceModMin = cfg.Bind("Jump.Effect", "Vertical Force Bonus Min", -10f,
                "% extra vertical jump force at level 0");
            JumpForceModMax = cfg.Bind("Jump.Effect", "Vertical Force Bonus Max", 90f,
                "% extra vertical jump force at level 100");
            JumpStaminaReduxMin = cfg.Bind("Jump.Effect", "Stamina Cost Reduction Min", 0f,
                "% less stamina cost to jump at level 0");
            JumpStaminaReduxMax = cfg.Bind("Jump.Effect", "Stamina Cost Reduction Max", 60f,
                "% less stamina cost to jump at level 100");
            JumpForwardForceModMin = cfg.Bind("Jump.Effect", "Horizontal Force Bonus Min", 0f,
                "% extra horizontal jump force at level 0");
            JumpForwardForceModMax = cfg.Bind("Jump.Effect", "Horizontal Force Bonus Max", 150f,
                "% extra horizontal jump force at level 100");
            //May not actually be used or be different from jump force
            JumpTiredReduxMax = cfg.Bind("Jump.Effect", "Jump Tired Stamina Reduction Max", 20f,
                "% added to the base game's tired factor, which reduces your jump force when out of stamina, at level 100");
            JumpTiredReduxMin = cfg.Bind("Jump.Effect", "Jump Tired Stamina Reduction Min", 0f,
                "% added to the base game's tired factor, which reduces your jump force when out of stamina, at level 0");

            WeaponEXPHoldUnarmedMod.Value = ModFix(WeaponEXPHoldUnarmedMod.Value);
            WeaponEXPHoldUnarmedMod.Value = ModFix(WeaponEXPHoldUnarmedMod.Value);
            WeaponEXPStrikeDamageMod.Value = ModFix(WeaponEXPStrikeDamageMod.Value);
            WeaponEXPStrikeDestructibleMod.Value = ModFix(WeaponEXPStrikeDestructibleMod.Value);
            WeaponBXPBowDistanceMod.Value = ModFix(WeaponBXPBowDistanceMod.Value);
            ToolEXPStrikeDamageMod.Value = ModFix(ToolEXPStrikeDamageMod.Value);
            BlockExpMod.Value = ModFix(BlockExpMod.Value);
            BlockParryExpMod.Value = ModFix(BlockParryExpMod.Value);
            BlockPowerModMin.Value = ModFix(BlockPowerModMin.Value);
            BlockPowerModMax.Value = ModFix(BlockPowerModMax.Value);
            BlockStaminaReduxMin.Value = ModFix(BlockStaminaReduxMin.Value, true);
            BlockStaminaReduxMax.Value = ModFix(BlockStaminaReduxMax.Value, true);
            BlockStaggerLimitModMin.Value = ModFix(BlockStaggerLimitModMin.Value);
            BlockStaggerLimitModMax.Value = ModFix(BlockStaggerLimitModMax.Value);
            JumpFallDamageReduxMin.Value = ModFix(JumpFallDamageReduxMin.Value, true);
            JumpFallDamageReduxMax.Value = ModFix(JumpFallDamageReduxMax.Value, true);
            SwimEXPSpeedMod.Value = ModFix(SwimEXPSpeedMod.Value);
            SwimSpeedModMin.Value = ModFix(SwimSpeedModMin.Value);
            SwimSpeedModMax.Value = ModFix(SwimSpeedModMax.Value);
            SwimAccelModMin.Value = ModFix(SwimAccelModMin.Value);
            SwimAccelModMax.Value = ModFix(SwimAccelModMax.Value);
            SwimTurnModMin.Value = ModFix(SwimTurnModMin.Value);
            SwimTurnModMax.Value = ModFix(SwimTurnModMax.Value);
            RunAbsoluteWeightExpMod.Value = ModFix(RunAbsoluteWeightExpMod.Value);
            RunRelativeWeightLight.Value = ModFix(RunRelativeWeightLight.Value);
            RunRelativeWeightLightMod.Value = ModFix(RunRelativeWeightLightMod.Value);
            RunRelativeWeightMed.Value = ModFix(RunRelativeWeightMed.Value);
            RunRelativeWeightMedMod.Value = ModFix(RunRelativeWeightMedMod.Value);
            RunRelativeWeightHighMed.Value = ModFix(RunRelativeWeightHighMed.Value);
            RunRelativeWeightHighMedMod.Value = ModFix(RunRelativeWeightHighMedMod.Value);
            RunRelativeWeightHeavy.Value = ModFix(RunRelativeWeightHeavy.Value);
            RunRelativeWeightHeavyMod.Value = ModFix(RunRelativeWeightHeavyMod.Value);
            RunRelativeWeightFull.Value = ModFix(RunRelativeWeightFull.Value);
            RunRelativeWeightFullMod.Value = ModFix(RunRelativeWeightFullMod.Value);
            RunRelativeWeightOverMod.Value = ModFix(RunRelativeWeightOverMod.Value);
            RunRelativeWeightExpMod.Value = ModFix(RunRelativeWeightExpMod.Value);
            RunEXPSpeedMod.Value = ModFix(RunEXPSpeedMod.Value);
            RunSpeedModMin.Value = ModFix(RunSpeedModMin.Value);
            RunSpeedModMax.Value = ModFix(RunSpeedModMax.Value);
            RunEquipmentReduxMin.Value = ModFix(RunEquipmentReduxMin.Value, true);
            RunEquipmentReduxMax.Value = ModFix(RunEquipmentReduxMax.Value, true);
            RunEncumberanceModMin.Value = ModFix(RunEncumberanceModMin.Value);
            RunEncumberanceModMax.Value = ModFix(RunEncumberanceModMax.Value);
            RunEncumberanceReduxMin.Value = ModFix(RunEncumberanceReduxMin.Value, true);
            RunEncumberanceReduxMax.Value = ModFix(RunEncumberanceReduxMax.Value, true);
            RunStaminaReduxMin.Value = ModFix(RunStaminaReduxMin.Value, true);
            RunStaminaReduxMax.Value = ModFix(RunStaminaReduxMax.Value, true);
            JumpForceModMin.Value = ModFix(JumpForceModMin.Value);
            JumpForceModMax.Value = ModFix(JumpForceModMax.Value);
            JumpStaminaReduxMin.Value = ModFix(JumpStaminaReduxMin.Value, true);
            JumpStaminaReduxMax.Value = ModFix(JumpStaminaReduxMax.Value, true);
            JumpForwardForceModMin.Value = ModFix(JumpForwardForceModMin.Value);
            JumpForwardForceModMax.Value = ModFix(JumpForwardForceModMax.Value);
            JumpTiredReduxMax.Value = ModFix(JumpTiredReduxMax.Value);
            JumpTiredReduxMin.Value = ModFix(JumpTiredReduxMin.Value);
        }

        // Quick and dirty
        private static float ModFix(float configVar, bool redux = false)
        {
            configVar /= 100;
            if (redux)
                configVar = 1 - configVar;
            return configVar;
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



        /*
         * 
         * Here be the Get functions
         * 
         */
        public static float GetBlockStaminaRedux(float skillFactor)
        {
            return 1f + Mathf.Lerp(BlockStaminaReduxMin.Value, BlockStaminaReduxMax.Value, skillFactor);
        }
        public static float GetFlatBlockPower(float skillFactor)
        {
            return Mathf.Lerp(BlockFlatPowerMin.Value, BlockFlatPowerMax.Value, skillFactor);
        }
        public static float GetBlockPowerMod(float skillFactor)
        {
            return 1f + Mathf.Lerp(BlockPowerModMin.Value, BlockPowerModMax.Value, skillFactor);
        }
        public static float GetFallDamageThreshold(float skillFactor)
        {
            return Mathf.Lerp(JumpFallDamageThresholdMin.Value, JumpFallDamageThresholdMax.Value, skillFactor);
        }
        public static float GetFallDamageRedux(float skillFactor)
        {
            return 1f + Mathf.Lerp(JumpFallDamageReduxMin.Value, JumpFallDamageReduxMax.Value, skillFactor);
        }
        public static float GetRunSpeedMod(float skillFactor)
        {
            return 1f + Mathf.Lerp(RunSpeedModMin.Value, RunSpeedModMax.Value, skillFactor);
        }
        public static float GetEncumberanceCurve(float encumberancePercent)
        {
            return Mathf.Lerp(RunEncumberanceModMin.Value, RunEncumberanceModMax.Value, ShapeFactorSin(encumberancePercent));
        }
        public static float GetEncumberanceRedux(float skillFactor)
        {
            return 1f + Mathf.Lerp(RunEncumberanceReduxMin.Value, RunEncumberanceReduxMax.Value, skillFactor);
        }
        public static float GetEquipmentRedux(float skillFactor)
        {
            return 1f + Mathf.Lerp(RunEquipmentReduxMin.Value, RunEquipmentReduxMax.Value, skillFactor);
        }
        public static float GetAbsoluteWeightPercent(float weight)
        {
            return Mathf.Clamp01((weight - RunAbsoluteWeightMinWeight.Value) / 
                (RunAbsoluteWeightMaxWeight.Value - RunAbsoluteWeightMinWeight.Value));
        }
        public static float GetAbsoluteWeightCurve(float weightPercent)
        {
            return 1f + RunAbsoluteWeightExpMod.Value * Mathf.Pow(weightPercent, RunAbsoluteWeightFactor.Value);
        }
        public static float GetRelativeWeightStage(float weightPercent)
        {
            float mod = 1f;

            if (weightPercent <= RunRelativeWeightLight.Value)
                mod = RunRelativeWeightLightMod.Value;

            else if (weightPercent <= RunRelativeWeightMed.Value)
                mod = RunRelativeWeightMedMod.Value;

            else if (weightPercent <= RunRelativeWeightHighMed.Value)
                mod = RunRelativeWeightHighMedMod.Value;

            else if (weightPercent <= RunRelativeWeightHeavy.Value)
                mod = RunRelativeWeightHeavyMod.Value;

            else if (weightPercent <= RunRelativeWeightFull.Value)
                mod = RunRelativeWeightFullMod.Value;

            else
                mod = RunRelativeWeightOverMod.Value;

            mod++;

            return mod * RunRelativeWeightExpMod.Value;
        }
        public static float GetRunStaminaRedux(float skillFactor)
        {
            return 1f + Mathf.Lerp(RunStaminaReduxMin.Value, RunStaminaReduxMax.Value, skillFactor);
        }
        public static float GetSwimSpeedMod(float skillFactor)
        {
            return 1f + Mathf.Lerp(SwimSpeedModMin.Value, SwimSpeedModMax.Value, skillFactor);
        }
        public static float GetSwimAccelMod(float skillFactor)
        {
            return 1f + Mathf.Lerp(SwimAccelModMin.Value, SwimAccelModMax.Value, skillFactor);
        }
        public static float GetSwimTurnMod(float skillFactor)
        {
            return 1f + Mathf.Lerp(SwimTurnModMin.Value, SwimTurnModMax.Value, skillFactor);
        }
        public static float GetJumpForceMod(float skillFactor)
        {
            return 1f + Mathf.Lerp(JumpForceModMin.Value, JumpForceModMax.Value, skillFactor);
        }
        public static float GetJumpForwardForceMod(float skillFactor)
        {
            return 1f + Mathf.Lerp(JumpForwardForceModMin.Value, JumpForwardForceModMax.Value, skillFactor);
        }
        public static float GetJumpStaminaRedux(float skillFactor)
        {
            return 1 + Mathf.Lerp(JumpStaminaReduxMin.Value, JumpStaminaReduxMax.Value, skillFactor);
        }
        public static float GetJumpTiredRedux(float skillFactor)
        {
            return Mathf.Lerp(JumpTiredReduxMin.Value, JumpTiredReduxMax.Value, skillFactor);
        }
        public static float GetWeaponDamageToExperience(float damage)
        {
            return WeaponEXPStrikeDamageMod.Value * Mathf.Pow(damage, WeaponEXPStrikeDamageFactor.Value);
        }
        public static float GetToolDamageToExperience(float damage)
        {
            return ToolEXPStrikeDamageMod.Value * Mathf.Pow(damage, ToolEXPStrikeDamageFactor.Value);
        }
        public static float GetSwimStaminaPerSec(float skillFactor)
        {
            return Mathf.Lerp(SwimStaminaPerSecMin.Value, SwimStaminaPerSecMax.Value, skillFactor);
        }

    }    
}
