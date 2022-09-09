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
    class SkillGUIUpdate
    {
        //how many seconds between GUI update
        public const float updateGUITimer = 2f;

        public static float timeSinceUpdate = 0f;
        public static bool closable = true;

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
                SkillGUI.Init();
            }
            if (!closable) return;

            bool state = !SkillGUI.SkillGUIWindow.activeSelf;

            SkillGUI.SkillGUIWindow.SetActive(state);

            GUIManager.BlockInput(state);

            if (state)
            {
                GUICheck();
            }
        }

        public static void OnDropdownValueChange()
        {
            //Reset the scroll position

            //Check if the up and down buttons should be turned off or on
            if (SkillGUI.dd.value == 0) SkillGUI.ddUpBtn.GetComponent<Button>().enabled = false;
            else SkillGUI.ddUpBtn.GetComponent<Button>().enabled = true;
            if (SkillGUI.dd.value == SkillGUI.dd.options.Count-1) SkillGUI.ddDownBtn.GetComponent<Button>().enabled = false;
            else SkillGUI.ddDownBtn.GetComponent<Button>().enabled = true;

            GUICheck();
            SkillGUI.LPEffectsScroll.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
            SkillGUI.LPTipsScroll.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
        }
        public static void DDDown()
        {
            if (SkillGUI.dd.value < SkillGUI.dd.options.Count-1)
            {
                SkillGUI.dd.value++;
            }
        }
        public static void DDUp()
        {
            if (SkillGUI.dd.value > 0)
            {
                SkillGUI.dd.value--;
            }
        }
        public static void StickGUI() => GUIManager.BlockInput(false);
        public static void OnEffectsTab() => SetTipsTab(false);
        public static void OnTipsTab() => SetTipsTab(true);
        public static void SetTipsTab(bool isActive)
        {
            SkillGUI.LeftPanelEffectsTab.SetActive(!isActive);
            SkillGUI.LeftPanelTipsTab.SetActive(isActive);
        }

        public static void SetInteractable(bool interactable)
        {
            SkillGUI.dd.interactable = interactable;
            SkillGUI.ddDownBtn.GetComponent<Button>().interactable = interactable;
            SkillGUI.ddUpBtn.GetComponent<Button>().interactable = interactable;
            SkillGUI.LPTipsTabBtn.GetComponent<Button>().interactable = interactable;
            SkillGUI.LPEffectsTabBtn.GetComponent<Button>().interactable = interactable;
            SkillGUI.RPAscendedBtn.GetComponent<Button>().interactable = interactable;
            SkillGUI.CloseBtn.GetComponent<Button>().interactable = interactable;
            SkillGUI.StickBtn.GetComponent<Button>().interactable = interactable;
            closable = interactable;
        }

        public static void GUICheck()
        {
            //Jotunn.Logger.LogMessage($"Detected a dropdown value change.");
            SkillGUIData data = new SkillGUIData();
            Player player = Player.m_localPlayer;
            Skills.SkillType skill = Skills.SkillType.None;

            string skillName = SkillGUI.dd.options[SkillGUI.dd.value].text;
            StatsUpdate.UpdateStats(player);
            ResetText();

            skill = CFG.GetSkillFromName(skillName);

            //Deciding which skillgui object to call for filling in the data
            switch (skillName)
            {
                case "Agriculture":
                    data = new AgricultureGUI();
                    break;
                case "Axes":
                    data = new AxeGUI();
                    break;
                case "Blocking":
                    data = new BlockGUI();
                    break;
                case "Bows":
                    data = new BowGUI();
                    break;
                case "Building":
                    data = new BuildGUI();
                    break;
                case "Clubs":
                    data = new ClubGUI();
                    break;
                case "Cooking":
                    data = new CookGUI();
                    break;
                case "Fists":
                    data = new FistGUI();
                    break;
                case "Jump":
                    data = new JumpGUI();
                    break;
                case "Knives":
                    data = new KnifeGUI();
                    break;
                case "Mining":
                    data = new MineGUI();
                    break;
                case "Polearms":
                    data = new PolearmGUI();
                    break;
                case "Run":
                    data = new RunGUI();
                    break;
                case "Sailing":
                    data = new SailGUI();
                    break;
                case "Spears":
                    data = new SpearGUI();
                    break;
                case "Sneak":
                    data = new SneakGUI();
                    break;
                case "Swim":
                    data = new SwimGUI();
                    break;
                case "Swords":
                    data = new SwordGUI();
                    break;
                case "Woodcutting":
                    data = new WoodGUI();
                    break;
            }



            Skills.Skill skillRef = player.GetSkills().GetSkill(skill);

            SkillGUI.SSIcon.GetComponent<Image>().sprite = player.m_skills.GetSkillDef(skill).m_icon;
            SkillGUI.SSName.GetComponent<Text>().text = skillName;
            //If we hit max level, no need to have an experience counter anymore
            if (skillRef.m_level >= CFG.MaxSkillLevel.Value)
            {
                SkillGUI.SSLevel.GetComponent<Text>().text = "Level: MAX!";
                SkillGUI.SSExp.GetComponent<Text>().text = "Experience: ---";
            }
            else
            {
                SkillGUI.SSLevel.GetComponent<Text>().text = "Level: " + skillRef.m_level.ToString("F0") + " / 100";
                SkillGUI.SSExp.GetComponent<Text>().text = "Experience: " + skillRef.m_accumulator.ToString("F2") + " / " + skillRef.GetNextLevelRequirement().ToString("F2");
            }

            if (!CFG.IsSkillActive(skill))
            {
                SkillGUI.LPEffectsTexts["experience"].GetComponent<Text>().text =
                    "Not currently active";
                SkillGUI.LPEffectsTexts["other"].GetComponent<Text>().text =
                    "Not currently active";
                ResetText();
                return;
            }

            //found in SkillGUIData, these functions open the panels, perk boxes, and
            //tips for the type of skill that was determined in the switch statement
            data.oPanels();
            data.oPerks();
            data.oTips();
            data.AddTipBreaks();

            if (data.isOutsideFactors())
            {
                SkillGUI.LPEffectsTexts["other"].GetComponent<Text>().text =
                    "Outside Factors:";
            }
            else
            {
                SkillGUI.LPEffectsTexts["other"].GetComponent<Text>().text =
                    "";
            }


            if (Perks.IsSkillAscended(skill))
            {
                SkillGUI.RPAscendedText.GetComponent<Text>().text = "Ascended";
            }
            else
            {
                SkillGUI.RPAscendedText.GetComponent<Text>().text = "";
            }

            Jotunn.Logger.LogMessage($"{skill} is ascendable: {AscensionManager.IsAscendable(skill)}");
            if (AscensionManager.IsAscendable(skill))
            {
                SkillGUI.RPAscendedBtn.SetActive(true);
            }
            else
            {
                SkillGUI.RPAscendedBtn.SetActive(false);
            }
            //Jotunn.Logger.LogMessage($"{Perks.IsSkillAscended(skill).ToString()}");
        }

        public static void ResetText()
        {
            SkillGUI.LPEffectsTexts["x1"].GetComponent<Text>().text = "";
            SkillGUI.LPEffectsTexts["x2"].GetComponent<Text>().text = "";
            SkillGUI.LPEffectsTexts["x3"].GetComponent<Text>().text = "";

            SkillGUI.LPEffectsTexts["f1"].GetComponent<Text>().text = "";
            SkillGUI.LPEffectsTexts["f2"].GetComponent<Text>().text = "";
            SkillGUI.LPEffectsTexts["f3"].GetComponent<Text>().text = "";
            SkillGUI.LPEffectsTexts["f4"].GetComponent<Text>().text = "";
            SkillGUI.LPEffectsTexts["f5"].GetComponent<Text>().text = "";
            SkillGUI.LPEffectsTexts["f6"].GetComponent<Text>().text = "";

            for (int i = 1; i <= SkillGUI.NumTipParagraphs; i++)
            {
                SkillGUI.LPTipsTexts[i].GetComponent<Text>().text = "";
            }
        }
    }


}
