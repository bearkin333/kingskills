using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace kingskills.Patches
{
    [HarmonyPatch(typeof(Player))]
    class LevelUpPatches
    {
        
        [HarmonyPatch(nameof(Player.GetSkillFactor))]
        [HarmonyPrefix]
        public static bool GetRealSkillFactor(Player __instance, ref float __result, Skills.SkillType skill)
        {
            //Jotunn.Logger.LogMessage("Checking skill");
            if (skill == Skills.SkillType.None) __result = 0f;
            
            //Ascended skills will always act as though at max level
            if (Perks.IsSkillAscended(skill))
                __result = 1f;
            else
                __result = Mathf.Clamp01(__instance.GetSkills().GetSkillLevel(skill) / ConfigMan.MaxSkillLevel.Value);

            return false;
        }

        [HarmonyPatch(nameof(Player.OnSkillLevelup))]
        [HarmonyPostfix]
        public static void SkillLevelupPatch(Player __instance, Skills.SkillType skill)
        {
            StatsPatch.UpdateStats(__instance, true, skill);
        }

        [HarmonyPatch(nameof(Player.RaiseSkill))]
        [HarmonyPrefix]
        public static bool RaiseSkillPatch(Player __instance, Skills.SkillType skill, float value = 1f)
        {
            //Largely stolen from the game
            if (skill == Skills.SkillType.None)
            {
                return false;
            }

            Skills.Skill skillActual = __instance.GetSkills().GetSkill(skill);

            //Allow status effects to modify exp gain rate
            __instance.m_seman.ModifyRaiseSkill(skill, ref value);


            if (ConfigMan.IsSkillActive(Skills.SkillType.Run) && skill == Skills.SkillType.Run)
            {
                value *= MovePatch.absoluteWeightBonus(__instance);
                value *= MovePatch.relativeWeightBonus(__instance);
                value *= MovePatch.runSpeedExpBonus(__instance);
            }
            else if (ConfigMan.IsSkillActive(Skills.SkillType.Swim) && skill == Skills.SkillType.Swim)
            {
                value *= MovePatch.absoluteWeightBonus(__instance);
                value *= MovePatch.relativeWeightBonus(__instance);
                value *= MovePatch.swimSpeedExpBonus(__instance);
            }


            if (SkillRaise(skillActual, __instance, value))
                if (__instance.GetSkills().m_useSkillCap)
                    __instance.GetSkills().RebalanceSkills(skill);

            return false;
        }


        public static bool SkillRaise(Skills.Skill skill, Player player, float factor)
        {
            if (skill.m_level >= ConfigMan.MaxSkillLevel.Value)
                return false;

            float num = skill.m_info.m_increseStep * factor;
            skill.m_accumulator += num;
            Skills.SkillType skillT = skill.m_info.m_skill;
            float percent = skill.m_accumulator / (skill.GetNextLevelRequirement() / ConfigMan.MaxSkillLevel.Value);

            if (factor >= ConfigMan.DisplayExperienceThreshold.Value)
            {
                string expMsg = "+" + factor.ToString("F1") + " experience - Level " + 
                    skill.m_level.ToString("F0") + " " + skillT
                    + " (" + percent.ToString("F0") + "%)";

                CustomWorldTextManager.AddCustomWorldText(ConfigMan.ColorExperienceYellow, 
                    CustomWorldTextManager.GetAboveCharacter(player) + CustomWorldTextManager.GetRandomPosOffset(), 
                    22, expMsg);
                /*
                player.Message(MessageHud.MessageType.TopLeft, expMsg, 0, skill.m_info.m_icon);
                */
            }

            float nextLevelRequirement = skill.GetNextLevelRequirement();
            while (skill.m_accumulator >= nextLevelRequirement)
            {
                skill.m_level += 1f;
                skill.m_level = Mathf.Clamp(skill.m_level, 0f, ConfigMan.MaxSkillLevel.Value);
                LevelUpPing(player, skill);

                skill.m_accumulator -= nextLevelRequirement;
                if (skill.m_level >= ConfigMan.MaxSkillLevel.Value)
                {
                    OnMaxLevel(skillT);
                    return false;
                }

                nextLevelRequirement = skill.GetNextLevelRequirement();
            }

            return false;
        }

        public static void OnMaxLevel(Skills.SkillType skill)
        {
            Perks.skillAscendedFlags[skill] = true;
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
