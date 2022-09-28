using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kingskills.Perks
{
    class P_CloakOfShadows
    {
        public static float timer = 0f;

        public static void UpdateOnSneak(float dt)
        {
            if (!CFG.CheckPlayerActivePerk(Player.m_localPlayer, PerkMan.PerkType.CloakOfShadows)) return;
            Player player = Player.m_localPlayer;

            timer += dt;
            if (timer < CFG.CloakOfShadowsTimer.Value) return;

            timer = 0f;
            player.Heal(player.GetMaxHealth()* CFG.GetCloakOfShadowsHealthRegenMod());
        }
    }
}
