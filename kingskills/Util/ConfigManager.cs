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

        /*
         * Generalized language:
         * 
         * Per (or percent) refers to a number, typically between 1 and 100, that will
         * probably be modified later on. Used for config entries.
         * Ex: A per of 50% implies the final number will have a final value of 150%
         * 
         * Redux (or Reduction) refers to a percent that will be subtracted from 
         * the whole.
         * Ex: A redux of 50% implies the final number will have a final value of 50%
         * 
         * Mod (or modifier) is a version of a percentage that multiplies something to
         * change the entire number.
         * Ex: a mod of .5 would be multiplied in if you wanted to halve a total number.
         * 
         * Mult (or multiplier) is math-ready verison of a percent that allows you
         * to multiply the number. Used mostly in the actual code implementation.
         * Ex: A per of 50% would generate a mult of 1.5 for actual handling in the math
         * 
         * Fact (or factor) is a number (usually between 0 and 1) that will be used as an 
         * exponent for the defining of a curve.
         * Ex: A factor of .4 will define the slope of an exponential function
         * to make some skill curve interesting
         * 
         */

        public static ConfigEntry<float> WeaponXPSwing;
        public static ConfigEntry<float> WeaponXPHoldPerTick;
        public static ConfigEntry<float> WeaponXPHoldTickLength;
        public static ConfigEntry<float> WeaponXPHoldUnarmedPercent;
        public static ConfigEntry<float> WeaponXPStrikeDamagePercent;
        public static ConfigEntry<float> WeaponXPStrikeDamageFactor;
        public static ConfigEntry<float> WeaponXPStrikeDestructiblePercent;

        public static ConfigEntry<float> WeaponBXPUnarmedBlock;
        public static ConfigEntry<float> WeaponBXPClubStagger;
        public static ConfigEntry<float> WeaponBXPSwordStagger;
        public static ConfigEntry<float> WeaponBXPKnifeBackstab;
        public static ConfigEntry<float> WeaponBXPAxeRange;
        public static ConfigEntry<float> WeaponBXPAxeTreeAmount;
        public static ConfigEntry<float> WeaponBXPSpearThrown;
        public static ConfigEntry<float> WeaponBXPBowDistancePercent;
        public static ConfigEntry<float> WeaponBXPPolearmDamageMod;

        public static ConfigEntry<float> ToolBXPWoodStubReward;

        public static ConfigEntry<float> ToolXPStrikeDamagePercent;
        public static ConfigEntry<float> ToolXPStrikeDamageFactor;

        public static ConfigEntry<float> AxeDamagePercentMin;
        public static ConfigEntry<float> AxeDamagePercentMax;
        public static ConfigEntry<float> AxeStaminaReduxMin;
        public static ConfigEntry<float> AxeStaminaReduxMax;
        public static ConfigEntry<float> AxeStaminaPerLevel;
        public static ConfigEntry<float> AxeChopDamagePercentMin;
        public static ConfigEntry<float> AxeChopDamagePercentMax;
        public static ConfigEntry<float> AxeCarryCapacityMin;
        public static ConfigEntry<float> AxeCarryCapacityMax;

        public static ConfigEntry<float> BowDamagePercentMin;
        public static ConfigEntry<float> BowDamagePercentMax;
        public static ConfigEntry<float> BowStaminaReduxMin;
        public static ConfigEntry<float> BowStaminaReduxMax;
        public static ConfigEntry<float> BowVelocityPercentMin;
        public static ConfigEntry<float> BowVelocityPercentMax;
        public static ConfigEntry<float> BowDropPercentMin;
        public static ConfigEntry<float> BowDropPercentMax;

        public static ConfigEntry<float> BlockXPPercent;
        public static ConfigEntry<float> BlockXPParryPercent;
        public static ConfigEntry<float> BlockFlatPowerMin;
        public static ConfigEntry<float> BlockFlatPowerMax;
        public static ConfigEntry<float> BlockPowerPercentMin;
        public static ConfigEntry<float> BlockPowerPercentMax;
        public static ConfigEntry<float> BlockStaminaReduxMin;
        public static ConfigEntry<float> BlockStaminaReduxMax;
        public static ConfigEntry<float> BlockHealthPerLevel;

        public static ConfigEntry<float> ClubDamagePercentMin;
        public static ConfigEntry<float> ClubDamagePercentMax;
        public static ConfigEntry<float> ClubStaminaReduxMin;
        public static ConfigEntry<float> ClubStaminaReduxMax;
        public static ConfigEntry<float> ClubBluntPercentMin;
        public static ConfigEntry<float> ClubBluntPercentMax;
        public static ConfigEntry<float> ClubKnockbackPercentMin;
        public static ConfigEntry<float> ClubKnockbackPercentMax;
        public static ConfigEntry<float> ClubStaggerPercentMin;
        public static ConfigEntry<float> ClubStaggerPercentMax;

        public static ConfigEntry<float> FistDamagePercentMin;
        public static ConfigEntry<float> FistDamagePercentMax;
        public static ConfigEntry<float> FistStaminaReduxMin;
        public static ConfigEntry<float> FistStaminaReduxMax;
        public static ConfigEntry<float> FistDamageFlatMin;
        public static ConfigEntry<float> FistDamageFlatMax;
        public static ConfigEntry<float> FistBlockArmorMin;
        public static ConfigEntry<float> FistBlockArmorMax;
        public static ConfigEntry<float> FistMovespeedPercentMin;
        public static ConfigEntry<float> FistMovespeedPercentMax;

        public static ConfigEntry<float> JumpFallDamageThresholdMin;
        public static ConfigEntry<float> JumpFallDamageThresholdMax;
        public static ConfigEntry<float> JumpFallDamageReduxMin;
        public static ConfigEntry<float> JumpFallDamageReduxMax;
        public static ConfigEntry<float> JumpForcePercentMin;
        public static ConfigEntry<float> JumpForcePercentMax;
        public static ConfigEntry<float> JumpStaminaReduxMin;
        public static ConfigEntry<float> JumpStaminaReduxMax;
        public static ConfigEntry<float> JumpForwardForcePercentMin;
        public static ConfigEntry<float> JumpForwardForcePercentMax;
        public static ConfigEntry<float> JumpTiredModMin;
        public static ConfigEntry<float> JumpTiredModMax;
        public static ConfigEntry<float> JumpXPPercent;

        public static ConfigEntry<float> SwimXPSpeedPercent;
        public static ConfigEntry<float> SwimSpeedPercentMin;
        public static ConfigEntry<float> SwimSpeedPercentMax;
        public static ConfigEntry<float> SwimAccelPercentMin;
        public static ConfigEntry<float> SwimAccelPercentMax;
        public static ConfigEntry<float> SwimTurnPercentMin;
        public static ConfigEntry<float> SwimTurnPercentMax;
        public static ConfigEntry<float> SwimStaminaPerSecMin;
        public static ConfigEntry<float> SwimStaminaPerSecMax;

        public static ConfigEntry<float> RunAbsoluteWeightMinWeight;
        public static ConfigEntry<float> RunAbsoluteWeightMaxWeight;
        public static ConfigEntry<float> RunAbsoluteWeightFactor;
        public static ConfigEntry<float> RunXPAbsoluteWeightPercent;
        public static ConfigEntry<float> RunRelativeWeightLight;
        public static ConfigEntry<float> RunRelativeWeightLightPercent;
        public static ConfigEntry<float> RunRelativeWeightMed;
        public static ConfigEntry<float> RunRelativeWeightMedPercent;
        public static ConfigEntry<float> RunRelativeWeightHighMed;
        public static ConfigEntry<float> RunRelativeWeightHighMedPercent;
        public static ConfigEntry<float> RunRelativeWeightHeavy;
        public static ConfigEntry<float> RunRelativeWeightHeavyPercent;
        public static ConfigEntry<float> RunRelativeWeightFull;
        public static ConfigEntry<float> RunRelativeWeightFullPercent;
        public static ConfigEntry<float> RunRelativeWeightOverPercent;
        public static ConfigEntry<float> RunXPRelativeWeightPercent;
        public static ConfigEntry<float> RunXPSpeedPercent;
        public static ConfigEntry<float> RunSpeedPercentMin;
        public static ConfigEntry<float> RunSpeedPercentMax;
        public static ConfigEntry<float> RunEquipmentReduxMin;
        public static ConfigEntry<float> RunEquipmentReduxMax;
        public static ConfigEntry<float> RunEncumberancePercentMin;
        public static ConfigEntry<float> RunEncumberancePercentMax;
        public static ConfigEntry<float> RunEncumberanceReduxMin;
        public static ConfigEntry<float> RunEncumberanceReduxMax;
        public static ConfigEntry<float> RunStaminaReduxMin;
        public static ConfigEntry<float> RunStaminaReduxMax;
        public static ConfigEntry<float> RunStaminaPerLevel;

        public static ConfigEntry<float> KnifeDamagePercentMin;
        public static ConfigEntry<float> KnifeDamagePercentMax;
        public static ConfigEntry<float> KnifeStaminaReduxMin;
        public static ConfigEntry<float> KnifeStaminaReduxMax;
        public static ConfigEntry<float> KnifeBackstabPercentMin;
        public static ConfigEntry<float> KnifeBackstabPercentMax;
        public static ConfigEntry<float> KnifeMovespeedPercentMin;
        public static ConfigEntry<float> KnifeMovespeedPercentMax;
        public static ConfigEntry<float> KnifePiercePercentMin;
        public static ConfigEntry<float> KnifePiercePercentMax;

        public static ConfigEntry<float> MiningPickDamagePercentMin;
        public static ConfigEntry<float> MiningPickDamagePercentMax;
        public static ConfigEntry<float> MiningStaminaRebateMin;
        public static ConfigEntry<float> MiningStaminaRebateMax;
        public static ConfigEntry<float> MiningDropPercentMin;
        public static ConfigEntry<float> MiningDropPercentMax;
        public static ConfigEntry<float> MiningRegenHealthMin;
        public static ConfigEntry<float> MiningRegenHealthMax;
        public static ConfigEntry<float> MiningCarryCapacityMin;
        public static ConfigEntry<float> MiningCarryCapacityMax;

        public static ConfigEntry<float> PolearmDamagePercentMin;
        public static ConfigEntry<float> PolearmDamagePercentMax;
        public static ConfigEntry<float> PolearmStaminaReduxMin;
        public static ConfigEntry<float> PolearmStaminaReduxMax;
        public static ConfigEntry<float> PolearmRangeMax;
        public static ConfigEntry<float> PolearmRangeMin;
        public static ConfigEntry<float> PolearmArmorMax;
        public static ConfigEntry<float> PolearmArmorMin;
        public static ConfigEntry<float> PolearmBlockMax;
        public static ConfigEntry<float> PolearmBlockMin;

        public static ConfigEntry<float> SneakXPThreatPercent;
        public static ConfigEntry<float> SneakStaminaDrainMin;
        public static ConfigEntry<float> SneakStaminaDrainMax;
        public static ConfigEntry<float> SneakSpeedPercentMin;
        public static ConfigEntry<float> SneakSpeedPercentMax;

        public static ConfigEntry<float> SpearDamagePercentMin;
        public static ConfigEntry<float> SpearDamagePercentMax;
        public static ConfigEntry<float> SpearStaminaReduxMin;
        public static ConfigEntry<float> SpearStaminaReduxMax;
        public static ConfigEntry<float> SpearVelocityPercentMax;
        public static ConfigEntry<float> SpearVelocityPercentMin;
        public static ConfigEntry<float> SpearProjectilePercentMax;
        public static ConfigEntry<float> SpearProjectileDamagePercentMin;
        public static ConfigEntry<float> SpearBlockArmorMax;
        public static ConfigEntry<float> SpearBlockArmorMin;

        public static ConfigEntry<float> SwordDamagePercentMin;
        public static ConfigEntry<float> SwordDamagePercentMax;
        public static ConfigEntry<float> SwordStaminaReduxMin;
        public static ConfigEntry<float> SwordStaminaReduxMax;
        public static ConfigEntry<float> SwordParryPercentMin;
        public static ConfigEntry<float> SwordParryPercentMax;
        public static ConfigEntry<float> SwordSlashPercentMin;
        public static ConfigEntry<float> SwordSlashPercentMax;
        public static ConfigEntry<float> SwordDodgeStaminaReduxMin;
        public static ConfigEntry<float> SwordDodgeStaminaReduxMax;

        public static ConfigEntry<float> WoodcuttingChopDamagePercentMin;
        public static ConfigEntry<float> WoodcuttingChopDamagePercentMax;
        public static ConfigEntry<float> WoodcuttingStaminaRebateMin;
        public static ConfigEntry<float> WoodcuttingStaminaRebateMax;
        public static ConfigEntry<float> WoodcuttingDropPercentMin;
        public static ConfigEntry<float> WoodcuttingDropPercentMax;
        public static ConfigEntry<float> WoodcuttingRegenStaminaMin;
        public static ConfigEntry<float> WoodcuttingRegenStaminaMax;
        public static ConfigEntry<float> WoodcuttingCarryCapacityMin;
        public static ConfigEntry<float> WoodcuttingCarryCapacityMax;

        public static ConfigEntry<bool> ActiveSkillAgriculture;
        public static ConfigEntry<bool> ActiveSkillAxe;
        public static ConfigEntry<bool> ActiveSkillBlock;
        public static ConfigEntry<bool> ActiveSkillBow;
        public static ConfigEntry<bool> ActiveSkillBuilding;
        public static ConfigEntry<bool> ActiveSkillClub;
        public static ConfigEntry<bool> ActiveSkillCooking;
        public static ConfigEntry<bool> ActiveSkillFist;
        public static ConfigEntry<bool> ActiveSkillJump;
        public static ConfigEntry<bool> ActiveSkillKnife;
        public static ConfigEntry<bool> ActiveSkillMine;
        public static ConfigEntry<bool> ActiveSkillPolearm;
        public static ConfigEntry<bool> ActiveSkillRun;
        public static ConfigEntry<bool> ActiveSkillSailing;
        public static ConfigEntry<bool> ActiveSkillSpear;
        public static ConfigEntry<bool> ActiveSkillSneak;
        public static ConfigEntry<bool> ActiveSkillSwim;
        public static ConfigEntry<bool> ActiveSkillSword;
        public static ConfigEntry<bool> ActiveSkillWood;

        public static ConfigEntry<float> DropNewItemThreshold;
        public static ConfigEntry<float> MaxSkillLevel;
        public static ConfigEntry<float> DisplayExperienceThreshold;

        public static ConfigEntry<bool> PerkExplorationOn;
        public static ConfigEntry<float> PerkOneLVLThreshold;
        public static ConfigEntry<float> PerkTwoLVLThreshold;
        public static ConfigEntry<float> PerkThreeLVLThreshold;
        public static ConfigEntry<float> PerkFourLVLThreshold;

        //Variables and constants for use that aren't configurable
        public static Dictionary<Skills.SkillType, bool> SkillActive = new Dictionary<Skills.SkillType, bool>();
        public static Dictionary<string, float> WoodcuttingDropTable = new Dictionary<string, float>();
        public static Dictionary<string, float> MiningDropTable = new Dictionary<string, float>();
        public static Dictionary<string, float> BowDropTable = new Dictionary<string, float>();

        public static Color ColorBonusBlue;
        public static Color ColorAscendedGreen;
        public static Color ColorExperienceYellow;
        public static Color ColorTitle;
        public static Color ColorWhite;
        public static Color ColorKingSkills;

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
        public const float BaseCrouchSpeed = 2f;
        public const float BaseFoodHealTimer = 10;
        public const float BaseStaminaRegenTimer = 1;

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

            ColorBonusBlue = new Color(0.20f, 0.88f, 0.93f);
            ColorAscendedGreen = new Color(0.45f, 0.92f, 0.32f);
            ColorExperienceYellow = new Color(0.98f, 0.96f, 0.23f);
            ColorTitle = new Color(0.98f, 0.96f, 0.43f);
            ColorWhite = new Color(1f, 1f, 1f);
            ColorKingSkills = new Color(0.77f, 0.23f, 0.99f);

            /* Template
            x = cfg.Bind("g", "t", 0f, 
                "d");
             */



        //
            PerkExplorationOn = cfg.Bind("Perks", "Perk Exploration", true,
                    "Whether or not locked perks are hidden from view");
            PerkOneLVLThreshold = cfg.Bind("Perks", "First Threshold", .5f,
                    "mod of max level before you unlock the first set of perks");
            PerkTwoLVLThreshold = cfg.Bind("Perks", "Second Threshold", 1f,
                    "mod of max level before you unlock the second set of perks");
            PerkThreeLVLThreshold = cfg.Bind("Perks", "ThirdThreshold", 10f,
                    "mod of max level before you unlock the third set of perks - NOT IMPLEMENTED");
            PerkFourLVLThreshold = cfg.Bind("Perks", "FourthThreshold", 10f,
                    "mod of max level before you unlock the fourth set of perks - NOT IMPLEMENTED");


            //Overall Changes
            MaxSkillLevel = cfg.Bind("Generic", "Max Skill Level", 100f,
                    "This is the level that all king skills can go up to.");
            DisplayExperienceThreshold = cfg.Bind("Generic", "Experience Display Threshold", .2f,
                    "Threshold under which experience earned will not display as a message.");
            DropNewItemThreshold = cfg.Bind("Generic", "Drop New Item Threshold", 50f,
                    "% of 1 item needed to generate before you round up to a full item.");



            //Active skills
            ActiveSkillAgriculture = cfg.Bind("Generic.Active", "Agriculture", true,
                    "Whether or not to allow King's agriculture");
            ActiveSkillAxe = cfg.Bind("Generic.Active", "Axes", true,
                    "Whether or not to activate king's skills version of the axes skill");
            ActiveSkillBlock = cfg.Bind("Generic.Active", "Blocking", true,
                    "Whether or not to activate king's skills version of the blocking skill"); ;
            ActiveSkillBow = cfg.Bind("Generic.Active", "Bows", true,
                    "Whether or not to activate king's skills version of the bows skill"); ;
            ActiveSkillBuilding = cfg.Bind("Generic.Active", "Building", true,
                    "Whether or not to allow King's building");
            ActiveSkillClub = cfg.Bind("Generic.Active", "Clubs", true,
                    "Whether or not to activate king's skills version of the clubs skill"); ;
            ActiveSkillCooking = cfg.Bind("Generic.Active", "Cooking", true,
                    "Whether or not to allow King's cooking");
            ActiveSkillFist = cfg.Bind("Generic.Active", "Fists", true,
                    "Whether or not to activate king's skills version of the unarmed skill"); ;
            ActiveSkillJump = cfg.Bind("Generic.Active", "Jump", true,
                    "Whether or not to activate king's skills version of the jump skill"); ;
            ActiveSkillKnife = cfg.Bind("Generic.Active", "Knives", true,
                    "Whether or not to activate king's skills version of the knives skill"); ;
            ActiveSkillMine = cfg.Bind("Generic.Active", "Mining", true,
                    "Whether or not to activate king's skills version of the mining skill"); ;
            ActiveSkillPolearm = cfg.Bind("Generic.Active", "Polearms", true,
                    "Whether or not to activate king's skills version of the polearms skill"); ;
            ActiveSkillRun = cfg.Bind("Generic.Active", "Run", true,
                    "Whether or not to activate king's skills version of the run skill"); ;
            ActiveSkillSailing = cfg.Bind("Generic.Active", "Sailing", true,
                    "Whether or not to allow King's sailing");
            ActiveSkillSpear = cfg.Bind("Generic.Active", "Spear", true,
                    "Whether or not to activate king's skills version of the spear skill"); ;
            ActiveSkillSneak = cfg.Bind("Generic.Active", "Sneak", true,
                    "Whether or not to activate king's skills version of the sneak skill"); ;
            ActiveSkillSwim = cfg.Bind("Generic.Active", "Swim", true,
                    "Whether or not to activate king's skills version of the swim skill"); ;
            ActiveSkillSword = cfg.Bind("Generic.Active", "Swords", true,
                    "Whether or not to activate king's skills version of the swords skill"); ;
            ActiveSkillWood = cfg.Bind("Generic.Active", "Woodcutting", true,
                    "Whether or not to activate king's skills version of the woodcutting skill"); ;



            //Weapon Swing Experence
            WeaponXPSwing = cfg.Bind("Experience.Weapons.Swing", "XP Flat", .25f,
                "Flat experience to be gained on each swing, regardless of hit.");


            //Weapon Hold Experience
            WeaponXPHoldPerTick = cfg.Bind("Experience.Weapons.Hold", "XP/s", .04f,
                "Flat experience to be gained every second holding any weapon.");
            WeaponXPHoldTickLength = cfg.Bind("Experience.Weapons.Hold", "Timer", 1.0f,
                "Seconds between ticks of hold experience.");
            WeaponXPHoldUnarmedPercent = cfg.Bind("Experience.Weapons.Hold", "Unarmed", 25f,
                "% of normal hold experience to unarmed when holding nothing. Should be lower than 100% " +
                "to account for how often a regular player is holding nothing.");
            

            //Weapon Strike Experience
            WeaponXPStrikeDamagePercent = cfg.Bind("Experience.Weapons", "XP", 20f,
                "% modifier to overall experience gained from damage.");
            WeaponXPStrikeDamageFactor = cfg.Bind("Experience.Weapons", "XP Factor", .24f,
                "Factor to define the slope of the damage to xp curve.");
            WeaponXPStrikeDestructiblePercent = cfg.Bind("Experience.Weapons", "Destructible", 15f,
                "% of experience gained when hit target is non living. Should be lover than 100%");
            

            //Weapon Bonus experience (BXP)
            WeaponBXPUnarmedBlock = cfg.Bind("Weapon.BonusExperience", "Unarmed Block", 3f,
                "Flat BXP gained every time you perform an unarmed block");
            WeaponBXPClubStagger = cfg.Bind("Weapon.BonusExperience", "Club Stagger", 5f,
                "Flat BXP gained every time you stagger an enemy with clubs");
            WeaponBXPSwordStagger = cfg.Bind("Weapon.BonusExperience", "Sword Parry Hit", 3f,
                "Flat BXP gained every time you hit a staggered enemy with a sword");
            WeaponBXPKnifeBackstab = cfg.Bind("Weapon.BonusExperience", "Knife Backstab", 10f,
                "Flat BXP gained every time you get a sneak attack using the knife.");
            WeaponBXPAxeTreeAmount = cfg.Bind("Weapon.BonusExperience", "Axe Log", 5f,
                "Flat BXP gained every time you break down a log");
            WeaponBXPAxeRange = cfg.Bind("Weapon.BonusExperience", "Axe Felling Range", 100f,
                "Distance to check for axe BXP gain");
            WeaponBXPSpearThrown = cfg.Bind("Weapon.BonusExperience", "Spear Throw", 10f,
                "Flat BXP gained every time you hit with a thrown spear");
            WeaponBXPBowDistancePercent = cfg.Bind("Weapon.BonusExperience", "Bow Distance Percent", 50f,
                "% of distance that becomes bow experience on hit.");
            WeaponBXPPolearmDamageMod = cfg.Bind("Weapon.BonusExperience", "Polearm Damage Mod", .5f,
                "amount of bonus experience to get for each damage blocked by armor.");


        //Tool experience
        ToolXPStrikeDamagePercent = cfg.Bind("Experience.Tools", "Tool Damage", 25f,
                "% of damage done to resources that becomes experience for gathering skills " +
                "(Woodcutting, Mining)");
            ToolXPStrikeDamageFactor = cfg.Bind("Experience.Tools", "Tool Damage Factor", .24f,
                "Factor to define the slope of the damage to xp curve");
            ToolBXPWoodStubReward = cfg.Bind("Experience.Tools", "Woodcutting BXP for Stump", 20f,
                "The amount of experience you get for breaking a stump");


            //Block experience
            BlockXPPercent = cfg.Bind("Block.Experience", "XP", 15f,
                "% of damage blocked that turns into experience");
            BlockXPParryPercent = cfg.Bind("Block.Experience", "Parry", 200f,
                "% extra experience earned when parrying an attack");

            //Block effects
            BlockFlatPowerMin = cfg.Bind("Block.Effect", "Block Armor Flat Min", 0f,
                "This flat value is added to block armor at level 0");
            BlockFlatPowerMax = cfg.Bind("Block.Effect", "Block Armor Flat Max", 50f,
                "This flat value is added to block armor at level 100");
            BlockPowerPercentMin = cfg.Bind("Block.Effect", "Block Armor Min", -25f,
                "% change in total block armor at level 0");
            BlockPowerPercentMax = cfg.Bind("Block.Effect", "Block Armor Max", 100f,
                "% change in total block armor at level 100");
            BlockStaminaReduxMin = cfg.Bind("Block.Effect", "Stamina Reduction Min", -10f,
                "% less stamina to block at level 0");
            BlockStaminaReduxMax = cfg.Bind("Block.Effect", "Stamina Reduction Max", 50f,
                "% less stamina to block at level 100");
            BlockHealthPerLevel= cfg.Bind("Block.Effect", "Health", .5f,
                "flat increase to max health per level of block");


            //Jump Experience
            JumpXPPercent = cfg.Bind("Jump.Experience", "Jump Experience Mod", 53f,
                "% of fall damage that becomes experience");

            //Jump Effects
            JumpFallDamageThresholdMin = cfg.Bind("Jump.Effect", "Fall Damage Threshold Min", 4f,
                "meters to fall before you start calculating fall damage at level 0");
            JumpFallDamageThresholdMax = cfg.Bind("Jump.Effect", "Fall Damage Threshold Max", 24f,
                "meters to fall before you start calculating fall damage at level 100");
            JumpFallDamageReduxMin = cfg.Bind("Jump.Effect", "Fall Damage Reduction Min", -20f,
                "% less fall damage to take at level 0");
            JumpFallDamageReduxMax = cfg.Bind("Jump.Effect", "Fall Damage Reduction Max", 75f,
                "% less fall damage to take at level 100");
            JumpForcePercentMin = cfg.Bind("Jump.Effect", "Vertical Force Min", -10f,
                "% extra vertical jump force at level 0");
            JumpForcePercentMax = cfg.Bind("Jump.Effect", "Vertical Force Max", 120f,
                "% extra vertical jump force at level 100");
            JumpForwardForcePercentMin = cfg.Bind("Jump.Effect", "Horizontal Force Min", 0f,
                "% extra horizontal jump force at level 0");
            JumpForwardForcePercentMax = cfg.Bind("Jump.Effect", "Horizontal Force Max", 250f,
                "% extra horizontal jump force at level 100");
            //May not actually be used or be different from jump force
            JumpStaminaReduxMin = cfg.Bind("Jump.Effect", "Stamina Cost Min", 0f,
                "% less stamina cost to jump at level 0");
            JumpStaminaReduxMax = cfg.Bind("Jump.Effect", "Stamina Cost Max", 60f,
                "% less stamina cost to jump at level 100");
            JumpTiredModMin = cfg.Bind("Jump.Effect", "Tired Stamina Reduction Min", 0f,
                "% jump force added to the base game's tired factor, which reduces your jump force when out of stamina, at level 0");
            JumpTiredModMax = cfg.Bind("Jump.Effect", "Tired Stamina Reduction Max", 20f,
                "% jump force added to the base game's tired factor, which reduces your jump force when out of stamina, at level 100");


            //Swim Experience
            SwimXPSpeedPercent = cfg.Bind("Swim.Experience", "XP", 10f,
                "% of swim speed that becomes bonus experience gain");
            
            //Swim effects
            SwimSpeedPercentMin = cfg.Bind("Swim.Effect", "Speed Min", 0f,
                "% to increase swim speed at level 0");
            SwimSpeedPercentMax = cfg.Bind("Swim.Effect", "Speed Max", 250f,
                "% to increase swim speed at level 100");
            SwimAccelPercentMin = cfg.Bind("Swim.Effect", "Acceleration Min", 0f,
                "% to increase swim acceleration at level 0");
            SwimAccelPercentMax = cfg.Bind("Swim.Effect", "Acceleration Max", 250f,
                "% to increase swim acceleration at level 100");
            SwimTurnPercentMin = cfg.Bind("Swim.Effect", "Turn Speed Min", 0f,
                "% to increase swim turn speed at level 0");
            SwimTurnPercentMax = cfg.Bind("Swim.Effect", "Turn Speed Max", 500f,
                "% to increase swim turn speed at level 100");
            SwimStaminaPerSecMin = cfg.Bind("Swim.Effect", "Stamina cost min", 5f,
                "How much stamina swimming will take per second at level 0");
            SwimStaminaPerSecMax = cfg.Bind("Swim.Effect", "Stamina cost max", .5f,
                "How much stamina swimming will take per second at level 100");


            //Run Absolute Weight Experience
            RunAbsoluteWeightMinWeight = cfg.Bind("Run.Experience.AbsoluteWeight", "Minimum Weight", 100f,
                "The lowest weight you will get an experience bonus for carrying");
            RunAbsoluteWeightMaxWeight = cfg.Bind("Run.Experience.AbsoluteWeight", "Maximum Weight", 1800f,
                "The heighest weight you will get an experience bonus for carrying");
            RunAbsoluteWeightFactor = cfg.Bind("Run.Experience.AbsoluteWeight", "Factor", .6f,
                "Factor to define the slope of the absolute weight curve");
            RunXPAbsoluteWeightPercent = cfg.Bind("Run.Experience.AbsoluteWeight", "XP", 400f, 
                "% modifier for how much experience you get from absolute weight");
            

            //Run Relative Weight Experience
            RunRelativeWeightLight = cfg.Bind("Run.Experience.RelativeWeight", "Light Threshold", 33f,
                "Threshold for being at 'light' encumberance");
            RunRelativeWeightLightPercent = cfg.Bind("Run.Experience.RelativeWeight", "Light XP", -25f,
                "Experience Bonus for being at 'light' encumberance");
            RunRelativeWeightMed = cfg.Bind("Run.Experience.RelativeWeight", "Medium Threshold", 50f,
                "Threshold for being at 'medium' encumberance");
            RunRelativeWeightMedPercent = cfg.Bind("Run.Experience.RelativeWeight", "Medium XP", 0f,
                "Experience Bonus for being at 'medium' encumberance");
            RunRelativeWeightHighMed = cfg.Bind("Run.Experience.RelativeWeight", "High Threshold", 66f,
                "Threshold for being at 'high' encumberance");
            RunRelativeWeightHighMedPercent = cfg.Bind("Run.Experience.RelativeWeight", "High XP", 25f,
                "Experience Bonus for being at 'high' encumberance");
            RunRelativeWeightHeavy = cfg.Bind("Run.Experience.RelativeWeight", "Heavy Threshold", 80f,
                "Threshold for being at 'heavy' encumberance");
            RunRelativeWeightHeavyPercent = cfg.Bind("Run.Experience.RelativeWeight", "Heavy XP", 50f,
                "Experience Bonus for being at 'heavy' encumberance");
            RunRelativeWeightFull = cfg.Bind("Run.Experience.RelativeWeight", "Full Threshold", 100f,
                "Threshold for being at 'full' encumberance");
            RunRelativeWeightFullPercent = cfg.Bind("Run.Experience.RelativeWeight", "Full XP", 80f,
                "Experience Bonus for being at 'full' encumberance");
            RunRelativeWeightOverPercent = cfg.Bind("Run.Experience.RelativeWeight", "Overweight XP", 200f,
                "Experience Bonus for being overencumbered");
            RunXPRelativeWeightPercent = cfg.Bind("Run.Experience.RelativeWeight", "Overall XP", 100f,
                "% modifier for how much experience you get from relative weight");
            

            //Run Experience
            RunXPSpeedPercent = cfg.Bind("Run.Experience", "XP", 5f,
                "% of extra experience gained from run speed");
            

            //Run Effects
            RunSpeedPercentMin = cfg.Bind("Run.Effect", "Speed Min", 0f,
                "% extra run speed at level 0");
            RunSpeedPercentMax = cfg.Bind("Run.Effect", "Speed Max", 175f,
                "% extra run speed at level 100");
            RunEquipmentReduxMin = cfg.Bind("Run.Effect", "Equipment Reduction Min", -20f,
                "% less movespeed reduction from equipment at level 0");
            RunEquipmentReduxMax = cfg.Bind("Run.Effect", "Equipment Reduction Max", 60f,
                "% less movespeed reduction from equipment at level 100");
            RunEncumberancePercentMin = cfg.Bind("Run.Effect", "Encumberance Min", 0f,
                "% less run speed when your inventory is empty");
            RunEncumberancePercentMax = cfg.Bind("Run.Effect", "Encumberance Max", 50f,
                "% less run speed when your inventory is full");
            RunEncumberanceReduxMin = cfg.Bind("Run.Effect", "Encumberance Reduction Min", -15f,
                "% less effect from encumberance at level 0");
            RunEncumberanceReduxMax = cfg.Bind("Run.Effect", "Encumberance Reduction Max", 75f, 
                "% less effect from encumberance at level 100");
            RunStaminaReduxMin = cfg.Bind("Run.Effect", "Stamina Reduction Min", -25f,
                "% less stamina cost to run at level 100");
            RunStaminaReduxMax = cfg.Bind("Run.Effect", "Stamina Reduction Max", 80f,
                "% less stamina cost to run at level 100");
            RunStaminaPerLevel = cfg.Bind("Run.Effect", "Stamina Flat", .25f,
                "How much base stamina is added per level of run");


            //
            //
            //   Weapon changes
            //
            //
            //


            //Axes
            AxeDamagePercentMin = cfg.Bind("Axe.Effect", "Damage Min", 0f,
                "% extra damage done with axes at level 0");
            AxeDamagePercentMax = cfg.Bind("Axe.Effect", "Damage Max", 200f,
                "% extra damage done with axes at level 100");
            AxeStaminaReduxMin = cfg.Bind("Axe.Effect", "Stamina Reduction Min", 0f,
                "% less stamina usage for axes at level 0");
            AxeStaminaReduxMax = cfg.Bind("Axe.Effect", "Stamina Reduction Max", 60f,
                "% less stamina usage for axes at level 100");
            AxeStaminaPerLevel = cfg.Bind("Axe.Effect", "Base Stamina Gain per Level", .44f,
                "Flat amount of base stamina gained per level in axe");
            AxeChopDamagePercentMin = cfg.Bind("Axe.Effect", "Woodcutting Damage Min", 0f,
                "% extra woodcutting damage done at level 0");
            AxeChopDamagePercentMax = cfg.Bind("Axe.Effect", "Woodcutting Damage Max", 50f,
                "% extra woodcutting damage done at level 100");
            AxeCarryCapacityMin = cfg.Bind("Axe.Effect", "Carry Capacity Min", 0f,
                "Flat extra carrying capacity at level 0");
            AxeCarryCapacityMax = cfg.Bind("Axe.Effect", "Carry Capacity Max", 250f,
                "Flat extra carrying capacity at level 100");


            //Bows
            BowDamagePercentMin = cfg.Bind("Bow.Effect", "Damage Min", 0f,
                "% extra damage done with bows at level 0");
            BowDamagePercentMax = cfg.Bind("Bow.Effect", "Damage Max", 150f,
                "% extra damage done with bows at level 100");
            BowStaminaReduxMin = cfg.Bind("Bow.Effect", "Stamina Reduction Min", 0f,
                "% less stamina usage for bows at level 0");
            BowStaminaReduxMax = cfg.Bind("Bow.Effect", "Stamina Reduction Max", 75f,
                "% less stamina usage for bows at level 100");
            BowVelocityPercentMin = cfg.Bind("Bow.Effect", "Velocity Min", 0f, 
                "% extra velocity to fired arrows at level 0");
            BowVelocityPercentMax = cfg.Bind("Bow.Effect", "Velocity Max", 250f, 
                "% extra velocity to fired arrows at level 100");
            //BowDrawSpeedModMin = cfg.Bind("Bow.Effect", "Draw Speed Min", 0f, 
            //    "% extra bow draw speed at level 0");
            //BowDrawSpeedModMax = cfg.Bind("Bow.Effect", "Draw Speed Max", 0f, 
            //    "% extra bow draw speed at level 100");
            BowDropPercentMin = cfg.Bind("Bow.Effect", "Drop rate min", 0f,
                "% to increase creature drops at level 0");
            BowDropPercentMax = cfg.Bind("Bow.Effect", "Drop rate max", 100f,
                "% to increase creature drops at level 100");


            //Clubs
            ClubDamagePercentMin = cfg.Bind("Club.Effect", "Damage Min", 0f,
                "% extra damage done with clubs at level 0");
            ClubDamagePercentMax = cfg.Bind("Club.Effect", "Damage Max", 200f,
                "% extra damage done with clubs at level 100");
            ClubStaminaReduxMin = cfg.Bind("Club.Effect", "Stamina Reduction Min", 0f,
                "% less stamina usage for clubs at level 0");
            ClubStaminaReduxMax = cfg.Bind("Club.Effect", "Stamina Reduction Max", 50f,
                "% less stamina usage for clubs at level 100");
            ClubBluntPercentMin = cfg.Bind("Club.Effect", "Generic Blunt Min", 0f, 
                "% extra blunt damage to ALL weapons at level 0");
            ClubBluntPercentMax = cfg.Bind("Club.Effect", "Generic Blunt Max", 40f,
                "% extra blunt damage to ALL weapons at level 100");
            ClubKnockbackPercentMin = cfg.Bind("Club.Effect", "Generic Knockback Min", 0f,
                "% extra knockback to ALL weapons at level 0");
            ClubKnockbackPercentMax = cfg.Bind("Club.Effect", "Generic Knockback Max", 250f,
                "% extra knockback to ALL weapons at level 100");
            ClubStaggerPercentMin = cfg.Bind("Club.Effect", "Generic Stagger Min", 0f, 
                "% extra stagger damage to ALL ATTACKS at level 0");
            ClubStaggerPercentMax = cfg.Bind("Club.Effect", "Generic Stagger Max", 100f,
                "% extra stagger damage to ALL ATTACKS at level 100");


            //Unarmed
            FistDamagePercentMin = cfg.Bind("Fist.Effect", "Damage Min", -10f,
                "% extra damage done with bare fists at level 0");
            FistDamagePercentMax = cfg.Bind("Fist.Effect", "Damage Max", 120f,
                "% extra damage done with bare fists at level 100");
            FistStaminaReduxMin = cfg.Bind("Fist.Effect", "Stamina Reduction Min", 0f,
                "% less stamina usage for fists at level 0");
            FistStaminaReduxMax = cfg.Bind("Fist.Effect", "Stamina Reduction Max", 80f,
                "% less stamina usage for fists at level 100");
            FistDamageFlatMin = cfg.Bind("Fist.Effect", "Flat Damage Min", -5f, 
                "Flat extra damage at level 0");
            FistDamageFlatMax = cfg.Bind("Fist.Effect", "Flat Damage Max", 62f, 
                "Flat extra damage at level 100");
            FistBlockArmorMin = cfg.Bind("Fist.Effect", "Unarmed Block Armor Flat Min", -5f, 
                "Flat extra unarmed block armor at level 0");
            FistBlockArmorMax = cfg.Bind("Fist.Effect", "Unarmed Block Armor Flat Max", 50f, 
                "Flat extra unarmed block armor at level 100");
            FistMovespeedPercentMin = cfg.Bind("Fist.Effect", "Movespeed Min", 0f, 
                "% movespeed increase at level 0");
            FistMovespeedPercentMax = cfg.Bind("Fist.Effect", "Movespeed Max", 50f,
                "% movespeed increase at level 100");


            //Knives
            KnifeDamagePercentMin = cfg.Bind("Knife.Effect", "Damage Min", 0f, 
                "% extra damage done with knives at level 0");
            KnifeDamagePercentMax = cfg.Bind("Knife.Effect", "Damage Max", 200f,
                "% extra damage done with knives at level 100");
            KnifeStaminaReduxMin = cfg.Bind("Knife.Effect", "Stamina Reduction Min", 0f,
                "% less stamina usage for knives at level 0");
            KnifeStaminaReduxMax = cfg.Bind("Knife.Effect", "Stamina Reduction Max", 65f,
                "% less stamina usage for knives at level 100");
            KnifeBackstabPercentMin = cfg.Bind("Knife.Effect", "Backstab Min", 0f,
                "% extra sneak attack damage with ALL weapons at level 0");
            KnifeBackstabPercentMax = cfg.Bind("Knife.Effect", "Backstab Max", 150f,
                "% extra sneak attack damage with ALL weapons at level 100");
            KnifeMovespeedPercentMin = cfg.Bind("Knife.Effect", "Movementspeed Min", 0f,
                "% movespeed increase at level 0");
            KnifeMovespeedPercentMax = cfg.Bind("Knife.Effect", "Movementspeed Max", 80f,
                "% movespeed increase at level 100");
            KnifePiercePercentMin = cfg.Bind("Knife.Effect", "Generic Pierce Min", 0f,
                "% extra pierce damage with ALL weapons at level 0");
            KnifePiercePercentMax = cfg.Bind("Knife.Effect", "Generic Pierce Max", 50f,
                "% extra pierce damage with ALL weapons at level 0");


            //Mining
            MiningPickDamagePercentMin = cfg.Bind("Mining.Effect", "Pick Damage Min", 0f,
                "% increase to pick damage at level 0");
            MiningPickDamagePercentMax = cfg.Bind("Mining.Effect", "Pick Damage Max", 200f,
                "% increase to pick damage at level 100");
            MiningStaminaRebateMin = cfg.Bind("Mining.Effect", "Stamina Rebate Min", 0f,
                "Flat stamina rebate on each hit of a rock at level 0");
            MiningStaminaRebateMax = cfg.Bind("Mining.Effect", "Stamina Rebate Max", 7f,
                "Flat stamina rebate on each hit of a rock at level 100");
            MiningDropPercentMin = cfg.Bind("Mining.Effect", "Drop rate Min", 0f,
                "% increase to ore drops at level 0");
            MiningDropPercentMax = cfg.Bind("Mining.Effect", "Drop rate Max", 100f,
                "% increase to ore drops at level 100");
            MiningRegenHealthMin = cfg.Bind("Mining.Effect", "Regen Timer Reduction Min", 0f,
                "How many seconds to reduce the health regeneration timer at level 0");
            MiningRegenHealthMax = cfg.Bind("Mining.Effect", "Regen Timer Reduction Max", 3f,
                "How many seconds to reduce the health regeneration timer at level 100");
            MiningCarryCapacityMin = cfg.Bind("Mining.Effect", "Carry Capacity Min", 0f,
                "How much extra carrying capacity you get at level 0");
            MiningCarryCapacityMax = cfg.Bind("Mining.Effect", "Carry Capacity Max", 50f,
                "How much extra carrying capacity you get at level 100");


            //Polearms
            PolearmDamagePercentMin = cfg.Bind("Polearm.Effect", "Damage Min", 0f,
                "% extra damage done with polearms at level 0");
            PolearmDamagePercentMax = cfg.Bind("Polearm.Effect", "Damage Max", 150f,
                "% extra damage done with polearms at level 100");
            PolearmStaminaReduxMin = cfg.Bind("Polearm.Effect", "Stamina Reduction Min", 0f,
                "% less stamina usage for polearms at level 0");
            PolearmStaminaReduxMax = cfg.Bind("Polearm.Effect", "Stamina Reduction Max", 70f,
                "% less stamina usage for polearms at level 100");
            PolearmRangeMin = cfg.Bind("Polearm.Effect", "Generic Range Min", 0f,
                "Added units of range to all weapon attacks at level 0");
            PolearmRangeMax = cfg.Bind("Polearm.Effect", "Generic Range Max", .8f,
                "Added units of range to all weapon attacks at level 100");
            PolearmArmorMin = cfg.Bind("Polearm.Effect", "Block Flat Min", 0f,
                "Flat armor added to character at level 0");
            PolearmArmorMax = cfg.Bind("Polearm.Effect", "Block Flat Max", 25f,
                "Flat armor added to character at level 100");
            PolearmBlockMin = cfg.Bind("Polearm.Effect", "Block Min", 0f,
                "Flat block armor added to polearms at level 0");
            PolearmBlockMax = cfg.Bind("Polearm.Effect", "Block Max", 43f,
                "Flat block armor added to polearms at level 100");


            //Sneak
            SneakXPThreatPercent = cfg.Bind("Sneak.Experience", "Experience Bonus per Danger", 24f,
                    "Determines how much each 'point of danger' is worth in sneak exp");
            SneakStaminaDrainMin = cfg.Bind("Sneak.Effect", "Stamina Drain Min", 10f,
                    "Amount of stamina drain per second while sneaking at level 0");
            SneakStaminaDrainMax = cfg.Bind("Sneak.Effect", "Stamina Drain Max", 1.5f,
                    "Amount of stamina drain per second while sneaking at level 100");
            SneakSpeedPercentMin = cfg.Bind("Sneak.Effect", "Speed Min", 0f,
                    "% speed increase while sneaking at level 0");
            SneakSpeedPercentMax = cfg.Bind("Sneak.Effect", "Speed Max", 250f,
                    "% speed increase while sneaking at level 100");


            //Spears
            SpearDamagePercentMin = cfg.Bind("Spear.Effect", "Damage Min", 0f,
                "% extra damage done with spears at level 0");
            SpearDamagePercentMax = cfg.Bind("Spear.Effect", "Damage Max", 200f,
                "% extra damage done with spears at level 100");
            SpearStaminaReduxMin = cfg.Bind("Spear.Effect", "Stamina Reduction Min", 0f,
                "% less stamina usage for spears at level 0");
            SpearStaminaReduxMax = cfg.Bind("Spear.Effect", "Stamina Reduction Max", 70f,
                "% less stamina usage for spears at level 100");
            SpearVelocityPercentMin = cfg.Bind("Spear.Effect", "Thrown Velocity Min", 0f,
                "% extra velocity on thrown weapons at level 0");
            SpearVelocityPercentMax = cfg.Bind("Spear.Effect", "Thrown Velocity Max", 300f,
                "% extra velocity on thrown weapons at level 100");
            SpearProjectileDamagePercentMin = cfg.Bind("Spear.Effect", "Thrown Damage Percent Min", 0f,
                "% extra damage done with thrown weapons at level 0");
            SpearProjectilePercentMax = cfg.Bind("Spear.Effect", "Thrown Damage Percent Max", 200f,
                "% extra damage done with thrown weapons at level 100");
            SpearBlockArmorMin = cfg.Bind("Spear.Effect", "Generic Block Armor Min", 0f, 
                "Flat block armor always applied at level 0");
            SpearBlockArmorMax = cfg.Bind("Spear.Effect", "Generic Block Armor Max", 25f,
                "Flat block armor always applied at level 100");


            //Swords
            SwordDamagePercentMin = cfg.Bind("Sword.Effect", "Damage Min", 0f,
                "% extra damage done with swords at level 0");
            SwordDamagePercentMax = cfg.Bind("Sword.Effect", "Damage Max", 200f,
                "% extra damage done with swords at level 100");
            SwordStaminaReduxMin = cfg.Bind("Sword.Effect", "Stamina Reduction Min", 0f,
                "% less stamina usage for swords at level 0");
            SwordStaminaReduxMax = cfg.Bind("Sword.Effect", "Stamina Reduction Max", 60f,
                "% less stamina usage for swords at level 100");
            SwordParryPercentMin = cfg.Bind("Sword.Effect", "Generic Parry Min", 0f,
                "% extra parry bonus for ALL weapons at level 0"); 
            SwordParryPercentMax = cfg.Bind("Sword.Effect", "Generic Parry Max", 80f,
                "% extra parry bonus for ALL weapons at level 100");
            SwordSlashPercentMin = cfg.Bind("Sword.Effect", "Generic Slash Min", 0f, 
                "% extra slash damage for ALL weapons at level 0");
            SwordSlashPercentMax = cfg.Bind("Sword.Effect", "Generic Slash Max", 50f,
                "% extra slash damage for ALL weapons at level 100"); 
            SwordDodgeStaminaReduxMin = cfg.Bind("Sword.Effect", "Dodgeroll Stamina Reduction Min", 0f, 
                "% less stamina cost to dodge roll at level 0");
            SwordDodgeStaminaReduxMax = cfg.Bind("Sword.Effect", "Dodgeroll Stamina Reduction Max", 40f,
                "% less stamina cost to dodge roll at level 0");
            

            //Woodcutting
            WoodcuttingChopDamagePercentMin = cfg.Bind("Wood.Effect", "Chop Damage Min", 0f,
                "% increase to chop damage at level 0");
            WoodcuttingChopDamagePercentMax = cfg.Bind("Wood.Effect", "Chop Damage Max", 200f,
                "% increase to chop damage at level 100");
            WoodcuttingStaminaRebateMin = cfg.Bind("Wood.Effect", "Stamina Rebate Min", 0f,
                "Flat stamina rebate on each hit of a tree at level 0");
            WoodcuttingStaminaRebateMax = cfg.Bind("Wood.Effect", "Stamina Rebate Max", 9f,
                "Flat stamina rebate on each hit of a tree at level 100");
            WoodcuttingDropPercentMin = cfg.Bind("Wood.Effect", "Drop rate min", 0f,
                "% increase to wood drops at level 0");
            WoodcuttingDropPercentMax = cfg.Bind("Wood.Effect", "Drop rate max", 150f,
                "% increase to wood drops at level 100");
            WoodcuttingRegenStaminaMin = cfg.Bind("Wood.Effect", "Regen Timer Reduction Min", -.1f,
                    "Amount of seconds to take off the stamina regeneration timer at level 0. Base value in vanilla is 1s");
            WoodcuttingRegenStaminaMax = cfg.Bind("Wood.Effect", "Regen Timer Reduction Min", .5f,
                    "Amount of seconds to take off the stamina regeneration timer at level 100");
            WoodcuttingCarryCapacityMin = cfg.Bind("Wood.Effect", "Carry Capacity Min", 0f,
                "How much extra carrying capacity you get at level 0");
            WoodcuttingCarryCapacityMax = cfg.Bind("Wood.Effect", "Carry Capacity Max", 50f,
                "How much extra carrying capacity you get at level 100");
        }

        public static void InitSkillActiveDict()
        {
            SkillActive.Add(SkillMan.Agriculture, ActiveSkillAgriculture.Value);
            SkillActive.Add(Skills.SkillType.Axes, ActiveSkillAxe.Value);
            SkillActive.Add(Skills.SkillType.Blocking, ActiveSkillBlock.Value);
            SkillActive.Add(Skills.SkillType.Bows, ActiveSkillBow.Value);
            SkillActive.Add(SkillMan.Building, ActiveSkillBuilding.Value);
            SkillActive.Add(Skills.SkillType.Clubs, ActiveSkillClub.Value);
            SkillActive.Add(SkillMan.Cooking, ActiveSkillCooking.Value);
            SkillActive.Add(Skills.SkillType.Unarmed, ActiveSkillFist.Value);
            SkillActive.Add(Skills.SkillType.Jump, ActiveSkillJump.Value);
            SkillActive.Add(Skills.SkillType.Knives, ActiveSkillKnife.Value);
            SkillActive.Add(Skills.SkillType.Pickaxes, ActiveSkillMine.Value);
            SkillActive.Add(Skills.SkillType.Polearms, ActiveSkillPolearm.Value);
            SkillActive.Add(Skills.SkillType.Run, ActiveSkillRun.Value);
            SkillActive.Add(SkillMan.Sailing, ActiveSkillSailing.Value);
            SkillActive.Add(Skills.SkillType.Spears, ActiveSkillSpear.Value);
            SkillActive.Add(Skills.SkillType.Sneak, ActiveSkillSneak.Value);
            SkillActive.Add(Skills.SkillType.Swim, ActiveSkillSwim.Value);
            SkillActive.Add(Skills.SkillType.Swords, ActiveSkillSword.Value);
            SkillActive.Add(Skills.SkillType.WoodCutting, ActiveSkillWood.Value);
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
        public static bool IsSkillActive(Skills.SkillType skill)
        {
            return SkillActive[skill];
        }
        public static float GetWeaponEXPStrikeDestructibleMod()
        {
            return PerToMod(WeaponXPStrikeDestructiblePercent);
        }
        public static float GetWeaponEXPHoldUnarmedMod()
        {
            return PerToMod(WeaponXPHoldUnarmedPercent);
        }
        public static float GetBlockExpMult()
        {
            return PerToMult(BlockXPPercent);
        }
        public static float GetBlockParryExpMult()
        {
            return PerToMult(BlockXPParryPercent);
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
        public static float GetBlockPowerMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(BlockPowerPercentMin), 
                PerToMult(BlockPowerPercentMax), skillFactor);
        }
        public static float GetBlockHealth(float skillFactor)
        {
            return BlockHealthPerLevel.Value * MaxSkillLevel.Value * skillFactor;
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
        public static float GetRunSpeedMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(RunSpeedPercentMin), 
                PerToMult(RunSpeedPercentMax), skillFactor);
        }
        public static float GetEncumberanceCurveMult(float encumberanceMod)
        {
            return Mathf.Lerp(PerToMult(RunEncumberancePercentMin, true),
                PerToMult(RunEncumberancePercentMax, true), ShapeFactorSin(encumberanceMod));
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
        public static float GetAbsoluteWeightMod(float weight)
        {
            return Mathf.Clamp01((weight - RunAbsoluteWeightMinWeight.Value) / 
                (RunAbsoluteWeightMaxWeight.Value - RunAbsoluteWeightMinWeight.Value));
        }
        public static float GetAbsoluteWeightCurveMult(float weightMod)
        {
            float minBonus = 0f;
            float maxBonus = PerToMod(RunXPAbsoluteWeightPercent);
            return 1f + Mathf.Lerp(minBonus, maxBonus, 
                Mathf.Pow(weightMod, RunAbsoluteWeightFactor.Value));
            /*
            Jotunn.Logger.LogMessage($"Absolute weight exp mod: I was given {weightPercent} as weight percent\n" +
                $"The overall exp mod is {RunAbsoluteWeightExpMod.Value}, which I've converted to {PerToMult(RunAbsoluteWeightExpMod)}\n" +
                $"I just multiply that by my exponent curve, which is {weightPercent}^{RunAbsoluteWeightFactor.Value}," +
                $"giving me {Mathf.Pow(weightPercent+1f, RunAbsoluteWeightFactor.Value)}");*/
        }
        public static float GetRelativeWeightStageMult(float weightMod)
        {
            float mult = 1f;
            //Jotunn.Logger.LogMessage($"Relative weight: I'm given a percent of {weightPercent}\n");

            if (weightMod <= PerToMod(RunRelativeWeightLight))
                mult = PerToMult(RunRelativeWeightLightPercent);

            else if (weightMod <= PerToMod(RunRelativeWeightMed))
                mult = PerToMult(RunRelativeWeightMedPercent);

            else if (weightMod <= PerToMod(RunRelativeWeightHighMed))
                mult = PerToMult(RunRelativeWeightHighMedPercent);

            else if (weightMod <= PerToMod(RunRelativeWeightHeavy))
                mult = PerToMult(RunRelativeWeightHeavyPercent);

            else if (weightMod <= PerToMod(RunRelativeWeightFull))
                mult = PerToMult(RunRelativeWeightFullPercent);

            else
                mult = PerToMult(RunRelativeWeightOverPercent);

            //Jotunn.Logger.LogMessage($"Which means the multiplier I'm returning is {mult} times {PerToMult(RunRelativeWeightExpMod)}\n");

            return mult * PerToMod(RunXPRelativeWeightPercent);
        }
        public static float GetRunStaminaRedux(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(RunStaminaReduxMin, true), 
                PerToMult(RunStaminaReduxMax, true), skillFactor);
        }
        public static float GetRunEXPSpeedMult()
        {
            return PerToMult(RunXPSpeedPercent);
        }
        public static float GetRunStamina(float skillFactor)
        {
            return RunStaminaPerLevel.Value * MaxSkillLevel.Value * skillFactor;
        }
        public static float GetSwimXPSpeedMult()
        {
            return PerToMult(SwimXPSpeedPercent);
        }
        public static float GetSwimSpeedMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(SwimSpeedPercentMin), 
                PerToMult(SwimSpeedPercentMax), skillFactor);
        }
        public static float GetSwimAccelMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(SwimAccelPercentMin), 
                PerToMult(SwimAccelPercentMax), skillFactor);
        }
        public static float GetSwimTurnMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(SwimTurnPercentMin), 
                PerToMult(SwimTurnPercentMax), skillFactor);
        }
        public static float GetJumpForceMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(JumpForcePercentMin), 
                PerToMult(JumpForcePercentMax), skillFactor);
        }
        public static float GetJumpForwardForceMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(JumpForwardForcePercentMin), 
                PerToMult(JumpForwardForcePercentMax), skillFactor);
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
        public static float GetJumpTiredMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMod(JumpTiredModMin), 
                PerToMod(JumpTiredModMax), skillFactor);
        }
        public static float GetJumpXPMod()
        {
            return PerToMod(JumpXPPercent);
        }
        public static float GetWeaponDamageToExperience(float damage)
        {
            return PerToMod(WeaponXPStrikeDamagePercent) * 
                Mathf.Pow(damage, WeaponXPStrikeDamageFactor.Value);
        }
        public static float GetToolDamageToExperience(float damage)
        {
            return PerToMod(ToolXPStrikeDamagePercent) * 
                Mathf.Pow(damage, ToolXPStrikeDamageFactor.Value);
        }
        public static float GetSwimStaminaPerSec(float skillFactor)
        {
            return Mathf.Lerp(SwimStaminaPerSecMin.Value, SwimStaminaPerSecMax.Value, skillFactor);
        }
        public static float GetAxeDamageMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(AxeDamagePercentMin), 
                PerToMult(AxeDamagePercentMax), skillFactor);
        }
        public static float GetAxeStaminaRedux(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(AxeStaminaReduxMin, true), 
                PerToMult(AxeStaminaReduxMax, true), skillFactor);
        }
        public static float GetAxeChopDamageMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMod(AxeChopDamagePercentMin), 
                PerToMod(AxeChopDamagePercentMax), skillFactor);
        }
        public static float GetAxeStamina(float skillFactor)
        { 
            return AxeStaminaPerLevel.Value * skillFactor * ConfigManager.MaxSkillLevel.Value;
        }
        public static float GetAxeCarryCapacity(float skillFactor)
        {
            return Mathf.Lerp(AxeCarryCapacityMin.Value, AxeCarryCapacityMax.Value, skillFactor);
        }
        public static float GetWeaponBXPBowDistanceMult()
        {
            return PerToMult(WeaponBXPBowDistancePercent);
        }
        public static float GetBowDamageMult(float skillFactor)
        { 
            return Mathf.Lerp(PerToMult(BowDamagePercentMin), 
                PerToMult(BowDamagePercentMax), skillFactor);
        }
        public static float GetBowStaminaRedux(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(BowStaminaReduxMin, true), 
                PerToMult(BowStaminaReduxMax, true), skillFactor);
        }
        public static float GetBowVelocityMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(BowVelocityPercentMin), 
                PerToMult(BowVelocityPercentMax), skillFactor);
        }
        public static float GetBowDrawSpeed(float skillFactor)
        {
            return Mathf.Lerp(BaseBowDrawSpeedMin, 
                BaseBowDrawSpeedMax, skillFactor);
        }
        public static float GetBowDropRateMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMod(BowDropPercentMin), 
                PerToMod(BowDropPercentMax), skillFactor);
        }
        public static float GetClubDamageMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(ClubDamagePercentMin), 
                PerToMult(ClubDamagePercentMax), skillFactor);
        }
        public static float GetClubStaminaRedux(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(ClubStaminaReduxMin, true), 
                PerToMult(ClubStaminaReduxMax, true), skillFactor);
        }
        public static float GetClubBluntMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(ClubBluntPercentMin), 
                PerToMult(ClubBluntPercentMax), skillFactor);
        }
        public static float GetClubKnockbackMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(ClubKnockbackPercentMin), 
                PerToMult(ClubKnockbackPercentMax), skillFactor);
        }
        public static float GetClubStaggerMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(ClubStaggerPercentMin), 
                PerToMult(ClubStaggerPercentMax), skillFactor);
        }
        public static float GetFistDamageMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(FistDamagePercentMin), 
                PerToMult(FistDamagePercentMax), skillFactor);
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
            return Mathf.Lerp(PerToMod(FistMovespeedPercentMin), 
                PerToMod(FistMovespeedPercentMax), skillFactor);
        }
        public static float GetKnifeDamageMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(KnifeDamagePercentMin),
                PerToMult(KnifeDamagePercentMax), skillFactor);
        }
        public static float GetKnifeStaminaRedux(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(KnifeStaminaReduxMin, true), 
                PerToMult(KnifeStaminaReduxMax, true), skillFactor);
        }
        public static float GetKnifeBackstabMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(KnifeBackstabPercentMin), 
                PerToMult(KnifeBackstabPercentMax), skillFactor);
        }
        public static float GetKnifeMovespeedMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMod(KnifeMovespeedPercentMin), 
                PerToMod(KnifeMovespeedPercentMax), skillFactor);
        }
        public static float GetKnifePierceMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(KnifePiercePercentMin), 
                PerToMult(KnifePiercePercentMax), skillFactor);
        }
        public static float GetPolearmDamageMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(PolearmDamagePercentMin), 
                PerToMult(PolearmDamagePercentMax), skillFactor);
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
        public static float GetSpearDamageMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(SpearDamagePercentMin), 
                PerToMult(SpearDamagePercentMax), skillFactor);
        }
        public static float GetSpearStaminaRedux(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(SpearStaminaReduxMin, true), 
                PerToMult(SpearStaminaReduxMax, true), skillFactor);
        }
        public static float GetSpearVelocityMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(SpearVelocityPercentMin), 
                PerToMult(SpearVelocityPercentMax), skillFactor);
        }
        public static float GetSpearProjectileDamageMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(SpearProjectileDamagePercentMin), 
                PerToMult(SpearProjectilePercentMax), skillFactor);
        }
        public static float GetSpearBlockArmor(float skillFactor)
        {
            return Mathf.Lerp(SpearBlockArmorMin.Value, SpearBlockArmorMax.Value, skillFactor);
        }
        public static float GetSwordDamageMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(SwordDamagePercentMin), 
                PerToMult(SwordDamagePercentMax), skillFactor);
        }
        public static float GetSwordStaminaRedux(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(SwordDodgeStaminaReduxMin, true), 
                PerToMult(SwordDodgeStaminaReduxMax, true), skillFactor);
        }
        public static float GetSwordParryMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(SwordParryPercentMin), 
                PerToMult(SwordParryPercentMax), skillFactor);
        }
        public static float GetSwordSlashMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(SwordSlashPercentMin), 
                PerToMult(SwordSlashPercentMax), skillFactor);
        }
        public static float GetSwordDodgeStaminaRedux(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(SwordDodgeStaminaReduxMin, true), 
                PerToMult(SwordDodgeStaminaReduxMax, true), skillFactor);
        }
        public static float GetWoodcuttingDamageMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMod(WoodcuttingChopDamagePercentMin), 
                PerToMod(WoodcuttingChopDamagePercentMax), skillFactor);
        }
        public static float GetWoodDropMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMod(WoodcuttingDropPercentMin), 
                PerToMod(WoodcuttingDropPercentMax), skillFactor);
        }
        public static float GetWoodcuttingStaminaRebate(float skillFactor)
        {
            return Mathf.Lerp(WoodcuttingStaminaRebateMin.Value, 
                WoodcuttingStaminaRebateMax.Value, skillFactor);
        }
        public static float GetWoodcuttingRegenLessTime(float skillFactor)
        {
            return Mathf.Lerp(WoodcuttingRegenStaminaMin.Value,
                WoodcuttingRegenStaminaMax.Value, skillFactor);
        }
        public static float GetWoodcuttingCarryCapacity(float skillFactor)
        {
            return Mathf.Lerp(WoodcuttingCarryCapacityMin.Value,
                WoodcuttingCarryCapacityMax.Value, skillFactor);
        }
        public static float GetMiningDamageMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(MiningPickDamagePercentMin), 
                PerToMult(MiningPickDamagePercentMax), skillFactor);
        }
        public static float GetMiningDropMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMod(MiningDropPercentMin), 
                PerToMod(MiningDropPercentMax), skillFactor);
        }
        public static float GetMiningStaminaRebate(float skillFactor)
        {
            return Mathf.Lerp(MiningStaminaRebateMin.Value, MiningStaminaRebateMax.Value, skillFactor);
        }
        public static float GetMiningRegenLessTime(float skillFactor)
        {
            return Mathf.Lerp(MiningRegenHealthMin.Value,
                MiningRegenHealthMax.Value, skillFactor);
        }
        public static float GetMiningCarryCapacity(float skillFactor)
        {
            return Mathf.Lerp(MiningCarryCapacityMin.Value,
                MiningCarryCapacityMax.Value, skillFactor);
        }
        public static float GetDropItemThreshold()
        {
            return PerToMod(DropNewItemThreshold);
        }
        public static float GetSneakXPMult()
        {
            return PerToMult(SneakXPThreatPercent);
        }
        public static float GetSneakStaminaDrain(float skillFactor)
        {
            return Mathf.Lerp(SneakStaminaDrainMin.Value,SneakStaminaDrainMax.Value, skillFactor);
        }
        public static float GetSneakSpeedMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(SneakSpeedPercentMin),
                PerToMult(SneakSpeedPercentMax), skillFactor);
        }
        public static float GetSneakFactor(float skillFactor, float lightFactor)
        {
            return Mathf.Lerp(0.5f + lightFactor * 0.5f, 0.2f + lightFactor * 0.4f, skillFactor);
            /*Mathf.Lerp(PerToMult(),
                PerToMult(), skillFactor);
            */
        }
        public static float GetVanillaBlockMult(float skillFactor)
        {
            return 1f + skillFactor * .5f;
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
