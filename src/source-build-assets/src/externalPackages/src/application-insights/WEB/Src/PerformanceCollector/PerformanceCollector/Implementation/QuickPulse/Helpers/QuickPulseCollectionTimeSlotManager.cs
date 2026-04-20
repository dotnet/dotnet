namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.Helpers
{
    using System;

    internal class QuickPulseCollectionTimeSlotManager
    {
        public virtual DateTimeOffset GetNextCollectionTimeSlot(DateTimeOffset currentTime)
        {
            return currentTime.Millisecond < 500 ? currentTime.AddMilliseconds(500 - currentTime.Millisecond) : currentTime.AddSeconds(1).AddMilliseconds(500 - currentTime.Millisecond);
        }
    }
}