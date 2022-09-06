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

    public class OpenPerks
    {
        public const int NoPerk = 20;

        public static List<Perk> openedPerks;
        public static List<bool> perkLocked;
        public static List<bool> perkClickable;
        public static float skillLevel;

        public static void UpdateOpenPerks()
        {
            if (openedPerks == null) return;
            if (SkillGUI.SkillGUIWindow == null) return;
            if (!SkillGUI.SkillGUIWindow.activeSelf) return;

            //Jotunn.Logger.LogMessage("eeeee");
            if (LearnConfirmationGUI.IsConfirmOpen())
                return;

            int hoveredPerkKey = MouseOverPerkBox();
            if (hoveredPerkKey == NoPerk)
            {
                PerkTooltipGUI.CloseTooltip();
                return;
            }

            PerkTooltipGUI.UpdateTooltip(openedPerks[hoveredPerkKey]);

            //and then check for learning
            if (perkClickable[hoveredPerkKey] && Input.GetMouseButtonDown(0))
            {
                LearnConfirmationGUI.OpenLearnConfirmation(hoveredPerkKey);
            }
        }

        public static void Init()
        {
            openedPerks = new List<Perk>();
            perkLocked = new List<bool>();
            perkClickable = new List<bool>();
            skillLevel = 0;
        }


        public static void LearnPerk(int perk)
        {
            if (!openedPerks[perk].learnable) return;
            if (perk == NoPerk) return;

            openedPerks[perk].learned = true;
            openedPerks[perk].learnable = false;
            //Jotunn.Logger.LogMessage($"{openedPerks[perk].name} just got learned, making it now unlearnable.");
            Perks.perkFlags.Add(openedPerks[perk].type, true);

            UpdateLearnables();
            UpdateGUI.GUICheck();
        }

        public static void UpdateLearnables()
        {
            if (openedPerks == null) return;


            if (openedPerks[0].learned)
            {
                openedPerks[1].learnable = false;
                //Jotunn.Logger.LogMessage($"just set perk {openedPerks[1].name} to unlearnable, since the other one is learned");
            }

            else if (openedPerks[1].learned)
            {
                openedPerks[0].learnable = false;
                //Jotunn.Logger.LogMessage($"just set perk {openedPerks[0].name} to unlearnable, since the other one is learned");
            }

            if (openedPerks[2].learned)
            {
                openedPerks[3].learnable = false;
                //Jotunn.Logger.LogMessage($"just set perk {openedPerks[3].name} to unlearnable, since the other one is learned");
            }

            else if (openedPerks[3].learned)
            {
                openedPerks[2].learnable = false;
                //Jotunn.Logger.LogMessage($"just set perk {openedPerks[2].name} to unlearnable, since the other one is learned");
            }

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
            if (openedPerks == null) Init();

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
            SetPerkBox(2, "2a", ConfigManager.PerkTwoLVLThreshold.Value);
            SetPerkBox(3, "2b", ConfigManager.PerkTwoLVLThreshold.Value);

            UpdateLearnables();
        }

        public static void SetPerkBox(int perk, string perkBoxString, float skillThreshold)
        {
            GameObject perkSprite = SkillGUI.RightPanelPerkBoxes[perkBoxString + "Perk"];
            Image perkImage = perkSprite.GetComponent<Image>();
            GameObject tint = SkillGUI.RightPanelPerkBoxes[perkBoxString + "Tint"];
            Image tintImg = tint.GetComponent<Image>();

            //Jotunn.Logger.LogMessage($"skill vs threshold: {skillLevel} < {skillThreshold}");
            if (skillLevel < skillThreshold)
            {
                //Jotunn.Logger.LogMessage($"too low. setting locked for perk {perk}");
                //If the player isn't high enough level to see them, they are always locked
                perkLocked[perk] = true;

                //Which means we don't get to see the perk's sprites themselves
                perkImage.enabled = false;
                tintImg.enabled = false;

                //And the parent perkbox will also show as locked
                SkillGUI.RightPanelPerkBoxes[perkBoxString].GetComponent<Image>().sprite =
                    Assets.AssetLoader.perkBoxSprites["perkboxLocked"];
            }
            else
            {
                //Jotunn.Logger.LogMessage($"good enough. unlocked perk {perk}");
                //If the player is high enough, they aren't locked, 
                perkLocked[perk] = false;

                //and you get to see the icon
                perkImage.sprite = openedPerks[perk].icon;
                perkImage.enabled = true;


                SkillGUI.RightPanelPerkBoxes[perkBoxString].GetComponent<Image>().sprite =
                    Assets.AssetLoader.perkBoxSprites["perkbox"];

                //If it's learned, it has no tint, and isn't clickable.
                if (openedPerks[perk].learned)
                {
                    //Jotunn.Logger.LogMessage($"{perk} is learned. making gold");

                    perkClickable[perk] = false;
                    tintImg.enabled = false;
                }
                //If it's learnable, and not learned, it has a gold tint, and it's clickable.
                else if (openedPerks[perk].learnable)
                {
                    //Jotunn.Logger.LogMessage($"{perk} is learnable. making clear");

                    perkClickable[perk] = true;
                    tintImg.sprite = Assets.AssetLoader.perkBoxSprites["goldtint"];
                    tintImg.enabled = true;
                }
                //Otherwise, neither learnable nor learned, it has a gray tint and isn't clickable.
                else
                {
                    //Jotunn.Logger.LogMessage($"{perk} isn't learned or learnable. making gray");

                    perkClickable[perk] = false;
                    tintImg.sprite = Assets.AssetLoader.perkBoxSprites["graytint"];
                    tintImg.enabled = true;
                }
            }
        }


        public static void OpenAgriculturePerkBoxes()
        {
            OpenPerksByType(SkillMan.Agriculture,
                Perks.PerkType.Decapitation,
                Perks.PerkType.Berserkr,
                Perks.PerkType.Highlander,
                Perks.PerkType.Throwback);
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
        public static void OpenBuildingPerkBoxes()
        {
            OpenPerksByType(SkillMan.Building,
                Perks.PerkType.Decapitation,
                Perks.PerkType.Berserkr,
                Perks.PerkType.Highlander,
                Perks.PerkType.Throwback);
        }
        public static void OpenClubPerkBoxes()
        {
            OpenPerksByType(Skills.SkillType.Clubs,
                Perks.PerkType.ClosingTheGap,
                Perks.PerkType.ThunderHammer,
                Perks.PerkType.TrollSmash,
                Perks.PerkType.PlusUltra);
        }
        public static void OpenCookingPerkBoxes()
        {
            OpenPerksByType(SkillMan.Cooking,
                Perks.PerkType.Decapitation,
                Perks.PerkType.Berserkr,
                Perks.PerkType.Highlander,
                Perks.PerkType.Throwback);
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
        public static void OpenSailingPerkBoxes()
        {
            OpenPerksByType(SkillMan.Sailing,
                Perks.PerkType.Decapitation,
                Perks.PerkType.Berserkr,
                Perks.PerkType.Highlander,
                Perks.PerkType.Throwback);
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
                Perks.PerkType.Treading,
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
