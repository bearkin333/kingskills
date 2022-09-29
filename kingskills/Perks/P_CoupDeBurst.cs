using HarmonyLib;
using kingskills.SE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace kingskills.Perks
{
    [HarmonyPatch]
    class P_CoupDeBurst
    {
        public static float cooldown = 0f;
        public static bool isJumping = false;

        public static void JumpCheck(string hotkey)
        {
            if (!ZInput.GetButtonDown(hotkey)) return;
            if (!CFG.CheckPlayerActivePerk(Player.m_localPlayer, PerkMan.PerkType.CoupDeBurst)) return;
            if (Player.m_localPlayer.GetControlledShip() is null) return;
            if (Player.m_localPlayer.GetSEMan().HaveStatusEffect(KS_SEMan.ks_CoupDeBurstName) || isJumping) return;

            Player player = Player.m_localPlayer;
            Ship ship = player.GetControlledShip();
            Rigidbody body = ship.GetComponent<Rigidbody>();

            if (body is null) Jotunn.Logger.LogMessage($"No rigidbody");

            float force = CFG.CoupDeBurstForce.Value;
            Vector3 direction = Vector3.Lerp(ship.transform.forward, Vector3.up, CFG.CoupDeBurstAngle.Value);
            Vector3 point = ship.transform.position;

            foreach (Player players in ship.m_players)
            {
                players.AddNoise(CFG.CoupDeBurstNoise.Value);
            }

            body.AddForceAtPosition(direction * force, point, ForceMode.Impulse);
            isJumping = true;
            player.GetSEMan().AddStatusEffect(ScriptableObject.CreateInstance<SE_CoupDeBurst>());
        }


        [HarmonyPatch(typeof(Ship), nameof(Ship.FixedUpdate)), HarmonyPostfix]
        public static void Stabilize(Ship __instance)
        {
            if (!IsLocalPlayerCaptain(__instance)) return;
            if (__instance.GetComponent<KS_RotationConstraint>() is null)
            {
                KS_RotationConstraint xConstraint = __instance.gameObject.AddComponent<KS_RotationConstraint>();
                xConstraint.min = -7f;
                xConstraint.max = 7f;
                xConstraint.axis = ConstraintAxis.X;

                //KS_RotationConstraint yConstraint = __instance.gameObject.AddComponent<KS_RotationConstraint>();
                //yConstraint.min = -15f;
                //yConstraint.max = 15f;
                //yConstraint.axis = ConstraintAxis.Y;

                KS_RotationConstraint zConstraint = __instance.gameObject.AddComponent<KS_RotationConstraint>();
                zConstraint.min = -7f;
                zConstraint.max = 7f;
                zConstraint.axis = ConstraintAxis.Z;
                Jotunn.Logger.LogMessage("added constraints");
            }

            bool setActive = false;

            if (isJumping)
                setActive = true;

            foreach (KS_RotationConstraint cons in __instance.GetComponents<KS_RotationConstraint>())
            {
                cons.active = setActive;
                //Jotunn.Logger.LogMessage($"constraint's activation is :{setActive}");
            }

        }

        public static float checkGroundTimer = 0f;

        [HarmonyPatch(typeof(Ship), nameof(Ship.UpdateWaterForce)), HarmonyPostfix]
        public static void HitWater(Ship __instance, float dt)
        {
            if (!IsLocalPlayerCaptain(__instance)) return;

            if (isJumping)
            {
                checkGroundTimer += dt;
                if (checkGroundTimer < CFG.CoupDeBurstJumpDeadzone.Value) return;
                isJumping = false;
            }
            else
            {
                checkGroundTimer = 0f;
            }
        }

        public static bool IsLocalPlayerCaptain(Ship ship)
        {
            if (Player.m_localPlayer is null) return false;
            if (Player.m_localPlayer.GetControlledShip() is null) return false;
            if (ship != Player.m_localPlayer.GetControlledShip()) return false;
            return true;
        }
    }
}
