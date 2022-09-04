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
    class PerkTooltipGUI
    {
        public static GameObject PerkTooltipWindow;
        public static GameObject PTTitle;
        public static GameObject PTTooltip;
        public static bool init = false;

        public static Perk highlightedPerk;

        public static void UpdateTooltip(Perk perk){
            if (!init) InitTooltip();
            if (highlightedPerk == null) NewTooltip(perk);
            if (!PerkTooltipWindow.activeSelf) PerkTooltipWindow.SetActive(true);

            //do position stuff
        }
        public static void CloseTooltip()
        {
            if (!init) InitTooltip();
            highlightedPerk = null;
            PerkTooltipWindow.SetActive(false);
        }
        public static void NewTooltip(Perk perk)
        {
            highlightedPerk = perk;
            PTTitle.GetComponent<Text>().text = perk.name;
            PTTooltip.GetComponent<Text>().text = perk.tooltip;
        }

        public static void InitTooltip()
        {
            init = true;
            Jotunn.Logger.LogMessage("Init the perk tooltip window");
            PerkTooltipWindow = GUIManager.Instance.CreateWoodpanel(
                    parent: SkillGUI.RightPanelPerks.transform,
                    anchorMin: new Vector2(.5f, .5f),
                    anchorMax: new Vector2(.5f, .5f),
                    position: new Vector2(0f, 20),
                    width: 320,
                    height: 80,
                    draggable: true);
            PerkTooltipWindow.SetActive(false);

            PTTitle =GUIManager.Instance.CreateText(
                text: "",
                parent: PerkTooltipWindow.transform,
                anchorMin: new Vector2(0.5f, 1f),
                anchorMax: new Vector2(0.5f, 1f),
                position: new Vector2(0f, -10f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 15,
                color: GUIManager.Instance.ValheimOrange,
                outline: true,
                outlineColor: Color.black,
                width: 300f,
                height: 19f,
                addContentSizeFitter: false);
            PTTitle.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

            PTTooltip = GUIManager.Instance.CreateText(
                text: "",
                parent: PerkTooltipWindow.transform,
                anchorMin: new Vector2(0.5f, 0f),
                anchorMax: new Vector2(0.5f, 0f),
                position: new Vector2(0f, 30f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 12,
                color: GUIManager.Instance.ValheimOrange,
                outline: true,
                outlineColor: Color.black,
                width: 300f,
                height: 58f,
                addContentSizeFitter: false);
            PTTooltip.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            PTTooltip.GetComponent<Text>().horizontalOverflow = HorizontalWrapMode.Wrap;
        }

    }
}
