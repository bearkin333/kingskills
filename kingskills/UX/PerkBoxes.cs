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
            openedPerks.Clear();
            openedPerks.Add(Perks.perkList[perk1a]);
            openedPerks.Add(Perks.perkList[perk1b]);
            openedPerks.Add(Perks.perkList[perk2a]);
            openedPerks.Add(Perks.perkList[perk2b]);

            SetPerkBox(openedPerks[0], "1aPerk");
            SetPerkBox(openedPerks[1], "1bPerk");
            SetPerkBox(openedPerks[2], "2aPerk");
            SetPerkBox(openedPerks[3], "2bPerk");
        }

        public static void SetPerkBox(Perk perk, string perkBoxString)
        {
            GameObject perkBox = SkillGUI.RightPanelPerkBoxes[perkBoxString];
            if (perk.learned)
            {

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
