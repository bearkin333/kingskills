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
        public static ConfigEntry<float> BlockFlatPowerMin;
        public static ConfigEntry<float> BlockFlatPowerMax;
        public static ConfigEntry<float> BlockPowerModMin;
        public static ConfigEntry<float> BlockPowerModMax;
        public static ConfigEntry<float> BlockStaminaReduxMin;
        public static ConfigEntry<float> BlockStaminaReduxMax;
        public static ConfigEntry<float> BlockStaggerLimitModMin;
        public static ConfigEntry<float> BlockStaggerLimitModMax;
        public static ConfigEntry<float> JumpFallDamageThresholdMin;
        public static ConfigEntry<float> JumpFallDamageThresholdMax;
        public static ConfigEntry<float> JumpFallDamageReduxMin;
        public static ConfigEntry<float> JumpFallDamageReduxMax;
        public static ConfigEntry<float> SwimEXPSpeedMod;
        public static ConfigEntry<float> SwimSpeedModMin;
        public static ConfigEntry<float> SwimSpeedModMax;
        public static ConfigEntry<float> SwimAccelModMin;
        public static ConfigEntry<float> SwimAccelModMax;
        public static ConfigEntry<float> SwimTurnModMin;
        public static ConfigEntry<float> SwimTurnModMax;
        public static ConfigEntry<float> SwimStaminaPerSecMin;
        public static ConfigEntry<float> SwimStaminaPerSecMax;
        public static ConfigEntry<float> RunAbsoluteWeightMinWeight;
        public static ConfigEntry<float> RunAbsoluteWeightMaxWeight;
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
        public static ConfigEntry<float> RunSpeedModMin;
        public static ConfigEntry<float> RunSpeedModMax;
        public static ConfigEntry<float> RunEquipmentReduxMin;
        public static ConfigEntry<float> RunEquipmentReduxMax;
        public static ConfigEntry<float> RunEncumberanceModMin;
        public static ConfigEntry<float> RunEncumberanceModMax;
        public static ConfigEntry<float> RunEncumberanceReduxMin;
        public static ConfigEntry<float> RunEncumberanceReduxMax;
        public static ConfigEntry<float> RunStaminaReduxMin;
        public static ConfigEntry<float> RunStaminaReduxMax;
        public static ConfigEntry<float> RunStaminaPerLevel;
        public static ConfigEntry<float> JumpForceModMin;
        public static ConfigEntry<float> JumpForceModMax;
        public static ConfigEntry<float> JumpStaminaReduxMin;
        public static ConfigEntry<float> JumpStaminaReduxMax;
        public static ConfigEntry<float> JumpForwardForceModMin;
        public static ConfigEntry<float> JumpForwardForceModMax;
        public static ConfigEntry<float> JumpTiredReduxMin;
        public static ConfigEntry<float> JumpTiredReduxMax;

        //Unorganized
        public static ConfigEntry<float> DropNewItemThreshold;
        public static ConfigEntry<float> AxeDamageModMin;
        public static ConfigEntry<float> AxeDamageModMax;
        public static ConfigEntry<float> AxeStaminaReduxMin;
        public static ConfigEntry<float> AxeStaminaReduxMax;
        public static ConfigEntry<float> AxeStaminaPerLevel;
        public static ConfigEntry<float> AxeChopDamageModMin;
        public static ConfigEntry<float> AxeChopDamageModMax;
        public static ConfigEntry<float> AxeCarryCapacityMin;
        public static ConfigEntry<float> AxeCarryCapacityMax;
        public static ConfigEntry<float> BowDamageModMin;
        public static ConfigEntry<float> BowDamageModMax;
        public static ConfigEntry<float> BowStaminaReduxMin;
        public static ConfigEntry<float> BowStaminaReduxMax;
        public static ConfigEntry<float> BowVelocityModMin;
        public static ConfigEntry<float> BowVelocityModMax;
        public static ConfigEntry<float> BowDrawSpeedModMin;
        public static ConfigEntry<float> BowDrawSpeedModMax;
        public static ConfigEntry<float> BowDropModMin;
        public static ConfigEntry<float> BowDropModMax;
        public static ConfigEntry<float> ClubDamageModMin;
        public static ConfigEntry<float> ClubDamageModMax;
        public static ConfigEntry<float> ClubStaminaReduxMin;
        public static ConfigEntry<float> ClubStaminaReduxMax;
        public static ConfigEntry<float> ClubBluntModMin;
        public static ConfigEntry<float> ClubBluntModMax;
        public static ConfigEntry<float> ClubKnockbackModMin;
        public static ConfigEntry<float> ClubKnockbackModMax;
        public static ConfigEntry<float> ClubStaggerModMin;
        public static ConfigEntry<float> ClubStaggerModMax;
        public static ConfigEntry<float> FistDamageModMin;
        public static ConfigEntry<float> FistDamageModMax;
        public static ConfigEntry<float> FistStaminaReduxMin;
        public static ConfigEntry<float> FistStaminaReduxMax;
        public static ConfigEntry<float> FistDamageFlatMin;
        public static ConfigEntry<float> FistDamageFlatMax;
        public static ConfigEntry<float> FistBlockArmorMin;
        public static ConfigEntry<float> FistBlockArmorMax;
        public static ConfigEntry<float> FistMovespeedModMin;
        public static ConfigEntry<float> FistMovespeedModMax;
        public static ConfigEntry<float> KnifeDamageModMin;
        public static ConfigEntry<float> KnifeDamageModMax;
        public static ConfigEntry<float> KnifeStaminaReduxMin;
        public static ConfigEntry<float> KnifeStaminaReduxMax;
        public static ConfigEntry<float> KnifeBackstabModMin;
        public static ConfigEntry<float> KnifeBackstabModMax;
        public static ConfigEntry<float> KnifeMovespeedModMin;
        public static ConfigEntry<float> KnifeMovespeedModMax;
        public static ConfigEntry<float> KnifePierceModMin;
        public static ConfigEntry<float> KnifePierceModMax;
        public static ConfigEntry<float> PolearmDamageModMin;
        public static ConfigEntry<float> PolearmDamageModMax;
        public static ConfigEntry<float> PolearmStaminaReduxMin;
        public static ConfigEntry<float> PolearmStaminaReduxMax;
        public static ConfigEntry<float> PolearmRangeMax;
        public static ConfigEntry<float> PolearmRangeMin;
        public static ConfigEntry<float> PolearmArmorMax;
        public static ConfigEntry<float> PolearmArmorMin;
        public static ConfigEntry<float> PolearmBlockMax;
        public static ConfigEntry<float> PolearmBlockMin;
        public static ConfigEntry<float> SpearDamageModMin;
        public static ConfigEntry<float> SpearDamageModMax;
        public static ConfigEntry<float> SpearStaminaReduxMin;
        public static ConfigEntry<float> SpearStaminaReduxMax;
        public static ConfigEntry<float> SpearVelocityModMax;
        public static ConfigEntry<float> SpearVelocityModMin;
        public static ConfigEntry<float> SpearProjectileDamageModMax;
        public static ConfigEntry<float> SpearProjectileDamageModMin;
        public static ConfigEntry<float> SpearBlockArmorMax;
        public static ConfigEntry<float> SpearBlockArmorMin;
        public static ConfigEntry<float> SwordDamageModMin;
        public static ConfigEntry<float> SwordDamageModMax;
        public static ConfigEntry<float> SwordStaminaReduxMin;
        public static ConfigEntry<float> SwordStaminaReduxMax;
        public static ConfigEntry<float> SwordParryModMin;
        public static ConfigEntry<float> SwordParryModMax;
        public static ConfigEntry<float> SwordSlashModMin;
        public static ConfigEntry<float> SwordSlashModMax;
        public static ConfigEntry<float> SwordDodgeStaminaReduxMin;
        public static ConfigEntry<float> SwordDodgeStaminaReduxMax;
        public static ConfigEntry<float> WoodcuttingChopDamageModMin;
        public static ConfigEntry<float> WoodcuttingChopDamageModMax;
        public static ConfigEntry<float> WoodcuttingStaminaRebateMin;
        public static ConfigEntry<float> WoodcuttingStaminaRebateMax;
        public static ConfigEntry<float> WoodcuttingDropModMin;
        public static ConfigEntry<float> WoodcuttingDropModMax;
        public static ConfigEntry<float> MiningPickDamageModMin;
        public static ConfigEntry<float> MiningPickDamageModMax;
        public static ConfigEntry<float> MiningStaminaRebateMin;
        public static ConfigEntry<float> MiningStaminaRebateMax;
        public static ConfigEntry<float> MiningDropModMin;
        public static ConfigEntry<float> MiningDropModMax;

        //Constants for use that aren't configurable
        public static Dictionary<string, float> WoodcuttingDropTable;
        public static Dictionary<string, float> MiningDropTable;
        public static Dictionary<string, float> BowDropTable;

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
            //List of all object names that count as woodcutting drops
            WoodcuttingDropTable.Add("BeechSeeds", 0);
            WoodcuttingDropTable.Add("ElderBark", 0);
            WoodcuttingDropTable.Add("FineWood", 0);
            WoodcuttingDropTable.Add("FirCone", 0);
            WoodcuttingDropTable.Add("PineCone", 0);
            WoodcuttingDropTable.Add("RoundLog", 0);
            WoodcuttingDropTable.Add("Wood", 0);

            //List of all object names that count as mining drops
            MiningDropTable.Add("CopperOre", 0);
            MiningDropTable.Add("IronScrap", 0);
            MiningDropTable.Add("Obsidian", 0);
            MiningDropTable.Add("SilverOre", 0);
            MiningDropTable.Add("TinOre", 0);
            MiningDropTable.Add("Stone", 0);
            MiningDropTable.Add("Chitin", 0);

            //List of all object names that count as bow drops
            BowDropTable.Add("LeatherScraps", 0);
            BowDropTable.Add("DeerHide", 0);
            BowDropTable.Add("BoarMeat", 0);
            BowDropTable.Add("DeerMeat", 0);
            BowDropTable.Add("NeckTail", 0);
            BowDropTable.Add("Resin", 0);
            BowDropTable.Add("Feathers", 0);
            BowDropTable.Add("Guck", 0);
            BowDropTable.Add("WitheredBone", 0);
            BowDropTable.Add("BoneFragments", 0);

            /* Template
            x = cfg.Bind("g", "t", 0f, 
                "d");
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
            PercentToMultiplier(ref  WeaponEXPHoldUnarmedMod);


            //Weapon Strike Experience
            WeaponEXPStrikeDamageMod = cfg.Bind("Experience.Weapons", "XP Per Damage", 40f,
                "% of damage dealt that becomes experience earned");
            PercentToMultiplier(ref WeaponEXPStrikeDamageMod);
            WeaponEXPStrikeDamageFactor = cfg.Bind("Experience.Weapons", "XP Per Damage Curve", .42f,
                "Factor to define the slope of the damage to xp curve");
            WeaponEXPStrikeDestructibleMod = cfg.Bind("Experience.Weapons", "Destructible Mod", 10f,
                "% of experience gained when you hit a non living thing");
            PercentToMultiplier(ref WeaponEXPStrikeDestructibleMod);


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
            PercentToMultiplier(ref WeaponBXPBowDistanceMod);


            //Tool experience
            ToolEXPStrikeDamageMod = cfg.Bind("Experience.Tools", "Tool Damage Mod", 150f,
                "% of damage done to resources that becomes experience for gathering skills " +
                "(Woodcutting, Mining)");
            PercentToMultiplier(ref ToolEXPStrikeDamageMod);
            ToolEXPStrikeDamageFactor = cfg.Bind("Experience.Tools", "Tool Damage Factor", .22f,
                "Factor to define the slope of the damage to xp curve");


            //Block experience
            BlockExpMod = cfg.Bind("Block.Experience", "Experience Mod", 22f,
                "% of damage blocked that turns into experience");
            PercentToMultiplier(ref BlockExpMod);
            BlockParryExpMod = cfg.Bind("Block.Experience", "Parry Bonus", 200f,
                "% extra experience earned when parrying an attack");
            PercentToMultiplier(ref BlockParryExpMod);


            //Block effects
            BlockFlatPowerMin = cfg.Bind("Block.Effect", "Flat Armor Bonus Min", 0f,
                "This flat value is added to block armor at level 0");
            BlockFlatPowerMax = cfg.Bind("Block.Effect", "Flat Armor Bonus Max", 50f,
                "This flat value is added to block armor at level 100");
            BlockPowerModMin = cfg.Bind("Block.Effect", "% Bonus Block Armor Min", -25f,
                "% change in total block armor at level 0");
            PercentToMultiplier(ref BlockPowerModMin);
            BlockPowerModMax = cfg.Bind("Block.Effect", "% Bonus Block Armor Max", 100f,
                "% change in total block armor at level 100");
            PercentToMultiplier(ref BlockPowerModMax);
            BlockStaminaReduxMin = cfg.Bind("Block.Effect", "Stamina Cost Reduction Min", -10f,
                "% less stamina to block at level 0");
            PercentToMultiplier(ref BlockStaminaReduxMin, true);
            BlockStaminaReduxMax = cfg.Bind("Block.Effect", "Stamina Cost Reduction Max", 50f,
                "% less stamina to block at level 100");
            PercentToMultiplier(ref BlockStaminaReduxMax, true);
            BlockStaggerLimitModMin = cfg.Bind("Block.Effect", "Stagger Limit Bonus Min", 0f,
                "% of max health to add to player's stagger limit at level 0");
            PercentToMultiplier(ref BlockStaggerLimitModMin);
            BlockStaggerLimitModMax = cfg.Bind("Block.Effect", "Stagger Limit Bonus Max", 30f,
                "% of max health to add to player's stagger limit at level 100");
            PercentToMultiplier(ref BlockStaggerLimitModMax);

            //Jump Effects
            JumpFallDamageThresholdMin = cfg.Bind("Jump.Effect", "Fall Damage Threshold Min", 4f,
                "meters to fall before you start calculating fall damage at level 0");
            JumpFallDamageThresholdMax = cfg.Bind("Jump.Effect", "Fall Damage Threshold Max", 12f,
                "meters to fall before you start calculating fall damage at level 100");
            JumpFallDamageReduxMin = cfg.Bind("Jump.Effect", "Fall Damage Reduction Min", -15f,
                "% less fall damage to take at level 0");
            PercentToMultiplier(ref JumpFallDamageReduxMin, true);
            JumpFallDamageReduxMax = cfg.Bind("Jump.Effect", "Fall Damage Reduction Max", 60f,
                "% less fall damage to take at level 100");
            PercentToMultiplier(ref JumpFallDamageReduxMax, true);


            //Swim Experience
            SwimEXPSpeedMod = cfg.Bind("Swim.Experience", "Swim Experience Mod", 100f,
                "% of swim speed that becomes bonus experience gain");
            PercentToMultiplier(ref SwimEXPSpeedMod);


            //Swim effects
            SwimSpeedModMin = cfg.Bind("Swim.Effect", "Speed Min", 0f,
                "% to increase swim speed at level 0");
            PercentToMultiplier(ref SwimSpeedModMin);
            SwimSpeedModMax = cfg.Bind("Swim.Effect", "Speed Max", 300f,
                "% to increase swim speed at level 100");
            PercentToMultiplier(ref SwimSpeedModMax);
            SwimAccelModMin = cfg.Bind("Swim.Effect", "Acceleration Min", 0f,
                "% to increase swim acceleration at level 0");
            PercentToMultiplier(ref SwimAccelModMin);
            SwimAccelModMax = cfg.Bind("Swim.Effect", "Acceleration Max", 300f,
                "% to increase swim acceleration at level 100");
            PercentToMultiplier(ref SwimAccelModMax);
            SwimTurnModMin = cfg.Bind("Swim.Effect", "Turn Speed Min", 0f,
                "% to increase swim turn speed at level 0");
            PercentToMultiplier(ref SwimTurnModMin);
            SwimTurnModMax = cfg.Bind("Swim.Effect", "Turn Speed Max", 500f,
                "% to increase swim turn speed at level 100");
            PercentToMultiplier(ref SwimTurnModMax);
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
            PercentToMultiplier(ref RunAbsoluteWeightExpMod);


            //Run Relative Weight Experience
            RunRelativeWeightLight = cfg.Bind("Run.Experience.RelativeWeight", "Light Threshold", 33f,
                "Threshold for being at 'light' encumberance");
            PercentToMultiplier(ref RunRelativeWeightLight);
            RunRelativeWeightLightMod = cfg.Bind("Run.Experience.RelativeWeight", "Light Modifier", -25f,
                "Experience Bonus for being at 'light' encumberance");
            PercentToMultiplier(ref RunRelativeWeightLightMod);
            RunRelativeWeightMed = cfg.Bind("Run.Experience.RelativeWeight", "Medium Threshold", 50f,
                "Threshold for being at 'medium' encumberance");
            PercentToMultiplier(ref RunRelativeWeightMed);
            RunRelativeWeightMedMod = cfg.Bind("Run.Experience.RelativeWeight", "Medium Modifier", 0f,
                "Experience Bonus for being at 'medium' encumberance");
            PercentToMultiplier(ref RunRelativeWeightMedMod);
            RunRelativeWeightHighMed = cfg.Bind("Run.Experience.RelativeWeight", "High Threshold", 66f,
                "Threshold for being at 'high' encumberance");
            PercentToMultiplier(ref RunRelativeWeightHighMed);
            RunRelativeWeightHighMedMod = cfg.Bind("Run.Experience.RelativeWeight", "High Modifier", 25f,
                "Experience Bonus for being at 'high' encumberance");
            PercentToMultiplier(ref RunRelativeWeightHighMedMod);
            RunRelativeWeightHeavy = cfg.Bind("Run.Experience.RelativeWeight", "Heavy Threshold", 80f,
                "Threshold for being at 'heavy' encumberance");
            PercentToMultiplier(ref RunRelativeWeightHeavy);
            RunRelativeWeightHeavyMod = cfg.Bind("Run.Experience.RelativeWeight", "Heavy Modifier", 50f,
                "Experience Bonus for being at 'heavy' encumberance");
            PercentToMultiplier(ref RunRelativeWeightHeavyMod);
            RunRelativeWeightFull = cfg.Bind("Run.Experience.RelativeWeight", "Full Threshold", 100f,
                "Threshold for being at 'full' encumberance");
            PercentToMultiplier(ref RunRelativeWeightFull);
            RunRelativeWeightFullMod = cfg.Bind("Run.Experience.RelativeWeight", "Full Modifier", 80f,
                "Experience Bonus for being at 'full' encumberance");
            PercentToMultiplier(ref RunRelativeWeightFullMod);
            RunRelativeWeightOverMod = cfg.Bind("Run.Experience.RelativeWeight", "Overweight Modifier", 200f,
                "Experience Bonus for being overencumbered");
            PercentToMultiplier(ref RunRelativeWeightOverMod);
            RunRelativeWeightExpMod = cfg.Bind("Run.Experience.RelativeWeight", "Overall Modifier", 100f,
                "% modifier for how much experience you get from relative weight");
            PercentToMultiplier(ref RunRelativeWeightExpMod);


            //Run Experience
            RunEXPSpeedMod = cfg.Bind("Run.Experience", "Run Experience Mod", 100f,
                "% of run speed that becomes bonus experience gain");
            PercentToMultiplier(ref RunEXPSpeedMod);


            //Run Effects
            RunSpeedModMin = cfg.Bind("Run.Effect", "Speed Min", 0f,
                "% extra run speed at level 0");
            PercentToMultiplier(ref RunSpeedModMin);
            RunSpeedModMax = cfg.Bind("Run.Effect", "Speed Max", 250f,
                "% extra run speed at level 100");
            PercentToMultiplier(ref RunSpeedModMax);
            RunEquipmentReduxMin = cfg.Bind("Run.Effect", "Equipment Reduction Min", 0f,
                "% less movespeed reduction from equipment at level 0");
            PercentToMultiplier(ref RunEquipmentReduxMin, true);
            RunEquipmentReduxMax = cfg.Bind("Run.Effect", "Equipment Reduction Max", 50f,
                "% less movespeed reduction from equipment at level 100");
            PercentToMultiplier(ref RunEquipmentReduxMax, true);
            RunEncumberanceModMin = cfg.Bind("Run.Effect", "Encumberance Min", 0f,
                "% less run speed when your inventory is empty");
            PercentToMultiplier(ref RunEncumberanceModMin);
            RunEncumberanceModMax = cfg.Bind("Run.Effect", "Encumberance Max", 50f,
                "% less run speed when your inventory is full");
            PercentToMultiplier(ref RunEncumberanceModMax);
            RunEncumberanceReduxMin = cfg.Bind("Run.Effect", "Encumberance Reduction Min", 0f,
                "% less effect from encumberance at level 0");
            PercentToMultiplier(ref RunEncumberanceReduxMin, true);
            RunEncumberanceReduxMax = cfg.Bind("Run.Effect", "Encumberance Reduction Max", 50f, 
                "% less effect from encumberance at level 100");
            PercentToMultiplier(ref RunEncumberanceReduxMax, true);
            RunStaminaReduxMin = cfg.Bind("Run.Effect", "Stamina Reduction Min", -25f,
                "% less stamina cost to run at level 100");
            PercentToMultiplier(ref RunStaminaReduxMin, true);
            RunStaminaReduxMax = cfg.Bind("Run.Effect", "Stamina Reduction Max", 80f,
                "% less stamina cost to run at level 100");
            PercentToMultiplier(ref RunStaminaReduxMax, true);
            RunStaminaPerLevel = cfg.Bind("Run.Effect", "Base Stamina Bonus", .6f,
                "How much base stamina is added per level of run");


            //Jump Effects
            JumpForceModMin = cfg.Bind("Jump.Effect", "Vertical Force Bonus Min", -10f,
                "% extra vertical jump force at level 0");
            PercentToMultiplier(ref JumpForceModMin);
            JumpForceModMax = cfg.Bind("Jump.Effect", "Vertical Force Bonus Max", 90f,
                "% extra vertical jump force at level 100");
            PercentToMultiplier(ref JumpForceModMax);
            JumpStaminaReduxMin = cfg.Bind("Jump.Effect", "Stamina Cost Reduction Min", 0f,
                "% less stamina cost to jump at level 0");
            PercentToMultiplier(ref JumpStaminaReduxMin, true);
            JumpStaminaReduxMax = cfg.Bind("Jump.Effect", "Stamina Cost Reduction Max", 60f,
                "% less stamina cost to jump at level 100");
            PercentToMultiplier(ref JumpStaminaReduxMax, true);
            JumpForwardForceModMin = cfg.Bind("Jump.Effect", "Horizontal Force Bonus Min", 0f,
                "% extra horizontal jump force at level 0");
            PercentToMultiplier(ref JumpForwardForceModMin);
            JumpForwardForceModMax = cfg.Bind("Jump.Effect", "Horizontal Force Bonus Max", 150f,
                "% extra horizontal jump force at level 100");
            PercentToMultiplier(ref JumpForwardForceModMax);
            //May not actually be used or be different from jump force
            JumpTiredReduxMax = cfg.Bind("Jump.Effect", "Jump Tired Stamina Reduction Max", 20f,
                "% added to the base game's tired factor, which reduces your jump force when out of stamina, at level 100");
            PercentToMultiplier(ref JumpTiredReduxMax, true);
            JumpTiredReduxMin = cfg.Bind("Jump.Effect", "Jump Tired Stamina Reduction Min", 0f,
                "% added to the base game's tired factor, which reduces your jump force when out of stamina, at level 0");
            PercentToMultiplier(ref JumpTiredReduxMin, true);

            //Unorganized



            AxeDamageModMin = cfg.Bind("Axe.Effect", "Damage Min", 0f, 
                "% extra damage done with axes at level 0");
            PercentToMultiplier(ref AxeDamageModMin);
            AxeDamageModMax = cfg.Bind("Axe.Effect", "Damage Max", 0f,
                "% extra damage done with axes at level 100");
            PercentToMultiplier(ref AxeDamageModMax);
            AxeStaminaReduxMin = cfg.Bind("Axe.Effect", "Stamina Reduction Min", 0f,
                "% less stamina usage for axes at level 0");
            PercentToMultiplier(ref AxeStaminaReduxMin, true);
            AxeStaminaReduxMax = cfg.Bind("Axe.Effect", "Stamina Reduction Max", 0f,
                "% less stamina usage for axes at level 100");
            PercentToMultiplier(ref AxeStaminaReduxMax, true);
            AxeStaminaPerLevel = cfg.Bind("Axe.Effect", "Base Stamina Gain per Level", 0f,
                "Flat amount of base stamina gained per level in axe");
            AxeChopDamageModMin = cfg.Bind("Axe.Effect", "Woodcutting Damage Min", 0f,
                "% extra woodcutting damage done at level 0");
            PercentToMultiplier(ref AxeChopDamageModMin);
            AxeChopDamageModMax = cfg.Bind("Axe.Effect", "Woodcutting Damage Max", 0f,
                "% extra woodcutting damage done at level 100");
            PercentToMultiplier(ref AxeChopDamageModMax);
            AxeCarryCapacityMin = cfg.Bind("Axe.Effect", "Carry Capacity Bonus Min", 0f, 
                "Flat extra carrying capacity at level 0");
            AxeCarryCapacityMax = cfg.Bind("Axe.Effect", "Carry Capacity Bonus Max", 0f, 
                "Flat extra carrying capacity at level 100");
            BowDamageModMin = cfg.Bind("Bow.Effect", "Damage Min", 0f,
                "% extra damage done with bows at level 0");
            PercentToMultiplier(ref BowDamageModMin);
            BowDamageModMax = cfg.Bind("Bow.Effect", "Damage Max", 0f,
                "% extra damage done with bows at level 100");
            PercentToMultiplier(ref BowDamageModMax);
            BowStaminaReduxMin = cfg.Bind("Bow.Effect", "Stamina Reduction Min", 0f,
                "% less stamina usage for bows at level 0");
            PercentToMultiplier(ref BowStaminaReduxMin, true);
            BowStaminaReduxMax = cfg.Bind("Bow.Effect", "Stamina Reduction Max", 0f,
                "% less stamina usage for bows at level 100");
            PercentToMultiplier(ref BowStaminaReduxMax, true);
            BowVelocityModMin = cfg.Bind("Bow.Effect", "Velocity Bonus Min", 0f, 
                "% extra velocity to fired arrows at level 0");
            PercentToMultiplier(ref BowVelocityModMin);
            BowVelocityModMax = cfg.Bind("Bow.Effect", "Velocity Bonus Max", 0f, 
                "% extra velocity to fired arrows at level 100");
            PercentToMultiplier(ref BowVelocityModMax);
            BowDrawSpeedModMin = cfg.Bind("Bow.Effect", "Draw Speed Min", 0f, 
                "% extra bow draw speed at level 0");
            PercentToMultiplier(ref BowDrawSpeedModMin);
            BowDrawSpeedModMax = cfg.Bind("Bow.Effect", "Draw Speed Max", 0f, 
                "% extra bow draw speed at level 100");
            PercentToMultiplier(ref BowDrawSpeedModMax);
            BowDropModMin = cfg.Bind("Bow.Effect", "Drop rate mod min", 0f,
                "% to increase creature drops at level 0");
            PercentToMultiplier(ref BowDropModMin);
            BowDropModMax = cfg.Bind("Bow.Effect", "Drop rate mod max", 100f,
                "% to increase creature drops at level 100");
            PercentToMultiplier(ref BowDropModMax);
            ClubDamageModMin = cfg.Bind("Club.Effect", "Damage Min", 0f,
                "% extra damage done with clubs at level 0");
            PercentToMultiplier(ref ClubDamageModMin);
            ClubDamageModMax = cfg.Bind("Club.Effect", "Damage Max", 0f,
                "% extra damage done with clubs at level 100");
            PercentToMultiplier(ref ClubDamageModMax);
            ClubStaminaReduxMin = cfg.Bind("Club.Effect", "Stamina Reduction Min", 0f,
                "% less stamina usage for clubs at level 0");
            PercentToMultiplier(ref ClubStaminaReduxMin, true);
            ClubStaminaReduxMax = cfg.Bind("Club.Effect", "Stamina Reduction Max", 0f,
                "% less stamina usage for clubs at level 100");
            PercentToMultiplier(ref ClubStaminaReduxMax, true);
            ClubBluntModMin = cfg.Bind("Club.Effect", "Generic Blunt Bonus Min", 0f, 
                "% extra blunt damage to ALL weapons at level 0");
            PercentToMultiplier(ref ClubBluntModMin);
            ClubBluntModMax = cfg.Bind("Club.Effect", "Generic Blunt Bonus Max", 0f,
                "% extra blunt damage to ALL weapons at level 100");
            PercentToMultiplier(ref ClubBluntModMax);
            ClubKnockbackModMin = cfg.Bind("Club.Effect", "Generic Knockback Bonus Min", 0f,
                "% extra knockback to ALL weapons at level 0");
            PercentToMultiplier(ref ClubKnockbackModMin);
            ClubKnockbackModMax = cfg.Bind("Club.Effect", "Generic Knockback Bonus Max", 0f,
                "% extra knockback to ALL weapons at level 100");
            PercentToMultiplier(ref ClubKnockbackModMax);
            ClubStaggerModMin = cfg.Bind("Club.Effect", "Generic Stagger Bonus Min", 0f, 
                "% extra stagger damage to ALL ATTACKS at level 0");
            PercentToMultiplier(ref ClubStaggerModMin);
            ClubStaggerModMax = cfg.Bind("Club.Effect", "Generic Stagger Bonus Min", 0f,
                "% extra stagger damage to ALL ATTACKS at level 100");
            PercentToMultiplier(ref ClubStaggerModMax);
            FistDamageModMin = cfg.Bind("Fist.Effect", "Damage Min", 0f,
                "% extra damage done with bare fists at level 0");
            PercentToMultiplier(ref FistDamageModMin);
            FistDamageModMax = cfg.Bind("Fist.Effect", "Damage Max", 0f,
                "% extra damage done with bare fists at level 100");
            PercentToMultiplier(ref FistDamageModMax);
            FistStaminaReduxMin = cfg.Bind("Fist.Effect", "Stamina Reduction Min", 0f,
                "% less stamina usage for fists at level 0");
            PercentToMultiplier(ref FistStaminaReduxMin, true);
            FistStaminaReduxMax = cfg.Bind("Fist.Effect", "Stamina Reduction Max", 0f,
                "% less stamina usage for fists at level 100");
            PercentToMultiplier(ref FistStaminaReduxMax, true);
            FistDamageFlatMin = cfg.Bind("Fist.Effect", "Flat Damage Min", 0f, 
                "Flat extra damage at level 0");
            FistDamageFlatMax = cfg.Bind("Fist.Effect", "Flat Damage Max", 0f, 
                "Flat extra damage at level 100");
            FistBlockArmorMin = cfg.Bind("Fist.Effect", "Unarmed Block Armor Min", 0f, 
                "Flat extra unarmed block armor at level 0");
            FistBlockArmorMax = cfg.Bind("Fist.Effect", "Unarmed Block Armor Min", 0f, 
                "Flat extra unarmed block armor at level 100");
            FistMovespeedModMin = cfg.Bind("Fist.Effect", "Movespeed Bonus Min", 0f, 
                "% movespeed increase at level 0");
            PercentToMultiplier(ref FistMovespeedModMin);
            FistMovespeedModMax = cfg.Bind("Fist.Effect", "Movespeed Bonus Min", 0f,
                "% movespeed increase at level 100");
            PercentToMultiplier(ref FistMovespeedModMax);
            KnifeDamageModMin = cfg.Bind("Knife.Effect", "Damage Min", 0f, 
                "% extra damage done with knives at level 0");
            PercentToMultiplier(ref KnifeDamageModMin);
            KnifeDamageModMax = cfg.Bind("Knife.Effect", "Damage Max", 0f,
                "% extra damage done with knives at level 100");
            PercentToMultiplier(ref KnifeDamageModMax);
            KnifeStaminaReduxMin = cfg.Bind("Knife.Effect", "Stamina Reduction Min", 0f,
                "% less stamina usage for knives at level 0");
            PercentToMultiplier(ref KnifeStaminaReduxMin, true);
            KnifeStaminaReduxMax = cfg.Bind("Knife.Effect", "Stamina Reduction Max", 0f,
                "% less stamina usage for knives at level 100");
            PercentToMultiplier(ref KnifeStaminaReduxMax, true);
            KnifeBackstabModMin = cfg.Bind("Knife.Effect", "Backstab Bonus Damage Min", 0f,
                "% extra sneak attack damage with ALL weapons at level 0");
            PercentToMultiplier(ref KnifeBackstabModMin);
            KnifeBackstabModMax = cfg.Bind("Knife.Effect", "Backstab Bonus Damage Max", 0f,
                "% extra sneak attack damage with ALL weapons at level 100");
            PercentToMultiplier(ref KnifeBackstabModMax);
            KnifeMovespeedModMin = cfg.Bind("Knife.Effect", "Movementspeed Bonus Min", 0f,
                "% movespeed increase at level 0");
            PercentToMultiplier(ref KnifeMovespeedModMin);
            KnifeMovespeedModMax = cfg.Bind("Knife.Effect", "Movementspeed Bonus Max", 0f,
                "% movespeed increase at level 100");
            PercentToMultiplier(ref KnifeMovespeedModMax);
            KnifePierceModMin = cfg.Bind("Knife.Effect", "Generic Pierce Damage Bonus Min", 0f,
                "% extra pierce damage with ALL weapons at level 0");
            PercentToMultiplier(ref KnifePierceModMin);
            KnifePierceModMax = cfg.Bind("Knife.Effect", "Generic Pierce Damage Bonus Max", 0f,
                "% extra pierce damage with ALL weapons at level 0");
            PercentToMultiplier(ref KnifePierceModMax);
            PolearmDamageModMin = cfg.Bind("Polearm.Effect", "Damage Min", 0f,
                "% extra damage done with polearms at level 0");
            PercentToMultiplier(ref PolearmDamageModMin);
            PolearmDamageModMax = cfg.Bind("Polearm.Effect", "Damage Max", 0f,
                "% extra damage done with polearms at level 100");
            PercentToMultiplier(ref PolearmDamageModMax);
            PolearmStaminaReduxMin = cfg.Bind("Polearm.Effect", "Stamina Reduction Min", 0f,
                "% less stamina usage for polearms at level 0");
            PercentToMultiplier(ref PolearmStaminaReduxMin, true);
            PolearmStaminaReduxMax = cfg.Bind("Polearm.Effect", "Stamina Reduction Max", 0f,
                "% less stamina usage for polearms at level 100");
            PercentToMultiplier(ref PolearmStaminaReduxMax, true);
            PolearmRangeMax = cfg.Bind("Polearm.Effect", "Generic Range Bonus Min", 0f,
                "Added units of range to all weapon attacks at level 0");
            PolearmRangeMin = cfg.Bind("Polearm.Effect", "Generic Range Bonus Min", 0f,
                "Added units of range to all weapon attacks at level 100");
            PolearmArmorMax = cfg.Bind("Polearm.Effect", "Flat Armor Bonus Min", 0f,
                "Flat armor added to character at level 0");
            PolearmArmorMin = cfg.Bind("Polearm.Effect", "Flat Armor Bonus Max", 0f,
                "Flat armor added to character at level 100");
            PolearmBlockMax = cfg.Bind("Polearm.Effect", "Block Armor Min", 0f,
                "Flat block armor added to polearms at level 0");
            PolearmBlockMin = cfg.Bind("Polearm.Effect", "Block Armor Max", 0f,
                "Flat block armor added to polearms at level 0");
            SpearDamageModMin = cfg.Bind("Spear.Effect", "Damage Min", 0f,
                "% extra damage done with spears at level 0");
            PercentToMultiplier(ref SpearDamageModMin);
            SpearDamageModMax = cfg.Bind("Spear.Effect", "Damage Max", 0f,
                "% extra damage done with spears at level 100");
            PercentToMultiplier(ref SpearDamageModMax);
            SpearStaminaReduxMin = cfg.Bind("Spear.Effect", "Stamina Reduction Min", 0f,
                "% less stamina usage for spears at level 0");
            PercentToMultiplier(ref SpearStaminaReduxMin, true);
            SpearStaminaReduxMax = cfg.Bind("Spear.Effect", "Stamina Reduction Max", 0f,
                "% less stamina usage for spears at level 100");
            PercentToMultiplier(ref SpearStaminaReduxMax, true);
            SpearVelocityModMin = cfg.Bind("Spear.Effect", "Thrown Velocity Min", 0f,
                "% extra velocity on thrown weapons at level 0");
            PercentToMultiplier(ref SpearVelocityModMin);
            SpearVelocityModMax = cfg.Bind("Spear.Effect", "Thrown Velocity Max", 0f,
                "% extra velocity on thrown weapons at level 100");
            PercentToMultiplier(ref SpearVelocityModMax);
            SpearProjectileDamageModMin = cfg.Bind("Spear.Effect", "Thrown Damage Min", 0f,
                "% extra damage done with thrown weapons at level 0");
            PercentToMultiplier(ref SpearProjectileDamageModMin);
            SpearProjectileDamageModMax = cfg.Bind("Spear.Effect", "Thrown Damage Max", 0f,
                "% extra damage done with thrown weapons at level 100");
            PercentToMultiplier(ref SpearProjectileDamageModMax);
            SpearBlockArmorMin = cfg.Bind("Spear.Effect", "Generic Block Armor Min", 0f, 
                "Flat block armor always applied at level 0");
            SpearBlockArmorMax = cfg.Bind("Spear.Effect", "Generic Block Armor Max", 0f,
                "Flat block armor always applied at level 100");
            SwordDamageModMin = cfg.Bind("Sword.Effect", "Damage Min", 0f,
                "% extra damage done with swords at level 0");
            PercentToMultiplier(ref SwordDamageModMin);
            SwordDamageModMax = cfg.Bind("Sword.Effect", "Damage Max", 0f,
                "% extra damage done with swords at level 100");
            PercentToMultiplier(ref SwordDamageModMax);
            SwordStaminaReduxMin = cfg.Bind("Sword.Effect", "Stamina Reduction Min", 0f,
                "% less stamina usage for swords at level 0");
            PercentToMultiplier(ref SwordStaminaReduxMin, true);
            SwordStaminaReduxMax = cfg.Bind("Sword.Effect", "Stamina Reduction Max", 0f,
                "% less stamina usage for swords at level 100");
            PercentToMultiplier(ref SwordStaminaReduxMax, true);
            SwordParryModMin = cfg.Bind("Sword.Effect", "Generic Parry Bonus Min", 0f,
                "% extra parry bonus for ALL weapons at level 0"); 
            PercentToMultiplier(ref SwordParryModMin);
            SwordParryModMax = cfg.Bind("Sword.Effect", "Generic Parry Bonus Max", 0f,
                "% extra parry bonus for ALL weapons at level 100");
            PercentToMultiplier(ref SwordParryModMax);
            SwordSlashModMin = cfg.Bind("Sword.Effect", "Generic Slash Damage Mod Min", 0f, 
                "% extra slash damage for ALL weapons at level 0");
            PercentToMultiplier(ref SwordSlashModMin);
            SwordSlashModMax = cfg.Bind("Sword.Effect", "Generic Slash Damage Mod Max", 0f,
                "% extra slash damage for ALL weapons at level 100"); 
            PercentToMultiplier(ref SwordSlashModMax);
            SwordDodgeStaminaReduxMin = cfg.Bind("Sword.Effect", "Dodgeroll Stamina Reduction Min", 0f, 
                "% less stamina cost to dodge roll at level 0");
            PercentToMultiplier(ref SwordDodgeStaminaReduxMin, true);
            SwordDodgeStaminaReduxMax = cfg.Bind("Sword.Effect", "Dodgeroll Stamina Reduction Min", 0f,
                "% less stamina cost to dodge roll at level 0");
            PercentToMultiplier(ref SwordDodgeStaminaReduxMax, true);


            WoodcuttingChopDamageModMin = cfg.Bind("Wood.Effect", "Chop Damage Bonus Min", 0f,
                "% increase to chop damage at level 0");
            PercentToMultiplier(ref WoodcuttingChopDamageModMin);
            WoodcuttingChopDamageModMax = cfg.Bind("Wood.Effect", "Chop Damage Bonus Max", 0f,
                "% increase to chop damage at level 100");
            PercentToMultiplier(ref WoodcuttingChopDamageModMax);
            WoodcuttingStaminaRebateMin = cfg.Bind("Wood.Effect", "Stamina Rebate Min", 0f,
                "Flat stamina rebate on each hit of a tree at level 0");
            WoodcuttingStaminaRebateMax = cfg.Bind("Wood.Effect", "Stamina Rebate Min", 0f,
                "Flat stamina rebate on each hit of a tree at level 100");
            WoodcuttingDropModMin = cfg.Bind("Wood.Effect", "Drop rate mod min", 0f,
                "% increase to wood drops at level 0");
            PercentToMultiplier(ref WoodcuttingDropModMin);
            WoodcuttingDropModMax = cfg.Bind("Wood.Effect", "Drop rate mod max", 100f,
                "% increase to wood drops at level 100");
            PercentToMultiplier(ref WoodcuttingDropModMax);
            MiningPickDamageModMin = cfg.Bind("Mining.Effect", "Pick Damage Bonus Min", 0f,
                "% increase to pick damage at level 0");
            PercentToMultiplier(ref MiningPickDamageModMin);
            MiningPickDamageModMax = cfg.Bind("Mining.Effect", "Pick Damage Bonus Max", 0f,
                "% increase to pick damage at level 100");
            PercentToMultiplier(ref MiningPickDamageModMax);
            MiningStaminaRebateMin = cfg.Bind("Mining.Effect", "Stamina Rebate Min", 0f,
                "Flat stamina rebate on each hit of a rock at level 0");
            MiningStaminaRebateMax = cfg.Bind("Mining.Effect", "Stamina Rebate Min", 0f,
                "Flat stamina rebate on each hit of a rock at level 100");
            MiningDropModMin = cfg.Bind("Mining.Effect", "Drop rate mod min", 0f,
                "% increase to ore drops at level 0");
            PercentToMultiplier(ref MiningDropModMin);
            MiningDropModMax = cfg.Bind("Mining.Effect", "Drop rate mod max", 100f,
                "% increase to ore drops at level 100");
            PercentToMultiplier(ref MiningDropModMax);

            DropNewItemThreshold = cfg.Bind("Drops", "Drop New Item Threshold", 50f,
                "% of 1 item needed to generate before you round up to a full item.");
            PercentToMultiplier(ref DropNewItemThreshold);

        }


        private static void PercentToMultiplier(ref ConfigEntry<float> config, bool redux = false)
        {
            config.Value = config.Value / 100;
            if (redux)
                config.Value = 1 - config.Value;
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
        public static float GetWoodDropRate(float skillFactor)
        {
            return Mathf.Lerp(WoodcuttingDropModMin.Value, WoodcuttingDropModMax.Value, skillFactor);
        }
        public static float GetMiningDropRate(float skillFactor)
        {
            return Mathf.Lerp(MiningDropModMin.Value,MiningDropModMax.Value, skillFactor);
        }
        public static float GetBowDropRate(float skillFactor)
        {
            return Mathf.Lerp(BowDropModMin.Value,BowDropModMax.Value, skillFactor);
        }

        /*
         * template
        public static float Get(float skillFactor)
        {
            return Mathf.Lerp(.Value,.Value, skillFactor);
        }
         */

    }
}
