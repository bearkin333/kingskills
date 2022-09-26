using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace kingskills.Perks
{
    [HarmonyPatch]
    class P_BlockExpert
    {
        [HarmonyPatch(typeof(Character),nameof(Character.RPC_Damage)), HarmonyPrefix]
        public static void CheckBlockValues(Character __instance, ref HitData hit)
        {
            if (!CFG.CheckPlayerAndActive(__instance, PerkMan.PerkType.BlockExpert)) return;
            hit.m_blockable = true;
        }
    }
}
