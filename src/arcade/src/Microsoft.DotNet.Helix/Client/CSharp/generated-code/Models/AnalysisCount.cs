using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class AnalysisCount
    {
        public AnalysisCount(string name, IImmutableDictionary<string, int> status)
        {
            Name = name;
            Status = status;
        }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Status")]
        public IImmutableDictionary<string, int> Status { get; set; }

        [JsonIgnore]
        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(Name))
                {
                    return false;
                }
                if (Status == default(IImmutableDictionary<string, int>))
                {
                    return false;
                }
                return true;
            }
        }
    }
}
