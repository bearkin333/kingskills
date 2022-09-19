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
        //These are just temps for making changing settings in the inits much easier
        public static RectTransform rect = null;
        public static Image image = null;
        public static Text text = null;
        public static GameObject obj = null;

        public const int NumTipParagraphs = 6;

        public static bool tipsActive = false;

        public static void Init()
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

            //Refers to the code in the regions below
            InitSkillWindow();
            InitEffectsPanel();
            InitTipsPanel();
            InitPerksPanel();


            SkillGUIWindow.SetActive(false);
            SkillGUIUpdate.SetTipsTab(false);

            SkillGUIUpdate.GUICheck();
        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region var
        public static GameObject SkillGUIWindow;

        public static GameObject SSIcon;
        public static GameObject SkillDropDown;
        public static GameObject SSName;
        public static GameObject SSLevel;
        public static GameObject SSExp;

        public static Dropdown dd;
        public static GameObject ddUpBtn;
        public static GameObject ddDownBtn;
        public static GameObject CloseBtn;
        public static GameObject PinBtn;

        public static GameObject LPTipsTabBtn;
        public static GameObject LPEffectsTabBtn;
        #endregion var
        public static void InitSkillWindow()
        {

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
            SSName =
            GUIManager.Instance.CreateText(
                text: "",
                parent: SkillGUIWindow.transform,
                anchorMin: new Vector2(0.5f, 1f),
                anchorMax: new Vector2(0.5f, 1f),
                position: new Vector2(0f, -60f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 50,
                color: CFG.ColorTitle,
                outline: true,
                outlineColor: Color.black,
                width: 400f,
                height: 80f,
                addContentSizeFitter: false);
            SSName.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

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
            SSLevel =
            GUIManager.Instance.CreateText(
                text: "Level: 1 / 100",
                parent: SkillGUIWindow.transform,
                anchorMin: new Vector2(0.5f, 1f),
                anchorMax: new Vector2(0.5f, 1f),
                position: new Vector2(-200f, -125f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 25,
                color: CFG.ColorExperienceYellow,
                outline: true,
                outlineColor: Color.black,
                width: 300f,
                height: 50f,
                addContentSizeFitter: false);
            SSLevel.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

            //Create experience text
            SSExp =
            GUIManager.Instance.CreateText(
                text: "Experience:  / ",
                parent: SkillGUIWindow.transform,
                anchorMin: new Vector2(0.5f, 1f),
                anchorMax: new Vector2(0.5f, 1f),
                position: new Vector2(200f, -125f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 25,
                color: CFG.ColorExperienceYellow,
                outline: true,
                outlineColor: Color.black,
                width: 400f,
                height: 50f,
                addContentSizeFitter: false);
            SSExp.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

            // Create the close button
            CloseBtn = GUIManager.Instance.CreateButton(
                text: "Close",
                parent: SkillGUIWindow.transform,
                anchorMin: new Vector2(1f, 1f),
                anchorMax: new Vector2(1f, 1f),
                position: new Vector2(-90f, -35f),
                width: 120f,
                height: 45f);

            // Add a listener to the button to close the panel again
            Button button = CloseBtn.GetComponent<Button>();
            button.onClick.AddListener(SkillGUIUpdate.CloseSkillGUI);

            // Create the close button
            PinBtn = GUIManager.Instance.CreateButton(
                text: "Pin",
                parent: SkillGUIWindow.transform,
                anchorMin: new Vector2(1f, 1f),
                anchorMax: new Vector2(1f, 1f),
                position: new Vector2(-90f, -85f),
                width: 120f,
                height: 45f);

            // Add a listener to the button to close the panel again
            button = PinBtn.GetComponent<Button>();
            button.onClick.AddListener(SkillGUIUpdate.PinGUI);


            LPEffectsTabBtn = GUIManager.Instance.CreateButton(
                text: "Effects",
                parent: SkillGUIWindow.transform,
                anchorMin: new Vector2(0f, 0f),
                anchorMax: new Vector2(0f, 0f),
                position: new Vector2(110f, 90f),
                width: 100f,
                height: 40f);

            // Add a listener to the button to close the panel again
            button = LPEffectsTabBtn.GetComponent<Button>();
            button.onClick.AddListener(SkillGUIUpdate.OnEffectsTab);


            LPTipsTabBtn = GUIManager.Instance.CreateButton(
                text: "Tips",
                parent: SkillGUIWindow.transform,
                anchorMin: new Vector2(0f, 0f),
                anchorMax: new Vector2(0f, 0f),
                position: new Vector2(240f, 90f),
                width: 100f,
                height: 40f);

            // Add a listener to the button to close the panel again
            button = LPTipsTabBtn.GetComponent<Button>();
            button.onClick.AddListener(SkillGUIUpdate.OnTipsTab);

            // Create a dropdown
            SkillDropDown =
                GUIManager.Instance.CreateDropDown(
                parent: SkillGUIWindow.transform,
                anchorMin: new Vector2(0f, 0f),
                anchorMax: new Vector2(0f, 0f),
                position: new Vector2(120f, 50f),
                fontSize: 18,
                width: 200f,
                height: 28f);
            dd = SkillDropDown.GetComponent<Dropdown>();
            dd.AddOptions(new List<string>{
                "Agriculture", "Axes", "Blocking", "Bows", "Building", "Clubs", "Cooking",
                "Fists", "Jump", "Knives", "Mining", "Polearms", "Run", "Sailing", "Spears",
                "Sneak", "Swim", "Swords", "Woodcutting"
            });
            dd.onValueChanged.AddListener(
                delegate {
                    SkillGUIUpdate.OnDropdownValueChange();
                });
            rect = SkillDropDown.GetComponent<RectTransform>();

            dd.captionText.horizontalOverflow = HorizontalWrapMode.Wrap;
            dd.itemText.horizontalOverflow = HorizontalWrapMode.Wrap;

            // Create the dropdown down button
            ddDownBtn = GUIManager.Instance.CreateButton(
                text: "Down",
                parent: SkillDropDown.transform,
                anchorMin: new Vector2(1f, 0.5f),
                anchorMax: new Vector2(1f, 0.5f),
                position: new Vector2(150f, 0f),
                width: 80f,
                height: 30f);

            // Add a listener to the button to close the panel again
            button = ddDownBtn.GetComponent<Button>();
            button.onClick.AddListener(SkillGUIUpdate.DDDown);


            // Create the dropdown down button
            ddUpBtn = GUIManager.Instance.CreateButton(
                text: "Up",
                parent: SkillDropDown.transform,
                anchorMin: new Vector2(1f, 0.5f),
                anchorMax: new Vector2(1f, 0.5f),
                position: new Vector2(50f, 0f),
                width: 80f,
                height: 30f);

            // Add a listener to the button to close the panel again
            button = ddUpBtn.GetComponent<Button>();
            button.onClick.AddListener(SkillGUIUpdate.DDUp);



            //Create the King's Skills brand text
            GUIManager.Instance.CreateText(
                text: "King's skills",
                parent: SkillGUIWindow.transform,
                anchorMin: new Vector2(1f, 0f),
                anchorMax: new Vector2(1f, 0f),
                position: new Vector2(-100f, 50f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 20,
                color: CFG.ColorKingSkills,
                outline: true,
                outlineColor: Color.black,
                width: 120f,
                height: 30f,
                addContentSizeFitter: false);

        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region var
        public static GameObject LeftPanelEffectsTab;
        public static Dictionary<string, GameObject> LPEffectsTexts;

        public static GameObject LPEffectsScroll;
        #endregion var

        public static void InitEffectsPanel()
        {
            //Create the left panel
            LeftPanelEffectsTab =
            GUIManager.Instance.CreateWoodpanel(
                    parent: SkillGUIWindow.transform,
                    anchorMin: new Vector2(0.5f, 0.5f),
                    anchorMax: new Vector2(0.5f, 0.5f),
                    position: new Vector2(-185f, -15f),
                    width: 350,
                    height: 530,
                    draggable: false);
            LeftPanelEffectsTab.AddComponent<Mask>();
            LeftPanelEffectsTab.GetComponent<Mask>().enabled = true;


            LPEffectsScroll =
            GUIManager.Instance.CreateScrollView(
                parent: LeftPanelEffectsTab.transform,
                showHorizontalScrollbar: false,
                showVerticalScrollbar: true,
                handleSize: 10f,
                handleDistanceToBorder: 0f,
                handleColors: GUIManager.Instance.ValheimScrollbarHandleColorBlock,
                slidingAreaBackgroundColor: Color.blue,
                width: 310f,
                height: 400f
                );
            LPEffectsScroll.transform.localScale = Vector3.one;
            LPEffectsScroll.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 500);
            LPEffectsScroll.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            LPEffectsScroll.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            LPEffectsScroll.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            LPEffectsScroll.AddComponent<ScrollRect>();
            ScrollRect scrollSet = LPEffectsScroll.GetComponent<ScrollRect>();
            scrollSet.horizontal = false;
            scrollSet.vertical = true;
            scrollSet.enabled = true;
            scrollSet.scrollSensitivity = 100f;


            GameObject scrollVert = GameObject.Instantiate(LPEffectsScroll);
            GameObject.Destroy(scrollVert.GetComponent<ScrollRect>());
            GameObject.Destroy(scrollVert.GetComponent<Scrollbar>());
            GameObject.Destroy(scrollVert.GetComponent<Mask>());
            scrollVert.transform.SetParent(LPEffectsScroll.transform);

            GameObject scrollBar = GameObject.Instantiate(scrollVert);
            scrollBar.transform.SetParent(LPEffectsScroll.transform);
            Scrollbar scrollB = scrollBar.AddComponent<Scrollbar>();
            rect = scrollBar.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(40, 500);
            rect.anchorMin = new Vector2(1f, 0.5f);
            rect.anchorMax = new Vector2(1f, 0.5f);
            rect.anchoredPosition = new Vector2(-20f, 0);

            scrollSet.horizontalScrollbar = scrollB;

            rect = scrollVert.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(300, 425);
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(0, 0);

            VerticalLayoutGroup vertSettings = scrollVert.AddComponent(typeof(VerticalLayoutGroup)) as VerticalLayoutGroup;
            vertSettings.spacing = 1f;
            vertSettings.childControlWidth = true;


            scrollSet.content = rect;
            scrollSet.verticalNormalizedPosition = 1;

            LPEffectsTexts = new Dictionary<string, GameObject>();


            //Create the Experience Title in the left panel
            LPEffectsTexts.Add("effects", GUIManager.Instance.CreateText(
                text: "Skill Effects",
                parent: scrollVert.transform,
                anchorMin: new Vector2(0f, 1f),
                anchorMax: new Vector2(0f, 1f),
                position: new Vector2(50f, 0f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 30,
                color: CFG.ColorTitle,
                outline: true,
                outlineColor: Color.black,
                width: 300f,
                height: 70f,
                addContentSizeFitter: false));
            text = LPEffectsTexts["effects"].GetComponent<Text>();
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.alignment = TextAnchor.UpperCenter;

            //create all the effects lines
            LPEffectsTexts.Add("f1", GUIManager.Instance.CreateText(
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
            text = LPEffectsTexts["f1"].GetComponent<Text>();
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.alignment = TextAnchor.UpperLeft;

            LPEffectsTexts.Add("f2", UnityEngine.Object.Instantiate(LPEffectsTexts["f1"]));
            LPEffectsTexts["f2"].GetComponent<RectTransform>().SetParent(scrollVert.transform);

            LPEffectsTexts.Add("f3", UnityEngine.Object.Instantiate(LPEffectsTexts["f1"]));
            LPEffectsTexts["f3"].GetComponent<RectTransform>().SetParent(scrollVert.transform);

            LPEffectsTexts.Add("f4", UnityEngine.Object.Instantiate(LPEffectsTexts["f1"]));
            LPEffectsTexts["f4"].GetComponent<RectTransform>().SetParent(scrollVert.transform);

            LPEffectsTexts.Add("f5", UnityEngine.Object.Instantiate(LPEffectsTexts["f1"]));
            LPEffectsTexts["f5"].GetComponent<RectTransform>().SetParent(scrollVert.transform);

            LPEffectsTexts.Add("f6", UnityEngine.Object.Instantiate(LPEffectsTexts["f1"]));
            LPEffectsTexts["f6"].GetComponent<RectTransform>().SetParent(scrollVert.transform);


            //Create the Effects Title in the left panel
            LPEffectsTexts.Add("other", GUIManager.Instance.CreateText(
                text: "Outside Factors",
                parent: scrollVert.transform,
                anchorMin: new Vector2(0f, 1f),
                anchorMax: new Vector2(0f, 1f),
                position: new Vector2(50f, 0f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 30,
                color: CFG.ColorTitle,
                outline: true,
                outlineColor: Color.black,
                width: 300f,
                height: 70f,
                addContentSizeFitter: false));
            text = LPEffectsTexts["other"].GetComponent<Text>();
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.alignment = TextAnchor.UpperCenter;


            //Create the variable Experience text in the left panel
            LPEffectsTexts.Add("x1", GUIManager.Instance.CreateText(
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
            text = LPEffectsTexts["x1"].GetComponent<Text>();
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.alignment = TextAnchor.UpperLeft;

            LPEffectsTexts.Add("x2", UnityEngine.Object.Instantiate(LPEffectsTexts["x1"]));
            LPEffectsTexts["x2"].GetComponent<RectTransform>().SetParent(scrollVert.transform);

            LPEffectsTexts.Add("x3", UnityEngine.Object.Instantiate(LPEffectsTexts["x1"]));
            LPEffectsTexts["x3"].GetComponent<RectTransform>().SetParent(scrollVert.transform);

        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region var
        public static GameObject LeftPanelTipsTab;
        public static GameObject LPTipsScroll;

        public static Dictionary<int, GameObject> LPTipsTexts;
        #endregion var
        public static void InitTipsPanel()
        {
            LeftPanelTipsTab =
            GUIManager.Instance.CreateWoodpanel(
                    parent: SkillGUIWindow.transform,
                    anchorMin: new Vector2(0.5f, 0.5f),
                    anchorMax: new Vector2(0.5f, 0.5f),
                    position: new Vector2(-185f, -15f),
                    width: 350,
                    height: 530,
                    draggable: false);
            LeftPanelTipsTab.AddComponent<Mask>();
            LeftPanelTipsTab.GetComponent<Mask>().enabled = true;
            LeftPanelTipsTab.SetActive(false);


            LPTipsScroll =
            GUIManager.Instance.CreateScrollView(
                parent: LeftPanelTipsTab.transform,
                showHorizontalScrollbar: false,
                showVerticalScrollbar: true,
                handleSize: 10f,
                handleDistanceToBorder: 0f,
                handleColors: GUIManager.Instance.ValheimScrollbarHandleColorBlock,
                slidingAreaBackgroundColor: Color.blue,
                width: 310f,
                height: 400f
                );
            LPTipsScroll.transform.localScale = Vector3.one;
            rect = LPTipsScroll.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(350, 500);
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(0, 0);
            LPTipsScroll.AddComponent<ScrollRect>();
            ScrollRect scrollSet = LPTipsScroll.GetComponent<ScrollRect>();
            scrollSet.horizontal = false;
            scrollSet.vertical = true;
            scrollSet.enabled = true;

            scrollSet.scrollSensitivity = 100f;

            GameObject scrollVert = GameObject.Instantiate(LPTipsScroll);
            GameObject.Destroy(scrollVert.GetComponent<ScrollRect>());
            GameObject.Destroy(scrollVert.GetComponent<Scrollbar>());
            GameObject.Destroy(scrollVert.GetComponent<Mask>());
            scrollVert.transform.SetParent(LPTipsScroll.transform);

            GameObject scrollBar = GameObject.Instantiate(scrollVert);
            scrollBar.transform.SetParent(LPTipsScroll.transform);
            Scrollbar scrollB = scrollBar.AddComponent<Scrollbar>();
            rect = scrollBar.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(100, 500);

            scrollSet.verticalScrollbar = scrollB;

            rect = scrollVert.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(300, 425);
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(0, 0);

            VerticalLayoutGroup vertSettings = scrollVert.AddComponent(typeof(VerticalLayoutGroup)) as VerticalLayoutGroup;
            vertSettings.spacing = 1f;
            vertSettings.childControlWidth = true;

            ContentSizeFitter sizeFitter = scrollVert.AddComponent(typeof(ContentSizeFitter)) as ContentSizeFitter;
            sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;


            scrollSet.content = rect;
            scrollSet.verticalNormalizedPosition = 1;

            LPTipsTexts = new Dictionary<int, GameObject>();

            //Create the Experience Title in the left panel
            LPTipsTexts.Add(0, GUIManager.Instance.CreateText(
                text: "      Tips\n",
                parent: scrollVert.transform,
                anchorMin: new Vector2(0f, 1f),
                anchorMax: new Vector2(0f, 1f),
                position: new Vector2(50f, 0f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 30,
                color: CFG.ColorTitle,
                outline: true,
                outlineColor: Color.black,
                width: 250f,
                height: 70f,
                addContentSizeFitter: false));
            text = LPTipsTexts[0].GetComponent<Text>();
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.alignment = TextAnchor.MiddleCenter;

            //Create the variable Experience text in the left panel
            LPTipsTexts.Add(1, GUIManager.Instance.CreateText(
                text: "x",
                parent: scrollVert.transform,
                anchorMin: new Vector2(0f, 1f),
                anchorMax: new Vector2(0f, 1f),
                position: new Vector2(0f, 0f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 16,
                color: GUIManager.Instance.ValheimOrange,
                outline: true,
                outlineColor: Color.black,
                width: 290f,
                height: 30f,
                addContentSizeFitter: false));
            text = LPTipsTexts[0].GetComponent<Text>();
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.alignment = TextAnchor.UpperLeft;

            for (int i = 2; i < NumTipParagraphs+1; i++)
            {
                LPTipsTexts.Add(i, UnityEngine.Object.Instantiate(LPTipsTexts[1]));
                LPTipsTexts[i].GetComponent<RectTransform>().SetParent(scrollVert.transform);
            }


        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region var
        public static GameObject RightPanel;

        public static GameObject RPAscendedText;
        public static GameObject RPAscendedBtn;

        public static GameObject RPFirstPerkLVLThreshold;
        public static GameObject RPSecondPerkLVLThreshold;
        public static GameObject RPThirdPerkLVLThreshold;
        public static Dictionary<string, GameObject> RPPerkBoxes;
        #endregion var

        public static void InitPerksPanel()
        {


            //Create the right panel
            RightPanel =
            GUIManager.Instance.CreateWoodpanel(
                    parent: SkillGUIWindow.transform,
                    anchorMin: new Vector2(0.5f, 0.5f),
                    anchorMax: new Vector2(0.5f, 0.5f),
                    position: new Vector2(185f, -15),
                    width: 350,
                    height: 530,
                    draggable: false);

            // Create Perk title
            obj = GUIManager.Instance.CreateText(
                text: "Perks",
                parent: RightPanel.transform,
                anchorMin: new Vector2(0.5f, 1f),
                anchorMax: new Vector2(0.5f, 1f),
                position: new Vector2(0f, -35f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 32,
                color: CFG.ColorTitle,
                outline: true,
                outlineColor: Color.black,
                width: 300f,
                height: 80f,
                addContentSizeFitter: false);
            obj.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

            // Create ascended text
            RPAscendedText =
            GUIManager.Instance.CreateText(
                text: "",
                parent: RightPanel.transform,
                anchorMin: new Vector2(0.5f, 0f),
                anchorMax: new Vector2(0.5f, 0f),
                position: new Vector2(0f, 50f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 26,
                color: CFG.ColorAscendedGreen,
                outline: true,
                outlineColor: Color.black,
                width: 300f,
                height: 80f,
                addContentSizeFitter: false);
            RPAscendedText.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

            RPAscendedBtn = GUIManager.Instance.CreateButton(
                text: "Ascend?",
                parent: RightPanel.transform,
                anchorMin: new Vector2(0.5f, 0f),
                anchorMax: new Vector2(0.5f, 0f),
                position: new Vector2(0f, 50f),
                width: 300f,
                height: 80f);

            // Add a listener to the button to close the panel again
            Button button = RPAscendedBtn.GetComponent<Button>();
            button.onClick.AddListener(AscensionManager.OnAscendButton);
            RPAscendedBtn.SetActive(false);

            RPAscendedText.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

            // Create perk 1 level text
            RPFirstPerkLVLThreshold =
            GUIManager.Instance.CreateText(
                text: "",
                parent: RightPanel.transform,
                anchorMin: new Vector2(0.5f, 1f),
                anchorMax: new Vector2(0.5f, 1f),
                position: new Vector2(0f, -110f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 20,
                color: CFG.ColorWhite,
                outline: true,
                outlineColor: Color.black,
                width: 200f,
                height: 80f,
                addContentSizeFitter: false);
            text = RPFirstPerkLVLThreshold.GetComponent<Text>();
            text.alignment = TextAnchor.MiddleCenter;
            text.text = "Level " + Mathf.Floor(CFG.PerkOneLVLThreshold.Value
                * CFG.MaxSkillLevel.Value);

            // Create perk 2 level text
            RPSecondPerkLVLThreshold =
            GUIManager.Instance.CreateText(
                text: "",
                parent: RightPanel.transform,
                anchorMin: new Vector2(0.5f, 1f),
                anchorMax: new Vector2(0.5f, 1f),
                position: new Vector2(0f, -200f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 20,
                color: CFG.ColorWhite,
                outline: true,
                outlineColor: Color.black,
                width: 200f,
                height: 80f,
                addContentSizeFitter: false);
            text = RPSecondPerkLVLThreshold.GetComponent<Text>();
            text.alignment = TextAnchor.MiddleCenter;
            text.text = "Level " + Mathf.Floor(CFG.PerkTwoLVLThreshold.Value
                * CFG.MaxSkillLevel.Value);

            // Create perk 3 level text
            RPThirdPerkLVLThreshold =
            GUIManager.Instance.CreateText(
                text: "",
                parent: RightPanel.transform,
                anchorMin: new Vector2(0.5f, 1f),
                anchorMax: new Vector2(0.5f, 1f),
                position: new Vector2(0f, -290f),   
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 20,
                color: CFG.ColorWhite,
                outline: true,
                outlineColor: Color.black,
                width: 200f,
                height: 80f,
                addContentSizeFitter: false);
            text = RPThirdPerkLVLThreshold.GetComponent<Text>();
            text.alignment = TextAnchor.MiddleCenter;
            text.text = "Level " + Mathf.Floor(CFG.PerkThreeLVLThreshold.Value
                * CFG.MaxSkillLevel.Value);



            RPPerkBoxes = new Dictionary<string, GameObject>();

            RPPerkBoxes.Add("1a", new GameObject());
            image = RPPerkBoxes["1a"].AddComponent<Image>();
            image.sprite = Assets.AssetLoader.perkBoxSprites["perkbox"];
            rect = RPPerkBoxes["1a"].GetComponent<RectTransform>();
            rect.SetParent(RightPanel.transform);
            rect.anchorMin = new Vector2(0.5f, 1f);
            rect.anchorMax = new Vector2(0.5f, 1f);
            rect.anchoredPosition = new Vector2(-100f, -110f);
            rect.sizeDelta = new Vector2(80f, 80f);
            rect.gameObject.AddComponent<IsPerkBox>();

            RPPerkBoxes.Add("1aTint", new GameObject());
            image = RPPerkBoxes["1aTint"].AddComponent<Image>();
            image.sprite = null;
            image.enabled = false;
            rect = RPPerkBoxes["1aTint"].GetComponent<RectTransform>();
            rect.SetParent(RPPerkBoxes["1a"].transform);
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(0f, 0f);
            rect.sizeDelta = new Vector2(80f, 80f);

            RPPerkBoxes.Add("1aPerk", new GameObject());
            image = RPPerkBoxes["1aPerk"].AddComponent<Image>();
            image.sprite = null;
            image.enabled = false;
            rect = RPPerkBoxes["1aPerk"].GetComponent<RectTransform>();
            rect.SetParent(RPPerkBoxes["1a"].transform);
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(0f, 0f);
            rect.sizeDelta = new Vector2(60f, 60f);



            RPPerkBoxes.Add("1b", new GameObject());
            image = RPPerkBoxes["1b"].AddComponent<Image>();
            image.sprite = Assets.AssetLoader.perkBoxSprites["perkbox"];
            rect = RPPerkBoxes["1b"].GetComponent<RectTransform>();
            rect.SetParent(RightPanel.transform);
            rect.anchorMin = new Vector2(0.5f, 1f);
            rect.anchorMax = new Vector2(0.5f, 1f);
            rect.anchoredPosition = new Vector2(100f, -110f);
            rect.sizeDelta = new Vector2(80f, 80f);
            rect.gameObject.AddComponent<IsPerkBox>();

            RPPerkBoxes.Add("1bTint", new GameObject());
            image = RPPerkBoxes["1bTint"].AddComponent<Image>();
            image.sprite = null;
            image.enabled = false;
            rect = RPPerkBoxes["1bTint"].GetComponent<RectTransform>();
            rect.SetParent(RPPerkBoxes["1b"].transform);
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(0f, 0f);
            rect.sizeDelta = new Vector2(80f, 80f);

            RPPerkBoxes.Add("1bPerk", new GameObject());
            image = RPPerkBoxes["1bPerk"].AddComponent<Image>();
            image.sprite = null;
            image.enabled = false;
            rect = RPPerkBoxes["1bPerk"].GetComponent<RectTransform>();
            rect.SetParent(RPPerkBoxes["1b"].transform);
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(0f, 0f);
            rect.sizeDelta = new Vector2(60f, 60f);


            RPPerkBoxes.Add("2a", new GameObject());
            image = RPPerkBoxes["2a"].AddComponent<Image>();
            image.sprite = Assets.AssetLoader.perkBoxSprites["perkbox"];
            rect = RPPerkBoxes["2a"].GetComponent<RectTransform>();
            rect.SetParent(RightPanel.transform);
            rect.anchorMin = new Vector2(0.5f, 1f);
            rect.anchorMax = new Vector2(0.5f, 1f);
            rect.anchoredPosition = new Vector2(-100f, -200f);
            rect.sizeDelta = new Vector2(80f, 80f);
            rect.gameObject.AddComponent<IsPerkBox>();

            RPPerkBoxes.Add("2aTint", new GameObject());
            image = RPPerkBoxes["2aTint"].AddComponent<Image>();
            image.sprite = null;
            image.enabled = false;
            rect = RPPerkBoxes["2aTint"].GetComponent<RectTransform>();
            rect.SetParent(RPPerkBoxes["2a"].transform);
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(0f, 0f);
            rect.sizeDelta = new Vector2(80f, 80f);

            RPPerkBoxes.Add("2aPerk", new GameObject());
            image = RPPerkBoxes["2aPerk"].AddComponent<Image>();
            image.sprite = null;
            image.enabled = false;
            rect = RPPerkBoxes["2aPerk"].GetComponent<RectTransform>();
            rect.SetParent(RPPerkBoxes["2a"].transform);
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(0f, 0f);
            rect.sizeDelta = new Vector2(60f, 60f);


            RPPerkBoxes.Add("2b", new GameObject());
            image = RPPerkBoxes["2b"].AddComponent<Image>();
            image.sprite = Assets.AssetLoader.perkBoxSprites["perkbox"];
            rect = RPPerkBoxes["2b"].GetComponent<RectTransform>();
            rect.SetParent(RightPanel.transform);
            rect.anchorMin = new Vector2(0.5f, 1f);
            rect.anchorMax = new Vector2(0.5f, 1f);
            rect.anchoredPosition = new Vector2(100f, -200f);
            rect.sizeDelta = new Vector2(80f, 80f);
            rect.gameObject.AddComponent<IsPerkBox>();

            RPPerkBoxes.Add("2bTint", new GameObject());
            image = RPPerkBoxes["2bTint"].AddComponent<Image>();
            image.sprite = null;
            image.enabled = false;
            rect = RPPerkBoxes["2bTint"].GetComponent<RectTransform>();
            rect.SetParent(RPPerkBoxes["2b"].transform);
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(0f, 0f);
            rect.sizeDelta = new Vector2(80f, 80f);

            RPPerkBoxes.Add("2bPerk", new GameObject());
            image = RPPerkBoxes["2bPerk"].AddComponent<Image>();
            image.sprite = null;
            image.enabled = false;
            rect = RPPerkBoxes["2bPerk"].GetComponent<RectTransform>();
            rect.SetParent(RPPerkBoxes["2b"].transform);
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(0f, 0f);
            rect.sizeDelta = new Vector2(60f, 60f);


            RPPerkBoxes.Add("3a", new GameObject());
            image = RPPerkBoxes["3a"].AddComponent<Image>();
            image.sprite = Assets.AssetLoader.perkBoxSprites["perkbox"];
            rect = RPPerkBoxes["3a"].GetComponent<RectTransform>();
            rect.SetParent(RightPanel.transform);
            rect.anchorMin = new Vector2(0.5f, 1f);
            rect.anchorMax = new Vector2(0.5f, 1f);
            rect.anchoredPosition = new Vector2(-100f, -290f);
            rect.sizeDelta = new Vector2(80f, 80f);
            rect.gameObject.AddComponent<IsPerkBox>();

            RPPerkBoxes.Add("3aTint", new GameObject());
            image = RPPerkBoxes["3aTint"].AddComponent<Image>();
            image.sprite = null;
            image.enabled = false;
            rect = RPPerkBoxes["3aTint"].GetComponent<RectTransform>();
            rect.SetParent(RPPerkBoxes["3a"].transform);
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(0f, 0f);
            rect.sizeDelta = new Vector2(80f, 80f);

            RPPerkBoxes.Add("3aPerk", new GameObject());
            image = RPPerkBoxes["3aPerk"].AddComponent<Image>();
            image.sprite = null;
            image.enabled = false;
            rect = RPPerkBoxes["3aPerk"].GetComponent<RectTransform>();
            rect.SetParent(RPPerkBoxes["3a"].transform);
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(0f, 0f);
            rect.sizeDelta = new Vector2(60f, 60f);


            RPPerkBoxes.Add("3b", new GameObject());
            image = RPPerkBoxes["3b"].AddComponent<Image>();
            image.sprite = Assets.AssetLoader.perkBoxSprites["perkbox"];
            rect = RPPerkBoxes["3b"].GetComponent<RectTransform>();
            rect.SetParent(RightPanel.transform);
            rect.anchorMin = new Vector2(0.5f, 1f);
            rect.anchorMax = new Vector2(0.5f, 1f);
            rect.anchoredPosition = new Vector2(100f, -290f);
            rect.sizeDelta = new Vector2(80f, 80f);
            rect.gameObject.AddComponent<IsPerkBox>();

            RPPerkBoxes.Add("3bTint", new GameObject());
            image = RPPerkBoxes["3bTint"].AddComponent<Image>();
            image.sprite = null;
            image.enabled = false;
            rect = RPPerkBoxes["3bTint"].GetComponent<RectTransform>();
            rect.SetParent(RPPerkBoxes["3b"].transform);
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(0f, 0f);
            rect.sizeDelta = new Vector2(80f, 80f);

            RPPerkBoxes.Add("3bPerk", new GameObject());
            image = RPPerkBoxes["3bPerk"].AddComponent<Image>();
            image.sprite = null;
            image.enabled = false;
            rect = RPPerkBoxes["3bPerk"].GetComponent<RectTransform>();
            rect.SetParent(RPPerkBoxes["3b"].transform);
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

        }
    }
}
