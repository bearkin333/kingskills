using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Configuration;
using UnityEngine;
using kingskills.Perks;
using Jotunn.Managers;
using kingskills.SE;

namespace kingskills
{
    //ConfigManager. I saved over 3,000 characters across the entire
    //mod by shortening the name, so... oh well. Makes things more readable.
    //Actually stands for Cool Fucking Game
    //Currently 773 references throughout the code. high score!
    class CFG
    {
        /*
         * Generalized language:
         * 
         * Per (or percent) refers to a number, typically between 1 and 100, that will
         * probably be modified later on. Used for config entries.
         * Ex: A per of 50% implies the final number will have a final value of 150%
         * 
         * Redux (or Reduction) refers to a percent that will be subtracted from 
         * the whole. NO REDUX SHOULD BE OVER 100!
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

        #region globalvariables
        //Variables and constants for use that aren't configurable
        public static Dictionary<Skills.SkillType, bool> SkillActive = new Dictionary<Skills.SkillType, bool>();


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

        public const bool SkipOriginal = false;
        public const bool DontSkipOriginal = true;

        /* flavor curve numbers
        const float TotalXp = 20553.6f;
        const float MasteryTime = 5f * 60 * 60;  // seconds
        const float XpPerSec = TotalXp / MasteryTime; // xp/s needed to master skill in target_mastery_time
        */
        #endregion globalvariables

        public static void Init(ConfigFile cfg)
        {
            //Read the config settings for all the generic stuff
            InitGenericConfigs(cfg);
            InitPerkConfig(cfg);
            InitWeaponEXPConfig(cfg);

            //Read the config settings for all the skills
            InitBotanyConfig(cfg);
            InitAxeConfigs(cfg);
            InitBlockConfigs(cfg);
            InitBowConfigs(cfg);
            InitBuildConfig(cfg);
            InitClubConfigs(cfg);
            InitCookConfig(cfg);
            InitFistConfigs(cfg);
            InitJumpConfigs(cfg);
            InitKnifeConfigs(cfg);
            InitMineConfigs(cfg);
            InitPolearmConfigs(cfg);
            InitRunConfigs(cfg);
            InitSailConfig(cfg);
            InitSneakConfigs(cfg);
            InitSpearConfigs(cfg);
            InitSwimConfigs(cfg);
            InitSwordConfigs(cfg);
            InitWoodConfigs(cfg);

            //If any of the configs change via sync, all the perks get re-initialized, to update their info
            SynchronizationManager.OnConfigurationSynchronized += (obj, attr) =>
            {
                PerkMan.InitPerkList();
            };
        }


        public static void InitSkillActiveDict()
        {
            SkillActive.Add(SkillMan.Agriculture, ActiveSkillBotany.Value);
            SkillActive.Add(Skills.SkillType.Axes, ActiveSkillAxe.Value);
            SkillActive.Add(Skills.SkillType.Blocking, ActiveSkillBlock.Value);
            SkillActive.Add(Skills.SkillType.Bows, ActiveSkillBow.Value);
            SkillActive.Add(SkillMan.Building, ActiveSkillBuild.Value);
            SkillActive.Add(Skills.SkillType.Clubs, ActiveSkillClub.Value);
            SkillActive.Add(SkillMan.Cooking, ActiveSkillCook.Value);
            SkillActive.Add(Skills.SkillType.Unarmed, ActiveSkillFist.Value);
            SkillActive.Add(Skills.SkillType.Jump, ActiveSkillJump.Value);
            SkillActive.Add(Skills.SkillType.Knives, ActiveSkillKnife.Value);
            SkillActive.Add(Skills.SkillType.Pickaxes, ActiveSkillMine.Value);
            SkillActive.Add(Skills.SkillType.Polearms, ActiveSkillPolearm.Value);
            SkillActive.Add(Skills.SkillType.Run, ActiveSkillRun.Value);
            SkillActive.Add(SkillMan.Sailing, ActiveSkillSail.Value);
            SkillActive.Add(Skills.SkillType.Spears, ActiveSkillSpear.Value);
            SkillActive.Add(Skills.SkillType.Sneak, ActiveSkillSneak.Value);
            SkillActive.Add(Skills.SkillType.Swim, ActiveSkillSwim.Value);
            SkillActive.Add(Skills.SkillType.Swords, ActiveSkillSword.Value);
            SkillActive.Add(Skills.SkillType.WoodCutting, ActiveSkillWood.Value);
        }


        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Generic Functions
        ////////////////////////////////////////////////////////////////////////////////////////
        #region genericfunc
        #region configdef
        public static ConfigEntry<float> DropNewItemThreshold;
        public static ConfigEntry<float> MaxSkillLevel;
        public static ConfigEntry<float> DisplayExperienceThreshold;
        public static ConfigEntry<float> XPTextScaleMin;
        public static ConfigEntry<float> XPTextScaleMax;
        public static ConfigEntry<float> XPTextValueMin;
        public static ConfigEntry<float> XPTextValueMax;
        public static ConfigEntry<float> XPTextCurveFactor;

        public static ConfigEntry<KeyCode> KeyBindingSkillGUI;
        public static ConfigEntry<KeyCode> KeyBindingSkillGUIExit;
        public static ConfigEntry<KeyCode> KeyBindingCollapseFood;
        public static ConfigEntry<KeyCode> KeyBindingConfirmShortcut;
        public static ConfigEntry<KeyCode> KeyBindingMeteorDrop;
        public static ConfigEntry<KeyCode> KeyBindingDetailedPerkTooltip;

        public static Color ColorBonusBlue;
        public static Color ColorAscendedGreen;
        public static Color ColorExperienceYellow;
        public static Color ColorTitle;
        public static Color ColorWhite;
        public static Color ColorKingSkills;
        public static Color ColorPTGreen;
        public static Color ColorPTRed;
        public static Color ColorKingBlurbs;

        public const string ColorBonusBlueFF = "<color=#33E0EDFF>";
        public const string ColorAscendedGreenFF = "<color=#73EA52FF>";
        public const string ColorExperienceYellowFF = "<color=#FAF53BFF>";
        public const string ColorTitleFF = "<color=#FAF56EFF>";
        public const string ColorWhiteFF = "<color=#FFFFFFFF>";
        public const string ColorKingSkillsFF = "<color=#C43BFDFF>";

        public const string ColorFireFF = "<color=#FF0829FF>";
        public const string ColorLightningFF = "<color=#FFE814FF>";
        public const string ColorSpiritFF = "<color=#A7F7FCFF>";
        public const string ColorPoisonFF = "<color=#1FED18FF>";
        public const string ColorFrostFF = "<color=#2462F2FF>";

        public const string ColorPTGreenFF = "<color=#7AF365FF>";
        public const string ColorPTRedFF = "<color=#EA0C0CFF>";
        public const string ColorPTWhiteFF = "<color=#FEFDF5FF>";

        public const string ColorEnd = "</color>";

        public const string ZDOKiller = "Attacking Player";
        public const string ZDOStaggerFlag = "Player Hit";

        #endregion configdef

        public static void InitGenericConfigs(ConfigFile cfg)
        {
            ColorBonusBlue = new Color(0.20f, 0.88f, 0.93f);
            ColorAscendedGreen = new Color(0.45f, 0.92f, 0.32f);
            ColorExperienceYellow = new Color(0.98f, 0.96f, 0.23f);
            ColorTitle = new Color(0.98f, 0.96f, 0.43f);
            ColorWhite = new Color(1f, 1f, 1f);
            ColorKingSkills = new Color(0.77f, 0.23f, 0.99f);
            ColorPTGreen = new Color(0.48f, 0.95f, 0.40f);
            ColorPTRed = new Color(0.92f, 0.05f, 0.05f);
            ColorKingBlurbs = new Color(0.7f, 0.4f, 0.71f);

            MaxSkillLevel = cfg.Bind("Generic", "Max Skill Level", 100f,
                    AdminCD("This is the level that all king skills can go up to.", true));
            DisplayExperienceThreshold = cfg.Bind("Generic", "Experience Display Threshold", .2f,
                    AdminCD("Threshold under which experience earned will not display as a message.", true));
            DropNewItemThreshold = cfg.Bind("Generic", "Drop New Item Threshold", 50f,
                    AdminCD("% of 1 item needed to generate before you round up to a full item."));


            XPTextScaleMin = cfg.Bind("Generic", "Text Scale Min", 16f,
                    AdminCD("Font size of the smallest possible exp text", true));
            XPTextScaleMax = cfg.Bind("Generic", "Text Scale Max", 75f,
                    AdminCD("Font size of the largest possible exp text", true));
            XPTextValueMin = cfg.Bind("Generic", "Text Value Min", 0.2f,
                    AdminCD("Experience value to generate the smallest size exp text", true));
            XPTextValueMax = cfg.Bind("Generic", "Text Value Max", 100f,
                    AdminCD("Experience value to generate the largest size exp text", true));
            XPTextCurveFactor = cfg.Bind("Generic", "Text Curve Factor", .6f,
                    AdminCD("Factor to define the slope of the curve for exp text scaling", true));


            KeyBindingSkillGUI = cfg.Bind("Generic.Bindings", "Toggle Skill GUI", KeyCode.I,
                    "Button to open and close the King Skills GUI");
            KeyBindingSkillGUIExit = cfg.Bind("Generic.Bindings", "Exit Skill GUI", KeyCode.Escape,
                    UnbrowsableCD("Shortcut to close the King Skills GUI. Shouldn't be changed."));
            KeyBindingCollapseFood = cfg.Bind("Generic.Bindings", "Collapse Food", KeyCode.Y,
                    "Button to collapse all food of the hovered type in inventory.");
            KeyBindingConfirmShortcut = cfg.Bind("Generic.Bindings", "Confirm Shortcut", KeyCode.Return,
                    UnbrowsableCD("Shortcut to hitting 'Yes' on confirmation windows. Probably shouldn't be changed."));
            KeyBindingMeteorDrop = cfg.Bind("Generic.Bindings", "Meteor Drop", KeyCode.C,
                    "Button to press for the Meteor Drop Perk");
            KeyBindingDetailedPerkTooltip = cfg.Bind("Generic.Bindings", "Detailed Perk Tooltip", KeyCode.LeftShift,
                    UnbrowsableCD("Button to hold to show the detailed perk tooltip"));

        }

        public static bool IsSkillActive(Skills.SkillType skill)
        {
            if (SkillActive.TryGetValue(skill, out bool value))
                return value;
            return false;
        }

        public static int GetXPTextScaledSize(float num)
        {

            float expPercent = (num - XPTextValueMin.Value) / (XPTextValueMax.Value - XPTextValueMin.Value);
            float curveFactor = Mathf.Pow(expPercent, XPTextCurveFactor.Value);

            //Jotunn.Logger.LogMessage($"this exp is {num}, which makes it  {expPercent}% of the max value"));
            //Jotunn.Logger.LogMessage($"I am running {expPercent} to the power of {XPTextCurveFactor.Value}"));
            //Jotunn.Logger.LogMessage($"applying our curve to it causes it to be {curveFactor}%"));
            //Jotunn.Logger.LogMessage($"our final size will be {(int)Mathf.Floor(Mathf.Lerp(XPTextScaleMin.Value, XPTextScaleMax.Value, curveFactor))}"));

            return (int)Mathf.Floor(Mathf.Lerp(XPTextScaleMin.Value, XPTextScaleMax.Value, curveFactor));
        }

        public static float GetDropItemThreshold()
        {
            return PerToMod(DropNewItemThreshold);
        }

        private static float PerToMult(ConfigEntry<float> config, bool redux = false)
        {
            float mult = config.Value;
            mult /= 100;
            if (redux)
                mult = 1 - mult;
            else
                mult++;
            //Jotunn.Logger.LogMessage($"Reading out {config.Definition.Key} as {mult}"));
            return mult;
        }

        private static float PerToMod(ConfigEntry<float> config, bool redux = false)
        {
            float mod = config.Value;
            mod /= 100;
            if (redux)
                mod = 1 - mod;

            // Jotunn.Logger.LogMessage($"Reading out {config.Definition.Key} as {mod}"));
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
            //Jotunn.Logger.LogMessage($"{x} is being sin curved into {Mathf.Sin(Mathf.Lerp(0f, Mathf.PI / 2, x))}"));
            return Mathf.Sin(Mathf.Lerp(0f, Mathf.PI / 2, x));
        }

        public static string GetNameFromSkill(Skills.SkillType skill)
        {
            switch (skill){
                case Skills.SkillType.Axes: return "Axes";
                case Skills.SkillType.Blocking: return "Blocking";
                case Skills.SkillType.Bows: return "Bows";
                case Skills.SkillType.Clubs: return "Clubs";
                case Skills.SkillType.Unarmed: return "Fists";
                case Skills.SkillType.Jump: return "Jump";
                case Skills.SkillType.Knives: return "Knives";
                case Skills.SkillType.Pickaxes: return "Mining";
                case Skills.SkillType.Polearms: return "Polearms";
                case Skills.SkillType.Run: return "Run";
                case Skills.SkillType.Spears: return "Spears";
                case Skills.SkillType.Sneak: return "Sneak";
                case Skills.SkillType.Swim: return "Swim";
                case Skills.SkillType.Swords: return "Swords";
                case Skills.SkillType.WoodCutting: return "Woodcutting";
            } if (skill == SkillMan.Agriculture) return "Agriculture";
            else if (skill == SkillMan.Sailing) return "Sailing";
            else if (skill == SkillMan.Building) return "Building";
            else if (skill == SkillMan.Cooking) return "Cooking";

            return "None";
        }

        public static Skills.SkillType GetSkillFromName(string name)
        {   
            switch (name){
                    case "Agriculture": return SkillMan.Agriculture;
                    case "Axes": return Skills.SkillType.Axes;
                    case "Blocking": return Skills.SkillType.Blocking;
                    case "Bows": return Skills.SkillType.Bows;
                    case "Building": return SkillMan.Building;
                    case "Clubs": return Skills.SkillType.Clubs;
                    case "Cooking": return SkillMan.Cooking;
                    case "Fists": return Skills.SkillType.Unarmed;
                    case "Jump": return Skills.SkillType.Jump;
                    case "Knives": return Skills.SkillType.Knives;
                    case "Mining": return Skills.SkillType.Pickaxes;
                    case "Polearms": return Skills.SkillType.Polearms;
                    case "Run": return Skills.SkillType.Run;
                    case "Sailing": return SkillMan.Sailing;
                    case "Spears": return Skills.SkillType.Spears;
                    case "Sneak": return Skills.SkillType.Sneak;
                    case "Swim": return Skills.SkillType.Swim;
                    case "Swords": return Skills.SkillType.Swords;
                    case "Woodcutting": return Skills.SkillType.WoodCutting;
                } return Skills.SkillType.None;
            }

        public static ConfigDescription AdminCD(string description, bool browsable = false)
        {
            return new ConfigDescription(description, null,
                new ConfigurationManagerAttributes { IsAdminOnly = true, Browsable = browsable });
        }

        public static ConfigDescription UnbrowsableCD(string description)
        {
            return new ConfigDescription(description, null,
                new ConfigurationManagerAttributes { Browsable = false });
        }

        public static bool CheckPlayerActivePerk(Character player, PerkMan.PerkType perk)
        {
            if (player == null ||
                Player.m_localPlayer == null ||
                !(player.GetZDOID() == Player.m_localPlayer.GetZDOID())) return false;


            if (!PerkMan.IsPerkActive(perk)) return false;
            return true;
        }

        public static bool SetZDOVariable(Character player, PerkMan.PerkType perk, string var, bool value)
        {
            if (!CheckPlayerActivePerk(player, perk)) return false;
            //Jotunn.Logger.LogMessage("perk active and valid, changing ZDO value");

            ZDO zdo = player.m_nview.m_zdo;
            zdo.Set(var, value);
            return true;
        }

        public static HitData CreateHitFromWeapon(ItemDrop.ItemData weapon, Player attacker)
        {
            HitData hitData = new HitData();
            hitData.m_toolTier = weapon.m_shared.m_toolTier;
            if (weapon.m_shared.m_attackStatusEffect)
                hitData.m_statusEffect = weapon.m_shared.m_attackStatusEffect.m_name;
            hitData.m_pushForce = weapon.m_shared.m_attackForce;
            hitData.m_backstabBonus = weapon.m_shared.m_backstabBonus;
            hitData.m_staggerMultiplier = 1f;
            hitData.m_dodgeable = weapon.m_shared.m_dodgeable;
            hitData.m_blockable = weapon.m_shared.m_blockable;
            hitData.m_skill = weapon.m_shared.m_skillType;
            hitData.m_damage = weapon.GetDamage();
            hitData.SetAttacker(attacker);
            return hitData;
        }

        public static bool GetKillerExists(ZNetView instanceNView, ref Player killer)
        {
            long killingPlayer = instanceNView.m_zdo.GetLong(ZDOKiller, 0);
            Jotunn.Logger.LogMessage($"killer was {killingPlayer}");
            killer = Player.GetPlayer(killingPlayer);
            if (killer == null)
            {
                Jotunn.Logger.LogMessage($"no killer");
                return false;
            }
            return true;
        }

        public static string ConditionalPluralize(float num, string text)
        {
            if (num > 1) return text + "s";
            return text;
        }

        #endregion genericfunc

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                    Weapons
        ////////////////////////////////////////////////////////////////////////////////////////
        #region weapons
        ////////////////////////////////////////////////////////////////////////////////////////     
        ///                             Axes
        ////////////////////////////////////////////////////////////////////////////////////////
        #region axe
        #region configdef
        public static ConfigEntry<bool> ActiveSkillAxe;
        public static ConfigEntry<float> WeaponBXPAxeRange;
        public static ConfigEntry<float> WeaponBXPAxeTreeAmount;
        public static ConfigEntry<float> AxeDamagePercentMin;
        public static ConfigEntry<float> AxeDamagePercentMax;
        public static ConfigEntry<float> AxeStaminaReduxMin;
        public static ConfigEntry<float> AxeStaminaReduxMax;
        public static ConfigEntry<float> AxeStaminaPerLevel;
        public static ConfigEntry<float> AxeChopDamagePercentMin;
        public static ConfigEntry<float> AxeChopDamagePercentMax;
        public static ConfigEntry<float> AxeCarryCapacityMin;
        public static ConfigEntry<float> AxeCarryCapacityMax;
        #endregion configdef

        private static void InitAxeConfigs(ConfigFile cfg)
        {
            ActiveSkillAxe = cfg.Bind("Generic.Active", "Axes", true,
                    AdminCD("Whether or not to activate king's skills version of the axes skill", true));

            AxeDamagePercentMin = cfg.Bind("Axe.Effect", "Damage Min", 0f,
                AdminCD("% extra damage done with axes at level 0"));
            AxeDamagePercentMax = cfg.Bind("Axe.Effect", "Damage Max", 200f,
                AdminCD("% extra damage done with axes at level 100"));
            AxeStaminaReduxMin = cfg.Bind("Axe.Effect", "Stamina Reduction Min", 0f,
                AdminCD("% less stamina usage for axes at level 0"));
            AxeStaminaReduxMax = cfg.Bind("Axe.Effect", "Stamina Reduction Max", 60f,
                AdminCD("% less stamina usage for axes at level 100"));
            AxeStaminaPerLevel = cfg.Bind("Axe.Effect", "Base Stamina Gain per Level", .44f,
                AdminCD("Flat amount of base stamina gained per level in axe"));
            AxeChopDamagePercentMin = cfg.Bind("Axe.Effect", "Woodcutting Damage Min", 0f,
                AdminCD("% extra woodcutting damage done at level 0"));
            AxeChopDamagePercentMax = cfg.Bind("Axe.Effect", "Woodcutting Damage Max", 50f,
                AdminCD("% extra woodcutting damage done at level 100"));
            AxeCarryCapacityMin = cfg.Bind("Axe.Effect", "Carry Capacity Min", 0f,
                AdminCD("Flat extra carrying capacity at level 0"));
            AxeCarryCapacityMax = cfg.Bind("Axe.Effect", "Carry Capacity Max", 250f,
                AdminCD("Flat extra carrying capacity at level 100"));

            WeaponBXPAxeTreeAmount = cfg.Bind("Weapon.BonusExperience", "Axe Log", 5f,
                AdminCD("Flat BXP gained every time you break down a log"));
            WeaponBXPAxeRange = cfg.Bind("Weapon.BonusExperience", "Axe Felling Range", 100f,
                AdminCD("Distance to check for axe BXP gain"));
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
            return AxeStaminaPerLevel.Value * skillFactor * CFG.MaxSkillLevel.Value;
        }
        public static float GetAxeCarryCapacity(float skillFactor)
        {
            return Mathf.Lerp(AxeCarryCapacityMin.Value, AxeCarryCapacityMax.Value, skillFactor);
        }


        #endregion axe

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                             Bows
        ////////////////////////////////////////////////////////////////////////////////////////
        #region bow
        #region configdef
        public static ConfigEntry<bool> ActiveSkillBow;
        public static ConfigEntry<float> BowBXPDistanceFactor;
        public static ConfigEntry<float> BowBXPDistanceMod;
        public static ConfigEntry<float> BowDamagePercentMin;
        public static ConfigEntry<float> BowDamagePercentMax;
        public static ConfigEntry<float> BowStaminaReduxMin;
        public static ConfigEntry<float> BowStaminaReduxMax;
        public static ConfigEntry<float> BowVelocityPercentMin;
        public static ConfigEntry<float> BowVelocityPercentMax;
        public static ConfigEntry<float> BowDropPercentMin;
        public static ConfigEntry<float> BowDropPercentMax;

        public static Dictionary<string, float> BowDropTable = new Dictionary<string, float>();
        #endregion configdef

        public static void InitBowConfigs(ConfigFile cfg)
        {
            ActiveSkillBow = cfg.Bind("Generic.Active", "Bows", true,
                    AdminCD("Whether or not to activate king's skills version of the bows skill", true)); ;

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

            //exp
            BowBXPDistanceFactor = cfg.Bind("Weapon.BonusExperience", "Bow Distance Factor", 1.12f,
                AdminCD("Factor to define the scale of the distance curve"));
            BowBXPDistanceMod = cfg.Bind("Weapon.BonusExperience", "Bow Distance Mod", .5f,
                AdminCD("how much each point of distance is worth base for distance bow bxp"));

            //effects
            BowDamagePercentMin = cfg.Bind("Bow.Effect", "Damage Min", 0f,
                AdminCD("% extra damage done with bows at level 0"));
            BowDamagePercentMax = cfg.Bind("Bow.Effect", "Damage Max", 150f,
                AdminCD("% extra damage done with bows at level 100"));
            BowStaminaReduxMin = cfg.Bind("Bow.Effect", "Stamina Reduction Min", 0f,
                AdminCD("% less stamina usage for bows at level 0"));
            BowStaminaReduxMax = cfg.Bind("Bow.Effect", "Stamina Reduction Max", 75f,
                AdminCD("% less stamina usage for bows at level 100"));
            BowVelocityPercentMin = cfg.Bind("Bow.Effect", "Velocity Min", 0f,
                AdminCD("% extra velocity to fired arrows at level 0"));
            BowVelocityPercentMax = cfg.Bind("Bow.Effect", "Velocity Max", 250f,
                AdminCD("% extra velocity to fired arrows at level 100"));
            //BowDrawSpeedModMin = cfg.Bind("Bow.Effect", "Draw Speed Min", 0f, 
            //    AdminCD("% extra bow draw speed at level 0"));
            //BowDrawSpeedModMax = cfg.Bind("Bow.Effect", "Draw Speed Max", 0f, 
            //    AdminCD("% extra bow draw speed at level 100"));
            BowDropPercentMin = cfg.Bind("Bow.Effect", "Drop rate min", 0f,
                AdminCD("% to increase creature drops at level 0"));
            BowDropPercentMax = cfg.Bind("Bow.Effect", "Drop rate max", 300f,
                AdminCD("% to increase creature drops at level 100"));

        }

        public static float GetBowBXPDistance(float distance)
        {
            return Mathf.Pow(BowBXPDistanceMod.Value * distance, BowBXPDistanceFactor.Value);
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

        #endregion bow

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                              Clubs
        ////////////////////////////////////////////////////////////////////////////////////////
        #region club
        #region configdef
        public static ConfigEntry<bool> ActiveSkillClub;
        public static ConfigEntry<float> ClubBXPHealthFactor;
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
        #endregion configdef

        public static void InitClubConfigs(ConfigFile cfg)
        {

            ActiveSkillClub = cfg.Bind("Generic.Active", "Clubs", true,
                    AdminCD("Whether or not to activate king's skills version of the clubs skill", true)); ;

            //exp
            ClubBXPHealthFactor = cfg.Bind("Weapon.BonusExperience", "Club Stagger factor", .23f,
                AdminCD("how much each point of max health is worth in exp when staggering an enemy with clubs"));

            //effects
            ClubDamagePercentMin = cfg.Bind("Club.Effect", "Damage Min", 0f,
                AdminCD("% extra damage done with clubs at level 0"));
            ClubDamagePercentMax = cfg.Bind("Club.Effect", "Damage Max", 200f,
                AdminCD("% extra damage done with clubs at level 100"));
            ClubStaminaReduxMin = cfg.Bind("Club.Effect", "Stamina Reduction Min", 0f,
                AdminCD("% less stamina usage for clubs at level 0"));
            ClubStaminaReduxMax = cfg.Bind("Club.Effect", "Stamina Reduction Max", 50f,
                AdminCD("% less stamina usage for clubs at level 100"));
            ClubBluntPercentMin = cfg.Bind("Club.Effect", "Generic Blunt Min", 0f,
                AdminCD("% extra blunt damage to ALL weapons at level 0"));
            ClubBluntPercentMax = cfg.Bind("Club.Effect", "Generic Blunt Max", 40f,
                AdminCD("% extra blunt damage to ALL weapons at level 100"));
            ClubKnockbackPercentMin = cfg.Bind("Club.Effect", "Generic Knockback Min", 0f,
                AdminCD("% extra knockback to ALL weapons at level 0"));
            ClubKnockbackPercentMax = cfg.Bind("Club.Effect", "Generic Knockback Max", 250f,
                AdminCD("% extra knockback to ALL weapons at level 100"));
            ClubStaggerPercentMin = cfg.Bind("Club.Effect", "Generic Stagger Min", 0f,
                AdminCD("% extra stagger damage to ALL ATTACKS at level 0"));
            ClubStaggerPercentMax = cfg.Bind("Club.Effect", "Generic Stagger Max", 100f,
                AdminCD("% extra stagger damage to ALL ATTACKS at level 100"));

        }

        public static float GetClubBXPStagger(float maxHealth)
        {
            return maxHealth * ClubBXPHealthFactor.Value;
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


        #endregion club

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                             Fists
        ////////////////////////////////////////////////////////////////////////////////////////
        #region fist
        #region configdef
        public static ConfigEntry<bool> ActiveSkillFist;
        public static ConfigEntry<float> FistBXPBlockFactor;

        public static ConfigEntry<float> FistDamagePercentMin;
        public static ConfigEntry<float> FistDamagePercentMax;
        public static ConfigEntry<float> FistStaminaReduxMin;
        public static ConfigEntry<float> FistStaminaReduxMax;
        public static ConfigEntry<float> FistDamageFlatMin;
        public static ConfigEntry<float> FistDamageFlatMax;
        public static ConfigEntry<float> FistDamageFlatFactor;
        public static ConfigEntry<float> FistBlockArmorMin;
        public static ConfigEntry<float> FistBlockArmorMax;
        public static ConfigEntry<float> FistMovespeedPercentMin;
        public static ConfigEntry<float> FistMovespeedPercentMax;
        #endregion configdef

        public static void InitFistConfigs(ConfigFile cfg)
        {

            ActiveSkillFist = cfg.Bind("Generic.Active", "Fists", true,
                    AdminCD("Whether or not to activate king's skills version of the unarmed skill", true)); ;

            //exp
            FistBXPBlockFactor = cfg.Bind("Weapon.BonusExperience", "Unarmed Block", .2f,
                AdminCD("mod to multiply fist block bonus exp per damage point"));

            //effects
            FistDamagePercentMin = cfg.Bind("Fist.Effect", "Damage Min", -10f,
                AdminCD("% extra damage done with bare fists at level 0"));
            FistDamagePercentMax = cfg.Bind("Fist.Effect", "Damage Max", 160f,
                AdminCD("% extra damage done with bare fists at level 100"));
            FistStaminaReduxMin = cfg.Bind("Fist.Effect", "Stamina Reduction Min", 0f,
                AdminCD("% less stamina usage for fists at level 0"));
            FistStaminaReduxMax = cfg.Bind("Fist.Effect", "Stamina Reduction Max", 80f,
                AdminCD("% less stamina usage for fists at level 100"));
            FistDamageFlatMin = cfg.Bind("Fist.Effect", "Flat Damage Min", -3f,
                AdminCD("Flat extra damage at level 0"));
            FistDamageFlatMax = cfg.Bind("Fist.Effect", "Flat Damage Max", 32f,
                AdminCD("Flat extra damage at level 100"));
            FistDamageFlatFactor = cfg.Bind("Fist.Effect", "Flat Damage Factor", 1.65f,
                AdminCD("Curve for fist damage increase over levels"));
            FistBlockArmorMin = cfg.Bind("Fist.Effect", "Unarmed Block Armor Flat Min", -5f,
                AdminCD("Flat extra unarmed block armor at level 0"));
            FistBlockArmorMax = cfg.Bind("Fist.Effect", "Unarmed Block Armor Flat Max", 50f,
                AdminCD("Flat extra unarmed block armor at level 100"));
            FistMovespeedPercentMin = cfg.Bind("Fist.Effect", "Movespeed Min", 0f,
                AdminCD("% movespeed increase at level 0"));
            FistMovespeedPercentMax = cfg.Bind("Fist.Effect", "Movespeed Max", 65f,
                AdminCD("% movespeed increase at level 100"));
        }


        public static float GetFistBXPBlock(float damage)
        {
            return damage * FistBXPBlockFactor.Value;
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
            return Mathf.Lerp(FistDamageFlatMin.Value, FistDamageFlatMax.Value, 
                Mathf.Pow(skillFactor, FistDamageFlatFactor.Value));
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


        #endregion fist

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                             Knives
        ////////////////////////////////////////////////////////////////////////////////////////
        #region knife
        #region configdef
        public static ConfigEntry<bool> ActiveSkillKnife;
        public static ConfigEntry<float> WeaponBXPKnifeBackstab;
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
        #endregion configdef

        public static void InitKnifeConfigs(ConfigFile cfg)
        {
            ActiveSkillKnife = cfg.Bind("Generic.Active", "Knives", true,
                    AdminCD("Whether or not to activate king's skills version of the knives skill", true)); ;

            //exp
            WeaponBXPKnifeBackstab = cfg.Bind("Weapon.BonusExperience", "Knife Backstab", 25f,
                AdminCD("Flat BXP gained every time you get a sneak attack using the knife."));

            //Knives
            KnifeDamagePercentMin = cfg.Bind("Knife.Effect", "Damage Min", 0f,
                AdminCD("% extra damage done with knives at level 0"));
            KnifeDamagePercentMax = cfg.Bind("Knife.Effect", "Damage Max", 200f,
                AdminCD("% extra damage done with knives at level 100"));
            KnifeStaminaReduxMin = cfg.Bind("Knife.Effect", "Stamina Reduction Min", 0f,
                AdminCD("% less stamina usage for knives at level 0"));
            KnifeStaminaReduxMax = cfg.Bind("Knife.Effect", "Stamina Reduction Max", 65f,
                AdminCD("% less stamina usage for knives at level 100"));
            KnifeBackstabPercentMin = cfg.Bind("Knife.Effect", "Backstab Min", 0f,
                AdminCD("% extra sneak attack damage with ALL weapons at level 0"));
            KnifeBackstabPercentMax = cfg.Bind("Knife.Effect", "Backstab Max", 150f,
                AdminCD("% extra sneak attack damage with ALL weapons at level 100"));
            KnifeMovespeedPercentMin = cfg.Bind("Knife.Effect", "Movementspeed Min", 0f,
                AdminCD("% movespeed increase at level 0"));
            KnifeMovespeedPercentMax = cfg.Bind("Knife.Effect", "Movementspeed Max", 45f,
                AdminCD("% movespeed increase at level 100"));
            KnifePiercePercentMin = cfg.Bind("Knife.Effect", "Generic Pierce Min", 0f,
                AdminCD("% extra pierce damage with ALL weapons at level 0"));
            KnifePiercePercentMax = cfg.Bind("Knife.Effect", "Generic Pierce Max", 50f,
                AdminCD("% extra pierce damage with ALL weapons at level 0"));
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

        #endregion knife

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                             Polearms
        ////////////////////////////////////////////////////////////////////////////////////////
        #region polearm
        #region configdef
        public static ConfigEntry<bool> ActiveSkillPolearm;
        public static ConfigEntry<float> WeaponBXPPolearmDamageMod;
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
        #endregion configdef

        public static void InitPolearmConfigs(ConfigFile cfg)
        {
            ActiveSkillPolearm = cfg.Bind("Generic.Active", "Polearms", true,
                    AdminCD("Whether or not to activate king's skills version of the polearms skill", true)); ;

            //exp
            WeaponBXPPolearmDamageMod = cfg.Bind("Weapon.BonusExperience", "Polearm Damage Mod", .5f,
                AdminCD("amount of bonus experience to get for each damage blocked by armor."));

            //effects
            PolearmDamagePercentMin = cfg.Bind("Polearm.Effect", "Damage Min", 0f,
                AdminCD("% extra damage done with polearms at level 0"));
            PolearmDamagePercentMax = cfg.Bind("Polearm.Effect", "Damage Max", 150f,
                AdminCD("% extra damage done with polearms at level 100"));
            PolearmStaminaReduxMin = cfg.Bind("Polearm.Effect", "Stamina Reduction Min", 0f,
                AdminCD("% less stamina usage for polearms at level 0"));
            PolearmStaminaReduxMax = cfg.Bind("Polearm.Effect", "Stamina Reduction Max", 70f,
                AdminCD("% less stamina usage for polearms at level 100"));
            PolearmRangeMin = cfg.Bind("Polearm.Effect", "Generic Range Min", 0f,
                AdminCD("Added units of range to all weapon attacks at level 0"));
            PolearmRangeMax = cfg.Bind("Polearm.Effect", "Generic Range Max", 1f,
                AdminCD("Added units of range to all weapon attacks at level 100"));
            PolearmArmorMin = cfg.Bind("Polearm.Effect", "Armor Flat Min", 0f,
                AdminCD("Flat armor added to character at level 0"));
            PolearmArmorMax = cfg.Bind("Polearm.Effect", "Armor Flat Max", 45f,
                AdminCD("Flat armor added to character at level 100"));
            PolearmBlockMin = cfg.Bind("Polearm.Effect", "Block Min", 0f,
                AdminCD("Flat block armor added to polearms at level 0"));
            PolearmBlockMax = cfg.Bind("Polearm.Effect", "Block Max", 43f,
                AdminCD("Flat block armor added to polearms at level 100"));

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


        #endregion polearm

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                             Spears
        ////////////////////////////////////////////////////////////////////////////////////////
        #region spear
        #region configdef
        public static ConfigEntry<bool> ActiveSkillSpear;
        public static ConfigEntry<float> SpearBXPDistanceFactor;
        public static ConfigEntry<float> SpearBXPDistanceMod;

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
        #endregion configdef

        public static void InitSpearConfigs(ConfigFile cfg)
        {

            ActiveSkillSpear = cfg.Bind("Generic.Active", "Spear", true,
                    AdminCD("Whether or not to activate king's skills version of the spear skill", true)); ;

            //exp
            SpearBXPDistanceFactor = cfg.Bind("Weapon.BonusExperience", "Spear Distance Factor", 1.12f,
                AdminCD("Factor to define the scale of the distance curve"));
            SpearBXPDistanceMod = cfg.Bind("Weapon.BonusExperience", "Spear Distance Mod", .5f,
                AdminCD("how much each point of distance is worth base for distance spear bxp"));

            //effect
            SpearDamagePercentMin = cfg.Bind("Spear.Effect", "Damage Min", 0f,
                AdminCD("% extra damage done with spears at level 0"));
            SpearDamagePercentMax = cfg.Bind("Spear.Effect", "Damage Max", 200f,
                AdminCD("% extra damage done with spears at level 100"));
            SpearStaminaReduxMin = cfg.Bind("Spear.Effect", "Stamina Reduction Min", 0f,
                AdminCD("% less stamina usage for spears at level 0"));
            SpearStaminaReduxMax = cfg.Bind("Spear.Effect", "Stamina Reduction Max", 70f,
                AdminCD("% less stamina usage for spears at level 100"));
            SpearVelocityPercentMin = cfg.Bind("Spear.Effect", "Thrown Velocity Min", 0f,
                AdminCD("% extra velocity on thrown weapons at level 0"));
            SpearVelocityPercentMax = cfg.Bind("Spear.Effect", "Thrown Velocity Max", 300f,
                AdminCD("% extra velocity on thrown weapons at level 100"));
            SpearProjectileDamagePercentMin = cfg.Bind("Spear.Effect", "Thrown Damage Percent Min", 0f,
                AdminCD("% extra damage done with thrown weapons at level 0"));
            SpearProjectilePercentMax = cfg.Bind("Spear.Effect", "Thrown Damage Percent Max", 200f,
                AdminCD("% extra damage done with thrown weapons at level 100"));
            SpearBlockArmorMin = cfg.Bind("Spear.Effect", "Generic Block Armor Min", 0f,
                AdminCD("Flat block armor always applied at level 0"));
            SpearBlockArmorMax = cfg.Bind("Spear.Effect", "Generic Block Armor Max", 40f,
                AdminCD("Flat block armor always applied at level 100"));
        }

        public static float GetSpearBXPDistance(float distance)
        {
            return Mathf.Pow(SpearBXPDistanceMod.Value * distance, SpearBXPDistanceFactor.Value);
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

        #endregion spear

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                             Swords
        ////////////////////////////////////////////////////////////////////////////////////////
        #region sword
        #region configdef
        public static ConfigEntry<bool> ActiveSkillSword;
        public static ConfigEntry<float> WeaponBXPSwordStagger;
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
        #endregion configdef

        public static void InitSwordConfigs(ConfigFile cfg)
        {

            ActiveSkillSword = cfg.Bind("Generic.Active", "Swords", true,
                    AdminCD("Whether or not to activate king's skills version of the swords skill", true)); ;
            //exp
            WeaponBXPSwordStagger = cfg.Bind("Weapon.BonusExperience", "Sword Parry Hit", 3f,
                AdminCD("Flat BXP gained every time you hit a staggered enemy with a sword"));


            //effects
            SwordDamagePercentMin = cfg.Bind("Sword.Effect", "Damage Min", 0f,
                AdminCD("% extra damage done with swords at level 0"));
            SwordDamagePercentMax = cfg.Bind("Sword.Effect", "Damage Max", 200f,
                AdminCD("% extra damage done with swords at level 100"));
            SwordStaminaReduxMin = cfg.Bind("Sword.Effect", "Stamina Reduction Min", 0f,
                AdminCD("% less stamina usage for swords at level 0"));
            SwordStaminaReduxMax = cfg.Bind("Sword.Effect", "Stamina Reduction Max", 60f,
                AdminCD("% less stamina usage for swords at level 100"));
            SwordParryPercentMin = cfg.Bind("Sword.Effect", "Generic Parry Min", 0f,
                AdminCD("% extra parry bonus for ALL weapons at level 0"));
            SwordParryPercentMax = cfg.Bind("Sword.Effect", "Generic Parry Max", 100f,
                AdminCD("% extra parry bonus for ALL weapons at level 100"));
            SwordSlashPercentMin = cfg.Bind("Sword.Effect", "Generic Slash Min", 0f,
                AdminCD("% extra slash damage for ALL weapons at level 0"));
            SwordSlashPercentMax = cfg.Bind("Sword.Effect", "Generic Slash Max", 65f,
                AdminCD("% extra slash damage for ALL weapons at level 100"));
            SwordDodgeStaminaReduxMin = cfg.Bind("Sword.Effect", "Dodgeroll Stamina Reduction Min", 0f,
                AdminCD("% less stamina cost to dodge roll at level 0"));
            SwordDodgeStaminaReduxMax = cfg.Bind("Sword.Effect", "Dodgeroll Stamina Reduction Max", 40f,
                AdminCD("% less stamina cost to dodge roll at level 0"));
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

        #endregion sword

        #endregion weapons

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                    Skills
        ////////////////////////////////////////////////////////////////////////////////////////
        #region skills

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                             Agriculture
        ////////////////////////////////////////////////////////////////////////////////////////
        #region botany
        #region configdef
        public static ConfigEntry<bool> ActiveSkillBotany;
        public static ConfigEntry<float> AgricultureXPPlantFlat;
        public static ConfigEntry<float> AgricultureYieldMin;
        public static ConfigEntry<float> AgricultureYieldMax;
        public static ConfigEntry<float> AgricultureGrowReduxMin;
        public static ConfigEntry<float> AgricultureGrowReduxMax;
        public static ConfigEntry<float> AgricultureAvgFQMin;
        public static ConfigEntry<float> AgricultureAvgFQMax;
        public static ConfigEntry<float> AgricultureFQRangeMin;
        public static ConfigEntry<float> AgricultureFQRangeMax;
        public static ConfigEntry<float> AgricultureHealthRegainMin;
        public static ConfigEntry<float> AgricultureHealthRegainMax;

        public static Dictionary<string, float> AgriculturePickableRewards;
        public static Dictionary<string, float> AgricultureTreeGrowRewards;
        #endregion configdef

        private static void InitBotanyConfig(ConfigFile cfg)
        {
            ActiveSkillBotany = cfg.Bind("Generic.Active", "Agriculture", true,
                    AdminCD("Whether or not to allow King's agriculture"));

            //list of plant experience bounties here
            AgriculturePickableRewards = new Dictionary<string, float>();
            AgricultureTreeGrowRewards = new Dictionary<string, float>();

            AgriculturePickableRewards.Add("Raspberry", 0.3f);
            AgriculturePickableRewards.Add("Blueberries", 1.8f);
            AgriculturePickableRewards.Add("Cloudberry", 3.4f);
            AgriculturePickableRewards.Add("Mushroom", 0.5f);
            AgriculturePickableRewards.Add("Dandelion", 0.8f);
            AgriculturePickableRewards.Add("Thistle", 3.5f);
            AgriculturePickableRewards.Add("CarrotSeeds", 2f);
            AgriculturePickableRewards.Add("Carrot", 8f);
            AgriculturePickableRewards.Add("MushroomYellow", 2.5f);
            AgriculturePickableRewards.Add("TurnipSeeds", 8f);
            AgriculturePickableRewards.Add("Turnip", 15f);
            AgriculturePickableRewards.Add("OnionSeeds", 10f);
            AgriculturePickableRewards.Add("Onion", 20f);
            AgriculturePickableRewards.Add("FlaxW", 15f);
            AgriculturePickableRewards.Add("Flax", 30f);
            AgriculturePickableRewards.Add("BarleyW", 15f);
            AgriculturePickableRewards.Add("Barley", 30f);
            AgriculturePickableRewards.Add("MushroomBlue", 200f);

            AgricultureTreeGrowRewards.Add("$prop_fir_sapling", 8f);
            AgricultureTreeGrowRewards.Add("$prop_beech_sapling", 12f);
            AgricultureTreeGrowRewards.Add("$prop_birch_sapling", 25f);
            AgricultureTreeGrowRewards.Add("$prop_pine_sapling", 45f);
            AgricultureTreeGrowRewards.Add("$prop_oak_sapling", 100f);

            //exp
            AgricultureXPPlantFlat = cfg.Bind("Agriculture.Experience", "Plant", 1f,
            AdminCD("Amount of experience gained per plant planted"));

            //effects
            AgricultureYieldMin = cfg.Bind("Agriculture.Effect", "Yield Min", 0f,
            AdminCD("% increase to yield of plants at level 0"));
            AgricultureYieldMax = cfg.Bind("Agriculture.Effect", "Yield Max", 450f,
            AdminCD("% increase to yield of plants at level 100"));
            AgricultureGrowReduxMin = cfg.Bind("Agriculture.Effect", "Grow Time Reduction Min", -10f,
            AdminCD("% less time on plant grow timers after you pick or plant at level 0"));
            AgricultureGrowReduxMax = cfg.Bind("Agriculture.Effect", "Grow Time Reduction Max", 85f,
            AdminCD("% less time on plant grow timers after you pick or plant at level 100"));
            AgricultureAvgFQMin = cfg.Bind("Agriculture.Effect", "Food Quality Min", -10f,
            AdminCD("% average food quality of harvested plants at level 0"));
            AgricultureAvgFQMax = cfg.Bind("Agriculture.Effect", "Food Quality Max", 60f,
            AdminCD("% average food quality of harvested plants at level 100"));
            AgricultureFQRangeMin = cfg.Bind("Agriculture.Effect", "Food Quality Range Min", 20f,
            AdminCD("spread of possible food quality values at level 0"));
            AgricultureFQRangeMax = cfg.Bind("Agriculture.Effect", "Food Quality Range Max", 50f,
            AdminCD("spread of possible food quality values at level 100"));
            AgricultureHealthRegainMin = cfg.Bind("Agriculture.Effect", "Health Regain Min", 0f,
            AdminCD("Amount of health regained each time you harvest a plant at level 0"));
            AgricultureHealthRegainMax = cfg.Bind("Agriculture.Effect", "Health Regain Max", 10f,
            AdminCD("Amount of health regained each time you harvest a plant at levle 100"));

        }

        //Returns 0 if object not in list of rewards
        public static float GetAgriculturePlantReward(GameObject pickableObj)
        {
            if (pickableObj == null) return 0f;
            Pickable pick = pickableObj.GetComponent<Pickable>();
            if (pick == null) return 0f;
            
            string name = pick.m_itemPrefab.name;
            if (pick.name.Contains("Wild")) name += "W";
            if (AgriculturePickableRewards.ContainsKey(name))
                return AgriculturePickableRewards[name];

            return 0f;
        }
        public static float GetAgricultureTreeReward(Plant plant)
        {
            if (plant == null) return 0f;

            string name = plant.m_name;
            Jotunn.Logger.LogMessage(name);

            if (AgricultureTreeGrowRewards.ContainsKey(name))
                return AgricultureTreeGrowRewards[name];

            return 0f;

        }
        public static bool GetAgricultureIsPlant(Pickable item)
        {
            if (item == null) return false;
            string name = item.m_itemPrefab.name;
            if (item.name.Contains("Wild")) name += "W";
            if (AgriculturePickableRewards.ContainsKey(name))
                return true;

            return false;
        }
        public static float GetAgricultureAverageFoodQualityMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMod(AgricultureAvgFQMin),
                PerToMod(AgricultureAvgFQMax), skillFactor);
        }
        public static float GetAgricultureFoodQualityRangeMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMod(AgricultureFQRangeMin),
                PerToMod(AgricultureFQRangeMax), skillFactor);
        }
        //Timing should be a number between 0 and 1 that reflects how close to perfect
        //the timing was, 1 being right on the money
        public static float GetAgricultureRandomFQ(float skillFactor)
        {
            float baseQ = GetAgricultureAverageFoodQualityMod(skillFactor);
            float range = GetAgricultureFoodQualityRangeMod(skillFactor) / 2;
            float randomFactor = UnityEngine.Random.Range(0f, 1f);


            float qualityChange = Mathf.Lerp(-range, range, randomFactor);


            float newQuality = Mathf.Clamp(baseQ + qualityChange, -1, 10);

            newQuality = (float)(Mathf.Round(newQuality * 100f) / 100f);

            return newQuality;
        }
        public static float GetAgricultureYieldMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMod(AgricultureYieldMin), 
                PerToMod(AgricultureYieldMax), skillFactor);
        }
        public static int GetAgricultureRandomAdditionalYield(float skillFactor, int baseYield, float nonRand = 10f)
        {
            float rand;
            if (nonRand == 10f) rand = UnityEngine.Random.Range(0f, 1f);
            else rand = nonRand;

            float yieldMult = GetAgricultureYieldMod(skillFactor);
            float bestCase = baseYield * yieldMult;
            float modifier = Mathf.Lerp(-yieldMult / 2, yieldMult / 2, rand);

            //Jotunn.Logger.LogMessage($"Our extra yield will be multiplied by {yieldMult}" +
            //    $"\nOur base case scenario, since the base is {baseYield}, is {bestCase}" +
            //    $"\nthe modifier, or half the extra yield, ended up as {modifier}" +
            //    $"\nwe're returning {Mathf.FloorToInt(bestCase + modifier)}"));
            if (modifier - Mathf.Floor(modifier) > 0.5f) modifier++;

            return Mathf.FloorToInt(bestCase + modifier);
        }
        public static float GetAgricultureGrowTimeRedux(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(AgricultureGrowReduxMin, true), 
                PerToMult(AgricultureGrowReduxMax, true), skillFactor);
        }
        public static float GetAgricultureFoodQualityMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(AgricultureAvgFQMin), 
                PerToMult(AgricultureAvgFQMax), skillFactor);
        }
        public static float GetAgricultureHealthRegain(float skillFactor)
        {
            return Mathf.Lerp(AgricultureHealthRegainMin.Value, 
                AgricultureHealthRegainMax.Value, skillFactor);
        }
        //public static float Get(float skillFactor)
        //{
        //    return Mathf.Lerp(PerToMult(), PerToMult(), skillFactor);
        //}

        #endregion botany

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                             Blocking
        ////////////////////////////////////////////////////////////////////////////////////////
        #region block
        #region configdef
        public static ConfigEntry<bool> ActiveSkillBlock;
        public static ConfigEntry<float> BlockXPPercent;
        public static ConfigEntry<float> BlockXPParryPercent;
        public static ConfigEntry<float> BlockFlatPowerMin;
        public static ConfigEntry<float> BlockFlatPowerMax;
        public static ConfigEntry<float> BlockPowerPercentMin;
        public static ConfigEntry<float> BlockPowerPercentMax;
        public static ConfigEntry<float> BlockStaminaReduxMin;
        public static ConfigEntry<float> BlockStaminaReduxMax;
        public static ConfigEntry<float> BlockHealthPerLevel;
        #endregion configdef

        public static void InitBlockConfigs(ConfigFile cfg)
        {
            ActiveSkillBlock = cfg.Bind("Generic.Active", "Blocking", true,
                    AdminCD("Whether or not to activate king's skills version of the blocking skill", true)); ;

            //experience
            BlockXPPercent = cfg.Bind("Block.Experience", "XP", 15f,
                AdminCD("% of damage blocked that turns into experience"));
            BlockXPParryPercent = cfg.Bind("Block.Experience", "Parry", 200f,
                AdminCD("% extra experience earned when parrying an attack"));

            //effects
            BlockFlatPowerMin = cfg.Bind("Block.Effect", "Block Armor Flat Min", 0f,
                AdminCD("This flat value is added to block armor at level 0"));
            BlockFlatPowerMax = cfg.Bind("Block.Effect", "Block Armor Flat Max", 50f,
                AdminCD("This flat value is added to block armor at level 100"));
            BlockPowerPercentMin = cfg.Bind("Block.Effect", "Block Armor Min", -25f,
                AdminCD("% change in total block armor at level 0"));
            BlockPowerPercentMax = cfg.Bind("Block.Effect", "Block Armor Max", 100f,
                AdminCD("% change in total block armor at level 100"));
            BlockStaminaReduxMin = cfg.Bind("Block.Effect", "Stamina Reduction Min", -10f,
                AdminCD("% less stamina to block at level 0"));
            BlockStaminaReduxMax = cfg.Bind("Block.Effect", "Stamina Reduction Max", 50f,
                AdminCD("% less stamina to block at level 100"));
            BlockHealthPerLevel = cfg.Bind("Block.Effect", "Health", .8f,
                AdminCD("flat increase to max health per level of block"));
        }


        public static float GetVanillaBlockMult(float skillFactor)
        {
            return 1f + skillFactor * .5f;
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

        #endregion block

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                             Building
        ////////////////////////////////////////////////////////////////////////////////////////
        #region build
        #region configdef
        public static ConfigEntry<bool> ActiveSkillBuild;
        public static ConfigEntry<float> BuildXPPerPiece;
        public static ConfigEntry<float> BuildXPRepairMod;
        public static ConfigEntry<float> BuildXPDamageTakenMod;
        public static ConfigEntry<float> BuildXPDamageDoneMod;
        public static ConfigEntry<float> BuildHealthMin;
        public static ConfigEntry<float> BuildHealthMax;
        public static ConfigEntry<float> BuildStabilityMin;
        public static ConfigEntry<float> BuildStabilityMax;
        public static ConfigEntry<float> BuildStabilityLossReduxMin;
        public static ConfigEntry<float> BuildStabilityLossReduxMax;
        public static ConfigEntry<float> BuildDamageMin;
        public static ConfigEntry<float> BuildDamageMax;
        public static ConfigEntry<float> BuildWNTReduxMin;
        public static ConfigEntry<float> BuildWNTReduxMax;
        public static ConfigEntry<float> BuildFreeChanceMin;
        public static ConfigEntry<float> BuildFreeChanceMax;
        public static ConfigEntry<float> BuildFreeChanceMinLevel;
        public static ConfigEntry<float> BuildFreeChanceFactor;
        public static ConfigEntry<float> BuildStaminaReduxMin;
        public static ConfigEntry<float> BuildStaminaReduxMax;
        #endregion configdef

        private static void InitBuildConfig(ConfigFile cfg)
        {
            ActiveSkillBuild = cfg.Bind("Generic.Active", "Building", true,
                    AdminCD("Whether or not to allow King's building"));

            BuildXPPerPiece = cfg.Bind("Build.Experience", "Per Piece", 2.2f,
                    AdminCD("How much experience for placed build piece"));
            BuildXPRepairMod = cfg.Bind("Build.Experience", "Repair Mod", .01f,
                    AdminCD("Amount of experience for each point of repaired damage"));
            BuildXPDamageTakenMod = cfg.Bind("Build.Experience", "Damage Taken Mod", .2f,
                    AdminCD("Amount of experience for each point of damage taken by buildings"));
            BuildXPDamageDoneMod = cfg.Bind("Build.Experience", "Damage Done Mod", 1f,
                    AdminCD("Amount of experience for each point of damage done by buildings"));

            BuildHealthMin = cfg.Bind("Build.Effect", "Health Min", 0f,
                    AdminCD("% increase to building health at level 0"));
            BuildHealthMax = cfg.Bind("Build.Effect", "Health Max", 200f,
                    AdminCD("% increase to building health at level 100"));
            BuildStabilityMin = cfg.Bind("Build.Effect", "Stability Min", -15f,
                    AdminCD("% increase to building stability at level 0"));
            BuildStabilityMax = cfg.Bind("Build.Effect", "Stability Max", 100f,
                    AdminCD("% increase to building stability at level 100"));
            BuildStabilityLossReduxMin = cfg.Bind("Build.Effect", "Stability Loss Reduction Min", -10f,
                    AdminCD("% reduction to loss of support at level 0"));
            BuildStabilityLossReduxMax = cfg.Bind("Build.Effect", "Stability Loss Reduction  Max", 45f,
                    AdminCD("% reduction to loss of support at level 100"));
            BuildDamageMin = cfg.Bind("Build.Effect", "Damage Min", 0f,
                    AdminCD("% increase to building damage at level 0"));
            BuildDamageMax = cfg.Bind("Build.Effect", "Damage Max", 400f,
                    AdminCD("% increase to building damage at level 0"));
            BuildWNTReduxMin = cfg.Bind("Build.Effect", "WNT Redux Min", 0f,
                    AdminCD("% less damage taken by buildings from wear and tear at level 0"));
            BuildWNTReduxMax = cfg.Bind("Build.Effect", "WNT Redux Max", 75f,
                    AdminCD("% less damage taken by buildings from wear and tear at level 100"));
            BuildFreeChanceMin = cfg.Bind("Build.Effect", "Free Chance Min", 0f,
                    AdminCD("% chance to build a piece for free at minimum level"));
            BuildFreeChanceMax = cfg.Bind("Build.Effect", "Free Chance Max", 35f,
                    AdminCD("% chance to build a piece for free at level 100"));
            BuildFreeChanceFactor = cfg.Bind("Build.Effect", "Free Chance Factor", 2.5f,
                    AdminCD("Factor for defining the slope of the free chance curve."));
            BuildFreeChanceMinLevel = cfg.Bind("Build.Effect", "Free Chance Level", 20f,
                    AdminCD("Smallest level at which free chance curve begins"));
            BuildStaminaReduxMin = cfg.Bind("Build.Effect", "Stamina Redux Min", -20f,
                    AdminCD("% reduction in stamina costs at level 0"));
            BuildStaminaReduxMax = cfg.Bind("Build.Effect", "Stamina Redux Max", 82f,
                    AdminCD("% reduction in stamina costs at level 100"));
        }



        public static float GetBuildingHealthMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(BuildHealthMin), 
                PerToMult(BuildHealthMax), skillFactor);
        }
        public static float GetBuildingStabilityMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(BuildStabilityMin), 
                PerToMult(BuildStabilityMax), skillFactor);
        }
        public static float GetBuildingStabilityLossRedux(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(BuildStabilityLossReduxMin, true),
                PerToMult(BuildStabilityLossReduxMax), skillFactor);
        }
        public static float GetBuildingDamageMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(BuildDamageMin), 
                PerToMult(BuildDamageMax), skillFactor);
        }
        public static float GetBuildingWNTRedux(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(BuildWNTReduxMin, true), 
                PerToMult(BuildWNTReduxMax, true), skillFactor);
        }
        public static float GetFCMinLevelAsMod()
        {
            return BuildFreeChanceMinLevel.Value / MaxSkillLevel.Value;
        }
        public static float GetBuildingFreeMod(float skillFactor)
        {
            float minLevel = GetFCMinLevelAsMod();
            float newSkillFactor = Mathf.Clamp01((skillFactor - minLevel) / (MaxSkillLevel.Value - minLevel));

            //This gets an exponential curve that doesn't start until the minimum level, supposedly
            return Mathf.Lerp(PerToMod(BuildFreeChanceMin), PerToMod(BuildFreeChanceMax), 
                Mathf.Pow(newSkillFactor, BuildFreeChanceFactor.Value));
        }
        public static bool GetBuildingRandomFreeChance(float skillFactor)
        {
            float rand = UnityEngine.Random.Range(0f, 1f);
            float skill = GetBuildingFreeMod(skillFactor);
            //Jotunn.Logger.LogMessage($"rolled a random number of {rand} against {skill}"));
            return rand < skill;
        }
        public static float GetBuildingStaminaRedux(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(BuildStaminaReduxMin, true), 
                PerToMult(BuildStaminaReduxMax, true), skillFactor);
        }
        /*
        public static float Get(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(), PerToMult(), skillFactor);
        }
        public static float Get(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(), PerToMult(), skillFactor);
        }
        */

        #endregion build

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                             Cooking
        ////////////////////////////////////////////////////////////////////////////////////////
        #region cook
        #region configdef
        public static ConfigEntry<bool> ActiveSkillCook;
        public static ConfigEntry<float> CookingXPStart;
        public static ConfigEntry<float> CookingXPFinish;
        public static ConfigEntry<float> CookingXPTierBonus;
        public static ConfigEntry<float> CookingXPStatMod;
        public static ConfigEntry<float> CookingXPCustomerBonus;
        public static ConfigEntry<float> CookingAverageFQMin;
        public static ConfigEntry<float> CookingAverageFQMax;
        public static ConfigEntry<float> CookingFQRangeMin;
        public static ConfigEntry<float> CookingFQRangeMax;
        public static ConfigEntry<float> CookingFQRangeTimingPercent;
        public static ConfigEntry<float> CookingFQRangeFactor;
        public static ConfigEntry<float> CookingTimeReduxMin;
        public static ConfigEntry<float> CookingTimeReduxMax;
        public static ConfigEntry<float> CookingTimeGuessMinLevel;
        public static ConfigEntry<float> CookingTimeGuessMin;
        public static ConfigEntry<float> CookingTimeGuessMax;
        public static ConfigEntry<float> CookingFermentTimeReduxMin;
        public static ConfigEntry<float> CookingFermentTimeReduxMax;

        public static Dictionary<string, float> CookingStationTiers;
        public static Dictionary<string, float> CookingStationLevelRQs;

        public const string ColorCookHealthFF = "<color=#F9582EFF>";
        public const string ColorCookStaminaFF = "<color=#F9DE2EFF>";
        public const string ColorCookDurationFF = "<color=#E9E9E9ff>";
        public const float MinimumFQ = -2f;
        public const float MaximumFQ = 50f;
        #endregion configdef

        private static void InitCookConfig(ConfigFile cfg)
        {
            ActiveSkillCook = cfg.Bind("Generic.Active", "Cooking", true,
                    AdminCD("Whether or not to allow King's cooking"));


            CookingXPStart = cfg.Bind("Cooking.Experience", "Start", 1.5f,
                    AdminCD("How much experience for a started project"));
            CookingXPFinish = cfg.Bind("Cooking.Experience", "Finish", 3f,
                    AdminCD("How much experience for successfully cooking a project"));
            CookingXPTierBonus = cfg.Bind("Cooking.Experience", "Tier Bonus", 50f,
                    AdminCD("% increase in exp gained per tier level of cooking machine"));
            CookingXPStatMod = cfg.Bind("Cooking.Experience", "Stat Mod", .08f,
                    AdminCD("Amount of experience gained per health and stamina of eaten food."));
            CookingXPCustomerBonus = cfg.Bind("Cooking.Experience", "Customer bonus", 100f,
                    AdminCD("% extra experience when your food is eaten by another person."));

            CookingStationTiers = new Dictionary<string, float>();
            CookingStationLevelRQs = new Dictionary<string, float>();

            CookingStationTiers.Add("piece_cookingstation", 0);
            CookingStationTiers.Add("piece_cauldron", 1);
            CookingStationTiers.Add("piece_cookingstation_iron", 2);
            CookingStationTiers.Add("piece_oven", 3);
            CookingStationTiers.Add("piece_fermenter", 4);

            CookingStationLevelRQs.Add("piece_cookingstation", 0);
            CookingStationLevelRQs.Add("piece_cauldron", 20);
            CookingStationLevelRQs.Add("piece_cookingstation_iron", 35);
            CookingStationLevelRQs.Add("piece_oven", 50);
            CookingStationLevelRQs.Add("piece_fermenter", 44);

            CookingAverageFQMin = cfg.Bind("Cooking.Effect", "Average FQ Min", 0f,
                    AdminCD("% of average food quality at level 0"));
            CookingAverageFQMax = cfg.Bind("Cooking.Effect", "Average FQ Max", 150f,
                    AdminCD("% of average food quality at level 100"));
            CookingFQRangeMin = cfg.Bind("Cooking.Effect", "FQ Range Min", 60f,
                    AdminCD("% range of possible food quality values at level 0"));
            CookingFQRangeMax = cfg.Bind("Cooking.Effect", "FQ Range Max", 20f,
                    AdminCD("% range of possible food quality values at level 100"));
            CookingFQRangeTimingPercent = cfg.Bind("Cooking.Effect", "FQ Range From Timing", 50f,
                    AdminCD("% of the range that can be controlled with timing. the rest is random based on skill level"));
            CookingFQRangeFactor = cfg.Bind("Cooking.Effect", "Cook! FQ Range Factor", 0.67f,
                    AdminCD("factor for defining the 1-x squared parabola of the quality calcs"));

            CookingTimeReduxMin = cfg.Bind("Cooking.Effect", "Time Redux Min", -20f,
                    AdminCD("% reduction in cooking times at level 0"));
            CookingTimeReduxMax = cfg.Bind("Cooking.Effect", "Time Redux Max", 70f,
                    AdminCD("% reduction in cooking times at level 100"));
            CookingTimeGuessMinLevel = cfg.Bind("Cooking.Effect", "Time Guess Min Level", 20f,
                    AdminCD("Level at which you start being able to see how long food will take"));
            CookingTimeGuessMin = cfg.Bind("Cooking.Effect", "Time Guess Min", 50f,
                    AdminCD("% the represents your uncertainty in your ability to guess how long food will take at minimum level"));
            CookingTimeGuessMax = cfg.Bind("Cooking.Effect", "Time Guess Max", 0f,
                    AdminCD("% the represents your uncertainty in your ability to guess how long food will take at maximum level"));
            CookingFermentTimeReduxMin = cfg.Bind("Cooking.Effect", "Fermentation Redux Min", 0f,
                    AdminCD("% reduction in fermentation times at level 0"));
            CookingFermentTimeReduxMax = cfg.Bind("Cooking.Effect", "Fermentation Redux Max", 85f,
                    AdminCD("% reduction in fermentation times at level 100"));

        }


        public static float GetCookingXP(string cook, bool start = true)
        {
            float XP = CookingXPFinish.Value;
            if (start)
                XP = CookingXPStart.Value;

            return XP * (1 + GetCookingXPTier(cook) * PerToMod(CookingXPTierBonus));
        }
        public static float GetCookingLevelRQ(string station)
        {
            if (CookingStationLevelRQs.ContainsKey(station)) return CookingStationLevelRQs[station];

            return 0f;
        }
        public static float GetCookingXPTier(string cook)
        {
            //Jotunn.Logger.LogMessage($"Comparing {cook}"));
            if (CookingStationTiers.ContainsKey(cook))
                return CookingStationTiers[cook];

            return 0f;
        }
        public static float GetCookingXPCustomerMult()
        {
            return PerToMult(CookingXPCustomerBonus);
        }
        public static float GetCookingAverageFoodQualityMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMod(CookingAverageFQMin), 
                PerToMod(CookingAverageFQMax), skillFactor);
        }
        public static float GetCookingFoodQualityRangeMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMod(CookingFQRangeMin), 
                PerToMod(CookingFQRangeMax), skillFactor);
        }
        //Timing should be a number between 0 and 1 that reflects how close to perfect
        //the timing was, 1 being right on the money
        public static float GetCookingTimingRandomFQ(float skillFactor, float timingPercent)
        {
            float baseQ = GetCookingAverageFoodQualityMod(skillFactor);
            float range = GetCookingFoodQualityRangeMod(skillFactor)/2;
            float timingMod = PerToMod(CookingFQRangeTimingPercent);
            float randomFactor = UnityEngine.Random.Range(0f, 1f);

            //the change to the overall quality is based (timingPercent)% on the timing,
            //and the rest on random chance.
            //at this point the negatives no longer serve us, so we're getting rid of them
            float timingAndRandom = (Mathf.Abs(timingPercent) * timingMod) + 
                (randomFactor * (1f - timingMod));

            //then we parabolize.
            float curvedQualityChange = Mathf.Pow(timingAndRandom, CookingFQRangeFactor.Value);

            if (curvedQualityChange < 0.02) curvedQualityChange = 0f;

            //This creates a nice U parabola. but, for it to be useful, we actually need it inverted
            float goodCurve = 1f - curvedQualityChange;


            //finally, this will give us a nice standard deviation type parabola where it's a little easier to
            //get middle values than otherwise.

            //Jotunn.Logger.LogMessage($"Since the effect of Timing was {timingPercent.ToString("F2")} and the effect of randomness was {randomFactor.ToString("F2")}," +
            //    $"the final result is a combination at {timingAndRandom.ToString("F2")}"));
            //Jotunn.Logger.LogMessage($"Applying our curve gives us {curvedQualityChange.ToString("F2")}, and then inversing it gives us " +
            //    $"{goodCurve.ToString("F2")}"));

            //(timingPercent * timingMod) + randomFactor * (1f - timingMod)
            //Jotunn.Logger.LogMessage($"Base quality is {baseQ}, and half of the range is {range}"));
            //Jotunn.Logger.LogMessage($"The random quality was {randomFactor}"));
            //Jotunn.Logger.LogMessage($"I clamped timing, and now it is {timing}"));


            float qualityChange = Mathf.Lerp(-range, range, goodCurve);

            qualityChange = Mathf.Clamp(baseQ + qualityChange, MinimumFQ, MaximumFQ);

            qualityChange = (float)(Mathf.Round(qualityChange * 100f) / 100f);

            return qualityChange;
        }

        public static float GetCookingRandomFQ(float skillFactor)
        {
            //yeah, I mean, it's stupid. but fuck you
            return GetCookingTimingRandomFQ(skillFactor, UnityEngine.Random.Range(-1f, 1f));
        }

        public static float GetCookingTimingPercent(float cookingFinishedTime, float timeToCook)
        {
            float cookingPeakTime = timeToCook * 1.5f;

            //Jotunn.Logger.LogMessage($"meat finishes at {timeToCook.ToString("F1")}, it was finished at " +
            //    $"{cookingFinishedTime.ToString("F1")}, and it would have been perfect at " +
            //    $"{cookingPeakTime.ToString("F1")}.");

            float timeP = (cookingFinishedTime - cookingPeakTime) / (timeToCook * 2 - cookingPeakTime);

            //Jotunn.Logger.LogMessage($"The resulting time percent is thus {timeP.ToString("F3")}");

            return timeP;

            // 10,   15,   20
            //     13
            //
            //-5,     0,    5
            //     -2
            // (cookingFinishedTime - cookingPeakTime) /  (upperBound - cookingPeakTime)
            // gives -1 to 1, with 0 being the peak
        }

        //timingpercent should be between -1 and 1, with 0 being ideal cooking time
        public static string GetCookingFlavorText(float timingPercent)
        {
            if (timingPercent > 1f) return "Impossibly well done";
            if (timingPercent > .75f) return "This meat was nearly grilled into charcoal.";
            if (timingPercent > .50f) return "This meat puts 'well done' to shame.";
            if (timingPercent > .25f) return "This meat is dry and tough.";
            if (timingPercent > .1f) return "This meat is lean, but packed with flavor.";
            if (timingPercent > .02f) return "This meat has a delicious, crunchy skin.";
            if (timingPercent > -.02f) return $"This meat is {CFG.ColorKingSkillsFF}Legendary{CFG.ColorEnd}.";
            if (timingPercent > -.10f) return "This meat is marbelized perfectly.";
            if (timingPercent > -.25f) return "This meat is tender and juicy.";
            if (timingPercent > -.50f) return "This meat is just a little too pink in the middle.";
            if (timingPercent > -.75f) return "This meat is chewy and pink.";
            if (timingPercent > -1f) return "This meat is bloody raw.";
            return "Impossibly raw";
        }
        public static bool GetCookingIsMeatStation(string name)
        {
            if (!CookingStationTiers.ContainsKey(name)) return false;
            return (name == "piece_cookingstation" || name == "piece_cookingstation_iron");
        }
        public static bool GetCookingIsKitchen(string name)
        {
            if (!CookingStationTiers.ContainsKey(name)) return false;
            return (name == "piece_cauldron" || name == "piece_oven");
        }
        public static float GetCookingTimeRedux(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(CookingTimeReduxMin, true), 
                PerToMult(CookingTimeReduxMax, true), skillFactor);
        }
        public static float GetCookingFermentTimeRedux(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(CookingFermentTimeReduxMin, true), 
                PerToMult(CookingFermentTimeReduxMin, true), skillFactor);
        }

        #endregion cook

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                             Jumping
        ////////////////////////////////////////////////////////////////////////////////////////
        #region jump
        #region configdef
        public static ConfigEntry<bool> ActiveSkillJump;
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
        #endregion configdef

        public static void InitJumpConfigs(ConfigFile cfg)
        {
            ActiveSkillJump = cfg.Bind("Generic.Active", "Jump", true,
                    AdminCD("Whether or not to activate king's skills version of the jump skill", true)); ;

            //Jump Experience
            JumpXPPercent = cfg.Bind("Jump.Experience", "Jump Experience Mod", 43f,
                AdminCD("% of fall damage that becomes experience"));

            //Jump Effects
            JumpFallDamageThresholdMin = cfg.Bind("Jump.Effect", "Fall Damage Threshold Min", 4f,
                AdminCD("meters to fall before you start calculating fall damage at level 0"));
            JumpFallDamageThresholdMax = cfg.Bind("Jump.Effect", "Fall Damage Threshold Max", 30f,
                AdminCD("meters to fall before you start calculating fall damage at level 100"));
            JumpFallDamageReduxMin = cfg.Bind("Jump.Effect", "Fall Damage Reduction Min", -25f,
                AdminCD("% less fall damage to take at level 0"));
            JumpFallDamageReduxMax = cfg.Bind("Jump.Effect", "Fall Damage Reduction Max", 85f,
                AdminCD("% less fall damage to take at level 100"));
            JumpForcePercentMin = cfg.Bind("Jump.Effect", "Vertical Force Min", -10f,
                AdminCD("% extra vertical jump force at level 0"));
            JumpForcePercentMax = cfg.Bind("Jump.Effect", "Vertical Force Max", 160f,
                AdminCD("% extra vertical jump force at level 100"));
            JumpForwardForcePercentMin = cfg.Bind("Jump.Effect", "Horizontal Force Min", 0f,
                AdminCD("% extra horizontal jump force at level 0"));
            JumpForwardForcePercentMax = cfg.Bind("Jump.Effect", "Horizontal Force Max", 310f,
                AdminCD("% extra horizontal jump force at level 100"));
            //May not actually be used or be different from jump force
            JumpStaminaReduxMin = cfg.Bind("Jump.Effect", "Stamina Cost Min", 0f,
                AdminCD("% less stamina cost to jump at level 0"));
            JumpStaminaReduxMax = cfg.Bind("Jump.Effect", "Stamina Cost Max", 80f,
                AdminCD("% less stamina cost to jump at level 100"));
            JumpTiredModMin = cfg.Bind("Jump.Effect", "Tired Stamina Reduction Min", 0f,
                AdminCD("% jump force added to the base game's tired factor, which " +
                "reduces your jump force when out of stamina, at level 0"));
            JumpTiredModMax = cfg.Bind("Jump.Effect", "Tired Stamina Reduction Max", 30f,
                AdminCD("% jump force added to the base game's tired factor, which " +
                "reduces your jump force when out of stamina, at level 100"));

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
                $"{Mathf.Lerp(PerToMult(JumpStaminaReduxMin, true),PerToMult(JumpStaminaReduxMax, true), skillFactor)}"));
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

        #endregion jump

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                             Mining
        ////////////////////////////////////////////////////////////////////////////////////////
        #region mine
        #region configdef
        public static ConfigEntry<bool> ActiveSkillMine;
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

        public static ConfigEntry<float> MiningXPRockTimer;

        public static Dictionary<string, float> MiningDropTable = new Dictionary<string, float>();
        public static Dictionary<string, float> MiningBXPTable = new Dictionary<string, float>();
        #endregion configdef

        public static void InitMineConfigs(ConfigFile cfg)
        {
            ActiveSkillMine = cfg.Bind("Generic.Active", "Mining", true,
                    AdminCD("Whether or not to activate king's skills version of the mining skill", true)); ;

            //List of all object names that count as mining drops
            MiningDropTable.Add("CopperOre", 0);
            MiningDropTable.Add("IronScrap", 0);
            MiningDropTable.Add("Obsidian", 0);
            MiningDropTable.Add("SilverOre", 0);
            MiningDropTable.Add("TinOre", 0);
            MiningDropTable.Add("Stone", 0);
            MiningDropTable.Add("Chitin", 0);


            //List of all objects that you can eat and their exp reward
            MiningBXPTable.Add("Stone", 2);
            MiningBXPTable.Add("Flint", 10);
            MiningBXPTable.Add("Coal", 15);
            MiningBXPTable.Add("CopperOre", 20);
            MiningBXPTable.Add("Copper", 32);
            MiningBXPTable.Add("TinOre", 33);
            MiningBXPTable.Add("Tin", 40);
            MiningBXPTable.Add("Bronze", 60);
            MiningBXPTable.Add("IronScrap", 80);
            MiningBXPTable.Add("Iron", 120);
            MiningBXPTable.Add("Obsidian", 140);
            MiningBXPTable.Add("SilverOre", 200);
            MiningBXPTable.Add("Silver", 245);
            MiningBXPTable.Add("Chitin", 85);


            MiningXPRockTimer = cfg.Bind("Mining.Experience", "Rock Timer", 21f,
                AdminCD("How many minutes a rock will last in your belly"));

            //effects
            MiningPickDamagePercentMin = cfg.Bind("Mining.Effect", "Pick Damage Min", 0f,
                AdminCD("% increase to pick damage at level 0"));
            MiningPickDamagePercentMax = cfg.Bind("Mining.Effect", "Pick Damage Max", 200f,
                AdminCD("% increase to pick damage at level 100"));
            MiningStaminaRebateMin = cfg.Bind("Mining.Effect", "Stamina Rebate Min", 0f,
                AdminCD("Flat stamina rebate on each hit of a rock at level 0"));
            MiningStaminaRebateMax = cfg.Bind("Mining.Effect", "Stamina Rebate Max", 7f,
                AdminCD("Flat stamina rebate on each hit of a rock at level 100"));
            MiningDropPercentMin = cfg.Bind("Mining.Effect", "Drop rate Min", 0f,
                AdminCD("% increase to ore drops at level 0"));
            MiningDropPercentMax = cfg.Bind("Mining.Effect", "Drop rate Max", 100f,
                AdminCD("% increase to ore drops at level 100"));
            MiningRegenHealthMin = cfg.Bind("Mining.Effect", "Regen Timer Reduction Min", 0f,
                AdminCD("How many seconds to reduce the health regeneration timer at level 0"));
            MiningRegenHealthMax = cfg.Bind("Mining.Effect", "Regen Timer Reduction Max", 3f,
                AdminCD("How many seconds to reduce the health regeneration timer at level 100"));
            MiningCarryCapacityMin = cfg.Bind("Mining.Effect", "Carry Capacity Min", 0f,
                AdminCD("How much extra carrying capacity you get at level 0"));
            MiningCarryCapacityMax = cfg.Bind("Mining.Effect", "Carry Capacity Max", 120f,
                AdminCD("How much extra carrying capacity you get at level 100"));
        }

        public static float GetMiningXPRockTimerInSeconds()
        {
            return MiningXPRockTimer.Value * 60f;
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
        public static float GetMiningXPEatRock(ItemDrop.ItemData rock)
        {
            if (rock == null) return 0f;
            string rockName = rock.m_dropPrefab.name;
            if (MiningBXPTable.ContainsKey(rockName))
            {
                return MiningBXPTable[rockName];
            }
            return 0f;
        }


        #endregion mine

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                             Running
        ////////////////////////////////////////////////////////////////////////////////////////
        #region run
        #region configdef
        public static ConfigEntry<bool> ActiveSkillRun;
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
        public static ConfigEntry<float> RunEncumberanceCurveFactor;

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
        #endregion configdef

        public static void InitRunConfigs(ConfigFile cfg)
        {
            ActiveSkillRun = cfg.Bind("Generic.Active", "Run", true,
                    AdminCD("Whether or not to activate king's skills version of the run skill", true)); ;


            //Run Absolute Weight Experience
            RunAbsoluteWeightMinWeight = cfg.Bind("Run.Experience.AbsoluteWeight", "Minimum Weight", 100f,
                AdminCD("The lowest weight you will get an experience bonus for carrying"));
            RunAbsoluteWeightMaxWeight = cfg.Bind("Run.Experience.AbsoluteWeight", "Maximum Weight", 1800f,
                AdminCD("The heighest weight you will get an experience bonus for carrying"));
            RunAbsoluteWeightFactor = cfg.Bind("Run.Experience.AbsoluteWeight", "Factor", .6f,
                AdminCD("Factor to define the slope of the absolute weight curve"));
            RunXPAbsoluteWeightPercent = cfg.Bind("Run.Experience.AbsoluteWeight", "XP", 400f,
                AdminCD("% modifier for how much experience you get from absolute weight"));


            //Run Relative Weight Experience
            RunRelativeWeightLight = cfg.Bind("Run.Experience.RelativeWeight", "Light Threshold", 33f,
                AdminCD("Threshold for being at 'light' encumberance"));
            RunRelativeWeightLightPercent = cfg.Bind("Run.Experience.RelativeWeight", "Light XP", -25f,
                AdminCD("Experience Bonus for being at 'light' encumberance"));
            RunRelativeWeightMed = cfg.Bind("Run.Experience.RelativeWeight", "Medium Threshold", 50f,
                AdminCD("Threshold for being at 'medium' encumberance"));
            RunRelativeWeightMedPercent = cfg.Bind("Run.Experience.RelativeWeight", "Medium XP", 0f,
                AdminCD("Experience Bonus for being at 'medium' encumberance"));
            RunRelativeWeightHighMed = cfg.Bind("Run.Experience.RelativeWeight", "High Threshold", 66f,
                AdminCD("Threshold for being at 'high' encumberance"));
            RunRelativeWeightHighMedPercent = cfg.Bind("Run.Experience.RelativeWeight", "High XP", 25f,
                AdminCD("Experience Bonus for being at 'high' encumberance"));
            RunRelativeWeightHeavy = cfg.Bind("Run.Experience.RelativeWeight", "Heavy Threshold", 80f,
                AdminCD("Threshold for being at 'heavy' encumberance"));
            RunRelativeWeightHeavyPercent = cfg.Bind("Run.Experience.RelativeWeight", "Heavy XP", 50f,
                AdminCD("Experience Bonus for being at 'heavy' encumberance"));
            RunRelativeWeightFull = cfg.Bind("Run.Experience.RelativeWeight", "Full Threshold", 100f,
                AdminCD("Threshold for being at 'full' encumberance"));
            RunRelativeWeightFullPercent = cfg.Bind("Run.Experience.RelativeWeight", "Full XP", 80f,
                AdminCD("Experience Bonus for being at 'full' encumberance"));
            RunRelativeWeightOverPercent = cfg.Bind("Run.Experience.RelativeWeight", "Overweight XP", 200f,
                AdminCD("Experience Bonus for being overencumbered"));
            RunXPRelativeWeightPercent = cfg.Bind("Run.Experience.RelativeWeight", "Overall XP", 100f,
                AdminCD("% modifier for how much experience you get from relative weight"));


            //Run Experience
            RunXPSpeedPercent = cfg.Bind("Run.Experience", "XP", 5f,
                AdminCD("% of extra experience gained from run speed"));



            //Run Effects
            RunSpeedPercentMin = cfg.Bind("Run.Effect", "Speed Min", 0f,
                AdminCD("% extra run speed at level 0"));
            RunSpeedPercentMax = cfg.Bind("Run.Effect", "Speed Max", 200f,
                AdminCD("% extra run speed at level 100"));
            RunEquipmentReduxMin = cfg.Bind("Run.Effect", "Equipment Reduction Min", -20f,
                AdminCD("% less movespeed reduction from equipment at level 0"));
            RunEquipmentReduxMax = cfg.Bind("Run.Effect", "Equipment Reduction Max", 60f,
                AdminCD("% less movespeed reduction from equipment at level 100"));

            RunEncumberancePercentMin = cfg.Bind("Run.Effect", "Encumberance Min", 0f,
                AdminCD("% less run speed when your inventory is empty"));
            RunEncumberancePercentMax = cfg.Bind("Run.Effect", "Encumberance Max", 50f,
                AdminCD("% less run speed when your inventory is full"));
            RunEncumberanceCurveFactor = cfg.Bind("Run.Effect", "Encumberance Curve", 1.8f,
                AdminCD("Factor for the curve of encumberance"));
            RunEncumberanceReduxMin = cfg.Bind("Run.Effect", "Encumberance Reduction Min", -15f,
                AdminCD("% less effect from encumberance at level 0"));
            RunEncumberanceReduxMax = cfg.Bind("Run.Effect", "Encumberance Reduction Max", 75f,
                AdminCD("% less effect from encumberance at level 100"));

            RunStaminaReduxMin = cfg.Bind("Run.Effect", "Stamina Reduction Min", -25f,
                AdminCD("% less stamina cost to run at level 100"));
            RunStaminaReduxMax = cfg.Bind("Run.Effect", "Stamina Reduction Max", 80f,
                AdminCD("% less stamina cost to run at level 100"));
            RunStaminaPerLevel = cfg.Bind("Run.Effect", "Stamina Flat", .44f,
                AdminCD("How much base stamina is added per level of run"));
        }

        public static float GetRunSpeedMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(RunSpeedPercentMin),
                PerToMult(RunSpeedPercentMax), skillFactor);
        }
        public static float GetEncumberanceCurveRedux(float encumberanceMod)
        {
            return Mathf.Lerp(PerToMult(RunEncumberancePercentMin, true),
                PerToMult(RunEncumberancePercentMax, true), 
                Mathf.Pow(encumberanceMod, RunEncumberanceCurveFactor.Value));
            //ShapeFactorSin(encumberanceMod));

            /*
            float runEncMin = PerToMult(RunEncumberanceModMin, true);
            float runEncMax = PerToMult(RunEncumberanceModMax, true);
            float result = Mathf.Lerp(runEncMin, runEncMax, ShapeFactorSin(encumberancePercent));
            Jotunn.Logger.LogMessage($"Generating the encumberance factor: encumberance percent is apparently {encumberancePercent}\n" +
                $"Min to max is read as {RunEncumberanceModMin.Value} - {RunEncumberanceModMax.Value}\n" +
                $"Using PerToMult, they are displayed as {runEncMin} - {runEncMax}\n" +
                $"running through my sin curve, I am returning {result}"));
            return result;*/
        }
        public static float GetEncumberanceRedux(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(RunEncumberanceReduxMin, true),
                PerToMult(RunEncumberanceReduxMax, true), skillFactor);
            /*
            Jotunn.Logger.LogMessage($"Encumberance redux: Config values are {RunEncumberanceReduxMin} to {RunEncumberanceReduxMax}\n" +
                $"PerToMult reads those values as {PerToMult(RunEncumberanceReduxMin, true)} to {PerToMult(RunEncumberanceReduxMax, true)}\n" +
                $"because I'm given {skillFactor}, I'm going to put out {answer}"));
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
                $"giving me {Mathf.Pow(weightPercent+1f, RunAbsoluteWeightFactor.Value)}"));*/
        }
        public static float GetRelativeWeightStageMult(float weightMod)
        {
            float mult = 1f;
            //Jotunn.Logger.LogMessage($"Relative weight: I'm given a percent of {weightPercent}\n"));

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

            //Jotunn.Logger.LogMessage($"Which means the multiplier I'm returning is {mult} times {PerToMult(RunRelativeWeightExpMod)}\n"));

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


        #endregion run

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                             Sailing
        ////////////////////////////////////////////////////////////////////////////////////////
        #region sail
        #region configdef
        public static ConfigEntry<bool> ActiveSkillSail;
        public static ConfigEntry<float> SailXPCrewBase;
        public static ConfigEntry<float> SailXPCaptainBase;
        public static ConfigEntry<float> SailXPWindMin;
        public static ConfigEntry<float> SailXPWindMax;
        public static ConfigEntry<float> SailXPSpeedMod;
        public static ConfigEntry<float> SailXPTierBonus;
        public static ConfigEntry<float> SailXPCrewBonus;
        public static ConfigEntry<float> SailXPTimerLength;

        public static Dictionary<string, ShipDef> SailShipDefs;

        public static ConfigEntry<float> SailSpeedMin;
        public static ConfigEntry<float> SailSpeedMax;
        public static ConfigEntry<float> SailWindNudgeMin;
        public static ConfigEntry<float> SailWindNudgeMax;
        public static ConfigEntry<float> SailExploreRangeMin;
        public static ConfigEntry<float> SailExploreRangeMax;
        public static ConfigEntry<float> SailPaddleSpeedMin;
        public static ConfigEntry<float> SailPaddleSpeedMax;
        public static ConfigEntry<float> SailRudderSpeedMin;
        public static ConfigEntry<float> SailRudderSpeedMax;
        public static ConfigEntry<float> SailControlTimer;
        public static ConfigEntry<float> SailDamageReduxMin;
        public static ConfigEntry<float> SailDamageReduxMax;

        public class ShipDef
        {
            public float LevelRQ;
            public int Tier;

            public ShipDef(float levelRQ, int tier)
            {
                LevelRQ = levelRQ;
                Tier = tier;
            }
        }
        #endregion configdef

        private static void InitSailConfig(ConfigFile cfg)
        {
            ActiveSkillSail = cfg.Bind("Generic.Active", "Sailing", true,
                    AdminCD("Whether or not to allow King's sailing"));


            SailXPCrewBase = cfg.Bind("Sailing.Experience", "Crew Base", .2f,
                    AdminCD("Base rate while swimming or standing on board."));
            SailXPCaptainBase = cfg.Bind("Sailing.Experience", "Captain Base", .45f,
                    AdminCD("Base rate while captaining a ship."));
            SailXPWindMin = cfg.Bind("Sailing.Experience", "Wind Min", 0f,
                    AdminCD("% experience bonus when wind is totally against you"));
            SailXPWindMax = cfg.Bind("Sailing.Experience", "Wind Max", 70f,
                    AdminCD("% experience bonus when wind is totally in the sails"));
            SailXPSpeedMod = cfg.Bind("Sailing.Experience", "Speed", .25f,
                    AdminCD("Amount of experience each speed unit is worth"));
            SailXPTierBonus = cfg.Bind("Sailing.Experience", "Ship Tier", 66f,
                    AdminCD("% of extra experience for each tier of ship after the first"));
            SailXPCrewBonus = cfg.Bind("Sailing.Experience", "Crew", 33f,
                    AdminCD("% extra experience for each crew member on board past the first"));
            SailXPTimerLength = cfg.Bind("Sailing.Experience", "Timer", 2f,
                    AdminCD("Number of seconds for each sailing experience bundle"));

            SailShipDefs = new Dictionary<string, ShipDef>();

            SailShipDefs.Add("Raft", new ShipDef(0, 0));
            SailShipDefs.Add("Karve", new ShipDef(20, 1));
            SailShipDefs.Add("Longship", new ShipDef(50, 2));

            SailSpeedMin = cfg.Bind("Sailing.Effect", "Speed Min", 0f,
                    AdminCD("% of extra sailing speed at level 0"));
            SailSpeedMax = cfg.Bind("Sailing.Effect", "Speed Max", 200f,
                    AdminCD("% of extra sailing speed at level 100"));
            SailWindNudgeMin = cfg.Bind("Sailing.Effect", "Wind Nudge Min", -15f,
                    AdminCD("% of nudge towards favorable winds at level 0"));
            SailWindNudgeMax = cfg.Bind("Sailing.Effect", "Wind Nudge Max", 30f,
                    AdminCD("% of nudge towards favorable winds at level 100"));
            SailExploreRangeMin = cfg.Bind("Sailing.Effect", "Explore Range Min", 0f,
                    AdminCD("extra units of explore range at level 0"));
            SailExploreRangeMax = cfg.Bind("Sailing.Effect", "Explore Range Max", 25f,
                    AdminCD("extra units of explore range at level 100"));
            SailPaddleSpeedMin = cfg.Bind("Sailing.Effect", "Paddle Speed Min", -25f,
                    AdminCD("% extra paddling speed at level 0"));
            SailPaddleSpeedMax = cfg.Bind("Sailing.Effect", "Paddle Speed Max", 100f,
                    AdminCD("% extra paddling speed at level 100"));
            SailRudderSpeedMin = cfg.Bind("Sailing.Effect", "Rudder Speed Min", -5f,
                    AdminCD("% extra rudder turning speed at level 0"));
            SailRudderSpeedMax = cfg.Bind("Sailing.Effect", "Rudder Speed Max", 80f,
                    AdminCD("% extra rudder turning speed at level 100"));
            SailControlTimer = cfg.Bind("Sailing.Effect", "Control Timer", 1.5f,
                    AdminCD("seconds between each update of the sail controls"));
            SailDamageReduxMin = cfg.Bind("Sailing.Effect", "Damage Redux Min", -20f,
                    AdminCD("% less damage taken by the boat at level 0"));
            SailDamageReduxMax = cfg.Bind("Sailing.Effect", "Damage Redux Max", 60f,
                    AdminCD("% less damage taken by the boat at level 0"));
        }


        public static float GetSailShipLevelRQ(Ship ship)
        {
            if (ship == null) return 0f;
            string name = ship.m_nview.GetPrefabName();
            if (SailShipDefs.ContainsKey(name)) return SailShipDefs[name].LevelRQ;

            return 0f;
        }
        public static float GetSailXPWindMult(float windFactor)
        {
            return Mathf.Lerp(PerToMult(SailXPWindMin), 
                PerToMult(SailXPWindMax), windFactor);
        }
        public static float GetSailXPTierMult(Ship ship)
        {
            if (ship == null) return 1f;
            string name = ship.m_nview.GetPrefabName();
            if (SailShipDefs.ContainsKey(name)) 
                return 1f + SailShipDefs[name].Tier * PerToMod(SailXPTierBonus);

            return 1f;
        }
        public static float GetSailXPCrewMult(float crewCount)
        {
            return 1f + ((crewCount - 1) * PerToMod(SailXPCrewBonus));
        }
        public static float GetSailSpeedMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(SailSpeedMin), 
                PerToMult(SailSpeedMax), skillFactor);
        }
        public static float GetSailWindNudgeMod(float skillFactor)
        {
            return Mathf.Lerp(PerToMod(SailWindNudgeMin), 
                PerToMult(SailWindNudgeMax), skillFactor);
        }
        public static float GetSailExploreRange(float skillFactor)
        {
            return Mathf.Lerp(SailExploreRangeMin.Value, 
                SailExploreRangeMax.Value, skillFactor);
        }
        public static float GetSailPaddleSpeedMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(SailPaddleSpeedMin), 
                PerToMult(SailPaddleSpeedMax), skillFactor);
        }
        public static float GetSailRudderSpeedMult(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(SailRudderSpeedMin),
                PerToMult(SailRudderSpeedMax), skillFactor);
        }
        public static float GetSailDamageRedux(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(SailDamageReduxMin, true), 
                PerToMult(SailDamageReduxMax, true), skillFactor);
        }

        #endregion sail

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                             Sneaking
        ////////////////////////////////////////////////////////////////////////////////////////
        #region sneak
        #region configdef
        public static ConfigEntry<bool> ActiveSkillSneak;
        public static ConfigEntry<float> SneakXPThreatPercent;
        public static ConfigEntry<float> SneakStaminaDrainMin;
        public static ConfigEntry<float> SneakStaminaDrainMax;
        public static ConfigEntry<float> SneakSpeedPercentMin;
        public static ConfigEntry<float> SneakSpeedPercentMax;
        #endregion configdef

        public static void InitSneakConfigs(ConfigFile cfg)
        {

            ActiveSkillSneak = cfg.Bind("Generic.Active", "Sneak", true,
                    AdminCD("Whether or not to activate king's skills version of the sneak skill", true)); ;

            //effects
            SneakXPThreatPercent = cfg.Bind("Sneak.Experience", "Experience Bonus per Danger", 1.2f,
                    AdminCD("Determines how much each 'point of danger' is worth in sneak exp"));
            SneakStaminaDrainMin = cfg.Bind("Sneak.Effect", "Stamina Drain Min", 10f,
                    AdminCD("Amount of stamina drain per second while sneaking at level 0"));
            SneakStaminaDrainMax = cfg.Bind("Sneak.Effect", "Stamina Drain Max", 1.0f,
                    AdminCD("Amount of stamina drain per second while sneaking at level 100"));
            SneakSpeedPercentMin = cfg.Bind("Sneak.Effect", "Speed Min", 0f,
                    AdminCD("% speed increase while sneaking at level 0"));
            SneakSpeedPercentMax = cfg.Bind("Sneak.Effect", "Speed Max", 350f,
                    AdminCD("% speed increase while sneaking at level 100"));
        }

        public static float GetSneakXPMult()
        {
            return PerToMult(SneakXPThreatPercent);
        }
        public static float GetSneakStaminaDrain(float skillFactor)
        {
            return Mathf.Lerp(SneakStaminaDrainMin.Value, SneakStaminaDrainMax.Value, skillFactor);
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


        #endregion sneak

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                             Swimming
        ////////////////////////////////////////////////////////////////////////////////////////
        #region swim
        #region configdef
        public static ConfigEntry<bool> ActiveSkillSwim;
        public static ConfigEntry<float> SwimXPSpeedPercent;
        public static ConfigEntry<float> SwimSpeedPercentMin;
        public static ConfigEntry<float> SwimSpeedPercentMax;
        public static ConfigEntry<float> SwimAccelPercentMin;
        public static ConfigEntry<float> SwimAccelPercentMax;
        public static ConfigEntry<float> SwimTurnPercentMin;
        public static ConfigEntry<float> SwimTurnPercentMax;
        public static ConfigEntry<float> SwimStaminaPerSecMin;
        public static ConfigEntry<float> SwimStaminaPerSecMax;
        #endregion configdef

        public static void InitSwimConfigs(ConfigFile cfg)
        {
            ActiveSkillSwim = cfg.Bind("Generic.Active", "Swim", true,
                    AdminCD("Whether or not to activate king's skills version of the swim skill", true)); ;

            //Swim Experience
            SwimXPSpeedPercent = cfg.Bind("Swim.Experience", "XP", 2f,
                AdminCD("% of swim speed that becomes bonus experience gain"));

            //Swim effects
            SwimSpeedPercentMin = cfg.Bind("Swim.Effect", "Speed Min", 0f,
                AdminCD("% to increase swim speed at level 0"));
            SwimSpeedPercentMax = cfg.Bind("Swim.Effect", "Speed Max", 350f,
                AdminCD("% to increase swim speed at level 100"));
            SwimAccelPercentMin = cfg.Bind("Swim.Effect", "Acceleration Min", 0f,
                AdminCD("% to increase swim acceleration at level 0"));
            SwimAccelPercentMax = cfg.Bind("Swim.Effect", "Acceleration Max", 350f,
                AdminCD("% to increase swim acceleration at level 100"));
            SwimTurnPercentMin = cfg.Bind("Swim.Effect", "Turn Speed Min", 0f,
                AdminCD("% to increase swim turn speed at level 0"));
            SwimTurnPercentMax = cfg.Bind("Swim.Effect", "Turn Speed Max", 500f,
                AdminCD("% to increase swim turn speed at level 100"));
            SwimStaminaPerSecMin = cfg.Bind("Swim.Effect", "Stamina cost min", 5f,
                AdminCD("How much stamina swimming will take per second at level 0"));
            SwimStaminaPerSecMax = cfg.Bind("Swim.Effect", "Stamina cost max", .5f,
                AdminCD("How much stamina swimming will take per second at level 100"));
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
        public static float GetSwimStaminaPerSec(float skillFactor)
        {
            return Mathf.Lerp(SwimStaminaPerSecMin.Value, SwimStaminaPerSecMax.Value, skillFactor);
        }


        #endregion swim

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                             Woodcutting
        ////////////////////////////////////////////////////////////////////////////////////////
        #region wood
        #region configdef
        public static ConfigEntry<bool> ActiveSkillWood;
        public static ConfigEntry<float> ToolBXPWoodStubReward;
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

        public static Dictionary<string, float> WoodcuttingDropTable = new Dictionary<string, float>();
        #endregion configdef

        public static void InitWoodConfigs(ConfigFile cfg)
        {
            ActiveSkillWood = cfg.Bind("Generic.Active", "Woodcutting", true,
                    AdminCD("Whether or not to activate king's skills version of the woodcutting skill", true)); ;

            //List of all object names that count as woodcutting drops
            WoodcuttingDropTable.Add("BeechSeeds", 0);
            WoodcuttingDropTable.Add("ElderBark", 0);
            WoodcuttingDropTable.Add("FineWood", 0);
            WoodcuttingDropTable.Add("FirCone", 0);
            WoodcuttingDropTable.Add("PineCone", 0);
            WoodcuttingDropTable.Add("RoundLog", 0);
            WoodcuttingDropTable.Add("Wood", 0);


            ToolBXPWoodStubReward = cfg.Bind("Experience.Tools", "Woodcutting BXP for Stump", 20f,
                AdminCD("The amount of experience you get for breaking a stump"));

            //effects
            WoodcuttingChopDamagePercentMin = cfg.Bind("Wood.Effect", "Chop Damage Min", 0f,
                AdminCD("% increase to chop damage at level 0"));
            WoodcuttingChopDamagePercentMax = cfg.Bind("Wood.Effect", "Chop Damage Max", 200f,
                AdminCD("% increase to chop damage at level 100"));
            WoodcuttingStaminaRebateMin = cfg.Bind("Wood.Effect", "Stamina Rebate Min", 0f,
                AdminCD("Flat stamina rebate on each hit of a tree at level 0"));
            WoodcuttingStaminaRebateMax = cfg.Bind("Wood.Effect", "Stamina Rebate Max", 9f,
                AdminCD("Flat stamina rebate on each hit of a tree at level 100"));
            WoodcuttingDropPercentMin = cfg.Bind("Wood.Effect", "Drop rate min", 0f,
                AdminCD("% increase to wood drops at level 0"));
            WoodcuttingDropPercentMax = cfg.Bind("Wood.Effect", "Drop rate max", 250f,
                AdminCD("% increase to wood drops at level 100"));
            WoodcuttingRegenStaminaMin = cfg.Bind("Wood.Effect", "Regen Timer Reduction Min", -.1f,
                AdminCD("Amount of seconds to take off the stamina regeneration timer at level 0. Base value in vanilla is 1s"));
            WoodcuttingRegenStaminaMax = cfg.Bind("Wood.Effect", "Regen Timer Reduction Min", .7f,
                AdminCD("Amount of seconds to take off the stamina regeneration timer at level 100"));
            WoodcuttingCarryCapacityMin = cfg.Bind("Wood.Effect", "Carry Capacity Min", 0f,
                AdminCD("How much extra carrying capacity you get at level 0"));
            WoodcuttingCarryCapacityMax = cfg.Bind("Wood.Effect", "Carry Capacity Max", 120f,
                AdminCD("How much extra carrying capacity you get at level 100"));
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


        #endregion wood

        #endregion skills

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                    Perks
        ////////////////////////////////////////////////////////////////////////////////////////
        #region perks

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                Generic Perks
        ////////////////////////////////////////////////////////////////////////////////////////
        #region genericperks
        #region configdef
        public static ConfigEntry<bool> PerkExplorationOn;
        public static ConfigEntry<float> PerkOneLVLThreshold;
        public static ConfigEntry<float> PerkTwoLVLThreshold;
        public static ConfigEntry<float> PerkThreeLVLThreshold;
        public static ConfigEntry<float> PerkFourLVLThreshold;
        public static ConfigEntry<float> PerkWeaponEnchantDamage;
        #endregion configdef
        public static void InitPerkConfig(ConfigFile cfg)
        {
            PerkExplorationOn = cfg.Bind("Perks", "Perk Exploration", true,
                    AdminCD("Whether or not locked perks are hidden from view", true));

            PerkOneLVLThreshold = cfg.Bind("Perks", "First Threshold", .3f,
                    AdminCD("mod of max level before you unlock the first set of perks", true));
            PerkTwoLVLThreshold = cfg.Bind("Perks", "Second Threshold", .6f,
                    AdminCD("mod of max level before you unlock the second set of perks", true));
            PerkThreeLVLThreshold = cfg.Bind("Perks", "ThirdThreshold", .9f,
                    AdminCD("mod of max level before you unlock the third set of perks", true));
            PerkFourLVLThreshold = cfg.Bind("Perks", "FourthThreshold", 10f,
                    AdminCD("mod of max level before you unlock the fourth set of perks - NOT IMPLEMENTED"));

            PerkWeaponEnchantDamage = cfg.Bind("Perks", "Weapon Enchant Damage", 10f,
                    AdminCD("% of extra damage added by weapon enchants"));

            InitAllPerkConfigs(cfg);
        }

        public static void InitAllPerkConfigs(ConfigFile cfg)
        {
            InitAirStepConfigs(cfg);
            InitAlwaysPreparedConfigs(cfg);
            InitAquamanConfigs(cfg);
            InitAsguardConfigs(cfg);
            InitAttackOfOpportunityConfigs(cfg);
            InitBerserkrConfigs(cfg);
            InitBigStickConfigs(cfg);
            InitBlackFlashConfigs(cfg);
            InitBlastWaveConfigs(cfg);
            InitBlockExpertConfigs(cfg);
            InitBoomerangConfigs(cfg);
            InitBotanyConfigs(cfg);
            InitBreakMyStrideConfigs(cfg);
            InitButterflyConfigs(cfg);
            InitCloakOfShadowsConfigs(cfg);
            InitClosingTheGapConfigs(cfg);
            InitControlledDemoConfigs(cfg);
            InitCouchedLanceConfigs(cfg);
            InitCoupDeBurstConfigs(cfg);
            InitCriticalBlowConfigs(cfg);
            InitDeadeyeConfigs(cfg);
            InitDecapitationConfigs(cfg);
            InitDidntHurtConfigs(cfg);
            InitDisarmingDefenseConfigs(cfg);
            InitESPConfigs(cfg);
            InitEfficiencyConfigs(cfg);
            InitEinherjarConfigs(cfg);
            InitEngineerConfigs(cfg);
            InitFalconKickConfigs(cfg);
            InitFirstMateConfigs(cfg);
            InitFishersBoonConfigs(cfg);
            InitFiveStarChefConfigs(cfg);
            InitFourStomachsConfigs(cfg);
            InitFragmentationConfigs(cfg);
            InitFrugalConfigs(cfg);
            InitGiantSmashConfigs(cfg);
            InitGodSlayingStrikeConfigs(cfg);
            InitGoombaStompConfigs(cfg);
            InitGreenThumbConfigs(cfg);
            InitGutAndRunConfigs(cfg);
            InitHarvesterConfigs(cfg);
            InitHeartOfTheForestConfigs(cfg);
            InitHeartOfTheMonkeyConfigs(cfg);
            InitHermesBootsConfigs(cfg);
            InitHideInPlainSightConfigs(cfg);
            InitHighlanderConfigs(cfg);
            InitHraesvelgConfigs(cfg);
            InitHydrodynamicConfigs(cfg);
            InitIaiConfigs(cfg);
            InitIronSkinConfigs(cfg);
            InitJoJoPoseConfigs(cfg);
            InitJotunnConfigs(cfg);
            InitJuggernautConfigs(cfg);
            InitKeenNoseConfigs(cfg);
            InitLightningReflexConfigs(cfg);
            InitLivingStoneConfigs(cfg);
            InitLodeBearingStoneConfigs(cfg);
            InitLogHorizonConfigs(cfg);
            InitLokisGiftConfigs(cfg);
            InitMagneticConfigs(cfg);
            InitManOverboardConfigs(cfg);
            InitMarathonSwimmerConfigs(cfg);
            InitMarketGardenerConfigs(cfg);
            InitMassiveStatureConfigs(cfg);
            InitMasterOfTheLogConfigs(cfg);
            InitMeditationConfigs(cfg);
            InitMeteorDropConfigs(cfg);
            InitMjolnirConfigs(cfg);
            InitMountainGoatConfigs(cfg);
            InitNailgunConfigs(cfg);
            InitNutritionConfigs(cfg);
            InitOdinJumpConfigs(cfg);
            InitOfferToUllrConfigs(cfg);
            InitPandemoniumPointConfigs(cfg);
            InitPerfectComboConfigs(cfg);
            InitPlusUltraConfigs(cfg);
            InitPowerDrawConfigs(cfg);
            InitPressurePointsConfigs(cfg);
            InitRammingSpeedConfigs(cfg);
            InitResponsibleLumberjackConfigs(cfg);
            InitRockDodgerConfigs(cfg);
            InitRockHaulerConfigs(cfg);
            InitRunedArrowsConfigs(cfg);
            InitSeaShantyConfigs(cfg);
            InitSeedSatchelConfigs(cfg);
            InitSeedingConfigs(cfg);
            InitShatterStrikeConfigs(cfg);
            InitSilentSprinterConfigs(cfg);
            InitSleightOfHandConfigs(cfg);
            InitSmokeBombConfigs(cfg);
            InitSoilMixingConfigs(cfg);
            InitSpearitConfigs(cfg);
            InitSpiceMasterConfigs(cfg);
            InitSpicySweetConfigs(cfg);
            InitSpikedShieldConfigs(cfg);
            InitSpiritGuideConfigs(cfg);
            InitStretchConfigs(cfg);
            InitSuperfuelConfigs(cfg);
            InitTackleConfigs(cfg);
            InitTasteTestingConfigs(cfg);
            InitThrowbackConfigs(cfg);
            InitTitanEnduranceConfigs(cfg);
            InitTitanStrengthConfigs(cfg);
            InitToxicConfigs(cfg);
            InitTrapmasterConfigs(cfg);
            InitTrenchDiggerConfigs(cfg);
            InitValkyrieFlightConfigs(cfg);
            InitVitalStudyConfigs(cfg);
            InitWarehousingConfigs(cfg);
            InitWarriorOfLightConfigs(cfg);
            InitWaterRunningConfigs(cfg);
            InitWorldlyConfigs(cfg);
            InitYmirConfigs(cfg);
        }

        public static float GetPerkWeaponEnchantMod()
        {
            return PerToMod(PerkWeaponEnchantDamage);
        }

        #endregion genericperks

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Air Step
        ////////////////////////////////////////////////////////////////////////////////////////
        #region airstep
        #region configdef
        public static ConfigEntry<int> AirStepNumJumps;
        #endregion configdef

        public static void InitAirStepConfigs(ConfigFile cfg)
        {
            AirStepNumJumps = cfg.Bind("Perks.AirStep", "Number of Jumps", 1,
                    AdminCD("The number of extra jumps in air that Air Step gives you"));
        }

        public static Perk GetPerkAirStep()
        {
            int jumpNum = GetAirStepExtraJumps();
            string times = ConditionalPluralize(jumpNum, "time");
            string jumps = ConditionalPluralize(jumpNum, "jump");

            return new Perk("Air Step",
                $"Allows you to jump {jumpNum} additional {times} while in the air.",
                "The laws of physics are nothing to a viking!",
                PerkMan.PerkType.AirStep, Skills.SkillType.Jump, "Icons/airstep.png",
                $"{jumpNum} extra {jumps}");
        }

        public static int GetAirStepExtraJumps()
        {
            return AirStepNumJumps.Value;
        }


        #endregion airstep

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Always Preapred
        ////////////////////////////////////////////////////////////////////////////////////////
        #region alwaysprepared
        #region configdef

        #endregion configdef

        public static void InitAlwaysPreparedConfigs(ConfigFile cfg)
        {
            //Nothing important yet
        }

        public static Perk GetPerkAlwaysPrepared()
        {
            return new Perk("Always Prepared",
                "Entering water no longer causes you to put your weapons or tools away.",
                "Breaking news: man literally too angry to swim",
                PerkMan.PerkType.AlwaysPrepared, Skills.SkillType.Swim, "Icons/alwaysprepared.png",
                "Equip and Attack underwater");
        }




        #endregion alwaysprepared

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Aquaman
        ////////////////////////////////////////////////////////////////////////////////////////
        #region aquaman
        #region configdef
        public static ConfigEntry<float> AquamanTimerStart;
        public static ConfigEntry<float> AquamanDamageMin;
        public static ConfigEntry<float> AquamanDamageMax;
        public static ConfigEntry<float> AquamanUpdateTimer;
        public static ConfigEntry<float> AquamanTimeTilMax;
        public static ConfigEntry<float> AquamanRangeMin;
        public static ConfigEntry<float> AquamanRangeMax;
        public static ConfigEntry<float> AquamanPushForce;
        public static ConfigEntry<float> AquamanBackstab;
        public static ConfigEntry<float> AquamanDecayOutOfWater;
        #endregion configdef

        public static void InitAquamanConfigs(ConfigFile cfg)
        {
            AquamanTimerStart = cfg.Bind("Perks.Aquaman", "Timer to Start", 10f,
                    AdminCD("seconds before Aquman starts happening"));
            AquamanDamageMin = cfg.Bind("Perks.Aquaman", "Min Damage", 5f,
                    AdminCD("damage dealt per hit at the minimum"));
            AquamanDamageMax = cfg.Bind("Perks.Aquaman", "Max Damage", 40f,
                    AdminCD("damage dealt per hit at the maximum"));
            AquamanUpdateTimer = cfg.Bind("Perks.Aquaman", "Time between ticks", 2.5f,
                    AdminCD("seconds bewteen reupdates of aquaman size and damage"));
            AquamanTimeTilMax = cfg.Bind("Perks.Aquaman", "Time until max damage", 50f,
                    AdminCD("seconds until aquaman has fully ramped up"));
            AquamanRangeMin = cfg.Bind("Perks.Aquaman", "Min Range", 1f,
                    AdminCD("unit radius of aquaman at min charge"));
            AquamanRangeMax = cfg.Bind("Perks.Aquaman", "Max range", 5.5f,
                    AdminCD("unit radius of aquaman at max charge"));
            AquamanPushForce = cfg.Bind("Perks.Aquaman", "Push Force", 10f,
                    AdminCD("Force that fish school hits with"));
            AquamanBackstab = cfg.Bind("Perks.Aquaman", "Backstab", 2f,
                    AdminCD("Sneak attack bonus for fish school"));
            AquamanDecayOutOfWater = cfg.Bind("Perks.Aquaman", "Decay", 3f,
                    AdminCD("Mod for the decay factor while out of water"));
        }

        public static Perk GetPerkAquaman()
        {
            return new Perk("Aquaman",
                $"As you remain in water, an increasingly larger school of fish will circle you and protect you from any " +
                $"aggressors. It takes {AquamanTimerStart.Value} seconds to start, and charges up in size and damage over the following " +
                $"{AquamanTimeTilMax.Value} seconds.",
                "It may not be laser eyes, but it's still an objectively cool power.",
                PerkMan.PerkType.Aquaman, Skills.SkillType.Swim, "Icons/aquaman.png",
                "Fish attack nearby units while in water");
        }

        public static float GetAquamanTimerPercent(float time)
        {
            return Mathf.Clamp01((time - AquamanTimerStart.Value) / AquamanTimeTilMax.Value);
        }
        public static float GetAquamanRadius(float time)
        {
            return Mathf.Lerp(AquamanRangeMin.Value, AquamanRangeMax.Value, GetAquamanTimerPercent(time));
        }
        public static float GetAquamanDamage(float time)
        {
            return Mathf.Lerp(AquamanDamageMin.Value, AquamanDamageMax.Value, GetAquamanTimerPercent(time));
        }



        #endregion aquaman

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Asguard
        ////////////////////////////////////////////////////////////////////////////////////////
        #region asguard
        #region configdef

        #endregion configdef

        public static void InitAsguardConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkAsguard()
        {
            return new Perk("Asguard",
                "You now block in all directions while blocking - but you can only get a perfect block when " +
                "properly facing the attack.",
                "Are all the stars in the sky my enemy?",
                PerkMan.PerkType.Asguard, Skills.SkillType.Polearms, "Icons/asguard.png",
                "Omnidirectional Block");
        }




        #endregion asguard

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 AttackOfOpportunity
        ////////////////////////////////////////////////////////////////////////////////////////
        #region attackofopportunity
        #region configdef
        public static ConfigEntry<float> AOOPCooldown;
        public static ConfigEntry<float> AOOPDamageMod;
        public static ConfigEntry<float> AOOPRange;
        public static ConfigEntry<float> AOOPPushForceMod;
        #endregion configdef

        public static void InitAttackOfOpportunityConfigs(ConfigFile cfg)
        {
            AOOPCooldown = cfg.Bind("Perks.AttackOfOpportunity", "Cooldown", 2f,
                    AdminCD("Seconds between possible AOOP hits"));
            AOOPDamageMod = cfg.Bind("Perks.AttackOfOpportunity", "Damage Mod", 40f,
                    AdminCD("% of regular damage dealt by an AOOP"));
            AOOPRange = cfg.Bind("Perks.AttackOfOpportunity", "Range", 1f,
                    AdminCD("unit radius of your AOOP range"));
            AOOPPushForceMod = cfg.Bind("Perks.AttackOfOpportunity", "Push Force", 120f,
                    AdminCD("% of regular knockback dealt by AOOP"));
        }

        public static Perk GetPerkAttackOfOpportunity()
        {
            return new Perk("Attack of Opportunity",
                "While you are running and unarmed, any enemy that comes close to you" +
                " gets automatically hit with a high-speed kick. Has a 2s cooldown.",
                "You're already in my range...",
                PerkMan.PerkType.AttackOfOpportunity, Skills.SkillType.Unarmed, "Icons/attackofopportunity.png",
                "Free hits while running");
        }

        public static float GetAOOPDamageMod()
        {
            return PerToMod(AOOPDamageMod);
        }
        public static float GetAOOPPushForceMod()
        {
            return PerToMod(AOOPPushForceMod);
        }



        #endregion attackofopportunity

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Berserkr
        ////////////////////////////////////////////////////////////////////////////////////////
        #region berserkr
        #region configdef
        public static ConfigEntry<float> BerserkrProcChance;
        public static ConfigEntry<int> BerserkrMaxStacks;
        public static ConfigEntry<float> BerserkrStaminaReduxPer;
        public static ConfigEntry<float> BerserkrDamageReduxPer;
        public static ConfigEntry<float> BerserkrMovespeedPer;
        public static ConfigEntry<float> BerserkrStackDuration;
        #endregion configdef

        public static void InitBerserkrConfigs(ConfigFile cfg)
        {
            BerserkrProcChance = cfg.Bind("Perks.Berserkr", "Proc Chance", 20f,
                    AdminCD("% chance per hit taken to activate Berserk"));
            BerserkrMaxStacks = cfg.Bind("Perks.Berserkr", "Max Stacks", 5,
                    AdminCD("Maximum number of stacks"));
            BerserkrStaminaReduxPer = cfg.Bind("Perks.Berserkr", "Stamina Redux Per", 10f,
                    AdminCD("% reduction in stamina per stack"));
            BerserkrDamageReduxPer = cfg.Bind("Perks.Berserkr", "Damage Redux Per", 6f,
                    AdminCD("% reduction in damage per stack"));
            BerserkrMovespeedPer = cfg.Bind("Perks.Berserkr", "Movespeed Per", 8f,
                    AdminCD("% increase in movespeed per stack"));
            BerserkrStackDuration = cfg.Bind("Perks.Berserkr", "Duration", 45f,
                    AdminCD("seconds before your stacks of berserk run out"));
        }

        public static Perk GetPerkBerserkr()
        {
            return new Perk("Berserkr",
                "When you take damage, there's a small chance to enter a berserker rage, which increases" +
                " your damage, reduces stamina costs, and increases your movespeed.",
                "That's my secret, Cap. I'm always angry.",
                PerkMan.PerkType.Berserkr, Skills.SkillType.Axes, "Icons/berserkr.png",
                $"{BerserkrProcChance.Value}% chance on damage taken to get a stack of berserk, refreshing the " +
                $"duration and providing buffs.");
        }

        public static bool GetBerserkCheck()
        {
            return UnityEngine.Random.Range(0f, 1f) < PerToMod(BerserkrProcChance);
        }
        public static float GetBerserkStaminaRedux()
        {
            SE_Stacking buff = (SE_Stacking)Player.m_localPlayer.GetSEMan().GetStatusEffect(KS_SEMan.ks_BerserkName);
            if (buff is null)
                return 1f;
            else
                return 1f - (PerToMod(BerserkrStaminaReduxPer) * buff.stacks);
        }
        public static float GetBerserkDamageRedux()
        {
            SE_Stacking buff = (SE_Stacking)Player.m_localPlayer.GetSEMan().GetStatusEffect(KS_SEMan.ks_BerserkName);
            if (buff is null)
                return 1f;
            else
                return 1f - (PerToMod(BerserkrDamageReduxPer) * buff.stacks);
        }
        public static float GetBerserkMovespeedMult()
        {
            SE_Stacking buff = (SE_Stacking)Player.m_localPlayer.GetSEMan().GetStatusEffect(KS_SEMan.ks_BerserkName);
            if (buff is null)
                return 1f;
            else
                return 1f + (PerToMod(BerserkrMovespeedPer) * buff.stacks);
        }
        public static float GetBerserkDuration()
        {
            return BerserkrStackDuration.Value;
        }




        #endregion berserkr

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 BigStick
        ////////////////////////////////////////////////////////////////////////////////////////
        #region BigStick
        #region configdef
        public static ConfigEntry<float> BigStickScale;
        public static ConfigEntry<float> BigStickStaminaMalus;
        public static ConfigEntry<float> BigStickDamageBonus;

        #endregion configdef

        public static void InitBigStickConfigs(ConfigFile cfg)
        {
            BigStickScale = cfg.Bind("Perks.BigStick", "Scale", 80f,
                    AdminCD("% increase to the size of your weapon."));
            BigStickStaminaMalus = cfg.Bind("Perks.BigStick", "Stamina Cost", 100f,
                    AdminCD("% increase to stamina costs."));
            BigStickDamageBonus = cfg.Bind("Perks.BigStick", "Damage Bonus", 40f,
                    AdminCD("% damage increase"));
        }

        public static Perk GetPerkBigStick()
        {
            return new Perk("Big Stick",
                "Your equipped weapon increases in size, costing more stamina but also dealing more damage.",
                "Unfortunately, does nothing for the softness of your voice.",
                PerkMan.PerkType.BigStick, Skills.SkillType.Polearms, "Icons/bigstick.png",
                $"{BigStickScale.Value}% bigger weapon, {BigStickStaminaMalus.Value}% more stamina cost, " +
                $"{BigStickDamageBonus.Value}% more damage");
        }

        public static float GetBigStickScaleMult()
        {
            return PerToMult(BigStickScale);
        }

        public static float GetBigStickStaminaMult()
        {
            if (P_BigStick.embiggened)
                return PerToMult(BigStickStaminaMalus);
            else
                return 1f;
        }
        public static float GetBigStickDamageMult()
        {
            if (P_BigStick.embiggened)
                return PerToMult(BigStickDamageBonus);
            else
                return 1f;
        }



        #endregion BigStick

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 BlackFlash
        ////////////////////////////////////////////////////////////////////////////////////////
        #region BlackFlash
        #region configdef
        public static ConfigEntry<float> BlackFlashDamage;
        public static ConfigEntry<float> BlackFlashRange;
        #endregion configdef

        public static void InitBlackFlashConfigs(ConfigFile cfg)
        {
            BlackFlashDamage = cfg.Bind("Perks.BlackFlash", "Damage", 40f,
                    AdminCD("% of your block power * parry bonus to use as damage"));
            BlackFlashRange = cfg.Bind("Perks.BlackFlash", "Range", 5f,
                    AdminCD("units of explosion radius"));
        }

        public static Perk GetPerkBlackFlash()
        {
            return new Perk("Black Flash",
                "Perfect blocks will now cause explosions, dealing damage in an area based on your block power" +
                " and parry bonus.",
                "'There's a sense of omnipotence, like everything else revolves around them.'",
                PerkMan.PerkType.BlackFlash, Skills.SkillType.Blocking, "Icons/blackflash.png",
                $"Parries cause explosions, damage based on block power * parry bonus");
        }

        public static float GetBlackFlashDamage()
        {
            Player player = Player.m_localPlayer;
            ItemDrop.ItemData shield = player.GetCurrentBlocker();
            float value = KSBlock.KSBlockPower(shield, player.GetSkillFactor(Skills.SkillType.Blocking), true, player);
            return value * PerToMult(BlackFlashDamage);
        }



        #endregion BlackFlash

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 BlastWave
        ////////////////////////////////////////////////////////////////////////////////////////
        #region BlastWave
        #region configdef
        public static ConfigEntry<float> BlastWaveScale;
        #endregion configdef

        public static void InitBlastWaveConfigs(ConfigFile cfg)
        {
            BlastWaveScale = cfg.Bind("Perks.BlastWave", "Scale", 100f,
                    AdminCD("% size to increase all AOEs by"));
        }

        public static Perk GetPerkBlastWave()
        {
            return new Perk("Blast Wave",
                "All attacks you inflict that hit an area will have their area of effect doubled.",
                "It was all reduced to rubble... and then, again, to ash.",
                PerkMan.PerkType.BlastWave, Skills.SkillType.Clubs, "Icons/blastwave.png",
                $"{BlastWaveScale.Value}% increase to AOE size");
        }

        public static float GetBlastWaveScaleMult()
        {
            return PerToMult(BlastWaveScale);
        }



        #endregion BlastWave

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 BlockExpert
        ////////////////////////////////////////////////////////////////////////////////////////
        #region BlockExpert
        #region configdef

        #endregion configdef

        public static void InitBlockExpertConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkBlockExpert()
        {
            return new Perk("Blocking Expertise",
                "All forms of damage are now blockable. Yes, even that.",
                "If it breathes, you can block it.",
                PerkMan.PerkType.BlockExpert, Skills.SkillType.None, "Icons/blockexpert.png",
                $"Everything is blockable");
        }




        #endregion BlockExpert

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Boomerang
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Boomerang
        #region configdef

        #endregion configdef

        public static void InitBoomerangConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkBoomerang()
        {
            return new Perk("Just a Guy with a Boomerang",
                "Spears now automatically return to you after they're thrown.",
                "Boomerang! You DO always come back!",
                PerkMan.PerkType.Boomerang, Skills.SkillType.Spears, "Icons/boomerang.png",
                "Returning spears");
        }




        #endregion Boomerang

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Botany
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Botany
        #region configdef

        #endregion configdef

        public static void InitBotanyConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkBotany()
        {
            return new Perk("Botany",
                "Intensive study of the workings of plants has increased your ability read basic information about them, such as time until " +
                "completion or yield.",
                "Turns out, when you study plants, you learn things.",
                PerkMan.PerkType.Botany, SkillMan.Agriculture, "Icons/botany.png",
                "1 level of plant info");
        }




        #endregion Botany

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 BreakMyStride
        ////////////////////////////////////////////////////////////////////////////////////////
        #region BreakMyStride
        #region configdef
        public static ConfigEntry<float> BreakMyStrideMassMod;
        #endregion configdef

        public static void InitBreakMyStrideConfigs(ConfigFile cfg)
        {
            BreakMyStrideMassMod = cfg.Bind("Perks.BreakMyStride", "Mass", 10f,
                AdminCD("% to multiply cart mass by"));
        }

        public static Perk GetPerkBreakMyStride()
        {
            return new Perk("Break My Stride",
                "Carts now reduce your speed by a signficantly less amount.",
                "Ain't nothing gonna...",
                PerkMan.PerkType.BreakMyStride, Skills.SkillType.Run, "Icons/breakmystride.png",
                $"Cart mass reduced to {BreakMyStrideMassMod.Value}%");
        }

        public static float GetBreakMyStrideMassMod()
        {
            return PerToMod(BreakMyStrideMassMod);
        }



        #endregion BreakMyStride

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Butterfly
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Butterfly
        #region configdef

        #endregion configdef

        public static void InitButterflyConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkButterfly()
        {
            return new Perk("Butterfly",
                "You can now jump while in water.",
                "Beautiful form!",
                PerkMan.PerkType.Butterfly, Skills.SkillType.Swim, "Icons/butterfly.png",
                "Jump while swimming");
        }




        #endregion Butterfly

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Cauterize
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Cauterize
        #region configdef

        #endregion configdef

        public static void InitCauterizeConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkCauterize()
        {
            return new Perk("Cauterize",
                $"You've had the incredible idea of setting all your weapons on fire. You now do " +
                $"{CFG.PerkWeaponEnchantDamage.Value}% extra damage as fire damage.",
                "Anything can be a torch, if you're brave enough.",
                PerkMan.PerkType.Cauterize, Skills.SkillType.Axes, "Icons/cauterize.png",
                $"Unlocked fire weapon enchant");
        }




        #endregion Cauterize

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 CloakOfShadows
        ////////////////////////////////////////////////////////////////////////////////////////
        #region CloakOfShadows
        #region configdef
        public static ConfigEntry<float> CloakOfShadowsTimer;
        public static ConfigEntry<float> CloakOfShadowsHealthRegen;
        #endregion configdef

        public static void InitCloakOfShadowsConfigs(ConfigFile cfg)
        {
            CloakOfShadowsTimer = cfg.Bind("Perks.CloakOfShadows", "Timer", 1f,
                AdminCD("seconds between regeneration ticks"));
            CloakOfShadowsHealthRegen = cfg.Bind("Perks.CloakOfShadows", "Health Regen", 5f,
                AdminCD("% of max health to regenerate every tick"));
        }

        public static Perk GetPerkCloakOfShadows()
        {
            float heal = CloakOfShadowsHealthRegen.Value;
            float secs = CloakOfShadowsTimer.Value;
            string seconds = ConditionalPluralize(secs, "second");
            return new Perk("Cloak of Shadows",
                $"While sneaking, you now passively regenerate {heal}% of your max health " +
                $"every {secs} {seconds}.",
                "You have no ally but the darkness.",
                PerkMan.PerkType.CloakOfShadows, Skills.SkillType.Sneak, "Icons/cloakofshadows.png",
                $"While sneaking, {heal}% heal every {secs} {seconds}");
        }

        public static float GetCloakOfShadowsHealthRegenMod()
        {
            return PerToMod(CloakOfShadowsHealthRegen);
        }



        #endregion CloakOfShadows

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 ClosingTheGap
        ////////////////////////////////////////////////////////////////////////////////////////
        #region ClosingTheGap
        #region configdef
        public static ConfigEntry<float> ClosingTheGapForce;
        public static ConfigEntry<float> ClosingTheGapStaminaRegain;
        #endregion configdef

        public static void InitClosingTheGapConfigs(ConfigFile cfg)
        {
            ClosingTheGapForce = cfg.Bind("Perks.ClosingTheGap", "Distance", 80f,
                AdminCD("force towards enemy on stagger"));
            ClosingTheGapStaminaRegain = cfg.Bind("Perks.ClosingTheGap", "Stamina", 5f,
                AdminCD("amount of stamina to regain on stagger"));
        }

        public static Perk GetPerkClosingTheGap()
        {
            float stamina = ClosingTheGapStaminaRegain.Value;
            return new Perk("Closing The Gap",
                $"Every time you stagger an enemy, you regain {stamina} stamina and skip towards them.",
                "Perfect for when your flight reflex is broken.",
                PerkMan.PerkType.ClosingTheGap, Skills.SkillType.Clubs, "Icons/closingthegap.png",
                $"{stamina} stamina and gap close on stagger");
        }




        #endregion ClosingTheGap

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 ControlledDemo
        ////////////////////////////////////////////////////////////////////////////////////////
        #region ControlledDemo
        #region configdef
        public static ConfigEntry<float> ControlledDemoDetectRange;
        public static ConfigEntry<float> ControlledDemoForce;
        #endregion configdef

        public static void InitControlledDemoConfigs(ConfigFile cfg)
        {
            ControlledDemoDetectRange = cfg.Bind("Perks.ControlledDemo", "Detection Range", 50f,
                AdminCD("Distance away trees can detect nearby enemies or trees"));
            ControlledDemoForce = cfg.Bind("Perks.ControlledDemo", "Force", 400f,
                AdminCD("Force that logs are launched at nearby enemies or trees"));
        }

        public static Perk GetPerkControlledDemo()
        {
            return new Perk("Controlled Demolition",
                "When you chop down a tree, the resulting log will blast towards any nearby enemies. If no enemies are " +
                "nearby, it will instead target another tree. Be careful not to stand in it's way!",
                "Instructions: Put tree between you and enemy",
                PerkMan.PerkType.ControlledDemo, Skills.SkillType.WoodCutting, "Icons/controlleddemo.png",
                "Fallen logs fling at enemies or trees");
        }




        #endregion ControlledDemo

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 CouchedLance
        ////////////////////////////////////////////////////////////////////////////////////////
        #region CouchedLance
        #region configdef

        #endregion configdef

        public static void InitCouchedLanceConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkCouchedLance()
        {
            return new Perk("Couched Lance",
                "After standing still for several seconds, you now gain a large boost to damage.",
                "Usually for charging cavalry, now for stampeding Loxes.",
                PerkMan.PerkType.CouchedLance, Skills.SkillType.None, "Icons/couchedlance.png");
        }




        #endregion CouchedLance

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 CoupDeBurst
        ////////////////////////////////////////////////////////////////////////////////////////
        #region CoupDeBurst
        #region configdef

        #endregion configdef

        public static void InitCoupDeBurstConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkCoupDeBurst()
        {
            return new Perk("Coup De Burst",
                "You can now activate a huge explosion, sending your ship flying through the air. Better" +
                " hold on!",
                "And so men set sights on the Grand Line, in pursuit of their dreams.",
                PerkMan.PerkType.CoupDeBurst, Skills.SkillType.None, "Icons/coupdeburst.png");
        }




        #endregion CoupDeBurst

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 CriticalBlow
        ////////////////////////////////////////////////////////////////////////////////////////
        #region CriticalBlow
        #region configdef

        #endregion configdef

        public static void InitCriticalBlowConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkCriticalBlow()
        {
            return new Perk("Critical Blow",
                "You have a 10% chance to deal double damage on each hit.",
                "The kind of hit your DM would be embarassed to narrate.",
                PerkMan.PerkType.CriticalBlow, Skills.SkillType.None, "Icons/criticalblow.png");
        }




        #endregion CriticalBlow

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Deadeye
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Deadeye
        #region configdef

        #endregion configdef

        public static void InitDeadeyeConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkDeadeye()
        {
            return new Perk("Deadeye",
                "You can now throw your knives, dealing half backstab damage.",
                "They don't come back to you, though.",
                PerkMan.PerkType.Deadeye, Skills.SkillType.None, "Icons/deadeye.png");
        }




        #endregion Deadeye

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Decapitation
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Decapitation
        #region configdef

        #endregion configdef

        public static void InitDecapitationConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkDecapitation()
        {
            return new Perk("Decapitate",
                "Trophies can be eaten to gain axe experience.",
                "My ancestors are smiling at me, imperial. Can you say the same?",
                PerkMan.PerkType.Decapitation, Skills.SkillType.None, "Icons/decapitation.png");
        }




        #endregion Decapitation

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 DidntHurt
        ////////////////////////////////////////////////////////////////////////////////////////
        #region DidntHurt
        #region configdef

        #endregion configdef

        public static void InitDidntHurtConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkDidntHurt()
        {
            return new Perk("That Didn't Even Hurt",
                "Any creature that deals less than 5 damage to you, while you're not blocking, now gets staggered out of fear.",
                "Is that all you've got? Pathetic.",
                PerkMan.PerkType.DidntHurt, Skills.SkillType.None, "Icons/didnthurt.png");
        }




        #endregion DidntHurt

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 DisarmingDefense
        ////////////////////////////////////////////////////////////////////////////////////////
        #region DisarmingDefense
        #region configdef

        #endregion configdef

        public static void InitDisarmingDefenseConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkDisarmingDefense()
        {
            return new Perk("Disarming Smile",
                "Knives now use their backstab bonus instead of their parry bonus for parrying.",
                "No better defense than an alarmingly quick offense.",
                PerkMan.PerkType.DisarmingDefense, Skills.SkillType.None, "Icons/disarmingsmile.png");
        }




        #endregion DisarmingDefense

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 ESP
        ////////////////////////////////////////////////////////////////////////////////////////
        #region ESP
        #region configdef

        #endregion configdef

        public static void InitESPConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkESP()
        {
            return new Perk("Extrasensory perception",
                "You can now see lines highlighting enemy sight cones while you're sneaking.",
                "That, or you're just starting to see things now.",
                PerkMan.PerkType.ESP, Skills.SkillType.None, "Icons/esp.png");
        }




        #endregion ESP

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Efficiency
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Efficiency
        #region configdef

        #endregion configdef

        public static void InitEfficiencyConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkEfficiency()
        {
            return new Perk("Efficiency",
                "New building techniques allow you to build new constructions at half the cost.",
                "It's not 'cutting corners'... If those corners were completely unnecessary!",
                PerkMan.PerkType.Efficiency, Skills.SkillType.None, "Icons/efficiency.png");
        }




        #endregion Efficiency

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Einherjar
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Einherjar
        #region configdef

        #endregion configdef

        public static void InitEinherjarConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkEinherjar()
        {
            return new Perk("Blessing of the Einherjar",
                "All of your projectiles now home towards the nearest target.",
                "Alone no longer.",
                PerkMan.PerkType.Einherjar, Skills.SkillType.None, "Icons/einherjar.png");
        }




        #endregion Einherjar

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Engineer
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Engineer
        #region configdef

        #endregion configdef

        public static void InitEngineerConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkEngineer()
        {
            return new Perk("Structural Engineer",
                "Advanced knowledge of civil engineering secrets have caused your support pillars" +
                " and beams to become almost three times as sturdy as usual.",
                "It's triangles all the way down.",
                PerkMan.PerkType.Engineer, Skills.SkillType.None, "Icons/engineer.png");
        }




        #endregion Engineer

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 FalconKick
        ////////////////////////////////////////////////////////////////////////////////////////
        #region FalconKick
        #region configdef

        #endregion configdef

        public static void InitFalconKickConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkFalconKick()
        {
            return new Perk("Falcon Kick",
                "Your kick now causes you to backflip into the air, and sends enemies flying.",
                "K.O.!",
                PerkMan.PerkType.FalconKick, Skills.SkillType.None, "Icons/falconkick.png");
        }




        #endregion FalconKick

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 FirstMate
        ////////////////////////////////////////////////////////////////////////////////////////
        #region FirstMate
        #region configdef

        #endregion configdef

        public static void InitFirstMateConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkFirstMate()
        {
            return new Perk("First Mate",
                "Whenever you are on a ship, but not the captain, a portion of your sailing level is added to" +
                " the captain's when determining sailing buffs.",
                "Every man does their part.",
                PerkMan.PerkType.FirstMate, Skills.SkillType.None, "Icons/firstmate.png");
        }




        #endregion FirstMate

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 FishersBoon
        ////////////////////////////////////////////////////////////////////////////////////////
        #region FishersBoon
        #region configdef

        #endregion configdef

        public static void InitFishersBoonConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkFishersBoon()
        {
            return new Perk("Spearfisher's Boon",
                "Whenever you throw a spear, a second spear will be thrown automatically towards the nearest enemy.",
                "Where does the second spear come from? A fisherman never tells.",
                PerkMan.PerkType.FishersBoon, Skills.SkillType.None, "Icons/fishersboon.png");
        }




        #endregion FishersBoon

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 FiveStarChef
        ////////////////////////////////////////////////////////////////////////////////////////
        #region FiveStarChef
        #region configdef

        #endregion configdef

        public static void InitFiveStarChefConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkFiveStarChef()
        {
            return new Perk("Five Star Chef",
                "Hours of experimentation have led you to understanding what works and what doesn't. " +
                "You have come up with several new recipes, both tasty and healthy.",
                "Now you can call your compatriots idiot sandwiches and they can't complain.",
                PerkMan.PerkType.FiveStarChef, Skills.SkillType.None, "Icons/fivestarchef.png");
        }




        #endregion FiveStarChef

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 FourStomachs
        ////////////////////////////////////////////////////////////////////////////////////////
        #region FourStomachs
        #region configdef

        #endregion configdef

        public static void InitFourStomachsConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkFourStomachs()
        {
            return new Perk("Four Stomachs",
                "All your food decays 25% slower, essentially making it last 50% longer.",
                "I'm not calling you a cow... But...",
                PerkMan.PerkType.FourStomachs, Skills.SkillType.None, "Icons/fourstomachs.png");
        }




        #endregion FourStomachs

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Fragmentation
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Fragmentation
        #region configdef

        #endregion configdef

        public static void InitFragmentationConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkFragmentation()
        {
            return new Perk("Frag Mine",
                "When you break a rock, if there are any enemies nearby, the rocks " +
                "and ore first pelt them for sizable damage before returning to your feet.",
                "It's not the explosion that gets you, it's the fragments.",
                Perks.PerkMan.PerkType.Fragmentation, Skills.SkillType.None, "Icons/fragmentation.png");
        }




        #endregion Fragmentation

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Frugal
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Frugal
        #region configdef

        #endregion configdef

        public static void InitFrugalConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkFrugal()
        {
            return new Perk("Frugal",
                "You have a 50% chance not to expend ammo.",
                "For the eco-friendly viking warrior.",
                PerkMan.PerkType.Frugal, Skills.SkillType.None, "Icons/frugal.png");
        }




        #endregion Frugal

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 GiantSmash
        ////////////////////////////////////////////////////////////////////////////////////////
        #region GiantSmash
        #region configdef

        #endregion configdef

        public static void InitGiantSmashConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkGiantSmash()
        {
            return new Perk("Giant Smash",
                "A special attack that ragdolls enemies into the air.",
                "A power even the dragonborn never mastered.",
                PerkMan.PerkType.GiantSmash, Skills.SkillType.None, "Icons/giantsmash.png");
        }




        #endregion GiantSmash

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 GodSlayingStrike
        ////////////////////////////////////////////////////////////////////////////////////////
        #region GodSlayingStrike
        #region configdef

        #endregion configdef

        public static void InitGodSlayingStrikeConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkGodSlayingStrike()
        {
            return new Perk("God Slayer",
                "Damage dealt to bosses is now increased by 50%.",
                "A god, are you? Good. I was just thinking there was an empty wall above my hearth.",
                PerkMan.PerkType.GodSlayingStrike, Skills.SkillType.None, "Icons/godslayer.png");
        }




        #endregion GodSlayingStrike

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 GoombaStomp
        ////////////////////////////////////////////////////////////////////////////////////////
        #region GoombaStomp
        #region configdef

        #endregion configdef

        public static void InitGoombaStompConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkGoombaStomp()
        {
            return new Perk("Goomba Stomp",
                "You now deal damage when landing on an enemy's head.",
                "Yahoo!",
                PerkMan.PerkType.GoombaStomp, Skills.SkillType.None, "Icons/goombastomp.png");
        }




        #endregion GoombaStomp

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 GreenThumb
        ////////////////////////////////////////////////////////////////////////////////////////
        #region GreenThumb
        #region configdef

        #endregion configdef

        public static void InitGreenThumbConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkGreenThumb()
        {
            return new Perk("Green Thumb",
                "Your natural talent for growing plants has manifested, giving you an extra insight into information such as time until " +
                "completion or yield.",
                "Much better than a yellow foot, which you ought to bring up with your doctor.",
                PerkMan.PerkType.GreenThumb, SkillMan.Agriculture, "Icons/greenthumb.png",
                "1 level of plant info");
        }




        #endregion GreenThumb

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 GutAndRun
        ////////////////////////////////////////////////////////////////////////////////////////
        #region GutAndRun
        #region configdef

        #endregion configdef

        public static void InitGutAndRunConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkGutAndRun()
        {
            return new Perk("Gut and Run",
                "Each attack against an enemy with full health will now cause them to start bleeding.",
                "Sorry to drop in out of nowhere, but...",
                PerkMan.PerkType.GutAndRun, Skills.SkillType.None, "Icons/gutandrun.png");
        }




        #endregion GutAndRun

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Harvester
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Harvester
        #region configdef

        #endregion configdef

        public static void InitHarvesterConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkHarvester()
        {
            return new Perk("Combine Harvester",
                "You have gotten so experienced that entire fields will seem to pick themselves before your very eyes. " +
                "You now harvest in a large radius.",
                "The botanical version of strip mining.",
                PerkMan.PerkType.Harvester, Skills.SkillType.None, "Icons/harvester.png");
        }




        #endregion Harvester

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 HeartOfTheForest
        ////////////////////////////////////////////////////////////////////////////////////////
        #region HeartOfTheForest
        #region configdef

        #endregion configdef

        public static void InitHeartOfTheForestConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkHeartOfTheForest()
        {
            return new Perk("Heart of the Forest",
                "Every hit on a tree stacks up a buff that reduces the stagger damage you take.",
                "For when you literally can't be bothered to entertain the greydwarves swarming you.",
                PerkMan.PerkType.HeartOfTheForest, Skills.SkillType.None, "Icons/heartoftheforest.png");
        }




        #endregion HeartOfTheForest

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 HeartOfTheMonkey
        ////////////////////////////////////////////////////////////////////////////////////////
        #region HeartOfTheMonkey
        #region configdef

        #endregion configdef

        public static void InitHeartOfTheMonkeyConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkHeartOfTheMonkey()
        {
            return new Perk("Heart of the Monkey",
                "By holding space while in the air, you can now cling to walls such as trees and cliffs. Drains stamina as though you were running.",
                "The will of D now lives inside you.",
                PerkMan.PerkType.HeartOfTheMonkey, Skills.SkillType.None, "Icons/heartofthemonkey.png");
        }




        #endregion HeartOfTheMonkey

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 HermesBoots
        ////////////////////////////////////////////////////////////////////////////////////////
        #region HermesBoots
        #region configdef

        #endregion configdef

        public static void InitHermesBootsConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkHermesBoots()
        {
            return new Perk("Hermes' Boots",
                "Running for an extended period of time without interruption will cause you to slowly accelerate " +
                "up to a point.",
                "Your world has been blessed with Speed!",
                PerkMan.PerkType.HermesBoots, Skills.SkillType.None, "Icons/hermesboots.png");
        }




        #endregion HermesBoots

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 HideInPlainSight
        ////////////////////////////////////////////////////////////////////////////////////////
        #region HideInPlainSight
        #region configdef

        #endregion configdef

        public static void InitHideInPlainSightConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkHideInPlainSight()
        {
            return new Perk("Hide in Plain Sight",
                "No matter the light conditions, you are always considered to be in pitch blackness.",
                "Much more effective than burying your head.",
                PerkMan.PerkType.HideInPlainSight, Skills.SkillType.None, "Icons/hideinplainsight.png");
        }




        #endregion HideInPlainSight

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Highlander
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Highlander
        #region configdef

        #endregion configdef

        public static void InitHighlanderConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkHighlander()
        {
            return new Perk("Highlander",
                "Increases your max health by 100.",
                "There can only be one - and it will be you.",
                PerkMan.PerkType.Highlander, Skills.SkillType.None, "Icons/highlander.png");
        }




        #endregion Highlander

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Hraesvelg
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Hraesvelg
        #region configdef

        #endregion configdef

        public static void InitHraesvelgConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkHraesvelg()
        {
            return new Perk("Wings of Hraesvelg",
                "Shots fired will now be accompanied by a pillar of wind, knocking back enemies that have gotten too close as well as yourself.",
                "The great eagle guides you. Fall damage reduction not included.",
                PerkMan.PerkType.Hraesvelg, Skills.SkillType.None, "Icons/hraesvelg.png");
        }




        #endregion Hraesvelg

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Hydrodynamic
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Hydrodynamic
        #region configdef

        #endregion configdef

        public static void InitHydrodynamicConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkHydrodynamic()
        {
            return new Perk("Hydrodynamic Form",
                "You are no longer affected by the 'Wet' Debuff.",
                "It just... slides right off.",
                PerkMan.PerkType.Hydrodynamic, Skills.SkillType.None, "Icons/hydrodynamic.png");
        }




        #endregion Hydrodynamic

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Iai
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Iai
        #region configdef

        #endregion configdef

        public static void InitIaiConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkIai()
        {
            return new Perk("Iai",
                "When you dodgeroll through an enemy, they take damage as though you backstabbed them. Has a " +
                "20 second cooldown.",
                "Lion song.",
                PerkMan.PerkType.Iai, Skills.SkillType.None, "Icons/iai.png");
        }




        #endregion Iai

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 IronSkin
        ////////////////////////////////////////////////////////////////////////////////////////
        #region IronSkin
        #region configdef

        #endregion configdef

        public static void InitIronSkinConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkIronSkin()
        {
            return new Perk("Iron Skin",
                "While you're not wearing armor, you get armor based on half of your unarmed damage.",
                "Breaking all those boards finally paid off.",
                PerkMan.PerkType.IronSkin, Skills.SkillType.None, "Icons/ironskin.png");
        }




        #endregion IronSkin

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 JoJoPose
        ////////////////////////////////////////////////////////////////////////////////////////
        #region JoJoPose
        #region configdef

        #endregion configdef

        public static void InitJoJoPoseConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkJoJoPose()
        {
            return new Perk("JoJo Pose",
                "When treading water, you now regenerate stamina.",
                "Become the kind of man Jotaro Kujo expects you to be",
                PerkMan.PerkType.JoJoPose, Skills.SkillType.None, "Icons/jojopose.png");
        }




        #endregion JoJoPose

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Jotunn
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Jotunn
        #region configdef

        #endregion configdef

        public static void InitJotunnConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkJotunn()
        {
            return new Perk("Jotunn",
                "Your overall size increases. You'll have to duck through short doorways now, but your carrying " +
                "capacity and health will increase.",
                "What's the weather like up there?",
                PerkMan.PerkType.Jotunn, Skills.SkillType.None, "Icons/jotunn.png");
        }




        #endregion Jotunn

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Juggernaut
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Juggernaut
        #region configdef

        #endregion configdef

        public static void InitJuggernautConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkJuggernaut()
        {
            return new Perk("I'm the Juggernaut",
                "Running into obstacles like trees and rocks will simply cause them to get obliterated " +
                "rather than slowing you down.",
                "Now you just need to find an immovable object.",
                PerkMan.PerkType.Juggernaut, Skills.SkillType.None, "Icons/juggernaut.png");
        }




        #endregion Juggernaut

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 KeenNose
        ////////////////////////////////////////////////////////////////////////////////////////
        #region KeenNose
        #region configdef

        #endregion configdef

        public static void InitKeenNoseConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkKeenNose()
        {
            return new Perk("Keen Nose",
                "Your nose is so powerful that you can detect what's in a dish without even seeing it. This lets you garner addition information " +
                "about cooking food, such as time until completion.",
                "Smells like... Cardamum and basil. And the lightest hint of lime...",
                PerkMan.PerkType.KeenNose, Skills.SkillType.None, "Icons/keennose.png");
        }




        #endregion KeenNose

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 LightningReflex
        ////////////////////////////////////////////////////////////////////////////////////////
        #region LightningReflex
        #region configdef

        #endregion configdef

        public static void InitLightningReflexConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkLightningReflex()
        {
            return new Perk("Lightning Reflexes",
                "Automatically catch arrows and rocks that enter your range of attack.",
                "Nothing goes over my head. My reflexes are too fast. I would catch it.",
                PerkMan.PerkType.LightningReflex, Skills.SkillType.None, "Icons/lightningreflex.png");
        }




        #endregion LightningReflex

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 LivingStone
        ////////////////////////////////////////////////////////////////////////////////////////
        #region LivingStone
        #region configdef

        #endregion configdef

        public static void InitLivingStoneConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkLivingStone()
        {
            return new Perk("Living Stone",
                "All knockback effects on you are greatly reduced, and you gain 10 flat armor.",
                "Now you just need to find an unstoppable force.",
                PerkMan.PerkType.LivingStone, Skills.SkillType.None, "Icons/livingstone.png");
        }




        #endregion LivingStone

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 LodeBearingStone
        ////////////////////////////////////////////////////////////////////////////////////////
        #region LodeBearingStone
        #region configdef

        #endregion configdef

        public static void InitLodeBearingStoneConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkLodeBearingStone()
        {
            return new Perk("Lode (Bearing) Stone",
                "When you start hitting an ore vein, a point of light appears. Hitting this point of light will " +
                "cause the entire vein to take full damage.",
                "Copper veins, watch out. I'm coming.",
                PerkMan.PerkType.LodeBearingStone, Skills.SkillType.None, "Icons/lodebearingstone.png");
        }




        #endregion LodeBearingStone

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 LogHorizon
        ////////////////////////////////////////////////////////////////////////////////////////
        #region LogHorizon
        #region configdef

        #endregion configdef

        public static void InitLogHorizonConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkLogHorizon()
        {
            return new Perk("Log Horizon",
                "Fallen logs can be picked up to be used as a single use attack, dealing incredible damage.",
                "You swing the log. The enemy sees the horizon... Now <i>that's</i> living in the database.",
                PerkMan.PerkType.LogHorizon, Skills.SkillType.None, "Icons/loghorizon.png");
        }




        #endregion LogHorizon

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 LokisGift
        ////////////////////////////////////////////////////////////////////////////////////////
        #region LokisGift
        #region configdef

        #endregion configdef

        public static void InitLokisGiftConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkLokisGift()
        {
            return new Perk("Loki's Gift",
                "Teleport to a target's back, backstabbing them immediately. Distance scales on sneak skill.",
                "Burdened with glorious purpose.",
                PerkMan.PerkType.LokisGift, Skills.SkillType.None, "Icons/lokisgift.png");
        }




        #endregion LokisGift

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Magnetic
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Magnetic
        #region configdef

        #endregion configdef

        public static void InitMagneticConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkMagnetic()
        {
            return new Perk("Magnetic Personality",
                "You are now magnetic, dramatically increasing your auto-pickup range.",
                "All that time rubbing metals together has finally affected you. Sure, that's how magnets work, why not?",
                PerkMan.PerkType.Magnetic, Skills.SkillType.None, "Icons/magnetic.png");
        }




        #endregion Magnetic

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 ManOverboard
        ////////////////////////////////////////////////////////////////////////////////////////
        #region ManOverboard
        #region configdef

        #endregion configdef

        public static void InitManOverboardConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkManOverboard()
        {
            return new Perk("Man Overboard",
                "While captaining a ship, you can now press B to automatically suck in all nearby players.",
                "Much better option than everyone jumping ship.",
                PerkMan.PerkType.ManOverboard, Skills.SkillType.None, "Icons/manoverboard.png");
        }




        #endregion ManOverboard

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 MarathonSwimmer
        ////////////////////////////////////////////////////////////////////////////////////////
        #region MarathonSwimmer
        #region configdef

        #endregion configdef

        public static void InitMarathonSwimmerConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkMarathonSwimmer()
        {
            return new Perk("Marathon Swimmer",
                "While you're swimming in the ocean, you gain a slowly stacking bonus to move speed, damage reduction, and stamina " +
                "regeneration.",
                "You're just on a roll.",
                PerkMan.PerkType.MarathonSwimmer, Skills.SkillType.None, "Icons/marathonswimmer.png");
        }




        #endregion MarathonSwimmer

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 MarketGardener
        ////////////////////////////////////////////////////////////////////////////////////////
        #region MarketGardener
        #region configdef

        #endregion configdef

        public static void InitMarketGardenerConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkMarketGardener()
        {
            return new Perk("The Market Gardener",
                "You now do 50% extra damage to enemies while in the air.",
                "Screamin' Eagles!",
                PerkMan.PerkType.MarketGardener, Skills.SkillType.None, "Icons/marketgardener.png");
        }




        #endregion MarketGardener

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 MassiveStature
        ////////////////////////////////////////////////////////////////////////////////////////
        #region MassiveStature
        #region configdef

        #endregion configdef

        public static void InitMassiveStatureConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkMassiveStature()
        {
            return new Perk("Massive Stature",
                "Increases your overall size even more. Your skin toughens like hide, causing you to be resistant to slashing, lightning, " +
                "and frost damage.",
                "You're gonna need to build a bigger house.",
                PerkMan.PerkType.MassiveStature, Skills.SkillType.None, "Icons/massivestature.png");
        }




        #endregion MassiveStature

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 MasterOfTheLog
        ////////////////////////////////////////////////////////////////////////////////////////
        #region MasterOfTheLog
        #region configdef

        #endregion configdef

        public static void InitMasterOfTheLogConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkMasterOfTheLog()
        {
            return new Perk("Master of the Log",
                "You are now immune to falling logs.",
                "You will be the last figure standing when the smoke clears.",
                PerkMan.PerkType.MasterOfTheLog, Skills.SkillType.None, "Icons/masterofthelog.png");
        }




        #endregion MasterOfTheLog

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Meditation
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Meditation
        #region configdef

        #endregion configdef

        public static void InitMeditationConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkMeditation()
        {
            return new Perk("I Studied the Blade",
                "Meditate by using the /sit emote shortly after battle to massively increase your " +
                "experience gain.",
                "Perfect for use while all your peers are partying.",
                PerkMan.PerkType.Meditation, Skills.SkillType.None, "Icons/meditation.png");
        }




        #endregion Meditation

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 MeteorDrop
        ////////////////////////////////////////////////////////////////////////////////////////
        #region MeteorDrop
        #region configdef

        #endregion configdef

        public static void InitMeteorDropConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkMeteorDrop()
        {
            return new Perk("Meteor Drop",
                "When you fall far enough, you make a huge crater and send a shockwave that deals damage " +
                "based on how far you fell.",
                "No promises that you'll survive it.",
                PerkMan.PerkType.MeteorDrop, Skills.SkillType.None, "Icons/meteordrop.png");
        }




        #endregion MeteorDrop

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Mjolnir
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Mjolnir
        #region configdef

        #endregion configdef

        public static void InitMjolnirConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkMjolnir()
        {
            return new Perk("Mjolnir",
                $"You deal an additional {CFG.PerkWeaponEnchantDamage.Value}% of damage as lightning damage.",
                "Whosoever holds this hammer, if they be worthy, shall possess the power of... you?",
                PerkMan.PerkType.Mjolnir, Skills.SkillType.Clubs, "Icons/mjolnir.png",
                "Unlocked lightning enchant");
        }




        #endregion Mjolnir

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 MountainGoat
        ////////////////////////////////////////////////////////////////////////////////////////
        #region MountainGoat
        #region configdef

        #endregion configdef

        public static void InitMountainGoatConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkMountainGoat()
        {
            return new Perk("Mountain Goat",
                "You can now run up nearly sheer angled surfaces, dramatically increasing your mountain mobility.",
                "The undisputed GOAT.",
                PerkMan.PerkType.MountainGoat, Skills.SkillType.None, "Icons/mountaingoat.png");
        }




        #endregion MountainGoat

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Nailgun
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Nailgun
        #region configdef

        #endregion configdef

        public static void InitNailgunConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkNailgun()
        {
            return new Perk("Ye Olde Nailgune",
                "Your strength and precision have caused your repair efforts to accelerate rapidly. You now " +
                "repair in a large radius with every swing of the hammer.",
                "When all you have is a hammer...",
                PerkMan.PerkType.Nailgun, Skills.SkillType.None, "Icons/nailgun.png");
        }




        #endregion Nailgun

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Nutrition
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Nutrition
        #region configdef

        #endregion configdef

        public static void InitNutritionConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkNutrition()
        {
            return new Perk("High Nutritional Content",
                "You've created a new form of cooking that packs tons of healthy nutrients into the " +
                "dish. You can now imbue your dishes with resistances to blunt, slashing, and piercing, " +
                "or move speed.",
                "The taste, however...",
                PerkMan.PerkType.Nutrition, Skills.SkillType.None, "Icons/nutrition.png");
        }




        #endregion Nutrition

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 OdinJump
        ////////////////////////////////////////////////////////////////////////////////////////
        #region OdinJump
        #region configdef

        #endregion configdef

        public static void InitOdinJumpConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkOdinJump()
        {
            return new Perk("Odin's Jump",
                "After concentration, you safely leap thousands of meters forwards.",
                "Now possible without getting hit by a giant first!",
                PerkMan.PerkType.OdinJump, Skills.SkillType.None, "Icons/odinjump.png");
        }




        #endregion OdinJump

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 OfferToUllr
        ////////////////////////////////////////////////////////////////////////////////////////
        #region OfferToUllr
        #region configdef

        #endregion configdef

        public static void InitOfferToUllrConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkOfferToUllr()
        {
            return new Perk("Offering to Ullr",
                "Regular offerings to Ullr have blessed your luck, causing monsters to drop better items.",
                "The norse god of the hunt recognizes your skill.",
                PerkMan.PerkType.OfferToUllr, Skills.SkillType.None, "Icons/ullr.png");
        }




        #endregion OfferToUllr

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 PandemoniumPoint
        ////////////////////////////////////////////////////////////////////////////////////////
        #region PandemoniumPoint
        #region configdef

        #endregion configdef

        public static void InitPandemoniumPointConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkPandemoniumPoint()
        {
            return new Perk("Pandemonium Point",
                "Standing in place for 20 seconds near a tree will cause a special target to appear. Striking it will cause a " +
                "shockwave that fells an entire forest.",
                "You might want to take cover first.",
                PerkMan.PerkType.PandemoniumPoint, Skills.SkillType.None, "Icons/pandemoniumpoint.png");
        }




        #endregion PandemoniumPoint

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 PerfectCombo
        ////////////////////////////////////////////////////////////////////////////////////////
        #region PerfectCombo
        #region configdef

        #endregion configdef

        public static void InitPerfectComboConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkPerfectCombo()
        {
            return new Perk("Perfect Combo",
                "Each uninterrupted hit now stacks a damage buff. Combo is lost on taking damage.",
                "C-C-COMBO BREAKER!",
                PerkMan.PerkType.PerfectCombo, Skills.SkillType.None, "Icons/perfectcombo.png");
        }




        #endregion PerfectCombo

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 PlusUltra
        ////////////////////////////////////////////////////////////////////////////////////////
        #region PlusUltra
        #region configdef

        #endregion configdef

        public static void InitPlusUltraConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkPlusUltra()
        {
            return new Perk("Plus Ultra",
                "Enemies can no longer be resistant to blunt damage.",
                "What do you do when punching something isn't working? Just punch it harder, obviously.",
                PerkMan.PerkType.PlusUltra, Skills.SkillType.None, "Icons/plusultra.png");
        }




        #endregion PlusUltra

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 PowerDraw
        ////////////////////////////////////////////////////////////////////////////////////////
        #region PowerDraw
        #region configdef

        #endregion configdef

        public static void InitPowerDrawConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkPowerDraw()
        {
            return new Perk("Power Draw",
                "A special shot that takes much more energy to draw, but fires with incredible speed and damage.",
                "For when you absolutely need that deer gone.",
                PerkMan.PerkType.PowerDraw, Skills.SkillType.None, "Icons/powerdraw.png");
        }




        #endregion PowerDraw

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 PressurePoints
        ////////////////////////////////////////////////////////////////////////////////////////
        #region PressurePoints
        #region configdef
        #endregion configdef

        public static void InitPressurePointsConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkPressurePoints()
        {
            return new Perk("Pressure Points",
                "Each attack against an enemy causes a stacking debuff, slowing them and decreasing" +
                " their damage dealt.",
                "Like acupuncture, with significantly less medical benefit.",
                PerkMan.PerkType.PressurePoints, Skills.SkillType.None, "Icons/pressurepoints.png");
        }




        #endregion PressurePoints

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 RammingSpeed
        ////////////////////////////////////////////////////////////////////////////////////////
        #region RammingSpeed
        #region configdef

        #endregion configdef

        public static void InitRammingSpeedConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkRammingSpeed()
        {
            return new Perk("Ramming Speed",
                "For every 3 seconds that you sail without changing directon, you slowly gain a stacking movespeed increase. The bonus increases " +
                "multiplicatively and does not have a limit.",
                "Brace for impact! Does not come with damage reduction.",
                PerkMan.PerkType.RammingSpeed, Skills.SkillType.None, "Icons/rammingspeed.png");
        }




        #endregion RammingSpeed

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 ResponsibleLumberjack
        ////////////////////////////////////////////////////////////////////////////////////////
        #region ResponsibleLumberjack
        #region configdef

        #endregion configdef

        public static void InitResponsibleLumberjackConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkResponsibleLumberjack()
        {
            return new Perk("Responsible Lumberjacking",
                "Stumps now get destroyed in one hit, and if possible, will automatically get replanted as saplings.",
                "Sustainable farming is the work of the finest lumberjacks!",
                PerkMan.PerkType.ResponsibleLumberjack, Skills.SkillType.None, "Icons/responsiblelumberjack.png");
        }




        #endregion ResponsibleLumberjack

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 RockDodger
        ////////////////////////////////////////////////////////////////////////////////////////
        #region RockDodger
        #region configdef

        #endregion configdef

        public static void InitRockDodgerConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkRockDodger()
        {
            return new Perk("Rock Dodger",
                "Significantly improves several ship manuverability values.",
                "I am a leaf on the wind.",
                PerkMan.PerkType.RockDodger, Skills.SkillType.None, "Icons/rockdodger.png");
        }




        #endregion RockDodger

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 RockHauler
        ////////////////////////////////////////////////////////////////////////////////////////
        #region RockHauler
        #region configdef

        #endregion configdef

        public static void InitRockHaulerConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkRockHauler()
        {
            return new Perk("Rock Hauler",
                "Rocks and metals now weigh 80% less.",
                "When you've handled so many, they all just sorta phase together.",
                PerkMan.PerkType.RockHauler, Skills.SkillType.None, "Icons/rockhauler.png");
        }




        #endregion RockHauler

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 RunedArrows
        ////////////////////////////////////////////////////////////////////////////////////////
        #region RunedArrows
        #region configdef

        #endregion configdef

        public static void InitRunedArrowsConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkRunedArrows()
        {
            return new Perk("Runed Arrows",
                "There's a 75% chance for your arrows to automatically return to you - the other 25% disappear.",
                "It takes pretty long to rune each arrowhead, but luckily, that part happens off-screen.",
                PerkMan.PerkType.RunedArrows, Skills.SkillType.None, "Icons/runedarrows.png");
        }




        #endregion RunedArrows

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 SeaShanty
        ////////////////////////////////////////////////////////////////////////////////////////
        #region SeaShanty
        #region configdef

        #endregion configdef

        public static void InitSeaShantyConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkSeaShanty()
        {
            return new Perk("Sea Shanty",
                "All crew members aboard your boat gain increased move speed, damage reduction, and damage.",
                "Anyone who boards your vessel will have a belly full of mead and a throat full of song.",
                PerkMan.PerkType.SeaShanty, Skills.SkillType.None, "Icons/seashanty.png");
        }




        #endregion SeaShanty

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 SeedSatchel
        ////////////////////////////////////////////////////////////////////////////////////////
        #region SeedSatchel
        #region configdef

        #endregion configdef

        public static void InitSeedSatchelConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkSeedSatchel()
        {
            return new Perk("Seed Satchel",
                "You now carry around an extra pouch, accessible from the inventory, which can store an unlimited number of seeds.",
                "What is a botanist without seeds?",
                PerkMan.PerkType.SeedSatchel, Skills.SkillType.None, "Icons/seedsatchel.png");
        }




        #endregion SeedSatchel

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Seeding
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Seeding
        #region configdef

        #endregion configdef

        public static void InitSeedingConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkSeeding()
        {
            return new Perk("Mass Seeding",
                "Pre-dug holes allow you to sprinkle seeds and vastly improve your planting rate. You now plant" +
                " in a large radius.",
                "Gaia ain't got nothing on you.",
                PerkMan.PerkType.Seeding, Skills.SkillType.None, "Icons/seeding.png");
        }




        #endregion Seeding

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 ShatterStrike
        ////////////////////////////////////////////////////////////////////////////////////////
        #region ShatterStrike
        #region configdef

        #endregion configdef

        public static void InitShatterStrikeConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkShatterStrike()
        {
            return new Perk("Shatterstrike",
                "You build up stacks every second over time while holding an axe. Each stack increases the damage you deal to the first tree " +
                "you hit. If you have maximum stacks, 60, hitting a tree will cause it to immediately get turned into wood. There's a 10 second " +
                "cooldown before the stacks begin building again.",
                "Be the kind of man Captain America believes you can be.",
                PerkMan.PerkType.ShatterStrike, Skills.SkillType.None, "Icons/shatterstrike.png");
        }




        #endregion ShatterStrike

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 SilentSprinter
        ////////////////////////////////////////////////////////////////////////////////////////
        #region SilentSprinter
        #region configdef

        #endregion configdef

        public static void InitSilentSprinterConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkSilentSprinter()
        {
            return new Perk("Silent Sprinter",
                "Adds 30% of your run speed to your sneak speed.",
                "Get back here, you goddamn boar -",
                PerkMan.PerkType.SilentSprinter, Skills.SkillType.None, "Icons/silentsprinter.png");
        }




        #endregion SilentSprinter

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 SleightOfHand
        ////////////////////////////////////////////////////////////////////////////////////////
        #region SleightOfHand
        #region configdef

        #endregion configdef

        public static void InitSleightOfHandConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkSleightOfHand()
        {
            return new Perk("Sleight of Hand",
                "You can now bring certain items through portals.",
                "With a quick enough hand, you can even trick game devs.",
                PerkMan.PerkType.SleightOfHand, Skills.SkillType.None, "Icons/sleightofhand.png");
        }




        #endregion SleightOfHand

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 SmokeBomb
        ////////////////////////////////////////////////////////////////////////////////////////
        #region SmokeBomb
        #region configdef

        #endregion configdef

        public static void InitSmokeBombConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkSmokeBomb()
        {
            return new Perk("Smoke Bomb",
                "When you enter stealth, all enemies will lose track of you momentarily. Has a 20 second cooldown.",
                "Where'd he go? Must have been the wind.",
                PerkMan.PerkType.SmokeBomb, Skills.SkillType.None, "Icons/smokebomb.png");
        }




        #endregion SmokeBomb

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 SoilMixing
        ////////////////////////////////////////////////////////////////////////////////////////
        #region SoilMixing
        #region configdef

        #endregion configdef

        public static void InitSoilMixingConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkSoilMixing()
        {
            return new Perk("Soil Mixing",
                "New techniques allow you to plant any crop anywhere, regardless of biome limitations.",
                "Why go to the biome when you can bring the biome to you?",
                PerkMan.PerkType.SoilMixing, Skills.SkillType.None, "Icons/soilworking.png");
        }




        #endregion SoilMixing

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Spearit
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Spearit
        #region configdef

        #endregion configdef

        public static void InitSpearitConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkSpearit()
        {
            return new Perk("In Spearit",
                $"Your spear now deals {CFG.PerkWeaponEnchantDamage.Value}% additional spirit damage.",
                "I'm not sorry.",
                PerkMan.PerkType.Spearit, Skills.SkillType.Spears, "Icons/spearit.png",
                "Unlocked spirit enchant");
        }




        #endregion Spearit

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 SpiceMaster
        ////////////////////////////////////////////////////////////////////////////////////////
        #region SpiceMaster
        #region configdef

        #endregion configdef

        public static void InitSpiceMasterConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkSpiceMaster()
        {
            return new Perk("Spice Master",
                "Your mastery of spices has entered a legendarily gourmet realm. You can now " +
                "imbue your food with extra damage, experience gain, and damage reduction.",
                "A power even the dragonborn never mastered.",
                PerkMan.PerkType.SpiceMaster, Skills.SkillType.None, "Icons/spicemaster.png");
        }




        #endregion SpiceMaster

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 SpicySweet
        ////////////////////////////////////////////////////////////////////////////////////////
        #region SpicySweet
        #region configdef

        #endregion configdef

        public static void InitSpicySweetConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkSpicySweet()
        {
            return new Perk("Spicy and Sweet",
                "The discovery of several new spices has revolutionized your cooking. You can now" +
                " imbue your dishes with resistances to fire, cold, lightning, or poison.",
                "Cornerstones of any refined chef's palate.",
                PerkMan.PerkType.SpicySweet, Skills.SkillType.None, "Icons/spicysweet.png");
        }




        #endregion SpicySweet

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 SpikedShield
        ////////////////////////////////////////////////////////////////////////////////////////
        #region SpikedShield
        #region configdef

        #endregion configdef

        public static void InitSpikedShieldConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkSpikedShield()
        {
            return new Perk("Spiked Shield",
                "Whenever you block an attack, you reflect 50% of the blocked damage to the attacker.",
                "Who needs a sword?",
                PerkMan.PerkType.SpikedShield, Skills.SkillType.None, "Icons/spikedshield.png");
        }




        #endregion SpikedShield

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 SpiritGuide
        ////////////////////////////////////////////////////////////////////////////////////////
        #region SpiritGuide
        #region configdef

        #endregion configdef

        public static void InitSpiritGuideConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkSpiritGuide()
        {
            return new Perk("Spirit Guide",
                "The ghosts of slain woodland creatures now haunt you, providing an excellent light source while hunting down their children.",
                "They say you should use every part of the animal, why not the soul?",
                PerkMan.PerkType.SpiritGuide, Skills.SkillType.None, "Icons/spiritguide.png");
        }




        #endregion SpiritGuide

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Stretch
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Stretch
        #region configdef

        #endregion configdef

        public static void InitStretchConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkStretch()
        {
            return new Perk("Stretch",
                "Increases the range of your pickaxe swings by 50%.",
                "When you just barely can't reach that last rock.",
                PerkMan.PerkType.Stretch, Skills.SkillType.None, "Icons/stretch.png");
        }




        #endregion Stretch

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Superfuel
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Superfuel
        #region configdef

        #endregion configdef

        public static void InitSuperfuelConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkSuperfuel()
        {
            return new Perk("Superfuel",
                "Constructions you build, such as torches or campfires, no longer require additional fuel to keep running.",
                "It's even renewable!",
                PerkMan.PerkType.Superfuel, Skills.SkillType.None, "Icons/superfuel.png");
        }




        #endregion Superfuel

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Tackle
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Tackle
        #region configdef

        #endregion configdef

        public static void InitTackleConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkTackle()
        {
            return new Perk("Tackle",
                "Running into an enemy causes a huge knockback effect and causes you to gain a quickly fading " +
                "burst of movespeed.",
                "Outta the way, I'm walkin' here!",
                PerkMan.PerkType.Tackle, Skills.SkillType.None, "Icons/tackle.png");
        }




        #endregion Tackle

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 TasteTesting
        ////////////////////////////////////////////////////////////////////////////////////////
        #region TasteTesting
        #region configdef

        #endregion configdef

        public static void InitTasteTestingConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkTasteTesting()
        {
            return new Perk("Taste Testing",
                "The simple act of taste testing the food has led you to be able to garner information about a piece of food, " +
                "such as time until completion.",
                "Hmmm. Needs more salt.",
                PerkMan.PerkType.TasteTesting, Skills.SkillType.None, "Icons/tastetesting.png");
        }




        #endregion TasteTesting

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Throwback
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Throwback
        #region configdef

        #endregion configdef

        public static void InitThrowbackConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkThrowback()
        {
            return new Perk("Throwback",
                "You can now throw your axe like a spear. This attack one-shots all trees it hits.",
                "For when you can't quite reach.",
                PerkMan.PerkType.Throwback, Skills.SkillType.None, "Icons/throwback.png");
        }




        #endregion Throwback

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 TitanEndurance
        ////////////////////////////////////////////////////////////////////////////////////////
        #region TitanEndurance
        #region configdef

        #endregion configdef

        public static void InitTitanEnduranceConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkTitanEndurance()
        {
            return new Perk("Titan Endurance",
                "Increases your stagger limit by an additional 20% of your max health.",
                "On that day, mankind receieved a grim reminder...",
                PerkMan.PerkType.TitanEndurance, Skills.SkillType.None, "Icons/titanendurance.png");
        }




        #endregion TitanEndurance

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 TitanStrength
        ////////////////////////////////////////////////////////////////////////////////////////
        #region TitanStrength
        #region configdef

        #endregion configdef

        public static void InitTitanStrengthConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkTitanStrength()
        {
            return new Perk("Titan Strength",
                "You are no longer encumbered by shields.",
                "If you win, you live. If you lose, you die. If you don't fight, you can't win!",
                PerkMan.PerkType.TitanStrength, Skills.SkillType.None, "Icons/titanstrength.png");
        }




        #endregion TitanStrength

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Toxic
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Toxic
        #region configdef

        #endregion configdef

        public static void InitToxicConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkToxic()
        {
            return new Perk("Poisoned Blade",
                $"Your attacks deal an additional {CFG.PerkWeaponEnchantDamage.Value}% damage as poison damage.",
                "The hardest part is remembering to wear gloves when you apply the poison.",
                PerkMan.PerkType.Toxic, Skills.SkillType.Swords, "Icons/toxic.png",
                "Unlocked poison enchant");
        }




        #endregion Toxic

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Trapmaster
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Trapmaster
        #region configdef

        #endregion configdef

        public static void InitTrapmasterConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkTrapmaster()
        {
            return new Perk("Trapmaster",
                "Your cunning has resulted in several new inventive blueprints of death. Adds several " +
                "new traps to your arsenal.",
                "Some people think it's either building or combat. Why not both?",
                PerkMan.PerkType.Trapmaster, Skills.SkillType.None, "Icons/trapmaster.png");
        }




        #endregion Trapmaster

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 TrenchDigger
        ////////////////////////////////////////////////////////////////////////////////////////
        #region TrenchDigger
        #region configdef

        #endregion configdef

        public static void InitTrenchDiggerConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkTrenchDigger()
        {
            return new Perk("Trench Digger",
                "Increases the depth and width of your pickaxe strikes on the ground.",
                "We both know why you're here.",
                PerkMan.PerkType.TrenchDigger, Skills.SkillType.None, "Icons/trenchdigger.png");
        }




        #endregion TrenchDigger

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 ValkyrieFlight
        ////////////////////////////////////////////////////////////////////////////////////////
        #region ValkyrieFlight
        #region configdef

        #endregion configdef

        public static void InitValkyrieFlightConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkValkyrieFlight()
        {
            return new Perk("Flight of the Valkyries",
                "When you hit with a thrown spear, a mark is formed. Pressing B will teleport you to that mark, consuming it.",
                "To valhalla leads the way",
                PerkMan.PerkType.ValkyrieFlight, Skills.SkillType.None, "Icons/valkyrieflight.png");
        }




        #endregion ValkyrieFlight

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 VitalStudy
        ////////////////////////////////////////////////////////////////////////////////////////
        #region VitalStudy
        #region configdef

        #endregion configdef

        public static void InitVitalStudyConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkVitalStudy()
        {
            return new Perk("Vital Study",
                "Any sneak attack now gains you bonus sneak experience.",
                "They say the appendix serves no function, but when you rip it out, people die? Think for yourself, sheeple.",
                PerkMan.PerkType.VitalStudy, Skills.SkillType.None, "Icons/vitalstudy.png");
        }




        #endregion VitalStudy

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Warehousing
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Warehousing
        #region configdef

        #endregion configdef

        public static void InitWarehousingConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkWarehousing()
        {
            return new Perk("Warehousing Techniques",
                "Your constructions are so sturdy and precise that you've managed to squeeze an extra few" +
                " inventory spaces into almost all forms of storage.",
                "Master of time and space. Well, space, at least.",
                PerkMan.PerkType.Warehousing, Skills.SkillType.None, "Icons/warehousing.png");
        }




        #endregion Warehousing

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 WarriorOfLight
        ////////////////////////////////////////////////////////////////////////////////////////
        #region WarriorOfLight
        #region configdef

        #endregion configdef

        public static void InitWarriorOfLightConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkWarriorOfLight()
        {
            return new Perk("Warrior of Light",
                "You take less damage based on how much light you are in.",
                "Now you truly do have the power of god and anime on your side.",
                PerkMan.PerkType.WarriorOfLight, Skills.SkillType.None, "Icons/warrioroflight.png");
        }




        #endregion WarriorOfLight

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 WaterRunning
        ////////////////////////////////////////////////////////////////////////////////////////
        #region WaterRunning
        #region configdef

        #endregion configdef

        public static void InitWaterRunningConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkWaterRunning()
        {
            return new Perk("Water Running",
                "You can now run on water.",
                "When you don't want to wait for the boat.",
                PerkMan.PerkType.WaterRunning, Skills.SkillType.None, "Icons/waterrunning.png");
        }




        #endregion WaterRunning

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Worldly
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Worldly
        #region configdef

        #endregion configdef

        public static void InitWorldlyConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkWorldly()
        {
            return new Perk("Worldly Existence",
                "Hours of meditation upon your material lifestyle has lead to an epiphany. You gain timeless wisdom of the ages..." +
                " and 50% increased experience gain.",
                "Material gwurl!",
                PerkMan.PerkType.Worldly, Skills.SkillType.None, "Icons/worldly.png");
        }




        #endregion Worldly

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 Ymir
        ////////////////////////////////////////////////////////////////////////////////////////
        #region Ymir
        #region configdef

        #endregion configdef

        public static void InitYmirConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerkYmir()
        {
            return new Perk("Memories of Ymir",
                $"Tapping into the land's ancient heritage, you gain a {CFG.PerkWeaponEnchantDamage.Value}% " +
                $"extra frost damage with each attack.",
                "Let's hope Odin is not a jealous god.",
                PerkMan.PerkType.Ymir, Skills.SkillType.Polearms, "Icons/ymir.png",
                "Unlocked frost enchant");
        }


        #endregion Ymir

        #endregion perks

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                      Generic Weapon->Damage Experience
        ////////////////////////////////////////////////////////////////////////////////////////
        #region exp
        #region configdef
        public static ConfigEntry<float> WeaponXPSwing;
        public static ConfigEntry<float> WeaponXPHoldPerTick;
        public static ConfigEntry<float> WeaponXPHoldTickLength;
        public static ConfigEntry<float> WeaponXPHoldUnarmedPercent;
        public static ConfigEntry<float> WeaponXPStrikeDamagePercent;
        public static ConfigEntry<float> WeaponXPStrikeDamageFactor;
        public static ConfigEntry<float> WeaponXPStrikeDestructiblePercent;

        public static ConfigEntry<float> ToolXPStrikeDamagePercent;
        public static ConfigEntry<float> ToolXPStrikeDamageFactor;
        #endregion configdef

        public static void InitWeaponEXPConfig(ConfigFile cfg)
        {
            //Weapon Swing Experence
            WeaponXPSwing = cfg.Bind("Experience.Weapons.Swing", "XP Flat", .3f,
                AdminCD("Flat experience to be gained on each swing, regardless of hit.", true));


            //Weapon Hold Experience
            WeaponXPHoldPerTick = cfg.Bind("Experience.Weapons.Hold", "XP/s", .04f,
                AdminCD("Flat experience to be gained every second holding any weapon.", true));
            WeaponXPHoldTickLength = cfg.Bind("Experience.Weapons.Hold", "Timer", 1.0f,
                AdminCD("Seconds between ticks of hold experience."));
            WeaponXPHoldUnarmedPercent = cfg.Bind("Experience.Weapons.Hold", "Unarmed", 25f,
                AdminCD("% of normal hold experience to unarmed when holding nothing. Should be lower than 100% " +
                "to account for how often a regular player is holding nothing.", true));


            //Weapon Strike Experience
            WeaponXPStrikeDamagePercent = cfg.Bind("Experience.Weapons", "XP", 60f,
                AdminCD("% modifier to overall experience gained from damage.", true));
            WeaponXPStrikeDamageFactor = cfg.Bind("Experience.Weapons", "XP Factor", .44f,
                AdminCD("Factor to define the slope of the damage to xp curve. Only change if you know what you're doing.", true));
            WeaponXPStrikeDestructiblePercent = cfg.Bind("Experience.Weapons", "Destructible", 15f,
                AdminCD("% of experience gained when hit target is non living. Should be lover than 100%", true));



            //Tool experience
            ToolXPStrikeDamagePercent = cfg.Bind("Experience.Tools", "Tool Damage", 55f,
                AdminCD("% of damage done to resources that becomes experience for gathering skills " +
                "(Woodcutting, Mining)", true));
            ToolXPStrikeDamageFactor = cfg.Bind("Experience.Tools", "Tool Damage Factor", .65f,
                AdminCD("Factor to define the slope of the damage to xp curve. Only change if you know what you're doing.", true));
        }

        

        public static float GetWeaponXPStrikeDestructibleMod()
        {
            return PerToMod(WeaponXPStrikeDestructiblePercent);
        }
        public static float GetWeaponEXPHoldUnarmedMod()
        {
            return PerToMod(WeaponXPHoldUnarmedPercent);
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

        #endregion exp



        #region templates
        /*
         
        /////////////////////////// config entry

        public static ConfigEntry<bool> ;




        /////////////////////////// get function

        public static float Get(float skillFactor)
        {
            return Mathf.Lerp(PerToMult(), PerToMult(), skillFactor);
        }

        

        //////////////////////////// config setting

        x = cfg.Bind("g", "t", 0f, 
                    AdminCD("d"));


        /////////////////////////// new section

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                              template
        ////////////////////////////////////////////////////////////////////////////////////////
        #region a
        #region configdef
        #endregion configdef

        public static void InitConfigs(ConfigFile cfg)
        {
        }


        #endregion a

        
        /////////////////////////// new perk

        ////////////////////////////////////////////////////////////////////////////////////////
        ///                                 P
        ////////////////////////////////////////////////////////////////////////////////////////
        #region 
        #region configdef

        #endregion configdef

        public static void InitConfigs(ConfigFile cfg)
        {
        }

        public static Perk GetPerk()
        {
            return new Perk("P",
                "",
                "",
                Perks.PerkMan.PerkType., Skills.SkillType.None, "Icons/.png",
                "");
        }




        #endregion 
        */
        #endregion templates
    }
}
