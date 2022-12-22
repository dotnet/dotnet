namespace Microsoft.ApplicationInsights.Metrics
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Metrics.Extensibility;

    /// <summary>A metric manager coordinates metrics aggregation at a specific scope.
    /// It keeps track of the known metrics and is ultimataly respnsibe for correctly
    /// initializeing metric data time series.
    /// Note that a metric manager deals with zero dimensional time series.
    /// Metric objects are multidimensional collections of such series and the manager 
    /// merely holds a collection of such containers for its scope.</summary>
    public sealed class MetricManager
    {
        private readonly MetricAggregationManager aggregationManager;
        private readonly DefaultAggregationPeriodCycle aggregationCycle;
        private readonly IMetricTelemetryPipeline telemetryPipeline;
        private readonly MetricsCollection metrics;

        /// <summary>Initializes a new metric manager.</summary>
        /// <param name="telemetryPipeline">The destination where aggregates will be sent.</param>
        public MetricManager(IMetricTelemetryPipeline telemetryPipeline)
        {
            Util.ValidateNotNull(telemetryPipeline, nameof(telemetryPipeline));

            this.telemetryPipeline = telemetryPipeline;
            this.aggregationManager = new MetricAggregationManager();
            this.aggregationCycle = new DefaultAggregationPeriodCycle(this.aggregationManager, this);

            this.metrics = new MetricsCollection(this);

            this.aggregationCycle.Start();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="MetricManager" /> class.
        /// </summary>
        ~MetricManager()
        {
            DefaultAggregationPeriodCycle aggregationCycle = this.aggregationCycle;
            if (aggregationCycle != null)
            {
                Task fireAndForget = this.aggregationCycle.StopAsync();
            }
        }

        /// <summary>Gets the collection of metrics available the this manager's scope.</summary>
        public MetricsCollection Metrics
        {
            get { return this.metrics; }
        }

        internal MetricAggregationManager AggregationManager
        {
            get { return this.aggregationManager; }
        }

        internal DefaultAggregationPeriodCycle AggregationCycle
        {
            get { return this.aggregationCycle; }
        }

        /// <summary>Creates and initilizes a new metric data time series.</summary>
        /// <param name="metricNamespace">Namespace of the metric to whcih the series belongs.</param>
        /// <param name="metricId">Id (name) if the metric to which the series belongs.</param>
        /// <param name="config">Configuration of the series, including the aggregatio kind and other aspects.</param>
        /// <returns>A new metric data time series.</returns>
        public MetricSeries CreateNewSeries(string metricNamespace, string metricId, IMetricSeriesConfiguration config)
        {
            return this.CreateNewSeries(
                            metricNamespace,
                            metricId,
                            dimensionNamesAndValues: null,
                            config: config);
        }

        /// <summary>Creates and initilizes a new metric data time series.</summary>
        /// <param name="metricNamespace">Namespace of the metric to whcih the series belongs.</param>
        /// <param name="metricId">Id (name) if the metric to which the series belongs.</param>
        /// <param name="dimensionNamesAndValues">The dimension names and values of the series within its metric.</param>
        /// <param name="config">Configuration of the series, including the aggregatio kind and other aspects.</param>
        /// <returns>A new metric data time series.</returns>
        public MetricSeries CreateNewSeries(
                                    string metricNamespace, 
                                    string metricId, 
                                    IEnumerable<KeyValuePair<string, string>> dimensionNamesAndValues, 
                                    IMetricSeriesConfiguration config)
        {
            // Create MetricIdentifier (it will also validate metricNamespace and metricId):
            List<string> dimNames = null;
            if (dimensionNamesAndValues != null)
            {
                dimNames = new List<string>();
                foreach (KeyValuePair<string, string> dimNameVal in dimensionNamesAndValues)
                {
                    dimNames.Add(dimNameVal.Key);
                }
            }

            var metricIdentifier = new MetricIdentifier(metricNamespace, metricId, dimNames);

            // Create series:
            return this.CreateNewSeries(metricIdentifier, dimensionNamesAndValues, config);
        }

        /// <summary>Creates and initilizes a new metric data time series.</summary>
        /// <param name="metricIdentifier">THe identify of the metric to whcih the series belongs.</param>
        /// <param name="dimensionNamesAndValues">The dimension names and values of the series within its metric.</param>
        /// <param name="config">Configuration of the series, including the aggregatio kind and other aspects.</param>
        /// <returns>A new metric data time series.</returns>
        public MetricSeries CreateNewSeries(MetricIdentifier metricIdentifier, IEnumerable<KeyValuePair<string, string>> dimensionNamesAndValues, IMetricSeriesConfiguration config)
        {
            Util.ValidateNotNull(metricIdentifier, nameof(metricIdentifier));
            Util.ValidateNotNull(config, nameof(config));

            var dataSeries = new MetricSeries(this.aggregationManager, metricIdentifier, dimensionNamesAndValues, config);
            return dataSeries;
        }

        /// <summary>Flushes cached metric data. The default aggregation cycle will be completed/restarted if required.</summary>
        public void Flush()
        {
            this.Flush(flushDownstreamPipeline: true);
        }

        internal void Flush(bool flushDownstreamPipeline)
        {
            CoreEventSource.Log.MetricManagerFlush();

            DateTimeOffset now = DateTimeOffset.Now;
            AggregationPeriodSummary aggregates = this.aggregationManager.StartOrCycleAggregators(MetricAggregationCycleKind.Default, futureFilter: null, tactTimestamp: now);
            this.TrackMetricAggregates(aggregates, flushDownstreamPipeline);
        }

        internal void TrackMetricAggregates(AggregationPeriodSummary aggregates, bool flush)
        {
            int? nonpersistentAggregatesCount = aggregates?.NonpersistentAggregates?.Count;
            int? persistentAggregatesCount = aggregates?.PersistentAggregates?.Count;

            int totalAggregatesCount = (nonpersistentAggregatesCount ?? 0) + (persistentAggregatesCount ?? 0);

            if (totalAggregatesCount == 0)
            {
                return;
            }

            Task[] trackTasks = new Task[totalAggregatesCount];
            int taskIndex = 0;

            if (nonpersistentAggregatesCount != 0)
            {
                foreach (MetricAggregate telemetryItem in aggregates.NonpersistentAggregates)
                {
                    if (telemetryItem != null)
                    {
                        Task trackTask = this.telemetryPipeline.TrackAsync(telemetryItem, CancellationToken.None);
                        trackTasks[taskIndex++] = trackTask;
                    }
                }
            }

            if (aggregates.PersistentAggregates != null && aggregates.PersistentAggregates.Count != 0)
            {
                foreach (MetricAggregate telemetryItem in aggregates.PersistentAggregates)
                {
                    if (telemetryItem != null)
                    {
                        Task trackTask = this.telemetryPipeline.TrackAsync(telemetryItem, CancellationToken.None);
                        trackTasks[taskIndex++] = trackTask;
                    }
                }
            }

            CoreEventSource.Log.MetricManagerCreatedTasks(trackTasks.Length);
            Task.WaitAll(trackTasks);

            if (flush)
            {
                Task flushTask = this.telemetryPipeline.FlushAsync(CancellationToken.None);
                flushTask.Wait();
            }
        }
    }        
}
