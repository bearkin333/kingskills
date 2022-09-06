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
    class SwordStaggerWatch
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
                //Jotunn.Logger.LogMessage($"Stagger flag redeemed! Turned back off");

                if ( ConfigMan.IsSkillActive(Skills.SkillType.Clubs) && 
                    Util.GetPlayerWeapon(playerRef).m_shared.m_skillType == Skills.SkillType.Clubs)
                {
                    playerRef.RaiseSkill(Skills.SkillType.Clubs, ConfigMan.WeaponBXPClubStagger.Value);
                    CustomWorldTextManager.CreateBXPText(
                        CustomWorldTextManager.GetInFrontOfCharacter(playerRef),
                        ConfigMan.WeaponBXPClubStagger.Value);
                }
                staggerFlag = false;
            }
        }

        private static void OnStaggerHurt(Player attacker)
        {
            if (ConfigMan.IsSkillActive(Skills.SkillType.Swords) &&
                    Util.GetPlayerWeapon(attacker).m_shared.m_skillType == Skills.SkillType.Swords)
            {
                attacker.RaiseSkill(Skills.SkillType.Swords, ConfigMan.WeaponBXPSwordStagger.Value);

                CustomWorldTextManager.CreateBXPText(
                    CustomWorldTextManager.GetInFrontOfCharacter(attacker),
                    ConfigMan.WeaponBXPSwordStagger.Value);
                //Jotunn.Logger.LogMessage($"A player just hit us with a sword while we were staggered, so applying bonus exp");
            }
        }

        private static void OnBackstab(Player attacker)
        {
            if (ConfigMan.IsSkillActive(Skills.SkillType.Knives) &&
                    Util.GetPlayerWeapon(attacker).m_shared.m_skillType == Skills.SkillType.Knives)
            {
                attacker.RaiseSkill(Skills.SkillType.Swords, ConfigMan.WeaponBXPKnifeBackstab.Value);

                CustomWorldTextManager.CreateBXPText(
                    CustomWorldTextManager.GetInFrontOfCharacter(attacker),
                    ConfigMan.WeaponBXPKnifeBackstab.Value);
                //Jotunn.Logger.LogMessage($"A player just backstabbed us with a knife, so +exp");
            }
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
            {
                if (Util.GetPlayerWeapon(closestPlayer).m_shared.m_skillType == Skills.SkillType.Axes)
                {
                    closestPlayer.RaiseSkill(Skills.SkillType.Axes, ConfigMan.WeaponBXPAxeTreeAmount.Value);
                    CustomWorldTextManager.CreateBXPText(
                        CustomWorldTextManager.GetInFrontOfCharacter(closestPlayer), 
                        ConfigMan.WeaponBXPAxeTreeAmount.Value);
                }

            }
        }

        [HarmonyPatch(typeof(Destructible),nameof(Destructible.Destroy))]
        [HarmonyPrefix]
        public static void StubDestroyPatch(Destructible __instance)
        {
            if (!ConfigMan.IsSkillActive(Skills.SkillType.WoodCutting)) return;
            if (!__instance.gameObject.name.Contains("Stub")) return;

            //Jotunn.Logger.LogMessage("Detected stub destroyed?");

            Player closestPlayer = Player.GetClosestPlayer(__instance.gameObject.transform.position, 
                ConfigMan.WeaponBXPAxeRange.Value);

            if (closestPlayer != null)
            {
                if (Util.GetPlayerWeapon(closestPlayer).m_shared.m_skillType == Skills.SkillType.Axes)
                {
                    closestPlayer.RaiseSkill(Skills.SkillType.WoodCutting, ConfigMan.ToolBXPWoodStubReward.Value);
                    CustomWorldTextManager.CreateBXPText(
                        CustomWorldTextManager.GetInFrontOfCharacter(closestPlayer),
                        ConfigMan.ToolBXPWoodStubReward.Value);
                }

            }
        }
    }
}
