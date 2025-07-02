namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.Helpers
{
        using System;

        /// <summary>
        /// Enum for all the performance counters collected for quick pulse.
        /// </summary>
        [Flags]
        internal enum QuickPulseCounter
        {
            /// <summary>
            /// Committed bytes counter.
            /// </summary>
            Bytes = 0,

            /// <summary>
            /// Processor time counter.
            /// </summary>
            ProcessorTime = 1,
        }
}
