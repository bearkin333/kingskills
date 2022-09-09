using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace kingskills.UX
{
    [HarmonyPatch(typeof(Player),nameof(Player.OnSpawned))]
    class Tutorials
    {
        [HarmonyPostfix]
        public static void AddKingSkillsTutorials(Player __instance)
        {
            Tutorial.TutorialText intro = new Tutorial.TutorialText
            {
                m_label = "King's Skills - Intro",
                m_name = "kingskills_intro",
                m_text = $"Welcome, brave warrior of Midgard, to your new life in Valheim! Though you've lost the skills you had " +
                $"in your previous life, I have no doubt with a little bit of training they will all come back to you." +
                $"\n\nIn the meantime, you can press 'I' to open up the King's Skills window, where you can learn everything you need " +
                $"to know about how to get them back.",
                m_topic = "King's Skills Intro"
            };
            if (!Tutorial.instance.m_texts.Contains(intro))
            {
                Tutorial.instance.m_texts.Add(intro);
            }


            Tutorial.TutorialText perks = new Tutorial.TutorialText
            {
                m_label = "King's Skills - Perks",
                m_name = "kingskills_perks",
                m_text = $"Congratulations! You've hit a high enough level that you've gained the ability to unlock one of two " +
                $"powerful new <i>perks</i>. \n\nNavigate to the King's Skills window, and look carefully at your options! You only get " +
                $"one... for now.",
                m_topic = "King's Skills Perks"
            };
            if (!Tutorial.instance.m_texts.Contains(perks))
            {
                Tutorial.instance.m_texts.Add(perks);
            }

            Tutorial.TutorialText ascend = new Tutorial.TutorialText
            {
                m_label = "King's Skills - Ascension",
                m_name = "kingskills_ascend",
                m_text = $"What's this? What {CFG.ColorPTRedFF}power{CFG.ColorEnd} you emanate! You must have fully realized one of your skills from your " +
                $"incredible life. I am beyond impressed. However, your growth isn't over.. " +
                $"\n\nIf you wish it, you can now choose to ascend. An ascended skill will reset to level 0, losing all of your " +
                $"earned bonuses {CFG.ColorExperienceYellowFF}EXCEPT{CFG.ColorEnd} any perks you have learned. " +
                $"Hitting the perk threshold again will allow you to learn the other perk as well." +
                $"\n\nWhat say you, o exalted spirit?",
                m_topic = "King's Skills Ascension"
            };
            if (!Tutorial.instance.m_texts.Contains(ascend))
            {
                Tutorial.instance.m_texts.Add(ascend);
            }


            __instance.ShowTutorial("kingskills_intro");
        }
    }
}
