// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class QueueStats
    {
        [JsonProperty("queueName")]
        public string QueueName { get; set; }

        [JsonProperty("depth")]
        public int? Depth { get; set; }

        [JsonProperty("averageRunDuration")]
        public TimeSpan? AverageRunDuration { get; set; }

        [JsonProperty("estimatedWait")]
        public TimeSpan? EstimatedWait { get; set; }

        [JsonProperty("estimatedWaitMethod")]
        public string EstimatedWaitMethod { get; set; }

        [JsonProperty("generatedAt")]
        public DateTimeOffset? GeneratedAt { get; set; }
    }
}
