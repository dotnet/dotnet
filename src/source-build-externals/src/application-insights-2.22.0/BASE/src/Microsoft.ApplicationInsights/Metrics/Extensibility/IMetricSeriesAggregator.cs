namespace Microsoft.ApplicationInsights.Metrics.Extensibility
{
    using System;

    /// <summary>The abstraction for a metric aggregator.
    /// An aggregator is a data processing type that inspects all values tracked for a metric series across an aggregation
    /// period and creates an aggregate that summarizes the period when it is completed. The most common aggregator is
    /// the <see cref="MeasurementAggregator"/>, which produces aggregates that contain the Min, Max, Sum and
    /// Count of values tracked over the aggregation time period.</summary>
    public interface IMetricSeriesAggregator
    {
        /// <summary>Gets the data series aggregated by this aggregator.</summary>
        MetricSeries DataSeries { get; }

        /// <summary>Attempts to reset this aggregator so it ban be reused for a new aggregation period.</summary>
        /// <returns>Whether the reset was successful (if not, this aggregator may not be reused).</returns>
        bool TryRecycle();

        /// <summary>Resets this aggregator and prepares it for a new aggregation period.</summary>
        /// <param name="periodStart">The start of the new aggregation period.</param>
        /// <param name="valueFilter">The filter for the values to be used.</param>
        /// @PublicExposureCandidate
        void Reset(DateTimeOffset periodStart, IMetricValueFilter valueFilter);

        /// <summary>Resets this aggregator and prepares it for a new aggregation period.</summary>
        /// <param name="periodStart">The start of the new aggregation period.</param>
        void Reset(DateTimeOffset periodStart);

        /// <summary>Wraps up the ongping aggregation period and procudes the resulting aggregate.</summary>
        /// <param name="periodEnd">The end timestamp of the period.</param>
        /// <returns>The aggregate containing the sumary of the completed period.</returns>
        MetricAggregate CompleteAggregation(DateTimeOffset periodEnd);

        /// <summary>Creates the aggregate for the ongoing aggregation period without completing the period. May not be thread safe.</summary>
        /// <param name="periodEnd">The ent timestamp for the aggregate.</param>
        /// <returns>An aggregate representing the ongoing period so far.</returns>
        MetricAggregate CreateAggregateUnsafe(DateTimeOffset periodEnd);

        /// <summary>Adds a value to the aggregation.</summary>
        /// <param name="metricValue">Metric value to be tracked.</param>
        void TrackValue(double metricValue);

        /// <summary>Adds a value to the aggregation.</summary>
        /// <param name="metricValue">Metric value to be tracked.</param>
        void TrackValue(object metricValue);
    }
}
