namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Authentication;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation.TransmissionPolicy;

    using TelemetryBuffer = Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation.TelemetryBuffer;

    /// <summary>
    /// Represents a communication channel for sending telemetry to Application Insights via HTTP/S.
    /// </summary>
    public sealed class ServerTelemetryChannel : ITelemetryChannel, IAsyncFlushable, ITelemetryModule, ISupportCredentialEnvelope
    {
        internal TelemetrySerializer TelemetrySerializer;
        internal TelemetryBuffer TelemetryBuffer;
        internal Transmitter Transmitter;

        private readonly InterlockedThrottle throttleEmptyIkeyLog = new InterlockedThrottle(interval: TimeSpan.FromSeconds(30));
        private readonly TransmissionPolicyCollection policies;

        private bool? developerMode;
        private int telemetryBufferCapacity;
        private ITelemetryProcessor telemetryProcessor;
        private bool isInitialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerTelemetryChannel"/> class.
        /// </summary>
#if NETFRAMEWORK
        [SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope", Justification = "WebApplicationLifecycle is needed for the life of the application.")]
        public ServerTelemetryChannel() : this(new Network(), new WebApplicationLifecycle())
#else
        // TODO: IApplicationLifecycle implementation for netcore need to be written instead of null here.
        public ServerTelemetryChannel() : this(new Network(), null)
#endif
        {
        }

        internal ServerTelemetryChannel(INetwork network, IApplicationLifecycle applicationLifecycle)
        {
            this.policies = new TransmissionPolicyCollection(network, applicationLifecycle);
            this.Transmitter = new Transmitter(policies: this.policies);

            this.TelemetrySerializer = new TelemetrySerializer(this.Transmitter);
            this.TelemetryBuffer = new TelemetryBuffer(this.TelemetrySerializer, applicationLifecycle);
            this.telemetryBufferCapacity = this.TelemetryBuffer.Capacity;

            this.TelemetryProcessor = this.TelemetryBuffer;
            this.isInitialized = false;
        }

        /// <summary>
        /// Gets or sets default interval after which diagnostics event will be logged if telemetry sending was disabled.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TimeSpan DefaultBackoffEnabledReportingInterval
        {
            get
            {
                return this.Transmitter.BackoffLogicManager.DefaultBackoffEnabledReportingInterval;
            }

            set
            {
                this.Transmitter.BackoffLogicManager.DefaultBackoffEnabledReportingInterval = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether developer mode of telemetry transmission is enabled.
        /// When developer mode is True, <see cref="TelemetryChannel"/> sends telemetry to Application Insights immediately 
        /// during the entire lifetime of the application. When developer mode is False, <see cref="TelemetryChannel"/>
        /// respects production sending policies defined by other properties.
        /// </summary>
        public bool? DeveloperMode
        {
            get
            {
                return this.developerMode;
            }

            set
            {
                if (value != this.developerMode)
                {
                    if (value.HasValue && value.Value)
                    {
                        this.telemetryBufferCapacity = this.TelemetryBuffer.Capacity;
                        this.TelemetryBuffer.Capacity = 1;
                    }
                    else
                    {
                        this.TelemetryBuffer.Capacity = this.telemetryBufferCapacity;
                    }

                    this.developerMode = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the HTTP address where the telemetry is sent.
        /// </summary>
        public string EndpointAddress
        {
            get { return this.TelemetrySerializer.EndpointAddress?.ToString(); }
            set { this.TelemetrySerializer.EndpointAddress = new Uri(value); }
        }

        /// <summary>
        /// Gets or Sets the subscriber to an event with Transmission and HttpWebResponseWrapper.
        /// </summary>
        public EventHandler<TransmissionStatusEventArgs> TransmissionStatusEvent
        {
            get
            {
                return this.TelemetrySerializer.TransmissionStatusEvent;
            }

            set
            {
                this.TelemetrySerializer.TransmissionStatusEvent = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum telemetry batching interval. Once the interval expires, <see cref="TelemetryChannel"/> 
        /// serializes the accumulated telemetry items for transmission.
        /// </summary>
        public TimeSpan MaxTelemetryBufferDelay
        {
            get { return this.TelemetryBuffer.MaxTransmissionDelay; }
            set { this.TelemetryBuffer.MaxTransmissionDelay = value; }
        }

        /// <summary>
        /// Gets or sets the maximum number of telemetry items will accumulate in a memory before 
        /// the <see cref="TelemetryChannel"/> serializing them for transmission to Application Insights.
        /// This is not a hard limit on how many unsent items can be in the buffer.
        /// </summary>
        public int MaxTelemetryBufferCapacity
        {
            get { return this.TelemetryBuffer.Capacity; }
            set { this.TelemetryBuffer.Capacity = value; }
        }

        /// <summary>
        /// Gets or sets the maximum number of telemetry items that can be in the backlog to send. This is a hard limit
        /// and Items will be dropped by the <see cref="ServerTelemetryChannel"/> once this limit is hit until items
        /// are drained from the buffer.
        /// </summary>
        public int MaxBacklogSize
        {
            get { return this.TelemetryBuffer.BacklogSize; }
            set { this.TelemetryBuffer.BacklogSize = value; }
        }

        /// <summary>
        /// Gets or sets the maximum amount of memory, in bytes, that <see cref="TelemetryChannel"/> will use 
        /// to buffer transmissions before sending them to Application Insights.
        /// </summary>
        public int MaxTransmissionBufferCapacity
        {
            get { return this.Transmitter.MaxBufferCapacity; }
            set { this.Transmitter.MaxBufferCapacity = value; }
        }

        /// <summary>
        /// Gets or sets the maximum number of telemetry transmissions that <see cref="TelemetryChannel"/> will 
        /// send to Application Insights at the same time.
        /// </summary>
        public int MaxTransmissionSenderCapacity
        {
            get { return this.Transmitter.MaxSenderCapacity; }
            set { this.Transmitter.MaxSenderCapacity = value; }
        }

        /// <summary>
        /// Gets or sets the maximum amount of disk space, in bytes, that <see cref="TelemetryChannel"/> will 
        /// use to store unsent telemetry transmissions.
        /// </summary>
        public long MaxTransmissionStorageCapacity
        {
            get { return this.Transmitter.MaxStorageCapacity; }
            set { this.Transmitter.MaxStorageCapacity = value; }
        }

        /// <summary>
        /// Gets or sets the folder to be used as a temporary storage for events that were not sent because of temporary connectivity issues. 
        /// It is the user's responsibility to put appropriate security permissions to this folder.
        /// If folder was not provided or inaccessible. %LocalAppData% or %Temp% folder will be used in Windows systems.
        /// For Non-Windows systems, providing this folder with write access to the process is required. If not provided or not accessible, 
        /// telemetry items will be dropped if there are temporary network issues.
        /// </summary>
        public string StorageFolder
        {
            get { return this.Transmitter.StorageFolder; }
            set { this.Transmitter.StorageFolder = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a limiter on the maximum number of <see cref="ITelemetry"/> objects 
        /// that can be sent in a given throttle window is enabled. Items attempted to be sent exceeding of the local 
        /// throttle amount will be treated the same as a backend throttle.
        /// </summary>
        public bool EnableLocalThrottling
        {
            get { return this.Transmitter.ApplyThrottle; }
            set { this.Transmitter.ApplyThrottle = value; }
        }

        /// <summary>
        /// Gets or sets the maximum number of items that will be allowed to send in a given throttle window.
        /// </summary>
        public int LocalThrottleLimit
        {
            get { return this.Transmitter.ThrottleLimit; }
            set { this.Transmitter.ThrottleLimit = value; }
        }

        /// <summary>
        /// Gets or sets the size of the self-limiting throttle window in milliseconds.
        /// </summary>
        public int LocalThrottleWindow
        {
            get { return this.Transmitter.ThrottleWindow; }
            set { this.Transmitter.ThrottleWindow = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="CredentialEnvelope"/> which is used for AAD.
        /// </summary>
        /// <remarks>
        /// <see cref="ISupportCredentialEnvelope.CredentialEnvelope"/> on <see cref="ServerTelemetryChannel"/> sets <see cref="Transmitter.CredentialEnvelope"/> and then sets <see cref="TransmissionSender.CredentialEnvelope"/> 
        /// which is used to set <see cref="Transmission.CredentialEnvelope"/> just before calling <see cref="Transmission.SendAsync"/>.
        /// </remarks>
        CredentialEnvelope ISupportCredentialEnvelope.CredentialEnvelope
        {
            get => this.Transmitter.CredentialEnvelope;
            set 
            {
                this.Transmitter.CredentialEnvelope = value;
                this.policies.EnableAuthenticationPolicy();
            }
        }

        /// <summary>
        /// Gets or sets first TelemetryProcessor in processor call chain.
        /// </summary>
        internal ITelemetryProcessor TelemetryProcessor
        {
            get
            {
                return this.telemetryProcessor;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                this.telemetryProcessor = value;
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            // Tested by FxCop rule CA2213
            this.TelemetryBuffer.Dispose();
            this.Transmitter.Dispose();
            this.policies.Dispose();
        }

        /// <summary>
        /// Sends an instance of ITelemetry through the channel.
        /// </summary>
        public void Send(ITelemetry item)
        {
            if (!this.isInitialized)
            {
                TelemetryChannelEventSource.Log.StorageNotInitializedError();
            }

            if (item != null)
            {
                if (string.IsNullOrEmpty(item.Context.InstrumentationKey))
                {
                    if (TelemetryChannelEventSource.IsVerboseEnabled)
                    {
                        TelemetryChannelEventSource.Log.ItemRejectedNoInstrumentationKey(item.ToString());
                    }
                    else
                    {
                        if (!Debugger.IsAttached)
                        {
                            this.throttleEmptyIkeyLog.PerformThrottledAction(() => TelemetryChannelEventSource.Log.TelemetryChannelNoInstrumentationKey());
                        }
                    }

                    return;
                }

                if (TelemetryChannelEventSource.IsVerboseEnabled)
                {
                    TelemetryChannelEventSource.Log.TelemetryChannelSend(
                        item.ToString(),
                        item.Context.InstrumentationKey.Substring(0, Math.Min(item.Context.InstrumentationKey.Length, 8)));
                }

                this.TelemetryProcessor.Process(item);
            }
        }

        /// <summary>
        /// Asynchronously flushes the telemetry buffer. 
        /// </summary>
        public void Flush()
        {
            if (!this.isInitialized)
            {
                TelemetryChannelEventSource.Log.StorageNotInitializedError();
            }

            TelemetryChannelEventSource.Log.TelemetryChannelFlush();
            this.TelemetryBuffer.FlushAsync().ConfigureAwait(false).GetAwaiter().GetResult(); // Don't use Task.Wait() because it wraps the original exception in an AggregateException.
        }

        /// <summary>
        /// Asynchronously flushes the telemetry buffer. 
        /// </summary>
        /// <returns>
        /// Returns true when telemetry data is transferred out of process (application insights server or local storage) and are emitted before the flush invocation.
        /// Returns false when transfer of telemetry data to server has failed with non-retriable http status code.
        /// </returns>
        public Task<bool> FlushAsync(CancellationToken cancellationToken)
        {
            if (!this.isInitialized)
            {
                TelemetryChannelEventSource.Log.StorageNotInitializedError();
            }

            TelemetryChannelEventSource.Log.TelemetryChannelFlushAsync();
            return cancellationToken.IsCancellationRequested ? TaskEx.FromCanceled<bool>(cancellationToken) : this.TelemetryBuffer.FlushAsync(cancellationToken);
        }

        /// <summary>
        /// Initialize method is called after all configuration properties have been loaded from the configuration.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings", Justification = "Private variable, low risk.")]
        public void Initialize(TelemetryConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            ((ISupportCredentialEnvelope)this).CredentialEnvelope = configuration.CredentialEnvelope;

            this.Transmitter.Initialize();

            if (this.EndpointAddress == null)
            {
                this.EndpointAddress = new Uri(configuration.EndpointContainer.Ingestion, "v2/track").AbsoluteUri;
            }

            // ApplyPolicies will synchronously get list of file names from disk and calculate size
            // Creating task to improve application startup time
            ExceptionHandler.Start(() => { return Task.Run(() => this.Transmitter.ApplyPolicies()); });

            this.isInitialized = true;
        }
    }
}
