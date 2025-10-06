// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.Css.Parser.TreeItems;
using Microsoft.Css.Parser.TreeItems.Comments;

namespace Microsoft.Css.Parser.Parser
{
    /// <summary>
    /// Base class for CSS items that can contain children
    /// </summary>
    public abstract class ComplexItem : ParseItem
    {
        public ParseItemList Children { get; private set; }

        /// <summary>
        /// Treat the range of this ParseItem as a single word, e.g. for Edit.SelectCurrentWord/double-click
        /// </summary>
        internal virtual bool TreatAsWord => false;

        protected ComplexItem()
        {
            Children = new ParseItemList
            {
                AutoParent = this
            };
        }

        public override bool IsValid
        {
            get
            {
                int childCount = Children.Count;
                for (int i = 0; i < childCount; i++)
                {
                    ParseItem child = Children[i];

                    if (!child.IsValid)
                    {
                        return false;
                    }
                }

                return base.IsValid;
            }
        }

        internal override bool ContainsParseErrors
        {
            get
            {
                int childCount = Children.Count;
                for (int i = 0; i < childCount; i++)
                {
                    ParseItem child = Children[i];

                    if (child.ContainsParseErrors)
                    {
                        return true;
                    }
                }

                return base.ContainsParseErrors;
            }
        }

        public override int Start
        {
            get { return Children.TextStart; }
        }

        public override int Length
        {
            get { return Children.TextLength; }
        }

        internal bool IsParentOf(ParseItem item)
        {
            ComplexItem parent = item?.Parent;

            while (parent != null && parent != this)
            {
                parent = parent.Parent;
            }

            return parent == this;
        }

        internal ParseItem PreviousChild(ParseItem child)
        {
            int index = (child != null) ? Children.IndexOf(child) : -1;

            if (index > 0)
            {
                return Children[index - 1];
            }

            return null;
        }

        internal ParseItem NextChild(ParseItem child)
        {
            int index = (child != null) ? Children.IndexOf(child) : -1;

            if (index != -1 && index + 1 < Children.Count)
            {
                return Children[index + 1];
            }

            return null;
        }

        internal void InsertChildIntoSubtree(ParseItem child)
        {
            ComplexItem parent = this;

            if (child.Start >= Children.TextStart &&
                child.Start < Children.TextAfterEnd)
            {
                // Might need to be added in one of my children
                parent = ComplexItemFromRange(child.Start, child.Length) ?? this;
            }

            int insertIndex = parent.Children.FindInsertIndex(child.Start, beforeExisting: false);

            if (insertIndex > 0)
            {
                // If the comment is going to be added right after an unclosed item,
                // then put the comment into the unclosed item. It makes the tree look as expected
                // and it helps incremental parsing work when the item does get closed.

                if (parent.Children[insertIndex - 1] is ComplexItem previousChild && previousChild.IsUnclosed)
                {
                    previousChild.InsertChildIntoSubtree(child);

                    return;
                }
            }

            parent.Children.Insert(insertIndex, child);
        }

        /// <summary>
        /// Search my subtree for the deepest item that contains a range of text
        /// </summary>
        public ParseItem ItemFromRange(int start, int length)
        {
            ParseItem item = null;

            if (ContainsRange(start, length))
            {
                item = this;

                // Check if a child of "rootItem" contains the range
                int childIndex = Children.FindInsertIndex(start, beforeExisting: false);

                if (childIndex > 0)
                {
                    // Since the children are sorted by start position, only one child needs to be checked
                    // (the one before the insertion position for "start")
                    ParseItem child = Children[childIndex - 1];

                    if (child.ContainsRange(start, length))
                    {
                        item = (child is ComplexItem complexChild)
                            ? complexChild.ItemFromRange(start, length)
                            : child;
                    }
                }
            }

            return item;
        }

        /// <summary>
        /// Search my subtree for the deepest item that contains a range of text
        /// </summary>
        internal ComplexItem ComplexItemFromRange(int start, int length)
        {
            ParseItem item = ItemFromRange(start, length);
            ComplexItem complexItem = item as ComplexItem;

            if (item != null)
            {
                return complexItem ?? item.Parent;
            }

            return null;
        }

        /// <summary>
        /// Returns the deepest item that starts before a position (starting from this)
        /// </summary>
        public ParseItem ItemBeforePosition(int pos)
        {
            ParseItem item = null;

            if (Start < pos)
            {
                int i = Children.FindInsertIndex(pos, beforeExisting: true) - 1;

                if (i >= 0)
                {
                    item = Children[i];

                    if (item is ComplexItem)
                    {
                        // Recurse to find the deepest item
                        item = ((ComplexItem)item).ItemBeforePosition(pos);
                    }
                }

                if (item == null)
                {
                    item = this;
                }
            }

            return item;
        }

        /// <summary>
        /// Returns the deepest item that starts at or after a position (starting from this)
        /// </summary>
        public ParseItem ItemAfterPosition(int pos)
        {
            ParseItem item = null;

            if (AfterEnd > pos)
            {
                int i = Children.FindInsertIndex(pos, beforeExisting: true);

                // The previous child could be overlapping "pos", so check it
                if (i > 0 && Children[i - 1] is ComplexItem)
                {
                    item = ((ComplexItem)Children[i - 1]).ItemAfterPosition(pos);
                }

                if (item == null && i < Children.Count)
                {
                    item = Children[i];

                    if (item is ComplexItem)
                    {
                        // Recurse to find the deepest item
                        item = ((ComplexItem)item).ItemAfterPosition(pos);
                    }
                }
            }

            if (item == null && Start >= pos)
            {
                item = this;
            }

            return item;
        }

        /// <summary>
        /// Recursively collect tokens that are children of this item.
        /// The tokens are ordered depth-first, so, just like they appear in the CSS document.
        /// </summary>
        internal void CollectTokens(IList<TokenItem> tokens, bool ignoreComments)
        {
            if (ignoreComments && this is Comment)
            {
                return;
            }

            int childCount = Children.Count;
            for (int i = 0; i < childCount; i++)
            {
                ParseItem child = Children[i];
                if (child is ComplexItem)
                {
                    ((ComplexItem)child).CollectTokens(tokens, ignoreComments);
                }
                else if (child is TokenItem)
                {
                    tokens.Add((TokenItem)child);
                }
            }
        }

        internal TokenItem FirstDeepestToken
        {
            get
            {
                if (Children.Count != 0)
                {
                    ParseItem firstChild = Children[0];

                    if (firstChild is ComplexItem)
                    {
                        return ((ComplexItem)firstChild).FirstDeepestToken;
                    }
                    else
                    {
                        return (TokenItem)firstChild;
                    }
                }

                return null;
            }
        }

        internal TokenItem LastDeepestToken
        {
            get
            {
                int childCount = Children.Count;
                if (childCount != 0)
                {
                    ParseItem lastChild = Children[childCount - 1];

                    if (lastChild is ComplexItem)
                    {
                        return ((ComplexItem)lastChild).LastDeepestToken;
                    }
                    else
                    {
                        return (TokenItem)lastChild;
                    }
                }

                return null;
            }
        }

        public override bool Accept(ICssSimpleTreeVisitor visitor)
        {
            VisitItemResult result = visitor.Visit(this);

            if (result == VisitItemResult.Cancel)
            {
                return false;
            }
            else if (result != VisitItemResult.SkipChildren)
            {
                int childCount = Children.Count;
                for (int i = 0; i < childCount; i++)
                {
                    ParseItem pi = Children[i];
                    if (!pi.Accept(visitor))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
