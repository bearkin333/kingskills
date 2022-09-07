using HarmonyLib;
using Jotunn.Managers;
using kingskills.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace kingskills.UX
{
    [HarmonyPatch]
    class UpdateGUI
    {
        //how many seconds between GUI update
        public const float updateGUITimer = 2f;

        public static float timeSinceUpdate = 0f;
        public static bool GUIOpen = false;

        [HarmonyPatch(typeof(Player), nameof(Player.FixedUpdate))]
        [HarmonyPrefix]
        public static void FixedUpdateGUI(Player __instance)
        {
            try
            {
                if (!__instance.m_nview) return;
                if (!__instance.m_nview.IsOwner()) return;
                if (!SkillGUI.SkillGUIWindow) return;
                if (!SkillGUI.SkillGUIWindow.activeSelf) return;
            }
            catch
            {
                Jotunn.Logger.LogWarning("Didn't check for GUI Update");
                return;
            }

            timeSinceUpdate += Time.fixedDeltaTime;
            if (timeSinceUpdate >= updateGUITimer)
            {
                timeSinceUpdate -= updateGUITimer;
                //Jotunn.Logger.LogMessage("Updating the values on the GUI");
                GUICheck();
            }
        }

        public static void ToggleSkillGUI()
        {
            if (Player.m_localPlayer == null)
            {
                return;
            }
            if (!SkillGUI.SkillGUIWindow)
            {
                SkillGUI.InitSkillWindow();
            }

            bool state = !SkillGUI.SkillGUIWindow.activeSelf;

            SkillGUI.SkillGUIWindow.SetActive(state);

            GUIManager.BlockInput(state);

            if (state)
            {
                GUICheck();
            }
        }

        public static void StickGUI()
        {
            GUIManager.BlockInput(false);
        }

        public static void OnDropdownValueChange()
        {
            SkillGUI.scroll.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
            GUICheck();
        }

        public static void GUICheck()
        {
            //Jotunn.Logger.LogMessage($"Detected a dropdown value change.");

            Player player = Player.m_localPlayer;
            Skills.SkillType skill = Skills.SkillType.None;

            string skillName = SkillGUI.dd.options[SkillGUI.dd.value].text;
            StatsPatch.UpdateStats(player);
            ResetText();

            switch (skillName)
            {
                case "Agriculture":
                    skill = SkillMan.Agriculture;
                    OpenAgriculturePanels();
                    OpenPerks.OpenAgriculturePerkBoxes();
                    break;
                case "Axes":
                    skill = Skills.SkillType.Axes;
                    OpenAxePanels();
                    OpenPerks.OpenAxePerkBoxes();
                    break;
                case "Blocking":
                    skill = Skills.SkillType.Blocking;
                    OpenBlockPanels();
                    OpenPerks.OpenBlockPerkBoxes();
                    break;
                case "Bows":
                    skill = Skills.SkillType.Bows;
                    OpenBowPanels();
                    OpenPerks.OpenBowPerkBoxes();
                    break;
                case "Building":
                    skill = SkillMan.Building;
                    OpenBuildingPanels();
                    OpenPerks.OpenBuildingPerkBoxes();
                    break;
                case "Clubs":
                    skill = Skills.SkillType.Clubs;
                    OpenClubPanels();
                    OpenPerks.OpenClubPerkBoxes();
                    break;
                case "Cooking":
                    skill = SkillMan.Cooking;
                    OpenCookingPanels();
                    OpenPerks.OpenCookingPerkBoxes();
                    break;
                case "Fists":
                    skill = Skills.SkillType.Unarmed;
                    OpenFistPanels();
                    OpenPerks.OpenFistPerkBoxes();
                    break;
                case "Jump":
                    skill = Skills.SkillType.Jump;
                    OpenJumpPanels();
                    OpenPerks.OpenJumpPerkBoxes();
                    break;
                case "Knives":
                    skill = Skills.SkillType.Knives;
                    OpenKnifePanels();
                    OpenPerks.OpenKnifePerkBoxes();
                    break;
                case "Mining":
                    skill = Skills.SkillType.Pickaxes;
                    OpenMiningPanels();
                    OpenPerks.OpenMiningPerkBoxes();
                    break;
                case "Polearms":
                    skill = Skills.SkillType.Polearms;
                    OpenPolearmPanels();
                    OpenPerks.OpenPolearmPerkBoxes();
                    break;
                case "Run":
                    skill = Skills.SkillType.Run;
                    OpenRunPanels();
                    OpenPerks.OpenRunPerkBoxes();
                    break;
                case "Sailing":
                    skill = SkillMan.Sailing;
                    OpenSailingPanels();
                    OpenPerks.OpenSailingPerkBoxes();
                    break;
                case "Spears":
                    skill = Skills.SkillType.Spears;
                    OpenSpearPanels();
                    OpenPerks.OpenSpearPerkBoxes();
                    break;
                case "Sneak":
                    skill = Skills.SkillType.Sneak;
                    OpenSneakPanels();
                    OpenPerks.OpenSneakPerkBoxes();
                    break;
                case "Swim":
                    skill = Skills.SkillType.Swim;
                    OpenSwimPanels();
                    OpenPerks.OpenSwimPerkBoxes();
                    break;
                case "Swords":
                    skill = Skills.SkillType.Swords;
                    OpenSwordPanels();
                    OpenPerks.OpenSwordPerkBoxes();
                    break;
                case "Woodcutting":
                    skill = Skills.SkillType.WoodCutting;
                    OpenWoodcuttingPanels();
                    OpenPerks.OpenWoodcuttingPerkBoxes();
                    break;
            }

            Skills.Skill skillRef = player.GetSkills().GetSkill(skill);
            //Jotunn.Logger.LogMessage($"The skill the player seems to want is {skillRef.m_info}");

            SkillGUI.SSIcon.GetComponent<Image>().sprite = player.m_skills.GetSkillDef(skill).m_icon;
            SkillGUI.SSkillName.GetComponent<Text>().text = skillName;
            SkillGUI.SSkillLevel.GetComponent<Text>().text = "Level: " + skillRef.m_level.ToString("F0") + " / 100";
            SkillGUI.SSkillExp.GetComponent<Text>().text = "Experience: " + skillRef.m_accumulator.ToString("F2") + " / " + skillRef.GetNextLevelRequirement().ToString("F2");
            //scroll.GetComponent<ScrollRect>().Rebuild(UnityEngine.UI.CanvasUpdate.PreRender);


            if (!ConfigMan.IsSkillActive(skill))
            {
                SkillGUI.LeftPanelTexts["experience"].GetComponent<Text>().text =
                    "Not currently active";
                SkillGUI.LeftPanelTexts["effects"].GetComponent<Text>().text =
                    "Not currently active";
                return;
            }

            if (Perks.IsSkillAscended(skill))
            {
                SkillGUI.RightPanelAscendedText.GetComponent<Text>().text = "Ascended";
            }
            else
            {
                SkillGUI.RightPanelAscendedText.GetComponent<Text>().text = "";
            }
            //Jotunn.Logger.LogMessage($"{Perks.IsSkillAscended(skill).ToString()}");
        }


        public static void OpenAgriculturePanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(SkillMan.Agriculture);

            float plantXPReward = ConfigMan.GetAgriculturePlantReward(Player.m_localPlayer.m_hovering);

            float botYield = MultToPer(ConfigMan.GetAgricultureYieldMult(skill));
            float botGrowRedux = MultToPer(ConfigMan.GetAgricultureGrowTimeRedux(skill), true);
            float botFoodQuality = MultToPer(ConfigMan.GetAgricultureFoodQualityMult(skill));
            float botHealthGain = ConfigMan.GetAgricultureHealthRegain(skill);

            string botYieldS = PT.Prettify(botYield, 2, PT.TType.Percent);
            string botGrowReduxS = PT.Prettify(botGrowRedux, 0, PT.TType.PercentRedux);
            string botFoodQualityS = PT.Prettify(botFoodQuality, 0, PT.TType.Percent);
            string botHealthGainS = PT.Prettify(botHealthGain, 1, PT.TType.Flat);


            SkillGUI.LeftPanelTexts["x1"].GetComponent<Text>().text =
                "Every item you harvest gives you experience, whether in the wild or in your planted garden. " +
                "You also get a small amount for each plant planted.";
            SkillGUI.LeftPanelTexts["x2"].GetComponent<Text>().text =
                "Different plants give you different rewards. As a rule, items you've " +
                "planted tend to give you more experience.";
            SkillGUI.LeftPanelTexts["x3"].GetComponent<Text>().text =
                "Trees you plant will give you experience as soon as they mature.";
            SkillGUI.LeftPanelTexts["bonus"].GetComponent<Text>().text =
                "Experience bounty from hovered plant: " + plantXPReward.ToString("F1");

            SkillGUI.LeftPanelTexts["f1"].GetComponent<Text>().text =
                botYieldS + " yield from plants";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                botGrowReduxS + " time to grow plants";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                botFoodQualityS + " food quality for food you harvest";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                botHealthGainS + " health regained every time you harvest a plant";

        }
        public static void OpenAxePanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Axes);

            float axeDamage = MultToPer(ConfigMan.GetAxeDamageMult(skill));
            float axeStaminaRedux = MultToPer(ConfigMan.GetAxeStaminaRedux(skill), true);
            float axeStaminaGain = ConfigMan.GetAxeStamina(skill);
            float axeChopBonus = MultToPer(1f + ConfigMan.GetAxeChopDamageMod(skill));
            float axeCarryCapacity = ConfigMan.GetAxeCarryCapacity(skill);

            string axeDamageS = PT.Prettify(axeDamage, 1, PT.TType.Percent);
            string axeStaminaReduxS = PT.Prettify(axeStaminaRedux, 1, PT.TType.PercentRedux);
            string axeStaminaGainS = PT.Prettify(axeStaminaGain, 0, PT.TType.Flat);
            string axeChopBonusS = PT.Prettify(axeChopBonus, 1, PT.TType.Percent);
            string axeCarryCapacityS = PT.Prettify(axeCarryCapacity, 0, PT.TType.Flat);

            //Jotunn.Logger.LogMessage($"I'm changing the axe values in now");
            SkillGUI.LeftPanelTexts["x1"].GetComponent<Text>().text =
                "A percentage of all damage dealt is turned into experience, " +
                "but the rate is higher when the damage is dealt to living cretures.";
            SkillGUI.LeftPanelTexts["x2"].GetComponent<Text>().text =
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not.";
            SkillGUI.LeftPanelTexts["x3"].GetComponent<Text>().text =
                "Holding an axe gains you experience at a very slow rate.";
            SkillGUI.LeftPanelTexts["bonus"].GetComponent<Text>().text =
                "Bonus experience for the axe is gained every time you break a log. ";

            SkillGUI.LeftPanelTexts["f1"].GetComponent<Text>().text =
                axeDamageS + " damage with axes";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                axeStaminaReduxS + " stamina usage with axes";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                axeStaminaGainS + " higher base stamina";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                axeChopBonusS + " chop damage";

            SkillGUI.LeftPanelTexts["f5"].GetComponent<Text>().text =
                axeCarryCapacityS + " extra carry capacity";
        }
        public static void OpenBlockPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Blocking);

            float staminaRedux = MultToPer(ConfigMan.GetBlockStaminaRedux(skill), true);
            float baseBlockPower = ConfigMan.GetBlockPowerFlat(skill);
            float blockPerArmor = MultToPer(ConfigMan.GetBlockPowerMult(skill));
            float blockHealth = ConfigMan.GetBlockHealth(skill);
            float parryExpMod = MultToPer(ConfigMan.GetBlockParryExpMult());

            string staminaReduxS = PT.Prettify(staminaRedux, 1, PT.TType.PercentRedux);
            string baseBlockPowerS = PT.Prettify(baseBlockPower, 0, PT.TType.Percent);
            string blockPerArmorS = PT.Prettify(blockPerArmor, 1, PT.TType.Percent);
            string blockHealthS = PT.Prettify(blockHealth, 0, PT.TType.Percent);

            SkillGUI.LeftPanelTexts["x1"].GetComponent<Text>().text =
                "A percentage of all damage you block is turned into experience.";
            SkillGUI.LeftPanelTexts["x2"].GetComponent<Text>().text =
                "This number is unaffected by resistances.";
            SkillGUI.LeftPanelTexts["bonus"].GetComponent<Text>().text =
                "You get " + parryExpMod.ToString("F0") + "% bonus experience for parrying an attack.";

            SkillGUI.LeftPanelTexts["f1"].GetComponent<Text>().text =
                staminaReduxS + " stamina cost for blocks";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                baseBlockPowerS + " flat block armor";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                blockPerArmorS + " block armor";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                blockHealthS + " max health";
        }
        public static void OpenBowPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Bows);

            float bowDamage = MultToPer(ConfigMan.GetBowDamageMult(skill));
            float bowStaminaRedux = MultToPer(ConfigMan.GetBowStaminaRedux(skill), true);
            float bowVelocity = MultToPer(ConfigMan.GetBowVelocityMult(skill));
            float bowDrawSpeed = ConfigMan.GetBowDrawSpeed(skill);
            float bowDropRate = MultToPer(ConfigMan.GetBowDropRateMod(skill)+1f);

            string bowDamageS = PT.Prettify(bowDamage, 1, PT.TType.Percent);
            string bowStaminaReduxS = PT.Prettify(bowStaminaRedux, 1, PT.TType.PercentRedux);
            string bowVelocityS = PT.Prettify(bowVelocity, 1, PT.TType.Percent);
            string bowDrawSpeedS = PT.Prettify(bowDrawSpeed, 1, PT.TType.Straight);
            string bowDropRateS = PT.Prettify(bowDropRate, 1, PT.TType.Percent);

            SkillGUI.LeftPanelTexts["x1"].GetComponent<Text>().text =
                "A percentage of all damage dealt is turned into experience, " +
                "but the rate is higher when the damage is dealt to living cretures.";
            SkillGUI.LeftPanelTexts["x2"].GetComponent<Text>().text =
                "Every time you shoot, a small amount of experience is gained, whether you hit anything or not.";
            SkillGUI.LeftPanelTexts["x3"].GetComponent<Text>().text =
                "Holding a bow gains you experience at a very slow rate.";
            SkillGUI.LeftPanelTexts["bonus"].GetComponent<Text>().text =
                "Bonus experience is gained based on the length and difficulty of successful shots with a bow.";

            SkillGUI.LeftPanelTexts["f1"].GetComponent<Text>().text =
                bowDamageS + " damage with bows";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                bowStaminaReduxS + " stamina usage with bows";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                bowVelocityS + " arrow velocity";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                bowDrawSpeedS + " draw speed";

            SkillGUI.LeftPanelTexts["f5"].GetComponent<Text>().text =
                bowDropRateS + " loot drops from creatures";
        }
        public static void OpenBuildingPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(SkillMan.Building);

            float buildHealth = MultToPer(ConfigMan.GetBuildingHealthMult(skill));
            float buildStability = MultToPer(ConfigMan.GetBuildingStabilityMult(skill));
            float buildDamage = MultToPer(ConfigMan.GetBuildingDamageMult(skill));
            float buildWNT = MultToPer(ConfigMan.GetBuildingWNTRedux(skill), true);
            float buildFreeChance = MultToPer(1f + ConfigMan.GetBuildingFreeMod(skill));
            float buildStaminaRedux = MultToPer(ConfigMan.GetBuildingStaminaRedux(skill), true);

            string buildHealthS = PT.Prettify(buildHealth, 1, PT.TType.Percent);
            string buildStabilityS = PT.Prettify(buildStability, 1, PT.TType.Percent);
            string buildDamageS = PT.Prettify(buildDamage, 0, PT.TType.Percent);
            string buildWNTS = PT.Prettify(buildWNT, 0, PT.TType.PercentRedux);
            string buildFreeChanceS = PT.Prettify(buildFreeChance, 0, PT.TType.ColorlessPercent);
            string buildStaminaReduxS = PT.Prettify(buildStaminaRedux, 0, PT.TType.PercentRedux);


            SkillGUI.LeftPanelTexts["x1"].GetComponent<Text>().text =
                "Placing a building gives you a flat amount of experience.";
            SkillGUI.LeftPanelTexts["x2"].GetComponent<Text>().text =
                "Repairing a building earns you experience based on how much health you repaired.";
            SkillGUI.LeftPanelTexts["x3"].GetComponent<Text>().text =
                "Creatures damaging your buildings also provide damage-based experience.";
            SkillGUI.LeftPanelTexts["bonus"].GetComponent<Text>().text =
                "Bonus experience for building is earned when your buildings damage creatures, based on damage dealt.";

            SkillGUI.LeftPanelTexts["f1"].GetComponent<Text>().text =
                buildHealthS + " building health";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                buildStabilityS + " building structural stability ";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                buildDamageS + " damage dealt by buildings";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                buildWNTS + " wear and tear on buildings";

            SkillGUI.LeftPanelTexts["f5"].GetComponent<Text>().text =
                buildFreeChanceS + " chance to place a free building";

            SkillGUI.LeftPanelTexts["f6"].GetComponent<Text>().text =
                buildStaminaReduxS + " stamina cost for construction and repair";
        }
        public static void OpenClubPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Clubs);

            float clubDamage = MultToPer(ConfigMan.GetClubDamageMult(skill));
            float clubStaminaRedux = MultToPer(ConfigMan.GetClubStaminaRedux(skill), true);
            float clubBlunt = MultToPer(ConfigMan.GetClubBluntMult(skill));
            float clubKnockback = MultToPer(ConfigMan.GetClubKnockbackMult(skill));
            float clubStagger = MultToPer(ConfigMan.GetClubStaggerMult(skill));

            string clubDamageS = PT.Prettify(clubDamage, 1, PT.TType.Percent);
            string clubStaminaReduxS = PT.Prettify(clubStaminaRedux, 1, PT.TType.PercentRedux);
            string clubBluntS = PT.Prettify(clubBlunt, 1, PT.TType.Percent);
            string clubKnockbackS = PT.Prettify(clubKnockback, 1, PT.TType.TextlessPercent);
            string clubStaggerS = PT.Prettify(clubStagger, 1, PT.TType.Percent);

            SkillGUI.LeftPanelTexts["x1"].GetComponent<Text>().text =
                "A percentage of all damage dealt is turned into experience, " +
                "but the rate is higher when the damage is dealt to living cretures.";
            SkillGUI.LeftPanelTexts["x2"].GetComponent<Text>().text =
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not.";
            SkillGUI.LeftPanelTexts["x3"].GetComponent<Text>().text =
                "Holding a club gains you experience at a very slow rate.";
            SkillGUI.LeftPanelTexts["bonus"].GetComponent<Text>().text =
                "Bonus experience every time you stagger an enemy with damage with a club.";

            SkillGUI.LeftPanelTexts["f1"].GetComponent<Text>().text =
                clubDamageS + " damage with clubs";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                clubStaminaReduxS + " stamina usage with clubs";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                clubBluntS + " bonus to ALL blunt damage";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                clubKnockbackS + " knockback bonus to ALL weapons";

            SkillGUI.LeftPanelTexts["f5"].GetComponent<Text>().text =
                clubStaggerS + " stagger damage to ALL weapons";
        }
        public static void OpenCookingPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(SkillMan.Cooking);

            float cookAvgFQ = MultToPer(1f + ConfigMan.GetCookingAverageFoodQualityMod(skill));
            float cookFQRange = MultToPer(1f + ConfigMan.GetCookingFoodQualityRangeMod(skill));
            float cookTimeRedux = MultToPer(ConfigMan.GetCookingTimeRedux(skill), true);
            float cookFermentTimeRedux = MultToPer(ConfigMan.GetCookingFermentTimeRedux(skill), true);

            string cookAvgFQS = PT.Prettify(cookAvgFQ, 1, PT.TType.TextlessPercent);
            string cookFQRangeS = PT.Prettify(cookFQRange, 1, PT.TType.TextlessPercent);
            string cookTimeReduxS = PT.Prettify(cookTimeRedux, 1, PT.TType.PercentRedux);
            string cookFermentTimeReduxS = PT.Prettify(cookFermentTimeRedux, 1, PT.TType.PercentRedux);


            SkillGUI.LeftPanelTexts["x1"].GetComponent<Text>().text =
                "Adding food to a cooking station will give you a small amount of experience, based " +
                "on the tier of the cooking station.";
            SkillGUI.LeftPanelTexts["x2"].GetComponent<Text>().text =
                "Successfully retrieving cooked food from a cooking station will reward you with twice " +
                "as much. You get no experience if you let the food burn.";

            SkillGUI.LeftPanelTexts["bonus"].GetComponent<Text>().text =
                "Bonus experience for cooking is gained whenever someone else eats your cooking, based on how " +
                "much stamina and health it gives.";

            SkillGUI.LeftPanelTexts["f1"].GetComponent<Text>().text =
                cookAvgFQS + " average food quality of cooked items";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                cookFQRangeS + " range in possible food qualities";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                cookTimeReduxS + " cooking time";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                cookFermentTimeReduxS + " fermentation time";

            SkillGUI.LeftPanelTexts["f5"].GetComponent<Text>().text =
                "\n[Food Quality]: \nFood quality is a king's skills specific property. Any item " +
                "you cook gets a random food quality, which directly affects it's health, stamina, and " +
                "duration. \nFQ is based primarily on your skill level, but your timing has an affect on the " +
                "overall quality as well.";

        }
        public static void OpenFistPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Unarmed);

            float fistDamage = MultToPer(ConfigMan.GetFistDamageMult(skill));
            float fistStaminaRedux = MultToPer(ConfigMan.GetFistStaminaRedux(skill));
            float fistFlatDamage = ConfigMan.GetFistDamageFlat(skill);
            float fistBlock = ConfigMan.GetFistBlockArmor(skill);
            float fistMovespeed = MultToPer(1f + ConfigMan.GetFistMovespeedMod(skill));

            string fistDamageS = PT.Prettify(fistDamage, 1, PT.TType.Percent);
            string fistStaminaReduxS = PT.Prettify(fistStaminaRedux, 1, PT.TType.PercentRedux);
            string fistFlatDamageS = PT.Prettify(fistFlatDamage, 0, PT.TType.Flat);
            string fistBlockS = PT.Prettify(fistBlock, 0, PT.TType.Flat);
            string fistMovespeedS = PT.Prettify(fistMovespeed, 1, PT.TType.Percent);

            SkillGUI.LeftPanelTexts["x1"].GetComponent<Text>().text =
                "A percentage of all damage dealt is turned into experience, " +
                "but the rate is higher when the damage is dealt to living cretures.";
            SkillGUI.LeftPanelTexts["x2"].GetComponent<Text>().text =
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not.";
            SkillGUI.LeftPanelTexts["x3"].GetComponent<Text>().text =
                "Holding nothing gains you experience at a very slow rate - although slower than other weapons.";
            SkillGUI.LeftPanelTexts["bonus"].GetComponent<Text>().text =
                "Bonus experence for fists is gained every time you perform an unarmed block.";

            SkillGUI.LeftPanelTexts["f1"].GetComponent<Text>().text =
                fistDamageS + " damage with fists";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                fistStaminaReduxS + " stamina usage with fists";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                fistFlatDamageS + " flat damage";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                fistBlockS + " unarmed block armor";

            SkillGUI.LeftPanelTexts["f5"].GetComponent<Text>().text =
                fistMovespeedS + " move speed";
        }
        public static void OpenJumpPanels()
        {
            Player player = Player.m_localPlayer;
            StatsPatch.JumpForceUpdate(player);
            float skill = player.GetSkillFactor(Skills.SkillType.Jump);

            float bonusJumpForce = MultToPer(ConfigMan.GetJumpForceMult(skill));
            float bonusJumpForwardForce = MultToPer(ConfigMan.GetJumpForwardForceMult(skill));
            float staminaRedux = MultToPer(ConfigMan.GetJumpStaminaRedux(skill), true);
            float tired = (ConfigMan.GetJumpTiredMod(skill) + ConfigMan.BaseJumpTiredFactor)*100f;

            float fallDamageThreshhold = ConfigMan.GetFallDamageThreshold(skill);
            float fallDamageRedux = MultToPer(ConfigMan.GetFallDamageRedux(skill), true);

            string bonusJumpForceS = PT.Prettify(bonusJumpForce, 1, PT.TType.Percent);
            string bonusJumpForwardForceS = PT.Prettify(bonusJumpForwardForce, 1, PT.TType.Percent);
            string staminaReduxS = PT.Prettify(staminaRedux, 1, PT.TType.PercentRedux);
            string tiredS = PT.Prettify(tired, 0, PT.TType.ColorlessPercent);
            string fallDamageThreshholdS = PT.Prettify(fallDamageThreshhold, 0, PT.TType.Flat);
            string fallDamageReduxS = PT.Prettify(fallDamageRedux, 1, PT.TType.PercentRedux);


            SkillGUI.LeftPanelTexts["x1"].GetComponent<Text>().text =
                "Every time you jump, you gain a small, flat amount of experience.";
            SkillGUI.LeftPanelTexts["bonus"].GetComponent<Text>().text =
                "Every time you land, you gain bonus experience based on " +
                "how far you fell and how much damage you would've normally taken without " +
                "fall damage resistance.";

            SkillGUI.LeftPanelTexts["f1"].GetComponent<Text>().text =
                bonusJumpForceS + " vertical jump force ";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                bonusJumpForwardForceS + " horizontal jump force ";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                staminaReduxS + " stamina cost to jump ";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                tiredS + " jump force modifier when tired";

            SkillGUI.LeftPanelTexts["f5"].GetComponent<Text>().text =
                fallDamageThreshholdS + "m minimum fall damage height ";

            SkillGUI.LeftPanelTexts["f6"].GetComponent<Text>().text =
                fallDamageReduxS + " fall damage";
        }
        public static void OpenKnifePanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Knives);

            float knifeDamage = MultToPer(ConfigMan.GetKnifeDamageMult(skill));
            float knifeStaminaRedux = MultToPer(ConfigMan.GetKnifeStaminaRedux(skill), true);
            float knifeBackstab = MultToPer(ConfigMan.GetKnifeBackstabMult(skill));
            float knifeMovespeed = MultToPer(1f + ConfigMan.GetKnifeMovespeedMod(skill));
            float knifePierce = MultToPer(ConfigMan.GetKnifePierceMult(skill));

            string knifeDamageS = PT.Prettify(knifeDamage, 1, PT.TType.Percent);
            string knifeStaminaReduxS = PT.Prettify(knifeStaminaRedux, 1, PT.TType.PercentRedux);
            string knifeBackstabS = PT.Prettify(knifeBackstab, 0, PT.TType.Percent);
            string knifeMovespeedS = PT.Prettify(knifeMovespeed, 1, PT.TType.Percent);
            string knifePierceS = PT.Prettify(knifePierce, 1, PT.TType.Percent);

            SkillGUI.LeftPanelTexts["x1"].GetComponent<Text>().text =
                "A percentage of all damage dealt is turned into experience, " +
                "but the rate is higher when the damage is dealt to living cretures.";
            SkillGUI.LeftPanelTexts["x2"].GetComponent<Text>().text =
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not.";
            SkillGUI.LeftPanelTexts["x3"].GetComponent<Text>().text =
                "Holding a knife gains you experience at a very slow rate.";
            SkillGUI.LeftPanelTexts["bonus"].GetComponent<Text>().text =
                "Bonus experience is gained every time you perform a sneak attack with a knife.";

            SkillGUI.LeftPanelTexts["f1"].GetComponent<Text>().text =
                knifeDamageS + " damage with knives ";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                knifeStaminaReduxS + " stamina usage with knives ";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                knifeBackstabS + " sneak attack damage ";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                knifeMovespeedS + " move speed ";

            SkillGUI.LeftPanelTexts["f5"].GetComponent<Text>().text =
                knifePierceS + " to ALL pierce damage";
        }
        public static void OpenMiningPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Pickaxes);

            float mineDamage = MultToPer(ConfigMan.GetMiningDamageMult(skill));
            float mineDrop = MultToPer(ConfigMan.GetMiningDropMod(skill) + 1f);
            float mineRebate = ConfigMan.GetMiningStaminaRebate(skill);
            float mineRegen = ConfigMan.GetMiningRegenLessTime(skill);
            float mineCarry = ConfigMan.GetMiningCarryCapacity(skill);


            string mineDamageS = PT.Prettify(mineDamage, 1, PT.TType.Percent);
            string mineDropS = PT.Prettify(mineDrop, 2, PT.TType.Percent);
            string mineRebateS = PT.Prettify(mineRebate, 0, PT.TType.Flat);
            string mineRegenS = PT.Prettify(mineRegen, 1, PT.TType.Straight);
            string mineCarryS = PT.Prettify(mineCarry, 0, PT.TType.Flat);

            SkillGUI.LeftPanelTexts["x1"].GetComponent<Text>().text =
                "A percentage of all damage dealt to rocks or ore is turned into experience.";
            SkillGUI.LeftPanelTexts["x2"].GetComponent<Text>().text =
                "You still gain experience for hitting the ground, but at a reduced pace.";
            SkillGUI.LeftPanelTexts["x3"].GetComponent<Text>().text =
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not.";
            SkillGUI.LeftPanelTexts["bonus"].GetComponent<Text>().text =
                "Bonus experience for the pick is gained based on the tier of the tool used.";

            SkillGUI.LeftPanelTexts["f1"].GetComponent<Text>().text =
                mineDamageS + " mining damage";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                mineDropS + " ore drop rates";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                mineRebateS + " stamina rebate on mining swings";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                mineRegenS + " fewer seconds between health regeneration ticks";

            SkillGUI.LeftPanelTexts["f5"].GetComponent<Text>().text =
                mineCarryS + " carrying capacity";
        }
        public static void OpenPolearmPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Polearms);

            float polearmDamage = MultToPer(ConfigMan.GetPolearmDamageMult(skill));
            float polearmStaminaRedux = MultToPer(ConfigMan.GetPolearmStaminaRedux(skill), true);
            float polearmRange = ConfigMan.GetPolearmRange(skill);
            float polearmArmor = ConfigMan.GetPolearmArmor(skill);
            float polearmBlock = ConfigMan.GetPolearmBlock(skill);

            string polearmDamageS = PT.Prettify(polearmDamage, 1, PT.TType.Percent);
            string polearmStaminaReduxS = PT.Prettify(polearmStaminaRedux, 1, PT.TType.PercentRedux);
            string polearmRangeS = PT.Prettify(polearmRange, 0, PT.TType.Flat);
            string polearmArmorS = PT.Prettify(polearmArmor, 0, PT.TType.Flat);
            string polearmBlockS = PT.Prettify(polearmBlock, 0, PT.TType.Flat);

            SkillGUI.LeftPanelTexts["x1"].GetComponent<Text>().text =
                "A percentage of all damage dealt is turned into experience, " +
                "but the rate is higher when the damage is dealt to living cretures.";
            SkillGUI.LeftPanelTexts["x2"].GetComponent<Text>().text =
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not.";
            SkillGUI.LeftPanelTexts["x3"].GetComponent<Text>().text =
                "Holding a polearm gains you experience at a very slow rate.";
            SkillGUI.LeftPanelTexts["bonus"].GetComponent<Text>().text =
                "Bonus experience for polearms is gained when you reduce damage taken with armor. " +
                "In other words, wear heavy armor, get hit!";

            SkillGUI.LeftPanelTexts["f1"].GetComponent<Text>().text =
                polearmDamageS + " damage with polearms ";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                polearmStaminaReduxS + " stamina usage with polearms";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                polearmRangeS + " units of range with all weapons";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                polearmArmorS + " armor";

            SkillGUI.LeftPanelTexts["f5"].GetComponent<Text>().text =
                polearmBlockS + " block power with polearms";
        }
        public static void OpenRunPanels()
        {
            Player player = Player.m_localPlayer;
            StatsPatch.RunSpeedUpdate(player);
            float skill = player.GetSkillFactor(Skills.SkillType.Run);

            float runSpeedBonus = MultToPer(ConfigMan.GetRunSpeedMult(skill));
            float equipmentMalusRedux = MultToPer(ConfigMan.GetEquipmentRedux(skill), true);
            float encumberanceRedux = MultToPer(ConfigMan.GetEncumberanceRedux(skill), true);
            float staminaDrainRedux = MultToPer(ConfigMan.GetRunStaminaRedux(skill), true);
            float baseStaminaGain = ConfigMan.GetRunStamina(skill);

            float encumberanceFactor = MultToPer(MovePatch.GetEncumberanceRedux(player), true);
            float equipmentFactor = MultToPer(MovePatch.GetEquipmentMult(player));

            float absWeightExp = MultToPer(MovePatch.absoluteWeightBonus(player));
            float relWeightExp = MultToPer(MovePatch.relativeWeightBonus(player));
            float runSpeedExp = MultToPer(MovePatch.runSpeedExpBonus(player));


            string runSpeedBonusS = PT.Prettify(runSpeedBonus, 1, PT.TType.Percent);
            string equipmentMalusReduxS = PT.Prettify(equipmentMalusRedux, 1, PT.TType.PercentRedux);
            string encumberanceReduxS = PT.Prettify(encumberanceRedux, 1, PT.TType.PercentRedux);
            string staminaDrainReduxS = PT.Prettify(staminaDrainRedux, 1, PT.TType.PercentRedux);
            string baseStaminaGainS = PT.Prettify(baseStaminaGain, 0, PT.TType.Flat);

            string encumberanceFactorS = PT.Prettify(encumberanceFactor, 1, PT.TType.PercentRedux);
            string equipmentFactorS = PT.Prettify(equipmentFactor, 1, PT.TType.PercentRedux);

            string absWeightExpS = PT.Prettify(absWeightExp, 1, PT.TType.ColorlessPercent);
            string relWeightExpS = PT.Prettify(relWeightExp, 1, PT.TType.ColorlessPercent);
            string runSpeedExpS = PT.Prettify(runSpeedExp, 1, PT.TType.ColorlessPercent);


            SkillGUI.LeftPanelTexts["x1"].GetComponent<Text>().text =
                "The faster you are moving, the more experience you get.";
            SkillGUI.LeftPanelTexts["x2"].GetComponent<Text>().text =
                "The closer you are to fully encumbered, the less movespeed you have.";
            SkillGUI.LeftPanelTexts["x3"].GetComponent<Text>().text =
                "You also gain experience based on how encumbered you are.";
            SkillGUI.LeftPanelTexts["bonus"].GetComponent<Text>().text =
                "Current bonuses to experience are: \n" +
                runSpeedExpS + " from current run speed \n" +
                absWeightExpS + " from absolute weight carried \n" +
                relWeightExpS + " from fullness of inventory";

            SkillGUI.LeftPanelTexts["f1"].GetComponent<Text>().text =
                runSpeedBonusS + " run speed";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                equipmentMalusReduxS + " penalty from equipment";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                encumberanceReduxS + " penalty from encumberance";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                staminaDrainReduxS + " run stamina cost";

            SkillGUI.LeftPanelTexts["f5"].GetComponent<Text>().text =
                baseStaminaGainS + " base stamina";

            SkillGUI.LeftPanelTexts["f6"].GetComponent<Text>().text =
                "Current effects from outside factors: \n" +
                encumberanceFactorS + " speed from encumberance\n " +
                equipmentFactorS + " speed from equipment";
        }
        public static void OpenSailingPanels()
        {
            Player player = Player.m_localPlayer;
            float skill = player.GetSkillFactor(SkillMan.Sailing);
            Ship ship = player.GetStandingOnShip() ?? player.GetControlledShip();

            float sailXPRate = 0f;
            float sailCaptainBonus = 0f;
            float sailVesselBonus = 0f;
            float sailCrewBonus = 0f;

            if (ship != null)
            {
                if (player.GetControlledShip() == ship)
                {
                    sailXPRate = ConfigMan.SailXPCaptainBase.Value;
                    sailCaptainBonus = Mathf.Abs(ship.GetSpeed()) * ConfigMan.SailXPSpeedMod.Value;
                    sailCaptainBonus *= ConfigMan.GetSailXPWindMult(ship.GetWindAngleFactor());
                }
                else
                {
                    sailXPRate = ConfigMan.SailXPCrewBase.Value;
                }
                sailVesselBonus = MultToPer(ConfigMan.GetSailXPTierMult(ship));
                sailCrewBonus = MultToPer(ConfigMan.GetSailXPCrewMult(ship.m_players.Count));
            }

            float sailSpeed = MultToPer(ConfigMan.GetSailSpeedMult(skill));
            float sailWindNudge = MultToPer(1f + ConfigMan.GetSailWindNudgeMod(skill));
            float sailExploreRange = ConfigMan.GetSailExploreRange(skill);
            float sailPaddleSpeed = MultToPer(ConfigMan.GetSailPaddleSpeedMult(skill));
            float sailShipDamageRedux = MultToPer(ConfigMan.GetSailDamageRedux(skill), true);


            string sailSpeedS = PT.Prettify(sailSpeed, 1, PT.TType.Percent);
            string sailWindNudgeS = PT.Prettify(sailWindNudge, 1, PT.TType.Percent);
            string sailExploreRangeS = PT.Prettify(sailExploreRange, 0, PT.TType.Flat);
            string sailPaddleSpeedS = PT.Prettify(sailPaddleSpeed, 1, PT.TType.Percent);
            string sailShipDamageReduxS = PT.Prettify(sailShipDamageRedux, 0, PT.TType.PercentRedux);


            string sailXPRateS = PT.Prettify(sailXPRate, 2, PT.TType.Straight);
            string sailCaptainBonusS = PT.Prettify(sailCaptainBonus, 2, PT.TType.Flat);
            string sailVesselBonusS = PT.Prettify(sailVesselBonus, 0, PT.TType.Percent);
            string sailCrewBonusS = PT.Prettify(sailCrewBonus, 0, PT.TType.Percent);



            SkillGUI.LeftPanelTexts["x1"].GetComponent<Text>().text =
                "Swimming or standing on a ship will grant you a small amount of experience.";
            SkillGUI.LeftPanelTexts["x2"].GetComponent<Text>().text =
                "If you're captaining a vessel, you have a higher base experience rate and " +
                "it's increased by how fully the wind is caught and how fast the ship is moving.";
            SkillGUI.LeftPanelTexts["x3"].GetComponent<Text>().text =
                "All crew get more experience based on the tier of the vessel.";
            SkillGUI.LeftPanelTexts["bonus"].GetComponent<Text>().text =
                "Bonus experience is earned for all crewmates for each additional person there is onboard. \n" +
                "Current Bonuses: \n" +
                sailXPRateS + " per second base rate\n" +
                sailCaptainBonusS + " from Captain bonuses\n" +
                sailVesselBonusS + " from vessel tier\n" +
                sailCrewBonusS + " from number of crewmates";

            SkillGUI.LeftPanelTexts["f1"].GetComponent<Text>().text =
                sailSpeedS + " ship sailing speed";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                sailWindNudgeS + " nudge towards favorable winds";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                sailExploreRangeS + "m exploration range while on board ";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                sailPaddleSpeedS + " paddle speed";

            SkillGUI.LeftPanelTexts["f5"].GetComponent<Text>().text =
                sailShipDamageReduxS + " ship damage taken";
        }
        public static void OpenSpearPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Spears);

            float spearDamage = MultToPer(ConfigMan.GetSpearDamageMult(skill));
            float spearStaminaRedux = MultToPer(ConfigMan.GetSpearStaminaRedux(skill), true);
            float spearVelocity = MultToPer(ConfigMan.GetSpearVelocityMult(skill));
            float spearThrowDamage = MultToPer(ConfigMan.GetSpearProjectileDamageMult(skill));
            float spearBlock = ConfigMan.GetSpearBlockArmor(skill);

            string spearDamageS = PT.Prettify(spearDamage, 1, PT.TType.Percent);
            string spearStaminaReduxS = PT.Prettify(spearStaminaRedux, 1, PT.TType.PercentRedux);
            string spearVelocityS = PT.Prettify(spearVelocity, 1, PT.TType.Percent);
            string spearThrowDamageS = PT.Prettify(spearThrowDamage, 1, PT.TType.Percent);
            string spearBlockS = PT.Prettify(spearBlock, 0, PT.TType.Flat);


            SkillGUI.LeftPanelTexts["x1"].GetComponent<Text>().text =
                "A percentage of all damage dealt is turned into experience, " +
                "but the rate is higher when the damage is dealt to living cretures.";
            SkillGUI.LeftPanelTexts["x2"].GetComponent<Text>().text =
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not.";
            SkillGUI.LeftPanelTexts["x3"].GetComponent<Text>().text =
                "Holding a spear gains you experience at a very slow rate.";
            SkillGUI.LeftPanelTexts["bonus"].GetComponent<Text>().text =
                "Bonus experience anytime you hit an enemy with a thrown weapon.";

            SkillGUI.LeftPanelTexts["f1"].GetComponent<Text>().text =
                spearDamageS + " damage with spears ";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                spearStaminaReduxS + " stamina usage with spears";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                spearVelocityS + " velocity with all thrown weapons";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                spearThrowDamageS + " damage with all thrown weapons";

            SkillGUI.LeftPanelTexts["f5"].GetComponent<Text>().text =
                spearBlockS + " block armor";
        }
        public static void OpenSneakPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Sneak);
            StatsPatch.SneakUpdate(Player.m_localPlayer);

            float sneakSpeed = MultToPer(ConfigMan.GetSneakSpeedMult(skill));
            float sneakStaminaCost = ConfigMan.GetSneakStaminaDrain(skill);
            float sneakLightFactor = MultToPer(ConfigMan.GetSneakFactor(skill, 2f));
            float sneakDarkFactor = MultToPer(ConfigMan.GetSneakFactor(skill, 0f));

            float sneakDangerXPMod = MultToPer(KSSneak.GetDangerEXPMult(Player.m_localPlayer));

            string sneakSpeedS = PT.Prettify(sneakSpeed, 1, PT.TType.Percent);
            string sneakStaminaCostS = PT.Prettify(sneakStaminaCost, 1, PT.TType.Straight);
            string sneakLightFactorS = PT.Prettify(sneakLightFactor, 0, PT.TType.Percent);
            string sneakDarkFactorS = PT.Prettify(sneakDarkFactor, 0, PT.TType.Percent);

            string sneakDangerXPModS = PT.Prettify(sneakDangerXPMod, 1, PT.TType.ColorlessPercent);

            SkillGUI.LeftPanelTexts["x1"].GetComponent<Text>().text =
                "While you are actively avoiding detection of a nearby enemy, " +
                "you gain experience every second.";
            SkillGUI.LeftPanelTexts["x2"].GetComponent<Text>().text =
                "If you aren't nearby an enemy while sneaking, you gain 10% experience.";
            SkillGUI.LeftPanelTexts["bonus"].GetComponent<Text>().text =
                "You get bonus experience based on how dangerous the biggest enemy you're sneaing past is.\n" +
                "Current bonus from spotted enemy: " + sneakDangerXPModS;

            SkillGUI.LeftPanelTexts["f1"].GetComponent<Text>().text =
                sneakSpeedS + " sneak speed";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                sneakStaminaCostS + " stamina per second while sneaking";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                sneakLightFactorS + " ---";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                sneakDarkFactorS + " ---";
        }
        public static void OpenSwimPanels()
        {
            Player player = Player.m_localPlayer;
            StatsPatch.SwimSpeedUpdate(player);
            float skill = player.GetSkillFactor(Skills.SkillType.Swim);

            float swimSpeed = MultToPer(ConfigMan.GetSwimSpeedMult(skill));
            float swimAccel = MultToPer(ConfigMan.GetSwimAccelMult(skill));
            float swimTurn = MultToPer(ConfigMan.GetSwimTurnMult(skill));
            float swimStaminaCost = ConfigMan.GetSwimStaminaPerSec(skill);

            float absWeightExp = MultToPer(MovePatch.absoluteWeightBonus(player));
            float relWeightExp = MultToPer(MovePatch.relativeWeightBonus(player));
            float swimSpeedExp = MultToPer(MovePatch.swimSpeedExpBonus(player));

            string swimSpeedS = PT.Prettify(swimSpeed, 1, PT.TType.Percent);
            string swimAccelS = PT.Prettify(swimAccel, 1, PT.TType.Percent);
            string swimTurnS = PT.Prettify(swimTurn, 1, PT.TType.Percent);
            string swimStaminaCostS = PT.Prettify(swimStaminaCost, 2, PT.TType.Straight);

            string absWeightExpS = PT.Prettify(absWeightExp, 1, PT.TType.ColorlessPercent);
            string relWeightExpS = PT.Prettify(relWeightExp, 1, PT.TType.ColorlessPercent);
            string swimSpeedExpS = PT.Prettify(swimSpeedExp, 1, PT.TType.ColorlessPercent);


            SkillGUI.LeftPanelTexts["x1"].GetComponent<Text>().text =
                "The faster you are moving, the more experience you get.";
            SkillGUI.LeftPanelTexts["x2"].GetComponent<Text>().text =
                "The closer you are to fully encumbered, the less movespeed you have.";
            SkillGUI.LeftPanelTexts["x3"].GetComponent<Text>().text =
                "You also gain experience based on how encumbered you are. \n";
            SkillGUI.LeftPanelTexts["bonus"].GetComponent<Text>().text =
                "Current bonuses to experience are: \n" +
                swimSpeedExpS + " from current swimming speed \n" +
                absWeightExpS + " from absolute weight carried \n" +
                relWeightExpS + " from fullness of inventory";

            SkillGUI.LeftPanelTexts["f1"].GetComponent<Text>().text =
                swimSpeedS + " swim speed";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                swimAccelS + " acceleration in water";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                swimTurnS + " turn speed in water";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                swimStaminaCostS + " stamina per second swim cost";
        }
        public static void OpenSwordPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Swords);
            StatsPatch.SwordUpdate(Player.m_localPlayer);


            float swordDamage = MultToPer(ConfigMan.GetSwordDamageMult(skill));
            float swordStaminaRedux = MultToPer(ConfigMan.GetSwordStaminaRedux(skill), true);
            float swordParry = MultToPer(ConfigMan.GetSwordParryMult(skill));
            float swordSlash = MultToPer(ConfigMan.GetSwordSlashMult(skill));
            float swordDodgeStaminaRedux = MultToPer(ConfigMan.GetSwordDodgeStaminaRedux(skill), true);

            string swordDamageS = PT.Prettify(swordDamage, 1, PT.TType.Percent);
            string swordStaminaReduxS = PT.Prettify(swordStaminaRedux, 1, PT.TType.PercentRedux);
            string swordParryS = PT.Prettify(swordParry, 0, PT.TType.Percent);
            string swordSlashS = PT.Prettify(swordSlash, 1, PT.TType.Percent);
            string swordDodgeStaminaReduxS = PT.Prettify(swordDodgeStaminaRedux, 1, PT.TType.PercentRedux);

            SkillGUI.LeftPanelTexts["x1"].GetComponent<Text>().text =
                "A percentage of all damage dealt is turned into experience, " +
                "but the rate is higher when the damage is dealt to living cretures.";
            SkillGUI.LeftPanelTexts["x2"].GetComponent<Text>().text =
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not.";
            SkillGUI.LeftPanelTexts["x3"].GetComponent<Text>().text =
                "Holding a sword gains you experience at a very slow rate.";
            SkillGUI.LeftPanelTexts["bonus"].GetComponent<Text>().text =
                "Bonus experience is gained every time you deal damage to a staggered enemy with a sword.";

            SkillGUI.LeftPanelTexts["f1"].GetComponent<Text>().text =
                swordDamageS + " damage with swords ";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                swordStaminaReduxS + " stamina usage with swords";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                swordParryS + " parry bonus with ALL weapons ";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                swordSlashS + " slash damage with ALL weapons ";

            SkillGUI.LeftPanelTexts["f5"].GetComponent<Text>().text =
                swordDodgeStaminaReduxS + " stamina cost to dodge";
        }
        public static void OpenWoodcuttingPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.WoodCutting);

            float woodDamage = MultToPer(1 + ConfigMan.GetWoodcuttingDamageMod(skill));
            float woodDrop = MultToPer(ConfigMan.GetWoodDropMod(skill)+1f);
            float woodRebate = ConfigMan.GetWoodcuttingStaminaRebate(skill);
            float woodRegen = ConfigMan.GetWoodcuttingRegenLessTime(skill);
            float woodCarry = ConfigMan.GetWoodcuttingCarryCapacity(skill);

            string woodDamageS = PT.Prettify(woodDamage, 1, PT.TType.Percent);
            string woodDropS = PT.Prettify(woodDrop, 2, PT.TType.Percent);
            string woodRebateS = PT.Prettify(woodRebate, 0, PT.TType.Flat);
            string woodRegenS = PT.Prettify(woodRegen, 0, PT.TType.Straight);
            string woodCarryS = PT.Prettify(woodCarry, 0, PT.TType.Flat);

            SkillGUI.LeftPanelTexts["x1"].GetComponent<Text>().text =
                "A percentage of all damage dealt to trees is turned into experience.";
            SkillGUI.LeftPanelTexts["x2"].GetComponent<Text>().text =
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not.";
            SkillGUI.LeftPanelTexts["bonus"].GetComponent<Text>().text = 
                "Bonus experience for the axe is gained when you destroy tree stumps.";

            SkillGUI.LeftPanelTexts["f1"].GetComponent<Text>().text =
                woodDamageS + " woodcutting damage";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                woodDropS + " drop rates from trees";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                woodRebateS + " stamina rebate on woodcutting swings";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                woodRegenS + " fewer seconds before stamina regeneration";

            SkillGUI.LeftPanelTexts["f5"].GetComponent<Text>().text =
                woodCarryS + " carrying capacity";
        }

        public static float MultToPer(float number, bool redux = false)
        {
            if (redux) return (1 - number) * 100; 
            return (number - 1) * 100;
        }



        public static void ResetText()
        {
            SkillGUI.LeftPanelTexts["x1"].GetComponent<Text>().text = "";
            SkillGUI.LeftPanelTexts["x2"].GetComponent<Text>().text = "";
            SkillGUI.LeftPanelTexts["x3"].GetComponent<Text>().text = "";
            //SkillGUI.LeftPanelTexts["x4"].GetComponent<Text>().text = "";
            SkillGUI.LeftPanelTexts["bonus"].GetComponent<Text>().text = "";

            SkillGUI.LeftPanelTexts["f1"].GetComponent<Text>().text = "";
            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text = "";
            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text = "";
            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text = "";
            SkillGUI.LeftPanelTexts["f5"].GetComponent<Text>().text = "";
            SkillGUI.LeftPanelTexts["f6"].GetComponent<Text>().text = "";
        }
    }


}
