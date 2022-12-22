using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class InvestigationGroup
    {
        public InvestigationGroup(IImmutableDictionary<string, string> key, IImmutableDictionary<string, IImmutableList<Models.InvestigationAnalysis>> data)
        {
            Key = key;
            Data = data;
        }

        [JsonProperty("Key")]
        public IImmutableDictionary<string, string> Key { get; set; }

        [JsonProperty("Data")]
        public IImmutableDictionary<string, IImmutableList<Models.InvestigationAnalysis>> Data { get; set; }

        [JsonIgnore]
        public bool IsValid
        {
            get
            {
                if (Key == default(IImmutableDictionary<string, string>))
                {
                    return false;
                }
                if (Data == default(IImmutableDictionary<string, IImmutableList<Models.InvestigationAnalysis>>))
                {
                    return false;
                }
                return true;
            }
        }
    }
}
