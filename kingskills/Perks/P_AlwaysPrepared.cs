using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace kingskills.Perks
{
    [HarmonyPatch(typeof(Humanoid))]
    class P_AlwaysPrepared
    {
        public const string IsChecking = "AP Swim Check";

        [HarmonyPatch(nameof(Humanoid.UpdateEquipment))]
        [HarmonyPrefix]
        public static void PreSwimCheck(Humanoid __instance) =>
            CFG.SetZDOVariable(__instance, PerkMan.PerkType.AlwaysPrepared, IsChecking, true);

        [HarmonyPatch(nameof(Humanoid.UpdateEquipment))]
        [HarmonyFinalizer]
        public static void PostJump(Character __instance) => 
            CFG.SetZDOVariable(__instance, PerkMan.PerkType.AlwaysPrepared, IsChecking, false);


        [HarmonyPatch(nameof(Humanoid.HideHandItems))]
        [HarmonyPrefix]
        public static bool SkipHideInWater(Humanoid __instance)
        {
            if (!__instance.m_nview.m_zdo.GetBool(IsChecking, false)) return CFG.DontSkipOriginal;

            if (__instance.IsSwiming()) return CFG.SkipOriginal;

            return CFG.DontSkipOriginal;
        }
    }
}
