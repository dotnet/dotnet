namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel
{
    using System;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation.SamplingInternals;

    /// <summary>
    /// Represents a telemetry processor for sampling telemetry at a fixed-rate before sending to Application Insights.
    /// Supports telemetry items sampled at head, adjusts gain up accordingly.
    /// </summary>
    public sealed class SamplingTelemetryProcessor : ITelemetryProcessor
    {
        private readonly AtomicSampledItemsCounter proactivelySampledOutCounters = new AtomicSampledItemsCounter();

        private string excludedTypesString;

        private SamplingTelemetryItemTypes includedTypesFlags;
        private string includedTypesString;

        /// <summary>
        /// Initializes a new instance of the <see cref="SamplingTelemetryProcessor"/> class.
        /// <param name="next">Next TelemetryProcessor in call chain.</param>
        /// </summary>
        public SamplingTelemetryProcessor(ITelemetryProcessor next)
        {
            this.SamplingPercentage = 100.0;
            this.ProactiveSamplingPercentage = null;
            this.SampledNext = next ?? throw new ArgumentNullException(nameof(next));
            this.UnsampledNext = next;
        }

        internal SamplingTelemetryProcessor(ITelemetryProcessor unsampledNext, ITelemetryProcessor sampledNext) : this(sampledNext)
        {
            this.UnsampledNext = unsampledNext ?? throw new ArgumentNullException(nameof(unsampledNext));
        }

        /// <summary>
        /// Gets or sets a semicolon separated list of telemetry types that should not be sampled.
        /// Allowed type names: Dependency, Event, Exception, PageView, Request, Trace. 
        /// Types listed are excluded even if they are set in IncludedTypes.
        /// Do not set both ExcludedTypes and IncludedTypes. ExcludedTypes will take precedence over IncludedTypes. 
        /// </summary>
        public string ExcludedTypes
        {
            get
            {
                return this.excludedTypesString;
            }

            set
            {
                this.excludedTypesString = value;

                if (value != null)
                {
                    var newIncludesFlags = SamplingIncludesUtility.CalculateFromExcludes(value);
                    this.includedTypesFlags = newIncludesFlags;

                    if (this.includedTypesString != null)
                    {
                        // excluded will always overwrite included. Log a "Configuration Error".
                        TelemetryChannelEventSource.Log.SamplingConfigErrorBothTypes();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a semicolon separated list of telemetry types that should be sampled. 
        /// Allowed type names: Dependency, Event, Exception, PageView, Request, Trace. 
        /// If left empty all types are included implicitly. 
        /// Types are not included if they are set in ExcludedTypes.
        /// Do not set both ExcludedTypes and IncludedTypes. ExcludedTypes will take precedence over IncludedTypes. 
        /// </summary>
        public string IncludedTypes
        {
            get
            {
                return this.includedTypesString;
            }

            set
            {
                this.includedTypesString = value;

                if (value != null)
                {
                    if (this.excludedTypesString != null)
                    {
                        // included cannot overwrite excluded. Log a "Configuration Error".
                        TelemetryChannelEventSource.Log.SamplingConfigErrorBothTypes();
                    }
                    else
                    {
                        var newIncludesFlags = SamplingIncludesUtility.CalculateFromIncludes(value);
                        this.includedTypesFlags = newIncludesFlags;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets data sampling percentage (between 0 and 100) for all <see cref="ITelemetry"/>
        /// objects logged in this <see cref="TelemetryClient"/>.
        /// </summary>
        /// <remarks>
        /// All sampling percentage must be in a ratio of 100/N where N is a whole number (2, 3, 4, …). E.g. 50 for 1/2 or 33.33 for 1/3.
        /// Failure to follow this pattern can result in unexpected / incorrect computation of values in the portal.
        /// </remarks>
        public double SamplingPercentage { get; set; }

        /// <summary>
        /// Gets or sets current proactive-sampling percentage of telemetry items.
        /// </summary>
        internal double? ProactiveSamplingPercentage { get; set; }

        /// <summary>
        /// Gets or sets the next TelemetryProcessor in call chain to send evaluated (sampled) telemetry items to.
        /// </summary>
        private ITelemetryProcessor SampledNext { get; set; }

        /// <summary>
        /// Gets or sets the next TelemetryProcessor to call in the chain if the ITelemetry item passed in is not sampled. Note that 
        /// for the public instances of this class (those created by naming the module in ApplicationInsights.config) this property
        /// will be equal to the <see cref="SampledNext"/> property.
        /// </summary>
        private ITelemetryProcessor UnsampledNext { get; set; }

        /// <summary>
        /// Process a collected telemetry item.
        /// </summary>
        /// <param name="item">A collected Telemetry item.</param>
        public void Process(ITelemetry item)
        {
            double samplingPercentage = this.SamplingPercentage;

            //// If sampling rate is 100% and we aren't distinguishing between evaluated/unevaluated items, there is nothing to do:
            if (samplingPercentage >= 100.0 - 1.0E-12 && this.SampledNext.Equals(this.UnsampledNext))
            {
                this.HandlePossibleProactiveSampling(item, samplingPercentage);
                return;
            }

            //// So sampling rate is not 100%, or we must evaluate further
            
            var advancedSamplingSupportingTelemetry = item as ISupportAdvancedSampling;

            // If someone implemented ISupportSampling and hopes that SamplingTelemetryProcessor will continue to work for them:
            var samplingSupportingTelemetry = advancedSamplingSupportingTelemetry ?? item as ISupportSampling;

            //// If null was passed in as item or if sampling not supported in general, do nothing:    
            if (samplingSupportingTelemetry == null)
            {
                this.UnsampledNext.Process(item);
                return;
            }

            //// If telemetry was excluded by type, do nothing:
            if (advancedSamplingSupportingTelemetry != null && !this.IsSamplingApplicable(advancedSamplingSupportingTelemetry.ItemTypeFlag))
            {
                if (TelemetryChannelEventSource.IsVerboseEnabled)
                {
                    TelemetryChannelEventSource.Log.SamplingSkippedByType(item.ToString());
                }

                this.UnsampledNext.Process(item);
                return;
            }

            //// If telemetry was already sampled, do nothing:
            bool itemAlreadySampled = samplingSupportingTelemetry.SamplingPercentage.HasValue;
            if (itemAlreadySampled)
            {
                this.UnsampledNext.Process(item);
                return;
            }

            //// Ok, now we can actually sample:

            samplingSupportingTelemetry.SamplingPercentage = samplingPercentage;

            bool isSampledIn;

            // if this is executed in adaptive sampling processor (rate ratio has value), 
            // and item supports proactive sampling and was sampled in before, we'll give it more weight
            if (this.ProactiveSamplingPercentage.HasValue &&
                advancedSamplingSupportingTelemetry != null &&
                advancedSamplingSupportingTelemetry.ProactiveSamplingDecision == SamplingDecision.SampledIn)
            {
                // if current rate of proactively sampled-in telemetry is too high, ProactiveSamplingPercentage is low:
                // we'll sample in as much proactively sampled in items as we can (based on their sampling score)
                // so that we still keep target rate.
                // if current rate of proactively sampled-in telemetry is less that configured, ProactiveSamplingPercentage
                // is high - it could be > 100 - and we'll sample in all items with proactive SampledIn decision (plus some more in else branch).
                isSampledIn = SamplingScoreGenerator.GetSamplingScore(item) < this.ProactiveSamplingPercentage;
            }
            else
            {
                isSampledIn = SamplingScoreGenerator.GetSamplingScore(item) < samplingPercentage;
            }

            if (isSampledIn)
            {
                if (advancedSamplingSupportingTelemetry != null)
                {
                    this.HandlePossibleProactiveSampling(item, samplingPercentage, advancedSamplingSupportingTelemetry);
                }
                else
                {
                    this.SampledNext.Process(item);
                }
            }
            else
            { 
                if (TelemetryChannelEventSource.IsVerboseEnabled)
                {
                    TelemetryChannelEventSource.Log.ItemSampledOut(item.ToString());
                }

                TelemetryDebugWriter.WriteTelemetry(item, nameof(SamplingTelemetryProcessor));
            }
        }

        private void HandlePossibleProactiveSampling(ITelemetry item, double currentSamplingPercentage, ISupportAdvancedSampling samplingSupportingTelemetry = null)
        {
            var advancedSamplingSupportingTelemetry = samplingSupportingTelemetry ?? item as ISupportAdvancedSampling;

            if (advancedSamplingSupportingTelemetry != null)
            {
                if (advancedSamplingSupportingTelemetry.ProactiveSamplingDecision == SamplingDecision.SampledOut)
                {
                    // Item is sampled in but was proactively sampled out: store the amount of items it represented and drop it
                    this.proactivelySampledOutCounters.AddItems(advancedSamplingSupportingTelemetry.ItemTypeFlag, Convert.ToInt64(100 / currentSamplingPercentage));

                    if (TelemetryChannelEventSource.IsVerboseEnabled)
                    {
                        TelemetryChannelEventSource.Log.ItemProactivelySampledOut(item.ToString());
                    }
                }
                else
                {
                    var proactivelySampledOutItemsCount = this.proactivelySampledOutCounters.GetItems(advancedSamplingSupportingTelemetry.ItemTypeFlag);
                    if (proactivelySampledOutItemsCount > 0)
                    {
                        // The item is sampled in and may need to represent all proactively sampled out items and itself.
                        // The current item with sample rate SR represents 100/SR items.
                        // We stored that it needs to represent X more items.
                        // We need to adjust sample rate to represent (100/SR + X) items in 1 item.
                        // It is 100 / (100/SR + X) = 100 / ((100 + X*SR)/SR) = (100 * SR) / (100 + X*SR)
                        advancedSamplingSupportingTelemetry.SamplingPercentage = (100 * advancedSamplingSupportingTelemetry.SamplingPercentage) / (100 + (proactivelySampledOutItemsCount * advancedSamplingSupportingTelemetry.SamplingPercentage));
                        this.proactivelySampledOutCounters.ClearItems(advancedSamplingSupportingTelemetry.ItemTypeFlag);
                    }

                    this.SampledNext.Process(item);
                }
            }
            else
            {
                this.SampledNext.Process(item);
            }
        }

        private bool IsSamplingApplicable(SamplingTelemetryItemTypes telemetryItemTypeFlag)
        {
            if (this.includedTypesFlags == SamplingTelemetryItemTypes.None)
            {
                // default value
                return true;
            }
            else
            {
                return this.includedTypesFlags.HasFlag(telemetryItemTypeFlag);
            }
        }
    }
}
