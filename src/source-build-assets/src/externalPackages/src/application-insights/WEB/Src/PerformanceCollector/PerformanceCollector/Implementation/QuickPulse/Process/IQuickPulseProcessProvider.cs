namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provider interface for Windows processes.
    /// </summary>
    internal interface IQuickPulseProcessProvider
    {
        /// <summary>
        /// Initializes the process provider.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Closes the process provider.
        /// </summary>
        void Close();

        /// <summary>
        /// Gets a collection of <see cref="QuickPulseProcess"/> objects - each corresponding to a system process and containing
        /// information about the amount of time the process has occupied CPU cores.
        /// </summary>
        /// <param name="totalTime">If available, contains the value of the _Total instance of the counter, which indicates the overall
        /// amount of time spent by CPU cores executing system processes.</param>
        IEnumerable<QuickPulseProcess> GetProcesses(out TimeSpan? totalTime);
    }
}