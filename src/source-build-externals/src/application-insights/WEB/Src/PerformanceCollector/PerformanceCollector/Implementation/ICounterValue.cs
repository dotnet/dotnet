namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation
{
    using Microsoft.ApplicationInsights.DataContracts;

    /// <summary>
    /// Interface represents the counter value.
    /// </summary>
    internal interface ICounterValue
    {
        /// <summary>
        /// Returns the current value of the counter.
        /// </summary>
        /// <returns>Value of the counter.</returns>
        double Collect();
    }
}
