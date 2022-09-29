using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using HarmonyLib;
using kingskills.SE;

namespace kingskills.Perks
{
    [HarmonyPatch]
    class P_Berserkr
    {

        [HarmonyPatch(typeof(Player),nameof(Player.OnDamaged))]
        [HarmonyPrefix]
        public static void PlayerDamaged(Player __instance)
        {
            if (__instance == Player.m_localPlayer) OnHitTrigger();
        }

        public static void OnHitTrigger()
        {
            if (!CFG.CheckPlayerActivePerk(Player.m_localPlayer, PerkMan.PerkType.Berserkr)) return;
            SEMan seman = Player.m_localPlayer.GetSEMan();
            if (seman is null) return;

            //This provides the actual logic for the random check. a 10% chance to return true
            if (!CFG.GetBerserkCheck()) return;

            if (seman.HaveStatusEffect(KS_SEMan.ks_BerserkName))
            {
                SE_Berserk buff = (SE_Berserk)seman.GetStatusEffect(KS_SEMan.ks_BerserkName);
                buff.AddStack();
            }
            else
            {
                SE_Berserk buff = (SE_Berserk)seman.AddStatusEffect(ScriptableObject.CreateInstance<SE_Berserk>());
                buff.AddStack();
            }

        }
    }
}
