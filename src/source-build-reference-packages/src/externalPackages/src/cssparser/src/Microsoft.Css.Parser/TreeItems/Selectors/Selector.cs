// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Classify;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.Utilities;

namespace Microsoft.Css.Parser.TreeItems.Selectors
{
    /// <summary>
    /// CSS selector, where selector : element_selector [ combinator selector | S+ [ combinator? selector ]? ]?
    /// and combinator : '+' S* | '>' S*
    /// </summary>
    public class Selector : ComplexItem
    {
        /// <summary>
        /// List of simple selectors. Selectors may be separated by comma and if there is one, it is
        /// stored in the selector as its last element
        /// </summary>
        public SortedRangeList<SimpleSelector> SimpleSelectors { get; private set; }
        /// <summary>
        /// Seperating comma
        /// </summary>
        public TokenItem Comma { get; protected set; }

        public Selector()
        {
            Context = CssClassifierContextCache.FromTypeEnum(CssClassifierContextType.Selector);
            SimpleSelectors = new SortedRangeList<SimpleSelector>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public override bool Parse(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            while (!IsAtSelectorGroupTerminator(tokens))
            {
                SimpleSelector simpleSelector = itemFactory.CreateSpecific<SimpleSelector>(this);
                if (!simpleSelector.Parse(itemFactory, text, tokens))
                {
                    break;
                }

                SimpleSelectors.Add(simpleSelector);
                Children.Add(simpleSelector);
            }

            if (tokens.CurrentToken.TokenType == CssTokenType.Comma)
            {
                Comma = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.Punctuation);

                if (SimpleSelectors.Count == 0)
                {
                    Comma.AddParseError(ParseErrorType.SelectorBeforeCommaMissing, ParseErrorLocation.BeforeItem);
                }
            }

            if (SimpleSelectors.Count > 0)
            {
                SimpleSelector lastSimpleSelector = SimpleSelectors[SimpleSelectors.Count - 1];

                if (lastSimpleSelector.SelectorCombineOperator != null)
                {
                    lastSimpleSelector.AddParseError(ParseErrorType.SelectorAfterCombineOperatorMissing, ParseErrorLocation.AfterItem);
                }
            }

            return Children.Count > 0;
        }

        protected virtual bool IsAtSelectorGroupTerminator(TokenStream tokens)
        {
            return tokens.CurrentToken.IsSelectorGroupTerminator();
        }
    }
}
