using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace kingskills.Perks
{
    // and green thumb
    [HarmonyPatch]
    class P_Botany
    {
        [HarmonyPatch(typeof(Plant),nameof(Plant.GetHoverText)), HarmonyPostfix]
        public static void DisplayPlantInfo(Plant __instance, ref string __result)
        {
            //Jotunn.Logger.LogMessage("Getting hover text for plant");
            int infoLevel = 0;
            if (PerkMan.IsPerkActive(PerkMan.PerkType.Botany)) infoLevel++;
            if (PerkMan.IsPerkActive(PerkMan.PerkType.GreenThumb)) infoLevel++;
            //Jotunn.Logger.LogMessage($"info level is {infoLevel}");
            if (infoLevel == 0) return;

            bool usesYield = true;
            int baseYield = 0;
            Pickable grownObj = __instance.m_grownPrefabs[0].GetComponent<Pickable>();
            if (grownObj is null) usesYield = false;
            else
            {
                baseYield = grownObj.m_amount;
            }
            float grownPercent = (float)(__instance.TimeSincePlanted() / __instance.GetGrowTime());

            NewPlantInfo(ref __result, infoLevel, baseYield, grownPercent, false, usesYield);
        }

        [HarmonyPatch(typeof(Pickable), nameof(Pickable.GetHoverText)), HarmonyPostfix]
        public static void DisplayPickableInfo(Pickable __instance, ref string __result)
        {
            //Jotunn.Logger.LogMessage("Getting hover text for plant");
            int infoLevel = 0;
            if (PerkMan.IsPerkActive(PerkMan.PerkType.Botany)) infoLevel++;
            if (PerkMan.IsPerkActive(PerkMan.PerkType.GreenThumb)) infoLevel++;
            //Jotunn.Logger.LogMessage($"info level is {infoLevel}");
            if (infoLevel == 0) return;

            int baseYield = __instance.m_amount;
            float grownTime = (float)(ZNet.instance.GetTime() - 
                new DateTime(__instance.m_nview.m_zdo.GetLong("picked_time", 0L))).TotalMinutes;
            float grownPercent = grownTime / __instance.m_respawnTimeMinutes;

            NewPlantInfo(ref __result, infoLevel, baseYield, grownPercent, !__instance.m_picked);
        }

        public static void NewPlantInfo(ref string infoText, int infoLevel, int baseYield, float grownPercent, bool grown, bool usesYield = true)
        {
            float skillFactor = Player.m_localPlayer.GetSkillFactor(SkillMan.Agriculture);
            int minYield = baseYield + CFG.GetAgricultureRandomAdditionalYield(skillFactor, baseYield, 0);
            int maxYield = baseYield + CFG.GetAgricultureRandomAdditionalYield(skillFactor, baseYield, 1);
            float cleanGrowPer = Mathf.Round(grownPercent * 100f);

            if (infoLevel == 1)
            {
                if (cleanGrowPer > 90f)
                {
                    infoText += $"\nThis plant is almost grown!";
                }
                if (cleanGrowPer > 75f)
                {
                    infoText += $"\nThis plant is starting to flower.";
                }
                else if (cleanGrowPer > 50f)
                {
                    infoText += $"\nThis plant has taken root.";
                }
                else if (cleanGrowPer > 25f)
                {
                    infoText += $"\nThis plant has a while to go.";
                }
                else
                {
                    infoText += $"\nThis plant is just starting to grow.";
                }
            }
            else if (infoLevel == 2)
            {
                if (!grown) infoText += $"\n{cleanGrowPer}% grown";
                if (usesYield) infoText += $"\nYield: {minYield}-{maxYield}";
            }
        }
    }
}
