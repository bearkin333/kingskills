using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace kingskills.Perks
{
    [HarmonyPatch(typeof(Humanoid))]
    class Asguard
    {
        [HarmonyPatch(nameof(Humanoid.BlockAttack))]
        [HarmonyPrefix]
        public static void CheckAsguard(Humanoid __instance)
        {
            KSBlock.asguardCheck = CFG.CheckPlayerAndActive(__instance, PerkMan.PerkType.Asguard);
        }


        /*
         
        //////////////////////////////////////////////////////////////
        //////////////////entirely handled within the KSBlock class
        //////////////////////////////////////////////////////////////
        ///
        [HarmonyPatch(nameof(Character.RPC_Damage))]
        [HarmonyPrefix]
        public static void CheckBlockValues(Character __instance, HitData hit)
        {
            if (!__instance.IsPlayer()) return;
            Jotunn.Logger.LogMessage($"Player taking damage");
            Jotunn.Logger.LogMessage($"Player blocking: {__instance.IsBlocking()}");
            Jotunn.Logger.LogMessage($"Hit blockable: {hit.m_blockable}");
        }
        [HarmonyPatch(typeof(Humanoid),nameof(Humanoid.BlockAttack))]
        [HarmonyPrefix]
        public static void CheckBlockAttack(Humanoid __instance, HitData hit)
        {
            Jotunn.Logger.LogMessage($"For sure blocking an attackright now");


        
        */

    }
}
