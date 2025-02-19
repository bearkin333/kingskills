﻿using ExtendedItemDataFramework;
using HarmonyLib;
using kingskills.UX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using kingskills.Patches;

namespace kingskills.KSkills
{

	/*
     * cooking
exp gained:
exp gained when you add food to cooking station, based on tier
more exp gained when you successfully remove done food from cooking station, again based on tier
bonus xp gained whenever someone eats food you make based on quality of food


effects:
increases item quality of cooked items (health and stamina gain and length)
item quality is random based on your skill
imbue food with new buffs? food of a certain quality can have a certain buff added
based on what you have selected in cooking menu
reduces fermentation time
reduces cook time

perks:
50
	learn new buffs to cook into food (element resist, damage type resist)
	learn new buffs (move speed)
100
	new recipes
	very good buffs (exp gain or damage)
    */

	public static class InitExtendedItem
	{
		public static void Init()
        {
			ExtendedItemData.NewExtendedItemData += itemMade =>
			{
				/*
                try
				{
					Jotunn.Logger.LogMessage($"This client responsible for creating an extended item. This one's " +
						$"ID is {Player.m_localPlayer.GetPlayerID()}" +
						$"\nLocal chef cooking is {LocalChefCooking.isTrue}" +
						$"\nLocal chef cooking from inventory is {LocalChefInvCooking.isTrue}");
				}
                catch
                {
					Jotunn.Logger.LogMessage($"Local player didn't exist trying to make an extended item");
				}
				try
				{
					Jotunn.Logger.LogMessage($"Item made is {itemMade.m_dropPrefab.name}, and it's stats are " +
						$"{itemMade.m_shared.m_food} and {itemMade.m_shared.m_foodStamina}");
				}
				catch
				{
					Jotunn.Logger.LogMessage($"item has no info");
				}
				*/
				if (!(itemMade.m_shared.m_food > 0 || itemMade.m_shared.m_foodStamina > 0)) return;
				/*
				Jotunn.Logger.LogMessage($"Local chef is cooking: {LocalChefCooking.isTrue}");
				Jotunn.Logger.LogMessage($"Local chef is cooking from inventory: {LocalChefInvCooking.isTrue}");
				Jotunn.Logger.LogMessage($"Local picker is picking: {PlayerPickRef.pickingPlayer != null}");
				*/
				if (LocalChefCooking.isTrue)
                {
					SaveFoodQuality FQ = itemMade.AddComponent<SaveFoodQuality>();
					Player player = Player.m_localPlayer;
					player.ShowTutorial("kingskills_foodQ");

					float timeToCook = 0f;
					float cookingFinishedTime = 0f;

					//Will be more interesting later
					if (ThrowItem.throwing)
					{
						timeToCook = ThrowItem.timeToCook;
						cookingFinishedTime = ThrowItem.cookingFinishedTime;
					}

					float skillFactor;
					if (player != null)
					{
						skillFactor = player.GetSkillFactor(SkillMan.Cooking);
						FQ.chefID = player.GetPlayerID();

						//Jotunn.Logger.LogMessage($"saved the chef as {FQ.chefID}");
					}
					else
                    {
						skillFactor = 0;
                    }
					float timingPercent = CFG.GetCookingTimingPercent(cookingFinishedTime, timeToCook);

					FQ.flavorText = CFG.GetCookingFlavorText(timingPercent);

					float newQuality = CFG.GetCookingTimingRandomFQ(skillFactor, timingPercent);

					FQ.foodQuality = newQuality;

					QualityTextDisplayer(newQuality, player);

					//Jotunn.Logger.LogMessage($"Showed tutorial and added new FQ components:" +
					//	$"{FQ.foodQuality}, ID: {FQ.chefID}");
				}
				else if (LocalChefInvCooking.isTrue)
				{
					SaveFoodQuality FQ = itemMade.AddComponent<SaveFoodQuality>();
					Player player = Player.m_localPlayer;

					float skillFactor;
					if (player != null)
					{
						skillFactor = player.GetSkillFactor(SkillMan.Cooking);
						FQ.chefID = player.GetPlayerID();

						//Jotunn.Logger.LogMessage($"saved the chef as {FQ.chefID}");
					}
					else
					{
						skillFactor = 0;
					}

					float newQuality = CFG.GetCookingRandomFQ(skillFactor);

					FQ.foodQuality = newQuality;

					QualityTextDisplayer(newQuality, player);
				}
				else if (LocalPlayerInteracting.isTrue)
				{
					SaveFoodQuality FQ = itemMade.AddComponent<SaveFoodQuality>();
					Player player = Player.m_localPlayer;

					FQ.chefID = 0;

					if (player != null)
						FQ.foodQuality = CFG.GetAgricultureRandomFQ(player.GetSkillFactor(SkillMan.Agriculture));
				}
			};
        }

		public static void QualityTextDisplayer(float quality, Character player)
		{
			string qualityMessage = (quality * 100f).ToString("F0") + "% quality";
			Vector3 msgPos = CustomWorldTextManager.GetInFrontOfCharacter(player) + Vector3.up;
			Color msgColor = Color.black;

			if (quality > 0) msgColor = CFG.ColorPTGreen;
			else msgColor = CFG.ColorPTRed;

			CustomWorldTextManager.AddCustomWorldText(CFG.ColorAscendedGreen, msgPos, 34, qualityMessage);
		}
	}


	[HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.DoCrafting))]
	public class LocalChefInvCooking
	{
		public static bool isTrue = false;

		[HarmonyPrefix]
		[HarmonyPriority(2)]
		public static void Prefix() 
		{
			isTrue = true;
		}

		[HarmonyFinalizer]
		public static void Finalizer() => isTrue = false;
	}
	[HarmonyPatch(typeof(CookingStation), nameof(CookingStation.OnInteract))]
	public class LocalChefCooking
	{
		public static bool isTrue = false;
		[HarmonyPrefix]
		public static void IsInteracting() => isTrue = true;

		[HarmonyFinalizer]
		public static void StopInteract() => isTrue = false;
	}


	[HarmonyPatch(typeof(CookingStation), nameof(CookingStation.CookItem))]
	public class StartCooking
    {
		[HarmonyPrefix]
		public static void CookItemPatch(CookingStation __instance, Humanoid user)
        {
			ZDO zdo = __instance.m_nview.m_zdo;
			if (zdo == null) return;

			if (!__instance.m_nview.IsOwner()) __instance.m_nview.ClaimOwnership();

			//Jotunn.Logger.LogMessage($" recording the player's cooking skill as {(user as Player).GetSkillFactor(SkillMan.Cooking)}");

			zdo.Set("Cooking Skill", (user as Player).GetSkillFactor(SkillMan.Cooking));
			//zdo.Set("Meat Station", true);

			(user as Player).RaiseSkill(SkillMan.Cooking, 
				CFG.GetCookingXP(__instance.m_nview.GetPrefabName()));
		}

		[HarmonyPatch(nameof(CookingStation.GetItemConversion))]
		[HarmonyPostfix]
		public static void ReduceCookTime(CookingStation __instance, ref CookingStation.ItemConversion __result)
		{
			if (!LocalChefCooking.isTrue) return;
			if (__result is null) return;

			ZDO zdo = __instance.m_nview.m_zdo;
			if (zdo is null) return;

			Player player = Player.m_localPlayer;
			if (player is null) return;

			if (!__instance.m_nview.IsOwner()) __instance.m_nview.ClaimOwnership();

			string itemName = __result.m_from.name;
			//Jotunn.Logger.LogMessage($"Checking for item {itemName}");

			float recordedSkill = zdo.GetFloat("Cooking Level for " + itemName, 0f);
			long recordedChefID = zdo.GetLong("Chef for " + itemName, 0);
			bool conversionRecorded = zdo.GetBool(itemName + " recorded", false);
			bool dataChanged = false;

			float redux = 1f;

			if (!conversionRecorded)
            {
				zdo.Set(itemName + " recorded", true);
				zdo.Set(itemName + " base time", __result.m_cookTime);
			}

			//If this conversion hasn't recorded a player yet, we claim it
			if (recordedChefID == 0 || 
				recordedChefID != player.GetPlayerID() ||
				player.GetSkillFactor(SkillMan.Cooking) != recordedSkill)
			{
				redux = CFG.GetCookingTimeRedux(player.GetSkillFactor(SkillMan.Cooking));


				zdo.Set("Cooking Level for " + itemName, player.GetSkillFactor(SkillMan.Cooking));
				zdo.Set("Chef for " + itemName, player.GetPlayerID());
				dataChanged = true;

			}

			//If nothing has changed, there's no need to touch the result
			if (!dataChanged) return;

			__result.m_cookTime = zdo.GetFloat(itemName + " base time", 1.2266f);

			__result.m_cookTime *= redux;
		}


		[HarmonyPatch(typeof(Fermenter),nameof(Fermenter.AddItem))]
		[HarmonyPostfix]
		public static void ReduceFermentTime(Fermenter __instance, Humanoid user, ref bool __result)
		{
			if (!__result) return;
			ZDO zdo = __instance.m_nview.m_zdo;
			if (zdo == null) return;

			if (!__instance.m_nview.IsOwner()) __instance.m_nview.ClaimOwnership();


			float recordedSkill = zdo.GetFloat("Cooking Level", 0f);
			long recordedChefID = zdo.GetLong("Current Chef", 0);
			bool conversionRecorded = zdo.GetBool("time recorded", false);
			Player player = (Player)user;

			float redux = 1f;
			bool dataChanged = false;

			if (!conversionRecorded)
			{
				zdo.Set("time changed", true);
				zdo.Set("base time", __instance.m_fermentationDuration);
			}

			//If our stats are different than the recorded stats at all
			if (recordedChefID == 0 ||
				recordedChefID != player.GetPlayerID() ||
				player.GetSkillFactor(SkillMan.Cooking) != recordedSkill)
			{
				redux = CFG.GetCookingFermentTimeRedux(player.GetSkillFactor(SkillMan.Cooking));

				zdo.Set("Cooking Level", player.GetSkillFactor(SkillMan.Cooking));
				zdo.Set("Current Chef", player.GetPlayerID());

				dataChanged = true;
			}
			//If nothing has changed, there's no need to touch the result
			if (!dataChanged) return;

			__instance.m_fermentationDuration = zdo.GetFloat("base time", 1.2266f);

			__instance.m_fermentationDuration *= redux;
		}
	}


	[HarmonyPatch(typeof(CookingStation), nameof(CookingStation.RPC_RemoveDoneItem))]
	public class ThrowItem
	{
		public static float timeToCook = 0;
		public static float cookingFinishedTime = 0;
		public static bool throwing = false;

		[HarmonyPrefix]
		public static void GrabCookTimes(CookingStation __instance)
		{
			for (int i = 0; i < __instance.m_slots.Length; i++)
			{
				__instance.GetSlot(i, out var itemName, out var timing, out var _);
				if (itemName != "" && __instance.IsItemDone(itemName))
				{
					throwing = true;
					cookingFinishedTime = timing;
					CookingStation.ItemConversion itemConv = __instance.GetItemConversion(itemName);
					if (itemConv is null)
                    {
						timeToCook = 50f;
                    }
                    else
					{
						timeToCook = itemConv.m_cookTime;
					}

					//Jotunn.Logger.LogMessage($"Grabbed a cook time of {timing}. " +
					//	$"item finishes at {__instance.GetItemConversion(itemName).m_cookTime}");

					if (LocalChefCooking.isTrue)
					{
						//Jotunn.Logger.LogMessage($"Local player was cooking when that piece got removed from the oven");
						Player player = Player.m_localPlayer;

						player.RaiseSkill(SkillMan.Cooking, 
							CFG.GetCookingXP(__instance.m_nview.GetPrefabName(), false));
					}
					break;
				}
			}
		}
		[HarmonyFinalizer]
		public static void DisposeCookTimes(CookingStation __instance)
		{
			throwing = false;
			cookingFinishedTime = 0;
			timeToCook = 0; 
		}
	}


	[HarmonyPatch(typeof(Player), nameof(Player.EatFood))]
	public class ApplyCookingBuffs
	{
		public static float food = 0;
		public static float foodStam = 0;
		public static float foodDuration = 0;
		public static float foodEitr = 0;
		public static float foodRegen = 0;
		[HarmonyPrefix]
		public static void TemporaryFQBoost(Player __instance, ItemDrop.ItemData item, bool __result)
		{
			if (item.m_shared.m_food <= 0 || item.m_shared.m_foodStamina <= 0 || item.m_shared.m_foodEitr <= 0 ) return;
			if (item.IsExtended())
            {
				SaveFoodQuality FQ = item.Extended().GetComponent<SaveFoodQuality>();

				if (FQ == null) return;
				if (FQ?.foodQuality == null) FQ.foodQuality = 0;

				//Jotunn.Logger.LogMessage($"Eating this yum yum food and its quality is {FQ.foodQuality}");
				float foodMult = 1f + FQ.foodQuality;

				food = item.m_shared.m_food;
				foodRegen = item.m_shared.m_foodRegen;
				foodStam = item.m_shared.m_foodStamina;
				foodDuration = item.m_shared.m_foodBurnTime;
				foodEitr = item.m_shared.m_foodEitr;

				item.m_shared.m_food = Mathf.Floor(item.m_shared.m_food * foodMult);
				item.m_shared.m_foodStamina = Mathf.Floor(item.m_shared.m_foodStamina * foodMult);
				item.m_shared.m_foodBurnTime = Mathf.Floor(item.m_shared.m_foodBurnTime * foodMult);
				item.m_shared.m_foodEitr = Mathf.Floor(item.m_shared.m_foodEitr * foodMult);
				item.m_shared.m_foodRegen = Mathf.Floor(item.m_shared.m_foodRegen * foodMult);

				float chefXPReward = item.m_shared.m_food + item.m_shared.m_foodStamina;
				chefXPReward *= CFG.CookingXPStatMod.Value;

				Player chef = Player.GetPlayer(FQ.chefID);

				if (chef != null)
                {
					//Jotunn.Logger.LogMessage($"chef was found as a player");
					if (__instance.GetZDOID().m_userID != FQ.chefID)
						chefXPReward *= CFG.GetCookingXPCustomerMult();

					RPC.RPCMan.SendXP_RPC(chef.m_nview, chefXPReward, SkillMan.Cooking, true);
                }
			}
		}

		[HarmonyFinalizer]
		public static void FQReset(ItemDrop.ItemData item)
		{
			if (item.IsExtended())
			{
				item.m_shared.m_food = food;
				item.m_shared.m_foodStamina = foodStam;
				item.m_shared.m_foodBurnTime = foodDuration;
				item.m_shared.m_foodRegen = foodRegen;
				item.m_shared.m_foodEitr = foodEitr;
			}
		}
	}


	[HarmonyPatch(typeof(Player), nameof(Player.Load))]
	public class LoadExtendedFood
	{
		[HarmonyPostfix]
		public static void PostLoad(Player __instance)
		{
			foreach (Player.Food food in __instance.m_foods)
			{
				if (!food.m_item.IsExtended())
				{
					food.m_item = new ExtendedItemData(food.m_item);
				}

				SaveFoodQuality FQ = food.m_item.Extended().GetComponent<SaveFoodQuality>();
				if (FQ == null)
				{
					FQ = food.m_item.Extended().AddComponent<SaveFoodQuality>();
					FQ.foodQuality = 0;
				}
			}
		}
	}

	[HarmonyPatch(typeof(ItemDrop.ItemData), nameof(ItemDrop.ItemData.GetTooltip), 
		typeof(ItemDrop.ItemData), typeof(int), typeof(bool))]
	public class UpdateFoodDisplay
	{
		public static void Postfix(ItemDrop.ItemData item, bool crafting, ref string __result)
		{
			if ((item.m_shared.m_food > 0 || item.m_shared.m_foodStamina > 0 || item.m_shared.m_foodEitr > 0) &&
				item.IsExtended())
			{
				SaveFoodQuality FQ = item.Extended().GetComponent<SaveFoodQuality>();
				if (FQ == null) return;

				float fq = FQ.foodQuality + 1f;
				if (fq != 1f)
				{
					float newFood = Mathf.Round(fq * item.m_shared.m_food);
					float newFoodStamina = Mathf.Round(fq * item.m_shared.m_foodStamina);
					float newFoodDuration = (fq * item.m_shared.m_foodBurnTime);
					newFoodDuration /= 60;
					newFoodDuration = Mathf.Round(newFoodDuration);
					float newFoodRegen = Mathf.Round(fq * item.m_shared.m_foodRegen);
					float newFoodEitr = Mathf.Round(fq * item.m_shared.m_foodEitr);

					__result = new Regex("(\\$item_food_health.*?</color>)").Replace(__result, 
						$"$1 ({CFG.ColorCookHealthFF}{newFood}{CFG.ColorEnd})");
					__result = new Regex("(\\$item_food_stamina.*?</color>)").Replace(__result, 
						$"$1 ({CFG.ColorCookStaminaFF}{newFoodStamina}{CFG.ColorEnd})");
					__result = new Regex("(\\$item_food_duration.*?</color>)").Replace(__result, 
						$"$1 ({CFG.ColorCookDurationFF}{newFoodDuration}m{CFG.ColorEnd})");
					__result = new Regex("(\\$item_food_eitr.*?</color>)").Replace(__result,
                        $"$1 ({CFG.ColorCookEitrFF}{newFoodEitr}m{CFG.ColorEnd})");
                    __result = new Regex("(\\$item_food_regen.*?</color>)").Replace(__result,
                        $"$1 ({CFG.ColorCookRegenFF}{newFoodRegen}m{CFG.ColorEnd})");

                    __result += "\nQuality: " + PT.Prettify(FQ.foodQuality*100f, 0, PT.TType.TextlessPercent);

					Player player = Player.GetPlayer(FQ.chefID);

					if (player != null)
					{
						__result += $"\nThis meal was prepared by {CFG.ColorKingSkillsFF}{player.GetPlayerName()}{CFG.ColorEnd}";
                    }
					if (FQ.flavorText != null && FQ.flavorText != "")
					{
						__result += $"\n{FQ.flavorText}";
					}
				}
			}
		}

		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			FieldInfo food = AccessTools.DeclaredField(typeof(ItemDrop.ItemData.SharedData), nameof(ItemDrop.ItemData.SharedData.m_food));
			FieldInfo foodStamina = AccessTools.DeclaredField(typeof(ItemDrop.ItemData.SharedData), nameof(ItemDrop.ItemData.SharedData.m_foodStamina));
			FieldInfo foodDuration = AccessTools.DeclaredField(typeof(ItemDrop.ItemData.SharedData), nameof(ItemDrop.ItemData.SharedData.m_foodBurnTime));

			foreach (CodeInstruction instruction in instructions)
			{
				yield return instruction;
				if (instruction.opcode == OpCodes.Ldfld && (instruction.OperandIs(food) || instruction.OperandIs(foodStamina) || instruction.OperandIs(foodDuration)))
				{
					yield return new CodeInstruction(OpCodes.Call, AccessTools.DeclaredMethod(typeof(Mathf), nameof(Mathf.Round)));
				}
			}
		}
	}


	[HarmonyPatch(typeof(CraftingStation), nameof(CraftingStation.CheckUsable))]
	public class CookingLevelRQ
	{
		[HarmonyPrefix]
		public static bool KitchenLevelCheck(CraftingStation __instance, Player player)
		{
			string name = __instance.m_nview.GetPrefabName();

			//Jotunn.Logger.LogMessage($"checked a bench, and its name was {name}");

			if (CFG.GetCookingIsKitchen(name))
				return CheckRQ(name, player);


			//Jotunn.Logger.LogMessage($"that wasn't a kitchen, so we good");

			return true;
		}

		[HarmonyPatch(typeof(CookingStation), nameof(CookingStation.OnInteract))]
		[HarmonyPrefix]
		[HarmonyPriority(1)]
		public static bool MeatLevelCheck(CookingStation __instance)
		{
			string name = __instance.m_nview.GetPrefabName();

			//Jotunn.Logger.LogMessage($"checked a cooking station, and its name was {name}");

			if (CFG.GetCookingIsMeatStation(name))
				return CheckRQ(name, Player.m_localPlayer);

			return false;
		}

		public static bool CheckRQ(string station, Player player)
		{
			float skillLevel = player.GetSkillFactor(SkillMan.Cooking) * CFG.MaxSkillLevel.Value;
			float skillRQ = CFG.GetCookingLevelRQ(station);

			//Jotunn.Logger.LogMessage($"{skillLevel} out of {skillRQ}");

			//You may enter the palace
			if (skillLevel >= skillRQ)
				return true;

			Player.m_localPlayer.Message(MessageHud.MessageType.Center,
				$"Your cooking skill is too low. You need {CFG.ColorPTRedFF}" + skillRQ.ToString("F0") +
				$" cooking{CFG.ColorEnd} to operate this station!");
			return false;
		}
	}
}
