using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class JobInfo
    {
        public JobInfo(string queueId, string source, string type, string build)
        {
            QueueId = queueId;
            Source = source;
            Type = type;
            Build = build;
        }

        [JsonProperty("QueueId")]
        public string QueueId { get; set; }

        [JsonProperty("Source")]
        public string Source { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("Build")]
        public string Build { get; set; }

        [JsonProperty("Attempt")]
        public string Attempt { get; set; }

        [JsonProperty("Properties")]
        public IImmutableDictionary<string, string> Properties { get; set; }

        [JsonProperty("InitialWorkItemCount")]
        public int? InitialWorkItemCount { get; set; }

        [JsonIgnore]
        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(QueueId))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(Source))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(Type))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(Build))
                {
                    return false;
                }
                return true;
            }
        }
    }
}
