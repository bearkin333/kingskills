using kingskills.Perks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kingskills.SE
{
    class SE_CouchedLance : StatusEffect
    {
        public SE_CouchedLance()
        {
            name = KS_SEMan.ks_CouchedLanceName;
            m_name = "Couched Lance";
            m_icon = PerkMan.perkList[PerkMan.PerkType.CouchedLance].icon;
            m_startMessageType = MessageHud.MessageType.Center;
            m_startMessage = "Lance has been set!";
            m_stopMessageType = MessageHud.MessageType.Center;
            m_stopMessage = "No longer braced.";
            m_tooltip = "You are braced on the ground, increasing your damage.";
            m_ttl = CFG.CouchedLanceDuration.Value;
        }
    }
}
