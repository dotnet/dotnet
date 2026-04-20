//-----------------------------------------------------------------------
// <copyright file="EventSourceModuleDiagnosticListener.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.EtwTelemetryCollector.Tests
{
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;

    internal class EventSourceModuleDiagnosticListener : EventListener
    {
        public EventSourceModuleDiagnosticListener()
        {
            this.EventsReceived = new List<EventWrittenEventArgs>();
        }

        public IList<EventWrittenEventArgs> EventsReceived { get; private set; }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            this.EventsReceived.Add(eventData);
        }

        protected override void OnEventSourceCreated(EventSource eventSource)
        {
#if REDFIELD
            if (string.Equals(eventSource.Name, "Redfield-Microsoft-ApplicationInsights-Extensibility-EventSourceListener", System.StringComparison.Ordinal))
#else
            if (string.Equals(eventSource.Name, "Microsoft-ApplicationInsights-Extensibility-EventSourceListener", System.StringComparison.Ordinal))
#endif   
            {
                EnableEvents(eventSource, EventLevel.LogAlways);
            }
        }
    }
}
