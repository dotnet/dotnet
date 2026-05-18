// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using NuGet.Protocol.Converters;

namespace NuGet.Protocol.Model
{
    internal sealed class ServiceIndexModel
    {
        [JsonPropertyName("version")]
        public string Version { get; }

        [JsonPropertyName("resources")]
        public List<ServiceIndexEntryModel>? Resources { get; }

        [JsonConstructor]
        public ServiceIndexModel(string? version, List<ServiceIndexEntryModel>? resources)
        {
            if (version is null)
            {
                throw new InvalidDataException(Strings.Protocol_MissingVersion);
            }
            Version = version;
            Resources = resources;
        }
    }

    internal sealed class ServiceIndexEntryModel
    {
        [JsonPropertyName("@id")]
        public string? Id { get; set; }

        [JsonPropertyName("@type")]
        [JsonConverter(typeof(ServiceIndexEntryStringOrArrayConverter))]
        public string[] Type { get; set; } = [];

        [JsonPropertyName("clientVersion")]
        [JsonConverter(typeof(ServiceIndexEntryStringOrArrayConverter))]
        public string[]? ClientVersion { get; set; }
    }
}
