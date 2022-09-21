using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace kingskills.Patches
{
    [HarmonyPatch]
    class KSPolearm
    {
        [HarmonyPatch(typeof(Player))]
        [HarmonyPatch(nameof(Player.GetBodyArmor))]
        [HarmonyPostfix]
        public static void ArmorPatch(Player __instance, ref float __result)
        {
            if (CFG.IsSkillActive(Skills.SkillType.Polearms))
            {
                //Jotunn.Logger.LogMessage($"Before patch, armor was {__result}");

                __result +=
                    Mathf.Round(CFG.GetPolearmArmor(__instance.GetSkillFactor(Skills.SkillType.Polearms)));

                //Jotunn.Logger.LogMessage($"now, armor is {__result}");
            }
        }

        [HarmonyPatch(typeof(Attack))]
        [HarmonyPatch(nameof(Attack.OnAttackTrigger))]
        [HarmonyPrefix]
        public static void RangePatch(Attack __instance)
        {
            if (CFG.IsSkillActive(Skills.SkillType.Polearms) &&
                __instance.m_character.IsPlayer() && 
                __instance.m_character.GetZDOID() == Player.m_localPlayer.GetZDOID())
            {
                //Jotunn.Logger.LogMessage($"before change, range is {__instance.m_attackRange}");

                __instance.m_attackRange +=
                    CFG.GetPolearmRange(Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Polearms));

                //Jotunn.Logger.LogMessage($"Increased range to {__instance.m_attackRange}");
            }
        }


        [HarmonyPatch(typeof(Character))]
        [HarmonyPatch(nameof(Character.RPC_Damage))]
        [HarmonyPrefix]
        public static void DamageArmorWatch(Character __instance, HitData hit)
        {
            if (!CFG.IsSkillActive(Skills.SkillType.Polearms) ||
                !__instance.IsPlayer() ||
                Util.GetPlayerWeapon((Player)__instance).m_shared.m_skillType != Skills.SkillType.Polearms ||
                (__instance.IsBlocking() && hit.m_blockable)) 
                return;
            Character attacker = hit.GetAttacker();
            if (!hit.HaveAttacker() || attacker == null || 
                !__instance.IsPVPEnabled() && attacker != null && attacker.IsPlayer())
                return;

            HitData sampleHit = hit.Clone();
            sampleHit.ApplyResistance(__instance.m_damageModifiers, out _);
            sampleHit.ApplyArmor(__instance.GetBodyArmor());
            float damageBlocked = hit.GetTotalDamage() - sampleHit.GetTotalDamage();

            //Jotunn.Logger.LogMessage($"Just blocked {damageBlocked} damage with armor.");
            __instance.RaiseSkill(Skills.SkillType.Polearms,
                damageBlocked * CFG.WeaponBXPPolearmDamageMod.Value);
        }
    }
}
