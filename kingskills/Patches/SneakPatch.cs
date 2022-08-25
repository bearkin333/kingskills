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
    class SneakPatch
	{
		[HarmonyPatch(nameof(Player.OnSneaking))]
		[HarmonyPrefix]
		public static bool OnSneakingTakeover(Player __instance, float dt)
        {
			//This code is pretty much ripped straight from valheim
			//since I have to replace or transpile it to do most of what I want to do

			float staminaUse = ConfigManager.GetSneakStaminaDrain(__instance.GetSkillFactor(Skills.SkillType.Sneak);
			__instance.UseStamina(dt * staminaUse);
			if (!__instance.HaveStamina())
			{
				Hud.instance.StaminaBarNoStaminaFlash();
			}
			__instance.m_sneakSkillImproveTimer += dt;
			if (__instance.m_sneakSkillImproveTimer > 1f)
			{
				__instance.m_sneakSkillImproveTimer = 0f;
				if (BaseAI.InStealthRange(__instance))
				{
					float sneakExpMod = GetStrongestEnemyInSight(__instance).GetLevel() *
						ConfigManager.GetSneakEXPPerDangerMod();

					__instance.RaiseSkill(Skills.SkillType.Sneak, sneakExpMod);
				}
				else
				{
					__instance.RaiseSkill(Skills.SkillType.Sneak, 0.1f);
				}
			}

			return false;
		}

		public static Character GetStrongestEnemyInSight(Player player, float range=0f)
        {
			List<BaseAI> inRangeEnemies = new List<BaseAI>();
			Character strongest = null;

			foreach (BaseAI allInstance in BaseAI.GetAllInstances())
            {
				float distance = Vector3.Distance(((Component)allInstance).transform.position, ((Component)player).transform.position);
				if (distance < allInstance.m_viewRange)
                {
					if (strongest == null)
                    {
						strongest = allInstance.m_character;
                    }
                    else
                    {
						if (allInstance.m_character.GetLevel() > strongest.GetLevel())
                        {
							strongest = allInstance.m_character;
						}
                    }
                }
            }
			return strongest;
        }
    }
}
