namespace Microsoft.ApplicationInsights.Metrics.Extensibility
{
    /// <summary>Abstraction for a filter that can controll whether values are being tracked or ignored by a metric aggregator.</summary>
    public interface IMetricValueFilter
    {
        /// <summary>Determine whether a value will be tracked or ignored while aggregating a metric data time series.</summary>
        /// <param name="dataSeries">A metric data time series.</param>
        /// <param name="metricValue">A metric value.</param>
        /// <returns>Whether or not a value will be tracked or ignored while aggregating a metric data time series.</returns>
        bool WillConsume(MetricSeries dataSeries, double metricValue);

        /// <summary>Determine whether a value will be tracked or ignored while aggregating a metric data time series.</summary>
        /// <param name="dataSeries">A metric data time series.</param>
        /// <param name="metricValue">A metric value.</param>
        /// <returns>Whether or not a value will be tracked or ignored while aggregating a metric data time series.</returns>
        bool WillConsume(MetricSeries dataSeries, object metricValue);
    }
}