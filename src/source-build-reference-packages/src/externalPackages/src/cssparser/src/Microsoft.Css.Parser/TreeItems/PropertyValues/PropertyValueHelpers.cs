// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Classify;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems.Functions;

namespace Microsoft.Css.Parser.TreeItems.PropertyValues
{
    internal static class PropertyValueHelpers
    {
        internal static ParseItem ParsePropertyValue(ComplexItem parent, ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            return ParsePropertyValue(parent, itemFactory, text, tokens, true);
        }

        internal static ParseItem ParsePropertyValue(ComplexItem parent, ItemFactory itemFactory, ITextProvider text, TokenStream tokens, bool callExternalFactory)
        {
            ParseItem pv = null;
            CssToken token = tokens.CurrentToken;

            // First give opportunity to override property value parsing
            if (callExternalFactory)
            {
                pv = itemFactory.Create<UnknownPropertyValue>(parent);
            }

            if (pv == null || pv.GetType() == typeof(UnknownPropertyValue))
            {
                switch (token.TokenType)
                {
                    case CssTokenType.HashName:
                        pv = itemFactory.Create<HexColorValue>(parent);
                        break;

                    case CssTokenType.Number:
                        pv = NumericalValue.ParseNumber(parent, itemFactory, text, tokens);
                        return pv;

                    case CssTokenType.Url:
                    case CssTokenType.UnquotedUrlString:
                        pv = itemFactory.Create<UrlItem>(parent);
                        break;

                    case CssTokenType.Function:
                        pv = Function.ParseFunction(parent, itemFactory, text, tokens);
                        return pv;

                    case CssTokenType.UnicodeRange:
                        pv = new TokenItem(tokens.CurrentToken, CssClassifierContextType.UnicodeRange);
                        break;

                    case CssTokenType.Comma:
                    case CssTokenType.Slash:
                        pv = new TokenItem(tokens.CurrentToken, CssClassifierContextType.Punctuation);
                        break;

                    case CssTokenType.String:
                    case CssTokenType.MultilineString:
                    case CssTokenType.InvalidString:
                        pv = new TokenItem(tokens.CurrentToken, CssClassifierContextType.String);
                        break;

                    case CssTokenType.Identifier:
                        pv = new TokenItem(tokens.CurrentToken, null);
                        break;

                    case CssTokenType.OpenSquareBracket:
                    case CssTokenType.OpenFunctionBrace:
                    case CssTokenType.OpenCurlyBrace:
                        // "grid-rows" uses square brackets - http://dev.w3.org/csswg/css3-grid/
                        // And this is from a Win8 spec: -ms-grid-columns: (200px 10px)[3];
                        // Also, custom property values may have curly brace blocks
                        pv = itemFactory.Create<PropertyValueBlock>(parent);
                        break;

                    default:
                        if (callExternalFactory)
                        {
                            pv = itemFactory.Create<UnknownPropertyValue>(parent);

                            if (pv.GetType() == typeof(UnknownPropertyValue))
                            {
                                // UnknownPropertyValue is just a placeholder for plugins to use.
                                // If one is actually created, discard it.
                                pv = null;
                            }
                        }
                        break;
                }
            }

            if (pv != null)
            {
                if (!pv.Parse(itemFactory, text, tokens))
                {
                    pv = null;
                }
            }

            return pv;
        }

        internal static bool IsValidInteger(ParseItem valueItem)
        {
            UnitValue unitValue = valueItem as UnitValue;

            // Must not have units after it, or a decimal point in it
            return unitValue == null && valueItem is NumericalValue numberValue && !numberValue.Text.Contains(".");
        }

        internal static bool IsValidNumber(ParseItem valueItem)
        {
            UnitValue unitValue = valueItem as UnitValue;

            // Must not have units after it
            return unitValue == null && valueItem is NumericalValue;
        }

        internal static UnitType GetUnitType(ParseItem valueItem)
        {
            return (valueItem is UnitValue unitValue) ? unitValue.UnitType : UnitType.Unknown;
        }
    }
}
