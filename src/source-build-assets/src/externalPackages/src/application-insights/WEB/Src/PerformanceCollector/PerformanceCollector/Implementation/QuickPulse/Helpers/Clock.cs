namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.Helpers
{
    using System;

    internal class Clock
    {
        public virtual DateTimeOffset UtcNow
        {
            get
            {
                return DateTimeOffset.UtcNow;
            }
        }
    }
}