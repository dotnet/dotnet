namespace Microsoft.ApplicationInsights.Metrics.Extensibility
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using static System.FormattableString;

    /// <summary>An adapter that represents the Application Insights SDK pipelie towards the Metrics Aggregation SDK subsystem.</summary>
    /// @PublicExposureCandidate
    internal class ApplicationInsightsTelemetryPipeline : IMetricTelemetryPipeline
    {
        private readonly ApplicationInsights.TelemetryClient trackingClient;
        private readonly Task completedTask = Task.FromResult(true);

        /// <summary>Creaes a new Application Insights telemetry pipeline adapter.</summary>
        /// <param name="telemetryPipeline">The Application Insights telemetry pipeline to be adapted.</param>
        public ApplicationInsightsTelemetryPipeline(ApplicationInsights.Extensibility.TelemetryConfiguration telemetryPipeline)
        {
            Util.ValidateNotNull(telemetryPipeline, nameof(telemetryPipeline));

            this.trackingClient = new ApplicationInsights.TelemetryClient(telemetryPipeline);
        }

        /// <summary>Creaes a new Application Insights telemetry pipeline adapter.</summary>
        /// <param name="telemetryClient">The Application Insights telemetry pipeline to be adapted.</param>
        public ApplicationInsightsTelemetryPipeline(ApplicationInsights.TelemetryClient telemetryClient)
        {
            Util.ValidateNotNull(telemetryClient, nameof(telemetryClient));

            this.trackingClient = telemetryClient;
        }

        /// <summary>
        /// Send a metric aggregate to the cloud using the local Application Insights pipeline.
        /// </summary>
        /// <param name="metricAggregate">The aggregate.</param>
        /// <param name="cancelToken">Cancellation is not supported by the underlying pipeline, but it is respected be this method.</param>
        /// <exception cref="ArgumentNullException">The specified <c>metricAggregate</c> is null.</exception>
        /// <exception cref="ArgumentException">The runtime class of the specified <c>metricAggregate</c> does not match the
        ///     telemetry destination type represented by this instance of <c>IMetricTelemetryPipeline</c>.</exception>
        /// <exception cref="OperationCanceledException">The specified <c>cancelToken</c> has had cancellation requested.</exception>
        /// <returns>The task representing the Track operation.</returns>
        public Task TrackAsync(MetricAggregate metricAggregate, CancellationToken cancelToken)
        {
            Util.ValidateNotNull(metricAggregate, nameof(metricAggregate));
            Util.ValidateNotNull(metricAggregate.AggregationKindMoniker, nameof(metricAggregate.AggregationKindMoniker));

            cancelToken.ThrowIfCancellationRequested();

            IMetricAggregateToTelemetryPipelineConverter converter;
            bool hasConverter = MetricAggregateToTelemetryPipelineConverters.Registry.TryGet(
                                                                                            typeof(ApplicationInsightsTelemetryPipeline),
                                                                                            metricAggregate.AggregationKindMoniker,
                                                                                            out converter);
            if (false == hasConverter)
            {
                throw new ArgumentException(Invariant($"Cannot track the specified {metricAggregate}, because there is no {nameof(IMetricAggregateToTelemetryPipelineConverter)}")
                                          + Invariant($" registered for it. A converter must be added to {nameof(MetricAggregateToTelemetryPipelineConverters)}")
                                          + Invariant($".{nameof(MetricAggregateToTelemetryPipelineConverters.Registry)} for the pipeline type")
                                          + Invariant($" '{nameof(ApplicationInsightsTelemetryPipeline)}' and {nameof(metricAggregate.AggregationKindMoniker)}")
                                          + Invariant($" '{metricAggregate.AggregationKindMoniker}'."));
            }

            object telemetryItem = converter.Convert(metricAggregate);
            var metricTelemetryItem = (ApplicationInsights.DataContracts.MetricTelemetry)telemetryItem;
            this.trackingClient.Track(metricTelemetryItem);

            return this.completedTask;
        }

        /// <summary>Flushes the Application Insights pipeline used by this adaptor.</summary>
        /// <param name="cancelToken">Cancellation is not supported by the underlying pipeline, but it is respected be this method.</param>
        /// <returns>The task representing the Flush operation.</returns>
        public Task FlushAsync(CancellationToken cancelToken)
        {
            cancelToken.ThrowIfCancellationRequested();

            this.trackingClient.Flush();

            return this.completedTask;
        }
    }
}
