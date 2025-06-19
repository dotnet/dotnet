// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Classify;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems.PropertyValues;

namespace Microsoft.Css.Parser.TreeItems.AtDirectives
{
    public class MediaExpression : ComplexItem
    {
        public ParseItemList Values { get; private set; }
        internal TokenItem MediaCombineOperator { get; private set; } // AND
        public TokenItem OpenFunctionBrace { get; private set; }
        public ParseItem MediaFeature { get; protected set; }  // media_feature [: expr]?
        public TokenItem Colon { get; private set; }
        internal TokenItem CloseFunctionBrace { get; private set; }

        public MediaExpression()
        {
            Values = new ParseItemList();
        }

        // Similar to declaration, but ends with ) and does not allow !important
        // expression : '(' S* media_feature S* [':' S* expr]? ')' S*
        // media_feature : IDENT
        // expr : term [ operator? term ]*
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public override bool Parse(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            if (tokens.CurrentToken.TokenType == CssTokenType.Identifier &&
                TextRange.CompareDecoded(tokens.CurrentToken.Start, tokens.CurrentToken.Length, text, "and", true))
            {
                MediaCombineOperator = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.MediaCombineOperator);
            }

            if (tokens.CurrentToken.TokenType == CssTokenType.OpenFunctionBrace)
            {
                OpenFunctionBrace = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.FunctionBrace);
                ParseFeatureName(itemFactory, text, tokens);

                if (tokens.CurrentToken.TokenType == CssTokenType.Colon)
                {
                    Colon = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.Punctuation);
                }

                while (tokens.CurrentToken.TokenType != CssTokenType.CloseFunctionBrace &&
                    tokens.CurrentToken.TokenType != CssTokenType.OpenCurlyBrace &&
                    !tokens.CurrentToken.IsScopeBlocker())
                {
                    ParseNextValue(itemFactory, text, tokens);
                }

                if (tokens.CurrentToken.TokenType == CssTokenType.CloseFunctionBrace)
                {
                    CloseFunctionBrace = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.FunctionBrace);
                }
                else
                {
                    OpenFunctionBrace.AddParseError(ParseErrorType.CloseFunctionBraceMissing, ParseErrorLocation.AfterItem);
                }

                if ((Colon != null && Children[Children.Count - 1] == Colon) ||
                    (Colon != null && CloseFunctionBrace != null && Children.IndexOf(Colon) + 1 == Children.IndexOf(CloseFunctionBrace)))
                {
                    // There was nothing between the colon and close brace
                    Colon.AddParseError(ParseErrorType.PropertyValueMissing, ParseErrorLocation.AfterItem);
                }
            }

            if (MediaCombineOperator != null && OpenFunctionBrace == null)
            {
                MediaCombineOperator.AddParseError(ParseErrorType.MediaExpressionExpected, ParseErrorLocation.AfterItem);
            }

            return Children.Count > 0;
        }

        protected virtual void ParseFeatureName(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            if (tokens.CurrentToken.TokenType == CssTokenType.Identifier)
            {
                MediaFeature = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.MediaFeatureName);
            }
            else
            {
                Children.AddParseError(ParseErrorType.PropertyNameMissing);
            }
        }

        protected virtual void ParseNextValue(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            if (Colon != null)
            {
                ParseItem pi = PropertyValueHelpers.ParsePropertyValue(this, itemFactory, text, tokens);

                if (pi != null)
                {
                    Values.Add(pi);
                    Children.Add(pi);
                }
                else
                {
                    Children.AddUnknownAndAdvance(itemFactory, text, tokens, ParseErrorType.PropertyValueExpected);
                }
            }
            else
            {
                Children.AddUnknownAndAdvance(itemFactory, text, tokens, ParseErrorType.UnexpectedToken);
            }
        }
    }
}
