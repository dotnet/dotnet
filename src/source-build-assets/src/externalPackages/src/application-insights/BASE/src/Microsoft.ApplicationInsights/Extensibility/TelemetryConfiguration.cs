namespace Microsoft.ApplicationInsights.Extensibility
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.ApplicationId;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Authentication;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Endpoints;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Sampling;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.SelfDiagnostics;
    using Microsoft.ApplicationInsights.Metrics;
    using Microsoft.ApplicationInsights.Metrics.Extensibility;

    /// <summary>
    /// Encapsulates the global telemetry configuration typically loaded from the ApplicationInsights.config file.
    /// </summary>
    /// <remarks>
    /// All <see cref="TelemetryContext"/> objects are initialized using the <see cref="Active"/> 
    /// telemetry configuration provided by this class.
    /// </remarks>
    public sealed class TelemetryConfiguration : IDisposable
    {
        internal readonly SamplingRateStore LastKnownSampleRateStore = new SamplingRateStore();

        private static object syncRoot = new object();
        private static TelemetryConfiguration active;

        private readonly SnapshottingList<ITelemetryInitializer> telemetryInitializers = new SnapshottingList<ITelemetryInitializer>();
        private readonly TelemetrySinkCollection telemetrySinks = new TelemetrySinkCollection();

        private TelemetryProcessorChain telemetryProcessorChain;
        private string instrumentationKey = string.Empty;
        private string connectionString;
        private bool disableTelemetry = false;
        private TelemetryProcessorChainBuilder builder;
        private MetricManager metricManager = null;
        private IApplicationIdProvider applicationIdProvider;

        /// <summary>
        /// Indicates if this instance has been disposed of.
        /// </summary>
        private bool isDisposed = false;

        /// <summary>
        /// Static Constructor which sets ActivityID Format to W3C if Format not enforced.
        /// This ensures SDK operates in W3C mode, unless turned off explicitily with the following 2 lines
        /// in user code in application startup.
        /// Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical
        /// Activity.ForceDefaultIdFormat = true.
        /// </summary>
        static TelemetryConfiguration()
        {
            ActivityExtensions.TryRun(() =>
            {
                if (!Activity.ForceDefaultIdFormat)
                {
                    Activity.DefaultIdFormat = ActivityIdFormat.W3C;
                    Activity.ForceDefaultIdFormat = true;
                }
            });
            SelfDiagnosticsInitializer.EnsureInitialized();
        }

        /// <summary>
        /// Initializes a new instance of the TelemetryConfiguration class.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable CS0618 // Type or member is obsolete
        public TelemetryConfiguration() : this(string.Empty, null)
#pragma warning restore CS0618 // Type or member is obsolete
        {
        }

        /// <summary>
        /// Initializes a new instance of the TelemetryConfiguration class.
        /// </summary>
        /// <param name="instrumentationKey">The instrumentation key this configuration instance will provide.</param>
        [Obsolete("InstrumentationKey based global ingestion is being deprecated. Use the default constructor and manually set TelemetryConfiguration.ConnectionString. See https://github.com/microsoft/ApplicationInsights-dotnet/issues/2560 for more details.")]
        public TelemetryConfiguration(string instrumentationKey) : this(instrumentationKey, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the TelemetryConfiguration class.
        /// </summary>
        /// <param name="instrumentationKey">The instrumentation key this configuration instance will provide.</param>
        /// <param name="channel">The telemetry channel to provide with this configuration instance.</param>
        [Obsolete("InstrumentationKey based global ingestion is being deprecated. Use the default constructor and manually set TelemetryConfiguration.ConnectionString. See https://github.com/microsoft/ApplicationInsights-dotnet/issues/2560 for more details.")]
        public TelemetryConfiguration(string instrumentationKey, ITelemetryChannel channel)
        {
            this.instrumentationKey = instrumentationKey ?? throw new ArgumentNullException(nameof(instrumentationKey));

            var ingestionEndpoint = this.EndpointContainer.GetFormattedIngestionEndpoint(enableAAD: this.CredentialEnvelope != null);
            SetTelemetryChannelEndpoint(channel, ingestionEndpoint, force: true);
            var defaultSink = new TelemetrySink(this, channel);
            defaultSink.Name = "default";
            this.telemetrySinks.Add(defaultSink);
        }

        /// <summary>
        /// Gets the active <see cref="TelemetryConfiguration"/> instance loaded from the ApplicationInsights.config file. 
        /// If the configuration file does not exist, the active configuration instance is initialized with minimum defaults 
        /// needed to send telemetry to Application Insights.
        /// </summary>
#if NETSTANDARD // This constant is defined for all versions of NetStandard https://docs.microsoft.com/en-us/dotnet/core/tutorials/libraries#how-to-multitarget
        [Obsolete("We do not recommend using TelemetryConfiguration.Active on .NET Core. See https://github.com/microsoft/ApplicationInsights-dotnet/issues/1152 for more details")]
#endif
        public static TelemetryConfiguration Active
        {
            get
            {
                if (active == null)
                {
                    lock (syncRoot)
                    {
                        if (active == null)
                        {
                            active = new TelemetryConfiguration();
                            TelemetryConfigurationFactory.Instance.Initialize(active, TelemetryModules.Instance);
                        }
                    }
                }

                return active;
            }

            internal set
            {
                lock (syncRoot)
                {
                    active = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the default instrumentation key for the application.
        /// </summary>
        /// <exception cref="ArgumentNullException">The new value is null.</exception>
        /// <remarks>
        /// This instrumentation key value is used by default by all <see cref="TelemetryClient"/> instances
        /// created in the application. This value can be overwritten by setting the <see cref="TelemetryContext.InstrumentationKey"/>
        /// property of the <see cref="TelemetryClient.Context"/>.
        /// </remarks>
        public string InstrumentationKey
        {
            get => this.instrumentationKey;

            [Obsolete("InstrumentationKey based global ingestion is being deprecated. Use TelemetryConfiguration.ConnectionString. See https://github.com/microsoft/ApplicationInsights-dotnet/issues/2560 for more details.")]
            set { this.instrumentationKey = value ?? throw new ArgumentNullException(nameof(this.InstrumentationKey)); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether sending of telemetry to Application Insights is disabled.
        /// </summary>
        /// <remarks>
        /// This disable tracking setting value is used by default by all <see cref="TelemetryClient"/> instances
        /// created in the application. 
        /// </remarks>
        public bool DisableTelemetry
        {
            get
            {
                return this.disableTelemetry;
            }

            set
            {
                // Log the state of tracking 
                if (value)
                {
                    CoreEventSource.Log.TrackingWasDisabled();
                }
                else
                {
                    CoreEventSource.Log.TrackingWasEnabled();
                }

                this.disableTelemetry = value;
            }
        }

        /// <summary>
        /// Gets the list of <see cref="ITelemetryInitializer"/> objects that supply additional information about telemetry.
        /// </summary>
        /// <remarks>
        /// Telemetry initializers extend Application Insights telemetry collection by supplying additional information 
        /// about individual <see cref="ITelemetry"/> items, such as <see cref="ITelemetry.Timestamp"/>. A <see cref="TelemetryClient"/>
        /// invokes telemetry initializers each time <see cref="TelemetryClient.Track"/> method is called.
        /// The default list of telemetry initializers is provided by the Application Insights NuGet packages and loaded from 
        /// the ApplicationInsights.config file located in the application directory. 
        /// </remarks>
        public IList<ITelemetryInitializer> TelemetryInitializers
        {
            get { return this.telemetryInitializers; }
        }

        /// <summary>
        /// Gets a readonly collection of TelemetryProcessors.
        /// </summary>
        public ReadOnlyCollection<ITelemetryProcessor> TelemetryProcessors
        {
            get
            {
                return new ReadOnlyCollection<ITelemetryProcessor>(this.TelemetryProcessorChain.TelemetryProcessors);
            }
        }

        /// <summary>
        /// Gets the TelemetryProcessorChainBuilder which can build and populate TelemetryProcessors in the TelemetryConfiguration.
        /// </summary>
        public TelemetryProcessorChainBuilder TelemetryProcessorChainBuilder
        {
            get
            {
                LazyInitializer.EnsureInitialized(ref this.builder, () => new TelemetryProcessorChainBuilder(this));
                return this.builder;
            }

            internal set
            {
                this.builder = value;
            }
        }

        /// <summary>
        /// Gets or sets the telemetry channel for the default sink. Will also attempt to set the Channel's endpoint.
        /// </summary>
        public ITelemetryChannel TelemetryChannel
        {
            get
            {
                // We do not ensure not disposed here because TelemetryChannel is accessed during configuration disposal.
                return this.telemetrySinks.DefaultSink.TelemetryChannel;
            }

            set
            {
                if (!this.isDisposed)
                {
                    this.telemetrySinks.DefaultSink.TelemetryChannel = value;
                    var ingestionEndpoint = this.EndpointContainer.GetFormattedIngestionEndpoint(enableAAD: this.CredentialEnvelope != null);
                    SetTelemetryChannelEndpoint(this.telemetrySinks.DefaultSink.TelemetryChannel, ingestionEndpoint);
                    SetTelemetryChannelCredentialEnvelope(value, this.CredentialEnvelope);
                }
            }
        }

        /// <summary>
        /// Gets or sets the Application Id Provider.
        /// </summary>
        /// <remarks>
        /// This feature is opt-in and must be configured to be enabled.
        /// </remarks>
        public IApplicationIdProvider ApplicationIdProvider
        {
            get
            {
                return this.applicationIdProvider;
            }

            set
            {
                this.applicationIdProvider = value;
                SetApplicationIdEndpoint(this.applicationIdProvider, this.EndpointContainer.FormattedApplicationIdEndpoint);
            }
        }

        /// <summary>
        /// Gets the Endpoint Container responsible for making service endpoints available.
        /// </summary>
        public EndpointContainer EndpointContainer { get; private set; } = new EndpointContainer(new EndpointProvider());

        /// <summary>
        /// Gets or sets the connection string. Setting this value will also set (and overwrite) the <see cref="InstrumentationKey"/>. The endpoints are validated and will be set (and overwritten) for <see cref="InMemoryChannel"/> and ServerTelemetryChannel as well as the <see cref="ApplicationIdProvider"/>.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return this.connectionString;
            }

            set
            {
                try
                {
                    this.connectionString = value ?? throw new ArgumentNullException(nameof(this.ConnectionString));

                    var endpointProvider = new EndpointProvider
                    {
                        ConnectionString = value,
                    };

#pragma warning disable CS0618 // Type or member is obsolete
                    this.InstrumentationKey = endpointProvider.GetInstrumentationKey();
#pragma warning restore CS0618 // Type or member is obsolete

                    this.EndpointContainer = new EndpointContainer(endpointProvider);

                    // UPDATE TELEMETRY CHANNEL
                    var ingestionEndpoint = this.EndpointContainer.GetFormattedIngestionEndpoint(enableAAD: this.CredentialEnvelope != null);
                    this.SetTelemetryChannelEndpoint(ingestionEndpoint);

                    // UPDATE APPLICATION ID PROVIDER
                    SetApplicationIdEndpoint(this.ApplicationIdProvider, this.EndpointContainer.FormattedApplicationIdEndpoint, force: true);
                }
                catch (Exception ex)
                {
                    CoreEventSource.Log.ConnectionStringSetFailed(ex.ToInvariantString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets a collection of strings indicating if an experimental feature should be enabled.
        /// The presence of a string in this collection will be evaluated as 'true'.
        /// </summary>
        /// <remarks>
        /// This property allows the dev team to ship and evaluate features before adding these to the public API.
        /// We are not committing to support any features enabled through this property.
        /// Use this at your own risk.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IList<string> ExperimentalFeatures { get; } = new List<string>(0);

        /// <summary>
        /// Gets a list of telemetry sinks associated with the configuration.
        /// </summary>
        public IList<TelemetrySink> TelemetrySinks => this.telemetrySinks;

        /// <summary>
        /// Gets the default telemetry sink.
        /// </summary>
        public TelemetrySink DefaultTelemetrySink => this.telemetrySinks.DefaultSink;

        /// <summary>
        /// Gets an envelope for Azure.Core.TokenCredential which provides an AAD Authenticated token.
        /// To set the Credential use <see cref="SetAzureTokenCredential"/>.
        /// </summary>
        internal CredentialEnvelope CredentialEnvelope { get; private set; }

        /// <summary>
        /// Gets or sets the chain of processors.
        /// </summary>
        internal TelemetryProcessorChain TelemetryProcessorChain
        {
            get
            {
                if (this.telemetryProcessorChain == null)
                {
                    this.TelemetryProcessorChainBuilder.Build();
                }

                return this.telemetryProcessorChain;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                this.telemetryProcessorChain = value;
            }
        }

        /// <summary>
        /// Creates a new <see cref="TelemetryConfiguration"/> instance loaded from the ApplicationInsights.config file.
        /// If the configuration file does not exist, the new configuration instance is initialized with minimum defaults 
        /// needed to send telemetry to Application Insights.
        /// </summary>
        public static TelemetryConfiguration CreateDefault()
        {
            var configuration = new TelemetryConfiguration();
            TelemetryConfigurationFactory.Instance.Initialize(configuration, null);

            return configuration;
        }

        /// <summary>
        /// Creates a new <see cref="TelemetryConfiguration"/> instance loaded from the specified configuration.
        /// </summary>
        /// <param name="config">An xml serialized configuration.</param>
        /// <exception cref="ArgumentNullException">Throws if the config value is null or empty.</exception>
        public static TelemetryConfiguration CreateFromConfiguration(string config)
        {
            if (string.IsNullOrWhiteSpace(config))
            {
                throw new ArgumentNullException(nameof(config));
            }

            var configuration = new TelemetryConfiguration();
            TelemetryConfigurationFactory.Instance.Initialize(configuration, null, config);
            return configuration;
        }

        /// <summary>
        /// Releases resources used by the current instance of the <see cref="TelemetryConfiguration"/> class.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Set a TokenCredential for this configuration.
        /// </summary>
        /// <remarks>
        /// For more information on expected types, review the documentation for the Azure.Identity library.
        /// (https://github.com/Azure/azure-sdk-for-net/tree/master/sdk/identity/Azure.Identity).
        /// </remarks>
        /// <param name="tokenCredential">An instance of Azure.Core.TokenCredential.</param>
        /// <exception cref="ArgumentException">An ArgumentException is thrown if the provided object does not inherit Azure.Core.TokenCredential.</exception>
        public void SetAzureTokenCredential(object tokenCredential)
        {
            this.CredentialEnvelope = new ReflectionCredentialEnvelope(tokenCredential);
            this.SetTelemetryChannelCredentialEnvelope();

            // Update Ingestion Endpoint.
            var ingestionEndpoint = this.EndpointContainer.GetFormattedIngestionEndpoint(enableAAD: true);
            this.SetTelemetryChannelEndpoint(ingestionEndpoint);
        }

        internal MetricManager GetMetricManager(bool createIfNotExists)
        {
            MetricManager manager = this.metricManager;
            if (manager == null && createIfNotExists)
            {
                var pipelineAdapter = new ApplicationInsightsTelemetryPipeline(this);
                MetricManager newManager = new MetricManager(pipelineAdapter);
                MetricManager prevManager = Interlocked.CompareExchange(ref this.metricManager, newManager, null);

                if (prevManager == null)
                {
                    manager = newManager;
                }
                else
                {
                    // We just created a new manager that we are not using. Stop is before discarding.
                    Task fireAndForget = newManager.StopDefaultAggregationCycleAsync();
                    manager = prevManager;
                }
            }

            return manager;
        }

        /// <summary>
        /// This will check the ApplicationIdProvider and attempt to set the endpoint.
        /// This only supports our first party providers <see cref="ApplicationInsightsApplicationIdProvider"/> and <see cref="DictionaryApplicationIdProvider"/>.
        /// </summary>
        /// <param name="applicationIdProvider">ApplicationIdProvider to set.</param>
        /// <param name="endpoint">Endpoint value to set.</param>
        /// <param name="force">When the ConnectionString is set, ApplicationId Endpoint should be forced to update. If the ApplicationId has been set separately, we will only set endpoint if it is null.</param>
        private static void SetApplicationIdEndpoint(IApplicationIdProvider applicationIdProvider, string endpoint, bool force = false)
        {
            if (applicationIdProvider != null)
            {
                if (applicationIdProvider is ApplicationInsightsApplicationIdProvider applicationInsightsApplicationIdProvider)
                {
                    if (force || applicationInsightsApplicationIdProvider.ProfileQueryEndpoint == null)
                    {
                        applicationInsightsApplicationIdProvider.ProfileQueryEndpoint = endpoint;
                    }
                }
                else if (applicationIdProvider is DictionaryApplicationIdProvider dictionaryApplicationIdProvider)
                {
                    if (dictionaryApplicationIdProvider.Next is ApplicationInsightsApplicationIdProvider innerApplicationIdProvider)
                    {
                        if (force || innerApplicationIdProvider.ProfileQueryEndpoint == null)
                        {
                            innerApplicationIdProvider.ProfileQueryEndpoint = endpoint;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This will check the TelemetryChannel and attempt to set the endpoint.
        /// This only supports our first party providers <see cref="InMemoryChannel"/> and ServerTelemetryChannel.
        /// </summary>
        /// <param name="channel">TelemetryChannel to set.</param>
        /// <param name="endpoint">Endpoint value to set.</param>
        /// /// <param name="force">When the ConnectionString is set, Channel Endpoint should be forced to update. If the Channel has been set separately, we will only set endpoint if it is null.</param>
        private static void SetTelemetryChannelEndpoint(ITelemetryChannel channel, string endpoint, bool force = false)
        {
            if (channel != null)
            {
                if (channel is InMemoryChannel || channel.GetType().FullName == "Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.ServerTelemetryChannel")
                {
                    if (force || channel.EndpointAddress == null)
                    {
                        channel.EndpointAddress = endpoint;
                    }
                }
            }
        }

        private static void SetTelemetryChannelCredentialEnvelope(ITelemetryChannel telemetryChannel, CredentialEnvelope credentialEnvelope)
        {
            if (telemetryChannel is ISupportCredentialEnvelope tc)
            {
                tc.CredentialEnvelope = credentialEnvelope;
            }
        }

        private void SetTelemetryChannelCredentialEnvelope()
        {
            foreach (var tSink in this.TelemetrySinks)
            {
                SetTelemetryChannelCredentialEnvelope(tSink.TelemetryChannel, this.CredentialEnvelope);
            }
        }

        private void SetTelemetryChannelEndpoint(string ingestionEndpoint)
        {
            foreach (var tSink in this.TelemetrySinks)
            {
                SetTelemetryChannelEndpoint(tSink.TelemetryChannel, ingestionEndpoint, force: true);
            }
        }

        /// <summary>
        /// Disposes of resources.
        /// </summary>
        /// <param name="disposing">Indicates if managed code is being disposed.</param>
        private void Dispose(bool disposing)
        {
            if (!this.isDisposed && disposing)
            {
                this.isDisposed = true;
                Interlocked.CompareExchange(ref active, null, this);

                // I think we should be flushing this.telemetrySinks.DefaultSink.TelemetryChannel at this point.
                // Filed https://github.com/Microsoft/ApplicationInsights-dotnet/issues/823 to track.
                // For now just flushing the metrics:
                this.metricManager?.Flush();

                if (this.telemetryProcessorChain != null)
                {
                    // Not setting this.telemetryProcessorChain to null because calls to the property getter would reinitialize it.
                    this.telemetryProcessorChain.Dispose();
                }

                foreach (TelemetrySink sink in this.telemetrySinks)
                {
                    sink.Dispose();
                    if (!object.ReferenceEquals(sink, this.telemetrySinks.DefaultSink))
                    {
                        this.telemetrySinks.Remove(sink);
                    }
                }
            }
        }
    }
}
