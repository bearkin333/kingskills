using kingskills.UX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kingskills
{
    class AscensionManager
    {
        public static Dictionary<Skills.SkillType, bool> isAscendable;

        public static void InitAscendables()
        {
            Init();
            foreach (KeyValuePair<Skills.SkillType, bool> ascendedSkill in Perks.skillAscendedFlags)
            {
                isAscendable.Add(ascendedSkill.Key, false);
            }
        }

        public static void Init()
        {
            isAscendable = new Dictionary<Skills.SkillType, bool>();
        }


        public static void OnAscendButton()
        {
            if (IsAscendable(OpenPerks.openSkill))
                AscendConfirmGUI.OpenAscendWindow();
        }

        public static void Ascend()
        {
            Skills.SkillType skill = AscendConfirmGUI.skill;

            if (Perks.skillAscendedFlags.ContainsKey(skill))
                Perks.skillAscendedFlags[skill] = true;
            else
                Perks.skillAscendedFlags.Add(skill, true);

            if (isAscendable.ContainsKey(skill))
                isAscendable[skill] = false;
            else
                isAscendable.Add(skill, false);

            Player.m_localPlayer.GetSkills().ResetSkill(skill);
            SkillGUIUpdate.GUICheck();
        }

        public static bool IsAscendable(Skills.SkillType skill)
        {
            if (isAscendable.ContainsKey(OpenPerks.openSkill))
                return isAscendable[OpenPerks.openSkill];

            return false;
        }
    }
}
