namespace Microsoft.ApplicationInsights.Metrics.Extensibility
{
    using System;

    /// <summary>Abstraction for data converters between <see cref="MetricAggregate"/> items and contracts supported by a ingestion pipeline.</summary>
    /// @PublicExposureCandidate
    internal interface IMetricAggregateToTelemetryPipelineConverter
    {
        /// <summary>Convert a <c>MetricAggregate</c> to a an exchange type supported by an ingestion pipeline.</summary>
        /// <param name="aggregate">The aggregate to convert.</param>
        /// <returns>Converted object.</returns>
        object Convert(MetricAggregate aggregate);
    }
}
