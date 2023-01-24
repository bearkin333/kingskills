using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace kingskills.Perks
{
    class P_FalconKick
    {
        /*
         * 
         * Instead of all of this, I should just change the data on the player's unarmed weapon secondary attack
         * 
        public static void AddFKCheck(ref HitData hit)
        {
            if (!CFG.CheckPlayerActivePerk(Player.m_localPlayer, PerkMan.PerkType.FalconKick)) return;
            //check if kick

            hit.m_damage.m_fire += CFG.FalconKickDamageBonus.Value;
            hit.m_pushForce *= CFG.GetFalconKickKnockbackMult();
        }
        */
    }

    /*

    [HarmonyPatch(typeof(Humanoid), nameof(Humanoid.StartAttack))]
    class IsLocalPlayerKicking
    {
        public static bool value = false;

        [HarmonyPrefix]
        public static void OnAttackStart(Humanoid __instance, Character target, bool secondaryAttack)
        {
            if (__instance != Player.m_localPlayer) return;
            bool unarmed = Player.m_localPlayer.GetCurrentWeapon() == Player.m_localPlayer.m_unarmedWeapon.m_itemData;
            value = secondaryAttack && unarmed;
            Jotunn.Logger.LogMessage($"attack started. local player kicking: {value}");
        }

        [HarmonyFinalizer]
        public static void AttackStartCleanup() => value = false;
    }

    [HarmonyPatch(typeof(Attack), nameof(Attack.Start))]
    class FalconKickStart
    {
        [HarmonyPrefix]
        public static void StartAttackPatch(Attack __instance)
        {

        }
    }
    */
}
