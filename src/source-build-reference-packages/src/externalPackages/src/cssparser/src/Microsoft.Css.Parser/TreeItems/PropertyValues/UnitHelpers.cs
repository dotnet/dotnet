// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Text;

namespace Microsoft.Css.Parser.TreeItems.PropertyValues
{
    internal static class UnitHelpers
    {
        // Length units

        private static bool IsCh(char[] ch, int count)
        {
            return count == 2 && (ch[0] == 'c' || ch[0] == 'C') && (ch[1] == 'h' || ch[1] == 'H');
        }

        private static bool IsEm(char[] ch, int count)
        {
            return count == 2 && (ch[0] == 'e' || ch[0] == 'E') && (ch[1] == 'm' || ch[1] == 'M');
        }

        private static bool IsRem(char[] ch, int count)
        {
            return count == 3 && (ch[0] == 'r' || ch[0] == 'R') && (ch[1] == 'e' || ch[1] == 'E') && (ch[2] == 'm' || ch[2] == 'M');
        }

        private static bool IsEx(char[] ch, int count)
        {
            return count == 2 && (ch[0] == 'e' || ch[0] == 'E') && (ch[1] == 'x' || ch[1] == 'X');
        }

        private static bool IsPx(char[] ch, int count)
        {
            return count == 2 && (ch[0] == 'p' || ch[0] == 'P') && (ch[1] == 'x' || ch[1] == 'X');
        }

        private static bool IsCm(char[] ch, int count)
        {
            return count == 2 && (ch[0] == 'c' || ch[0] == 'C') && (ch[1] == 'm' || ch[1] == 'M');
        }

        private static bool IsMm(char[] ch, int count)
        {
            return count == 2 && (ch[0] == 'm' || ch[0] == 'M') && (ch[1] == 'm' || ch[1] == 'M');
        }

        private static bool IsIn(char[] ch, int count)
        {
            return count == 2 && (ch[0] == 'i' || ch[0] == 'I') && (ch[1] == 'n' || ch[1] == 'N');
        }

        private static bool IsPt(char[] ch, int count)
        {
            return count == 2 && (ch[0] == 'p' || ch[0] == 'P') && (ch[1] == 't' || ch[1] == 'T');
        }

        private static bool IsPc(char[] ch, int count)
        {
            return count == 2 && (ch[0] == 'p' || ch[0] == 'P') && (ch[1] == 'c' || ch[1] == 'C');
        }

        // Angle units

        private static bool IsDeg(char[] ch, int count)
        {
            return count == 3 && (ch[0] == 'd' || ch[0] == 'D') && (ch[1] == 'e' || ch[1] == 'E') && (ch[2] == 'g' || ch[2] == 'G');
        }

        private static bool IsRad(char[] ch, int count)
        {
            return count == 3 && (ch[0] == 'r' || ch[0] == 'R') && (ch[1] == 'a' || ch[1] == 'A') && (ch[2] == 'd' || ch[2] == 'D');
        }

        private static bool IsGrad(char[] ch, int count)
        {
            return count == 4 && (ch[0] == 'g' || ch[0] == 'G') && (ch[1] == 'r' || ch[1] == 'R') && (ch[2] == 'a' || ch[2] == 'A') && (ch[3] == 'd' || ch[3] == 'D');
        }

        private static bool IsTurn(char[] ch, int count)
        {
            return count == 4 && (ch[0] == 't' || ch[0] == 'T') && (ch[1] == 'u' || ch[1] == 'U') && (ch[2] == 'r' || ch[2] == 'R') && (ch[3] == 'n' || ch[3] == 'N');
        }

        // Frequency units

        private static bool IsMs(char[] ch, int count)
        {
            return count == 2 && (ch[0] == 'm' || ch[0] == 'M') && (ch[1] == 's' || ch[1] == 'S');
        }

        private static bool IsS(char[] ch, int count)
        {
            return count == 1 && (ch[0] == 's' || ch[0] == 'S');
        }

        private static bool IsHz(char[] ch, int count)
        {
            return count == 2 && (ch[0] == 'h' || ch[0] == 'H') && (ch[1] == 'z' || ch[1] == 'Z');
        }

        private static bool IsKHz(char[] ch, int count)
        {
            return count == 3 && (ch[0] == 'k' || ch[0] == 'K') && (ch[1] == 'h' || ch[1] == 'H') && (ch[2] == 'z' || ch[2] == 'Z');
        }

        // Resolution units

        private static bool IsDpi(char[] ch, int count)
        {
            return count == 3 && (ch[0] == 'd' || ch[0] == 'D') && (ch[1] == 'p' || ch[1] == 'P') && (ch[2] == 'i' || ch[2] == 'I');
        }

        private static bool IsDpcm(char[] ch, int count)
        {
            return count == 4 && (ch[0] == 'd' || ch[0] == 'D') && (ch[1] == 'p' || ch[1] == 'P') && (ch[2] == 'c' || ch[2] == 'C') && (ch[3] == 'm' || ch[3] == 'M');
        }

        private static bool IsDppx(char[] ch, int count)
        {
            return count == 4 && (ch[0] == 'd' || ch[0] == 'D') && (ch[1] == 'p' || ch[1] == 'P') && (ch[2] == 'p' || ch[2] == 'P') && (ch[3] == 'x' || ch[3] == 'X');
        }

        // Semitones units

        private static bool IsSt(char[] ch, int count)
        {
            return count == 2 && (ch[0] == 's' || ch[0] == 'S') && (ch[1] == 't' || ch[1] == 'T');
        }

        // Grid units

        private static bool IsGd(char[] ch, int count)
        {
            return count == 2 && (ch[0] == 'g' || ch[0] == 'G') && (ch[1] == 'd' || ch[1] == 'D');
        }

        private static bool IsGr(char[] ch, int count)
        {
            return count == 2 && (ch[0] == 'g' || ch[0] == 'G') && (ch[1] == 'r' || ch[1] == 'R');
        }

        private static bool IsFr(char[] ch, int count)
        {
            return count == 2 && (ch[0] == 'f' || ch[0] == 'F') && (ch[1] == 'r' || ch[1] == 'R');
        }

        // Viewport units

        private static bool IsVmin(char[] ch, int count)
        {
            return count == 4 && (ch[0] == 'v' || ch[0] == 'V') && (ch[1] == 'm' || ch[1] == 'M') && (ch[2] == 'i' || ch[2] == 'I') && (ch[3] == 'n' || ch[3] == 'N');
        }

        private static bool IsVmax(char[] ch, int count)
        {
            return count == 4 && (ch[0] == 'v' || ch[0] == 'V') && (ch[1] == 'm' || ch[1] == 'M') && (ch[2] == 'a' || ch[2] == 'A') && (ch[3] == 'x' || ch[3] == 'X');
        }

        private static bool IsVh(char[] ch, int count)
        {
            return count == 2 && (ch[0] == 'v' || ch[0] == 'V') && (ch[1] == 'h' || ch[1] == 'H');
        }

        private static bool IsVw(char[] ch, int count)
        {
            return count == 2 && (ch[0] == 'v' || ch[0] == 'V') && (ch[1] == 'w' || ch[1] == 'W');
        }

        // Volume units

        private static bool IsDb(char[] ch, int count)
        {
            return count == 2 && (ch[0] == 'd' || ch[0] == 'D') && (ch[1] == 'b' || ch[1] == 'B');
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        internal static UnitType GetUnitType(ITextProvider textProvider, int start, int length)
        {
            if (length == 0)
            {
                return UnitType.Unknown;
            }

            char[] unitChars = textProvider.GetText(start, length).ToCharArray();
            int count = unitChars.Length;

            if (count == 1 && unitChars[0] == '%')
            {
                return UnitType.Percentage;
            }

            if (IsEm(unitChars, count) ||
                IsRem(unitChars, count) ||
                IsEx(unitChars, count) ||
                IsCh(unitChars, count))
            {
                return UnitType.Length;
            }

            if (IsPx(unitChars, count) ||
                IsCm(unitChars, count) ||
                IsMm(unitChars, count) ||
                IsIn(unitChars, count))
            {
                return UnitType.Length;
            }

            if (IsPt(unitChars, count) ||
                IsPc(unitChars, count))
            {
                return UnitType.Length;
            }

            if (IsDeg(unitChars, count) ||
                IsRad(unitChars, count) ||
                IsGrad(unitChars, count) ||
                IsTurn(unitChars, count))
            {
                return UnitType.Angle;
            }

            if (IsMs(unitChars, count) ||
                IsS(unitChars, count))
            {
                return UnitType.Time;
            }

            if (IsHz(unitChars, count) ||
                IsKHz(unitChars, count))
            {
                return UnitType.Frequency;
            }

            if (IsDpi(unitChars, count) ||
                IsDpcm(unitChars, count) ||
                IsDppx(unitChars, count))
            {
                return UnitType.Resolution;
            }

            if (IsSt(unitChars, count))
            {
                return UnitType.Semitones;
            }

            if (IsGd(unitChars, count) ||
                IsGr(unitChars, count) ||
                IsFr(unitChars, count))
            {
                return UnitType.Grid;
            }

            if (IsVmin(unitChars, count) ||
                IsVmax(unitChars, count) ||
                IsVh(unitChars, count) ||
                IsVw(unitChars, count))
            {
                return UnitType.Viewport;
            }

            if (IsDb(unitChars, count))
            {
                return UnitType.Volume;
            }

            return UnitType.Unknown;
        }
    }
}
