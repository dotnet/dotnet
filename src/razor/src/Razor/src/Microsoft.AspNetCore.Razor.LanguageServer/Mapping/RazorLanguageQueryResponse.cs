// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text.Json.Serialization;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

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
