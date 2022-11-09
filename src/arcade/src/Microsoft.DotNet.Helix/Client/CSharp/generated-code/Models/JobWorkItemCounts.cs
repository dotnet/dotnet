using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class JobWorkItemCounts
    {
        public JobWorkItemCounts(int unscheduled, int waiting, int running, int finished, string listUrl)
        {
            Unscheduled = unscheduled;
            Waiting = waiting;
            Running = running;
            Finished = finished;
            ListUrl = listUrl;
        }

        [JsonProperty("Unscheduled")]
        public int Unscheduled { get; set; }

        [JsonProperty("Waiting")]
        public int Waiting { get; set; }

        [JsonProperty("Running")]
        public int Running { get; set; }

        [JsonProperty("Finished")]
        public int Finished { get; set; }

        [JsonProperty("ListUrl")]
        public string ListUrl { get; set; }

        [JsonIgnore]
        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(ListUrl))
                {
                    return false;
                }
                return true;
            }
        }
    }
}
