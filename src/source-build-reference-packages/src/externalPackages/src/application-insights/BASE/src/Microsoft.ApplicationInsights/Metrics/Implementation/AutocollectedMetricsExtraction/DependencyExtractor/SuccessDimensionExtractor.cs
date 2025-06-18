namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics
{
    using System;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;

    internal class SuccessDimensionExtractor : IDimensionExtractor
    {
        public int MaxValues { get; set; } = 2;

        public string DefaultValue { get; set; } = bool.TrueString;

        public string Name { get; set; } = MetricTerms.Autocollection.DependencyCall.PropertyNames.Success;

        public string ExtractDimension(ITelemetry item)
        {
            if (item is DependencyTelemetry dep)
            {
                bool dependencyFailed = (dep.Success != null) && (dep.Success == false);
                string dependencySuccessString = dependencyFailed ? bool.FalseString : bool.TrueString;
                return dependencySuccessString;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
