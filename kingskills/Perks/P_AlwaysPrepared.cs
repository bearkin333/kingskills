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
        private const string IsOn = "AlwaysP On";

        [HarmonyPatch(nameof(Humanoid.UpdateEquipment)), HarmonyPrefix]
        public static void PreUpdateCheck(Humanoid __instance) =>
            CFG.SetZDOVariable(__instance, PerkMan.PerkType.AlwaysPrepared, IsOn, true);

        [HarmonyPatch(nameof(Humanoid.UpdateEquipment)), HarmonyFinalizer]
        public static void PostUpdateCheck(Humanoid __instance) => 
            CFG.SetZDOVariable(__instance, PerkMan.PerkType.AlwaysPrepared, IsOn, false);

        [HarmonyPatch(nameof(Humanoid.EquipItem)), HarmonyPrefix]
        public static void PreEquipCheck(Humanoid __instance) =>
            CFG.SetZDOVariable(__instance, PerkMan.PerkType.AlwaysPrepared, IsOn, true);

        [HarmonyPatch(nameof(Humanoid.EquipItem)), HarmonyFinalizer]
        public static void PostEquipCheck(Humanoid __instance) =>
            CFG.SetZDOVariable(__instance, PerkMan.PerkType.AlwaysPrepared, IsOn, false);

        /*
        [HarmonyPatch(nameof(Humanoid.HideHandItems)), HarmonyPrefix]
        public static bool SkipHideInWater(Humanoid __instance)
        {
            if (!__instance.m_nview.m_zdo.GetBool(IsOn, false)) return CFG.DontSkipOriginal;

            if (__instance.IsSwiming()) return CFG.SkipOriginal;

            return CFG.DontSkipOriginal;
        }
        */

        [HarmonyPatch(typeof(Character),nameof(Character.IsSwiming)), HarmonyPrefix]
        public static bool FakeSwimming(Character __instance, ref bool __result)
        {
            if (__instance.m_nview.m_zdo.GetBool(IsOn, false))
            {
                __result = false;
                return CFG.SkipOriginal;
            }
            return CFG.DontSkipOriginal;
        }
    }
}
