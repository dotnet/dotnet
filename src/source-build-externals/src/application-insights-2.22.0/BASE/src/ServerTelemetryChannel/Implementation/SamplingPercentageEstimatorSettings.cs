namespace Microsoft.ApplicationInsights.WindowsServer.Channel.Implementation
{
    using System;

    /// <summary>
    /// Container for all the settings applicable to the process of dynamically estimating 
    /// application telemetry sampling percentage.
    /// </summary>
    public class SamplingPercentageEstimatorSettings
    {
        /// <summary>
        /// Set of default settings.
        /// </summary>
        private static SamplingPercentageEstimatorSettings @default = new SamplingPercentageEstimatorSettings();

        /// <summary>
        /// Initializes a new instance of the <see cref="SamplingPercentageEstimatorSettings"/> class.
        /// </summary>
        public SamplingPercentageEstimatorSettings()
        {
            // set default values
            this.MaxTelemetryItemsPerSecond = 5.0;
            this.InitialSamplingPercentage = 100.0;
            this.MinSamplingPercentage = 0.1;
            this.MaxSamplingPercentage = 100.0;
            this.EvaluationInterval = TimeSpan.FromSeconds(15);
            this.SamplingPercentageDecreaseTimeout = TimeSpan.FromMinutes(2);
            this.SamplingPercentageIncreaseTimeout = TimeSpan.FromMinutes(15);
            this.MovingAverageRatio = 0.25;
        }
        
        /// <summary>
        /// Gets or sets maximum rate of telemetry items per second
        /// dynamic sampling will try to adhere to.
        /// </summary>
        public double MaxTelemetryItemsPerSecond { get; set; }

        /// <summary>
        /// Gets or sets initial sampling percentage applied at the start
        /// of the process to dynamically vary the percentage.
        /// </summary>
        public double InitialSamplingPercentage { get; set; }

        /// <summary>
        /// Gets or sets minimum sampling percentage that can be set 
        /// by the dynamic sampling percentage algorithm.
        /// </summary>
        public double MinSamplingPercentage { get; set; }

        /// <summary>
        /// Gets or sets maximum sampling percentage that can be set 
        /// by the dynamic sampling percentage algorithm.
        /// </summary>
        public double MaxSamplingPercentage { get; set; }

        /// <summary>
        /// Gets or sets duration of the sampling percentage evaluation 
        /// interval in seconds.
        /// </summary>
        public TimeSpan EvaluationInterval { get; set; }

        /// <summary>
        /// Gets or sets a value indicating how long to not to decrease
        /// sampling percentage after last change to prevent excessive fluctuation.
        /// </summary>
        public TimeSpan SamplingPercentageDecreaseTimeout { get; set; }

        /// <summary>
        /// Gets or sets a value indicating how long to not to increase
        /// sampling percentage after last change to prevent excessive fluctuation.
        /// </summary>
        public TimeSpan SamplingPercentageIncreaseTimeout { get; set; }

        /// <summary>
        /// Gets or sets exponential moving average ratio (factor) applied
        /// during calculation of rate of telemetry items produced by the application.
        /// </summary>
        public double MovingAverageRatio { get; set; }

        /// <summary>
        /// Gets effective maximum telemetry items rate per second 
        /// adjusted in case user makes an error while setting a value.
        /// </summary>
        internal double EffectiveMaxTelemetryItemsPerSecond
        {
            get
            {
                return this.MaxTelemetryItemsPerSecond <= 0 ? 1E-12 : this.MaxTelemetryItemsPerSecond;
            }
        }

        /// <summary>
        /// Gets effective initial sampling rate
        /// adjusted in case user makes an error while setting a value.
        /// </summary>
        internal int EffectiveInitialSamplingRate
        {
            get
            {
                return (int)Math.Floor(100 / AdjustSamplingPercentage(this.InitialSamplingPercentage));
            }
        }

        /// <summary>
        /// Gets effective minimum sampling rate
        /// adjusted in case user makes an error while setting a value.
        /// </summary>
        internal int EffectiveMinSamplingRate
        {
            get
            {
                return (int)Math.Floor(100 / AdjustSamplingPercentage(this.MaxSamplingPercentage));
            }
        }

        /// <summary>
        /// Gets effective maximum sampling rate
        /// adjusted in case user makes an error while setting a value.
        /// </summary>
        internal int EffectiveMaxSamplingRate
        {
            get
            {
                return (int)Math.Ceiling(100 / AdjustSamplingPercentage(this.MinSamplingPercentage));
            }
        }

        /// <summary>
        /// Gets effective sampling percentage evaluation interval
        /// adjusted in case user makes an error while setting a value.
        /// </summary>
        internal TimeSpan EffectiveEvaluationInterval
        {
            get
            {
                return this.EvaluationInterval == TimeSpan.Zero
                    ? @default.EvaluationInterval
                    : this.EvaluationInterval;
            }
        }

        /// <summary>
        /// Gets effective sampling percentage decrease timeout
        /// adjusted in case user makes an error while setting a value.
        /// </summary>
        internal TimeSpan EffectiveSamplingPercentageDecreaseTimeout
        {
            get
            {
                return this.SamplingPercentageDecreaseTimeout == TimeSpan.Zero
                    ? @default.SamplingPercentageDecreaseTimeout
                    : this.SamplingPercentageDecreaseTimeout;
            }
        }

        /// <summary>
        /// Gets effective sampling percentage increase timeout
        /// adjusted in case user makes an error while setting a value.
        /// </summary>
        internal TimeSpan EffectiveSamplingPercentageIncreaseTimeout
        {
            get
            {
                return this.SamplingPercentageIncreaseTimeout == TimeSpan.Zero
                    ? @default.EffectiveSamplingPercentageIncreaseTimeout
                    : this.SamplingPercentageIncreaseTimeout;
            }
        }

        /// <summary>
        /// Gets effective exponential moving average ratio
        /// adjusted in case user makes an error while setting a value.
        /// </summary>
        internal double EffectiveMovingAverageRatio
        {
            get
            {
                return this.MovingAverageRatio < 0 
                    ? @default.MovingAverageRatio 
                    : this.MovingAverageRatio;
            }
        }

        /// <summary>
        /// Adjusts sampling percentage set by user to account for errors
        /// such as setting it below zero or above 100%.
        /// </summary>
        /// <param name="samplingPercentage">Input sampling percentage.</param>
        /// <returns>Adjusted sampling percentage in range &gt; 0 and &lt;= 100.</returns>
        private static double AdjustSamplingPercentage(double samplingPercentage)
        {
            return samplingPercentage > 100 ? 100 : samplingPercentage <= 0 ? 1E-6 : samplingPercentage;
        }
    }
}
