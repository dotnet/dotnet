namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;

    /// <summary>
    /// Base class for telemetry types representing duration in time.
    /// </summary>
    public abstract class OperationTelemetry : ITelemetry, ISupportMetrics, ISupportProperties
    {
        internal const string TelemetryName = "Operation";

        /// <summary>
        /// Gets or sets the start time of the operation.
        /// </summary>
        [Obsolete("Use Timestamp")]
        public DateTimeOffset StartTime
        {
            get
            {
                return this.Timestamp;
            }

            set
            {
                this.Timestamp = value;
            }
        }

        /// <summary>  
        /// Gets or sets Operation ID.
        /// </summary>  
        public abstract string Id { get; set; }        

        /// <summary>
        /// Gets or sets the name of the operation.
        /// </summary>
        public abstract string Name { get; set; }

        /// <summary>
        /// Gets or sets whether operation has finished successfully.
        /// </summary>
        public abstract bool? Success { get; set; }

        /// <summary>
        /// Gets or sets the duration of the operation.
        /// </summary>
        public abstract TimeSpan Duration { get; set;  }

        /// <summary>
        /// Gets the custom metrics collection.
        /// </summary>
        public abstract IDictionary<string, double> Metrics { get; }

        /// <summary>
        /// Gets the custom properties collection.
        /// </summary>
        public abstract IDictionary<string, string> Properties { get; }

        /// <summary>
        /// Gets or sets the timestamp for the operation.
        /// </summary>
        public abstract DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// Gets the object that contains contextual information about the application at the time when it handled the request.
        /// </summary>
        public abstract TelemetryContext Context { get; }

        /// <summary>
        /// Gets or sets the value that defines absolute order of the telemetry item.
        /// </summary>
        public abstract string Sequence { get; set; }

        /// <summary>
        /// Gets or sets gets the extension used to extend this telemetry instance using new strong typed object.
        /// </summary>
        public abstract IExtension Extension { get; set; }

        /// <summary>  
        /// Gets or sets Time in StopWatch ticks representing begin time of the operation. Used internally
        /// for calculating duration between begin and end.
        /// </summary>  
        internal long BeginTimeInTicks { get; set; }

        /// <summary>
        /// Sanitizes the properties based on constraints.
        /// </summary>
        void ITelemetry.Sanitize()
        {
            this.Sanitize();
        }

        /// <summary>
        /// Deeply clones a <see cref="OperationTelemetry"/> object.
        /// </summary>
        /// <returns>A cloned instance.</returns>
        public abstract ITelemetry DeepClone();

        /// <inheritdoc/>
        public abstract void SerializeData(ISerializationWriter serializationWriter);

        /// <summary>
        /// Sets operation Id.
        /// </summary>
        internal void GenerateId()
        {
            this.Id = W3C.W3CUtilities.GenerateSpanId();
        }

        /// <summary>
        /// Allow to call OperationTelemetry.Sanitize method from child classes.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "This method is expected to be overloaded")]
        protected void Sanitize()
        {
        }
    }
}
