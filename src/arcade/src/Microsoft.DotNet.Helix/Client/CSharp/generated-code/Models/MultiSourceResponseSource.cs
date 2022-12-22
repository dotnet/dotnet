using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class MultiSourceResponseSource
    {
        public MultiSourceResponseSource(IImmutableDictionary<string, IImmutableList<Models.AggregatedWorkItemCounts>> types)
        {
            Types = types;
        }

        [JsonProperty("Types")]
        public IImmutableDictionary<string, IImmutableList<Models.AggregatedWorkItemCounts>> Types { get; }
    }
}
