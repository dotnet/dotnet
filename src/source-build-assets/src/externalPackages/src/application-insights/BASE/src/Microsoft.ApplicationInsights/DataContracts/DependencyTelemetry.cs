namespace Microsoft.ApplicationInsights.DataContracts
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Threading;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics;
    using static System.Threading.LazyInitializer;

    /// <summary>
    /// The class that represents information about the collected dependency.
    /// <a href="https://go.microsoft.com/fwlink/?linkid=839889">Learn more.</a>
    /// </summary>
    public sealed class DependencyTelemetry : OperationTelemetry, ITelemetry, ISupportProperties, ISupportAdvancedSampling, ISupportMetrics, IAiSerializableTelemetry
    {
        internal const string EtwEnvelopeName = "RemoteDependency";
        internal string EnvelopeName = "AppDependencies";
        
        private readonly TelemetryContext context;
        private IExtension extension;
        private double? samplingPercentage;
        private bool successFieldSet;
        private bool success = true;
        private IDictionary<string, double> measurementsValue;
        private RemoteDependencyData internalDataPrivate;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyTelemetry"/> class.
        /// </summary>
        public DependencyTelemetry()
        {            
            this.successFieldSet = true;
            this.context = new TelemetryContext();
            this.GenerateId();
            this.Name = string.Empty;
            this.ResultCode = string.Empty;
            this.Duration = TimeSpan.Zero;
            this.Target = string.Empty;
            this.Type = string.Empty;
            this.Data = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyTelemetry"/> class with the given <paramref name="dependencyName"/>, <paramref name="data"/>,
        /// <paramref name="startTime"/>, <paramref name="duration"/> and <paramref name="success"/> property values.
        /// </summary>
        [Obsolete("Use other constructors which allows to define dependency call with all the properties.")]
        public DependencyTelemetry(string dependencyName, string data, DateTimeOffset startTime, TimeSpan duration, bool success)
            : this()
        {
            this.Name = dependencyName;
            this.Data = data;
            this.Duration = duration;
            this.Success = success;
            this.Timestamp = startTime;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyTelemetry"/> class with the given <paramref name="dependencyTypeName"/>, <paramref name="target"/>,
        /// <paramref name="dependencyName"/>, <paramref name="data"/> property values.
        /// </summary>
        public DependencyTelemetry(string dependencyTypeName, string target, string dependencyName, string data)
            : this()
        {
            this.Type = dependencyTypeName;
            this.Target = target;
            this.Name = dependencyName;
            this.Data = data;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyTelemetry"/> class with the given <paramref name="dependencyTypeName"/>, <paramref name="target"/>,
        /// <paramref name="dependencyName"/>, <paramref name="data"/>, <paramref name="startTime"/>, <paramref name="duration"/>, <paramref name="resultCode"/>
        /// and <paramref name="success"/> and  property values.
        /// </summary>
        public DependencyTelemetry(string dependencyTypeName, string target, string dependencyName, string data, DateTimeOffset startTime, TimeSpan duration, string resultCode, bool success)
            : this()
        {
            this.Type = dependencyTypeName;
            this.Target = target;
            this.Name = dependencyName;
            this.Data = data;
            this.Timestamp = startTime;
            this.Duration = duration;
            this.ResultCode = resultCode;
            this.Success = success;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyTelemetry"/> class by cloning an existing instance.
        /// </summary>
        /// <param name="source">Source instance of <see cref="DependencyTelemetry"/> to clone from.</param>
        private DependencyTelemetry(DependencyTelemetry source)
        {            
            if (source.measurementsValue != null)
            {
                Utils.CopyDictionary(source.Metrics, this.Metrics);
            }

            this.context = source.context.DeepClone();
            this.Sequence = source.Sequence;
            this.Timestamp = source.Timestamp;
            this.samplingPercentage = source.samplingPercentage;
            this.ProactiveSamplingDecision = source.ProactiveSamplingDecision;
            this.successFieldSet = source.successFieldSet;
            this.extension = source.extension?.DeepClone();
            this.Name = source.Name;
            this.Id = source.Id;
            this.ResultCode = source.ResultCode;
            this.Duration = source.Duration;
            this.Success = source.Success;
            this.Data = source.Data;
            this.Target = source.Target;
            this.Type = source.Type;            
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
        string IAiSerializableTelemetry.BaseType => nameof(RemoteDependencyData);

        /// <summary>
        /// Gets or sets date and time when telemetry was recorded.
        /// </summary>
        public override DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the value that defines absolute order of the telemetry item.
        /// </summary>
        public override string Sequence { get; set; }

        /// <summary>
        /// Gets the context associated with the current telemetry item.
        /// </summary>
        public override TelemetryContext Context
        {
            get { return this.context; }
        }

        /// <summary>
        /// Gets or sets gets the extension used to extend this telemetry instance using new strongly typed object.
        /// </summary>
        public override IExtension Extension
        {
            get { return this.extension; }
            set { this.extension = value; }
        }

        /// <summary>
        /// Gets or sets Dependency ID.
        /// </summary>
        public override string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Result Code.
        /// </summary>
        public string ResultCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets resource name.
        /// </summary>
        public override string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets text of SQL command or empty it not applicable.
        /// </summary>
        [Obsolete("Renamed to Data")]
        public string CommandName
        {
            get { return this.Data; }
            set { this.Data = value; }
        }

        /// <summary>
        /// Gets or sets data associated with the current dependency instance. Command name/statement for SQL dependency, URL for http dependency.
        /// </summary>
        public string Data
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets target of dependency call. SQL server name, url host, etc.
        /// </summary>
        public string Target
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the dependency type name.
        /// </summary>
        [Obsolete("Renamed to Type")]
        public string DependencyTypeName
        {
            get { return this.Type;  }
            set { this.Type = value; }
        }

        /// <summary>
        /// Gets or sets the dependency type name.
        /// </summary>
        public string Type
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
        /// Gets or sets a value indicating whether the dependency call was successful or not.
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
        /// Gets a dictionary of application-defined property names and values providing additional information about this remote dependency.
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#properties">Learn more</a>
        /// </summary>
        public override IDictionary<string, string> Properties
        {            
            get
            {
#pragma warning disable CS0618 // Type or member is obsolete
                if (!string.IsNullOrEmpty(this.MetricExtractorInfo) && !this.Context.Properties.ContainsKey(MetricTerms.Extraction.ProcessedByExtractors.Moniker.Key))
                {
                    this.Context.Properties[MetricTerms.Extraction.ProcessedByExtractors.Moniker.Key] = this.MetricExtractorInfo;
                }

                return this.Context.Properties;
#pragma warning restore CS0618 // Type or member is obsolete
            }
        }

        /// <summary>
        /// Gets a dictionary of application-defined event metrics.
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#properties">Learn more</a>
        /// </summary>
        public override IDictionary<string, double> Metrics
        {
            get { return LazyInitializer.EnsureInitialized(ref this.measurementsValue, () => new ConcurrentDictionary<string, double>()); }
        }

        /// <summary>
        /// Gets or sets the dependency kind, like SQL, HTTP, Azure, etc.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use Type")]
        public string DependencyKind
        {
            get
            {
                return this.DependencyTypeName;
            }

            set
            {
                this.DependencyTypeName = value;
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
        public SamplingTelemetryItemTypes ItemTypeFlag => SamplingTelemetryItemTypes.RemoteDependency;

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
        /// Gets the InternalData associated with this Telemetry instance.
        /// This is being served by a singleton instance, so this will
        /// not pickup changes made to the telemetry after first call to this.
        /// It is recommended to make all changes (including sanitization)
        /// to this telemetry before calling InternalData.
        /// </summary>
        internal RemoteDependencyData InternalData
        {
            get
            {
                return LazyInitializer.EnsureInitialized(ref this.internalDataPrivate,
                    () =>
                    {
                        var req = new RemoteDependencyData();
                        req.duration = this.Duration;
                        req.id = this.Id;
                        req.measurements = this.measurementsValue;
                        req.name = this.Name;
                        req.properties = this.context.PropertiesValue;
                        req.resultCode = this.ResultCode;
                        req.target = this.Target;
                        req.success = this.success;
                        req.data = this.Data;
                        req.type = this.Type;
                        return req;
                    });
            }

            private set
            {
                this.internalDataPrivate = value;
            }
        }

        /// <summary>
        /// Deeply clones a <see cref="DependencyTelemetry"/> object.
        /// </summary>
        /// <returns>A cloned instance.</returns>
        public override ITelemetry DeepClone()
        {
            return new DependencyTelemetry(this);
        }

        /// <summary>
        /// In specific collectors, objects are added to the dependency telemetry which may be useful
        /// to enhance DependencyTelemetry telemetry by <see cref="ITelemetryInitializer" /> implementations.
        /// Objects retrieved here are not automatically serialized and sent to the backend.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="detail">When this method returns, contains the object that has the specified key, or the default value of the type if the operation failed.</param>
        /// <returns>true if the key was found; otherwise, false.</returns>
        public bool TryGetOperationDetail(string key, out object detail)
        {
            return this.Context.TryGetRawObject(key, out detail);
        }

        /// <summary>
        /// Sets the operation detail specific against the key specified. Objects set through this method
        /// are not automatically serialized and sent to the backend.
        /// </summary>
        /// <param name="key">The key to store the detail against.</param>
        /// <param name="detail">Detailed information collected by the tracked operation.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetOperationDetail(string key, object detail)
        {
            this.Context.StoreRawObject(key, detail, true);
        }

        /// <inheritdoc/>
        public override void SerializeData(ISerializationWriter serializationWriter)
        {
            // To ensure that all changes to telemetry are reflected in serialization,
            // the underlying field is set to null, which forces it to be re-created.
            this.internalDataPrivate = null;
            serializationWriter.WriteProperty(this.InternalData);            
        }

        /// <summary>
        /// Sanitizes the properties based on constraints.
        /// </summary>
        void ITelemetry.Sanitize()
        {
            this.Name = this.Name.SanitizeName();
            this.Name = Utils.PopulateRequiredStringValue(this.Name, "name", typeof(DependencyTelemetry).FullName);
            this.Id.SanitizeName();
            this.ResultCode = this.ResultCode.SanitizeResultCode();
            this.Type = this.Type.SanitizeDependencyType();
            this.Data = this.Data.SanitizeData();
            this.Properties.SanitizeProperties();
            this.Metrics.SanitizeMeasurements();
        }
    }
}
