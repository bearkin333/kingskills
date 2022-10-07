using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace kingskills.Patches
{
    [HarmonyPatch(typeof(Projectile))]
    class ProjectilePatch
    {
        [HarmonyPatch(nameof(Projectile.FixedUpdate))]
        [HarmonyPrefix]
        public static void OnFirstUpdate(Projectile __instance)
        {
            if (__instance.m_didHit || !__instance.m_nview.IsValid() || !__instance.m_nview.IsOwner() ||
                __instance.m_owner is null || !__instance.m_owner.IsPlayer()) return;

            //Jotunn.Logger.LogMessage("");

            ZDO zdo = __instance.m_nview.m_zdo;

            if (zdo.GetLong(CFG.SpearZDOOwner, 0) != 0) return;
            //Jotunn.Logger.LogMessage($"This projectile's owner was read as {zdo.GetLong(CFG.SpearZDOOwner)}. since that's 0, " +
            //    $"we are setting it's owner now to {__instance.m_owner.GetZDOID()}, the ZDOID of the projectile's m_owner");

            zdo.Set(CFG.SpearZDOOwner, __instance.m_owner.GetZDOID().m_userID);
            zdo.Set(CFG.SpearZDOStartPos, __instance.transform.position);

            Player playerRef = Player.m_localPlayer;
            //Jotunn.Logger.LogMessage($"the associated skill is {__instance.m_skill}");

            if (__instance.m_owner.GetZDOID() == playerRef.GetZDOID())
            {
                //Jotunn.Logger.LogMessage($"changing vel and damange from {__instance.m_vel} and {__instance.m_damage.GetTotalDamage()}");

                if (CFG.IsSkillActive(Skills.SkillType.Bows) &&
                    __instance.m_skill == Skills.SkillType.Bows)
                {
                    //Jotunn.Logger.LogMessage($"changing bow stats");

                    __instance.m_vel *=
                        CFG.GetBowVelocityMult(playerRef.GetSkillFactor(Skills.SkillType.Bows));
                }
                else if (CFG.IsSkillActive(Skills.SkillType.Spears) &&
                    __instance.m_skill == Skills.SkillType.Spears)
                {
                    //Jotunn.Logger.LogMessage($"changing spear stats");

                    __instance.m_damage.Modify(
                        CFG.GetSpearProjectileDamageMult(playerRef.GetSkillFactor(Skills.SkillType.Spears)));
                    __instance.m_vel *=
                        CFG.GetSpearVelocityMult(playerRef.GetSkillFactor(Skills.SkillType.Spears));
                }

                //Jotunn.Logger.LogMessage($"to {__instance.m_vel} and {__instance.m_damage.GetTotalDamage()}");
            }
        }


        public static void RaiseProjectileSkill (Character player, Skills.SkillType skill, float useless, Projectile instance)
        {
            ZDO zdo = instance.m_nview.m_zdo;
            if (zdo.GetLong(CFG.SpearZDOOwner, 0) != Player.m_localPlayer.GetZDOID().m_userID) return;

            float distanceTravelled = (zdo.GetVec3(CFG.SpearZDOStartPos, Vector3.zero) - instance.transform.position).magnitude;
            //Jotunn.Logger.LogMessage($"That projectile travelled {distanceTravelled} units");

            if (skill == Skills.SkillType.Spears)
                LevelUp.BXP(player as Player, Skills.SkillType.Spears,
                    CFG.GetSpearBXPDistance(distanceTravelled));

            else if (skill == Skills.SkillType.Bows)
                LevelUp.BXP(player as Player, Skills.SkillType.Bows, 
                    CFG.GetBowBXPDistance(distanceTravelled));
        }
    }
}
