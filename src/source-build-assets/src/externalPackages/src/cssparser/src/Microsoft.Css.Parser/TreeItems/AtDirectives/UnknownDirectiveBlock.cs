// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Diagnostics;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.Utilities;

namespace Microsoft.Css.Parser.TreeItems.AtDirectives
{
    /// <summary>
    /// This class holds declarations and @directives that are inside of another @directive.
    /// The parent @directive has an unknown name.
    /// We don't really know what contents are valid, so allow anything.
    /// </summary>
    internal sealed class UnknownDirectiveBlock : RuleBlock
    {
        internal SortedRangeList<RuleSet> RuleSets { get; private set; }

        internal UnknownDirectiveBlock()
        {
            RuleSets = new SortedRangeList<RuleSet>();
        }

        protected override ParseItem CreateDefaultChild(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            // http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems?id=468547 - The contents of unknown directives could be rules or declarations.
            // Make a good guess for which one to create.

            int startTokenPosition = tokens.Position;
            bool childIsDeclaration = true;
            ParseItem newChild;

            for (bool done = false; !done && !tokens.CurrentToken.IsBlockTerminator();)
            {
                // If I see a curly brace before a semicolon, then this child is probably a RuleSet

                switch (tokens.CurrentToken.TokenType)
                {
                    case CssTokenType.OpenCurlyBrace:
                        childIsDeclaration = false;
                        done = true;
                        break;

                    case CssTokenType.Semicolon:
                        done = true;
                        break;

                    default:
                        tokens.AdvanceToken();
                        break;
                }
            }

            tokens.Position = startTokenPosition;

            if (childIsDeclaration)
            {
                newChild = base.CreateDefaultChild(itemFactory, text, tokens);
            }
            else
            {
                newChild = itemFactory.Create<RuleSet>(this);

                if (!newChild.Parse(itemFactory, text, tokens))
                {
                    newChild = UnknownItem.ParseUnknown(this, itemFactory, text, tokens, ParseErrorType.UnexpectedToken);
                }
            }

            Debug.Assert(newChild != null);
            return newChild;
        }

        /// <summary>
        /// This only difference from a rule block is that unknown directive blocks allow child @directives.
        /// </summary>
        protected override ParseItem CreateDirective(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            ParseItem parseItem = AtDirective.ParseDirective(this, itemFactory, text, tokens);

            if (parseItem == null)
            {
                return UnknownItem.ParseUnknown(this, itemFactory, text, tokens, ParseErrorType.UnexpectedParseError);
            }

            return parseItem;
        }

        public override void UpdateCachedChildren()
        {
            RuleSets.Clear();

            foreach (ParseItem child in Children)
            {
                if (child is RuleSet)
                {
                    RuleSets.Add((RuleSet)child);
                }
            }

            base.UpdateCachedChildren();
        }
    }
}
