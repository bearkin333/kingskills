using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace kingskills.Perks
{
    [HarmonyPatch(typeof(Humanoid))]
    class Asguard
    {
        public static float BlockDirectionPatch(Vector3 hitdir, Vector3 player_forward, Humanoid player)
        {
            //Jotunn.Logger.LogMessage($"Asguard check");
            //Jotunn.Logger.LogMessage($"Asguard is active:{PerkMan.IsPerkActive(PerkMan.PerkType.Asguard)}");
            if (CFG.CheckPlayerAndActive(player, PerkMan.PerkType.Asguard))
            {
                //Jotunn.Logger.LogMessage($"Successfully Asguarded");
                return 0f;
            }
            else
            {
                return Vector3.Dot(hitdir, player_forward);
            }
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
