namespace Microsoft.ApplicationInsights.Channel
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Authentication;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// Represents a communication channel for sending telemetry to Application Insights via HTTPS. There will be a buffer that will not be persisted, to enforce the 
    /// queued telemetry items to be sent, <see cref="ITelemetryChannel.Flush"/> should be called.    
    /// </summary>
    public class InMemoryChannel : ITelemetryChannel, IAsyncFlushable, ISupportCredentialEnvelope
    {
        private readonly TelemetryBuffer buffer;
        private readonly InMemoryTransmitter transmitter;

        private readonly InterlockedThrottle throttleEmptyIkeyLog = new InterlockedThrottle(interval: TimeSpan.FromSeconds(30));

        private bool? developerMode = false;
        private int bufferSize;

        /// <summary>
        /// Indicates if this instance has been disposed of.
        /// </summary>
        private bool isDisposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryChannel" /> class.
        /// </summary>
        public InMemoryChannel()
        {
            this.buffer = new TelemetryBuffer();
            this.transmitter = new InMemoryTransmitter(this.buffer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryChannel" /> class. Used in unit tests for constructor injection.  
        /// </summary>
        /// <param name="telemetryBuffer">The telemetry buffer that will be used to enqueue new events.</param>
        /// <param name="transmitter">The in memory transmitter that will send the events queued in the buffer.</param>
        internal InMemoryChannel(TelemetryBuffer telemetryBuffer, InMemoryTransmitter transmitter)
        {
            this.buffer = telemetryBuffer;
            this.transmitter = transmitter;
        }

        /// <summary>
        /// Gets or sets a value indicating whether developer mode of telemetry transmission is enabled.
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
                        this.bufferSize = this.buffer.Capacity;
                        this.buffer.Capacity = 1;
                    }
                    else
                    {
                        this.buffer.Capacity = this.bufferSize;
                    }

                    this.developerMode = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the sending interval. Once the interval expires, <see cref="InMemoryChannel"/> 
        /// serializes the accumulated telemetry items for transmission and sends it over the wire.
        /// </summary>    
        public TimeSpan SendingInterval
        {
            get
            {
                return this.transmitter.SendingInterval;
            }

            set
            {
                this.transmitter.SendingInterval = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="CredentialEnvelope"/> which is used for AAD.
        /// </summary>
        /// <remarks>
        /// <see cref="ISupportCredentialEnvelope.CredentialEnvelope"/> on <see cref="InMemoryChannel"/> sets <see cref="InMemoryTransmitter.CredentialEnvelope"/> 
        /// which is used to set <see cref="Transmission.CredentialEnvelope"/> just before calling <see cref="Transmission.SendAsync"/>.
        /// </remarks>
        CredentialEnvelope ISupportCredentialEnvelope.CredentialEnvelope
        {
            get => this.transmitter.CredentialEnvelope;
            set => this.transmitter.CredentialEnvelope = value;
        }

        /// <summary>
        /// Gets or sets the HTTP address where the telemetry is sent.
        /// </summary>
        public string EndpointAddress
        {
            get { return this.transmitter.EndpointAddress?.ToString(); }
            set { this.transmitter.EndpointAddress = new Uri(value); }
        }

        /// <summary>
        /// Gets or sets the maximum number of telemetry items will accumulate in a memory before 
        /// the <see cref="InMemoryChannel"/> serializing them for transmission to Application Insights.
        /// This is not a hard limit on how many unsent items can be in the buffer.
        /// </summary>
        public int MaxTelemetryBufferCapacity
        {
            get { return this.buffer.Capacity; }
            set { this.buffer.Capacity = value; }
        }

        /// <summary>
        /// Gets or sets the maximum number of telemetry items that can be in the backlog to send. This is a hard limit
        /// and Items will be dropped by the <see cref="InMemoryChannel"/> once this limit is hit until items are drained from the buffer.
        /// </summary>
        public int BacklogSize
        {
            get { return this.buffer.BacklogSize; }
            set { this.buffer.BacklogSize = value; }
        }

        internal bool IsDisposed => this.isDisposed;

        /// <summary>
        /// Sends an instance of ITelemetry through the channel.
        /// </summary>
        public void Send(ITelemetry item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (this.isDisposed)
            {
                CoreEventSource.Log.InMemoryChannelSendCalledAfterBeingDisposed();
                return;
            }

            if (string.IsNullOrEmpty(item.Context.InstrumentationKey))
            {
                if (CoreEventSource.IsVerboseEnabled)
                {
                    CoreEventSource.Log.ItemRejectedNoInstrumentationKey(item.ToString());
                }
                else
                {
                    if (!Debugger.IsAttached)
                    {
                        this.throttleEmptyIkeyLog.PerformThrottledAction(() => CoreEventSource.Log.TelemetryChannelNoInstrumentationKey());
                    }
                }

                return;
            }

            try
            {
                this.buffer.Enqueue(item);
            }
            catch (Exception e)
            {
                CoreEventSource.Log.LogVerbose("TelemetryBuffer.Enqueue failed: " + e.ToString());
            }
        }

        /// <summary>
        /// Will send all the telemetry items stored in the memory.
        /// </summary>
        public void Flush()
        {
            this.Flush(default(TimeSpan)); // when default(TimeSpan) is provided, value is ignored and default timeout of 100 sec is used
        }

        /// <summary>
        /// Will send all the telemetry items stored in the memory.
        /// </summary>
        /// <param name="timeout">Timeout interval to abort sending.</param>
        public void Flush(TimeSpan timeout)
        {
            this.transmitter.Flush(timeout);
            if (this.isDisposed)
            {
                CoreEventSource.Log.InMemoryChannelFlushedAfterBeingDisposed();
            }
        }

        /// <summary>
        /// Will send all the telemetry items stored in the memory asynchronously.
        /// </summary>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <returns>
        /// This method is hard-coded to return true. This channel offers minimal reliability guarantees and doesn't retry sending telemetry after a failure.
        /// </returns>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/azure/azure-monitor/app/telemetry-channels#built-in-telemetry-channels">Learn more</a>
        /// </remarks>
        public Task<bool> FlushAsync(CancellationToken cancellationToken)
        {
            return Task<bool>.Run(() =>
            {
                this.Flush(default(TimeSpan)); // when default(TimeSpan) is provided, value is ignored and default timeout of 100 sec is used
                return Task.FromResult(true);
            }, cancellationToken);
        }

        /// <summary>
        /// Disposing the channel.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose the channel if not already disposed.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !this.isDisposed)
            {
                this.isDisposed = true;
                if (this.transmitter != null)
                {
                    this.transmitter.Dispose();
                }
            }
        }
    }
}
