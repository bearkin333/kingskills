using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jotunn.Entities;
using Jotunn.Managers;
using kingskills.Perks;

namespace kingskills.SE
{
    class KS_SEMan
    {
        public static StatusEffect ks_Berserk;
        public const string ks_BerserkName = "KS_SE_Berserk";

        public static void InitSE()
        {
            ks_Berserk = P_Berserkr.GetEffectData();







            ItemManager.Instance.AddStatusEffect(new CustomStatusEffect(ks_Berserk, fixReference: false));
        }
    }
}
