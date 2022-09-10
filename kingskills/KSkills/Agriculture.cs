using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace kingskills.KSkills
{
    /*
     * 
     * agriculture
exp gained:
every item picked up gives you experience, based on the amount
and higher level/better items give you better rewards
farm grown stuff gives much better experience
also get a flat exp reward for each plant planted
trees give you experience when they mature
hovered item exp per 1: 

effects:
increased yield for pickables
reduced grow time on planted foods
increased food quality for pickable foods
heal on picked item
(random crop rotation)

perks:
	Ignore biome
	Show time estimates and yield estimates	
	
100
	Mass Planting
	Mass Harvesting
     * 
     */

    [HarmonyPatch(typeof(Plant), nameof(Plant.Grow))]
    public class TreeGrowWatcher
    {
        [HarmonyPrefix]
        public static void TreeGrowing(Plant __instance)
        {
            if (__instance.m_status != 0) return;
            if (__instance.m_nview.m_zdo.GetLong("Current Botanist", 0) != Player.m_localPlayer.GetPlayerID()) return;
            float reward = CFG.GetAgricultureTreeReward(__instance);
            if (reward == 0) return;

            Player.m_localPlayer.RaiseSkill(SkillMan.Agriculture, reward);
        }
    }


    [HarmonyPatch(typeof(Player), nameof(Player.PlacePiece))]
    public class LocalPlayerPlacing
    {
        public static bool isTrue = false;
        [HarmonyPrefix]
        private static void BeforePlacement() => isTrue = true;
        [HarmonyFinalizer]
        private static void AfterPlacement() => isTrue = false;
    }

    [HarmonyPatch(typeof(Plant), nameof(Plant.Awake))]
    public class SaveSkillLevel
    {
        [HarmonyPostfix]
        private static void AfterPlantWakeup(Plant __instance)
        {
            if (LocalPlayerPlacing.isTrue)
            {
                Player player = Player.m_localPlayer;

                __instance.m_nview.GetZDO().Set("Agriculture Level", 
                    player.GetSkillFactor(SkillMan.Agriculture));
               player.RaiseSkill(SkillMan.Agriculture, CFG.AgricultureXPPlantFlat.Value);
                __instance.m_nview.m_zdo.Set("Current Botanist", player.GetPlayerID());
            }
        }
    }

    [HarmonyPatch(typeof(Plant), nameof(Plant.GetGrowTime))]
    public static class PlantSpeedup
    {
        [HarmonyPostfix]
        public static void GrowTimeAccelerate(Plant __instance, ref float __result)
        {
            ZDO zdo = __instance.m_nview.GetZDO();
            if (zdo.m_floats.ContainsKey("Agriculture Level".GetStableHashCode()))
                __result *= CFG.GetAgricultureGrowTimeRedux(zdo.GetFloat("Agriculture Level"));
        }
    }



    //This class grabs a reference to the player interacting
    // every time a player interacts
    [HarmonyPatch(typeof(Player),nameof(Player.Interact))]
    public static class PlayerPickRef
    {
        public static ZDOID? pickingPlayer = null;
        [HarmonyPrefix]
        public static void RegisterPlayer(Player __instance)
        {
            pickingPlayer = __instance.GetZDOID();
        }
        [HarmonyFinalizer]
        public static void CleanUp()
        {
            pickingPlayer = null;
        }
    }


    [HarmonyPatch(typeof(Pickable), nameof(Pickable.Interact))]
    public static class PickItem
    {
        [HarmonyPrefix]
        public static void BeforeInteract(Pickable __instance, Humanoid character)
        {
            if (!__instance.m_nview.IsValid() ||
                PlayerPickRef.pickingPlayer == null ||
                PlayerPickRef.pickingPlayer != character.GetZDOID() ||
                __instance.m_tarPreventsPicking) return;

            if (!CFG.GetAgricultureIsPlant(__instance)) return;

            float skillF = character.GetSkillFactor(SkillMan.Agriculture);

            //We increase the drop amount
            __instance.m_amount += CFG.GetAgricultureRandomAdditionalYield(skillF, __instance.m_amount);

            //I could also put the quality code in here, but I'm gonna wait until I work that out with cooking

            float expReward = CFG.GetAgriculturePlantReward(__instance.gameObject);

            //We get experience based on the dictionary defined rewards
            if (expReward > 0)
            {
                for (int i = 0; i < __instance.m_amount; i++)
                character.RaiseSkill(SkillMan.Agriculture, expReward);
            }

            //We regain health
            character.Heal(CFG.GetAgricultureHealthRegain(skillF));

            //This piece of code checks to see if this pickable has already had it's respawn timer tinkered with
            //If not, then it tinkers with it
            if (__instance.m_nview.GetZDO().GetFloat("Modified Respawn Time", -20) == -20)
            {
                __instance.m_respawnTimeMinutes = (int)Mathf.Floor(
                        __instance.m_respawnTimeMinutes * CFG.GetAgricultureGrowTimeRedux(skillF));

                __instance.m_nview.GetZDO().Set("Modified Respawn Time", __instance.m_respawnTimeMinutes);
            }
            else
            {
                __instance.m_respawnTimeMinutes = 
                    (int)__instance.m_nview.GetZDO().GetFloat("Modified Respawn Time", -20);
            }
        }
    }
}
