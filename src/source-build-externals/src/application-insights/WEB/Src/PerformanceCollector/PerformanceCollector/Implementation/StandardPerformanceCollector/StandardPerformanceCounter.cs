namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.StandardPerfCollector
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    /// <summary>
    /// Interface represents the counter value.
    /// </summary>
    internal class StandardPerformanceCounter : ICounterValue, IDisposable
    {
        private PerformanceCounter performanceCounter = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardPerformanceCounter" /> class.
        /// </summary>
        /// <param name="categoryName">The counter category name.</param>
        /// <param name="counterName">The counter name.</param>
        /// <param name="instanceName">The instance name.</param>
        internal StandardPerformanceCounter(string categoryName, string counterName, string instanceName)
        {
            this.performanceCounter = new PerformanceCounter(categoryName, counterName, instanceName, true);
        }

        /// <summary>
        /// Returns the current value of the counter as a <c ref="MetricTelemetry"/>.
        /// </summary>
        /// <returns>Value of the counter.</returns>
        public double Collect()
        {
            try
            {
                return this.performanceCounter.NextValue();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        "Failed to perform a read for performance counter {0}",
                        PerformanceCounterUtility.FormatPerformanceCounter(this.performanceCounter)),
                    e);
            }
        }

        /// <summary>
        /// Disposes resources allocated by this type.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose implementation.
        /// </summary>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.performanceCounter != null)
                {
                    this.performanceCounter.Dispose();
                    this.performanceCounter = null;
                }
            }
        }
    }
}
