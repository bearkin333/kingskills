using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kingskills.Perks
{
    class P_DidntHurt
    {

        public static void CheckStagger(HitData hit)
        {
            if (!PerkMan.IsPerkActive(PerkMan.PerkType.DidntHurt)) return;
            if (hit.GetTotalDamage() > CFG.DidntHurtStaggerThreshold.Value) return;
            if (!hit.HaveAttacker() || hit.GetAttacker() is null) return;
            hit.GetAttacker().Stagger(-hit.m_dir);
        }
    }
}
