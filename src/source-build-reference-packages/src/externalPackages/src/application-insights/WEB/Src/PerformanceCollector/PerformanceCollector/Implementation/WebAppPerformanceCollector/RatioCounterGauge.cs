namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.WebAppPerfCollector
{
    /// <summary>
    /// Gauge that computes the ratio of two different gauges.
    /// </summary>
    internal class RatioCounterGauge : ICounterValue
    {
        /// <summary>
        /// The numerator gauge used to compute the target ratio.
        /// </summary>
        private readonly ICounterValue numeratorGauge;

       /// <summary>
        /// The denominator gauge used to compute the target ratio.
        /// </summary>
        private readonly ICounterValue denominatorGauge;

        /// <summary>
        /// Name of the counter.
        /// </summary>
        private string name;

        /// <summary>
        /// Scale to measure the percentage or increase the scaling of the ratio.
        /// </summary>
        private double scale;

        /// <summary>
        /// Initializes a new instance of the <see cref="RatioCounterGauge"/> class.
        /// </summary>
        /// <param name="name"> Name of the RatioCounterGauge.</param>
        /// <param name="numeratorGauge">The numerator for computing the ratio.</param>
        /// <param name="denominatorGauge">The denominator for computing the ratio.</param>
        /// <param name="scale">Scale to measure the percentage or increase the scaling of the ratio.</param>
        public RatioCounterGauge(string name, ICounterValue numeratorGauge, ICounterValue denominatorGauge, double scale = 1)
        {
            this.name = name;
            this.numeratorGauge = numeratorGauge;
            this.denominatorGauge = denominatorGauge;
            this.scale = scale;
        }

        /// <summary>
        /// Returns the current value of the sum of all different gauges attached to this one and resets their values.
        /// </summary>
        /// <returns>The value of the target metric.</returns>
        public double Collect()
        {
            if ((this.numeratorGauge != null) && (this.denominatorGauge != null))
            {
                double denominatorValue = this.denominatorGauge.Collect();
                return (denominatorValue == 0) ? 0 : (this.numeratorGauge.Collect() / denominatorValue) * this.scale;
            } 

            return 0;
        }
    }
}
