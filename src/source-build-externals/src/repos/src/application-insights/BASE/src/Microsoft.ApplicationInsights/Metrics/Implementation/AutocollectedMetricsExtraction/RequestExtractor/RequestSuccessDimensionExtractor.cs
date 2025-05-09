namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics
{
    using System;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;

    internal class RequestSuccessDimensionExtractor : IDimensionExtractor
    {
        public int MaxValues { get; set; } = 2;

        public string DefaultValue { get; set; } = bool.TrueString;

        public string Name { get; set; } = MetricTerms.Autocollection.Request.PropertyNames.Success;

        public string ExtractDimension(ITelemetry item)
        {
            if (item is RequestTelemetry req)
            {
                bool isFailed = req.Success.HasValue
                                ? (req.Success.Value == false)
                                : false;
                return isFailed ? bool.FalseString : bool.TrueString;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
