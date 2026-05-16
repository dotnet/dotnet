// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text.Json.Serialization;

namespace NuGet.Protocol.Model
{
    internal sealed class AutoCompleteModel
    {
        [JsonPropertyName("data")]
        public string[]? Data { get; set; }
    }
}
