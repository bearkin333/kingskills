using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtendedItemDataFramework;
using HarmonyLib;
using UnityEngine;
using UnityEngine.EventSystems;

namespace kingskills.UX
{
    class QuickCombineGUI
    {
		public static GameObject hover = null;

		[HarmonyPatch(typeof(UITooltip), nameof(UITooltip.OnHoverStart))]
		[HarmonyPrefix]
		public static void GetTooltipHover(UITooltip __instance, GameObject go) => hover = go;

		[HarmonyPatch(typeof(UITooltip), nameof(UITooltip.OnPointerExit))]
		[HarmonyPostfix]
		public static void CleanTooltipHover(UITooltip __instance) => hover = null;


		public static void CollapseHoveredFood()
		{
			Player player = Player.m_localPlayer;
			//Jotunn.Logger.LogMessage($"input read");

			if (player == null)return;
			ItemDrop.ItemData item = null;

			PointerEventData pointer = new PointerEventData(EventSystem.current);
			pointer.position = Input.mousePosition;

			item = InventoryGui.instance.m_playerGrid.GetItem(new Vector2i(pointer.position));

			//Jotunn.Logger.LogMessage($"item is {item}");

			if (item is null) return;

			//Jotunn.Logger.LogMessage($"found hovered item {item.m_dropPrefab.name}");

			CombineAllOfType(player.m_inventory, item);
		}

        public static void CombineAllOfType(Inventory inv, ItemDrop.ItemData itemType)
        {
            List<ItemDrop.ItemData> removeList = new List<ItemDrop.ItemData>();
			SaveFoodQuality FQtemp;
			float lowestFQ = 1000f;
			int totalAmount = 0;

			ItemDrop.ItemData itemClone = itemType.Clone();

			foreach (ItemDrop.ItemData item in inv.GetAllItems())
            {
                if (item.IsExtended() &&
                    item.m_dropPrefab.name == itemType.m_dropPrefab.name)
                {
					FQtemp = item.Extended().GetComponent<SaveFoodQuality>();
					if (FQtemp != null)
					{
						if (FQtemp.foodQuality < lowestFQ) lowestFQ = FQtemp.foodQuality;

						removeList.Add(item);
						totalAmount += item.m_stack;
					}
                }
            }

			foreach(ItemDrop.ItemData item in removeList)
            {
				inv.RemoveItem(item);
            }

			itemClone.Extended().GetComponent<SaveFoodQuality>().foodQuality = lowestFQ;

			int stackSize = itemType.m_shared.m_maxStackSize;

			for (int i = totalAmount; i > 0; i -= stackSize)
            {
				ItemDrop.ItemData newItem = itemClone.Clone();
				newItem.m_stack = Math.Min(totalAmount, stackSize);
				inv.AddItem(newItem);
            }

			inv.Changed();
		}
    }

}
