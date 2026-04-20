// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Diagnostics;

namespace Microsoft.Css.Parser.Text
{
    internal static class TextRange
    {
        internal static string GetDecodedText(int start, int length, ITextProvider textProvider, bool forStringToken)
        {
            Debug.Assert(start >= 0 && (length == 0 || start + length <= textProvider.Length));

            return (length > 0 && textProvider != null)
                ? TextHelpers.DecodeText(textProvider, start, length, forStringToken)
                : string.Empty;
        }

        internal static bool ContainsChar(int start, int length, int index)
        {
            return start <= index && index < start + length;
        }

        internal static bool Touches(int start, int length, int pos)
        {
            return start <= pos && pos <= start + length;
        }

        internal static bool Intersects(int rangeStart, int rangeLength, int start, int length)
        {
            return start + length > rangeStart && start < rangeStart + rangeLength;
        }

        internal static bool Compare(int start, int length, ITextProvider textProvider, string compareText, bool ignoreCase)
        {
            if (textProvider == null || length == 0)
            {
                return string.IsNullOrEmpty(compareText);
            }
            else
            {
                return length == compareText.Length &&
                    textProvider.CompareTo(start, compareText, ignoreCase);
            }
        }

        internal static bool CompareDecoded(int start, int length, ITextProvider textProvider, string compareText, bool ignoreCase)
        {
#if SUPPORT_ENCODED_CSS
            if (textProvider == null || _length == 0)
            {
                return string.IsNullOrEmpty(compareText);
            }
            else
            {
                CharStream cs = new CharStream(textProvider);
                cs.Position = _start;

                int matchLength = 0;

                return TextHelpers.CompareCurrentDecodedString(cs, compareText, ignoreCase, out matchLength) &&
                    matchLength == Length;
            }
#else
            return Compare(start, length, textProvider, compareText, ignoreCase);
#endif
        }
    }
}
