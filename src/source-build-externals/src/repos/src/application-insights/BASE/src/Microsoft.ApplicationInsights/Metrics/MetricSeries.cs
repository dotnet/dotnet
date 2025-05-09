namespace Microsoft.ApplicationInsights.Metrics
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.ExceptionServices;
    using System.Threading;
    using Microsoft.ApplicationInsights.Metrics.Extensibility;
    using static System.FormattableString;
    using CycleKind = Microsoft.ApplicationInsights.Metrics.Extensibility.MetricAggregationCycleKind;

    /// <summary>
    /// Represents a data time series of metric values.
    /// One or more <c>MetricSeries</c> are grouped into a single <c>Metric</c>.
    /// Use <c>MetricSeries</c> to track, aggregate and send values without the overhead of looking them up from the
    /// corresponding <c>Metric</c> object.
    /// Each <c>Metric</c> object contains a special zero-dimension series, plus, for multi-dimensional metrics, one
    /// series per unique dimension-values combination.
    /// </summary>
    public sealed class MetricSeries
    {
#pragma warning disable SA1401, SA1304, SA1307 // intended to be an internal, lower-case field 
        internal readonly IMetricSeriesConfiguration configuration;
#pragma warning restore SA1307, SA1304, SA1401

        private readonly MetricAggregationManager aggregationManager;
        private readonly bool requiresPersistentAggregator;
        private readonly IReadOnlyDictionary<string, string> dimensionNamesAndValues;

        private IMetricSeriesAggregator aggregatorPersistent;
        private WeakReference<IMetricSeriesAggregator> aggregatorDefault;
        private WeakReference<IMetricSeriesAggregator> aggregatorQuickPulse;
        private WeakReference<IMetricSeriesAggregator> aggregatorCustom;

        private IMetricSeriesAggregator aggregatorRecycleCacheDefault;
        private IMetricSeriesAggregator aggregatorRecycleCacheQuickPulse;
        private IMetricSeriesAggregator aggregatorRecycleCacheCustom;

        internal MetricSeries(
                            MetricAggregationManager aggregationManager,
                            MetricIdentifier metricIdentifier,
                            IEnumerable<KeyValuePair<string, string>> dimensionNamesAndValues,
                            IMetricSeriesConfiguration configuration)
        {
            // Validate and store aggregationManager:
            Util.ValidateNotNull(aggregationManager, nameof(aggregationManager));
            this.aggregationManager = aggregationManager;

            // Validate and store metricIdentifier:
            Util.ValidateNotNull(metricIdentifier, nameof(metricIdentifier));
            this.MetricIdentifier = metricIdentifier;

            // Copy dimensionNamesAndValues, validate values (keys are implicitly validated as they need to match the keys in the identifier):
            var dimNameVals = new Dictionary<string, string>();
            if (dimensionNamesAndValues != null)
            {
                int dimIndex = 0;
                foreach (KeyValuePair<string, string> dimNameVal in dimensionNamesAndValues)
                {
                    if (dimNameVal.Value == null)
                    {
                        throw new ArgumentNullException(Invariant($"The value for dimension '{dimNameVal.Key}' number is null."));
                    }

                    if (String.IsNullOrWhiteSpace(dimNameVal.Value))
                    {
                        throw new ArgumentNullException(Invariant($"The value for dimension '{dimNameVal.Key}' is empty or white-space."));
                    }

                    dimNameVals[dimNameVal.Key] = dimNameVal.Value;
                    dimIndex++;
                }
            }

            // Validate that metricIdentifier and dimensionNamesAndValues contain consistent dimension names:
            if (metricIdentifier.DimensionsCount != dimNameVals.Count)
            {
                throw new ArgumentException(Invariant($"The specified {nameof(metricIdentifier)} contains {metricIdentifier.DimensionsCount} dimensions,")
                                          + Invariant($" however the specified {nameof(dimensionNamesAndValues)} contains {dimNameVals.Count} name-value pairs with unique names."));
            }

            foreach (string dimName in metricIdentifier.GetDimensionNames())
            {
                if (false == dimNameVals.ContainsKey(dimName))
                {
                    throw new ArgumentException(Invariant($"The specified {nameof(metricIdentifier)} contains a dimension named \"{dimName}\",")
                                              + Invariant($" however the specified {nameof(dimensionNamesAndValues)} does not contain an entry for that name."));
                }
            }

            // Store copied dimensionNamesAndValues:
            this.dimensionNamesAndValues = dimNameVals;

            // Validate and store configuration:
            Util.ValidateNotNull(configuration, nameof(configuration));
            this.configuration = configuration;
            this.requiresPersistentAggregator = configuration.RequiresPersistentAggregation;

            // Init other instance vars:
            this.aggregatorPersistent = null;
            this.aggregatorDefault = null;
            this.aggregatorQuickPulse = null;
            this.aggregatorCustom = null;
        }

        /// <summary>Gets a table that describes the names and values of the dimensions that describe this metric time series.</summary>
        public IReadOnlyDictionary<string, string> DimensionNamesAndValues
        {
            get { return this.dimensionNamesAndValues; }
        }

        /// <summary>Gets the identifier of the metric that contains this metric time series.</summary>
        public MetricIdentifier MetricIdentifier { get; }

        /// <summary>
        /// Includes the specified value into the current aggregate of this metric time series.<br />
        /// An aggregate representing tracked values will be automatically sent to the cloud ingestion endpoint at the end of each aggregation period.<br />
        /// <para>(Advanced note: When non-default aggregation cycles are active, additional aggregates may be obtained by cycling respective aggregators.)</para>
        /// </summary>
        /// <param name="metricValue">The value to be aggregated.</param>
        public void TrackValue(double metricValue)
        {
            List<Exception> errors = null;

            if (this.requiresPersistentAggregator)
            {
                TrackValue(this.GetOrCreatePersistentAggregator(), metricValue, ref errors);
            }
            else
            {
                IMetricSeriesAggregator aggregator;

                aggregator = this.GetOrCreateAggregator(CycleKind.Default, ref this.aggregatorDefault);
                TrackValue(aggregator, metricValue, ref errors);

                aggregator = this.GetOrCreateAggregator(CycleKind.QuickPulse, ref this.aggregatorQuickPulse);
                TrackValue(aggregator, metricValue, ref errors);

                aggregator = this.GetOrCreateAggregator(CycleKind.Custom, ref this.aggregatorCustom);
                TrackValue(aggregator, metricValue, ref errors);
            }

            if (errors != null)
            {
                if (errors.Count == 1)
                {
                    ExceptionDispatchInfo.Capture(errors[0]).Throw();
                }
                else
                {
                    throw new AggregateException(errors);
                }
            }
        }

        /// <summary>
        /// Includes the specified value into the current aggregate of this metric time series.<br />
        /// An aggregate representing tracked values will be automatically sent to the cloud ingestion endpoint at the end of each aggregation period.<br />
        /// This overload allows creating aggregators that can aggregate non-numeric values (e.g. a distinct count of strings aggregator).
        /// <para>(Advanced note: When non-default aggregation cycles are active, additional aggregates may be obtained by cycling respective aggregators.)</para>
        /// </summary>
        /// <param name="metricValue">The value to be aggregated.</param>
        public void TrackValue(object metricValue)
        {
            List<Exception> errors = null;

            if (this.requiresPersistentAggregator)
            {
                TrackValue(this.GetOrCreatePersistentAggregator(), metricValue, ref errors);
            }
            else
            {
                IMetricSeriesAggregator aggregator;

                aggregator = this.GetOrCreateAggregator(CycleKind.Default, ref this.aggregatorDefault);
                TrackValue(aggregator, metricValue, ref errors);

                aggregator = this.GetOrCreateAggregator(CycleKind.QuickPulse, ref this.aggregatorQuickPulse);
                TrackValue(aggregator, metricValue, ref errors);

                aggregator = this.GetOrCreateAggregator(CycleKind.Custom, ref this.aggregatorCustom);
                TrackValue(aggregator, metricValue, ref errors);
            }

            if (errors != null)
            {
                if (errors.Count == 1)
                {
                    ExceptionDispatchInfo.Capture(errors[0]).Throw();
                }
                else
                {
                    throw new AggregateException(errors);
                }
            }
        }

        // @PublicExposureCandidate
        internal void ResetAggregation()
        {
            this.ResetAggregation(periodStart: DateTimeOffset.Now);
        }

        // @PublicExposureCandidate
        internal void ResetAggregation(DateTimeOffset periodStart)
        {
            periodStart = Util.RoundDownToSecond(periodStart);

            if (this.requiresPersistentAggregator)
            {
                IMetricSeriesAggregator aggregator = this.aggregatorPersistent;
                aggregator?.Reset(periodStart);
            }
            else
            {
                {
                    IMetricSeriesAggregator aggregator = UnwrapAggregator(this.aggregatorDefault);
                    aggregator?.Reset(periodStart);
                }

                {
                    IMetricSeriesAggregator aggregator = UnwrapAggregator(this.aggregatorQuickPulse);
                    aggregator?.Reset(periodStart);
                }

                {
                    IMetricSeriesAggregator aggregator = UnwrapAggregator(this.aggregatorCustom);
                    aggregator?.Reset(periodStart);
                }
            }
        }

        // @PublicExposureCandidate
        internal MetricAggregate GetCurrentAggregateUnsafe()
        {
            return this.GetCurrentAggregateUnsafe(CycleKind.Default, DateTimeOffset.Now);
        }

        // @PublicExposureCandidate
        internal MetricAggregate GetCurrentAggregateUnsafe(MetricAggregationCycleKind aggregationCycleKind, DateTimeOffset dateTime)
        {
            IMetricSeriesAggregator aggregator = null;

            if (this.requiresPersistentAggregator)
            {
                aggregator = this.aggregatorPersistent;
            }
            else
            {
                switch (aggregationCycleKind)
                {
                    case CycleKind.Default:
                        aggregator = UnwrapAggregator(this.aggregatorDefault);
                        break;

                    case CycleKind.QuickPulse:
                        aggregator = UnwrapAggregator(this.aggregatorQuickPulse);
                        break;

                    case CycleKind.Custom:
                        aggregator = UnwrapAggregator(this.aggregatorCustom);
                        break;

                    default:
                        throw new ArgumentException(Invariant($"Unexpected value of {nameof(aggregationCycleKind)}: {aggregationCycleKind}."));
                }
            }

            dateTime = Util.RoundDownToSecond(dateTime);
            MetricAggregate aggregate = aggregator?.CreateAggregateUnsafe(dateTime);
            return aggregate;
        }

        internal void ClearAggregator(MetricAggregationCycleKind aggregationCycleKind)
        {
            if (this.requiresPersistentAggregator)
            {
                return;
            }

            WeakReference<IMetricSeriesAggregator> aggregatorWeakRef;
            switch (aggregationCycleKind)
            {
                case CycleKind.Default:
                    aggregatorWeakRef = Interlocked.Exchange(ref this.aggregatorDefault, null);
                    this.aggregatorRecycleCacheDefault = UnwrapAggregator(aggregatorWeakRef);
                    break;

                case CycleKind.QuickPulse:
                    aggregatorWeakRef = Interlocked.Exchange(ref this.aggregatorQuickPulse, null);
                    this.aggregatorRecycleCacheQuickPulse = UnwrapAggregator(aggregatorWeakRef);
                    break;

                case CycleKind.Custom:
                    aggregatorWeakRef = Interlocked.Exchange(ref this.aggregatorCustom, null);
                    this.aggregatorRecycleCacheCustom = UnwrapAggregator(aggregatorWeakRef);
                    break;

                default:
                    throw new ArgumentException(Invariant($"Unexpected value of {nameof(aggregationCycleKind)}: {aggregationCycleKind}."));
            }
        }

        private static void TrackValue(IMetricSeriesAggregator aggregator, double metricValue, ref List<Exception> errors)
        {
            if (aggregator != null)
            {
                try
                {
                    aggregator.TrackValue(metricValue);
                }
                catch (Exception ex)
                {
                    (errors = errors ?? new List<Exception>()).Add(ex);
                }
            }
        }

        private static void TrackValue(IMetricSeriesAggregator aggregator, object metricValue, ref List<Exception> errors)
        {
            if (aggregator != null)
            {
                try
                {
                    aggregator.TrackValue(metricValue);
                }
                catch (Exception ex)
                {
                    (errors = errors ?? new List<Exception>()).Add(ex);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IMetricSeriesAggregator UnwrapAggregator(WeakReference<IMetricSeriesAggregator> aggregatorWeakRef)
        {
            if (aggregatorWeakRef != null)
            {
                IMetricSeriesAggregator aggregatorHardRef = null;
                if (aggregatorWeakRef.TryGetTarget(out aggregatorHardRef))
                {
                    return aggregatorHardRef;
                }
            }

            return null;
        }

        private IMetricSeriesAggregator GetOrCreatePersistentAggregator()
        {
            IMetricSeriesAggregator aggregator = this.aggregatorPersistent;

            if (aggregator == null)
            {
                aggregator = this.configuration.CreateNewAggregator(this, CycleKind.Default);
                IMetricSeriesAggregator prevAggregator = Interlocked.CompareExchange(ref this.aggregatorPersistent, aggregator, null);

                if (prevAggregator == null)
                {
                    bool added = this.aggregationManager.AddAggregator(aggregator, CycleKind.Default);
                    if (added == false)
                    {
                        throw new InvalidOperationException("Internal SDK gub. Please report!"
                                                          + " Info: _aggregationManager.AddAggregator reports false for a PERSISTENT aggregator."
                                                          + " This should never happen.");
                    }
                }
                else
                {
                    aggregator = prevAggregator;
                }
            }

            return aggregator;
        }
        
        private IMetricSeriesAggregator GetOrCreateAggregator(MetricAggregationCycleKind aggregationCycleKind, ref WeakReference<IMetricSeriesAggregator> aggregatorWeakRef)
        {
            while (true)
            {
                // Local cache for the reference in case of concurrnet updates:
                WeakReference<IMetricSeriesAggregator> currentAggregatorWeakRef = aggregatorWeakRef;

                // Try to dereference the weak reference:
                IMetricSeriesAggregator aggregatorHardRef = UnwrapAggregator(currentAggregatorWeakRef);
                if (aggregatorHardRef != null)
                {
                    return aggregatorHardRef;
                }

                // END OF FAST PATH. Could not dereference aggregator. Will attempt to create it...

                // So aggretator is NULL. For non-default cycle kinds, see if the there is even a cycle hooked up:

                if (aggregationCycleKind != CycleKind.Default)
                { 
                    IMetricSeriesFilter dataSeriesFilter;
                    if (false == this.aggregationManager.IsCycleActive(aggregationCycleKind, out dataSeriesFilter))
                    {
                        return null;
                    }

                    // Ok, a cycle is, indeed, acgive up. See if the cycle's filter is interested in this series:

                    // Respect the filter. Note: Filter may be user code. If user code is broken, assume we accept the series.
                    IMetricValueFilter valuesFilter;
                    if (false == Util.FilterWillConsume(dataSeriesFilter, this, out valuesFilter))
                    {
                        return null;
                    }
                }

                // Ok, they want to consume us. Create new aggregator and a weak reference to it:

                IMetricSeriesAggregator newAggregator = this.GetNewOrRecycledAggregatorInstance(aggregationCycleKind);
                WeakReference<IMetricSeriesAggregator> newAggregatorWeakRef = new WeakReference<IMetricSeriesAggregator>(newAggregator, trackResurrection: false);

                // Store the weak reference to the aggregator. However, there is a race on doing it, so check for it:
                WeakReference<IMetricSeriesAggregator> prevAggregatorWeakRef = Interlocked.CompareExchange(ref aggregatorWeakRef, newAggregatorWeakRef, currentAggregatorWeakRef);
                if (prevAggregatorWeakRef == currentAggregatorWeakRef)
                {
                    // We won the race and stored the aggregator. Now tell the manager about it and go on using it.
                    // Note that the status of IsCycleActive and the dataSeriesFilter may have changed concurrently.
                    // So it is essential that we do this after the above interlocked assignment of aggregator.
                    // It ensures that only objects are added to the aggregator collection that are also referenced by the data series.
                    // In addition, AddAggregator(..) always uses the current value of the aggregator-collection in a thread-safe manner.
                    // Becasue the aggregator collection reference is always updated before telling the aggregators to cycle,
                    // we know that any aggregator added will be eventually cycled.
                    // If adding succeeds, AddAggregator(..) will have set the correct filter.
                    bool added = this.aggregationManager.AddAggregator(newAggregator, aggregationCycleKind);

                    // If the manager does not accept the addition, it means that the aggregationCycleKind is disabled or that the filter is not interested in this data series.
                    // Either way we need to give up.
                    if (added)
                    {
                        return newAggregator;
                    }
                    else
                    {
                        // We could have accepted some values into this aggregator in the short time it was set in this series. We will loose those values.
                        Interlocked.CompareExchange(ref aggregatorWeakRef, null, newAggregatorWeakRef);
                        return null;
                    }
                }

                // We lost the race and a different aggregator was used. Loop again. Doing so will attempt to dereference the latest aggregator reference.
            }
        }

        private IMetricSeriesAggregator GetNewOrRecycledAggregatorInstance(MetricAggregationCycleKind aggregationCycleKind)
        {
            IMetricSeriesAggregator aggregator = this.GetRecycledAggregatorInstance(aggregationCycleKind);
            return aggregator ?? this.configuration.CreateNewAggregator(this, aggregationCycleKind);
        }

        /// <summary>
        /// The lifetime of an aggragator can easily be a minute or so. So, it is a relatively small object that can easily get into Gen-2 GC heap,
        /// but then will need to be reclaimed from there relatively quickly. This can lead to a fragmentation of Gen-2 heap. To avoid this we employ
        /// a simple form of object pooling: Each data series keeps an instance of a past aggregator and tries to reuse it.
        /// Aggregator implementations which believe that they are too expensive to recycle for this, can opt out of this strategy by returning FALSE from
        /// their CanRecycle property.
        /// </summary>
        /// <param name="aggregationCycleKind">The kind of the metric aggregation cycle.</param>
        /// <returns>An empty aggregator.</returns>
        private IMetricSeriesAggregator GetRecycledAggregatorInstance(MetricAggregationCycleKind aggregationCycleKind)
        {
            if (this.requiresPersistentAggregator)
            {
                return null;
            }

            IMetricSeriesAggregator aggregator = null;
            switch (aggregationCycleKind)
            {
                case CycleKind.Default:
                    aggregator = Interlocked.Exchange(ref this.aggregatorRecycleCacheDefault, null);
                    break;

                case CycleKind.QuickPulse:
                    aggregator = Interlocked.Exchange(ref this.aggregatorRecycleCacheQuickPulse, null);
                    break;

                case CycleKind.Custom:
                    aggregator = Interlocked.Exchange(ref this.aggregatorRecycleCacheCustom, null);
                    break;
            }

            if (aggregator == null)
            {
                return null;
            }

            if (aggregator.TryRecycle())
            {
                return aggregator;
            }

            return null;
        }
    }
}
