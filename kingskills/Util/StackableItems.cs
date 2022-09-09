using ExtendedItemDataFramework;
using HarmonyLib;
using JetBrains.Annotations;
using Jotunn.Managers;
using kingskills.UX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace kingskills
{
	public static class StackableItems
	{

		[PublicAPI]
		public interface ExtendedItemUnique<in T> where T : BaseExtendedItemComponent
		{
			bool Equals(T obj);
		}

		public static bool IsExtendedStackable(ItemDrop.ItemData a, ItemDrop.ItemData b)
		{
			//If neither a nor b are extended, then stacking is fine
			if (a?.IsExtended() != true && b?.IsExtended() != true)
			{
				return true;
			}

			//A quick checker function for if an extended item is a unique extended item
			bool IsExtendedUniqueType(Type i) => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ExtendedItemUnique<>);

			//If B isn't extended, and A has no unique components, stacking is fine
			if (b?.IsExtended() != true)
			{
				return a?.Extended().Components.Any(c => c.GetType().GetInterfaces().Any(IsExtendedUniqueType)) != true;
			}
			// Same for A
			if (a.IsExtended() != true)
			{
				return b.Extended().Components.Any(c => c.GetType().GetInterfaces().Any(IsExtendedUniqueType)) != true;
			}

			//We create a dictionary of all the extended uniques in A
			Dictionary<Type, object> extendedUniques = a.Extended().Components.SelectMany(c =>
			{
				Type uniqueType = c.GetType().GetInterfaces().FirstOrDefault(IsExtendedUniqueType);
				if (uniqueType != null)
				{
					return new[] { new KeyValuePair<Type, object>(uniqueType, c) };
				}

				return Enumerable.Empty<KeyValuePair<Type, object>>();
			}).ToDictionary(kv => kv.Key, kv => kv.Value);

			foreach (BaseExtendedItemComponent component in b.Extended().Components)
			{
				Type uniqueType = component.GetType().GetInterfaces().FirstOrDefault(IsExtendedUniqueType);
				if (uniqueType != null)
				{
					if (extendedUniques.TryGetValue(uniqueType, out object other))
					{
						if (!(bool)uniqueType.GetMethod("Equals").Invoke(component, new[] { other }))
						{
							return false;
						}

						extendedUniques.Remove(uniqueType);
					}
					else
					{
						return false;
					}
				}
			}

			// All Unique components present in a were also present in b
			return extendedUniques.Count == 0;
		}
	}

	[HarmonyPatch]
	public class CheckExtendedUniqueCanAddItem
	{
		private static IEnumerable<MethodBase> TargetMethods() => new[]
		{
			AccessTools.DeclaredMethod(typeof(Inventory), nameof(Inventory.CanAddItem), new[] { typeof(ItemDrop.ItemData), typeof(int) }),
			AccessTools.DeclaredMethod(typeof(Inventory), nameof(Inventory.AddItem), new[] { typeof(ItemDrop.ItemData) }),
		};

		private static void Prefix(ItemDrop.ItemData item) => CheckExtendedUniqueFindFreeStack.CheckingFor = item;
		private static void Finalizer() => CheckExtendedUniqueFindFreeStack.CheckingFor = null;
	}

	[HarmonyPatch]
	public class CheckExtendedUniqueFindFreeStack
	{
		public static ItemDrop.ItemData CheckingFor;

		private static IEnumerable<MethodBase> TargetMethods() => new[]
		{
			AccessTools.DeclaredMethod(typeof(Inventory), nameof(Inventory.FindFreeStackSpace)),
			AccessTools.DeclaredMethod(typeof(Inventory), nameof(Inventory.FindFreeStackItem))
		};

		private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructionsEnumerable)
		{
			CodeInstruction[] instructions = instructionsEnumerable.ToArray();
			Label target = (Label)instructions.First(i => i.opcode == OpCodes.Br || i.opcode == OpCodes.Br_S).operand;
			CodeInstruction targetedInstr = instructions.First(i => i.labels.Contains(target));
			CodeInstruction lastBranch = instructions.Reverse().First(i => i.Branches(out Label? label) && targetedInstr.labels.Contains(label.Value));
			CodeInstruction loadingInstruction = null;

			for (int i = 0; i < instructions.Length; ++i)
			{
				yield return instructions[i];
				// get hold of the loop variable store (the itemdata we want to compare against)
				if (loadingInstruction == null && instructions[i].opcode == OpCodes.Call && ((MethodInfo)instructions[i].operand).Name == "get_Current")
				{
					loadingInstruction = instructions[i + 1].Clone();
					loadingInstruction.opcode = new Dictionary<OpCode, OpCode>
					{
						{ OpCodes.Stloc_0, OpCodes.Ldloc_0 },
						{ OpCodes.Stloc_1, OpCodes.Ldloc_1 },
						{ OpCodes.Stloc_2, OpCodes.Ldloc_2 },
						{ OpCodes.Stloc_3, OpCodes.Ldloc_3 },
						{ OpCodes.Stloc_S, OpCodes.Ldloc_S }
					}[loadingInstruction.opcode];
				}
				if (instructions[i] == lastBranch)
				{
					yield return loadingInstruction;
					yield return new CodeInstruction(OpCodes.Ldsfld, AccessTools.DeclaredField(typeof(CheckExtendedUniqueFindFreeStack), nameof(CheckingFor)));
					yield return new CodeInstruction(OpCodes.Call, AccessTools.DeclaredMethod(typeof(StackableItems), nameof(StackableItems.IsExtendedStackable)));
					yield return new CodeInstruction(OpCodes.Brfalse, target);
				}
			}
		}
	}

	[HarmonyPatch(typeof(Inventory), nameof(Inventory.AddItem), typeof(ItemDrop.ItemData), typeof(int), typeof(int), typeof(int))]
	public class CheckExtendedUniqueAddItem
	{
		private static bool Prefix(Inventory __instance, ItemDrop.ItemData item, int amount, int x, int y, ref bool __result)
		{
			ItemDrop.ItemData itemAt = __instance.GetItemAt(x, y);
			if (itemAt != null && !StackableItems.IsExtendedStackable(itemAt, item))
			{
				CombineGUI.OpenWindow(__instance, item, amount, itemAt);

				__result = false;
				return false;
			}

			return true;
		}
	}

	[HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.SetupDragItem))]
	public class Dragging
	{
		public static bool isTrue = false;
		public static InventoryGui invRef;

		[HarmonyPrefix]
		public static void OnDragStart(InventoryGui __instance) { invRef = __instance; CheckDrag(); }

		[HarmonyPatch(nameof(InventoryGui.OnDropOutside))]
		[HarmonyFinalizer]
		public static void OnOutsideDrop() => CheckDrag();

		[HarmonyPatch(typeof(InventoryGrid),nameof(InventoryGrid.DropItem))]
		[HarmonyFinalizer]
		public static void OnInventoryDrop() => CheckDrag();

		public static void CheckDrag()
		{
			if (invRef.m_dragGo == null)
				isTrue = false;
			else
				isTrue = true;
		}
	}

}
