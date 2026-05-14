// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Classify;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems.PropertyValues;

namespace Microsoft.Css.Parser.TreeItems.AtDirectives
{
    internal enum KeyFrameSelectorType
    {
        Unknown,
        From,
        To,
        Percentage
    }

    internal sealed class KeyFramesSelector : ComplexItem
    {
        internal KeyFrameSelectorType SelectorType { get; private set; }
        internal TokenItem Name { get; private set; }
        internal TokenItem Comma { get; private set; }

        internal KeyFramesSelector()
        {
            SelectorType = KeyFrameSelectorType.Unknown;
            Context = CssClassifierContextCache.FromTypeEnum(CssClassifierContextType.Selector);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public override bool Parse(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            switch (tokens.CurrentToken.TokenType)
            {
                case CssTokenType.Identifier:
                    if (TextRange.CompareDecoded(tokens.CurrentToken.Start, tokens.CurrentToken.Length, text, "from", true))
                    {
                        SelectorType = KeyFrameSelectorType.From;
                        Name = Children.AddCurrentAndAdvance(tokens, null);
                    }
                    else if (TextRange.CompareDecoded(tokens.CurrentToken.Start, tokens.CurrentToken.Length, text, "to", true))
                    {
                        SelectorType = KeyFrameSelectorType.To;
                        Name = Children.AddCurrentAndAdvance(tokens, null);
                    }
                    break;

                case CssTokenType.Number:
                    // Must be a percentage:
                    if (tokens.Peek(1).TokenType == CssTokenType.Units &&
                        TextRange.Compare(tokens.Peek(1).Start, tokens.Peek(1).Length, text, "%", ignoreCase: false))
                    {
                        UnitValue uv = new UnitValue();

                        if (uv.Parse(itemFactory, text, tokens))
                        {
                            SelectorType = KeyFrameSelectorType.Percentage;
                            Children.Add(uv);
                        }
                    }
                    break;
            }

            if (tokens.CurrentToken.TokenType == CssTokenType.Comma && Children.Count > 0)
            {
                Comma = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.Punctuation);
            }

            return Children.Count > 0;
        }
    }
}
