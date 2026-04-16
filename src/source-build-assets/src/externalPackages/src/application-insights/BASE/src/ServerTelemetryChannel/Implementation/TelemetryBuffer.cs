namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
    /// Accumulates <see cref="ITelemetry"/> items for efficient transmission.
    /// </summary>
    [SuppressMessage("Microsoft.Reliability", "CA2002:DoNotLockObjectsWithWeakIdentity", Justification = "This should be removed, but there is currently a dependency on this behavior.")]
    internal class TelemetryBuffer : IEnumerable<ITelemetry>, ITelemetryProcessor, IDisposable
    {
        private static readonly TimeSpan DefaultFlushDelay = TimeSpan.FromSeconds(30);

        private readonly TaskTimerInternal flushTimer;
        private readonly TelemetrySerializer serializer;

        private int capacity = 500;
        private int backlogSize = 1000000;
        private int minimumBacklogSize = 1001;
        private bool itemDroppedMessageLogged = false;
        private List<ITelemetry> itemBuffer;

        public TelemetryBuffer(TelemetrySerializer serializer, IApplicationLifecycle applicationLifecycle)
            : this()
        {
            if (serializer == null)
            {
                throw new ArgumentNullException(nameof(serializer));
            }

#if NETFRAMEWORK
            // We don't have implementation for IApplicationLifecycle for .NET Core
            if (applicationLifecycle == null)
            {
                throw new ArgumentNullException(nameof(applicationLifecycle));
            }
#endif

            if (applicationLifecycle != null)
            {
                applicationLifecycle.Stopping += this.HandleApplicationStoppingEvent;
            }

            this.serializer = serializer;
        }

        protected TelemetryBuffer()
        {
            this.flushTimer = new TaskTimerInternal { Delay = DefaultFlushDelay };
            this.itemBuffer = new List<ITelemetry>(this.Capacity);
        }

        /// <summary>
        /// Gets or sets the maximum number of telemetry items that can be buffered before transmission.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The value is zero or less.</exception>
        /// <exception cref="ArgumentException">The value is greater than the MaximumBacklogSize.</exception>
        public int Capacity
        {
            get
            {
                return this.capacity;
            }

            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Capacity must be greater than 0");
                }

                if (value > this.backlogSize)
                {
                    throw new ArgumentException("Capacity cannot be greater than MaximumBacklogSize", nameof(value));
                }

                this.capacity = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of telemetry items that can be in the backlog to send. Items will be dropped
        /// once this limit is hit.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The value is zero or less.</exception>
        /// <exception cref="ArgumentException">The value is less than the Capacity.</exception>
        public int BacklogSize
        {
            get
            {
                return this.backlogSize;
            }

            set
            {
                if (value < this.minimumBacklogSize)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                if (value < this.capacity)
                {
                    throw new ArgumentException(nameof(this.BacklogSize) + " cannot be lower than capacity", nameof(value));
                }

                this.backlogSize = value;
            }
        }

        public TimeSpan MaxTransmissionDelay 
        {
            get { return this.flushTimer.Delay; }
            set { this.flushTimer.Delay = value; }
        }

        /// <summary>
        /// Releases resources used by this <see cref="TelemetryBuffer"/> instance.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        /// <summary>
        /// Processes the specified <paramref name="item"/> item.
        /// </summary>
        /// <exception cref="ArgumentNullException">The <paramref name="item"/> is null.</exception>
        public virtual void Process(ITelemetry item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (!this.flushTimer.IsStarted)
            {
                this.flushTimer.Start(this.FlushAsync);
            }

            lock (this)
            {
                if (this.itemBuffer.Count >= this.BacklogSize)
                {
                    if (!this.itemDroppedMessageLogged)
                    {
                        TelemetryChannelEventSource.Log.ItemDroppedAsMaximumUnsentBacklogSizeReached(this.BacklogSize);
                        this.itemDroppedMessageLogged = true;
                    }

                    return;
                }

                this.itemBuffer.Add(item);
                if (this.itemBuffer.Count >= this.Capacity)
                {
                    ExceptionHandler.Start(this.FlushAsync);
                }
            }
        }

        /// <summary>
        /// Passes all <see cref="ITelemetry"/> items to the <see cref="TelemetrySerializer"/> and empties the queue.
        /// </summary>
        public virtual async Task FlushAsync()
        {
            List<ITelemetry> telemetryToFlush = this.GetBufferTelemetryAndResetBuffer();

            if (telemetryToFlush != null)
            {
                TelemetryChannelEventSource.Log.SerializationStarted(telemetryToFlush.Count);

                // Flush on thread pull to offload the rest of the channel logic from the customer's thread.
                // This also works around the problem in ASP.NET 4.0, does not support await and SynchronizationContext correctly.
                // See also: http://www.bing.com/search?q=UseTaskFriendlySynchronizationContext
                await Task.Run(() => this.serializer.Serialize(telemetryToFlush)).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Passes all <see cref="ITelemetry"/> items to the <see cref="TelemetrySerializer"/>, empties the queue and returns a task.
        /// </summary>
        public virtual Task<bool> FlushAsync(CancellationToken cancellationToken)
        {
            List<ITelemetry> telemetryToFlush = this.GetBufferTelemetryAndResetBuffer();

            if (!cancellationToken.IsCancellationRequested)
            {
                return this.serializer.SerializeAsync(telemetryToFlush, cancellationToken);
            }

            return TaskEx.FromCanceled<bool>(cancellationToken);
        }

        public IEnumerator<ITelemetry> GetEnumerator()
        {
            return this.itemBuffer.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private List<ITelemetry> GetBufferTelemetryAndResetBuffer()
        {
            List<ITelemetry> telemetryToFlush = null;
            if (this.itemBuffer.Count > 0)
            {
                lock (this)
                {
                    if (this.itemBuffer.Count > 0)
                    {
                        this.flushTimer.Cancel();
                        telemetryToFlush = this.itemBuffer;
                        this.itemBuffer = new List<ITelemetry>(this.Capacity);
                        this.itemDroppedMessageLogged = false;
                    }
                }
            }

            return telemetryToFlush;
        }

        private void HandleApplicationStoppingEvent(object sender, ApplicationStoppingEventArgs e)
        {
            e.Run(this.FlushAsync);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.flushTimer.Dispose();
            }
        }
    }
}
