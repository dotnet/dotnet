namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Threading;

    /// <summary>
    /// Exponential moving average counter.
    /// </summary>
    internal class ExponentialMovingAverageCounter
    {
        /// <summary>
        /// Average value of the counter.
        /// </summary>
        private double? average;

        /// <summary>
        /// Value of the counter during current interval of time.
        /// </summary>
        private long current;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExponentialMovingAverageCounter"/> class.
        /// </summary>
        /// <param name="coefficient">Exponential coefficient.</param>
        public ExponentialMovingAverageCounter(double coefficient)
        {
            this.Coefficient = coefficient;
        }

        /// <summary>
        /// Gets exponential coefficient (must be between 0 and 1).
        /// </summary>
        public double Coefficient { get; private set; }

        /// <summary>
        /// Gets exponential moving average value of the counter.
        /// </summary>
        public double Average
        {
            get
            {
                return this.average ?? this.current;
            }
        }

        /// <summary>
        /// Increments counter value.
        /// </summary>
        /// <returns>Incremented value.</returns>
        public long Increment()
        {
            return Interlocked.Increment(ref this.current);
        }

        /// <summary>
        /// Zeros out current value and starts new 'counter interval'.
        /// </summary>
        public double StartNewInterval()
        {
            var count = Interlocked.Exchange(ref this.current, 0);

            this.average = this.average.HasValue
                               ? (this.Coefficient * count) + ((1 - this.Coefficient) * this.average)
                               : count;

            return this.average.Value;
        }
    }
}
