// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Globalization;
using Microsoft.Css.Parser.Classify;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems.AtDirectives;
using Microsoft.Css.Parser.TreeItems.Comments;
using Microsoft.Css.Parser.Utilities;

namespace Microsoft.Css.Parser.TreeItems
{
    // Classes are defined similar to BNF grammar here
    // http://www.w3.org/TR/2007/CR-CSS21-20070719/grammar.html

    // styleheet:
    // [ CHARSET_SYM STRING ';' ]?
    // [S|CDO|CDC]* [ import [ CDO S* | CDC S* ]* ]*
    // [ [ ruleset | media | page ] [ CDO S* | CDC S* ]* ]*

    public class StyleSheet : BlockItem, IIncrementalParseItem
    {
        internal SortedRangeList<RuleSet> RuleSets { get; private set; }
        internal SortedRangeList<Declaration> CustomProperties { get; private set; }
        public ITextProvider TextProvider { get; set; }
        internal bool IsNestedBlock { get; set; }

        public StyleSheet()
        {
            RuleSets = new SortedRangeList<RuleSet>();
            CustomProperties = new SortedRangeList<Declaration>();
        }

        internal override bool IsUnclosed
        {
            get
            {
                if (!IsNestedBlock)
                {
                    // The root stylesheet can't be "unclosed"
                    return false;
                }

                return base.IsUnclosed;
            }
        }

        public override bool Parse(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            if (!IsNestedBlock)
            {
                TextProvider = text;
            }

            return IncrementalParseHelper.FullParseIncrementalItem(this, itemFactory, text, tokens);
        }

        public virtual ParseItem CreateNextChild(ParseItem previousChild, ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            if (IsNestedBlock)
            {
                if (tokens.CurrentToken.IsScopeBlocker())
                {
                    return null;
                }

                // Only nested stylesheets (like in @media) should look for braces

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

                if (tokens.CurrentToken.TokenType == CssTokenType.OpenCurlyBrace)
                {
                    return (previousChild == null) ? new TokenItem(tokens.AdvanceToken(), CssClassifierContextType.CurlyBrace) : null;
                }

                if (tokens.CurrentToken.TokenType == CssTokenType.CloseCurlyBrace)
                {
                    return new TokenItem(tokens.AdvanceToken(), CssClassifierContextType.CurlyBrace);
                }
            }

            ParseItem newChild = null;

            switch (tokens.CurrentToken.TokenType)
            {
                case CssTokenType.EndOfFile:
                    break;

                case CssTokenType.ScopeBlocker:
                    newChild = UnknownItem.ParseUnknown(this, itemFactory, text, tokens);
                    break;

                case CssTokenType.At:
                    newChild = AtDirective.ParseDirective(this, itemFactory, text, tokens);
                    break;

                default:
                    newChild = ParseDefaultChild(itemFactory, text, tokens);
                    break;
            }

            return newChild;
        }

        protected virtual ParseItem ParseDefaultChild(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            ParseItem newChild = itemFactory.Create<RuleSet>(this);

            if (!newChild.Parse(itemFactory, text, tokens))
            {
                newChild = UnknownItem.ParseUnknown(this, itemFactory, text, tokens, ParseErrorType.UnexpectedToken);
            }

            return newChild;
        }

        /// <summary>
        /// Always called after parsing is done (full or incremental)
        /// </summary>
        public override void UpdateCachedChildren()
        {
            RuleSets.Clear();
            CustomProperties.Clear();

            foreach (ParseItem child in Children)
            {
                if (child is RuleSet)
                {
                    RuleSets.Add((RuleSet)child);

                    if (((RuleSet)child).Block != null)
                    {
                        IEnumerable<Declaration> childCustomDecls = ((RuleSet)child).Block.CustomProperties;
                        CustomProperties.AddRange(childCustomDecls);
                    }
                }
            }

            // Don't cache the curly braces for the root stylesheet
            if (IsNestedBlock)
            {
                base.UpdateCachedChildren();
            }
        }

        internal virtual bool IsValidBeforeFirstImport(ParseItem item)
        {
            // @imports have to be at the top (ignoring comments and @charset).

            return item is Comment || item is CharsetDirective;
        }

        /// <summary>
        /// This is for finding @imports that are only valid in the editor
        /// </summary>
        internal virtual bool IsReferenceComment(ParseItem item, out string importPath)
        {
            importPath = string.Empty;
            return false;
        }

        /// <summary>
        /// import can be null
        /// </summary>
        internal virtual IEnumerable<string> TransformImportFile(ParseItem originalFileItem, string fileName)
        {
            return new string[] { fileName };
        }

        internal virtual string CreateUrlToken(string url)
        {
            // Don't use a quote that is already in the URL
            char quote = '\'';
            if (url.Contains("'"))
            {
                quote = url.Contains("\"")
                    ? '\0' // no quote will work
                    : '"';
            }

            return string.Format(CultureInfo.InvariantCulture, "url({0}{1}{0})",
                quote == '\0' ? string.Empty : quote.ToString(CultureInfo.InvariantCulture), // {0}
                url); // {1}
        }

        internal virtual string CreateImportDirective(string url)
        {
            return string.Format(CultureInfo.InvariantCulture, "@import {0};", CreateUrlToken(url));
        }
    }
}
