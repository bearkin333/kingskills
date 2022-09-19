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
        public static Skills.SkillType openSkill;

        public static void Init()
        {
            openedPerks = new List<Perk>();
            perkLocked = new List<bool>();
            perkClickable = new List<bool>();
            skillLevel = 0;
            openSkill = Skills.SkillType.None;
        }


        public static void UpdateOpenPerks()
        {
            if (openedPerks == null) return;
            if (SkillGUI.SkillGUIWindow == null) return;
            if (!SkillGUI.SkillGUIWindow.activeSelf) return;

            //Jotunn.Logger.LogMessage("eeeee");
            if (LearnConfirmationGUI.IsConfirmOpen() || !SkillGUIUpdate.closable)
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


        public static void LearnPerk(int perk)
        {
            if (!openedPerks[perk].learnable) return;
            if (perk == NoPerk) return;

            openedPerks[perk].learned = true;
            openedPerks[perk].learnable = false;
            //Jotunn.Logger.LogMessage($"{openedPerks[perk].name} just got learned, making it now unlearnable.");
            Perks.perkFlags.Add(openedPerks[perk].type, true);

            UpdateLearnables();
            SkillGUIUpdate.GUICheck();
        }


        public static void UpdateLearnables()
        {
            if (openedPerks == null) return;
            bool ascended = Perks.IsSkillAscended(openSkill);


            if (openedPerks[0].learned)
            {
                if (!ascended)
                    openedPerks[1].learnable = false;
                else
                    openedPerks[1].learnable = true;
                //Jotunn.Logger.LogMessage($"just set perk {openedPerks[1].name} to unlearnable, since the other one is learned");
            }

            else if (openedPerks[1].learned)
            {
                if (!ascended)
                    openedPerks[0].learnable = false;
                else
                    openedPerks[0].learnable = true;
                //Jotunn.Logger.LogMessage($"just set perk {openedPerks[0].name} to unlearnable, since the other one is learned");
            }

            if (openedPerks[2].learned)
            {
                if (!ascended)
                    openedPerks[3].learnable = false;
                else
                    openedPerks[3].learnable = true;
                //Jotunn.Logger.LogMessage($"just set perk {openedPerks[3].name} to unlearnable, since the other one is learned");
            }

            else if (openedPerks[3].learned)
            {
                if (!ascended)
                    openedPerks[2].learnable = false;
                else
                    openedPerks[2].learnable = true;
                //Jotunn.Logger.LogMessage($"just set perk {openedPerks[2].name} to unlearnable, since the other one is learned");
            }

            if (openedPerks[4].learned)
            {
                if (!ascended)
                    openedPerks[5].learnable = false;
                else
                    openedPerks[5].learnable = true;
                //Jotunn.Logger.LogMessage($"just set perk {openedPerks[3].name} to unlearnable, since the other one is learned");
            }

            else if (openedPerks[5].learned)
            {
                if (!ascended)
                    openedPerks[4].learnable = false;
                else
                    openedPerks[4].learnable = true;
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
                    if (result.gameObject == SkillGUI.RPPerkBoxes["1a"])
                        return GetPerkKeyFromBox(0);
                    else if (result.gameObject == SkillGUI.RPPerkBoxes["1b"])
                        return GetPerkKeyFromBox(1);
                    else if (result.gameObject == SkillGUI.RPPerkBoxes["2a"])
                        return GetPerkKeyFromBox(2);
                    else if (result.gameObject == SkillGUI.RPPerkBoxes["2b"])
                        return GetPerkKeyFromBox(3);
                    else if (result.gameObject == SkillGUI.RPPerkBoxes["3a"])
                        return GetPerkKeyFromBox(4);
                    else if (result.gameObject == SkillGUI.RPPerkBoxes["3b"])
                        return GetPerkKeyFromBox(5);
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
            Perks.PerkType perk2a, Perks.PerkType perk2b,
            Perks.PerkType perk3a, Perks.PerkType perk3b)
        {
            openSkill = skill;
            skillLevel = Player.m_localPlayer.GetSkillFactor(openSkill);
            if (openedPerks == null) Init();

            openedPerks.Clear();
            openedPerks.Add(Perks.perkList[perk1a]);
            openedPerks.Add(Perks.perkList[perk1b]);
            openedPerks.Add(Perks.perkList[perk2a]);
            openedPerks.Add(Perks.perkList[perk2b]);
            openedPerks.Add(Perks.perkList[perk3a]);
            openedPerks.Add(Perks.perkList[perk3b]);

            perkClickable.Clear();
            perkClickable.Add(true);
            perkClickable.Add(true);
            perkClickable.Add(true);
            perkClickable.Add(true);
            perkClickable.Add(true);
            perkClickable.Add(true);

            perkLocked.Clear();
            perkLocked.Add(false);
            perkLocked.Add(false);
            perkLocked.Add(false);
            perkLocked.Add(false);
            perkLocked.Add(false);
            perkLocked.Add(false);

            UpdateLearnables();

            SetPerkBox(0, "1a", CFG.PerkOneLVLThreshold.Value);
            SetPerkBox(1, "1b", CFG.PerkOneLVLThreshold.Value);
            SetPerkBox(2, "2a", CFG.PerkTwoLVLThreshold.Value);
            SetPerkBox(3, "2b", CFG.PerkTwoLVLThreshold.Value);
            SetPerkBox(4, "3a", CFG.PerkThreeLVLThreshold.Value);
            SetPerkBox(5, "3b", CFG.PerkThreeLVLThreshold.Value);
        }


        public static void SetPerkBox(int perk, string perkBoxString, float skillThreshold)
        {
            GameObject perkSprite = SkillGUI.RPPerkBoxes[perkBoxString + "Perk"];
            Image perkImage = perkSprite.GetComponent<Image>();
            GameObject tint = SkillGUI.RPPerkBoxes[perkBoxString + "Tint"];
            Image tintImg = tint.GetComponent<Image>();

            if (Perks.IsSkillAscended(openSkill) && openedPerks[perk].learned)
            {
                perkClickable[perk] = false;
                tintImg.enabled = false;
                perkImage.sprite = openedPerks[perk].icon;
                perkImage.enabled = true;

                SkillGUI.RPPerkBoxes[perkBoxString].GetComponent<Image>().sprite =
                    Assets.AssetLoader.perkBoxSprites["perkbox"];
            }
            else if (Mathf.Floor(skillLevel * CFG.MaxSkillLevel.Value) < 
                Mathf.Floor(skillThreshold * CFG.MaxSkillLevel.Value))
            {
                //Jotunn.Logger.LogMessage($"too low. setting locked for perk {perk}");
                //If the player isn't high enough level to see them, they are always locked
                perkLocked[perk] = true;

                //Which means we don't get to see the perk's sprites themselves
                perkImage.enabled = false;
                tintImg.enabled = false;

                //And the parent perkbox will also show as locked
                SkillGUI.RPPerkBoxes[perkBoxString].GetComponent<Image>().sprite =
                    Assets.AssetLoader.perkBoxSprites["perkboxLocked"];
            }
            else
            {
                Player.m_localPlayer.ShowTutorial("kingskills_perks");

                //Jotunn.Logger.LogMessage($"good enough. unlocked perk {perk}");
                //If the player is high enough, they aren't locked, 
                perkLocked[perk] = false;

                //and you get to see the icon
                perkImage.sprite = openedPerks[perk].icon;
                perkImage.enabled = true;


                SkillGUI.RPPerkBoxes[perkBoxString].GetComponent<Image>().sprite =
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
    }
}
