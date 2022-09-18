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
                __instance.m_nview is null ||
                !__instance.m_nview.IsValid() ||
                __instance.m_owner is null ||
                !__instance.m_owner.IsPlayer()) return;

            Jotunn.Logger.LogMessage("");

            ZDO zdo = __instance.m_nview.m_zdo;

            if (zdo.GetLong("Projectile Owner", 0) != 0) return;

            zdo.Set("Projectile Owner", __instance.m_owner.GetZDOID());
            zdo.Set("Start Pos", __instance.transform.position);

            Player playerRef = Player.m_localPlayer;

            if (__instance.m_owner.GetZDOID() == playerRef.GetZDOID())
            {
                    
                if (CFG.IsSkillActive(Skills.SkillType.Bows) &&
                    __instance.m_skill == Skills.SkillType.Bows)
                {
                    __instance.m_vel *=
                        CFG.GetBowVelocityMult(playerRef.GetSkillFactor(Skills.SkillType.Bows));
                }
                else if (CFG.IsSkillActive(Skills.SkillType.Spears) &&
                    __instance.m_skill == Skills.SkillType.Spears)
                {
                    __instance.m_damage.Modify(
                        CFG.GetSpearProjectileDamageMult(playerRef.GetSkillFactor(Skills.SkillType.Spears)));
                    __instance.m_vel *=
                        CFG.GetSpearVelocityMult(playerRef.GetSkillFactor(Skills.SkillType.Spears));
                }
            }
        }


        public static void RaiseProjectileSkill (Character player, Skills.SkillType skill, float useless, Projectile instance)
        {
            ZDO zdo = instance.m_nview.m_zdo;
            if (zdo.GetLong("Projectile Owner", 0) != Player.m_localPlayer.GetZDOID().m_userID) return;

            float distanceTravelled = (zdo.GetVec3("Start Pos", Vector3.zero) - instance.transform.position).magnitude;
                Jotunn.Logger.LogMessage($"That projectile travelled {distanceTravelled} units");

            if (skill == Skills.SkillType.Spears)
                LevelUp.BXP(player as Player, Skills.SkillType.Spears,
                    CFG.GetSpearBXPDistance(distanceTravelled));

            else if (skill == Skills.SkillType.Bows)
                LevelUp.BXP(player as Player, Skills.SkillType.Bows, 
                    CFG.GetBowBXPDistance(distanceTravelled));
        }
    }
}
