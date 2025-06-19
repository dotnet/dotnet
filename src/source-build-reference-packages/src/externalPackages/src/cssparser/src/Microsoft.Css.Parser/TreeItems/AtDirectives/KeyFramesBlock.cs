// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Classify;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.Utilities;

namespace Microsoft.Css.Parser.TreeItems.AtDirectives
{
    internal class KeyFramesBlock : BlockItem, IIncrementalParseItem
    {
        internal SortedRangeList<KeyFramesRuleSet> KeyFrames { get; private set; }

        public KeyFramesBlock()
        {
            KeyFrames = new SortedRangeList<KeyFramesRuleSet>();
        }

        public override bool Parse(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            return IncrementalParseHelper.FullParseIncrementalItem(this, itemFactory, text, tokens);
        }

        public ParseItem CreateNextChild(ParseItem previousChild, ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            if (previousChild is TokenItem && ((TokenItem)previousChild).TokenType == CssTokenType.CloseCurlyBrace)
            {
                // No more children after the close curly brace
                return null;
            }

            if (previousChild == null && tokens.CurrentToken.TokenType != CssTokenType.OpenCurlyBrace)
            {
                // First child must be a curly brace
                return null;
            }

            if (tokens.CurrentToken.IsScopeBlocker())
            {
                return null;
            }

            ParseItem newChild;
            switch (tokens.CurrentToken.TokenType)
            {
                case CssTokenType.OpenCurlyBrace:
                    newChild = (previousChild == null) ? new TokenItem(tokens.AdvanceToken(), CssClassifierContextType.CurlyBrace) : null;
                    break;

                case CssTokenType.CloseCurlyBrace:
                    newChild = new TokenItem(tokens.AdvanceToken(), CssClassifierContextType.CurlyBrace);
                    break;

                default:
                    newChild = CreateDefaultChild(itemFactory, text, tokens);
                    break;
            }

            return newChild;
        }

        protected virtual ParseItem CreateDefaultChild(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            ParseItem child = itemFactory.Create<KeyFramesRuleSet>(this);

            if (!child.Parse(itemFactory, text, tokens))
            {
                child = UnknownItem.ParseUnknown(this, itemFactory, text, tokens, ParseErrorType.UnexpectedParseError);
            }

            return child;
        }

        /// <summary>
        /// Always called after parsing is done (full or incremental)
        /// </summary>
        public override void UpdateCachedChildren()
        {
            KeyFrames.Clear();

            foreach (ParseItem child in Children)
            {
                if (child is KeyFramesRuleSet)
                {
                    KeyFrames.Add((KeyFramesRuleSet)child);
                }
            }

            base.UpdateCachedChildren();
        }
    }
}
