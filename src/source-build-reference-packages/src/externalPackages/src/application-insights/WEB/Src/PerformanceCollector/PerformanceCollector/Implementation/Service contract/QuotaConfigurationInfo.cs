namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.ServiceContract
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    [DataContract]
    internal class QuotaConfigurationInfo
    {
        [DataMember(IsRequired = false)]
        public float? InitialQuota { get; set; }

        [DataMember(IsRequired = true)]
        public float MaxQuota { get; set; }

        [DataMember(IsRequired = true)]
        public float QuotaAccrualRatePerSec { get; set; }
    }
}
