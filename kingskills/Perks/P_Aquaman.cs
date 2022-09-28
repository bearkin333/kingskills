using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace kingskills.Perks
{
    [HarmonyPatch(typeof(Character))]
    class P_Aquaman
    {
        public static Aoe fishAoe = null;
        public static float fishTimer = 0f;
        public static float tickTimer = 0f;
        public static bool maxed = false;

        [HarmonyPatch(nameof(Character.UpdateMotion))]
        [HarmonyPostfix]
        public static void AquamanUpdate(Player __instance, float dt)
        {
            if (!CFG.CheckPlayerActivePerk(__instance, PerkMan.PerkType.Aquaman)) return;
            if (fishAoe is null || fishAoe.gameObject == null) SetupAOE();

            if (!__instance.InLiquidSwimDepth())
            {
                if (fishTimer > 0f) fishTimer -= dt * CFG.AquamanDecayOutOfWater.Value;
                else fishTimer = 0f;

                tickTimer = 0f;
                try
                {
                    fishAoe.gameObject.SetActive(false);
                }
                catch
                {
                    Jotunn.Logger.LogWarning($"fishaoe does not have a gameobject to set false");
                }
                maxed = false;
            }
            else
            {
                fishTimer += dt;
                tickTimer += dt;

                if (tickTimer < CFG.AquamanUpdateTimer.Value) return;

                tickTimer -= CFG.AquamanUpdateTimer.Value;

                if (fishTimer < CFG.AquamanTimerStart.Value)
                {
                    //Aquaman hasn't started yet. Carry on
                    AquamanNotStarted();
                }
                else if (fishTimer < CFG.AquamanTimeTilMax.Value + CFG.AquamanTimerStart.Value)
                {
                    AquamanGrowing();
                }
                else
                {
                    AquamanMax();
                }
            }
        }


        public static void AquamanNotStarted()
        {
        }
        public static void AquamanGrowing()
        {
            try
            {
                //Jotunn.Logger.LogMessage("Setting the fishaoe gameobject to active");
                fishAoe.gameObject.SetActive(true);
                //Jotunn.Logger.LogMessage($"it's name is {fishAoe.gameObject.name}");
            }
            catch
            {
                Jotunn.Logger.LogWarning($"fishaoe does not have a gameobject");
            }
            //
            //Jotunn.Logger.LogMessage("growing Aquaman");
            UpdateAOEStats();
        }
        public static void AquamanMax()
        {
            if (maxed) return;
            maxed = true;

            //Jotunn.Logger.LogMessage("Maxed Aquaman");
            UpdateAOEStats();
        }


        public static void SetupAOE()
        {
            GameObject obj = new GameObject();
            fishAoe = obj.AddComponent<Aoe>();
            //BoxCollider collide = obj.AddComponent<BoxCollider>();
            obj.name = "fishAoe";

            Player player = Player.m_localPlayer;

            obj.transform.parent = player.gameObject.transform;
            obj.transform.localPosition = Vector3.zero;

            fishAoe.m_attachToCaster = true;
            fishAoe.m_hitInterval = CFG.AquamanUpdateTimer.Value;
            fishAoe.m_ttl = 0f;
            fishAoe.m_useTriggers = false;
            //fishAoe.m_triggerEnterOnly = true;
            fishAoe.m_useCollider = null;
            fishAoe.m_hitParent = false;
            fishAoe.m_hitFriendly = false;
            fishAoe.m_hitEnemy = true;
            fishAoe.m_hitCharacters = true;
            fishAoe.m_hitProps = true;
            fishAoe.m_skill = Skills.SkillType.Swim;

            fishAoe.Awake();

            HitData hit = new HitData();
            HitData.DamageTypes damage = new HitData.DamageTypes();
            hit.m_blockable = false;
            hit.m_dodgeable = true;
            hit.m_pushForce = CFG.AquamanPushForce.Value;
            hit.m_backstabBonus = CFG.AquamanBackstab.Value;
            hit.m_damage = damage;

            fishAoe.Setup(player, Vector3.zero, 0f, hit, null);
            fishAoe.Start();
            obj.SetActive(false);
        }
        public static void UpdateAOEStats()
        {
            Player player = Player.m_localPlayer;

            fishAoe.m_radius = CFG.GetAquamanRadius(fishTimer);
            fishAoe.m_damage.m_pierce = CFG.GetAquamanDamage(fishTimer);

            //Jotunn.Logger.LogMessage($"fishaoe position is {fishAoe.gameObject.transform.position} " +
            //    $"\nlocal position is supposedly {fishAoe.gameObject.transform.localPosition}");
            //Jotunn.Logger.LogMessage($"player position is {player.gameObject.transform.position} ");

            //Jotunn.Logger.LogMessage($"Radius is now {fishAoe.m_radius}");
            //Jotunn.Logger.LogMessage($"damage is now {fishAoe.m_damage.GetTotalDamage()} pierce");
        }

        /*
        [HarmonyPatch(typeof(Aoe), nameof(Aoe.OnHit))]
        [HarmonyPrefix]
        public static void DetectedHit(Aoe __instance)
        {
            Jotunn.Logger.LogMessage($"Detected onhit on an AOE whose name is {__instance.gameObject.name}");
        }
        */
    }
}
