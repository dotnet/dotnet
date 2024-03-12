namespace Microsoft.ApplicationInsights.Extensibility
{
    using System;
    using System.Collections.ObjectModel;
    using System.Threading;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// Represents a destination for telemetry, consisting of a set of telemetry processors and a channel.
    /// </summary>
    public sealed class TelemetrySink : IDisposable, ITelemetryModule
    {
        /// <summary>
        /// The name to use for the default telemetry sink when specifying its properties through configuration.
        /// </summary>
        /// <remarks>The name is not case-sensitive.</remarks>
        public static readonly string DefaultSinkName = "default";

        private TelemetryConfiguration telemetryConfiguration;
        private ITelemetryChannel telemetryChannel;
        private bool shouldDisposeChannel;
        private TelemetryProcessorChain telemetryProcessorChain;
        private TelemetryProcessorChainBuilder telemetryProcessorChainBuilder;
        private string name;
        private bool isDisposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetrySink"/> class.
        /// </summary>
        /// <param name="telemetryConfiguration">Telemetry configuration to use for the new <see cref="TelemetrySink"/> instance.</param>
        /// <param name="telemetryChannel">Telemetry channel to use for the new <see cref="TelemetrySink"/> instance.</param>
        public TelemetrySink(TelemetryConfiguration telemetryConfiguration, ITelemetryChannel telemetryChannel = null)
        {
            this.telemetryConfiguration = telemetryConfiguration ?? throw new ArgumentNullException(nameof(telemetryConfiguration));

            if (telemetryChannel != null)
            {
                this.telemetryChannel = telemetryChannel;
                this.shouldDisposeChannel = false;
            }
            else
            {
                this.telemetryChannel = new InMemoryChannel
                {
                    EndpointAddress = telemetryConfiguration.EndpointContainer.FormattedIngestionEndpoint,
                };
                this.shouldDisposeChannel = true;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetrySink"/> class. 
        /// </summary>
        public TelemetrySink()
        {
            this.telemetryChannel = new InMemoryChannel();
            this.shouldDisposeChannel = true;
        }

        /// <summary>
        /// Gets or sets an instance of the <see cref="TelemetryProcessorChainBuilder"/> that this sink is using.
        /// </summary>
        public TelemetryProcessorChainBuilder TelemetryProcessorChainBuilder
        {
            get
            {
                return LazyInitializer.EnsureInitialized(ref this.telemetryProcessorChainBuilder, () => new TelemetryProcessorChainBuilder(this.telemetryConfiguration, this));
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                this.EnsureNotDisposed();

                if (!object.ReferenceEquals(value.TelemetrySink, this))
                {
                    throw new ArgumentException("The passed TelemetryProcessorChainBuilder has been configured to use a different TelemetrySink instance", nameof(value));
                }

                if (this.telemetryProcessorChain != null && this.telemetryProcessorChainBuilder != null && !object.ReferenceEquals(this.telemetryProcessorChainBuilder, value))
                {
                    // If a new builder is assigned to the sink, dispose the old chain, giving the new builder a chance to build it.
                    this.telemetryProcessorChain.Dispose();
                    this.telemetryProcessorChain = null;
                }

                this.telemetryProcessorChainBuilder = value;
            }
        }

        /// <summary>
        /// Gets or sets the telemetry channel.
        /// </summary>
        public ITelemetryChannel TelemetryChannel
        {
            get => this.telemetryChannel;
            set
            {
                this.EnsureNotDisposed();

                ITelemetryChannel oldChannel = this.telemetryChannel;
                this.telemetryChannel = value;

                // If we have a previously assigned channel which was created by us and is not the same one as the
                // "new" value passed in then we need to dispose of the old channel to keep from leaking resources.
                if (oldChannel != null && oldChannel != value && this.shouldDisposeChannel)
                {
                    oldChannel.Dispose();
                    this.shouldDisposeChannel = false; // The new one wasn't created by us so it should be managed by whoever created it.
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the sink.
        /// </summary>
        public string Name { get => this.name; set => this.name = value; }

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

        internal TelemetryProcessorChain TelemetryProcessorChain
        {
            get
            {
                if (this.telemetryProcessorChain == null && !this.isDisposed)
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

                this.EnsureNotDisposed();

                this.telemetryProcessorChain = value;
            }
        }

        /// <summary>
        /// Releases resources used by the instance of <see cref="TelemetrySink"/> class.
        /// </summary>
        public void Dispose()
        {
            this.isDisposed = true;

            if (this.shouldDisposeChannel)
            {
                this.telemetryChannel?.Dispose();
            }

            this.telemetryChannel = null;

            this.telemetryProcessorChain?.Dispose();
            this.telemetryProcessorChain = null;
        }

        /// <summary>
        /// Initializes the sink.
        /// </summary>
        /// <param name="configuration">Telemetry configuration to be used during sink operation.</param>
        public void Initialize(TelemetryConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            this.EnsureNotDisposed();

            this.telemetryConfiguration = configuration;

            (this.telemetryChannel as ITelemetryModule)?.Initialize(configuration);
            foreach (var telemetryProcessor in this.TelemetryProcessorChain.TelemetryProcessors)
            {
                (telemetryProcessor as ITelemetryModule)?.Initialize(configuration);
            }
        }

        /// <summary>
        /// Processes a collected telemetry item.
        /// </summary>
        /// <param name="item">Item to process.</param>
        public void Process(ITelemetry item)
        {
            if (this.isDisposed)
            {
                CoreEventSource.Log.TelemetrySinkCalledAfterBeingDisposed();
                return;
            }

            this.TelemetryProcessorChain.Process(item);
        }

        private void EnsureNotDisposed()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.Name);
            }
        }
    }
}
