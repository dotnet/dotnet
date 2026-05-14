// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Diagnostics;
using Microsoft.Css.Parser.Text;

namespace Microsoft.Css.Parser.Tokens
{
    /// <summary>
    /// Helps iterate over an array of tokens
    /// </summary>
    public class TokenStream
    {
        private readonly int _length;
        private bool _skipComments;

        internal TokenStream(TokenList tokens)
            : this(tokens, -1)
        {
        }

        internal TokenStream(TokenList tokens, int length)
        {
            Tokens = tokens;
            _length = length;
        }

        internal TokenList Tokens { get; }

        internal int Length
        {
            get { return _length == -1 ? Tokens.Count : _length; }
        }

        internal int Position { get; set; }

        internal bool SkipComments
        {
            get => _skipComments;

            set
            {
                _skipComments = value;

                // Make sure index is correctly positioned
                if (_skipComments)
                {
                    while (Position >= 0 && Position < Length && Tokens[Position].IsComment)
                    {
                        Position++;
                    }
                }
            }
        }

        internal CssToken CurrentToken
        {
            get { return Peek(0); }
        }

        internal CssToken Peek(int offset)
        {
            int index = OffsetCurrentIndex(offset);

            return (index >= 0 && index < Length)
                ? Tokens[index]
                : CssToken.EndOfFileToken();
        }

        internal CssToken AdvanceToken()
        {
            return Advance(1);
        }

        internal CssToken Advance(int offset)
        {
            CssToken prev = CurrentToken;

            Debug.Assert(!SkipComments || !prev.IsComment);

            Position = OffsetCurrentIndex(offset);

            Debug.Assert(Position >= -1 && Position <= Length);
            Debug.Assert(!SkipComments || !CurrentToken.IsComment);

            return prev;
        }

        private int OffsetCurrentIndex(int offset)
        {
            int newIndex = Position;

            while (offset != 0)
            {
                if (offset > 0 && newIndex < Length)
                {
                    newIndex++;
                    offset--;

                    if (SkipComments)
                    {
                        // Allow the new index to be Length if the last token is a comment
                        while (newIndex < Length && Tokens[newIndex].IsComment)
                        {
                            newIndex++;
                        }
                    }
                }
                else if (offset < 0 && newIndex >= 0)
                {
                    newIndex--;
                    offset++;

                    if (SkipComments)
                    {
                        // Allow the new index to be -1 if the first token is a comment
                        while (newIndex >= 0 && Tokens[newIndex].IsComment)
                        {
                            newIndex--;
                        }
                    }
                }
                else
                {
                    // Can't move any further
                    break;
                }
            }

            return newIndex;
        }

        internal bool IsWhiteSpaceAfterCurrentToken()
        {
            CssToken ct = CurrentToken;
            CssToken nt = Peek(1);

            if (ct.TokenType == CssTokenType.EndOfFile || nt.TokenType == CssTokenType.EndOfFile)
            {
                return false;
            }

            return ct.AfterEnd < nt.Start;
        }

        internal bool IsWhiteSpaceBeforeCurrentToken()
        {
            CssToken ct = CurrentToken;
            CssToken pt = Peek(-1);

            if (pt.TokenType == CssTokenType.EndOfFile)
            {
                return false;
            }

            return ct.Start > pt.AfterEnd;
        }

        internal bool IsNewLineAfterCurrentToken(ITextProvider text)
        {
            CssToken ct = CurrentToken;
            CssToken nt = Peek(1);

            if (!IsWhiteSpaceAfterCurrentToken())
            {
                return false;
            }

            for (int i = ct.AfterEnd; i < nt.Start; i++)
            {
                if (text[i] == '\r' || text[i] == '\n')
                {
                    return true;
                }
            }

            return false;
        }

        internal bool IsNewLineBeforeCurrentToken(ITextProvider text)
        {
            CssToken ct = CurrentToken;
            CssToken pt = Peek(-1);

            int limit = pt.TokenType == CssTokenType.EndOfFile ? 0 : pt.AfterEnd;

            for (int i = ct.Start - 1; i >= limit; i--)
            {
                if (text[i] == '\r' || text[i] == '\n')
                {
                    return true;
                }
            }

            return false;
        }
    }
}
