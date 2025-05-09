namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics
{
    using System;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;

    internal class TypeDimensionExtractor : IDimensionExtractor
    {
        public int MaxValues { get; set; }

        public string DefaultValue { get; set; } = MetricTerms.Autocollection.Common.PropertyValues.Unknown;

        public string Name { get; set; } = MetricTerms.Autocollection.DependencyCall.PropertyNames.TypeName;

        public string ExtractDimension(ITelemetry item)
        {
            if (item is DependencyTelemetry dep)
            {
                return dep.Type;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
