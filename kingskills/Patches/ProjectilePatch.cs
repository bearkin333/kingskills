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
        
        /*
        [HarmonyPatch(nameof(Projectile.Awake))]
        [HarmonyPostfix]
        public static void InitProj(Projectile __instance)
        {
            //Jotunn.Logger.LogMessage($"Projectile just awakened.");
        }
        */

        [HarmonyPatch(nameof(Projectile.FixedUpdate))]
        [HarmonyPrefix]
        public static void OnFirstUpdate(Projectile __instance)
        {
            if (__instance.m_didHit == true) return;
            if (!projectileList.ContainsKey(__instance.gameObject.GetInstanceID()))
            {
                //Jotunn.Logger.LogMessage($"This projectile update, I've found a projectile that isn't in my dictionary.");
                Player playerRef = Player.m_localPlayer;
                if (__instance.m_owner.IsPlayer() && __instance.m_owner.GetZDOID() == playerRef.GetZDOID())
                {
                    projectileList.Add(__instance.gameObject.GetInstanceID(), __instance.transform.position);
                    Jotunn.Logger.LogMessage($"Added projectile {__instance.gameObject.GetInstanceID()}");

                    //Jotunn.Logger.LogMessage($"The skill is {__instance.m_skill} and the current velocity is {__instance.m_vel}");
                    if (ConfigMan.IsSkillActive(Skills.SkillType.Bows) &&
                        __instance.m_skill == Skills.SkillType.Bows)
                    {
                        __instance.m_vel *=
                            ConfigMan.GetBowVelocityMult(playerRef.GetSkillFactor(Skills.SkillType.Bows));
                    }
                    else if (ConfigMan.IsSkillActive(Skills.SkillType.Spears) &&
                        __instance.m_skill == Skills.SkillType.Spears)
                    {
                        __instance.m_damage.Modify(
                            ConfigMan.GetSpearProjectileDamageMult(playerRef.GetSkillFactor(Skills.SkillType.Spears)));
                        __instance.m_vel *=
                            ConfigMan.GetSpearVelocityMult(playerRef.GetSkillFactor(Skills.SkillType.Spears));
                    }

                }
            }
        }


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
                //Jotunn.Logger.LogError("Didn't find the Raise skill during projectile transpile");
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
            Jotunn.Logger.LogMessage($"On hit was triggerred");

            float distanceTravelled = 0;
            if (projectileList.ContainsKey(instance.gameObject.GetInstanceID()))
            {
                distanceTravelled = (projectileList[instance.gameObject.GetInstanceID()] - instance.transform.position).magnitude;
                Jotunn.Logger.LogMessage($"That projectile travelled {distanceTravelled} units");
            }
            else
            {
                Jotunn.Logger.LogError("Projectile not in dictionairy!");
            }

            if (ConfigMan.IsSkillActive(Skills.SkillType.Spears) &&
                skill == Skills.SkillType.Spears)
            {
                player.RaiseSkill(Skills.SkillType.Spears, ConfigMan.WeaponBXPSpearThrown.Value);

                CustomWorldTextManager.CreateBXPText(
                    CustomWorldTextManager.GetInFrontOfCharacter(player),
                    ConfigMan.WeaponBXPSpearThrown.Value);
            } 
            else if (ConfigMan.IsSkillActive(Skills.SkillType.Bows) &&
                skill == Skills.SkillType.Bows)
            {
                player.RaiseSkill(Skills.SkillType.Bows, distanceTravelled * ConfigMan.GetWeaponBXPBowDistanceMult());

                CustomWorldTextManager.CreateBXPText(
                    CustomWorldTextManager.GetInFrontOfCharacter(player),
                    distanceTravelled * ConfigMan.GetWeaponBXPBowDistanceMult());
                //Jotunn.Logger.LogMessage($"Bow bonus exp: {distanceTravelled * BowBXPDistanceMod}");
            }
        }


    }
}
