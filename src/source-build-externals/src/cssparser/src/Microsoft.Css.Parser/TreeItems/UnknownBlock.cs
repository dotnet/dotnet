// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems.PropertyValues;

namespace Microsoft.Css.Parser.TreeItems
{
    /// <summary>
    /// This represents unexpected braces and their content: {...}, [...], or (...)
    /// Derived classes can be used in cases where braces are expected.
    /// </summary>
    internal class UnknownBlock : ComplexItem
    {
        internal TokenItem OpenBlock { get; private set; }
        internal TokenItem CloseBlock { get; private set; }

        public UnknownBlock()
        {
        }

        public override bool IsValid
        {
            get { return IsBlockValid && base.IsValid; }
        }

        /// <summary>
        /// Don't know what this block is, so it's invalid.
        /// Derived classes can override this if the block is known to be valid.
        /// </summary>
        protected virtual bool IsBlockValid
        {
            get { return false; }
        }

        /// <summary>
        /// Skip over a complete block of unknown content
        /// </summary>
        public override bool Parse(ItemFactory itemFactory, ITextProvider text, TokenStream tokens)
        {
            // Save the opening token for this block
            OpenBlock = Children.AddCurrentAndAdvance(tokens, null);

            CssTokenType matchingEndType = GetMatchingTokenType(OpenBlock.TokenType);
            Debug.Assert(matchingEndType != CssTokenType.Unknown);

            // Search for the matching end of the block
            bool invalidBlock = false;

            while (!invalidBlock &&
                tokens.CurrentToken.TokenType != matchingEndType &&
                !tokens.CurrentToken.IsScopeBlocker())
            {
                // Treat the contents of the block as property values so that units, functions, etc. get parsed
                ParseItem pi = PropertyValueHelpers.ParsePropertyValue(this, itemFactory, text, tokens);

                if (pi != null)
                {
                    Children.Add(pi);
                }
                else
                {
                    switch (tokens.CurrentToken.TokenType)
                    {
                        case CssTokenType.CloseCurlyBrace:
                        case CssTokenType.CloseFunctionBrace:
                        case CssTokenType.CloseSquareBracket:
                            // Found a non-matching end brace/bracket, so stop parsing
                            invalidBlock = true;
                            break;

                        default:
                            Children.AddUnknownAndAdvance(itemFactory, text, tokens);
                            break;
                    }
                }
            }

            if (tokens.CurrentToken.TokenType == matchingEndType)
            {
                CloseBlock = Children.AddCurrentAndAdvance(tokens, null);
            }
            else
            {
                OpenBlock.AddParseError(ParseErrorType.CloseBraceMismatch, ParseErrorLocation.AfterItem);
            }

            return Children.Count > 0;
        }

        /// <summary>
        /// Returns true if the token stream was moved forward at all
        /// </summary>
        internal static bool SkipBlock(TokenStream tokens, bool allowCurlyBraces)
        {
            Stack<CssTokenType> endMatches = new Stack<CssTokenType>();

            if ((tokens.CurrentToken.TokenType == CssTokenType.OpenCurlyBrace && allowCurlyBraces) ||
                tokens.CurrentToken.TokenType == CssTokenType.OpenFunctionBrace ||
                tokens.CurrentToken.TokenType == CssTokenType.OpenSquareBracket ||
                tokens.CurrentToken.TokenType == CssTokenType.Function)
            {
                endMatches.Push(GetMatchingTokenType(tokens.CurrentToken.TokenType));
                tokens.AdvanceToken();
            }
            else
            {
                Debug.Fail("Called UnknownItem.SkipBlock at somewhere other than a block");
                return false;
            }

            while (!tokens.CurrentToken.IsScopeBlocker() && endMatches.Count > 0)
            {
                switch (tokens.CurrentToken.TokenType)
                {
                    case CssTokenType.OpenCurlyBrace:
                        if (!allowCurlyBraces)
                        {
                            // stop at the first open curly brace
                            endMatches.Clear();
                        }
                        else
                        {
                            endMatches.Push(GetMatchingTokenType(tokens.CurrentToken.TokenType));
                            tokens.AdvanceToken();
                        }
                        break;

                    case CssTokenType.OpenSquareBracket:
                    case CssTokenType.OpenFunctionBrace:
                    case CssTokenType.Function:
                        endMatches.Push(GetMatchingTokenType(tokens.CurrentToken.TokenType));
                        tokens.AdvanceToken();
                        break;

                    case CssTokenType.CloseCurlyBrace:
                    case CssTokenType.CloseSquareBracket:
                    case CssTokenType.CloseFunctionBrace:
                        if (tokens.CurrentToken.TokenType == endMatches.Peek())
                        {
                            endMatches.Pop();
                            tokens.AdvanceToken();
                        }
                        else
                        {
                            // bad nesting, so bail out
                            endMatches.Clear();
                        }
                        break;

                    default:
                        tokens.AdvanceToken();
                        break;
                }
            }

            return true;
        }

        /// <summary>
        /// Helper function for matching braces
        /// </summary>
        private static CssTokenType GetMatchingTokenType(CssTokenType type)
        {
            switch (type)
            {
                case CssTokenType.OpenCurlyBrace:
                    return CssTokenType.CloseCurlyBrace;

                case CssTokenType.OpenSquareBracket:
                    return CssTokenType.CloseSquareBracket;

                case CssTokenType.Function:
                case CssTokenType.OpenFunctionBrace:
                    return CssTokenType.CloseFunctionBrace;

                case CssTokenType.CloseCurlyBrace:
                    return CssTokenType.OpenCurlyBrace;

                case CssTokenType.CloseSquareBracket:
                    return CssTokenType.OpenSquareBracket;

                case CssTokenType.CloseFunctionBrace:
                    return CssTokenType.OpenFunctionBrace;

                default:
                    Debug.Fail("Unexpected brace token in GetMatchingTokenType");
                    return CssTokenType.Unknown;
            }
        }
    }
}
