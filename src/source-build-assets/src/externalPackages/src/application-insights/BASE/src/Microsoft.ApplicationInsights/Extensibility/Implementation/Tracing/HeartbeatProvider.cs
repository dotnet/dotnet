namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
    /// Implementation of heartbeat functionality.
    /// </summary>
    internal class HeartbeatProvider : IDisposable, IHeartbeatProvider
    {
        /// <summary>
        /// The default interval between heartbeats if not specified by the user. Left public for use in unit tests.
        /// </summary>
        public static readonly TimeSpan DefaultHeartbeatInterval = TimeSpan.FromMinutes(15.0);

        /// <summary>
        /// The minimum interval that can be set between heartbeats.
        /// </summary>
        public static readonly TimeSpan MinimumHeartbeatInterval = TimeSpan.FromSeconds(30.0);

        /// <summary>
        /// Value for property indicating 'app insights version' related specifically to heartbeats.
        /// </summary>        
#if NETSTANDARD // This constant is defined for all versions of NetStandard https://docs.microsoft.com/en-us/dotnet/core/tutorials/libraries#how-to-multitarget
        private static string sdkVersionPropertyValue = SdkVersionUtils.GetSdkVersion("hbnetc:");
#else
        private static string sdkVersionPropertyValue = SdkVersionUtils.GetSdkVersion("hbnet:");
#endif

        /// <summary>
        /// The name of the heartbeat metric item and operation context. 
        /// </summary>
        private static string heartbeatSyntheticMetricName = "HeartbeatState";

        /// <summary>
        /// List of fields that are not to be sent with the payload. Empty list means send everything available.
        /// </summary>
        private readonly List<string> disabledDefaultFields = new List<string>();

        /// <summary>
        /// List of default heartbeat property providers that are not to contribute to the payload. Empty list means send everything available.
        /// </summary>
        private readonly List<string> disabledHeartbeatPropertyProviders = new List<string>();

        private UInt64 heartbeatsSent; // counter of all heartbeats

        /// <summary>
        /// The payload items to send out with each heartbeat.
        /// </summary>
        private ConcurrentDictionary<string, HeartbeatPropertyPayload> heartbeatProperties;

        private bool disposedValue = false; // To detect redundant calls to dispose
        private TimeSpan interval; // time between heartbeats emitted
        private TelemetryClient telemetryClient; // client to use in sending our heartbeat
        private volatile bool isEnabled; // no need for locks or volatile here, we can skip/add a beat if the module is disabled between heartbeats

        public HeartbeatProvider()
        {
            this.interval = DefaultHeartbeatInterval;
            this.heartbeatProperties = new ConcurrentDictionary<string, HeartbeatPropertyPayload>(StringComparer.OrdinalIgnoreCase);
            this.heartbeatsSent = 0; // count up from construction time
            this.isEnabled = true;
        }

        /// <summary>
        /// Gets or sets the currently defined interval between heartbeats.
        /// </summary>
        public TimeSpan HeartbeatInterval
        {
            get => this.interval;
            set
            {
                if (value <= MinimumHeartbeatInterval)
                {
                    this.interval = MinimumHeartbeatInterval;
                }
                else
                {
                    this.interval = value;
                }

                this.InitTimer();
            }
        }

        /// <summary>
        /// Gets or sets the currently defined instrumentation key to send heartbeat telemetry items to.
        /// 
        /// Note that if the heartbeat provider has not been initialized yet, this key would get reset to 
        /// whatever the telemetry configuration the heartbeat provider is initialized with.
        /// </summary>
        public string InstrumentationKey
        {
            get => this.telemetryClient?.InstrumentationKey;
            set
            {
                if (this.telemetryClient != null)
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    this.telemetryClient.InstrumentationKey = value;
#pragma warning restore CS0618 // Type or member is obsolete
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not heartbeats are enabled.
        /// </summary>
        public bool IsHeartbeatEnabled
        {
            get => this.isEnabled;
            set
            {
                this.isEnabled = value;
                this.InitTimer();
            }
        }

        /// <summary>
        /// Gets a list of default heartbeat property providers that are disabled and will not contribute to the
        /// default heartbeat properties.
        /// </summary>
        public IList<string> ExcludedHeartbeatPropertyProviders
        {
            get => this.disabledHeartbeatPropertyProviders;
        }

        /// <summary>
        /// Gets a list of default field names that should not be sent with each heartbeat.
        /// </summary>
        public IList<string> ExcludedHeartbeatProperties
        {
            get => this.disabledDefaultFields;
        }

        private Timer HeartbeatTimer { get; set; } // timer that will send each heartbeat in intervals

        public void Initialize(TelemetryConfiguration configuration)
        {
            if (this.telemetryClient == null)
            {
                this.telemetryClient = new TelemetryClient(configuration);
            }

            Task.Factory.StartNew(async () => await HeartbeatDefaultPayload.PopulateDefaultPayload(this.ExcludedHeartbeatProperties, this.ExcludedHeartbeatPropertyProviders, this).ConfigureAwait(false));

            this.InitTimer();
        }

        public bool AddHeartbeatProperty(string heartbeatPropertyName, string heartbeatPropertyValue, bool isHealthy)
        {
            return this.AddHeartbeatProperty(propertyName: heartbeatPropertyName, propertyValue: heartbeatPropertyValue, isHealthy: isHealthy, overrideDefaultField: false);
        }

        public bool SetHeartbeatProperty(string heartbeatPropertyName, string heartbeatPropertyValue = null, bool? isHealthy = null)
        {
            return this.SetHeartbeatProperty(propertyName: heartbeatPropertyName, propertyValue: heartbeatPropertyValue, isHealthy: isHealthy, overrideDefaultField: false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool AddHeartbeatProperty(string propertyName, bool overrideDefaultField, string propertyValue, bool isHealthy)
        {
            bool isAdded = false;

            if (!string.IsNullOrEmpty(propertyName)
                && (overrideDefaultField || !HeartbeatDefaultPayload.IsDefaultKeyword(propertyName)))
            {
                try
                {
                    var existingProp = this.heartbeatProperties.GetOrAdd(propertyName, (key) =>
                    {
                        isAdded = true;
                        return new HeartbeatPropertyPayload()
                        {
                            IsHealthy = isHealthy,
                            PayloadValue = propertyValue,
                        };
                    });
                }
                catch (Exception e)
                {
                    CoreEventSource.Log.FailedToAddHeartbeatProperty(propertyName, propertyValue, e.ToString());
                }
            }
            else
            {
                CoreEventSource.Log.HeartbeatPropertyAddedWithoutAnyName(propertyValue, isHealthy);
            }

            return isAdded;
        }

        public bool SetHeartbeatProperty(string propertyName, bool overrideDefaultField, string propertyValue = null, bool? isHealthy = null)
        {
            bool setResult = false;
            if (!string.IsNullOrEmpty(propertyName)
                && (overrideDefaultField || !HeartbeatDefaultPayload.IsDefaultKeyword(propertyName)))
            {
                try
                {
                    this.heartbeatProperties.AddOrUpdate(propertyName, (key) =>
                    {
                        throw new Exception("Cannot set a heartbeat property without adding it first.");
                    },
                    (key, property) =>
                    {
                        if (isHealthy != null)
                        {
                            property.IsHealthy = isHealthy.Value;
                        }

                        if (propertyValue != null)
                        {
                            property.PayloadValue = propertyValue;
                        }

                        return property;
                    });
                    setResult = true;
                }
                catch (Exception e)
                {
                    CoreEventSource.Log.FailedToSetHeartbeatProperty(propertyName, propertyValue, isHealthy.HasValue, isHealthy.GetValueOrDefault(false), e.ToString());
                }
            }
            else
            {
                CoreEventSource.Log.CannotSetHeartbeatPropertyWithNoNameOrDefaultName(propertyName, propertyValue, isHealthy.HasValue, isHealthy.GetValueOrDefault(false));
            }

            return setResult;
        }

        /// <summary>
        /// Get the metric telemetry item that will be sent.
        /// 
        /// Note: exposed to internal to allow inspection for testing.
        /// </summary>
        /// <returns>A MetricTelemtry item that contains the currently defined payload for a heartbeat 'pulse'.</returns>
        internal ITelemetry GatherData()
        {
            var hbeat = new MetricTelemetry(heartbeatSyntheticMetricName, 0.0);

            foreach (var payloadItem in this.heartbeatProperties)
            {
                hbeat.Properties.Add(payloadItem.Key, payloadItem.Value.PayloadValue);
                hbeat.Sum += payloadItem.Value.IsHealthy ? 0 : 1;
            }

            hbeat.Sequence = string.Format(CultureInfo.CurrentCulture, "{0}", this.heartbeatsSent++);

            return hbeat;
        }

        /// <summary>
        /// This method is intended to be called from the <see cref="Initialize"/> method, or whenever <see cref="IsHeartbeatEnabled"/> or <see cref="HeartbeatInterval"/> properties have been set.
        /// This will ensure that any changes to properties will be immediately applied to the <see cref="HeartbeatTimer"/>.
        /// </summary>
        internal void InitTimer()
        {
            if (this.IsHeartbeatEnabled && this.HeartbeatTimer == null)
            {
                this.HeartbeatTimer = new Timer(callback: this.HeartbeatPulse, state: this, dueTime: this.HeartbeatInterval, period: this.HeartbeatInterval);
            }
            else if (this.IsHeartbeatEnabled)
            {
                this.HeartbeatTimer.Change(dueTime: this.HeartbeatInterval, period: this.HeartbeatInterval);
            }
            else if (this.HeartbeatTimer != null)
            {
                this.HeartbeatTimer.Change(dueTime: Timeout.Infinite, period: Timeout.Infinite);
                this.HeartbeatTimer.Dispose();
                this.HeartbeatTimer = null;
            }
        }

        protected void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    if (this.HeartbeatTimer != null)
                    {
                        this.isEnabled = false;

                        try
                        {
                            this.HeartbeatTimer.Dispose();
                        }
                        catch (Exception)
                        {
                            CoreEventSource.Log.LogError("Error occurred when disposing heartbeat timer within HeartbeatProvider");
                        }
                    }
                }

                this.disposedValue = true;
            }
        }

        private void Send()
        {
            if (this.telemetryClient.TelemetryConfiguration.TelemetryChannel == null)
            {
                return;
            }

            var eventData = (MetricTelemetry)this.GatherData();

            eventData.Context.Operation.SyntheticSource = heartbeatSyntheticMetricName;
            eventData.Context.GetInternalContext().SdkVersion = sdkVersionPropertyValue;

            this.telemetryClient.Track(eventData);
        }

        private void HeartbeatPulse(object state)
        {
            if (state is HeartbeatProvider hp && hp.IsHeartbeatEnabled)
            {
                // we will be prone to overlap if any extension payload provider takes a longer time to process than our timer
                // interval. Best that we reset the timer each time round.
                try
                {
                    this.HeartbeatTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    hp.Send();
                }
                catch (ObjectDisposedException)
                {
                    // swallow this exception but log it just the same.
                    CoreEventSource.Log.LogError("Heartbeat timer change during dispose occurred.");
                }
                finally
                {
                    if (this.IsHeartbeatEnabled)
                    {
                        this.HeartbeatTimer.Change(this.HeartbeatInterval, this.HeartbeatInterval);
                    }
                }
            }
            else
            {
                CoreEventSource.Log.LogError("Heartbeat pulse being sent without valid instance of HeartbeatProvider as its state");
            }
        }
    }
}
