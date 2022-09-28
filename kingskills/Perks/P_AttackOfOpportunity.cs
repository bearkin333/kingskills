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
    class P_AttackOfOpportunity
    {
        public static float hitTimer = CFG.AOOPCooldown.Value;
        public static AOOPZone aoopZone;
        public static Attack attackFX;

        [HarmonyPatch(typeof(Player),nameof(Player.CheckRun))]
        [HarmonyPostfix]
        public static void UpdateAOOP(Player __instance, float dt, ref bool __result)
        {
            //Jotunn.Logger.LogMessage($"Check run supposedly returning: {__result}");

            if (__result && CFG.CheckPlayerActivePerk(__instance, PerkMan.PerkType.AttackOfOpportunity) &&
                Util.GetPlayerWeapon(__instance) == __instance.m_unarmedWeapon.m_itemData)
            {
                if (!aoopZone || !aoopZone.obj) InitAOOP();
                //Jotunn.Logger.LogMessage($"Activating AOOP zone");


                if (!aoopZone.obj.activeSelf) aoopZone.obj.SetActive(true);
                if (hitTimer > 0) hitTimer -= dt;
            }
            else
            {
                if (!aoopZone) return;
                else if (aoopZone.obj.activeSelf) aoopZone.obj.SetActive(false);
            }
        }


        public static void OnHitTrigger(Character enemy)
        {
            //Jotunn.Logger.LogMessage($"triggered a hit against a certified enemy");
            if (hitTimer > 0) return;
            hitTimer = CFG.AOOPCooldown.Value;

            Player player = Player.m_localPlayer;

            ItemDrop.ItemData fists = player.m_unarmedWeapon.m_itemData;

            attackFX.DoNonAttack();
            HitData newHit = CFG.CreateHitFromWeapon(fists, player);
            newHit.m_dir = (player.transform.position - enemy.transform.position).normalized;
            newHit.m_damage.Modify(CFG.GetAOOPDamageMod());
            newHit.m_pushForce *= CFG.GetAOOPPushForceMod();

            enemy.Damage(newHit);
        }

        public static void InitAOOP()
        {
            GameObject obj = new GameObject();
            aoopZone = obj.AddComponent<AOOPZone>();
            aoopZone.obj = obj;
            aoopZone.InitAoe();

            attackFX = Player.m_localPlayer.m_unarmedWeapon.m_itemData.m_shared.m_attack.Clone();
            attackFX.m_weapon = Player.m_localPlayer.m_unarmedWeapon.m_itemData;
            attackFX.m_character = Player.m_localPlayer;
        }


    }

    public class AOOPZone : MonoBehaviour
    {
        public GameObject obj;
        public SphereCollider trigger;

        public void InitAoe()
        {
            trigger = obj.AddComponent<SphereCollider>();
            //BoxCollider collide = obj.AddComponent<BoxCollider>();
            obj.name = "player AOOP";
            //Jotunn.Logger.LogMessage($"AOE is being init");

            Player player = Player.m_localPlayer;

            obj.transform.parent = player.gameObject.transform;
            obj.transform.localPosition = Vector3.zero;

            trigger.radius = CFG.AOOPRange.Value;
            trigger.isTrigger = true;


            obj.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            //Jotunn.Logger.LogMessage($"AOOP registered a collision with {other}, and that game object is " +
            //    $"{other.gameObject.name}");
            Character hitChar = other.GetComponent<Character>();
            if (hitChar is null) return;
            if (!hitChar.IsMonsterFaction()) return;
            P_AttackOfOpportunity.OnHitTrigger(hitChar);
        }
    }
}

