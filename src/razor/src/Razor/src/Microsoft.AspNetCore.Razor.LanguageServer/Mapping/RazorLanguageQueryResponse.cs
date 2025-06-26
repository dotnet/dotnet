// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json.Serialization;
using Microsoft.CodeAnalysis.Razor.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Mapping;

internal class RazorLanguageQueryResponse
{
    [JsonPropertyName("kind")]
    public RazorLanguageKind Kind { get; set; }

    [JsonPropertyName("positionIndex")]
    public int PositionIndex { get; set; }

    [JsonPropertyName("position")]
    public required Position Position { get; set; }

    [JsonPropertyName("hostDocumentVersion")]
    public int? HostDocumentVersion { get; set; }
}
