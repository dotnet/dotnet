namespace Microsoft.ApplicationInsights.Metrics
{
    using System;
    using Microsoft.ApplicationInsights.Metrics.Extensibility;

    /// <summary>Abstraction for the configuration of a metric series.
    /// The configuration is a factory that can produce an arbitrary aggregator to be used by the respective series.</summary>
    public interface IMetricSeriesConfiguration : IEquatable<IMetricSeriesConfiguration>
    {
        /// <summary>Gets a value indicating whether the aggrgator produced by this settings object is
        /// persistent (carries state across aggregation periods) or not.</summary>
        bool RequiresPersistentAggregation { get; }

        /// <summary>Create an aggregator for any series configured by this confuguration object.</summary>
        /// <param name="dataSeries">Metric data series for which to produce the aggregator..</param>
        /// <param name="aggregationCycleKind">THe kind of the aggregation cycle (not the aggregation kind).</param>
        /// <returns>A new aggregator for the specified series and the specified cycle.</returns>
        IMetricSeriesAggregator CreateNewAggregator(MetricSeries dataSeries, MetricAggregationCycleKind aggregationCycleKind);
    }
}
