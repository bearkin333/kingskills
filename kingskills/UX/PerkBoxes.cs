using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace kingskills.UX
{
    public class PerkBoxes
    {
        public static List<Perk> openedPerks = new List<Perk>();

        public static void OpenPerksByType(Skills.SkillType skill,
            Perks.PerkType perk1a, Perks.PerkType perk1b,
            Perks.PerkType perk2a, Perks.PerkType perk2b)
        {
            float skillFactor = Player.m_localPlayer.GetSkillFactor(skill);

            openedPerks.Clear();
            openedPerks.Add(Perks.perkList[perk1a]);
            openedPerks.Add(Perks.perkList[perk1b]);
            openedPerks.Add(Perks.perkList[perk2a]);
            openedPerks.Add(Perks.perkList[perk2b]);

            SetPerkBoxPair(openedPerks[0], openedPerks[1], 
                "1aPerk", "1bPerk",
                skillFactor, ConfigManager.PerkOneLVLThreshold.Value);
            SetPerkBoxPair(openedPerks[2], openedPerks[3],
                "2aPerk", "2bPerk",
                skillFactor, ConfigManager.PerkTwoLVLThreshold.Value);
        }

        public static void SetPerkBoxPair(Perk perkA, Perk perkB, 
            string perkBoxAString, string perkBoxBString,
            float skillLevel, float skillThreshold)
        {
            GameObject perkBoxA = SkillGUI.RightPanelPerkBoxes[perkBoxAString];
            GameObject perkBoxB = SkillGUI.RightPanelPerkBoxes[perkBoxBString];
            Image perkBoxAImage = perkBoxA.GetComponent<Image>();
            Image perkBoxBImage = perkBoxB.GetComponent<Image>();

            if (skillLevel < skillThreshold)
            {
                perkBoxAImage.sprite = Assets.AssetLoader.perkSprites["perkboxLocked"];
                perkBoxBImage.sprite = Assets.AssetLoader.perkSprites["perkboxLocked"];
            }
            else
            {
                perkBoxAImage.sprite = perkA.icon;
                perkBoxBImage.sprite = perkB.icon;

                if (perkA.learned || perkA.learnable)
                {
                    //Jotunn.Logger.LogMessage($"color {ConfigManager.PerkOnTint}");
                    //perkBoxAImage.color = ConfigManager.PerkOnTint;
                    //tinting images is gonna be harder than expected. gonna have to attempt
                    //a different methodology
                }
                else
                {
                    //Jotunn.Logger.LogMessage($"color {ConfigManager.PerkOffTint}");
                    //perkBoxAImage.color = ConfigManager.PerkOffTint;
                }
               
                if (perkB.learned || perkB.learnable)
                {
                    //Jotunn.Logger.LogMessage($"color {ConfigManager.PerkOnTint}");
                    //perkBoxBImage.color = ConfigManager.PerkOnTint;
                }
                else
                {
                    //Jotunn.Logger.LogMessage($"color {ConfigManager.PerkOffTint}");
                    //perkBoxBImage.color = ConfigManager.PerkOffTint;
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

        }
        public static void OpenBowPerkBoxes()
        {

        }
        public static void OpenClubPerkBoxes()
        {

        }
        public static void OpenFistPerkBoxes()
        {

        }
        public static void OpenKnifePerkBoxes()
        {

        }
        public static void OpenJumpPerkBoxes()
        {

        }
        public static void OpenMiningPerkBoxes()
        {

        }
        public static void OpenPolearmPerkBoxes()
        {

        }
        public static void OpenRunPerkBoxes()
        {

        }
        public static void OpenSpearPerkBoxes()
        {

        }
        public static void OpenSneakPerkBoxes()
        {

        }
        public static void OpenSwimPerkBoxes()
        {

        }
        public static void OpenSwordPerkBoxes()
        {

        }
        public static void OpenWoodcuttingPerkBoxes()
        {

        }
    }
}
