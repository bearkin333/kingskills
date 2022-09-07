using HarmonyLib;
using Jotunn;
using Jotunn.GUI;
using Jotunn.Managers;
using Jotunn.Utils;
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
    class SkillGUI
    {
        public static GameObject SkillGUIWindow;

        public static GameObject SSIcon;
        public static GameObject SkillDropDown;
        public static GameObject SSkillName;
        public static GameObject SSkillLevel;
        public static GameObject SSkillExp;

        public static GameObject scroll;
        public static Dropdown dd;

        public static GameObject LeftPanel;
        public static Dictionary<string, GameObject> LeftPanelTexts;


        public static GameObject RightPanel;

        public static GameObject RightPanelAscendedText;

        public static GameObject RightPanelPerkOneLevel;
        public static GameObject RightPanelPerkTwoLevel;
        public static Dictionary<string, GameObject> RightPanelPerkBoxes;


        public static void InitSkillWindow()
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
            //temps for addressing settings
            RectTransform rect;
            Image image;
            Text text;
            GameObject obj;

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
                position: new Vector2(0f, -60f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 50,
                color: ConfigMan.ColorTitle,
                outline: true,
                outlineColor: Color.black,
                width: 400f,
                height: 80f,
                addContentSizeFitter: false);
            SSkillName.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

            //Create the skill box
            obj = new GameObject();
            image = obj.AddComponent<Image>();
            image.sprite = Assets.AssetLoader.perkBoxSprites["skillbox"];
            rect = obj.GetComponent<RectTransform>();
            rect.SetParent(SkillGUIWindow.transform);
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.anchoredPosition = new Vector2(150f, -60f);
            rect.sizeDelta = new Vector2(90f, 90f);

            //Create skill icon
            SSIcon = new GameObject();
            image = SSIcon.AddComponent<Image>();
            image.sprite = GUIManager.Instance.GetSprite("ArmorBronzeChest");
            rect = SSIcon.GetComponent<RectTransform>();
            rect.SetParent(SkillGUIWindow.transform);
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.anchoredPosition = new Vector2(150f, -60f);
            //rect.localPosition = new Vector2(-40f, -30f);
            rect.sizeDelta = new Vector2(80f, 80f);


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
                color: ConfigMan.ColorExperienceYellow,
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
                color: ConfigMan.ColorExperienceYellow,
                outline: true,
                outlineColor: Color.black,
                width: 400f,
                height: 50f,
                addContentSizeFitter: false);
            SSkillExp.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

            // Create the close button
            GameObject closeBtn = GUIManager.Instance.CreateButton(
                text: "Close",
                parent: SkillGUIWindow.transform,
                anchorMin: new Vector2(1f, 1f),
                anchorMax: new Vector2(1f, 1f),
                position: new Vector2(-90f, -35f),
                width: 120f,
                height: 45f);

            // Add a listener to the button to close the panel again
            Button button = closeBtn.GetComponent<Button>();
            button.onClick.AddListener(UpdateGUI.ToggleSkillGUI);

            // Create the close button
            GameObject stickBtn = GUIManager.Instance.CreateButton(
                text: "Stick",
                parent: SkillGUIWindow.transform,
                anchorMin: new Vector2(1f, 1f),
                anchorMax: new Vector2(1f, 1f),
                position: new Vector2(-90f, -85f),
                width: 120f,
                height: 45f);

            // Add a listener to the button to close the panel again
            button = stickBtn.GetComponent<Button>();
            button.onClick.AddListener(UpdateGUI.StickGUI);

            /*
            // Create the refresh button
            GameObject refreshBtn = GUIManager.Instance.CreateButton(
                text: "Refresh",
                parent: SkillGUIWindow.transform,
                anchorMin: new Vector2(1f, 0f),
                anchorMax: new Vector2(1f, 0f),
                position: new Vector2(-90f, 100f),
                width: 120f,
                height: 45f);
            refreshBtn.SetActive(true);

            // Add a listener to the button to close the panel again
            button = refreshBtn.GetComponent<Button>();
            button.onClick.AddListener(GUICheck);*/

            // Create a dropdown
            SkillDropDown =
                GUIManager.Instance.CreateDropDown(
                parent: SkillGUIWindow.transform,
                anchorMin: new Vector2(0f, 0f),
                anchorMax: new Vector2(0f, 0f),
                position: new Vector2(120f, 70f),
                fontSize: 20,
                width: 200f,
                height: 30f);
            dd = SkillDropDown.GetComponent<Dropdown>();
            dd.AddOptions(new List<string>{
                "Agriculture", "Axes", "Blocking", "Bows", "Building", "Clubs", "Cooking", 
                "Fists", "Jump", "Knives", "Mining", "Polearms", "Run", "Sailing", "Spears", 
                "Sneak", "Swim", "Swords", "Woodcutting"
            });
            dd.onValueChanged.AddListener(
                delegate {
                    UpdateGUI.OnDropdownValueChange();
                });
            rect = SkillDropDown.GetComponent<RectTransform>();

            dd.captionText.horizontalOverflow = HorizontalWrapMode.Wrap;
            dd.itemText.horizontalOverflow = HorizontalWrapMode.Wrap;



            //Create the King's Skills brand text
            GUIManager.Instance.CreateText(
                text: "King's skills",
                parent: SkillGUIWindow.transform,
                anchorMin: new Vector2(1f, 0f),
                anchorMax: new Vector2(1f, 0f),
                position: new Vector2(-100f, 50f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 20,
                color: ConfigMan.ColorKingSkills,
                outline: true,
                outlineColor: Color.black,
                width: 120f,
                height: 30f,
                addContentSizeFitter: false);


            //Create the left panel
            LeftPanel =
            GUIManager.Instance.CreateWoodpanel(
                    parent: SkillGUIWindow.transform,
                    anchorMin: new Vector2(0.5f, 0.5f),
                    anchorMax: new Vector2(0.5f, 0.5f),
                    position: new Vector2(-185f, 0),
                    width: 350,
                    height: 500,
                    draggable: false);
            LeftPanel.AddComponent<Mask>();
            LeftPanel.GetComponent<Mask>().enabled = true;


            scroll =
            GUIManager.Instance.CreateScrollView(
                parent: LeftPanel.transform,
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

            rect = scrollVert.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(300, 600);
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(0, 0);

            VerticalLayoutGroup vertSettings = scrollVert.AddComponent(typeof(VerticalLayoutGroup)) as VerticalLayoutGroup;
            vertSettings.spacing = 1f;
            vertSettings.childControlWidth = true;


            scrollSet.content = rect;
            scrollSet.verticalNormalizedPosition = 1;

            LeftPanelTexts = new Dictionary<string, GameObject>();
            

            //Create the Experience Title in the left panel
            LeftPanelTexts.Add("experience", GUIManager.Instance.CreateText(
                text: "Experience",
                parent: scrollVert.transform,
                anchorMin: new Vector2(0f, 1f),
                anchorMax: new Vector2(0f, 1f),
                position: new Vector2(40f, 0f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 30,
                color: ConfigMan.ColorTitle,
                outline: true,
                outlineColor: Color.black,
                width: 300f,
                height: 70f,
                addContentSizeFitter: false));
            text = LeftPanelTexts["experience"].GetComponent<Text>();
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.alignment = TextAnchor.UpperCenter;

            //Create the variable Experience text in the left panel
            LeftPanelTexts.Add("x1", GUIManager.Instance.CreateText(
                text: "x1",
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
                addContentSizeFitter: false));
            text = LeftPanelTexts["x1"].GetComponent<Text>();
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.alignment = TextAnchor.UpperLeft;

            LeftPanelTexts.Add("x2", UnityEngine.Object.Instantiate(LeftPanelTexts["x1"]));
            LeftPanelTexts["x2"].GetComponent<RectTransform>().SetParent(scrollVert.transform);

            LeftPanelTexts.Add("x3", UnityEngine.Object.Instantiate(LeftPanelTexts["x1"]));
            LeftPanelTexts["x3"].GetComponent<RectTransform>().SetParent(scrollVert.transform);

            //LeftPanelTexts.Add("x4", UnityEngine.Object.Instantiate(LeftPanelTexts["x1"]));
            //LeftPanelTexts["x4"].GetComponent<RectTransform>().SetParent(scrollVert.transform);

            LeftPanelTexts.Add("bonus", UnityEngine.Object.Instantiate(LeftPanelTexts["x1"]));
            LeftPanelTexts["bonus"].GetComponent<RectTransform>().SetParent(scrollVert.transform);
            LeftPanelTexts["bonus"].GetComponent<Text>().color = ConfigMan.ColorBonusBlue;

            //Create the Effects Title in the left panel
            LeftPanelTexts.Add("effects", GUIManager.Instance.CreateText(
                text: "Effects",
                parent: scrollVert.transform,
                anchorMin: new Vector2(0f, 1f),
                anchorMax: new Vector2(0f, 1f),
                position: new Vector2(40f, 0f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 30,
                color: ConfigMan.ColorTitle,
                outline: true,
                outlineColor: Color.black,
                width: 300f,
                height: 70f,
                addContentSizeFitter: false));
            text = LeftPanelTexts["effects"].GetComponent<Text>();
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.alignment = TextAnchor.UpperCenter;

            //create all the effects lines
            LeftPanelTexts.Add("f1", GUIManager.Instance.CreateText(
                text: "f1",
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
                addContentSizeFitter: false));
            text = LeftPanelTexts["f1"].GetComponent<Text>();
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.alignment = TextAnchor.UpperLeft;

            LeftPanelTexts.Add("f2", UnityEngine.Object.Instantiate(LeftPanelTexts["f1"]));
            LeftPanelTexts["f2"].GetComponent<RectTransform>().SetParent(scrollVert.transform);

            LeftPanelTexts.Add("f3", UnityEngine.Object.Instantiate(LeftPanelTexts["f1"]));
            LeftPanelTexts["f3"].GetComponent<RectTransform>().SetParent(scrollVert.transform);

            LeftPanelTexts.Add("f4", UnityEngine.Object.Instantiate(LeftPanelTexts["f1"]));
            LeftPanelTexts["f4"].GetComponent<RectTransform>().SetParent(scrollVert.transform);

            LeftPanelTexts.Add("f5", UnityEngine.Object.Instantiate(LeftPanelTexts["f1"]));
            LeftPanelTexts["f5"].GetComponent<RectTransform>().SetParent(scrollVert.transform);

            LeftPanelTexts.Add("f6", UnityEngine.Object.Instantiate(LeftPanelTexts["f1"]));
            LeftPanelTexts["f6"].GetComponent<RectTransform>().SetParent(scrollVert.transform);


            //Create the right panel
            RightPanel =
            GUIManager.Instance.CreateWoodpanel(
                    parent: SkillGUIWindow.transform,
                    anchorMin: new Vector2(0.5f, 0.5f),
                    anchorMax: new Vector2(0.5f, 0.5f),
                    position: new Vector2(185f, 0),
                    width: 350,
                    height: 500,
                    draggable: false);

            // Create Perk title
            obj = GUIManager.Instance.CreateText(
                text: "Perks",
                parent: RightPanel.transform,
                anchorMin: new Vector2(0.5f, 1f),
                anchorMax: new Vector2(0.5f, 1f),
                position: new Vector2(0f, -50f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 32,
                color: ConfigMan.ColorTitle,
                outline: true,
                outlineColor: Color.black,
                width: 300f,
                height: 80f,
                addContentSizeFitter: false);
            obj.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

            // Create ascended text
            RightPanelAscendedText =
            GUIManager.Instance.CreateText(
                text: "",
                parent: RightPanel.transform,
                anchorMin: new Vector2(0.5f, 0f),
                anchorMax: new Vector2(0.5f, 0f),
                position: new Vector2(0f, 50f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 26,
                color: ConfigMan.ColorAscendedGreen,
                outline: true,
                outlineColor: Color.black,
                width: 300f,
                height: 80f,
                addContentSizeFitter: false);
            RightPanelAscendedText.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

            // Create perk 1 level text
            RightPanelPerkOneLevel =
            GUIManager.Instance.CreateText(
                text: "",
                parent: RightPanel.transform,
                anchorMin: new Vector2(0.5f, 1f),
                anchorMax: new Vector2(0.5f, 1f),
                position: new Vector2(0f, -140f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 20,
                color: ConfigMan.ColorWhite,
                outline: true,
                outlineColor: Color.black,
                width: 200f,
                height: 80f,
                addContentSizeFitter: false);
            text = RightPanelPerkOneLevel.GetComponent<Text>();
            text.alignment = TextAnchor.MiddleCenter;
            text.text = "Level " + (int)(ConfigMan.PerkOneLVLThreshold.Value 
                * ConfigMan.MaxSkillLevel.Value);

            // Create perk 2 level text
            RightPanelPerkTwoLevel =
            GUIManager.Instance.CreateText(
                text: "",
                parent: RightPanel.transform,
                anchorMin: new Vector2(0.5f, 1f),
                anchorMax: new Vector2(0.5f, 1f),
                position: new Vector2(0f, -310f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 20,
                color: ConfigMan.ColorWhite,
                outline: true,
                outlineColor: Color.black,
                width: 200f,
                height: 80f,
                addContentSizeFitter: false);
            text = RightPanelPerkTwoLevel.GetComponent<Text>();
            text.alignment = TextAnchor.MiddleCenter;
            text.text = "Level " + (int)(ConfigMan.PerkTwoLVLThreshold.Value
                * ConfigMan.MaxSkillLevel.Value);



            RightPanelPerkBoxes = new Dictionary<string, GameObject>();

            RightPanelPerkBoxes.Add("1a", new GameObject());
            image = RightPanelPerkBoxes["1a"].AddComponent<Image>();
            image.sprite = Assets.AssetLoader.perkBoxSprites["perkbox"];
            rect = RightPanelPerkBoxes["1a"].GetComponent<RectTransform>();
            rect.SetParent(RightPanel.transform);
            rect.anchorMin = new Vector2(0.5f, 1f);
            rect.anchorMax = new Vector2(0.5f, 1f);
            rect.anchoredPosition = new Vector2(-100f, -140f);
            rect.sizeDelta = new Vector2(80f, 80f);
            rect.gameObject.AddComponent<IsPerkBox>();

            RightPanelPerkBoxes.Add("1aTint", new GameObject());
            image = RightPanelPerkBoxes["1aTint"].AddComponent<Image>();
            image.sprite = null;
            image.enabled = false;
            rect = RightPanelPerkBoxes["1aTint"].GetComponent<RectTransform>();
            rect.SetParent(RightPanelPerkBoxes["1a"].transform);
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(0f, 0f);
            rect.sizeDelta = new Vector2(80f, 80f);

            RightPanelPerkBoxes.Add("1aPerk", new GameObject()); 
            image = RightPanelPerkBoxes["1aPerk"].AddComponent<Image>();
            image.sprite = null;
            image.enabled = false;
            rect = RightPanelPerkBoxes["1aPerk"].GetComponent<RectTransform>();
            rect.SetParent(RightPanelPerkBoxes["1a"].transform);
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(0f, 0f);
            rect.sizeDelta = new Vector2(60f, 60f);



            RightPanelPerkBoxes.Add("1b", new GameObject());
            image = RightPanelPerkBoxes["1b"].AddComponent<Image>();
            image.sprite = Assets.AssetLoader.perkBoxSprites["perkbox"];
            rect = RightPanelPerkBoxes["1b"].GetComponent<RectTransform>();
            rect.SetParent(RightPanel.transform);
            rect.anchorMin = new Vector2(0.5f, 1f);
            rect.anchorMax = new Vector2(0.5f, 1f);
            rect.anchoredPosition = new Vector2(100f, -140f);
            rect.sizeDelta = new Vector2(80f, 80f);
            rect.gameObject.AddComponent<IsPerkBox>();

            RightPanelPerkBoxes.Add("1bTint", new GameObject());
            image = RightPanelPerkBoxes["1bTint"].AddComponent<Image>();
            image.sprite = null;
            image.enabled = false;
            rect = RightPanelPerkBoxes["1bTint"].GetComponent<RectTransform>();
            rect.SetParent(RightPanelPerkBoxes["1b"].transform);
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(0f, 0f);
            rect.sizeDelta = new Vector2(80f, 80f);

            RightPanelPerkBoxes.Add("1bPerk", new GameObject());
            image = RightPanelPerkBoxes["1bPerk"].AddComponent<Image>();
            image.sprite = null;
            image.enabled = false;
            rect = RightPanelPerkBoxes["1bPerk"].GetComponent<RectTransform>();
            rect.SetParent(RightPanelPerkBoxes["1b"].transform);
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(0f, 0f);
            rect.sizeDelta = new Vector2(60f, 60f);


            RightPanelPerkBoxes.Add("2a", new GameObject());
            image = RightPanelPerkBoxes["2a"].AddComponent<Image>();
            image.sprite = Assets.AssetLoader.perkBoxSprites["perkbox"];
            rect = RightPanelPerkBoxes["2a"].GetComponent<RectTransform>();
            rect.SetParent(RightPanel.transform);
            rect.anchorMin = new Vector2(0.5f, 1f);
            rect.anchorMax = new Vector2(0.5f, 1f);
            rect.anchoredPosition = new Vector2(-100f, -310f);
            rect.sizeDelta = new Vector2(80f, 80f);
            rect.gameObject.AddComponent<IsPerkBox>();

            RightPanelPerkBoxes.Add("2aTint", new GameObject());
            image = RightPanelPerkBoxes["2aTint"].AddComponent<Image>();
            image.sprite = null;
            image.enabled = false;
            rect = RightPanelPerkBoxes["2aTint"].GetComponent<RectTransform>();
            rect.SetParent(RightPanelPerkBoxes["2a"].transform);
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(0f, 0f);
            rect.sizeDelta = new Vector2(80f, 80f);

            RightPanelPerkBoxes.Add("2aPerk", new GameObject());
            image = RightPanelPerkBoxes["2aPerk"].AddComponent<Image>();
            image.sprite = null;
            image.enabled = false;
            rect = RightPanelPerkBoxes["2aPerk"].GetComponent<RectTransform>();
            rect.SetParent(RightPanelPerkBoxes["2a"].transform);
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(0f, 0f);
            rect.sizeDelta = new Vector2(60f, 60f);


            RightPanelPerkBoxes.Add("2b", new GameObject());
            image = RightPanelPerkBoxes["2b"].AddComponent<Image>();
            image.sprite = Assets.AssetLoader.perkBoxSprites["perkbox"];
            rect = RightPanelPerkBoxes["2b"].GetComponent<RectTransform>();
            rect.SetParent(RightPanel.transform);
            rect.anchorMin = new Vector2(0.5f, 1f);
            rect.anchorMax = new Vector2(0.5f, 1f);
            rect.anchoredPosition = new Vector2(100f, -310f);
            rect.sizeDelta = new Vector2(80f, 80f);
            rect.gameObject.AddComponent<IsPerkBox>();

            RightPanelPerkBoxes.Add("2bTint", new GameObject());
            image = RightPanelPerkBoxes["2bTint"].AddComponent<Image>();
            image.sprite = null;
            image.enabled = false;
            rect = RightPanelPerkBoxes["2bTint"].GetComponent<RectTransform>();
            rect.SetParent(RightPanelPerkBoxes["2b"].transform);
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(0f, 0f);
            rect.sizeDelta = new Vector2(80f, 80f);

            RightPanelPerkBoxes.Add("2bPerk", new GameObject());
            image = RightPanelPerkBoxes["2bPerk"].AddComponent<Image>();
            image.sprite = null;
            image.enabled = false;
            rect = RightPanelPerkBoxes["2bPerk"].GetComponent<RectTransform>();
            rect.SetParent(RightPanelPerkBoxes["2b"].transform);
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(0f, 0f);
            rect.sizeDelta = new Vector2(60f, 60f);



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

            UpdateGUI.GUICheck();
        }
    }
}
