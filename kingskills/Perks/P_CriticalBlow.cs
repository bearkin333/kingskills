using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace kingskills.Perks
{
    class P_CriticalBlow
    {
        public static void ApplyDamage(ref HitData hit)
        {
            if (!CFG.CheckPlayerActivePerk(Player.m_localPlayer, PerkMan.PerkType.CriticalBlow)) return;
            if (!CFG.GetIsCriticalBlow()) return;
            hit.m_damage.Modify(CFG.GetCriticalBlowDamageMult());
            CustomWorldTextManager.AddCustomWorldText(CFG.ColorPTRed, hit.m_point + Vector3.up * 1.5f, CFG.CriticalBlowFontSize.Value,
                "CRIT!!");
        }
    }
}
