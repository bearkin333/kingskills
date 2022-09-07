using HarmonyLib;
using kingskills.WeaponExperience;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace kingskills
{

    [HarmonyPatch(typeof(Character))]
    class StaggerWatch
    {
        //For transferring stagger experience check
        static Player playerRef = null;
        static bool staggerFlag = false;

        [HarmonyPatch(nameof(Character.ApplyDamage))]
        [HarmonyPrefix]
        private static void OnDamageTrigger(Character __instance, HitData hit)
        {
            if (Player.m_localPlayer == null) return;
            //Jotunn.Logger.LogMessage($"An apply damage function has run, and I'm catching a hit. the hit says");
            ZDOID player = hit.m_attacker;
            //Jotunn.Logger.LogMessage($"{player.ToString()} is the one perpetrating this attack");

            if (Player.m_localPlayer.GetZDOID().Equals(player))
            {
                //Jotunn.Logger.LogMessage($"A player hit someone");
                playerRef = Player.m_localPlayer;

                if (__instance.IsStaggering())
                {
                    staggerFlag = false;
                    OnStaggerHurt(playerRef);
                }
                else
                {
                    staggerFlag = true;
                }

            }
        }

        [HarmonyPatch(nameof(Character.RPC_Damage))]
        [HarmonyPrefix]
        private static void OnDamageRPCCall(Character __instance, long sender, HitData hit)
        {
            ZDOID player = hit.m_attacker;

            if (Player.m_localPlayer.GetZDOID().Equals(player))
            {
                Player attacker = Player.m_localPlayer;

                if (__instance.m_baseAI != null)
                {
                    if (!__instance.m_baseAI.IsAlerted() && hit.m_backstabBonus > 1f && Time.time - __instance.m_backstabTime > 300f)
                    {
                        //Jotunn.Logger.LogMessage($"I am pretty sure I just got backstabbed");
                        OnBackstab(attacker);
                    }
                }
            }
        }

        [HarmonyPatch(nameof(Character.RPC_Stagger))]
        [HarmonyPostfix]
        private static void StaggerPostFix(Character __instance)
        {
            if (staggerFlag)
            {
                if (Util.GetPlayerWeapon(playerRef).m_shared.m_skillType == Skills.SkillType.Clubs)
                    LevelUp.BXP(playerRef, Skills.SkillType.Clubs, ConfigMan.WeaponBXPClubStagger.Value);
                        
                staggerFlag = false;
            }
        }

        private static void OnStaggerHurt(Player attacker)
        {
            if (Util.GetPlayerWeapon(attacker).m_shared.m_skillType == Skills.SkillType.Swords)
                LevelUp.BXP(attacker, Skills.SkillType.Swords, ConfigMan.WeaponBXPSwordStagger.Value);
        }

        private static void OnBackstab(Player attacker)
        {
            if (Util.GetPlayerWeapon(attacker).m_shared.m_skillType == Skills.SkillType.Knives)
                LevelUp.BXP(attacker, Skills.SkillType.Swords, ConfigMan.WeaponBXPKnifeBackstab.Value);
        }
    }

    [HarmonyPatch(typeof(TreeLog))]
    class AxeLogWatch
    {
        [HarmonyPatch(nameof(TreeLog.Destroy))]
        [HarmonyPrefix]
        public static void TreeLogDestroyPatch(TreeLog __instance)
        {
            if (!ConfigMan.IsSkillActive(Skills.SkillType.Axes)) return;

            //Jotunn.Logger.LogMessage($"This log is killed. Closest player's getting the exp");
            Player closestPlayer = Player.GetClosestPlayer(__instance.m_body.transform.position, 
                ConfigMan.WeaponBXPAxeRange.Value);

            if (closestPlayer != null)
                if (Util.GetPlayerWeapon(closestPlayer).m_shared.m_skillType == Skills.SkillType.Axes)
                    LevelUp.BXP(closestPlayer, Skills.SkillType.Axes, ConfigMan.WeaponBXPAxeTreeAmount.Value);
        }

        [HarmonyPatch(typeof(Destructible),nameof(Destructible.Destroy))]
        [HarmonyPrefix]
        public static void StubDestroyPatch(Destructible __instance)
        {
            if (!ConfigMan.IsSkillActive(Skills.SkillType.WoodCutting)) return;
            if (!__instance.gameObject.name.Contains("Stub")) return;

            Player closestPlayer = Player.GetClosestPlayer(__instance.gameObject.transform.position, 
                ConfigMan.WeaponBXPAxeRange.Value);

            if (closestPlayer != null)
                if (Util.GetPlayerWeapon(closestPlayer).m_shared.m_skillType == Skills.SkillType.Axes)
                    LevelUp.BXP(closestPlayer, Skills.SkillType.WoodCutting, ConfigMan.ToolBXPWoodStubReward.Value);
        }
    }
}
