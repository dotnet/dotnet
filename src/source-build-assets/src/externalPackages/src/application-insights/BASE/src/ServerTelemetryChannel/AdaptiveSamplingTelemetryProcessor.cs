namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel
{
    using System;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;

    /// <summary>
    /// Telemetry processor for sampling telemetry at a dynamic rate before sending to Application Insights.
    /// </summary>
    public class AdaptiveSamplingTelemetryProcessor : ITelemetryProcessor, IDisposable
    {
        /// <summary>
        /// Fixed-rate sampling telemetry processor.
        /// </summary>
        private readonly SamplingTelemetryProcessor samplingProcessor;
        
        /// <summary>
        /// Sampling percentage estimator settings.
        /// </summary>
        private readonly Channel.Implementation.SamplingPercentageEstimatorSettings estimatorSettings;

        /// <summary>
        /// Callback invoked every time sampling percentage is evaluated.
        /// </summary>
        private readonly Channel.Implementation.AdaptiveSamplingPercentageEvaluatedCallback evaluationCallback;

        /// <summary>
        /// Sampling percentage estimator telemetry processor.
        /// </summary>
        private SamplingPercentageEstimatorTelemetryProcessor estimatorProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdaptiveSamplingTelemetryProcessor"/> class.
        /// <param name="next">Next TelemetryProcessor in call chain.</param>
        /// </summary>
        public AdaptiveSamplingTelemetryProcessor(ITelemetryProcessor next)
            : this(new Channel.Implementation.SamplingPercentageEstimatorSettings(), null, next)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdaptiveSamplingTelemetryProcessor"/> class.
        /// <param name="settings">Sampling percentage estimator settings.</param>
        /// <param name="callback">Callback invoked every time sampling percentage is evaluated.</param>
        /// <param name="next">Next TelemetryProcessor in call chain.</param>
        /// </summary>
        public AdaptiveSamplingTelemetryProcessor(
            Channel.Implementation.SamplingPercentageEstimatorSettings settings,
            Channel.Implementation.AdaptiveSamplingPercentageEvaluatedCallback callback,
            ITelemetryProcessor next)
        {
            this.estimatorSettings = settings;
            this.evaluationCallback = callback;

            // make estimator telemetry processor  work after sampling was done
            this.estimatorProcessor = new SamplingPercentageEstimatorTelemetryProcessor(settings, this.SamplingPercentageChanged, next);
            this.samplingProcessor = new SamplingTelemetryProcessor(next, this.estimatorProcessor)
            {
                SamplingPercentage = this.estimatorSettings.InitialSamplingPercentage,
                ProactiveSamplingPercentage = null,
            };
        }

        /// <summary>
        /// Gets or sets a semicolon separated list of telemetry types that should not be sampled. 
        /// Types listed are excluded even if they are set in IncludedTypes.
        /// </summary>
        public string ExcludedTypes
        {
            get { return this.samplingProcessor.ExcludedTypes; }

            set { this.samplingProcessor.ExcludedTypes = value; }
        }

        /// <summary>
        /// Gets or sets a semicolon separated list of telemetry types that should be sampled. 
        /// If left empty all types are included implicitly. 
        /// Types are not included if they are set in ExcludedTypes.
        /// </summary>
        public string IncludedTypes
        {
            get { return this.samplingProcessor.IncludedTypes; }

            set { this.samplingProcessor.IncludedTypes = value; }
        }

        /// <summary>
        /// Gets or sets initial sampling percentage applied at the start
        /// of the process to dynamically vary the percentage.
        /// </summary>
        public double InitialSamplingPercentage
        {
            get
            {
                return this.estimatorSettings.InitialSamplingPercentage;
            }

            set
            {
                // note: 'initial' percentage will affect sampling even 
                // if it was running for a while
                this.estimatorSettings.InitialSamplingPercentage = value;
                this.estimatorProcessor.CurrentSamplingRate = this.estimatorSettings.EffectiveInitialSamplingRate;
                this.samplingProcessor.SamplingPercentage = value;
            }
        }

        /// <summary>
        /// Gets or sets maximum rate of telemetry items per second
        /// dynamic sampling will try to adhere to.
        /// </summary>
        public double MaxTelemetryItemsPerSecond
        {
            get
            {
                return this.estimatorSettings.MaxTelemetryItemsPerSecond;
            }

            set
            {
                this.estimatorSettings.MaxTelemetryItemsPerSecond = value;
            }
        }

        /// <summary>
        /// Gets or sets minimum sampling percentage that can be set 
        /// by the dynamic sampling percentage algorithm.
        /// </summary>
        public double MinSamplingPercentage
        {
            get
            {
                return this.estimatorSettings.MinSamplingPercentage;
            }

            set
            {
                this.estimatorSettings.MinSamplingPercentage = value;
            }
        }

        /// <summary>
        /// Gets or sets maximum sampling percentage that can be set 
        /// by the dynamic sampling percentage algorithm.
        /// </summary>
        public double MaxSamplingPercentage
        {
            get
            {
                return this.estimatorSettings.MaxSamplingPercentage;
            }

            set
            {
                this.estimatorSettings.MaxSamplingPercentage = value;
            }
        }

        /// <summary>
        /// Gets or sets duration of the sampling percentage evaluation interval.
        /// </summary>
        public TimeSpan EvaluationInterval
        {
            get
            {
                return this.estimatorSettings.EvaluationInterval;
            }

            set
            {
                this.estimatorSettings.EvaluationInterval = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating how long to not to decrease
        /// sampling percentage after last change to prevent excessive fluctuation.
        /// </summary>
        public TimeSpan SamplingPercentageDecreaseTimeout
        {
            get
            {
                return this.estimatorSettings.SamplingPercentageDecreaseTimeout;
            }

            set
            {
                this.estimatorSettings.SamplingPercentageDecreaseTimeout = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating how long to not to increase
        /// sampling percentage after last change to prevent excessive fluctuation.
        /// </summary>
        public TimeSpan SamplingPercentageIncreaseTimeout
        {
            get
            {
                return this.estimatorSettings.SamplingPercentageIncreaseTimeout;
            }

            set
            {
                this.estimatorSettings.SamplingPercentageIncreaseTimeout = value;
            }
        }

        /// <summary>
        /// Gets or sets exponential moving average ratio (factor) applied
        /// during calculation of rate of telemetry items produced by the application.
        /// </summary>
        public double MovingAverageRatio
        {
            get
            {
                return this.estimatorSettings.MovingAverageRatio;
            }

            set
            {
                this.estimatorSettings.MovingAverageRatio = value;
            }
        }

        /// <summary>
        /// Gets sampling telemetry processor.
        /// </summary>
        internal SamplingTelemetryProcessor SamplingTelemetryProcessor => this.samplingProcessor;

        /// <summary>
        /// Gets sampling percentage estimator telemetry processor.
        /// </summary>
        internal SamplingPercentageEstimatorTelemetryProcessor SamplingPercentageEstimatorTelemetryProcessor => this.estimatorProcessor;

        /// <summary>
        /// Processes telemetry item.
        /// </summary>
        /// <param name="item">Telemetry item to process.</param>
        public void Process(ITelemetry item)
        {
            this.samplingProcessor.Process(item);
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">True if disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                IDisposable estimatorProc = this.estimatorProcessor;
                if (estimatorProc != null)
                {
                    estimatorProc.Dispose();
                    this.estimatorProcessor = null;
                }
            }
        }

        private void SamplingPercentageChanged(
            double afterSamplingTelemetryItemRatePerSecond,
            double currentSamplingPercentage,
            double newSamplingPercentage,
            bool isSamplingPercentageChanged,
            Channel.Implementation.SamplingPercentageEstimatorSettings settings)
        {
            if (isSamplingPercentageChanged)
            {
                this.samplingProcessor.SamplingPercentage = newSamplingPercentage;
                this.samplingProcessor.ProactiveSamplingPercentage = 100 / this.estimatorProcessor.CurrentProactiveSamplingRate;
                TelemetryChannelEventSource.Log.SamplingChanged(newSamplingPercentage);
            }

            this.evaluationCallback?.Invoke(
                afterSamplingTelemetryItemRatePerSecond,
                currentSamplingPercentage,
                newSamplingPercentage,
                isSamplingPercentageChanged,
                settings);
        }
    }
}
