using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jotunn.Entities;
using Jotunn.Managers;
using kingskills.Perks;
using UnityEngine;

namespace kingskills.SE
{
    class KS_SEMan
    {
        public static StatusEffect ks_Berserk;
        public const string ks_BerserkName = "KS_SE_Berserk";

        public static StatusEffect ks_CouchedLance;
        public const string ks_CouchedLanceName = "KS_SE_CouchedLance";

        public static StatusEffect ks_CoupDeBurst;
        public const string ks_CoupDeBurstName = "KS_SE_CoupDeBurst";

        public static void InitSE()
        {
            ks_Berserk = ScriptableObject.CreateInstance<SE_Berserk>();
            ks_CouchedLance = ScriptableObject.CreateInstance<SE_CouchedLance>();
            ks_CoupDeBurst = ScriptableObject.CreateInstance<SE_CoupDeBurst>();






            ItemManager.Instance.AddStatusEffect(new CustomStatusEffect(ks_Berserk, fixReference: false));
            ItemManager.Instance.AddStatusEffect(new CustomStatusEffect(ks_CouchedLance, fixReference: false));
            ItemManager.Instance.AddStatusEffect(new CustomStatusEffect(ks_CoupDeBurst, fixReference: false));
        }
    }
}
