using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;

namespace EventCounterCollector.Tests
{
    [EventSource(Name = "Microsoft-ApplicationInsights-Extensibility-EventCounterCollector.Tests.TestEventCounter")]
    public sealed class TestEventCounter : EventSource
    {
        // define the singleton instance of the event source
        public static TestEventCounter Log = new TestEventCounter();
        private EventCounter testCounter1;
        private EventCounter testCounter2;

        private TestEventCounter()
        {
            this.testCounter1 = new EventCounter("mycountername1", this);
            this.testCounter2 = new EventCounter("mycountername2", this);
        }

        public void SampleCounter1(float counterValue)
        {
            this.testCounter1.WriteMetric(counterValue);        
        }

        public void SampleCounter2(float counterValue)
        {
            this.testCounter2.WriteMetric(counterValue);
        }
    }
}
