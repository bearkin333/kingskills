using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace kingskills.KSkills
{
    /*
     * sailing
exp gained:
exp gained slowly while standing on ship
exp gained slowly while swimming
exp gained based on movespeed while commandeering ship
exp gain increased based on how fully wind is caught
exp gain is increased based on tier of ship
all crew gets more experience for how many people are on the boat

effects:
required sailing levels for higher level ships
increases sailing speed while on a ship
nudges wind towards your sail on ship
increases exploration range while on ship
increased paddle speeds
    reduced ship damage taken

perks:
50
	buffs for current crew
	blah
100
	coup de burst
	blah
    */


    [HarmonyPatch(typeof(Ship))]
    public class ShipPatch
    {

        public static float controlTimer = 0f;
        [HarmonyPatch(nameof(Ship.FixedUpdate))]
        [HarmonyPostfix]
        public static void FixedUpdatePatch(Ship __instance)
        {
            Player player = Player.m_localPlayer;
            if (player == null) return;
            if (!__instance.IsPlayerInBoat(player)) return;

            float dt = Time.fixedDeltaTime;

            HandleExpUpdate(__instance, dt);

            //I only run this update once every few seconds, to save on computing power
            controlTimer += dt;
            if (controlTimer < CFG.SailControlTimer.Value)
                return;

            controlTimer = 0f;

            ZDO zdo = __instance?.m_nview?.GetZDO();
            if (zdo == null) return;
            if (!__instance.m_nview.IsOwner()) {Jotunn.Logger.LogMessage($"I don't own this boat! don't ask me"); return; }

            float baseRudderSpeed = zdo.GetFloat("Base Rudder Speed", 0);
            float basePaddleSpeed = zdo.GetFloat("Base Paddle Speed", 0);

            if (baseRudderSpeed == 0)
            {
                zdo.Set("Base Rudder Speed", __instance.m_rudderSpeed);
                baseRudderSpeed = __instance.m_rudderSpeed;
            }
            if (basePaddleSpeed == 0)
            {
                zdo.Set("Base Paddle Speed", __instance.m_backwardForce);
                basePaddleSpeed = __instance.m_backwardForce;
            }
            float skill = player.GetSkillFactor(SkillMan.Sailing);

            __instance.m_rudderSpeed = baseRudderSpeed * CFG.GetSailRudderSpeedMult(skill);
            __instance.m_backwardForce = basePaddleSpeed * CFG.GetSailPaddleSpeedMult(skill);
        }


        public static float expTimer = 0f;
        public static void HandleExpUpdate(Ship ship, float dt)
        {
            Player player = Player.m_localPlayer;
            float expBounty;

            bool captain = player.GetControlledShip() == ship;

            if (player.GetControlledShip() == ship)
            {
                expBounty = CFG.SailXPCaptainBase.Value;
                float capBounty = Mathf.Abs(ship.GetSpeed()) * CFG.SailXPSpeedMod.Value;
                capBounty *= CFG.GetSailXPWindMult(ship.GetWindAngleFactor());
                expBounty += capBounty;

            }
            else
            {
                expBounty = CFG.SailXPCrewBase.Value;
            }

            expBounty *= CFG.GetSailXPTierMult(ship);
            expBounty *= CFG.GetSailXPCrewMult(ship.m_players.Count);

            expTimer += dt;
            //Jotunn.Logger.LogMessage($"{expTimer} < {ConfigMan.SailXPTimerLength.Value}");

            if (expTimer >= CFG.SailXPTimerLength.Value)
            {
                expTimer -= CFG.SailXPTimerLength.Value;

                player.RaiseSkill(SkillMan.Sailing, expBounty);
            }
        }


        [HarmonyPatch(nameof(Ship.GetSailForce))]
        [HarmonyPostfix]
        public static void GetMySailForce(Ship __instance, ref Vector3 __result)
        {
            Player player = Player.m_localPlayer;
            if (!IsCaptain(__instance)) return;

            __result *= CFG.GetSailSpeedMult(player.GetSkillFactor(SkillMan.Sailing));
        }


        [HarmonyPatch(nameof(Ship.GetWindAngle))]
        [HarmonyPostfix]
        public static void GetWindAngleForHUD(Ship __instance, ref float __result)
        {
            Player player = Player.m_localPlayer;
            if (!IsCaptain(__instance)) return;

            Vector3 desiredWindDir = __instance.transform.forward;
            Vector3 windDir = EnvMan.instance.GetWindDir();
            Vector3 newResult = Vector3.Lerp(windDir, desiredWindDir,
                CFG.GetSailWindNudgeMod(player.GetSkillFactor(SkillMan.Sailing)));

            //No clue if this will work
            __result = Utils.YawFromDirection(__instance.transform.InverseTransformDirection(newResult));
        }


        [HarmonyPatch(nameof(Ship.GetWindAngleFactor))]
        [HarmonyPrefix]
        public static bool GetNudgedWindAngleFactor(Ship __instance, ref float __result)
        {
            Player player = Player.m_localPlayer;
            if (!IsCaptain(__instance)) return true;

            //I don't get paid to understand any of this
            //I just know it works
            float windAngleDif = Vector3.Dot(EnvMan.instance.GetWindDir(), -__instance.transform.forward);
            windAngleDif = Mathf.Clamp01(windAngleDif + 
                CFG.GetSailWindNudgeMod(player.GetSkillFactor(SkillMan.Sailing)));

            float magicPowers = Mathf.Lerp(0.7f, 1f, 1f - Mathf.Abs(windAngleDif));
            float mysticPowers = 1f - Utils.LerpStep(0.75f, 0.8f, windAngleDif);
            __result = magicPowers * mysticPowers;
            return false;
        }


        public static bool IsCaptain(Ship ship)
        {
            Player player = Player.m_localPlayer;
            if (player == null) return false;
            if (player.GetControlledShip() == null) return false;
            return player.GetControlledShip() == ship;
        }

    }


    [HarmonyPatch(typeof(Minimap), nameof(Minimap.Explore), typeof(Vector3), typeof(float))]
    public class ExploreRangePatch
    {
        [HarmonyPrefix]
        public static void BeforeExplore(Minimap __instance, ref float radius)
        {
            Player player = Player.m_localPlayer;
            if (player.GetStandingOnShip())
            {
                radius += CFG.GetSailExploreRange(player.GetSkillFactor(SkillMan.Sailing));
            }
        }
    }


    [HarmonyPatch(typeof(WearNTear), nameof(WearNTear.RPC_Damage))]
    public class ShipDamageReduction
    {
        [HarmonyPrefix]
        public static void ShipDamage(WearNTear __instance, ref HitData hit)
        {
            Ship ship = __instance.GetComponent<Ship>();
            Player player = Player.m_localPlayer;
            if (!ship || !ship.IsPlayerInBoat(player)) return;

            hit.m_damage.Modify(CFG.GetSailDamageRedux(player.GetSkillFactor(SkillMan.Sailing)));
        }
    }


    [HarmonyPatch(typeof(ShipControlls), nameof(ShipControlls.Interact))]
    public class BlockSailing
    {
        [HarmonyPrefix]
        public static bool YouMayNotPass(ShipControlls __instance)
        {
            Player player = Player.m_localPlayer;
            float skillLevel = player.GetSkillFactor(SkillMan.Sailing)*CFG.MaxSkillLevel.Value;
            float skillRQ = CFG.GetSailShipLevelRQ(__instance.m_ship);
            //Jotunn.Logger.LogMessage($"{skillLevel} out of {skillRQ}");

            //You may enter the palace
            if (skillLevel >= skillRQ)
                return CFG.DontSkipOriginal;

            Player.m_localPlayer.Message(MessageHud.MessageType.Center, 
                $"Your sailing skill is too low. You need {CFG.ColorPTRedFF}" + skillRQ.ToString("F0") + 
                $" sailing{CFG.ColorEnd} to commandeer this vessel!");
            return CFG.SkipOriginal;
        }
    }

}
