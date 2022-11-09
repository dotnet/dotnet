namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Generic;

    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse;

    internal class QuickPulseProcessProviderMock : IQuickPulseProcessProvider
    {
        public List<QuickPulseProcess> Processes { get; set; }

        public Exception AlwaysThrow { get; set; } = null;

        public TimeSpan? OverallTimeValue { get; set; } = null;

        public void Initialize()
        {
            if (this.AlwaysThrow != null)
            {
                throw this.AlwaysThrow;
            }
        }

        public void Close()
        {
            if (this.AlwaysThrow != null)
            {
                throw this.AlwaysThrow;
            }
        }

        public IEnumerable<QuickPulseProcess> GetProcesses(out TimeSpan? totalTime)
        {
            totalTime = this.OverallTimeValue;

            if (this.AlwaysThrow != null)
            {
                throw this.AlwaysThrow;
            }

            return this.Processes;
        }
    }
}