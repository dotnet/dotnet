namespace Microsoft.ApplicationInsights
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Platform;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Metrics;
    using Microsoft.ApplicationInsights.Metrics.Extensibility;

    /// <summary>
    /// Send events, metrics and other telemetry to the Application Insights service.
    /// <a href="https://go.microsoft.com/fwlink/?linkid=525722">Learn more</a>
    /// </summary>
    public sealed class TelemetryClient
    {
#if NETSTANDARD // This constant is defined for all versions of NetStandard https://docs.microsoft.com/en-us/dotnet/core/tutorials/libraries#how-to-multitarget
        private const string VersionPrefix = "dotnetc:";
#else
        private const string VersionPrefix = "dotnet:";
#endif  
        private readonly TelemetryConfiguration configuration;

        private string sdkVersion;

#pragma warning disable 612, 618 // TelemetryConfiguration.Active
        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryClient" /> class. Send telemetry with the active configuration, usually loaded from ApplicationInsights.config.
        /// </summary>
#if NETSTANDARD // This constant is defined for all versions of NetStandard https://docs.microsoft.com/en-us/dotnet/core/tutorials/libraries#how-to-multitarget
        [Obsolete("We do not recommend using TelemetryConfiguration.Active on .NET Core. See https://github.com/microsoft/ApplicationInsights-dotnet/issues/1152 for more details")]
#endif
        public TelemetryClient() : this(TelemetryConfiguration.Active)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryClient" /> class. Send telemetry with the specified <paramref name="configuration"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException">The <paramref name="configuration"/> is null.</exception>
        /// <exception cref="ArgumentException">The <paramref name="configuration"/> does not contain a telemetry channel.</exception>
        public TelemetryClient(TelemetryConfiguration configuration)
        {
            if (configuration == null)
            {
                CoreEventSource.Log.TelemetryClientConstructorWithNoTelemetryConfiguration();
                configuration = TelemetryConfiguration.Active;
            }

            this.configuration = configuration;

            if (this.configuration.TelemetryChannel == null)
            {
                throw new ArgumentException("The specified configuration does not have a telemetry channel.", nameof(configuration));
            }
        }
#pragma warning restore 612, 618 // TelemetryConfiguration.Active

        /// <summary>
        /// Gets the current context that will be used to augment telemetry you send.
        /// </summary>
        public TelemetryContext Context
        {
            get;
            internal set;
        }

        = new TelemetryContext();

        /// <summary>
        /// Gets or sets the default instrumentation key for all <see cref="ITelemetry"/> objects logged in this <see cref="TelemetryClient"/>.
        /// </summary>
        public string InstrumentationKey
        {
            get => this.Context.InstrumentationKey;

            [Obsolete("InstrumentationKey based global ingestion is being deprecated. Recommended to set TelemetryConfiguration.ConnectionString. See https://github.com/microsoft/ApplicationInsights-dotnet/issues/2560 for more details.")]
            set { this.Context.InstrumentationKey = value; }
        }

        /// <summary>
        /// Gets the <see cref="TelemetryConfiguration"/> object associated with this telemetry client instance.
        /// Changes made to the configuration can affect other clients.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TelemetryConfiguration TelemetryConfiguration
        {
            get { return this.configuration; }
        }

        /// <summary>
        /// Check to determine if the tracking is enabled.
        /// </summary>
        public bool IsEnabled()
        {
            return !this.configuration.DisableTelemetry;
        }

        /// <summary>
        /// Send an <see cref="EventTelemetry"/> for display in Diagnostic Search and in the Analytics Portal.
        /// </summary>
        /// <remarks>
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#trackevent">Learn more</a>
        /// </remarks>
        /// <param name="eventName">A name for the event.</param>
        /// <param name="properties">Named string values you can use to search and classify events.</param>
        /// <param name="metrics">Measurements associated with this event.</param>
        public void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            var telemetry = new EventTelemetry(eventName);

            if (properties != null && properties.Count > 0)
            {
                Utils.CopyDictionary(properties, telemetry.Properties);
            }

            if (metrics != null && metrics.Count > 0)
            {
                Utils.CopyDictionary(metrics, telemetry.Metrics);
            }

            this.TrackEvent(telemetry);
        }

        /// <summary>
        /// Send an <see cref="EventTelemetry"/> for display in Diagnostic Search and in the Analytics Portal.
        /// Create a separate <see cref="EventTelemetry"/> instance for each call to <see cref="TrackEvent(EventTelemetry)"/>.
        /// </summary>
        /// <remarks>
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#trackevent">Learn more</a>
        /// </remarks>
        /// <param name="telemetry">An event log item.</param>
        public void TrackEvent(EventTelemetry telemetry)
        {
            if (telemetry == null)
            {
                telemetry = new EventTelemetry();
            }

            this.Track(telemetry);
        }

        /// <summary>
        /// Send a trace message for display in Diagnostic Search.
        /// </summary>
        /// <remarks>
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#tracktrace">Learn more</a>
        /// </remarks>
        /// <param name="message">Message to display.</param>
        public void TrackTrace(string message)
        {
            this.TrackTrace(new TraceTelemetry(message));
        }

        /// <summary>
        /// Send a trace message for display in Diagnostic Search.
        /// </summary>
        /// <remarks>
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#tracktrace">Learn more</a>
        /// </remarks>
        /// <param name="message">Message to display.</param>
        /// <param name="severityLevel">Trace severity level.</param>
        public void TrackTrace(string message, SeverityLevel severityLevel)
        {
            this.TrackTrace(new TraceTelemetry(message, severityLevel));
        }

        /// <summary>
        /// Send a trace message for display in Diagnostic Search.
        /// </summary>
        /// <remarks>
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#tracktrace">Learn more</a>
        /// </remarks>
        /// <param name="message">Message to display.</param>
        /// <param name="properties">Named string values you can use to search and classify events.</param>
        public void TrackTrace(string message, IDictionary<string, string> properties)
        {
            TraceTelemetry telemetry = new TraceTelemetry(message);

            if (properties != null && properties.Count > 0)
            {
                Utils.CopyDictionary(properties, telemetry.Properties);
            }

            this.TrackTrace(telemetry);
        }

        /// <summary>
        /// Send a trace message for display in Diagnostic Search.
        /// </summary>
        /// <remarks>
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#tracktrace">Learn more</a>
        /// </remarks>
        /// <param name="message">Message to display.</param>
        /// <param name="severityLevel">Trace severity level.</param>
        /// <param name="properties">Named string values you can use to search and classify events.</param>
        public void TrackTrace(string message, SeverityLevel severityLevel, IDictionary<string, string> properties)
        {
            TraceTelemetry telemetry = new TraceTelemetry(message, severityLevel);

            if (properties != null && properties.Count > 0)
            {
                Utils.CopyDictionary(properties, telemetry.Properties);
            }

            this.TrackTrace(telemetry);
        }

        /// <summary>
        /// Send a trace message for display in Diagnostic Search.
        /// Create a separate <see cref="TraceTelemetry"/> instance for each call to <see cref="TrackTrace(TraceTelemetry)"/>.
        /// </summary>
        /// <remarks>
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#tracktrace">Learn more</a>
        /// </remarks>
        /// <param name="telemetry">Message with optional properties.</param>
        public void TrackTrace(TraceTelemetry telemetry)
        {
            telemetry = telemetry ?? new TraceTelemetry();
            this.Track(telemetry);
        }

        /// <summary>
        /// This method is not the preferred method for sending metrics.
        /// Metrics should always be pre-aggregated across a time period before being sent.<br />
        /// Use one of the <c>GetMetric(..)</c> overloads to get a metric object for accessing SDK pre-aggregation capabilities.<br />
        /// If you are implementing your own pre-aggregation logic, then you can use this method.
        /// If your application requires sending a separate telemetry item at every occasion without aggregation across time,
        /// you likely have a use case for event telemetry; see <see cref="TrackEvent(EventTelemetry)"/>.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="value">Metric value.</param>
        /// <param name="properties">Named string values you can use to classify and filter metrics.</param>        
        public void TrackMetric(string name, double value, IDictionary<string, string> properties = null)
        {
            var telemetry = new MetricTelemetry(name, value);
            if (properties != null && properties.Count > 0)
            {
                Utils.CopyDictionary(properties, telemetry.Properties);
            }

            this.TrackMetric(telemetry);
        }

        /// <summary>
        /// This method is not the preferred method for sending metrics.
        /// Metrics should always be pre-aggregated across a time period before being sent.<br />
        /// Use one of the <c>GetMetric(..)</c> overloads to get a metric object for accessing SDK pre-aggregation capabilities.<br />
        /// If you are implementing your own pre-aggregation logic, then you can use this method.
        /// If your application requires sending a separate telemetry item at every occasion without aggregation across time,
        /// you likely have a use case for event telemetry; see <see cref="TrackEvent(EventTelemetry)"/>.
        /// </summary>
        /// <param name="telemetry">The metric telemetry item.</param>        
        public void TrackMetric(MetricTelemetry telemetry)
        {
            if (telemetry == null)
            {
                telemetry = new MetricTelemetry();
            }

            this.Track(telemetry);
        }

        /// <summary>
        /// Send an <see cref="ExceptionTelemetry"/> for display in Diagnostic Search.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="properties">Named string values you can use to classify and search for this exception.</param>
        /// <param name="metrics">Additional values associated with this exception.</param>
        /// <remarks>
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#trackexception">Learn more</a>
        /// </remarks>
        public void TrackException(Exception exception, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            if (exception == null)
            {
                exception = new Exception(Utils.PopulateRequiredStringValue(null, "message", typeof(ExceptionTelemetry).FullName));
            }

            var telemetry = new ExceptionTelemetry(exception);

            if (properties != null && properties.Count > 0)
            {
                Utils.CopyDictionary(properties, telemetry.Properties);
            }

            if (metrics != null && metrics.Count > 0)
            {
                Utils.CopyDictionary(metrics, telemetry.Metrics);
            }

            this.TrackException(telemetry);
        }

        /// <summary>
        /// Send an <see cref="ExceptionTelemetry"/> for display in Diagnostic Search.
        /// Create a separate <see cref="ExceptionTelemetry"/> instance for each call to <see cref="TrackException(ExceptionTelemetry)"/>.
        /// </summary>
        /// <remarks>
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#trackexception">Learn more</a>
        /// </remarks>
        public void TrackException(ExceptionTelemetry telemetry)
        {
            if (telemetry == null)
            {
                var exception = new Exception(Utils.PopulateRequiredStringValue(null, "message", typeof(ExceptionTelemetry).FullName));
                telemetry = new ExceptionTelemetry(exception);
            }

            this.Track(telemetry);
        }

        /// <summary>
        /// Send information about an external dependency (outgoing call) in the application.
        /// </summary>
        /// <param name="dependencyName">Name of the command initiated with this dependency call. Low cardinality value. Examples are stored procedure name and URL path template.</param>
        /// <param name="data">Command initiated by this dependency call. Examples are SQL statement and HTTP URL's with all query parameters.</param>
        /// <param name="startTime">The time when the dependency was called.</param>
        /// <param name="duration">The time taken by the external dependency to handle the call.</param>
        /// <param name="success">True if the dependency call was handled successfully.</param>
        /// <remarks>
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#trackdependency">Learn more</a>
        /// </remarks>
        [Obsolete("Please use a different overload of TrackDependency")]
        public void TrackDependency(string dependencyName, string data, DateTimeOffset startTime, TimeSpan duration, bool success)
        {
#pragma warning disable 618
            this.TrackDependency(new DependencyTelemetry(dependencyName, data, startTime, duration, success));
#pragma warning restore 618
        }

        /// <summary>
        /// Send information about an external dependency (outgoing call) in the application.
        /// </summary>
        /// <param name="dependencyTypeName">External dependency type. Very low cardinality value for logical grouping and interpretation of fields. Examples are SQL, Azure table, and HTTP.</param>
        /// <param name="dependencyName">Name of the command initiated with this dependency call. Low cardinality value. Examples are stored procedure name and URL path template.</param>
        /// <param name="data">Command initiated by this dependency call. Examples are SQL statement and HTTP URL's with all query parameters.</param>
        /// <param name="startTime">The time when the dependency was called.</param>
        /// <param name="duration">The time taken by the external dependency to handle the call.</param>
        /// <param name="success">True if the dependency call was handled successfully.</param>
        /// <remarks>
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#trackdependency">Learn more</a>
        /// </remarks>
        public void TrackDependency(string dependencyTypeName, string dependencyName, string data, DateTimeOffset startTime, TimeSpan duration, bool success)
        {
            this.TrackDependency(new DependencyTelemetry(dependencyTypeName, null, dependencyName, data, startTime, duration, null, success));
        }

        /// <summary>
        /// Send information about an external dependency (outgoing call) in the application.
        /// </summary>
        /// <param name="dependencyTypeName">External dependency type. Very low cardinality value for logical grouping and interpretation of fields. Examples are SQL, Azure table, and HTTP.</param>
        /// <param name="target">External dependency target.</param>
        /// <param name="dependencyName">Name of the command initiated with this dependency call. Low cardinality value. Examples are stored procedure name and URL path template.</param>
        /// <param name="data">Command initiated by this dependency call. Examples are SQL statement and HTTP URL's with all query parameters.</param>
        /// <param name="startTime">The time when the dependency was called.</param>
        /// <param name="duration">The time taken by the external dependency to handle the call.</param>
        /// <param name="resultCode">Result code of dependency call execution.</param>
        /// <param name="success">True if the dependency call was handled successfully.</param>
        /// <remarks>
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#trackdependency">Learn more</a>
        /// </remarks>
        public void TrackDependency(string dependencyTypeName, string target, string dependencyName, string data, DateTimeOffset startTime, TimeSpan duration, string resultCode, bool success)
        {
            this.TrackDependency(new DependencyTelemetry(dependencyTypeName, target, dependencyName, data, startTime, duration, resultCode, success));
        }

        /// <summary>
        /// Send information about external dependency call in the application.
        /// Create a separate <see cref="DependencyTelemetry"/> instance for each call to <see cref="TrackDependency(DependencyTelemetry)"/>.
        /// </summary>
        /// <remarks>
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#trackdependency">Learn more</a>
        /// </remarks>
        public void TrackDependency(DependencyTelemetry telemetry)
        {
            if (telemetry == null)
            {
                telemetry = new DependencyTelemetry();
            }

            this.Track(telemetry);
        }

        /// <summary>
        /// Send information about availability of an application.
        /// </summary>
        /// <param name="name">Availability test name.</param>
        /// <param name="timeStamp">The time when the availability was captured.</param>
        /// <param name="duration">The time taken for the availability test to run.</param>
        /// <param name="runLocation">Name of the location the availability test was run from.</param>
        /// <param name="success">True if the availability test ran successfully.</param>
        /// <param name="message">Error message on availability test run failure.</param>
        /// <param name="properties">Named string values you can use to classify and search for this availability telemetry.</param>
        /// <param name="metrics">Additional values associated with this availability telemetry.</param>
        /// <remarks>
        /// <a href="https://go.microsoft.com/fwlink/?linkid=517889">Learn more</a>
        /// </remarks>
        public void TrackAvailability(string name, DateTimeOffset timeStamp, TimeSpan duration, string runLocation, bool success, string message = null, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            var availabilityTelemetry = new AvailabilityTelemetry(name, timeStamp, duration, runLocation, success, message);

            if (properties != null && properties.Count > 0)
            {
                Utils.CopyDictionary(properties, availabilityTelemetry.Properties);
            }

            if (metrics != null && metrics.Count > 0)
            {
                Utils.CopyDictionary(metrics, availabilityTelemetry.Metrics);
            }

            this.TrackAvailability(availabilityTelemetry);
        }

        /// <summary>
        /// Send information about availability of an application.
        /// Create a separate <see cref="AvailabilityTelemetry"/> instance for each call to <see cref="TrackAvailability(AvailabilityTelemetry)"/>.
        /// </summary>
        /// <remarks>
        /// <a href="https://go.microsoft.com/fwlink/?linkid=517889">Learn more</a>
        /// </remarks>
        public void TrackAvailability(AvailabilityTelemetry telemetry)
        {
            if (telemetry == null)
            {
                telemetry = new AvailabilityTelemetry();
            }

            this.Track(telemetry);
        }

        /// <summary>
        /// This method is an internal part of Application Insights infrastructure. Do not call.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Track(ITelemetry telemetry)
        {
            if (telemetry == null)
            {
                throw new ArgumentNullException(nameof(telemetry));
            }

            // TALK TO YOUR TEAM MATES BEFORE CHANGING THIS.
            // This method needs to be public so that we can build and ship new telemetry types without having to ship core.
            // It is hidden from intellisense to prevent customer confusion.
            if (this.IsEnabled())
            {
                this.Initialize(telemetry);

                telemetry.Context.ClearTempRawObjects();

                // invokes the Process in the first processor in the chain
                this.configuration.TelemetryProcessorChain.Process(telemetry);

                // logs rich payload ETW event for any partners to process it
                RichPayloadEventSource.Log.Process(telemetry);
            }
        }

        /// <summary>
        /// This method is an internal part of Application Insights infrastructure. Do not call.
        /// </summary>
        /// <param name="telemetry">Telemetry item to initialize instrumentation key.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void InitializeInstrumentationKey(ITelemetry telemetry)
        {
            if (telemetry == null)
            {
                throw new ArgumentNullException(nameof(telemetry));
            }

            string instrumentationKey = this.Context.InstrumentationKey;

            if (string.IsNullOrEmpty(instrumentationKey))
            {
                instrumentationKey = this.configuration.InstrumentationKey;
            }

            telemetry.Context.InitializeInstrumentationkey(instrumentationKey);
        }

        /// <summary>
        /// This method is an internal part of Application Insights infrastructure. Do not call.
        /// </summary>
        /// <param name="telemetry">Telemetry item to initialize.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Initialize(ITelemetry telemetry)
        {
            if (telemetry == null)
            {
                throw new ArgumentNullException(nameof(telemetry));
            }

            ISupportAdvancedSampling telemetryWithSampling = telemetry as ISupportAdvancedSampling;

            // Telemetry can be already sampled out if that decision was made before calling Track()
            bool sampledOut = false;
            if (telemetryWithSampling != null)
            {
                sampledOut = telemetryWithSampling.ProactiveSamplingDecision == SamplingDecision.SampledOut;
            }

            if (!sampledOut)
            {
                if (telemetry is ISupportProperties telemetryWithProperties)
                {
                    if (this.configuration.TelemetryChannel?.DeveloperMode != null && this.configuration.TelemetryChannel.DeveloperMode.Value)
                    {
                        if (!telemetryWithProperties.Properties.ContainsKey("DeveloperMode"))
                        {
                            telemetryWithProperties.Properties.Add("DeveloperMode", "true");
                        }
                    }
                }

                // Properties set of TelemetryClient's Context are copied over to that of ITelemetry's Context
#pragma warning disable CS0618 // Type or member is obsolete
                if (this.Context.PropertiesValue != null)
                {
                    Utils.CopyDictionary(this.Context.Properties, telemetry.Context.Properties);
                }

#pragma warning restore CS0618 // Type or member is obsolete

                // This check avoids accessing the public accessor GlobalProperties
                // unless needed, to avoid the penalty of ConcurrentDictionary instantiation.
                if (this.Context.GlobalPropertiesValue != null)
                {
                    Utils.CopyDictionary(this.Context.GlobalProperties, telemetry.Context.GlobalProperties);
                }

                string instrumentationKey = this.Context.InstrumentationKey;

                if (string.IsNullOrEmpty(instrumentationKey))
                {
                    instrumentationKey = this.configuration.InstrumentationKey;
                }

                telemetry.Context.Initialize(this.Context, instrumentationKey);

                for (int index = 0; index < this.configuration.TelemetryInitializers.Count; index++)
                {
                    try
                    {
                        this.configuration.TelemetryInitializers[index].Initialize(telemetry);
                    }
                    catch (Exception exception)
                    {
                        CoreEventSource.Log.LogError(string.Format(
                                                        CultureInfo.InvariantCulture,
                                                        "Exception while initializing {0}, exception message - {1}",
                                                        this.configuration.TelemetryInitializers[index].GetType().FullName,
                                                        exception));
                    }
                }

                if (telemetry.Timestamp == default(DateTimeOffset))
                {
                    telemetry.Timestamp = PreciseTimestamp.GetUtcNow();
                }

                // Currently backend requires SDK version to comply "name: version"
                if (string.IsNullOrEmpty(telemetry.Context.Internal.SdkVersion))
                {
                    var version = this.sdkVersion ?? (this.sdkVersion = SdkVersionUtils.GetSdkVersion(VersionPrefix));
                    telemetry.Context.Internal.SdkVersion = version;
                }

                // set NodeName to the machine name if it's not initialized yet, if RoleInstance is also not set then we send only RoleInstance
                if (string.IsNullOrEmpty(telemetry.Context.Internal.NodeName) && !string.IsNullOrEmpty(telemetry.Context.Cloud.RoleInstance))
                {
                    telemetry.Context.Internal.NodeName = PlatformSingleton.Current.GetMachineName();
                }

                // set RoleInstance to the machine name if it's not initialized yet
                if (string.IsNullOrEmpty(telemetry.Context.Cloud.RoleInstance))
                {
                    telemetry.Context.Cloud.RoleInstance = PlatformSingleton.Current.GetMachineName();
                }
            }
            else
            {
                CoreEventSource.Log.InitializationIsSkippedForSampledItem();
            }
        }

        /// <summary>
        /// Send information about the page viewed in the application.
        /// </summary>
        /// <param name="name">Name of the page.</param>
        /// <remarks>
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#page-views">Learn more</a>
        /// </remarks>
        public void TrackPageView(string name)
        {
            this.Track(new PageViewTelemetry(name));
        }

        /// <summary>
        /// Send information about the page viewed in the application.
        /// Create a separate <see cref="PageViewTelemetry"/> instance for each call to <see cref="TrackPageView(PageViewTelemetry)"/>.
        /// </summary>
        /// <remarks>
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#page-views">Learn more</a>
        /// </remarks>
        public void TrackPageView(PageViewTelemetry telemetry)
        {
            if (telemetry == null)
            {
                telemetry = new PageViewTelemetry();
            }

            this.Track(telemetry);
        }

        /// <summary>
        /// Send information about a request handled by the application.
        /// </summary>
        /// <param name="name">The request name.</param>
        /// <param name="startTime">The time when the page was requested.</param>
        /// <param name="duration">The time taken by the application to handle the request.</param>
        /// <param name="responseCode">The response status code.</param>
        /// <param name="success">True if the request was handled successfully by the application.</param>
        /// <remarks>
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#trackrequest">Learn more</a>
        /// </remarks>
        public void TrackRequest(string name, DateTimeOffset startTime, TimeSpan duration, string responseCode, bool success)
        {
            this.Track(new RequestTelemetry(name, startTime, duration, responseCode, success));
        }

        /// <summary>
        /// Send information about a request handled by the application.
        /// Create a separate <see cref="RequestTelemetry"/> instance for each call to <see cref="TrackRequest(RequestTelemetry)"/>.
        /// </summary>
        /// <remarks>
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#trackrequest">Learn more</a>
        /// </remarks>
        public void TrackRequest(RequestTelemetry request)
        {
            if (request == null)
            {
                request = new RequestTelemetry();
            }

            this.Track(request);
        }

        /// <summary>
        /// Flushes the in-memory buffer and any metrics being pre-aggregated.
        /// </summary>
        /// <remarks>
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#flushing-data">Learn more</a>
        /// </remarks>
        public void Flush()
        {
            CoreEventSource.Log.TelemetlyClientFlush();

            if (this.TryGetMetricManager(out MetricManager privateMetricManager))
            {
                privateMetricManager.Flush(flushDownstreamPipeline: false);
            }

            TelemetryConfiguration pipeline = this.configuration;
            if (pipeline != null)
            {
                MetricManager sharedMetricManager = pipeline.GetMetricManager(createIfNotExists: false);
                sharedMetricManager?.Flush(flushDownstreamPipeline: false);

                ITelemetryChannel channel = pipeline.TelemetryChannel;
                channel?.Flush();
            }
        }

        /// <summary>
        /// Asynchronously Flushes the in-memory buffer and any metrics being pre-aggregated.
        /// </summary>
        /// <returns>
        /// Returns true when telemetry data is transferred out of process (application insights server or local storage) and are emitted before the flush invocation.
        /// Returns false when transfer of telemetry data to server has failed with non-retriable http status.
        /// FlushAsync on InMemoryChannel always returns true, as the channel offers minimal reliability guarantees and doesn't retry sending telemetry after a failure.
        /// </returns>
        /// TODO: Metrics flush to respect CancellationToken.
        public Task<bool> FlushAsync(CancellationToken cancellationToken)
        {
            if (this.TryGetMetricManager(out MetricManager privateMetricManager))
            {
                privateMetricManager.Flush(flushDownstreamPipeline: false);
            }

            TelemetryConfiguration pipeline = this.configuration;
            if (pipeline != null)
            {
                MetricManager sharedMetricManager = pipeline.GetMetricManager(createIfNotExists: false);
                sharedMetricManager?.Flush(flushDownstreamPipeline: false);

                ITelemetryChannel channel = pipeline.TelemetryChannel;

                if (channel is IAsyncFlushable asyncFlushableChannel && !cancellationToken.IsCancellationRequested)
                {
                    return asyncFlushableChannel.FlushAsync(cancellationToken);
                }
            }

            return cancellationToken.IsCancellationRequested ? TaskEx.FromCanceled<bool>(cancellationToken) : Task.FromResult(false);
        }

        /// <summary>
        /// Gets or creates a metric container that you can use to track, aggregate and send metric values.<br />
        /// Optionally specify a metric configuration to control how the tracked values are aggregated.
        /// </summary>
        /// <remarks>The aggregated values will be sent to the <c>TelemetryConfiguration</c>
        /// associated with this client.<br />
        /// The aggregation scope of the fetched<c>Metric</c> is <c>TelemetryConfiguration</c>; this
        /// means that all values tracked for a given metric ID and dimensions will be aggregated together
        /// across all clients that share the same <c>TelemetryConfiguration</c>.</remarks>
        /// <param name="metricId">The ID (name) of the metric.
        ///   (The namespace specified in <see cref="MetricIdentifier.DefaultMetricNamespace"/> will be used.
        ///   To specify another namespace, use an overload that takes a <c>MetricIdentifier</c> parameter instead.)</param>
        /// <returns>A <c>Metric</c> with the specified ID and dimensions. If you call this method several times
        /// with the same metric ID and dimensions for a given aggregation scope, you will receive the same
        /// instance of <c>Metric</c>.</returns>
        /// <exception cref="ArgumentException">If you previously created a metric with the same namespace, ID, dimensions
        /// and aggregation scope, but with a different configuration. When calling this method to get a previously
        /// created metric, you can simply avoid specifying any configuration (or specify null) to imply the
        /// configuration used earlier.</exception>
        public Metric GetMetric(
                            string metricId)
        {
            return this.GetOrCreateMetric(
                        MetricAggregationScope.TelemetryConfiguration,
                        new MetricIdentifier(metricId),
                        metricConfiguration: null);
        }

        /// <summary>
        /// Gets or creates a metric container that you can use to track, aggregate and send metric values.<br />
        /// Optionally specify a metric configuration to control how the tracked values are aggregated.
        /// </summary>
        /// <remarks>The aggregated values will be sent to the <c>TelemetryConfiguration</c>
        /// associated with this client.<br />
        /// The aggregation scope of the fetched<c>Metric</c> is <c>TelemetryConfiguration</c>; this
        /// means that all values tracked for a given metric ID and dimensions will be aggregated together
        /// across all clients that share the same <c>TelemetryConfiguration</c>.</remarks>
        /// <param name="metricId">The ID (name) of the metric.
        ///   (The namespace specified in <see cref="MetricIdentifier.DefaultMetricNamespace"/> will be used.
        ///   To specify another namespace, use an overload that takes a <c>MetricIdentifier</c> parameter instead.)</param>
        /// <param name="metricConfiguration">Determines how tracked values will be aggregated. <br />
        /// Use presets in <see cref="MetricConfigurations.Common"/> or specify your own settings. </param>
        /// <returns>A <c>Metric</c> with the specified ID and dimensions. If you call this method several times
        /// with the same metric ID and dimensions for a given aggregation scope, you will receive the same
        /// instance of <c>Metric</c>.</returns>
        /// <exception cref="ArgumentException">If you previously created a metric with the same namespace, ID, dimensions
        /// and aggregation scope, but with a different configuration. When calling this method to get a previously
        /// created metric, you can simply avoid specifying any configuration (or specify null) to imply the
        /// configuration used earlier.</exception>
        public Metric GetMetric(
                            string metricId,
                            MetricConfiguration metricConfiguration)
        {
            return this.GetOrCreateMetric(
                        MetricAggregationScope.TelemetryConfiguration,
                        new MetricIdentifier(metricId),
                        metricConfiguration: metricConfiguration);
        }

        /// <summary>
        /// Gets or creates a metric container that you can use to track, aggregate and send metric values.<br />
        /// Optionally specify a metric configuration to control how the tracked values are aggregated.
        /// </summary>
        /// <param name="metricId">The ID (name) of the metric.
        ///   (The namespace specified in <see cref="MetricIdentifier.DefaultMetricNamespace"/> will be used.
        ///   To specify another namespace, use an overload that takes a <c>MetricIdentifier</c> parameter instead.)</param>
        /// <param name="metricConfiguration">Determines how tracked values will be aggregated. <br />
        /// Use presets in <see cref="MetricConfigurations.Common"/> or specify your own settings. </param>
        /// <returns>A <c>Metric</c> with the specified ID and dimensions. If you call this method several times
        /// with the same metric ID and dimensions for a given aggregation scope, you will receive the same
        /// instance of <c>Metric</c>.</returns>
        /// <exception cref="ArgumentException">If you previously created a metric with the same namespace, ID, dimensions
        /// and aggregation scope, but with a different configuration. When calling this method to get a previously
        /// created metric, you can simply avoid specifying any configuration (or specify null) to imply the
        /// configuration used earlier.</exception>
        /// <param name="aggregationScope">The scope across which the values for the metric are to be aggregated in memory.
        /// See <see cref="MetricAggregationScope" /> for more info.</param>
        /// <returns>A <see cref="Metric"/> instance that you can use to automatically aggregate and then sent metric data value.</returns>
        public Metric GetMetric(
                            string metricId,
                            MetricConfiguration metricConfiguration,
                            MetricAggregationScope aggregationScope)
        {
            return this.GetOrCreateMetric(
                        aggregationScope,
                        new MetricIdentifier(metricId),
                        metricConfiguration: metricConfiguration);
        }

        /// <summary>
        /// Gets or creates a metric container that you can use to track, aggregate and send metric values.<br />
        /// Optionally specify a metric configuration to control how the tracked values are aggregated.
        /// </summary>
        /// <remarks>The aggregated values will be sent to the <c>TelemetryConfiguration</c>
        /// associated with this client.<br />
        /// The aggregation scope of the fetched<c>Metric</c> is <c>TelemetryConfiguration</c>; this
        /// means that all values tracked for a given metric ID and dimensions will be aggregated together
        /// across all clients that share the same <c>TelemetryConfiguration</c>.</remarks>
        /// <param name="metricId">The ID (name) of the metric.
        ///   (The namespace specified in <see cref="MetricIdentifier.DefaultMetricNamespace"/> will be used.
        ///   To specify another namespace, use an overload that takes a <c>MetricIdentifier</c> parameter instead.)</param>
        /// <param name="dimension1Name">The name of the first dimension.</param>
        /// <exception cref="ArgumentException">If you previously created a metric with the same namespace, ID, dimensions
        /// and aggregation scope, but with a different configuration. When calling this method to get a previously
        /// created metric, you can simply avoid specifying any configuration (or specify null) to imply the
        /// configuration used earlier.</exception>
        /// <returns>A <see cref="Metric"/> instance that you can use to automatically aggregate and then sent metric data value.</returns>
        public Metric GetMetric(
                            string metricId,
                            string dimension1Name)
        {
            return this.GetOrCreateMetric(
                        MetricAggregationScope.TelemetryConfiguration,
                        new MetricIdentifier(MetricIdentifier.DefaultMetricNamespace, metricId, dimension1Name),
                        metricConfiguration: null);
        }

        /// <summary>
        /// Gets or creates a metric container that you can use to track, aggregate and send metric values.<br />
        /// Optionally specify a metric configuration to control how the tracked values are aggregated.
        /// </summary>
        /// <remarks>The aggregated values will be sent to the <c>TelemetryConfiguration</c>
        /// associated with this client.<br />
        /// The aggregation scope of the fetched<c>Metric</c> is <c>TelemetryConfiguration</c>; this
        /// means that all values tracked for a given metric ID and dimensions will be aggregated together
        /// across all clients that share the same <c>TelemetryConfiguration</c>.</remarks>
        /// <param name="metricId">The ID (name) of the metric.
        ///   (The namespace specified in <see cref="MetricIdentifier.DefaultMetricNamespace"/> will be used.
        ///   To specify another namespace, use an overload that takes a <c>MetricIdentifier</c> parameter instead.)</param>
        /// <param name="dimension1Name">The name of the first dimension.</param>
        /// <param name="metricConfiguration">Determines how tracked values will be aggregated. <br />
        /// Use presets in <see cref="MetricConfigurations.Common"/> or specify your own settings. </param>
        /// <returns>A <c>Metric</c> with the specified ID and dimensions. If you call this method several times
        /// with the same metric ID and dimensions for a given aggregation scope, you will receive the same
        /// instance of <c>Metric</c>.</returns>
        /// <exception cref="ArgumentException">If you previously created a metric with the same namespace, ID, dimensions
        /// and aggregation scope, but with a different configuration. When calling this method to get a previously
        /// created metric, you can simply avoid specifying any configuration (or specify null) to imply the
        /// configuration used earlier.</exception>
        public Metric GetMetric(
                            string metricId,
                            string dimension1Name,
                            MetricConfiguration metricConfiguration)
        {
            return this.GetOrCreateMetric(
                        MetricAggregationScope.TelemetryConfiguration,
                        new MetricIdentifier(MetricIdentifier.DefaultMetricNamespace, metricId, dimension1Name),
                        metricConfiguration: metricConfiguration);
        }

        /// <summary>
        /// Gets or creates a metric container that you can use to track, aggregate and send metric values.<br />
        /// Optionally specify a metric configuration to control how the tracked values are aggregated.
        /// </summary>
        /// <param name="metricId">The ID (name) of the metric.
        ///   (The namespace specified in <see cref="MetricIdentifier.DefaultMetricNamespace"/> will be used.
        ///   To specify another namespace, use an overload that takes a <c>MetricIdentifier</c> parameter instead.)</param>
        /// <param name="dimension1Name">The name of the first dimension.</param>
        /// <param name="metricConfiguration">Determines how tracked values will be aggregated. <br />
        /// Use presets in <see cref="MetricConfigurations.Common"/> or specify your own settings. </param>
        /// <returns>A <c>Metric</c> with the specified ID and dimensions. If you call this method several times
        /// with the same metric ID and dimensions for a given aggregation scope, you will receive the same
        /// instance of <c>Metric</c>.</returns>
        /// <exception cref="ArgumentException">If you previously created a metric with the same namespace, ID, dimensions
        /// and aggregation scope, but with a different configuration. When calling this method to get a previously
        /// created metric, you can simply avoid specifying any configuration (or specify null) to imply the
        /// configuration used earlier.</exception>
        /// <param name="aggregationScope">The scope across which the values for the metric are to be aggregated in memory.
        /// See <see cref="MetricAggregationScope" /> for more info.</param>
        public Metric GetMetric(
                            string metricId,
                            string dimension1Name,
                            MetricConfiguration metricConfiguration,
                            MetricAggregationScope aggregationScope)
        {
            return this.GetOrCreateMetric(
                        aggregationScope,
                        new MetricIdentifier(MetricIdentifier.DefaultMetricNamespace, metricId, dimension1Name),
                        metricConfiguration: metricConfiguration);
        }

        /// <summary>
        /// Gets or creates a metric container that you can use to track, aggregate and send metric values.<br />
        /// Optionally specify a metric configuration to control how the tracked values are aggregated.
        /// </summary>
        /// <remarks>The aggregated values will be sent to the <c>TelemetryConfiguration</c>
        /// associated with this client.<br />
        /// The aggregation scope of the fetched<c>Metric</c> is <c>TelemetryConfiguration</c>; this
        /// means that all values tracked for a given metric ID and dimensions will be aggregated together
        /// across all clients that share the same <c>TelemetryConfiguration</c>.</remarks>
        /// <param name="metricId">The ID (name) of the metric.
        ///   (The namespace specified in <see cref="MetricIdentifier.DefaultMetricNamespace"/> will be used.
        ///   To specify another namespace, use an overload that takes a <c>MetricIdentifier</c> parameter instead.)</param>
        /// <param name="dimension1Name">The name of the first dimension.</param>
        /// <param name="dimension2Name">The name of the second dimension.</param>
        /// <exception cref="ArgumentException">If you previously created a metric with the same namespace, ID, dimensions
        /// and aggregation scope, but with a different configuration. When calling this method to get a previously
        /// created metric, you can simply avoid specifying any configuration (or specify null) to imply the
        /// configuration used earlier.</exception>
        /// <returns>A <see cref="Metric"/> instance that you can use to automatically aggregate and then sent metric data value.</returns>
        public Metric GetMetric(
                            string metricId,
                            string dimension1Name,
                            string dimension2Name)
        {
            return this.GetOrCreateMetric(
                        MetricAggregationScope.TelemetryConfiguration,
                        new MetricIdentifier(MetricIdentifier.DefaultMetricNamespace, metricId, dimension1Name, dimension2Name),
                        metricConfiguration: null);
        }

        /// <summary>
        /// Gets or creates a metric container that you can use to track, aggregate and send metric values.<br />
        /// Optionally specify a metric configuration to control how the tracked values are aggregated.
        /// </summary>
        /// <remarks>The aggregated values will be sent to the <c>TelemetryConfiguration</c>
        /// associated with this client.<br />
        /// The aggregation scope of the fetched<c>Metric</c> is <c>TelemetryConfiguration</c>; this
        /// means that all values tracked for a given metric ID and dimensions will be aggregated together
        /// across all clients that share the same <c>TelemetryConfiguration</c>.</remarks>
        /// <param name="metricId">The ID (name) of the metric.
        ///   (The namespace specified in <see cref="MetricIdentifier.DefaultMetricNamespace"/> will be used.
        ///   To specify another namespace, use an overload that takes a <c>MetricIdentifier</c> parameter instead.)</param>
        /// <param name="dimension1Name">The name of the first dimension.</param>
        /// <param name="dimension2Name">The name of the second dimension.</param>
        /// <param name="metricConfiguration">Determines how tracked values will be aggregated. <br />
        /// Use presets in <see cref="MetricConfigurations.Common"/> or specify your own settings. </param>
        /// <returns>A <c>Metric</c> with the specified ID and dimensions. If you call this method several times
        /// with the same metric ID and dimensions for a given aggregation scope, you will receive the same
        /// instance of <c>Metric</c>.</returns>
        /// <exception cref="ArgumentException">If you previously created a metric with the same namespace, ID, dimensions
        /// and aggregation scope, but with a different configuration. When calling this method to get a previously
        /// created metric, you can simply avoid specifying any configuration (or specify null) to imply the
        /// configuration used earlier.</exception>
        public Metric GetMetric(
                            string metricId,
                            string dimension1Name,
                            string dimension2Name,
                            MetricConfiguration metricConfiguration)
        {
            return this.GetOrCreateMetric(
                        MetricAggregationScope.TelemetryConfiguration,
                        new MetricIdentifier(MetricIdentifier.DefaultMetricNamespace, metricId, dimension1Name, dimension2Name),
                        metricConfiguration);
        }

        /// <summary>
        /// Gets or creates a metric container that you can use to track, aggregate and send metric values.<br />
        /// Optionally specify a metric configuration to control how the tracked values are aggregated.
        /// </summary>
        /// <param name="metricId">The ID (name) of the metric.
        ///   (The namespace specified in <see cref="MetricIdentifier.DefaultMetricNamespace"/> will be used.
        ///   To specify another namespace, use an overload that takes a <c>MetricIdentifier</c> parameter instead.)</param>
        /// <param name="dimension1Name">The name of the first dimension.</param>
        /// <param name="dimension2Name">The name of the second dimension.</param>
        /// <param name="metricConfiguration">Determines how tracked values will be aggregated. <br />
        /// Use presets in <see cref="MetricConfigurations.Common"/> or specify your own settings. </param>
        /// <returns>A <c>Metric</c> with the specified ID and dimensions. If you call this method several times
        /// with the same metric ID and dimensions for a given aggregation scope, you will receive the same
        /// instance of <c>Metric</c>.</returns>
        /// <exception cref="ArgumentException">If you previously created a metric with the same namespace, ID, dimensions
        /// and aggregation scope, but with a different configuration. When calling this method to get a previously
        /// created metric, you can simply avoid specifying any configuration (or specify null) to imply the
        /// configuration used earlier.</exception>
        /// <param name="aggregationScope">The scope across which the values for the metric are to be aggregated in memory.
        /// See <see cref="MetricAggregationScope" /> for more info.</param>
        public Metric GetMetric(
                            string metricId,
                            string dimension1Name,
                            string dimension2Name,
                            MetricConfiguration metricConfiguration,
                            MetricAggregationScope aggregationScope)
        {
            return this.GetOrCreateMetric(
                        aggregationScope,
                        new MetricIdentifier(MetricIdentifier.DefaultMetricNamespace, metricId, dimension1Name, dimension2Name),
                        metricConfiguration);
        }

        /// <summary>
        /// Gets or creates a metric container that you can use to track, aggregate and send metric values.<br />
        /// Optionally specify a metric configuration to control how the tracked values are aggregated.
        /// </summary>
        /// <remarks>The aggregated values will be sent to the <c>TelemetryConfiguration</c>
        /// associated with this client.<br />
        /// The aggregation scope of the fetched<c>Metric</c> is <c>TelemetryConfiguration</c>; this
        /// means that all values tracked for a given metric ID and dimensions will be aggregated together
        /// across all clients that share the same <c>TelemetryConfiguration</c>.</remarks>
        /// <param name="metricId">The ID (name) of the metric.
        ///   (The namespace specified in <see cref="MetricIdentifier.DefaultMetricNamespace"/> will be used.
        ///   To specify another namespace, use an overload that takes a <c>MetricIdentifier</c> parameter instead.)</param>
        /// <param name="dimension1Name">The name of the first dimension.</param>
        /// <param name="dimension2Name">The name of the second dimension.</param>
        /// <param name="dimension3Name">The name of the third dimension.</param>
        /// <exception cref="ArgumentException">If you previously created a metric with the same namespace, ID, dimensions
        /// and aggregation scope, but with a different configuration. When calling this method to get a previously
        /// created metric, you can simply avoid specifying any configuration (or specify null) to imply the
        /// configuration used earlier.</exception>
        /// <returns>A <see cref="Metric"/> instance that you can use to automatically aggregate and then sent metric data value.</returns>
        public Metric GetMetric(
                            string metricId,
                            string dimension1Name,
                            string dimension2Name,
                            string dimension3Name)
        {
            return this.GetOrCreateMetric(
                        MetricAggregationScope.TelemetryConfiguration,
                        new MetricIdentifier(MetricIdentifier.DefaultMetricNamespace, metricId, dimension1Name, dimension2Name, dimension3Name),
                        metricConfiguration: null);
        }

        /// <summary>
        /// Gets or creates a metric container that you can use to track, aggregate and send metric values.<br />
        /// Optionally specify a metric configuration to control how the tracked values are aggregated.
        /// </summary>
        /// <remarks>The aggregated values will be sent to the <c>TelemetryConfiguration</c>
        /// associated with this client.<br />
        /// The aggregation scope of the fetched<c>Metric</c> is <c>TelemetryConfiguration</c>; this
        /// means that all values tracked for a given metric ID and dimensions will be aggregated together
        /// across all clients that share the same <c>TelemetryConfiguration</c>.</remarks>
        /// <param name="metricId">The ID (name) of the metric.
        ///   (The namespace specified in <see cref="MetricIdentifier.DefaultMetricNamespace"/> will be used.
        ///   To specify another namespace, use an overload that takes a <c>MetricIdentifier</c> parameter instead.)</param>
        /// <param name="dimension1Name">The name of the first dimension.</param>
        /// <param name="dimension2Name">The name of the second dimension.</param>
        /// <param name="dimension3Name">The name of the third dimension.</param>
        /// <param name="metricConfiguration">Determines how tracked values will be aggregated. <br />
        /// Use presets in <see cref="MetricConfigurations.Common"/> or specify your own settings. </param>
        /// <returns>A <c>Metric</c> with the specified ID and dimensions. If you call this method several times
        /// with the same metric ID and dimensions for a given aggregation scope, you will receive the same
        /// instance of <c>Metric</c>.</returns>
        /// <exception cref="ArgumentException">If you previously created a metric with the same namespace, ID, dimensions
        /// and aggregation scope, but with a different configuration. When calling this method to get a previously
        /// created metric, you can simply avoid specifying any configuration (or specify null) to imply the
        /// configuration used earlier.</exception>
        public Metric GetMetric(
                            string metricId,
                            string dimension1Name,
                            string dimension2Name,
                            string dimension3Name,
                            MetricConfiguration metricConfiguration)
        {
            return this.GetOrCreateMetric(
                        MetricAggregationScope.TelemetryConfiguration,
                        new MetricIdentifier(MetricIdentifier.DefaultMetricNamespace, metricId, dimension1Name, dimension2Name, dimension3Name),
                        metricConfiguration);
        }

        /// <summary>
        /// Gets or creates a metric container that you can use to track, aggregate and send metric values.<br />
        /// Optionally specify a metric configuration to control how the tracked values are aggregated.
        /// </summary>
        /// <param name="metricId">The ID (name) of the metric.
        ///   (The namespace specified in <see cref="MetricIdentifier.DefaultMetricNamespace"/> will be used.
        ///   To specify another namespace, use an overload that takes a <c>MetricIdentifier</c> parameter instead.)</param>
        /// <param name="dimension1Name">The name of the first dimension.</param>
        /// <param name="dimension2Name">The name of the second dimension.</param>
        /// <param name="dimension3Name">The name of the third dimension.</param>
        /// <param name="metricConfiguration">Determines how tracked values will be aggregated. <br />
        /// Use presets in <see cref="MetricConfigurations.Common"/> or specify your own settings. </param>
        /// <returns>A <c>Metric</c> with the specified ID and dimensions. If you call this method several times
        /// with the same metric ID and dimensions for a given aggregation scope, you will receive the same
        /// instance of <c>Metric</c>.</returns>
        /// <exception cref="ArgumentException">If you previously created a metric with the same namespace, ID, dimensions
        /// and aggregation scope, but with a different configuration. When calling this method to get a previously
        /// created metric, you can simply avoid specifying any configuration (or specify null) to imply the
        /// configuration used earlier.</exception>
        /// <param name="aggregationScope">The scope across which the values for the metric are to be aggregated in memory.
        /// See <see cref="MetricAggregationScope" /> for more info.</param>
        public Metric GetMetric(
                            string metricId,
                            string dimension1Name,
                            string dimension2Name,
                            string dimension3Name,
                            MetricConfiguration metricConfiguration,
                            MetricAggregationScope aggregationScope)
        {
            return this.GetOrCreateMetric(
                        aggregationScope,
                        new MetricIdentifier(MetricIdentifier.DefaultMetricNamespace, metricId, dimension1Name, dimension2Name, dimension3Name),
                        metricConfiguration);
        }

        /// <summary>
        /// Gets or creates a metric container that you can use to track, aggregate and send metric values.<br />
        /// Optionally specify a metric configuration to control how the tracked values are aggregated.
        /// </summary>
        /// <remarks>The aggregated values will be sent to the <c>TelemetryConfiguration</c>
        /// associated with this client.<br />
        /// The aggregation scope of the fetched<c>Metric</c> is <c>TelemetryConfiguration</c>; this
        /// means that all values tracked for a given metric ID and dimensions will be aggregated together
        /// across all clients that share the same <c>TelemetryConfiguration</c>.</remarks>
        /// <param name="metricId">The ID (name) of the metric.
        ///   (The namespace specified in <see cref="MetricIdentifier.DefaultMetricNamespace"/> will be used.
        ///   To specify another namespace, use an overload that takes a <c>MetricIdentifier</c> parameter instead.)</param>
        /// <param name="dimension1Name">The name of the first dimension.</param>
        /// <param name="dimension2Name">The name of the second dimension.</param>
        /// <param name="dimension3Name">The name of the third dimension.</param>
        /// <param name="dimension4Name">The name of the fourth dimension.</param>
        /// <exception cref="ArgumentException">If you previously created a metric with the same namespace, ID, dimensions
        /// and aggregation scope, but with a different configuration. When calling this method to get a previously
        /// created metric, you can simply avoid specifying any configuration (or specify null) to imply the
        /// configuration used earlier.</exception>
        /// <returns>A <see cref="Metric"/> instance that you can use to automatically aggregate and then sent metric data value.</returns>
        public Metric GetMetric(
                            string metricId,
                            string dimension1Name,
                            string dimension2Name,
                            string dimension3Name,
                            string dimension4Name)
        {
            return this.GetOrCreateMetric(
                        MetricAggregationScope.TelemetryConfiguration,
                        new MetricIdentifier(MetricIdentifier.DefaultMetricNamespace, metricId, dimension1Name, dimension2Name, dimension3Name, dimension4Name),
                        metricConfiguration: null);
        }

        /// <summary>
        /// Gets or creates a metric container that you can use to track, aggregate and send metric values.<br />
        /// Optionally specify a metric configuration to control how the tracked values are aggregated.
        /// </summary>
        /// <remarks>The aggregated values will be sent to the <c>TelemetryConfiguration</c>
        /// associated with this client.<br />
        /// The aggregation scope of the fetched<c>Metric</c> is <c>TelemetryConfiguration</c>; this
        /// means that all values tracked for a given metric ID and dimensions will be aggregated together
        /// across all clients that share the same <c>TelemetryConfiguration</c>.</remarks>
        /// <param name="metricId">The ID (name) of the metric.
        ///   (The namespace specified in <see cref="MetricIdentifier.DefaultMetricNamespace"/> will be used.
        ///   To specify another namespace, use an overload that takes a <c>MetricIdentifier</c> parameter instead.)</param>
        /// <param name="dimension1Name">The name of the first dimension.</param>
        /// <param name="dimension2Name">The name of the second dimension.</param>
        /// <param name="dimension3Name">The name of the third dimension.</param>
        /// <param name="dimension4Name">The name of the fourth dimension.</param>
        /// <param name="metricConfiguration">Determines how tracked values will be aggregated. <br />
        /// Use presets in <see cref="MetricConfigurations.Common"/> or specify your own settings. </param>
        /// <returns>A <c>Metric</c> with the specified ID and dimensions. If you call this method several times
        /// with the same metric ID and dimensions for a given aggregation scope, you will receive the same
        /// instance of <c>Metric</c>.</returns>
        /// <exception cref="ArgumentException">If you previously created a metric with the same namespace, ID, dimensions
        /// and aggregation scope, but with a different configuration. When calling this method to get a previously
        /// created metric, you can simply avoid specifying any configuration (or specify null) to imply the
        /// configuration used earlier.</exception>
        public Metric GetMetric(
                            string metricId,
                            string dimension1Name,
                            string dimension2Name,
                            string dimension3Name,
                            string dimension4Name,
                            MetricConfiguration metricConfiguration)
        {
            return this.GetOrCreateMetric(
                        MetricAggregationScope.TelemetryConfiguration,
                        new MetricIdentifier(MetricIdentifier.DefaultMetricNamespace, metricId, dimension1Name, dimension2Name, dimension3Name, dimension4Name),
                        metricConfiguration);
        }

        /// <summary>
        /// Gets or creates a metric container that you can use to track, aggregate and send metric values.<br />
        /// Optionally specify a metric configuration to control how the tracked values are aggregated.
        /// </summary>
        /// <param name="metricId">The ID (name) of the metric.
        ///   (The namespace specified in <see cref="MetricIdentifier.DefaultMetricNamespace"/> will be used.
        ///   To specify another namespace, use an overload that takes a <c>MetricIdentifier</c> parameter instead.)</param>
        /// <param name="dimension1Name">The name of the first dimension.</param>
        /// <param name="dimension2Name">The name of the second dimension.</param>
        /// <param name="dimension3Name">The name of the third dimension.</param>
        /// <param name="dimension4Name">The name of the fourth dimension.</param>
        /// <param name="metricConfiguration">Determines how tracked values will be aggregated. <br />
        /// Use presets in <see cref="MetricConfigurations.Common"/> or specify your own settings. </param>
        /// <returns>A <c>Metric</c> with the specified ID and dimensions. If you call this method several times
        /// with the same metric ID and dimensions for a given aggregation scope, you will receive the same
        /// instance of <c>Metric</c>.</returns>
        /// <exception cref="ArgumentException">If you previously created a metric with the same namespace, ID, dimensions
        /// and aggregation scope, but with a different configuration. When calling this method to get a previously
        /// created metric, you can simply avoid specifying any configuration (or specify null) to imply the
        /// configuration used earlier.</exception>
        /// <param name="aggregationScope">The scope across which the values for the metric are to be aggregated in memory.
        /// See <see cref="MetricAggregationScope" /> for more info.</param>
        public Metric GetMetric(
                            string metricId,
                            string dimension1Name,
                            string dimension2Name,
                            string dimension3Name,
                            string dimension4Name,
                            MetricConfiguration metricConfiguration,
                            MetricAggregationScope aggregationScope)
        {
            return this.GetOrCreateMetric(
                        aggregationScope,
                        new MetricIdentifier(MetricIdentifier.DefaultMetricNamespace, metricId, dimension1Name, dimension2Name, dimension3Name, dimension4Name),
                        metricConfiguration);
        }

        /// <summary>
        /// Gets or creates a metric container that you can use to track, aggregate and send metric values.<br />
        /// Optionally specify a metric configuration to control how the tracked values are aggregated.
        /// </summary>
        /// <remarks>The aggregated values will be sent to the <c>TelemetryConfiguration</c>
        /// associated with this client.<br />
        /// The aggregation scope of the fetched<c>Metric</c> is <c>TelemetryConfiguration</c>; this
        /// means that all values tracked for a given metric ID and dimensions will be aggregated together
        /// across all clients that share the same <c>TelemetryConfiguration</c>.</remarks>
        /// <param name="metricIdentifier">A grouping containing the Namespace, the ID (name) and the dimension names of the metric.</param>
        /// <exception cref="ArgumentException">If you previously created a metric with the same namespace, ID, dimensions
        /// and aggregation scope, but with a different configuration. When calling this method to get a previously
        /// created metric, you can simply avoid specifying any configuration (or specify null) to imply the
        /// configuration used earlier.</exception>
        /// <returns>A <see cref="Metric"/> instance that you can use to automatically aggregate and then sent metric data value.</returns>
        public Metric GetMetric(
                            MetricIdentifier metricIdentifier)
        {
            return this.GetOrCreateMetric(
                        MetricAggregationScope.TelemetryConfiguration,
                        metricIdentifier,
                        metricConfiguration: null);
        }

        /// <summary>
        /// Gets or creates a metric container that you can use to track, aggregate and send metric values.<br />
        /// Optionally specify a metric configuration to control how the tracked values are aggregated.
        /// </summary>
        /// <remarks>The aggregated values will be sent to the <c>TelemetryConfiguration</c>
        /// associated with this client.<br />
        /// The aggregation scope of the fetched<c>Metric</c> is <c>TelemetryConfiguration</c>; this
        /// means that all values tracked for a given metric ID and dimensions will be aggregated together
        /// across all clients that share the same <c>TelemetryConfiguration</c>.</remarks>
        /// <param name="metricIdentifier">A grouping containing the Namespace, the ID (name) and the dimension names of the metric.</param>
        /// <param name="metricConfiguration">Determines how tracked values will be aggregated. <br />
        /// Use presets in <see cref="MetricConfigurations.Common"/> or specify your own settings. </param>
        /// <returns>A <c>Metric</c> with the specified ID and dimensions. If you call this method several times
        /// with the same metric ID and dimensions for a given aggregation scope, you will receive the same
        /// instance of <c>Metric</c>.</returns>
        /// <exception cref="ArgumentException">If you previously created a metric with the same namespace, ID, dimensions
        /// and aggregation scope, but with a different configuration. When calling this method to get a previously
        /// created metric, you can simply avoid specifying any configuration (or specify null) to imply the
        /// configuration used earlier.</exception>
        public Metric GetMetric(
                            MetricIdentifier metricIdentifier,
                            MetricConfiguration metricConfiguration)
        {
            return this.GetOrCreateMetric(
                        MetricAggregationScope.TelemetryConfiguration,
                        metricIdentifier,
                        metricConfiguration);
        }

        /// <summary>
        /// Gets or creates a metric container that you can use to track, aggregate and send metric values.<br />
        /// Optionally specify a metric configuration to control how the tracked values are aggregated.
        /// </summary>
        /// <param name="metricIdentifier">A grouping containing the Namespace, the ID (name) and the dimension names of the metric.</param>
        /// <param name="metricConfiguration">Determines how tracked values will be aggregated. <br />
        /// Use presets in <see cref="MetricConfigurations.Common"/> or specify your own settings. </param>
        /// <returns>A <c>Metric</c> with the specified ID and dimensions. If you call this method several times
        /// with the same metric ID and dimensions for a given aggregation scope, you will receive the same
        /// instance of <c>Metric</c>.</returns>
        /// <exception cref="ArgumentException">If you previously created a metric with the same namespace, ID, dimensions
        /// and aggregation scope, but with a different configuration. When calling this method to get a previously
        /// created metric, you can simply avoid specifying any configuration (or specify null) to imply the
        /// configuration used earlier.</exception>
        /// <param name="aggregationScope">The scope across which the values for the metric are to be aggregated in memory.
        /// See <see cref="MetricAggregationScope" /> for more info.</param>
        public Metric GetMetric(
                            MetricIdentifier metricIdentifier,
                            MetricConfiguration metricConfiguration,
                            MetricAggregationScope aggregationScope)
        {
            return this.GetOrCreateMetric(
                        aggregationScope,
                        metricIdentifier,
                        metricConfiguration);
        }

        private Metric GetOrCreateMetric(
                                    MetricAggregationScope aggregationScope,
                                    MetricIdentifier metricIdentifier,
                                    MetricConfiguration metricConfiguration)
        {
            MetricManager metricManager = this.GetMetricManager(aggregationScope);
            Metric metric = metricManager.Metrics.GetOrCreate(metricIdentifier, metricConfiguration);
            return metric;
        }
    }
}
