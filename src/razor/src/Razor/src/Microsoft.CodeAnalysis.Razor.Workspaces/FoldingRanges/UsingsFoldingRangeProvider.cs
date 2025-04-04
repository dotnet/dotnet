// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.FoldingRanges;

internal class UsingsFoldingRangeProvider : IRazorFoldingRangeProvider
{
    public ImmutableArray<FoldingRange> GetFoldingRanges(RazorCodeDocument codeDocument)
    {
        var builder = new List<FoldingRange>();

        var razorFileSyntaxWalker = new RazorFileUsingsFoldingSyntaxWalker(codeDocument.Source);
        razorFileSyntaxWalker.Visit(codeDocument.GetSyntaxTree().Root);
        builder.AddRange(razorFileSyntaxWalker.Ranges);

        return builder.ToImmutableArray();
    }
}
