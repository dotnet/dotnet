namespace Microsoft.ApplicationInsights.Web.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    
    public class SimpleTelemetryProcessorSpy : ITelemetryProcessor
    {
        private readonly object lockObject = new object();

        private readonly List<Channel.ITelemetry> receivedItems = new List<ITelemetry>();

        public SimpleTelemetryProcessorSpy()
        {
        }

        public int ReceivedCalls
        {
            get
            {
                lock (this.lockObject)
                {
                    return this.receivedItems.Count;
                }
            }
        }

        public List<Channel.ITelemetry> ReceivedItems
        {
            get
            {
                return this.receivedItems;
            }
        } 

        public void Process(Channel.ITelemetry item)
        {
            lock (this.lockObject)
            {
                this.receivedItems.Add(item);
            }
        }
    }
}
