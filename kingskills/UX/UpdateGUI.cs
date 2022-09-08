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
            SkillGUIData data = new SkillGUIData();
            Player player = Player.m_localPlayer;
            Skills.SkillType skill = Skills.SkillType.None;

            string skillName = SkillGUI.dd.options[SkillGUI.dd.value].text;
            StatsPatch.UpdateStats(player);
            ResetText();

            //Deciding which skillgui object to call for filling in the data
            switch (skillName)
            {
                case "Agriculture":
                    skill = SkillMan.Agriculture;
                    data = new AgricultureGUI();
                    break;
                case "Axes":
                    skill = Skills.SkillType.Axes;
                    data = new AxeGUI();
                    break;
                case "Blocking":
                    skill = Skills.SkillType.Blocking;
                    data = new BlockGUI();
                    break;
                case "Bows":
                    skill = Skills.SkillType.Bows;
                    data = new BowGUI();
                    break;
                case "Building":
                    skill = SkillMan.Building;
                    data = new BuildGUI();
                    break;
                case "Clubs":
                    skill = Skills.SkillType.Clubs;
                    data = new ClubGUI();
                    break;
                case "Cooking":
                    skill = SkillMan.Cooking;
                    data = new CookGUI();
                    break;
                case "Fists":
                    skill = Skills.SkillType.Unarmed;
                    data = new FistGUI();
                    break;
                case "Jump":
                    skill = Skills.SkillType.Jump;
                    data = new JumpGUI();
                    break;
                case "Knives":
                    skill = Skills.SkillType.Knives;
                    data = new KnifeGUI();
                    break;
                case "Mining":
                    skill = Skills.SkillType.Pickaxes;
                    data = new MineGUI();
                    break;
                case "Polearms":
                    skill = Skills.SkillType.Polearms;
                    data = new PolearmGUI();
                    break;
                case "Run":
                    skill = Skills.SkillType.Run;
                    data = new RunGUI();
                    break;
                case "Sailing":
                    skill = SkillMan.Sailing;
                    data = new SailGUI();
                    break;
                case "Spears":
                    skill = Skills.SkillType.Spears;
                    data = new SpearGUI();
                    break;
                case "Sneak":
                    skill = Skills.SkillType.Sneak;
                    data = new SneakGUI();
                    break;
                case "Swim":
                    skill = Skills.SkillType.Swim;
                    data = new SwimGUI();
                    break;
                case "Swords":
                    skill = Skills.SkillType.Swords;
                    data = new SwordGUI();
                    break;
                case "Woodcutting":
                    skill = Skills.SkillType.WoodCutting;
                    data = new WoodGUI();
                    break;
            }

            Skills.Skill skillRef = player.GetSkills().GetSkill(skill);

            SkillGUI.SSIcon.GetComponent<Image>().sprite = player.m_skills.GetSkillDef(skill).m_icon;
            SkillGUI.SSkillName.GetComponent<Text>().text = skillName;
            if (skillRef.m_level >= ConfigMan.MaxSkillLevel.Value)
            {
                SkillGUI.SSkillLevel.GetComponent<Text>().text = "Level: MAX!";
                SkillGUI.SSkillExp.GetComponent<Text>().text = "Experience: ---";
            }
            else
            {
                SkillGUI.SSkillLevel.GetComponent<Text>().text = "Level: " + skillRef.m_level.ToString("F0") + " / 100";
                SkillGUI.SSkillExp.GetComponent<Text>().text = "Experience: " + skillRef.m_accumulator.ToString("F2") + " / " + skillRef.GetNextLevelRequirement().ToString("F2");
            }

            data.oPanels();
            data.oPerks();
            data.oTips();


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
