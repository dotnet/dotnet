// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Protocol.Folding;

internal sealed record RazorFoldingRangeResponse(ImmutableArray<FoldingRange> HtmlRanges, ImmutableArray<FoldingRange> CSharpRanges)
{
    public static readonly RazorFoldingRangeResponse Empty = new(ImmutableArray<FoldingRange>.Empty, ImmutableArray<FoldingRange>.Empty);
}
