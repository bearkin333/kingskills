using HarmonyLib;
using kingskills.WeaponExperience;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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

                if ( ConfigManager.IsSkillActive(Skills.SkillType.Clubs) && 
                    Util.GetPlayerWeapon(playerRef).m_shared.m_skillType == Skills.SkillType.Clubs)
                {
                    playerRef.RaiseSkill(Skills.SkillType.Clubs, ConfigManager.WeaponBXPClubStagger.Value);
                    BonusExperienceDamageText.CreateBXPText(
                        BonusExperienceDamageText.GetInFrontOfCharacter(playerRef),
                        ConfigManager.WeaponBXPClubStagger.Value);
                }
                staggerFlag = false;
            }
        }

        private static void OnStaggerHurt(Player attacker)
        {
            if (ConfigManager.IsSkillActive(Skills.SkillType.Swords) &&
                    Util.GetPlayerWeapon(attacker).m_shared.m_skillType == Skills.SkillType.Swords)
            {
                attacker.RaiseSkill(Skills.SkillType.Swords, ConfigManager.WeaponBXPSwordStagger.Value);

                BonusExperienceDamageText.CreateBXPText(
                    BonusExperienceDamageText.GetInFrontOfCharacter(attacker),
                    ConfigManager.WeaponBXPSwordStagger.Value);
                //Jotunn.Logger.LogMessage($"A player just hit us with a sword while we were staggered, so applying bonus exp");
            }
        }

        private static void OnBackstab(Player attacker)
        {
            if (ConfigManager.IsSkillActive(Skills.SkillType.Knives) &&
                    Util.GetPlayerWeapon(attacker).m_shared.m_skillType == Skills.SkillType.Knives)
            {
                attacker.RaiseSkill(Skills.SkillType.Swords, ConfigManager.WeaponBXPKnifeBackstab.Value);

                BonusExperienceDamageText.CreateBXPText(
                    BonusExperienceDamageText.GetInFrontOfCharacter(attacker),
                    ConfigManager.WeaponBXPKnifeBackstab.Value);
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
            if (!ConfigManager.IsSkillActive(Skills.SkillType.Axes)) return;

                //Jotunn.Logger.LogMessage($"This log is killed. Closest player's getting the exp");
                Player closestPlayer = Player.GetClosestPlayer(__instance.m_body.transform.position, ConfigManager.WeaponBXPAxeRange.Value);
            if (closestPlayer != null)
            {
                if (Util.GetPlayerWeapon(closestPlayer).m_shared.m_skillType == Skills.SkillType.Axes)
                {
                    closestPlayer.RaiseSkill(Skills.SkillType.Axes, ConfigManager.WeaponBXPAxeTreeAmount.Value);
                    BonusExperienceDamageText.CreateBXPText(
                        BonusExperienceDamageText.GetInFrontOfCharacter(closestPlayer), 
                        ConfigManager.WeaponBXPAxeTreeAmount.Value);
                }

            }
        }
    }

    [HarmonyPatch]
    class BonusExperienceDamageText
    {
        public static void CreateBXPText(Vector3 pos, float number)
        {
            Jotunn.Logger.LogMessage("Created Bonus Experience text!");
            DamageText.instance.ShowText(DamageText.TextType.Normal, pos, number, true);
        }

        public const float InFrontRange = 1.5f;
        public static Vector3 GetInFrontOfCharacter(Character character)
        {
            return character.transform.position + character.m_lookDir * InFrontRange;
        }

        [HarmonyPatch(typeof(DamageText))]
        [HarmonyPatch(nameof(DamageText.UpdateWorldTexts))]
        [HarmonyPrefix]
        public static void AddWorldBXPText()
        {

        }
    }
}
