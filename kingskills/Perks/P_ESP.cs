using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace kingskills.Perks
{
    [HarmonyPatch]
    class P_ESP
    {
        [HarmonyPatch(typeof(BaseAI),nameof(BaseAI.CanSeeTarget), new Type[] { typeof(Character) }), HarmonyPrefix]
        public static bool TotalInvisSeePatch(BaseAI __instance, Character target, ref bool __result)
        {
            if (!IsInvisible(target, ref __result)) return CFG.DontSkipOriginal;
            return CFG.SkipOriginal;
        }

        [HarmonyPatch(typeof(BaseAI), nameof(BaseAI.CanHearTarget)), HarmonyPrefix]
        public static bool TotalInvisHearPatch(BaseAI __instance, Character target, ref bool __result)
        {
            if (!IsInvisible(target, ref __result)) return CFG.DontSkipOriginal;
            return CFG.SkipOriginal;
        }

        public static bool IsInvisible(Character target, ref bool canSee)
        {
            if (!CFG.CheckPlayerActivePerk(target, PerkMan.PerkType.ESP) || 
                !target.IsSneaking()) return false;
            //Jotunn.Logger.LogMessage($"ESP is making player invisible");
            canSee = false;
            return true;
        }
    }
}
