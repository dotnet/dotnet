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
    internal static class ExperimentalConstants
    {
        /// <summary>
        /// This is used to defer setting properties on RequestTelemetry until after Sampling.
        /// QuickPulse expects these properties so we have to set them here as well.
        /// Used to set QuickPulseTelemetryProcessor.EvaluateDisabledTrackingProperties and RequestTrackingTelemetryModule.DisableTrackingProperties.
        /// </summary>
        public const string DeferRequestTrackingProperties = nameof(DeferRequestTrackingProperties);
    }
}
