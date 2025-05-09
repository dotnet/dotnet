namespace Microsoft.ApplicationInsights.Channel
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    
    internal class EventCounterListener : EventListener
    {
        private const long AllKeywords = -1;
        public ConcurrentQueue<EventWrittenEventArgs> EventsReceived { get; private set; }

        public EventCounterListener()
        {
            this.EventsReceived = new ConcurrentQueue<EventWrittenEventArgs>();
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            this.EventsReceived.Enqueue(eventData);
        }

        protected override void OnEventSourceCreated(EventSource eventSource)
        {
#if REDFIELD
            if (string.Equals(eventSource.Name, "Redfield-Microsoft-ApplicationInsights-Core", StringComparison.Ordinal))
#else
            if (string.Equals(eventSource.Name, "Microsoft-ApplicationInsights-Core", StringComparison.Ordinal))
#endif
            {
                var eventCounterArguments = new Dictionary<string, string>
                {
                    {"EventCounterIntervalSec", "1"}
                };
                EnableEvents(eventSource, EventLevel.LogAlways, (EventKeywords)AllKeywords, eventCounterArguments);
            }
        }

    }
}
