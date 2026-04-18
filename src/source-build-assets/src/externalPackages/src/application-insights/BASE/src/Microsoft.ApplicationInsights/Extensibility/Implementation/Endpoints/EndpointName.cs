namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Endpoints
{
    /// <summary>
    /// These enums represent all possible endpoints within application insights infrastructure.
    /// </summary>
    internal enum EndpointName
    {
        /// <summary>Telemetry ingestion service (aka Breeze).</summary>
        [EndpointMeta(ExplicitName = "IngestionEndpoint", EndpointPrefix = Constants.IngestionPrefix, Default = Constants.DefaultIngestionEndpoint)]
        Ingestion,

        /// <summary>Live Metrics service (aka QuickPulse).</summary>
        [EndpointMeta(ExplicitName = "LiveEndpoint", EndpointPrefix = Constants.LiveMetricsPrefix, Default = Constants.DefaultLiveMetricsEndpoint)]
        Live,

        /// <summary>Application Insights Profiler service.</summary>
        [EndpointMeta(ExplicitName = "ProfilerEndpoint", EndpointPrefix = Constants.ProfilerPrefix, Default = Constants.DefaultProfilerEndpoint)]
        Profiler,

        /// <summary>Snapshot debugger service.</summary>
        [EndpointMeta(ExplicitName = "SnapshotEndpoint", EndpointPrefix = Constants.SnapshotPrefix, Default = Constants.DefaultSnapshotEndpoint)]
        Snapshot,
    }
}
