#if DEPENDENCY_COLLECTOR
    namespace Microsoft.ApplicationInsights.Common
#else
namespace Microsoft.ApplicationInsights.Common.Internal
#endif
{
    /// <summary>
    /// These values are listed to guard against malicious injections by limiting the max size allowed in an HTTP Response.
    /// These max limits are intentionally exaggerated to allow for unexpected responses, while still guarding against unreasonably large responses.
    /// Example: While a 32 character response may be expected, 50 characters may be permitted while a 10,000 character response would be unreasonable and malicious.
    /// </summary>
#if DEPENDENCY_COLLECTOR
    public 
#else
    internal
#endif
    static class InjectionGuardConstants
    {
        /// <summary>
        /// Max length of AppId allowed in response from Breeze.
        /// </summary>
        public const int AppIdMaxLength = 50;

        /// <summary>
        /// Max length of incoming Request Header value allowed.
        /// </summary>
        public const int RequestHeaderMaxLength = 1024;

        /// <summary>
        /// Max length of context header key.
        /// </summary>
        public const int ContextHeaderKeyMaxLength = 50;

        /// <summary>
        /// Max length of context header value.
        /// </summary>
        public const int ContextHeaderValueMaxLength = 1024;

        /// <summary>
        /// Max length of traceparent header value.
        /// </summary>
        public const int TraceParentHeaderMaxLength = 55;

        /// <summary>
        /// Max length of tracestate header value string.
        /// </summary>
        public const int TraceStateHeaderMaxLength = 512;

        /// <summary>
        /// Max number of key value pairs in the tracestate header.
        /// </summary>
        public const int TraceStateMaxPairs = 32;

        /// <summary>
        /// Max length of incoming Response Header value allowed.
        /// </summary>
        public const int QuickPulseResponseHeaderMaxLength = 1024;
    }
}
