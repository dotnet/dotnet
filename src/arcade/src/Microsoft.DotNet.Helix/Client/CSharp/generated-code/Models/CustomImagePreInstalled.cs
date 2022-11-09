using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class CustomImagePreInstalled
    {
        public CustomImagePreInstalled()
        {
        }

        [JsonProperty("System")]
        public string System { get; set; }

        [JsonProperty("Processor")]
        public string Processor { get; set; }

        [JsonProperty("Version")]
        public string Version { get; set; }

        [JsonProperty("Distro")]
        public string Distro { get; set; }

        [JsonProperty("DistroVersion")]
        public string DistroVersion { get; set; }
    }
}
