// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Classify;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;

namespace Microsoft.Css.Parser.TreeItems
{
    // This is the simplest type of item in the tree, just a single token
    // that cannot have any children. All leaf nodes MUST be a TokenItem.
    // The leaf nodes can point to eachother in a linked list for
    // faster breadth-first token traversal during colorization.

    public class TokenItem : ParseItem
    {
        internal CssToken Token { get; set; }

        internal TokenItem(CssToken token, CssClassifierContextType context)
            : this(token, CssClassifierContextCache.FromTypeEnum(context))
        {
        }

        internal TokenItem(CssToken token, IClassifierContext context)
        {
            Token = token;
            Context = context;
        }

        public CssTokenType TokenType
        {
            get { return Token.TokenType; }
        }

        public override int Start
        {
            get { return Token.Start; }
        }

        public override int Length
        {
            get { return Token.Length; }
        }

        internal TokenItem PreviousTokenItem
        {
            get
            {
                ParseItem sibling = PreviousSibling;
                ComplexItem parent = Parent;
                while (sibling == null && parent != null)
                {
                    sibling = parent.PreviousSibling;
                    parent = parent.Parent;
                }

                if (sibling is ComplexItem)
                {
                    sibling = (sibling as ComplexItem).LastDeepestToken;
                }

                return sibling as TokenItem;
            }
        }

        internal TokenItem NextTokenItem
        {
            get
            {
                ParseItem sibling = NextSibling;
                ComplexItem parent = Parent;
                while (sibling == null && parent != null)
                {
                    sibling = parent.NextSibling;
                    parent = parent.Parent;
                }

                if (sibling is ComplexItem)
                {
                    sibling = (sibling as ComplexItem).FirstDeepestToken;
                }

                return sibling as TokenItem;
            }
        }

        internal override bool IsUnclosed
        {
            get { return TokenType == CssTokenType.InvalidString; }
        }

        public override bool Parse(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            if (tokens != null)
            {
                Token = tokens.AdvanceToken();
            }

            return Token != null;
        }

#if DEBUG
        public override string ToString()
        {
            return Token.ToString();
        }
#endif
    }
}
