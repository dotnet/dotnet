namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System.Diagnostics.Tracing;
    
    /// <summary>
    /// Event Source exposes Application Insights telemetry information as ETW events.
    /// </summary>
    internal partial class RichPayloadEventSource
    {
        /// <summary>
        /// Keywords for the RichPayloadEventSource.
        /// </summary>
        public sealed class Keywords
        {
            /// <summary>
            /// Keyword for requests.
            /// </summary>
            public const EventKeywords Requests = (EventKeywords)0x1;

            /// <summary>
            /// Keyword for traces.
            /// </summary>
            public const EventKeywords Traces = (EventKeywords)0x2;

            /// <summary>
            /// Keyword for events.
            /// </summary>
            public const EventKeywords Events = (EventKeywords)0x4;

            /// <summary>
            /// Keyword for exceptions.
            /// </summary>
            public const EventKeywords Exceptions = (EventKeywords)0x8;

            /// <summary>
            /// Keyword for dependencies.
            /// </summary>
            public const EventKeywords Dependencies = (EventKeywords)0x10;

            /// <summary>
            /// Keyword for metrics.
            /// </summary>
            public const EventKeywords Metrics = (EventKeywords)0x20;

            /// <summary>
            /// Keyword for page views.
            /// </summary>
            public const EventKeywords PageViews = (EventKeywords)0x40;

            /// <summary>
            /// Keyword for performance counters.
            /// </summary>
            public const EventKeywords PerformanceCounters = (EventKeywords)0x80;

            /// <summary>
            /// Keyword for session state.
            /// </summary>
            public const EventKeywords SessionState = (EventKeywords)0x100;

            /// <summary>
            /// Keyword for availability.
            /// </summary>
            public const EventKeywords Availability = (EventKeywords)0x200;

            /// <summary>
            /// Keyword for operations (Start/Stop).
            /// </summary>
            public const EventKeywords Operations = (EventKeywords)0x400;

            /// <summary>
            /// Keyword for page view performance.
            /// </summary>
            public const EventKeywords PageViewPerformance = (EventKeywords)0x800;
        }
    }
}
