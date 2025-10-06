// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json.Serialization;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.Razor.Protocol;

namespace Microsoft.VisualStudioCode.RazorExtension.Services;

internal sealed class RazorProvideDynamicFileResponse
{
    [JsonPropertyName("csharpDocument")]
    public required TextDocumentIdentifier CSharpDocument { get; set; }

    [JsonPropertyName("edits")]
    public required RazorTextChange[] Edits { get; set; }

    [JsonPropertyName("checksum")]
    public required string Checksum { get; set; }

    [JsonPropertyName("checksumAlgorithm")]
    public SourceHashAlgorithm ChecksumAlgorithm { get; set; }

    [JsonPropertyName("encodingCodePage")]
    public int? SourceEncodingCodePage { get; set; }
}
