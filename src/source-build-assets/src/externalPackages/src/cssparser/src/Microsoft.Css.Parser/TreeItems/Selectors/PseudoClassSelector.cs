// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Diagnostics;
using Microsoft.Css.Parser.Classify;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;

namespace Microsoft.Css.Parser.TreeItems.Selectors
{
    public class PseudoClassSelector : ComplexItem // E:first-child
    {
        internal ParseItem Colon { get; private set; }
        public ParseItem PseudoClass { get; protected set; }

        public PseudoClassSelector()
        {
            Context = CssClassifierContextCache.FromTypeEnum(CssClassifierContextType.PseudoClass);
        }

        internal static bool IsNameToken(CssToken token)
        {
            return
                token.TokenType == CssTokenType.Identifier ||
                token.TokenType == CssTokenType.Minus;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public override bool Parse(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            Debug.Assert(tokens.CurrentToken.TokenType == CssTokenType.Colon);
            if (tokens.CurrentToken.TokenType == CssTokenType.Colon)
            {
                Colon = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.PseudoClass);
                ParseName(itemFactory, text, tokens);
            }

            return Children.Count > 0;
        }

        protected virtual void ParseName(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            if (IsNameToken(tokens.CurrentToken) && tokens.CurrentToken.Start == Colon.AfterEnd)
            {
                PseudoClass = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.PseudoClass);
            }
            else
            {
                Children.AddParseError(ParseErrorType.PseudoClassNameMissing);
            }
        }
    }
}
