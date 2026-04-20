// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.Utilities;

namespace Microsoft.Css.Parser.TreeItems.AtDirectives
{
    // CSS3 : http://www.w3.org/TR/css3-mediaqueries/
    // media : MEDIA_SYM S* media_list LBRACE S* ruleset* '}' S*
    //
    //  @media screen {
    //    * { font-family: sans-serif }
    //  }
    //
    // @media screen and (color), projection and (color) { ... }
    // @media all and (orientation: portrait) { ... }
    // @media (orientation: portrait) { ... }
    // @media print and (width: 21cm) and (height: 29.7cm) {
    //       @page { margin: 3cm; }
    //  }
    //
    // Interestingly enough, CSS2 grammar does not allow @page inside @media
    // since it only allows rulesets in BNF. CSS3 media spec does not mention
    // @page either. However, in page spec http://www.w3.org/TR/2006/WD-css3-page-20061010/
    // there are samples with @page inside @media, like above. Thus we will be parsing
    // media block as stylesheet and leave it to validator to show any errors.
    //
    // media_query: [ONLY | NOT]? S* media_type S* [ AND S* expression ]* | expression [ AND S* expression ]
    // media_type: IDENT
    // expression: '(' S* media_feature S* [':' S* expr]? ')' S*
    // media_feature: IDENT

    public class MediaDirective : AtBlockDirective
    {
        // remaining tokens until semicolon or next block, typically IDENTIFIER - COMMA - WHITESPACE
        public SortedRangeList<MediaQuery> MediaQueries { get; private set; }
        public StyleSheet MediaBlock { get; protected set; }

        public MediaDirective()
        {
            MediaQueries = new SortedRangeList<MediaQuery>();
        }

        internal override BlockItem Block
        {
            get { return MediaBlock; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public override bool Parse(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            ParseAtAndKeyword(itemFactory, text, tokens);

            // http://www.w3.org/TR/2009/CR-css3-mediaqueries-20090915/
            // media_query_list: : S* media_query [ ',' S* media_query]*
            //
            // Several Media Queries can be combined in a comma-separated list.
            // If one or more of the Media Queries in the comma-separated list is true,
            // the associated style sheet is applied, otherwise the associated style sheet
            // is ignored. Consider this example written in CSS:
            // @media screen and (color), projection and (color) { ... }

            while (!tokens.CurrentToken.IsDirectiveTerminator())
            {
                if (!ParseMediaQuery(itemFactory, text, tokens))
                {
                    // Don't know what this is, but I need to eat it and keep looking for the terminator
                    Children.AddUnknownAndAdvance(itemFactory, text, tokens, ParseErrorType.UnexpectedParseError);
                }
            }

            MediaBlock = itemFactory.CreateSpecific<StyleSheet>(this);
            MediaBlock.IsNestedBlock = true;

            if (!ParseBlock(MediaBlock, itemFactory, text, tokens))
            {
                MediaBlock = null;
            }

            return Children.Count > 0;
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
