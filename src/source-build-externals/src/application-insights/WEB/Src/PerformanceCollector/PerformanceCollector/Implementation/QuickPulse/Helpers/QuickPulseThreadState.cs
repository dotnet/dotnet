namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.Helpers
{
    /// <summary>
    /// Represents the state of a thread.
    /// </summary>
    internal class QuickPulseThreadState
    {
        /// <summary>
        /// Indicates if thread has been requested to abort.
        /// </summary>
        public volatile bool IsStopRequested = false;
    }
}