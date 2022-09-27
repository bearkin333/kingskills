using Jotunn.Managers;
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
    class LearnConfirmationGUI
    {
        public static int confirmPerkKey;

        public static void OpenLearnConfirmation(int confirmPerk)
        {
            if (!Window) Init();

            Window.SetActive(true);
            GUIManager.BlockInput(true);
            confirmPerkKey = confirmPerk;

            LCPerkTitle.GetComponent<Text>().text = OpenPerks.openedPerks[confirmPerk].name;
            LCPerkDescription.GetComponent<Text>().text = OpenPerks.openedPerks[confirmPerk].description;
        }

        public static GameObject Window;
        public static GameObject LCPerkTitle;
        public static GameObject LCPerkDescription;
        public static GameObject LCYesBtn;
        public static GameObject LCNoBtn;

        public static void Init()
        {
            Window = GUIManager.Instance.CreateWoodpanel(
                    parent: GUIManager.CustomGUIFront.transform,
                    anchorMin: new Vector2(0.5f, 0.5f),
                    anchorMax: new Vector2(0.5f, 0.5f),
                    position: new Vector2(0f, 0),
                    width: 800,
                    height: 300,
                    draggable: true);
            Window.SetActive(false);


            GameObject obj = GUIManager.Instance.CreateText(
                text: "Are you sure you want to learn",
                parent: Window.transform,
                anchorMin: new Vector2(0.5f, 1f),
                anchorMax: new Vector2(0.5f, 1f),
                position: new Vector2(0f, -75f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 30,
                color: GUIManager.Instance.ValheimOrange,
                outline: true,
                outlineColor: Color.black,
                width: 600f,
                height: 80f,
                addContentSizeFitter: false);
            obj.GetComponent<Text>().alignment = TextAnchor.UpperCenter;


            LCPerkTitle = GUIManager.Instance.CreateText(
                text: "",
                parent: Window.transform,
                anchorMin: new Vector2(0.5f, 1f),
                anchorMax: new Vector2(0.5f, 1f),
                position: new Vector2(0f, -115f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 48,
                color: GUIManager.Instance.ValheimOrange,
                outline: true,
                outlineColor: Color.black,
                width: 800f,
                height: 80f,
                addContentSizeFitter: false);
            LCPerkTitle.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;


            LCPerkDescription = GUIManager.Instance.CreateText(
                text: "",
                parent: Window.transform,
                anchorMin: new Vector2(0.5f, 1f),
                anchorMax: new Vector2(0.5f, 1f),
                position: new Vector2(0f, -190f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 16,
                color: GUIManager.Instance.ValheimOrange,
                outline: true,
                outlineColor: Color.black,
                width: 550f,
                height: 100f,
                addContentSizeFitter: false);
            Text text = LCPerkDescription.GetComponent<Text>();
            text.alignment = TextAnchor.MiddleCenter;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;


            LCYesBtn = GUIManager.Instance.CreateButton(
                text: "Yes",
                parent: Window.transform,
                anchorMin: new Vector2(0.5f, 0f),
                anchorMax: new Vector2(0.5f, 0f),
                position: new Vector2(-120f, 38f),
                width: 120f,
                height: 55f);
            Button btn = LCYesBtn.GetComponent<Button>();
            btn.onClick.AddListener(OnConfirmClick);

            LCNoBtn = GUIManager.Instance.CreateButton(
                text: "No",
                parent: Window.transform,
                anchorMin: new Vector2(0.5f, 0f),
                anchorMax: new Vector2(0.5f, 0f),
                position: new Vector2(120f, 38f),
                width: 120f,
                height: 55f);
            btn = LCNoBtn.GetComponent<Button>();
            btn.onClick.AddListener(Close);

            confirmPerkKey = OpenPerks.NoPerk;
        }

        public static void OnConfirmClick()
        {
            OpenPerks.LearnPerk(confirmPerkKey);
            Close();
        }

        public static void Close()
        {
            if (!Window) Init();
            Window.SetActive(false);
            GUIManager.BlockInput(false);

            confirmPerkKey = OpenPerks.NoPerk;
        }

        public static bool IsConfirmOpen()
        {
            if (Window == null) return false;
            else return Window.activeSelf;
        }

    }
}
