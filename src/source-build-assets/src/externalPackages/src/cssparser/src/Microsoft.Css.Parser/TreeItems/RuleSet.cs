// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems.Selectors;
using Microsoft.Css.Parser.Utilities;

namespace Microsoft.Css.Parser.TreeItems
{
    // http://www.w3.org/TR/2009/WD-css3-selectors-20090310/#grammar
    // ruleset : selectors_group
    //  '{' S* declaration? [ ';' S* declaration? ]* '}' S*
    // selectors_group : selector [ COMMA S* selector ]*
    // selector : simple_selector_sequence [ combinator simple_selector_sequence ]*
    // combinator  /* combinators can be surrounded by whitespace */ : PLUS S* | GREATER S* | TILDE S* | S+
    // simple_selector_sequence : [ type_selector | universal ]  [ HASH | class | attrib | pseudo | negation ]* | [ HASH | class | attrib | pseudo | negation ]+

    // http://www.w3.org/TR/css3-selectors
    // http://www.w3.org/TR/css3-selectors/#selector-syntax
    // A selector is a chain of one or more sequences of simple selectors separated by combinators.
    // A sequence of simple selectors is a chain of simple selectors that are not separated by
    // a combinator. It always begins with a type selector or a universal selector.
    // No other type selector or universal selector is allowed in the sequence.
    // A simple selector is either a type selector, universal selector, attribute selector,
    // class selector, ID selector, or pseudo-class. One pseudo-element may be appended
    // to the last sequence of simple selectors in a selector.
    //
    // Combinators are: whitespace, "greater-than sign" (U+003E, >), "plus sign" (U+002B, +)
    // and "tilde" (U+007E, ~). White space may appear between a combinator and the simple selectors
    // around it. Only the characters "space" (U+0020), "tab" (U+0009), "line feed" (U+000A),
    // "carriage return" (U+000D), and "form feed" (U+000C) can occur in whitespace.
    // Other space-like characters, such as "em-space" (U+2003) and "ideographic space" (U+3000),
    // are never part of whitespace

    /// <summary>
    /// CSS ruleset
    /// </summary>
    public class RuleSet : ComplexItem
    {
        public SortedRangeList<Selector> Selectors { get; private set; }
        public RuleBlock Block { get; private set; }

        public RuleSet()
        {
            Selectors = new SortedRangeList<Selector>();
        }

        /// <summary>
        /// True if ruleset block is not closed, i.e. closing curly brace is missing.
        /// </summary>
        internal override bool IsUnclosed
        {
            get { return Block != null && Block.IsUnclosed; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public override bool Parse(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            while (true)
            {
                if (tokens.CurrentToken.TokenType != CssTokenType.Comma &&
                    tokens.CurrentToken.IsSelectorGroupTerminator())
                {
                    break;
                }

                Selector selector = itemFactory.CreateSpecific<Selector>(this);
                if (!selector.Parse(itemFactory, text, tokens))
                {
                    break;
                }

                Selectors.Add(selector);
                Children.Add(selector);
            }

            Selector lastSelector = (Selectors.Count > 0) ? Selectors[Selectors.Count - 1] : null;

            if (tokens.CurrentToken.TokenType == CssTokenType.OpenCurlyBrace)
            {
                if (Children.Count == 0)
                {
                    Children.AddParseError(ParseErrorType.SelectorBeforeRuleBlockMissing);
                }

                if (lastSelector != null && lastSelector.Comma != null)
                {
                    Children.AddParseError(ParseErrorType.SelectorAfterCommaMissing);
                }

                RuleBlock ruleBlock = itemFactory.CreateSpecific<RuleBlock>(this);
                if (ruleBlock.Parse(itemFactory, text, tokens))
                {
                    Block = ruleBlock;
                    Children.Add(ruleBlock);
                }
            }
            else if (lastSelector != null)
            {
                AddParseError(ParseErrorType.OpenCurlyBraceMissingForRule, ParseErrorLocation.AfterItem);
            }

            return Children.Count > 0;
        }
    }
}
