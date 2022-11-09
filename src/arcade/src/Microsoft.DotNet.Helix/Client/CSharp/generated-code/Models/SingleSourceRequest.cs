using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class SingleSourceRequest
    {
        public SingleSourceRequest(string name, IImmutableList<string> types)
        {
            Name = name;
            Types = types;
        }

        [JsonProperty("Name")]
        public string Name { get; }

        [JsonProperty("Types")]
        public IImmutableList<string> Types { get; }
    }
}
