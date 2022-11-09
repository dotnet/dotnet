using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class AggregateAnalysisDetail
    {
        public AggregateAnalysisDetail(Newtonsoft.Json.Linq.JToken analysis, string job, string workItem, IImmutableDictionary<string, string> key)
        {
            Analysis = analysis;
            Job = job;
            WorkItem = workItem;
            Key = key;
        }

        [JsonProperty("Analysis")]
        public Newtonsoft.Json.Linq.JToken Analysis { get; set; }

        [JsonProperty("Job")]
        public string Job { get; set; }

        [JsonProperty("WorkItem")]
        public string WorkItem { get; set; }

        [JsonProperty("Key")]
        public IImmutableDictionary<string, string> Key { get; set; }

        [JsonIgnore]
        public bool IsValid
        {
            get
            {
                if (Analysis == default(Newtonsoft.Json.Linq.JToken))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(Job))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(WorkItem))
                {
                    return false;
                }
                if (Key == default(IImmutableDictionary<string, string>))
                {
                    return false;
                }
                return true;
            }
        }
    }
}
