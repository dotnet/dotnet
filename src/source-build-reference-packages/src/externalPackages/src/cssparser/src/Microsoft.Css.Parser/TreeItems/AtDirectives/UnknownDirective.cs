// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Diagnostics;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;

namespace Microsoft.Css.Parser.TreeItems.AtDirectives
{
    /// <summary>
    /// The type of the directive is unknown, so read up until the next
    /// block of curly braces or semicolon as defined in the CSS spec:
    /// http://www.w3.org/TR/css3-syntax/#error-handling
    /// </summary>
    internal class UnknownDirective : AtDirective
    {
        public BlockItem Block { get; protected set; }
        public TokenItem Semicolon { get; protected set; }

        public UnknownDirective()
        {
        }

        public override bool Parse(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            ParseAtAndKeyword(itemFactory, text, tokens);
            ParseAfterName(itemFactory, text, tokens);

            bool foundEnd = false;

            while (!tokens.CurrentToken.IsBlockTerminator())
            {
                CssTokenType tokenType = tokens.CurrentToken.TokenType;

                if (tokens.CurrentToken.TokenType == CssTokenType.OpenCurlyBrace)
                {
                    BlockItem block = NewBlock();
                    if (block == null)
                    {
                        break;
                    }
                    else if (block.Parse(itemFactory, text, tokens))
                    {
                        foundEnd = true;
                        Block = block;
                        Children.Add(block);
                    }
                    else
                    {
                        foundEnd = true;
                        Children.AddUnknownAndAdvance(itemFactory, text, tokens);
                    }
                }
                else if (!ParseNextChild(itemFactory, text, tokens))
                {
                    TokenItem item = Children.AddUnknownAndAdvance(itemFactory, text, tokens) as TokenItem;
                    if (item != null && item.TokenType == CssTokenType.Semicolon)
                    {
                        Debug.Assert(Semicolon == null);
                        Semicolon = item;
                    }
                }

                if (tokenType == CssTokenType.Semicolon)
                {
                    foundEnd = true;
                    break;
                }
                else if (tokenType == CssTokenType.OpenCurlyBrace)
                {
                    break;
                }
            }

            if (!foundEnd && !AllowUnclosed)
            {
                Children.AddParseError(ParseErrorType.AtDirectiveSemicolonMissing);
            }

            return Children.Count > 0;
        }

        protected virtual bool AllowUnclosed
        {
            get { return false; }
        }

        protected virtual BlockItem NewBlock()
        {
            return new UnknownDirectiveBlock();
        }

        protected virtual void ParseAfterName(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            // nothing to do here
        }

        protected virtual bool ParseNextChild(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            // nothing to do here
            return false;
        }
    }
}
