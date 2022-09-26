using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
//using UnityEngine;

namespace kingskills.Perks
{
    // and green thumb
    [HarmonyPatch]
    class P_BreakMyStride
    {

        [HarmonyPatch(typeof(Vagon), nameof(Vagon.SetMass)), HarmonyPrefix]
        public static void CartMassReduction(Vagon __instance, ref float mass)
        {
            if (!__instance.m_nview.IsOwner()) return;
            if (!PerkMan.IsPerkActive(PerkMan.PerkType.BreakMyStride)) return;
            mass *= CFG.GetBreakMyStrideMassMod();
        }
    }
}
