using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kingskills.UX
{
    class PrettyText
    {
        public static string Prettify(float input, float decimals, TextType type)
        {
            string color = "<color=";
            string endColor = "</color>";
            string extraColorText = "";
            string extraCleanText = "";

            switch (type)
            {
                case TextType.TextlessPercent:
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
                case TextType.Percent:
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
                case TextType.Flat:
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
                case TextType.PercentRedux:
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
                case TextType.Straight:
                    color += "white>";
                    break;
                case TextType.ColorlessPercent:
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

        public enum TextType
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
