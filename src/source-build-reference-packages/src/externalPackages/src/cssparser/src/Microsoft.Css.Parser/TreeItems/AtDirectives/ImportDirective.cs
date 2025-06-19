// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.Css.Parser.Classify;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems.Functions;
using Microsoft.Css.Parser.Utilities;

namespace Microsoft.Css.Parser.TreeItems.AtDirectives
{
    internal class ImportDirective : AtLineDirective
    {
        // import : IMPORT_SYM S* [STRING|URI] S* media_list? ';' S*
        // media : MEDIA_SYM S* media_list LBRACE S* ruleset* '}' S*
        // media_list : medium [ COMMA S* medium]*
        // medium : IDENT S*

        internal SortedRangeList<MediaQuery> MediaQueries { get; private set; }
        internal SortedRangeList<ParseItem> FileNames { get; private set; }

        public ImportDirective()
        {
            MediaQueries = new SortedRangeList<MediaQuery>();
            FileNames = new SortedRangeList<ParseItem>();
        }

        /// <summary>
        /// Can returns multiple interpretations of the fileName.
        /// For example, SASS will transform "foo" into either "_foo.scss" or "foo.scss"
        /// </summary>
        internal virtual IEnumerable<string> TransformImportFile(ParseItem originalFileItem, string fileName)
        {
            return StyleSheet.TransformImportFile(originalFileItem, fileName);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public override bool Parse(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            // @import "subs.css";
            // @import "print-main.css" print;
            // @import url("bluish.css") projection, tv;
            // @import url(color.css) screen and (color); <<- logical expression (media query)

            ParseAtAndKeyword(itemFactory, text, tokens);
            ParseBeforeUrl(itemFactory, text, tokens);
            ParseUrl(itemFactory, text, tokens);
            ParseAfterUrl(itemFactory, text, tokens);
            CheckSemicolon(tokens);

            return Children.Count > 0;
        }

        protected virtual void ParseBeforeUrl(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            // Standard CSS doesn't have anything before the URL
        }

        protected virtual void ParseUrl(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            if (tokens.CurrentToken.IsString())
            {
                FileNames.Add(Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.ImportUrl));
            }
            else if (tokens.CurrentToken.TokenType == CssTokenType.Url)
            {
                UrlItem url = new UrlItem
                {
                    Context = CssClassifierContextCache.FromTypeEnum(CssClassifierContextType.ImportUrl)
                };

                if (url.Parse(itemFactory, text, tokens))
                {
                    FileNames.Add(url);
                    Children.Add(url);
                }
            }
            else
            {
                Children.AddParseError(ParseErrorType.UrlImportMissing);
            }
        }

        protected virtual void ParseAfterUrl(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            // Parse media queries

            while (!tokens.CurrentToken.IsDirectiveTerminator())
            {
                if (!ParseMediaQuery(itemFactory, text, tokens))
                {
                    break;
                }
            }
        }

        protected virtual bool ParseMediaQuery(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            MediaQuery mq = itemFactory.CreateSpecific<MediaQuery>(this);

            if (mq.Parse(itemFactory, text, tokens))
            {
                MediaQueries.Add(mq);
                Children.Add(mq);
                return true;
            }

            return false;
        }
    }
}
