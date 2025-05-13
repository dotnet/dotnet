namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics
{
    using System;
    using System.Globalization;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;

    internal class TraceSeverityLevelDimensionExtractor : IDimensionExtractor
    {
        // There are 5 enumeration values to identify severity level. 
        // Plus 1 for trace called without severity level specified.
        // https://docs.microsoft.com/en-us/dotnet/api/microsoft.applicationinsights.datacontracts.severitylevel?view=azure-dotnet
        public int MaxValues { get; set; } = 6;

        public string DefaultValue { get; set; } = MetricTerms.Autocollection.Common.PropertyValues.Unspecified;

        public string Name { get; set; } = MetricTerms.Autocollection.TraceCount.PropertyNames.SeverityLevel;

        public string ExtractDimension(ITelemetry item)
        {
            var trace = item as TraceTelemetry;
            if (trace?.SeverityLevel != null)
            {
                var sevLevel = (int)trace.SeverityLevel;
                return sevLevel.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
