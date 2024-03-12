namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.Helpers;

    /// <summary>
    /// Top CPU collector.
    /// </summary>
    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "Ignore Warning. This is a stub class for NetCore")]
    internal sealed class QuickPulseTopCpuCollector : IQuickPulseTopCpuCollector
    {
        private static readonly Tuple<string, int>[] emptyResult = Array.Empty<Tuple<string, int>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="QuickPulseTopCpuCollector"/> class. 
        /// </summary>
        /// <param name="timeProvider">Time provider.</param>
        /// <param name="processProvider">Process provider.</param>
        public QuickPulseTopCpuCollector(Clock timeProvider, IQuickPulseProcessProvider processProvider)
        {
        }

        /// <summary>
        /// Gets a value indicating whether the initialization has failed.
        /// </summary>
        public bool InitializationFailed { get; } = false;

        /// <summary>
        /// Gets a value indicating whether the Access Denied error has taken place.
        /// </summary>
        public bool AccessDenied { get; } = false;

        /// <summary>
        /// Gets top N processes by CPU consumption.
        /// </summary>
        /// <param name="topN">Top N processes.</param>
        /// <returns>List of top processes by CPU consumption.</returns>
        public IEnumerable<Tuple<string, int>> GetTopProcessesByCpu(int topN)
        {
            return QuickPulseTopCpuCollector.emptyResult;
        }

        /// <summary>
        /// Initializes the top CPU collector.
        /// </summary>
        public void Initialize()
        {
        }

        /// <summary>
        /// Closes the top CPU collector.
        /// </summary>
        public void Close()
        {
        }
    }
}