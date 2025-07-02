namespace Microsoft.ApplicationInsights.Extensibility.W3C
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    /// <summary>
    /// W3C distributed tracing utilities.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class W3CUtilities
    {
        private static readonly uint[] Lookup32 = CreateLookup32();
        private static readonly Regex TraceIdRegex = new Regex("^[a-f0-9]{32}$", RegexOptions.Compiled);

        /// <summary>
        /// Generates random trace Id as per W3C Distributed tracing specification.
        /// https://github.com/w3c/distributed-tracing/blob/master/trace_context/HTTP_HEADER_FORMAT.md#trace-id .
        /// </summary>
        /// <returns>Random 16 bytes array encoded as hex string.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use ActivityTraceId.CreateRandom().ToHexString() instead.")]
        public static string GenerateTraceId()
        {
            return ActivityTraceId.CreateRandom().ToHexString();
        }

        /// <summary>
        /// Checks if the given string is a valid trace-id as per W3C Specs.
        /// https://github.com/w3c/distributed-tracing/blob/master/trace_context/HTTP_HEADER_FORMAT.md#trace-id .
        /// </summary>
        /// <returns>true if valid w3c trace id, otherwise false.</returns>
        internal static bool IsCompatibleW3CTraceId(string traceId)
        {
            return TraceIdRegex.IsMatch(traceId);
        }

        /// <summary>
        /// Generates random span Id as per W3C Distributed tracing specification.
        /// https://github.com/w3c/distributed-tracing/blob/master/trace_context/HTTP_HEADER_FORMAT.md#span-id .
        /// </summary>
        /// <returns>Random 8 bytes array encoded as hex string.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static string GenerateSpanId()
        {
            return GenerateId(BitConverter.GetBytes(WeakConcurrentRandom.Instance.Next()), 0, 8);
        }

        /// <summary>
        /// Converts byte array to hex lower case string.
        /// </summary>
        /// <returns>Array encoded as hex string.</returns>
        private static string GenerateId(byte[] bytes, int start, int length)
        {
            // See https://stackoverflow.com/questions/311165/how-do-you-convert-a-byte-array-to-a-hexadecimal-string-and-vice-versa/24343727#24343727
            var result = new char[length * 2];
            for (int i = start; i < start + length; i++)
            {
                var val = Lookup32[bytes[i]];
                result[2 * i] = (char)val;
                result[(2 * i) + 1] = (char)(val >> 16);
            }

            return new string(result);
        }

        private static uint[] CreateLookup32()
        {
            // See https://stackoverflow.com/questions/311165/how-do-you-convert-a-byte-array-to-a-hexadecimal-string-and-vice-versa/24343727#24343727
            var result = new uint[256];
            for (int i = 0; i < 256; i++)
            {
                string s = i.ToString("x2", CultureInfo.InvariantCulture);
                result[i] = ((uint)s[0]) + ((uint)s[1] << 16);
            }

            return result;
        }
    }
}
