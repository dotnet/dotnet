namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.WebAppPerfCollector
{
    using System;

    /// <summary>
    /// Gauge that computes the CPU percentage utilized by a process by utilizing the last computed time.
    /// </summary>
    internal class CPUPercenageGauge : ICounterValue
    {
        /// <summary>
        /// Name of the counter.
        /// </summary>
        private string name;

        private double lastCollectedValue;

        private DateTimeOffset lastCollectedTime;

        private ICounterValue valueProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CPUPercenageGauge"/> class.
        /// </summary>
        /// <param name="name"> Name of the SumUpCountersGauge.</param>
        /// <param name="value"> Gauges to sum.</param>
        public CPUPercenageGauge(string name, ICounterValue value)
        {
            this.name = name;
            this.valueProvider = value;
        }

        /// <summary>
        /// Returns the percentage of the CPU process utilization time with respect to the total duration.
        /// </summary>
        /// <returns>The value of the target metric.</returns>
        public double Collect()
        {
            return this.CollectPercentage();
        }

        /// <summary>
        /// Returns the percentage of the CPU process utilization time with respect to the total duration.
        /// </summary>
        /// <returns>The value of the target metric.</returns>
        protected virtual double CollectPercentage()
        {
            double previouslyCollectedValue = this.lastCollectedValue;
            this.lastCollectedValue = this.valueProvider.Collect();

            var previouslyCollectedTime = this.lastCollectedTime;
            this.lastCollectedTime = DateTimeOffset.UtcNow;

            double value = 0;
            if (previouslyCollectedTime != DateTimeOffset.MinValue)
            {
                var baseValue = this.lastCollectedTime.Ticks - previouslyCollectedTime.Ticks;
                baseValue = baseValue != 0 ? baseValue : 1;

                var diff = this.lastCollectedValue - previouslyCollectedValue;

                if (diff < 0)
                {
                    PerformanceCollectorEventSource.Log.WebAppCounterNegativeValue(
                    this.lastCollectedValue,
                    previouslyCollectedValue,
                    this.name);
                }
                else
                {
                    value = (double)(diff * 100.0 / baseValue);
                }
            }

            return value;
        }
    }
}
