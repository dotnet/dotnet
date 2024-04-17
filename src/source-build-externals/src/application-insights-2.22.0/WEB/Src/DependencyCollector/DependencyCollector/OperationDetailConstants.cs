namespace Microsoft.ApplicationInsights.DependencyCollector
{
    /// <summary>
    /// Constants for operation details.
    /// </summary>
    public static class OperationDetailConstants
    {
        /// <summary>
        /// Constant for HTTP request operation detail name.
        /// </summary>
        public const string HttpRequestOperationDetailName = "HttpRequest";

        /// <summary>
        /// Constant for HTTP response operation detail name.
        /// </summary>
        public const string HttpResponseOperationDetailName = "HttpResponse";

        /// <summary>
        /// Constant for HTTP response header operation detail name.
        /// </summary>
        public const string HttpResponseHeadersOperationDetailName = "HttpResponseHeaders";

        /// <summary>
        /// Constant for SQL command operation detail name.
        /// </summary>
        public const string SqlCommandOperationDetailName = "SqlCommand";
    }
}
