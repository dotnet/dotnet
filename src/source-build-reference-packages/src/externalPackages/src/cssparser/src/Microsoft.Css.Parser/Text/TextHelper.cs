// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Microsoft.Css.Parser.Text
{
    /// <summary>
    /// A helper class exposing various helper functions that
    /// are used in formatting, smart indent and elsewhere else.
    /// </summary>
    // TODO: Merge this with TextHelper*s*.cs
    internal static class TextHelper
    {
        public static char[] EndOfLineChars = { '\r', '\n' };

        /// <summary>
        /// Detemines if there is nothing but whitespace between
        /// given position and preceding line break or beginning
        /// of the file.
        /// </summary>
        /// <param name="textProvider">Text provider</param>
        /// <param name="position">Position to check</param>
        public static bool IsNewLineBeforePosition(ITextProvider textProvider, int position)
        {
            // Walk backwards from the requested position
            for (int i = position - 1; i >= 0; i--)
            {
                char ch = textProvider[i];

                if (ch == '\n' || ch == '\r')
                {
                    return true;
                }

                if (!char.IsWhiteSpace(ch))
                {
                    break;
                }
            }

            return false;
        }

        public static bool IsNewLineBetweenPositions(ITextProvider textProvider, int start, int end)
        {
            Debug.Assert(start >= 0);
            Debug.Assert(end <= textProvider.Length);

            start = Math.Max(start, 0);
            end = Math.Min(textProvider.Length, end);

            for (int i = start; i < end; i++)
            {
                char ch = textProvider[i];

                if (ch == '\n' || ch == '\r')
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines if there is nothing but whitespace between
        /// given position and the next line break or end of file.
        /// </summary>
        /// <param name="textProvider">Text provider</param>
        /// <param name="position">Position to check</param>
        public static bool IsNewLineAfterPosition(ITextProvider textProvider, int position)
        {
            // Walk backwards from the artifact position
            for (int i = position; i < textProvider.Length; i++)
            {
                char ch = textProvider[i];

                if (ch == '\n' || ch == '\r')
                {
                    return true;
                }

                if (!char.IsWhiteSpace(ch))
                {
                    break;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines if there is nothing but whitespace between
        /// given positions (end is non-inclusive).
        /// </summary>
        /// <param name="textProvider">Text provider</param>
        /// <param name="position">Start position (inclusive)</param>
        /// <param name="position">End position (non-inclusive)</param>
        public static bool IsWhitespaceOnlyBetweenPositions(ITextProvider textProvider, int start, int end)
        {
            end = Math.Min(textProvider.Length, end);
            for (int i = start; i < end; i++)
            {
                char ch = textProvider[i];

                if (!char.IsWhiteSpace(ch))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Splits string into lines based on line breaks
        /// </summary>
        public static IList<string> SplitTextIntoLines(string text)
        {
            List<string> lines = new List<string>();
            int lineStart = 0;

            for (int i = 0; i < text.Length; i++)
            {
                char ch = text[i];
                if (ch == '\r' || ch == '\n')
                {
                    lines.Add(text.Substring(lineStart, i - lineStart));

                    // Skip '\n' but only in "\r\n" sequence.
                    if (ch == '\r' && i + 1 < text.Length && text[i + 1] == '\n')
                    {
                        i++;
                    }

                    lineStart = i + 1;
                }
            }

            lines.Add(text.Substring(lineStart, text.Length - lineStart));

            return lines;
        }

        public static IList<string> SplitTextIntoLinesWithNewLines(string text)
        {
            List<string> lines = new List<string>();
            int lineStart = 0;

            for (int i = 0; i < text.Length; i++)
            {
                char ch = text[i];
                if (ch == '\r' || ch == '\n')
                {
                    lines.Add(text.Substring(lineStart, i - lineStart));

                    // Skip '\n' but only in "\r\n" sequence.
                    if (ch == '\r' && i + 1 < text.Length && text[i + 1] == '\n')
                    {
                        lines.Add("\r\n");
                        i++;
                    }
                    else
                    {
                        // CodeAnalysis complains if an IFormatProvider isn't passed in. However, the Char.ToString
                        //   implementation doesn't use the provider, so we'll just pass in null.
                        lines.Add(ch.ToString(provider: null));
                    }

                    lineStart = i + 1;
                }
            }

            lines.Add(text.Substring(lineStart, text.Length - lineStart));

            return lines;
        }

        public static int GetNewLineCount(string text)
        {
            int newLineCount = 0;

            for (int i = 0; i < text.Length; i++)
            {
                char ch = text[i];

                if (ch == '\r' || ch == '\n')
                {
                    // Skip '\n' but only in "\r\n" sequence.
                    if (ch == '\r' && i + 1 < text.Length && text[i + 1] == '\n')
                    {
                        i++;
                    }

                    newLineCount++;
                }
            }

            return newLineCount;
        }

        public static string ConvertTabsToSpaces(string text, int tabSize)
        {
            StringBuilder sb = new StringBuilder(text.Length);
            int charsSoFar = 0;

            for (int i = 0; i < text.Length; i++)
            {
                char ch = text[i];

                if (ch == '\t')
                {
                    int spaces = tabSize - (charsSoFar % tabSize);
                    sb.Append(' ', spaces);
                    charsSoFar = 0;
                }
                else if (ch == '\r' || ch == '\n')
                {
                    charsSoFar = 0;
                    sb.Append(ch);
                }
                else
                {
                    charsSoFar++;
                    charsSoFar %= tabSize;
                    sb.Append(ch);
                }
            }

            return sb.ToString();
        }

        internal static bool IsDigit(char ch)
        {
            return ch >= '0' && ch <= '9';
        }

        internal static bool IsHexDigit(char ch)
        {
            return
                (ch >= '0' && ch <= '9') ||
                (ch >= 'a' && ch <= 'f') ||
                (ch >= 'A' && ch <= 'F');
        }

        internal static bool IsAnsiLetter(char ch)
        {
            return (ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z');
        }

        internal static bool IsWhiteSpace(char ch)
        {
            // Char.IsWhiteSpace is slow
            return (ch == ' ' || ch == '\t' || ch == '\r' ||
                    ch == '\n' || ch == '\f');
        }

        /// <summary>
        /// Returns true if the Unicode character is a whitespace or a newline character.
        /// </summary>
        /// <param name="ch">The Unicode character.</param>
        internal static bool IsWhitespaceOrNewLine(char c)
        {
            return IsWhitespaceAndNotNewLine(c) || IsNewLine(c);
        }

        /// <summary>
        /// Returns true if the Unicode character represents a whitespace.
        /// </summary>
        /// <param name="ch">The Unicode character.</param>
        internal static bool IsWhitespaceAndNotNewLine(char ch)
        {
            // whitespace:
            //   Any character with Unicode class Zs
            //   Horizontal tab character (U+0009)
            //   Vertical tab character (U+000B)
            //   Form feed character (U+000C)

            // Space and no-break space are the only space separators (Zs) in ASCII range

            // Minor optimization for most likely case
            if (ch < 127)
            {
                return ch == ' '
                    || ch == '\t'
                    || ch == '\v'
                    || ch == '\f';
            }

            return ch == '\u00A0' // NO-BREAK SPACE
                || ch == '\uFEFF' // BOM
                || ch == '\u001A' // ^Z
                || (ch > 255 && CharUnicodeInfo.GetUnicodeCategory(ch) == UnicodeCategory.SpaceSeparator);
        }

        /// <summary>
        /// Returns true if the Unicode character is a newline character.
        /// </summary>
        /// <param name="ch">The Unicode character.</param>
        internal static bool IsNewLine(char ch)
        {
            // new-line-character:
            //   Carriage return character (U+000D)
            //   Line feed character (U+000A)
            //   Next line character (U+0085)
            //   Line separator character (U+2028)
            //   Paragraph separator character (U+2029)

            if (ch < 127)
            {
                return ch == '\r'
                    || ch == '\n';
            }

            return ch == '\u0085'
                || ch == '\u2028'
                || ch == '\u2029';
        }

        internal static bool IsQuote(char ch)
        {
            return (ch == '\'' || ch == '\"');
        }
    }
}
