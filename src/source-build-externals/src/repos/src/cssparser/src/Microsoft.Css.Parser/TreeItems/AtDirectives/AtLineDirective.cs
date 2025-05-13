// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Classify;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Tokens;

namespace Microsoft.Css.Parser.TreeItems.AtDirectives
{
    /// <summary>
    /// Single line expected, in the form: @item1 item2 ... ;
    /// </summary>
    internal abstract class AtLineDirective : AtDirective
    {
        internal TokenItem Semicolon { get; private set; }

        protected AtLineDirective()
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        protected bool CheckSemicolon(TokenStream tokens)
        {
            if (tokens.CurrentToken.TokenType == CssTokenType.Semicolon)
            {
                Semicolon = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.Default);
            }
            else
            {
                AddParseError(ParseErrorType.AtDirectiveSemicolonMissing, ParseErrorLocation.AfterItem);
            }

            return Semicolon != null;
        }
    }
}
