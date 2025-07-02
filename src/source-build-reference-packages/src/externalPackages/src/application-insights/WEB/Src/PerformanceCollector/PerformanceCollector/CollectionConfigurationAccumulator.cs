namespace Microsoft.ApplicationInsights.Extensibility.Filtering
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// Represents the part of the QuickPulse accumulator which holds calculated metric data.
    /// As telemetry item pass through the pipeline, they are being filtered, projected, and the resulting
    /// values are stored here - both for calculated metrics and full telemetry document streams.
    /// Unlike the main accumulator, this one might not have finished being processed at swap time,
    /// so the consumer should keep the reference to it post-swap and make the best effort not to send
    /// prematurely. <see cref="referenceCount"/> indicates that the accumulator is still being processed
    /// when non-zero.
    /// </summary>
    internal class CollectionConfigurationAccumulator
    {
        /// <summary>
        /// Used by writers to indicate that a processing operation is still in progress.
        /// </summary>
        private long referenceCount = 0;

        public CollectionConfigurationAccumulator(CollectionConfiguration collectionConfiguration)
        {
            this.CollectionConfiguration = collectionConfiguration;

            // prepare the accumulators based on the collection configuration
            IEnumerable<Tuple<string, AggregationType>> allMetrics = collectionConfiguration?.TelemetryMetadata;
            foreach (Tuple<string, AggregationType> metricId in allMetrics ?? Enumerable.Empty<Tuple<string, AggregationType>>())
            {
                var accumulatedValues = new AccumulatedValues(metricId.Item1, metricId.Item2);

                this.MetricAccumulators.Add(metricId.Item1, accumulatedValues);
            }
        }

        /// <summary>
        /// Gets a dictionary of metricId => AccumulatedValues.
        /// </summary>
        public Dictionary<string, AccumulatedValues> MetricAccumulators { get; } = new Dictionary<string, AccumulatedValues>();

        public CollectionConfiguration CollectionConfiguration { get; }

        public void AddRef()
        {
            Interlocked.Increment(ref this.referenceCount);
        }

        public void Release()
        {
            Interlocked.Decrement(ref this.referenceCount);
        }

        public long GetRef()
        {
            return Interlocked.Read(ref this.referenceCount);
        }
    }
}