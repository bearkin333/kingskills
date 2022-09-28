using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using kingskills.Patches;
using kingskills.RPC;
using kingskills.Perks;

namespace kingskills.Weapons
{

    [HarmonyPatch(typeof(Character))]
    class StaggerWatch
    {
        [HarmonyPatch(nameof(Character.ApplyDamage))]
        [HarmonyPostfix]
        private static void CheckForStaggerHit(Character __instance, HitData hit)
        {
            Character player = hit.GetAttacker();
            if (player == null || !player.IsPlayer()) return;

            if (__instance.IsStaggering())
                RPCMan.SendXP_RPC(player.m_nview,
                    CFG.WeaponBXPSwordStagger.Value, Skills.SkillType.Swords, true, true);
        }

        [HarmonyPatch(nameof(Character.RPC_Damage))]
        [HarmonyPrefix]
        private static void CheckForBackstab(Character __instance, long sender, HitData hit)
        {
            Character player = hit.GetAttacker();
            if (player == null || !player.IsPlayer()) return;

            if (__instance.m_baseAI != null &&
                    !__instance.m_baseAI.IsAlerted() &&
                     hit.m_backstabBonus > 1f &&
                     Time.time - __instance.m_backstabTime > 300f)
            {
                RPCMan.SendXP_RPC(player.m_nview,
                    CFG.WeaponBXPKnifeBackstab.Value, Skills.SkillType.Knives, true, true);
            }
            
        }

        [HarmonyPatch(nameof(Character.RPC_Stagger))]
        [HarmonyPrefix]
        private static void CheckForStaggeredByPlayer(Character __instance)
        {
            ZNetView nview = __instance.m_nview;
            //if (!nview.IsOwner()) return;
            Jotunn.Logger.LogMessage($"Stagger flag for {__instance.m_name}: {nview.m_zdo.GetBool(CFG.ZDOStaggerFlag, false)}");

            if (__instance.IsStaggering()) return;
            if (!nview.m_zdo.GetBool(CFG.ZDOStaggerFlag, false)) return;

            nview.m_zdo.Set(CFG.ZDOStaggerFlag, false);

                
            Player attacker = Player.GetPlayer(nview.m_zdo.GetLong(CFG.ZDOKiller, 0));
            if (attacker is null) return;
                
            P_ClosingTheGap.GapClose(__instance, attacker);
                
            RPCMan.SendXP_RPC(attacker.m_nview,CFG.GetClubBXPStagger(__instance.GetMaxHealth()), Skills.SkillType.Clubs, true, true);
        }
    }

    [HarmonyPatch]
    class TreeWatch
    {
        [HarmonyPatch(typeof(MineRock), nameof(MineRock.AllDestroyed)), HarmonyPostfix]
        static void MineRock_Destroy(MineRock __instance, bool __result)
        {
            if (__result) ResourceDestroyed(__instance, __instance.m_nview);
        }

        [HarmonyPatch(typeof(MineRock5), nameof(MineRock5.AllDestroyed)), HarmonyPostfix]
        static void MineRock5_Destroy(MineRock5 __instance, bool __result)
        {
            if (__result) ResourceDestroyed(__instance, __instance.m_nview);
        }
        /*
        [HarmonyPatch(typeof(TreeBase), nameof(TreeBase.Destroy))]
        [HarmonyPrefix]
        static void TreeBase_Destroy(TreeBase __instance)
        {
            Jotunn.Logger.LogMessage($"{__instance.gameObject.name} was destroyed");
            ResourceDestroyed(__instance, __instance.m_nview);
        }
        */
        [HarmonyPatch(typeof(TreeLog), nameof(TreeLog.Destroy)), HarmonyPrefix]
        static void TreeLog_Destroy(TreeLog __instance)
        {
            ResourceDestroyed(__instance, __instance.m_nview);
        }

        [HarmonyPatch(typeof(Destructible), nameof(Destructible.Destroy)), HarmonyPrefix]
        static void Destructible_Destroy(Destructible __instance)
        {
            ResourceDestroyed(__instance, __instance.m_nview, __instance.gameObject.name);
        }

        public static void ResourceDestroyed(IDestructible __instance, ZNetView nview, string name = "")
        {
            //Jotunn.Logger.LogMessage($"{name} was destroyed");

            Player killer = null;
            if (!CFG.GetKillerExists(nview, ref killer)) return;

            //Jotunn.Logger.LogMessage($"killer was found as a player {killer.GetPlayerName()}");

            if (name.Contains("Stub"))
            {
                RPCMan.SendXP_RPC(killer.m_nview,
                    CFG.ToolBXPWoodStubReward.Value, Skills.SkillType.WoodCutting, true, true);
            }
            else if (__instance.GetType() == typeof(TreeLog))
            {
                RPCMan.SendXP_RPC(killer.m_nview,
                    CFG.WeaponBXPAxeTreeAmount.Value, Skills.SkillType.Axes, true, true);
            }
        }

    }
}
