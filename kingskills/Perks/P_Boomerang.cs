using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace kingskills.Perks
{
    [HarmonyPatch]
    class P_Boomerang
    {
        [HarmonyPatch(typeof(Projectile), nameof(Projectile.SpawnOnHit)), HarmonyPrefix]
        public static void SpearReturn(Projectile __instance)
        {
            Player player = Player.m_localPlayer;
            //Jotunn.Logger.LogMessage($"Projectile spawn");

            if (__instance.m_owner.GetZDOID() != player.GetZDOID()) return;
            //Jotunn.Logger.LogMessage($"Player is local player");

            if (!CFG.CheckPlayerActivePerk(player, PerkMan.PerkType.Boomerang)) return;
            //Jotunn.Logger.LogMessage($"Boomerang is active");

            if (__instance.m_spawnItem is null || !__instance.m_spawnItem.m_dropPrefab.name.Contains("Spear")) return;
            //Jotunn.Logger.LogMessage($"Spear return triggered!");

            player.m_inventory.AddItem(__instance.m_spawnItem);
            player.EquipItem(__instance.m_spawnItem);
            __instance.m_spawnItem = null;
        }
    }
}
