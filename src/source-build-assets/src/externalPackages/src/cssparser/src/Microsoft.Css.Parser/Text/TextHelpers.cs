// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Diagnostics;

namespace Microsoft.Css.Parser.Text
{
    /// <summary>
    /// Helpers functions for dealing with text sources
    /// </summary>
    internal static class TextHelpers
    {
        public static bool AtUnicodeEscape(CharacterStream cs)
        {
            return cs.CurrentChar == '\\' && TextHelper.IsHexDigit(cs.Peek(1));
        }

        public static bool AtEscape(CharacterStream cs)
        {
            if (cs.CurrentChar == '\\')
            {
                switch (cs.Peek(1))
                {
                    case '\r':
                    case '\n':
                    case '\f':
                        return false;

                    default:
                        return true;
                }
            }

            return false;
        }

        public static bool AtEscapedNewLine(CharacterStream cs)
        {
            if (cs.CurrentChar == '\\')
            {
                switch (cs.Peek(1))
                {
                    case '\r':
                    case '\n':
                    case '\f':
                        return true;

                    default:
                        return false;
                }
            }

            return false;
        }

        public static bool SkipUnicodeEscape(CharacterStream cs)
        {
            // Skip: \012345

            if (AtUnicodeEscape(cs))
            {
                cs.Advance(2); // Skip "\X"

                // Skip up to six hex characters

                for (int count = 1; count < 6; count++, cs.Advance(1))
                {
                    if (!TextHelper.IsHexDigit(cs.CurrentChar))
                    {
                        break;
                    }
                }

                // Eat a single space or line after the unicode character

                if (TextHelper.IsNewLine(cs.CurrentChar))
                {
                    SkipNewLine(cs);
                }
                else if (TextHelper.IsWhiteSpace(cs.CurrentChar))
                {
                    cs.Advance(1);
                }

                return true;
            }

            return false;
        }

        public static bool SkipEscape(CharacterStream cs)
        {
            // Skips:
            // "\abc " - Unicode escape (notice that the trailing space is included)
            // "\X"   - Any other character escape (except line breaks)

            if (AtUnicodeEscape(cs))
            {
                return SkipUnicodeEscape(cs);
            }
            else if (AtEscape(cs))
            {
                cs.Advance(2);
                return true;
            }

            return false;
        }

        public static bool SkipNewLine(CharacterStream cs)
        {
            switch (cs.CurrentChar)
            {
                case '\r':
                    cs.Advance(1);

                    if (cs.CurrentChar == '\n')
                    {
                        // A "\r\n" pair is always treated as a single line break
                        cs.Advance(1);
                    }

                    return true;

                case '\n':
                case '\f':
                    cs.Advance(1);
                    return true;

                default:
                    return false;
            }
        }

        public static void SkipToEOL(CharacterStream cs)
        {
            while (!(cs.IsAtEnd || TextHelper.IsNewLine(cs.CurrentChar)))
            {
                cs.Advance(1);
            }
        }

        public static bool SkipWhitespace(CharacterStream cs)
        {
            int start = cs.Position;

            while (TextHelper.IsWhiteSpace(cs.CurrentChar))
            {
                cs.Advance(1);
            }

            return start != cs.Position;
        }

        public static bool SkipWhitespaceReverse(CharacterStream cs)
        {
            int start = cs.Position;

            while (TextHelper.IsWhiteSpace(cs.Peek(-1)))
            {
                cs.Advance(-1);
            }

            return start != cs.Position;
        }

        public static bool RangeCouldContainEncodedChars(ITextProvider textProvider, int start, int length)
        {
            for (int i = start; i < start + length; i++)
            {
                if (textProvider[i] == '\\')
                {
                    return true;
                }
            }

            return false;
        }

        internal static bool CompareCurrentDecodedString(CharacterStream stream, string compareText, bool ignoreCase, out int matchLength)
        {
#if SUPPORT_ENCODED_CSS
            matchLength = 0;

            if (AtEnd)
            {
                return compareText.Length == 0;
            }

            if (_textProvider.Length - _index < compareText.Length)
            {
                return false;
            }

            if (!TextHelpers.RangeCouldContainEncodedChars(_textProvider, _index, compareText.Length))
            {
                // No slashes, so no escapes, and a simple string comparison can be done

                matchLength = compareText.Length;
                return CompareCurrentString(compareText, ignoreCase);
            }

            // There's a slash in there somewhere, so decode every character
            // (this requires no memory allocations)

            int startPosition = Position;
            bool result = true;

            for (int i = 0; i < compareText.Length; i++)
            {
                char compareChar = compareText[i];
                DecodedChar decodedChar = DecodedCurrentChar;

                if (ignoreCase
                    ? (char.ToLowerInvariant(compareChar) == char.ToLowerInvariant(decodedChar.Char))
                    : (compareChar == decodedChar.Char))
                {
                    // Matching so far...
                    Advance(decodedChar.EncodedLength);
                }
                else
                {
                    result = false;
                    break;
                }
            }

            matchLength = Position - startPosition;
            Position = startPosition;

            return result;
#else
            matchLength = string.IsNullOrEmpty(compareText) ? 0 : compareText.Length;
            return stream.TextProvider.CompareTo(stream.Position, compareText, ignoreCase);
#endif
        }

        /// <summary>
        /// This is similar to string.Substring, but it deals with encoded unicode chars and escaped chars.
        /// Escaped line breaks are only valid within strings, so set "forStringToken" to true for strings.
        /// </summary>
        public static string DecodeText(ITextProvider textProvider, int start, int length, bool forStringToken)
        {
            if (RangeCouldContainEncodedChars(textProvider, start, length))
            {
                // Need to carefully investigate every character and decode it

                System.Text.StringBuilder sb = new System.Text.StringBuilder(length);
                CharacterStream cs = new CharacterStream(textProvider);

                for (cs.Position = start; cs.Position < start + length && !cs.IsAtEnd;)
                {
                    if (forStringToken && AtEscapedNewLine(cs))
                    {
                        // Ignore this line break within a string
                        cs.Advance(1);
                        SkipNewLine(cs);
                    }
                    else
                    {
                        DecodedChar decodedChar = TextHelpers.DecodeCurrentChar(cs);

                        if (decodedChar.RequiresUtf32)
                        {
                            // http://www.w3.org/TR/CSS21/syndata.html#characters
                            //
                            // If the number is outside the range allowed by Unicode (e.g., "\110000" is above the maximum 10FFFF
                            // allowed in current Unicode), the UA may replace the escape with the "replacement character" (U+FFFD).

                            int utf32 = decodedChar.CharUtf32;

                            if ((utf32 < 0) || (utf32 > 0x10FFFF))
                            {
                                utf32 = 0xFFFD;
                            }

                            sb.Append(char.ConvertFromUtf32(utf32));
                        }
                        else
                        {
                            sb.Append(decodedChar.Char);
                        }

                        cs.Advance(decodedChar.EncodedLength);
                    }
                }

                return sb.ToString();
            }
            else
            {
                // Nothing can possibly be encoded, so return the plain string

                return textProvider.GetText(start, length);
            }
        }

        public static DecodedChar DecodeCurrentChar(CharacterStream cs)
        {
            if (AtUnicodeEscape(cs))
            {
                // Find out how many hex chars are part of the encoded char
                int startPos = cs.Position;
                SkipUnicodeEscape(cs);

                int endPos = cs.Position;
                cs.Position = startPos;

                // Convert the hex characters into an integer
                int encodedLength = endPos - startPos;
                int decodedChar = 0;

                for (int i = 1; i < encodedLength; i++)
                {
                    char ch = cs.Peek(i);

                    if (TextHelper.IsHexDigit(ch))
                    {
                        decodedChar *= 16;

                        if (char.IsDigit(ch))
                        {
                            decodedChar += ch - '0';
                        }
                        else
                        {
                            decodedChar += char.ToLowerInvariant(ch) - 'a' + 10;
                        }
                    }
                    else
                    {
                        Debug.Assert(char.IsWhiteSpace(ch));
                        break;
                    }
                }

                return new DecodedChar(decodedChar, encodedLength);
            }
            else if (AtEscape(cs))
            {
                return new DecodedChar(cs.Peek(1), 2);
            }

            return new DecodedChar(cs.CurrentChar, 1);
        }
    }
}
