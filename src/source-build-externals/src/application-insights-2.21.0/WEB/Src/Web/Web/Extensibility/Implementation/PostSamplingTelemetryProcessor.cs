namespace Microsoft.ApplicationInsights.Web.Extensibility.Implementation
{
    using System.Web;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
    /// PostSamplingTelemetryProcessor evaluates deferred properties.
    /// It is intended to be used with <see cref="RequestTrackingTelemetryModule.DisableTrackingProperties"/> after Sampling.
    /// </summary>
    /// <remarks>
    /// This feature is still being evaluated and not recommended for end users.
    /// </remarks>
    internal class PostSamplingTelemetryProcessor : ITelemetryProcessor
    {
        private readonly ITelemetryProcessor nextProcessorInPipeline;
        private TelemetryConfiguration telemetryConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostSamplingTelemetryProcessor"/> class.
        /// </summary>
        /// <param name="nextProcessorInPipeline">The next TelemetryProcessor in the chain.</param>
        public PostSamplingTelemetryProcessor(ITelemetryProcessor nextProcessorInPipeline)
        {
            this.nextProcessorInPipeline = nextProcessorInPipeline;
        }

        private TelemetryConfiguration TelemetryConfiguration => this.telemetryConfiguration ?? (this.telemetryConfiguration = TelemetryConfiguration.Active);

        /// <inheritdoc />
        public void Process(ITelemetry item)
        {
            if (item is RequestTelemetry requestTelemetry)
            {
                var request = HttpContext.Current?.Request;
                RequestTrackingUtilities.UpdateRequestTelemetryFromRequest(requestTelemetry, request, this.telemetryConfiguration?.ApplicationIdProvider);
            }

            this.nextProcessorInPipeline?.Process(item);
        }
    }
}
