namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.XPlatform
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    /// <summary>
    /// Represents normalized value of CPU Utilization by Process counter value (divided by the processors count).
    /// </summary>
    internal class XPlatProcessCPUPerformanceCounterNormalized : XPlatProcessCPUPerformanceCounter
    {        
        private readonly bool isInitialized = false;
        private readonly int processorsCount = -1;

        /// <summary>
        ///  Initializes a new instance of the <see cref="XPlatProcessCPUPerformanceCounterNormalized" /> class.
        /// </summary>
        internal XPlatProcessCPUPerformanceCounterNormalized() : base()
        {
            int? count = PerformanceCounterUtility.GetProcessorCount();

            if (count.HasValue)
            {
                this.processorsCount = count.Value;
                this.isInitialized = true;
            }
        }

        /// <summary>
        /// Returns the current value of the counter as a <c ref="MetricTelemetry"/>.
        /// </summary>
        /// <returns>Value of the counter.</returns>
        public override double Collect()
        {
            if (!this.isInitialized)
            {
                return 0;
            }

            double result = 0;
            if (this.processorsCount >= 1)
            {
                double value = base.Collect();
                result = value / this.processorsCount;
            }

            return result;
        }
    }
}
