namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics
{
    using System;
    using Microsoft.ApplicationInsights.Channel;

    internal interface ISpecificAutocollectedMetricsExtractor
    {
        /// <summary>
        /// Gets the name of this extractor.
        /// All telemetry that has been processed by this extractor will be tagged by adding the
        /// string "<c>(Name: {ExtractorName}, Ver:{ExtractorVersion})</c>" to the <c>xxx.ProcessedByExtractors</c> property.
        /// The respective logic is in the <see cref="AutocollectedMetricsExtractor"/>-class.
        /// </summary>
        string ExtractorName { get; }

        /// <summary>
        /// Gets the version of this extractor.
        /// All telemetry that has been processed by this extractor will be tagged by adding the
        /// string "<c>(Name: {ExtractorName}, Ver:{ExtractorVersion})</c>" to the <c>xxx.ProcessedByExtractors</c> property.
        /// The respective logic is in the <see cref="AutocollectedMetricsExtractor"/>-class.
        /// </summary>
        string ExtractorVersion { get; }

        /// <summary>
        /// Pre-initialize this extractor.
        /// </summary>
        /// <param name="metricTelemetryClient">The <c>TelemetryClient</c> to be used for sending extracted metrics.</param>
        void InitializeExtractor(TelemetryClient metricTelemetryClient);

        /// <summary>
        /// Perform actual metric data point extraction from the specified item.
        /// </summary>
        /// <param name="fromItem">The item from which to extract metrics.</param>
        /// <param name="isItemProcessed">Whether the specified item was processed (or ignored) by this extractor.
        /// This determines whether the specified item will be tagged accordingly by adding the
        /// string "<c>(Name: {ExtractorName}, Ver:{ExtractorVersion})</c>" to the <c>xxx.ProcessedByExtractors></c> property.</param>
        void ExtractMetrics(ITelemetry fromItem, out bool isItemProcessed);
    }
}
