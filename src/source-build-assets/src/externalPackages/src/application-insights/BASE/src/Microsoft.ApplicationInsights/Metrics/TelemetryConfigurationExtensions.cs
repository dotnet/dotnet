namespace Microsoft.ApplicationInsights.Metrics
{
    using System;
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>Container for extension methods on <c>TelemetryConfiguration</c>.</summary>
    public static class TelemetryConfigurationExtensions
    {
        /// <summary><c>TelemetryConfiguration.GetMetricManager(..)</c> is a internal method to avoid puluting the public surface.
        /// You can use the namespace <c>Microsoft.ApplicationInsights.Extensibility</c> to get access to the <c>MetricManager</c> via this extension method.</summary>
        /// <param name="telemetryPipeline">A <c>TelemetryConfiguration</c>.</param>
        /// <returns>The <c>MetricManager</c> instscne assiciated with the specified telemetry pipeline.</returns>
        public static MetricManager GetMetricManager(this TelemetryConfiguration telemetryPipeline)
        {
            return telemetryPipeline?.GetMetricManager(createIfNotExists: true);
        }
    }
}
