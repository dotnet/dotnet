// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class Artifact
    {
        public Artifact()
        {
        }

        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Parameters")]
        public IImmutableDictionary<string, Newtonsoft.Json.Linq.JToken> Parameters { get; set; }
    }
}
