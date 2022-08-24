using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kingskills
{
    public class Util
    {

        // Get the actual weapon a player would swing with, or null if the player couldn't swing in this state.
        // Compared to Player.GetCurrentWeapon(), for example, this returns null if the player is holding
        // a hammer or pickaxe, but will return unarmed even if the player is holding a shield.
        public static ItemDrop.ItemData GetPlayerWeapon(Player p)
        {
            if (p.m_leftItem != null && p.m_leftItem.IsWeapon() && p.m_leftItem.m_shared.m_skillType != Skills.SkillType.Pickaxes)
            {
                return p.m_leftItem;
            }
            if (p.m_rightItem != null && p.m_rightItem.IsWeapon() && p.m_rightItem.m_shared.m_skillType != Skills.SkillType.Pickaxes)
            {
                return p.m_rightItem;
            }
            if (p.m_rightItem == null && (p.m_leftItem == null
                || p.m_leftItem.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Shield))
            {
                return p.m_unarmedWeapon.m_itemData;
            }
            return null;
        }
    }
}
