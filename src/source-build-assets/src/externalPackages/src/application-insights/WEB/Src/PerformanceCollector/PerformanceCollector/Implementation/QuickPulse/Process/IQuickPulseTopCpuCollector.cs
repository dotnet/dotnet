namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Interface for top CPU collector.
    /// </summary>
    internal interface IQuickPulseTopCpuCollector
    {
        /// <summary>
        /// Gets a value indicating whether the initialization has failed.
        /// </summary>
        bool InitializationFailed { get; }

        /// <summary>
        /// Gets a value indicating whether the Access Denied error has taken place.
        /// </summary>
        bool AccessDenied { get; }

        /// <summary>
        /// Gets top N processes by CPU consumption.
        /// </summary>
        /// <param name="topN">Top N processes.</param>
        /// <returns>List of top processes by CPU consumption.</returns>
        IEnumerable<Tuple<string, int>> GetTopProcessesByCpu(int topN);

        /// <summary>
        /// Initializes the top CPU collector.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Closes the top CPU collector.
        /// </summary>
        void Close();
    }
}