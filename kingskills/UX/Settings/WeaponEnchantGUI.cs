using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using kingskills.Perks;
using Jotunn.Managers;
using UnityEngine.UI;

namespace kingskills.UX
{
    class WeaponEnchantGUI
    {
        public static GameObject Window;
        public static GameObject Title;
        public static GameObject ActiveEnchant;
        public static Dictionary<WeaponEnchant, GameObject> Enchants;
        public static WeaponEnchant loadedEnchant = WeaponEnchant.None;

        public static bool learnedAtLeastOne = false;
        public static void Open()
        {
            if (!Window) Init();
            UpdateLearnedEnchants();
            Window.SetActive(true);
        }

        public static void Close()
        {
            if (!Window) Init();
            Window.SetActive(false);
        }


        public static void SetActive(WeaponEnchant enchant)
        {
            if (!IsEnchantLearned(enchant)) return;

            string activeText = $"{enchant}";

            GetEnchantColors(enchant, ref activeText);
  
            P_WeaponEnchants.activeEnchant = enchant;
            ActiveEnchant.GetComponent<Text>().text = $"Active: {activeText}";
        }

        public static bool IsEnchantLearned(WeaponEnchant enchant)
        {
            switch (enchant)
            {
                case WeaponEnchant.Fire:
                    return PerkMan.IsPerkActive(PerkMan.PerkType.Cauterize);
                case WeaponEnchant.Lightning:
                    return PerkMan.IsPerkActive(PerkMan.PerkType.Mjolnir);
                case WeaponEnchant.Spirit:
                    return PerkMan.IsPerkActive(PerkMan.PerkType.Spearit);
                case WeaponEnchant.Poison:
                    return PerkMan.IsPerkActive(PerkMan.PerkType.Toxic);
                case WeaponEnchant.Frost:
                    return PerkMan.IsPerkActive(PerkMan.PerkType.Ymir);
                case WeaponEnchant.None:
                    return true;
            }
            return false;
        }

        public static void UpdateLearnedEnchants()
        {
            bool fire = false;
            bool lightning = false;
            bool spirit = false;
            bool poison = false;
            bool frost = false;

            if (PerkMan.IsPerkActive(PerkMan.PerkType.Cauterize)) fire = true;
            if (PerkMan.IsPerkActive(PerkMan.PerkType.Mjolnir)) lightning = true;
            if (PerkMan.IsPerkActive(PerkMan.PerkType.Spearit)) spirit = true;
            if (PerkMan.IsPerkActive(PerkMan.PerkType.Toxic)) poison = true;
            if (PerkMan.IsPerkActive(PerkMan.PerkType.Ymir)) frost = true;

            learnedAtLeastOne = fire || lightning || spirit || poison || frost;

            switch (P_WeaponEnchants.activeEnchant)
            {
                case WeaponEnchant.Fire:
                    if (!fire) SetActive(WeaponEnchant.None);
                    break;
                case WeaponEnchant.Lightning:
                    if (!lightning) SetActive(WeaponEnchant.None);
                    break;
                case WeaponEnchant.Spirit:
                    if (!spirit) SetActive(WeaponEnchant.None);
                    break;
                case WeaponEnchant.Poison:
                    if (!poison) SetActive(WeaponEnchant.None);
                    break;
                case WeaponEnchant.Frost:
                    if (!frost) SetActive(WeaponEnchant.None);
                    break;
            }



            Enchants[WeaponEnchant.Fire].GetComponent<Button>().interactable = fire;
            Enchants[WeaponEnchant.Lightning].GetComponent<Button>().interactable = lightning;
            Enchants[WeaponEnchant.Spirit].GetComponent<Button>().interactable = spirit;
            Enchants[WeaponEnchant.Poison].GetComponent<Button>().interactable = poison;
            Enchants[WeaponEnchant.Frost].GetComponent<Button>().interactable = frost;
        }

        public static void GetEnchantColors(WeaponEnchant enchant, ref string coloredText)
        {
            string start = "";
            string end = CFG.ColorEnd;
            switch (enchant)
            {
                case WeaponEnchant.None:
                    start = ""; end = "";
                    break;
                case WeaponEnchant.Fire:
                    start = CFG.ColorFireFF;
                    break;
                case WeaponEnchant.Lightning:
                    start = CFG.ColorLightningFF;
                    break;
                case WeaponEnchant.Spirit:
                    start = CFG.ColorSpiritFF;
                    break;
                case WeaponEnchant.Poison:
                    start = CFG.ColorPoisonFF;
                    break;
                case WeaponEnchant.Frost:
                    start = CFG.ColorFrostFF;
                    break;
            }
            coloredText = start + coloredText + end;
        }

        public static void UpdateShouldDisplay()
        {
            if (!Window) Init();
            UpdateLearnedEnchants();
            bool display = false;
            if (learnedAtLeastOne)
                display = true;
            else
                SetActive(WeaponEnchant.None);
                
            //This is the button for the weapon enchant. I really ought to have a better way to do this
            //eww magic numbers
            SkillGUI.LPSetButtons[2].SetActive(display);
        }

        public static void OnFire() => SetActive(WeaponEnchant.Fire);
        public static void OnLightning() => SetActive(WeaponEnchant.Lightning);
        public static void OnSpirit() => SetActive(WeaponEnchant.Spirit);
        public static void OnPoison() => SetActive(WeaponEnchant.Poison);
        public static void OnFrost() => SetActive(WeaponEnchant.Frost);
        public static void OnNone() => SetActive(WeaponEnchant.None);

        public static void Init()
        {
            Window = GUIManager.Instance.CreateWoodpanel(
                       parent: GUIManager.CustomGUIFront.transform,
                       anchorMin: new Vector2(0.5f, 0.5f),
                       anchorMax: new Vector2(0.5f, 0.5f),
                       position: new Vector2(0f, 0),
                       width: 600,
                       height: 600,
                       draggable: true);
            Window.SetActive(false);

            Title = GUIManager.Instance.CreateText(
                text: "Weapon Enchants",
                parent: Window.transform,
                anchorMin: new Vector2(0.5f, 1f),
                anchorMax: new Vector2(0.5f, 1f),
                position: new Vector2(0f, -70f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 45,
                color: CFG.ColorTitle,
                outline: true,
                outlineColor: Color.black,
                width: 600f,
                height: 80f,
                addContentSizeFitter: false);
            Title.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

            GameObject obj = GUIManager.Instance.CreateButton(
                text: "Exit",
                parent: Window.transform,
                anchorMin: new Vector2(1f, 1f),
                anchorMax: new Vector2(1f, 1f),
                position: new Vector2(-60f, -70f),
                width: 70f,
                height: 50f);

            Button button = obj.GetComponent<Button>();
            button.onClick.AddListener(Close);


            ActiveEnchant = GUIManager.Instance.CreateText(
                text: "",
                parent: Window.transform,
                anchorMin: new Vector2(0.5f, 1f),
                anchorMax: new Vector2(0.5f, 1f),
                position: new Vector2(0f, -160f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 40,
                color: CFG.ColorTitle,
                outline: true,
                outlineColor: Color.black,
                width: 600f,
                height: 80f,
                addContentSizeFitter: false);
            ActiveEnchant.GetComponent<Text>().alignment = TextAnchor.UpperCenter;

            Enchants = new Dictionary<WeaponEnchant, GameObject>();

            obj = GUIManager.Instance.CreateButton(
                text: "Fire",
                parent: Window.transform,
                anchorMin: new Vector2(0.5f, 0f),
                anchorMax: new Vector2(0.5f, 0f),
                position: new Vector2(-150f, 330),
                width: 200f,
                height: 100f);

            button = obj.GetComponent<Button>();
            button.onClick.AddListener(OnFire);
            button.interactable = false;
            Enchants.Add(WeaponEnchant.Fire, obj);

            obj = GUIManager.Instance.CreateButton(
                text: "Lightning",
                parent: Window.transform,
                anchorMin: new Vector2(0.5f, 0f),
                anchorMax: new Vector2(0.5f, 0f),
                position: new Vector2(-150f, 220),
                width: 200f,
                height: 100f);

            button = obj.GetComponent<Button>();
            button.onClick.AddListener(OnLightning);
            button.interactable = false;
            Enchants.Add(WeaponEnchant.Lightning, obj);

            obj = GUIManager.Instance.CreateButton(
                text: "Spirit",
                parent: Window.transform,
                anchorMin: new Vector2(0.5f, 0f),
                anchorMax: new Vector2(0.5f, 0f),
                position: new Vector2(-150f, 110),
                width: 200f,
                height: 100f);

            button = obj.GetComponent<Button>();
            button.onClick.AddListener(OnSpirit);
            button.interactable = false;
            Enchants.Add(WeaponEnchant.Spirit, obj);

            obj = GUIManager.Instance.CreateButton(
                text: "Poison",
                parent: Window.transform,
                anchorMin: new Vector2(0.5f, 0f),
                anchorMax: new Vector2(0.5f, 0f),
                position: new Vector2(150f, 330),
                width: 200f,
                height: 100f);

            button = obj.GetComponent<Button>();
            button.onClick.AddListener(OnPoison);
            button.interactable = false;
            Enchants.Add(WeaponEnchant.Poison, obj);

            obj = GUIManager.Instance.CreateButton(
                text: "Frost",
                parent: Window.transform,
                anchorMin: new Vector2(0.5f, 0f),
                anchorMax: new Vector2(0.5f, 0f),
                position: new Vector2(150f, 220),
                width: 200f,
                height: 100f);

            button = obj.GetComponent<Button>();
            button.onClick.AddListener(OnFrost);
            button.interactable = false;
            Enchants.Add(WeaponEnchant.Frost, obj);

            obj = GUIManager.Instance.CreateButton(
                text: "None",
                parent: Window.transform,
                anchorMin: new Vector2(0.5f, 0f),
                anchorMax: new Vector2(0.5f, 0f),
                position: new Vector2(150f, 110),
                width: 200f,
                height: 100f);

            button = obj.GetComponent<Button>();
            button.onClick.AddListener(OnNone);
            button.interactable = true;
            Enchants.Add(WeaponEnchant.None, obj);

            SetActive(loadedEnchant);
        }


        public static void LoadWeaponEnchant(int enchant)
        {
            loadedEnchant = (WeaponEnchant)enchant;
        }
    }
}
