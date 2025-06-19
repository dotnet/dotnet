#if DEPENDENCY_COLLECTOR
    namespace Microsoft.ApplicationInsights.Common
#else
    namespace Microsoft.ApplicationInsights.Common.Internal
#endif
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    /// <summary>
    /// Generic functions to perform common operations on a string.
    /// </summary>
#if DEPENDENCY_COLLECTOR
    public
#else
    internal
#endif
    static class StringUtilities
    {
        /// <summary>
        /// Check a strings length and trim to a max length if needed.
        /// </summary>
        public static string EnforceMaxLength(string input, int maxLength)
        {
            Debug.Assert(
                maxLength > 0,
                string.Format(CultureInfo.CurrentCulture, "{0} must be greater than 0", nameof(maxLength)));

            if (input != null && input.Length > maxLength)
            {
                input = input.Substring(0, maxLength);
            }

            return input;
        }

        /// <summary>
        /// Generates random trace Id as per W3C Distributed tracing specification.
        /// https://github.com/w3c/distributed-tracing/blob/master/trace_context/HTTP_HEADER_FORMAT.md#trace-id .
        /// </summary>
        /// <returns>Random 16 bytes array encoded as hex string.</returns>
        [Obsolete("Use System.Diagnostics.ActivityTraceId.CreateRandom().ToHexString() instead.")]
        public static string GenerateTraceId()
        {
            return ActivityTraceId.CreateRandom().ToHexString();
        }

        /// <summary>
        /// Generates random span Id as per W3C Distributed tracing specification.
        /// https://github.com/w3c/distributed-tracing/blob/master/trace_context/HTTP_HEADER_FORMAT.md#span-id .
        /// </summary>
        /// <returns>Random 8 bytes array encoded as hex string.</returns>
        [Obsolete("Use System.Diagnostics.ActivitySpanId.CreateRandom().ToHexString() instead.")]
        public static string GenerateSpanId()
        {
            return ActivitySpanId.CreateRandom().ToHexString();
        }

        /// <summary>
        /// Formats trace Id and span Id into valid Request-Id: |trace.span.
        /// </summary>
        /// <param name="traceId">Trace Id.</param>
        /// <param name="spanId">Span id.</param>
        /// <returns>valid Request-Id.</returns>
        [Obsolete("Obsolete, use spanId instead")]
        public static string FormatRequestId(string traceId, string spanId)
        {
            return spanId;
        }
    }
}
