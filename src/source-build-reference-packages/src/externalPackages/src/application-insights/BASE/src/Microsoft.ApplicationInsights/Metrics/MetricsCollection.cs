namespace Microsoft.ApplicationInsights.Metrics
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using static System.FormattableString;

    /// <summary>A collection of metrics available at a specific scope.
    /// A metric is itself a colection of data time series identified by dimension name-values.</summary>
    public sealed class MetricsCollection : ICollection<Metric>
    {
        private readonly MetricManager metricManager;
        private readonly ConcurrentDictionary<MetricIdentifier, Metric> metrics = new ConcurrentDictionary<MetricIdentifier, Metric>();

        /// <summary>Initializes a metric collection.</summary>
        /// <param name="metricManager">The manager that owns the scope of this metric collection.</param>
        internal MetricsCollection(MetricManager metricManager)
        {
            Util.ValidateNotNull(metricManager, nameof(metricManager));
            this.metricManager = metricManager;
        }

        /// <summary>Gets the number of metrics in this collection.</summary>
        public int Count
        {
            get { return this.metrics.Count; }
        }

        /// <summary>Gets a value indicating whether this collection is read-only. It is not.</summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>Gets the specified metric, or creates one if no such metric exists.</summary>
        /// <param name="metricIdentifier">The identity of the metric.</param>
        /// <param name="metricConfiguration">@The configutration of the metric.</param>
        /// <returns>A metric with the specified identify and configuration.</returns>
        /// <exception cref="ArgumentException">If a metric with the specified identify exists,
        /// but its configuration does not match the specified configuration.
        /// You may not change a metric configurations once a metric was created for the first time.
        /// Either specify the same configuration every time, or specify <c>null</c> during every
        /// invocation except the first one. <c>null</c> will match against any previously specified 
        /// configuration when retrieving existing metrics, or fall back to the default when 
        /// creating new metrics.</exception>
        public Metric GetOrCreate(
                                MetricIdentifier metricIdentifier,
                                MetricConfiguration metricConfiguration)
        {
            Util.ValidateNotNull(metricIdentifier, nameof(metricIdentifier));
            
            Metric metric = this.metrics.GetOrAdd(
                                            metricIdentifier,
                                            (key) => new Metric(
                                                                this.metricManager,
                                                                metricIdentifier,
                                                                metricConfiguration ?? MetricConfigurations.Common.Default()));

            if (metricConfiguration != null && false == metric.configuration.Equals(metricConfiguration))
            {
                throw new ArgumentException("A Metric with the specified Namespace, Id and dimension names already exists, but it has a configuration"
                                          + " that is different from the specified configuration. You may not change configurations once a"
                                          + " metric was created for the first time. Either specify the same configuration every time, or"
                                          + " specify 'null' during every invocation except the first one. 'Null' will match against any"
                                          + " previously specified configuration when retrieving existing metrics, or fall back to"
                               + Invariant($" the default when creating new metrics. ({nameof(metricIdentifier)} = \"{metricIdentifier.ToString()}\".)"));
            }

            return metric;
        }

        /// <summary>Gets the metric with the specified identify, if it exists.</summary>
        /// <param name="metricIdentifier">A metric identity.</param>
        /// <param name="metric">The metric (if it exists) or <c>null</c>.</param>
        /// <returns><c>true</c> if the metric was retrieved, or <c>false</c> otherwise.</returns>
        public bool TryGet(MetricIdentifier metricIdentifier, out Metric metric)
        {
            Util.ValidateNotNull(metricIdentifier, nameof(metricIdentifier));

            return this.metrics.TryGetValue(metricIdentifier, out metric);
        }

        /// <summary>Removes all metrics from this collection.</summary>
        public void Clear()
        {
            this.metrics.Clear();
        }

        /// <summary>Checks if a metric is present in this collection.</summary>
        /// <param name="metric">A metric.</param>
        /// <returns><c>true</c> if the metric exists in this collection, or <c>false</c> otherwise.</returns>
        public bool Contains(Metric metric)
        {
            if (metric == null)
            {
                return false;
            }

            return this.metrics.ContainsKey(metric.Identifier);
        }

        /// <summary>Checks if a metric with th specified identity is present in this collection.</summary>
        /// <param name="metricIdentifier">A metric identity.</param>
        /// <returns><c>true</c> if a metric with the specified exists in this collection, or <c>false</c> otherwise.</returns>
        public bool Contains(MetricIdentifier metricIdentifier)
        {
            if (metricIdentifier == null)
            {
                return false;
            }

            return this.metrics.ContainsKey(metricIdentifier);
        }

        /// <summary>Copies the contents of this collection to the specified array.</summary>
        /// <param name="array">An artay.</param>
        /// <param name="arrayIndex">Array index where to start the copy.</param>
        public void CopyTo(Metric[] array, int arrayIndex)
        {
            Util.ValidateNotNull(array, nameof(array));

            if (arrayIndex < 0 || arrayIndex >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            }

            this.metrics.Values.CopyTo(array, arrayIndex);
        }

        /// <summary>Removes the specified metric from this collection.</summary>
        /// <param name="metric">A metric.</param>
        /// <returns>Whether the metric was found and removed.</returns>
        public bool Remove(Metric metric)
        {
            if (metric == null)
            {
                return false;
            }

            Metric removedMetric;
            return this.metrics.TryRemove(metric.Identifier, out removedMetric);
        }

        /// <summary>Removes a metric with the specified identity from this collection.</summary>
        /// <param name="metricIdentifier">A metric identifier.</param>
        /// <returns>Whether the metric was found and removed.</returns>
        public bool Remove(MetricIdentifier metricIdentifier)
        {
            Metric removedMetric;
            return this.Remove(metricIdentifier, out removedMetric);
        }

        /// <summary>Removes a metric with the specified identity from this collection.</summary>
        /// <param name="metricIdentifier">A metric identifier.</param>
        /// <param name="removedMetric">The metric that was removed or <c>null</c>.</param>
        /// <returns>Whether the metric was found and removed.</returns>
        public bool Remove(MetricIdentifier metricIdentifier, out Metric removedMetric)
        {
            if (metricIdentifier == null)
            {
                removedMetric = null;
                return false;
            }

            return this.metrics.TryRemove(metricIdentifier, out removedMetric);
        }

        /// <summary>Gets an enumerator for this collection.</summary>
        /// <returns>An enumerator for this collection.</returns>
        public IEnumerator<Metric> GetEnumerator()
        {
            return this.metrics.Values.GetEnumerator();
        }

        /// <summary>Gets an enumerator for this collection.</summary>
        /// <returns>An enumerator for this collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// The Add(..) method is not supported. To add a new metric, use the GetOrCreate(..) method.
        /// </summary>
        /// <param name="unsupported">Ignored.</param>
        /// <exception cref="NotSupportedException">Is always thrown.</exception>
        void ICollection<Metric>.Add(Metric unsupported)
        {
            throw new NotSupportedException(Invariant($"The Add(..) method is not supported by this {nameof(MetricsCollection)}.")
                                                     + " To add a new metric, use the GetOrCreate(..) method.");
        }
    }
}
