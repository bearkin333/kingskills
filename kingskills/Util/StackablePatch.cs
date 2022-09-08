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
	public static class StackablePatch
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
					yield return new CodeInstruction(OpCodes.Call, AccessTools.DeclaredMethod(typeof(StackablePatch), nameof(StackablePatch.IsExtendedStackable)));
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
			if (itemAt != null && !StackablePatch.IsExtendedStackable(itemAt, item))
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

	public class CombineGUI
    {
		public static GameObject CombineWindow;
		public static GameObject ConfirmationTextA;
		public static GameObject FoodComparisonWindow;
		public static GameObject ItemImg;
		public static GameObject ItemQuality;
		public static GameObject ItemAtImg;
		public static GameObject ItemAtQuality;
		public static GameObject WithText;
		public static GameObject QuestionMarkText;
		public static GameObject ConfirmationTextB;
		public static GameObject YesBtn;
		public static GameObject NoBtn;
		public static GameObject DontAskText;
		public static GameObject DontAskBtn;

		public static ItemDrop.ItemData item;
		public static int itemAmount;
		public static ItemDrop.ItemData itemAt;
		public static int invX;
		public static int invY;
		public static Inventory invReference;
		public static bool ask;

		public static void Init(bool askSetting = true)
        {
			ask = askSetting;

			CombineWindow = GUIManager.Instance.CreateWoodpanel(
					parent: GUIManager.CustomGUIFront.transform,
					anchorMin: new Vector2(0.5f, 0.5f),
					anchorMax: new Vector2(0.5f, 0.5f),
					position: new Vector2(0f, 0),
					width: 600,
					height: 400,
					draggable: true);
			CombineWindow.SetActive(false);


			ConfirmationTextA = GUIManager.Instance.CreateText(
				text: "Are you sure you want to package",
				parent: CombineWindow.transform,
				anchorMin: new Vector2(0.5f, 1f),
				anchorMax: new Vector2(0.5f, 1f),
				position: new Vector2(0f, -75f),
				font: GUIManager.Instance.AveriaSerifBold,
				fontSize: 30,
				color: GUIManager.Instance.ValheimOrange,
				outline: true,
				outlineColor: Color.black,
				width: 600f,
				height: 80f,
				addContentSizeFitter: false);
			ConfirmationTextA.GetComponent<Text>().alignment = TextAnchor.UpperCenter;

			FoodComparisonWindow = GUIManager.Instance.CreateWoodpanel(
					parent: CombineWindow.transform,
					anchorMin: new Vector2(0.5f, 0.5f),
					anchorMax: new Vector2(0.5f, 0.5f),
					position: new Vector2(0f, 20f),
					width: 440,
					height: 150,
					draggable: true);

			ItemImg = new GameObject();
			Image image = ItemImg.AddComponent<Image>();
			image.sprite = null;
			image.enabled = false;
			RectTransform rect = ItemImg.GetComponent<RectTransform>();
			rect.SetParent(FoodComparisonWindow.transform);
			rect.anchorMin = new Vector2(0.5f, 0.5f);
			rect.anchorMax = new Vector2(0.5f, 0.5f);
			rect.anchoredPosition = new Vector2(-140f, 15f);
			rect.sizeDelta = new Vector2(90f, 90f);

			ItemQuality = GUIManager.Instance.CreateText(
				text: "50",
				parent: ItemImg.transform,
				anchorMin: new Vector2(0.5f, 0.5f),
				anchorMax: new Vector2(0.5f, 0.5f),
				position: new Vector2(0f, -70f),
				font: GUIManager.Instance.AveriaSerifBold,
				fontSize: 24,
				color: GUIManager.Instance.ValheimOrange,
				outline: true,
				outlineColor: Color.black,
				width: 90f,
				height: 40f,
				addContentSizeFitter: false);
			ItemQuality.GetComponent<Text>().alignment = TextAnchor.UpperCenter;

			ConfirmationTextA = GUIManager.Instance.CreateText(
				text: "with",
				parent: FoodComparisonWindow.transform,
				anchorMin: new Vector2(0.5f, 0.5f),
				anchorMax: new Vector2(0.5f, 0.5f),
				position: new Vector2(0f, 0f),
				font: GUIManager.Instance.AveriaSerifBold,
				fontSize: 30,
				color: GUIManager.Instance.ValheimOrange,
				outline: true,
				outlineColor: Color.black,
				width: 100f,
				height: 80f,
				addContentSizeFitter: false);
			ConfirmationTextA.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;


			ItemAtImg = new GameObject();
			image = ItemAtImg.AddComponent<Image>();
			image.sprite = null;
			image.enabled = false;
			rect = ItemAtImg.GetComponent<RectTransform>();
			rect.SetParent(FoodComparisonWindow.transform);
			rect.anchorMin = new Vector2(0.5f, 0.5f);
			rect.anchorMax = new Vector2(0.5f, 0.5f);
			rect.anchoredPosition = new Vector2(140f, 15f);
			rect.sizeDelta = new Vector2(90f, 90f);

			ItemAtQuality = GUIManager.Instance.CreateText(
				text: "50",
				parent: ItemAtImg.transform,
				anchorMin: new Vector2(0.5f, 0.5f),
				anchorMax: new Vector2(0.5f, 0.5f),
				position: new Vector2(0f, -70f),
				font: GUIManager.Instance.AveriaSerifBold,
				fontSize: 24,
				color: GUIManager.Instance.ValheimOrange,
				outline: true,
				outlineColor: Color.black,
				width: 90f,
				height: 40f,
				addContentSizeFitter: false);
			ItemAtQuality.GetComponent<Text>().alignment = TextAnchor.UpperCenter;


			ConfirmationTextB = GUIManager.Instance.CreateText(
				text: "This cannot be undone. The resulting meals will inherit the lower quality of the two.",
				parent: CombineWindow.transform,
				anchorMin: new Vector2(0.5f, 1f),
				anchorMax: new Vector2(0.5f, 1f),
				position: new Vector2(0f, -280f),
				font: GUIManager.Instance.AveriaSerifBold,
				fontSize: 24,
				color: GUIManager.Instance.ValheimOrange,
				outline: true,
				outlineColor: Color.black,
				width: 500f,
				height: 80f,
				addContentSizeFitter: false);
			ConfirmationTextA.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;


			YesBtn = GUIManager.Instance.CreateButton(
				text: "Yes",
				parent: CombineWindow.transform,
				anchorMin: new Vector2(0.5f, 0f),
				anchorMax: new Vector2(0.5f, 0f),
				position: new Vector2(-120f, 38f),
				width: 120f,
				height: 55f);
			Button btn = YesBtn.GetComponent<Button>();
			btn.onClick.AddListener(OnConfirmClick);

			NoBtn = GUIManager.Instance.CreateButton(
				text: "No",
				parent: CombineWindow.transform,
				anchorMin: new Vector2(0.5f, 0f),
				anchorMax: new Vector2(0.5f, 0f),
				position: new Vector2(120f, 38f),
				width: 120f,
				height: 55f);
			btn = NoBtn.GetComponent<Button>();
			btn.onClick.AddListener(CloseWindow);


			DontAskBtn = GUIManager.Instance.CreateButton(
				text: "Don't ask me again",
				parent: CombineWindow.transform,
				anchorMin: new Vector2(0.5f, 0f),
				anchorMax: new Vector2(0.5f, 0f),
				position: new Vector2(0f, -38f),
				width: 250f,
				height: 55f);
			btn = NoBtn.GetComponent<Button>();
			btn.onClick.AddListener(DontAsk);

		}

		public static void OnConfirmClick()
        {
			SaveFoodQuality FQ = item.Extended().GetComponent<SaveFoodQuality>();
			SaveFoodQuality FQAt = itemAt.Extended().GetComponent<SaveFoodQuality>();

			float newQuality;

			//We use the smaller quality for the new stacked item
			if (FQ.foodQuality < FQAt.foodQuality) newQuality = FQ.foodQuality;
			else newQuality = FQAt.foodQuality;

			int room = itemAt.m_shared.m_maxStackSize - itemAt.m_stack;

			if (room <= 0)
			{
				Jotunn.Logger.LogMessage("No room left in the stack");
			}
            else
			{
				int moveStack = Mathf.Min(room, itemAmount);
				itemAt.m_stack += moveStack;
				item.m_stack -= moveStack;

				ZLog.Log("Added to stack" + itemAt.m_stack + " " + item.m_stack);
				FQAt.foodQuality = newQuality;

				if (item.m_stack <= 0)
                {
					Dragging.invRef.SetupDragItem(null, null, 0);
					invReference.RemoveItem(item);
                }

				invReference.Changed();

			}

			CloseWindow();
        }

		public static void CloseWindow()
        {
			CombineWindow.SetActive(false);
			GUIManager.BlockInput(false);
			ItemImg.GetComponent<Image>().enabled = false;
			ItemAtImg.GetComponent<Image>().enabled = false;
			item = null;
			itemAt = null;
		}

		public static void DontAsk()
        {
			Jotunn.Logger.LogMessage("Jeez, I'm just trying to help. No need to shout");
        }

		public static void OpenWindow(Inventory inv, ItemDrop.ItemData nitem, int nItemAmount, ItemDrop.ItemData nitemAt)
		{
			if (CombineWindow == null) Init();

			item = nitem;
			itemAt = nitemAt;

			//if at least one of them is not extended, return
			if (!(item.IsExtended() || itemAt.IsExtended()))
			{
				item = null; itemAt = null;
				return;
			}

			itemAmount = nItemAmount;

			invReference = inv;

			if (!ask)
            {
				OnConfirmClick();
            }
			{
				ItemImg.GetComponent<Image>().sprite = item.GetIcon();
				ItemImg.GetComponent<Image>().enabled = true;
				ItemQuality.GetComponent<Text>().text =
					PT.Prettify(item.Extended().GetComponent<SaveFoodQuality>().foodQuality*100f, 0, PT.TType.TextlessPercent);

				ItemAtImg.GetComponent<Image>().sprite = itemAt.GetIcon();
				ItemAtImg.GetComponent<Image>().enabled = true;
				ItemAtQuality.GetComponent<Text>().text =
					PT.Prettify(itemAt.Extended().GetComponent<SaveFoodQuality>().foodQuality*100f, 0, PT.TType.TextlessPercent);


				CombineWindow.SetActive(true);
				GUIManager.BlockInput(true);
			}

		}
	}
}
