// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Classify;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;

namespace Microsoft.Css.Parser.TreeItems.Selectors
{
    /// <summary>
    /// CSS id selector, like E#id
    /// </summary>
    public class IdSelector : ComplexItem  // E#id
    {
        public TokenItem HashName { get; private set; }

        public IdSelector()
        {
        }

        internal bool TryGetIdRange(out int start, out int length)
        {
            if (HashName == null)
            {
                start = 0;
                length = 0;
                return false;
            }

            start = HashName.Start + 1;
            length = HashName.Length - 1;
            return true;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public override bool Parse(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            // Oddly enough, CSS2 grammar says #124 is a legal selector (#{name},
            // where {name}:[nmchar]+ which is different from {ident} that must begin
            // with  {nmstart} (letter). I don't think it is really true since
            // none of W3C samples ever show #123 and id="123" is not valid in XHTML.

            CssToken token = tokens.CurrentToken;

            if (token.TokenType == CssTokenType.HashName || token.TokenType == CssTokenType.Hash)
            {
                HashName = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.IdSelector);
                ParseAfterName(itemFactory, text, tokens);
            }

            return Children.Count > 0;
        }

        protected virtual void ParseAfterName(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            if (HashName.Length < 2) // need more than just a "#"
            {
                HashName.AddParseError(ParseErrorType.IdMissing, ParseErrorLocation.AfterItem);
            }
        }

        public override bool IsValid
        {
            get { return HashName != null && HashName.Length > 1 && base.IsValid; }
        }
    }
}
