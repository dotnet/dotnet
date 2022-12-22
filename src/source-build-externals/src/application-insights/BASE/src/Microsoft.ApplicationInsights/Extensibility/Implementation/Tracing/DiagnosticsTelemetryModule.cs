namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using System.Linq;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule;

    /// <summary>
    /// Use diagnostics telemetry module to report SDK internal problems to the portal and VS debug output window.
    /// </summary>
    public sealed class DiagnosticsTelemetryModule : ITelemetryModule, IHeartbeatPropertyManager, IDisposable
    {
        internal readonly IList<IDiagnosticsSender> Senders = new List<IDiagnosticsSender>();
        internal readonly DiagnosticsListener EventListener;
        internal readonly IHeartbeatProvider HeartbeatProvider = null;
        
        private readonly object lockObject = new object();
        private readonly IDiagnoisticsEventThrottlingScheduler throttlingScheduler = new DiagnoisticsEventThrottlingScheduler();
        private volatile bool disposed = false;
        private string instrumentationKey;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticsTelemetryModule"/> class. 
        /// </summary>
        public DiagnosticsTelemetryModule()
        {
            // Adding a dummy queue sender to keep the data to be sent to the portal before the initialize method is called
            this.Senders.Add(new PortalDiagnosticsQueueSender());

            this.EventListener = new DiagnosticsListener(this.Senders);

            this.HeartbeatProvider = new HeartbeatProvider();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="DiagnosticsTelemetryModule" /> class.
        /// </summary>
        ~DiagnosticsTelemetryModule() => this.Dispose(false);

        /// <summary>
        /// Gets or sets a value indicating whether or not the Heartbeat feature is disabled.
        /// </summary>
        public bool IsHeartbeatEnabled
        {
            get => this.HeartbeatProvider.IsHeartbeatEnabled;
            set => this.HeartbeatProvider.IsHeartbeatEnabled = value;
        }

        /// <summary>
        /// Gets or sets the delay interval between heartbeats. Setting this value will immediately reset the heartbeat timer.
        /// 
        /// <remarks>
        /// Note that there is a minimum interval <see cref="HeartbeatProvider.MinimumHeartbeatInterval"/> and if an 
        /// attempt to make the interval less than this minimum value is detected, the interval rate will be set to 
        /// the minimum. 
        /// Also note, if the interval is set to any value less than the current channel flush rate, the heartbeat may 
        /// not be emitted at expected times. (The heartbeat will still be sent, but after having been cached for a 
        /// time first).
        /// </remarks>
        /// </summary>
        public TimeSpan HeartbeatInterval
        {
            get => this.HeartbeatProvider.HeartbeatInterval;
            set => this.HeartbeatProvider.HeartbeatInterval = value;
        }

        /// <summary>
        /// Gets a list of default heartbeat property providers that are disabled and will not contribute to the
        /// default heartbeat properties. The only default heartbeat property provide currently defined is named
        /// 'Base'.
        /// </summary>
        public IList<string> ExcludedHeartbeatPropertyProviders => this.HeartbeatProvider.ExcludedHeartbeatPropertyProviders;

        /// <summary>
        /// Gets a list of property names that are not to be sent with the heartbeats. null/empty list means allow all default properties through.
        /// 
        /// <remarks>
        /// Default properties supplied by the Application Insights SDK:
        /// baseSdkTargetFramework, osType, processSessionId
        /// </remarks>
        /// </summary>
        public IList<string> ExcludedHeartbeatProperties => this.HeartbeatProvider.ExcludedHeartbeatProperties;

        /// <summary>
        /// Gets or sets diagnostics Telemetry Module LogLevel configuration setting. 
        /// Possible values LogAlways, Critical, Error, Warning, Informational and Verbose.
        /// </summary>
        public string Severity
        {
            get => this.EventListener.LogLevel.ToString();
            set => this.EventListener.SetLogLevel(value);
        }

        /// <summary>
        /// Gets or sets instrumentation key for diagnostics. Use to redirect SDK 
        /// internal problems reporting to the separate instrumentation key.
        /// </summary>
        public string DiagnosticsInstrumentationKey
        {
            get
            {
                return this.instrumentationKey;
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.instrumentationKey = value;

                    // Set instrumentation key in Portal sender
                    foreach (var portalSender in this.Senders.OfType<PortalDiagnosticsSender>())
                    {
                        portalSender.DiagnosticsInstrumentationKey = this.instrumentationKey;
                    }

                    this.HeartbeatProvider.InstrumentationKey = this.instrumentationKey;
                }
            }
        }

        /// <summary>Gets a value indicating whether this module has been initialized.</summary>
        internal bool IsInitialized { get; private set; } = false;

        /// <summary>
        /// Initializes this telemetry module.
        /// </summary>
        /// <param name="configuration">Telemetry configuration to use for this telemetry module.</param>
        public void Initialize(TelemetryConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            // Temporary fix to make sure that we initialize module once.
            // It should be removed when configuration reading logic is moved to Web SDK.
            if (!this.IsInitialized)
            {
                lock (this.lockObject)
                {
                    if (!this.IsInitialized)
                    {
                        // Swap out the PortalDiagnosticsQueueSender for the PortalDiagnosticsSender
                        var queueSender = this.Senders.OfType<PortalDiagnosticsQueueSender>().First();
                        queueSender.IsDisabled = true;
                        this.Senders.Remove(queueSender);

                        PortalDiagnosticsSender portalSender = new PortalDiagnosticsSender(
                            configuration,
                            new DiagnoisticsEventThrottlingManager<DiagnoisticsEventThrottling>(
                                new DiagnoisticsEventThrottling(DiagnoisticsEventThrottlingDefaults.DefaultThrottleAfterCount),
                                this.throttlingScheduler,
                                DiagnoisticsEventThrottlingDefaults.DefaultThrottlingRecycleIntervalInMinutes))
                        {
                            DiagnosticsInstrumentationKey = this.DiagnosticsInstrumentationKey,
                        };

                        this.Senders.Add(portalSender);

                        queueSender.FlushQueue(portalSender);

                        // set up heartbeat
                        this.HeartbeatProvider.Initialize(configuration);

                        this.IsInitialized = true;
                    }
                }
            }
        }

        /// <summary>
        /// Add a new Heartbeat property to the payload sent with each heartbeat.
        /// 
        /// To update the value of the property you are adding to the heartbeat, 
        /// <see cref="DiagnosticsTelemetryModule.SetHeartbeatProperty"/>.
        /// 
        /// Note that you cannot add a HeartbeatProperty with a name that already exists in the Heartbeat
        /// payload, including (but not limited to) the name of SDK-default items.
        /// 
        /// </summary>
        /// <param name="propertyName">Name of the heartbeat value to add.</param>
        /// <param name="propertyValue">Current value of the heartbeat value to add.</param>
        /// <param name="isHealthy">Flag indicating whether or not the property represents a healthy value.</param>
        /// <returns>True if the new payload item is successfully added, false otherwise.</returns>
        public bool AddHeartbeatProperty(string propertyName, string propertyValue, bool isHealthy)
        {
            return this.HeartbeatProvider.AddHeartbeatProperty(propertyName, false, propertyValue, isHealthy);
        }

        /// <summary>
        /// Set an updated value into an existing property of the heartbeat. The propertyName must be non-null and non-empty
        /// and at least one of the propertyValue and isHealthy parameters must be non-null.
        /// 
        /// After the new HeartbeatProperty has been added (<see cref="DiagnosticsTelemetryModule.AddHeartbeatProperty"/>) to the 
        /// heartbeat payload, the value represented by that item can be updated using this method at any time.
        /// 
        /// </summary>
        /// <param name="propertyName">Name of the heartbeat payload item property to set the value and/or its health status.</param>
        /// <param name="propertyValue">Value of the heartbeat payload item. If this is null, the current value in the item is left unchanged.</param>
        /// <param name="isHealthy">Health status of the heartbeat payload item. If this is set to null the health status is left unchanged.</param>
        /// <returns>True if the payload provider was added, false if it hasn't been added yet 
        /// (<see cref="DiagnosticsTelemetryModule.AddHeartbeatProperty"/>).</returns>
        public bool SetHeartbeatProperty(string propertyName, string propertyValue = null, bool? isHealthy = null)
        {
            if (!string.IsNullOrEmpty(propertyName) && (propertyValue != null || isHealthy != null))
            {
                return this.HeartbeatProvider.SetHeartbeatProperty(propertyName, false, propertyValue, isHealthy);
            }
            else
            {
                CoreEventSource.Log.LogVerbose("Did not set a valid heartbeat property. Ensure you set a valid propertyName and one or both of the propertyValue and isHealthy parameters.");
            }

            return false;
        }

        /// <summary>
        /// Disposes this object.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of resources.
        /// </summary>
        /// <param name="managed">Indicates if managed code is being disposed.</param>
        private void Dispose(bool managed)
        {
            if (managed && !this.disposed)
            {
                this.EventListener.Dispose();
                (this.throttlingScheduler as IDisposable).Dispose();
                foreach (var disposableSender in this.Senders.OfType<IDisposable>())
                {
                    disposableSender.Dispose();
                }

                this.HeartbeatProvider.Dispose();
            }

            this.disposed = true;
        }
    }
}
