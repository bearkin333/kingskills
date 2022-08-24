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
    class LootDropPatch
    {
        [HarmonyPatch(typeof(DropTable), "GetDropList", new Type[] { typeof(int) })]
        [HarmonyPostfix]
        public static void DropTableFix(ref DropTable __instance, ref List<GameObject> __result)
        {
            Player playerRef = Player.m_localPlayer;

            //Check for player and do player stuff here

            List<GameObject> newDrops = __result;
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
                i++;
                if (i == __result.Count() - 1)
                    lastResult = true;

                dropChecker(woodDrops, woodDropRate, drop, ref newDrops, lastResult);
                dropChecker(mineDrops, mineDropRate, drop, ref newDrops, lastResult);
                dropChecker(bowDrops, bowDropRate, drop, ref newDrops, lastResult);
            }

            __result = newDrops;
        }

        public static void dropChecker(Dictionary<string, float> dropCheckList, float dropRate,
            GameObject drop, ref List<GameObject> newDrops, bool lastResult)
        {
            foreach (KeyValuePair<string, float> item in dropCheckList)
            {
                if (drop.name == item.Key)
                {
                    dropCheckList[item.Key] += dropRate;
                    if (item.Value >= 1f)
                    {
                        float decreaseValue = 0;
                        for (int i = 0; i <= Mathf.Floor(item.Value); i++)
                        {
                            newDrops.Add(drop);
                        }
                        dropCheckList[item.Key] -= decreaseValue;
                    }
                    if (lastResult &&
                        item.Value >= ConfigManager.DropNewItemThreshold.Value)
                    {
                        newDrops.Add(drop);
                    }
                }
            }
        }
    }
}
