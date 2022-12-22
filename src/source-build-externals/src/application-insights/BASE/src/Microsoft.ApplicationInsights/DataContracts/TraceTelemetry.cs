namespace Microsoft.ApplicationInsights.DataContracts
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics;

    /// <summary>
    /// Telemetry type used for log messages.
    /// Contains a time and message and optionally some additional metadata.
    /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#tracktrace">Learn more</a>
    /// </summary>
    public sealed class TraceTelemetry : ITelemetry, ISupportProperties, ISupportAdvancedSampling, IAiSerializableTelemetry
    {
        internal const string EtwEnvelopeName = "Message";
        internal readonly MessageData Data;
        internal string EnvelopeName = "AppTraces";
        private readonly TelemetryContext context;
        private IExtension extension;

        private double? samplingPercentage;

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceTelemetry"/> class.
        /// </summary>
        public TraceTelemetry()
        {
            this.Data = new MessageData();
            this.context = new TelemetryContext(this.Data.properties);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceTelemetry"/> class.
        /// </summary>
        public TraceTelemetry(string message) : this()
        {
            this.Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceTelemetry"/> class.
        /// </summary>
        public TraceTelemetry(string message, SeverityLevel severityLevel) : this(message)
        {
            this.SeverityLevel = severityLevel;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceTelemetry"/> class by cloning an existing instance.
        /// </summary>
        /// <param name="source">Source instance of <see cref="TraceTelemetry"/> to clone from.</param>
        private TraceTelemetry(TraceTelemetry source)
        {
            this.Data = source.Data.DeepClone();
            this.context = source.context.DeepClone(this.Data.properties);
            this.Sequence = source.Sequence;
            this.Timestamp = source.Timestamp;
            this.samplingPercentage = source.samplingPercentage;
            this.ProactiveSamplingDecision = source.ProactiveSamplingDecision;
            this.extension = source.extension?.DeepClone();
        }

        /// <inheritdoc />
        string IAiSerializableTelemetry.TelemetryName
        {
            get
            {
                return this.EnvelopeName;
            }

            set
            {
                this.EnvelopeName = value;
            }
        }

        /// <inheritdoc />
        string IAiSerializableTelemetry.BaseType => nameof(MessageData);

        /// <summary>
        /// Gets or sets date and time when event was recorded.
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the value that defines absolute order of the telemetry item.
        /// </summary>
        public string Sequence { get; set; }

        /// <summary>
        /// Gets the context associated with the current telemetry item.
        /// </summary>
        public TelemetryContext Context
        {
            get { return this.context; }
        }

        /// <summary>
        /// Gets or sets gets the extension used to extend this telemetry instance using new strong typed object.
        /// </summary>
        public IExtension Extension
        {
            get { return this.extension; }
            set { this.extension = value; }
        }

        /// <summary>
        /// Gets or sets the message text. For example, the text that would normally be written to a log file line.
        /// </summary>
        public string Message
        {
            get { return this.Data.message; }
            set { this.Data.message = value; }
        }

        /// <summary>
        /// Gets or sets Trace severity level.
        /// </summary>
        public SeverityLevel? SeverityLevel
        {
            get { return this.Data.severityLevel.TranslateSeverityLevel(); }
            set { this.Data.severityLevel = value.TranslateSeverityLevel(); }
        }

        /// <summary>
        /// Gets a dictionary of application-defined property names and values providing additional information about this trace.
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#properties">Learn more</a>
        /// </summary>
        public IDictionary<string, string> Properties
        {
#pragma warning disable CS0618 // Type or member is obsolete
            get
            {
                if (!string.IsNullOrEmpty(this.MetricExtractorInfo) && !this.Context.Properties.ContainsKey(MetricTerms.Extraction.ProcessedByExtractors.Moniker.Key))
                {
                    this.Context.Properties[MetricTerms.Extraction.ProcessedByExtractors.Moniker.Key] = this.MetricExtractorInfo;
                }

                return this.Context.Properties;
#pragma warning restore CS0618 // Type or member is obsolete
            }
        }

        /// <summary>
        /// Gets or sets data sampling percentage (between 0 and 100).
        /// Should be 100/n where n is an integer. <a href="https://go.microsoft.com/fwlink/?linkid=832969">Learn more</a>
        /// </summary>
        double? ISupportSampling.SamplingPercentage
        {
            get { return this.samplingPercentage; }
            set { this.samplingPercentage = value; }
        }

        /// <summary>
        /// Gets item type for sampling evaluation.
        /// </summary>
        public SamplingTelemetryItemTypes ItemTypeFlag => SamplingTelemetryItemTypes.Message;

        /// <inheritdoc/>
        public SamplingDecision ProactiveSamplingDecision { get; set; }

        /// <summary>
        /// Gets or sets the MetricExtractorInfo.
        /// </summary>
        internal string MetricExtractorInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Deeply clones a <see cref="TraceTelemetry"/> object.
        /// </summary>
        /// <returns>A cloned instance.</returns>
        public ITelemetry DeepClone()
        {
            return new TraceTelemetry(this);
        }

        /// <inheritdoc/>
        public void SerializeData(ISerializationWriter serializationWriter)
        {
            if (serializationWriter == null)
            {
                throw new ArgumentNullException(nameof(serializationWriter));
            }

            serializationWriter.WriteProperty(this.Data);
        }

        /// <summary>
        /// Sanitizes the properties based on constraints.
        /// </summary>
        void ITelemetry.Sanitize()
        {
            this.Data.message = this.Data.message.SanitizeMessage();
            this.Data.message = Utils.PopulateRequiredStringValue(this.Data.message, "message", typeof(TraceTelemetry).FullName);
            this.Data.properties.SanitizeProperties();
        }
    }
}
