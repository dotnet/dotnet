// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems;
using Microsoft.Css.Parser.TreeItems.Comments;

namespace Microsoft.Css.Parser.Parser
{
    /// <summary>
    /// CSS parser interface. CSS parser is thread-safe.
    /// </summary>
    public interface ICssParser
    {
        /// <summary>
        /// Parse CSS in a string
        /// </summary>
        /// <param name="text">Text to parse</param>
        /// <param name="insertComments">True stylesheet should include comment items</param>
        /// <returns>Stylesheet object</returns>
        StyleSheet Parse(string text, bool insertComments);

        /// <summary>
        /// Parse CSS from a text provider
        /// </summary>
        /// <param name="text">Text provider that supplies text</param>
        /// <param name="insertComments">True stylesheet should include comment items</param>
        /// <returns>Stylesheet object</returns>
        StyleSheet Parse(ITextProvider text, bool insertComments);

        /// <summary>
        /// Parse CSS from a pre-created stream of tokens
        /// </summary>
        /// <param name="text">Text provider that supplies text</param>
        /// <param name="tokens">Tokens that match the supplied text</param>
        /// <param name="insertComments">True stylesheet should include comment items</param>
        /// <returns>Stylesheet object</returns>
        StyleSheet Parse(ITextProvider text, TokenList tokens, bool insertComments);

        /// <summary>
        /// Extracts comment items from the supplied token collection
        /// </summary>
        /// <param name="textProvider">Text provider</param>
        /// <param name="tokens">Token array that matches the supplied text</param>
        /// <param name="startToken">Index into the token array at which parser should start</param>
        /// <param name="tokenCount">Number of tokens to process</param>
        /// <returns></returns>
        IList<Comment> ExtractComments(ITextProvider textProvider, TokenList tokens, int startToken, int tokenCount);

        ICssTokenizerFactory TokenizerFactory { get; }

        /// <summary>
        /// Returns external item factory, if any.
        /// </summary>
        ICssItemFactory ExternalItemFactory { get; }

        // Only used after a parse

        /// <summary>
        /// How much time was spent in a tokenization phase
        /// </summary>
        int LastTokenizeMilliseconds { get; }

        /// <summary>
        /// How much time was spent in a parsing phase
        /// </summary>
        int LastParseMilliseconds { get; }
    }

    /// <summary>
    /// Main CSS parser class
    /// </summary>
    internal sealed class CssParser : ICssParser
    {
        internal CssParser()
            : this(null, null)
        {
        }

        internal CssParser(ICssItemFactory itemFactory)
            : this(null, itemFactory)
        {
        }

        /// <summary>
        /// CSS parser constructor
        /// </summary>
        /// <param name="itemFactory">Item factory <seealso cref=""/>. Null cause parser use
        /// default item factory. You can provide different factory if you are extending CSS
        /// parser, such as when implementing LESS CSS support.</param>
        internal CssParser(ICssTokenizerFactory tokenizerFactory, ICssItemFactory itemFactory)
        {
            TokenizerFactory = tokenizerFactory ?? new DefaultTokenizerFactory();
            ExternalItemFactory = itemFactory;
        }

        public ICssTokenizerFactory TokenizerFactory { get; }

        public ICssItemFactory ExternalItemFactory { get; }

        /// <summary>
        /// Parse CSS in a string
        /// </summary>
        /// <param name="text">Text to parse</param>
        /// <param name="insertComments">True stylesheet should include comment items</param>
        /// <returns>Stylesheet object</returns>
        public StyleSheet Parse(string text, bool insertComments)
        {
            return Parse(new StringTextProvider(text ?? string.Empty), insertComments);
        }

        /// <summary>
        /// Parse CSS from a text provider
        /// </summary>
        /// <param name="text">Text provider that supplies text</param>
        /// <param name="insertComments">True stylesheet should include comment items</param>
        /// <returns>Stylesheet object</returns>
        public StyleSheet Parse(ITextProvider text, bool insertComments)
        {
            DateTime startTime = DateTime.UtcNow;

            ICssTokenizer tokenizer = CreateTokenizer();
            ITextProvider textProvider = text ?? new StringTextProvider(string.Empty);
            TokenList tokens = tokenizer.Tokenize(textProvider, 0, textProvider.Length, keepWhiteSpace: false);

            LastTokenizeMilliseconds = (int)(DateTime.UtcNow - startTime).TotalMilliseconds;

            return Parse(text, tokens, insertComments);
        }

        /// <summary>
        /// Parse CSS from a pre-created stream of tokens
        /// </summary>
        /// <param name="text">Text provider that supplies text</param>
        /// <param name="tokens">Tokens that match the supplied text</param>
        /// <param name="insertComments">True stylesheet should include comment items</param>
        /// <returns>Stylesheet object</returns>
        public StyleSheet Parse(ITextProvider text, TokenList tokens, bool insertComments)
        {
            if (text == null || tokens == null)
            {
                return null;
            }

            DateTime startTime = DateTime.UtcNow;
            IList<Comment> comments = ExtractComments(text, tokens, 0, tokens.Count);

            TokenStream tokenStream = new TokenStream(tokens)
            {
                SkipComments = true
            };

            ItemFactory itemFactory = new ItemFactory(ExternalItemFactory, text, tokenStream);
            StyleSheet styleSheet = itemFactory.CreateSpecific<StyleSheet>(null);
            styleSheet.Parse(itemFactory, text, tokenStream);

            if (insertComments)
            {
                foreach (ParseItem comment in comments)
                {
                    styleSheet.InsertChildIntoSubtree(comment);
                }
            }

            LastParseMilliseconds = (int)(DateTime.UtcNow - startTime).TotalMilliseconds;

            return styleSheet;
        }

        internal InlineStyle ParseInlineStyle(string cssText, bool insertComments)
        {
            return ParseInlineStyle(new StringTextProvider(cssText ?? string.Empty), insertComments);
        }

        internal InlineStyle ParseInlineStyle(ITextProvider text, bool insertComments)
        {
            InlineStyle inlineStyle = new InlineStyle();

            ICssTokenizer tokenizer = CreateTokenizer();
            ITextProvider textProvider = text ?? new StringTextProvider(string.Empty);
            TokenList tokens = tokenizer.Tokenize(textProvider, 0, textProvider.Length, keepWhiteSpace: false);

            IList<Comment> comments = ExtractComments(text, tokens, 0, tokens.Count);
            TokenStream tokenStream = new TokenStream(tokens)
            {
                SkipComments = true
            };

            ItemFactory itemFactory = new ItemFactory(ExternalItemFactory, textProvider, tokenStream);

            if (!inlineStyle.Parse(itemFactory, textProvider, tokenStream))
            {
                inlineStyle = null;
            }
            else
            {
                if (insertComments)
                {
                    foreach (ParseItem comment in comments)
                    {
                        inlineStyle.InsertChildIntoSubtree(comment);
                    }
                }

                // There must be a StyleSheet root object, so create one
                StyleSheet styleSheet = new StyleSheet
                {
                    TextProvider = textProvider
                };

                styleSheet.Children.Add(inlineStyle);
            }

            return inlineStyle;
        }

        /// <summary>
        /// How much time was spent in a tokenization phase
        /// </summary>
        public int LastTokenizeMilliseconds { get; private set; }

        /// <summary>
        /// How much time was spent in a parsing phase
        /// </summary>
        public int LastParseMilliseconds { get; private set; }

        /// <summary>
        /// Extracts comment items from the supplied token collection
        /// </summary>
        /// <param name="textProvider">Text provider</param>
        /// <param name="tokens">Token array that matches the supplied text</param>
        /// <param name="startToken">Index into the token array at which parser should start</param>
        /// <param name="tokenCount">Number of tokens to process</param>
        /// <returns></returns>
        public IList<Comment> ExtractComments(
            ITextProvider text,
            TokenList tokens,
            int startToken,
            int tokenCount)
        {
            TokenStream tokenIter = new TokenStream(tokens);
            ItemFactory itemFactory = new ItemFactory(ExternalItemFactory, text, tokenIter);
            IList<Comment> comments = new List<Comment>();

            for (tokenIter.Position = startToken;
                tokenIter.Position < startToken + tokenCount && tokenIter.Position < tokens.Count;)
            {
                CssToken token = tokenIter.CurrentToken;
                Comment comment = null;

                switch (token.TokenType)
                {
                    case CssTokenType.OpenHtmlComment:
                    case CssTokenType.CloseHtmlComment:
                        comment = itemFactory.CreateSpecific<HtmlComment>(null);
                        break;

                    case CssTokenType.OpenCComment:
                        comment = itemFactory.CreateSpecific<CComment>(null);
                        break;

                    case CssTokenType.CommentText:
                    case CssTokenType.SingleTokenComment:
                    case CssTokenType.SingleLineComment:
                        comment = itemFactory.CreateSpecific<CommentTokenItem>(null);
                        break;

                    default:
                        tokenIter.AdvanceToken();
                        break;
                }

                if (comment != null && comment.Parse(itemFactory, text, tokenIter))
                {
                    comments.Add(comment);
                }
            }

            return comments;
        }

        private ICssTokenizer CreateTokenizer()
        {
            return TokenizerFactory.CreateTokenizer() ?? new CssTokenizer();
        }
    }
}
