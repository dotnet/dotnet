namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation
{
    using System;
    using System.Collections.Generic;

    internal interface IPerformanceCollector
    {
        /// <summary>
        /// Gets a collection of counters that are currently registered with the collector.
        /// </summary>
        IEnumerable<PerformanceCounterData> PerformanceCounters { get; }

        /// <summary>
        /// Performs collection for all registered counters.
        /// </summary>
        /// <param name="onReadingFailure">Invoked when an individual counter fails to be read.</param>
        IEnumerable<Tuple<PerformanceCounterData, double>> Collect(Action<string, Exception> onReadingFailure = null);

        /// <summary>
        /// Refreshes and rebinds all the set of counters that are intended to be collected.
        /// </summary>
        void RefreshCounters();

        /// <summary>
        /// Registers a counter using the counter name and reportAs value to the total list of counters.
        /// </summary>
        /// <param name="perfCounter">Name of the performance counter.</param>
        /// <param name="reportAs">Report as name for the performance counter.</param>        
        /// <param name="error">Captures the error logged.</param>
        /// <param name="blockCounterWithInstancePlaceHolder">Boolean that controls the registry of the counter based on the availability of instance place holder.</param>
        void RegisterCounter(string perfCounter, string reportAs, out string error, bool blockCounterWithInstancePlaceHolder);

        /// <summary>
        /// Removes a counter.
        /// </summary>
        /// <param name="perfCounter">Name of the performance counter to remove.</param>
        /// <param name="reportAs">ReportAs value of the counter to remove.</param>
        void RemoveCounter(string perfCounter, string reportAs);
    }
}
