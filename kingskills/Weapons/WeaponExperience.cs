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
using kingskills.Patches;

namespace kingskills.Weapons
{

    class WeaponMan
    {
        // Patch IDestructible.Damage() to gain experience for player based on damage events.
        public static void Strike(IDestructible victimD, HitData hit, float mod=1.0f, bool tool=false)
        {
            if (!WeaponDamagePatch.CheckHitGood(hit)) return;

            Character attacker = hit.GetAttacker();
            Character victim = null;
            if (victimD.GetDestructibleType() == DestructibleType.Character)
                victim = victimD as Character;

            //If both parties are players, and either one of them doesn't have PVP enabled, we stop
            if (victim != null && 
                (victim.IsPlayer() && attacker.IsPlayer()) &&
                !(victim.IsPVPEnabled() && attacker.IsPVPEnabled())) return;

            Player p = Player.m_localPlayer;

            if (attacker.GetZDOID() == p.GetZDOID())
            {
                //Jotunn.Logger.LogMessage($"Player dealt damage to {__instance.GetDestructibleType()}");
                float damage = hit.m_damage.GetTotalDamage();

                float damage_xp = 1f;

                if (CFG.IsSkillActive(hit.m_skill))
                {
                    if (tool)
                        damage_xp = CFG.GetToolDamageToExperience(damage);
                    else
                        damage_xp = CFG.GetWeaponDamageToExperience(damage);
                }
                
                float final_xp = damage_xp * mod;

                WeaponRaiseSkillTakeover.DoNotIgnore();

                //Jotunn.Logger.LogMessage($"Incrementing {hit.m_skill} by {final_xp} = damage {damage} ^ {ConfigManager.XpDamageDegree} * factor {factor}");
                p.RaiseSkill(hit.m_skill, final_xp);
            }
        }

        public static void Swing(Player p, Skills.SkillType skill)
        {
            if (!CFG.IsSkillActive(skill)) return;

            //Jotunn.Logger.LogMessage($"Player swinging with {skill} for {CFG.WeaponXPSwing.Value} XP");
            LevelUp.CustomRaiseSkill(p, skill, CFG.WeaponXPSwing.Value, false);

            if (skill == Skills.SkillType.Axes)
                LevelUp.CustomRaiseSkill(p, Skills.SkillType.WoodCutting, CFG.WeaponXPSwing.Value, false);
        }

        public static void DamageToExp(IDestructible __instance, HitData hit, bool livingTarget)
        {
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
                    factor = CFG.GetWeaponXPStrikeDestructibleMod();
            }

            Strike(__instance, hit, factor, tool);
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
                WeaponMan.Swing(__instance as Player, __instance.GetCurrentWeapon().m_shared.m_skillType);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Attack), nameof(Attack.SpawnOnHitTerrain))]
        static void Attack_SpawnOnHitTerrain(Attack __instance, Vector3 hitPoint, GameObject prefab)
        {
            if (__instance.m_pickaxeSpecial && __instance.m_character is Player)
            {
                WeaponMan.Swing(__instance.m_character as Player, Skills.SkillType.Pickaxes);
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

            ItemDrop.ItemData weapon = CFG.GetPlayerWeapon(__instance);
            if (weapon == null)
            {
                last = null;
                return;
            }
            if (!CFG.IsSkillActive(weapon.m_shared.m_skillType))
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
                if (timer >= CFG.WeaponXPHoldTickLength.Value)
                {
                    float ticks = timer / CFG.WeaponXPHoldTickLength.Value;
                    float holdXp = ticks * CFG.WeaponXPHoldPerTick.Value;
                    if (weapon == __instance.m_unarmedWeapon.m_itemData || 
                        weapon.m_shared.m_skillType == Skills.SkillType.Unarmed)
                    {
                        holdXp *= CFG.GetWeaponEXPHoldUnarmedMod();
                    }
                    Skills.SkillType skill = weapon.m_shared.m_skillType;
                    //Jotunn.Logger.LogMessage($"Holding {skill} for {timer}s, adding {holdXp} xp");
                    __instance.RaiseSkill(skill, holdXp);
                    timer -= ticks * CFG.WeaponXPHoldTickLength.Value;
                }
            }
        }
    }
}
