// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text.Json.Serialization;
using Microsoft.CodeAnalysis.Razor.Protocol;

namespace Microsoft.VisualStudioCode.RazorExtension.Services;

internal sealed class RazorMapTextChangesResponse
{
    [JsonPropertyName("razorDocument")]
    public required TextDocumentIdentifier RazorDocument { get; set; }

    [JsonPropertyName("mappedTextChanges")]
    public required RazorTextChange[] MappedTextChanges { get; set; }
}

