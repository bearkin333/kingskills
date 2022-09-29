using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using kingskills.SE;
using UnityEngine;

namespace kingskills.Perks
{
    [HarmonyPatch]
    class P_CouchedLance
    {
        public static float timer = 0f;
        public static float checkPosTimer = 0f;
        public static Vector3 oldPos = Vector3.zero;
        public static bool still = false;

        [HarmonyPatch(typeof(Character), nameof(Character.UpdateMotion)), HarmonyPostfix]
        public static void UpdateStandingStill(Character __instance, float dt)
        {
            if (!CFG.CheckPlayerActivePerk(__instance, PerkMan.PerkType.CouchedLance)) return;

            checkPosTimer += dt;

            if (checkPosTimer < CFG.CouchedLanceCheckTimer.Value) return;

            Vector3 newPos = __instance.transform.position;

            //Jotunn.Logger.LogMessage($"Distance: {Vector3.Distance(oldPos, newPos)}");

            if (Vector3.Distance(oldPos, newPos) > CFG.CouchedLanceDeadzone.Value)
            {
                still = false;
                timer = 0f;
            }
            else
            {
                still = true;
                timer += checkPosTimer;
            }

            oldPos = newPos;
            checkPosTimer = 0f;

            //Jotunn.Logger.LogMessage($"Standing still: {still} and timer is {timer}");
            if (timer >= CFG.CouchedLanceTimer.Value)
            {
                SEMan seman = Player.m_localPlayer.GetSEMan();

                if (!seman.HaveStatusEffect(KS_SEMan.ks_CouchedLanceName))
                {
                    seman.AddStatusEffect((SE_CouchedLance)ScriptableObject.CreateInstance(typeof(SE_CouchedLance)));
                }
                else
                {
                    seman.GetStatusEffect(KS_SEMan.ks_CouchedLanceName).ResetTime();
                }
            }
        }

        public static void ApplyDamage(ref HitData hit)
        {
            if (Player.m_localPlayer.GetSEMan().HaveStatusEffect(KS_SEMan.ks_CouchedLanceName))
                hit.m_damage.Modify(CFG.GetCouchedLanceDamageMult());
        }
    }
}
