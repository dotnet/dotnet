// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Diagnostics;
using Microsoft.Css.Parser.Classify;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems.Functions;

namespace Microsoft.Css.Parser.TreeItems
{
    internal static class UnknownItem
    {
        /// <summary>
        /// This function must not fail. It has to advance the token iterator and return an item.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public static ParseItem ParseUnknown(
            ComplexItem parent,
            ItemFactory itemFactory,
            ITextProvider text,
            TokenStream tokens,
            ParseErrorType? errorType = null)
        {
            ParseItem pi = null;
            bool alreadyParsed = false;

            // For a single unknown token, let this switch fall through where a
            // ParseErrorItem will get created. For multiple unknown tokens, deal with
            // them in this switch and let them automatically get wrapped in an unknown block.

            CssClassifierContextType contextType = CssClassifierContextType.Default;

            switch (tokens.CurrentToken.TokenType)
            {
                case CssTokenType.Url:
                    pi = itemFactory.Create<UrlItem>(parent);
                    break;

                case CssTokenType.Function:
                    pi = Function.ParseFunction(parent, itemFactory, text, tokens);
                    alreadyParsed = true;
                    break;

                case CssTokenType.OpenFunctionBrace:
                case CssTokenType.OpenSquareBracket:
                case CssTokenType.OpenCurlyBrace:
                    pi = itemFactory.Create<UnknownBlock>(parent);
                    break;

                case CssTokenType.String:
                case CssTokenType.MultilineString:
                case CssTokenType.InvalidString:
                    contextType = CssClassifierContextType.String;
                    break;
            }

            if (pi == null)
            {
                pi = new TokenItem(tokens.CurrentToken, contextType);
            }

            if (!alreadyParsed && !pi.Parse(itemFactory, text, tokens))
            {
                Debug.Fail("Parse of an unknown item failed.");

                // I've done all I can do to deal with this unknown token, but now
                // it must be totally ignored so that parsing doesn't get into an infinite loop.
                tokens.AdvanceToken();
                pi = null;
            }

            if (pi != null && errorType.HasValue)
            {
                pi.AddParseError(errorType.Value, ParseErrorLocation.WholeItem);
            }

            return pi;
        }
    }
}
