// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems.Functions;

namespace Microsoft.Css.Parser.TreeItems.Selectors
{
    /// <summary>
    /// An simple selector argument for a pseudo function.
    /// </summary>
    internal sealed class PseudoSelectorArgument : FunctionArgument
    {
        internal SimpleSelector Selector { get; private set; }

        public PseudoSelectorArgument()
        {
        }

        protected override void ParseArgument(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            SimpleSelector simpleSelector = itemFactory.CreateSpecific<SimpleSelector>(this);

            if (simpleSelector.ParseInFunction(itemFactory, text, tokens))
            {
                Selector = simpleSelector;
                ArgumentItems.Add(Selector);
                Children.Add(Selector);
            }

            if (tokens.CurrentToken.TokenType != CssTokenType.Comma &&
                tokens.CurrentToken.TokenType != CssTokenType.CloseFunctionBrace)
            {
                Children.AddParseError(ParseErrorType.FunctionArgumentCommaMissing);
            }
        }
    }
}
