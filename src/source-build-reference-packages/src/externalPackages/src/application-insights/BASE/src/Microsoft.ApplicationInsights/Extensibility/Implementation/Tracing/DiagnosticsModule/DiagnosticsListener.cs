namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;

    /// <summary>
    /// Subscriber to ETW Event source events, which sends data to other Senders (F5 and Portal).
    /// </summary>
    internal class DiagnosticsListener : IDisposable
    {
        private readonly IList<IDiagnosticsSender> diagnosticsSenders = new List<IDiagnosticsSender>();
        private EventLevel logLevel = EventLevel.Error;
        private DiagnosticsEventListener eventListener;

        public DiagnosticsListener(IList<IDiagnosticsSender> senders)
        {
            if (senders == null || senders.Count < 1)
            {
                throw new ArgumentNullException(nameof(senders));
            }

            this.diagnosticsSenders = senders;
            this.eventListener = new DiagnosticsEventListener(this, this.LogLevel);
        }

        public EventLevel LogLevel
        {
            get => this.logLevel;

            set
            {
                if (this.LogLevel != value)
                {
                    var oldListener = this.eventListener;
                    this.eventListener = new DiagnosticsEventListener(this, value);
                    oldListener.Dispose();
                }

                this.logLevel = value;
            }
        }

        /// <summary>
        /// Sets LogLevel. Possible values LogAlways, Critical, Error, Warning, Informational and Verbose.
        /// </summary>
        public void SetLogLevel(string value)
        {
            // Once logLevel is set from configuration, restart listener with new value
            if (!string.IsNullOrEmpty(value))
            {
                EventLevel parsedValue;
                if (Enum.IsDefined(typeof(EventLevel), value) == true)
                {
                    parsedValue = (EventLevel)Enum.Parse(typeof(EventLevel), value, true);
                    this.LogLevel = parsedValue;
                }
            }
        }

        public void WriteEvent(TraceEvent eventData)
        {
            if (eventData.MetaData != null && eventData.MetaData.MessageFormat != null)
            {
                // check severity because it is not done in Silverlight EventSource implementation 
                if (eventData.MetaData.Level <= this.LogLevel)
                {
                    foreach (var sender in this.diagnosticsSenders)
                    {
                        sender.Send(eventData);
                    }
                }
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.eventListener.Dispose();
            }
        }
    }
}
