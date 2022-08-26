using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Configuration;
using UnityEngine;

namespace kingskills
{
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
        public static ConfigEntry<float> BlockHealthPerLevel;
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
        //public static ConfigEntry<float> BowDrawSpeedModMin;
        //public static ConfigEntry<float> BowDrawSpeedModMax;
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
        public static ConfigEntry<float> SneakEXPPerDangerMod;
        public static ConfigEntry<float> SneakStaminaDrainMin;
        public static ConfigEntry<float> SneakStaminaDrainMax;
        public static ConfigEntry<float> SneakSpeedModMin;
        public static ConfigEntry<float> SneakSpeedModMax;
        public static ConfigEntry<float> SneakBrightestMin;
        public static ConfigEntry<float> SneakBrightestMax;
        public static ConfigEntry<float> SneakDarkestMin;
        public static ConfigEntry<float> SneakDarkestMax;

        //Constants for use that aren't configurable
        public static Dictionary<string, float> WoodcuttingDropTable = new Dictionary<string, float>();
        public static Dictionary<string, float> MiningDropTable = new Dictionary<string, float>();
        public static Dictionary<string, float> BowDropTable = new Dictionary<string, float>();

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
        public const float BaseBowDrawSpeedMin = 0f;
        public const float BaseBowDrawSpeedMax = .8f;
        public const float BaseDodgeStaminaUsage = 10f;

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
            BlockHealthPerLevel= cfg.Bind("Block.Effect", "Block Health", 1.2f,
                "flat increase to max health per level of block");
            
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
            RunAbsoluteWeightFactor = cfg.Bind("Run.Experience.AbsoluteWeight", "Factor", .86f,
                "Factor to define the slope of the absolute weight curve");
            RunAbsoluteWeightExpMod = cfg.Bind("Run.Experience.AbsoluteWeight", "Mod", 1000f, 
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
            JumpForwardForceModMin = cfg.Bind("Jump.Effect", "Horizontal Force Bonus Min", 0f,
                "% extra horizontal jump force at level 0");
            JumpForwardForceModMax = cfg.Bind("Jump.Effect", "Horizontal Force Bonus Max", 150f,
                "% extra horizontal jump force at level 100");
            JumpStaminaReduxMin = cfg.Bind("Jump.Effect", "Stamina Cost Reduction Min", 0f,
                "% less stamina cost to jump at level 0");
            JumpStaminaReduxMax = cfg.Bind("Jump.Effect", "Stamina Cost Reduction Max", 60f,
                "% less stamina cost to jump at level 100");
            //May not actually be used or be different from jump force
            JumpTiredReduxMin = cfg.Bind("Jump.Effect", "Jump Tired Stamina Reduction Min", 0f,
                "% added to the base game's tired factor, which reduces your jump force when out of stamina, at level 0");
            JumpTiredReduxMax = cfg.Bind("Jump.Effect", "Jump Tired Stamina Reduction Max", 20f,
                "% added to the base game's tired factor, which reduces your jump force when out of stamina, at level 100");

            //Unorganized


            AxeDamageModMin = cfg.Bind("Axe.Effect", "Damage Min", 0f, 
                "% extra damage done with axes at level 0");
            AxeDamageModMax = cfg.Bind("Axe.Effect", "Damage Max", 400f,
                "% extra damage done with axes at level 100");
            AxeStaminaReduxMin = cfg.Bind("Axe.Effect", "Stamina Reduction Min", 0f,
                "% less stamina usage for axes at level 0");
            AxeStaminaReduxMax = cfg.Bind("Axe.Effect", "Stamina Reduction Max", 60f,
                "% less stamina usage for axes at level 100");
            AxeStaminaPerLevel = cfg.Bind("Axe.Effect", "Base Stamina Gain per Level", 1.2f,
                "Flat amount of base stamina gained per level in axe");
            AxeChopDamageModMin = cfg.Bind("Axe.Effect", "Woodcutting Damage Min", 0f,
                "% extra woodcutting damage done at level 0");
            AxeChopDamageModMax = cfg.Bind("Axe.Effect", "Woodcutting Damage Max", 100f,
                "% extra woodcutting damage done at level 100");
            AxeCarryCapacityMin = cfg.Bind("Axe.Effect", "Carry Capacity Bonus Min", 0f, 
                "Flat extra carrying capacity at level 0");
            AxeCarryCapacityMax = cfg.Bind("Axe.Effect", "Carry Capacity Bonus Max", 250f, 
                "Flat extra carrying capacity at level 100");
            BowDamageModMin = cfg.Bind("Bow.Effect", "Damage Min", 0f,
                "% extra damage done with bows at level 0");
            BowDamageModMax = cfg.Bind("Bow.Effect", "Damage Max", 300f,
                "% extra damage done with bows at level 100");
            BowStaminaReduxMin = cfg.Bind("Bow.Effect", "Stamina Reduction Min", 0f,
                "% less stamina usage for bows at level 0");
            BowStaminaReduxMax = cfg.Bind("Bow.Effect", "Stamina Reduction Max", 70f,
                "% less stamina usage for bows at level 100");
            BowVelocityModMin = cfg.Bind("Bow.Effect", "Velocity Bonus Min", 0f, 
                "% extra velocity to fired arrows at level 0");
            BowVelocityModMax = cfg.Bind("Bow.Effect", "Velocity Bonus Max", 200f, 
                "% extra velocity to fired arrows at level 100");
            //BowDrawSpeedModMin = cfg.Bind("Bow.Effect", "Draw Speed Min", 0f, 
            //    "% extra bow draw speed at level 0");
            //BowDrawSpeedModMax = cfg.Bind("Bow.Effect", "Draw Speed Max", 0f, 
            //    "% extra bow draw speed at level 100");
            BowDropModMin = cfg.Bind("Bow.Effect", "Drop rate mod min", 0f,
                "% to increase creature drops at level 0");
            BowDropModMax = cfg.Bind("Bow.Effect", "Drop rate mod max", 100f,
                "% to increase creature drops at level 100");
            ClubDamageModMin = cfg.Bind("Club.Effect", "Damage Min", 0f,
                "% extra damage done with clubs at level 0");
            ClubDamageModMax = cfg.Bind("Club.Effect", "Damage Max", 400f,
                "% extra damage done with clubs at level 100");
            ClubStaminaReduxMin = cfg.Bind("Club.Effect", "Stamina Reduction Min", 0f,
                "% less stamina usage for clubs at level 0");
            ClubStaminaReduxMax = cfg.Bind("Club.Effect", "Stamina Reduction Max", 60f,
                "% less stamina usage for clubs at level 100");
            ClubBluntModMin = cfg.Bind("Club.Effect", "Generic Blunt Bonus Min", 0f, 
                "% extra blunt damage to ALL weapons at level 0");
            ClubBluntModMax = cfg.Bind("Club.Effect", "Generic Blunt Bonus Max", 100f,
                "% extra blunt damage to ALL weapons at level 100");
            ClubKnockbackModMin = cfg.Bind("Club.Effect", "Generic Knockback Bonus Min", 0f,
                "% extra knockback to ALL weapons at level 0");
            ClubKnockbackModMax = cfg.Bind("Club.Effect", "Generic Knockback Bonus Max", 200f,
                "% extra knockback to ALL weapons at level 100");
            ClubStaggerModMin = cfg.Bind("Club.Effect", "Generic Stagger Bonus Min", 0f, 
                "% extra stagger damage to ALL ATTACKS at level 0");
            ClubStaggerModMax = cfg.Bind("Club.Effect", "Generic Stagger Bonus Max", 150f,
                "% extra stagger damage to ALL ATTACKS at level 100");
            FistDamageModMin = cfg.Bind("Fist.Effect", "Damage Min", 0f,
                "% extra damage done with bare fists at level 0");
            FistDamageModMax = cfg.Bind("Fist.Effect", "Damage Max", 400f,
                "% extra damage done with bare fists at level 100");
            FistStaminaReduxMin = cfg.Bind("Fist.Effect", "Stamina Reduction Min", 0f,
                "% less stamina usage for fists at level 0");
            FistStaminaReduxMax = cfg.Bind("Fist.Effect", "Stamina Reduction Max", 80f,
                "% less stamina usage for fists at level 100");
            FistDamageFlatMin = cfg.Bind("Fist.Effect", "Flat Damage Min", -5f, 
                "Flat extra damage at level 0");
            FistDamageFlatMax = cfg.Bind("Fist.Effect", "Flat Damage Max", 50f, 
                "Flat extra damage at level 100");
            FistBlockArmorMin = cfg.Bind("Fist.Effect", "Unarmed Block Armor Min", -5f, 
                "Flat extra unarmed block armor at level 0");
            FistBlockArmorMax = cfg.Bind("Fist.Effect", "Unarmed Block Armor Max", 40f, 
                "Flat extra unarmed block armor at level 100");
            FistMovespeedModMin = cfg.Bind("Fist.Effect", "Movespeed Bonus Min", 0f, 
                "% movespeed increase at level 0");
            FistMovespeedModMax = cfg.Bind("Fist.Effect", "Movespeed Bonus Max", 70f,
                "% movespeed increase at level 100");
            KnifeDamageModMin = cfg.Bind("Knife.Effect", "Damage Min", 0f, 
                "% extra damage done with knives at level 0");
            KnifeDamageModMax = cfg.Bind("Knife.Effect", "Damage Max", 400f,
                "% extra damage done with knives at level 100");
            KnifeStaminaReduxMin = cfg.Bind("Knife.Effect", "Stamina Reduction Min", 0f,
                "% less stamina usage for knives at level 0");
            KnifeStaminaReduxMax = cfg.Bind("Knife.Effect", "Stamina Reduction Max", 50f,
                "% less stamina usage for knives at level 100");
            KnifeBackstabModMin = cfg.Bind("Knife.Effect", "Backstab Bonus Damage Min", 0f,
                "% extra sneak attack damage with ALL weapons at level 0");
            KnifeBackstabModMax = cfg.Bind("Knife.Effect", "Backstab Bonus Damage Max", 200f,
                "% extra sneak attack damage with ALL weapons at level 100");
            KnifeMovespeedModMin = cfg.Bind("Knife.Effect", "Movementspeed Bonus Min", 0f,
                "% movespeed increase at level 0");
            KnifeMovespeedModMax = cfg.Bind("Knife.Effect", "Movementspeed Bonus Max", 100f,
                "% movespeed increase at level 100");
            KnifePierceModMin = cfg.Bind("Knife.Effect", "Generic Pierce Damage Bonus Min", 0f,
                "% extra pierce damage with ALL weapons at level 0");
            KnifePierceModMax = cfg.Bind("Knife.Effect", "Generic Pierce Damage Bonus Max", 100f,
                "% extra pierce damage with ALL weapons at level 0");
            PolearmDamageModMin = cfg.Bind("Polearm.Effect", "Damage Min", 0f,
                "% extra damage done with polearms at level 0");
            PolearmDamageModMax = cfg.Bind("Polearm.Effect", "Damage Max", 400f,
                "% extra damage done with polearms at level 100");
            PolearmStaminaReduxMin = cfg.Bind("Polearm.Effect", "Stamina Reduction Min", 0f,
                "% less stamina usage for polearms at level 0");
            PolearmStaminaReduxMax = cfg.Bind("Polearm.Effect", "Stamina Reduction Max", 70f,
                "% less stamina usage for polearms at level 100");
            PolearmRangeMin = cfg.Bind("Polearm.Effect", "Generic Range Bonus Min", 0f,
                "Added units of range to all weapon attacks at level 0");
            PolearmRangeMax = cfg.Bind("Polearm.Effect", "Generic Range Bonus Max", 20f,
                "Added units of range to all weapon attacks at level 100");
            PolearmArmorMin = cfg.Bind("Polearm.Effect", "Flat Armor Bonus Min", 0f,
                "Flat armor added to character at level 0");
            PolearmArmorMax = cfg.Bind("Polearm.Effect", "Flat Armor Bonus Max", 25f,
                "Flat armor added to character at level 100");
            PolearmBlockMin = cfg.Bind("Polearm.Effect", "Block Armor Min", 0f,
                "Flat block armor added to polearms at level 0");
            PolearmBlockMax = cfg.Bind("Polearm.Effect", "Block Armor Max", 50f,
                "Flat block armor added to polearms at level 100");
            SpearDamageModMin = cfg.Bind("Spear.Effect", "Damage Min", 0f,
                "% extra damage done with spears at level 0");
            SpearDamageModMax = cfg.Bind("Spear.Effect", "Damage Max", 400f,
                "% extra damage done with spears at level 100");
            SpearStaminaReduxMin = cfg.Bind("Spear.Effect", "Stamina Reduction Min", 0f,
                "% less stamina usage for spears at level 0");
            SpearStaminaReduxMax = cfg.Bind("Spear.Effect", "Stamina Reduction Max", 70f,
                "% less stamina usage for spears at level 100");
            SpearVelocityModMin = cfg.Bind("Spear.Effect", "Thrown Velocity Min", 0f,
                "% extra velocity on thrown weapons at level 0");
            SpearVelocityModMax = cfg.Bind("Spear.Effect", "Thrown Velocity Max", 300f,
                "% extra velocity on thrown weapons at level 100");
            SpearProjectileDamageModMin = cfg.Bind("Spear.Effect", "Thrown Damage Min", 0f,
                "% extra damage done with thrown weapons at level 0");
            SpearProjectileDamageModMax = cfg.Bind("Spear.Effect", "Thrown Damage Max", 200f,
                "% extra damage done with thrown weapons at level 100");
            SpearBlockArmorMin = cfg.Bind("Spear.Effect", "Generic Block Armor Min", 0f, 
                "Flat block armor always applied at level 0");
            SpearBlockArmorMax = cfg.Bind("Spear.Effect", "Generic Block Armor Max", 25f,
                "Flat block armor always applied at level 100");
            SwordDamageModMin = cfg.Bind("Sword.Effect", "Damage Min", 0f,
                "% extra damage done with swords at level 0");
            SwordDamageModMax = cfg.Bind("Sword.Effect", "Damage Max", 400f,
                "% extra damage done with swords at level 100");
            SwordStaminaReduxMin = cfg.Bind("Sword.Effect", "Stamina Reduction Min", 0f,
                "% less stamina usage for swords at level 0");
            SwordStaminaReduxMax = cfg.Bind("Sword.Effect", "Stamina Reduction Max", 60f,
                "% less stamina usage for swords at level 100");
            SwordParryModMin = cfg.Bind("Sword.Effect", "Generic Parry Bonus Min", 0f,
                "% extra parry bonus for ALL weapons at level 0"); 
            SwordParryModMax = cfg.Bind("Sword.Effect", "Generic Parry Bonus Max", 150f,
                "% extra parry bonus for ALL weapons at level 100");
            SwordSlashModMin = cfg.Bind("Sword.Effect", "Generic Slash Damage Mod Min", 0f, 
                "% extra slash damage for ALL weapons at level 0");
            SwordSlashModMax = cfg.Bind("Sword.Effect", "Generic Slash Damage Mod Max", 100f,
                "% extra slash damage for ALL weapons at level 100"); 
            SwordDodgeStaminaReduxMin = cfg.Bind("Sword.Effect", "Dodgeroll Stamina Reduction Min", 0f, 
                "% less stamina cost to dodge roll at level 0");
            SwordDodgeStaminaReduxMax = cfg.Bind("Sword.Effect", "Dodgeroll Stamina Reduction Max", 40f,
                "% less stamina cost to dodge roll at level 0");
            

            WoodcuttingChopDamageModMin = cfg.Bind("Wood.Effect", "Chop Damage Bonus Min", 0f,
                "% increase to chop damage at level 0");
            WoodcuttingChopDamageModMax = cfg.Bind("Wood.Effect", "Chop Damage Bonus Max", 300f,
                "% increase to chop damage at level 100");
            WoodcuttingStaminaRebateMin = cfg.Bind("Wood.Effect", "Stamina Rebate Min", 0f,
                "Flat stamina rebate on each hit of a tree at level 0");
            WoodcuttingStaminaRebateMax = cfg.Bind("Wood.Effect", "Stamina Rebate Max", 9f,
                "Flat stamina rebate on each hit of a tree at level 100");
            WoodcuttingDropModMin = cfg.Bind("Wood.Effect", "Drop rate mod min", 0f,
                "% increase to wood drops at level 0");
            WoodcuttingDropModMax = cfg.Bind("Wood.Effect", "Drop rate mod max", 1000f,
                "% increase to wood drops at level 100");
            MiningPickDamageModMin = cfg.Bind("Mining.Effect", "Pick Damage Bonus Min", 0f,
                "% increase to pick damage at level 0");
            MiningPickDamageModMax = cfg.Bind("Mining.Effect", "Pick Damage Bonus Max", 300f,
                "% increase to pick damage at level 100");
            MiningStaminaRebateMin = cfg.Bind("Mining.Effect", "Stamina Rebate Min", 0f,
                "Flat stamina rebate on each hit of a rock at level 0");
            MiningStaminaRebateMax = cfg.Bind("Mining.Effect", "Stamina Rebate Max", 7f,
                "Flat stamina rebate on each hit of a rock at level 100");
            MiningDropModMin = cfg.Bind("Mining.Effect", "Drop rate mod Min", 0f,
                "% increase to ore drops at level 0");
            MiningDropModMax = cfg.Bind("Mining.Effect", "Drop rate mod Max", 100f,
                "% increase to ore drops at level 100");


        SneakEXPPerDangerMod = cfg.Bind("Sneak.Experience", "Experience Bonus per Danger", 120f, 
                "Determines how much each 'point of danger' is worth in sneak exp");
        SneakStaminaDrainMin = cfg.Bind("Sneak.Effect", "Stamina Drain Min", 10f, 
                "Amount of stamina drain per second while sneaking at level 0");
        SneakStaminaDrainMax = cfg.Bind("Sneak.Effect", "Stamina Drain Max", 2f,
                "Amount of stamina drain per second while sneaking at level 100");
        SneakSpeedModMin = cfg.Bind("Sneak.Effect", "Speed mod Min", 0f, 
                "% speed increase while sneaking at level 0");
        SneakSpeedModMax = cfg.Bind("Sneak.Effect", "Speed mod Max", 250f,
                "% speed increase while sneaking at level 100");
        SneakBrightestMin = cfg.Bind("Sneak.Effect", "Brightest Value Min", 20f, 
                "% decrease to enemy sight range while in the brightest possible area at level 0");
        SneakBrightestMax = cfg.Bind("Sneak.Effect", "Brightest Value Max", 60f,
                "% decrease to enemy sight range while in the brightest possible area at level 100");
        SneakDarkestMin = cfg.Bind("Sneak.Effect", "Darkest Value Min", 20f,
                "% decrease to enemy sight range while in the darkest possible area at level 0");
        SneakDarkestMax = cfg.Bind("Sneak.Effect", "Darkest Value Max", 60f,
                "% decrease to enemy sight range while in the darkest possible area at level 100");


        DropNewItemThreshold = cfg.Bind("Drops", "Drop New Item Threshold", 50f,
                "% of 1 item needed to generate before you round up to a full item.");

        }


        private static float PerToMult(ConfigEntry<float> config, bool redux = false)
        {
            float mult = config.Value;
            mult /= 100;
            if (redux)
                mult = 1 - mult;
            else
                mult++;
            //Jotunn.Logger.LogMessage($"Reading out {config.Definition.Key} as {mult}");
            return mult;
        }

        private static float PerToMod(ConfigEntry<float> config, bool redux = false)
        {
            float mod = config.Value;
            mod /= 100;
            if (redux)
                mod = 1 - mod;

           // Jotunn.Logger.LogMessage($"Reading out {config.Definition.Key} as {mod}");
            return mod;
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
            //Jotunn.Logger.LogMessage($"{x} is being sin curved into {Mathf.Sin(Mathf.Lerp(0f, Mathf.PI / 2, x))}");
            return Mathf.Sin(Mathf.Lerp(0f, Mathf.PI / 2, x));
        }



        /*
         * 
         * Here be the Get functions
         * 
         */
        public static float GetBlockExpMod()
        {
            return PerToMult(BlockExpMod);
        }
        public static float GetBlockParryExpMod()
        {
            return PerToMult(BlockParryExpMod);
        }
        public static float GetBlockStaminaRedux(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(BlockStaminaReduxMin, true), 
                PerToMult(BlockStaminaReduxMax, true), skillFactor);
        }
        public static float GetBlockPowerFlat(float skillFactor)
        {
            return Mathf.Lerp(BlockFlatPowerMin.Value, 
                BlockFlatPowerMax.Value, skillFactor);
        }
        public static float GetBlockPowerMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(BlockPowerModMin), 
                PerToMult(BlockPowerModMax), skillFactor);
        }
        public static float GetFallDamageThreshold(float skillFactor)
        {
            return Mathf.Lerp(JumpFallDamageThresholdMin.Value, 
                JumpFallDamageThresholdMax.Value, skillFactor);
        }
        public static float GetFallDamageRedux(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(JumpFallDamageReduxMin, true), 
                PerToMult(JumpFallDamageReduxMax, true), skillFactor);
        }
        public static float GetRunSpeedMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(RunSpeedModMin), 
                PerToMult(RunSpeedModMax), skillFactor);
        }
        public static float GetEncumberanceCurve(float encumberancePercent)
        {
            return Mathf.Lerp(PerToMult(RunEncumberanceModMin, true),
                PerToMult(RunEncumberanceModMax, true), ShapeFactorSin(encumberancePercent));
            /*
            float runEncMin = PerToMult(RunEncumberanceModMin, true);
            float runEncMax = PerToMult(RunEncumberanceModMax, true);
            float result = Mathf.Lerp(runEncMin, runEncMax, ShapeFactorSin(encumberancePercent));
            Jotunn.Logger.LogMessage($"Generating the encumberance factor: encumberance percent is apparently {encumberancePercent}\n" +
                $"Min to max is read as {RunEncumberanceModMin.Value} - {RunEncumberanceModMax.Value}\n" +
                $"Using PerToMult, they are displayed as {runEncMin} - {runEncMax}\n" +
                $"running through my sin curve, I am returning {result}");
            return result;*/
        }
        public static float GetEncumberanceRedux(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(RunEncumberanceReduxMin, true),
                PerToMult(RunEncumberanceReduxMax, true), skillFactor);
            /*
            Jotunn.Logger.LogMessage($"Encumberance redux: Config values are {RunEncumberanceReduxMin} to {RunEncumberanceReduxMax}\n" +
                $"PerToMult reads those values as {PerToMult(RunEncumberanceReduxMin, true)} to {PerToMult(RunEncumberanceReduxMax, true)}\n" +
                $"because I'm given {skillFactor}, I'm going to put out {answer}");
            return answer;*/
        }
        public static float GetEquipmentRedux(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(RunEquipmentReduxMin, true), 
                PerToMult(RunEquipmentReduxMax, true), skillFactor);
        }
        public static float GetAbsoluteWeightPercent(float weight)
        {
            return Mathf.Clamp01((weight - RunAbsoluteWeightMinWeight.Value) / 
                (RunAbsoluteWeightMaxWeight.Value - RunAbsoluteWeightMinWeight.Value));
        }
        public static float GetAbsoluteWeightCurve(float weightPercent)
        {
            float minBonus = 1f;
            float maxBonus = PerToMult(RunAbsoluteWeightExpMod);
            return Mathf.Lerp(minBonus, maxBonus, 
                Mathf.Pow(weightPercent, RunAbsoluteWeightFactor.Value));
            /*
            Jotunn.Logger.LogMessage($"Absolute weight exp mod: I was given {weightPercent} as weight percent\n" +
                $"The overall exp mod is {RunAbsoluteWeightExpMod.Value}, which I've converted to {PerToMult(RunAbsoluteWeightExpMod)}\n" +
                $"I just multiply that by my exponent curve, which is {weightPercent}^{RunAbsoluteWeightFactor.Value}," +
                $"giving me {Mathf.Pow(weightPercent+1f, RunAbsoluteWeightFactor.Value)}");*/
        }
        public static float GetRelativeWeightStage(float weightPercent)
        {
            float mult = 1f;
            //Jotunn.Logger.LogMessage($"Relative weight: I'm given a percent of {weightPercent}\n");

            if (weightPercent <= PerToMod(RunRelativeWeightLight))
                mult = PerToMult(RunRelativeWeightLightMod);

            else if (weightPercent <= PerToMod(RunRelativeWeightMed))
                mult = PerToMult(RunRelativeWeightMedMod);

            else if (weightPercent <= PerToMod(RunRelativeWeightHighMed))
                mult = PerToMult(RunRelativeWeightHighMedMod);

            else if (weightPercent <= PerToMod(RunRelativeWeightHeavy))
                mult = PerToMult(RunRelativeWeightHeavyMod);

            else if (weightPercent <= PerToMod(RunRelativeWeightFull))
                mult = PerToMult(RunRelativeWeightFullMod);

            else
                mult = PerToMult(RunRelativeWeightOverMod);

            //Jotunn.Logger.LogMessage($"Which means the multiplier I'm returning is {mult} times {PerToMult(RunRelativeWeightExpMod)}\n");

            return mult * PerToMult(RunRelativeWeightExpMod);
        }
        public static float GetRunStaminaRedux(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(RunStaminaReduxMin, true), 
                PerToMult(RunStaminaReduxMax, true), skillFactor);
        }
        public static float GetRunEXPSpeedMod()
        {
            return PerToMult(RunEXPSpeedMod);
        }
        public static float GetSwimSpeedMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(SwimSpeedModMin), 
                PerToMult(SwimSpeedModMax), skillFactor);
        }
        public static float GetSwimAccelMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(SwimAccelModMin), 
                PerToMult(SwimAccelModMax), skillFactor);
        }
        public static float GetSwimTurnMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(SwimTurnModMin), 
                PerToMult(SwimTurnModMax), skillFactor);
        }
        public static float GetJumpForceMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(JumpForceModMin), 
                PerToMult(JumpForceModMax), skillFactor);
        }
        public static float GetJumpForwardForceMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(JumpForwardForceModMin), 
                PerToMult(JumpForwardForceModMax), skillFactor);
        }
        public static float GetJumpStaminaRedux(float skillFactor)
        {
            /*
            Jotunn.Logger.LogMessage($"Jump stamina reduction. Reading level as" +
                $"{skillFactor}. The two possible clamps are {JumpStaminaReduxMin.Value}" +
                $"and {JumpStaminaReduxMax.Value}. Using PerToMult on both gives me" +
                $"{PerToMult(JumpStaminaReduxMin, true)} to {PerToMult(JumpStaminaReduxMax, true)}." +
                $"Based on that, my LERP is returning this number as " +
                $"{Mathf.Lerp(PerToMult(JumpStaminaReduxMin, true),PerToMult(JumpStaminaReduxMax, true), skillFactor)}");
            */
            return Mathf.Lerp(PerToMult(JumpStaminaReduxMin, true), 
                PerToMult(JumpStaminaReduxMax, true), skillFactor);
        }
        public static float GetJumpTiredRedux(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(JumpTiredReduxMin, true), 
                PerToMult(JumpTiredReduxMax, true), skillFactor);
        }
        public static float GetWeaponDamageToExperience(float damage)
        {
            return PerToMult(WeaponEXPStrikeDamageMod) * 
                Mathf.Pow(damage, WeaponEXPStrikeDamageFactor.Value);
        }
        public static float GetToolDamageToExperience(float damage)
        {
            return PerToMult(ToolEXPStrikeDamageMod) * 
                Mathf.Pow(damage, ToolEXPStrikeDamageFactor.Value);
        }
        public static float GetSwimStaminaPerSec(float skillFactor)
        {
            return Mathf.Lerp(SwimStaminaPerSecMin.Value, SwimStaminaPerSecMax.Value, skillFactor);
        }
        public static float GetAxeDamageMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(AxeDamageModMin), 
                PerToMult(AxeDamageModMax), skillFactor);
        }
        public static float GetAxeStaminaRedux(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(AxeStaminaReduxMin, true), 
                PerToMult(AxeStaminaReduxMax, true), skillFactor);
        }
        public static float GetAxeChopDamageMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(AxeChopDamageModMin), 
                PerToMult(AxeChopDamageModMax), skillFactor);
        }
        public static float GetAxeStamina(float skillFactor)
        { 
            return AxeStaminaPerLevel.Value * skillFactor * 100;
        }
        public static float GetAxeCarryCapacity(float skillFactor)
        {
            return Mathf.Lerp(AxeCarryCapacityMin.Value, AxeCarryCapacityMax.Value, skillFactor);
        }
        public static float GetBowDamageMod(float skillFactor)
        { 
            return Mathf.Lerp(PerToMult(BowDamageModMin), 
                PerToMult(BowDamageModMax), skillFactor);
        }
        public static float GetBowStaminaRedux(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(BowStaminaReduxMin, true), 
                PerToMult(BowStaminaReduxMax, true), skillFactor);
        }
        public static float GetBowVelocityMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(BowVelocityModMin), 
                PerToMult(BowVelocityModMax), skillFactor);
        }
        public static float GetBowDrawSpeedMod(float skillFactor)
        {
            return Mathf.Lerp(BaseBowDrawSpeedMin, 
                BaseBowDrawSpeedMax, skillFactor);
        }
        public static float GetBowDropRate(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(BowDropModMin), 
                PerToMult(BowDropModMax), skillFactor);
        }
        public static float GetClubDamageMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(ClubDamageModMin), 
                PerToMult(ClubDamageModMax), skillFactor);
        }
        public static float GetClubStaminaRedux(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(ClubStaminaReduxMin, true), 
                PerToMult(ClubStaminaReduxMax, true), skillFactor);
        }
        public static float GetClubBluntMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(ClubBluntModMin), 
                PerToMult(ClubBluntModMax), skillFactor);
        }
        public static float GetClubKnockbackMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(ClubKnockbackModMin), 
                PerToMult(ClubKnockbackModMax), skillFactor);
        }
        public static float GetClubStaggerMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(ClubStaggerModMin), 
                PerToMult(ClubStaggerModMax), skillFactor);
        }
        public static float GetFistDamageMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(FistDamageModMin), 
                PerToMult(FistDamageModMax), skillFactor);
        }
        public static float GetFistStaminaRedux(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(FistStaminaReduxMin, true), 
                PerToMult(FistStaminaReduxMax, true), skillFactor);
        }
        public static float GetFistDamageFlat(float skillFactor)
        {
            return Mathf.Lerp(FistDamageFlatMin.Value, FistDamageFlatMax.Value, skillFactor);
        }
        public static float GetFistBlockArmor(float skillFactor)
        {
            return Mathf.Lerp(FistBlockArmorMin.Value, FistBlockArmorMax.Value, skillFactor);
        }
        public static float GetFistMovespeedMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(FistMovespeedModMin), 
                PerToMult(FistMovespeedModMax), skillFactor);
        }
        public static float GetKnifeDamageMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(KnifeDamageModMin),
                PerToMult(KnifeDamageModMax), skillFactor);
        }
        public static float GetKnifeStaminaRedux(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(KnifeStaminaReduxMin, true), 
                PerToMult(KnifeStaminaReduxMax, true), skillFactor);
        }
        public static float GetKnifeBackstabMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(KnifeBackstabModMin), 
                PerToMult(KnifeBackstabModMax), skillFactor);
        }
        public static float GetKnifeMovespeedMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(KnifeMovespeedModMin), 
                PerToMult(KnifeMovespeedModMax), skillFactor);
        }
        public static float GetKnifePierceMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(KnifePierceModMin), 
                PerToMult(KnifePierceModMax), skillFactor);
        }
        public static float GetPolearmDamageMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(PolearmDamageModMin), 
                PerToMult(PolearmDamageModMax), skillFactor);
        }
        public static float GetPolearmStaminaRedux(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(PolearmStaminaReduxMin, true), 
                PerToMult(PolearmStaminaReduxMax, true), skillFactor);
        }
        public static float GetPolearmRange(float skillFactor)
        {
            return Mathf.Lerp(PolearmRangeMin.Value, PolearmRangeMax.Value, skillFactor);
        }
        public static float GetPolearmArmor(float skillFactor)
        {
            return Mathf.Lerp(PolearmArmorMin.Value, PolearmArmorMax.Value, skillFactor);
        }
        public static float GetPolearmBlock(float skillFactor)
        {
            return Mathf.Lerp(PolearmBlockMin.Value, PolearmBlockMax.Value, skillFactor);
        }
        public static float GetSpearDamageMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(SpearDamageModMin), 
                PerToMult(SpearDamageModMax), skillFactor);
        }
        public static float GetSpearStaminaRedux(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(SpearStaminaReduxMin, true), 
                PerToMult(SpearStaminaReduxMax, true), skillFactor);
        }
        public static float GetSpearVelocityMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(SpearVelocityModMin), 
                PerToMult(SpearVelocityModMax), skillFactor);
        }
        public static float GetSpearProjectileDamageMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(SpearProjectileDamageModMin), 
                PerToMult(SpearProjectileDamageModMax), skillFactor);
        }
        public static float GetSpearBlockArmor(float skillFactor)
        {
            return Mathf.Lerp(SpearBlockArmorMin.Value, SpearBlockArmorMax.Value, skillFactor);
        }
        public static float GetSwordDamageMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(SwordDamageModMin), 
                PerToMult(SwordDamageModMax), skillFactor);
        }
        public static float GetSwordStaminaRedux(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(SwordDodgeStaminaReduxMin, true), 
                PerToMult(SwordDodgeStaminaReduxMax, true), skillFactor);
        }
        public static float GetSwordParryMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(SwordParryModMin), 
                PerToMult(SwordParryModMax), skillFactor);
        }
        public static float GetSwordSlashMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(SwordSlashModMin), 
                PerToMult(SwordSlashModMax), skillFactor);
        }
        public static float GetSwordDodgeStaminaRedux(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(SwordDodgeStaminaReduxMin, true), 
                PerToMult(SwordDodgeStaminaReduxMax, true), skillFactor);
        }
        public static float GetWoodcuttingDamageMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(WoodcuttingChopDamageModMin), 
                PerToMult(WoodcuttingChopDamageModMax), skillFactor);
        }
        public static float GetWoodDropRate(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(WoodcuttingDropModMin), 
                PerToMult(WoodcuttingDropModMax), skillFactor);
        }
        public static float GetWoodcuttingStaminaRebate(float skillFactor)
        {
            return Mathf.Lerp(WoodcuttingStaminaRebateMin.Value, 
                WoodcuttingStaminaRebateMax.Value, skillFactor);
        }
        public static float GetMiningDamageMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(MiningPickDamageModMin), 
                PerToMult(MiningPickDamageModMax), skillFactor);
        }
        public static float GetMiningDropRate(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(MiningDropModMin), 
                PerToMult(MiningDropModMax), skillFactor);
        }
        public static float GetMiningStaminaRebate(float skillFactor)
        {
            return Mathf.Lerp(MiningStaminaRebateMin.Value, MiningStaminaRebateMax.Value, skillFactor);
        }
        public static float GetDropItemThreshold()
        {
            return PerToMod(DropNewItemThreshold);
        }
        public static float GetSneakEXPPerDangerMod()
        {
            return PerToMult(SneakEXPPerDangerMod);
        }
        public static float GetSneakStaminaDrain(float skillFactor)
        {
            return Mathf.Lerp(SneakStaminaDrainMin.Value,SneakStaminaDrainMax.Value, skillFactor);
        }
        public static float GetSneakSpeedMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(SneakSpeedModMin),
                PerToMult(SneakSpeedModMax), skillFactor);
        }
        public static float GetSneakBrightest(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(SneakBrightestMin), 
                PerToMult(SneakBrightestMax), skillFactor);
        }
        public static float GetSneakDarkest(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(SneakDarkestMin),
                PerToMult(SneakDarkestMax), skillFactor);
        }

        /*
         * template
        public static float Get(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(), 
                PerToMult(), skillFactor);
        }
         */

    }
}
