using kingskills.UX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kingskills.Perks
{
    class P_WeaponEnchants
    {
        public static WeaponEnchant activeEnchant = WeaponEnchant.None;

        public static void ApplyWeaponEnchant(ref HitData.DamageTypes damage)
        {
            float extraDamage = damage.GetTotalDamage() * CFG.GetPerkWeaponEnchantMod();
            switch (activeEnchant)
            {
                case WeaponEnchant.Fire:
                    damage.m_fire += extraDamage;
                    break;
                case WeaponEnchant.Lightning:
                    damage.m_lightning += extraDamage;
                    break;
                case WeaponEnchant.Spirit:
                    damage.m_spirit += extraDamage;
                    break;
                case WeaponEnchant.Poison:
                    damage.m_poison += extraDamage;
                    break;
                case WeaponEnchant.Frost:
                    damage.m_frost += extraDamage;
                    break;
                case WeaponEnchant.None:
                    break;
            }
        }

    }

    public enum WeaponEnchant 
    { 
        Fire, //Cauterize
        Lightning, //Mjolnir
        Spirit, //Spearit
        Poison, //Toxic
        Frost, //Ymir
        None
    }

}
