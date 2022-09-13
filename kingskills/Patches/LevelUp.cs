using HarmonyLib;
using kingskills.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace kingskills
{
    [HarmonyPatch(typeof(Player))]
    class LevelUp
    {

        [HarmonyPatch(nameof(Player.GetSkillFactor))]
        [HarmonyPrefix]
        public static bool GetMySkillFactor(Player __instance, ref float __result, Skills.SkillType skill)
        {
            //Jotunn.Logger.LogMessage("Checking skill");
            
            if (skill == Skills.SkillType.None) __result = 0f;

            //Ascended skills will always act as though at max level
            //No longer the case
            //if (Perks.IsSkillAscended(skill))
            //    __result = 1f;
            //else
                
            __result = Mathf.Clamp01(__instance.GetSkills().GetSkillLevel(skill) / CFG.MaxSkillLevel.Value);

            return false;
        }

        [HarmonyPatch(nameof(Player.OnSkillLevelup))]
        [HarmonyPostfix]
        public static void SkillLevelup(Player __instance, Skills.SkillType skill)
        {
            StatsUpdate.UpdateStats(__instance, true, skill);
        }

        [HarmonyPatch(nameof(Player.RaiseSkill))]
        [HarmonyPrefix]
        public static bool RaiseMySkills(Player __instance, Skills.SkillType skill, float value = 1f)
        {
            //Largely stolen from the game
            if (skill == Skills.SkillType.None)
            {
                return false;
            }
            if (!WeaponRaiseSkillTakeover.ksOverride &&
                (WeaponRaiseSkillTakeover.aoeIgnore ||
                WeaponRaiseSkillTakeover.areaIgnore ||
                WeaponRaiseSkillTakeover.meleeIgnore)) return false;
            WeaponRaiseSkillTakeover.ksOverride = false;

            Skills.Skill skillActual = __instance.GetSkills().GetSkill(skill);

            //Allow status effects to modify exp gain rate
            __instance.m_seman.ModifyRaiseSkill(skill, ref value);


            if (CFG.IsSkillActive(Skills.SkillType.Run) && skill == Skills.SkillType.Run)
            {
                value *= MovePatch.absoluteWeightBonus(__instance);
                value *= MovePatch.relativeWeightBonus(__instance);
                value *= MovePatch.runSpeedExpBonus(__instance);
            }
            else if (CFG.IsSkillActive(Skills.SkillType.Swim) && skill == Skills.SkillType.Swim)
            {
                value *= MovePatch.absoluteWeightBonus(__instance);
                value *= MovePatch.relativeWeightBonus(__instance);
                value *= MovePatch.swimSpeedExpBonus(__instance);
            }


            if (KingSkillRaise(skillActual, __instance, value))
                if (__instance.GetSkills().m_useSkillCap)
                    __instance.GetSkills().RebalanceSkills(skill);

            return false;
        }

        public static void BXP(Player player, Skills.SkillType skill, float number){
            if (!CFG.IsSkillActive(skill)) return;

            player.m_nview.GetZDO().Set("BXP", true);
            WeaponRaiseSkillTakeover.DoNotIgnore();

            player.RaiseSkill(skill, number);

            player.m_nview.GetZDO().Set("BXP", false);
        }

        public static void CustomRaiseSkill(Player player, Skills.SkillType skill, float value, bool displayEXP = true)
        {
            player.m_nview.GetZDO().Set("Display", displayEXP);

            player.RaiseSkill(skill, value);

            player.m_nview.GetZDO().Set("Display", true);
        }


        public static bool KingSkillRaise(Skills.Skill skill, Player player, float factor)
        {
            if (skill.m_level >= CFG.MaxSkillLevel.Value)
                return false;

            float num = skill.m_info.m_increseStep * factor;
            skill.m_accumulator += num;
            Skills.SkillType skillT = skill.m_info.m_skill;

            float nextLevelRequirement = skill.GetNextLevelRequirement();
            while (skill.m_accumulator >= nextLevelRequirement)
            {
                skill.m_level += 1f;
                skill.m_level = Mathf.Clamp(skill.m_level, 0f, CFG.MaxSkillLevel.Value);
                LevelUpPing(player, skill);

                skill.m_accumulator -= nextLevelRequirement;
                if (skill.m_level >= CFG.MaxSkillLevel.Value)
                {
                    OnMaxLevel(skillT);
                    player.Message(MessageHud.MessageType.Center, "MAX LEVEL!");
                    break;
                }

                nextLevelRequirement = skill.GetNextLevelRequirement();
            }

            float percent = skill.m_accumulator / (skill.GetNextLevelRequirement() / CFG.MaxSkillLevel.Value);

            //Display the in-world text
            if (factor >= CFG.DisplayExperienceThreshold.Value &&
                player.m_nview.m_zdo.GetBool("Display", true))
            {
                string skillName = CFG.GetNameFromSkill(skillT);
                Color msgColor;
                string msgTxt;
                Vector3 pos;
                int textSize = CFG.GetXPTextScaledSize(factor);

                //When we do bonus experience, we run through a function that automatically sets
                //BXP to true. Using this, we can get information into this function - even though
                //Raise Skill is normally only called using their framework
                if (player.m_nview.m_zdo.GetBool("BXP", false))
                {
                    msgTxt = "+" + factor.ToString("F1") + " BONUS EXPERIENCE!\n" + skillName;
                    msgColor = CFG.ColorBonusBlue;
                    pos = CustomWorldTextManager.GetInFrontOfCharacter(player) +
                        CustomWorldTextManager.GetRandomPosOffset();
                }
                else
                {
                    msgTxt = "+" + factor.ToString("F1") + " xp\n" + skillName;
                    msgColor = CFG.ColorExperienceYellow;
                    pos = CustomWorldTextManager.GetAboveCharacter(player) +
                        CustomWorldTextManager.GetRandomPosOffset();
                }

                CustomWorldTextManager.AddCustomWorldText(msgColor, pos, textSize, msgTxt);
            }

            return false;
        }

        public static void OnMaxLevel(Skills.SkillType skill)
        {
            if (!AscensionManager.isAscendable.ContainsKey(skill))
                AscensionManager.isAscendable[skill] = true;
            else
                AscensionManager.isAscendable.Add(skill, true);

            Player.m_localPlayer.ShowTutorial("kingskills_ascend");
        }

        [HarmonyPatch(typeof(Skills),nameof(Skills.LowerAllSkills))]
        [HarmonyPostfix]
        public static void LoseAscendable(Skills __instance)
        {
            Dictionary<Skills.SkillType, bool> ascendableChecker = 
                new Dictionary<Skills.SkillType, bool>(AscensionManager.isAscendable);

            foreach (KeyValuePair<Skills.SkillType, bool> ascendable in ascendableChecker)
            {
                if (ascendable.Value)
                {
                    Jotunn.Logger.LogMessage($"Since we lost skills, just checking again to see if {ascendable.Key} is still ascendable");
                    if (__instance.GetSkillLevel(ascendable.Key) < CFG.MaxSkillLevel.Value)
                    {
                        Jotunn.Logger.LogMessage("Yup. Not ascendable anymore");
                        AscensionManager.isAscendable[ascendable.Key] = false;
                    }
                }
            }
        }

        public static void LevelUpPing(Player player, Skills.Skill skill)
        {
            float level = skill.m_level;
            player.OnSkillLevelup(skill.m_info.m_skill, level);
            MessageHud.MessageType type = (((int)level != 0) ? MessageHud.MessageType.TopLeft : MessageHud.MessageType.Center);
            player.Message(type, "$msg_skillup $skill_" + skill.m_info.m_skill.ToString().ToLower() + ": " + (int)skill.m_level, 0, skill.m_info.m_icon);
            Gogan.LogEvent("Game", "Levelup", skill.m_info.m_skill.ToString(), (int)skill.m_level);
        }
    }
}
