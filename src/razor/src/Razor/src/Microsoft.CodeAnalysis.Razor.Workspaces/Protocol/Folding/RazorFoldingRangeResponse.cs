// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace Microsoft.CodeAnalysis.Razor.Protocol.Folding;

internal sealed record RazorFoldingRangeResponse(
    [property: JsonPropertyName("htmlRanges")] ImmutableArray<FoldingRange> HtmlRanges,
    [property: JsonPropertyName("csharpRanges")] ImmutableArray<FoldingRange> CSharpRanges)
{
    public static readonly RazorFoldingRangeResponse Empty = new(ImmutableArray<FoldingRange>.Empty, ImmutableArray<FoldingRange>.Empty);
}
