namespace Microsoft.ApplicationInsights.Metrics
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Microsoft.ApplicationInsights.Metrics.ConcurrentDatastructures;
    using Microsoft.ApplicationInsights.Metrics.Extensibility;
    using static System.FormattableString;
    using CycleKind = Microsoft.ApplicationInsights.Metrics.Extensibility.MetricAggregationCycleKind;

    internal class MetricAggregationManager
    {
        // We support 4 aggregation cycles. 2 of them can be accessed from the outside:

        private AggregatorCollection aggregatorsForPersistent = null;
        private AggregatorCollection aggregatorsForDefault = null;
        private AggregatorCollection aggregatorsForQuickPulse = null;
        private AggregatorCollection aggregatorsForCustom = null;

        internal MetricAggregationManager()
        {
            DateTimeOffset now = DateTimeOffset.Now;
            DateTimeOffset timestamp = Util.RoundDownToSecond(now);

            this.aggregatorsForDefault = new AggregatorCollection(timestamp, filter: null);
            this.aggregatorsForPersistent = new AggregatorCollection(timestamp, filter: null);
        }

        public AggregationPeriodSummary StartOrCycleAggregators(MetricAggregationCycleKind aggregationCycleKind, DateTimeOffset tactTimestamp, IMetricSeriesFilter futureFilter)
        {
            switch (aggregationCycleKind)
            {
                case CycleKind.Default:
                    if (futureFilter != null)
                    {
                        throw new ArgumentException(Invariant($"Cannot specify non-null {nameof(futureFilter)} when {nameof(aggregationCycleKind)} is {aggregationCycleKind}."));
                    }

                    return this.CycleAggregators(ref this.aggregatorsForDefault, tactTimestamp, futureFilter, stopAggregators: false);

                case CycleKind.QuickPulse:
                    return this.CycleAggregators(ref this.aggregatorsForQuickPulse, tactTimestamp, futureFilter, stopAggregators: false);

                case CycleKind.Custom:
                    return this.CycleAggregators(ref this.aggregatorsForCustom, tactTimestamp, futureFilter, stopAggregators: false);

                default:
                    throw new ArgumentException(Invariant($"Unexpected value of {nameof(aggregationCycleKind)}: {aggregationCycleKind}."));
            }
        }

        public AggregationPeriodSummary StopAggregators(MetricAggregationCycleKind aggregationCycleKind, DateTimeOffset tactTimestamp)
        {
            switch (aggregationCycleKind)
            {
                case CycleKind.QuickPulse:
                    return this.CycleAggregators(ref this.aggregatorsForQuickPulse, tactTimestamp, futureFilter: null, stopAggregators: true);

                case CycleKind.Custom:
                    return this.CycleAggregators(ref this.aggregatorsForCustom, tactTimestamp, futureFilter: null, stopAggregators: true);

                case CycleKind.Default:
                    throw new ArgumentException(Invariant($"Cannot invoke {nameof(this.StopAggregators)} for Default {nameof(MetricAggregationCycleKind)}: Default aggregators are always active."));

                default:
                    throw new ArgumentException(Invariant($"Unexpected value of {nameof(aggregationCycleKind)}: {aggregationCycleKind}."));
            }
        }

        internal bool IsCycleActive(MetricAggregationCycleKind aggregationCycleKind, out IMetricSeriesFilter filter)
        {
            switch (aggregationCycleKind)
            {
                case CycleKind.Default:
                    filter = null;
                    return true;

                case CycleKind.QuickPulse:
                    AggregatorCollection qpAggs = this.aggregatorsForQuickPulse;
                    filter = qpAggs?.Filter;
                    return qpAggs != null;
                case CycleKind.Custom:
                    AggregatorCollection cAggs = this.aggregatorsForCustom;
                    filter = cAggs?.Filter;
                    return cAggs != null;

                default:
                    throw new ArgumentException(Invariant($"Unexpected value of {nameof(aggregationCycleKind)}: {aggregationCycleKind}."));
            }
        }

        internal bool AddAggregator(IMetricSeriesAggregator aggregator, MetricAggregationCycleKind aggregationCycleKind)
        {
            Util.ValidateNotNull(aggregator, nameof(aggregator));

            if (aggregator.DataSeries.configuration.RequiresPersistentAggregation)
            {
                return AddAggregator(aggregator, this.aggregatorsForPersistent);
            }

            switch (aggregationCycleKind)
            {
                case CycleKind.Default:
                    return AddAggregator(aggregator, this.aggregatorsForDefault);

                case CycleKind.QuickPulse:
                    return AddAggregator(aggregator, this.aggregatorsForQuickPulse);

                case CycleKind.Custom:
                    return AddAggregator(aggregator, this.aggregatorsForCustom);

                default:
                    throw new ArgumentException(Invariant($"Unexpected value of {nameof(aggregationCycleKind)}: {aggregationCycleKind}."));
            }
        }

        private static bool AddAggregator(IMetricSeriesAggregator aggregator, AggregatorCollection aggregatorCollection)
        {
            if (aggregatorCollection == null)
            {
                return false;
            }

            IMetricSeriesFilter seriesFilter = aggregatorCollection.Filter;
            IMetricValueFilter valueFilter;
            if (false == Util.FilterWillConsume(seriesFilter, aggregator.DataSeries, out valueFilter))
            {
                return false;
            }

            // @ToDo
            // @PublicExposureCandidate 
            // aggregator.Reset(aggregatorCollection.PeriodStart, valueFilter);
            aggregator.Reset(aggregatorCollection.PeriodStart);

            aggregatorCollection.Aggregators.Add(aggregator);

            return true;
        }

        private static List<MetricAggregate> GetNonpersistentAggregations(DateTimeOffset tactTimestamp, AggregatorCollection aggregators)
        {
            // Complete each non-persistent aggregator:
            // (we snapshotted the entire collection, so Count is stable)

            GrowingCollection<IMetricSeriesAggregator> actualAggregators = aggregators?.Aggregators;

            if (null == actualAggregators || 0 == actualAggregators.Count)
            {
                return new List<MetricAggregate>(capacity: 0);
            }

            List<MetricAggregate> nonpersistentAggregations = new List<MetricAggregate>(capacity: actualAggregators.Count);

            foreach (IMetricSeriesAggregator aggregator in actualAggregators)
            {
                if (aggregator != null)
                {
                    MetricAggregate aggregate = aggregator.CompleteAggregation(tactTimestamp);

                    if (aggregate != null)
                    {
                        nonpersistentAggregations.Add(aggregate);
                    }
                }
            }

            return nonpersistentAggregations;
        }

        private AggregationPeriodSummary CycleAggregators(
                                                ref AggregatorCollection aggregators,
                                                DateTimeOffset tactTimestamp,
                                                IMetricSeriesFilter futureFilter,
                                                bool stopAggregators)
        {
            if (aggregators == this.aggregatorsForPersistent)
            {
                throw new InvalidOperationException("Internal SDK bug. Please report. Cannot cycle persistent aggregators.");
            }

            tactTimestamp = Util.RoundDownToSecond(tactTimestamp);

            // For non-persistent aggregators: create empty holder for the next aggregation period and swap for the previous holder:
            AggregatorCollection prevAggregators;
            if (stopAggregators)
            {
                prevAggregators = Interlocked.Exchange(ref aggregators, null);
            }
            else
            {
                AggregatorCollection nextAggregators = new AggregatorCollection(tactTimestamp, futureFilter);
                prevAggregators = Interlocked.Exchange(ref aggregators, nextAggregators);
            }

            // Get persistent aggregations. We do this for any cycle kind, i.e. for whatever the aggregators collection was:
            List<MetricAggregate> persistentValsAggregations = this.GetPersistentAggregations(tactTimestamp, prevAggregators?.Filter);

            // Get non-persistent aggregations:
            List<MetricAggregate> nonpersistentAggregations = GetNonpersistentAggregations(tactTimestamp, prevAggregators);

            var summary = new AggregationPeriodSummary(persistentValsAggregations, nonpersistentAggregations);
            return summary;
        }

        private List<MetricAggregate> GetPersistentAggregations(DateTimeOffset tactTimestamp, IMetricSeriesFilter previousFilter)
        {
            // Complete each persistent aggregator:
            // The Enumerator of GrowingCollection is a thread-safe lock-free implementation that operates on a "snapshot" of a collection taken at the
            // time when the enumerator is created. We expand the foreach statement (like the compiler normally does) so that we can use the typed
            // enumerator's Count property which is constsent with the data in the snapshot.

            GrowingCollection<IMetricSeriesAggregator>.Enumerator persistentValsAggregators = this.aggregatorsForPersistent.Aggregators.GetEnumerator();
            List<MetricAggregate> persistentValsAggregations = new List<MetricAggregate>(capacity: persistentValsAggregators.Count);
            try
            {
                while (persistentValsAggregators.MoveNext())
                {
                    IMetricSeriesAggregator aggregator = persistentValsAggregators.Current;
                    if (aggregator != null)
                    {
                        // Persistent aggregators are always active, regardless of filters for a particular cycle.
                        // But we can apply the cycle's filters to determine whether or not to pull the aggregator
                        // for a aggregate at this time. Of course, only series filters, not value filters, can be considered.
                        IMetricValueFilter unusedValueFilter;
                        bool satisfiesFilter = Util.FilterWillConsume(previousFilter, aggregator.DataSeries, out unusedValueFilter);
                        if (satisfiesFilter)
                        {
                            MetricAggregate aggregate = aggregator.CompleteAggregation(tactTimestamp);

                            if (aggregate != null)
                            {
                                persistentValsAggregations.Add(aggregate);
                            }
                        }
                    }
                }
            }
            finally
            {
                persistentValsAggregators.Dispose();
            }

            return persistentValsAggregations;
        }

        #region class AggregatorCollection

        private class AggregatorCollection
        {
            public AggregatorCollection(DateTimeOffset periodStart, IMetricSeriesFilter filter)
            {
                this.PeriodStart = periodStart;
                this.Aggregators = new GrowingCollection<IMetricSeriesAggregator>();
                this.Filter = filter;
            }

            public DateTimeOffset PeriodStart { get; }

            public GrowingCollection<IMetricSeriesAggregator> Aggregators { get; }

            public IMetricSeriesFilter Filter { get; }
        }

        #endregion class AggregatorCollection
    }
}
