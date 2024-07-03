// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Text.Json.Serialization;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Protocol.Folding;

internal sealed record RazorFoldingRangeResponse(
    [property: JsonPropertyName("htmlRanges")] ImmutableArray<FoldingRange> HtmlRanges,
    [property: JsonPropertyName("csharpRanges")] ImmutableArray<FoldingRange> CSharpRanges)
{
    public static readonly RazorFoldingRangeResponse Empty = new(ImmutableArray<FoldingRange>.Empty, ImmutableArray<FoldingRange>.Empty);
}
