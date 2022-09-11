using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace kingskills.Overhauls
{
    [HarmonyPatch(typeof(Humanoid), "UseItem")]
    public class EatRocks
    {
        [HarmonyPrefix]
        public static bool OnUseItem(Humanoid __instance, Inventory inventory, ItemDrop.ItemData item, bool fromInventoryGui, Inventory ___m_inventory, ZSyncAnimation ___m_zanim)
        {
            Player player = __instance as Player;

            if (player != null)
            {
                float edible = CFG.GetMiningXPEatRock(item);

                if (edible <= 0) return true;

                if (!inventory.ContainsItem(item))
                    return false;

                //Not sure what the point of this is, but since the original code had it, so shall it be
                GameObject hoverObject = __instance.GetHoverObject();
                Hoverable hoverable = ((bool)hoverObject) ? hoverObject.GetComponentInParent<Hoverable>() : null;
                if (hoverable != null && !fromInventoryGui && (hoverObject.GetComponentInParent<Interactable>()?.UseItem(__instance, item) ?? false))
                    return false;

                item.m_shared.m_foodBurnTime = CFG.GetMiningXPRockTimerInSeconds();

                if (player.EatFood(item))
                {
                    LevelUp.BXP(player, Skills.SkillType.Pickaxes, edible);

                    inventory.RemoveOneItem(item);
                    __instance.m_consumeItemEffects.Create(Player.m_localPlayer.transform.position, Quaternion.identity);
                    ___m_zanim.SetTrigger("eat");
                }
                return false;
            }

            return true;
        }
    }
}
