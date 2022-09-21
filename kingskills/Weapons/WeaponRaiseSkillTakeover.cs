using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using kingskills.Patches;

namespace kingskills.Weapons
{
    [HarmonyPatch]
    class WeaponRaiseSkillTakeover
    {
        public static bool aoeIgnore = false;
        public static bool areaIgnore = false;
        public static bool meleeIgnore = false;
        public static bool ksOverride = false;

        [HarmonyPatch(typeof(Aoe), nameof(Aoe.OnHit))] [HarmonyPrefix]
        public static void AoeOnHitIgnore() => aoeIgnore = true;
        [HarmonyPatch(typeof(Aoe), nameof(Aoe.OnHit))] [HarmonyFinalizer]
        public static void AoeCleanup() => aoeIgnore = false;


        [HarmonyPatch(typeof(Attack), nameof(Attack.DoAreaAttack))] [HarmonyPrefix]
        public static void AreaDoIgnore() => areaIgnore = true;
        [HarmonyPatch(typeof(Attack), nameof(Attack.DoAreaAttack))] [HarmonyFinalizer]
        public static void AreaCleanup() => areaIgnore = false;

        [HarmonyPatch(typeof(Attack), nameof(Attack.DoMeleeAttack))] [HarmonyPrefix]
        public static void MeleeDoIgnore() => meleeIgnore = true;
        [HarmonyPatch(typeof(Attack), nameof(Attack.DoMeleeAttack))] [HarmonyFinalizer]
        public static void MeleeCleanup() => meleeIgnore = false;

        public static void DoNotIgnore() =>
            ksOverride = true;


        [HarmonyPatch(typeof(Projectile),nameof(Projectile.OnHit))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> ProjectileRaiseSkillReplacer(IEnumerable<CodeInstruction> instructions)
        {
            //Jotunn.Logger.LogMessage("The call is replaced here");
            bool patchDone = false;

            foreach (var instruction in instructions)
            {
                if (instruction.Calls(AccessTools.Method(typeof(Character), "RaiseSkill")))
                {
                    instruction.opcode = OpCodes.Call;
                    instruction.operand = AccessTools.DeclaredMethod(typeof(ProjectilePatch), nameof(ProjectilePatch.RaiseProjectileSkill));
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return instruction;

                    patchDone = true;
                }
                else
                    yield return instruction;
            }

            if (!patchDone)
            {
                //Jotunn.Logger.LogError("Didn't find the Raise skill during projectile transpile");
            }
        }
    }

    class WeaponSkillReroute
    {
        public static void NewAoeOnHit(Skills.SkillType s, float f)
        {

        }

        public static void NewAreaAttack(Skills.SkillType s, float f)
        {

        }

        public static void NewMeleeAttack(Skills.SkillType s, float f)
        {

        }
    }
}
