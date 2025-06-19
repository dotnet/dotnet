namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.Helpers
{
    using System;

    internal interface IQuickPulseModuleSchedulerHandle : IDisposable
    {
        void Stop(bool wait);
    }
}
