// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace NuGet.Protocol.Model
{
    internal class V3SearchResults
    {
        [JsonProperty("totalHits")]
        [JsonPropertyName("totalHits")]
        public long TotalHits { get; set; }

        [JsonProperty("data")]
        [JsonPropertyName("data")]
        public List<PackageSearchMetadata> Data { get; set; } = new List<PackageSearchMetadata>();
    }
}
