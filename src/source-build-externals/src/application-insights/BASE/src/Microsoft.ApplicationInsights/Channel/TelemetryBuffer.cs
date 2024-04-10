namespace Microsoft.ApplicationInsights.Channel
{
    using System;
    using System.Collections.Generic;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// Accumulates <see cref="ITelemetry"/> items for efficient transmission.
    /// </summary>
    internal class TelemetryBuffer
    {
        /// <summary>
        /// Delegate that is raised when the buffer is full.
        /// </summary>
        public Action OnFull;

        private const int DefaultCapacity = 500;
        private const int DefaultBacklogSize = 1000000;        
        private readonly object lockObj = new object();
        private int capacity = DefaultCapacity;
        private int backlogSize = DefaultBacklogSize;
        private int minimumBacklogSize = 1001;
        private List<ITelemetry> items;
        private bool itemDroppedMessageLogged = false;

        internal TelemetryBuffer()
        {
            this.items = new List<ITelemetry>(this.Capacity);
        }

        /// <summary>
        /// Gets or sets the maximum number of telemetry items that can be buffered before transmission.
        /// </summary>        
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
                    this.capacity = DefaultCapacity;
                    return;
                }

                if (value > this.backlogSize)
                {
                    this.capacity = this.backlogSize;
                    return;
                }

                this.capacity = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of telemetry items that can be in the backlog to send. Items will be dropped
        /// once this limit is hit.
        /// </summary>        
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
                    this.backlogSize = this.minimumBacklogSize;
                    return;
                }

                if (value < this.capacity)
                {
                    this.backlogSize = this.capacity;
                    return;
                }

                this.backlogSize = value;
            }
        }

        public void Enqueue(ITelemetry item)
        {
            if (item == null)
            {
                CoreEventSource.Log.LogVerbose("item is null in TelemetryBuffer.Enqueue");
                return;
            }

            lock (this.lockObj)
            {
                if (this.items.Count >= this.BacklogSize)
                {
                    if (!this.itemDroppedMessageLogged)
                    {
                        CoreEventSource.Log.ItemDroppedAsMaximumUnsentBacklogSizeReached(this.BacklogSize);
                        this.itemDroppedMessageLogged = true;
                    }

                    return;
                }

                this.items.Add(item);
                if (this.items.Count >= this.Capacity)
                {
                    var onFull = this.OnFull;
                    if (onFull != null)
                    {
                        onFull();
                    }
                }
            }
        }

        public virtual IEnumerable<ITelemetry> Dequeue()
        {
            List<ITelemetry> telemetryToFlush = null;

            if (this.items.Count > 0)
            {
                lock (this.lockObj)
                {
                    if (this.items.Count > 0)
                    {
                        telemetryToFlush = this.items;
                        this.items = new List<ITelemetry>(this.Capacity);
                        this.itemDroppedMessageLogged = false;
                    }
                }
            }

            return telemetryToFlush;
        }
    }
}
