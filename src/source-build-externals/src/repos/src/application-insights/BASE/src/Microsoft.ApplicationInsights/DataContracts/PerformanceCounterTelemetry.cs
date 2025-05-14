namespace Microsoft.ApplicationInsights.DataContracts
{
    using System;
    using System.Collections.Generic;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
    /// The class that represents information about performance counters.
    /// </summary>
    [Obsolete("Use MetricTelemetry instead.")]
    public sealed class PerformanceCounterTelemetry : ITelemetry, ISupportProperties, IAiSerializableTelemetry
    {
        internal readonly MetricTelemetry Data;        
        private string categoryName = string.Empty;
        private string counterName = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceCounterTelemetry"/> class.
        /// </summary>
        public PerformanceCounterTelemetry()
        {
            this.Data = new MetricTelemetry();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceCounterTelemetry"/> class.
        /// </summary>
        /// <param name="categoryName">Category name.</param>
        /// <param name="counterName">Performance counter name.</param>
        /// <param name="instanceName">Instance name.</param>
        /// <param name="value">Performance counter value.</param>
        public PerformanceCounterTelemetry(string categoryName, string counterName, string instanceName, double value)
            : this()
        {
            this.CategoryName = categoryName;
            this.CounterName = counterName;
            this.InstanceName = instanceName;
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceCounterTelemetry"/> class by cloning an existing instance.
        /// </summary>
        /// <param name="source">Source instance of <see cref="PerformanceCounterTelemetry"/> to clone from.</param>
        private PerformanceCounterTelemetry(PerformanceCounterTelemetry source)
        {
            this.Data = (MetricTelemetry)source.Data.DeepClone();
            this.categoryName = source.categoryName;
            this.counterName = source.counterName;
        }
        
        /// <inheritdoc />
        string IAiSerializableTelemetry.TelemetryName
        {
            get
            {
                return ((IAiSerializableTelemetry)this.Data).TelemetryName;
            }

            set
            {
                ((IAiSerializableTelemetry)this.Data).TelemetryName = value;
            }
        }

        /// <inheritdoc />
        string IAiSerializableTelemetry.BaseType => ((IAiSerializableTelemetry)this.Data).BaseType;

        /// <summary>
        /// Gets or sets date and time when telemetry was recorded.
        /// </summary>
        public DateTimeOffset Timestamp
        {
            get
            {
                return this.Data.Timestamp;
            }

            set
            {
                this.Data.Timestamp = value;
            }
        }

        /// <summary>
        /// Gets or sets the value that defines absolute order of the telemetry item.
        /// </summary>
        public string Sequence
        {
            get
            {
                return this.Data.Sequence;
            }

            set
            {
                this.Data.Sequence = value;
            }
        }

        /// <summary>
        /// Gets the context associated with the current telemetry item.
        /// </summary>
        public TelemetryContext Context
        {
            get
            {
                return this.Data.Context;
            }
        }

        /// <summary>
        /// Gets or sets gets the extension used to extend this telemetry instance using new strong typed object.
        /// </summary>
        public IExtension Extension
        {
            get { return this.Data.Extension; }
            set { this.Data.Extension = value; }
        }

        /// <summary>
        /// Gets or sets the counter value.
        /// </summary>
        public double Value
        {
            get
            {
                return this.Data.Value;
            }

            set
            {
                this.Data.Value = value;
            }
        }

        /// <summary>
        /// Gets or sets the category name.
        /// </summary>
        public string CategoryName
        {
            get
            {
                return this.categoryName;
            }

            set
            {
                this.categoryName = value;
                this.UpdateName();
            }
        }

        /// <summary>
        /// Gets or sets the counter name.
        /// </summary>
        public string CounterName
        {
            get
            {
                return this.counterName;
            }

            set
            {
                this.counterName = value;
                this.UpdateName();
            }
        }

        /// <summary>
        /// Gets or sets the instance name.
        /// </summary>
        public string InstanceName
        {
            get
            {
                if (this.Properties.ContainsKey("CounterInstanceName"))
                {
                    return this.Properties["CounterInstanceName"];
                }

                return string.Empty;
            }

            set
            {
                this.Properties["CounterInstanceName"] = value;
                this.UpdateName();
            }
        }

        /// <summary>
        /// Gets a dictionary of application-defined property names and values providing additional information about this exception.
        /// </summary>
        public IDictionary<string, string> Properties
        {
            get { return this.Data.Properties; }
        }

        /// <summary>
        /// Deeply clones a <see cref="PerformanceCounterTelemetry"/> object.
        /// </summary>
        /// <returns>A cloned instance.</returns>
        public ITelemetry DeepClone()
        {
            return new PerformanceCounterTelemetry(this);
        }

        /// <inheritdoc/>
        public void SerializeData(ISerializationWriter serializationWriter)
        {
            this.Data.SerializeData(serializationWriter);
        }

        /// <summary>
        /// Sanitizes the properties based on constraints.
        /// </summary>
        void ITelemetry.Sanitize()
        {
            ((ITelemetry)this.Data).Sanitize();
        }

        private void UpdateName()
        {
            if (this.categoryName == "Processor")
            {
                this.Data.Name = "\\" + this.categoryName + "(_Total)\\" + this.counterName;
            }
            else if (this.categoryName == "Process")
            {
                this.Data.Name = "\\" + this.categoryName + "(??APP_WIN32_PROC??)\\" + this.counterName;
            }
            else if (this.categoryName == "ASP.NET Applications")
            {
                this.Data.Name = "\\" + this.categoryName + "(??APP_W3SVC_PROC??)\\" + this.counterName;
            }
            else if (this.categoryName == ".NET CLR Exceptions")
            {
                this.Data.Name = "\\" + this.categoryName + "(??APP_CLR_PROC??)\\" + this.counterName;
            }
            else
            {
                this.Data.Name = string.IsNullOrEmpty(this.InstanceName) ?
                    this.Data.Name = "\\" + this.categoryName + "\\" + this.counterName :
                    this.Data.Name = "\\" + this.categoryName + "(" + this.InstanceName + ")\\" + this.counterName;
            }
        }
    }
}
