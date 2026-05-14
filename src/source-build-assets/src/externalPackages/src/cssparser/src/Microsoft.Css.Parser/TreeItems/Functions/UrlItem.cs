// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Classify;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;

namespace Microsoft.Css.Parser.TreeItems.Functions
{
    public class UrlItem : ComplexItem
    {
        internal TokenItem FunctionName { get; private set; }
        public ParseItem UrlString { get; private set; }
        internal TokenItem CloseFunctionBrace { get; private set; }

        public UrlItem()
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public override bool Parse(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            FunctionName = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.UrlFunction); // url(

            if (tokens.CurrentToken.IsUrlString())
            {
                UrlString = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.UrlString);
            }

            if (tokens.CurrentToken.TokenType == CssTokenType.CloseFunctionBrace)
            {
                CloseFunctionBrace = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.UrlFunction);
            }
            else
            {
                FunctionName.AddParseError(ParseErrorType.CloseFunctionBraceMissing, ParseErrorLocation.AfterItem);
            }

            return Children.Count > 0;
        }
    }
}
