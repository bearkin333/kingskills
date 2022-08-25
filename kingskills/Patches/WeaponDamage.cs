using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace kingskills
{
    class WeaponDamagePatch
    {
        [HarmonyPatch(typeof(Character))]
        [HarmonyPatch(nameof(Character.RPC_Damage))]
        [HarmonyPrefix]
        public static void PreDamageHitPatch(Character __instance, ref HitData hit)
        {
            Character charac = hit.GetAttacker();
            //If the attacker is the local player 
            if (charac.IsPlayer() && charac.GetZDOID() == Player.m_localPlayer.GetZDOID())
            {
                Player player = (Player)charac;


                //First, we add the randomness back in
                hit.m_damage.Modify(GetRandomAttackMod());

                //Now, we increase damage based on what kind of attack it is.
                //Their solution was more elegant, but unfortunately, if we want to have any
                //attack bonus be different from another one, we're gonna have to iterate through all
                //of them.
                switch (hit.m_skill)
                {
                    case Skills.SkillType.Swords:
                        hit.m_damage.Modify(
                            ConfigManager.GetSwordDamageMod(player.GetSkillFactor(Skills.SkillType.Swords)));
                        break;
                    case Skills.SkillType.Axes:
                        hit.m_damage.Modify(
                            ConfigManager.GetAxeDamageMod(player.GetSkillFactor(Skills.SkillType.Axes)));
                        break;
                    case Skills.SkillType.Bows:
                        hit.m_damage.Modify(
                            ConfigManager.GetBowDamageMod(player.GetSkillFactor(Skills.SkillType.Bows)));
                        break;
                    case Skills.SkillType.Clubs:
                        hit.m_damage.Modify(
                            ConfigManager.GetClubDamageMod(player.GetSkillFactor(Skills.SkillType.Clubs)));
                        break;
                    case Skills.SkillType.Unarmed: //Unarmed also gets a flat damage bonus
                        hit.m_damage.m_blunt += 
                            ConfigManager.GetFistDamageFlat(player.GetSkillFactor(Skills.SkillType.Unarmed));
                        hit.m_damage.Modify(
                            ConfigManager.GetFistDamageMod(player.GetSkillFactor(Skills.SkillType.Unarmed)));
                        break;
                    case Skills.SkillType.Knives:
                        hit.m_damage.Modify(
                            ConfigManager.GetKnifeDamageMod(player.GetSkillFactor(Skills.SkillType.Knives)));
                        break;
                    case Skills.SkillType.Polearms:
                        hit.m_damage.Modify(
                            ConfigManager.GetPolearmDamageMod(player.GetSkillFactor(Skills.SkillType.Polearms)));
                        break;
                    case Skills.SkillType.Spears:
                        hit.m_damage.Modify(
                            ConfigManager.GetSpearDamageMod(player.GetSkillFactor(Skills.SkillType.Spears)));
                        break;
                }

                //Increase sneak attack damage for knife skill
                hit.m_backstabBonus *=
                    ConfigManager.GetKnifeBackstabMod(player.GetSkillFactor(Skills.SkillType.Knives));

                //The generic bonuses from your various skill levels
                hit.m_damage.m_blunt *=
                    ConfigManager.GetClubBluntMod(player.GetSkillFactor(Skills.SkillType.Clubs));
                hit.m_damage.m_slash *=
                    ConfigManager.GetSwordSlashMod(player.GetSkillFactor(Skills.SkillType.Swords));
                hit.m_damage.m_pierce *=
                    ConfigManager.GetKnifeDamageMod(player.GetSkillFactor(Skills.SkillType.Knives));

                //The knockback and stagger bonuses from club levels
                hit.m_pushForce *=
                    ConfigManager.GetClubKnockbackMod(player.GetSkillFactor(Skills.SkillType.Clubs));
                hit.m_staggerMultiplier *=
                    ConfigManager.GetClubStaggerMod(player.GetSkillFactor(Skills.SkillType.Clubs));


            } 
            //Or if the attacked is the local player
            else if (__instance.IsPlayer() && __instance.GetZDOID() == Player.m_localPlayer.GetZDOID())
            {
                Player player = (Player)__instance;

                //We can do stuff here probably. Will we need to? Who knows.
            }

        }


        [HarmonyPostfix]
        [HarmonyPatch(typeof(MineRock), nameof(MineRock.Damage))]
        static void MineRock_Damage(Destructible __instance, HitData hit)
        {
            ResourceDamagePatch(__instance, ref hit);
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MineRock5), nameof(MineRock5.Damage))]
        static void MineRock5_Damage(Destructible __instance, HitData hit)
        {
            ResourceDamagePatch(__instance, ref hit);
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(TreeBase), nameof(TreeBase.Damage))]
        static void TreeBase_Damage(Destructible __instance, HitData hit)
        {
            ResourceDamagePatch(__instance, ref hit);
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(TreeLog), nameof(TreeLog.Damage))]
        static void TreeLog_Damage(Destructible __instance, HitData hit)
        {
            ResourceDamagePatch(__instance, ref hit);
        }

        public static void ResourceDamagePatch(Destructible __instance, ref HitData hit)
        {
            Jotunn.Logger.LogMessage("Hitting a resource patch detected!");
            if (hit.m_attacker == Player.m_localPlayer.GetZDOID())
            {
                Player player = Player.m_localPlayer;
                if (hit.m_skill == Skills.SkillType.WoodCutting)
                {
                    hit.m_damage.m_chop *=
                        ConfigManager.GetWoodcuttingDamageMod(player.GetSkillFactor(Skills.SkillType.WoodCutting))
                        * ConfigManager.GetAxeChopDamageMod(player.GetSkillFactor(Skills.SkillType.Axes));
                    player.AddStamina(ConfigManager.GetWoodcuttingStaminaRebate(player.GetSkillFactor(Skills.SkillType.WoodCutting)));
                } 
                else if (hit.m_skill == Skills.SkillType.Pickaxes)
                {
                    hit.m_damage.m_pickaxe *=
                        ConfigManager.GetMiningDamageMod(player.GetSkillFactor(Skills.SkillType.Pickaxes));
                    player.AddStamina(ConfigManager.GetMiningStaminaRebate(player.GetSkillFactor(Skills.SkillType.Pickaxes)));
                }
            }
        }

        [HarmonyPatch(typeof(Character))]
        [HarmonyPatch(nameof(Character.GetRandomSkillFactor))]
        [HarmonyPrefix]
        public static bool PlayerDisarmPatch(Player __instance, ref float __result)
        {
            //This piece of code stops the player from generating a random amount of damage based on their
            //skill level in a weapon. In return, we do that later on with our code.
            //We could do it here, but this automatically counts for the "knockback" code as well, which
            //we want to do on our own.
            __result = 1;
            return false;
        }

        public static float GetRandomAttackMod()
        {
            return UnityEngine.Random.Range(0.85f, 1.15f);
        }

    }
    

}
