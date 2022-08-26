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
                "Axes", "Blocking", "Bows", "Clubs", "Fists", "Jump", "Knives", "Mining",
                "Polearms", "Run", "Spears", "Sneak", "Swim", "Swords", "Woodcutting"
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
                case "Mining":
                    skill = Skills.SkillType.Pickaxes;
                    OpenMiningPanels();
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
                case "Sneak":
                    skill = Skills.SkillType.Sneak;
                    OpenSneakPanels();
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
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Axes);

            float axeDamage = ToPercent(ConfigManager.GetAxeDamageMod(skill));
            float axeStaminaRedux = ToPercent(ConfigManager.GetAxeStaminaRedux(skill));
            float axeStaminaGain = ConfigManager.GetAxeStamina(skill);
            float axeChopBonus = ToPercent(ConfigManager.GetAxeChopDamageMod(skill));
            float axeCarryCapacity = ConfigManager.GetAxeCarryCapacity(skill);

            //Jotunn.Logger.LogMessage($"I'm changing the axe values in now");
            LeftPanelExperienceText.GetComponent<Text>().text =
                "A percentage of all axe damage dealt is turned into experience. \n" +
                "The exp gain rate is much higher versus living targets. \n" +
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not. \n" +
                "Holding an axe gains you experience at a very slow rate. \n" +
                "Bonus experience for the axe is gained every time you break a log. ";
            LeftPanelEffectsText.GetComponent<Text>().text =
                axeDamage.ToString("F1") + "% extra damage with axes\n" +
                axeStaminaRedux.ToString("F1") + "% stamina usage with axes\n" +
                axeStaminaGain.ToString("F0") + " higher base stamina \n" +
                axeChopBonus.ToString("F1") + "% extra chop damage \n" +
                axeCarryCapacity.ToString("F0") + " extra carry capacity";
        }
        public static void OpenBlockPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Blocking);

            float staminaRedux = ToPercent(ConfigManager.GetBlockStaminaRedux(skill));
            float baseBlockPower = ConfigManager.GetBlockPowerFlat(skill);
            float blockPerArmor = ToPercent(ConfigManager.GetBlockPowerMod(skill));
            float blockHealth = ConfigManager.GetBlockHealth(skill);
            float parryExpMod = ToPercent(ConfigManager.GetBlockParryExpMod());

            LeftPanelExperienceText.GetComponent<Text>().text =
                "A percentage of all damage you block is turned into experience. \n" +
                "This number is unaffected by resistances. \n" +
                "You get " + parryExpMod.ToString("F0") + "% experience for parrying an attack.";
            LeftPanelEffectsText.GetComponent<Text>().text =
                staminaRedux.ToString("F1") + "% stamina cost for blocks\n" +
                baseBlockPower.ToString("F0") + " extra flat block armor\n" +
                blockPerArmor.ToString("F1") + "% extra block armor\n" +
                blockHealth.ToString("F0") + " extra max health";
        }
        public static void OpenBowPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Bows);

            float bowDamage = ToPercent(ConfigManager.GetBowDamageMod(skill));
            float bowStaminaRedux = ToPercent(ConfigManager.GetBowStaminaRedux(skill));
            float bowVelocity = ToPercent(ConfigManager.GetBowVelocityMod(skill));
            float bowDrawSpeed = ToPercent(ConfigManager.GetBowDrawSpeedMod(skill));
            float bowDropRate = ToPercent(ConfigManager.GetBowDropRate(skill));

            LeftPanelExperienceText.GetComponent<Text>().text =
                "A percentage of all bow damage dealt is turned into experience. \n" +
                "The exp gain rate is much higher versus living targets. \n" +
                "Every time you shoot, a small amount of experience is gained, whether you hit anything or not. \n" +
                "Holding a bow gains you experience at a very slow rate. \n" +
                "Bonus experience is gained based on the length and difficulty of successful shots with a bow.";
            LeftPanelEffectsText.GetComponent<Text>().text =
                bowDamage.ToString("F1") + "% extra damage with bows\n" +
                bowStaminaRedux.ToString("F1") + "% stamina usage with bows\n" +
                bowVelocity.ToString("F1") + "% extra arrow velocity\n" +
                bowDrawSpeed.ToString("F1") + "% faster draw speed\n" +
                bowDropRate.ToString("F1") + "% extra loot drops from creatures";
        }
        public static void OpenClubPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Clubs);

            float clubDamage = ToPercent(ConfigManager.GetClubDamageMod(skill));
            float clubStaminaRedux = ToPercent(ConfigManager.GetClubStaminaRedux(skill));
            float clubBlunt = ToPercent(ConfigManager.GetClubBluntMod(skill));
            float clubKnockback = ToPercent(ConfigManager.GetClubKnockbackMod(skill));
            float clubStagger = ToPercent(ConfigManager.GetClubStaggerMod(skill));

            LeftPanelExperienceText.GetComponent<Text>().text =
                "A percentage of all club damage dealt is turned into experience. \n" +
                "The exp gain rate is much higher versus living targets. \n" +
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not. \n" +
                "Holding a club gains you experience at a very slow rate. \n" +
                "Bonus experience every time you stagger an enemy with damage with a club.";
            LeftPanelEffectsText.GetComponent<Text>().text =
                clubDamage.ToString("F1") + "% extra damage with clubs\n" +
                clubStaminaRedux.ToString("F1") + "% stamina usage with clubs\n" +
                clubBlunt.ToString("F1") + "% extra bonus to ALL blunt damage\n" +
                clubKnockback.ToString("F1") + "% knockback bonus to ALL weapons\n" +
                clubStagger.ToString("F1") + "% extra stagger damage to ALL weapons";
        }
        public static void OpenFistPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Unarmed);

            float fistDamage = ToPercent(ConfigManager.GetFistDamageMod(skill));
            float fistStaminaRedux = ToPercent(ConfigManager.GetFistStaminaRedux(skill));
            float fistFlatDamage = ConfigManager.GetFistDamageFlat(skill);
            float fistBlock = ConfigManager.GetFistBlockArmor(skill);
            float fistMovespeed = ToPercent(ConfigManager.GetFistMovespeedMod(skill));

            LeftPanelExperienceText.GetComponent<Text>().text =
                "A percentage of all unarmed damage dealt is turned into experience. \n" +
                "The exp gain rate is much higher versus living targets. \n" +
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not. \n" +
                "Holding no weapon gains you experience at a very slow rate. \n" +
                "Bonus experence for fists is gained every time you perform an unarmed block.";
            LeftPanelEffectsText.GetComponent<Text>().text =
                fistDamage.ToString("F1") + "% extra damage with fists\n" +
                fistStaminaRedux.ToString("F1") + "% stamina usage with fists\n" +
                fistFlatDamage.ToString("F0") + " extra flat damage\n" +
                fistBlock.ToString("F0") + " extra unarmed block armor\n" +
                fistMovespeed.ToString("F1") + "% extra move speed";
        }
        public static void OpenJumpPanels()
        {
            Player player = Player.m_localPlayer;
            LevelPatch.JumpForceUpdate(player);
            float skill = player.GetSkillFactor(Skills.SkillType.Jump);

            float bonusJumpForce = ToPercent(ConfigManager.GetJumpForceMod(skill));
            float bonusJumpForwardForce = ToPercent(ConfigManager.GetJumpForwardForceMod(skill));
            float staminaRedux = ToPercent(ConfigManager.GetJumpStaminaRedux(skill));
            float tired = ToPercent(ConfigManager.GetJumpTiredRedux(skill));

            float fallDamageThreshhold = ConfigManager.GetFallDamageThreshold(skill);
            float fallDamageRedux = ToPercent(ConfigManager.GetFallDamageRedux(skill));

            LeftPanelExperienceText.GetComponent<Text>().text =
                "Every time you jump, you gain a small amount of experience. \n" +
                "Every time you land, you gain an amount of experience based on " +
                "how far you fell and how much damage you'd take.";
            LeftPanelEffectsText.GetComponent<Text>().text =
                bonusJumpForce.ToString("F1") + "% extra vertical jump force \n" +
                bonusJumpForwardForce.ToString("F1") + "% extra horizontal jump force \n" +
                staminaRedux.ToString("F1") + "% less stamina cost to jump \n" +
                tired.ToString("F0") + "% jump force modifier when tired \n" +
                fallDamageThreshhold.ToString("F1") + "m minimum fall damage height \n" +
                fallDamageRedux.ToString("F1") + "% less fall damage";
        }
        public static void OpenKnifePanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Knives);

            float knifeDamage = ToPercent(ConfigManager.GetKnifeDamageMod(skill));
            float knifeStaminaRedux = ToPercent(ConfigManager.GetKnifeStaminaRedux(skill));
            float knifeBackstab = ToPercent(ConfigManager.GetKnifeBackstabMod(skill));
            float knifeMovespeed = ToPercent(ConfigManager.GetKnifeMovespeedMod(skill));
            float knifePierce = ToPercent(ConfigManager.GetKnifePierceMod(skill));

            LeftPanelExperienceText.GetComponent<Text>().text =
                "A percentage of all knife damage dealt is turned into experience. \n" +
                "The exp gain rate is much higher versus living targets. \n" +
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not. \n" +
                "Holding a knife gains you experience at a very slow rate. \n" +
                "Bonus experience is gained every time you perform a sneak attack with a knife.";
            LeftPanelEffectsText.GetComponent<Text>().text =
                knifeDamage.ToString("F1") + "% extra damage with knives \n" +
                knifeStaminaRedux.ToString("F1") + "% stamina usage with knives \n" +
                knifeBackstab.ToString("F0") + "% sneak attack bonus damage \n" +
                knifeMovespeed.ToString("F1") + "% extra move speed \n" +
                knifePierce.ToString("F1") + "% extra to ALL pierce damage";
        }
        public static void OpenPolearmPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Polearms);

            float polearmDamage = ToPercent(ConfigManager.GetPolearmDamageMod(skill));
            float polearmStaminaRedux = ToPercent(ConfigManager.GetPolearmStaminaRedux(skill));
            float polearmRange = ConfigManager.GetPolearmRange(skill);
            float polearmArmor = ConfigManager.GetPolearmArmor(skill);
            float polearmBlock = ConfigManager.GetPolearmBlock(skill);

            LeftPanelExperienceText.GetComponent<Text>().text =
                "A percentage of all polearm damage dealt is turned into experience. \n" +
                "The exp gain rate is much higher versus living targets. \n" +
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not. \n" +
                "Holding a polearm gains you experience at a very slow rate. \n" +
                "Bonus experience for the polearm is gained everytime you hit multiple targets in a single swing.";
            LeftPanelEffectsText.GetComponent<Text>().text =
                polearmDamage.ToString("F1") + "% extra damage with polearms \n" +
                polearmStaminaRedux.ToString("F1") + "% stamina usage with polearms\n" +
                polearmRange.ToString("F0") + " increased units of range with all weapons\n" +
                polearmArmor.ToString("F0") + " increased armor\n" +
                polearmBlock.ToString("F0") + " extra block power with polearms";
        }
        public static void OpenRunPanels()
        {
            Player player = Player.m_localPlayer;
            LevelPatch.RunSpeedUpdate(player);
            float skill = player.GetSkillFactor(Skills.SkillType.Run);

            float runSpeedBonus = ToPercent(ConfigManager.GetRunSpeedMod(skill));
            float equipmentMalusRedux = ToPercent(ConfigManager.GetEquipmentRedux(skill));
            float encumberanceRedux = ToPercent(ConfigManager.GetEncumberanceRedux(skill));
            float staminaDrainRedux = ToPercent(ConfigManager.GetRunStaminaRedux(skill));
            float baseStaminaGain = ConfigManager.GetRunStamina(skill);


            float encumberanceFactor = ToPercent(MovePatch.GetEncumberanceFactor(player));
            float equipmentFactor = ToPercent(MovePatch.GetEquipmentFactor(player));

            float absWeightExp = ToPercent(MovePatch.absoluteWeightBonus(player));
            float relWeightExp = ToPercent(MovePatch.relativeWeightBonus(player));
            float runSpeedExp = ToPercent(MovePatch.runSpeedExpBonus(player));

            LeftPanelExperienceText.GetComponent<Text>().text =
                "The faster you are moving, the more experience you get.\n" +
                "The closer you are to fully encumbered, the less movespeed you have.\n" +
                "You also gain experience based on how encumbered you are. \n" +
                runSpeedExp.ToString("F1") + "% experience bonus from current run speed \n" +
                absWeightExp.ToString("F1") + "% experience bonus from absolute weight carried \n" +
                relWeightExp.ToString("F1") + "% experience bonus from fullness of inventory";
            LeftPanelEffectsText.GetComponent<Text>().text =
                runSpeedBonus.ToString("F1") + "% extra run speed\n" +
                equipmentMalusRedux.ToString("F1") + "% reduction to equipment penalty\n" +
                encumberanceRedux.ToString("F1") + "% reduction to encumberance penalty\n" +
                staminaDrainRedux.ToString("F1") + "% stamina cost to run\n" +
                baseStaminaGain.ToString("F1") + " extra base stamina\n" +
                "\nCurrent effects from outside factors: \n" +
                encumberanceFactor.ToString("F1") + "% speed from encumberance\n" +
                equipmentFactor.ToString("F1") + "% speed from equipment";
        }
        public static void OpenSpearPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Spears);

            float spearDamage = ToPercent(ConfigManager.GetSpearDamageMod(skill));
            float spearStaminaRedux = ToPercent(ConfigManager.GetSpearStaminaRedux(skill));
            float spearVelocity = ToPercent(ConfigManager.GetSpearVelocityMod(skill));
            float spearThrowDamage = ToPercent(ConfigManager.GetSpearProjectileDamageMod(skill));
            float spearBlock = ConfigManager.GetSpearBlockArmor(skill);

            LeftPanelExperienceText.GetComponent<Text>().text =
                "A percentage of all spear damage dealt is turned into experience. \n" +
                "The exp gain rate is much higher versus living targets. \n" +
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not. \n" +
                "Holding a spear gains you experience at a very slow rate. \n" +
                "Bonus experience anytime you hit an enemy with a thrown weapon.";
            LeftPanelEffectsText.GetComponent<Text>().text =
                spearDamage.ToString("F1") + "% extra damage with spears \n" +
                spearStaminaRedux.ToString("F1") + "% stamina usage with spears\n" +
                spearVelocity.ToString("F1") + "% increased velocity with all thrown weapons\n" +
                spearThrowDamage.ToString("F1") + "% increased damage with all thrown weapons\n" +
                spearBlock.ToString("F0") + " higher flat block armor";
        }

        public static void OpenSneakPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Sneak);
            LevelPatch.SneakUpdate(Player.m_localPlayer);

            float sneakSpeed = ToPercent(ConfigManager.GetSneakSpeedMod(skill));
            float sneakStaminaCost = ToPercent(ConfigManager.GetSneakStaminaDrain(skill));
            float sneakLightFactor = ToPercent(ConfigManager.GetSneakFactor(skill, 2f));
            float sneakDarkFactor = ToPercent(ConfigManager.GetSneakFactor(skill, 0f));

            LeftPanelExperienceText.GetComponent<Text>().text =
                "While you are inside the vision angle of an enemy " +
                "but outside its vision range, you gain a flat amount of sneak " +
                "experience every second. \n This value is reduced to 10% while" +
                "you aren't being observed. \n" +
                "You get an additional experience bonus based on how dangerous the " +
                "creature you're sneaking around is.";
            LeftPanelEffectsText.GetComponent<Text>().text =
                sneakSpeed.ToString("F1") + "% increased speed while crouching\n" +
                sneakStaminaCost.ToString("F1") + " stamina per second while crouching\n" +
                sneakLightFactor.ToString("F0") + "% increased sneakiness in the light \n" +
                sneakDarkFactor.ToString("F0") + "% increased sneakiness in the dark";
        }
        public static void OpenSwimPanels()
        {
            Player player = Player.m_localPlayer;
            LevelPatch.SwimSpeedUpdate(player);
            float skill = player.GetSkillFactor(Skills.SkillType.Run);

            float swimSpeed = ToPercent(ConfigManager.GetSwimSpeedMod(skill));
            float swimAccel = ToPercent(ConfigManager.GetSwimAccelMod(skill));
            float swimTurn = ToPercent(ConfigManager.GetSwimTurnMod(skill));
            float swimStaminaCost = ConfigManager.GetSwimStaminaPerSec(skill);

            float absWeightExp = ToPercent(MovePatch.absoluteWeightBonus(player));
            float relWeightExp = ToPercent(MovePatch.relativeWeightBonus(player));
            float swimSpeedExp = ToPercent(MovePatch.swimSpeedExpBonus(player));


            LeftPanelExperienceText.GetComponent<Text>().text =
                "The faster you are moving, the more experience you get.\n" +
                "You also gain experience based on how encumbered you are. \n" +
                swimSpeedExp.ToString("F1") + "% experience bonus from current swimming speed \n" +
                absWeightExp.ToString("F1") + "% experience bonus from absolute weight carried \n" +
                relWeightExp.ToString("F1") + "% experience bonus from fullness of inventory";
            LeftPanelEffectsText.GetComponent<Text>().text =
                swimSpeed.ToString("F1") + "% extra swim speed\n" +
                swimAccel.ToString("F1") + "% extra acceleration in water\n" +
                swimTurn.ToString("F1") + "% extra turn speed in water\n" +
                swimStaminaCost.ToString("F2") + " stamina per second while swimming";
        }
        public static void OpenSwordPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Swords);
            LevelPatch.SwordUpdate(Player.m_localPlayer);


            float swordDamage = ToPercent(ConfigManager.GetSwordDamageMod(skill));
            float swordStaminaRedux = ToPercent(ConfigManager.GetSwordStaminaRedux(skill));
            float swordParry = ToPercent(ConfigManager.GetSwordParryMod(skill));
            float swordSlash = ToPercent(ConfigManager.GetSwordSlashMod(skill));
            float swordDodgeStaminaRedux = ToPercent(ConfigManager.GetSwordDodgeStaminaRedux(skill));

            LeftPanelExperienceText.GetComponent<Text>().text =
                "A percentage of all sword damage dealt is turned into experience. \n" +
                "The exp gain rate is much higher versus living targets. \n" +
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not. \n" +
                "Holding a sword gains you experience at a very slow rate. \n" +
                "Bonus experience is gained every time you deal damage to a staggered enemy with a sword.";
            LeftPanelEffectsText.GetComponent<Text>().text =
                swordDamage.ToString("F1") + "% extra damage with swords \n" +
                swordStaminaRedux.ToString("F1") + "% stamina usage with swords\n" +
                swordParry.ToString("F0") + "% higher parry bonus with ALL weapons \n" +
                swordSlash.ToString("F1") + "% increased slash damage with ALL weapons \n" +
                swordDodgeStaminaRedux.ToString("F1") + "% stamina cost to dodge";
        }
        public static void OpenWoodcuttingPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.WoodCutting);

            float woodDamage = ToPercent(ConfigManager.GetWoodcuttingDamageMod(skill));
            float woodDrop = ToPercent(ConfigManager.GetWoodDropRate(skill));
            float woodRebate = ConfigManager.GetWoodcuttingStaminaRebate(skill);
            float woodSomething = ToPercent(0f);

            LeftPanelExperienceText.GetComponent<Text>().text =
                "A percentage of all chop damage dealt is turned into experience. \n" +
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not. \n" +
                "Bonus experience for the axe is gained based on the tier of the tool used.";
            LeftPanelEffectsText.GetComponent<Text>().text =
                woodDamage.ToString("F1") + "% extra damage to trees\n" +
                woodDrop.ToString("F2") + "% increased wood drop rates from trees\n" +
                woodRebate.ToString("F0") + " stamina rebate on axe swings that hit a tree\n" +
                woodSomething.ToString("F0") + " something else!";
        }

        public static void OpenMiningPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Pickaxes);

            float mineDamage = ToPercent(ConfigManager.GetMiningDamageMod(skill));
            float mineDrop = ToPercent(ConfigManager.GetMiningDropRate(skill));
            float mineRebate = ConfigManager.GetMiningStaminaRebate(skill);
            float mineSomething = ToPercent(0f);

            LeftPanelExperienceText.GetComponent<Text>().text =
                "A percentage of all pick damage dealt to ground turned into experience. \n" +
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not. \n" +
                "Bonus experience for the pick is gained based on the tier of the tool used.\n" +
                "You also gain experience, if reduced, for mining ground without ore.";
            LeftPanelEffectsText.GetComponent<Text>().text =
                mineDamage.ToString("F1") + "% extra damage to rocks\n" +
                mineDrop.ToString("F2") + "% increased ore drop rates\n" +
                mineRebate.ToString("F0") + " stamina rebate on pick swings that hit a rock\n" +
                mineSomething.ToString("F0") + " something else!";
        }

        public static float ToPercent(float number)
        {
            return (number - 1) * 100;
        }
    }
}
