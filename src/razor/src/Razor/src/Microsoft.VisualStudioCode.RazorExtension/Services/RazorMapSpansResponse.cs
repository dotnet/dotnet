// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json.Serialization;
using Microsoft.CodeAnalysis.Razor.Protocol;

namespace Microsoft.VisualStudioCode.RazorExtension.Services;

internal sealed class RazorMapSpansResponse
{
    [JsonPropertyName("ranges")]
    public required LspRange[] Ranges { get; set; }

    [JsonPropertyName("spans")]
    public required RazorTextSpan[] Spans { get; set; }

    [JsonPropertyName("razorDocument")]
    public required TextDocumentIdentifier RazorDocument { get; set; }
}
