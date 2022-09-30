using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kingskills.Perks
{
    class P_DisarmingDefense
    {
        public static bool CheckDisarm(ItemDrop.ItemData weapon)
        {
            return PerkMan.IsPerkActive(PerkMan.PerkType.DisarmingDefense) && weapon.m_shared.m_skillType == Skills.SkillType.Knives;
        }
    }
}
