// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class JobCreationRequest
    {
        public JobCreationRequest(string type, string listUri, string queueId)
        {
            Type = type;
            ListUri = listUri;
            QueueId = queueId;
        }

        [JsonProperty("Creator")]
        public string Creator { get; set; }

        [JsonProperty("Source")]
        public string Source { get; set; }

        [JsonProperty("SourcePrefix")]
        public string SourcePrefix { get; set; }

        [JsonProperty("TeamProject")]
        public string TeamProject { get; set; }

        [JsonProperty("Repository")]
        public string Repository { get; set; }

        [JsonProperty("Branch")]
        public string Branch { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("Properties")]
        public IImmutableDictionary<string, string> Properties { get; set; }

        [JsonProperty("ListUri")]
        public string ListUri { get; set; }

        [JsonProperty("QueueId")]
        public string QueueId { get; set; }

        [JsonProperty("QueueAlias")]
        public string QueueAlias { get; set; }

        [JsonProperty("DockerTag")]
        public string DockerTag { get; set; }

        [JsonProperty("ResultContainerPrefix")]
        public string ResultContainerPrefix { get; set; }

        [JsonIgnore]
        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(Type))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(ListUri))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(QueueId))
                {
                    return false;
                }
                return true;
            }
        }
    }
}
