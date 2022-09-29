using kingskills.Perks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kingskills.SE
{
    class SE_CoupDeBurst : StatusEffect
    {
        public SE_CoupDeBurst()
        {
            name = KS_SEMan.ks_CoupDeBurstName;
            m_name = "Coup De Burst cooldown";
            m_icon = PerkMan.perkList[PerkMan.PerkType.CoupDeBurst].icon;
            m_startMessageType = MessageHud.MessageType.Center;
            m_startMessage = "Launched!";
            m_stopMessageType = MessageHud.MessageType.Center;
            m_stopMessage = "Coup De Burst available";
            m_tooltip = "You're out of cola. You'll have to wait before using Coup De Burst again.";
            m_ttl = CFG.CoupDeBurstCooldown.Value;
            m_cooldownIcon = true;
        }
    }
}
