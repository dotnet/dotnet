namespace Microsoft.ApplicationInsights.Common
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
        public const string RequestContextCorrelationSourceKey = "appId";

        /// <summary>
        /// Target key in the request context header that is added to the response and retrieved by the calling application when processing incoming responses.
        /// </summary>
        public const string RequestContextCorrelationTargetKey = "appId"; // Although the name of Source and Target key is the same - appId. Conceptually they are different and hence, we intentionally have two constants here. Makes for better reading of the code.

        /// <summary>
        /// Legacy parent Id header.
        /// </summary>
        public const string StandardParentIdHeader = "x-ms-request-id";

        /// <summary>
        /// Legacy root id header.
        /// </summary>
        public const string StandardRootIdHeader = "x-ms-request-root-id";

        /// <summary>
        /// Standard Request-Id Id header.
        /// </summary>
        public const string RequestIdHeader = "Request-Id";

        /// <summary>
        /// Standard Correlation-Context header.
        /// </summary>
        public const string CorrelationContextHeader = "Correlation-Context";

        /// <summary>
        /// Access-Control-Expose-Headers header indicates which headers can be exposed as part of the response by listing their names.
        /// Should contain Request-Context value that will allow reading Request-Context in JavaScript SDK on Browser side.
        /// </summary>
        public const string AccessControlExposeHeadersHeader = "Access-Control-Expose-Headers";
    }
}
