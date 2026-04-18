// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Diagnostics;
using Microsoft.Css.Parser.Classify;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;

namespace Microsoft.Css.Parser.TreeItems.Comments
{
    // http://www.w3.org/TR/CSS21/syndata.html#comments
    // CSS also allows the SGML comment delimiters ("<!--" and "-->") in certain places defined by the grammar,
    // but they do not delimit CSS comments. They are permitted so that style rules appearing in an HTML source
    // document (in the STYLE element) may be hidden from pre-HTML 3.2 user agents.

    internal sealed class HtmlComment : Comment
    {
        public HtmlComment()
        {
        }

        // Just holds onto either "<!--" or "-->" which are ignored in the stylesheet
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public override bool Parse(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            Debug.Assert(
                tokens.CurrentToken.TokenType == CssTokenType.OpenHtmlComment ||
                tokens.CurrentToken.TokenType == CssTokenType.CloseHtmlComment);

            Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.Default);

            return Children.Count > 0;
        }
    }
}
