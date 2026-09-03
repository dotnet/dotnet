// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text.Json.Serialization;

namespace NuGet.Credentials
{
    [JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)]
    [JsonSerializable(typeof(PluginCredentialResponse))]
    internal partial class PluginCredentialResponseJsonContext : JsonSerializerContext
    {
    }
}
