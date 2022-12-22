using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class BuildHistoryItem
    {
        public BuildHistoryItem(string buildNumber, DateTimeOffset timestamp, bool passed)
        {
            BuildNumber = buildNumber;
            Timestamp = timestamp;
            Passed = passed;
        }

        [JsonProperty("BuildNumber")]
        public string BuildNumber { get; set; }

        [JsonProperty("Timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        [JsonProperty("Passed")]
        public bool Passed { get; set; }

        [JsonIgnore]
        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(BuildNumber))
                {
                    return false;
                }
                return true;
            }
        }
    }
}
