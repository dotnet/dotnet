namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics
{
    using System;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;

    internal class SyntheticDimensionExtractor : IDimensionExtractor
    {
        public int MaxValues { get; set; } = 2;

        public string DefaultValue { get; set; } = bool.FalseString;

        public string Name { get; set; } = MetricTerms.Autocollection.Common.PropertyNames.Synthetic;

        public string ExtractDimension(ITelemetry item)
        {
            bool isSynthetic = item.Context.Operation.SyntheticSource != null;
            string isSyntheticString = isSynthetic ? bool.TrueString : bool.FalseString;
            return isSyntheticString;
        }
    }
}
