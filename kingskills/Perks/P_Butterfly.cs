using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace kingskills.Perks
{
    [HarmonyPatch(typeof(Character))]
    class P_Butterfly
    {
        private const string IsBOn = "Butterfly On";

        [HarmonyPatch(nameof(Character.Jump)), HarmonyPrefix]
        public static void OnJumping(Humanoid __instance)
        {
            CFG.SetZDOVariable(__instance, PerkMan.PerkType.Butterfly, IsBOn, true);
        }

        [HarmonyPatch(nameof(Character.Jump)), HarmonyFinalizer]
        public static void OnJumpingCleanup(Humanoid __instance)
        {
            CFG.SetZDOVariable(__instance, PerkMan.PerkType.Butterfly, IsBOn, false);
        }

        [HarmonyPatch(nameof(Character.IsOnGround)), HarmonyPrefix]
        public static bool SwimJumpChange(Character __instance, ref bool __result)
        {
            if (__instance.IsSwiming() && __instance.m_nview.IsValid() && __instance.m_nview.m_zdo.GetBool(IsBOn, false))
            {
                float liquidDepth = Mathf.Max(0f, __instance.GetLiquidLevel() - __instance.transform.position.y);
                __result = liquidDepth > Mathf.Max(0f, __instance.m_swimDepth - .4f);
                __instance.m_lastGroundNormal = Vector3.up;
                __instance.m_swimTimer = 0f;
                return CFG.SkipOriginal;
            }
            return CFG.DontSkipOriginal;
        }
    }
}
