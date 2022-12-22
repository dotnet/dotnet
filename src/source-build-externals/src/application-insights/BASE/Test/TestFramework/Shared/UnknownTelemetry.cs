namespace Microsoft.ApplicationInsights.TestFramework
{
    using System;
    using System.Collections.Generic;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    // Request Telemetry in disguise
    public sealed class UnknownTelemetry : OperationTelemetry, ITelemetry, ISupportProperties, ISupportMetrics, ISupportSampling
    {
        internal new const string TelemetryName = "Unknown";
        private readonly TelemetryContext context;
        private IExtension extension;
        private double? samplingPercentage;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownTelemetry"/> class.
        /// </summary>
        public UnknownTelemetry()
        {
            this.Properties = new Dictionary<string, string>();
            this.Metrics = new Dictionary<string, double>();
            this.context = new TelemetryContext(this.Properties);
            this.GenerateId();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownTelemetry"/> class with the given <paramref name="name"/>,
        /// <paramref name="startTime"/>, <paramref name="duration"/>, <paramref name="responseCode"/> and <paramref name="success"/> property values.
        /// </summary>
        public UnknownTelemetry(string name, DateTimeOffset startTime, TimeSpan duration, string responseCode, bool success)
            : this()
        {
            this.Name = name; // Name is optional but without it UX does not make much sense
            this.Timestamp = startTime;
            this.Duration = duration;
            this.ResponseCode = responseCode;
            this.Success = success;
            this.Properties = new Dictionary<string, string>();
            this.Metrics = new Dictionary<string, double>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownTelemetry"/> class by cloning an existing instance.
        /// </summary>
        /// <param name="source">Source instance of <see cref="UnknownTelemetry"/> to clone from.</param>
        private UnknownTelemetry(UnknownTelemetry source)
        {
            this.context = source.context.DeepClone(this.Properties);
            this.Sequence = source.Sequence;
            this.Timestamp = source.Timestamp;
            this.extension = source.extension?.DeepClone();
            this.Properties = new Dictionary<string, string>();
            this.Metrics = new Dictionary<string, double>();
        }

        /// <summary>
        /// Gets or sets date and time when telemetry was recorded.
        /// </summary>
        public override DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the value that defines absolute order of the telemetry item.
        /// </summary>
        public override string Sequence { get; set; }

        /// <summary>
        /// Gets the object that contains contextual information about the application at the time when it handled the request.
        /// </summary>
        public override TelemetryContext Context
        {
            get { return this.context; }
        }

        /// <summary>
        /// Gets or sets gets the extension used to extend this telemetry instance using new strong typed object.
        /// </summary>
        public override IExtension Extension
        {
            get { return this.extension; }
            set { this.extension = value; }
        }

        /// <summary>
        /// Gets or sets Request ID.
        /// </summary>
        public override string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets human-readable name of the requested page.
        /// </summary>
        public override string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets response code returned by the application after handling the request.
        /// </summary>
        public string ResponseCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether application handled the request successfully.
        /// </summary>
        public override bool? Success
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the amount of time it took the application to handle the request.
        /// </summary>
        public override TimeSpan Duration
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a dictionary of application-defined property names and values providing additional information about this request.        
        /// </summary>
        public override IDictionary<string, string> Properties
        {
            get;
        }

        /// <summary>
        /// Gets or sets request url (optional).
        /// </summary>
        public Uri Url
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a dictionary of application-defined request metrics.        
        /// </summary>
        public override IDictionary<string, double> Metrics
        {
            get;
        }

        /// <summary>
        /// Gets or sets the HTTP method of the request.
        /// </summary>        
        public string HttpMethod
        {
            get { return this.Properties["httpMethod"]; }
            set { this.Properties["httpMethod"] = value; }
        }

        /// <summary>
        /// Gets or sets data sampling percentage (between 0 and 100).
        /// </summary>
        double? ISupportSampling.SamplingPercentage
        {
            get { return this.samplingPercentage; }
            set { this.samplingPercentage = value; }
        }

        /// <summary>
        /// Gets or sets the source for the request telemetry object. This often is a hashed instrumentation key identifying the caller.
        /// </summary>
        public string Source
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the MetricExtractorInfo.
        /// </summary>
        internal string MetricExtractorInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Deeply clones a <see cref="UnknownTelemetry"/> object.
        /// </summary>
        /// <returns>A cloned instance.</returns>
        public override ITelemetry DeepClone()
        {
            return new UnknownTelemetry(this);
        }

        /// <inheritdoc/>
        public override void SerializeData(ISerializationWriter serializationWriter)
        {
            serializationWriter.WriteProperty("id", this.Id);
            serializationWriter.WriteProperty("source", this.Source);
            serializationWriter.WriteProperty("name", this.Name);
            serializationWriter.WriteProperty("duration", this.Duration);
            serializationWriter.WriteProperty("success", this.Success);
            serializationWriter.WriteProperty("responseCode", this.ResponseCode);
            if (this.Url != null)
            {
                serializationWriter.WriteProperty("url", this.Url.ToString());
            }
            serializationWriter.WriteProperty("properties", this.Properties);
            serializationWriter.WriteProperty("measurements", this.Metrics);
        }

        /// <summary>
        /// Sanitizes the properties based on constraints.
        /// </summary>
        void ITelemetry.Sanitize()
        {
            this.Name = this.Name.SanitizeName();
            this.Properties.SanitizeProperties();
            this.Metrics.SanitizeMeasurements();
            if (this.Url != null)
            {
                this.Url = this.Url.SanitizeUri();
            }
            this.Id = Utils.PopulateRequiredStringValue(this.Id, "id", typeof(UnknownTelemetry).FullName);

            // Required fields
            if (!this.Success.HasValue)
            {
                this.Success = true;
            }

            if (string.IsNullOrEmpty(this.ResponseCode))
            {
                this.ResponseCode = this.Success.Value ? "200" : string.Empty;
            }
        }
    }
}