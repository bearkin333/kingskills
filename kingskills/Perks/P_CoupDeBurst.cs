using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace kingskills.Perks
{
    class P_CoupDeBurst
    {
        public static void JumpCheck(string hotkey)
        {
            if (!ZInput.GetButtonDown(hotkey)) return;
            if (!CFG.CheckPlayerActivePerk(Player.m_localPlayer, PerkMan.PerkType.CoupDeBurst)) return;
            if (Player.m_localPlayer.GetControlledShip() is null) return;

            Player player = Player.m_localPlayer;
            Ship ship = player.GetControlledShip();
            IDestructible shipD = ship.GetComponent<IDestructible>();

            if (shipD is null) return;

            HitData push = new HitData();
            push.m_pushForce = CFG.CoupDeBurstForce.Value;
            push.m_dir = Vector3.Lerp(ship.transform.forward, Vector3.up, CFG.CoupDeBurstAngle.Value);
            push.m_point = ship.transform.position;

            foreach (Player players in ship.m_players)
            {
                players.AddNoise(CFG.CoupDeBurstNoise.Value);
            }

            shipD.Damage(push);
        }
    }
}
