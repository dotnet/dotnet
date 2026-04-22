namespace Microsoft.ManagementServices.RealTimeDataProcessing.QuickPulseService
{
    using System;
    using System.Runtime.Serialization;

    using Microsoft.ApplicationInsights.Extensibility.Filtering;

    [DataContract]
    [KnownType(typeof(RequestTelemetryDocument))]
    [KnownType(typeof(DependencyTelemetryDocument))]
    [KnownType(typeof(ExceptionTelemetryDocument))]
    [KnownType(typeof(EventTelemetryDocument))]
    [KnownType(typeof(TraceTelemetryDocument))]
    internal struct MonitoringDataPoint
    {
        /*
         * 5 - adding support for ping interval hint and endpoint redirect hint
         * 4 - adding errors for extended backchannel, adding EventTelemetryDocument and TraceTelemetryDocument, adding DocumentStreamId for full documents,
         *      adding ProcessorCount
         * 3 - adding TopCpuProcesses
        */
        public const int CurrentInvariantVersion = 5;

        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public int InvariantVersion { get; set; }

        [DataMember]
        public string InstrumentationKey { get; set; }

        [DataMember]
        public string Instance { get; set; }

        [DataMember]
        public string RoleName { get; set; }

        [DataMember]
        public string StreamId { get; set; }

        [DataMember]
        public string MachineName { get; set; }

        [DataMember]
        public DateTime Timestamp { get; set; }

        [DataMember]
        public bool IsWebApp { get; set; }

        [DataMember]
        public bool PerformanceCollectionSupported { get; set; }

        [DataMember]
        public int ProcessorCount { get; set; }

        [DataMember]
        public MetricPoint[] Metrics { get; set; }

        [DataMember]
        public ITelemetryDocument[] Documents { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool GlobalDocumentQuotaReached { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ProcessCpuData[] TopCpuProcesses { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool TopCpuDataAccessDenied { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public CollectionConfigurationError[] CollectionConfigurationErrors { get; set; }
    }
}