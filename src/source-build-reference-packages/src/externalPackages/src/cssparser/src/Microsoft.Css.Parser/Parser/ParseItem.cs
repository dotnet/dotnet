// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Css.Parser.Classify;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems;
using Microsoft.Css.Parser.Utilities;

namespace Microsoft.Css.Parser.Parser
{
    /// <summary>
    /// Base class for any item in the CSS tree
    /// </summary>
    public abstract class ParseItem : IRange
    {
        private IClassifierContext _classifierContext;
        private IList<ParseError> _parseErrors;

        protected ParseItem()
        {
        }

        // These are pure-virtual functions that must be implemented in derived classes:

        public abstract bool Parse(ItemFactory itemFactory, ITextProvider text, TokenStream tokens);
        public abstract int Start { get; }
        public abstract int Length { get; }

        /// <summary>
        /// Derived classes can decide if they are valid or not
        /// </summary>
        public virtual bool IsValid
        {
            get { return !HasParseErrors; }
        }

        /// <summary>
        /// Is this item missing the token or character that closes its scope?
        /// (like a rule with a missing end curly brace, or string with a missing quote)
        /// </summary>
        internal virtual bool IsUnclosed
        {
            get { return false; }
        }

        public int AfterEnd
        {
            get { return Start + Length; }
        }

        public ComplexItem Parent { get; set; }

        public ParseItem PreviousSibling
        {
            get
            {
                return Parent?.PreviousChild(this);
            }
        }

        public ParseItem NextSibling
        {
            get
            {
                return Parent?.NextChild(this);
            }
        }

        /// <summary>
        /// Does this item contain a specific single character position?
        /// </summary>
        internal bool ContainsChar(int charStart)
        {
            return charStart >= Start && charStart < AfterEnd;
        }

        /// <summary>
        /// Does this item fully contain a range of characters?
        /// Watch out for zero-length ranges at the start or end, they
        /// must return false.
        /// </summary>
        internal bool ContainsRange(int start, int length)
        {
            // Is the range completely outside of my bounds?
            if (start + length <= Start || start >= AfterEnd)
            {
                return false;
            }

            // Is the range completely inside my bounds?
            if (Start <= start && AfterEnd >= start + length)
            {
                return true;
            }

            return false;
        }

        public StyleSheet StyleSheet
        {
            get
            {
                // Look up the parent chain for a StyleSheet

                ParseItem parseItem = this;

                while (parseItem.Parent != null)
                {
                    parseItem = parseItem.Parent;
                }

                StyleSheet styleSheet = parseItem as StyleSheet;
                return styleSheet;
            }
        }

        /// <summary>
        /// The root stylesheet is depth 0, its children are 1, etc...
        /// </summary>
        internal int TreeDepth
        {
            get
            {
                int depth = 0;
                for (ParseItem parseItem = this; parseItem.Parent != null; parseItem = parseItem.Parent)
                {
                    depth++;
                }

                return depth;
            }
        }

        /// <summary>
        /// Looks here and up the parent chain for a certain type of tree item
        /// </summary>
        public T FindType<T>() where T : ParseItem
        {
            for (ParseItem parseItem = this; parseItem != null; parseItem = parseItem.Parent)
            {
                if (typeof(T).IsAssignableFrom(parseItem.GetType()))
                {
                    return parseItem as T;
                }
            }

            return null;
        }

        internal IClassifierContext Context
        {
            get
            {
                if (_classifierContext == null)
                {
                    // Look up the parent chain for a context
                    return (Parent == null)
                        ? CssClassifierContextCache.FromTypeEnum(CssClassifierContextType.Default)
                        : Parent.Context;
                }

                return _classifierContext;
            }

            set
            {
                _classifierContext = value;
            }
        }

        public string Text
        {
            get
            {
                StyleSheet styleSheet = StyleSheet;

                return styleSheet != null
                    ? styleSheet.TextProvider.GetText(Start, Length)
                    : string.Empty;
            }
        }

        /// <summary>
        /// Does this item directly have parse errors?
        /// </summary>
        public bool HasParseErrors
        {
            get { return _parseErrors != null && _parseErrors.Count > 0; }
        }

        /// <summary>
        /// Does this item or children have parse errors?
        /// </summary>
        internal virtual bool ContainsParseErrors
        {
            get { return HasParseErrors; }
        }

        internal IList<ParseError> ParseErrors
        {
            get { return _parseErrors ?? Array.Empty<ParseError>(); }
        }

        internal bool AddParseError(ParseErrorType type, ParseErrorLocation location)
        {
            return AddParseError(type, location, null);
        }

        internal bool AddParseError(ParseErrorType type, ParseErrorLocation location, string customText)
        {
            if (_parseErrors == null)
            {
                _parseErrors = new List<ParseError>();
            }
            else
            {
                // Check if it was already added

                for (int i = 0; i < _parseErrors.Count; i++)
                {
                    if (_parseErrors[i].ErrorType == type && _parseErrors[i].Location == location)
                    {
                        return false;
                    }
                }
            }

            _parseErrors.Add(new ParseError(type, location, customText));

            return true;
        }

        internal bool RemoveParseError(ParseErrorType type)
        {
            bool removed = false;

            if (_parseErrors != null)
            {
                for (int i = _parseErrors.Count - 1; i >= 0; i--)
                {
                    if (_parseErrors[i].ErrorType == type)
                    {
                        _parseErrors.RemoveAt(i);
                        removed = true;
                    }
                }
            }

            return removed;
        }

        /// <summary>
        /// ICssTreeVisitorPattern. Override to provide custom item traversal.
        /// </summary>
        public virtual bool Accept(ICssSimpleTreeVisitor visitor)
        {
            VisitItemResult result = visitor.Visit(this);

            return (result != VisitItemResult.Cancel);
        }

#if DEBUG
        internal string DebugTreeDump
        {
            get
            {
                ITextProvider textProvider = StyleSheet?.TextProvider;
                string text = string.Empty;

                if (textProvider != null)
                {
                    text = new DebugWriter().Serialize(textProvider, this);
                }

                return text;
            }
        }
#endif
    }
}
