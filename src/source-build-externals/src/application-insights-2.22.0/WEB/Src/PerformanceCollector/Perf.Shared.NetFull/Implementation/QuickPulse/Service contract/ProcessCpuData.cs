namespace Microsoft.ManagementServices.RealTimeDataProcessing.QuickPulseService
{
    using System.Runtime.Serialization;

    [DataContract]
    internal struct ProcessCpuData
    {
        [DataMember]
        public string ProcessName { get; set; }

        [DataMember]
        public int CpuPercentage { get; set; }
    }
}