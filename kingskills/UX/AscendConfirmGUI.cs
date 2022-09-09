using Jotunn.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace kingskills.UX
{
    class AscendConfirmGUI
    {
        public static Skills.SkillType skill;

        public static void OpenAscendWindow()
        {
            if (AscendConfirmWindow == null) AwakeAscendConfirmation();

            skill = OpenPerks.openSkill;

            if (skill == Skills.SkillType.None) return;

            Skills.SkillDef actualSkill = Player.m_localPlayer.GetSkills().GetSkillDef(skill);

            ACSkill.GetComponent<Text>().text = CFG.GetNameFromSkill(skill);
            ACSkillIcon.GetComponent<Image>().sprite = actualSkill.m_icon;

            AscendConfirmWindow.SetActive(true);
            SkillGUIUpdate.SetInteractable(false);
            GUIManager.BlockInput(true);
        }

        public static GameObject AscendConfirmWindow;
        public static GameObject ACSkill;
        public static Image ACSkillIcon;
        public static GameObject ACYesBtn;
        public static GameObject ACNoBtn;

        public static void AwakeAscendConfirmation()
        {
            AscendConfirmWindow = GUIManager.Instance.CreateWoodpanel(
                    parent: GUIManager.CustomGUIFront.transform,
                    anchorMin: new Vector2(0.5f, 0.5f),
                    anchorMax: new Vector2(0.5f, 0.5f),
                    position: new Vector2(0f, 0),
                    width: 600,
                    height: 300,
                    draggable: true);
            AscendConfirmWindow.SetActive(false);


            GameObject obj = GUIManager.Instance.CreateText(
                text: "Are you sure you want to ascend ",
                parent: AscendConfirmWindow.transform,
                anchorMin: new Vector2(0.5f, 1f),
                anchorMax: new Vector2(0.5f, 1f),
                position: new Vector2(0f, -60f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 30,
                color: GUIManager.Instance.ValheimOrange,
                outline: true,
                outlineColor: Color.black,
                width: 600f,
                height: 80f,
                addContentSizeFitter: false);
            obj.GetComponent<Text>().alignment = TextAnchor.UpperCenter;


            ACSkill = GUIManager.Instance.CreateText(
                text: "",
                parent: AscendConfirmWindow.transform,
                anchorMin: new Vector2(0.5f, 0.5f),
                anchorMax: new Vector2(0.5f, 0.5f),
                position: new Vector2(0f, 40f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 50,
                color: GUIManager.Instance.ValheimOrange,
                outline: true,
                outlineColor: Color.black,
                width: 400f,
                height: 90f,
                addContentSizeFitter: false);
            ACSkill.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;


            obj = new GameObject();
            ACSkillIcon = obj.AddComponent<Image>();
            ACSkillIcon.sprite = null;
            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.SetParent(AscendConfirmWindow.transform);
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(-160f, 40f);
            rect.sizeDelta = new Vector2(90f, 90f);


            obj = GUIManager.Instance.CreateText(
                text: "This cannot be undone. The skill will reset to level 0, and you will lose all relevant effects, except " +
                "learned perks. However, you will be able to level the skill up again, this time learning all the perks you did " +
                "not pick the first time.",
                parent: AscendConfirmWindow.transform,
                anchorMin: new Vector2(0.5f, 0f),
                anchorMax: new Vector2(0.5f, 0f),
                position: new Vector2(0f, 100f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 18,
                color: GUIManager.Instance.ValheimOrange,
                outline: true,
                outlineColor: Color.black,
                width: 550f,
                height: 80f,
                addContentSizeFitter: false);
            obj.GetComponent<Text>().alignment = TextAnchor.UpperCenter;
            obj.GetComponent<Text>().horizontalOverflow = HorizontalWrapMode.Wrap;
            obj.GetComponent<Text>().verticalOverflow = VerticalWrapMode.Overflow;


            ACYesBtn = GUIManager.Instance.CreateButton(
                text: "Yes",
                parent: AscendConfirmWindow.transform,
                anchorMin: new Vector2(0.5f, 0f),
                anchorMax: new Vector2(0.5f, 0f),
                position: new Vector2(-120f, 30f),
                width: 120f,
                height: 45f);
            Button btn = ACYesBtn.GetComponent<Button>();
            btn.onClick.AddListener(OnConfirmClick);


            ACNoBtn = GUIManager.Instance.CreateButton(
                text: "No",
                parent: AscendConfirmWindow.transform,
                anchorMin: new Vector2(0.5f, 0f),
                anchorMax: new Vector2(0.5f, 0f),
                position: new Vector2(120f, 30f),
                width: 120f,
                height: 45f);
            btn = ACNoBtn.GetComponent<Button>();
            btn.onClick.AddListener(CloseAscendWindow);


            skill = Skills.SkillType.None;
        }

        public static void OnConfirmClick()
        {
            AscensionManager.Ascend();
            CloseAscendWindow();
        }

        public static void CloseAscendWindow()
        {
            AscendConfirmWindow.SetActive(false);
            GUIManager.BlockInput(false);

            skill = Skills.SkillType.None;
            SkillGUIUpdate.SetInteractable(true);
        }

        public static bool IsOpen()
        {
            if (AscendConfirmWindow == null) return false;
            else return AscendConfirmWindow.activeSelf;
        }

    }
}

