// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class AggregatedWorkItemCounts
    {
        public AggregatedWorkItemCounts(IImmutableDictionary<string, string> key, Models.WorkItemStatusCounts data)
        {
            Key = key;
            Data = data;
        }

        [JsonProperty("Key")]
        public IImmutableDictionary<string, string> Key { get; set; }

        [JsonProperty("Data")]
        public Models.WorkItemStatusCounts Data { get; set; }

        [JsonIgnore]
        public bool IsValid
        {
            get
            {
                if (Key == default(IImmutableDictionary<string, string>))
                {
                    return false;
                }
                if (Data == default(Models.WorkItemStatusCounts))
                {
                    return false;
                }
                return true;
            }
        }
    }
}
