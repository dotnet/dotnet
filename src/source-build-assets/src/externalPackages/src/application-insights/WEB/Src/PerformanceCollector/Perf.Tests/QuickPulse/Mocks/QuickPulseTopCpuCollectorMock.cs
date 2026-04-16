namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Generic;

    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse;

    internal class QuickPulseTopCpuCollectorMock : IQuickPulseTopCpuCollector
    {
        public List<Tuple<string, int>> TopProcesses { get; set; } = new List<Tuple<string, int>>();

        public bool InitializationFailed { get; }

        public bool AccessDenied { get; }

        public IEnumerable<Tuple<string, int>> GetTopProcessesByCpu(int topN)
        {
            return this.TopProcesses;
        }

        public void Initialize()
        {
        }

        public void Close()
        {
        }
    }
}