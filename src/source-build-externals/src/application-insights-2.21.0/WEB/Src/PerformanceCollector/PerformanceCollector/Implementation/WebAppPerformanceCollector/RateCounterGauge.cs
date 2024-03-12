namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.WebAppPerfCollector
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Struct for metrics dependant on time.
    /// </summary>
    internal class RateCounterGauge : ICounterValue
    {
        /// <summary>
        /// Name of the counter.
        /// </summary>
        private string name;

        /// <summary>
        /// JSON identifier of the counter variable.
        /// </summary>
        private string jsonId;

        /// <summary>
        /// Identifier of the environment variable.
        /// </summary>
        private AzureWebApEnvironmentVariables environmentVariable;

        private ICounterValue counter;

        /// <summary>
        /// To keep track of the value read last time this metric was retrieved.
        /// </summary>
        private double lastValue;

        private ICachedEnvironmentVariableAccess cacheHelper;

        /// <summary>
        /// DateTime object to keep track of the last time this metric was retrieved.
        /// </summary>
        private DateTimeOffset lastCollectedTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="RateCounterGauge"/> class.
        /// </summary>
        /// <param name="name"> Name of counter variable.</param>
        /// <param name="jsonId">JSON identifier of the counter variable.</param>
        /// <param name="environmentVariable"> Identifier of the corresponding environment variable.</param>
        /// <param name="counter">Dependant counter.</param>
        public RateCounterGauge(string name, string jsonId, AzureWebApEnvironmentVariables environmentVariable, ICounterValue counter = null)
            : this(name, jsonId, environmentVariable, counter, CacheHelper.Instance)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RateCounterGauge"/> class. 
        /// This constructor is intended for Unit Tests.
        /// </summary>
        /// <param name="name"> Name of the counter variable.</param>
        /// <param name="jsonId">JSON identifier of the counter variable.</param>
        /// <param name="environmentVariable"> Identifier of the corresponding environment variable.</param>
        /// <param name="counter">Dependant counter.</param>
        /// <param name="cache"> Cache object.</param>
        internal RateCounterGauge(string name, string jsonId, AzureWebApEnvironmentVariables environmentVariable, ICounterValue counter, ICachedEnvironmentVariableAccess cache)
        {
            this.name = name;
            this.jsonId = jsonId;
            this.environmentVariable = environmentVariable;
            this.counter = counter;
            this.cacheHelper = cache;
        }

        /// <summary>
        /// Computes the rate of a specific counter by tracking the last collected time and value.
        /// </summary>
        /// <returns>The value of the target metric.</returns>
        public double Collect()
        {
            double previouslyCollectedValue = this.lastValue;
            this.lastValue = (this.counter == null) ? this.cacheHelper.GetCounterValue(this.jsonId, this.environmentVariable) : this.counter.Collect();

            var previouslyCollectedTime = this.lastCollectedTime;
            this.lastCollectedTime = DateTimeOffset.UtcNow;

            double value = 0;
            if (previouslyCollectedTime != DateTimeOffset.MinValue)
            {
                var timeDifferenceInSeconds = this.lastCollectedTime.Subtract(previouslyCollectedTime).Seconds;                

                var diff = this.lastValue - previouslyCollectedValue;

                if (diff < 0)
                {
                    PerformanceCollectorEventSource.Log.WebAppCounterNegativeValue(
                    this.lastValue,
                    previouslyCollectedValue,
                    this.name);
                }
                else
                {
                    value = timeDifferenceInSeconds != 0 ? (double)(diff / timeDifferenceInSeconds) : 0;
                }
            }

            return value;
        }
    }
}
