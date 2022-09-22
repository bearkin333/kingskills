using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace kingskills.SE
{
    [HarmonyPatch]
    class SE_RestedChange
    {
        [HarmonyPatch(typeof(SE_Stats),nameof(SE_Stats.ModifyRaiseSkill))]
        [HarmonyPrefix]
        public static bool RestedPercentChange(SE_Rested __instance, Skills.SkillType skill, ref float value)
        {
            if (__instance.m_skillLevel != 0 && (__instance.m_skillLevel == Skills.SkillType.All || __instance.m_skillLevel == skill))
                value *= 1.5f;

            return CFG.SkipOriginal;
        }
    }
}
