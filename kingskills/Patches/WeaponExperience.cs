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

namespace kingskills.WeaponExperience
{

    class Manager
    {
        // Patch IDestructible.Damage() to gain experience for player based on damage events.
        public static void Strike(Player p, HitData hit, float factor=1.0f, bool tool=false)
        {
            if (!WeaponDamagePatch.CheckHitGood(hit)) return;

            if (hit.m_attacker == p.GetZDOID())
            {
                //Jotunn.Logger.LogMessage($"Player dealt damage to {__instance.GetDestructibleType()}");
                float damage = hit.m_damage.GetTotalDamage();

                float damage_xp;

                if (tool)
                    damage_xp = ConfigManager.GetToolDamageToExperience(damage);
                else
                    damage_xp = ConfigManager.GetWeaponDamageToExperience(damage);
                
                float final_xp = damage_xp * factor;
                //Jotunn.Logger.LogMessage($"Incrementing {hit.m_skill} by {final_xp} = damage {damage} ^ {ConfigManager.XpDamageDegree} * factor {factor}");
                p.RaiseSkill(hit.m_skill, final_xp);
            }
        }

        public static void Swing(Player p, Skills.SkillType skill)
        {
            //Jotunn.Logger.LogMessage($"Player swinging with {skill} for {ConfigManager.XpSwingRate} XP");
            p.RaiseSkill(skill, ConfigManager.WeaponEXPSwing.Value);

            if (skill == Skills.SkillType.Axes)
                p.RaiseSkill(Skills.SkillType.WoodCutting, ConfigManager.WeaponEXPSwing.Value);
        }
    }

    [HarmonyPatch]
    class WeaponStrikeDetection
    {
        //Damage patch rerouting
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Character), nameof(Character.Damage))]
        static void Character_Damage(Character __instance, HitData hit)
        {
            DamageToExp(__instance, hit, true);
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Destructible), nameof(Destructible.Damage))]
        static void Destructible_Damage(Destructible __instance, HitData hit)
        {
            DamageToExp(__instance, hit, false);
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MineRock), nameof(MineRock.Damage))]
        static void MineRock_Damage(Destructible __instance, HitData hit)
        {
            DamageToExp(__instance, hit, false);
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MineRock5), nameof(MineRock5.Damage))]
        static void MineRock5_Damage(Destructible __instance, HitData hit)
        {
            DamageToExp(__instance, hit, false);
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(TreeBase), nameof(TreeBase.Damage))]
        static void TreeBase_Damage(Destructible __instance, HitData hit)
        {
            DamageToExp(__instance, hit, false);
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(TreeLog), nameof(TreeLog.Damage))]
        static void TreeLog_Damage(Destructible __instance, HitData hit)
        {
            DamageToExp(__instance, hit, false);
        }
        static void DamageToExp(IDestructible __instance, HitData hit, bool livingTarget)
        {
            Player playerRef = Player.m_localPlayer;
            bool tool = false;
            float factor = 0f;

            if (hit.m_skill == Skills.SkillType.WoodCutting || hit.m_skill == Skills.SkillType.Pickaxes)
            {
                tool = true;

                if (livingTarget)
                    factor = 0f;
                else
                    factor = 1f;
            }
            else
            {
                if (livingTarget)
                    factor = 1f;
                else
                    factor = ConfigManager.GetWeaponEXPStrikeDestructibleMod();
            }


            Manager.Strike(playerRef, hit, factor, tool);
        }
    }

    [HarmonyPatch]
    class WeaponSwingDetection
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Humanoid), nameof(Humanoid.StartAttack))]
        static void Humanoid_StartAttack(Humanoid __instance, Character target, bool secondaryAttack, bool __result)
        {
            if (__result && __instance is Player && __instance == Player.m_localPlayer)
            {
                Manager.Swing(__instance as Player, __instance.GetCurrentWeapon().m_shared.m_skillType);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Attack), nameof(Attack.SpawnOnHitTerrain))]
        static void Attack_SpawnOnHitTerrain(Attack __instance, Vector3 hitPoint, GameObject prefab)
        {
            if (__instance.m_pickaxeSpecial && __instance.m_character is Player)
            {
                Manager.Swing(__instance.m_character as Player, Skills.SkillType.Pickaxes);
            }
        }
    }

    [HarmonyPatch]
    class WeaponHold
    {
        static ItemDrop.ItemData last;
        static float timer;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Player), nameof(Player.FixedUpdate))]
        static void Player_FixedUpdate(Player __instance)
        {
            if (__instance != Player.m_localPlayer || !__instance.CanMove())
            {
                return;
            }

            ItemDrop.ItemData weapon = Util.GetPlayerWeapon(__instance);
            if (weapon == null)
            {
                last = null;
                return;
            }

            float dt = Time.fixedDeltaTime;
            // Unequipped or changed weapon, restart timer.
            if (weapon != last)
            {
                last = weapon;
                timer = dt;
            }
            // Held onto same weapon for some time.
            else
            {
                timer += dt;
                if (timer >= ConfigManager.WeaponEXPHoldTickLength.Value)
                {
                    float ticks = timer / ConfigManager.WeaponEXPHoldTickLength.Value;
                    float holdXp = ticks * ConfigManager.WeaponEXPHoldPerTick.Value;
                    if (weapon == __instance.m_unarmedWeapon.m_itemData)
                    {
                        holdXp *= ConfigManager.GetWeaponEXPHoldUnarmedMod();
                    }
                    Skills.SkillType skill = weapon.m_shared.m_skillType;
                    //Jotunn.Logger.LogMessage($"Holding {skill} for {timer}s, adding {holdXp} xp");
                    __instance.RaiseSkill(skill, holdXp);
                    timer -= ticks * ConfigManager.WeaponEXPHoldTickLength.Value;
                }
            }
        }
    }
}
