namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics
{
    using System;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;

    internal class DurationBucketExtractor : IDimensionExtractor
    {
        public int MaxValues { get; set; } = 11;

        public string DefaultValue { get; set; } = MetricTerms.Autocollection.Common.PropertyValues.Unknown;

        public string Name { get; set; } = MetricTerms.Autocollection.Request.PropertyNames.PerformanceBucket;

        public string ExtractDimension(ITelemetry item)
        {
            if (item is RequestTelemetry req)
            {
                return DurationBucketizer.GetPerformanceBucket(req.Duration);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
