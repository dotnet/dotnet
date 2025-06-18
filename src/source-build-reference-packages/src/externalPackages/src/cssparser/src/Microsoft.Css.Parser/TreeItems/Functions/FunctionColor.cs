// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Globalization;
using Microsoft.Css.Parser.Classify;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.TreeItems.PropertyValues;

namespace Microsoft.Css.Parser.TreeItems.Functions
{
    internal enum ColorFunctionType
    {
        None,
        Rgb,
        Rgba,
        Hsl,
        Hsla,
    }

    /// <summary>
    /// CSS color functions (rgb, rgba, hsl, hsla)
    /// </summary>
    public class FunctionColor : Function
    {
        internal ColorFunctionType ColorFunction { get; set; }

        public FunctionColor()
        {
            // The function type gets updated during parsing
            ColorFunction = ColorFunctionType.None;
            Context = CssClassifierContextCache.FromTypeEnum(CssClassifierContextType.FunctionColor);
        }

        /// <summary>
        /// Do the function parameters follow the CSS standard?
        /// </summary>
        internal virtual bool HasStandardParameters
        {
            get { return true; }
        }

        /// <summary>
        /// This helps the ColorParser class to figure out the values of each color argument.
        /// If the output argumentValue is float.NaN then the argument can still be valid, but
        /// its actualy value cannot be determined.
        /// </summary>
        internal virtual bool GetColorArgumentValue(int argumentIndex, bool looseParsing, out float argumentValue)
        {
            argumentValue = 0;

            FunctionArgument colorArgument = (argumentIndex >= 0 && argumentIndex < Arguments.Count)
                ? Arguments[argumentIndex] as FunctionArgument
                : null;

            // Make sure the argument has the right number of children

            if (colorArgument == null ||
                colorArgument.ArgumentItems.Count == 0 ||
                (!looseParsing && colorArgument.ArgumentItems.Count != 1))
            {
                return false;
            }

            ParseItem colorNumber = colorArgument.ArgumentItems[0];
            UnitType unitType = PropertyValueHelpers.GetUnitType(colorNumber);

            if (argumentIndex > 3)
            {
                // No color function has more than four arguments
                return false;
            }
            else if (argumentIndex == 3)
            {
                // It's the alpha value
                if (ColorFunction == ColorFunctionType.Rgba || ColorFunction == ColorFunctionType.Hsla)
                {
                    if (PropertyValueHelpers.IsValidNumber(colorNumber))
                    {
                        if (float.TryParse(colorNumber.Text, NumberStyles.Number, NumberFormatInfo.InvariantInfo, out argumentValue))
                        {
                            return true;
                        }
                    }
                }
            }
            else if (ColorFunction == ColorFunctionType.Rgb || ColorFunction == ColorFunctionType.Rgba)
            {
                // Red, Green, and Blue are always a <percentage> or <integer>

                if (unitType == UnitType.Percentage)
                {
                    if (float.TryParse(colorNumber.Text.TrimEnd('%'), NumberStyles.Number, NumberFormatInfo.InvariantInfo, out argumentValue))
                    {
                        argumentValue /= 100.0f;
                        return true;
                    }
                }
                else if (PropertyValueHelpers.IsValidInteger(colorNumber))
                {
                    if (int.TryParse(colorNumber.Text, out int intValue))
                    {
                        argumentValue = intValue / 255.0f;
                        return true;
                    }
                }
            }
            else if (argumentIndex == 0)
            {
                // Hue is always a <number>

                if (PropertyValueHelpers.IsValidNumber(colorNumber))
                {
                    if (float.TryParse(colorNumber.Text, NumberStyles.Number, NumberFormatInfo.InvariantInfo, out argumentValue))
                    {
                        argumentValue /= 360.0f;
                        return true;
                    }
                }
            }
            else
            {
                // Saturation and Lightness are always a percentage

                if (unitType == UnitType.Percentage)
                {
                    if (float.TryParse(colorNumber.Text.TrimEnd('%'), NumberStyles.Number, NumberFormatInfo.InvariantInfo, out argumentValue))
                    {
                        argumentValue /= 100.0f;
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
