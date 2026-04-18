namespace Microsoft.ManagementServices.RealTimeDataProcessing.QuickPulseService
{
    using System.Runtime.Serialization;

    [DataContract]
    internal struct MetricPoint
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public double Value { get; set; }

        [DataMember]
        public int Weight { get; set; }
    }
}
