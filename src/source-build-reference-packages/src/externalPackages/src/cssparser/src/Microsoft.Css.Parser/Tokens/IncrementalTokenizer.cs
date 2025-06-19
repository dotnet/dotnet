// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Microsoft.Css.Parser.Text;

namespace Microsoft.Css.Parser.Tokens
{
    /// <summary>
    /// Computes new tokens for a changed range of text (given the old tokens)
    /// </summary>
    internal static class IncrementalTokenizer
    {
        public struct Result
        {
            public TokenList NewTokens { get; set; } // only newly created tokens
            public TokenList OldTokens { get; set; } // complete list of old tokens
            public int TokenizationStart { get; set; }
            public int OldTokenStart { get; set; } // start of old tokens to delete
            public int OldTokenCount { get; set; } // count of old tokens to delete
            public int OldTokenTextOffset { get; set; } // offset after the change

            public int TextDeletedLength
            {
                get
                {
                    // Easily computed from the old token list

                    if (OldTokenCount > 0)
                    {
                        return OldTokens[OldTokenStart + OldTokenCount - 1].AfterEnd - TokenizationStart;
                    }

                    return 0;
                }
            }

            public int TextInsertedLength
            {
                get
                {
                    // Easily computed from the new token list

                    if (NewTokens.Count > 0)
                    {
                        return NewTokens[NewTokens.Count - 1].AfterEnd - TokenizationStart;
                    }

                    return 0;
                }
            }
        }

        /// <summary>
        /// Global method to compute the Result of a token change
        /// </summary>
        public static Result TokenizeChange(
            ICssTokenizerFactory tokenizerFactory,
            TokenList oldTokens,
            ITextProvider oldText,
            ITextProvider newText,
            int changeStart,
            int deletedLength,
            int insertedLength)
        {
            Result result = new Result();
            char firstInsertedChar = (insertedLength > 0) ? newText[changeStart] : '\0';

            result.NewTokens = new TokenList();
            result.OldTokens = oldTokens;
            result.OldTokenStart = FindTokenToStart(oldTokens, changeStart, firstInsertedChar);
            result.OldTokenCount = oldTokens.Count - result.OldTokenStart; // assume delete to EOF
            result.OldTokenTextOffset = insertedLength - deletedLength;
            result.TokenizationStart = changeStart;

            if (result.OldTokenStart < oldTokens.Count)
            {
                // The first old token may start before the actual text change.
                // Adjust where tokenization starts:
                result.TokenizationStart = Math.Min(result.TokenizationStart, oldTokens[result.OldTokenStart].Start);
            }

            // Tokenize until EOF or until the new tokens start matching the old tokens
            bool tokenizeUntilEOF = (oldTokens.Count == 0);

            // Create and init a streaming tokenizer
            ICssTokenizer tokenizer = tokenizerFactory.CreateTokenizer();
            int estimatedLength = (tokenizeUntilEOF ? newText.Length - result.TokenizationStart : insertedLength);
            tokenizer.InitStream(newText, result.TokenizationStart, estimatedLength, keepWhiteSpace: false);

            for (CssToken token = tokenizer.StreamNextToken(); true; token = tokenizer.StreamNextToken())
            {
                if (token.TokenType != CssTokenType.EndOfFile && !tokenizeUntilEOF &&
                    token.Start >= changeStart + insertedLength)
                {
                    // This could be a good token for stopping, see if it matches an old token

                    int oldTokenStart = token.Start - result.OldTokenTextOffset;
                    int oldTokenIndex = oldTokens.FindInsertIndex(oldTokenStart, beforeExisting: true);

                    if (oldTokenIndex == oldTokens.Count)
                    {
                        tokenizeUntilEOF = true;
                    }
                    else
                    {
                        CssToken oldToken = oldTokens[oldTokenIndex];

                        if (oldToken.Start == oldTokenStart && CssToken.CompareTokens(token, oldToken, newText, oldText))
                        {
                            result.OldTokenCount = oldTokenIndex - result.OldTokenStart;
                            break;
                        }
                    }
                }

                result.NewTokens.Add(token);

                if (token.TokenType == CssTokenType.EndOfFile)
                {
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Call this after calling TokenizeChange to update the old tokens
        /// </summary>
        public static void ApplyResult(Result result)
        {
            if (result.OldTokens != null && result.NewTokens != null)
            {
                if (result.OldTokenTextOffset != 0)
                {
                    for (int i = result.OldTokenStart + result.OldTokenCount; i < result.OldTokens.Count; i++)
                    {
                        result.OldTokens[i].Shift(result.OldTokenTextOffset);
                    }
                }

                result.OldTokens.RemoveRange(result.OldTokenStart, result.OldTokenCount);
                result.OldTokens.AddRange(result.NewTokens);
            }
        }

        /// <summary>
        /// Figures out what token to start replacing in the old token list.
        /// Tokenization should start right before the returned token index.
        /// </summary>
        private static int FindTokenToStart(TokenList tokens, int textStart, char firstNewChar)
        {
            int oldTokenIndex = tokens.FindInsertIndex(textStart, beforeExisting: true);

            if (oldTokenIndex > 0)
            {
                // Go back until there is whitespace before the current token (oldTokenIndex)
                int mustEndBefore = char.IsWhiteSpace(firstNewChar) ? textStart + 1 : textStart;

                while (oldTokenIndex > 0 && !TokenEndsBefore(tokens[oldTokenIndex - 1], mustEndBefore))
                {
                    oldTokenIndex--;
                    mustEndBefore = tokens[oldTokenIndex].Start;
                }

                if (oldTokenIndex < tokens.Count)
                {
                    // Go back to find a non-child token (example: can't start tokenzing inside of comment text)

                    while (oldTokenIndex > 0 && tokens[oldTokenIndex].IsChildToken)
                    {
                        oldTokenIndex--;
                    }
                }
            }

            return oldTokenIndex;
        }

        private static bool TokenEndsBefore(CssToken token, int mustEndBefore)
        {
            switch (token.TokenType)
            {
                case CssTokenType.OpenCComment:
                case CssTokenType.CommentText:
                case CssTokenType.SingleTokenComment:
                case CssTokenType.SingleLineComment:
                    // Comment text will expand to include any new text, so pretend that it doesn't end
                    return false;

                case CssTokenType.InvalidString:
                    // Pretend that the unclosed string doesn't end
                    return false;

                case CssTokenType.Url:
                    // This is for "url(", which alters the token after it, so pretend that it doesn't end
                    return false;

                default:
                    return token.AfterEnd < mustEndBefore;
            }
        }
    }
}
