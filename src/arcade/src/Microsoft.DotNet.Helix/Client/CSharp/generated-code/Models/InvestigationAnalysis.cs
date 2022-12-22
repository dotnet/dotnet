using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class InvestigationAnalysis
    {
        public InvestigationAnalysis(string job, string workItem, string name, Newtonsoft.Json.Linq.JToken analysis)
        {
            Job = job;
            WorkItem = workItem;
            Name = name;
            Analysis = analysis;
        }

        [JsonProperty("Job")]
        public string Job { get; set; }

        [JsonProperty("WorkItem")]
        public string WorkItem { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Analysis")]
        public Newtonsoft.Json.Linq.JToken Analysis { get; set; }

        [JsonIgnore]
        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(Job))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(WorkItem))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(Name))
                {
                    return false;
                }
                if (Analysis == default(Newtonsoft.Json.Linq.JToken))
                {
                    return false;
                }
                return true;
            }
        }
    }
}
