namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.Helpers;

    internal class ClockMock : Clock
    {
        private readonly object lockObj = new object();

        private DateTimeOffset now = DateTimeOffset.UtcNow;

        public override DateTimeOffset UtcNow
        {
            get
            {
                lock (this.lockObj)
                {
                    return this.now;
                }
            }
        }

        public void FastForward(TimeSpan span)
        {
            lock (this.lockObj)
            {
                this.now += span;
            }
        }
    }
}