namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.XPlatform
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    /// <summary>
    /// Represents value of CPU Utilization by Process counter value.
    /// </summary>
    internal class XPlatProcessCPUPerformanceCounter : ICounterValue
    {
        private double lastCollectedValue = 0;
        private DateTimeOffset lastCollectedTime = DateTimeOffset.MinValue;

        /// <summary>
        ///  Initializes a new instance of the <see cref="XPlatProcessCPUPerformanceCounter" /> class.
        /// </summary>
        internal XPlatProcessCPUPerformanceCounter()
        {
            this.lastCollectedValue = Process.GetCurrentProcess().TotalProcessorTime.Ticks;
            this.lastCollectedTime = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// Returns the current value of the counter.
        /// </summary>
        /// <returns>Value of the counter.</returns>
        public virtual double Collect()
        {
            try
            {
                double previouslyCollectedValue = this.lastCollectedValue;
                this.lastCollectedValue = Process.GetCurrentProcess().TotalProcessorTime.Ticks;

                var previouslyCollectedTime = this.lastCollectedTime;
                this.lastCollectedTime = DateTimeOffset.UtcNow;

                double value = 0;
                if (previouslyCollectedTime != DateTimeOffset.MinValue)
                {
                    var baseValue = this.lastCollectedTime.Ticks - previouslyCollectedTime.Ticks;
                    if (baseValue < 0)
                    {
                        // Not likely to happen but being safe here incase of clock issues in multi-core.
                        PerformanceCollectorEventSource.Log.WebAppCounterNegativeValue(
                            this.lastCollectedTime.Ticks,
                            previouslyCollectedTime.Ticks,
                            "XPlatProcessCPUPerformanceCounter.BaseValue");
                        return 0;
                    }

                    baseValue = baseValue != 0 ? baseValue : 1;

                    var diff = this.lastCollectedValue - previouslyCollectedValue;

                    if (diff >= 0)                    
                    {
                        value = (double)(diff * 100.0 / baseValue);
                    }
                }

                return value;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        "Failed to perform a read for performance counter NormalizedXPlatProcessCPUPerformanceCounter. Exception: {0}", e));
            }
        }
    }
}
