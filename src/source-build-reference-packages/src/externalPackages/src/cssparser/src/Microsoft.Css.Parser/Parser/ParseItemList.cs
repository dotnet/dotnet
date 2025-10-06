// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Css.Parser.Classify;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems;
using Microsoft.Css.Parser.Utilities;

namespace Microsoft.Css.Parser.Parser
{
    /// <summary>
    /// A sorted collection of parse items (sorted by start position within the text)
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "Public API surface")]
    public class ParseItemList : SortedRangeList<ParseItem>
    {
        private ComplexItem _parent;

        internal ParseItemList()
        {
        }

        internal ParseItemList(int capacity)
            : base(capacity)
        {
        }

        /// <summary>
        /// The start of the first child
        /// </summary>
        public int TextStart
        {
            get { return (Count != 0) ? this[0].Start : 0; }
        }

        /// <summary>
        /// The distance from the start of the first child to after the end of the last child
        /// </summary>
        public int TextLength
        {
            get { return TextAfterEnd - TextStart; }
        }

        /// <summary>
        /// The position after the end of the last child
        /// </summary>
        public int TextAfterEnd
        {
            get { return (Count != 0) ? this[Count - 1].AfterEnd : 0; }
        }

        /// <summary>
        /// Set this to automatically set the parent of inserted items
        /// </summary>
        internal ComplexItem AutoParent
        {
            get
            {
                return _parent;
            }

            set
            {
                _parent = value;

                int count = Count;
                for (int i = 0; i < count; i++)
                {
                    ParseItem item = this[i];
                    item.Parent = _parent;
                }
            }
        }

        /// <summary>
        /// Automatically sets the parent of inserted items
        /// </summary>
        protected override void OnAdded(int index)
        {
            if (_parent != null)
            {
                this[index].Parent = _parent;
            }

            base.OnAdded(index);
        }

        #region Parsing Helpers

        internal ParseItem AddItem(ParseItem parseItem, IClassifierContext context)
        {
            parseItem.Context = context;
            Add(parseItem);

            return parseItem;
        }

        internal TokenItem AddCurrentAndAdvance(TokenStream tokens, IClassifierContext context)
        {
            TokenItem item = new TokenItem(tokens.AdvanceToken(), context);
            Add(item);

            return item;
        }

        internal TokenItem AddCurrentAndAdvance(TokenStream tokens, CssClassifierContextType ct)
        {
            return AddCurrentAndAdvance(tokens, CssClassifierContextCache.FromTypeEnum(ct));
        }

        internal TokenItem AddCurrentAndAdvance(TokenStream tokens)
        {
            return AddCurrentAndAdvance(tokens, null);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "I don't care")]
        internal ParseItem AddUnknownAndAdvance(ItemFactory itemFactory, ITextProvider text, TokenStream tokens, ParseErrorType? errorType = null)
        {
            ParseItem item = UnknownItem.ParseUnknown(_parent, itemFactory, text, tokens, errorType);
            Debug.Assert(item != null);
            Add(item);

            return item;
        }

        /// <summary>
        /// Adds an error at the current parse location - usually after the last
        /// child. When there are no children, the error goes before any future children.
        /// </summary>
        internal void AddParseError(ParseErrorType errorType)
        {
            if (Count > 0)
            {
                this[Count - 1].AddParseError(errorType, ParseErrorLocation.AfterItem);
            }
            else if (_parent != null)
            {
                _parent.AddParseError(errorType, ParseErrorLocation.BeforeItem);
            }
            else
            {
                Debug.Fail("Can't add this parse error anywhere");
            }
        }

        #endregion
    }
}
