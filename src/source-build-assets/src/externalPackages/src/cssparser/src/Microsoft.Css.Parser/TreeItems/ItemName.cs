// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Classify;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;

namespace Microsoft.Css.Parser.TreeItems
{
    /// <summary>
    /// CSS item name (i.e. namespace|name sequence)
    /// </summary>
    public class ItemName : ComplexItem
    {
        /// <summary>
        /// Namespace token
        /// </summary>
        internal TokenItem Namespace { get; private set; }
        /// <summary>
        /// Separator token ('|')
        /// </summary>
        internal TokenItem Separator { get; private set; }
        /// <summary>
        /// Item name. Nay be missing or be a *
        /// </summary>
        internal TokenItem Name { get; private set; } // may be missing or *

        public ItemName()
        {
        }

        internal static bool IsAtItemName(TokenStream tokens)
        {
            switch (tokens.CurrentToken.TokenType)
            {
                case CssTokenType.Identifier:
                case CssTokenType.Asterisk:
                case CssTokenType.Or:
                    return true;

                default:
                    return false;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public override bool Parse(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            // Parse namespace: foo|bar, *|, *, |A, ..., applies to both elements and attributes
            // note that there should not be spaces between namespace, | and name i.e.
            // foo | bar are actually three selectors and middle one is missing namespace and name.
            //
            // Note that we are not going to parse 3|3 as a valid name. Technically we could do it and
            // leave validation to verify and squiggle with it, but parsing such construct as a legal
            // name would cause colorizer to colorize sequence as an item name which would be confusing
            // to the user. I'd rather have it not colorized as legal name and have it apper black instead
            // telling customer that it is wrong as she types, well before validation pass hits.

            if (tokens.CurrentToken.TokenType == CssTokenType.Identifier || tokens.CurrentToken.TokenType == CssTokenType.Asterisk)
            {
                // There should be no spaces between namespace, | and the element name
                if (tokens.IsWhiteSpaceAfterCurrentToken() || tokens.Peek(1).IsSelectorTerminator())
                {
                    Name = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.ItemName);
                }
                else if (tokens.Peek(1).TokenType == CssTokenType.Or)
                {
                    // validator will deal with invalid namespaces
                    Namespace = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.ItemNamespace);
                    Separator = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.Default);
                }
            }
            else if (tokens.CurrentToken.TokenType == CssTokenType.Or)
            {
                Separator = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.Default);
            }

            if (Name == null && (tokens.CurrentToken.TokenType == CssTokenType.Identifier || tokens.CurrentToken.TokenType == CssTokenType.Asterisk))
            {
                if (Separator == null || !tokens.IsWhiteSpaceBeforeCurrentToken())
                {
                    Name = Children.AddCurrentAndAdvance(tokens, CssClassifierContextType.ItemName);
                }
            }
            else
            {
                // It is OK for the name to be missing. Lonely | is the same as *|*
                // so we are not going to add missing item here
            }

            return Children.Count > 0;
        }
    }
}
