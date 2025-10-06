namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.ComponentModel;
    using Microsoft.ApplicationInsights.DataContracts;

    /// <summary>
    /// Extension methods for TelemetryConfiguration.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class TelemetryConfigurationExtensions
    {
        /// <summary>
        /// Gets last known request sampling percentage to skip initializers for sampled requests.
        /// </summary>
        public static double GetLastObservedSamplingPercentage(this TelemetryConfiguration configuration, SamplingTelemetryItemTypes samplingItemType)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            return configuration.LastKnownSampleRateStore.GetLastObservedSamplingPercentage(samplingItemType);
        }

        /// <summary>
        /// Sets last known request sampling percentage to skip initializers for sampled requests.
        /// </summary>
        public static void SetLastObservedSamplingPercentage(this TelemetryConfiguration configuration, SamplingTelemetryItemTypes samplingItemType, double value)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            configuration.LastKnownSampleRateStore.SetLastObservedSamplingPercentage(samplingItemType, value);
        }
    }
}
