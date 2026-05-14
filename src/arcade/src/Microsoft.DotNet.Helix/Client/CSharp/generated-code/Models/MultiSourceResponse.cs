// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class MultiSourceResponse
    {
        public MultiSourceResponse(IImmutableDictionary<string, Models.MultiSourceResponseSource> sources)
        {
            Sources = sources;
        }

        [JsonProperty("Sources")]
        public IImmutableDictionary<string, Models.MultiSourceResponseSource> Sources { get; }
    }
}
