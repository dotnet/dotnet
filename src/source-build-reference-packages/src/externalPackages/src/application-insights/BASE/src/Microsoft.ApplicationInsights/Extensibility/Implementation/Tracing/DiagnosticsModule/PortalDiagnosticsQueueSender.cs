namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule
{
    using System.Collections.Generic;

    /// <summary>
    /// A dummy queue sender to keep the data to be sent to the portal before the initialize method is called.
    /// This is due to the fact that initialize method cannot be called without the configuration and 
    /// the event listener write event is triggered before the diagnosticTelemetryModule initialize method is triggered.
    /// </summary>
    internal class PortalDiagnosticsQueueSender : IDiagnosticsSender
    {
        public PortalDiagnosticsQueueSender()
        {
            this.EventData = new List<TraceEvent>();
            this.IsDisabled = false;
        }

        public IList<TraceEvent> EventData { get; }

        public bool IsDisabled { get; set; }

        public void Send(TraceEvent eventData)
        {
            if (!this.IsDisabled)
            {
                this.EventData.Add(eventData);
            }
        }

        public void FlushQueue(IDiagnosticsSender sender)
        {
            foreach (var traceEvent in this.EventData)
            {
                sender.Send(traceEvent);
            }
        }
    }
}
