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


            if (!ConfigManager.IsSkillActive(skill))
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

            float plantXPReward = 0f;

            float botYield = MultToPer(ConfigManager.GetAgricultureYieldMult(skill));
            float botGrowRedux = MultToPer(ConfigManager.GetAgricultureGrowTimeRedux(skill));
            float botFoodQuality = MultToPer(ConfigManager.GetAgricultureFoodQualityMult(skill));
            float botHealthGain = ConfigManager.GetAgricultureHealthRegain(skill);


            SkillGUI.LeftPanelTexts["x1"].GetComponent<Text>().text =
                "Every item you harvest gives you experience, whether in the wild or in your planted garden.";
            SkillGUI.LeftPanelTexts["x2"].GetComponent<Text>().text =
                "Different plants give you different rewards. As a rule, items you've " +
                "planted tend to give you more experience.";
            SkillGUI.LeftPanelTexts["x3"].GetComponent<Text>().text =
                "Trees you plant will give you experience as soon as they mature.";
            SkillGUI.LeftPanelTexts["bonus"].GetComponent<Text>().text =
                "Experience bounty from hovered plant: " + plantXPReward.ToString("F1");

            SkillGUI.LeftPanelTexts["f1"].GetComponent<Text>().text =
                botYield.ToString("F2") + "% extra yield from plants";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                botGrowRedux.ToString("F0") + "% less time to grow plants";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                botFoodQuality.ToString("F0") + "% increased food quality for food you harvest";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                botHealthGain.ToString("F1") + " health regained every time you harvest a plant";

        }
        public static void OpenAxePanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Axes);

            float axeDamage = MultToPer(ConfigManager.GetAxeDamageMult(skill));
            float axeStaminaRedux = MultToPer(ConfigManager.GetAxeStaminaRedux(skill));
            float axeStaminaGain = ConfigManager.GetAxeStamina(skill);
            float axeChopBonus = MultToPer(1f + ConfigManager.GetAxeChopDamageMod(skill));
            float axeCarryCapacity = ConfigManager.GetAxeCarryCapacity(skill);

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
                axeDamage.ToString("F1") + "% extra damage with axes";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                axeStaminaRedux.ToString("F1") + "% stamina usage with axes";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                axeStaminaGain.ToString("F0") + " higher base stamina";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                axeChopBonus.ToString("F1") + "% extra chop damage";

            SkillGUI.LeftPanelTexts["f5"].GetComponent<Text>().text =
                axeCarryCapacity.ToString("F0") + " extra carry capacity";
        }
        public static void OpenBlockPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Blocking);

            float staminaRedux = MultToPer(ConfigManager.GetBlockStaminaRedux(skill));
            float baseBlockPower = ConfigManager.GetBlockPowerFlat(skill);
            float blockPerArmor = MultToPer(ConfigManager.GetBlockPowerMult(skill));
            float blockHealth = ConfigManager.GetBlockHealth(skill);
            float parryExpMod = MultToPer(ConfigManager.GetBlockParryExpMult());

            SkillGUI.LeftPanelTexts["x1"].GetComponent<Text>().text =
                "A percentage of all damage you block is turned into experience.";
            SkillGUI.LeftPanelTexts["x2"].GetComponent<Text>().text =
                "This number is unaffected by resistances.";
            SkillGUI.LeftPanelTexts["bonus"].GetComponent<Text>().text =
                "You get " + parryExpMod.ToString("F0") + "% bonus experience for parrying an attack.";

            SkillGUI.LeftPanelTexts["f1"].GetComponent<Text>().text =
                staminaRedux.ToString("F1") + "% stamina cost for blocks";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                baseBlockPower.ToString("F0") + " extra flat block armor";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                blockPerArmor.ToString("F1") + "% extra block armor";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                blockHealth.ToString("F0") + " extra max health";
        }
        public static void OpenBowPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Bows);

            float bowDamage = MultToPer(ConfigManager.GetBowDamageMult(skill));
            float bowStaminaRedux = MultToPer(ConfigManager.GetBowStaminaRedux(skill));
            float bowVelocity = MultToPer(ConfigManager.GetBowVelocityMult(skill));
            float bowDrawSpeed = ConfigManager.GetBowDrawSpeed(skill);
            float bowDropRate = MultToPer(ConfigManager.GetBowDropRateMod(skill)+1f);

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
                bowDamage.ToString("F1") + "% extra damage with bows";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                bowStaminaRedux.ToString("F1") + "% stamina usage with bows";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                bowVelocity.ToString("F1") + "% extra arrow velocity";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                bowDrawSpeed.ToString("F1") + "% draw speed";

            SkillGUI.LeftPanelTexts["f5"].GetComponent<Text>().text =
                bowDropRate.ToString("F1") + "% extra loot drops from creatures";
        }
        public static void OpenBuildingPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(SkillMan.Building);

            float buildHealth = MultToPer(ConfigManager.GetBuildingHealthMult(skill));
            float buildStability = MultToPer(ConfigManager.GetBuildingStabilityMult(skill));
            float buildDamage = MultToPer(ConfigManager.GetBuildingDamageMult(skill));
            float buildWNT = MultToPer(ConfigManager.GetBuildingWNTRedux(skill));
            float buildFreeChance = MultToPer(1f + ConfigManager.GetBuildingFreeMod(skill));
            float buildStaminaRedux = MultToPer(ConfigManager.GetBuildingStaminaRedux(skill));


            SkillGUI.LeftPanelTexts["x1"].GetComponent<Text>().text =
                "Placing a building gives you a flat amount of experience..";
            SkillGUI.LeftPanelTexts["x2"].GetComponent<Text>().text =
                "Repairing a building earns you experience based on how much health you repaired.";
            SkillGUI.LeftPanelTexts["x3"].GetComponent<Text>().text =
                "Creatures damaging your buildings also provide damage-based experience.";
            SkillGUI.LeftPanelTexts["bonus"].GetComponent<Text>().text =
                "Bonus experience for building is earned when your buildings damage creatures, based on damage dealt.";

            SkillGUI.LeftPanelTexts["f1"].GetComponent<Text>().text =
                buildHealth.ToString("F1") + "% extra health to your buildings";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                buildStability.ToString("F1") + "% extra structural stability to your buildings";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                buildDamage.ToString("F0") + "% extra damage dealt by your buildings ";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                buildWNT.ToString("F0") + "% reduced weather damage taken by your buildings";

            SkillGUI.LeftPanelTexts["f5"].GetComponent<Text>().text =
                buildFreeChance.ToString("F0") + "% chance to place a free building";

            SkillGUI.LeftPanelTexts["f6"].GetComponent<Text>().text =
                buildStaminaRedux.ToString("F0") + "% reduced stamina cost for placing buildings and repairing";
        }
        public static void OpenClubPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Clubs);

            float clubDamage = MultToPer(ConfigManager.GetClubDamageMult(skill));
            float clubStaminaRedux = MultToPer(ConfigManager.GetClubStaminaRedux(skill));
            float clubBlunt = MultToPer(ConfigManager.GetClubBluntMult(skill));
            float clubKnockback = MultToPer(ConfigManager.GetClubKnockbackMult(skill));
            float clubStagger = MultToPer(ConfigManager.GetClubStaggerMult(skill));

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
                clubDamage.ToString("F1") + "% extra damage with clubs";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                clubStaminaRedux.ToString("F1") + "% stamina usage with clubs";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                clubBlunt.ToString("F1") + "% extra bonus to ALL blunt damage";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                clubKnockback.ToString("F1") + "% knockback bonus to ALL weapons";

            SkillGUI.LeftPanelTexts["f5"].GetComponent<Text>().text =
                clubStagger.ToString("F1") + "% extra stagger damage to ALL weapons";
        }
        public static void OpenCookingPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(SkillMan.Cooking);

            float cookAvgFQ = MultToPer(ConfigManager.GetCookingAverageFoodQualityMod(skill));
            float cookFQRange = MultToPer(ConfigManager.GetCookingFoodQualityRangeMod(skill));
            float cookTimeRedux = MultToPer(ConfigManager.GetCookingTimeRedux(skill));
            float cookFermentTimeRedux = MultToPer(ConfigManager.GetCookingFermentTimeRedux(skill));


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
                cookAvgFQ.ToString("F1") + "% average food quality of cooked items";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                cookFQRange.ToString("F1") + "% range in possible food qualities";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                cookTimeRedux.ToString("F1") + "% reduced cooking time";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                cookFermentTimeRedux.ToString("F0") + "% reduced fermentation time";

            SkillGUI.LeftPanelTexts["f5"].GetComponent<Text>().text =
                "[Food Quality]: \nFood quality is a king's skills specific property. Any item " +
                "you cook gets a random food quality, which directly affects it's health, stamina, and " +
                "duration. FQ is based primarily on your skill level, but your timing has an affect on the " +
                "overall quality as well.";

        }
        public static void OpenFistPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Unarmed);

            float fistDamage = MultToPer(ConfigManager.GetFistDamageMult(skill));
            float fistStaminaRedux = MultToPer(ConfigManager.GetFistStaminaRedux(skill));
            float fistFlatDamage = ConfigManager.GetFistDamageFlat(skill);
            float fistBlock = ConfigManager.GetFistBlockArmor(skill);
            float fistMovespeed = MultToPer(1f + ConfigManager.GetFistMovespeedMod(skill));

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
                fistDamage.ToString("F1") + "% extra damage with fists";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                fistStaminaRedux.ToString("F1") + "% stamina usage with fists";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                fistFlatDamage.ToString("F0") + " extra flat damage";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                fistBlock.ToString("F0") + " extra unarmed block armor";

            SkillGUI.LeftPanelTexts["f5"].GetComponent<Text>().text =
                fistMovespeed.ToString("F1") + "% extra move speed";
        }
        public static void OpenJumpPanels()
        {
            Player player = Player.m_localPlayer;
            StatsPatch.JumpForceUpdate(player);
            float skill = player.GetSkillFactor(Skills.SkillType.Jump);

            float bonusJumpForce = MultToPer(ConfigManager.GetJumpForceMult(skill));
            float bonusJumpForwardForce = MultToPer(ConfigManager.GetJumpForwardForceMult(skill));
            float staminaRedux = MultToPer(ConfigManager.GetJumpStaminaRedux(skill));
            float tired = MultToPer(ConfigManager.GetJumpTiredMod(skill) + ConfigManager.BaseJumpTiredFactor);

            float fallDamageThreshhold = ConfigManager.GetFallDamageThreshold(skill);
            float fallDamageRedux = MultToPer(ConfigManager.GetFallDamageRedux(skill));

            SkillGUI.LeftPanelTexts["x1"].GetComponent<Text>().text =
                "Every time you jump, you gain a small, flat amount of experience.";
            SkillGUI.LeftPanelTexts["bonus"].GetComponent<Text>().text =
                "Every time you land, you gain bonus experience based on " +
                "how far you fell and how much damage you would've normally taken without " +
                "fall damage resistance.";

            SkillGUI.LeftPanelTexts["f1"].GetComponent<Text>().text =
                bonusJumpForce.ToString("F1") + "% extra vertical jump force ";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                bonusJumpForwardForce.ToString("F1") + "% extra horizontal jump force ";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                staminaRedux.ToString("F1") + "% less stamina cost to jump ";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                tired.ToString("F0") + "% jump force modifier when tired";

            SkillGUI.LeftPanelTexts["f5"].GetComponent<Text>().text =
                fallDamageThreshhold.ToString("F1") + "m minimum fall damage height ";

            SkillGUI.LeftPanelTexts["f6"].GetComponent<Text>().text =
                fallDamageRedux.ToString("F1") + "% less fall damage";
        }
        public static void OpenKnifePanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Knives);

            float knifeDamage = MultToPer(ConfigManager.GetKnifeDamageMult(skill));
            float knifeStaminaRedux = MultToPer(ConfigManager.GetKnifeStaminaRedux(skill));
            float knifeBackstab = MultToPer(ConfigManager.GetKnifeBackstabMult(skill));
            float knifeMovespeed = MultToPer(1f + ConfigManager.GetKnifeMovespeedMod(skill));
            float knifePierce = MultToPer(ConfigManager.GetKnifePierceMult(skill));

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
                knifeDamage.ToString("F1") + "% extra damage with knives ";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                knifeStaminaRedux.ToString("F1") + "% stamina usage with knives ";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                knifeBackstab.ToString("F0") + "% sneak attack bonus damage ";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                knifeMovespeed.ToString("F1") + "% extra move speed ";

            SkillGUI.LeftPanelTexts["f5"].GetComponent<Text>().text =
                knifePierce.ToString("F1") + "% extra to ALL pierce damage";
        }
        public static void OpenMiningPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Pickaxes);

            float mineDamage = MultToPer(ConfigManager.GetMiningDamageMult(skill));
            float mineDrop = MultToPer(ConfigManager.GetMiningDropMod(skill) + 1f);
            float mineRebate = ConfigManager.GetMiningStaminaRebate(skill);
            float mineRegen = ConfigManager.GetMiningRegenLessTime(skill);
            float mineCarry = ConfigManager.GetMiningCarryCapacity(skill);

            SkillGUI.LeftPanelTexts["x1"].GetComponent<Text>().text =
                "A percentage of all damage dealt to rocks or ore is turned into experience.";
            SkillGUI.LeftPanelTexts["x2"].GetComponent<Text>().text =
                "You still gain experience for hitting the ground, but at a reduced pace.";
            SkillGUI.LeftPanelTexts["x3"].GetComponent<Text>().text =
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not.";
            SkillGUI.LeftPanelTexts["bonus"].GetComponent<Text>().text =
                "Bonus experience for the pick is gained based on the tier of the tool used.";

            SkillGUI.LeftPanelTexts["f1"].GetComponent<Text>().text =
                mineDamage.ToString("F1") + "% extra damage to rocks";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                mineDrop.ToString("F2") + "% increased ore drop rates";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                mineRebate.ToString("F0") + " stamina rebate on pick swings that hit a rock";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                mineRegen.ToString("F0") + " fewer seconds between health regeneration ticks";

            SkillGUI.LeftPanelTexts["f5"].GetComponent<Text>().text =
                mineCarry.ToString("F0") + " extra carrying capacity";
        }
        public static void OpenPolearmPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Polearms);

            float polearmDamage = MultToPer(ConfigManager.GetPolearmDamageMult(skill));
            float polearmStaminaRedux = MultToPer(ConfigManager.GetPolearmStaminaRedux(skill));
            float polearmRange = ConfigManager.GetPolearmRange(skill);
            float polearmArmor = ConfigManager.GetPolearmArmor(skill);
            float polearmBlock = ConfigManager.GetPolearmBlock(skill);

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
                polearmDamage.ToString("F1") + "% extra damage with polearms ";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                polearmStaminaRedux.ToString("F1") + "% stamina usage with polearms";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                polearmRange.ToString("F0") + " increased units of range with all weapons";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                polearmArmor.ToString("F0") + " increased armor";

            SkillGUI.LeftPanelTexts["f5"].GetComponent<Text>().text =
                polearmBlock.ToString("F0") + " extra block power with polearms";
        }
        public static void OpenRunPanels()
        {
            Player player = Player.m_localPlayer;
            StatsPatch.RunSpeedUpdate(player);
            float skill = player.GetSkillFactor(Skills.SkillType.Run);

            float runSpeedBonus = MultToPer(ConfigManager.GetRunSpeedMult(skill));
            float equipmentMalusRedux = MultToPer(ConfigManager.GetEquipmentRedux(skill));
            float encumberanceRedux = MultToPer(ConfigManager.GetEncumberanceRedux(skill));
            float staminaDrainRedux = MultToPer(ConfigManager.GetRunStaminaRedux(skill));
            float baseStaminaGain = ConfigManager.GetRunStamina(skill);


            float encumberanceFactor = MultToPer(MovePatch.GetEncumberanceMult(player));
            float equipmentFactor = MultToPer(MovePatch.GetEquipmentMult(player));

            float absWeightExp = MultToPer(MovePatch.absoluteWeightBonus(player));
            float relWeightExp = MultToPer(MovePatch.relativeWeightBonus(player));
            float runSpeedExp = MultToPer(MovePatch.runSpeedExpBonus(player));

            SkillGUI.LeftPanelTexts["x1"].GetComponent<Text>().text =
                "The faster you are moving, the more experience you get.";
            SkillGUI.LeftPanelTexts["x2"].GetComponent<Text>().text =
                "The closer you are to fully encumbered, the less movespeed you have.";
            SkillGUI.LeftPanelTexts["x3"].GetComponent<Text>().text =
                "You also gain experience based on how encumbered you are.";
            SkillGUI.LeftPanelTexts["bonus"].GetComponent<Text>().text =
                "Current bonuses to experience are: \n" +
                runSpeedExp.ToString("F1") + "% from current run speed \n" +
                absWeightExp.ToString("F1") + "% from absolute weight carried \n" +
                relWeightExp.ToString("F1") + "% from fullness of inventory";

            SkillGUI.LeftPanelTexts["f1"].GetComponent<Text>().text =
                runSpeedBonus.ToString("F1") + "% extra run speed";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                equipmentMalusRedux.ToString("F1") + "% to equipment penalty";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                encumberanceRedux.ToString("F1") + "% to encumberance penalty";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                staminaDrainRedux.ToString("F1") + "% to stamina cost to run";

            SkillGUI.LeftPanelTexts["f5"].GetComponent<Text>().text =
                baseStaminaGain.ToString("F1") + " extra base stamina";

            SkillGUI.LeftPanelTexts["f6"].GetComponent<Text>().text =
                "Current effects from outside factors: \n" +
                encumberanceFactor.ToString("F1") + "% speed from encumberance\n " +
                equipmentFactor.ToString("F1") + "% speed from equipment";
        }
        public static void OpenSailingPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(SkillMan.Sailing);

            float sailXPRate = 0f;//ConfigManager.GetSailXPRate();
            float sailCaptainBonus = 0f;//MultToPer(ConfigManager.GetSailCaptainBonus());
            float sailVeselBonus = 0f;//MultToPer(ConfigManager.GetSailVesselBonus());
            float sailCrewBonus = 0f;//MultToPer(ConfigManager.GetSailCrewBonus());

            float sailSpeed = MultToPer(ConfigManager.GetSailSpeedMult(skill));
            float sailWindNudge = MultToPer(ConfigManager.GetSailWindNudgeMod(skill));
            float sailExploreRange = ConfigManager.GetSailExploreRange(skill);
            float sailPaddleSpeed = MultToPer(ConfigManager.GetSailPaddleSpeedMult(skill));
            float sailShipDamageRedux = MultToPer(ConfigManager.GetSailDamageRedux(skill));



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
                sailXPRate.ToString("F0") + " per second base rate\n" +
                sailCaptainBonus.ToString("F1") + "% from Captain bonuses\n" +
                sailVeselBonus.ToString("F0") + "% from vessel tier\n" +
                sailCrewBonus.ToString("F0") + "% from number of crewmates";

            SkillGUI.LeftPanelTexts["f1"].GetComponent<Text>().text =
                sailSpeed.ToString("F1") + "% extra faster ship sailing speed";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                sailWindNudge.ToString("F1") + "% nudge towards favorable winds";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                sailExploreRange.ToString("F0") + "% increased exploration range while on board ";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                sailPaddleSpeed.ToString("F1") + "% increased paddle speed";

            SkillGUI.LeftPanelTexts["f5"].GetComponent<Text>().text =
                sailShipDamageRedux.ToString("F0") + "% less ship damage taken";
        }
        public static void OpenSpearPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Spears);

            float spearDamage = MultToPer(ConfigManager.GetSpearDamageMult(skill));
            float spearStaminaRedux = MultToPer(ConfigManager.GetSpearStaminaRedux(skill));
            float spearVelocity = MultToPer(ConfigManager.GetSpearVelocityMult(skill));
            float spearThrowDamage = MultToPer(ConfigManager.GetSpearProjectileDamageMult(skill));
            float spearBlock = ConfigManager.GetSpearBlockArmor(skill);

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
                spearDamage.ToString("F1") + "% extra damage with spears ";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                spearStaminaRedux.ToString("F1") + "% stamina usage with spears";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                spearVelocity.ToString("F1") + "% increased velocity with all thrown weapons";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                spearThrowDamage.ToString("F1") + "% increased damage with all thrown weapons";

            SkillGUI.LeftPanelTexts["f5"].GetComponent<Text>().text =
                spearBlock.ToString("F0") + " higher flat block armor";
        }
        public static void OpenSneakPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Sneak);
            StatsPatch.SneakUpdate(Player.m_localPlayer);

            float sneakSpeed = MultToPer(ConfigManager.GetSneakSpeedMult(skill));
            float sneakStaminaCost = ConfigManager.GetSneakStaminaDrain(skill);
            float sneakLightFactor = MultToPer(ConfigManager.GetSneakFactor(skill, 2f));
            float sneakDarkFactor = MultToPer(ConfigManager.GetSneakFactor(skill, 0f));

            float sneakDangerXPMod = MultToPer(SneakPatch.GetDangerEXPMult(Player.m_localPlayer));

            SkillGUI.LeftPanelTexts["x1"].GetComponent<Text>().text =
                "While you are actively avoiding detection of a nearby enemy, " +
                "you gain experience every second.";
            SkillGUI.LeftPanelTexts["x2"].GetComponent<Text>().text =
                "If you aren't nearby an enemy while sneaking, you gain 10% experience.";
            SkillGUI.LeftPanelTexts["bonus"].GetComponent<Text>().text =
                "You get bonus experience based on how dangerous the biggest enemy you're sneaing past is.\n" +
                "Current bonus from spotted enemy: " +
                sneakDangerXPMod.ToString("F1") + "%";

            SkillGUI.LeftPanelTexts["f1"].GetComponent<Text>().text =
                sneakSpeed.ToString("F1") + "% increased speed while crouching";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                sneakStaminaCost.ToString("F1") + " stamina per second while crouching";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                sneakLightFactor.ToString("F0") + "% increased sneakiness in the light ";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                sneakDarkFactor.ToString("F0") + "% increased sneakiness in the dark";
        }
        public static void OpenSwimPanels()
        {
            Player player = Player.m_localPlayer;
            StatsPatch.SwimSpeedUpdate(player);
            float skill = player.GetSkillFactor(Skills.SkillType.Swim);

            float swimSpeed = MultToPer(ConfigManager.GetSwimSpeedMult(skill));
            float swimAccel = MultToPer(ConfigManager.GetSwimAccelMult(skill));
            float swimTurn = MultToPer(ConfigManager.GetSwimTurnMult(skill));
            float swimStaminaCost = ConfigManager.GetSwimStaminaPerSec(skill);

            float absWeightExp = MultToPer(MovePatch.absoluteWeightBonus(player));
            float relWeightExp = MultToPer(MovePatch.relativeWeightBonus(player));
            float swimSpeedExp = MultToPer(MovePatch.swimSpeedExpBonus(player));


            SkillGUI.LeftPanelTexts["x1"].GetComponent<Text>().text =
                "The faster you are moving, the more experience you get.";
            SkillGUI.LeftPanelTexts["x2"].GetComponent<Text>().text =
                "The closer you are to fully encumbered, the less movespeed you have.";
            SkillGUI.LeftPanelTexts["x3"].GetComponent<Text>().text =
                "You also gain experience based on how encumbered you are. \n";
            SkillGUI.LeftPanelTexts["bonus"].GetComponent<Text>().text =
                "Current bonuses to experience are: \n" +
                swimSpeedExp.ToString("F1") + "% experience bonus from current swimming speed \n" +
                absWeightExp.ToString("F1") + "% experience bonus from absolute weight carried \n" +
                relWeightExp.ToString("F1") + "% experience bonus from fullness of inventory";

            SkillGUI.LeftPanelTexts["f1"].GetComponent<Text>().text =
                swimSpeed.ToString("F1") + "% extra swim speed";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                swimAccel.ToString("F1") + "% extra acceleration in water";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                swimTurn.ToString("F1") + "% extra turn speed in water";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                swimStaminaCost.ToString("F2") + " stamina per second while swimming";
        }
        public static void OpenSwordPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Swords);
            StatsPatch.SwordUpdate(Player.m_localPlayer);


            float swordDamage = MultToPer(ConfigManager.GetSwordDamageMult(skill));
            float swordStaminaRedux = MultToPer(ConfigManager.GetSwordStaminaRedux(skill));
            float swordParry = MultToPer(ConfigManager.GetSwordParryMult(skill));
            float swordSlash = MultToPer(ConfigManager.GetSwordSlashMult(skill));
            float swordDodgeStaminaRedux = MultToPer(ConfigManager.GetSwordDodgeStaminaRedux(skill));

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
                swordDamage.ToString("F1") + "% extra damage with swords ";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                swordStaminaRedux.ToString("F1") + "% stamina usage with swords";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                swordParry.ToString("F0") + "% higher parry bonus with ALL weapons ";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                swordSlash.ToString("F1") + "% increased slash damage with ALL weapons ";

            SkillGUI.LeftPanelTexts["f5"].GetComponent<Text>().text =
                swordDodgeStaminaRedux.ToString("F1") + "% stamina cost to dodge";
        }
        public static void OpenWoodcuttingPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.WoodCutting);

            float woodDamage = MultToPer(1 + ConfigManager.GetWoodcuttingDamageMod(skill));
            float woodDrop = MultToPer(ConfigManager.GetWoodDropMod(skill)+1f);
            float woodRebate = ConfigManager.GetWoodcuttingStaminaRebate(skill);
            float woodRegen = ConfigManager.GetWoodcuttingRegenLessTime(skill);
            float woodCarry = ConfigManager.GetWoodcuttingCarryCapacity(skill);

            SkillGUI.LeftPanelTexts["x1"].GetComponent<Text>().text =
                "A percentage of all damage dealt to trees is turned into experience.";
            SkillGUI.LeftPanelTexts["x2"].GetComponent<Text>().text =
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not.";
            SkillGUI.LeftPanelTexts["bonus"].GetComponent<Text>().text = 
                "Bonus experience for the axe is gained when you destroy tree stumps.";

            SkillGUI.LeftPanelTexts["f1"].GetComponent<Text>().text =
                woodDamage.ToString("F1") + "% extra damage to trees";

            SkillGUI.LeftPanelTexts["f2"].GetComponent<Text>().text =
                woodDrop.ToString("F2") + "% increased wood drop rates from trees";

            SkillGUI.LeftPanelTexts["f3"].GetComponent<Text>().text =
                woodRebate.ToString("F0") + " stamina rebate on axe swings that hit a tree";

            SkillGUI.LeftPanelTexts["f4"].GetComponent<Text>().text =
                woodRegen.ToString("F0") + " fewer seconds before stamina regeneration";

            SkillGUI.LeftPanelTexts["f5"].GetComponent<Text>().text =
                woodCarry.ToString("F0") + " extra carrying capacity";
        }

        public static float MultToPer(float number)
        {
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
