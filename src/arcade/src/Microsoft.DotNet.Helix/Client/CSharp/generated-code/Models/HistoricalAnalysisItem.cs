// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class HistoricalAnalysisItem
    {
        public HistoricalAnalysisItem()
        {
        }

        [JsonProperty("Queued")]
        public DateTimeOffset? Queued { get; set; }

        [JsonProperty("Build")]
        public string Build { get; set; }

        [JsonProperty("Results")]
        public IImmutableDictionary<string, int> Results { get; set; }
    }
}
