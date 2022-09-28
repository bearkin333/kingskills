using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace kingskills.Perks
{
    class P_ClosingTheGap
    {
        public static void GapClose(Character enemy, Player player)
        {
            if (!CFG.CheckPlayerActivePerk(player, PerkMan.PerkType.ClosingTheGap)) return;

            player.AddStamina(CFG.ClosingTheGapStaminaRegain.Value);

            Vector3 direction = -Vector3.Normalize(player.transform.position - enemy.transform.position);
            HitData push = new HitData();
            push.m_dir = direction;
            push.m_pushForce = CFG.ClosingTheGapForce.Value;

            player.ApplyPushback(push);
        }
    }
}
