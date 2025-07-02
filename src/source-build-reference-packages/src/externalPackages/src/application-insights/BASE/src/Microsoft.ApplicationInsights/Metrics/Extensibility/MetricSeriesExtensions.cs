namespace Microsoft.ApplicationInsights.Metrics.Extensibility
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// There are some APIs on <c>MetricSeries</c> that we hide from Intellisense by making them internal until the ..Extensibility namespace is imported.
    /// This class exposes them.
    /// </summary>
    public static class MetricSeriesExtensions
    {
        /// <summary>
        /// Exposes the <c>Configuration</c> property for users who imported the <c>Microsoft.ApplicationInsights.Metrics.Extensibility</c> namespace.
        /// </summary>
        /// <param name="metricSeries">Metric data series for whcih to get configuration.</param>
        /// <returns>Configuration of the specified series.</returns>
        public static IMetricSeriesConfiguration GetConfiguration(this MetricSeries metricSeries)
        {
            if (metricSeries == null)
            {
                throw new ArgumentNullException(nameof(metricSeries));
            }

            return metricSeries.configuration;
        }
    }
}
