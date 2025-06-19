namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse
{
    using System;

    /// <summary>
    /// Top CPU collector.
    /// </summary>
    internal sealed class QuickPulseProcess
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuickPulseProcess"/> class. 
        /// </summary>
        /// <param name="processName">Process name.</param>
        /// <param name="totalProcessorTime">Total processor time.</param>
        public QuickPulseProcess(string processName, TimeSpan totalProcessorTime)
        {
            this.ProcessName = processName;
            this.TotalProcessorTime = totalProcessorTime;
        }
        
        /// <summary>
        /// Gets the process name.
        /// </summary>
        public string ProcessName { get; }

        /// <summary>
        /// Gets the total processor time.
        /// </summary>
        public TimeSpan TotalProcessorTime { get; }
    }
}