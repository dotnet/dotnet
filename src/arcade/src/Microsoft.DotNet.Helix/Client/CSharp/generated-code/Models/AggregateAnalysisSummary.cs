// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class AggregateAnalysisSummary
    {
        public AggregateAnalysisSummary(string type, string name, IImmutableDictionary<string, int> status)
        {
            Type = type;
            Name = name;
            Status = status;
        }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Status")]
        public IImmutableDictionary<string, int> Status { get; set; }

        [JsonIgnore]
        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(Type))
                {
                    return false;
                }
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
