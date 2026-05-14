// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class BuildAggregationSource
    {
        public BuildAggregationSource(IImmutableDictionary<string, Models.AggregatedWorkItemCounts> types)
        {
            Types = types;
        }

        [JsonProperty("Types")]
        public IImmutableDictionary<string, Models.AggregatedWorkItemCounts> Types { get; }
    }
}
