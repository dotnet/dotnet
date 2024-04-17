namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Endpoints
{
    /// <summary>
    /// Endpoint Constants.
    /// </summary>
    internal static class Constants
    {
        /// <summary>Default endpoint for Ingestion (aka Ingestion).</summary>
        internal const string DefaultIngestionEndpoint = "https://dc.services.visualstudio.com/";

        /// <summary>Default endpoint for Live Metrics (aka QuickPulse).</summary>
        internal const string DefaultLiveMetricsEndpoint = "https://rt.services.visualstudio.com/";

        /// <summary>Default endpoint for Profiler.</summary>
        internal const string DefaultProfilerEndpoint = "https://profiler.monitor.azure.com/";

        /// <summary>Default endpoint for Snapshot Debugger.</summary>
        internal const string DefaultSnapshotEndpoint = "https://snapshot.monitor.azure.com/";

        /// <summary>Sub-domain for Ingestion endpoint (aka Breeze). (https://dc.applicationinsights.azure.com/).</summary>
        internal const string IngestionPrefix = "dc";

        /// <summary>Sub-domain for Live Metrics endpoint (aka QuickPulse). (https://live.applicationinsights.azure.com/).</summary>
        internal const string LiveMetricsPrefix = "live";

        /// <summary>Sub-domain for Profiler endpoint. (https://profiler.applicationinsights.azure.com/).</summary>
        internal const string ProfilerPrefix = "profiler";

        /// <summary>Sub-domain for Snapshot Debugger endpoint. (https://snapshot.applicationinsights.azure.com/).</summary>
        internal const string SnapshotPrefix = "snapshot";
    }
}
