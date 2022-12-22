namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse
{
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.PerfLib;

    /// <summary>
    /// Interface for the Performance library.
    /// </summary>
    internal interface IQuickPulsePerfLib
    {
        /// <summary>
        /// Gets the category sample.
        /// </summary>
        /// <param name="categoryIndex">Category index.</param>
        /// <param name="counterIndex">Counter index.</param>
        /// <returns>The category sample.</returns>
        CategorySample GetCategorySample(int categoryIndex, int counterIndex);

        /// <summary>
        /// Initializes the performance library.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Closes the performance library.
        /// </summary>
        void Close();
    }
}