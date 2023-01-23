using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kingskills.Weapons
{
    [HarmonyPatch(typeof(Attack))]
    class WeaponStamina
    {
        [HarmonyPatch(nameof(Attack.GetAttackStamina))]
        [HarmonyPrefix]
        public static bool StaminaUsePatch(Attack __instance, ref float __result)
        {
            Skills.SkillType skillT = __instance.m_weapon.m_shared.m_skillType;

            float stamina = __instance.m_attackStamina;
            float skillFactor = __instance.m_character.GetSkillFactor(skillT);

            if (stamina <= 0f) __result = 0;

            stamina *= CFG.GetBerserkStaminaRedux()
                    * CFG.GetBigStickStaminaMult();

            /*
            Jotunn.Logger.LogMessage($"Going for stamina use - attack stamina is supposed to be " +
                $"{stamina}, but since the skill is {skillT}, ");
            */
            if (!CFG.IsSkillActive(skillT)) skillT = Skills.SkillType.None;
            switch (skillT)
            {
                case Skills.SkillType.None:
                    break;
                case Skills.SkillType.Swords:
                    stamina *=
                        CFG.GetSwordStaminaRedux(skillFactor);
                    break;
                case Skills.SkillType.Unarmed:
                    stamina *=
                        CFG.GetFistStaminaRedux(skillFactor);
                    break;
                case Skills.SkillType.Axes:
                    stamina *=
                        CFG.GetAxeStaminaRedux(skillFactor);
                    break;
                case Skills.SkillType.Bows:
                    stamina *=
                        CFG.GetBowStaminaRedux(skillFactor);
                    break;
                case Skills.SkillType.Clubs:
                    stamina *=
                        CFG.GetClubStaminaRedux(skillFactor);
                    break;
                case Skills.SkillType.Knives:
                    stamina *=
                        CFG.GetKnifeStaminaRedux(skillFactor);
                    break;
                case Skills.SkillType.Polearms:
                    stamina *=
                        CFG.GetPolearmStaminaRedux(skillFactor);
                    break;
            }

            //Jotunn.Logger.LogMessage($"It's only going to be {stamina }");

            __result = stamina;

            return CFG.SkipOriginal;
        }

        [HarmonyPatch(typeof(ItemDrop.ItemData))]
        [HarmonyPatch(nameof(ItemDrop.ItemData.GetDrawStaminaDrain))]
        [HarmonyPrefix]
        public static bool StaminaHoldPatch(ItemDrop.ItemData __instance, ref float __result)
        {

            Skills.SkillType skillT = __instance.m_shared.m_skillType;
            if (!CFG.IsSkillActive(skillT)) return CFG.DontSkipOriginal;

            float stamina = __instance.m_shared.m_foodStamina;
            float skillFactor = Player.m_localPlayer.GetSkillFactor(skillT);
            if (stamina <= 0f) __result = 0;

            //Jotunn.Logger.LogMessage($"Checking for bow drain - supposed to be {stamina }");

            if (skillT == Skills.SkillType.Bows)
                stamina *=
                    CFG.GetBowStaminaRedux(skillFactor);

            //Jotunn.Logger.LogMessage($"But we end up turning it into {stamina }"); 

            __result = stamina;
            
            return CFG.SkipOriginal;
        }
    }
}
