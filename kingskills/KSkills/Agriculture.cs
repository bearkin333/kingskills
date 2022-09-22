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

            long botanistID = __instance.m_nview.m_zdo.GetLong("Current Botanist", 0);
            if (botanistID == 0) return;

            float reward = CFG.GetAgricultureTreeReward(__instance);
            if (reward == 0) return;

            Player botanist = Player.GetPlayer(botanistID);
            if (botanist is null) return;

            RPC.RPCMan.SendXP_RPC(botanist.m_nview, reward, SkillMan.Agriculture);
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

    //This class grabs a reference to the player interacting
    // every time a player interacts
    [HarmonyPatch(typeof(Player), nameof(Player.Interact))]
    public static class LocalPlayerInteracting
    {
        public static bool isTrue = false;
        [HarmonyPrefix]
        private static void BeforeInteract() => isTrue = true;
        [HarmonyFinalizer]
        private static void AfterInteract() => isTrue = false;
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
                ZDO zdo = __instance.m_nview.m_zdo;

                zdo.Set("Agriculture Level", 
                    player.GetSkillFactor(SkillMan.Agriculture));
                player.RaiseSkill(SkillMan.Agriculture, CFG.AgricultureXPPlantFlat.Value);
                zdo.Set("Current Botanist", player.GetPlayerID());
                Jotunn.Logger.LogMessage($"This plant's botanist set to {zdo.GetLong("Current Botanist", 0)}");
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

    [HarmonyPatch(typeof(Pickable), nameof(Pickable.Interact))]
    public static class PickableInteractionSwitch
    {
        [HarmonyPatch(typeof(Pickable), nameof(Pickable.Awake))]
        [HarmonyPostfix]
        public static void RegisterKSRPCS(Pickable __instance)
        {
            if (!__instance.m_nview) return;

            __instance.m_nview.Register<long, ZDOID>("RPC_RequestPickable", RPC_RequestPickable);
        }


        [HarmonyPatch(typeof(Pickable), nameof(Pickable.Interact))]
        [HarmonyPrefix]
        public static void BeforeInteract(Pickable __instance)
        {
            ZNetView nview = __instance.m_nview;
            if (!nview.IsValid()) return;
            if (nview.IsOwner()) return;
            nview.InvokeRPC("RPC_RequestPickable", Game.instance.GetPlayerProfile().GetPlayerID(), 
                __instance.m_nview.m_zdo.m_uid);
        }

        public static void RPC_RequestPickable(long uid, long playerID, ZDOID pickableZDOID)
        {
            ZDOMan.instance.ForceSendZDO(uid, pickableZDOID);
            ZNetScene.instance.FindInstance(pickableZDOID).GetComponent<ZNetView>().m_zdo.SetOwner(uid);
        }
    }

    

    [HarmonyPatch(typeof(Pickable), nameof(Pickable.RPC_Pick))]
    public static class PickItem
    {
        [HarmonyPrefix]
        public static void BeforePick(Pickable __instance, long sender)
        {
            if (!LocalPlayerInteracting.isTrue ||
                !__instance.m_nview.IsOwner() ||
                !CFG.GetAgricultureIsPlant(__instance) ||
               __instance.m_picked)
               return;


            Player player = Player.m_localPlayer;

            float skillF = player.GetSkillFactor(SkillMan.Agriculture);

            //We increase the drop amount
            __instance.m_amount += CFG.GetAgricultureRandomAdditionalYield(skillF, __instance.m_amount);


            float expReward = CFG.GetAgriculturePlantReward(__instance.gameObject);

            //We get experience based on the dictionary defined rewards
            if (expReward > 0)
            {
                for (int i = 0; i < __instance.m_amount; i++)
                player.RaiseSkill(SkillMan.Agriculture, expReward);
            }

            //We regain health
            player.Heal(CFG.GetAgricultureHealthRegain(skillF));

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
                //__instance.m_respawnTimeMinutes = 
                //    (int)__instance.m_nview.GetZDO().GetFloat("Modified Respawn Time", -20);
            }
        }
    }
}
