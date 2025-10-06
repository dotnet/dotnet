// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Mapping;

internal class RazorLanguageQueryParams
{
    [JsonPropertyName("uri")]
    public required Uri Uri { get; set; }

    [JsonPropertyName("position")]
    public required Position Position { get; set; }
}
