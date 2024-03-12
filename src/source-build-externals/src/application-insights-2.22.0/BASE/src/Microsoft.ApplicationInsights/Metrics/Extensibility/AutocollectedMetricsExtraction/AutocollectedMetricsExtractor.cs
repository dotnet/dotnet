namespace Microsoft.ApplicationInsights.Extensibility
{
    using System;
    using System.Globalization;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// Extracts auto-collected, pre-aggregated (aka. "standard") metrics from telemetry.
    /// Metric Extractors participate in the telemetry pipeline as telemetry processors. They examine telemetry items going through
    /// the pipeline and create pre-aggregated metrics based on the encountered items. The metrics can be anything. For example, one may
    /// choose to extract a metric for "Request Duration" from RequestTelemetry items. Or one may choose to create a metric "Cows Sold"
    /// from specific user-tracked EventTelemetry items that contain respective information. 
    /// <br />
    /// Metric Extractors should be placed into the pipeline after telemetry initializers and before any telemetry processors that may
    /// perform any kind of filtering, e.g. before any sampling processors. Placing metric extractors after any filters will prevent them
    /// from seeing all potentially relevant telemetry which will skew the extracted metrics.
    /// <br />
    /// This extractor is responsible for aggregating auto-collected, pre-aggregated (aka. "standard") metrics, such as failed request
    /// count, dependency call durations and similar. Users may use the same pattern to create their own extractors for any metrics
    /// they want from any kind of telemetry. 
    /// This extractor contains several implementations of the (internal) <c>ISpecificAutocollectedMetricsExtractor</c>-interface to which
    /// it delegates the aggregation of particular metrics. All those implementations share the
    /// same (dedicated) <see cref="Microsoft.ApplicationInsights.Metrics.MetricManager"/>-instance for metric aggregation.
    /// </summary>
    public sealed class AutocollectedMetricsExtractor : ITelemetryProcessor, ITelemetryModule, IDisposable
    {
        // List of all participating extractors that take care of specific metrics kinds:
        private readonly RequestMetricsExtractor extractorForRequestMetrics;
        private readonly DependencyMetricsExtractor extractorForDependencyMetrics;
        private readonly ExceptionMetricsExtractor extractorForExceptionMetrics;
        private readonly TraceMetricsExtractor extractorForTraceMetrics;

        /// <summary>
        /// We have dedicated instance variables to refer to each individual extractors because we are exposing some of their properties to the config subsystem here.
        /// However, for calling common methods for all of them, we also group them together.
        /// </summary>
        private readonly ExtractorWithInfo[] extractors;

        /// <summary>
        /// The <see cref="TelemetryClient"/> that will be used to send all extracted metrics.
        /// !!! All participating implementations of <see cref="ISpecificAutocollectedMetricsExtractor" /> must             !!! 
        /// !!! use <see cref="MetricAggregationScope.TelemetryClient" /> when accessing <see cref="Metric" /> for          !!! 
        /// !!! extracted metrics, and NOT the default (which is <c>MetricAggregationScope.TelemetryConfiguration</c>).         !!! 
        /// !!! This will make sure that this specific instance of <c>TelemetryClient</c> is used and its Context is respected. !!! 
        /// The above is required to ensure that all metric documents are tagged correctly and are processed using the particular 
        /// <see cref="TelemetryConfiguration"/> instance used to initialize this extractor.
        /// </summary>
        private TelemetryClient metricTelemetryClient = null;

        /// <summary>
        /// The telemetry processor that will be called after this processor.
        /// </summary>
        private ITelemetryProcessor nextProcessorInPipeline = null;

        /// <summary>
        /// Marks if we ever log MetricExtractorAfterSamplingError so that if we do we use Verbosity level subsequently.
        /// </summary>
        private bool isMetricExtractorAfterSamplingLogged = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutocollectedMetricsExtractor" /> class.
        /// </summary>
        /// <param name="nextProcessorInPipeline">Subsequent telemetry processor.</param>
        public AutocollectedMetricsExtractor(ITelemetryProcessor nextProcessorInPipeline)
        {
            this.nextProcessorInPipeline = nextProcessorInPipeline;

            this.extractorForRequestMetrics = new RequestMetricsExtractor();
            this.extractorForDependencyMetrics = new DependencyMetricsExtractor();
            this.extractorForExceptionMetrics = new ExceptionMetricsExtractor();
            this.extractorForTraceMetrics = new TraceMetricsExtractor();

            this.extractors = new ExtractorWithInfo[]
                    {
                        new ExtractorWithInfo(this.extractorForRequestMetrics, GetExtractorInfo(this.extractorForRequestMetrics)),
                        new ExtractorWithInfo(this.extractorForDependencyMetrics, GetExtractorInfo(this.extractorForDependencyMetrics)),
                        new ExtractorWithInfo(this.extractorForExceptionMetrics, GetExtractorInfo(this.extractorForExceptionMetrics)),
                        new ExtractorWithInfo(this.extractorForTraceMetrics, GetExtractorInfo(this.extractorForTraceMetrics)),
                    };
        }

        /// <summary>
        /// Gets or sets the maximum distinct values for DependencyType.
        /// Types encountered after this limit is hit will be collapsed into a single value DIMENSION_CAPPED.
        /// Setting 0 will all values to be replaced with a single value "Other".
        /// </summary>
        public int MaxDependencyTypesToDiscover
        {
            get
            {
                return this.extractorForDependencyMetrics.MaxDependencyTypesToDiscover;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value, "MaxDependencyTypesToDiscover value may not be negative.");
                }

                this.extractorForDependencyMetrics.MaxDependencyTypesToDiscover = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum distinct values for Dependency Result Code.
        /// Types encountered after this limit is hit will be collapsed into a single value DIMENSION_CAPPED.
        /// Setting 0 will all values to be replaced with a single value "Other".
        /// </summary>
        public int MaxDependencyResultCodesToDiscover
        {
            get
            {
                return this.extractorForDependencyMetrics.MaxDependencyResultCodesToDiscover;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value, "MaxDependencyResultCodesToDiscover value may not be negative.");
                }

                this.extractorForDependencyMetrics.MaxDependencyResultCodesToDiscover = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum distinct values for Dependency Target.
        /// Targets encountered after this limit is hit will be collapsed into a single value DIMENSION_CAPPED.
        /// Setting 0 will all values to be replaced with a single value "Other".
        /// </summary>
        public int MaxDependencyTargetValuesToDiscover
        {
            get
            {
                return this.extractorForDependencyMetrics.MaxDependencyTargetValuesToDiscover;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value, "MaxDependencyTargetValuesToDiscover value may not be negative.");
                }

                this.extractorForDependencyMetrics.MaxDependencyTargetValuesToDiscover = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum distinct values for CloudRoleInstance for Dependency telemetry.
        /// Values encountered after this limit is hit will be collapsed into a single value DIMENSION_CAPPED.
        /// Setting 0 will all values to be replaced with a single value "Other".
        /// </summary>
        public int MaxDependencyCloudRoleInstanceValuesToDiscover
        {
            get
            {
                return this.extractorForDependencyMetrics.MaxCloudRoleInstanceValuesToDiscover;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value, "MaxDependencyCloudRoleInstanceValuesToDiscover value may not be negative.");
                }

                this.extractorForDependencyMetrics.MaxCloudRoleInstanceValuesToDiscover = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum distinct values for CloudRoleName for Dependency telemetry.
        /// Values encountered after this limit is hit will be collapsed into a single value DIMENSION_CAPPED.
        /// Setting 0 will all values to be replaced with a single value "Other".
        /// </summary>
        public int MaxDependencyCloudRoleNameValuesToDiscover
        {
            get
            {
                return this.extractorForDependencyMetrics.MaxCloudRoleNameValuesToDiscover;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value, "MaxDependencyCloudRoleNameValuesToDiscover value may not be negative.");
                }

                this.extractorForDependencyMetrics.MaxCloudRoleNameValuesToDiscover = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum distinct values for CloudRoleInstance for Exception telemetry.
        /// Values encountered after this limit is hit will be collapsed into a single value DIMENSION_CAPPED.
        /// Setting 0 will all values to be replaced with a single value "Other".
        /// </summary>
        public int MaxExceptionCloudRoleInstanceValuesToDiscover
        {
            get
            {
                return this.extractorForExceptionMetrics.MaxCloudRoleInstanceValuesToDiscover;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value, "MaxExceptionCloudRoleInstanceValuesToDiscover value may not be negative.");
                }

                this.extractorForExceptionMetrics.MaxCloudRoleInstanceValuesToDiscover = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum distinct values for CloudRoleName for Exception telemetry.
        /// Values encountered after this limit is hit will be collapsed into a single value DIMENSION_CAPPED.
        /// Setting 0 will all values to be replaced with a single value "Other".
        /// </summary>
        public int MaxExceptionCloudRoleNameValuesToDiscover
        {
            get
            {
                return this.extractorForExceptionMetrics.MaxCloudRoleNameValuesToDiscover;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value, "MaxExceptionCloudRoleNameValuesToDiscover value may not be negative.");
                }

                this.extractorForExceptionMetrics.MaxCloudRoleNameValuesToDiscover = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum distinct values for CloudRoleInstance for Trace telemetry.
        /// Values encountered after this limit is hit will be collapsed into a single value DIMENSION_CAPPED.
        /// Setting 0 will all values to be replaced with a single value "Other".
        /// </summary>
        public int MaxTraceCloudRoleInstanceValuesToDiscover
        {
            get
            {
                return this.extractorForTraceMetrics.MaxCloudRoleInstanceValuesToDiscover;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value, "MaxTraceCloudRoleInstanceValuesToDiscover value may not be negative.");
                }

                this.extractorForTraceMetrics.MaxCloudRoleInstanceValuesToDiscover = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum distinct values for CloudRoleName for Trace telemetry.
        /// Values encountered after this limit is hit will be collapsed into a single value DIMENSION_CAPPED.
        /// Setting 0 will all values to be replaced with a single value "Other".
        /// </summary>
        public int MaxTraceCloudRoleNameValuesToDiscover
        {
            get
            {
                return this.extractorForTraceMetrics.MaxCloudRoleNameValuesToDiscover;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value, "MaxTraceCloudRoleNameValuesToDiscover value may not be negative.");
                }

                this.extractorForTraceMetrics.MaxCloudRoleNameValuesToDiscover = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum distinct values for CloudRoleInstance for Request telemetry.
        /// Values encountered after this limit is hit will be collapsed into a single value DIMENSION_CAPPED.
        /// Setting 0 will all values to be replaced with a single value "Other".
        /// </summary>
        public int MaxRequestCloudRoleInstanceValuesToDiscover
        {
            get
            {
                return this.extractorForRequestMetrics.MaxCloudRoleInstanceValuesToDiscover;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value, "MaxRequestCloudRoleInstanceValuesToDiscover value may not be negative.");
                }

                this.extractorForRequestMetrics.MaxCloudRoleInstanceValuesToDiscover = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum distinct values for CloudRoleName for Request telemetry.
        /// Values encountered after this limit is hit will be collapsed into a single value DIMENSION_CAPPED.
        /// Setting 0 will all values to be replaced with a single value "Other".
        /// </summary>
        public int MaxRequestCloudRoleNameValuesToDiscover
        {
            get
            {
                return this.extractorForRequestMetrics.MaxCloudRoleNameValuesToDiscover;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value, "MaxRequestCloudRoleNameValuesToDiscover value may not be negative.");
                }

                this.extractorForRequestMetrics.MaxCloudRoleNameValuesToDiscover = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum distinct values for Request response code.
        /// Targets encountered after this limit is hit will be collapsed into a single value DIMENSION_CAPPED.
        /// Setting 0 will all values to be replaced with a single value "Other".
        /// </summary>
        public int MaxRequestResponseCodeValuesToDiscover
        {
            get
            {
                return this.extractorForRequestMetrics.MaxResponseCodeToDiscover;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value, "MaxRequestResponseCodeValuesToDiscover value may not be negative.");
                }

                this.extractorForRequestMetrics.MaxResponseCodeToDiscover = value;
            }
        }

        /// <summary>
        /// This class implements the <see cref="ITelemetryModule"/> interface by defining this method.
        /// It will be called by the infrastructure when the telemetry pipeline is being built.
        /// This will ensure that the extractor is initialized using the same <see cref="TelemetryConfiguration" /> as the rest of the pipeline.
        /// Specifically, this will also ensure that the <see cref="TelemetryClient" /> used internally for sending extracted metrics uses
        /// the same configuration.
        /// </summary>
        /// <param name="configuration">The telemetry configuration to be used by this extractor.</param>
        public void Initialize(TelemetryConfiguration configuration)
        {
#pragma warning disable 612, 618 // TelemetryConfigration.Active and TelemetryClient()
            TelemetryClient metricsClient = (configuration == null)
                                                    ? new TelemetryClient()
                                                    : new TelemetryClient(configuration);
#pragma warning restore 612, 618 // TelemetryConfigration.Active and TelemetryClient()

            if (false == string.IsNullOrWhiteSpace(MetricTerms.Autocollection.Moniker.Key))
            {
                metricsClient.Context.GlobalProperties[MetricTerms.Autocollection.Moniker.Key] = MetricTerms.Autocollection.Moniker.Value;
            }

            this.InitializeExtractors(metricsClient);
        }

        /// <summary>
        /// This class implements the <see cref="ITelemetryProcessor"/> interface by defining this method.
        /// This method will be called by the pipeline for each telemetry item that goes through it.
        /// It invokes <see cref="ExtractMetrics(ITelemetry)"/> to actually do the extraction.
        /// </summary>
        /// <param name="item">The telemetry item from which the metrics will be extracted.</param>
        public void Process(ITelemetry item)
        {
            if (item != null)
            {
                this.ExtractMetrics(item);
            }

            this.InvokeNextProcessor(item);
        }

        /// <summary>
        /// Disposes this telemetry extractor.
        /// </summary>
        /// <remarks>This class is sealed. The pattern "private void Dispose(bool disposing)" is not applicable.</remarks>
        public void Dispose()
        {
            TelemetryClient metricsClient = this.metricTelemetryClient;
            if (metricsClient != null)
            {
                metricsClient.Flush();
                this.InitializeExtractors(metricsClient: null);
            }
        }

        /// <summary>
        /// Constructs the extractor info string for caching.
        /// </summary>
        /// <param name="extractor">The extractor to describe.</param>
        /// <returns>Extractor info string for caching.</returns>
        private static string GetExtractorInfo(ISpecificAutocollectedMetricsExtractor extractor)
        {
            string extractorName;
            string extractorVersion;

            try
            {
                extractorName = extractor?.ExtractorName ?? "null";
            }
            catch
            {
                extractorName = extractor.GetType().FullName;
            }

            try
            {
                extractorVersion = extractor.ExtractorVersion ?? "null";
            }
            catch
            {
                extractorVersion = "unknown";
            }

            string extractorInfo = String.Format(
                                        CultureInfo.InvariantCulture,
                                        MetricTerms.Extraction.ProcessedByExtractors.Moniker.ExtractorInfoTemplate,
                                        extractorName,
                                        extractorVersion);
            return extractorInfo;
        }

        /// <summary>
        /// All telemetry that has been processed by this extractor will be tagged by adding the
        /// string "<c>(Name: {ExtractorName}, Ver:{ExtractorVersion})</c>" to the <c>xxx.ProcessedByExtractors</c> property.
        /// This method adds that string to the specified telemetry item's properties.
        /// </summary>
        /// <param name="item">The telemetry item to be tagged.</param>
        /// <param name="extractorInfo">The string to be added to the item's properties.</param>
        private static void AddExtractorInfo(ITelemetry item, string extractorInfo)
        {            
            if (item is RequestTelemetry)
            {
                var req = item as RequestTelemetry;
                req.MetricExtractorInfo = ExtractionPipelineInfo(req.MetricExtractorInfo, extractorInfo);
            }
            else if (item is DependencyTelemetry)
            {
                var dep = item as DependencyTelemetry;
                dep.MetricExtractorInfo = ExtractionPipelineInfo(dep.MetricExtractorInfo, extractorInfo);
            }
            else if (item is ExceptionTelemetry)
            {
                var exp = item as ExceptionTelemetry;
                exp.MetricExtractorInfo = ExtractionPipelineInfo(exp.MetricExtractorInfo, extractorInfo);
            }
            else if (item is TraceTelemetry)
            {
                var trace = item as TraceTelemetry;
                trace.MetricExtractorInfo = ExtractionPipelineInfo(trace.MetricExtractorInfo, extractorInfo);
            }
        }

        private static string ExtractionPipelineInfo(string extractionPipelineInfo, string extractorInfo)
        {
            if (extractionPipelineInfo?.Length > 0)
            {
                extractionPipelineInfo = extractionPipelineInfo + "; ";
            }
            else
            {
                extractionPipelineInfo = String.Empty;
            }
            
            return extractionPipelineInfo + extractorInfo;
        }

        /// <summary>
        /// Calls all participating extractors to initialize themselves.
        /// </summary>
        private void InitializeExtractors(TelemetryClient metricsClient)
        {
            this.metricTelemetryClient = metricsClient;

            for (int e = 0; e < this.extractors.Length; e++)
            {
                try
                {
                    this.extractors[e].Extractor.InitializeExtractor(metricsClient);
                }
                catch (Exception ex)
                {
                    CoreEventSource.Log.LogError("Initialization error in " + this.extractors[e].Info + ": " + ex.ToString());
                }
            }
        }

        /// <summary>
        /// Calls the <see cref="ISpecificAutocollectedMetricsExtractor.ExtractMetrics(ITelemetry, out Boolean)"/> of each participating extractor for the specified item.
        /// Catches and logs all errors.
        /// If <c>isItemProcessed</c> is True, adds a corresponding marker to the item's properties.
        /// </summary>
        /// <param name="fromItem">The item from which to extract metrics.</param>
        private void ExtractMetrics(ITelemetry fromItem)
        {
            if (fromItem is ISupportSampling potentiallySampledItem && false == this.EnsureItemNotSampled(potentiallySampledItem))
            {
                return;
            }

            for (int e = 0; e < this.extractors.Length; e++)
            {
                ExtractorWithInfo participant = this.extractors[e];
                try
                {
                    bool isItemProcessed;
                    participant.Extractor.ExtractMetrics(fromItem, out isItemProcessed);

                    if (isItemProcessed)
                    {
                        AddExtractorInfo(fromItem, participant.Info);
                    }
                }
                catch (Exception ex)
                {
                    CoreEventSource.Log.LogError("Extraction error in " + participant.Extractor.GetType().Name + ": " + ex.ToString());
                }
            }
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private bool EnsureItemNotSampled(ISupportSampling item)
        {
            if (item.SamplingPercentage.HasValue
                    && item.SamplingPercentage.Value < (100.0 - 1.0E-12))
            {
                if (false == this.isMetricExtractorAfterSamplingLogged)
                {
                    //// benign race
                    this.isMetricExtractorAfterSamplingLogged = true;
                    CoreEventSource.Log.MetricExtractorAfterSamplingError();
                }
                else
                {
                    CoreEventSource.Log.MetricExtractorAfterSamplingVerbose();
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Invokes the subsequent telemetry processor if it has been initialized.
        /// </summary>
        /// <param name="item">Item to pass.</param>
        private void InvokeNextProcessor(ITelemetry item)
        {
            ITelemetryProcessor next = this.nextProcessorInPipeline;
            if (next != null)
            {
                next.Process(item);
            }
        }

        /// <summary>
        /// Groups an instance of <c>ISpecificAutocollectedMetricsExtractor</c> with a cached version of it's pipeline processing info.
        /// </summary>
        private class ExtractorWithInfo
        {
            public ExtractorWithInfo(ISpecificAutocollectedMetricsExtractor extractor, string info)
            {
                this.Extractor = extractor;
                this.Info = info;
            }

            public ISpecificAutocollectedMetricsExtractor Extractor { get; private set; }

            public string Info { get; private set; }
        }
    }
}
