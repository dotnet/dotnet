// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Text.Json.Serialization;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;

namespace Microsoft.CodeAnalysis.Razor.Protocol.DocumentMapping;

internal class RazorMapToDocumentRangesParams
{
    [JsonPropertyName("kind")]
    public RazorLanguageKind Kind { get; init; }

    [JsonPropertyName("razorDocumentUri")]
    public required Uri RazorDocumentUri { get; init; }

    [JsonPropertyName("projectedRanges")]
    public required LspRange[] ProjectedRanges { get; init; }

    [JsonPropertyName("mappingBehavior")]
    public MappingBehavior MappingBehavior { get; init; }
}
