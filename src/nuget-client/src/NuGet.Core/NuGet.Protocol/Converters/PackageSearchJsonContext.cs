// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text.Json.Serialization;
using NuGet.Packaging.Core;
using NuGet.Protocol.Model;

namespace NuGet.Protocol.Converters
{
#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
    [JsonSourceGenerationOptions(
        PropertyNameCaseInsensitive = true,
        GenerationMode = JsonSourceGenerationMode.Metadata,
        Converters = new[] { typeof(PackageDependencyGroupStjConverter), typeof(SafeUriStjConverter), typeof(VersionRangeStjConverter), typeof(NuGetVersionStjConverter) })]
#pragma warning restore CS3016
    [JsonSerializable(typeof(V3SearchResults))]
    [JsonSerializable(typeof(PackageDependency))]
    internal partial class PackageSearchJsonContext : JsonSerializerContext
    {
    }
}
