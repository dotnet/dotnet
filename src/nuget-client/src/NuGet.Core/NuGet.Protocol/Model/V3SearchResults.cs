// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace NuGet.Protocol.Model
{
    internal class V3SearchResults
    {
        [JsonProperty("totalHits")]
        public long TotalHits { get; set; }

        [JsonProperty("data")]
        public List<PackageSearchMetadata> Data { get; private set; } = new List<PackageSearchMetadata>();
    }
}
