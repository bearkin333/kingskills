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
    class P_BigStick
    {
        public static bool embiggened = false;

        [HarmonyPatch(typeof(Humanoid),nameof(Humanoid.EquipItem))] [HarmonyPostfix]
        public static void IncreaseWeaponSize(Humanoid __instance, ItemDrop.ItemData item, bool __result)
        {
            if (!CFG.CheckPlayerActivePerk(__instance, PerkMan.PerkType.BigStick)) return;
            if (__result && item.IsWeapon())
            {
                embiggened = true;
            }
        }

        [HarmonyPatch(typeof(Humanoid), nameof(Humanoid.UnequipItem))] [HarmonyPostfix]
        public static void ReduceWeaponSize(Humanoid __instance, ItemDrop.ItemData item)
        {
            if (!embiggened || item == null) return;
            embiggened = false;
        }


        [HarmonyPatch(typeof(VisEquipment), nameof(VisEquipment.SetRightHandEquiped))] [HarmonyPostfix]
        public static void GetWeaponOnEquip(VisEquipment __instance, bool __result)
        {
            if (!(__result && embiggened)) return;
            Player localP = __instance.GetComponentInParent<Player>();
            if (!localP || localP != Player.m_localPlayer) return;
            MakeBig(__instance);
        }

        public static void MakeBig(VisEquipment visEquip)
        {
            visEquip.m_rightItemInstance.transform.localScale *= CFG.GetBigStickScaleMult();
        }

        /*
        Transform itemInstance = Player.m_localPlayer.m_visEquipment.m_rightItemInstance.transform;
        itemInstance.localScale *= CFG.GetBigStickScaleMult();
        */
    }
}
