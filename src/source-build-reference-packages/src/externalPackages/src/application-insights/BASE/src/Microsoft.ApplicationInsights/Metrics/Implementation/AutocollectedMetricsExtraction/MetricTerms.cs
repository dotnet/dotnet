namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics
{
    using System;

    internal static class MetricTerms
    {       
        private const string MetricPropertiesNamePrefix = "_MS";

        public static class Aggregation
        {
            public static class Interval
            {
                public static class Moniker
                {
                    public const string Key = MetricPropertiesNamePrefix + ".AggregationIntervalMs";
                }
            }
        }

        public static class Extraction
        {
            public static class ProcessedByExtractors
            {
                public static class Moniker
                {
                    public const string Key = MetricPropertiesNamePrefix + ".ProcessedByMetricExtractors";
                    public const string ExtractorInfoTemplate = "(Name:'{0}', Ver:'{1}')";      // $"(Name:'{ExtractorName}', Ver:'{ExtractorVersion}')"
                }
            }
        }

        public static class Autocollection
        {
            public static class Moniker
            {
                public const string Key = MetricPropertiesNamePrefix + ".IsAutocollected";
                public const string Value = "True";
            }

            public static class MetricId
            {
                public static class Moniker
                {
                    public const string Key = MetricPropertiesNamePrefix + ".MetricId";
                }
            }

            public static class Metric
            {
                public static class RequestDuration
                {
                    public const string Name = "Server response time";
                    public const string Id = "requests/duration";
                }

                public static class DependencyCallDuration
                {
                    public const string Name = "Dependency duration";
                    public const string Id = "dependencies/duration";
                }

                public static class ExceptionCount
                {
                    public const string Name = "Exceptions";
                    public const string Id = "exceptions/count";
                }

                public static class TraceCount
                {
                    public const string Name = "Traces";
                    public const string Id = "traces/count";
                }
            }

            public static class Request
            {
                public static class PropertyNames
                {
                    public const string Success = "Request.Success";
                    public const string ResultCode = "request/resultCode";
                    public const string PerformanceBucket = "request/performanceBucket";
                }
            }

            public static class DependencyCall
            {
                public static class PropertyNames
                {
                    public const string Success = "Dependency.Success";
                    public const string TypeName = "Dependency.Type";
                    public const string PerformanceBucket = "dependency/performanceBucket";
                    public const string Target = "dependency/target";
                    public const string ResultCode = "dependency/resultCode";
                }
            }

            public static class TraceCount
            {
                public static class PropertyNames
                {
                    public const string SeverityLevel = "trace/severityLevel";
                }
            }

            public static class Common
            {
                public static class PropertyNames
                {
                    public const string CloudRoleInstance = "cloud/roleInstance";
                    public const string CloudRoleName = "cloud/roleName";
                    public const string Synthetic = "operation/synthetic";
                }

                public static class PropertyValues
                {
                    public const string DimensionCapFallbackValue = "DIMENSION-CAPPED";
                    public const string Unknown = "Unknown";
                    public const string Unspecified = "Unspecified";
                    public const string Other = "Other";
                }
            }
        }
    }
}
