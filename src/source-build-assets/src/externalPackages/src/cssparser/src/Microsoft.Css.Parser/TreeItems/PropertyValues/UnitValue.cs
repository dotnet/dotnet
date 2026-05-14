// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Diagnostics;
using Microsoft.Css.Parser.Classify;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;

namespace Microsoft.Css.Parser.TreeItems.PropertyValues
{
    // http://www.w3.org/TR/2005/WD-css3-values-20050726/
    /// <summary>
    /// CSS units type
    /// </summary>
    public enum UnitType
    {
        Unknown,
        Percentage,
        Length,
        Angle,
        Time,
        Frequency,
        Resolution,
        Semitones,
        Grid,       // http://dev.w3.org/csswg/css3-values/
        Viewport,   // http://www.w3.org/TR/2005/WD-css3-values-20050726/#relative0
        Volume,     // http://www.w3.org/TR/css3-speech/#voice-volume
    }

    /// <summary>
    /// CSS property unit value
    /// </summary>
    public class UnitValue : NumericalValue
    {
        /// <summary>
        /// Units token
        /// </summary>
        internal TokenItem UnitToken { get; private set; }
        /// <summary>
        /// Units type
        /// </summary>
        public UnitType UnitType { get; private set; }

        internal override bool TreatAsWord => true;

        public UnitValue()
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public override bool Parse(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            UnitType = UnitType.Unknown;

            if (tokens.CurrentToken.TokenType == CssTokenType.Number)
            {
                if (!base.Parse(itemFactory, text, tokens))
                {
                    return false;
                }
            }

            Debug.Assert(tokens.CurrentToken.TokenType == CssTokenType.Units);
            UnitToken = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.Units);
            UnitType = UnitHelpers.GetUnitType(text, UnitToken.Start, UnitToken.Length);

            CssClassifierContextType ct = CssClassifierContextType.Units;

            switch (UnitType)
            {
                case UnitType.Angle:
                    ct = CssClassifierContextType.AngleUnits;
                    break;

                case UnitType.Frequency:
                    ct = CssClassifierContextType.FrequencyUnits;
                    break;

                case UnitType.Grid:
                    ct = CssClassifierContextType.GridUnits;
                    break;

                case UnitType.Length:
                    ct = CssClassifierContextType.LengthUnits;
                    break;

                case UnitType.Percentage:
                    ct = CssClassifierContextType.PercentUnits;
                    break;

                case UnitType.Resolution:
                    ct = CssClassifierContextType.ResolutionUnits;
                    break;

                case UnitType.Time:
                    ct = CssClassifierContextType.TimeUnits;
                    break;

                case UnitType.Viewport:
                    ct = CssClassifierContextType.ViewUnits;
                    break;
            }

            UnitToken.Context = CssClassifierContextCache.FromTypeEnum(ct);

            return Children.Count > 0;
        }
    }
}
