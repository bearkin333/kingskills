using HarmonyLib;
using kingskills.Perks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace kingskills.Patches
{
    [HarmonyPatch(typeof(Humanoid), "UseItem")]
    public class EatRocks
    {
        [HarmonyPrefix]
        public static bool OnUseItem(Humanoid __instance, Inventory inventory, ItemDrop.ItemData item, bool fromInventoryGui, Inventory ___m_inventory, ZSyncAnimation ___m_zanim)
        {
            Player player = __instance as Player;

            if (player is null || item is null) return CFG.DontSkipOriginal;

            float xp = 0f;
            Skills.SkillType xpSkill = Skills.SkillType.None;

            string itemName = item.m_dropPrefab.name;
            if (CFG.MiningBXPTable.ContainsKey(itemName))
            {
                xpSkill = Skills.SkillType.Pickaxes;
                xp = CFG.MiningBXPTable[itemName];
            }
            else if (PerkMan.IsPerkActive(PerkMan.PerkType.Decapitation) && itemName.Contains("Trophy"))
            {
                xpSkill = Skills.SkillType.Axes;
                xp = CFG.DecapitationExp.Value;
            }

            if (xp <= 0f) return CFG.DontSkipOriginal;

            if (!inventory.ContainsItem(item))
                return CFG.SkipOriginal;

            //Not sure what the point of this is, but since the original code had it, so shall it be
            GameObject hoverObject = __instance.GetHoverObject();
            Hoverable hoverable = ((bool)hoverObject) ? hoverObject.GetComponentInParent<Hoverable>() : null;
            if (hoverable != null && !fromInventoryGui && (hoverObject.GetComponentInParent<Interactable>()?.UseItem(__instance, item) ?? false))
                return CFG.SkipOriginal;

            if (xpSkill == Skills.SkillType.Pickaxes)
                item.m_shared.m_foodBurnTime = CFG.GetMiningXPRockTimerInSeconds();

            if (player.EatFood(item))
            {
                LevelUp.BXP(player, xpSkill, xp);

                inventory.RemoveOneItem(item);
                __instance.m_consumeItemEffects.Create(Player.m_localPlayer.transform.position, Quaternion.identity);
                ___m_zanim.SetTrigger("eat");
            }
            return CFG.SkipOriginal;


        }
    }
}
