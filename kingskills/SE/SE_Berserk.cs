using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kingskills.Perks;

namespace kingskills.SE
{
    class SE_Berserk : SE_Stacking
    {
        public SE_Berserk()
        {
            name = KS_SEMan.ks_BerserkName;
            m_name = "Berserk!";
            m_icon = PerkMan.perkList[PerkMan.PerkType.Berserkr].icon;
            m_startMessageType = MessageHud.MessageType.Center;
            m_startMessage = "You're pissed off!";
            m_stopMessageType = MessageHud.MessageType.Center;
            m_stopMessage = "You've calmed down.";
            m_tooltip = "You are raging, decreasing your damage taken and stamina costs, and increasing your movespeed.";
            m_baseTTL = CFG.GetBerserkDuration();
            m_ttl = m_baseTTL;
            maxStacks = CFG.BerserkrMaxStacks.Value;
        }
    }
}
