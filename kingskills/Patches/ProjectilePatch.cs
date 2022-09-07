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
    [HarmonyPatch(typeof(Projectile))]
    class ProjectilePatch
    {
        [HarmonyPatch(nameof(Projectile.FixedUpdate))]
        [HarmonyPrefix]
        public static void OnFirstUpdate(Projectile __instance)
        {
            if (__instance.m_didHit == true ||
                !__instance.m_owner.IsPlayer() ||
                !__instance.m_nview.IsValid()) return;

            ZDO zdo = __instance.m_nview.m_zdo;

            if (zdo.GetFloat("Projectile Owner", 0) != 0) return;

            zdo.Set("Projectile Owner", __instance.m_owner.GetZDOID());
            zdo.Set("Start Pos", __instance.transform.position);

            Player playerRef = Player.m_localPlayer;

            if (__instance.m_owner.GetZDOID() == playerRef.GetZDOID())
            {
                    
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

        public static void RaiseProjectileSkill (Character player, Skills.SkillType skill, float useless, Projectile instance)
        {
            ZDO zdo = instance.m_nview.m_zdo;
            if (zdo.GetLong("Projectile Owner", 0) != Player.m_localPlayer.GetZDOID().m_userID) return;

            float distanceTravelled = (zdo.GetVec3("Start Pos", Vector3.zero) - instance.transform.position).magnitude;
                Jotunn.Logger.LogMessage($"That projectile travelled {distanceTravelled} units");

            if (skill == Skills.SkillType.Spears)
                LevelUp.BXP(player as Player, Skills.SkillType.Spears, ConfigMan.WeaponBXPSpearThrown.Value);

            else if (skill == Skills.SkillType.Bows)
                LevelUp.BXP(player as Player, Skills.SkillType.Bows, 
                    distanceTravelled * ConfigMan.GetWeaponBXPBowDistanceMult());
        }
    }
}
