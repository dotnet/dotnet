namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics
{
    using Microsoft.ApplicationInsights.Channel;

    internal class CloudRoleInstanceDimensionExtractor : IDimensionExtractor
    {
        public int MaxValues { get; set; }

        public string DefaultValue { get; set; } = MetricTerms.Autocollection.Common.PropertyValues.Unknown;

        public string Name { get; set; } = MetricTerms.Autocollection.Common.PropertyNames.CloudRoleInstance;

        public string ExtractDimension(ITelemetry item)
        {
            return item.Context.Cloud.RoleInstance;
        }
    }
}
