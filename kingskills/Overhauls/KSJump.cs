using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace kingskills.Overhauls
{

    [HarmonyPatch(typeof(Character), "UpdateGroundContact")]
    class KSJump
    {
        static ConstructorInfo hitDataConstructor = AccessTools.Constructor(typeof(HitData));
        static MethodInfo resetGroundContactMTD = AccessTools.Method(typeof(Character), "ResetGroundContact");
        static MethodInfo fallDamageOverrideMTD = AccessTools.DeclaredMethod(typeof(FallPatch), nameof(FallPatch.FallDamageOverride));

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instruct)
        {
            var newInstructions = new List<CodeInstruction>(instruct);
            bool flaggedNop = false;

            for (int i = 0; i < newInstructions.Count(); i++)
            {
                var instruction = newInstructions[i];
                if (flaggedNop)
                {
                    if (newInstructions[i + 1].Calls(resetGroundContactMTD))
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

    class FallPatch : Character
    {
        public void FallDamageOverride()
        {
            float fallHeight = Mathf.Max(0f, m_maxAirAltitude - this.GetTransform().position.y);
            float skillFactor = GetSkillFactor(Skills.SkillType.Jump);

            float fallThreshold = 4;
            if (CFG.IsSkillActive(Skills.SkillType.Jump))
                fallThreshold = CFG.GetFallDamageThreshold(skillFactor);


            if (IsPlayer())
            {
                if (m_nview.m_zdo.GetBool("Valkyrie Dropped", false))
                {
                    m_nview.m_zdo.Set("Valkyrie Dropped", false);
                    return;
                }

                float expGained = 0f;
                if (fallHeight > 4f)
                {
                    expGained = (fallHeight - 4) * 8.33f;
                }

                if (fallHeight > fallThreshold)
                {
                    HitData hitData = new HitData();

                    float fallDamage = (fallHeight - fallThreshold) * 8.33f;

                    if (CFG.IsSkillActive(Skills.SkillType.Jump))
                        fallDamage *= CFG.GetFallDamageRedux(skillFactor);

                    hitData.m_damage.m_damage = fallDamage;
                    hitData.m_point = m_lastGroundPoint;
                    hitData.m_dir = m_lastGroundNormal;

                    Damage(hitData);

                }
                //Jotunn.Logger.LogMessage("Jump exp just increased by " + expGained + " thanks to fall damage");
                if (CFG.IsSkillActive(Skills.SkillType.Jump) && expGained > 0)
                {
                    expGained *= CFG.GetJumpXPMod();
                    RaiseSkill(Skills.SkillType.Jump, expGained);
                }
            }
        }
    }

    [HarmonyPatch(typeof(Valkyrie), nameof(Valkyrie.DropPlayer))]
    class IntroPatch 
    {

        [HarmonyPrefix]
        public static void ResetAltitude(Valkyrie __instance)
        {
            Player.m_localPlayer.m_nview.m_zdo.Set("Valkyrie Dropped", true);
        }
    }
}
