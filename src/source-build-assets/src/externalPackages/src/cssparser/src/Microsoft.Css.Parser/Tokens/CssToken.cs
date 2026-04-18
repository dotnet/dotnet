// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Utilities;

namespace Microsoft.Css.Parser.Tokens
{
    // Basic tokens. Note that some tokens represent more complex constructs than others.
    // If character sequence is unique and cannot be interpreted differently and most probably
    // will be colorized as a unit, we produce a single token. Otherwise we leave it to the lexer.
    // For example, @ is valid only in @directive hence we make AtDirective token rather than
    // separate At and Identifier tokens. Same for ~= and += sequences.

    public enum CssTokenType
    {
        Unknown,
        EndOfFile,
        WhiteSpace,
        At,
        Dot,
        Asterisk,
        Ampersand,
        Tilde,
        Equals,
        Plus,
        Minus,
        Colon,
        Semicolon,
        Comma,
        Greater,
        Or,
        Caret,
        Dollar,
        Slash,
        Percent,
        Bang,
        Identifier,
        Hash,
        HashName,
        Number,             // Decimal or float

        [SuppressMessage("Microsoft.Name", "CA1720:IdentifiersShouldNotContainTypeNames")]
        String,

        MultilineString,
        InvalidString,      // Missing end quote, mismatched quotes (missing start quote will yield one or more identifiers)
        Url,                // "URL(" string  - note that space between URL and ( is not allowed
        UnquotedUrlString,  // Unquoted URL
        OneOf,              // ~=
        ContainsString,     // *=
        EndsWith,           // $=
        BeginsWith,         // ^=
        ListBeginsWith,     // |=
        DoubleColon,        // ::
        DoublePipe,         // ||
        OpenCurlyBrace,
        CloseCurlyBrace,
        OpenSquareBracket,
        CloseSquareBracket,
        OpenFunctionBrace,
        CloseFunctionBrace,
        OpenCComment,
        CloseCComment,
        OpenHtmlComment,
        CloseHtmlComment,
        CommentText,
        SingleTokenComment,
        SingleLineComment,
        Function,
        Units,
        Quote,
        UnicodeRange, // U+4E00-9FFF, U+30?? see http://www.w3.org/TR/css3-webfonts/#character-range-the-unicode-range-descri
        ScopeBlocker, // blocks the parser from continuing the parse in the current scope (not a standard token of course)
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
    public class CssToken : IRange, ICloneable
    {
        // two more bools can fit before enum flags should be used

        internal CssToken(CssTokenType tokenType, int start, int length)
        {
            TokenType = tokenType;
            Start = start;
            Length = length;

            switch (TokenType)
            {
                case CssTokenType.CloseCComment:
                case CssTokenType.CommentText:
                    IsComment = true;
                    IsChildToken = true;
                    break;

                case CssTokenType.CloseHtmlComment:
                case CssTokenType.OpenCComment:
                case CssTokenType.OpenHtmlComment:
                case CssTokenType.SingleLineComment:
                case CssTokenType.SingleTokenComment:
                    IsComment = true;
                    break;
            }
        }

        internal static CssToken EndOfFileToken()
        {
            return new CssToken(CssTokenType.EndOfFile, 0, 0);
        }

        internal static CssToken EndOfFileToken(ITextProvider textProvider)
        {
            return (textProvider == null)
                ? EndOfFileToken()
                : new CssToken(CssTokenType.EndOfFile, textProvider.Length, 0);
        }

        internal CssTokenType TokenType { get; set; }

        public int Start { get; private set; }

        public int Length { get; private set; }

        public int AfterEnd
        {
            get { return Start + Length; }
        }

        internal virtual void Shift(int offset)
        {
            Start += offset;
        }

        internal bool IsComment { get; set; }

        /// <summary>
        /// Does this token only exist because of a previous token?
        /// (for example, comment text only exists because of the start comment token)
        /// </summary>
        internal bool IsChildToken { get; set; }

        internal bool IsString()
        {
            return TokenType == CssTokenType.String ||
                TokenType == CssTokenType.InvalidString ||
                TokenType == CssTokenType.MultilineString;
        }

        internal bool IsUrlString()
        {
            return TokenType == CssTokenType.UnquotedUrlString ||
                IsString();
        }

        internal bool IsScopeBlocker()
        {
            return TokenType == CssTokenType.ScopeBlocker ||
                TokenType == CssTokenType.EndOfFile;
        }

        internal bool IsBlockTerminator()
        {
            return TokenType == CssTokenType.CloseCurlyBrace ||
                IsScopeBlocker();
        }

        internal bool IsFunctionTerminator()
        {
            return IsFunctionTerminator(terminateOnSemicolon: true);
        }

        internal bool IsFunctionTerminator(bool terminateOnSemicolon)
        {
            return TokenType == CssTokenType.CloseFunctionBrace ||
                (TokenType == CssTokenType.Semicolon && terminateOnSemicolon) ||
                IsBlockTerminator();
        }

        internal bool IsFunctionArgumentTerminator()
        {
            return TokenType == CssTokenType.Comma ||
                IsFunctionTerminator();
        }

        internal bool IsDeclarationTerminator()
        {
            return TokenType == CssTokenType.Semicolon ||
                TokenType == CssTokenType.OpenCurlyBrace ||
                IsBlockTerminator();
        }

        internal bool IsDirectiveTerminator()
        {
            return TokenType == CssTokenType.Semicolon ||
                TokenType == CssTokenType.OpenCurlyBrace ||
                IsBlockTerminator();
        }

        internal bool IsSimpleSelectorCombineOperator()
        {
            return TokenType == CssTokenType.Greater ||
                TokenType == CssTokenType.Plus ||
                TokenType == CssTokenType.Tilde ||
                TokenType == CssTokenType.DoublePipe;
        }

        internal bool IsSelectorTerminator()
        {
            return IsSimpleSelectorCombineOperator() ||
                IsSelectorGroupTerminator();
        }

        internal bool IsSelectorGroupTerminator()
        {
            return TokenType == CssTokenType.OpenCurlyBrace ||
                TokenType == CssTokenType.Comma ||
                IsBlockTerminator();
        }

        internal bool IsComparableByType()
        {
            switch (TokenType)
            {
                default:
                    // These token types contain unique text that must be compared
                    return false;

                // These token types all represent chars that never change, so
                // comparing the type is good enough:
                case CssTokenType.EndOfFile:
                case CssTokenType.At:
                case CssTokenType.Dot:
                case CssTokenType.Asterisk:
                case CssTokenType.Ampersand:
                case CssTokenType.Tilde:
                case CssTokenType.Equals:
                case CssTokenType.Plus:
                case CssTokenType.Minus:
                case CssTokenType.Colon:
                case CssTokenType.Semicolon:
                case CssTokenType.Comma:
                case CssTokenType.Greater:
                case CssTokenType.Or:
                case CssTokenType.Caret:
                case CssTokenType.Dollar:
                case CssTokenType.Slash:
                case CssTokenType.Percent:
                case CssTokenType.Hash:
                case CssTokenType.Bang:
                case CssTokenType.OneOf:
                case CssTokenType.ContainsString:
                case CssTokenType.EndsWith:
                case CssTokenType.BeginsWith:
                case CssTokenType.ListBeginsWith:
                case CssTokenType.DoubleColon:
                case CssTokenType.DoublePipe:
                case CssTokenType.OpenCurlyBrace:
                case CssTokenType.CloseCurlyBrace:
                case CssTokenType.OpenSquareBracket:
                case CssTokenType.CloseSquareBracket:
                case CssTokenType.OpenFunctionBrace:
                case CssTokenType.CloseFunctionBrace:
                case CssTokenType.OpenCComment:
                case CssTokenType.CloseCComment:
                case CssTokenType.OpenHtmlComment:
                case CssTokenType.CloseHtmlComment:
                case CssTokenType.ScopeBlocker:
                    return true;
            }
        }

#if DEBUG
        private string _debugText = string.Empty;
        internal string DebugText
        {
            get { return _debugText; }
            set { _debugText = value ?? string.Empty; }
        }

        public override string ToString()
        {
            if (TokenType == CssTokenType.EndOfFile)
            {
                return "EOF";
            }
            else
            {
                string typeName = TokenType.ToString();
                int typePeriodIndex = typeName.LastIndexOf('.');

                return string.Format(
                    CultureInfo.InvariantCulture,
                    "{3}, {0}:({1}-{2})",
                    typeName.Substring(typePeriodIndex + 1), // {0}
                    Start, // {1}
                    AfterEnd, // {2}
                    (Length > 24) ? DebugText.Substring(0, 24) + "..." : DebugText); // {3}
            }
        }
#endif

        internal static bool CompareTokens(CssToken token1, CssToken token2, ITextProvider textProvider1, ITextProvider textProvider2)
        {
            if (token1.TokenType != token2.TokenType)
            {
                return false;
            }

            if (token1.IsComparableByType())
            {
                return true;
            }

            if (token1.Length == token2.Length &&
                textProvider1 != null &&
                textProvider2 != null)
            {
                return textProvider1.CompareTo(token1.Start, textProvider2, token2.Start, token1.Length, ignoreCase: false);
            }

            return false;
        }

        #region ICloneable Members
        public object Clone()
        {
            CssToken clone = new CssToken(TokenType, Start, Length);
            clone.IsComment = IsComment;
            clone.IsChildToken = IsChildToken;
#if DEBUG
            clone.DebugText = DebugText;
#endif
            return clone;
        }
        #endregion
    }
}
