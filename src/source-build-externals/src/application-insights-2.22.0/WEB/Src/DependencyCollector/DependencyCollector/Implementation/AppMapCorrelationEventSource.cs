namespace Microsoft.ApplicationInsights.Common
{
    using System;
    using System.Diagnostics.Tracing;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// ETW EventSource tracing class.
    /// </summary>
#if REDFIELD
    [EventSource(Name = "Redfield-Microsoft-ApplicationInsights-Extensibility-AppMapCorrelation-Dependency")]
#else
    [EventSource(Name = "Microsoft-ApplicationInsights-Extensibility-AppMapCorrelation-Dependency")]
#endif
    internal sealed partial class AppMapCorrelationEventSource : EventSource
    {
    }
}
