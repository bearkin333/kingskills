﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace kingskills
{
    class JumpChanges : Character
    {
        public const float FallDamageThresholdMax = 12f;
        public const float FallDamageThresholdMin = 4f;
        public const float FallDamageReduxMax = .6f;
        public const float FallDamageReduxMin = -.15f;

        public void FallDamageOverride()
        {
            float fallHeight = Mathf.Max(0f, m_maxAirAltitude - this.GetTransform().position.y);
            float skillFactor = GetSkillFactor(Skills.SkillType.Jump);
            float fallThreshold = Mathf.Lerp(FallDamageThresholdMin,FallDamageThresholdMax, skillFactor);
            if (IsPlayer() && fallHeight > fallThreshold)
            {
                HitData hitData = new HitData();
                float fallDamage = (fallHeight - fallThreshold) * 8.33f;
                fallDamage *= (1 - Mathf.Lerp(FallDamageReduxMin, FallDamageReduxMax, skillFactor));
                hitData.m_damage.m_damage = fallDamage;
                hitData.m_point = m_lastGroundPoint;
                hitData.m_dir = m_lastGroundNormal;
                Damage(hitData);
                RaiseSkill(Skills.SkillType.Jump, hitData.GetTotalDamage());
                //Jotunn.Logger.LogMessage("Jump exp just increased by " + hitData.GetTotalDamage() + " thanks to fall damage");
            }
        }
    }

    [HarmonyPatch(typeof(Character), "UpdateGroundContact")]
        class JumpExp
    {
        static ConstructorInfo hitDataConstructor = AccessTools.Constructor(typeof(HitData));
        static MethodInfo resetGroundContactMTD = AccessTools.Method(typeof(Character), "ResetGroundContact");
        static MethodInfo fallDamageOverrideMTD = AccessTools.DeclaredMethod(typeof(JumpChanges), nameof(JumpChanges.FallDamageOverride));
        
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instruct)
        {
            var newInstructions = new List<CodeInstruction>(instruct);
            bool flaggedNop = false;
            for (int i = 0; i < newInstructions.Count(); i++)
            {
                var instruction = newInstructions[i];
                if (flaggedNop)
                {
                    if (newInstructions[i+1].Calls(resetGroundContactMTD))
                    {
                        newInstructions.Insert(i + 1, new CodeInstruction(OpCodes.Call, fallDamageOverrideMTD));
                        newInstructions.Insert(i + 2, new CodeInstruction(OpCodes.Ldarg_0));
                        break;
                    }
                    newInstructions[i].opcode = OpCodes.Nop;
                    newInstructions[i].operand = null;
                } 
                else if (instruction.opcode == OpCodes.Newobj && Equals(instruction.operand, hitDataConstructor))
                {
                    newInstructions[i].opcode = OpCodes.Nop;
                    newInstructions[i].operand = null;
                    flaggedNop = true;
                }
            }
            return newInstructions.AsEnumerable();
        }
    }
}
