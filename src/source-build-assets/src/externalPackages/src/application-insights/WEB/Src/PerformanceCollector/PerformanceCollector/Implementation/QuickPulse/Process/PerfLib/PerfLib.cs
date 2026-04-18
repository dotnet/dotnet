namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.PerfLib
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Represents a library that works with performance data through a low-level pseudo-registry interface.
    /// </summary>
    internal class PerfLib : IQuickPulsePerfLib
    {
        private static PerfLib library = null;

        private PerformanceMonitor performanceMonitor;

        private PerfLib()
        {
        }

        /// <summary>
        /// Gets the performance library instance.
        /// </summary>
        /// <returns>The performance library instance.</returns>
        public static PerfLib GetPerfLib()
        {
            library = library ?? new PerfLib();

            return library;
        }

        /// <summary>
        /// Gets the category sample.
        /// </summary>
        /// <param name="categoryIndex">Index of the category.</param>
        /// <param name="counterIndex">Index of the counter.</param>
        /// <returns>The category sample.</returns>
        public CategorySample GetCategorySample(int categoryIndex, int counterIndex)
        {
            byte[] dataRef = this.GetPerformanceData(categoryIndex.ToString(CultureInfo.InvariantCulture));
            if (dataRef == null)
            {
                throw new InvalidOperationException("Could not read data for category index " + categoryIndex.ToString(CultureInfo.InvariantCulture));
            }

            return new CategorySample(dataRef, categoryIndex, counterIndex, this);
        }

        /// <summary>
        /// Initializes the library.
        /// </summary>
        public void Initialize()
        {
            this.performanceMonitor = new PerformanceMonitor();
        }

        /// <summary>
        /// Closes the library.
        /// </summary>
        public void Close()
        {
            this.performanceMonitor?.Close();

            library = null;
        }

        /// <summary>
        /// Gets performance data for the given category index.
        /// </summary>
        /// <param name="categoryIndex">Index of the category.</param>
        /// <returns>Performance data.</returns>
        public byte[] GetPerformanceData(string categoryIndex)
        {
            return this.performanceMonitor.GetData(categoryIndex);
        }
    }
}
