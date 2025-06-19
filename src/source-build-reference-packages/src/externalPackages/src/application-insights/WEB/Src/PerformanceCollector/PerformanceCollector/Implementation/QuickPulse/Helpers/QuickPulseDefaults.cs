namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.Helpers
{
    using System;
    using System.Collections.Generic;

    internal static class QuickPulseDefaults
    {
        public static readonly Uri QuickPulseServiceEndpoint = new Uri("https://rt.services.visualstudio.com/QuickPulseService.svc");

        /// <summary>
        /// Dictionary of performance counters to collect for standard framework.
        /// </summary>
        private static readonly Dictionary<QuickPulseCounter, string> DefaultPerformanceCountersToCollect = new Dictionary<QuickPulseCounter, string>
        {
            [QuickPulseCounter.Bytes] = @"\Memory\Committed Bytes",
            [QuickPulseCounter.ProcessorTime] = @"\Processor(_Total)\% Processor Time",
        };

        /// <summary>
        /// Dictionary of performance counters to collect for WEB APP framework.
        /// </summary>
        private static readonly Dictionary<QuickPulseCounter, string> WebAppDefaultPerformanceCountersToCollect = new Dictionary<QuickPulseCounter, string>
        {
            [QuickPulseCounter.Bytes] = @"\Process(??APP_WIN32_PROC??)\Private Bytes",
            [QuickPulseCounter.ProcessorTime] = @"\Process(??APP_WIN32_PROC??)\% Processor Time",
        };

        /// <summary>
        /// Mapping between the counters collected in WEB APP to the counters collected in Standard Framework.
        /// </summary>
        private static readonly Dictionary<string, string> WebAppToStandardCounterMapping = new Dictionary<string, string>
        {
            [WebAppDefaultPerformanceCountersToCollect[QuickPulseCounter.Bytes]] = DefaultPerformanceCountersToCollect[QuickPulseCounter.Bytes],
            [WebAppDefaultPerformanceCountersToCollect[QuickPulseCounter.ProcessorTime]] = DefaultPerformanceCountersToCollect[QuickPulseCounter.ProcessorTime],
        };

        public static Dictionary<QuickPulseCounter, string> DefaultCountersToCollect
        {
            get
            {
                if (PerformanceCounterUtility.IsWebAppRunningInAzure())
                {
                    return WebAppDefaultPerformanceCountersToCollect;
                }
                else
                {
#if NETSTANDARD2_0
                    if (PerformanceCounterUtility.IsWindows)
                    {
                        return DefaultPerformanceCountersToCollect;
                    }
                    else
                    {
                        return WebAppDefaultPerformanceCountersToCollect;
                    }
#else
                    return DefaultPerformanceCountersToCollect;
#endif

                }
            }
        }

        public static Dictionary<string, string> DefaultCounterOriginalStringMapping => WebAppToStandardCounterMapping;
    }
}