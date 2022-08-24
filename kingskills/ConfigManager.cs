using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Configuration;

namespace kingskills
{
    /*
     * 
        //How much flat block armor do we get per level up to max
        public const float FlatBlockPowerMax = 50f;
        public const float FlatBlockPowerMin = 0f;
        //What percent block armor do we get per level min to max
        public const float PerBlockPowerMax = 1f;
        public const float PerBlockPowerMin = -.25f;
        //How much is the stamina cost for blocking reduced from min to max
        public const float BlockStaminaReduxMax = .5f;
        public const float BlockStaminaReduxMin = -.1f;
        //How much is the player's stagger limit increased from min to max
        public const float AbsoluteStaggerLimitIncreaseMax = .3f;
        public const float AbsoluteStaggerLimitIncreaseMin = 0f;
        //Used for perks, shouldn't be a float eventually
        public const float AdditionalParryBonus = 1f;
        //How much experience do we get per damage blocked?
        public const float BlockExpMod = .22f;
        //What is the bonus experience for parrying?
        public const float ParryExpMod = 2f;

    
        public const float FallDamageThresholdMax = 12f;
        public const float FallDamageThresholdMin = 4f;
        public const float FallDamageReduxMax = .6f;
        public const float FallDamageReduxMin = -.15f;
    
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
            WeaponEXPSwing = cfg.Bind("Experience.Weapons", "XP Per Swing", 1f,
                "Flat experience to be gained on each swing, regardless of hit.");

            WeaponEXPHoldPerTick = cfg.Bind("Experience.Weapons", "XP Per Second", .05f,
                "Flat experience to be gained every second holding a particular weapon");
            WeaponEXPHoldTickLength = cfg.Bind("Experience.Weapons", "Hold Timer", 1.0f,
                "Seconds between ticks of hold experience");
            WeaponEXPHoldUnarmedMod = cfg.Bind("Experience.Weapons", "Unarmed Mod", 20f,
                "% of normal hold experience gained for unarmed when holding nothing");
            WeaponEXPHoldUnarmedMod /= 100;

            WeaponEXPStrikeDamageMod = cfg.Bind("Experience.Weapons", "XP Per Damage", 0f,
                "Multiplied by damage dealt to get experience earned");
            WeaponEXPStrikeDestructibleMod = cfg.Bind("Experience.Weapons", "Destructible Mod", 10f,
                "% of experience gained when you hit a non living thing");
            WeaponEXPStrikeDestructibleMod /= 100; //Change from % into muliplier

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
            WeaponBXPBowDistanceMod /= 100; //Change from % into muliplier


            ToolEXPStrikeDamageMod = cfg.Bind("Experience.Tools", "Strike Damage Mod", 1.5f,
                "% of damage done to resources that becomes experience for gathering skills " +
                "(Woodcutting, Mining)");


        }
    }    
}
