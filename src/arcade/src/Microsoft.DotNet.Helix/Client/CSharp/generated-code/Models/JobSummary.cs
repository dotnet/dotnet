// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class JobSummary
    {
        public JobSummary(string detailsUrl, string name, string waitUrl, string source, string type, string build)
        {
            DetailsUrl = detailsUrl;
            Name = name;
            WaitUrl = waitUrl;
            Source = source;
            Type = type;
            Build = build;
        }

        [JsonProperty("FailureReason")]
        public Models.FailureReason FailureReason { get; set; }

        [JsonProperty("QueueId")]
        public string QueueId { get; set; }

        [JsonProperty("DetailsUrl")]
        public string DetailsUrl { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Creator")]
        public string Creator { get; set; }

        [JsonProperty("Created")]
        public string Created { get; set; }

        [JsonProperty("Finished")]
        public string Finished { get; set; }

        [JsonProperty("InitialWorkItemCount")]
        public int? InitialWorkItemCount { get; set; }

        [JsonProperty("WaitUrl")]
        public string WaitUrl { get; set; }

        [JsonProperty("Source")]
        public string Source { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("Build")]
        public string Build { get; set; }

        [JsonProperty("Properties")]
        public Newtonsoft.Json.Linq.JToken Properties { get; set; }

        [JsonProperty("Errors")]
        public IImmutableList<Models.WorkItemError> Errors { get; set; }

        [JsonIgnore]
        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(DetailsUrl))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(Name))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(WaitUrl))
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
