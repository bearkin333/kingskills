using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kingskills.UX
{
    class PT
    {
        public static string Prettify(float input, float decimals, TType type)
        {
            string color = "<color=";
            string endColor = "</color>";
            string extraColorText = "";
            string extraCleanText = "";

            switch (type)
            {
                case TType.TextlessPercent:
                    if (input < 0)
                    {
                        color += "red>";
                        extraColorText = "%";
                    }
                    else if (input > 0)
                    {
                        color += "green>";
                        extraColorText = "%";
                    }
                    else
                    {
                        color += "white>";
                        extraColorText = "%";
                    }
                    break;
                case TType.Percent:
                    if (input < 0)
                    {
                        color += "red>";
                        extraColorText = "% less";
                    }
                    else if (input > 0)
                    {
                        color += "green>+";
                        extraColorText = "% extra";
                    }
                    else
                    {
                        color += "white>";
                        extraColorText = "% extra";
                    }
                    break;
                case TType.Flat:
                    if (input < 0)
                    {
                        color += "red>";
                    }
                    else if (input > 0)
                    {
                        color += "green>+";
                    }
                    else
                    {
                        color += "white>";
                    }
                    break;
                case TType.PercentRedux:
                    if (input < 0)
                    {
                        input *= -1f;
                        color += "red>+";
                        extraColorText = "% increased";
                    }
                    else if (input > 0)
                    {
                        color += "green>-";
                        extraColorText = "% reduced";
                    }
                    else
                    {
                        color += "white>";
                        extraColorText = "% reduced";
                    }
                    break;
                case TType.Straight:
                    color += "white>";
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

            string result = color + inputText + extraColorText + endColor + extraCleanText;

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
    }
}
