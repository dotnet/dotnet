using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class MachineStateChangeRequest
    {
        public MachineStateChangeRequest()
        {
        }

        [JsonProperty("Enabled")]
        public bool? Enabled { get; set; }

        [JsonProperty("Reason")]
        public string Reason { get; set; }
    }
}
