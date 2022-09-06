using BepInEx;
using HarmonyLib;
using Jotunn.Entities;
using Jotunn.Managers;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace kingskills
{
    [HarmonyPatch(typeof(Player))]
    class MovePatch
    {
        [HarmonyPatch(nameof(Player.GetRunSpeedFactor))]
        [HarmonyPrefix]
        public static bool GetRunSpeedPatch(Player __instance, ref float __result)
        {
            if (!ConfigMan.IsSkillActive(Skills.SkillType.Run))
            {
                return true;
            }

            //This function provides a number to multiply the base run speed by
            float runSkillFactor = ConfigMan.GetRunSpeedMult(__instance.m_skills.GetSkillFactor(Skills.SkillType.Run));

            float runSpeed = runSkillFactor * GetGenericMovespeedAdjustments(__instance);
            __result = runSpeed;
            /*
            Jotunn.Logger.LogMessage($"Skill factor was {skillFactor},\n" +
                $"runSkill factor was {runSkillFactor},\n" +
                $"equipment malus redux was {equipmentMalusRedux},\n" +
                $"equipment factor was {equipmentFactor},\n" +
                $"encumberance percent was {encumberancePercent},\n" +
                $"encumberance percent curved was {encumberancePercentCurved},\n" +
                $"skill encumberance redux was {skillEncumberanceRedux},\n" +
                $"encumberance factor was {encumberanceFactor},\n" +
                $"total run speed was was {runSpeed},\n");*/

            //Returning false skips the original implementation of GetRunSpeedFactor
            return false;
        }

        [HarmonyPatch(nameof(Player.GetJogSpeedFactor))]
        [HarmonyPrefix]
        public static bool GetJogSpeedPatch(Player __instance, ref float __result)
        {
            __result = GetGenericMovespeedAdjustments(__instance);
            return false;
        }

        [HarmonyPatch(nameof(Player.UpdateMovementModifier))]
        [HarmonyPrefix]
        public static bool UpdateEquipmentModOverride(Player __instance)
        {
            //Another override of the original with mostly copied code.
            if (!ConfigMan.IsSkillActive(Skills.SkillType.Run)) return true;

            float equipmentMalusRedux = ConfigMan.GetEquipmentRedux(__instance.GetSkillFactor(Skills.SkillType.Run));
            float m;

            __instance.m_equipmentMovementModifier = 0;
            if (__instance.m_rightItem != null)
            {
                m = __instance.m_rightItem.m_shared.m_movementModifier;
                if (m < 0) m *= equipmentMalusRedux;
                __instance.m_equipmentMovementModifier += m;
            }
            if (__instance.m_leftItem != null)
            {
                m = __instance.m_leftItem.m_shared.m_movementModifier;
                if (m < 0) m *= equipmentMalusRedux;
                __instance.m_equipmentMovementModifier += m;
            }
            if (__instance.m_chestItem != null)
            {
                m = __instance.m_chestItem.m_shared.m_movementModifier;
                if (m < 0) m *= equipmentMalusRedux;
                __instance.m_equipmentMovementModifier += m;
            }
            if (__instance.m_legItem != null)
            {
                m = __instance.m_legItem.m_shared.m_movementModifier;
                if (m < 0) m *= equipmentMalusRedux;
                __instance.m_equipmentMovementModifier += m;
            }
            if (__instance.m_helmetItem != null)
            {
                m = __instance.m_helmetItem.m_shared.m_movementModifier;
                if (m < 0) m *= equipmentMalusRedux;
                __instance.m_equipmentMovementModifier += m;
            }
            if (__instance.m_shoulderItem != null)
            {
                m = __instance.m_shoulderItem.m_shared.m_movementModifier;
                if (m < 0) m *= equipmentMalusRedux;
                __instance.m_equipmentMovementModifier += m;
            }
            if (__instance.m_utilityItem != null)
            {
                m = __instance.m_utilityItem.m_shared.m_movementModifier;
                if (m < 0) m *= equipmentMalusRedux;
                __instance.m_equipmentMovementModifier += m;
            }

            return false;
        }

        public static float GetGenericMovespeedAdjustments(Player player){
            float mod = 1f;
            mod *= GetSkillMovespeedMod(player) 
                * GetEncumberanceRedux(player) 
                * GetEquipmentMult(player);
            return mod;
        }

        public static float GetSkillMovespeedMod(Player player)
        {
            float mod = 1f;

            if (ConfigMan.IsSkillActive(Skills.SkillType.Unarmed))
                mod += ConfigMan.GetFistMovespeedMod(player.GetSkillFactor(Skills.SkillType.Unarmed));

            if (ConfigMan.IsSkillActive(Skills.SkillType.Run))
                mod += ConfigMan.GetKnifeMovespeedMod(player.GetSkillFactor(Skills.SkillType.Knives));

            return mod;
        }

        // 
        // Experience determining functions
        //
        public static float GetEncumberanceRedux(Player player)
        {
            if (!ConfigMan.IsSkillActive(Skills.SkillType.Run)) return 1f;

            float skillFactor = player.GetSkillFactor(Skills.SkillType.Run);
            float encumberanceMod = Mathf.Clamp01(player.GetInventory().GetTotalWeight() / player.GetMaxCarryWeight());
            float encumberanceCurved = ConfigMan.GetEncumberanceCurveRedux(encumberanceMod);
            float skillEncumberanceRedux = ConfigMan.GetEncumberanceRedux(skillFactor);
            return encumberanceCurved * skillEncumberanceRedux;
        }
        public static float GetEquipmentMult(Player player)
        {
            float equipmentFactor = player.m_equipmentMovementModifier;

            return equipmentFactor + 1f;
        }
        public static float absoluteWeightBonus(Player player)
        {
            float weightPercent = ConfigMan.GetAbsoluteWeightMod(player.m_inventory.GetTotalWeight());
            return ConfigMan.GetAbsoluteWeightCurveMult(weightPercent);
        }
        public static float relativeWeightBonus(Player player)
        {
            float weightPercent = player.GetInventory().GetTotalWeight() / player.GetMaxCarryWeight();
            return ConfigMan.GetRelativeWeightStageMult(weightPercent);
        }
        public static float runSpeedExpBonus(Player player)
        {
            float runMod = player.GetRunSpeedFactor();
            player.m_seman.ApplyStatusEffectSpeedMods(ref runMod);

            return runMod * ConfigMan.GetRunEXPSpeedMult();
        }
        public static float swimSpeedExpBonus(Player player)
        {
            float swimMod = player.m_swimSpeed;
            player.m_seman.ApplyStatusEffectSpeedMods(ref swimMod);

            return swimMod * ConfigMan.GetSwimXPSpeedMult();
        }
    }
       
    
}
