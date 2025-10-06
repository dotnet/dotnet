namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics
{
    using System;
    using System.Collections.Generic;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Metrics;
    using static System.FormattableString;

    /// <summary>
    /// An instance of this class is contained within the <see cref="AutocollectedMetricsExtractor"/> telemetry processor.
    /// It extracts auto-collected, pre-aggregated (aka. "standard") metrics from RequestTelemetry objects which represent
    /// invocations of the monitored service.
    /// </summary>
    internal class RequestMetricsExtractor : ISpecificAutocollectedMetricsExtractor
    {
        /// <summary>
        /// The default value for the <see cref="MaxResponseCodeToDiscover"/> property.        
        /// </summary>
        public const int MaxResponseCodeToDiscoverDefault = 30;

        /// <summary>
        /// The default value for the <see cref="MaxCloudRoleInstanceValuesToDiscover"/> property.
        /// </summary>
        public const int MaxCloudRoleInstanceValuesToDiscoverDefault = 2;

        /// <summary>
        /// The default value for the <see cref="MaxCloudRoleNameValuesToDiscover"/> property.
        /// </summary>
        public const int MaxCloudRoleNameValuesToDiscoverDefault = 2;

        private readonly object lockObject = new object();

        /// <summary>
        /// Extracted metric.
        /// </summary>
        private Metric requestDurationMetric = null;
        private bool isInitialized = false;
        private List<IDimensionExtractor> dimensionExtractors = new List<IDimensionExtractor>();

        public RequestMetricsExtractor()
        {
        }

        public string ExtractorName { get; } = "Requests";

        public string ExtractorVersion { get; } = "1.1";

        /// <summary>
        /// Gets or sets the maximum number of auto-discovered Request response code.
        /// </summary>
        public int MaxResponseCodeToDiscover { get; set; } = MaxResponseCodeToDiscoverDefault;

        /// <summary>
        /// Gets or sets the maximum number of auto-discovered Cloud RoleInstance values.
        /// </summary>
        public int MaxCloudRoleInstanceValuesToDiscover { get; set; } = MaxCloudRoleInstanceValuesToDiscoverDefault;

        /// <summary>
        /// Gets or sets the maximum number of auto-discovered Cloud RoleName values.
        /// </summary>
        public int MaxCloudRoleNameValuesToDiscover { get; set; } = MaxCloudRoleNameValuesToDiscoverDefault;

        public void InitializeExtractor(TelemetryClient metricTelemetryClient)
        {
            if (metricTelemetryClient == null)
            {
                this.requestDurationMetric = null;
                return;
            }

            if (!this.isInitialized)
            {
                lock (this.lockObject)
                {
                    if (!this.isInitialized)
                    {
                        this.dimensionExtractors.Add(new RequestMetricIdDimensionExtractor());
                        this.dimensionExtractors.Add(new RequestSuccessDimensionExtractor());
                        this.dimensionExtractors.Add(new DurationBucketExtractor());
                        this.dimensionExtractors.Add(new SyntheticDimensionExtractor());
                        this.dimensionExtractors.Add(new RequestResponseCodeDimensionExtractor() { MaxValues = this.MaxResponseCodeToDiscover });
                        this.dimensionExtractors.Add(new CloudRoleInstanceDimensionExtractor() { MaxValues = this.MaxCloudRoleInstanceValuesToDiscover });
                        this.dimensionExtractors.Add(new CloudRoleNameDimensionExtractor() { MaxValues = this.MaxCloudRoleNameValuesToDiscover });

                        int seriesCountLimit = 1;
                        int[] valuesPerDimensionLimit = new int[this.dimensionExtractors.Count];
                        int i = 0;

                        foreach (var dim in this.dimensionExtractors)
                        {
                            int dimLimit = 1;
                            dimLimit = dim.MaxValues == 0 ? 1 : dim.MaxValues;

                            seriesCountLimit = seriesCountLimit * (1 + dimLimit);
                            valuesPerDimensionLimit[i++] = dimLimit;
                        }

                        MetricConfiguration config = new MetricConfigurationForMeasurement(
                                                                        seriesCountLimit,
                                                                        valuesPerDimensionLimit,
                                                                        new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));
                        config.ApplyDimensionCapping = true;
                        config.DimensionCappedString = MetricTerms.Autocollection.Common.PropertyValues.DimensionCapFallbackValue;

                        IList<string> dimensionNames = new List<string>(this.dimensionExtractors.Count);
                        for (i = 0; i < this.dimensionExtractors.Count; i++)
                        {
                            dimensionNames.Add(this.dimensionExtractors[i].Name);
                        }

                        MetricIdentifier metricIdentifier = new MetricIdentifier(MetricIdentifier.DefaultMetricNamespace,
                                    MetricTerms.Autocollection.Metric.RequestDuration.Name,
                                    dimensionNames);

                        this.requestDurationMetric = metricTelemetryClient.GetMetric(
                                                                    metricIdentifier: metricIdentifier,
                                                                    metricConfiguration: config,
                                                                    aggregationScope: MetricAggregationScope.TelemetryClient);
                        this.isInitialized = true;
                    }
                }
            }            
        }

        public void ExtractMetrics(ITelemetry fromItem, out bool isItemProcessed)
        {
            RequestTelemetry request = fromItem as RequestTelemetry;
            if (request == null)
            {
                isItemProcessed = false;
                return;
            }

            //// If there is no Metric, then this extractor has not been properly initialized yet:
            if (this.requestDurationMetric == null)
            {
                //// This should be caught and properly logged by the base class:
                throw new InvalidOperationException(Invariant($"Cannot execute {nameof(this.ExtractMetrics)}.")
                                                  + Invariant($" There is no {nameof(this.requestDurationMetric)}.")
                                                  + Invariant($" Either this metrics extractor has not been initialized, or it has been disposed."));
            }

            string[] dimValues = new string[this.dimensionExtractors.Count];
            
            for (int i = 0; i < this.dimensionExtractors.Count; i++)
            {
                var dim = this.dimensionExtractors[i];
                if (dim.MaxValues == 0)
                {
                    dimValues[i] = MetricTerms.Autocollection.Common.PropertyValues.Other;
                }
                else
                {
                    dimValues[i] = dim.ExtractDimension(request);
                    if (string.IsNullOrEmpty(dimValues[i]))
                    {
                        dimValues[i] = dim.DefaultValue;
                    }
                }
            }

            CommonHelper.TrackValueHelper(this.requestDurationMetric, request.Duration.TotalMilliseconds, dimValues);
            isItemProcessed = true;
        }
    }
}
