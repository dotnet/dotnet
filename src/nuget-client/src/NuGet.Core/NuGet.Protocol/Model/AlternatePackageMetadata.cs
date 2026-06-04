// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text.Json.Serialization;
using Newtonsoft.Json;
using NuGet.Versioning;

namespace NuGet.Protocol
{
    public class AlternatePackageMetadata
    {
        [JsonProperty(PropertyName = JsonProperties.PackageId)]
        [JsonPropertyName(JsonProperties.PackageId)]
        [JsonInclude]
        public string? PackageId { get; internal set; }

        [JsonProperty(PropertyName = JsonProperties.Range, ItemConverterType = typeof(VersionRangeConverter))]
        [JsonPropertyName(JsonProperties.Range)]
        [JsonInclude]
        public VersionRange? Range { get; internal set; }
    }
}
