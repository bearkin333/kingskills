﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using static CharacterDrop;

namespace kingskills.Patches
{
    [HarmonyPatch(typeof(DropTable))]
    class LootDropPatch
    {
        [HarmonyPatch("GetDropList", new Type[] { typeof(int) })]
        [HarmonyPostfix]
        public static void DropTableFix(ref DropTable __instance, ref List<GameObject> __result, int amount)
        {
            //Jotunn.Logger.LogMessage("I think we're responsible for this kill, so I'm running the drop table change");
            Player playerRef = Player.m_localPlayer;
            
            //Check for player and do player stuff here
            if (__result.Count <= 0)
            {
                return;
            }

            List<GameObject> newDrops = new List<GameObject>(__result);
            float woodSkill = playerRef.GetSkillFactor(Skills.SkillType.WoodCutting);
            float mineSkill = playerRef.GetSkillFactor(Skills.SkillType.Pickaxes);
            float bowSkill = playerRef.GetSkillFactor(Skills.SkillType.Bows);

            float woodDropRate = CFG.GetWoodDropMod(woodSkill);
            float mineDropRate = CFG.GetMiningDropMod(mineSkill);
            float bowDropRate = CFG.GetBowDropRateMod(bowSkill);

            Dictionary<string, float> woodDrops
                = new Dictionary<string, float>(CFG.WoodcuttingDropTable);
            Dictionary<string, float> mineDrops
                = new Dictionary<string, float>(CFG.MiningDropTable);
            Dictionary<string, float> bowDrops
                = new Dictionary<string, float>(CFG.BowDropTable);

            bool lastResult = false;
            int i = 0;

            foreach (GameObject drop in __result)
            {
                if (i == __result.Count() - 1)
                {
                    //Jotunn.Logger.LogMessage("this entry is the last one");
                    lastResult = true;
                }

                //Jotunn.Logger.LogMessage($"list entry {i}: Drop is called {drop.name}");

                //Jotunn.Logger.LogMessage("Checking for wood drops..");
                if (CFG.IsSkillActive(Skills.SkillType.WoodCutting))
                    dropChecker(CFG.WoodcuttingDropTable, ref woodDrops,
                        woodDropRate, drop, ref newDrops, lastResult);

                //Jotunn.Logger.LogMessage("Checking for mine drops..");
                if (CFG.IsSkillActive(Skills.SkillType.Pickaxes))
                    dropChecker(CFG.MiningDropTable, ref mineDrops,
                        mineDropRate, drop, ref newDrops, lastResult);

                //Jotunn.Logger.LogMessage("Checking for bow drops..");
                if (CFG.IsSkillActive(Skills.SkillType.Bows))
                    dropChecker(CFG.BowDropTable, ref bowDrops,
                        bowDropRate, drop, ref newDrops, lastResult);

                i++;
            }

            __result = newDrops;
            
        }

        public static void dropChecker(Dictionary<string, float> dropCheckList, 
            ref Dictionary<string, float> dropTrackList,
            float dropRate, GameObject drop, ref List<GameObject> newDrops, 
            bool lastResult)
        {
            float dropBonus;

            foreach (KeyValuePair<string, float> item in dropCheckList)
            {
                if (drop.name == item.Key)
                {
                    //Jotunn.Logger.LogMessage($"Successfully matched item key '{item.Key}'");
                    dropBonus = dropTrackList[item.Key];

                    dropBonus += dropRate;
                    //Jotunn.Logger.LogMessage($"Increased that key's value to {dropBonus}");
                    if (dropBonus >= 1f)
                    {
                        for (int i = 0; i < Mathf.Floor(dropBonus); dropBonus--)
                        {
                            //Jotunn.Logger.LogMessage($"Adding an extra drop, making drop bonus {dropBonus}");
                            newDrops.Add(drop);
                        }

                        //Jotunn.Logger.LogMessage($"Now that I've made all those, it should be {dropBonus}");
                    }
                    if (lastResult &&
                        dropBonus >= CFG.GetDropItemThreshold())
                    {
                        newDrops.Add(drop);
                    }

                    dropTrackList[item.Key] = dropBonus;
                }
            }
        }
    }
}
