using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kingskills.UX
{
    //Pretty Text. I shortened it since it gets referenced all over the place and I wanted
    //things to be more readable
    class PT
    {

        public static string Prettify(float input, float decimals, TType type)
        {
            string color = "";
            string extraColorText = "";
            string extraCleanText = "";

            switch (type)
            {
                case TType.TextlessPercent:
                    if (input < 0)
                    {
                        color += CFG.ColorPTRedFF;
                        extraColorText = "%";
                    }
                    else if (input > 0)
                    {
                        color += CFG.ColorPTGreenFF;
                        extraColorText = "%";
                    }
                    else
                    {
                        color += CFG.ColorPTWhiteFF;
                        extraColorText = "%";
                    }
                    break;
                case TType.Percent:
                    if (input < 0)
                    {
                        color += CFG.ColorPTRedFF;
                        extraColorText = "% less";
                    }
                    else if (input > 0)
                    {
                        color += CFG.ColorPTGreenFF + "+";
                        extraColorText = "% extra";
                    }
                    else
                    {
                        color += CFG.ColorPTWhiteFF;
                        extraColorText = "% extra";
                    }
                    break;
                case TType.Flat:
                    if (input < 0)
                    {
                        color += CFG.ColorPTRedFF;
                    }
                    else if (input > 0)
                    {
                        color += CFG.ColorPTGreenFF + "+";
                    }
                    else
                    {
                        color += CFG.ColorPTWhiteFF;
                    }
                    break;
                case TType.PercentRedux:
                    if (input < 0)
                    {
                        input *= -1f;
                        color += CFG.ColorPTRedFF + "+";
                        extraColorText = "% increased";
                    }
                    else if (input > 0)
                    {
                        color += CFG.ColorPTGreenFF + "-";
                        extraColorText = "% reduced";
                    }
                    else
                    {
                        color += CFG.ColorPTWhiteFF;
                        extraColorText = "% reduced";
                    }
                    break;
                case TType.Straight:
                    color += CFG.ColorPTWhiteFF;
                    break;
                case TType.ColorlessPercent:
                    color = "";
                    if (input < 0)
                    {
                        extraColorText = "%";
                    }
                    else if (input > 0)
                    {
                        color += "+";
                        extraColorText = "%";
                    }
                    else
                    {
                        extraColorText = "%";
                    }
                    return color + input.ToString("F" + decimals) + extraColorText;

            }

            string inputText = input.ToString("F" + decimals);

            string result = color + inputText + extraColorText + CFG.ColorEnd + extraCleanText;

            return result;
        }

        public enum TType
        {
            TextlessPercent,
            Percent,
            PercentRedux,
            Flat,
            Straight,
            ColorlessPercent
        }

        public static float MultToPer(float number, bool redux = false)
        {
            if (redux) return (1 - number) * 100;
            return (number - 1) * 100;
        }
    }
}
