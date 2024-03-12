namespace Microsoft.ApplicationInsights
{
    using System;

    /// <summary>
    /// Used when getting or creating a <see cref="Metric" /> to optionally specify the scope across which the values for the metric are to be aggregated in memory.<br />
    /// Intended for advanced scenarios.
    /// The default "<see cref="TelemetryConfiguration" />" is used whenever <c>MetricAggregationScope</c> is not specified explicitly.
    /// </summary>
    /// <seealso cref="MetricAggregationScope.TelemetryConfiguration" />
    /// <seealso cref="MetricAggregationScope.TelemetryClient" />
    public enum MetricAggregationScope
    {
        /// <summary>
        /// <para>Metric values will be aggregated ACROSS all telemetry clients that belong to the same <c>TelemetryConfiguration</c>.<br />
        /// This is the default. It fits most use cases and is more conservative towards resources.</para>
        /// <para>Background-Info: When you use this option with the <c>.GetMetric(..)</c> extension method of a <c>TelemetryClient</c>,
        /// the <see cref="Microsoft.ApplicationInsights.Metrics.MetricManager" /> instance that owns the retrieved <c>Metric</c> will
        /// be attached to a <c>TelemetryConfiguration</c> instance associated with that <c>TelemetryClient</c>. Thus, the <c>MetricManager</c>
        /// will be shared across all clients of this telemetry config. As a result, the <c>Context</c>, the <c>InstrumentationKey</c>
        /// and other properties of the respective <c>TelemetryClient</c> will be ignored in favor of the <c>TelemetryConfiguration</c>-wide
        /// settings.</para>
        /// </summary>
        TelemetryConfiguration = 0,

        /// <summary>
        /// <para>Metric values will be aggregated only across a specific <c>TelemetryClient</c> instance and then sent using that
        /// particular instance.<br />
        /// Such aggregation across many smaller scopes can be resource intensive. This option is only recommended when a particular instance
        /// of <c>TelementryClient</c> needs to be used for sending telemetry. Typically, <c>MetricAggregationScope.TelemetryConfiguration</c>
        /// is the preferred option.</para>
        /// <para>Background-Info: This option causes the <see cref="Microsoft.ApplicationInsights.Metrics.MetricManager" /> instance that
        /// owns the retrieved <c>Metric</c> to be attached to a specified <c>TelemetryClient</c> instance.
        /// As a result, the <c>Context</c> and the <c>InstrumentationKey</c> of the specified <c>TelemetryClient</c> will be respected.
        /// However, each <c>MetricManager</c> instance encapsulates a managed thread and each aggregator uses additional memory.</para>
        /// </summary>
        TelemetryClient = 1,
    }
}
