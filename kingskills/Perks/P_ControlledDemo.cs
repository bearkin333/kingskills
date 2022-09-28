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
    class P_ControlledDemo
    {

        [HarmonyPatch(typeof(TreeBase),nameof(TreeBase.SpawnLog)), HarmonyPrefix]
        public static void LaunchLog(TreeBase __instance, ref Vector3 hitDir)
        {
            Player killer = null;
            if (!CFG.GetKillerExists(__instance.m_nview, ref killer)) return;
            if (!CFG.CheckPlayerActivePerk(killer, PerkMan.PerkType.ControlledDemo)) return;

            bool changed = false;

            Character nearestEnemy = GetNearestEnemy(killer, CFG.ControlledDemoDetectRange.Value);
            Vector3 targetPos;

            if (nearestEnemy is null) targetPos = GetNearestTreePos(killer, CFG.ControlledDemoDetectRange.Value);
            else targetPos = nearestEnemy.transform.position;

            if (targetPos != Vector3.zero) changed = true;

            Jotunn.Logger.LogMessage($"changed is {changed}");
            if (changed)
            {
                Vector3 newDir = -Vector3.Normalize(__instance.transform.position - targetPos);
                newDir *= CFG.ControlledDemoForce.Value;

                Vector3 heightBoost = __instance.m_logSpawnPoint.transform.position;
                heightBoost.y += .3f;
                __instance.m_logSpawnPoint.transform.position = heightBoost;

                //Jotunn.Logger.LogMessage($"original hit direction was {hitDir}, and controlled demo direction is {newDir}");
                hitDir = newDir;
            }
        }

        public static Character GetNearestEnemy(Character player, float range)
        {
            Character closest = null;
            float savedDistance = 0f;

            foreach (BaseAI allInstance in BaseAI.GetAllInstances())
            {
                float distance = Vector3.Distance(allInstance.transform.position, player.transform.position);
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

        public static Vector3 GetNearestTreePos(Character player, float range)
        {
            Vector3 closest = Vector3.zero;
            float savedDistance = 0f;

            Collider[] hitCollders = Physics.OverlapSphere(player.transform.position, range);
            //Jotunn.Logger.LogMessage($"trying to find a nearby tree");

            foreach (var collider in hitCollders)
            {
                Jotunn.Logger.LogMessage($"collider found: {collider.gameObject.name}");
                IDestructible dest = collider.gameObject.GetComponentInChildren(typeof(IDestructible)) as IDestructible;
                if (dest is null) break;
                Jotunn.Logger.LogMessage($"collider with idestruct found. name: {collider.gameObject.name}");
                if (dest.GetDestructibleType() != DestructibleType.Tree) break;

                float distance = Vector3.Distance(collider.transform.position, player.transform.position);
                if (distance < range)
                {
                    if (closest == Vector3.zero)
                    {
                        closest = collider.transform.position;
                        savedDistance = distance;
                    }
                    else
                    {
                        if (distance < savedDistance)
                        {
                            closest = collider.transform.position;
                            savedDistance = distance;
                        }
                    }
                }
            }

            return closest;

        }
    }
}
