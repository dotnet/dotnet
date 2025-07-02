// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Diagnostics;
using Microsoft.Css.Parser.Classify;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems.Functions;

namespace Microsoft.Css.Parser.TreeItems.Selectors
{
    public class PseudoClassFunctionSelector : ComplexItem // E:foo(arg)
    {
        internal TokenItem Colon { get; private set; }
        public Function Function { get; protected set; }

        public PseudoClassFunctionSelector()
        {
            Context = CssClassifierContextCache.FromTypeEnum(CssClassifierContextType.PseudoClass);
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
            if (tokens.CurrentToken.TokenType == CssTokenType.Function &&
                tokens.CurrentToken.Start == Colon.AfterEnd)
            {
                if (TextRange.CompareDecoded(tokens.CurrentToken.Start, tokens.CurrentToken.Length, text, "not(", ignoreCase: true))
                {
                    Function = itemFactory.CreateSpecific<PseudoFunctionNot>(this);
                }
                else if (TextRange.CompareDecoded(tokens.CurrentToken.Start, tokens.CurrentToken.Length, text, "matches(", ignoreCase: true))
                {
                    Function = itemFactory.CreateSpecific<PseudoFunctionMatches>(this);
                }
                else
                {
                    Function = itemFactory.CreateSpecific<Function>(this);
                }

                Function.Parse(itemFactory, text, tokens);

                // Function name should be colorizer as pseudo-class
                Function.Context = CssClassifierContextCache.FromTypeEnum(CssClassifierContextType.PseudoClass);
                Function.FunctionName.Context = Function.Context;

                Children.Add(Function);
            }
            else
            {
                Children.AddParseError(ParseErrorType.PseudoClassNameMissing);
            }
        }
    }
}
