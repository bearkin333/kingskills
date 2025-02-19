﻿using HarmonyLib;
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
			if (!CFG.IsSkillActive(Skills.SkillType.Sneak)) return CFG.DontSkipOriginal;

			float staminaUse = CFG.GetSneakStaminaDrain(__instance.GetSkillFactor(Skills.SkillType.Sneak));
			if (PerkMan.IsPerkActive(PerkMan.PerkType.ESP))
            {
				staminaUse *= CFG.GetESPStaminaMult();
            }

			__instance.UseStamina(dt * staminaUse);
			P_CloakOfShadows.UpdateOnSneak(dt);

			if (!__instance.HaveStamina())
			{
				Hud.instance.StaminaBarEmptyFlash();
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

			return CFG.SkipOriginal;
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


		[HarmonyPatch(nameof(Player.UpdateStealth))]
		[HarmonyPrefix]
		public static bool UpdateStealthTakeover(Player __instance, float dt)
		{
			//Again, this function is entirely taken over.
			//It pretty much seems like the only way.
			//Aside from a messy transpile that I won't be able to pull off myself.
			if (!CFG.IsSkillActive(Skills.SkillType.Sneak)) return CFG.DontSkipOriginal;

			Player p = __instance;

			p.m_stealthFactorUpdateTimer += dt;
			if (p.m_stealthFactorUpdateTimer > 0.5f)
			{
				p.m_stealthFactorUpdateTimer = 0f;
				p.m_stealthFactorTarget = 0f;
				if (p.IsCrouching())
				{
					p.m_lastStealthPosition = p.transform.position;
					float skillFactor = p.GetSkillFactor(Skills.SkillType.Sneak);
					float lightFactor = StealthSystem.instance.GetLightFactor(p.GetCenterPoint());
					//Jotunn.Logger.LogMessage($"Your light factor is currently {lightFactor}");
					p.m_stealthFactorTarget = CFG.GetSneakFactor(skillFactor, lightFactor);
					p.m_stealthFactorTarget = Mathf.Clamp01(p.m_stealthFactorTarget);
					p.m_seman.ModifyStealth(p.m_stealthFactorTarget, ref p.m_stealthFactorTarget);
					p.m_stealthFactorTarget = Mathf.Clamp01(p.m_stealthFactorTarget);
				}
				else
				{
					p.m_stealthFactorTarget = 1f;
				}
			}
			p.m_stealthFactor = Mathf.MoveTowards(p.m_stealthFactor, p.m_stealthFactorTarget, dt / 4f);
			p.m_nview.m_zdo.Set("Stealth", p.m_stealthFactor);

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
