// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class AggregatedVMScalingHistory
    {
        public AggregatedVMScalingHistory(DateTimeOffset timestamp, int totalMessageCount, int totalVMCount)
        {
            Timestamp = timestamp;
            TotalMessageCount = totalMessageCount;
            TotalVMCount = totalVMCount;
        }

        [JsonProperty("Timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        [JsonProperty("TotalMessageCount")]
        public int TotalMessageCount { get; set; }

        [JsonProperty("TotalVMCount")]
        public int TotalVMCount { get; set; }
    }
}
