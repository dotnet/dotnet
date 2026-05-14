// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Classify;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;

namespace Microsoft.Css.Parser.TreeItems.Comments
{
    /// <summary>
    /// Holds onto a single comment token
    /// </summary>
    internal sealed class CommentTokenItem : Comment
    {
        public TokenItem Comment { get; private set; }

        public CommentTokenItem()
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public override bool Parse(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            CssTokenType tokenType = tokens.CurrentToken.TokenType;
            if (tokenType == CssTokenType.CommentText ||
                tokenType == CssTokenType.SingleTokenComment ||
                tokenType == CssTokenType.SingleLineComment)
            {
                CssClassifierContextType context = tokenType == CssTokenType.CommentText
                    ? CssClassifierContextType.Default
                    : CssClassifierContextType.Comment;

                Comment = Children.AddCurrentAndAdvance(tokens, context);
            }

            return Children.Count > 0;
        }
    }
}
