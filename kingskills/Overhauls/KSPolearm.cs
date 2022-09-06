using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (ConfigMan.IsSkillActive(Skills.SkillType.Polearms))
            {
                //Jotunn.Logger.LogMessage($"Before patch, armor was {__result}");

                __result +=
                    ConfigMan.GetPolearmArmor(__instance.GetSkillFactor(Skills.SkillType.Polearms));

                //Jotunn.Logger.LogMessage($"now, armor is {__result}");
            }
        }

        [HarmonyPatch(typeof(Attack))]
        [HarmonyPatch(nameof(Attack.OnAttackTrigger))]
        [HarmonyPrefix]
        public static void RangePatch(Attack __instance)
        {
            if (ConfigMan.IsSkillActive(Skills.SkillType.Polearms) &&
                __instance.m_character.IsPlayer() && 
                __instance.m_character.GetZDOID() == Player.m_localPlayer.GetZDOID())
            {
                //Jotunn.Logger.LogMessage($"before change, range is {__instance.m_attackRange}");

                __instance.m_attackRange +=
                    ConfigMan.GetPolearmRange(Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Polearms));

                //Jotunn.Logger.LogMessage($"Increased range to {__instance.m_attackRange}");
            }
        }


        [HarmonyPatch(typeof(Character))]
        [HarmonyPatch(nameof(Character.RPC_Damage))]
        [HarmonyPrefix]
        public static void DamageArmorWatch(Character __instance, HitData hit)
        {
            if (!ConfigMan.IsSkillActive(Skills.SkillType.Polearms)) return;
            if (!__instance.IsPlayer()) return;
            if (__instance.IsBlocking() && hit.m_blockable) return;

            HitData sampleHit = hit.Clone();
            sampleHit.ApplyArmor(__instance.GetBodyArmor());
            float damageBlocked = hit.GetTotalDamage() - sampleHit.GetTotalDamage();

            Jotunn.Logger.LogMessage($"Just blocked {damageBlocked} damage with armor.");
            __instance.RaiseSkill(Skills.SkillType.Polearms,
                damageBlocked * ConfigMan.WeaponBXPPolearmDamageMod.Value);
        }
    }
}
