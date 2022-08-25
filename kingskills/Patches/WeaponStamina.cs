using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kingskills.Patches
{
    [HarmonyPatch(typeof(Attack))]
    class WeaponStamina
    {
        [HarmonyPatch(nameof(Attack.GetAttackStamina))]
        [HarmonyPrefix]
        public static bool StaminaUsePatch(Attack __instance, ref float __result)
        {
            float stamina = __instance.m_attackStamina;
            Skills.SkillType skillT = __instance.m_weapon.m_shared.m_skillType;
            float skillFactor = __instance.m_character.GetSkillFactor(skillT);
            if (stamina <= 0f) __result = 0;

            switch (skillT)
            {
                case Skills.SkillType.Swords:
                    stamina *=
                        ConfigManager.GetSwordStaminaRedux(skillFactor);
                    break;
                case Skills.SkillType.Unarmed:
                    stamina *=
                        ConfigManager.GetFistStaminaRedux(skillFactor);
                    break;
                case Skills.SkillType.Axes:
                    stamina *=
                        ConfigManager.GetAxeStaminaRedux(skillFactor);
                    break;
                case Skills.SkillType.Bows:
                    stamina *=
                        ConfigManager.GetBowStaminaRedux(skillFactor);
                    break;
                case Skills.SkillType.Clubs:
                    stamina *=
                        ConfigManager.GetClubStaminaRedux(skillFactor);
                    break;
                case Skills.SkillType.Knives:
                    stamina *=
                        ConfigManager.GetKnifeStaminaRedux(skillFactor);
                    break;
                case Skills.SkillType.Polearms:
                    stamina *=
                        ConfigManager.GetPolearmStaminaRedux(skillFactor);
                    break;
            }

            __result = stamina;

            return false;
        }

    }
}
