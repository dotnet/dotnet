// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Diagnostics;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.TreeItems.PropertyValues;

// Reference:
// http://www.w3.org/TR/CSS21/syndata.html
// http://www.w3.org/TR/CSS21/grammar.html

namespace Microsoft.Css.Parser.Tokens
{
    public interface ICssTokenizer
    {
        TokenList Tokenize(string cssText, int start, int length, bool keepWhiteSpace);
        TokenList Tokenize(ITextProvider textProvider, int start, int length, bool keepWhiteSpace);

        void InitStream(string cssText, int start, bool keepWhiteSpace);
        void InitStream(ITextProvider textProvider, int start, bool keepWhiteSpace);
        void InitStream(ITextProvider textProvider, int start, int estimatedLength, bool keepWhiteSpace);
        CssToken StreamNextToken();
    }

    internal class CssTokenizer : ICssTokenizer
    {
        protected TokenList Tokens { get; private set; }
        protected CharacterStream CS { get; private set; }
        protected bool KeepWhiteSpace { get; private set; }
        private int _streamToken;

        // Special token for allowing multiple stylesheets to live in one buffer (from htmled\cssbuffergenerator.cpp)
        private const string ScopeBlockerText = "/* END EXTERNAL SOURCE */";

        internal CssTokenizer()
        {
        }

        /// <summary>
        /// Create a list of CSS tokens given a string
        /// </summary>
        public TokenList Tokenize(string cssText, int start, int length, bool keepWhiteSpace)
        {
            return Tokenize(new StringTextProvider(cssText ?? string.Empty), start, length, keepWhiteSpace);
        }

        /// <summary>
        /// Create a list of CSS tokens given any text source
        /// </summary>
        public TokenList Tokenize(ITextProvider textProvider, int start, int length, bool keepWhiteSpace)
        {
            Debug.Assert(start >= 0 && length >= 0 && start + length <= textProvider.Length);

            InitStream(textProvider, start, keepWhiteSpace);

            while (CS.Position < start + length && AddNextTokenWrapper())
            {
                // Keep on adding tokens...
            }

            Tokens.Add(CssToken.EndOfFileToken(textProvider));

            return Tokens;
        }

        public void InitStream(string cssText, int start, bool keepWhiteSpace)
        {
            InitStream(new StringTextProvider(cssText), start, keepWhiteSpace);
        }

        public void InitStream(ITextProvider textProvider, int start, bool keepWhiteSpace)
        {
            InitStream(textProvider, start, textProvider.Length - start, keepWhiteSpace);
        }

        public void InitStream(ITextProvider textProvider, int start, int estimatedLength, bool keepWhiteSpace)
        {
            CS = new CharacterStream(textProvider)
            {
                Position = start
            };

            // Guess how many tokens will be allocated (5 was the average token length of the 090 test files)
            const int averageTokenLength = 5;
            int tokenCountGuess = estimatedLength / averageTokenLength;

            Tokens = new TokenList(tokenCountGuess);
            KeepWhiteSpace = keepWhiteSpace;
            _streamToken = 0;
        }

        public CssToken StreamNextToken()
        {
            if (_streamToken == Tokens.Count && !AddNextTokenWrapper())
            {
                return CssToken.EndOfFileToken(CS.TextProvider);
            }

            return Tokens[_streamToken++];
        }

        /// <summary>
        /// Wrapper for calling AddNextToken and also detecting child tokens
        /// </summary>
        private bool AddNextTokenWrapper()
        {
            int oldTokenCount = Tokens.Count;
            bool success = AddNextToken();

            // AddNextToken always returns true for whitespace, even if a token wasn't added.
            while (success && Tokens.Count == oldTokenCount)
            {
                success = AddNextToken();
            }

            if (success)
            {
                for (int i = oldTokenCount + 1; i < Tokens.Count; i++)
                {
                    // Whenever multiple tokens are added at once, every token after
                    // the first is a "child" token that depends on the first token.

                    Tokens[i].IsChildToken = true;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Appends zero or more tokens to the end of _tokenList.
        /// Returns true is tokenizing can continue, false if the end of file was reached.
        /// </summary>
        private bool AddNextToken()
        {
            if (CS.IsAtEnd)
            {
                return false;
            }

            if (HandleWhitespace() ||
                HandleDimension())
            {
                return true;
            }

            return HandleToken();
        }

        protected virtual bool HandleToken()
        {
            CssTokenType tokenType = CssTokenType.Unknown;
            int tokenStart = CS.Position;

            switch (CS.CurrentChar)
            {
                case '*':
                    if (CS.Peek(1) == '=')
                    {
                        tokenType = CssTokenType.ContainsString;
                        CS.Advance(1);
                    }
                    else
                    {
                        tokenType = CssTokenType.Asterisk;
                    }
                    break;

                case '&':
                    tokenType = CssTokenType.Ampersand;
                    break;

                case '.':
                    tokenType = CssTokenType.Dot;
                    break;

                case '!':
                    tokenType = CssTokenType.Bang;
                    break;

                case ',':
                    tokenType = CssTokenType.Comma;
                    break;

                case '^':
                    if (CS.Peek(1) == '=')
                    {
                        tokenType = CssTokenType.BeginsWith;
                        CS.Advance(1);
                    }
                    else
                    {
                        tokenType = CssTokenType.Caret;
                    }
                    break;

                case ':':
                    if (CS.Peek(1) == ':')
                    {
                        tokenType = CssTokenType.DoubleColon;
                        CS.Advance(1);
                    }
                    else
                    {
                        tokenType = CssTokenType.Colon;
                    }
                    break;

                case '$':
                    if (CS.Peek(1) == '=')
                    {
                        tokenType = CssTokenType.EndsWith;
                        CS.Advance(1);
                    }
                    else
                    {
                        tokenType = CssTokenType.Dollar;
                    }
                    break;

                case '=':
                    tokenType = CssTokenType.Equals;
                    break;

                case '>':
                    tokenType = CssTokenType.Greater;
                    break;

                case '|':
                    if (CS.Peek(1) == '=')
                    {
                        tokenType = CssTokenType.ListBeginsWith;
                        CS.Advance(1);
                    }
                    else if (CS.Peek(1) == '|')
                    {
                        tokenType = CssTokenType.DoublePipe;
                        CS.Advance(1);
                    }
                    else
                    {
                        tokenType = CssTokenType.Or;
                    }
                    break;

                case '%':
                    tokenType = CssTokenType.Percent;
                    break;

                case ';':
                    tokenType = CssTokenType.Semicolon;
                    break;

                case '/':
                    if (HandleComment())
                    {
                        return true;
                    }
                    else
                    {
                        tokenType = CssTokenType.Slash;
                    }
                    break;

                case '\\':
                    if (HandleSlashNineHack())
                    {
                        return true;
                    }
                    break;

                case '<':
                    if (CS.TextProvider.CompareTo(CS.Position, "<!--", ignoreCase: false))
                    {
                        CS.Advance(4);
                        AddToken(CssTokenType.OpenHtmlComment, tokenStart, CS.Position - tokenStart);
                        return true;
                    }
                    break;

                case '~':
                    if (CS.Peek(1) == '=')
                    {
                        tokenType = CssTokenType.OneOf;
                        CS.Advance(1);
                    }
                    else
                    {
                        tokenType = CssTokenType.Tilde;
                    }
                    break;

                case '(':
                    tokenType = CssTokenType.OpenFunctionBrace;
                    break;

                case ')':
                    tokenType = CssTokenType.CloseFunctionBrace;
                    break;

                case '[':
                    tokenType = CssTokenType.OpenSquareBracket;
                    break;

                case ']':
                    tokenType = CssTokenType.CloseSquareBracket;
                    break;

                case '{':
                    tokenType = CssTokenType.OpenCurlyBrace;
                    break;

                case '}':
                    tokenType = CssTokenType.CloseCurlyBrace;
                    break;

                case '@':
                    tokenType = CssTokenType.At;
                    break;

                case '\'':
                case '\"':
                    if (HandleString() != CssTokenType.Unknown)
                    {
                        return true;
                    }
                    break;

                case '#':
                    if (HandleHash())
                    {
                        return true;
                    }
                    else
                    {
                        tokenType = CssTokenType.Hash;
                    }
                    break;

                case '+':
                    tokenType = CssTokenType.Plus;
                    break;

                case '-':
                    if (HandleIdentifier())
                    {
                        return true;
                    }
                    else if (CS.TextProvider.CompareTo(CS.Position, "-->", ignoreCase: false))
                    {
                        CS.Advance(3);
                        AddToken(CssTokenType.CloseHtmlComment, tokenStart, CS.Position - tokenStart);
                        return true;
                    }
                    else
                    {
                        tokenType = CssTokenType.Minus;
                    }
                    break;

                case 'u':
                case 'U':
                    if (HandleUnicodeRange())
                    {
                        return true;
                    }
                    break;
            }

            if (tokenType == CssTokenType.Unknown &&
                HandleIdentifier())
            {
                return true;
            }

            if (!HandleUnknown())
            {
                // Some kind of junk in the CSS, just deal with it by making an unknown token
                CS.Advance(1);
                AddToken(tokenType, tokenStart, CS.Position - tokenStart);
            }

            return true;
        }

        /// <summary>
        /// Derived tokenizers can create custom tokens for normally unknown token types
        /// </summary>
        protected virtual bool HandleUnknown()
        {
            return false;
        }

        protected bool HandleWhitespace()
        {
            int start = CS.Position;

            if (SkipWhitespace())
            {
                if (KeepWhiteSpace)
                {
                    AddToken(CssTokenType.WhiteSpace, start, CS.Position - start);
                }
            }

            return start != CS.Position;
        }

        private bool HandleWhitespaceAndComments()
        {
            bool handled = false;

            while (HandleWhitespace() || HandleComment())
            {
                handled = true;
            }

            return handled;
        }

        private bool HandleComment()
        {
            const CssTokenType startTokenType = CssTokenType.OpenCComment;
            const CssTokenType endTokenType = CssTokenType.CloseCComment;
            const string commentStart = "/*";
            const string commentEnd = "*/";

            if (AtScopeBlocker())
            {
                AddToken(CssTokenType.ScopeBlocker, CS.Position, ScopeBlockerText.Length);
                CS.Advance(ScopeBlockerText.Length);

                return true;
            }

            // Comment: /* foo */
            // Makes three tokens for a comment (start, text, and end)

            if (!CS.TextProvider.CompareTo(CS.Position, commentStart, ignoreCase: false))
            {
                return false;
            }

            // Skip the start of the comment

            AddToken(startTokenType, CS.Position, commentStart.Length);
            CS.Advance(commentStart.Length);

            // Skip the inner text of the comment

            bool endFound = false;

            if (!CS.IsAtEnd)
            {
                SkipWhitespace();

                int start = CS.Position;

                for (; !CS.IsAtEnd; CS.Advance(1))
                {
                    if (CS.TextProvider.CompareTo(CS.Position, commentEnd, ignoreCase: false))
                    {
                        endFound = true;
                        break;
                    }

                    if (AtScopeBlocker())
                    {
                        // Catch: /* Foo /* END EXTERNAL SOURCE */
                        break;
                    }
                }

                if (CS.Position >= start)
                {
                    // Create a token for the comment text, don't include trailing whitespace
                    SkipWhitespaceReverse();

                    if (CS.Position > start)
                    {
                        AddToken(CssTokenType.CommentText, start, CS.Position - start);
                    }

                    SkipWhitespace();
                }
            }

            // Skip the end of the comment

            if (endFound)
            {
                AddToken(endTokenType, CS.Position, commentEnd.Length);
                CS.Advance(commentEnd.Length);
            }

            return true;
        }

        private CssTokenType HandleString()
        {
            int start = CS.Position;
            CssTokenType tokenType = SkipString();

            if (tokenType != CssTokenType.Unknown)
            {
                AddToken(tokenType, start, CS.Position - start);
            }

            return tokenType;
        }

        private bool HandleHash()
        {
            // Makes a token for: #foo

            int start = CS.Position;

            if (CS.CurrentChar == '#')
            {
                CS.Advance(1);

                if (SkipName())
                {
                    AddToken(CssTokenType.HashName, start, CS.Position - start);
                }
                else
                {
                    // There is no name after the '#'
                    CS.Position = start;
                }
            }

            return start != CS.Position;
        }

        private bool HandleDimension()
        {
            // Makes tokens for: 100, 100px, +0.7em, -20vw

            int start = CS.Position;

            if (CS.CurrentChar == '+' || CS.CurrentChar == '-')
            {
                CS.Advance(1);
            }

            if (!SkipNumber())
            {
                CS.Position = start;
                return false;
            }

            AddToken(CssTokenType.Number, start, CS.Position - start);

            // Deal with the units after the number (don't care if it fails)
            HandleUnits();

            return true;
        }

        private bool HandleUnits()
        {
            // Makes a token for the units identifier after a number

            int start = CS.Position;

            if (CS.CurrentChar == '%')
            {
                AddToken(CssTokenType.Units, start, 1);
                CS.Advance(1);
                return true;
            }

            if (SkipIdentifier())
            {
                UnitType unitType = UnitHelpers.GetUnitType(CS.TextProvider, start, CS.Position - start);

                if (unitType != UnitType.Unknown)
                {
                    // only known units after whitespace (CSS2)
                    AddToken(CssTokenType.Units, start, CS.Position - start);
                    return true;
                }
            }

            // Not a valid unit identifier, go back
            CS.Position = start;

            return false;
        }

        private bool HandleIdentifier()
        {
            // Makes a token for any identifier

            int start = CS.Position;

            if (SkipProgramIdFunction()) // progid:Foo.Bar(...)
            {
                AddToken(CssTokenType.Function, start, CS.Position - start);
            }
            else if (SkipIdentifier())
            {
                bool isFunction = false;
                if (CS.CurrentChar == '(')
                {
                    // Dev12 bug 744783 - Allow "foo(" but not "@foo(" to be a function token
                    CssToken prevToken = Tokens.Count > 0 ? Tokens[Tokens.Count - 1] : null;
                    if (prevToken == null ||
                        prevToken.TokenType != CssTokenType.At ||
                        prevToken.AfterEnd != start)
                    {
                        isFunction = true;
                    }
                }

                if (isFunction)
                {
                    // Go back to the start of the function name and check it against
                    // certain names (actually, only against "url")

                    int afterParen = CS.Position + 1;
                    CS.Position = start;
                    bool isUrl = TextHelpers.CompareCurrentDecodedString(CS, "url(", ignoreCase: true, matchLength: out _);
                    CS.Position = afterParen;

                    // Create a function token now, but it might need to change to a URL token
                    AddToken(CssTokenType.Function, start, CS.Position - start);
                    int functionTokenIndex = Tokens.Count - 1;

                    if (isUrl && HandleUrlParameter())
                    {
                        // The url() function parameter is valid, so change the function token type

                        CssToken token = Tokens[functionTokenIndex];
                        token.TokenType = CssTokenType.Url;
                        Tokens[functionTokenIndex] = token;
                    }
                }
                else
                {
                    AddToken(CssTokenType.Identifier, start, CS.Position - start);
                }
            }

            return start != CS.Position;
        }

        /// <summary>
        /// If the user puts \9 after an identifier, then we're going to treat that
        /// \9 as a comment. That workaround is used to make different values work in different browsers.
        /// Old IE browsers probably ignore the \9 even though it really should be part of the identifier.
        /// </summary>
        private bool HandleSlashNineHack()
        {
            if (AtSlashNineHack())
            {
                int start = CS.Position;

                CS.Advance(2);
                AddToken(CssTokenType.CommentText, start, CS.Position - start);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Call this only after skipping the chars for "url(".
        /// Returns true: A URI token was created
        /// Returns false: No new tokens were created, the URI was not valid
        /// </summary>
        private bool HandleUrlParameter()
        {
            int start = CS.Position;
            int startTokenCount = Tokens.Count;

            HandleWhitespaceAndComments();

            if (HandleString() == CssTokenType.Unknown)
            {
                // Unquoted URI, try and skip it
                int startURI = CS.Position;

                if (SkipUri())
                {
                    AddToken(CssTokenType.UnquotedUrlString, startURI, CS.Position - startURI);
                }
            }

            HandleWhitespaceAndComments();

            if (!IsValidUrlParameterEnd())
            {
                // Not a valid url(...) parameter, go back
                CS.Position = start;

                if (Tokens.Count > startTokenCount)
                {
                    // Get rid of any tokens that were added (comments, string, ...)

                    Tokens.RemoveRange(startTokenCount, Tokens.Count - startTokenCount);
                }
            }

            return start != CS.Position || CS.CurrentChar == ')';
        }

        protected virtual bool IsValidUrlParameterEnd()
        {
#if STRICT_URL_TOKEN
            return CS.CurrentChar == ')';
#else
            return true;
#endif
        }

        private bool HandleUnicodeRange()
        {
            // Makes a token for: "U+12??-12FF"

            int start = CS.Position;

            if (SkipUnicodeRange())
            {
                AddToken(CssTokenType.UnicodeRange, start, CS.Position - start);
                return true;
            }

            return false;
        }

        protected void AddToken(CssTokenType type, int start, int length)
        {
            CssToken token = new CssToken(type, start, length);
            Tokens.Add(token);
#if DEBUG
            token.DebugText = (type != CssTokenType.EndOfFile)
                ? CS.TextProvider.GetText(start, length)
                : "EOF";

            if (type != CssTokenType.WhiteSpace && token.DebugText.Length > 0)
            {
                // Tokens must not have whitespace at either end (that can break incremental tokenizing)
                Debug.Assert(
                    !TextHelper.IsWhiteSpace(token.DebugText[0]) &&
                    !TextHelper.IsWhiteSpace(token.DebugText[token.DebugText.Length - 1]));
            }
#endif
        }

        // The next set of functions are helpers to detect the
        // standard CSS2.1 tokenization macros as shown here:
        // http://www.w3.org/TR/CSS21/syndata.html

        protected static bool IsHexDigit(char ch)
        {
            return TextHelper.IsHexDigit(ch);
        }

        protected static bool IsNonAscii(char ch)
        {
            return ch >= 128;
        }

        protected bool AtName()
        {
            char ch = CS.CurrentChar;

            return AtNameStart() || ch == '-' || char.IsDigit(ch);
        }

        protected bool AtNameStart()
        {
            char ch = CS.CurrentChar;

            return char.IsLetter(ch) || ch == '_' || IsNonAscii(ch) || (ch == '\\' && AtEscape());
        }

        protected bool AtUnicodeRange()
        {
            return
                (CS.CurrentChar == 'u' || CS.CurrentChar == 'U') &&
                CS.Peek(1) == '+' &&
                (IsHexDigit(CS.Peek(2)) || CS.Peek(2) == '?');
        }

        protected bool AtEscape()
        {
            return TextHelpers.AtEscape(CS);
        }

        protected bool AtSlashNineHack()
        {
            // Use _cs.DecodedCurrentChar because it is smart enough to know that \93 is not a \9
            if (CS.CurrentChar == '\\' &&
                CS.Peek(1) == '9' &&
                TextHelpers.DecodeCurrentChar(CS).Char == 9)
            {
                return true;
            }

            return false;
        }

        protected bool AtScopeBlocker()
        {
            return
                ScopeBlockerText != null &&
                CS.CurrentChar == '/' &&
                CS.TextProvider.CompareTo(CS.Position, ScopeBlockerText, ignoreCase: false);
        }

        protected bool AtComment()
        {
            return CS.CurrentChar == '/' && CS.Peek(1) == '*';
        }

        /// <summary>
        /// Skips: "progid:Foo.Bar(foo=bar)"
        /// </summary>
        protected bool SkipProgramIdFunction()
        {
            int start = CS.Position;

            if ((CS.CurrentChar == 'p' || CS.CurrentChar == 'P') &&
                TextHelpers.CompareCurrentDecodedString(CS, "progid:", /* ignoreCase = */ true, out _))
            {
                CS.Advance(7);

                while (true)
                {
                    if (SkipIdentifier())
                    {
                        // part of the function name
                    }
                    else if (CS.CurrentChar == '.')
                    {
                        // still part of the function name
                        CS.Advance(1);
                    }
                    else if (CS.CurrentChar == '(')
                    {
                        // found the end of the function name
                        CS.Advance(1);
                        return true;
                    }
                    else
                    {
                        // bad function name
                        CS.Position = start;
                        break;
                    }
                }
            }

            return start != CS.Position;
        }

        protected bool SkipIdentifier()
        {
            int start = CS.Position;

            // Identifiers can start with one dash or two
            if (CS.CurrentChar == '-')
            {
                CS.Advance(1);

                if (CS.CurrentChar == '-')
                {
                    CS.Advance(1);
                }
            }

            if (!AtNameStart())
            {
                // Not a valid name
                CS.Position = start;

                return false;
            }
            else
            {
                if (AtEscape())
                {
                    SkipEscape();
                }
                else
                {
                    CS.Advance(1);
                }

                // The rest is just name chars
                SkipName();

                return true;
            }
        }

        protected bool SkipName()
        {
            int start = CS.Position;

            while (AtName())
            {
                if (AtEscape())
                {
                    if (AtSlashNineHack())
                    {
                        // Stop the name token at the start of the \9, which allows that CSS workaround to work:
                        //    color: red\9;
                        //    border-width: 50px\9;
                        break;
                    }
                    else
                    {
                        SkipEscape();
                    }
                }
                else
                {
                    CS.Advance(1);
                }
            }

            // In case the last escaped char had a space after it:
            SkipWhitespaceReverse();

            return start != CS.Position;
        }

        protected bool SkipUnicodeRange()
        {
            // Skip U+012???-012FFF

            // The rules for a Unicode range in the lexer are more relaxed than in the parser.
            // The lexer allows question marks anywhere within the first part of the range,
            // while the parser would consider "U+??99" to be invalid.
            //
            // Lexer: u\+[0-9a-f?]{1,6}(-[0-9a-f]{1,6})?
            // Parser: http://www.w3.org/TR/css3-webfonts/#character-range-the-unicode-range-descri

            int start = CS.Position;

            if (AtUnicodeRange())
            {
                CS.Advance(3); // Skip "U+X"

                for (int count = 1; count < 6; count++, CS.Advance(1))
                {
                    if (!IsHexDigit(CS.CurrentChar) && CS.CurrentChar != '?')
                    {
                        break;
                    }
                }

                if (CS.CurrentChar == '-' && IsHexDigit(CS.Peek(1)))
                {
                    CS.Advance(2); // Skip "-X"

                    for (int count = 1; count < 6; count++, CS.Advance(1))
                    {
                        if (!IsHexDigit(CS.CurrentChar))
                        {
                            break;
                        }
                    }
                }
            }

            return start != CS.Position;
        }

        protected bool SkipEscape()
        {
            return TextHelpers.SkipEscape(CS);
        }

        protected bool SkipNumber()
        {
            // Skips: 123.456

            int start = CS.Position;
            bool seenDot = false;
            bool seenDigit = false;

            while (true)
            {
                if (!char.IsDigit(CS.CurrentChar))
                {
                    if (!seenDot && CS.CurrentChar == '.' && char.IsDigit(CS.Peek(1)))
                    {
                        seenDot = true;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    seenDigit = true;
                }

                CS.Advance(1);
            }

            if (!seenDigit)
            {
                CS.Position = start;
            }

            return start != CS.Position;
        }

        protected CssTokenType SkipString()
        {
            // This can detect single and double quoted strings, spanning a single or multiple lines.
            // Also, unterminated strings are detected. The return value tells you the type:
            //
            // CssTokenType.Unknown (not a string at all)
            // CssTokenType.MultilineString
            // CssTokenType.String
            // CssTokenType.InvalidString (unterminated)

            CssTokenType tokenType = CssTokenType.Unknown;

            if (TextHelper.IsQuote(CS.CurrentChar))
            {
                // Guilty until proven valid
                tokenType = CssTokenType.InvalidString;

                char quote = CS.CurrentChar;
                bool multiLine = false;

                CS.Advance(1);

                while (!CS.IsAtEnd)
                {
                    if (AtScopeBlocker())
                    {
                        break;
                    }
                    else if (CS.CurrentChar == quote)
                    {
                        // Found matching end quote
                        tokenType = multiLine ? CssTokenType.MultilineString : CssTokenType.String;

                        CS.Advance(1);
                        break;
                    }
                    else if (TextHelper.IsNewLine(CS.CurrentChar))
                    {
                        break;
                    }
                    else if (AtEscape())
                    {
                        SkipEscape();
                    }
                    else if (CS.CurrentChar == '\\')
                    {
                        // must be an escaped line break
                        CS.Advance(1);

                        Debug.Assert(TextHelper.IsNewLine(CS.CurrentChar));
                        SkipNewLine();

                        multiLine = true;
                    }
                    else
                    {
                        CS.Advance(1);
                    }
                }

                if (tokenType == CssTokenType.InvalidString)
                {
                    SkipWhitespaceReverse();
                }
            }

            return tokenType;
        }

        protected bool SkipUri()
        {
            // The CSS spec uses this notation for URIs:
            //    ([!#$%&*-~]|{nonascii}|{escape})*
            // http://www.w3.org/TR/CSS21/syndata.html

            int start = CS.Position;

            while (!CS.IsAtEnd)
            {
                char ch = CS.CurrentChar;

                if (AtComment() || AtScopeBlocker())
                {
                    // Found the end
                    break;
                }
                else if (AtEscape())
                {
                    SkipEscape();
                }
                else if ((ch >= '*' && ch <= '~') ||
                    ch == '!' ||
                    ch == '#' ||
                    ch == '$' ||
                    ch == '%' ||
                    ch == '&' ||
                    IsNonAscii(ch))
                {
                    CS.Advance(1);
                }
                else
                {
                    // Found the end
                    break;
                }
            }

            if (CS.Position > start)
            {
                // In case the last escaped char had a space after it:
                SkipWhitespaceReverse();
            }

            return start != CS.Position;
        }

        protected bool SkipNewLine()
        {
            return TextHelpers.SkipNewLine(CS);
        }

        protected bool SkipWhitespace()
        {
            return TextHelpers.SkipWhitespace(CS);
        }

        protected bool SkipWhitespaceReverse()
        {
            return TextHelpers.SkipWhitespaceReverse(CS);
        }
    }
}
