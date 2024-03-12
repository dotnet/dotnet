namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.XPlatform
{
    using System;

    /// <summary>
    /// Factory to create different counters.
    /// </summary>
    internal static class CounterFactoryXPlatform
    {
        /// <summary>
        /// Gets a counter.
        /// </summary>
        /// <param name="counterName">Counter name.</param>
        /// <returns>The counter identified by counter name.</returns>
        internal static ICounterValue GetCounter(string counterName)
        {
            switch (counterName)
            {
                case @"\Process(??APP_WIN32_PROC??)\% Processor Time Normalized":
                    return new XPlatProcessCPUPerformanceCounterNormalized();
                case @"\Process(??APP_WIN32_PROC??)\% Processor Time":
                    return new XPlatProcessCPUPerformanceCounter();
                case @"\Process(??APP_WIN32_PROC??)\Private Bytes":
                    return new XPlatProcessMemoryPerformanceCounter();
                default:
                    throw new ArgumentException("Performance counter not supported in XPlatform.", counterName);
            }
        }
    }
}