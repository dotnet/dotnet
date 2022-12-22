using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class BuildAggregationSource
    {
        public BuildAggregationSource(IImmutableDictionary<string, Models.AggregatedWorkItemCounts> types)
        {
            Types = types;
        }

        [JsonProperty("Types")]
        public IImmutableDictionary<string, Models.AggregatedWorkItemCounts> Types { get; }
    }
}
