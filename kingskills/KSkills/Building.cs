using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace kingskills.KSkills
{
    /*
     * building
exp gained:
gained every time you place down a piece
you also gain exp based on how much you repair pieces
you gain bonus experience any time a creature damages your pieces
or even more if your pieces deal damage to a creature

effects:
increases piece damage
increases piece health
increases piece structural stability
gives chance for material rebate
reduces damage taken from wear and tear

perks:
50
	super poles - poles now offer like triple stability or something
	aoe repair
100
	adds several buildable traps
	all chests you build now have extra slots
    */

    [HarmonyPatch(typeof(WearNTear), nameof(WearNTear.OnPlaced))]
    public class WNTPlaced
    {
        public static bool isFree = false;

        [HarmonyPostfix]
        public static void AfterPlacement(WearNTear __instance)
        {
            if (LocalPlayerPlacing.isTrue)
            {
                ZDO zdo = __instance.GetComponent<ZNetView>().GetZDO();
                if (zdo == null) return;

                Player player = Player.m_localPlayer;
                float skill = player.GetSkillFactor(SkillMan.Building);

                //We decide if this is free
                isFree = ConfigMan.GetBuildingRandomFreeChance(skill);

                //When we place a piece down, it now remembers who we are and our skill level
                zdo.Set("Building Level", skill);
                zdo.Set("Building Owner", player.GetZDOID());
                zdo.Set("Is Free", isFree);

                Aoe trap = __instance.GetComponentInChildren<Aoe>();
                if (trap)
                {
                    zdo.Set("Trap", true);
                    //Jotunn.Logger.LogMessage("Set a trap!");
                }

                //And has addtional health
                __instance.m_health *= ConfigMan.GetBuildingHealthMult(skill);

                //We get some experience for it as well
                player.RaiseSkill(SkillMan.Building, ConfigMan.BuildXPPerPiece.Value);
            }
        }
    }


    [HarmonyPatch(typeof(Player), nameof(Player.UpdatePlacement))]
    public class AfterPlacement
    {
        [HarmonyFinalizer]
        private static void ResetFreeFlag()
        {
            WNTPlaced.isFree = false;
        }
    }


    //This will update the building to the correct health every time it is 
    //recreated
    [HarmonyPatch(typeof(WearNTear), nameof(WearNTear.Awake))]
    public class NewGameHealthCheck
    {
        [HarmonyPrefix]
        private static void BeforeAwaken(WearNTear __instance)
        {
            ZDO zdo = __instance.GetComponent<ZNetView>().GetZDO();
            if (zdo == null) return;

            float skill = zdo.GetFloat("Building Level", 0f);
            if (skill > 0)
                __instance.m_health *= ConfigMan.GetBuildingHealthMult(skill);


        }
    }


    [HarmonyPatch(typeof(WearNTear), nameof(WearNTear.GetMaterialProperties))]
    public class StabilityPatch
    {
        [HarmonyPostfix]
        private static void GetMyMaterialProps(WearNTear __instance,
            ref float maxSupport, ref float minSupport, ref float horizontalLoss, ref float verticalLoss)
        {
            ZDO zdo = __instance.GetComponent<ZNetView>().GetZDO();
            if (zdo == null) return;

            float skill;
            Player player = Player.m_localPlayer;
            Piece piece = __instance.GetComponent<Piece>();

            //This jumble is designed to ensure that we're using the stats of the correct player
            //when updating the stats
            //IsCreator calls the local player to ask if they're the creator
            if (piece != null && piece.IsCreator() && player != null)
            {
                skill = player.GetSkillFactor(SkillMan.Building);
                //Jotunn.Logger.LogMessage("Checked with the right player");
            }
            else
                skill = zdo.GetFloat("Building Level", 0f);

            float supportMult = ConfigMan.GetBuildingStabilityMult(skill);
            float lossRedux = ConfigMan.GetBuildingStabilityLossRedux(skill);

            maxSupport *= supportMult;
            horizontalLoss *= lossRedux;
            verticalLoss *= lossRedux;
        }
    }


    [HarmonyPatch(typeof(Player), nameof(Player.ConsumeResources))]
    public class FreeBuildingPatch
    {
        [HarmonyPrefix]
        [HarmonyPriority(Priority.First)]
        public static bool FreeCheck()
        {
            if (WNTPlaced.isFree) return false;

            return true;
        }
    }


    [HarmonyPatch(typeof(Piece), nameof(Piece.DropResources))]
    public class ShouldIGetResources
    {
        [HarmonyPrefix]
        private static bool CheckFree(Piece __instance)
        {
            //returns false if the building was free, which refuses to run drop resources
            return !__instance.GetComponent<ZNetView>().GetZDO().GetBool("Is Free");
        }
    }


    [HarmonyPatch(typeof(WearNTear), nameof(WearNTear.RPC_Damage))]
    public class BuildingGetDamaged
    {
        [HarmonyPrefix]
        public static void IsBuilding(WearNTear __instance, ref HitData hit)
        {
            //Jotunn.Logger.LogWarning($"I am a building and I took damage!");

            if (hit.m_attacker.IsNone())
            {
                //Jotunn.Logger.LogWarning($"From wear n tear!");

                float skill = __instance.m_nview.GetZDO().GetFloat("Building Level", 0f);

                hit.m_damage.Modify(ConfigMan.GetBuildingWNTRedux(skill));
            }
            else if (hit.GetAttacker().IsMonsterFaction())
            {
                Player player = Player.GetPlayer(__instance.m_nview.GetZDO().GetLong("Building Player", 0));
                if (player != null)
                {
                    player.RaiseSkill(SkillMan.Building, 
                        hit.GetTotalDamage() * ConfigMan.BuildXPDamageTakenMod.Value);
                }
            }
        }
    }

    [HarmonyPatch(typeof(Aoe))]
    public class BuildingDealDamage
    {
        [HarmonyPatch(nameof(Aoe.GetDamage), typeof(int))]
        [HarmonyPostfix]
        public static void GetMyDamage(Aoe __instance, ref HitData.DamageTypes __result)
        {
            if (__instance.m_nview?.GetZDO() == null || !__instance.m_nview.GetZDO().GetBool("Trap", false)) return;
                
            Player player = Player.m_localPlayer;
            if (player.GetZDOID().m_userID != __instance.m_nview.GetZDO().GetLong("Building Player")) return;

            __result.Modify(ConfigMan.GetBuildingDamageMult(player.GetSkillFactor(SkillMan.Building)));
            Jotunn.Logger.LogMessage($"Just multiplied the damage of this shit by {ConfigMan.GetBuildingDamageMult(player.GetSkillFactor(SkillMan.Building))}");
        }

        [HarmonyPatch(nameof(Aoe.OnHit))]
        [HarmonyPostfix]
        public static void GetXPBounty(Aoe __instance, Collider collider)
        {
            if (__instance.m_nview?.GetZDO() == null || !__instance.m_nview.GetZDO().GetBool("Trap", false)) return;

            Player player = Player.m_localPlayer;
            if (player.GetZDOID().m_userID != (long)__instance.m_nview.GetZDO().GetLong("Building Player")) return;

            Character enemy = collider.GetComponent<Character>();
            if (enemy == null || !enemy.IsMonsterFaction()) return;

            Jotunn.Logger.LogMessage($"Pretty sure we just hit an enemy. Gonna just give the exp to my player now");
            try
            {
                Jotunn.Logger.LogMessage($"also my owner is {__instance.m_owner.m_name}");
            }
            catch
            {
                Jotunn.Logger.LogMessage($"mb. we do not have an owner");
            }
            float damageXP = __instance.GetDamage().GetTotalDamage() * ConfigMan.BuildXPDamageDoneMod.Value;

            player.RaiseSkill(SkillMan.Building, damageXP);
        }
    }


    [HarmonyPatch(typeof(Player), nameof(Player.Repair))]
    public class LocalPlayerRepairing
    {
        public static bool isTrue = false;

        //I hate this caret, but it is just a space efficient way of doing {  }
        [HarmonyPrefix]
        public static void StartRepair() => 
            isTrue = true;

        [HarmonyFinalizer]
        public static void FinishRepair() => 
            isTrue = false;
    }


    [HarmonyPatch(typeof(WearNTear), nameof(WearNTear.Repair))]
    public class RepairTracker
    {
        public static Player playerRef = null;
        [HarmonyPrefix]
        public static void OnRepair()
        {
            if (LocalPlayerRepairing.isTrue)
                playerRef = Player.m_localPlayer;
            else
                playerRef = null;
        }

        [HarmonyPatch(nameof(WearNTear.RPC_Repair))]
        [HarmonyPrefix]
        public static void OnRPCRepair(WearNTear __instance)
        {
            if (playerRef != null)
            {
                if (!__instance.m_nview.IsValid() || !__instance.m_nview.IsOwner()) return;

                float healthChange = __instance.m_health - __instance.m_nview.GetZDO().GetFloat("health", __instance.m_health);
                if (healthChange < 0) healthChange = 0;

                playerRef.RaiseSkill(SkillMan.Building, ConfigMan.BuildXPRepairMod.Value * healthChange);
            }
        }
    }


    [HarmonyPatch(typeof(Player), nameof(Player.UseStamina))]
    public class UseLessStamina
    {
        [HarmonyPrefix]
        public static void BeforeCost(Player __instance, ref float v)
        {
            if (LocalPlayerRepairing.isTrue || LocalPlayerPlacing.isTrue)
                v *= ConfigMan.GetBuildingStaminaRedux(Player.m_localPlayer.GetSkillFactor(SkillMan.Building));
        }
    }

}
