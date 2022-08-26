using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kingskills.Patches
{
    [HarmonyPatch]
    class PolearmPatch
    {

        [HarmonyPatch(typeof(Player))]
        [HarmonyPatch(nameof(Player.GetBodyArmor))]
        [HarmonyPostfix]
        public static void ArmorPatch(Player __instance, ref float __result)
        {
            Jotunn.Logger.LogMessage($"Before patch, armor was {__result}");
            __result +=
                ConfigManager.GetPolearmArmor(__instance.GetSkillFactor(Skills.SkillType.Polearms));
            Jotunn.Logger.LogMessage($"now, armor is {__result}");
        }

        [HarmonyPatch(typeof(Attack))]
        [HarmonyPatch(nameof(Attack.OnAttackTrigger))]
        [HarmonyPrefix]
        public static void RangePatch(Attack __instance)
        {
            if (__instance.m_character.IsPlayer() && 
                __instance.m_character.GetZDOID() == Player.m_localPlayer.GetZDOID())
            {
                Jotunn.Logger.LogMessage($"before change, range is {__instance.m_attackRange}");
                __instance.m_attackRange +=
                    ConfigManager.GetPolearmRange(Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Polearms));
                Jotunn.Logger.LogMessage($"Increased range to {__instance.m_attackRange}");
            }
        }
    }
}
