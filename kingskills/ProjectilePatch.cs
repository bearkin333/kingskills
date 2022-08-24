using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace kingskills
{
    [HarmonyPatch(typeof(Attack))]

    [HarmonyPatch(typeof(Projectile))]
    class ProjectilePatch
    {

        public static Dictionary<int, Vector3> projectileList 
            = new Dictionary<int, Vector3>();
        
        [HarmonyPatch(nameof(Projectile.Awake))]
        [HarmonyPostfix]
        public static void InitProj(Projectile __instance)
        {
            projectileList.Add(__instance.gameObject.GetInstanceID(), __instance.transform.position);
            //Jotunn.Logger.LogMessage($"Added projectile {__instance.gameObject.GetInstanceID()}");
        }
        /* Deprecated distance finding 
        [HarmonyPatch(nameof(Projectile.FixedUpdate))]
        [HarmonyPostfix]
        public static void FixedUpdateDistancePatch(Projectile __instance)
        {
            if (!__instance.m_didHit)
            {
                distanceTravelled += Time.fixedDeltaTime;
                //Jotunn.Logger.LogMessage($"distance: {distanceTravelled}");
            }
        }*/


        [HarmonyPatch(nameof(Projectile.OnHit))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> OnHitTranspile(IEnumerable<CodeInstruction> instructions)
        {
            //Jotunn.Logger.LogMessage("The call is replaced here");
            bool patchDone = false;

            foreach (var instruction in instructions)
            {
                if (instruction.Calls(AccessTools.Method(typeof(Character), "RaiseSkill")))
                  {
                    instruction.opcode = OpCodes.Call;
                    instruction.operand = AccessTools.DeclaredMethod(typeof(ProjectilePatch), nameof(ProjectilePatch.RaiseProjectileSkill));
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return instruction;

                    patchDone = true;
                } else
                    yield return instruction;
            }

            if (!patchDone)
            {
                Jotunn.Logger.LogError("Didn't find the Raise skill during projectile transpile");
            }
        }

        [HarmonyPatch(nameof(Projectile.RPC_OnHit))]
        [HarmonyPostfix]
        public static void RemoveProjectile(Projectile __instance)
        {
            Jotunn.Logger.LogMessage($"{__instance.gameObject.GetInstanceID()} destroyed");
            projectileList.Remove(__instance.gameObject.GetInstanceID());
        }

        public static void RaiseProjectileSkill (Character player, Skills.SkillType skill, float useless, Projectile instance)
        {
            float distanceTravelled = 0;
            if (projectileList.ContainsKey(instance.gameObject.GetInstanceID()))
            {
                distanceTravelled = (projectileList[instance.gameObject.GetInstanceID()] - instance.transform.position).magnitude;
                //Jotunn.Logger.LogMessage($"That projectile travelled {distanceTravelled} units");
            }
            else
            {
                Jotunn.Logger.LogError("Projectile not in dictionairy!");
            }

            if (skill == Skills.SkillType.Spears)
            {
                player.RaiseSkill(Skills.SkillType.Spears, ConfigManager.WeaponBXPSpearThrown.Value);
                //Jotunn.Logger.LogMessage($"Spear bonus exp!");
            } else if (skill == Skills.SkillType.Bows)
            {
                player.RaiseSkill(Skills.SkillType.Bows, distanceTravelled * ConfigManager.WeaponBXPBowDistanceMod.Value);
                //Jotunn.Logger.LogMessage($"Bow bonus exp: {distanceTravelled * BowBXPDistanceMod}");
            }
        }


    }
}
