namespace Microsoft.ApplicationInsights.Metrics.Extensibility
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents an eventual destination for metric telemetry.
    /// For example, an Application Insights telemetry pipeline, a file or some other ingestion point.
    /// </summary>
    public interface IMetricTelemetryPipeline
    {
        /// <summary>
        /// Send a metric aggregate to the eventual destination.
        /// </summary>
        /// <param name="metricAggregate">The aggregate.</param>
        /// <param name="cancelToken">Cancellation may or may not be supported by different destinations.</param>
        /// <exception cref="ArgumentNullException">The specified <c>metricAggregate</c> is null.</exception>
        /// <exception cref="ArgumentException">The runtime class of the specified <c>metricAggregate</c> does not match the
        ///     telemetry destination type represented by this instance of <c>IMetricTelemetryPipeline</c>.</exception>
        /// <exception cref="OperationCanceledException">The specified <c>cancelToken</c> has had cancellation requested.</exception>
        /// <returns>The task representing the Track operation.</returns>
        Task TrackAsync(MetricAggregate metricAggregate, CancellationToken cancelToken);

        /// <summary>Flushes the telemetry pipeline in case it had cached any data.</summary>
        /// <param name="cancelToken">Cancellation may or may not be supported by different destinations.</param>
        /// <returns>The task representing the Flush operation.</returns>
        Task FlushAsync(CancellationToken cancelToken);
    }
}
