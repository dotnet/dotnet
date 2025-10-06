namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.PerfLib;

    /// <summary>
    /// Top CPU process provider.
    /// </summary>
    internal sealed class QuickPulseProcessProvider : IQuickPulseProcessProvider
    {
        private readonly IQuickPulsePerfLib perfLib;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuickPulseProcessProvider"/> class. 
        /// </summary>
        /// <param name="perfLib">Performance library.</param>
        public QuickPulseProcessProvider(IQuickPulsePerfLib perfLib)
        {
            this.perfLib = perfLib;
        }

        /// <summary>
        /// Initializes the process provider.
        /// </summary>
        public void Initialize()
        {
            this.perfLib.Initialize();
        }

        /// <summary>
        /// Closes the process provider.
        /// </summary>
        public void Close()
        {
            this.perfLib.Close();
        }

        /// <summary>
        /// Gets a collection of <see cref="QuickPulseProcess"/> objects - each corresponding to a system process and containing
        /// information about the amount of time the process has occupied CPU cores.
        /// </summary>
        /// <param name="totalTime">If available, contains the value of the _Total instance of the counter, which indicates the overall
        /// amount of time spent by CPU cores executing system processes.</param>
        public IEnumerable<QuickPulseProcess> GetProcesses(out TimeSpan? totalTime)
        {
            // "Process" object
            const int CategoryIndex = 230;

            // "% Processor Time" counter
            const int CounterIndex = 6;

            const string TotalInstanceName = "_Total";
            const string IdleInstanceName = "Idle";

            CategorySample categorySample = this.perfLib.GetCategorySample(CategoryIndex, CounterIndex);
            CounterDefinitionSample counterSample = categorySample.CounterTable[CounterIndex];

            var procValues = new Dictionary<string, long>();
            foreach (var pair in categorySample.InstanceNameTable)
            {
                string instanceName = pair.Key;
                int valueIndex = pair.Value;

                long instanceValue = counterSample.GetInstanceValue(valueIndex);

                procValues.Add(instanceName, instanceValue);
            }

            long overallTime;
            totalTime = procValues.TryGetValue(TotalInstanceName, out overallTime) ? TimeSpan.FromTicks(overallTime) : (TimeSpan?)null;

            return
                procValues.Where(
                    pv =>
                    !string.Equals(pv.Key, TotalInstanceName, StringComparison.Ordinal)
                    && !string.Equals(pv.Key, IdleInstanceName, StringComparison.Ordinal))
                    .Select(pv => new QuickPulseProcess(pv.Key, TimeSpan.FromTicks(pv.Value)));
        }
    }
}