namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using System.Globalization;
    using System.Linq;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;

    internal class AzureSdkEventListener : EventListener
    {
#if NET452
        private static readonly object[] EmptyArray = new object[0];
#else
        private static readonly object[] EmptyArray = Array.Empty<object>();
#endif

        private readonly List<EventSource> eventSources = new List<EventSource>();
        private readonly TelemetryClient telemetryClient;
        private readonly EventLevel level;
        private readonly string prefix;

        public AzureSdkEventListener(TelemetryClient telemetryClient, EventLevel level, string prefix)
        {
            this.prefix = prefix ?? throw new ArgumentNullException(nameof(prefix));
            this.level = level;
            this.telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));

            foreach (EventSource eventSource in this.eventSources)
            {
                this.OnEventSourceCreated(eventSource);
            }

            this.eventSources.Clear();
        }

        protected sealed override void OnEventSourceCreated(EventSource eventSource)
        {
            base.OnEventSourceCreated(eventSource);

            if (this.telemetryClient == null)
            {
                this.eventSources.Add(eventSource);
                return;
            }

            // EventSource names are deduplicated for environments like
            // Functions where the same library can be loaded twice.
            // Two EventSources with the same name are not allowed.
            if (eventSource.Name != null && eventSource.Name.StartsWith(this.prefix, StringComparison.Ordinal))
            {
                this.EnableEvents(eventSource, this.level);
            }
        }
        
        protected sealed override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            // Workaround https://github.com/dotnet/corefx/issues/42600
            if (eventData.EventId == -1)
            {
                return;
            }

            var payloadArray = eventData.Payload?.ToArray() ?? EmptyArray;
            string message = string.Empty;
            if (eventData.Message != null)
            {
                try
                {
                    message = string.Format(CultureInfo.InvariantCulture, eventData.Message, payloadArray);
                }
                catch (FormatException)
                {
                }
            }
            else
            {
                message = String.Join(", ", payloadArray); 
            }

            var trace = new TraceTelemetry(message, FromEventLevel(eventData.Level));
            trace.Properties["CategoryName"] = eventData.EventSource.Name;

            if (eventData.EventId > 0)
            {
                trace.Properties["EventId"] = eventData.EventId.ToString(CultureInfo.InvariantCulture);
            }

#if !NET452
            if (!string.IsNullOrEmpty(eventData.EventName))
            {
                trace.Properties["EventName"] = eventData.EventName;
            }
#endif
            this.telemetryClient?.TrackTrace(trace);
        }

        private static SeverityLevel FromEventLevel(EventLevel level)
        {
            switch (level)
            {
                case EventLevel.Critical:
                    return SeverityLevel.Critical;
                case EventLevel.Error:
                    return SeverityLevel.Error;
                case EventLevel.Warning:
                    return SeverityLevel.Warning;
                case EventLevel.Informational:
                    return SeverityLevel.Information;
                case EventLevel.Verbose:
                default:
                    return SeverityLevel.Verbose;
            }
        }
    }
}
