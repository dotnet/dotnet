// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text.Json.Serialization;
using NuGet.Protocol.Converters;
using NuGet.Protocol.Model;

namespace NuGet.Protocol.Utility
{
#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
    [JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
        GenerationMode = JsonSourceGenerationMode.Metadata,
        Converters = [typeof(FingerprintsStjConverter)])]
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
    [JsonSerializable(typeof(RepositorySignatureModel))]
    internal partial class RepositorySignatureJsonContext : JsonSerializerContext
    {
    }
}
