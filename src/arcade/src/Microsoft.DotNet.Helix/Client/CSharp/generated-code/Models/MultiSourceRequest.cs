using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class MultiSourceRequest
    {
        public MultiSourceRequest(IImmutableList<Models.SingleSourceRequest> sources, int? buildCount)
        {
            Sources = sources;
            BuildCount = buildCount;
        }

        [JsonProperty("Sources")]
        public IImmutableList<Models.SingleSourceRequest> Sources { get; }

        [JsonProperty("BuildCount")]
        public int? BuildCount { get; }
    }
}
