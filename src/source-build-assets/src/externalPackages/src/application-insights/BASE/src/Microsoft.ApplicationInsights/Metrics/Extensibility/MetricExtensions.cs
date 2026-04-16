namespace Microsoft.ApplicationInsights.Metrics.Extensibility
{
    using System;

    /// <summary>
    /// There are some APIs on <c>Metric</c> that we hide from Intellisense by making them internal until the ...Extensibility namespace is imported.
    /// This class exposes them.
    /// </summary>
    public static class MetricExtensions
    {
        /// <summary>
        /// Exposes the <c>Configuration</c> property for users who imported the <c>Microsoft.ApplicationInsights.Metrics.Extensibility</c> namespace.
        /// </summary>
        /// <param name="metric">A metric.</param>
        /// <returns>The configuration of the metric.</returns>
        public static MetricConfiguration GetConfiguration(this Metric metric)
        {
            if (metric == null)
            {
                throw new ArgumentNullException(nameof(metric));
            }

            return metric.configuration;
        }

        ///// <summary>
        ///// Exposes the <c>MetricManager</c> property for users who imported the <c>Microsoft.ApplicationInsights.Metrics.Extensibility</c> namespace.
        ///// </summary>
        ///// <param name="metric">The metric for which the obtain the manager.</param>
        ///// <returns>The manager of the specified metric.</returns>
        ////public static MetricManager GetMetricManager(this Metric metric)
        ////{
        ////    return metric._metricManager;
        ////}
    }
}
