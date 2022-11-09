using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class WorkItemError
    {
        public WorkItemError(string id, string message)
        {
            Id = id;
            Message = message;
        }

        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Message")]
        public string Message { get; set; }

        [JsonProperty("LogUri")]
        public string LogUri { get; set; }

        [JsonIgnore]
        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(Id))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(Message))
                {
                    return false;
                }
                return true;
            }
        }
    }
}
