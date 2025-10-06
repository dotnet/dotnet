namespace Microsoft.ApplicationInsights.DataContracts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;

    /// <summary>
    /// Telemetry type used to track metrics. Represents a sample set of values with a specified count, sum, max, min, and standard deviation.
    /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#trackmetric">Learn more</a>
    /// </summary>
    public sealed class MetricTelemetry : ITelemetry, ISupportProperties, IAiSerializableTelemetry
    {
        internal const string EtwEnvelopeName = "Metric";
        internal readonly MetricData Data;
        internal readonly DataPoint Metric;
        internal string EnvelopeName = "AppMetrics";
        private IExtension extension;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetricTelemetry"/> class with empty
        /// properties.
        /// </summary>
        public MetricTelemetry()
        {
            this.Data = new MetricData();
            this.Metric = new DataPoint();
            this.Context = new TelemetryContext(this.Data.properties);

            this.Metric.kind = DataPointType.Aggregation;

            // We always have a single 'metric'.
            this.Data.metrics.Add(this.Metric);
        }

        /// <summary>
        /// Obsolete - use MetricTelemetry(name,count,sum,min,max,standardDeviation). Initializes a new instance of the <see cref="MetricTelemetry"/> class with the
        /// specified <paramref name="metricName"/> and <paramref name="metricValue"/>.
        /// </summary>
        /// <exception cref="ArgumentException">The <paramref name="metricName"/> is null or empty string.</exception>
        public MetricTelemetry(string metricName, double metricValue) : this()
        {
            this.Name = metricName;
#pragma warning disable 618
            this.Value = metricValue;
#pragma warning restore 618
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetricTelemetry"/> class with properties provided.
        /// </summary>
        /// <remarks>
        /// Metrics should always be pre-aggregated across a time period before being sent.
        /// Most applications do not need to explicitly create <c>MetricTelemetry</c> objects. Instead, use one of
        /// the <c>GetMetric(..)</c> overloads on the <see cref="TelemetryClient" /> class to get a metric object
        /// for accessing SDK pre-aggregation capabilities. <br />
        /// However, you can use this ctor to create metric telemetry items if you have implemented your own metric
        /// aggregation. In that case, use <see cref="TelemetryClient.Track(ITelemetry)"/> method to send your aggregates.
        /// </remarks>
        /// <param name="name">Metric name.</param>
        /// <param name="count">Count of values taken during aggregation interval.</param>
        /// <param name="sum">Sum of values taken during aggregation interval.</param>
        /// <param name="min">Minimum value taken during aggregation interval.</param>
        /// <param name="max">Maximum of values taken during aggregation interval.</param>
        /// <param name="standardDeviation">Standard deviation of values taken during aggregation interval.</param>
        public MetricTelemetry(
            string name,
            int count,
            double sum,
            double min,
            double max,
            double standardDeviation)
            : this()
        {
            this.Name = name;
            this.Count = count;
            this.Sum = sum;
            this.Min = min;
            this.Max = max;
            this.StandardDeviation = standardDeviation;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetricTelemetry"/> class with properties provided.
        /// </summary>
        /// <remarks>
        /// Metrics should always be pre-aggregated across a time period before being sent.
        /// Most applications do not need to explicitly create <c>MetricTelemetry</c> objects. Instead, use one of
        /// the <c>GetMetric(..)</c> overloads on the <see cref="TelemetryClient" /> class to get a metric object
        /// for accessing SDK pre-aggregation capabilities. <br />
        /// However, you can use this ctor to create metric telemetry items if you have implemented your own metric
        /// aggregation. In that case, use <see cref="TelemetryClient.Track(ITelemetry)"/> method to send your aggregates.
        /// </remarks>
        /// <param name="metricNamespace">Metric namespace.</param>
        /// <param name="name">Metric name.</param>
        /// <param name="count">Count of values taken during aggregation interval.</param>
        /// <param name="sum">Sum of values taken during aggregation interval.</param>
        /// <param name="min">Minimum value taken during aggregation interval.</param>
        /// <param name="max">Maximum of values taken during aggregation interval.</param>
        /// <param name="standardDeviation">Standard deviation of values taken during aggregation interval.</param>
        public MetricTelemetry(
            string metricNamespace,
            string name,
            int count,
            double sum,
            double min,
            double max,
            double standardDeviation)
            : this()
        {
            this.MetricNamespace = metricNamespace;
            this.Name = name;
            this.Count = count;
            this.Sum = sum;
            this.Min = min;
            this.Max = max;
            this.StandardDeviation = standardDeviation;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetricTelemetry"/> class by cloning an existing instance.
        /// </summary>
        /// <param name="source">Source instance of <see cref="MetricTelemetry"/> to clone from.</param>
        private MetricTelemetry(MetricTelemetry source)
        {
            this.Data = source.Data.DeepClone();
            this.Metric = source.Metric.DeepClone();
            this.Context = source.Context.DeepClone(this.Data.properties);
            this.Sequence = source.Sequence;
            this.Timestamp = source.Timestamp;
            this.extension = source.extension?.DeepClone();
        }

        /// <inheritdoc />
        string IAiSerializableTelemetry.TelemetryName
        {
            get
            {
                return this.EnvelopeName;
            }

            set
            {
                this.EnvelopeName = value;
            }
        }

        /// <inheritdoc />
        string IAiSerializableTelemetry.BaseType => nameof(MetricData);

        /// <summary>
        /// Gets or sets date and time when event was recorded.
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the value that defines absolute order of the telemetry item.
        /// </summary>
        public string Sequence { get; set; }

        /// <summary>
        /// Gets the context associated with the current telemetry item.
        /// </summary>
        public TelemetryContext Context { get; }

        /// <summary>
        /// Gets or sets gets the extension used to extend this telemetry instance using new strong typed object.
        /// </summary>
        public IExtension Extension
        {
            get { return this.extension; }
            set { this.extension = value; }
        }

        /// <summary>
        /// Gets or sets the name of the metric.
        /// </summary>
        public string MetricNamespace
        {
            get { return this.Metric.ns; }
            set { this.Metric.ns = value; }
        }

        /// <summary>
        /// Gets or sets the name of the metric.
        /// </summary>
        public string Name
        {
            get { return this.Metric.name; }
            set { this.Metric.name = value; }
        }

        /// <summary>
        /// Gets or sets the value of this metric.
        /// </summary>
        [Obsolete("This property is obsolete. Use Sum property instead.")]
        public double Value
        {
            get { return this.Metric.value; }
            set { this.Metric.value = value; }
        }

        /// <summary>
        /// Gets or sets sum of the values of the metric samples.
        /// </summary>
        public double Sum
        {
            get { return this.Metric.value; }
            set { this.Metric.value = value; }
        }

        /// <summary>
        /// Gets or sets the number of values in the sample set.
        /// </summary>
        public int? Count
        {
            get { return this.Metric.count.HasValue ? this.Metric.count : 1; }
            set { this.Metric.count = value; }
        }

        /// <summary>
        /// Gets or sets the min value of this metric across the sample set.
        /// </summary>
        public double? Min
        {
            get { return this.Metric.min; }
            set { this.Metric.min = value; }
        }

        /// <summary>
        /// Gets or sets the max value of this metric across the sample set.
        /// </summary>
        public double? Max
        {
            get { return this.Metric.max; }
            set { this.Metric.max = value; }
        }

        /// <summary>
        /// Gets or sets the standard deviation of this metric across the sample set.
        /// </summary>
        public double? StandardDeviation
        {
            get { return this.Metric.stdDev; }
            set { this.Metric.stdDev = value; }
        }

        /// <summary>
        /// Gets a dictionary of application-defined property names and values providing additional information about this metric.
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#properties">Learn more</a>
        /// </summary>
        public IDictionary<string, string> Properties
        {
            get { return this.Data.properties; }
        }

        /// <summary>
        /// Deeply clones a <see cref="MetricTelemetry"/> object.
        /// </summary>
        /// <returns>A cloned instance.</returns>
        public ITelemetry DeepClone()
        {
            return new MetricTelemetry(this);
        }

        /// <inheritdoc/>
        public void SerializeData(ISerializationWriter serializationWriter)
        {
            if (serializationWriter == null)
            {
                throw new ArgumentNullException(nameof(serializationWriter));
            }

            serializationWriter.WriteProperty(this.Data);
        }

        /// <summary>
        /// Sanitizes the properties based on constraints.
        /// </summary>
        void ITelemetry.Sanitize()
        {
            this.MetricNamespace = Property.TrimAndTruncate(this.MetricNamespace, Property.MaxMetricNamespaceLength);
            this.Name = this.Name.SanitizeName();
            this.Name = Utils.PopulateRequiredStringValue(this.Name, "name", typeof(MetricTelemetry).FullName);
            this.Properties.SanitizeProperties();
            this.Sum = Utils.SanitizeNanAndInfinity(this.Sum);

            // note: we set count to 1 if it isn't a positive integer
            // thinking that if it is zero (negative case is clearly broken)
            // that most likely means somebody created instance but forgot to set count
            this.Count = (!this.Count.HasValue) || (this.Count <= 0) ? 1 : this.Count;

            if (this.Min.HasValue)
            {
                this.Min = Utils.SanitizeNanAndInfinity(this.Min.Value);
            }

            if (this.Max.HasValue)
            {
                this.Max = Utils.SanitizeNanAndInfinity(this.Max.Value);
            }

            if (this.StandardDeviation.HasValue)
            {
                this.StandardDeviation = Utils.SanitizeNanAndInfinity(this.StandardDeviation.Value);
            }
        }
    }
}
