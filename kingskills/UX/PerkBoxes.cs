using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Jotunn.Managers;

namespace kingskills.UX
{

    public class PerkBoxes : MonoBehaviour
    {
        public const int NoPerk = 20;

        public static List<Perk> openedPerks;
        public static List<bool> perkLocked;
        public static List<bool> perkClickable;
        public static float skillLevel;

        public static int confirmPerkKey;

        public void Awake()
        {
            openedPerks = new List<Perk>();
            perkLocked = new List<bool>();
            perkClickable = new List<bool>();
            skillLevel = 0;
            confirmPerkKey = NoPerk;
        }

        public void Update()
        {
            if (openedPerks.Count < 1) return;

            int hoveredPerkKey = MouseOverPerkBox();
            if (hoveredPerkKey == NoPerk) return;

            //create tooltip stuff here


            //and then check for learning
            if (perkClickable[hoveredPerkKey] && Input.GetMouseButtonDown(0))
            {
                OpenLearnConfirmation(hoveredPerkKey);
            }
        }

        public void OpenLearnConfirmation(int confirmPerk)
        {
            if (LearnConfirmWindow == null) AwakeLearnConfirmation();

            LearnConfirmWindow.SetActive(true);
            GUIManager.BlockInput(true);
            confirmPerkKey = confirmPerk;

            LCPerkTitle.GetComponent<Text>().text = openedPerks[confirmPerk].name;
            LCPerkDescription.GetComponent<Text>().text = openedPerks[confirmPerk].description;
        }

        public static GameObject LearnConfirmWindow;
        public static GameObject LCPerkTitle;
        public static GameObject LCPerkDescription;
        public static GameObject LCYesBtn;
        public static GameObject LCNoBtn;

        public void AwakeLearnConfirmation()
        {
            LearnConfirmWindow = GUIManager.Instance.CreateWoodpanel(
                    parent: GUIManager.CustomGUIFront.transform,
                    anchorMin: new Vector2(0.5f, 0.5f),
                    anchorMax: new Vector2(0.5f, 0.5f),
                    position: new Vector2(0f, 0),
                    width: 400,
                    height: 300,
                    draggable: true);
            LearnConfirmWindow.SetActive(false);


            GameObject obj = GUIManager.Instance.CreateText(
                text: "Are you sure you want to learn",
                parent: LearnConfirmWindow.transform,
                anchorMin: new Vector2(0.5f, 1f),
                anchorMax: new Vector2(0.5f, 1f),
                position: new Vector2(0f, -75f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 40,
                color: GUIManager.Instance.ValheimOrange,
                outline: true,
                outlineColor: Color.black,
                width: 400f,
                height: 80f,
                addContentSizeFitter: false);
            obj.GetComponent<Text>().alignment = TextAnchor.UpperCenter;


            LCPerkTitle = GUIManager.Instance.CreateText(
                text: "",
                parent: LearnConfirmWindow.transform,
                anchorMin: new Vector2(0.5f, 1f),
                anchorMax: new Vector2(0.5f, 1f),
                position: new Vector2(0f, -140f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 30,
                color: GUIManager.Instance.ValheimOrange,
                outline: true,
                outlineColor: Color.black,
                width: 400f,
                height: 80f,
                addContentSizeFitter: false);
            LCPerkTitle.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;


            LCPerkDescription = GUIManager.Instance.CreateText(
                text: "",
                parent: LearnConfirmWindow.transform,
                anchorMin: new Vector2(0.5f, 1f),
                anchorMax: new Vector2(0.5f, 1f),
                position: new Vector2(0f, -200f),
                font: GUIManager.Instance.AveriaSerifBold,
                fontSize: 25,
                color: GUIManager.Instance.ValheimOrange,
                outline: true,
                outlineColor: Color.black,
                width: 400f,
                height: 80f,
                addContentSizeFitter: false);
            Text text = LCPerkDescription.GetComponent<Text>();
            text.alignment = TextAnchor.MiddleCenter;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;


            LCYesBtn = GUIManager.Instance.CreateButton(
                text: "Yes",
                parent: LearnConfirmWindow.transform,
                anchorMin: new Vector2(0.5f, 0f),
                anchorMax: new Vector2(0.5f, 0f),
                position: new Vector2(-150f, 30f),
                width: 80f,
                height: 45f);
            Button btn = LCYesBtn.GetComponent<Button>();
            btn.onClick.AddListener(OnConfirmClick);

            LCNoBtn = GUIManager.Instance.CreateButton(
                text: "No",
                parent: LearnConfirmWindow.transform,
                anchorMin: new Vector2(0.5f, 0f),
                anchorMax: new Vector2(0.5f, 0f),
                position: new Vector2(150f, 30f),
                width: 80f,
                height: 45f);
            btn = LCNoBtn.GetComponent<Button>();
            btn.onClick.AddListener(CloseLearnConfirmWindow);
        }

        public void OnConfirmClick()
        {
            LearnPerk(confirmPerkKey);
            CloseLearnConfirmWindow();
        }

        public void CloseLearnConfirmWindow()
        {
            LearnConfirmWindow.SetActive(false);
            GUIManager.BlockInput(false);

            confirmPerkKey = NoPerk;
        }
            
        public void LearnPerk(int perk)
        {
            if (!openedPerks[perk].learnable) return;
            if (perk == NoPerk) return;

            openedPerks[perk].learned = true;
            openedPerks[perk].learnable = false;
            Perks.perkFlags.Add(openedPerks[perk].type, true);

            UpdateLearnables();
            GUIUpdate.GUICheck();
        }

        public static void UpdateLearnables()
        {
            if (openedPerks.Count < 1) return;


            if (openedPerks[0].learned)
                openedPerks[1].learnable = false;

            else if (openedPerks[1].learned)
                openedPerks[0].learnable = false;

            if (openedPerks[2].learned)
                openedPerks[3].learnable = false;

            else if (openedPerks[3].learned)
                openedPerks[2].learnable = false;

        }


        public static int MouseOverPerkBox()
        {
            PointerEventData pointer = new PointerEventData(EventSystem.current);
            pointer.position = Input.mousePosition;

            List<RaycastResult> raycastResultList = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, raycastResultList);
            foreach (RaycastResult result in raycastResultList)
            {
                if (result.gameObject.GetComponent<IsPerkBox>() != null)
                {
                    if (result.gameObject == SkillGUI.RightPanelPerkBoxes["1a"])
                        return GetPerkKeyFromBox(0);
                    else if (result.gameObject == SkillGUI.RightPanelPerkBoxes["1b"])
                        return GetPerkKeyFromBox(1);
                    else if (result.gameObject == SkillGUI.RightPanelPerkBoxes["2a"])
                        return GetPerkKeyFromBox(2);
                    else if (result.gameObject == SkillGUI.RightPanelPerkBoxes["2b"])
                        return GetPerkKeyFromBox(3);
                }
            }

            return NoPerk;
        }

        public static int GetPerkKeyFromBox(int boxKey)
        {
            if (perkLocked[boxKey]) return NoPerk;

            return boxKey;
        }

        public static void OpenPerksByType(Skills.SkillType skill,
            Perks.PerkType perk1a, Perks.PerkType perk1b,
            Perks.PerkType perk2a, Perks.PerkType perk2b)
        {
            skillLevel = Player.m_localPlayer.GetSkillFactor(skill);

            openedPerks.Clear();
            openedPerks.Add(Perks.perkList[perk1a]);
            openedPerks.Add(Perks.perkList[perk1b]);
            openedPerks.Add(Perks.perkList[perk2a]);
            openedPerks.Add(Perks.perkList[perk2b]);

            perkClickable.Clear();
            perkClickable.Add(true);
            perkClickable.Add(true);
            perkClickable.Add(true);
            perkClickable.Add(true);

            perkLocked.Clear();
            perkLocked.Add(false);
            perkLocked.Add(false);
            perkLocked.Add(false);
            perkLocked.Add(false);

            SetPerkBox(0, "1a", ConfigManager.PerkOneLVLThreshold.Value);
            SetPerkBox(1, "1b", ConfigManager.PerkOneLVLThreshold.Value);
            SetPerkBox(2, "2a", ConfigManager.PerkOneLVLThreshold.Value);
            SetPerkBox(3, "2b", ConfigManager.PerkOneLVLThreshold.Value);

            UpdateLearnables();
        }

        public static void SetPerkBox(int perk, string perkBoxString, float skillThreshold)
        {
            GameObject perkBoxA = SkillGUI.RightPanelPerkBoxes[perkBoxString + "Perk"];
            Image perkBoxAImage = perkBoxA.GetComponent<Image>();
            GameObject tintA = SkillGUI.RightPanelPerkBoxes[perkBoxString + "Tint"];
            Image tintAimg = tintA.GetComponent<Image>();

            if (skillLevel < skillThreshold)
            {
                //If the player isn't high enough level to see them, they are always locked
                perkLocked[perk] = true;

                //Which means we don't get to see the perk's sprites themselves
                perkBoxAImage.sprite = null;

                //And the parent perkbox will also show as locked
                SkillGUI.RightPanelPerkBoxes[perkBoxString].GetComponent<Image>().sprite =
                    Assets.AssetLoader.perkBoxSprites["perkboxLocked"];
            }
            else
            {
                //If the player is high enough, they aren't locked, 
                perkLocked[perk] = false;

                //and you get to see the icon
                perkBoxAImage.sprite = openedPerks[perk].icon;

                //If it's learned, it has a gold tint, and it's not clickable.
                if (openedPerks[perk].learned) {
                    perkClickable[perk] = false;
                    tintAimg.sprite = Assets.AssetLoader.perkBoxSprites["goldtint"];

                }
                //If it's learnable, and not learned, it has no tint, and it's clickable.
                else if (openedPerks[perk].learnable)
                {
                    perkClickable[perk] = true;
                    tintAimg.sprite = null;
                }
                //Otherwise, neither learnable nor learned, it has a gray tint and isn't clickable.
                else
                {
                    perkClickable[perk] = false;
                    tintAimg.sprite = Assets.AssetLoader.perkBoxSprites["graytint"];
                }
            }
        }


        public static void OpenAxePerkBoxes()
        {
            OpenPerksByType(Skills.SkillType.Axes,
                Perks.PerkType.Decapitation, 
                Perks.PerkType.Berserkr, 
                Perks.PerkType.Highlander, 
                Perks.PerkType.Throwback);
        }
        public static void OpenBlockPerkBoxes()
        {
            OpenPerksByType(Skills.SkillType.Blocking,
                Perks.PerkType.TitanEndurance,
                Perks.PerkType.SpikedShield,
                Perks.PerkType.TitanStrength,
                Perks.PerkType.BlackFlash);
        }
        public static void OpenBowPerkBoxes()
        {
            OpenPerksByType(Skills.SkillType.Bows,
                Perks.PerkType.PowerDraw,
                Perks.PerkType.Frugal,
                Perks.PerkType.RunedArrows,
                Perks.PerkType.OfferToUllr);
        }
        public static void OpenClubPerkBoxes()
        {
            OpenPerksByType(Skills.SkillType.Clubs,
                Perks.PerkType.ClosingTheGap,
                Perks.PerkType.ThunderHammer,
                Perks.PerkType.TrollSmash,
                Perks.PerkType.PlusUltra);
        }
        public static void OpenFistPerkBoxes()
        {
            OpenPerksByType(Skills.SkillType.Unarmed,
                Perks.PerkType.IronSkin,
                Perks.PerkType.LightningReflex,
                Perks.PerkType.FalconKick,
                Perks.PerkType.PressurePoints);
        }
        public static void OpenKnifePerkBoxes()
        {
            OpenPerksByType(Skills.SkillType.Knives,
                Perks.PerkType.Deadeye,
                Perks.PerkType.Iai,
                Perks.PerkType.LokisGift,
                Perks.PerkType.DisarmingDefense);
        }
        public static void OpenJumpPerkBoxes()
        {
            OpenPerksByType(Skills.SkillType.Jump,
                Perks.PerkType.GoombaStomp,
                Perks.PerkType.MarketGardener,
                Perks.PerkType.MeteorDrop,
                Perks.PerkType.OdinJump);
        }
        public static void OpenMiningPerkBoxes()
        {
            OpenPerksByType(Skills.SkillType.Pickaxes,
                Perks.PerkType.TrenchDigger,
                Perks.PerkType.Stretch,
                Perks.PerkType.RockHauler,
                Perks.PerkType.LodeBearingStone);
        }
        public static void OpenPolearmPerkBoxes()
        {
            OpenPerksByType(Skills.SkillType.Polearms,
                Perks.PerkType.Jotunn,
                Perks.PerkType.LivingStone,
                Perks.PerkType.BigStick,
                Perks.PerkType.Asguard);
        }
        public static void OpenRunPerkBoxes()
        {
            OpenPerksByType(Skills.SkillType.Run,
                Perks.PerkType.Tackle,
                Perks.PerkType.HermesBoots,
                Perks.PerkType.WaterRunning,
                Perks.PerkType.Juggernaut);
        }
        public static void OpenSpearPerkBoxes()
        {
            OpenPerksByType(Skills.SkillType.Spears,
                Perks.PerkType.ValkyriesBoon,
                Perks.PerkType.Boomerang,
                Perks.PerkType.CouchedLance,
                Perks.PerkType.EinherjarsBlessing);
        }
        public static void OpenSneakPerkBoxes()
        {
            OpenPerksByType(Skills.SkillType.Sneak,
                Perks.PerkType.SmokeBomb,
                Perks.PerkType.SilentSprinter,
                Perks.PerkType.HideInPlainSight,
                Perks.PerkType.CloakOfShadows);
        }
        public static void OpenSwimPerkBoxes()
        {
            OpenPerksByType(Skills.SkillType.Swim,
                Perks.PerkType.SeaLegs,
                Perks.PerkType.Butterfly,
                Perks.PerkType.AlwaysPrepared,
                Perks.PerkType.Aerodynamic);
        }
        public static void OpenSwordPerkBoxes()
        {
            OpenPerksByType(Skills.SkillType.Swords,
                Perks.PerkType.PerfectCombo,
                Perks.PerkType.Meditation,
                Perks.PerkType.GodSlayingStrike,
                Perks.PerkType.WarriorOfLight);
        }
        public static void OpenWoodcuttingPerkBoxes()
        {
            OpenPerksByType(Skills.SkillType.WoodCutting,
                Perks.PerkType.HeartOfTheForest,
                Perks.PerkType.MasterOfTheLog,
                Perks.PerkType.HeartOfTheMonkey,
                Perks.PerkType.PandemoniumSwing);
        }
    }
}
