using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace kingskills.Perks
{
    class P_Deadeye
    {
        public static void ThrowCheck(string hotkey)
        {
            if (!ZInput.GetButtonDown(hotkey)) return;
            if (!CFG.CheckPlayerActivePerk(Player.m_localPlayer, PerkMan.PerkType.Deadeye)) return;
            Player player = Player.m_localPlayer;
            ItemDrop.ItemData weapon = CFG.GetPlayerWeapon(player);
            if (weapon is null) return;
            if (weapon.m_shared.m_skillType != Skills.SkillType.Knives) return;

            CreateAndStartCustomThrowAttack(player, weapon);
        }

        public static void CreateAndStartCustomThrowAttack(Player p, ItemDrop.ItemData weapon)
        {
            p.AbortEquipQueue();
            //Attack start checks
            if ((p.InAttack() && !p.HaveQueuedChain()) || p.InDodge() || !p.CanMove() ||
                p.IsKnockedBack() || p.IsStaggering() || p.InMinorAction())
            {
                return;
            }
            if (p.m_currentAttack != null)
            {
                p.m_currentAttack.Stop();
                p.m_previousAttack = p.m_currentAttack;
                p.m_currentAttack = null;
            }

            Attack throwAtk = new Attack();
            Attack cloneAtk = weapon.m_shared.m_attack;

            throwAtk.m_attackType = Attack.AttackType.Projectile;
            throwAtk.m_attackAnimation = "spear_throw";
            throwAtk.m_consumeItem = true;

            throwAtk.m_attackStamina = cloneAtk.m_attackStamina;
            throwAtk.m_speedFactor = .3f;
            throwAtk.m_speedFactorRotation = 5f;

            throwAtk.m_skillHitType = DestructibleType.Character;

            GameObject projPrefab = GameObject.Instantiate(weapon.m_dropPrefab);
            GameObject.DestroyImmediate(projPrefab.GetComponentInChildren<ItemDrop>());
            GameObject.DestroyImmediate(projPrefab.GetComponentInChildren<ParticleSystem>());
            GameObject.DestroyImmediate(projPrefab.GetComponentInChildren<ParticleSystemRenderer>());
            GameObject.DestroyImmediate(projPrefab.GetComponentInChildren<Floating>());
            GameObject.DestroyImmediate(projPrefab.GetComponentInChildren<Rigidbody>());
            Projectile proj = projPrefab.AddComponent<Projectile>();

            proj.m_backstabBonus = 1f;
            proj.m_gravity = 6f;
            proj.m_hitNoise = 40f;
            proj.m_stopEmittersOnHit = true;
            proj.m_hitEffects = cloneAtk.m_hitEffect;
            proj.m_respawnItemOnHit = true;
            proj.m_showBreakMessage = true;
            proj.m_ttl = 10f;
            projPrefab.gameObject.SetActive(true);
            proj.m_visual = projPrefab;
            foreach (Component comp in projPrefab.GetComponentsInChildren<Component>())
            {
                Jotunn.Logger.LogMessage($"component: {comp}");
            }

            throwAtk.m_attackProjectile = projPrefab;
            throwAtk.m_projectileVel = 20f;
            throwAtk.m_projectileVelMin = 2f;
            throwAtk.m_projectileAccuracy = 1f;
            throwAtk.m_projectileAccuracyMin = 20f;
            throwAtk.m_projectiles = 1;
            throwAtk.m_projectileBursts = 1;

            throwAtk.m_attackRange = 1f;
            throwAtk.m_attackHeight = 1.5f;
            throwAtk.m_attackOffset = .2f;

            bool atkSucceed = throwAtk.Start(p, p.m_body, p.m_zanim, p.m_animEvent, p.m_visEquipment, weapon, p.m_previousAttack,
                p.m_timeSinceLastAttack, 1f);

            //Jotunn.Logger.LogMessage($"throw success: {atkSucceed}");
            if (atkSucceed)
            {
                throwAtk.OnAttackTrigger();
            }
        }
    }
}
