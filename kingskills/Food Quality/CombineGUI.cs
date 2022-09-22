using ExtendedItemDataFramework;
using Jotunn.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using kingskills;
using HarmonyLib;

namespace kingskills.UX
{
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
		public static bool loadedAsk = false;

		public static bool quickConfirm = false;

		public static void Init()
		{
			if (!loadedAsk) ask = true;

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
					position: new Vector2(0f, 35f),
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
				position: new Vector2(0f, -290f),
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

		public static void CombineUpdate(string buttonName)
		{
			if (CombineGUI.CombineWindow == null) return;
			if (!CombineGUI.CombineWindow.activeSelf) return;
			if (ZInput.instance == null) return;

			if (ZInput.GetButtonDown(buttonName) || quickConfirm)
			{
				quickConfirm = false;
				OnConfirmClick();
			}
		}

		public static void LoadAskSetting(bool nAsk)
        {
			loadedAsk = true;
			ask = nAsk;
			//Jotunn.Logger.LogMessage($"Loaded dont ask me again as {!nAsk}");
		}

		public static void OnConfirmClick()
		{
			SaveFoodQuality FQ = item.Extended().GetComponent<SaveFoodQuality>();
			SaveFoodQuality FQAt = itemAt.Extended().GetComponent<SaveFoodQuality>();

			float newQuality;
			long newChef;

			//We use the smaller quality for the new stacked item
			if (FQ.foodQuality < FQAt.foodQuality) {
				newQuality = FQ.foodQuality;
				newChef = FQ.chefID;
			}
			else {
				newQuality = FQAt.foodQuality;
				newChef = FQAt.chefID;
			}

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
				FQAt.chefID = newChef;
				FQAt.flavorText = "This is a collection of packaged foodstuffs.";

				if (item.m_stack <= 0)
				{
					Dragging.invRef.m_dragInventory.RemoveItem(item);
					Dragging.invRef.SetupDragItem(null, null, 0);
					//invReference.RemoveItem(item);

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
			ask = false;
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
				quickConfirm = true;
			}
			{
				ItemImg.GetComponent<Image>().sprite = item.GetIcon();
				ItemImg.GetComponent<Image>().enabled = true;
				ItemQuality.GetComponent<Text>().text =
					PT.Prettify(item.Extended().GetComponent<SaveFoodQuality>().foodQuality * 100f, 0, PT.TType.TextlessPercent);

				ItemAtImg.GetComponent<Image>().sprite = itemAt.GetIcon();
				ItemAtImg.GetComponent<Image>().enabled = true;
				ItemAtQuality.GetComponent<Text>().text =
					PT.Prettify(itemAt.Extended().GetComponent<SaveFoodQuality>().foodQuality * 100f, 0, PT.TType.TextlessPercent);


				CombineWindow.SetActive(true);
				GUIManager.BlockInput(true);
			}

		}
	}

	/*
	//I have to do this because when you combine without asking, the inventory processes an extra click and gets very confused
	//about the lack of the item that was just there... and I'm stupid xD
	//or maybe I just can't fix it this way LOL
	[HarmonyPatch(typeof(InventoryGrid), nameof(InventoryGrid.OnLeftClick))]
	public static class InventoryLeftClickErasure
    {
		[HarmonyPrefix]
		public static bool StopClick()
        {
			if (CombineGUI.ignoreClick)
			{
				Jotunn.Logger.LogMessage("Ignored evil click");
				CombineGUI.ignoreClick = false;
				return CFG.SkipOriginal;
			}
			Jotunn.Logger.LogMessage("click went through");

			return CFG.DontSkipOriginal;
        }
    }
	*/
}
