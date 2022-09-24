using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace kingskills.Perks
{
    [HarmonyPatch]
    class P_BlockExpert
    {
        [HarmonyPatch(typeof(Attack), nameof(Attack.DoAreaAttack))]
        [HarmonyPrefix]
    }
}
