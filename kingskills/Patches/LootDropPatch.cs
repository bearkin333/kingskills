using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace kingskills.Patches
{
    class LootDropPatch
    {
        [HarmonyPatch(typeof(DropTable), "GetDropList", new Type[] { typeof(int) })]
        [HarmonyPostfix]
        public static void DropTableFix (ref DropTable __instance, ref List<GameObject> __result, int amount)
        {
            Player playerRef = Player.m_localPlayer;

        }
    }
}
