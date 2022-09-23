using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace kingskills.Perks
{
    [HarmonyPatch(typeof(Character))]
    class P_AirStep
    {
        public const string IsJumping = "AS Jumping";
        public const string NumJumps = "AS Num Jumps";

        [HarmonyPatch(nameof(Character.IsOnGround))]
        [HarmonyPrefix]
        public static bool ExtraJump(Character __instance, ref bool __result)
        {
            if (!__instance.m_nview.IsValid()) return CFG.DontSkipOriginal;

            ZDO zdo = __instance.m_nview.m_zdo;

            if (!zdo.GetBool(IsJumping, false)) return CFG.DontSkipOriginal;


            //Jotunn.Logger.LogMessage($"Onground was checked. I have our num jumps as {zdo.GetInt(NumJumps, 100)}, " +
            //    $"compared to the max jumps, {CFG.GetAirStepExtraJumps()+1}");

            if (zdo.GetInt(NumJumps, 100) < CFG.GetAirStepExtraJumps()+1)
            {
                __result = true;
                return CFG.SkipOriginal;
            }
            else
            {
                return CFG.DontSkipOriginal;
            }
        }


        [HarmonyPatch(nameof(Character.Jump))]
        [HarmonyPrefix]
        public static void PreJump(Character __instance)
        {
            CFG.SetZDOVariable(__instance, PerkMan.PerkType.AirStep, IsJumping, true);

            //Jotunn.Logger.LogMessage($"Jump start."); // This will fail if any of the followin are true:");

            /*
            Jotunn.Logger.LogMessage($"!IsOnGround: {!__instance.IsOnGround()} ||");
            Jotunn.Logger.LogMessage($"IsDead: {__instance.IsDead()} ||");
            Jotunn.Logger.LogMessage($"InAttack: {__instance.InAttack()} ||");
            Jotunn.Logger.LogMessage($"IsEncumbered: {__instance.IsEncumbered()} ||");
            Jotunn.Logger.LogMessage($"InDodge: {__instance.InDodge()} ||");
            Jotunn.Logger.LogMessage($"IsKnockedBack: {__instance.IsKnockedBack()} ||");
            Jotunn.Logger.LogMessage($"IsStaggering: {__instance.IsStaggering()} ||");
            */
        }

        [HarmonyPatch(nameof(Character.Jump))]
        [HarmonyFinalizer]
        public static void PostJump(Character __instance)
        {
            CFG.SetZDOVariable(__instance, PerkMan.PerkType.AirStep, IsJumping, false);
        }

        [HarmonyPatch(typeof(Player),nameof(Player.OnJump))]
        [HarmonyPrefix]
        public static void IncrementJumpCount(Player __instance)
        {
            if (!(__instance.GetZDOID() == Player.m_localPlayer.GetZDOID())) return;

            ZDO zdo = __instance.m_nview.m_zdo;

            //if (!PerkMan.IsPerkActive(PerkMan.PerkType.AirStep)) return;
            //Jotunn.Logger.LogMessage($"This is a player's jump call. Increasing number of jumps from");

            int currentJumps = zdo.GetInt(NumJumps, 0);
            zdo.Set(NumJumps, (int)(currentJumps + 1));

            //Jotunn.Logger.LogMessage($"{currentJumps} to {currentJumps+1}");
        }

        public static void ResetJumps(Character player)
        {
            player.m_nview.m_zdo.Set(NumJumps, (int)0);
        }
    }
}
