namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse
{
    /// <summary>
    /// Constants related to quick pulse service.
    /// </summary>
    internal static class QuickPulseConstants
    {
        internal const string AuthorizationHeaderName = "Authorization";

        internal const string AuthorizationTokenPrefix = "Bearer ";

        /// <summary>
        /// Subscribed header.
        /// </summary>
        internal const string XMsQpsSubscribedHeaderName = "x-ms-qps-subscribed";

        /// <summary>
        /// Transmission time header.
        /// </summary>
        internal const string XMsQpsTransmissionTimeHeaderName = "x-ms-qps-transmission-time";

        /// <summary>
        /// Configuration ETag header.
        /// </summary>
        internal const string XMsQpsConfigurationETagHeaderName = "x-ms-qps-configuration-etag";

        /// <summary>
        /// Instance name header.
        /// </summary>
        internal const string XMsQpsInstanceNameHeaderName = "x-ms-qps-instance-name";

        /// <summary>
        /// Stream id header.
        /// </summary>
        internal const string XMsQpsStreamIdHeaderName = "x-ms-qps-stream-id";

        /// <summary>
        /// Machine name header.
        /// </summary>
        internal const string XMsQpsMachineNameHeaderName = "x-ms-qps-machine-name";

        /// <summary>
        /// Role name header.
        /// </summary>
        internal const string XMsQpsRoleNameHeaderName = "x-ms-qps-role-name";

        /// <summary>
        /// Invariant version header.
        /// </summary>
        internal const string XMsQpsInvariantVersionHeaderName = "x-ms-qps-invariant-version";

        /// <summary>
        /// Authentication API key.
        /// </summary>
        internal const string XMsQpsAuthApiKeyHeaderName = "x-ms-qps-auth-api-key";

        /// <summary>
        /// Service polling interval hint.
        /// </summary>
        /// <remarks>Contains a recommended time (in milliseconds) before we ping the service again.</remarks>
        internal const string XMsQpsServicePollingIntervalHintHeaderName = "x-ms-qps-service-polling-interval-hint";

        /// <summary>
        /// Service endpoint redirect.
        /// </summary>
        /// <remarks>Contains a URI of the service endpoint we must permanently use <b>for the particular resource</b>.</remarks>
        internal const string XMsQpsServiceEndpointRedirectHeaderName = "x-ms-qps-service-endpoint-redirect";

        /// <summary>
        /// The following authentication headers must be received and submitted back to the service with no modification.
        /// </summary>
        internal static readonly string[] XMsQpsAuthOpaqueHeaderNames =
        {
            "x-ms-qps-auth-app-id",
            "x-ms-qps-auth-status",
            "x-ms-qps-auth-token-expiry",
            "x-ms-qps-auth-token-signature",
            "x-ms-qps-auth-token-signature-alg",
        };
    }
}
