using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class DetailedVMScalingHistory
    {
        public DetailedVMScalingHistory(string scaleSetName, string vMState, int vMCount, DateTimeOffset timestamp)
        {
            ScaleSetName = scaleSetName;
            VMState = vMState;
            VMCount = vMCount;
            Timestamp = timestamp;
        }

        [JsonProperty("ScaleSetName")]
        public string ScaleSetName { get; set; }

        [JsonProperty("VMState")]
        public string VMState { get; set; }

        [JsonProperty("VMCount")]
        public int VMCount { get; set; }

        [JsonProperty("QueueName")]
        public string QueueName { get; set; }

        [JsonProperty("MessageCount")]
        public int? MessageCount { get; set; }

        [JsonProperty("Timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        [JsonIgnore]
        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(ScaleSetName))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(VMState))
                {
                    return false;
                }
                return true;
            }
        }
    }
}
