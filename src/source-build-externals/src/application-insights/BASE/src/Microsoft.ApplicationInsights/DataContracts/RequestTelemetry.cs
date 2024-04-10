namespace Microsoft.ApplicationInsights.DataContracts
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics;

    /// <summary>
    /// Encapsulates information about a web request handled by the application.
    /// </summary>
    /// <remarks>
    /// You can send information about requests processed by your web application to Application Insights by
    /// passing an instance of the <see cref="RequestTelemetry"/> class to the <see cref="TelemetryClient.TrackRequest(RequestTelemetry)"/>
    /// method.
    /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#trackrequest">Learn more</a>
    /// </remarks>
    public sealed class RequestTelemetry : OperationTelemetry, ITelemetry, ISupportProperties, ISupportMetrics, ISupportAdvancedSampling, IAiSerializableTelemetry
    {
        internal const string EtwEnvelopeName = "Request";
        internal string EnvelopeName = "AppRequests";

        private readonly TelemetryContext context;
        private RequestData dataPrivate;
        private bool successFieldSet;
        private IExtension extension;
        private double? samplingPercentage;
        private bool success = true;
        private IDictionary<string, double> measurementsValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestTelemetry"/> class.
        /// </summary>
        public RequestTelemetry()
        {
            this.context = new TelemetryContext();
            this.GenerateId();
            this.Source = string.Empty;
            this.Name = string.Empty;
            this.ResponseCode = string.Empty;            
            this.Duration = System.TimeSpan.Zero;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestTelemetry"/> class with the given <paramref name="name"/>,
        /// <paramref name="startTime"/>, <paramref name="duration"/>, <paramref name="responseCode"/> and <paramref name="success"/> property values.
        /// </summary>
        public RequestTelemetry(string name, DateTimeOffset startTime, TimeSpan duration, string responseCode, bool success)
            : this()
        {
            this.Name = name; // Name is optional but without it UX does not make much sense
            this.Timestamp = startTime;
            this.Duration = duration;
            this.ResponseCode = responseCode;
            this.Success = success;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestTelemetry"/> class by cloning an existing instance.
        /// </summary>
        /// <param name="source">Source instance of <see cref="RequestTelemetry"/> to clone from.</param>
        private RequestTelemetry(RequestTelemetry source)
        {
            this.Duration = source.Duration;
            this.Id = source.Id;
            if (source.measurementsValue != null)
            {
                Utils.CopyDictionary(source.Metrics, this.Metrics);
            }

            this.Name = source.Name;
            this.context = source.context.DeepClone();
            this.ResponseCode = source.ResponseCode;
            this.Source = source.Source;
            this.Success = source.Success;
            this.Url = source.Url;
            this.Sequence = source.Sequence;
            this.Timestamp = source.Timestamp;
            this.successFieldSet = source.successFieldSet;
            this.extension = source.extension?.DeepClone();
            this.samplingPercentage = source.samplingPercentage;
            this.ProactiveSamplingDecision = source.ProactiveSamplingDecision;
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
        string IAiSerializableTelemetry.BaseType => nameof(RequestData);

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
            get
            {
                if (this.successFieldSet)
                {
                    return this.success;
                }

                return null;
            }

            set
            {
                if (value != null && value.HasValue)
                {
                    this.success = value.Value;
                    this.successFieldSet = true;
                }
                else
                {
                    this.success = true;
                    this.successFieldSet = false;
                }
            }
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
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#properties">Learn more</a>
        /// </summary>
        public override IDictionary<string, string> Properties
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
        /// Gets or sets request url (optional).
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        /// Gets a dictionary of application-defined request metrics.
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#properties">Learn more</a>
        /// </summary>
        public override IDictionary<string, double> Metrics
        {
            get { return LazyInitializer.EnsureInitialized(ref this.measurementsValue, () => new ConcurrentDictionary<string, double>()); }
        }

        /// <summary>
        /// Gets or sets the HTTP method of the request.
        /// </summary>
        [Obsolete("Include http verb into request telemetry name and use custom properties to report http method as a dimension.")]
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
        /// Gets item type for sampling evaluation.
        /// </summary>
        public SamplingTelemetryItemTypes ItemTypeFlag => SamplingTelemetryItemTypes.Request;

        /// <inheritdoc/>
        public SamplingDecision ProactiveSamplingDecision { get; set; }

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
        /// Gets the Data associated with this Telemetry instance.
        /// This is being served by a singleton instance, so this will
        /// not pickup changes made to the telemetry after first call to this.
        /// It is recommended to make all changes (including sanitization)
        /// to this telemetry before calling Data.
        /// </summary>
        internal RequestData Data
        {
            get
            {
                return LazyInitializer.EnsureInitialized(ref this.dataPrivate,
                     () =>
                         {
                             var req = new RequestData();
                             req.duration = this.Duration;
                             req.id = this.Id;
                             req.measurements = this.measurementsValue;
                             req.name = this.Name;
                             req.properties = this.context.PropertiesValue;
                             req.responseCode = this.ResponseCode;
                             req.source = this.Source;
                             req.success = this.success;
                             req.url = this.Url?.ToString();
                             return req;
                         });
            }

            private set
            {
                 this.dataPrivate = value;
            }
        }

        /// <summary>
        /// Deeply clones a <see cref="RequestTelemetry"/> object.
        /// </summary>
        /// <returns>A cloned instance.</returns>
        public override ITelemetry DeepClone()
        {
            return new RequestTelemetry(this);
        }

        /// <inheritdoc/>
        public override void SerializeData(ISerializationWriter serializationWriter)
        {
            // To ensure that all changes to telemetry are reflected in serialization,
            // the underlying field is set to null, which forces it to be re-created.
            this.dataPrivate = null;
            serializationWriter.WriteProperty(this.Data);
        }

        /// <summary>
        /// Sanitizes the properties based on constraints.
        /// </summary>
        void ITelemetry.Sanitize()
        {
            this.Name = this.Name.SanitizeName();
            this.Properties.SanitizeProperties();
            if (this.measurementsValue != null)
            {
                this.Metrics.SanitizeMeasurements();
            }
            else
            {
                this.measurementsValue = new Dictionary<string, double>();
            }

            this.Url = this.Url.SanitizeUri();

            // Set for backward compatibility:
            this.Id = this.Id.SanitizeName();
            this.Id = Utils.PopulateRequiredStringValue(this.Id, "id", typeof(RequestTelemetry).FullName);

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
