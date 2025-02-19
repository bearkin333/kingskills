﻿using HarmonyLib;
using kingskills.Perks;
using kingskills.RPC;
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
        public static float costMod = 1f;
        [HarmonyPostfix]
        public static void AfterPlacement(WearNTear __instance)
        {
            if (LocalPlayerPlacing.isTrue)
            {
                ZDO zdo = __instance.m_nview.m_zdo; 
                if (zdo == null) return;
                if (!__instance.m_nview.IsOwner()) __instance.m_nview.ClaimOwnership();

                Player player = Player.m_localPlayer;
                float skill = player.GetSkillFactor(SkillMan.Building);

                //Jotunn.Logger.LogMessage($"This building being free {isFree}");


                //We decide if this is free
                if (CFG.GetBuildingRandomFreeChance(skill)) 
                {
                    CustomWorldTextManager.AddCustomWorldText(CFG.ColorTitle,
                        __instance.transform.position + Vector3.up * 1.5f,
                        30, "Free Building!");
                    costMod = 0f;
                }
                else if (PerkMan.IsPerkActive(PerkMan.PerkType.Efficiency))
                {
                    costMod = CFG.GetEfficiencyCostRedux();
                }


                //When we place a piece down, it now remembers who we are and our skill level
                zdo.Set(CFG.BuildZDOLevel, skill);
                zdo.Set(CFG.BuildZDOOwner, player.GetPlayerID());
                zdo.Set(CFG.BuildZDOCost, costMod);
                zdo.Set(CFG.EngineerZDOActive, PerkMan.IsPerkActive(PerkMan.PerkType.Engineer));

                Aoe trap = __instance.GetComponentInChildren<Aoe>();
                if (trap)
                {
                    zdo.Set(CFG.BuildZDOIsTrap, true);
                    //Jotunn.Logger.LogMessage("Set a trap!");
                }

                //And has addtional health
                __instance.m_health *= CFG.GetBuildingHealthMult(skill);

                //We get some experience for it as well
                player.RaiseSkill(SkillMan.Building, CFG.BuildXPPerPiece.Value);
            }
        }
    }


    //This is put out so that our consume resources function can recognize we're
    //in the middle of a build place
    [HarmonyPatch(typeof(Player), nameof(Player.UpdatePlacement))]
    public class AfterPlacement
    {
        [HarmonyFinalizer]
        private static void ResetCostMod()
        {
            WNTPlaced.costMod = 1f;
        }
    }

    //This will update the building to the correct health every time it is 
    //recreated
    [HarmonyPatch(typeof(WearNTear), nameof(WearNTear.Awake))]
    public class NewGameHealthCheck
    {
        [HarmonyPostfix]
        private static void BeforeAwaken(WearNTear __instance)
        {
            ZDO zdo = __instance.m_nview?.m_zdo;
            if (zdo == null) return;

            float skill = zdo.GetFloat(CFG.BuildZDOLevel, 0f);
            if (skill > 0)
                __instance.m_health *= CFG.GetBuildingHealthMult(skill);

        }
    }


    [HarmonyPatch(typeof(WearNTear), nameof(WearNTear.GetMaterialProperties))]
    public class StabilityPatch
    {
        [HarmonyPostfix]
        private static void GetMyMaterialProps(WearNTear __instance,
            ref float maxSupport, ref float minSupport, ref float horizontalLoss, ref float verticalLoss)
        {
            ZDO zdo = __instance.m_nview.m_zdo;
            if (zdo == null) return;

            float skill;
            bool engineerActive;
            Player player = Player.m_localPlayer;
            Piece piece = __instance.GetComponent<Piece>();

            //This jumble is designed to ensure that we're using the stats of the correct player
            //when updating the stats
            //IsCreator calls the local player to ask if they're the creator
            if (piece != null && piece.IsCreator() && player != null)
            {
                skill = player.GetSkillFactor(SkillMan.Building);
                engineerActive = PerkMan.IsPerkActive(PerkMan.PerkType.Engineer);
                //Jotunn.Logger.LogMessage("Checked with the right player");
            }
            else
            {
                skill = zdo.GetFloat(CFG.BuildZDOLevel, 0f);
                engineerActive = zdo.GetBool(CFG.EngineerZDOActive, false);
            }

            float supportMult = CFG.GetBuildingStabilityMult(skill);
            float lossRedux = CFG.GetBuildingStabilityLossRedux(skill);

            if (engineerActive && P_Engineer.IsPillar(__instance))
            {
                supportMult *= CFG.GetEngineerStabilityMult();
                lossRedux *= CFG.GetEngineerSupportLossRedux();
            }

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
        public static bool FreeCheck(Player __instance, Piece.Requirement[] requirements, int qualityLevel)
        {
            float costMod = WNTPlaced.costMod;
            if (costMod == 1f) return CFG.DontSkipOriginal;
            if (costMod == 0f) return CFG.SkipOriginal;


            //Jotunn.Logger.LogMessage($"Requirement check:");
            foreach (Piece.Requirement requirement in requirements)
            {
                if (requirement.m_resItem is null) continue;


                int baseAmount = requirement.m_amount;
                int amount = Mathf.RoundToInt(baseAmount * costMod);
                if (baseAmount >= 1 && amount == 0) amount = 1;


                //Jotunn.Logger.LogMessage($"{requirement.m_resItem.m_itemData.m_shared.m_name}: " +
                //    $"was {baseAmount}, now {amount}");

                if (amount > 0)
                {
                    __instance.m_inventory.RemoveItem(requirement.m_resItem.m_itemData.m_shared.m_name, amount);
                }

            }

            return CFG.SkipOriginal;
        }
    }


    [HarmonyPatch(typeof(Piece), nameof(Piece.DropResources))]
    public class ShouldIGetResources
    {
        [HarmonyPrefix]
        private static bool CheckFree(Piece __instance)
        {
            float costMod = __instance.GetComponent<ZNetView>().GetZDO().GetFloat(CFG.BuildZDOCost, 1f);
            if (costMod == 0f) return CFG.SkipOriginal;
            if (costMod == 1f) return CFG.DontSkipOriginal;

            CFG.BuildCostChange(ref __instance.m_resources, costMod);

            return CFG.DontSkipOriginal;
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
                //Jotunn.Logger.LogWarning($"WNT damage on building");

                float skill = __instance.m_nview.GetZDO().GetFloat(CFG.BuildZDOLevel, 0f);

                hit.m_damage.Modify(CFG.GetBuildingWNTRedux(skill));
            }
            else if (hit.GetAttacker().IsMonsterFaction())
            {
                //Jotunn.Logger.LogWarning($"monster damage on building");

                Player player = Player.GetPlayer(__instance.m_nview.GetZDO().GetLong(CFG.BuildZDOOwner, 0));
                if (player != null)
                {
                    player.RaiseSkill(SkillMan.Building, 
                        hit.GetTotalDamage() * CFG.BuildXPDamageTakenMod.Value);
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
            if (__instance.m_nview?.GetZDO() == null || !__instance.m_nview.GetZDO().GetBool(CFG.BuildZDOIsTrap, false)) return;
            //Jotunn.Logger.LogMessage("Trap dealt damage");
                
            Player player = Player.m_localPlayer;
            if (player.GetPlayerID() != __instance.m_nview.GetZDO().GetLong(CFG.BuildZDOOwner)) return;

            __result.Modify(CFG.GetBuildingDamageMult(player.GetSkillFactor(SkillMan.Building)));
            //Jotunn.Logger.LogMessage($"Just multiplied the damage of this shit by {CFG.GetBuildingDamageMult(player.GetSkillFactor(SkillMan.Building))}");
        }

        [HarmonyPatch(nameof(Aoe.OnHit))]
        [HarmonyPostfix]
        public static void GetXPBounty(Aoe __instance, Collider collider)
        {
            if (__instance.m_nview?.GetZDO() == null || !__instance.m_nview.GetZDO().GetBool(CFG.BuildZDOIsTrap, false)) return;

            Player player = Player.m_localPlayer;
            if (player.GetPlayerID() != (long)__instance.m_nview.GetZDO().GetLong(CFG.BuildZDOOwner)) return;

            Character enemy = collider.GetComponent<Character>();
            if (enemy == null || !enemy.IsMonsterFaction()) return;

            //Jotunn.Logger.LogMessage($"Pretty sure we just hit an enemy. Gonna just give the exp to my player now");

            float damageXP = __instance.GetDamage().GetTotalDamage() * CFG.BuildXPDamageDoneMod.Value;

            player.RaiseSkill(SkillMan.Building, damageXP);
        }
    }


    [HarmonyPatch(typeof(Player), nameof(Player.Repair))]
    public class LocalPlayerRepairing
    {
        //I hate this caret, but it is just a space efficient way of doing {  }
        [HarmonyPrefix]
        public static void StartRepair(Player __instance) => __instance.m_nview.m_zdo.Set(CFG.BuildZDOPlayerRepairing, true);

        [HarmonyFinalizer]
        public static void FinishRepair(Player __instance) => __instance.m_nview.m_zdo.Set(CFG.BuildZDOPlayerRepairing, false);
    }


    [HarmonyPatch(typeof(WearNTear), nameof(WearNTear.Repair))]
    public class RepairTracker
    {
        [HarmonyPatch(nameof(WearNTear.RPC_Repair))]
        [HarmonyPrefix]
        public static void OnRPCRepair(WearNTear __instance)
        {
            //if (!__instance.m_nview.IsValid() || !__instance.m_nview.IsOwner()) return;
            Player playerRef = null;
            foreach (Player player in Player.GetAllPlayers())
            {
                if (player.m_nview.m_zdo.GetBool(CFG.BuildZDOPlayerRepairing, false))
                    playerRef = player;
            }
            if (playerRef is null) return;
            Jotunn.Logger.LogMessage($"Repairing player was {playerRef.GetPlayerName()}");

            float healthChange = __instance.m_health - __instance.m_nview.m_zdo.GetFloat("health", __instance.m_health);
            if (healthChange < 0) healthChange = 0;

            RPCMan.SendXP_RPC(playerRef.m_nview, CFG.BuildXPRepairMod.Value * healthChange, SkillMan.Building);
        }
    }


    [HarmonyPatch(typeof(Player), nameof(Player.UseStamina))]
    public class UseLessStamina
    {
        [HarmonyPrefix]
        public static void BeforeCost(Player __instance, ref float v)
        {
            if (Player.m_localPlayer.m_nview.m_zdo.GetBool(CFG.BuildZDOPlayerRepairing, false) || LocalPlayerPlacing.isTrue)
                v *= CFG.GetBuildingStaminaRedux(Player.m_localPlayer.GetSkillFactor(SkillMan.Building));
        }
    }

}
