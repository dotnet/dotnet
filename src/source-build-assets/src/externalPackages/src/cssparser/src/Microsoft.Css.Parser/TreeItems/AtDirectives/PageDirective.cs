// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Classify;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;

namespace Microsoft.Css.Parser.TreeItems.AtDirectives
{
    /// <summary>
    /// Page directive (@page)
    /// </summary>
    internal sealed class PageDirective : AtBlockDirective
    {
        // In CSS3 page directive allows new @margin directives inside, namely
        // http://www.w3.org/TR/2006/WD-css3-page-20061010/#syntax-page-selector

        // TOPLEFTCORNER_SYM     ::= "@top-left-corner"
        // TOPLEFT_SYM           ::= "@top-left"
        // TOPCENTER_SYM         ::= "@top-center"
        // TOPRIGHT_SYM          ::= "@top-right"
        // TOPRIGHTCORNER_SYM    ::= "@top-right-corner"
        // BOTTOMLEFTCORNER_SYM  ::= "@bottom-left-corner"
        // BOTTOMLEFT_SYM        ::= "@bottom-left"
        // BOTTOMCENTER_SYM      ::= "@bottom-center"
        // BOTTOMRIGHT_SYM       ::= "@bottom-right"
        // BOTTOMRIGHTCORNER_SYM ::= "@bottom-right-corner"
        // LEFTTOP_SYM           ::= "@left-top"
        // LEFTMIDDLE_SYM        ::= "@left-middle"
        // LEFTBOTTOM_SYM        ::= "@right-bottom"
        // RIGHTTOP_SYM          ::= "@right-top"
        // RIGHTMIDDLE_SYM       ::= "@right-middle"
        // RIGHTBOTTOM_SYM       ::= "@right-bottom"

        // page :  PAGE_SYM S* IDENT? pseudo_page? S*  '{' S* [ declaration | margin ]? [ ';' S* [ declaration | margin ]? ]* '}' S*
        // pseudo_page : ':' [ "left" | "right" | "first" ]
        // margin : margin_sym S* '{' declaration [ ';' S* declaration? ]* '}' S*
        // margin_sym :
        //       TOPLEFTCORNER_SYM |
        //       TOPLEFT_SYM |
        //       TOPCENTER_SYM |
        //       TOPRIGHT_SYM |
        //       TOPRIGHTCORNER_SYM |
        //       BOTTOMLEFTCORNER_SYM |
        //       BOTTOMLEFT_SYM |
        //       BOTTOMCENTER_SYM |
        //       BOTTOMRIGHT_SYM |
        //       BOTTOMRIGHTCORNER_SYM |
        //       LEFTTOP_SYM |
        //       LEFTMIDDLE_SYM |
        //       LEFTBOTTOM_SYM |
        //       RIGHTTOP_SYM |
        //       RIGHTMIDDLE_SYM |
        //       RIGHTBOTTOM_SYM
        //
        // @page {
        //    size: auto;/* auto is the initial value */
        //    margin: 10%;
        // }
        // @page :left { ... }
        // @page :right { ... }
        // @page LandscapeTable { ... }
        // @page CompanyLetterHead:first { ... } /*  identifier and pseudo page. */
        // @page:first { ... };
        // @page {
        //  @top-left { ... /* document name */ }
        //  @bottom-center { ... /* page number */}
        // }

        internal PageDirectiveBlock PageDirectiveBlock { get; private set; }
        internal TokenItem Identifier { get; private set; }
        internal TokenItem Colon { get; private set; }
        internal TokenItem PseudoPage { get; private set; }

        public PageDirective()
        {
        }

        internal override BlockItem Block
        {
            get { return PageDirectiveBlock; }
        }

        public override bool IsValid
        {
            get
            {
                if (Colon != null && PseudoPage == null)
                {
                    return false;
                }

                return base.IsValid;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public override bool Parse(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            ParseAtAndKeyword(itemFactory, text, tokens);

            // page : PAGE_SYM S* IDENT? pseudo_page? S* '{' S* [ declaration | margin ]? [ ';' S* [ declaration | margin ]? ]* '}' S*
            // pseudo_page : ':' [ "left" | "right" | "first" ]

            if (tokens.CurrentToken.TokenType == CssTokenType.Identifier)
            {
                Identifier = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.PseudoPageType);
            }

            if (tokens.CurrentToken.TokenType == CssTokenType.Colon)
            {
                Colon = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.PseudoPageType);

                if (tokens.CurrentToken.TokenType == CssTokenType.Identifier &&
                    tokens.CurrentToken.Start == Colon.AfterEnd)
                {
                    PseudoPage = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.PseudoPageType);
                }
                else
                {
                    Children.AddParseError(ParseErrorType.PseudoPageIdentifierMissing);
                }
            }

            PageDirectiveBlock = new PageDirectiveBlock();

            if (!ParseBlock(PageDirectiveBlock, itemFactory, text, tokens))
            {
                PageDirectiveBlock = null;
            }

            return Children.Count > 0;
        }
    }
}
