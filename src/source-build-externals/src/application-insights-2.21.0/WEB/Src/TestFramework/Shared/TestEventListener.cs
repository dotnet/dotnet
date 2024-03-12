namespace Microsoft.ApplicationInsights.Web.TestFramework
{
    using System;
    using System.Collections.Generic;

    using System.Diagnostics.Tracing;

    internal class TestEventListener : EventListener
    {
        private readonly IList<EventWrittenEventArgs> events;
        
        public TestEventListener()
        {
            this.events = new List<EventWrittenEventArgs>();
            this.OnOnEventWritten = e =>
            {
                this.events.Add(e);
            };
        }

        public Action<EventSource> OnOnEventSourceCreated { get; set; }

        public Action<EventWrittenEventArgs> OnOnEventWritten { get; set; }

        public IList<EventWrittenEventArgs> Messages
        {
            get
            {
                return this.events;               
            }
        }
        
        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            this.OnOnEventWritten(eventData);
        }

        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            // Check for null because this method is called by the base class constror before we can initialize it
            Action<EventSource> callback = this.OnOnEventSourceCreated;
            if (callback != null)
            {
                callback(eventSource);
            }
        }
    }
}
