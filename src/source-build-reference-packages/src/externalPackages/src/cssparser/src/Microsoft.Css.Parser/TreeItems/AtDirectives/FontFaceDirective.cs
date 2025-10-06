// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;

namespace Microsoft.Css.Parser.TreeItems.AtDirectives
{
    // http://www.w3.org/TR/css3-webfonts/#the-font-face-rule
    // http://www.w3.org/TR/2002/WD-css3-webfonts-20020802/#font-descriptions
    //
    // The general form of an @font-face at-rule is:
    //
    //  @font-face { <font-description> }
    //
    // where <font-description> has the form:
    //
    //  descriptor: value;
    //  descriptor: value;
    //  [...]
    //  descriptor: value;

    // @font-face {  font-family: Headline; src: local(Futura-Medium), url(fonts.svg#MyGeometricModern) format("svg"); }

    internal sealed class FontFaceDirective : AtBlockDirective
    {
        internal RuleBlock RuleBlock { get; private set; }

        public FontFaceDirective()
        {
        }

        internal override BlockItem Block
        {
            get { return RuleBlock; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public override bool Parse(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            ParseAtAndKeyword(itemFactory, text, tokens);

            RuleBlock = itemFactory.CreateSpecific<RuleBlock>(this);

            if (!ParseBlock(RuleBlock, itemFactory, text, tokens))
            {
                RuleBlock = null;
            }

            return Children.Count > 0;
        }
    }
}
