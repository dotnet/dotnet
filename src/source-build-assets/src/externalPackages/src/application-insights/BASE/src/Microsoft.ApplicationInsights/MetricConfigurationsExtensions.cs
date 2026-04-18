namespace Microsoft.ApplicationInsights
{
    using System;
    using System.ComponentModel;

    using Microsoft.ApplicationInsights.Metrics;

    /// <summary>
    /// Provides the default Metric Configuration for Measurements.
    /// Do not use directly. Instead, use: <c>MetricConfigurations.Common.Xxxx()</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class MetricConfigurationsExtensions
    {
        private const int DefaultSeriesCountLimit = 1000;
        private const int DefaultValuesPerDimensionLimit = 100;

        private static MetricConfigurationForMeasurement defaultConfigForMeasurement = new MetricConfigurationForMeasurement(
                                                                    DefaultSeriesCountLimit,
                                                                    DefaultValuesPerDimensionLimit,
                                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

        /// <summary>
        /// <para>Use to measure properties and/or counts of items. Also, use to measure attributes and/or rates of events.<br />
        /// Produces aggregates that contain simple statistics about tracked values per time period: Count, Sum, Min, Max.<br />
        /// (This is the most commonly used metric configuration and is the default unless otherwise specified.)</para>
        /// 
        /// <para>For example, use this metric configuration to measure:<br />
        /// Size and number of server requests per time period; Duration and rate of database calls per time period;
        /// Number of sale events and number of items sold per sale event over a time period, etc.</para>
        /// </summary>
        /// <param name="metricConfigPresets">A static attachment point for this extension method.</param>
        /// <returns>The default <see cref="MetricConfiguration"/> for measurement metrics.</returns>
        public static MetricConfigurationForMeasurement Measurement(this MetricConfigurations metricConfigPresets)
        {
            return defaultConfigForMeasurement;
        }

        /// <summary>
        /// Set the configuration returned from <c>MetricConfigurations.Common.Measurement()</c>.
        /// </summary>
        /// <param name="metricConfigPresets">Will be ignored.</param>
        /// <param name="defaultConfigurationForMeasurement">Future default config.</param>
        public static void SetDefaultForMeasurement(
                                                this MetricConfigurations metricConfigPresets,
                                                MetricConfigurationForMeasurement defaultConfigurationForMeasurement)
        {
            Util.ValidateNotNull(
                        defaultConfigurationForMeasurement, 
                        nameof(defaultConfigurationForMeasurement));
            Util.ValidateNotNull(
                        defaultConfigurationForMeasurement.SeriesConfig, 
                        nameof(defaultConfigurationForMeasurement) + "." + nameof(defaultConfigurationForMeasurement.SeriesConfig));
            
            defaultConfigForMeasurement = defaultConfigurationForMeasurement;
        }

        internal static MetricConfiguration Default(this MetricConfigurations metricConfigPresets)
        {
            return metricConfigPresets.Measurement();
        }
    }
}
