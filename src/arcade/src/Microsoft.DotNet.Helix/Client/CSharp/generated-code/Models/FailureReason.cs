using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class FailureReason
    {
        public FailureReason()
        {
        }

        [JsonProperty("Issue")]
        public Models.FailureReasonPart Issue { get; set; }

        [JsonProperty("Owner")]
        public Models.FailureReasonPart Owner { get; set; }
    }
}
