using HarmonyLib;
using kingskills.Perks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace kingskills.Patches
{
    [HarmonyPatch(typeof(Player))]
    public static class KSSneak
	{
		[HarmonyPatch(nameof(Player.OnSneaking))]
		[HarmonyPrefix]
		public static bool OnSneakingTakeover(Player __instance, float dt)
        {
			//This code is pretty much ripped straight from valheim
			//since I have to replace or transpile it to do most of what I want to do
			if (!CFG.IsSkillActive(Skills.SkillType.Sneak)) return true;

			float staminaUse = CFG.GetSneakStaminaDrain(__instance.GetSkillFactor(Skills.SkillType.Sneak));

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
					float sneakExpMod = GetDangerEXPMult(__instance);

					__instance.RaiseSkill(Skills.SkillType.Sneak, sneakExpMod);
				}
				else
				{
					__instance.RaiseSkill(Skills.SkillType.Sneak, 0.1f);
				}
			}

			return false;
		}

		public static float GetDangerEXPMult(Player player)
        {
			float mult = 1;
			Character strongest = GetStrongestEnemyInSight(player);
			if (strongest != null)
            {
				mult = GetStrongestEnemyInSight(player).m_health *
					CFG.GetSneakXPMult();
			}

			return mult;
		}


		[HarmonyPatch(nameof(Player.OnSneaking))]
		[HarmonyPrefix]
		public static bool UpdateStealthTakeover(Player __instance, float dt)
		{
			//Again, this function is entirely taken over.
			//It pretty much seems like the only way.
			//Aside from a messy transpile that I won't be able to pull off myself.
			if (!CFG.IsSkillActive(Skills.SkillType.Sneak)) return true;

			P_CloakOfShadows.UpdateOnSneak(dt);

			__instance.m_stealthFactorUpdateTimer += dt;
			if (__instance.m_stealthFactorUpdateTimer > 0.5f)
			{
				__instance.m_stealthFactorUpdateTimer = 0f;
				__instance.m_stealthFactorTarget = 0f;
				if (__instance.IsCrouching())
				{
					__instance.m_lastStealthPosition = __instance.transform.position;
					float skillFactor = __instance.GetSkillFactor(Skills.SkillType.Sneak);
					float lightFactor = StealthSystem.instance.GetLightFactor(__instance.GetCenterPoint());
					//Jotunn.Logger.LogMessage($"Your light factor is currently {lightFactor}");
					__instance.m_stealthFactorTarget = CFG.GetSneakFactor(skillFactor, lightFactor);
					__instance.m_stealthFactorTarget = Mathf.Clamp01(__instance.m_stealthFactorTarget);
					__instance.m_seman.ModifyStealth(__instance.m_stealthFactorTarget, ref __instance.m_stealthFactorTarget);
					__instance.m_stealthFactorTarget = Mathf.Clamp01(__instance.m_stealthFactorTarget);
				}
				else
				{
					__instance.m_stealthFactorTarget = 1f;
				}
			}
			__instance.m_stealthFactor = Mathf.MoveTowards(__instance.m_stealthFactor, __instance.m_stealthFactorTarget, dt / 4f);
			__instance.m_nview.GetZDO().Set("Stealth", __instance.m_stealthFactor);

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
						if (allInstance.m_character.m_health > strongest.m_health)
                        {
							strongest = allInstance.m_character;
						}
                    }
                }
            }
			if (strongest != null)
			{
				//Jotunn.Logger.LogMessage($"Strongest enemy in sight was {strongest.m_name}");
			}
			return strongest;
        }
    }
}
