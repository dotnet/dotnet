// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text.Json.Serialization;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.Protocol;

internal sealed class UpdateBufferRequest
{
    [JsonPropertyName("hostDocumentVersion")]
    public int? HostDocumentVersion { get; set; }

    [JsonPropertyName("previousHostDocumentVersion")]
    public int? PreviousHostDocumentVersion { get; set; }

    [JsonPropertyName("projectKeyId")]
    public string? ProjectKeyId { get; set; }

    [JsonPropertyName("hostDocumentFilePath")]
    public string? HostDocumentFilePath { get; set; }

    [JsonPropertyName("changes")]
    public required RazorTextChange[] Changes { get; set; }

    [JsonPropertyName("previousWasEmpty")]
    public bool PreviousWasEmpty { get; set; }

    [JsonPropertyName("checksum")]
    public required string Checksum { get; set; }

    [JsonPropertyName("checksumAlgorithm")]
    public SourceHashAlgorithm ChecksumAlgorithm { get; set; }

    [JsonPropertyName("encodingCodePage")]
    public int? SourceEncodingCodePage { get; set; }
}
