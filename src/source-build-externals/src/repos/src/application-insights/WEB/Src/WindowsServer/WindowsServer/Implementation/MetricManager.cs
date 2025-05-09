#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Tracing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Linq;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Platform;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    using TaskEx = System.Threading.Tasks.Task;

    /// <summary>
    /// Provides functionality to process metric values prior to aggregation.
    /// </summary>
    internal interface IMetricProcessor
    {
        /// <summary>
        /// Process metric value.
        /// </summary>
        /// <param name="metric">Metric definition.</param>
        /// <param name="value">Metric value.</param>
        void Track(Metric metric, double value);
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "This is a temporary private MetricManager")]
    internal static class MetricTerms
    {
        private const string MetricPropertiesNamePrefix = "_MS";

        public static class Aggregation
        {
            public static class Interval
            {
                public static class Moniker
                {
                    public const string Key = MetricPropertiesNamePrefix + ".AggregationIntervalMs";
                }
            }
        }

        public static class Extraction
        {
            public static class ProcessedByExtractors
            {
                public static class Moniker
                {
                    public const string Key = MetricPropertiesNamePrefix + ".ProcessedByMetricExtractors";
                    public const string ExtractorInfoTemplate = "(Name:'{0}', Ver:'{1}')";      // $"(Name:'{ExtractorName}', Ver:'{ExtractorVersion}')"
                }
            }
        }

        public static class Autocollection
        {
            public static class Moniker
            {
                public const string Key = MetricPropertiesNamePrefix + ".IsAutocollected";
                public const string Value = "True";
            }

            public static class MetricId
            {
                public static class Moniker
                {
                    public const string Key = MetricPropertiesNamePrefix + ".MetricId";
                }
            }

            public static class Metric
            {
                public static class RequestDuration
                {
                    public const string Name = "Server response time";
                    public const string Id = "requests/duration";
                }

                public static class DependencyCallDuration
                {
                    public const string Name = "Dependency duration";
                    public const string Id = "dependencies/duration";
                }
            }

            public static class Request
            {
                public static class PropertyNames
                {
                    public const string Success = "Request.Success";
                }
            }

            public static class DependencyCall
            {
                public static class PropertyNames
                {
                    public const string Success = "Dependency.Success";
                    public const string TypeName = "Dependency.Type";
                }

                public static class TypeNames
                {
                    public const string Other = "Other";
                    public const string Unknown = "Unknown";
                }
            }
        }
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "This is a temporary private MetricManager")]
    internal static class EventSourceKeywords
    {
        public const long UserActionable = 0x1;

        public const long Diagnostics = 0x2;

        public const long VerboseFailure = 0x4;

        public const long ErrorFailure = 0x8;

        public const long ReservedUserKeywordBegin = 0x10;
    }

    /// <summary>
    /// Metric factory and controller. Sends metrics to Application Insights service. Pre-aggregates metrics to reduce bandwidth.
    /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#send-metrics">Learn more</a>
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "This is a temporary private MetricManager")]
    internal sealed class MetricManager : IDisposable
    {
        /// <summary>
        /// Value of the property indicating 'app insights version' allowing to tell metric was built using metric manager.
        /// </summary>
        private static string sdkVersionPropertyValue = SdkVersionUtils.GetSdkVersion("m-agg:");

        /// <summary>
        /// Reporting frequency.
        /// </summary>
        private static TimeSpan aggregationPeriod = TimeSpan.FromMinutes(1);

        /// <summary>
        /// Telemetry client used to track resulting aggregated metrics.
        /// </summary>
        private readonly TelemetryClient telemetryClient;

        /// <summary>
        /// Telemetry config for this telemetry client.
        /// </summary>
        private readonly TelemetryConfiguration telemetryConfig;

        private SnapshottingList<IMetricProcessor> metricProcessors = new SnapshottingList<IMetricProcessor>();

        /// <summary>
        /// Metric aggregation snapshot task.
        /// </summary>
        private TaskTimerInternal snapshotTimer;

        /// <summary>
        /// Last time snapshot was initiated.
        /// </summary>
        private DateTimeOffset lastSnapshotStartDateTime;

        /// <summary>
        /// A dictionary of all metrics instantiated via this manager.
        /// </summary>
        private ConcurrentDictionary<Metric, SimpleMetricStatisticsAggregator> metricDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetricManager"/> class.
        /// </summary>
        /// <param name="client">Telemetry client to use to output aggregated metric data.</param>
        /// <param name="config">Telemetry configuration for the telemetry client.</param>
        public MetricManager(TelemetryClient client, TelemetryConfiguration config)
        {
            this.telemetryClient = client;
            this.telemetryConfig = config;

            this.metricDictionary = new ConcurrentDictionary<Metric, SimpleMetricStatisticsAggregator>();

            this.lastSnapshotStartDateTime = DateTimeOffset.UtcNow;

            this.snapshotTimer = new TaskTimerInternal() { Delay = GetWaitTime() };
            this.snapshotTimer.Start(this.SnapshotAndReschedule);
        }

        /// <summary>
        /// Gets a list of metric processors associated
        /// with this instance of <see cref="MetricManager"/>.
        /// </summary>
        internal IList<IMetricProcessor> MetricProcessors
        {
            get
            {
                return this.metricProcessors;
            }
        }

        /// <summary>
        /// Creates metric.
        /// </summary>
        /// <param name="name">Name of the metric.</param>
        /// <param name="dimensions">Optional dimensions.</param>
        /// <returns>Metric instance.</returns>
        /// <remarks>
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#send-metrics">Learn more</a>
        /// </remarks>
        public Metric CreateMetric(string name, IDictionary<string, string> dimensions = null)
        {
            return new Metric(this, name, dimensions);
        }

        /// <summary>
        /// Flushes the in-memory aggregation buffers. Not normally required - occurs automatically at intervals and on Dispose.
        /// </summary>
        public void Flush()
        {
            try
            {
                this.Snapshot();
                this.telemetryClient.Flush();
            }
            catch (Exception ex)
            {
                WindowsServerCoreEventSource.Log.FailedToFlushMetricAggregators(ex.ToString());
            }
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        public void Dispose()
        {
            this.snapshotTimer.Dispose();
            this.Flush();
        }

        internal SimpleMetricStatisticsAggregator GetStatisticsAggregator(Metric metric)
        {
            return this.metricDictionary.GetOrAdd(metric, (m) => { return new SimpleMetricStatisticsAggregator(); });
        }

        /// <summary>
        /// Calculates wait time until next snapshot of the aggregators.
        /// </summary>
        /// <returns>Wait time.</returns>
        private static TimeSpan GetWaitTime()
        {
            DateTimeOffset currentTime = DateTimeOffset.UtcNow;

            double minutesFromZero = currentTime.Subtract(DateTimeOffset.MinValue).TotalMinutes;

            // we want to wake up exactly at 1 second past minute
            // to make perceived system latency look smaller
            var nextWakeTime = DateTimeOffset.MinValue
                .AddMinutes((long)minutesFromZero)
                .Add(aggregationPeriod)
                .AddSeconds(1);

            TimeSpan sleepTime = nextWakeTime - DateTimeOffset.UtcNow;

            // adjust wait time to a bit longer than a minute if the wake up time is within few seconds from now
            return sleepTime < TimeSpan.FromSeconds(3) ? sleepTime.Add(aggregationPeriod) : sleepTime;
        }

        /// <summary>
        /// Generates telemetry object based on the metric aggregator.
        /// </summary>
        /// <param name="metric">Metric definition.</param>
        /// <param name="statistics">Metric aggregator statistics calculated for a period of time.</param>
        /// <returns>Metric telemetry object resulting from aggregation.</returns>
        private static MetricTelemetry CreateAggregatedMetricTelemetry(Metric metric, SimpleMetricStatisticsAggregator statistics)
        {
            var telemetry = new MetricTelemetry(
                metric.Name,
                statistics.Count,
                statistics.Sum,
                statistics.Min,
                statistics.Max,
                statistics.StandardDeviation);

            if (metric.Dimensions != null)
            {
                foreach (KeyValuePair<string, string> property in metric.Dimensions)
                {
                    if (string.Compare(property.Key, FirstChanceExceptionStatisticsTelemetryModule.OperationNameTag, StringComparison.Ordinal) == 0)
                    {
                        if (string.IsNullOrEmpty(property.Value) == false)
                        {
                            telemetry.Context.Operation.Name = property.Value;
                        }
                    }
                    else
                    {
                        telemetry.Properties.Add(property);
                    }
                }
            }

            // add a header allowing to distinguish metrics
            // built using metric manager from other metrics
            telemetry.Context.GetInternalContext().SdkVersion = sdkVersionPropertyValue;
            
            return telemetry;
        }

        /// <summary>
        /// Takes a snapshot of aggregators collected by this instance of the manager
        /// and schedules the next snapshot.
        /// </summary>
        private Task SnapshotAndReschedule()
        {
            return Task.Factory.StartNew(
                () =>
                {
                    try
                    {
                        this.Snapshot();
                    }
                    catch (Exception ex)
                    {
                        WindowsServerCoreEventSource.Log.FailedToSnapshotMetricAggregators(ex.ToString());
                    }
                    finally
                    {
                        this.snapshotTimer.Delay = GetWaitTime();
                        this.snapshotTimer.Start(this.SnapshotAndReschedule);
                    }
                });
        }

        /// <summary>
        /// Takes snapshot of all active metric aggregators and turns results into metric telemetry.
        /// </summary>
        private void Snapshot()
        {
            ConcurrentDictionary<Metric, SimpleMetricStatisticsAggregator> aggregatorSnapshot =
                Interlocked.Exchange(ref this.metricDictionary, new ConcurrentDictionary<Metric, SimpleMetricStatisticsAggregator>());

            // calculate aggregation interval duration interval
            TimeSpan aggregationIntervalDuation = DateTimeOffset.UtcNow - this.lastSnapshotStartDateTime;
            this.lastSnapshotStartDateTime = DateTimeOffset.UtcNow;

            // prevent zero duration for interval
            if (aggregationIntervalDuation.TotalMilliseconds < 1)
            {
                aggregationIntervalDuation = TimeSpan.FromMilliseconds(1);
            }

            // adjust interval duration to exactly snapshot frequency if it is close (within 1%)
            double difference = Math.Abs(aggregationIntervalDuation.TotalMilliseconds - aggregationPeriod.TotalMilliseconds);

            if (difference <= aggregationPeriod.TotalMilliseconds / 100)
            {
                aggregationIntervalDuation = aggregationPeriod;
            }

            if (!aggregatorSnapshot.IsEmpty)
            {
                foreach (KeyValuePair<Metric, SimpleMetricStatisticsAggregator> aggregatorWithStats in aggregatorSnapshot)
                {
                    if (aggregatorWithStats.Value.Count > 0)
                    { 
                        MetricTelemetry aggregatedMetricTelemetry = CreateAggregatedMetricTelemetry(aggregatorWithStats.Key, aggregatorWithStats.Value);

                        aggregatedMetricTelemetry.Properties.Add(
                                                        MetricTerms.Aggregation.Interval.Moniker.Key,
                                                        ((long)aggregationIntervalDuation.TotalMilliseconds).ToString(CultureInfo.InvariantCulture));

                        // set the timestamp back by aggregation period
                        aggregatedMetricTelemetry.Timestamp = DateTimeOffset.Now - aggregationPeriod;

                        this.telemetryClient.Track(aggregatedMetricTelemetry);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Represents mechanism to calculate basic statistical parameters of a series of numeric values.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "This is a temporary private MetricManager")]
    internal class SimpleMetricStatisticsAggregator
    {
        /// <summary>
        /// Lock to make Track() method thread-safe.
        /// </summary>
        private SpinLock trackLock = new SpinLock();

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleMetricStatisticsAggregator"/> class.
        /// </summary>
        internal SimpleMetricStatisticsAggregator()
        {
        }

        /// <summary>
        /// Gets sample count.
        /// </summary>
        internal int Count { get; private set; }

        /// <summary>
        /// Gets sum of the samples.
        /// </summary>
        internal double Sum { get; private set; }

        /// <summary>
        /// Gets sum of squares of the samples.
        /// </summary>
        internal double SumOfSquares { get; private set; }

        /// <summary>
        /// Gets minimum sample value.
        /// </summary>
        internal double Min { get; private set; }

        /// <summary>
        /// Gets maximum sample value.
        /// </summary>
        internal double Max { get; private set; }

        /// <summary>
        /// Gets arithmetic average value in the population.
        /// </summary>
        internal double Average
        {
            get
            {
                return this.Count == 0 ? 0 : this.Sum / this.Count;
            }
        }

        /// <summary>
        /// Gets variance of the values in the population.
        /// </summary>
        internal double Variance
        {
            get
            {
                return this.Count == 0 ? 0 : (this.SumOfSquares / this.Count) - (this.Average * this.Average);
            }
        }

        /// <summary>
        /// Gets standard deviation of the values in the population.
        /// </summary>
        internal double StandardDeviation
        {
            get
            {
                return Math.Sqrt(this.Variance);
            }
        }

        /// <summary>
        /// Adds a value to the time series.
        /// </summary>
        /// <param name="value">Metric value.</param>
        public void Track(double value)
        {
            bool lockAcquired = false;

            try
            {
                this.trackLock.Enter(ref lockAcquired);

                if ((this.Count == 0) || (value < this.Min))
                {
                    this.Min = value;
                }

                if ((this.Count == 0) || (value > this.Max))
                {
                    this.Max = value;
                }

                this.Count++;
                this.Sum += value;
                this.SumOfSquares += value * value;
            }
            finally
            {
                if (lockAcquired)
                {
                    this.trackLock.Exit();
                }
            }
        }
    }

    /// <summary>
    /// Represents aggregator for a single time series of a given metric.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "This is a temporary private MetricManager")]
    internal class Metric : IEquatable<Metric>
    {
        /// <summary>
        /// Aggregator manager for the aggregator.
        /// </summary>
        private readonly MetricManager manager;

        /// <summary>
        /// Metric aggregator id to look for in the aggregator dictionary.
        /// </summary>
        private readonly string aggregatorId;

        /// <summary>
        /// Aggregator hash code.
        /// </summary>
        private readonly int hashCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="Metric"/> class.
        /// </summary>
        /// <param name="manager">Aggregator manager handling this instance.</param>
        /// <param name="name">Metric name.</param>
        /// <param name="dimensions">Metric dimensions.</param>
        internal Metric(
            MetricManager manager,
            string name,
            IDictionary<string, string> dimensions = null)
        {
            this.manager = manager ?? throw new ArgumentNullException(nameof(manager));
            this.Name = name;
            this.Dimensions = dimensions;

            this.aggregatorId = Metric.GetAggregatorId(name, dimensions);
            this.hashCode = this.aggregatorId.GetHashCode();
        }

        /// <summary>
        /// Gets metric name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets a set of metric dimensions and their values.
        /// </summary>
        public IDictionary<string, string> Dimensions { get; private set; }

        /// <summary>
        /// Adds a value to the time series.
        /// </summary>
        /// <param name="value">Metric value.</param>
        public void Track(double value)
        {
            SimpleMetricStatisticsAggregator aggregator = this.manager.GetStatisticsAggregator(this);
            aggregator.Track(value);

            this.ForwardToProcessors(value);
        }

        /// <summary>
        /// Returns the hash code for this object.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return this.hashCode;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="other">The object to compare with the current object. </param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public bool Equals(Metric other)
        {
            if (other == null)
            {
                return false;
            }

            return this.aggregatorId.Equals(other.aggregatorId, StringComparison.Ordinal);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object. </param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return this.Equals(obj as Metric);
        }

        /// <summary>
        /// Generates id of the aggregator serving time series specified in the parameters.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="dimensions">Optional metric dimensions.</param>
        /// <returns>Aggregator id that can be used to get aggregator.</returns>
        private static string GetAggregatorId(string name, IDictionary<string, string> dimensions = null)
        {
            StringBuilder aggregatorIdBuilder = new StringBuilder(name ?? "n/a");

            if (dimensions != null)
            {
                var sortedDimensions = dimensions.OrderBy((pair) => { return pair.Key; });

                foreach (KeyValuePair<string, string> pair in sortedDimensions)
                {
                    aggregatorIdBuilder.AppendFormat(CultureInfo.InvariantCulture, "\n{0}\t{1}", pair.Key ?? string.Empty, pair.Value ?? string.Empty);
                }
            }

            return aggregatorIdBuilder.ToString();
        }

        /// <summary>
        /// Forwards value to metric processors.
        /// </summary>
        /// <param name="value">Value tracked on time series.</param>
        private void ForwardToProcessors(double value)
        {
            // create a local reference to metric processor collection
            // if collection changes after that - it will be copied not affecting local reference
            IList<IMetricProcessor> metricProcessors = this.manager.MetricProcessors;

            if (metricProcessors != null)
            {
                int processorCount = metricProcessors.Count;

                for (int i = 0; i < processorCount; i++)
                {
                    IMetricProcessor processor = metricProcessors[i];

                    try
                    {
                        processor.Track(this, value);
                    }
                    catch (Exception ex)
                    {
                        WindowsServerCoreEventSource.Log.FailedToRunMetricProcessor(processor.GetType().FullName, ex.ToString());
                    }
                }
            }
        }
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "This is a temporary private MetricManager")]
    internal abstract class SnapshottingCollection<TItem, TCollection> : ICollection<TItem>
    where TCollection : class, ICollection<TItem>
    {
        protected readonly TCollection Collection;
        protected TCollection snapshot;

        protected SnapshottingCollection(TCollection collection)
        {
            Debug.Assert(collection != null, "collection");
            this.Collection = collection;
        }

        public int Count
        {
            get { return this.GetSnapshot().Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public void Add(TItem item)
        {
            lock (this.Collection)
            {
                this.Collection.Add(item);
                this.snapshot = default(TCollection);
            }
        }

        public void Clear()
        {
            lock (this.Collection)
            {
                this.Collection.Clear();
                this.snapshot = default(TCollection);
            }
        }

        public bool Contains(TItem item)
        {
            return this.GetSnapshot().Contains(item);
        }

        public void CopyTo(TItem[] array, int arrayIndex)
        {
            this.GetSnapshot().CopyTo(array, arrayIndex);
        }

        public bool Remove(TItem item)
        {
            lock (this.Collection)
            {
                bool removed = this.Collection.Remove(item);
                if (removed)
                {
                    this.snapshot = default(TCollection);
                }

                return removed;
            }
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            return this.GetSnapshot().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        protected abstract TCollection CreateSnapshot(TCollection collection);

        protected TCollection GetSnapshot()
        {
            TCollection localSnapshot = this.snapshot;
            if (localSnapshot == null)
            {
                lock (this.Collection)
                {
                    this.snapshot = this.CreateSnapshot(this.Collection);
                    localSnapshot = this.snapshot;
                }
            }

            return localSnapshot;
        }
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "This is a temporary private MetricManager")]
    internal class SnapshottingList<T> : SnapshottingCollection<T, IList<T>>, IList<T>
    {
        public SnapshottingList()
            : base(new List<T>())
        {
        }

        public T this[int index]
        {
            get
            {
                return this.GetSnapshot()[index];
            }

            set
            {
                lock (this.Collection)
                {
                    this.Collection[index] = value;
                    this.snapshot = null;
                }
            }
        }

        public int IndexOf(T item)
        {
            return this.GetSnapshot().IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            lock (this.Collection)
            {
                this.Collection.Insert(index, item);
                this.snapshot = null;
            }
        }

        public void RemoveAt(int index)
        {
            lock (this.Collection)
            {
                this.Collection.RemoveAt(index);
                this.snapshot = null;
            }
        }

        protected sealed override IList<T> CreateSnapshot(IList<T> collection)
        {
            return new List<T>(collection);
        }
    }

    /// <summary>
    /// Runs a task after a certain delay and log any error.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "This is a temporary private MetricManager")]
    internal class TaskTimerInternal : IDisposable
    {
        /// <summary>
        /// Represents an infinite time span.
        /// </summary>
        public static readonly TimeSpan InfiniteTimeSpan = new TimeSpan(0, 0, 0, 0, Timeout.Infinite);

        private TimeSpan delay = TimeSpan.FromMinutes(1);
        private CancellationTokenSource tokenSource;

        /// <summary>
        /// Gets or sets the delay before the task starts. 
        /// </summary>
        public TimeSpan Delay
        {
            get
            {
                return this.delay;
            }

            set
            {
                if ((value <= TimeSpan.Zero || value.TotalMilliseconds > int.MaxValue) && value != InfiniteTimeSpan)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                this.delay = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether value that indicates if a task has already started.
        /// </summary>
        public bool IsStarted
        {
            get { return this.tokenSource != null; }
        }

        /// <summary>
        /// Start the task.
        /// </summary>
        /// <param name="elapsed">The task to run.</param>
        public void Start(Func<Task> elapsed)
        {
            var newTokenSource = new CancellationTokenSource();

            TaskEx.Delay(this.Delay, newTokenSource.Token)
                .ContinueWith(
                    async previousTask =>
                        {
                            CancelAndDispose(Interlocked.CompareExchange(ref this.tokenSource, null, newTokenSource));
                            try
                            {
                                Task task = elapsed();

                                // Task may be executed synchronously
                                // It should return Task.FromResult but just in case we check for null if someone returned null
                                if (task != null)
                                {
                                    await task.ConfigureAwait(false);
                                }
                            }
                            catch (Exception exception)
                            {
                                LogException(exception);
                            }
                        },
                    CancellationToken.None,
                    TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.ExecuteSynchronously,
                    TaskScheduler.Default);

            CancelAndDispose(Interlocked.Exchange(ref this.tokenSource, newTokenSource));
        }

        /// <summary>
        /// Cancels the current task.
        /// </summary>
        public void Cancel()
        {
            CancelAndDispose(Interlocked.Exchange(ref this.tokenSource, null));
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Log exception thrown by outer code.
        /// </summary>
        /// <param name="exception">Exception to log.</param>
        private static void LogException(Exception exception)
        {
            var aggregateException = exception as AggregateException;
            if (aggregateException != null)
            {
                aggregateException = aggregateException.Flatten();
                foreach (Exception e in aggregateException.InnerExceptions)
                {
                    WindowsServerCoreEventSource.Log.LogError(e.ToInvariantString());
                }
            }

            WindowsServerCoreEventSource.Log.LogError(exception.ToInvariantString());
        }

        private static void CancelAndDispose(CancellationTokenSource tokenSource)
        {
            if (tokenSource != null)
            {
                tokenSource.Cancel();
                tokenSource.Dispose();
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Cancel();
            }
        }
    }
    
    [EventSource(Name = "Microsoft-ApplicationInsights-WindowsServer-Core")]
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "This is a temporary private MetricManager")]
    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "appDomainName is required")]
    internal sealed class WindowsServerCoreEventSource : EventSource
    {
        public static readonly WindowsServerCoreEventSource Log = new WindowsServerCoreEventSource();

        private readonly ApplicationNameProvider nameProvider = new ApplicationNameProvider();

        public static bool IsVerboseEnabled
        {
            [NonEvent]
            get
            {
                return Log.IsEnabled(EventLevel.Verbose, (EventKeywords)(-1));
            }
        }

        /// <summary>
        /// Logs the information when there operation to track is null.
        /// </summary>
        [Event(1, Message = "Operation object is null.", Level = EventLevel.Warning)]
        public void OperationIsNullWarning(string appDomainName = "Incorrect")
        {
            this.WriteEvent(1, this.nameProvider.Name);
        }

        /// <summary>
        /// Logs the information when there operation to stop does not match the current operation.
        /// </summary>
        [Event(2, Message = "Operation to stop does not match the current operation.", Level = EventLevel.Error)]
        public void InvalidOperationToStopError(string appDomainName = "Incorrect")
        {
            this.WriteEvent(2, this.nameProvider.Name);
        }

        [Event(
            3,
            Keywords = Keywords.VerboseFailure,
            Message = "[msg=Log verbose];[msg={0}]",
            Level = EventLevel.Verbose)]
        public void LogVerbose(string msg, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                3,
                msg ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            4,
            Keywords = Keywords.Diagnostics | Keywords.UserActionable,
            Message = "Diagnostics event throttling has been started for the event {0}",
            Level = EventLevel.Informational)]
        public void DiagnosticsEventThrottlingHasBeenStartedForTheEvent(
            string eventId,
            string appDomainName = "Incorrect")
        {
            this.WriteEvent(4, eventId ?? "NULL", this.nameProvider.Name);
        }

        [Event(
            5,
            Keywords = Keywords.Diagnostics | Keywords.UserActionable,
            Message = "Diagnostics event throttling has been reset for the event {0}, event was fired {1} times during last interval",
            Level = EventLevel.Informational)]
        public void DiagnosticsEventThrottlingHasBeenResetForTheEvent(
            int eventId,
            int executionCount,
            string appDomainName = "Incorrect")
        {
            this.WriteEvent(5, eventId, executionCount, this.nameProvider.Name);
        }

        [Event(
            6,
            Keywords = Keywords.Diagnostics,
            Message = "Scheduler timer dispose failure: {0}",
            Level = EventLevel.Warning)]
        public void DiagnoisticsEventThrottlingSchedulerDisposeTimerFailure(
            string exception,
            string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                6,
                exception ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            7,
            Keywords = Keywords.Diagnostics,
            Message = "A scheduler timer was created for the interval: {0}",
            Level = EventLevel.Verbose)]
        public void DiagnoisticsEventThrottlingSchedulerTimerWasCreated(
            string intervalInMilliseconds,
            string appDomainName = "Incorrect")
        {
            this.WriteEvent(7, intervalInMilliseconds ?? "NULL", this.nameProvider.Name);
        }

        [Event(
            8,
            Keywords = Keywords.Diagnostics,
            Message = "A scheduler timer was removed",
            Level = EventLevel.Verbose)]
        public void DiagnoisticsEventThrottlingSchedulerTimerWasRemoved(string appDomainName = "Incorrect")
        {
            this.WriteEvent(8, this.nameProvider.Name);
        }

        [Event(
            9,
            Message = "No Telemetry Configuration provided. Using the default TelemetryConfiguration.Active.",
            Level = EventLevel.Warning)]
        public void TelemetryClientConstructorWithNoTelemetryConfiguration(string appDomainName = "Incorrect")
        {
            this.WriteEvent(9, this.nameProvider.Name);
        }

        [Event(
            10,
            Message = "Value for property '{0}' of {1} was not found. Populating it by default.",
            Level = EventLevel.Verbose)]
        public void PopulateRequiredStringWithValue(string parameterName, string telemetryType, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                10,
                parameterName ?? string.Empty,
                telemetryType ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            11,
            Message = "Invalid duration for Telemetry. Setting it to '00:00:00'.",
            Level = EventLevel.Warning)]
        public void TelemetryIncorrectDuration(string appDomainName = "Incorrect")
        {
            this.WriteEvent(11, this.nameProvider.Name);
        }

        [Event(
           12,
           Message = "Telemetry tracking was disabled. Message is dropped.",
           Level = EventLevel.Verbose)]
        public void TrackingWasDisabled(string appDomainName = "Incorrect")
        {
            this.WriteEvent(12, this.nameProvider.Name);
        }

        [Event(
           13,
           Message = "Telemetry tracking was enabled. Messages are being logged.",
           Level = EventLevel.Verbose)]
        public void TrackingWasEnabled(string appDomainName = "Incorrect")
        {
            this.WriteEvent(13, this.nameProvider.Name);
        }

        [Event(
            14,
            Keywords = Keywords.ErrorFailure,
            Message = "[msg=Log Error];[msg={0}]",
            Level = EventLevel.Error)]
        public void LogError(string msg, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                14,
                msg ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            15,
            Keywords = Keywords.UserActionable,
            Message = "ApplicationInsights configuration file loading failed. Type '{0}' was not found. Type loading was skipped. Monitoring will continue.",
            Level = EventLevel.Error)]
        public void TypeWasNotFoundConfigurationError(string type, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                15,
                type ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            16,
            Keywords = Keywords.UserActionable,
            Message = "ApplicationInsights configuration file loading failed. Type '{0}' does not implement '{1}'. Type loading was skipped. Monitoring will continue.",
            Level = EventLevel.Error)]
        public void IncorrectTypeConfigurationError(string type, string expectedType, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                16,
                type ?? string.Empty,
                expectedType ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            17,
            Keywords = Keywords.UserActionable,
            Message = "ApplicationInsights configuration file loading failed. Type '{0}' does not have property '{1}'. Property initialization was skipped. Monitoring will continue.",
            Level = EventLevel.Error)]
        public void IncorrectPropertyConfigurationError(string type, string property, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                17,
                type ?? string.Empty,
                property ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            18,
            Keywords = Keywords.UserActionable,
            Message = "ApplicationInsights configuration file loading failed. Element '{0}' element does not have a Type attribute, does not specify a value and is not a valid collection type. Type initialization was skipped. Monitoring will continue.",
            Level = EventLevel.Error)]
        public void IncorrectInstanceAtributesConfigurationError(string definition, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                18,
                definition ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            19,
            Keywords = Keywords.UserActionable,
            Message = "ApplicationInsights configuration file loading failed. '{0}' element has unexpected contents: '{1}': '{2}'. Type initialization was skipped. Monitoring will continue.",
            Level = EventLevel.Error)]
        public void LoadInstanceFromValueConfigurationError(string element, string contents, string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                19,
                element ?? string.Empty,
                contents ?? string.Empty,
                error ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            20,
            Keywords = Keywords.UserActionable,
            Message = "ApplicationInsights configuration file loading failed. Exception: '{0}'. Monitoring will continue if you set InstrumentationKey programmatically.",
            Level = EventLevel.Error)]
        public void ConfigurationFileCouldNotBeParsedError(string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                20,
                error ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            21,
            Keywords = Keywords.UserActionable,
            Message = "ApplicationInsights configuration file loading failed. Type '{0}' will not be create. Error: '{1}'. Monitoring will continue if you set InstrumentationKey programmatically.",
            Level = EventLevel.Error)]
        public void MissingMethodExceptionConfigurationError(string type, string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                21,
                type ?? string.Empty,
                error ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            22,
            Keywords = Keywords.UserActionable,
            Message = "ApplicationInsights configuration file loading failed. Type '{0}' will not be initialized. Error: '{1}'. Monitoring will continue if you set InstrumentationKey programmatically.",
            Level = EventLevel.Error)]
        public void ComponentInitializationConfigurationError(string type, string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                22,
                type ?? string.Empty,
                error ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            23,
            Message = "ApplicationInsights configuration file '{0}' was not found.",
            Level = EventLevel.Warning)]
        public void ApplicationInsightsConfigNotFoundWarning(string file, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                23,
                file ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            24,
            Message = "Failed to send: {0}.",
            Level = EventLevel.Warning)]
        public void FailedToSend(string msg, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                24,
                msg ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
           25,
           Message = "Exception happened during getting the machine name: '{0}'.",
           Level = EventLevel.Error)]
        public void FailedToGetMachineName(string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                25,
                error ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            26,
            Message = "Failed to flush aggregated metrics. Exception: {0}.",
            Level = EventLevel.Error)]
        public void FailedToFlushMetricAggregators(string ex, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                26,
                ex ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            27,
            Message = "Failed to snapshot aggregated metrics. Exception: {0}.",
            Level = EventLevel.Error)]
        public void FailedToSnapshotMetricAggregators(string ex, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                27,
                ex ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            28,
            Message = "Failed to invoke metric processor '{0}'. If the issue persists, remove the processor. Exception: {1}.",
            Level = EventLevel.Error)]
        public void FailedToRunMetricProcessor(string processorName, string ex, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                28,
                processorName ?? string.Empty,
                ex ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            29,
            Message = "The backlog of unsent items has reached maximum size of {0}. Items will be dropped until the backlog is cleared.",
            Level = EventLevel.Error)]
        public void ItemDroppedAsMaximumUnsentBacklogSizeReached(int maxBacklogSize, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                29,
                maxBacklogSize,
                this.nameProvider.Name);
        }

        [Event(
            30,
            Message = "Flush was called on the telemetry channel (InMemoryChannel) after it was disposed.",
            Level = EventLevel.Warning)]
        public void InMemoryChannelFlushedAfterBeingDisposed(string appDomainName = "Incorrect")
        {
            this.WriteEvent(30, this.nameProvider.Name);
        }

        [Event(
            31,
            Message = "Send was called on the telemetry channel (InMemoryChannel) after it was disposed, the telemetry data was dropped.",
            Level = EventLevel.Warning)]
        public void InMemoryChannelSendCalledAfterBeingDisposed(string appDomainName = "Incorrect")
        {
            this.WriteEvent(31, this.nameProvider.Name);
        }

        [Event(
            32,
            Message = "Failed to get environment variables due to security exception; code is likely running in partial trust. Exception: {0}.",
            Level = EventLevel.Warning)]
        public void FailedToLoadEnvironmentVariables(string ex, string appDomainName = "Incorrect")
        {
            this.WriteEvent(32, ex, this.nameProvider.Name);
        }

        // Verbosity is Error - so it is always sent to portal; Keyword is Diagnostics so throttling is not applied.
        [Event(33,
            Message = "A Metric Extractor detected a telemetry item with SamplingPercentage < 100. Metrics Extractors should be used before Sampling Processors or any other Telemetry Processors that might filter out Telemetry Items. Otherwise, extracted metrics may be incorrect.",
            Level = EventLevel.Error,
            Keywords = Keywords.Diagnostics | Keywords.UserActionable)]
        public void MetricExtractorAfterSamplingError(string appDomainName = "Incorrect")
        {
            this.WriteEvent(33, this.nameProvider.Name);
        }

        // Verbosity is Verbose - targeted at support personnel; Keyword is Diagnostics so throttling is not applied.
        [Event(34,
            Message = "A Metric Extractor detected a telemetry item with SamplingPercentage < 100. Metrics Extractors Extractor should be used before Sampling Processors or any other Telemetry Processors that might filter out Telemetry Items. Otherwise, extracted metrics may be incorrect.",
            Level = EventLevel.Verbose,
            Keywords = Keywords.Diagnostics | Keywords.UserActionable)]
        public void MetricExtractorAfterSamplingVerbose(string appDomainName = "Incorrect")
        {
            this.WriteEvent(34, this.nameProvider.Name);
        }

        /// <summary>
        /// Keywords for the PlatformEventSource.
        /// </summary>
        public sealed class Keywords
        {
            /// <summary>
            /// Key word for user actionable events.
            /// </summary>
            public const EventKeywords UserActionable = (EventKeywords)EventSourceKeywords.UserActionable;

            /// <summary>
            /// Keyword for errors that trace at Verbose level.
            /// </summary>
            public const EventKeywords Diagnostics = (EventKeywords)EventSourceKeywords.Diagnostics;

            /// <summary>
            /// Keyword for errors that trace at Verbose level.
            /// </summary>
            public const EventKeywords VerboseFailure = (EventKeywords)EventSourceKeywords.VerboseFailure;

            /// <summary>
            /// Keyword for errors that trace at Error level.
            /// </summary>
            public const EventKeywords ErrorFailure = (EventKeywords)EventSourceKeywords.ErrorFailure;
        }
    }
}
#endif