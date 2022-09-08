using ExtendedItemDataFramework;
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
				if ((LocalChefCooking.isTrue || LocalChefInvCooking.isTrue) &&
				(itemMade.m_shared.m_food > 0 || itemMade.m_shared.m_foodStamina > 0))
                {
					SaveFoodQuality FQ = itemMade.AddComponent<SaveFoodQuality>();
					Player player = Player.m_localPlayer;

					float cookingTime = 1.25f;
					float cookingFinished = 1f;
					float cookingPeakTime = 1.5f;

					//Will be more interesting later
					if (ThrowItem.throwing)
					{
						cookingTime = ThrowItem.cookTime;
						cookingFinished = ThrowItem.cookFinishedTime;
						cookingPeakTime = cookingFinished * 1.5f;
					}

					float timingPercent = 1f - Mathf.Abs(cookingPeakTime - cookingTime)/(cookingFinished/2);

					Jotunn.Logger.LogMessage($"Your cooking rating was {timingPercent*100}%");

					float skillFactor;
					if (player != null)
					{
						skillFactor = player.GetSkillFactor(SkillMan.Cooking);
					}
                    else
                    {
						skillFactor = 0;
                    }

					float newQuality = ConfigMan.GetCookingRandomFQ(skillFactor, timingPercent);

					FQ.foodQuality = newQuality;

					FQ.chefID = player.GetPlayerID();
					Jotunn.Logger.LogMessage($"saved the chef as {FQ.chefID}");
				}
			};
        }
	}


	[HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.DoCrafting))]
	public class LocalChefInvCooking
	{
		public static bool isTrue = false;

		[HarmonyPrefix]
		public static void Prefix() => isTrue = true;

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

			Jotunn.Logger.LogMessage($" recording the player's cooking skill as {(user as Player).GetSkillFactor(SkillMan.Cooking)}");
			zdo.Set("Cooking Skill", (user as Player).GetSkillFactor(SkillMan.Cooking));

			(user as Player).RaiseSkill(SkillMan.Cooking, ConfigMan.GetCookingXP(__instance));
		}

		[HarmonyPatch(nameof(CookingStation.GetItemConversion))]
		[HarmonyPostfix]
		public static void ReduceCookTime(CookingStation __instance, ref CookingStation.ItemConversion __result)
		{
			ZDO zdo = __instance.m_nview.m_zdo;
			if (zdo == null) return;

			float recordedSkill = zdo.GetFloat("Cooking Skill", 0);
			//Jotunn.Logger.LogMessage($"Reading my zdo says cooking skill is {zdo.GetFloat("Cooking Skill", 0)}");
			if (recordedSkill > 0)
			{
				__result.m_cookTime *= ConfigMan.GetCookingTimeRedux(zdo.GetFloat("Cooking Skill", 0));
				//Jotunn.Logger.LogMessage($"Cook time is now {__result.m_cookTime}. removing the cooking skill float.");
				zdo.Set("Cooking Skill", 0f);
			}
		}


		[HarmonyPatch(typeof(Fermenter),nameof(Fermenter.AddItem))]
		[HarmonyPostfix]
		public static void ReduceFermentTime(Fermenter __instance, Humanoid user, ref bool __result)
		{
			if (!__result) return;
			ZDO zdo = __instance.m_nview.m_zdo;
			if (zdo == null) return;

			float recordedRedux = zdo.GetFloat("Ferment Redux", 0f);
			float recordedSkill = zdo.GetFloat("Cooking Level", 0f);
			long recordedChefID = zdo.GetLong("Current Chef", 0);

			//If no one has claimed it yet, we apply their time reduction to the process.
			if (recordedChefID == 0)
            {
				float redux = ConfigMan.GetCookingFermentTimeRedux(user.GetSkillFactor(SkillMan.Cooking));
				__instance.m_fermentationDuration *= redux;

				zdo.Set("Ferment Redux", redux);
				zdo.Set("Cooking Level", user.GetSkillFactor(SkillMan.Cooking));
				zdo.Set("Current Chef", user.GetZDOID().m_userID);
			}
			//If another player besides us has claimed it, we overwrite them.
			//Or if our chef skill has changed.
			//We have to undo the previous reduction first, though.
			else if (recordedChefID != user.GetZDOID().m_userID ||
				user.GetSkillFactor(SkillMan.Cooking) != recordedSkill)
			{
				__instance.m_fermentationDuration /= zdo.GetFloat("Ferment Redux", 0f);

				float redux = ConfigMan.GetCookingFermentTimeRedux(user.GetSkillFactor(SkillMan.Cooking));
				__instance.m_fermentationDuration *= redux;

				zdo.Set("Ferment Redux", redux);
				zdo.Set("Cooking Level", user.GetSkillFactor(SkillMan.Cooking));
				zdo.Set("Current Chef", user.GetZDOID().m_userID);
			}
		}
	}


	[HarmonyPatch(typeof(CookingStation), nameof(CookingStation.RPC_RemoveDoneItem))]
	public class ThrowItem
	{
		public static float cookTime = 0;
		public static float cookFinishedTime = 0;
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
					cookTime = timing;
					cookFinishedTime = __instance.GetItemConversion(itemName).m_cookTime;

					Jotunn.Logger.LogMessage($"Grabbed a cook time of {timing}. " +
						$"item finishes at {__instance.GetItemConversion(itemName).m_cookTime}");

					if (LocalChefCooking.isTrue)
					{
						Player player = Player.m_localPlayer;

						player.RaiseSkill(SkillMan.Cooking, ConfigMan.GetCookingXP(__instance, false));
					}
					break;
				}
			}
		}
		[HarmonyFinalizer]
		public static void DisposeCookTimes(CookingStation __instance)
		{
			throwing = false;
			cookFinishedTime = 0;
			cookTime = 0; 
		}
	}


	[HarmonyPatch(typeof(Player), nameof(Player.EatFood))]
	public class ApplyCookingBuffs
	{
		public static float food = 0;
		public static float foodStam = 0;
		public static float foodDuration = 0;
		[HarmonyPrefix]
		public static void TemporaryFQBoost(Player __instance, ItemDrop.ItemData item, bool __result)
		{
			if (item.m_shared.m_food <= 0 || item.m_shared.m_foodStamina <= 0) return;
			if (item.IsExtended())
            {
				SaveFoodQuality FQ = item.Extended().GetComponent<SaveFoodQuality>();

				if (FQ == null) return;
				if (FQ?.foodQuality == null) FQ.foodQuality = 0;

				Jotunn.Logger.LogMessage($"Eating this yum yum food and its quality is {FQ.foodQuality}");
				float foodMult = 1f + FQ.foodQuality;

				food = item.m_shared.m_food;
				foodStam = item.m_shared.m_foodStamina;
				foodDuration = item.m_shared.m_foodBurnTime;

				item.m_shared.m_food = Mathf.Floor(item.m_shared.m_food * foodMult);
				item.m_shared.m_foodStamina = Mathf.Floor(item.m_shared.m_foodStamina * foodMult);
				item.m_shared.m_foodBurnTime = Mathf.Floor(item.m_shared.m_foodBurnTime * foodMult);

				float chefXPReward = item.m_shared.m_food + item.m_shared.m_foodStamina;
				chefXPReward *= ConfigMan.CookingXPStatMod.Value;

				Player chef = Player.GetPlayer(FQ.chefID);

				if (chef != null)
                {
					Jotunn.Logger.LogMessage($"chef was found as a player");
					if (__instance.GetZDOID().m_userID != FQ.chefID)
						chefXPReward *= ConfigMan.GetCookingXPCustomerMult();

					chef.RaiseSkill(SkillMan.Cooking, chefXPReward);
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
			if (item.m_shared.m_food > 0 && item.m_shared.m_foodStamina > 0)
			{
				SaveFoodQuality FQ = item.Extended().GetComponent<SaveFoodQuality>();
				if (FQ == null) return;

				float fq = FQ.foodQuality;
				if (fq != 0)
				{
					float newFood = Mathf.Round((fq + 1f) * item.m_shared.m_food);
					float newFoodStamina = Mathf.Round((fq + 1f) * item.m_shared.m_foodStamina);
					float newFoodDuration = (fq + 1f) * item.m_shared.m_foodBurnTime;
					newFoodDuration /= 60;
					newFoodDuration = Mathf.Round(newFoodDuration);

					__result = new Regex("(\\$item_food_health.*?</color>)").Replace(__result, 
						$"$1 ({ConfigMan.CookHealthColor}{newFood}{ConfigMan.EndColor})");
					__result = new Regex("(\\$item_food_stamina.*?</color>)").Replace(__result, 
						$"$1 ({ConfigMan.CookStaminaColor}{newFoodStamina}{ConfigMan.EndColor})");
					__result = new Regex("(\\$item_food_duration.*?</color>)").Replace(__result, 
						$"$1 ({ConfigMan.CookDurationColor}{newFoodDuration}m{ConfigMan.EndColor})");

					__result += "\nQuality: " + PT.Prettify(FQ.foodQuality*100f, 0, PT.TType.TextlessPercent);

					Player player = Player.GetPlayer(FQ.chefID);

					if (player != null)
					{
						__result += $"\nThis meal was prepared by {ConfigMan.CookDurationColor}{player.GetPlayerName()}{ConfigMan.EndColor}";
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
}
