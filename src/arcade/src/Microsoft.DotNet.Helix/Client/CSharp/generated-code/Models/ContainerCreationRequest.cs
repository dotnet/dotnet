using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class ContainerCreationRequest
    {
        public ContainerCreationRequest(double expirationInDays, string desiredName, string targetQueue)
        {
            ExpirationInDays = expirationInDays;
            DesiredName = desiredName;
            TargetQueue = targetQueue;
        }

        [JsonProperty("ExpirationInDays")]
        public double ExpirationInDays { get; set; }

        [JsonProperty("DesiredName")]
        public string DesiredName { get; set; }

        [JsonProperty("TargetQueue")]
        public string TargetQueue { get; set; }

        [JsonIgnore]
        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(DesiredName))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(TargetQueue))
                {
                    return false;
                }
                return true;
            }
        }
    }
}
