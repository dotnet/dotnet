namespace Microsoft.ApplicationInsights.AspNetCore.DiagnosticListeners
{
    /// <summary>
    /// Header names for requests / responses.
    /// </summary>
    internal static class RequestResponseHeaders
    {
        /// <summary>
        /// Request-Context header.
        /// </summary>
        public const string RequestContextHeader = "Request-Context";

        /// <summary>
        /// Source key in the request context header that is added by an application while making http requests and retrieved by the other application when processing incoming requests.
        /// </summary>
        public const string RequestContextSourceKey = "appId";

        /// <summary>
        /// Target key in the request context header that is added to the response and retrieved by the calling application when processing incoming responses.
        /// </summary>
        public const string RequestContextTargetKey = "appId"; // Although the name of Source and Target key is the same - appId. Conceptually they are different and hence, we intentionally have two constants here. Makes for better reading of the code.

        /// <summary>
        /// Request-Id header.
        /// </summary>
        public const string RequestIdHeader = "Request-Id";

        /// <summary>
        /// Correlation-Context header.
        /// </summary>
        public const string CorrelationContextHeader = "Correlation-Context";

        /// <summary>
        /// W3C traceparent header name.
        /// </summary>
        public const string TraceParentHeader = "traceparent";

        /// <summary>
        /// W3C tracestate header name.
        /// </summary>
        public const string TraceStateHeader = "tracestate";
    }
}
