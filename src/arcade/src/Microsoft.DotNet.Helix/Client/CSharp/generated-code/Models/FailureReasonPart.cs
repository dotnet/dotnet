using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class FailureReasonPart
    {
        public FailureReasonPart()
        {
        }

        [JsonProperty("Display")]
        public string Display { get; set; }

        [JsonProperty("Link")]
        public string Link { get; set; }
    }
}
