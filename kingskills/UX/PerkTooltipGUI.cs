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
    class PerkTooltipGUI
    {
        public static GameObject MiniTooltipWindow;
        public static GameObject MiniTitle;
        public static GameObject MiniBlurb;

        public static GameObject DetailedTooltipWindow;
        public static GameObject DetailTitle;
        public static GameObject DetailDescription;
        public static GameObject DetailBlurb;
        public static GameObject DetailDeactivated;

        public static Perk highlightedPerk;

        public static void UpdateTooltip(Perk perk, string DetailedHoverButton)
        {
            InitCheck();
            if (highlightedPerk != perk) NewTooltip(perk);

            if (ZInput.GetButton(DetailedHoverButton))
            {
                if (!DetailedTooltipWindow.activeSelf) DetailedTooltipWindow.SetActive(true);
            }
            else
            {
                if (!MiniTooltipWindow.activeSelf) MiniTooltipWindow.SetActive(true);
                DetailedTooltipWindow.SetActive(false);
            }


        }
        public static void CloseTooltip()
        {
            InitCheck();
            highlightedPerk = null;
            MiniTooltipWindow.SetActive(false);
            DetailedTooltipWindow.SetActive(false);
        }
        public static void NewTooltip(Perk perk)
        {
            highlightedPerk = perk;
            MiniTitle.GetComponent<Text>().text = perk.name;
            MiniBlurb.GetComponent<Text>().text = perk.blurb;
            DetailTitle.GetComponent<Text>().text = perk.name;
            DetailDescription.GetComponent<Text>().text = perk.description;
            DetailBlurb.GetComponent<Text>().text = "<i>" + perk.blurb + "</i>";
            if (PerkMan.IsPerkDeactivated(perk.type))
            {
                DetailDeactivated.GetComponent<Text>().enabled = true;
            }
            else
            {
                DetailDeactivated.GetComponent<Text>().enabled = false;
            }
        }


        public static void InitCheck()
        {
            if (!MiniTooltipWindow) InitMiniTooltip();
            if (!DetailedTooltipWindow) InitDetailedTooltip();
        }


        public static void InitMiniTooltip()
        {
            //Jotunn.Logger.LogMessage("Init the perk tooltip window");
            MiniTooltipWindow = GUIManager.Instance.CreateWoodpanel(
                    parent: SkillGUI.RightPanel.transform,
                    anchorMin: new Vector2(.5f, 1f),
                    anchorMax: new Vector2(.5f, 1f),
                    position: new Vector2(0f, -385),
                    width: 320,
                    height: 100,
                    draggable: true);
            MiniTooltipWindow.SetActive(false);

            MiniTitle =GUIManager.Instance.CreateText(
                text: "",
                parent: MiniTooltipWindow.transform,
                anchorMin: new Vector2(0.5f, 1f),
                anchorMax: new Vector2(0.5f, 1f),
                position: new Vector2(0f, -22f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 16,
                color: CFG.ColorTitle,
                outline: true,
                outlineColor: Color.black,
                width: 300f,
                height: 19f,
                addContentSizeFitter: false);
            MiniTitle.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

            MiniBlurb = GUIManager.Instance.CreateText(
                text: "",
                parent: MiniTooltipWindow.transform,
                anchorMin: new Vector2(0.5f, 0f),
                anchorMax: new Vector2(0.5f, 0f),
                position: new Vector2(0f, 50f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 13,
                color: CFG.ColorKingBlurbs,
                outline: true,
                outlineColor: Color.black,
                width: 300f,
                height: 58f,
                addContentSizeFitter: false);
            MiniBlurb.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            MiniBlurb.GetComponent<Text>().horizontalOverflow = HorizontalWrapMode.Wrap;

            GameObject obj = GUIManager.Instance.CreateText(
                text: $"Press {CFG.ColorTitleFF}[Shift]{CFG.ColorEnd} for more details.",
                parent: MiniTooltipWindow.transform,
                anchorMin: new Vector2(0.5f, 0f),
                anchorMax: new Vector2(0.5f, 0f),
                position: new Vector2(0f, 15f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 13,
                color: CFG.ColorKingBlurbs,
                outline: true,
                outlineColor: Color.black,
                width: 300f,
                height: 40f,
                addContentSizeFitter: false);
            obj.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            obj.GetComponent<Text>().horizontalOverflow = HorizontalWrapMode.Wrap;
        }

        public static void InitDetailedTooltip()
        {
            DetailedTooltipWindow = GUIManager.Instance.CreateWoodpanel(
                    parent: SkillGUI.SkillGUIWindow.transform,
                    anchorMin: new Vector2(.5f, .5f),
                    anchorMax: new Vector2(.5f, .5f),
                    position: new Vector2(0f, -215f),
                    width: 700,
                    height: 320,
                    draggable: true);
            DetailedTooltipWindow.SetActive(false);

            DetailTitle = GUIManager.Instance.CreateText(
                text: "",
                parent: DetailedTooltipWindow.transform,
                anchorMin: new Vector2(0.5f, 1f),
                anchorMax: new Vector2(0.5f, 1f),
                position: new Vector2(0f, -50f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 34,
                color: CFG.ColorTitle,
                outline: true,
                outlineColor: Color.black,
                width: 600f,
                height: 80f,
                addContentSizeFitter: false);
            DetailTitle.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

            DetailDescription = GUIManager.Instance.CreateText(
                text: "",
                parent: DetailedTooltipWindow.transform,
                anchorMin: new Vector2(0.5f, 1f),
                anchorMax: new Vector2(0.5f, 1f),
                position: new Vector2(0f, -125f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 23,
                color: GUIManager.Instance.ValheimOrange,
                outline: true,
                outlineColor: Color.black,
                width: 670f,
                height: 150f,
                addContentSizeFitter: false);
            DetailDescription.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            DetailDescription.GetComponent<Text>().horizontalOverflow = HorizontalWrapMode.Wrap;

            DetailBlurb = GUIManager.Instance.CreateText(
                text: "",
                parent: DetailedTooltipWindow.transform,
                anchorMin: new Vector2(0.5f, 0f),
                anchorMax: new Vector2(0.5f, 0f),
                position: new Vector2(0f, 40f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 20,
                color: CFG.ColorKingBlurbs,
                outline: true,
                outlineColor: Color.black,
                width: 600f,
                height: 50f,
                addContentSizeFitter: false);
            DetailBlurb.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            DetailBlurb.GetComponent<Text>().horizontalOverflow = HorizontalWrapMode.Wrap;

            DetailDeactivated = GUIManager.Instance.CreateText(
                text: "DEACTIVATED",
                parent: DetailedTooltipWindow.transform,
                anchorMin: new Vector2(0.5f, 0.5f),
                anchorMax: new Vector2(0.5f, 0.5f),
                position: new Vector2(0f, 0f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 80,
                color: CFG.ColorPTRed,
                outline: true,
                outlineColor: Color.black,
                width: 600f,
                height: 200f,
                addContentSizeFitter: false);
            DetailDeactivated.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            DetailDeactivated.transform.Rotate(new Vector3(0, 0, 24));
        }
    }
}
