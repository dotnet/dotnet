namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.WebAppPerfCollector
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Gauge that computes normalized CPU percentage utilized by a process by utilizing the last computed time (divided by the processors count).
    /// </summary>
    internal class NormalizedCPUPercentageGauge : CPUPercenageGauge
    {
        private readonly bool isInitialized = false;
        private readonly int processorsCount = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="NormalizedCPUPercentageGauge"/> class.
        /// </summary>
        /// <param name="name"> Name of the SumUpCountersGauge.</param>
        /// <param name="value"> Gauges to sum.</param>
        public NormalizedCPUPercentageGauge(string name, ICounterValue value) : base(name, value)
        {
            int? count = PerformanceCounterUtility.GetProcessorCount();

            if (count.HasValue)
            {
                this.processorsCount = count.Value;
                this.isInitialized = true;
            }
        }
        
        /// <summary>
        /// Returns the normalized percentage of the CPU process utilization time divided by the number of processors with respect to the total duration.
        /// </summary>
        /// <returns>The value of the target metric.</returns>
        protected override double CollectPercentage()
        {
            if (!this.isInitialized)
            {
                return 0;
            }

            double result = 0;
            if (this.processorsCount >= 1)
            {
                double value = base.CollectPercentage();
                result = value / this.processorsCount;
            }

            return result;
        }
    }
}
