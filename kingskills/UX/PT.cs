using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kingskills.UX
{
    class PT
    {
        public const string greenCode = "#7AF365FF>";
        public const string redCode = "#EA0C0CFF>";
        public const string whiteCode = "#FEFDF5FF>";

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
                        color += redCode;
                        extraColorText = "%";
                    }
                    else if (input > 0)
                    {
                        color += greenCode;
                        extraColorText = "%";
                    }
                    else
                    {
                        color += whiteCode;
                        extraColorText = "%";
                    }
                    break;
                case TType.Percent:
                    if (input < 0)
                    {
                        color += redCode;
                        extraColorText = "% less";
                    }
                    else if (input > 0)
                    {
                        color += greenCode + "+";
                        extraColorText = "% extra";
                    }
                    else
                    {
                        color += whiteCode;
                        extraColorText = "% extra";
                    }
                    break;
                case TType.Flat:
                    if (input < 0)
                    {
                        color += redCode;
                    }
                    else if (input > 0)
                    {
                        color += greenCode + "+";
                    }
                    else
                    {
                        color += whiteCode;
                    }
                    break;
                case TType.PercentRedux:
                    if (input < 0)
                    {
                        input *= -1f;
                        color += redCode + "+";
                        extraColorText = "% increased";
                    }
                    else if (input > 0)
                    {
                        color += greenCode + "-";
                        extraColorText = "% reduced";
                    }
                    else
                    {
                        color += whiteCode;
                        extraColorText = "% reduced";
                    }
                    break;
                case TType.Straight:
                    color += whiteCode;
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

        public static float MultToPer(float number, bool redux = false)
        {
            if (redux) return (1 - number) * 100;
            return (number - 1) * 100;
        }
    }
}
