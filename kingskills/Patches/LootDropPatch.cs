using System;
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
            //Jotunn.Logger.LogMessage("drop table fix running");
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

            float woodDropRate = ConfigManager.GetWoodDropRate(woodSkill);
            float mineDropRate = ConfigManager.GetMiningDropRate(mineSkill);
            float bowDropRate = ConfigManager.GetBowDropRate(bowSkill);

            Dictionary<string, float> woodDrops
                = new Dictionary<string, float>(ConfigManager.WoodcuttingDropTable);
            Dictionary<string, float> mineDrops
                = new Dictionary<string, float>(ConfigManager.MiningDropTable);
            Dictionary<string, float> bowDrops
                = new Dictionary<string, float>(ConfigManager.BowDropTable);

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
                dropChecker(ConfigManager.WoodcuttingDropTable, ref woodDrops,
                    woodDropRate, drop, ref newDrops, lastResult);

                //Jotunn.Logger.LogMessage("Checking for mine drops..");
                dropChecker(ConfigManager.MiningDropTable, ref mineDrops,
                    mineDropRate, drop, ref newDrops, lastResult);

                //Jotunn.Logger.LogMessage("Checking for bow drops..");
                dropChecker(ConfigManager.BowDropTable, ref bowDrops,
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
            float dropBonus = 0;

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
                        for (int i = 1; i < Mathf.Floor(dropBonus); dropBonus--)
                        {
                            //Jotunn.Logger.LogMessage($"Adding an extra drop, making drop bonus {dropBonus}");
                            newDrops.Add(drop);
                        }

                        //Jotunn.Logger.LogMessage($"Now that I've made all those, it should be {dropBonus}");
                    }
                    if (lastResult &&
                        dropBonus >= ConfigManager.GetDropItemThreshold())
                    {
                        newDrops.Add(drop);
                    }

                    dropTrackList[item.Key] = dropBonus;
                }
            }
        }
    }
}
