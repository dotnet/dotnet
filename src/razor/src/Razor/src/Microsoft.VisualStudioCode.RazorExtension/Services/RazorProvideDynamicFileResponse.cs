// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text.Json.Serialization;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
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
