namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.Helpers;

    internal class QuickPulseCollectionTimeSlotManagerMock : QuickPulseCollectionTimeSlotManager
    {
        private readonly TimeSpan interval;

        public QuickPulseCollectionTimeSlotManagerMock(QuickPulseTimings timings)
        {
            this.interval = timings.CollectionInterval;
        }

        public override DateTimeOffset GetNextCollectionTimeSlot(DateTimeOffset currentTime)
        {
            return currentTime + this.interval;
        }
    }
}