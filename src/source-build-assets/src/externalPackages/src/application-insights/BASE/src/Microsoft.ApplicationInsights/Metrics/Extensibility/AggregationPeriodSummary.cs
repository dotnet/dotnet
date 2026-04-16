namespace Microsoft.ApplicationInsights.Metrics.Extensibility
{
    using System;
    using System.Collections.Generic;

    /// <summary>A container used by a <see cref="MetricAggregationManager"/> to hold all aggregates resukting from a particular aggregation period.</summary>
    /// @PublicExposureCandidate
    internal class AggregationPeriodSummary
    {
        /// <summary>Creates a new <c>AggregationPeriodSummary</c>.</summary>
        /// <param name="persistentAggregates">Persistent aggregators carry forward their state across aggregation cycles.</param>
        /// <param name="nonpersistentAggregates">Non-persistent aggregators do not keep state from previous aggregation cycles.</param>
        public AggregationPeriodSummary(IReadOnlyList<MetricAggregate> persistentAggregates, IReadOnlyList<MetricAggregate> nonpersistentAggregates)
        {
            this.PersistentAggregates = persistentAggregates;
            this.NonpersistentAggregates = nonpersistentAggregates;
        }

        /// <summary>Gets persistent aggregators, which carry forward their state across aggregation cycles.</summary>
        public IReadOnlyList<MetricAggregate> PersistentAggregates { get; }

        /// <summary>Gets Non-persistent aggregators, which do not keep state from previous aggregation cycles.</summary>
        public IReadOnlyList<MetricAggregate> NonpersistentAggregates { get; }
    }
}
