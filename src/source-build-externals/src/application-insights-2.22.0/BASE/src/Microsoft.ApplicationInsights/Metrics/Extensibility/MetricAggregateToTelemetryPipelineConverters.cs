namespace Microsoft.ApplicationInsights.Metrics.Extensibility
{
    using System;
    using System.Collections.Concurrent;

    /// <summary>A registry for injecting converters from <c>MetricAggregate</c> items to data exchange
    /// types employed by the respective data ingestion/processing/sink mechanism. </summary>
    /// @PublicExposureCandidate
    internal sealed class MetricAggregateToTelemetryPipelineConverters 
    {
        /// <summary>Default singelton.</summary>
        public static readonly MetricAggregateToTelemetryPipelineConverters Registry = new MetricAggregateToTelemetryPipelineConverters();

        private ConcurrentDictionary<Type, ConcurrentDictionary<string, IMetricAggregateToTelemetryPipelineConverter>> pipelineTable
                                                        = new ConcurrentDictionary<Type, ConcurrentDictionary<string, IMetricAggregateToTelemetryPipelineConverter>>();

        /// <summary>Adds a converter to the registry.</summary>
        /// <param name="pipelineType">Type of the data output pipeline.</param>
        /// <param name="aggregationKindMoniker">Aggregation kind moniker.</param>
        /// <param name="converter">The converter being registered.</param>
        public void Add(Type pipelineType, string aggregationKindMoniker, IMetricAggregateToTelemetryPipelineConverter converter)
        {
            ValidateKeys(pipelineType, aggregationKindMoniker);
            Util.ValidateNotNull(converter, nameof(converter));

            ConcurrentDictionary<string, IMetricAggregateToTelemetryPipelineConverter> converters = this.pipelineTable.GetOrAdd(
                                                                                pipelineType,
                                                                                new ConcurrentDictionary<string, IMetricAggregateToTelemetryPipelineConverter>());

            converters[aggregationKindMoniker] = converter;
        }

        /// <summary>Attempts to get a metric aggregate converter from the registry.</summary>
        /// <param name="pipelineType">Type of the target pipeline.</param>
        /// <param name="aggregationKindMoniker">Aggregation kind.</param>
        /// <param name="converter">The registered converter, or <c>null</c>.</param>
        /// <returns><c>true</c> if a comverter was retrieved, <c>false</c> otherwise.</returns>
        public bool TryGet(Type pipelineType, string aggregationKindMoniker, out IMetricAggregateToTelemetryPipelineConverter converter)
        {
            ValidateKeys(pipelineType, aggregationKindMoniker);

            ConcurrentDictionary<string, IMetricAggregateToTelemetryPipelineConverter> converters;
            if (false == this.pipelineTable.TryGetValue(pipelineType, out converters))
            {
                converter = null;
                return false;
            }

            bool hasConverter = converters.TryGetValue(aggregationKindMoniker, out converter);
            return hasConverter;
        }

        private static void ValidateKeys(Type pipelineType, string aggregationKindMoniker)
        {
            Util.ValidateNotNull(pipelineType, nameof(pipelineType));

            ////if (false == typeof(IMetricTelemetryPipeline).IsAssignableFrom(pipelineType))
            ////{
            ////    throw new ArgumentException($"{nameof(pipelineType)} must specify a type that implements the interface '{nameof(IMetricTelemetryPipeline)}'"
            ////                              + $", but it specifies the type '{pipelineType.Name}' that does not implement that interface.");
            ////}

            Util.ValidateNotNullOrWhitespace(aggregationKindMoniker, nameof(aggregationKindMoniker));
        }
    }
}
