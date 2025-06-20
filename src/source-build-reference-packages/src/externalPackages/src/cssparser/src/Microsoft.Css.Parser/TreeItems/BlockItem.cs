// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Tokens;

namespace Microsoft.Css.Parser.TreeItems
{
    /// <summary>
    /// Class represents block item i.g. item that has opening and closing curly braces
    /// </summary>
    public abstract class BlockItem : ComplexItem
    {
        /// <summary>
        /// Token of the opening curly brace
        /// </summary>
        public TokenItem OpenCurlyBrace { get; protected set; }
        /// <summary>
        /// Token of the closing curly brace
        /// </summary>
        public TokenItem CloseCurlyBrace { get; protected set; }

        protected BlockItem()
        {
        }

        /// <summary>
        /// True if block is not closed, i.e. closing curly brace is missing
        /// </summary>
        internal override bool IsUnclosed
        {
            get { return CloseCurlyBrace == null; }
        }

        public virtual void UpdateCachedChildren()
        {
            OpenCurlyBrace = null;
            CloseCurlyBrace = null;

            int childCount = Children.Count;
            for (int i = 0; i < childCount; i++)
            {
                ParseItem child = Children[i];
                if (child is TokenItem tokenItem)
                {
                    if (tokenItem.TokenType == CssTokenType.OpenCurlyBrace && OpenCurlyBrace == null)
                    {
                        OpenCurlyBrace = tokenItem;
                    }
                    else if (tokenItem.TokenType == CssTokenType.CloseCurlyBrace && CloseCurlyBrace == null)
                    {
                        CloseCurlyBrace = tokenItem;
                    }
                }
            }
        }

        public virtual bool UpdateParseErrors()
        {
            bool changed;

            if (OpenCurlyBrace != null && CloseCurlyBrace == null)
            {
                changed = AddParseError(ParseErrorType.CloseCurlyBraceMissing, ParseErrorLocation.AfterItem);
            }
            else
            {
                changed = RemoveParseError(ParseErrorType.CloseCurlyBraceMissing);
            }

            return changed;
        }
    }
}
