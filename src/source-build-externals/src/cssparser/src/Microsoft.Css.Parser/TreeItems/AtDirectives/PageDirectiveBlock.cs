// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.Utilities;

namespace Microsoft.Css.Parser.TreeItems.AtDirectives
{
    /// <summary>
    /// Adds @margin directive support to normal rule blocks
    /// </summary>
    internal sealed class PageDirectiveBlock : RuleBlock
    {
        internal SortedRangeList<MarginDirective> Margins { get; private set; }

        internal PageDirectiveBlock()
        {
            Margins = new SortedRangeList<MarginDirective>();
        }

        protected override ParseItem CreateDirective(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            MarginDirective marginDirective = new MarginDirective();

            if (marginDirective.Parse(itemFactory, text, tokens))
            {
                return marginDirective;
            }
            else
            {
                return UnknownItem.ParseUnknown(this, itemFactory, text, tokens, ParseErrorType.UnexpectedParseError);
            }
        }

        public override void UpdateCachedChildren()
        {
            Margins.Clear();

            foreach (ParseItem child in Children)
            {
                if (child is MarginDirective)
                {
                    Margins.Add((MarginDirective)child);
                }
            }

            base.UpdateCachedChildren();
        }
    }
}
