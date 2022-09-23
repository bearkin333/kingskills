using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kingskills.SE
{
    class SE_Stacking : SE_Stats
    {
        public static float m_baseTTL;
        public static int maxStacks;
        public int stacks;

        public SE_Stacking()
        {
            stacks = 0;
        }

        public void AddStack(int addStacks = 1)
        {
            stacks += addStacks;
            if (stacks > maxStacks) stacks = maxStacks;

            ResetTime();
        }

        public override string GetIconText()
        {
            return stacks.ToString("F0") + "\n" + base.GetIconText();
        }
    }
}
