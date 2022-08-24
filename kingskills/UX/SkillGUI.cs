using Jotunn;
using Jotunn.GUI;
using Jotunn.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace kingskills
{
    class SkillGUI
    {
        public static bool GUIOpen = false;
        public static GameObject SkillGUIWindow;

        public static GameObject SkillDropDown;
        public static GameObject SSkillName;
        public static GameObject SSkillLevel;
        public static GameObject SSkillExp;

        public static GameObject scroll;
        public static Dropdown dd;
        public static bool ddFlag;

        public static GameObject LeftPanelEffects;
        public static GameObject LeftPanelExperienceText;
        public static GameObject LeftPanelEffectsText;
        public static GameObject LeftPanelExperienceTitle;
        public static GameObject LeftPanelEffectsTitle;


        public static GameObject RightPanelPerks;


        private static void SkillGUIAwake()
        {

            if (GUIManager.Instance == null)
            {
                Jotunn.Logger.LogError("GUIManager instance is null");
                return;
            }

            if (!GUIManager.CustomGUIFront)
            {
                Jotunn.Logger.LogError("GUIManager CustomGUI is null");
                return;
            }

            SkillGUIWindow = GUIManager.Instance.CreateWoodpanel(
                    parent: GUIManager.CustomGUIFront.transform,
                    anchorMin: new Vector2(0.5f, 0.5f),
                    anchorMax: new Vector2(0.5f, 0.5f),
                    position: new Vector2(0f, 0),
                    width: 750,
                    height: 800,
                    draggable: true);
            SkillGUIWindow.SetActive(false);
            
            
            // Create Selected Skill text
            SSkillName =
            GUIManager.Instance.CreateText(
                text: "",
                parent: SkillGUIWindow.transform,
                anchorMin: new Vector2(0.5f, 1f),
                anchorMax: new Vector2(0.5f, 1f),
                position: new Vector2(0f, -75f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 50,
                color: GUIManager.Instance.ValheimOrange,
                outline: true,
                outlineColor: Color.black,
                width: 300f,
                height: 80f,
                addContentSizeFitter: false);
            SSkillName.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

            // Create Level text
            SSkillLevel =
            GUIManager.Instance.CreateText(
                text: "Level: 1 / 100",
                parent: SkillGUIWindow.transform,
                anchorMin: new Vector2(0.5f, 1f),
                anchorMax: new Vector2(0.5f, 1f),
                position: new Vector2(-200f, -125f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 25,
                color: GUIManager.Instance.ValheimOrange,
                outline: true,
                outlineColor: Color.black,
                width: 300f,
                height: 50f,
                addContentSizeFitter: false);
            SSkillLevel.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

            //Create experience text
            SSkillExp = 
            GUIManager.Instance.CreateText(
                text: "Experience:  / ",
                parent: SkillGUIWindow.transform,
                anchorMin: new Vector2(0.5f, 1f),
                anchorMax: new Vector2(0.5f, 1f),
                position: new Vector2(200f, -125f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 25,
                color: GUIManager.Instance.ValheimOrange,
                outline: true,
                outlineColor: Color.black,
                width: 300f,
                height: 50f,
                addContentSizeFitter: false);
            SSkillExp.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

            // Create the close button
            GameObject closeBtn = GUIManager.Instance.CreateButton(
                text: "Close",
                parent: SkillGUIWindow.transform,
                anchorMin: new Vector2(1f, 1f),
                anchorMax: new Vector2(1f, 1f),
                position: new Vector2(-90f, -75f),
                width: 150f,
                height: 50f);
            closeBtn.SetActive(true);

            // Add a listener to the button to close the panel again
            Button button = closeBtn.GetComponent<Button>();
            button.onClick.AddListener(ToggleSkillGUI);

            // Create the refresh button
            GameObject refreshBtn = GUIManager.Instance.CreateButton(
                text: "Refresh",
                parent: SkillGUIWindow.transform,
                anchorMin: new Vector2(1f, 0f),
                anchorMax: new Vector2(1f, 0f),
                position: new Vector2(-90f, 100f),
                width: 150f,
                height: 50f);
            refreshBtn.SetActive(true);

            // Add a listener to the button to close the panel again
            Button refButton = refreshBtn.GetComponent<Button>();
            refButton.onClick.AddListener(UpdateGUI);

            

            //Create the King's Skills brand text
            GUIManager.Instance.CreateText(
                text: "King's skills",
                parent: SkillGUIWindow.transform,
                anchorMin: new Vector2(1f, 0f),
                anchorMax: new Vector2(1f, 0f),
                position: new Vector2(-100f, 50f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 20,
                color: GUIManager.Instance.ValheimOrange,
                outline: true,
                outlineColor: Color.black,
                width: 120f,
                height: 30f,
                addContentSizeFitter: false);


            //Create the left panel
            LeftPanelEffects =
            GUIManager.Instance.CreateWoodpanel(
                    parent: SkillGUIWindow.transform,
                    anchorMin: new Vector2(0.5f, 0.5f),
                    anchorMax: new Vector2(0.5f, 0.5f),
                    position: new Vector2(-185f, 0),
                    width: 350,
                    height: 500,
                    draggable: false);
            LeftPanelEffects.AddComponent<Mask>();
            LeftPanelEffects.GetComponent<Mask>().enabled = true;


            scroll =
            GUIManager.Instance.CreateScrollView(
                parent: LeftPanelEffects.transform,
                showHorizontalScrollbar: false,
                showVerticalScrollbar: true,
                handleSize: 10f,
                handleDistanceToBorder: 0f,
                handleColors: GUIManager.Instance.ValheimScrollbarHandleColorBlock,
                slidingAreaBackgroundColor: Color.blue,
                width: 310f,
                height: 400f
                );
            scroll.transform.localScale = Vector3.one;
            scroll.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 500);
            scroll.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            scroll.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            scroll.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            scroll.AddComponent<ScrollRect>();
            ScrollRect scrollSet = scroll.GetComponent<ScrollRect>();
            scrollSet.horizontal = false;
            scrollSet.vertical = true;
            scrollSet.enabled = true;


            GameObject scrollVert = GameObject.Instantiate(scroll);
            GameObject.Destroy(scrollVert.GetComponent<ScrollRect>());
            GameObject.Destroy(scrollVert.GetComponent<Scrollbar>());
            GameObject.Destroy(scrollVert.GetComponent<Mask>());
            scrollVert.transform.SetParent(scroll.transform);

            scrollVert.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 600);
            scrollVert.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            scrollVert.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            scrollVert.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

            VerticalLayoutGroup vertSettings = scrollVert.AddComponent(typeof(VerticalLayoutGroup)) as VerticalLayoutGroup;
            vertSettings.spacing = 1f;
            vertSettings.childControlWidth = true;


            scrollSet.content = scrollVert.GetComponent<RectTransform>();
            scrollSet.verticalNormalizedPosition = 0;
            

            //Create the Experience Title in the left panel
            LeftPanelExperienceTitle =
            GUIManager.Instance.CreateText(
                text: "Experience",
                parent: scrollVert.transform,
                anchorMin: new Vector2(0f, 1f),
                anchorMax: new Vector2(0f, 1f),
                position: new Vector2(40f, 0f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 30,
                color: GUIManager.Instance.ValheimOrange,
                outline: true,
                outlineColor: Color.black,
                width: 300f,
                height: 70f,
                addContentSizeFitter: false);
            LeftPanelExperienceTitle.GetComponent<Text>().verticalOverflow = VerticalWrapMode.Overflow;
            LeftPanelExperienceTitle.GetComponent<Text>().alignment = TextAnchor.UpperCenter;
            //Create the variable Experience text in the left panel
            LeftPanelExperienceText =
            GUIManager.Instance.CreateText(
                text: "This text is the experience explainer",
                parent: scrollVert.transform,
                anchorMin: new Vector2(1f, 1f),
                anchorMax: new Vector2(1f, 1f),
                position: new Vector2(0f, 0f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 16,
                color: GUIManager.Instance.ValheimOrange,
                outline: true,
                outlineColor: Color.black,
                width: 300f,
                height: 30f,
                addContentSizeFitter: false);
            LeftPanelExperienceText.GetComponent<Text>().verticalOverflow = VerticalWrapMode.Overflow;
            LeftPanelExperienceText.GetComponent<Text>().alignment = TextAnchor.UpperLeft;
            //Create the Effects Title in the left panel
            LeftPanelEffectsTitle =
            GUIManager.Instance.CreateText(
                text: "Current Effects",
                parent: scrollVert.transform,
                anchorMin: new Vector2(1f, 0f),
                anchorMax: new Vector2(1f, 0f),
                position: new Vector2(40f, 0f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 30,
                color: GUIManager.Instance.ValheimOrange,
                outline: true,
                outlineColor: Color.black,
                width: 300f,
                height: 70f,
                addContentSizeFitter: false);
            LeftPanelEffectsTitle.GetComponent<Text>().verticalOverflow = VerticalWrapMode.Overflow;
            LeftPanelEffectsTitle.GetComponent<Text>().alignment = TextAnchor.UpperCenter;
            //Create the variable Effects text in the left panel
            LeftPanelEffectsText =
            GUIManager.Instance.CreateText(
                text: "This text is the effects",
                parent: scrollVert.transform,
                anchorMin: new Vector2(0f, 0f),
                anchorMax: new Vector2(0f, 0f),
                position: new Vector2(0f, 0f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 16,
                color: GUIManager.Instance.ValheimOrange,
                outline: true,
                outlineColor: Color.black,
                width: 300f,
                height: 30f,
                addContentSizeFitter: false);
            LeftPanelEffectsText.GetComponent<Text>().verticalOverflow = VerticalWrapMode.Overflow;
            LeftPanelEffectsText.GetComponent<Text>().alignment = TextAnchor.UpperLeft;

            //Create the right panel
            RightPanelPerks =
            GUIManager.Instance.CreateWoodpanel(
                    parent: SkillGUIWindow.transform,
                    anchorMin: new Vector2(0.5f, 0.5f),
                    anchorMax: new Vector2(0.5f, 0.5f),
                    position: new Vector2(185f, 0),
                    width: 350,
                    height: 500,
                    draggable: false);

            // Create a dropdown
            SkillDropDown =
                GUIManager.Instance.CreateDropDown(
                parent: SkillGUIWindow.transform,
                anchorMin: new Vector2(0f, 0f),
                anchorMax: new Vector2(0f, 0f),
                position: new Vector2(180f, 70f),
                fontSize: 15,
                width: 250f,
                height: 60f);
            dd = SkillDropDown.GetComponent<Dropdown>();
            dd.ClearOptions();
            dd.AddOptions(new List<string>{
                "Axes", "Blocking", "Bows", "Clubs", "Fists", "Jump", "Knives",
                "Polearms", "Run", "Spears", "Swim", "Swords", "Woodcutting"
            });
            dd.onValueChanged.AddListener(
                delegate {
                    SkillGUI.OnDropdownValueChange();
                });

            dd.captionText.fontSize = 15;
            dd.itemText.fontSize = 15;
            dd.captionText.horizontalOverflow = HorizontalWrapMode.Wrap;
            dd.itemText.horizontalOverflow = HorizontalWrapMode.Wrap;
            dd.captionText.resizeTextForBestFit = true;
            dd.itemText.resizeTextForBestFit = true;
            dd.itemText.resizeTextMaxSize = 15;

            ddFlag = false;

            //Jotunn.Logger.LogMessage($"There are {dd.options.Count} options. The first is {dd.options[0].text}, the last is {dd.options[dd.options.Count-1].text}"); 
            /* Big long debug thing. Everything is fine over here apparently
            string logDD = "";
            int i = 0;
            foreach (Dropdown.OptionData option in dd.options)
            {
                 logDD += "Option " + i + " is " + option.text + ", ";
                 logDD += "(Or " + dd.options[i].text +"), \n";
                 i++;
            }
            Jotunn.Logger.LogMessage(logDD);*/

            SkillGUIWindow.SetActive(false);
            
            UpdateGUI();
        }

        public static void ToggleSkillGUI()
        {
            if (!SkillGUIWindow)
            {
                SkillGUIAwake();
            }

            bool state = !SkillGUIWindow.activeSelf;

            SkillGUIWindow.SetActive(state);
                
            //GUIManager.BlockInput(state);
        }

        public static void OnDropdownValueChange()
        {
            //This awful mess is an awful fix to the dropdown always selecting the option 1 in front of what was clicked
            //This function gets called everytime dd.value gets changed
            if (!ddFlag)
            {
                Jotunn.Logger.LogMessage($"Dropdown changed, setting the flag for another change and decreasing the value");
                ddFlag = true;
                dd.value--;
            }
            else
            {
                Jotunn.Logger.LogMessage($"Now I'm actually taking the value seriously");
                UpdateGUI();
                ddFlag = false;
            }
        }

        public static void UpdateGUI()
        {
            scroll.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;

            //Jotunn.Logger.LogMessage($"Detected a dropdown value change.");
            Player player = Player.m_localPlayer;
            Skills.SkillType skill = Skills.SkillType.None;

            string skillName = dd.options[dd.value].text;

            switch (skillName)
            {
                case "Axes":
                    skill = Skills.SkillType.Axes;
                    OpenAxePanels();
                    break;
                case "Blocking":
                    skill = Skills.SkillType.Blocking;
                    OpenBlockPanels();
                    break;
                case "Bows":
                    skill = Skills.SkillType.Bows;
                    OpenBowPanels();
                    break;
                case "Clubs":
                    skill = Skills.SkillType.Clubs;
                    OpenClubPanels();
                    break;
                case "Fists":
                    skill = Skills.SkillType.Unarmed;
                    OpenFistPanels();
                    break;
                case "Jump":
                    skill = Skills.SkillType.Jump;
                    OpenJumpPanels();
                    break;
                case "Knives":
                    skill = Skills.SkillType.Knives;
                    OpenKnifePanels();
                    break;
                case "Polearms":
                    skill = Skills.SkillType.Polearms;
                    OpenPolearmPanels();
                    break;
                case "Run":
                    skill = Skills.SkillType.Run;
                    OpenRunPanels();
                    break;
                case "Spears":
                    skill = Skills.SkillType.Spears;
                    OpenSpearPanels();
                    break;
                case "Swim":
                    skill = Skills.SkillType.Swim;
                    OpenSwimPanels();
                    break;
                case "Swords":
                    skill = Skills.SkillType.Swords;
                    OpenSwordPanels();
                    break;
                case "Woodcutting":
                    skill = Skills.SkillType.WoodCutting;
                    OpenWoodcuttingPanels();
                    break;
            }
            Skills.Skill skillRef = player.GetSkills().GetSkill(skill);
            //Jotunn.Logger.LogMessage($"The skill the player seems to want is {skillRef.m_info}");

            SSkillName.GetComponent<Text>().text = skillName;
            SSkillLevel.GetComponent<Text>().text = "Level: " + skillRef.m_level.ToString("F0") + " / 100";
            SSkillExp.GetComponent<Text>().text = "Experience: " + skillRef.m_accumulator.ToString("F2") + " / " + skillRef.GetNextLevelRequirement().ToString("F2");
            scroll.GetComponent<ScrollRect>().Rebuild(UnityEngine.UI.CanvasUpdate.PreRender);
        }

        public static void OpenAxePanels()
        {
            //Jotunn.Logger.LogMessage($"I'm changing the axe values in now");
            LeftPanelExperienceText.GetComponent<Text>().text =
                "A percentage of all axe damage dealt is turned into experience. \n" +
                "The exp gain rate is much higher versus living targets. \n" +
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not. \n" +
                "Holding an axe gains you experience at a very slow rate. \n" +
                "Bonus experience for the axe is gained every time you break a log. ";
            LeftPanelEffectsText.GetComponent<Text>().text =
                "0" + "% extra damage with axes\n" +
                "0" + "% less stamina usage with axes\n" +
                "0" + " higher base stamina \n" +
                "0" + "% increased chop damage \n" +
                "0" + " extra carry capacity";
        }
        public static void OpenBlockPanels()
        {
            float skillFactor = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Blocking);
            float staminaRedux = Mathf.Lerp(BlockPatch.BlockStaminaReduxMin, BlockPatch.BlockStaminaReduxMax, skillFactor) * 100;
            float baseBlockPower = Mathf.Lerp(BlockPatch.FlatBlockPowerMin, BlockPatch.FlatBlockPowerMax, skillFactor);
            float blockPerArmor = Mathf.Lerp(BlockPatch.PerBlockPowerMin, BlockPatch.PerBlockPowerMax, skillFactor) * 100;
            float parryExpMod = BlockPatch.ParryExpMod * 100;

            LeftPanelExperienceText.GetComponent<Text>().text =
                "A percentage of all damage you block is turned into experience. \n" +
                "This number is unaffected by resistances. \n" +
                "You get " + parryExpMod.ToString("F0") + "% experience for parrying an attack.";
            LeftPanelEffectsText.GetComponent<Text>().text =
                staminaRedux.ToString("F0") + "% reduced stamina for blocks\n" +
                baseBlockPower.ToString("F0") + " extra flat block armor\n" +
                blockPerArmor.ToString("F0") + "% increased overall block armor\n" +
                "0" + "% increased stagger limit";
        }
        public static void OpenBowPanels()
        {
            LeftPanelExperienceText.GetComponent<Text>().text =
                "A percentage of all bow damage dealt is turned into experience. \n" +
                "The exp gain rate is much higher versus living targets. \n" +
                "Every time you shoot, a small amount of experience is gained, whether you hit anything or not. \n" +
                "Holding a bow gains you experience at a very slow rate. \n" +
                "Bonus experience is gained based on the length and difficulty of successful shots with a bow.";
            LeftPanelEffectsText.GetComponent<Text>().text =
                "0" + "% extra damage with bows\n" +
                "0" + "% less stamina usage with bows\n" +
                "0" + "% extra arrow velocity\n" +
                "0" + "% faster draw speed\n" +
                "0" + "% more loot drops from creatures";
        }
        public static void OpenClubPanels()
        {
            LeftPanelExperienceText.GetComponent<Text>().text =
                "A percentage of all club damage dealt is turned into experience. \n" +
                "The exp gain rate is much higher versus living targets. \n" +
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not. \n" +
                "Holding a club gains you experience at a very slow rate. \n" +
                "Bonus experience every time you stagger an enemy with damage with a club.";
            LeftPanelEffectsText.GetComponent<Text>().text =
                "0" + "% extra damage with clubs\n" +
                "0" + "% less stamina usage with clubs\n" +
                "0" + "% extra bonus to ALL blunt damage\n" +
                "0" + "% knockback bonus to ALL weapons\n" +
                "0" + "% extra stagger damage to ALL weapons";
        }
        public static void OpenFistPanels()
        {
            LeftPanelExperienceText.GetComponent<Text>().text =
                "A percentage of all unarmed damage dealt is turned into experience. \n" +
                "The exp gain rate is much higher versus living targets. \n" +
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not. \n" +
                "Holding no weapon gains you experience at a very slow rate. \n" +
                "Bonus experence for fists is gained every time you perform an unarmed block.";
            LeftPanelEffectsText.GetComponent<Text>().text =
                "0" + "% extra damage with fists\n" +
                "0" + "% less stamina usage with fists\n" +
                "0" + " extra flat damage\n" +
                "0" + " extra unarmed block armor\n" +
                "0" + "% extra move speed";
        }
        public static void OpenJumpPanels()
        {
            Player player = Player.m_localPlayer;
            MovePatch.JumpForceUpdate(player);
            float skillFactor = player.GetSkillFactor(Skills.SkillType.Jump);

            float bonusJumpForce = Mathf.Lerp(MovePatch.JumpForceMin, MovePatch.JumpForceMax, skillFactor) * 100;
            float staminaRedux = Mathf.Lerp(MovePatch.JumpStaminaReduxMin, MovePatch.JumpStaminaReduxMax, skillFactor) * 100;
            float bonusJumpForwardForce = Mathf.Lerp(MovePatch.JumpForwardForceMin, MovePatch.JumpForwardForceMax, skillFactor) * 100;
            float tired = Mathf.Lerp(MovePatch.BaseJumpTiredFactor, MovePatch.MaxJumpTiredFactor, skillFactor) * 100;

            float fallDamageThreshhold = Mathf.Lerp(JumpPatch.FallDamageThresholdMin, JumpPatch.FallDamageThresholdMax, skillFactor);
            float fallDamageRedux = Mathf.Lerp(JumpPatch.FallDamageReduxMin, JumpPatch.FallDamageReduxMax, skillFactor) * 100;

            LeftPanelExperienceText.GetComponent<Text>().text =
                "Every time you jump, you gain a small amount of experience. \n" +
                "Every time you land, you gain an amount of experience based on " +
                "how far you fell and how much damage you'd take.";
            LeftPanelEffectsText.GetComponent<Text>().text =
                bonusJumpForce.ToString("F0") + "% extra vertical jump force \n" +
                bonusJumpForwardForce.ToString("F0") + "% extra horizontal jump force \n" +
                staminaRedux.ToString("F0") + "% less stamina cost to jump \n" +
                tired.ToString("F0") + "% jump force modifier when tired \n" +
                fallDamageThreshhold.ToString("F1") + "m minimum fall damage height \n" +
                fallDamageRedux.ToString("F0") + "% less fall damage";
        }
        public static void OpenKnifePanels()
        {
            LeftPanelExperienceText.GetComponent<Text>().text =
                "A percentage of all knife damage dealt is turned into experience. \n" +
                "The exp gain rate is much higher versus living targets. \n" +
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not. \n" +
                "Holding a knife gains you experience at a very slow rate. \n" +
                "Bonus experience is gained every time you perform a sneak attack with a knife.";
            LeftPanelEffectsText.GetComponent<Text>().text =
                "0" + "% extra damage with knives\n" +
                "0" + "% less stamina usage with knives\n" +
                "0" + "% sneak attack bonus damage \n" +
                "0" + "% bonus move speed \n" +
                "0" + "% bonus to ALL pierce damage";
        }
        public static void OpenPolearmPanels()
        {
            LeftPanelExperienceText.GetComponent<Text>().text =
                "A percentage of all polearm damage dealt is turned into experience. \n" +
                "The exp gain rate is much higher versus living targets. \n" +
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not. \n" +
                "Holding a polearm gains you experience at a very slow rate. \n" +
                "Bonus experience for the polearm is gained everytime you hit multiple targets in a single swing.";
            LeftPanelEffectsText.GetComponent<Text>().text =
                "0" + "% extra damage with polearms \n" +
                "0" + "% less stamina usage with polearms\n" +
                "0" + " increased units of range with all weapons\n" +
                "0" + " increased armor\n" +
                "0" + "% increased block power with polearms";
        }
        public static void OpenRunPanels()
        {
            Player player = Player.m_localPlayer;
            MovePatch.RunSpeedUpdate(player);
            float skillFactor = player.GetSkillFactor(Skills.SkillType.Run);

            float runSpeedBonus = Mathf.Lerp(MovePatch.RunSpeedMin, MovePatch.RunSpeedMax, skillFactor) * 100;
            float equipmentMalusRedux = Mathf.Lerp(MovePatch.RunEquipmentReduxMin, MovePatch.RunEquipmentReduxMax, skillFactor) * 100;
            float encumberanceRedux = Mathf.Lerp(MovePatch.RunEncumberanceReduxMin, MovePatch.RunEncumberanceReduxMax, skillFactor) * 100;
            float staminaDrainRedux = Mathf.Lerp(MovePatch.RunStaminaReduxMin, MovePatch.RunStaminaReduxMax, skillFactor) * 100;
            float baseStaminaGain = skillFactor * 100 * MovePatch.RunStaminaPerLevel;


            float encumberanceFactor = (MovePatch.GetEncumberanceFactor(player, skillFactor) - 1) * 100;
            float equipmentFactor = (MovePatch.GetEquipmentFactor(player, skillFactor) - 1) * 100;

            float absWeightExp = (MovePatch.absoluteWeightBonus(player)-1)*100;
            float relWeightExp = (MovePatch.relativeWeightBonus(player)-1)*100;
            float runSpeedExp = (MovePatch.runSpeedExpBonus(player)-1)*100;

            LeftPanelExperienceText.GetComponent<Text>().text =
                "The faster you are moving, the more experience you get.\n" +
                "The closer you are to fully encumbered, the less movespeed you have.\n" +
                "You also gain experience based on how encumbered you are. \n" +
                runSpeedExp.ToString("F0") + "% experience bonus from current run speed \n" +
                absWeightExp.ToString("F0") + "% experience bonus from absolute weight carried \n" +
                relWeightExp.ToString("F0") + "% experience bonus from fullness of inventory";
            LeftPanelEffectsText.GetComponent<Text>().text =
                runSpeedBonus.ToString("F0") + "% extra run speed\n" +
                equipmentMalusRedux.ToString("F0") + "% reduction to move speed penalties from equipment\n" +
                encumberanceRedux.ToString("F0") + "% reduced speed malus from encumberance\n" +
                staminaDrainRedux.ToString("F0") + "% less stamina cost to run\n" +
                baseStaminaGain.ToString("F0") + " extra base stamina\n" +
                "\nCurrent effects from outside factors: \n" +
                encumberanceFactor.ToString("F1") + "% reduced speed from encumberance\n" +
                equipmentFactor.ToString("F0") + "% change to speed from equipment";
        }
        public static void OpenSpearPanels()
        {
            LeftPanelExperienceText.GetComponent<Text>().text =
                "A percentage of all spear damage dealt is turned into experience. \n" +
                "The exp gain rate is much higher versus living targets. \n" +
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not. \n" +
                "Holding a spear gains you experience at a very slow rate. \n" +
                "Bonus experience anytime you hit an enemy with a thrown weapon.";
            LeftPanelEffectsText.GetComponent<Text>().text =
                "0" + "% extra damage with spears \n" +
                "0" + "% less stamina usage with spears\n" +
                "0" + "% increased velocity with all thrown weapons\n" +
                "0" + "% increased damage with all thrown weapons\n" +
                "0" + " higher flat block armor";
        }
        public static void OpenSwimPanels()
        {
            Player player = Player.m_localPlayer;
            MovePatch.SwimSpeedUpdate(player);
            float skillFactor = player.GetSkillFactor(Skills.SkillType.Run);

            float swimSpeed = Mathf.Lerp(MovePatch.SwimSpeedMin, MovePatch.SwimSpeedMax, skillFactor) * 100;
            float swimAccel = Mathf.Lerp(MovePatch.SwimAccelMin, MovePatch.SwimAccelMax, skillFactor) * 100;
            float swimTurn = Mathf.Lerp(MovePatch.SwimTurnMin, MovePatch.SwimTurnMax, skillFactor) * 100;
            float swimStaminaCost = Mathf.Lerp(MovePatch.SwimStaminaPSMin, MovePatch.SwimStaminaPSMax, skillFactor);

            float absWeightExp = (MovePatch.absoluteWeightBonus(player) - 1) * 100;
            float relWeightExp = (MovePatch.relativeWeightBonus(player) - 1) * 100;
            float swimSpeedExp = (MovePatch.swimSpeedExpBonus(player) - 1) * 100;


            LeftPanelExperienceText.GetComponent<Text>().text =
                "The faster you are moving, the more experience you get.\n" +
                "You also gain experience based on how encumbered you are. \n" +
                swimSpeedExp.ToString("F0") + "% experience bonus from current swimming speed \n" +
                absWeightExp.ToString("F0") + "% experience bonus from absolute weight carried \n" +
                relWeightExp.ToString("F0") + "% experience bonus from fullness of inventory";
            LeftPanelEffectsText.GetComponent<Text>().text =
                swimSpeed.ToString("F0") + "% extra swim speed\n" +
                swimAccel.ToString("F0") + "% extra acceleration in water\n" +
                swimTurn.ToString("F0") + "% extra turn speed in water\n" +
                swimStaminaCost.ToString("F0") + " stamina per second while swimming";
        }
        public static void OpenSwordPanels()
        {
            LeftPanelExperienceText.GetComponent<Text>().text =
                "A percentage of all sword damage dealt is turned into experience. \n" +
                "The exp gain rate is much higher versus living targets. \n" +
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not. \n" +
                "Holding a sword gains you experience at a very slow rate. \n" +
                "Bonus experience is gained every time you deal damage to a staggered enemy with a sword.";
            LeftPanelEffectsText.GetComponent<Text>().text =
                "0" + "% extra damage with swords \n" +
                "0" + "% less stamina usage with swords\n" +
                "0" + "% higher parry bonus with ALL weapons \n" +
                "0" + "% increased slash damage with ALL weapons \n" +
                "0" + "% less stamina cost to dodge";
        }
        public static void OpenWoodcuttingPanels()
        {
            LeftPanelExperienceText.GetComponent<Text>().text =
                "A percentage of all chop damage dealt is turned into experience. \n" +
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not. \n" +
                "Bonus experience for the axe is gained based on the tier of the tool used.";
            LeftPanelEffectsText.GetComponent<Text>().text =
                "0" + "% extra damage to trees\n" +
                "0" + "% increased wood drop rates from trees\n" +
                "0" + "% rebate on axe swings that hit a tree\n" +
                "0" + " something else!";
        }
    }
}
