namespace Microsoft.ApplicationInsights.Extensibility.Filtering
{
    using System.Runtime.Serialization;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.ServiceContract;

    /// <summary>
    /// DTO that represents the collection configuration - a customizable description of performance counters, metrics, and full telemetry documents
    /// to be collected by the SDK. Processed and encapsulated by <see cref="CollectionConfiguration"/> at the time of actual collection.
    /// </summary>
    [DataContract]
    internal class CollectionConfigurationInfo
    {
        [DataMember]
        public string ETag { get; set; }

        [DataMember(IsRequired = false)]
        public QuotaConfigurationInfo QuotaInfo { get; set; }

        [DataMember]
        public CalculatedMetricInfo[] Metrics { get; set; }

        [DataMember]
        public DocumentStreamInfo[] DocumentStreams { get; set; }
    }
}
