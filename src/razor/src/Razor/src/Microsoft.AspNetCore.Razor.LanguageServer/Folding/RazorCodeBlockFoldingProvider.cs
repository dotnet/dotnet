// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Folding;

internal sealed class RazorCodeBlockFoldingProvider : IRazorFoldingRangeProvider
{
    public async Task<ImmutableArray<FoldingRange>> GetFoldingRangesAsync(DocumentContext documentContext, CancellationToken cancellationToken)
    {
        var sourceText = await documentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
        var syntaxTree = await documentContext.GetSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
        var codeBlocks = syntaxTree.GetCodeBlockDirectives();

        var builder = new List<FoldingRange>();
        foreach (var codeBlock in codeBlocks)
        {
            sourceText.GetLineAndOffset(codeBlock.Span.Start, out var startLine, out var startOffset);
            sourceText.GetLineAndOffset(codeBlock.Span.End, out var endLine, out var endOffset);
            var foldingRange = new FoldingRange()
            {
                StartCharacter = startOffset,
                StartLine = startLine,
                EndCharacter = endOffset,
                EndLine = endLine,

                // Directives remove the "@" but for collapsing we want to keep it for users.
                // Shows "@code" instead of "code".
                CollapsedText = "@" + codeBlock.DirectiveDescriptor.Directive
            };

            builder.Add(foldingRange);
        }

        return builder.ToImmutableArray();
    }
}
