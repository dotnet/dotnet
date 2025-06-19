namespace Microsoft.ApplicationInsights.Metrics
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>Holds the metric aggregation results of a particular metric data series over an aggregation time period.
    /// The specific data fields on instanced of this class are not strongly typed (property bag) which allows using this
    /// aggregate type for aggregates of any aggregation kind.</summary>
    public class MetricAggregate
    {
        // We want to make the aggregate thread safe, but we expect no significant contention, so a simple lock will suffice.
        private readonly object updateLock = new object();

        private DateTimeOffset aggregationPeriodStart;
        private TimeSpan aggregationPeriodDuration;

        /// <summary>Ceates a new metric aggregate.</summary>
        /// <param name="metricNamespace">The namespace of the metric that produces this aggregate.</param>
        /// <param name="metricId">The id (name) of the metric that produced this aggregate.</param>
        /// <param name="aggregationKindMoniker">A moniker defining the kind of the aggregation used for the respective metric.</param>
        public MetricAggregate(string metricNamespace, string metricId, string aggregationKindMoniker)
        {
            Util.ValidateNotNull(metricNamespace, nameof(metricNamespace));
            Util.ValidateNotNull(metricId, nameof(metricId));
            Util.ValidateNotNull(aggregationKindMoniker, nameof(aggregationKindMoniker));

            this.MetricNamespace = metricNamespace;
            this.MetricId = metricId;
            this.AggregationKindMoniker = aggregationKindMoniker;

            this.aggregationPeriodStart = default(DateTimeOffset);
            this.aggregationPeriodDuration = default(TimeSpan);

            this.Dimensions = new ConcurrentDictionary<string, string>();
            this.Data = new ConcurrentDictionary<string, object>();
        }

        /// <summary>Gets the namespace of the metric that produces this aggregate.</summary>
        public string MetricNamespace { get; }

        /// <summary>Gets the id (name) of the metric that produced this aggregate.</summary>
        public string MetricId { get; }

        /// <summary>Gets the moniker defining the kind of the aggregation used for the respective metric.</summary>
        public string AggregationKindMoniker { get; }

        /// <summary>Gets or sets the start of the aggregation period summarized by this aggregate.</summary>
        public DateTimeOffset AggregationPeriodStart
        {
            get
            {
                lock (this.updateLock)
                {
                    return this.aggregationPeriodStart;
                }
            }

            set
            {
                lock (this.updateLock)
                {
                    this.aggregationPeriodStart = value;
                }
            }
        }

        /// <summary>Gets or sets the length of the aggregation period summarized by this aggregate.</summary>
        public TimeSpan AggregationPeriodDuration
        {
            get
            {
                lock (this.updateLock)
                {
                    return this.aggregationPeriodDuration;
                }
            }

            set
            {
                lock (this.updateLock)
                {
                    this.aggregationPeriodDuration = value;
                }
            }
        }

        /// <summary>Gets get table of dimension name-values that specify the data series that produced this agregate within the overall metric.</summary>
        public IDictionary<string, string> Dimensions { get; }

        /// <summary>Gets the property bag that contains the actual aggregate data.
        /// For example, if the aggregate was produced for a metric of the aggregation kind Measurement,
        /// the look-up key for this property bag are accessible via
        /// <see cref="MetricSeriesConfigurationForMeasurement.Constants.AggregateKindDataKeys" />.</summary>
        public IDictionary<string, object> Data { get; }

        /// <summary>
        /// This is a convenience method to retrieve the object at <c>Data[dataKey]</c>.
        /// It attempts to convert that object to the specified type <c>T</c>. If the conversion fails, the specified <c>defaultValue</c> is returned.
        /// </summary>
        /// <typeparam name="T">Type to which to convert the object at <c>Data[dataKey]</c>.</typeparam>
        /// <param name="dataKey">Key for the data item.</param>
        /// <param name="defaultValue">The value to return if conversion fails.</param>
        /// <returns>This aggregate's component object available at <c>Data[dataKey]</c>.</returns>
        public T GetDataValue<T>(string dataKey, T defaultValue)
        {
            object dataValue;
            if (this.Data.TryGetValue(dataKey, out dataValue))
            {
                try
                {
                    T value = (T)Convert.ChangeType(dataValue, typeof(T), CultureInfo.InvariantCulture);
                    return value;
                }
                catch
                {
                    return defaultValue;
                }
            }

            return defaultValue;
        }
    }
}
