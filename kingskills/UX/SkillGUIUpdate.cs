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
using kingskills.Perks;

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
            //Jotunn.Logger.LogWarning("DID UPDATE");
            try
            {
                if (!__instance.m_nview) return;
                if (!__instance.m_nview.IsOwner()) return;
                if (!SkillGUI.SkillGUIWindow) return;
                if (!SkillGUI.SkillGUIWindow.activeSelf) return;
            }
            catch
            {
                //Jotunn.Logger.LogWarning("Didn't check for GUI Update");
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
            if (!GoodInput()) return;
            if (SkillGUI.SkillGUIWindow.activeSelf) CloseSkillGUI();
            else OpenSkillGUI();
        }

        public static bool GoodInput()
        {
            if (Player.m_localPlayer == null)
            {
                return false;
            }
            if (!SkillGUI.SkillGUIWindow)
            {
                SkillGUI.Init();
                CheckDropdownButtons();
                return true;
            }
            if (!closable) return false;

            return true;
        }

        public static void OpenSkillGUI()
        {
            if (!GoodInput()) return;

            SkillGUI.SkillGUIWindow.SetActive(true);
            GUIManager.BlockInput(true);
            GUICheck();
        }

        public static void CloseSkillGUI()
        {
            if (!GoodInput()) return;

            SkillGUI.SkillGUIWindow.SetActive(false);
            GUIManager.BlockInput(false);
        }



        public static void OnDropdownValueChange()
        {
            CheckDropdownButtons();

            GUICheck();
            //Reset the scroll position
            SkillGUI.LPEffectsScroll.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
            SkillGUI.LPTipsScroll.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
        }

        public static void CheckDropdownButtons()
        {
            //Check if the up and down buttons should be turned off or on
            if (SkillGUI.dd.value == 0) SkillGUI.ddUpBtn.GetComponent<Button>().interactable = false;
            else SkillGUI.ddUpBtn.GetComponent<Button>().interactable = true;
            if (SkillGUI.dd.value == SkillGUI.dd.options.Count - 1) SkillGUI.ddDownBtn.GetComponent<Button>().interactable = false;
            else SkillGUI.ddDownBtn.GetComponent<Button>().interactable = true;
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
        public static void PinGUI() => GUIManager.BlockInput(false);
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
            SkillGUI.PinBtn.GetComponent<Button>().interactable = interactable;
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

            List<string> perkEffects = OpenPerks.GetActivePerkEffectTips();
            if (perkEffects.Count > 0)
            {
                UpdatePerkEffects(perkEffects);
                SetPerkTexts(true);
            }
            else
                SetPerkTexts(false);

            if (data.isOutsideFactors())
                SetOtherTexts(true);
            else
                SetOtherTexts(false);


            if (PerkMan.IsSkillAscended(skill))
                SkillGUI.RPAscendedText.GetComponent<Text>().enabled = true;
            else
                SkillGUI.RPAscendedText.GetComponent<Text>().enabled = false;

            //Jotunn.Logger.LogMessage($"{skill} is ascendable: {AscensionManager.IsAscendable(skill)}");
            
            if (AscensionMan.IsAscendable(skill))
            {
                SkillGUI.RPAscendedBtn.SetActive(true);
                Player.m_localPlayer.ShowTutorial("kingskills_ascend");
            }
            else
                SkillGUI.RPAscendedBtn.SetActive(false);

            //Jotunn.Logger.LogMessage($"{Perks.IsSkillAscended(skill).ToString()}");
        }

        public static void ResetText()
        {
            for (int i = 1; i <= SkillGUI.LPNumEffectsTexts; i++)
            {
                SkillGUI.LPEffectsTexts["f"+i].GetComponent<Text>().text = "";
            }

            for (int i = 1; i <= SkillGUI.LPNumPerkTexts; i++)
            {
                SkillGUI.LPEffectsTexts["p" + i].GetComponent<Text>().text = "";
            }

            for (int i = 1; i <= SkillGUI.LPNumOtherTexts; i++)
            {
                SkillGUI.LPEffectsTexts["x" + i].GetComponent<Text>().text = "";
            }


            for (int i = 1; i <= SkillGUI.NumTipParagraphs; i++)
            {
                SkillGUI.LPTipsTexts[i].GetComponent<Text>().text = "";
            }
        }

        public static void SetPerkTexts(bool enabled)
        {
            if (SkillGUI.LPEffectsTexts["P"].GetComponent<Text>().enabled == enabled) return;
            SkillGUI.LPEffectsTexts["P"].GetComponent<Text>().enabled = enabled;
            for (int i = 1; i <= SkillGUI.LPNumPerkTexts; i++)
            {
                SkillGUI.LPEffectsTexts["p" + i].GetComponent<Text>().enabled = enabled;
            }
        }
        public static void SetOtherTexts(bool enabled)
        {
            if (SkillGUI.LPEffectsTexts["X"].GetComponent<Text>().enabled == enabled) return;
            SkillGUI.LPEffectsTexts["X"].GetComponent<Text>().enabled = enabled;
            for (int i = 1; i <= SkillGUI.LPNumOtherTexts; i++)
            {
                SkillGUI.LPEffectsTexts["x" + i].GetComponent<Text>().enabled = enabled;
            }
        }

        public static void UpdatePerkEffects(List<string> effectsTexts)
        {
            if (effectsTexts.Count > SkillGUI.LPNumPerkTexts)
            {
                Jotunn.Logger.LogWarning("There were more perks active than perk texts");
                return;
            }
            for (int i = 0; i < effectsTexts.Count; i++)
            {
                int j = i + 1;
                SkillGUI.LPEffectsTexts["p" + j].GetComponent<Text>().text = "+" + effectsTexts[i];
            }
        }
    }


}
