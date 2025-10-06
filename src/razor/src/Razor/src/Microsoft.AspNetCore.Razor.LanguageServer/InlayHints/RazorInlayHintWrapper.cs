// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json.Serialization;
using Microsoft.CodeAnalysis.Razor.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Workspaces.InlayHints;

internal class RazorInlayHintWrapper
{
    [JsonPropertyName("textDocument")]
    public required TextDocumentIdentifierAndVersion TextDocument { get; set; }
    [JsonPropertyName("originalData")]
    public required object? OriginalData { get; set; }
    [JsonPropertyName("originalPosition")]
    public required Position OriginalPosition { get; set; }
}
