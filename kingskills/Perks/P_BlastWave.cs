using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace kingskills.Perks
{
    [HarmonyPatch]
    class P_BlastWave
    {
        [HarmonyPatch(typeof(Attack), nameof(Attack.DoAreaAttack))]
        [HarmonyPrefix]
        public static void CheckAOEIncrease(Attack __instance)
        {
            if (!CFG.CheckPlayerActivePerk(__instance.m_character, PerkMan.PerkType.BlastWave)) return;
            __instance.m_attackRayWidth *= CFG.GetBlastWaveScaleMult();
            //Jotunn.Logger.LogMessage($"New attack area is {__instance.m_attackRayWidth}");
        }
    }
}
