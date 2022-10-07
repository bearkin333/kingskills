using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace kingskills.Perks
{
    [HarmonyPatch]
    class P_Einherjar
    {
        [HarmonyPatch(typeof(Projectile), nameof(Projectile.FixedUpdate)), HarmonyPrefix]
        public static void ProjectileUpdateHoming(Projectile __instance)
        {
            if (!__instance.m_nview.IsValid() || !__instance.m_nview.IsOwner() || __instance.m_didHit ||
                __instance.m_owner is null || !__instance.m_owner.IsPlayer()) return;
            if (!CFG.CheckPlayerActivePerk(__instance.m_owner, PerkMan.PerkType.Einherjar)) return;

            Character target = GetNearestEnemyInDeadzone(__instance.transform, 
                CFG.EinherjarDetectRange.Value, CFG.GetEinherjarAngleDeadzone());

            if (target is null) return;
            //Jotunn.Logger.LogMessage($"located homing target: {target.name}");

            Vector3 curDir = __instance.transform.forward;

            //This code is to try and grab the position of a creature's center.
            CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();
            Vector3 targetCenterOffset = target.transform.up;
            if (targetCollider is null) targetCenterOffset *= 1.5f;
            else targetCenterOffset *= targetCollider.height / 2f;
            Vector3 targetAdjustedPos = target.transform.position + targetCenterOffset;

            Vector3 perfectDir = (targetAdjustedPos - __instance.transform.position).normalized;
            Vector3 changedDir = Vector3.MoveTowards(curDir, perfectDir, CFG.EinherjarHomeSpeed.Value);
            __instance.m_vel = changedDir * __instance.m_vel.magnitude;
        }

        public static Character GetNearestEnemyInDeadzone(Transform detectingPos, float range, float deadzone)
        {
            Character closest = null;
            float savedDistance = 0f;

            foreach (BaseAI allInstance in BaseAI.GetAllInstances())
            {
                float angleDiff = Vector3.Angle(detectingPos.forward, (allInstance.transform.position - detectingPos.position).normalized);
                if (angleDiff > deadzone) continue;

                float distance = Vector3.Distance(allInstance.transform.position, detectingPos.position);
                if (distance < range)
                {
                    if (closest is null)
                    {
                        closest = allInstance.m_character;
                        savedDistance = distance;
                    }
                    else
                    {
                        if (distance < savedDistance)
                        {
                            closest = allInstance.m_character;
                            savedDistance = distance;
                        }
                    }
                }
            }

            return closest;
        }
    }
}
