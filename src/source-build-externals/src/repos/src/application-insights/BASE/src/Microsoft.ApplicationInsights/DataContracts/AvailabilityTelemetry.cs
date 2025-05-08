namespace Microsoft.ApplicationInsights.DataContracts
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;

    /// <summary>
    /// Telemetry type used for availability web test results.
    /// Contains a time and message and optionally some additional metadata.
    /// <a href="https://go.microsoft.com/fwlink/?linkid=517889">Learn more</a>
    /// </summary>
    public sealed class AvailabilityTelemetry : ITelemetry, ISupportProperties, ISupportMetrics, IAiSerializableTelemetry
    {
        internal const string EtwEnvelopeName = "Availability";
        internal readonly AvailabilityData Data;
        internal string EnvelopeName = "AppAvailabilityResults";
        private readonly TelemetryContext context;
        private IExtension extension;

        /// <summary>
        /// Initializes a new instance of the <see cref="AvailabilityTelemetry"/> class with empty properties.
        /// </summary>
        public AvailabilityTelemetry()
        {
            this.Data = new AvailabilityData();
            this.context = new TelemetryContext(this.Data.properties);
            this.Data.id = Convert.ToBase64String(BitConverter.GetBytes(WeakConcurrentRandom.Instance.Next()));
            this.Success = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvailabilityTelemetry"/> class with empty properties.
        /// </summary>
        public AvailabilityTelemetry(string name, DateTimeOffset timeStamp, TimeSpan duration, string runLocation, bool success, string message = null)
            : this()
        {
            this.Data.name = name;
            this.Data.duration = duration;
            this.Data.success = success;
            this.Data.runLocation = runLocation;
            this.Data.message = message;
            this.Timestamp = timeStamp;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvailabilityTelemetry"/> class by cloning an existing instance.
        /// </summary>
        /// <param name="source">Source instance of <see cref="AvailabilityTelemetry"/> to clone from.</param>
        private AvailabilityTelemetry(AvailabilityTelemetry source)
        {
            this.Data = source.Data.DeepClone();
            this.context = source.context.DeepClone(this.Data.properties);
            this.Sequence = source.Sequence;
            this.Timestamp = source.Timestamp;
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
        string IAiSerializableTelemetry.BaseType => nameof(AvailabilityData);

        /// <summary>
        /// Gets or sets the test run id.
        /// </summary>
        public string Id
        {
            get { return this.Data.id; }
            set { this.Data.id = value; }
        }

        /// <summary>
        /// Gets or sets the test name.
        /// </summary>
        public string Name
        {
            get { return this.Data.name; }
            set { this.Data.name = value; }
        }

        /// <summary>
        /// Gets or sets availability test duration.
        /// </summary>
        public TimeSpan Duration
        {
            get
            {
                return this.Data.duration;
            }

            set
            {
                this.Data.duration = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the availability test was successful or not.
        /// </summary>
        public bool Success
        {
            get { return this.Data.success; }
            set { this.Data.success = value; }
        }

        /// <summary>
        /// Gets or sets location where availability test was run.
        /// </summary>
        public string RunLocation
        {
            get { return this.Data.runLocation; }
            set { this.Data.runLocation = value; }
        }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string Message
        {
            get { return this.Data.message; }
            set { this.Data.message = value; }
        }

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
        /// Gets a dictionary of application-defined property names and values providing additional information about this availability test run.
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#properties">Learn more</a>
        /// </summary>
        public IDictionary<string, string> Properties
        {
            get { return this.Data.properties; }
        }

        /// <summary>
        /// Gets a dictionary of application-defined event metrics.
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#properties">Learn more</a>
        /// </summary>
        public IDictionary<string, double> Metrics
        {
            get { return this.Data.measurements; }
        }

        /// <summary>
        /// Gets or sets date and time when telemetry was recorded.
        /// </summary>
        public DateTimeOffset Timestamp
        {
            get; set;
        }

        /// <summary>
        /// Deeply clones an  <see cref="AvailabilityTelemetry"/> object.
        /// </summary>
        /// <returns>A cloned instance.</returns>
        public ITelemetry DeepClone()
        {
            return new AvailabilityTelemetry(this);
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
            // Makes message content similar to OOB web test results on the portal.
            this.Message = (this.Data.success && string.IsNullOrEmpty(this.Message)) ? "Passed" : ((!this.Data.success && string.IsNullOrEmpty(this.Message)) ? "Failed" : this.Message);

            this.Name = this.Name.SanitizeTestName();
            this.Name = Utils.PopulateRequiredStringValue(this.Name, "TestName", typeof(AvailabilityTelemetry).FullName);

            this.RunLocation = this.RunLocation.SanitizeRunLocation();
            this.Message = this.Message.SanitizeAvailabilityMessage();

            this.Data.properties.SanitizeProperties();
            this.Data.measurements.SanitizeMeasurements();
        }
    }
}
