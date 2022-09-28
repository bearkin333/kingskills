using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace kingskills.Perks
{
    [HarmonyPatch]
    class P_ControlledDemo
    {

        [HarmonyPatch(typeof(TreeBase),nameof(TreeBase.SpawnLog)), HarmonyPrefix]
        public static void LaunchLog(TreeBase __instance, Vector3 hitDir)
        {
            Player killer = null;
            if (!CFG.GetKillerExists(__instance.m_nview, ref killer)) return;
            if (!CFG.CheckPlayerActivePerk(killer, PerkMan.PerkType.ControlledDemo)) return;

            Vector3 newDir = Vector3.zero;


        }

        public static Character GetNearestEnemy(Character player)
        {
            return player;
        }
    }
}
