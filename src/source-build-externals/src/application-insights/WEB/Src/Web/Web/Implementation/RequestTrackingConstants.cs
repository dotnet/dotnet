namespace Microsoft.ApplicationInsights.Web.Implementation
{
    /// <summary>
    /// Request tracking constants and keys.
    /// </summary>
    internal static class RequestTrackingConstants
    {
        /// <summary>
        /// Name of the HttpContext item containing RequestTelemetry object.
        /// </summary>
        internal const string RequestTelemetryItemName = "Microsoft.ApplicationInsights.RequestTelemetry";

        /// <summary>
        /// The name of the cookie which holds authenticated user context information.
        /// </summary>
        internal const string WebAuthenticatedUserCookieName = "ai_authUser";
    }
}
