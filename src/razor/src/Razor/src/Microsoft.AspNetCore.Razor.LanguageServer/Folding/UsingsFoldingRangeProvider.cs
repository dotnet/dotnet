// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Folding;

internal sealed class UsingsFoldingRangeProvider : IRazorFoldingRangeProvider
{
    public async Task<ImmutableArray<FoldingRange>> GetFoldingRangesAsync(DocumentContext documentContext, CancellationToken cancellationToken)
    {
        var builder = new List<FoldingRange>();

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

        var razorFileSyntaxWalker = new RazorFileUsingsFoldingSyntaxWalker(codeDocument.Source);
        razorFileSyntaxWalker.Visit(codeDocument.GetSyntaxTree().Root);
        builder.AddRange(razorFileSyntaxWalker.Ranges);

        return builder.ToImmutableArray();
    }
}
