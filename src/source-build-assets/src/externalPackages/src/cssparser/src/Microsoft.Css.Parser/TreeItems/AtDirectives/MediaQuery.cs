// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Classify;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.Utilities;

namespace Microsoft.Css.Parser.TreeItems.AtDirectives
{
    public class MediaQuery : ComplexItem
    {
        internal SortedRangeList<MediaExpression> Expressions { get; private set; }
        public TokenItem Operation { get; private set; } // ONLY or NOT
        public TokenItem MediaType { get; private set; }
        internal TokenItem Comma { get; private set; }

        public MediaQuery()
        {
            Expressions = new SortedRangeList<MediaExpression>();
            Context = CssClassifierContextCache.FromTypeEnum(CssClassifierContextType.MediaQuery);
        }

        // media_query : [ONLY | NOT]? S* media_type S* [ AND S* expression ]* | expression [ AND S* expression ]*
        // media_type : IDENT
        // expression : '(' S* media_feature S* [':' S* expr]? ')' S*
        // media_feature : IDENT
        // expr : term [ operator? term ]*
        //
        // 'expr' is generally similar to declaration except it does not allow !important qualifier

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public override bool Parse(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            if (tokens.CurrentToken.TokenType == CssTokenType.Comma)
            {
                Comma = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.Default);
            }

            if (tokens.CurrentToken.TokenType == CssTokenType.Identifier)
            {
                if (TextRange.CompareDecoded(tokens.CurrentToken.Start, tokens.CurrentToken.Length, text, "only", ignoreCase: true) ||
                    TextRange.CompareDecoded(tokens.CurrentToken.Start, tokens.CurrentToken.Length, text, "not", ignoreCase: true))
                {
                    Operation = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.MediaQueryOperation);
                }
            }

            if (tokens.CurrentToken.TokenType == CssTokenType.Identifier &&
                !IsMediaExpressionStart(text, tokens.CurrentToken))
            {
                MediaType = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.MediaType);
            }
            else if (tokens.CurrentToken.TokenType != CssTokenType.OpenFunctionBrace)
            {
                Children.AddParseError(ParseErrorType.MediaTypeMissing);
            }

            while (!tokens.CurrentToken.IsDirectiveTerminator() && tokens.CurrentToken.TokenType != CssTokenType.Comma)
            {
                if (IsMediaExpressionStart(text, tokens.CurrentToken))
                {
                    MediaExpression mx = itemFactory.CreateSpecific<MediaExpression>(this);
                    if (mx.Parse(itemFactory, text, tokens))
                    {
                        Expressions.Add(mx);
                        Children.Add(mx);
                    }
                    else
                    {
                        Children.AddUnknownAndAdvance(itemFactory, text, tokens, ParseErrorType.MediaExpressionExpected);
                    }
                }
                else
                {
                    Children.AddUnknownAndAdvance(itemFactory, text, tokens, ParseErrorType.UnexpectedMediaQueryToken);
                }
            }

            return Children.Count > 0;
        }

        internal static bool IsMediaExpressionStart(ITextProvider text, CssToken token)
        {
            switch (token.TokenType)
            {
                case CssTokenType.OpenFunctionBrace:
                    return true;

                case CssTokenType.Identifier:
                    if (TextRange.CompareDecoded(token.Start, token.Length, text, "and", true))
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }
    }
}
