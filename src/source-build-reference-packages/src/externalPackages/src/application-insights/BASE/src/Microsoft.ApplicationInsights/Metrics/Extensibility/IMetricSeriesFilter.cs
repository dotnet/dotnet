namespace Microsoft.ApplicationInsights.Metrics.Extensibility
{
    using System;

    /// <summary>A filter that determines whether a series is being tracked.</summary>
    /// @PublicExposureCandidate
    internal interface IMetricSeriesFilter
    {
        /// <summary>Determine if a series is being tracked and fetch the rspective value filter.</summary>
        /// <param name="dataSeries">A metric data series.</param>
        /// <param name="valueFilter">A values filter used for this series (if any) or <c>null</c>.</param>
        /// <returns>WHether a series is being tracked.</returns>
        bool WillConsume(MetricSeries dataSeries, out IMetricValueFilter valueFilter);
    }
}