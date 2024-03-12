using System;

namespace Microsoft.ServiceProfiler.Agent.Utilities
{
    /// <summary>
    /// This code is derived from the Application Insights Profiler agent. It is included in this repo
    /// in order to validate ETW payload serialization in RichPayloadEventSource.
    /// </summary>
    internal static class PayloadParserUtilities
    {
        /// <summary>
        /// First try to get the size of the next string value payload. Then try to retrieve the corresponding string payload according to the size.
        /// </summary>
        /// <param name="value">Set to the string value payload when success.</param>
        public static unsafe bool TryParseNextLengthPrefixedUnicodeString(ref byte* ptr, byte* end, out string value)
        {
            short size;
            if (TryParseNextInt16(ref ptr, end, out size))
            {
                return TryParseNextUnicodeString(ref ptr, end, size, out value);
            }
            else
            {
                value = string.Empty;
                return false;
            }
        }

        /// <summary>
        /// Try to get the size of the next string value payload.
        /// </summary>
        public static unsafe bool TryParseNextInt16(ref byte* ptr, byte* end, out short value)
        {
            var afterPtr = ptr + sizeof(short);
            if (afterPtr <= end)
            {
                value = *((short*)ptr);
                ptr = afterPtr;
                return true;
            }
            else
            {
                value = 0;
                return false;
            }
        }

        /// <summary>
        /// Try to get the size of the next string value payload.
        /// </summary>
        public static unsafe bool TryParseNextInt32(ref byte* ptr, byte* end, out int value)
        {
            var afterPtr = ptr + sizeof(int);
            if (afterPtr <= end)
            {
                value = *((int*)ptr);
                ptr = afterPtr;
                return true;
            }
            else
            {
                value = 0;
                return false;
            }
        }

        /// <summary>
        /// Try to get the size of the next string value payload.
        /// </summary>
        public static unsafe bool TryParseNextInt64(ref byte* ptr, byte* end, out long value)
        {
            var afterPtr = ptr + sizeof(long);
            if (afterPtr <= end)
            {
                value = *((long*)ptr);
                ptr = afterPtr;
                return true;
            }
            else
            {
                value = 0;
                return false;
            }
        }

        /// <summary>
        /// Try to get the next string value from the payload.
        /// </summary>
        public static unsafe bool TryParseNextUnicodeString(ref byte* ptr, byte* end, short lengthInBytes, out string value)
        {
            // Size of bytes is different from size of chars.
            if (lengthInBytes >= 0)
            {
                var afterPtr = ptr + lengthInBytes;
                if (afterPtr <= end)
                {
                    value = new string((char*)ptr, startIndex: 0, length: lengthInBytes / sizeof(char));
                    ptr = afterPtr;
                    return true;
                }
            }

            value = null;
            return false;
        }

        /// <summary>
        /// Try to parse a <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="ptr">Pointer to data.</param>
        /// <param name="end">Pointer to end of data.</param>
        /// <param name="timeSpan">The timespan.</param>
        /// <returns>True if the timespan was parsed successfully.</returns>
        public static unsafe bool TryParseTimespan(ref byte* ptr, byte* end, out TimeSpan timeSpan)
        {
            // The TimeSpan may be serialized either as a string or an Int64 (tick count).
            // Use a heuristic to determine which.

            var originalPtr = ptr;

            if (!TryParseNextInt64(ref ptr, end, out long tickCount))
            {
                timeSpan = default(TimeSpan);
                return false;
            }

            if (IsLengthPrefixedTimeSpanString(tickCount))
            {
                ptr = originalPtr;
                if (TryParseNextLengthPrefixedUnicodeString(ref ptr, end, out string timeSpanString) && TimeSpan.TryParse(timeSpanString, out timeSpan))
                {
                    return true;
                }
            }

            timeSpan = TimeSpan.FromTicks(tickCount);
            return true;
        }

        /// <summary>
        /// Given 8 bytes read from the beginning of a payload field, determine if
        /// this could be interpreted as a valid TimeSpan string.
        /// </summary>
        /// <param name="val">The first 8 bytes read from the payload.</param>
        /// <returns>True if we think the payload might be a serialized TimeSpan string.</returns>
        /// <remarks>This is a heuristic. It's not perfect, but any real TimeSpan that
        /// gets through these checks would be quite abnormal. For example, the smallest
        /// value that would (incorrectly) be detected as a string is 0x002E002E00300010
        /// which is is "14986.03:57:30.0331536" or >41 years.
        /// </remarks>
        private static bool IsLengthPrefixedTimeSpanString(long val)
        {
            // Quick check to reject values that could never be valid strings.
            // The mask value here tests for ASCII range 0000-003F in the top three 'chars'
            // and odd numbers in the range 0000-003F (0 to 63) for the length.
            if (unchecked((ulong)val & 0xFFC0FFC0FFC0FFC1UL) != 0)
            {
                return false;
            }

            // The shortest TimeSpan string is 8 characters (16 bytes) "00:00:00"
            // The longest is 26 characters (52 bytes) "-10675199.02:48:05.4775808"
            // If this is a length-prefixed string, then the length should be an
            // even number between 16 and 50. Evenness was already checked with
            // the mask above.
            var length = (ushort)val;
            if (!(length >= 16 && length <= 52))
            {
                return false;
            }

            // The first character must be a digit or minus sign.
            var ch = (char)(val >> 16);
            if (!(IsDigit(ch) || ch == '-'))
            {
                return false;
            }

            // The 2nd must be a digit or a period
            ch = (char)(val >> 32);
            if (!(IsDigit(ch) || ch == '.'))
            {
                return false;
            }

            // The 3rd must be a digit, period or colon
            ch = (char)(val >> 48);
            if (!(IsDigit(ch) || ch == '.' || ch == ':'))
            {
                return false;
            }

            // We have a good candidate for a length-prefixed string.
            return true;
        }

        /// <summary>
        /// Is the given character a decimal digit (0 - 9)
        /// </summary>
        /// <param name="ch">The character.</param>
        /// <returns>True if it's a decimal digit.</returns>
        /// <remarks>This is not the same as <see cref="char.IsDigit(char)"/> because it does not include digits from the extended Unicode range.</remarks>
        private static bool IsDigit(char ch) => unchecked((uint)(ch - '0')) < 10u;
    }
}
