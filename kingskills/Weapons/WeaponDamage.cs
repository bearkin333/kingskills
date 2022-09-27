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
using kingskills.RPC;
using kingskills.Perks;

namespace kingskills.Weapons
{
    [HarmonyPatch]
    class WeaponDamagePatch
    {
        [HarmonyPatch(typeof(Character), nameof(Character.Damage))]
        [HarmonyPrefix]
        public static void ModifyCharacterDamage(Character __instance, ref HitData hit)
        {
            //Jotunn.Logger.LogMessage($"Damage function running");

            if (!CheckHitGood(hit)) return;

            Character charac = hit.GetAttacker();
            //If the attacker is the local player 
            /*
            Jotunn.Logger.LogMessage($"The attacker is a player = {charac.IsPlayer()}");
            Jotunn.Logger.LogMessage($"The local player's ZDOID is = {Player.m_localPlayer.GetZDOID()}");
            Jotunn.Logger.LogMessage($"The attacker's ZDOID is = {charac.GetZDOID()}");
            */
            if (charac.IsPlayer() && charac.GetZDOID() == Player.m_localPlayer.GetZDOID())
            {
                Player player = (Player)charac;

                ZNetView nview = __instance.m_nview;

                RPCMan.IAmKiller_RPC(nview);

                //Jotunn.Logger.LogMessage($"This attack begins as having {hit.m_damage.GetTotalDamage()} damage");
                //First, we add the randomness back in
                hit.m_damage.Modify(GetRandomAttackMod());

                //Jotunn.Logger.LogMessage($"After randomness, it's {hit.m_damage.GetTotalDamage()} damage");
                //Now, we increase damage based on what kind of attack it is.
                //Their solution was more elegant, but unfortunately, if we want to have any
                //attack bonus be different from another one, we're gonna have to iterate through all
                //of them.
                if (CFG.IsSkillActive(hit.m_skill))
                {
                    switch (hit.m_skill)
                    {
                        case Skills.SkillType.Swords:
                            hit.m_damage.Modify(
                                CFG.GetSwordDamageMult(player.GetSkillFactor(Skills.SkillType.Swords)));
                            break;
                        case Skills.SkillType.Axes:
                            hit.m_damage.Modify(
                                CFG.GetAxeDamageMult(player.GetSkillFactor(Skills.SkillType.Axes)));
                            break;
                        case Skills.SkillType.Bows:
                            hit.m_damage.Modify(
                                CFG.GetBowDamageMult(player.GetSkillFactor(Skills.SkillType.Bows)));
                            break;
                        case Skills.SkillType.Clubs:
                            hit.m_damage.Modify(
                                CFG.GetClubDamageMult(player.GetSkillFactor(Skills.SkillType.Clubs)));
                            break;
                        case Skills.SkillType.Unarmed: //Unarmed also gets a flat damage bonus
                            hit.m_damage.m_blunt +=
                                CFG.GetFistDamageFlat(player.GetSkillFactor(Skills.SkillType.Unarmed));
                            hit.m_damage.Modify(
                                CFG.GetFistDamageMult(player.GetSkillFactor(Skills.SkillType.Unarmed)));
                            break;
                        case Skills.SkillType.Knives:
                            hit.m_damage.Modify(
                                CFG.GetKnifeDamageMult(player.GetSkillFactor(Skills.SkillType.Knives)));
                            break;
                        case Skills.SkillType.Polearms:
                            hit.m_damage.Modify(
                                CFG.GetPolearmDamageMult(player.GetSkillFactor(Skills.SkillType.Polearms)));
                            break;
                        case Skills.SkillType.Spears:
                            hit.m_damage.Modify(
                                CFG.GetSpearDamageMult(player.GetSkillFactor(Skills.SkillType.Spears)));
                            break;
                    }
                }

                //Jotunn.Logger.LogMessage($"Now, thanks to the skill, it's {hit.m_damage.GetTotalDamage()} damage");

                //Jotunn.Logger.LogMessage($"Sneak attack bonus was {hit.m_backstabBonus}");
                //Increase sneak attack damage for knife skill
                if (CFG.IsSkillActive(Skills.SkillType.Knives)) 
                { 
                    hit.m_backstabBonus *=
                        CFG.GetKnifeBackstabMult(player.GetSkillFactor(Skills.SkillType.Knives));
                    hit.m_damage.m_pierce *=
                        CFG.GetKnifeDamageMult(player.GetSkillFactor(Skills.SkillType.Knives));
                }

                //Jotunn.Logger.LogMessage($"Now it's {hit.m_backstabBonus}");

                //The generic bonuses from your various skill levels
                if (CFG.IsSkillActive(Skills.SkillType.Swords))
                    hit.m_damage.m_slash *=
                        CFG.GetSwordSlashMult(player.GetSkillFactor(Skills.SkillType.Swords));


                //Jotunn.Logger.LogMessage($"Pushforce and stagger damage go from {hit.m_pushForce} and {hit.m_damage.GetTotalStaggerDamage()}");
                //The knockback and stagger bonuses from club levels
                if (CFG.IsSkillActive(Skills.SkillType.Clubs))
                {
                    hit.m_damage.m_blunt *=
                           CFG.GetClubBluntMult(player.GetSkillFactor(Skills.SkillType.Clubs));
                    hit.m_pushForce *=
                        CFG.GetClubKnockbackMult(player.GetSkillFactor(Skills.SkillType.Clubs));
                    hit.m_staggerMultiplier *=
                        CFG.GetClubStaggerMult(player.GetSkillFactor(Skills.SkillType.Clubs));
                }
                //Jotunn.Logger.LogMessage($"Now, thanks to the b/p/s bonuses, the damage is {hit.m_damage.GetTotalDamage()}");

                //Jotunn.Logger.LogMessage($"To {hit.m_pushForce} and {hit.m_damage.GetTotalStaggerDamage()}");

                hit.m_damage.Modify(CFG.GetBigStickDamageMult());
                P_WeaponEnchants.ApplyWeaponEnchant(ref hit.m_damage);

            }
            //Or if the attacked is the local player
            else if (__instance.IsPlayer() && __instance.GetZDOID() == Player.m_localPlayer.GetZDOID())
            {
                //Player player = (Player)__instance;

                //Jotunn.Logger.LogMessage($"Player got hit");
                //We can do stuff here probably. Will we need to? Who knows.
            }

            WeaponMan.DamageToExp(__instance, hit, false);
        }

        [HarmonyPatch(typeof(MineRock), nameof(MineRock.Damage))] [HarmonyPrefix]
        static void MineRockDamage(MineRock __instance, ref HitData hit) => 
            ModifyResourceDamage(__instance, __instance.m_nview, __instance.m_minToolTier, ref hit);
       
        [HarmonyPatch(typeof(MineRock5), nameof(MineRock5.Damage))] [HarmonyPrefix]
        static void MineRock5Damage(MineRock5 __instance, ref HitData hit) =>
            ModifyResourceDamage(__instance, __instance.m_nview, __instance.m_minToolTier, ref hit);
        
        [HarmonyPatch(typeof(TreeBase), nameof(TreeBase.Damage))] [HarmonyPrefix]
        static void TreeBaseDamage(TreeBase __instance, ref HitData hit) =>
            ModifyResourceDamage(__instance, __instance.m_nview, __instance.m_minToolTier, ref hit);
        
        [HarmonyPatch(typeof(TreeLog), nameof(TreeLog.Damage))] [HarmonyPrefix]
        static void TreeLogDamage(TreeLog __instance, ref HitData hit) =>
            ModifyResourceDamage(__instance, __instance.m_nview, __instance.m_minToolTier, ref hit);
        
        [HarmonyPatch(typeof(Destructible), nameof(Destructible.Damage))] [HarmonyPrefix]
        static void DestructibleDamage(Destructible __instance, ref HitData hit) =>
            ModifyResourceDamage(__instance, __instance.m_nview, __instance.m_minToolTier, ref hit);

        public static void ModifyResourceDamage(IDestructible resource, ZNetView nview, int minToolTier, ref HitData hit)
        {
            //Jotunn.Logger.LogMessage("Hitting a resource patch detected!");
            if (!CheckHitGood(hit)) return;

            if (hit.m_toolTier < minToolTier) return;

            RPCMan.IAmKiller_RPC(nview);

            if (hit.m_attacker == Player.m_localPlayer.GetZDOID())
            {
                Player player = Player.m_localPlayer;
                if (hit.m_skill == Skills.SkillType.WoodCutting)
                {
                    //Jotunn.Logger.LogMessage($"Chop damage was {hit.m_damage.m_chop}");
                    float chopMod = 1f;

                    if (CFG.IsSkillActive(Skills.SkillType.WoodCutting))
                        chopMod += CFG.GetWoodcuttingDamageMod(player.GetSkillFactor(Skills.SkillType.WoodCutting));
                    if (CFG.IsSkillActive(Skills.SkillType.Axes))
                        chopMod += CFG.GetAxeChopDamageMod(player.GetSkillFactor(Skills.SkillType.Axes));

                    hit.m_damage.m_chop *= chopMod;

                    if (CFG.IsSkillActive(Skills.SkillType.WoodCutting))
                        player.AddStamina(CFG.GetWoodcuttingStaminaRebate(player.GetSkillFactor(Skills.SkillType.WoodCutting)));

                   //Jotunn.Logger.LogMessage($"Now it's {hit.m_damage.m_chop}! I also added some stamina");
                }
                else if (CFG.IsSkillActive(Skills.SkillType.Pickaxes) &&
                    hit.m_skill == Skills.SkillType.Pickaxes)
                {
                    //Jotunn.Logger.LogMessage($"Pick damage was {hit.m_damage.m_pickaxe}");

                    hit.m_damage.m_pickaxe *=
                        CFG.GetMiningDamageMult(player.GetSkillFactor(Skills.SkillType.Pickaxes));

                    player.AddStamina(CFG.GetMiningStaminaRebate(player.GetSkillFactor(Skills.SkillType.Pickaxes)));

                    //Jotunn.Logger.LogMessage($"Now it's {hit.m_damage.m_pickaxe}! I also added some stamina");
                }

                WeaponMan.DamageToExp(resource, hit, false);
            }
        }

        [HarmonyPatch(typeof(Character), nameof(Character.GetRandomSkillFactor))] [HarmonyPrefix]
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

        public static bool CheckHitGood(HitData hit)
        {
            if (hit == null)
            {
                return false;
            }
            if (hit.m_damage.GetTotalDamage() < 0.1f)
            {
                return false;
            }
            if (hit.GetAttacker() == null)
            {
                return false;
            }
            return true;
        }
    }
}
