namespace Microsoft.ApplicationInsights.Metrics.Extensibility
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>A <c>IMetricTelemetryPipeline</c> that holds aggregates in memory instead of sending them anywhere for processing.
    /// This is useful for local testing and debugging scenarios.
    /// An instance of this class holds up to <see cref="CountLimit"/> aggregates in memory. WHen additional aggregates are written,
    /// the oldest ones get discarded.</summary>
    /// @PublicExposureCandidate
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001: Types that own disposable fields should be disposable", Justification = "OK not to explicitly dispose a released SemaphoreSlim.")]
    internal class MemoryMetricTelemetryPipeline : IMetricTelemetryPipeline, IReadOnlyList<MetricAggregate>
    {
        /// <summary>Default setting for how many items to hold in memory.</summary>
        public const int CountLimitDefault = 1000;

        // private readonly Task completedTask = Task.FromResult(true);
        private readonly SemaphoreSlim updateLock = new SemaphoreSlim(1);

        private readonly IList<MetricAggregate> metricAgregates = new List<MetricAggregate>();

        /// <summary>Creates a new <c>MemoryMetricTelemetryPipeline</c> that holds up to <c>CountLimitDefault</c> aggregates in memory.</summary>
        public MemoryMetricTelemetryPipeline()
            : this(CountLimitDefault)
        {
        }

        /// <summary>Creates a new <c>MemoryMetricTelemetryPipeline</c> that holds up to the specified number of aggregates in memory.</summary>
        /// <param name="countLimit">Max number of most recent aggregates to hold in memory.</param>
        public MemoryMetricTelemetryPipeline(int countLimit)
        {
            if (countLimit <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(countLimit));
            }

            this.CountLimit = countLimit;
        }

        /// <summary>Gets the max buffer size.</summary>
        public int CountLimit { get; }

        /// <summary>Gets the current number of metric aggregates in the buffer.</summary>
        public int Count
        {
            get
            {
                int count;
                this.updateLock.WaitAsync().GetAwaiter().GetResult();
                try
                {
                    count = this.metricAgregates.Count;
                }
                finally
                {
                    this.updateLock.Release();
                }

                return count;
            }
        }

        /// <summary>Provides access to the metric aggregates that have been written to this pipeline.</summary>
        /// <param name="index">Index of the aggregate in the buffer.</param>
        /// <returns>Metric aggregate at the specified index.</returns>
        public MetricAggregate this[int index]
        {
            get
            {
                MetricAggregate metricAggregate;
                this.updateLock.WaitAsync().GetAwaiter().GetResult();
                try
                {
                    metricAggregate = this.metricAgregates[index];
                }
                finally
                {
                    this.updateLock.Release();
                }

                return metricAggregate;
            }
        }

        /// <summary>Clears the buffer.</summary>
        public void Clear()
        {
            this.updateLock.WaitAsync().GetAwaiter().GetResult();
            try
            {
                this.metricAgregates.Clear();
            }
            finally
            {
                this.updateLock.Release();
            }
        }

        /// <summary>Stores a metric aggregate in a memory buffer. The aggregate is not further processed,
        /// but it can be accessed from the buffer. If the buffer already contains <see cref="CountLimit"/> items,
        /// the oldest item (the one at index 0) gets discarded before adding the new item at the end of the buffer.</summary>
        /// <param name="metricAggregate">Aggregate to keep.</param>
        /// <param name="cancelToken">To signal cancellation of the track-operation.</param>
        /// <returns>A task representing the completion of this operation.</returns>
        public async Task TrackAsync(MetricAggregate metricAggregate, CancellationToken cancelToken)
        {
            Util.ValidateNotNull(metricAggregate, nameof(metricAggregate));

            await this.updateLock.WaitAsync(cancelToken).ConfigureAwait(true);
            try
            {
                while (this.metricAgregates.Count >= this.CountLimit)
                {
                    this.metricAgregates.RemoveAt(0);
                }

                this.metricAgregates.Add(metricAggregate);
            }
            finally
            {
                this.updateLock.Release();
            }
        }

        /// <summary>No-op.</summary>
        /// <param name="cancelToken">Ignored.</param>
        /// <returns>A completed task.</returns>
        public Task FlushAsync(CancellationToken cancelToken)
        {
            return Task.FromResult(true);
        }

        IEnumerator<MetricAggregate> IEnumerable<MetricAggregate>.GetEnumerator()
        {
            IEnumerator<MetricAggregate> enumerator;
            this.updateLock.WaitAsync().GetAwaiter().GetResult();
            try
            {
                enumerator = this.metricAgregates.GetEnumerator();
            }
            finally
            {
                this.updateLock.Release();
            }

            return enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<MetricAggregate>)this).GetEnumerator();
        }
    }
}
