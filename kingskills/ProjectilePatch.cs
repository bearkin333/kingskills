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
        public static float distanceTravelled;
        public const float SpearBXPThrown = 20f;
        public const float BowBXPDistanceMod = .5f;
        public static Projectile projRef;

        [HarmonyPatch(nameof(Projectile.Awake))]
        [HarmonyPostfix]
        public static void InitProj(Projectile __instance)
        {
            distanceTravelled = 0f;
            projRef = __instance;
        }


        [HarmonyPatch(nameof(Projectile.FixedUpdate))]
        [HarmonyPostfix]
        public static void FixedUpdateDistancePatch(Projectile __instance)
        {
            if (!__instance.m_didHit)
            {
                distanceTravelled += Time.fixedDeltaTime;
                //Jotunn.Logger.LogMessage($"distance: {distanceTravelled}");
            }
        }


        [HarmonyPatch(nameof(Projectile.OnHit))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> OnHitTranspile(IEnumerable<CodeInstruction> instructions)
        {
            //Jotunn.Logger.LogMessage("The call is replaced here");
            var newInstructions = new List<CodeInstruction>(instructions);
            bool patchDone = false;

            foreach (var instruction in instructions)
            {
                if (instruction.Calls(AccessTools.Method(typeof(Character), "RaiseSkill")))
                  {
                    instruction.opcode = OpCodes.Call;
                    instruction.operand = AccessTools.DeclaredMethod(typeof(ProjectilePatch), nameof(ProjectilePatch.RaiseProjectileSkill));
                        
                    yield return instruction;

                    patchDone = true;
                }
                    
                else
                {
                        yield return instruction;
                }
                
            }

            if (!patchDone)
            {
                Jotunn.Logger.LogFatal("Didn't find the Raise skill during projectile transpile");
            }
        }
    
        public static void RaiseProjectileSkill (Skills.SkillType skill)
        {
            //Jotunn.Logger.LogMessage("Raise projectile skill is called");
            if (projRef == null) return;

            //Jotunn.Logger.LogMessage($"And carries on, and the skill is {skill}");
            if (skill == Skills.SkillType.Spears)
            {
                projRef.m_owner.RaiseSkill(Skills.SkillType.Spears, SpearBXPThrown);
                Jotunn.Logger.LogMessage($"Spear bonus exp!");
            } else if (skill == Skills.SkillType.Bows)
            {
                projRef.m_owner.RaiseSkill(Skills.SkillType.Bows, distanceTravelled * BowBXPDistanceMod);
                Jotunn.Logger.LogMessage($"Bow bonus exp: {distanceTravelled * BowBXPDistanceMod}");
            }
        }
    }
}
