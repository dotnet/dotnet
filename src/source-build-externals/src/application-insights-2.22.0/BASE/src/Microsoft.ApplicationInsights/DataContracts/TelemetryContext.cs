namespace Microsoft.ApplicationInsights.DataContracts
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    /// <summary>
    /// Represents a context for sending telemetry to the Application Insights service.
    /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#telemetrycontext">Learn more</a>
    /// </summary>
    public sealed class TelemetryContext
    {
        /// <summary>
        /// Value for the flag that indicates that server should not store IP address from incoming events.
        /// </summary>
        public const long FlagDropIdentifiers = 0x200000;
        internal IDictionary<string, string> GlobalPropertiesValue;
        internal IDictionary<string, string> PropertiesValue;
        private readonly InternalContext internalContext = new InternalContext();
        private string instrumentationKey;

        private IDictionary<string, object> rawObjectsTemp = new Dictionary<string, object>();
        private IDictionary<string, object> rawObjectsPerm = new Dictionary<string, object>();
        private ComponentContext component;
        private DeviceContext device;
        private CloudContext cloud;
        private SessionContext session;
        private UserContext user;
        private OperationContext operation;
        private LocationContext location;

        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryContext"/> class.
        /// </summary>
        public TelemetryContext()
            : this(null, null)
        {
        }

        internal TelemetryContext(IDictionary<string, string> properties)
            : this(properties, null)
        {
        }

        internal TelemetryContext(IDictionary<string, string> properties, IDictionary<string, string> globalProperties)
        {
            this.PropertiesValue = properties;
            this.GlobalPropertiesValue = globalProperties;
        }

        /// <summary>
        /// Gets or sets the default instrumentation key for all <see cref="ITelemetry"/> objects logged in this <see cref="TelemetryContext"/>.
        /// </summary>
        /// <remarks>
        /// By default, this property is initialized with the <see cref="TelemetryConfiguration.InstrumentationKey"/> value
        /// of the <see cref="TelemetryConfiguration.Active"/> instance of <see cref="TelemetryConfiguration"/>. You can specify it
        /// for all telemetry tracked via a particular <see cref="TelemetryClient"/> or for a specific <see cref="ITelemetry"/>
        /// instance.
        /// </remarks>
        public string InstrumentationKey
        {
            get { return this.instrumentationKey ?? string.Empty; }
            set { Property.Set(ref this.instrumentationKey, value); }
        }

        /// <summary> 
        /// Gets or sets flags which controls events priority and endpoint behavior.
        /// </summary> 
        public long Flags { get; set; }

        /// <summary>
        /// Gets the object describing the component tracked by this <see cref="TelemetryContext"/>.
        /// </summary>
        public ComponentContext Component
        {
            get { return LazyInitializer.EnsureInitialized(ref this.component, () => new ComponentContext()); }
        }

        /// <summary>
        /// Gets the object describing the device tracked by this <see cref="TelemetryContext"/>.
        /// </summary>
        public DeviceContext Device
        {
#pragma warning disable CS0618 // Type or member is obsolete
            get { return LazyInitializer.EnsureInitialized(ref this.device, () => new DeviceContext(this.Properties)); }
#pragma warning restore CS0618 // Type or member is obsolete
        }

        /// <summary>
        /// Gets the object describing the cloud tracked by this <see cref="TelemetryContext"/>.
        /// </summary>
        public CloudContext Cloud
        {
            get { return LazyInitializer.EnsureInitialized(ref this.cloud, () => new CloudContext()); }
        }

        /// <summary>
        /// Gets the object describing a user session tracked by this <see cref="TelemetryContext"/>.
        /// </summary>
        public SessionContext Session
        {
            get { return LazyInitializer.EnsureInitialized(ref this.session, () => new SessionContext()); }
        }

        /// <summary>
        /// Gets the object describing a user tracked by this <see cref="TelemetryContext"/>.
        /// </summary>
        public UserContext User
        {
            get { return LazyInitializer.EnsureInitialized(ref this.user, () => new UserContext()); }
        }

        /// <summary>
        /// Gets the object describing a operation tracked by this <see cref="TelemetryContext"/>.
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#operationcontext">Learn more</a>
        /// </summary>
        public OperationContext Operation
        {
            get { return LazyInitializer.EnsureInitialized(ref this.operation, () => new OperationContext()); }
        }

        /// <summary>
        /// Gets the object describing a location tracked by this <see cref="TelemetryContext" />.
        /// </summary>
        public LocationContext Location
        {
            get { return LazyInitializer.EnsureInitialized(ref this.location, () => new LocationContext()); }
        }

        /// <summary>
        /// Gets a dictionary of application-defined property values.
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#properties">Learn more</a>
        /// </summary>        
        [Obsolete("Use GlobalProperties to set global level properties. For properties at item level, use ISupportProperties.Properties.")]
        public IDictionary<string, string> Properties
        {
            get { return LazyInitializer.EnsureInitialized(ref this.PropertiesValue, () => new ConcurrentDictionary<string, string>()); }
        }

        /// <summary>
        /// Gets a dictionary of application-defined property values which are global in scope.
        /// Future SDK versions could serialize this separately from the item level properties.
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#properties">Learn more</a>
        /// </summary>
        public IDictionary<string, string> GlobalProperties
        {
            get { return LazyInitializer.EnsureInitialized(ref this.GlobalPropertiesValue, () => new ConcurrentDictionary<string, string>()); }
        }

        internal InternalContext Internal => this.internalContext;

        /// <summary>
        /// Gets a dictionary of context tags.
        /// </summary>
        internal IDictionary<string, string> SanitizedTags
        {
            get
            {
                var result = new Dictionary<string, string>();
                this.component?.UpdateTags(result);
                this.device?.UpdateTags(result);
                this.cloud?.UpdateTags(result);
                this.session?.UpdateTags(result);
                this.user?.UpdateTags(result);
                this.operation?.UpdateTags(result);
                this.location?.UpdateTags(result);
                this.Internal.UpdateTags(result);
                return result;
            }
        }

        /// <summary>
        /// Returns the raw object with the given key.        
        /// Objects retrieved here are not automatically serialized and sent to the backend.
        /// They are shared (i.e not cloned) if multiple sinks are configured, so sinks should treat them as read-only.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="rawObject">When this method returns, contains the object that has the specified key, or the default value of the type if the operation failed.</param>
        /// <returns>true if the key was found; otherwise, false.</returns>
        /// <remarks>
        /// This method is not thread-safe. Objects should be stored from Collectors or TelemetryInitializers that are run synchronously.
        /// </remarks>        
        public bool TryGetRawObject(string key, out object rawObject)
        {
            if (key == null)
            {
                rawObject = null;
                return false;
            }

            if (this.rawObjectsTemp.TryGetValue(key, out rawObject))
            {
                return true;
            }
            else
            {
                return this.rawObjectsPerm.TryGetValue(key, out rawObject);
            }
        }

        /// <summary>
        /// Stores the raw object against the key specified.
        /// Use this to store raw objects from data collectors so that TelemetryInitializers can access
        /// them to extract additional details to enrich telemetry.
        /// Objects stored through this method are not automatically serialized and sent to the backend.
        /// They are shared (i.e not cloned) if multiple sinks are configured, so sinks should treat them as read-only.
        /// </summary>
        /// <param name="key">The key to store the object against.</param>
        /// <param name="rawObject">Object to be stored.</param>
        /// <param name="keepForInitializationOnly">Boolean flag indicating if this object should be made available only during TelemetryInitializers.
        /// If set to true, then the object will not accessible in TelemetryProcessors and TelemetryChannel.</param>
        /// <remarks>
        /// This method is not thread-safe. Objects should be stored from Collectors or TelemetryInitializers that are run synchronously.
        /// </remarks>
        public void StoreRawObject(string key, object rawObject, bool keepForInitializationOnly = true)
        {
            if (key == null)
            {
                return;
            }

            if (keepForInitializationOnly)
            {
                this.rawObjectsTemp[key] = rawObject;
                this.rawObjectsPerm.Remove(key);
            }
            else
            {
                this.rawObjectsPerm[key] = rawObject;
                this.rawObjectsTemp.Remove(key);
            }
        }

        internal void SanitizeGlobalProperties()
        {
            this.GlobalPropertiesValue?.SanitizeProperties();
        }

        internal void ClearTempRawObjects()
        {
            this.rawObjectsTemp.Clear();
        }

        internal TelemetryContext DeepClone(IDictionary<string, string> properties)
        {
            var newTelemetryContext = new TelemetryContext(properties);
            // This check avoids accessing the public accessor GlobalProperties
            // unless needed, to avoid the penality of ConcurrentDictionary instantiation.
            if (this.GlobalPropertiesValue != null)
            {
                Utils.CopyDictionary(this.GlobalProperties, newTelemetryContext.GlobalProperties);
            }

            if (this.PropertiesValue != null)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                Utils.CopyDictionary(this.Properties, newTelemetryContext.Properties);
#pragma warning restore CS0618 // Type or member is obsolete
            }

            newTelemetryContext.Initialize(this, this.instrumentationKey);

            // RawObject collection is not cloned by design, they share the same collection.
            newTelemetryContext.rawObjectsTemp = this.rawObjectsTemp;
            newTelemetryContext.rawObjectsPerm = this.rawObjectsPerm;
            return newTelemetryContext;
        }

        internal TelemetryContext DeepClone()
        {
            return this.DeepClone(null);
        }

        /// <summary>
        /// Initialize this instance's Context properties with the values from another TelemetryContext.
        /// First check that source is not null, then copy to this instance.
        /// Note that invoking the public getter instead of the private field will call the LazyInitializer.
        /// </summary>
        internal void Initialize(TelemetryContext source, string instrumentationKey)
        {
            this.InitializeInstrumentationkey(instrumentationKey);

            this.Flags |= source.Flags;

            source.component?.CopyTo(this.Component);
            source.device?.CopyTo(this.Device);
            source.cloud?.CopyTo(this.Cloud);
            source.session?.CopyTo(this.Session);
            source.user?.CopyTo(this.User);
            source.operation?.CopyTo(this.Operation);
            source.location?.CopyTo(this.Location);
            source.Internal.CopyTo(this.Internal);
        }

        /// <summary>
        /// Initialize this instance's instrumentation key.
        /// </summary>
        internal void InitializeInstrumentationkey(string instrumentationKey)
        {
            Property.Initialize(ref this.instrumentationKey, instrumentationKey);
        }
    }
}