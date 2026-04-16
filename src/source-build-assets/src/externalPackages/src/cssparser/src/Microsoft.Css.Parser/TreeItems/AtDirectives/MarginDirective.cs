// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;

namespace Microsoft.Css.Parser.TreeItems.AtDirectives
{
    internal enum MarginDirectiveType
    {
        TopLeftCorner,
        TopLeft,
        TopCenter,
        TopRight,
        TopRightCorner,
        BottomLeftCorner,
        BottomLeft,
        BottomCenter,
        BottomRight,
        BottomRightCorner,
        LeftTop,
        LeftMiddle,
        LeftBottom,
        RightTop,
        RightMiddle,
        RightBottom,
        Unknown
    }

    internal sealed class MarginDirective : AtBlockDirective
    {
        // margin : margin_sym S* '{' declaration [ ';' S* declaration? ]* '}' S*
        // margin_sym :  TOPLEFTCORNER_SYM | ...

        internal MarginDirectiveType DirectiveType { get; private set; }
        internal RuleBlock RuleBlock { get; private set; }

        internal MarginDirective()
        {
            DirectiveType = MarginDirectiveType.Unknown;
        }

        internal override BlockItem Block
        {
            get { return RuleBlock; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public override bool Parse(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            if (tokens.CurrentToken.TokenType == CssTokenType.At)
            {
                ParseAtAndKeyword(itemFactory, text, tokens);

                // all lowercase per W3C BNF

                if (Keyword == null)
                {
                    Children.AddParseError(ParseErrorType.AtDirectiveNameMissing);
                }
                else if (TextRange.CompareDecoded(Keyword.Start, Keyword.Length, text, "top-left-corner", ignoreCase: false))
                {
                    DirectiveType = MarginDirectiveType.TopLeftCorner;
                }
                else if (TextRange.CompareDecoded(Keyword.Start, Keyword.Length, text, "top-left", ignoreCase: false))
                {
                    DirectiveType = MarginDirectiveType.TopLeft;
                }
                else if (TextRange.CompareDecoded(Keyword.Start, Keyword.Length, text, "top-center", ignoreCase: false))
                {
                    DirectiveType = MarginDirectiveType.TopCenter;
                }
                else if (TextRange.CompareDecoded(Keyword.Start, Keyword.Length, text, "top-right", ignoreCase: false))
                {
                    DirectiveType = MarginDirectiveType.TopRight;
                }
                else if (TextRange.CompareDecoded(Keyword.Start, Keyword.Length, text, "top-right-corner", ignoreCase: false))
                {
                    DirectiveType = MarginDirectiveType.TopRightCorner;
                }
                else if (TextRange.CompareDecoded(Keyword.Start, Keyword.Length, text, "bottom-left-corner", ignoreCase: false))
                {
                    DirectiveType = MarginDirectiveType.BottomLeftCorner;
                }
                else if (TextRange.CompareDecoded(Keyword.Start, Keyword.Length, text, "bottom-left", ignoreCase: false))
                {
                    DirectiveType = MarginDirectiveType.BottomLeft;
                }
                else if (TextRange.CompareDecoded(Keyword.Start, Keyword.Length, text, "bottom-center", ignoreCase: false))
                {
                    DirectiveType = MarginDirectiveType.BottomCenter;
                }
                else if (TextRange.CompareDecoded(Keyword.Start, Keyword.Length, text, "bottom-right", ignoreCase: false))
                {
                    DirectiveType = MarginDirectiveType.BottomRight;
                }
                else if (TextRange.CompareDecoded(Keyword.Start, Keyword.Length, text, "bottom-right-corner", ignoreCase: false))
                {
                    DirectiveType = MarginDirectiveType.BottomRightCorner;
                }
                else if (TextRange.CompareDecoded(Keyword.Start, Keyword.Length, text, "left-top", ignoreCase: false))
                {
                    DirectiveType = MarginDirectiveType.LeftTop;
                }
                else if (TextRange.CompareDecoded(Keyword.Start, Keyword.Length, text, "left-middle", ignoreCase: false))
                {
                    DirectiveType = MarginDirectiveType.LeftMiddle;
                }
                else if (TextRange.CompareDecoded(Keyword.Start, Keyword.Length, text, "left-bottom", ignoreCase: false))
                {
                    DirectiveType = MarginDirectiveType.LeftBottom;
                }
                else if (TextRange.CompareDecoded(Keyword.Start, Keyword.Length, text, "right-top", ignoreCase: false))
                {
                    DirectiveType = MarginDirectiveType.RightTop;
                }
                else if (TextRange.CompareDecoded(Keyword.Start, Keyword.Length, text, "right-middle", ignoreCase: false))
                {
                    DirectiveType = MarginDirectiveType.RightMiddle;
                }
                else if (TextRange.CompareDecoded(Keyword.Start, Keyword.Length, text, "right-bottom", ignoreCase: false))
                {
                    DirectiveType = MarginDirectiveType.RightBottom;
                }
                else
                {
                    DirectiveType = MarginDirectiveType.Unknown;
                }
            }

            RuleBlock = itemFactory.CreateSpecific<RuleBlock>(this);

            if (!ParseBlock(RuleBlock, itemFactory, text, tokens))
            {
                RuleBlock = null;
            }

            return Children.Count > 0;
        }
    }
}
