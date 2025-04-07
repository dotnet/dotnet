// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
