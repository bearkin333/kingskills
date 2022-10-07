using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kingskills.Perks
{
    class P_Engineer
    {
        public static bool IsPillar(WearNTear wnt)
        {
            Piece piece = wnt.m_piece;
            if (piece is null) return false;

            bool isPillar = false;
            string name = piece.m_name;
            //Jotunn.Logger.LogMessage($"checking {name} for pillar status");

            foreach (String acceptedPillar in CFG.EngineerPillarTable)
            {
                if (name.Contains(acceptedPillar))
                {
                    //Jotunn.Logger.LogMessage($"yup thats a pillar!");
                    isPillar = true;
                }
            }

            return isPillar;
        }
    }
}
