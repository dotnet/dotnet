using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class BuildAggregation
    {
        public BuildAggregation(string buildNumber, IImmutableDictionary<string, Models.BuildAggregationSource> sources)
        {
            BuildNumber = buildNumber;
            Sources = sources;
        }

        [JsonProperty("BuildNumber")]
        public string BuildNumber { get; set; }

        [JsonProperty("Sources")]
        public IImmutableDictionary<string, Models.BuildAggregationSource> Sources { get; }

        [JsonIgnore]
        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(BuildNumber))
                {
                    return false;
                }
                return true;
            }
        }
    }
}
