// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Folding;

internal abstract class AbstractSyntaxNodeFoldingProvider<TNode> : IRazorFoldingRangeProvider
    where TNode : RazorSyntaxNode
{
    public async Task<ImmutableArray<FoldingRange>> GetFoldingRangesAsync(DocumentContext documentContext, CancellationToken cancellationToken)
    {
        var sourceText = await documentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
        var syntaxTree = await documentContext.GetSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
        var nodes = GetFoldableNodes(syntaxTree);

        using var builder = new PooledArrayBuilder<FoldingRange>(nodes.Length);
        foreach (var node in nodes)
        {
            sourceText.GetLineAndOffset(node.Span.Start, out var startLine, out var startOffset);
            sourceText.GetLineAndOffset(node.Span.End, out var endLine, out var endOffset);
            var foldingRange = new FoldingRange()
            {
                StartCharacter = startOffset,
                StartLine = startLine,
                EndCharacter = endOffset,
                EndLine = endLine,

                // Directives remove the "@" but for collapsing we want to keep it for users.
                // Shows "@code" instead of "code".
                CollapsedText = GetCollapsedText(node)
            };

            builder.Add(foldingRange);
        }

        return builder.DrainToImmutable();
    }

    protected abstract ImmutableArray<TNode> GetFoldableNodes(RazorSyntaxTree syntaxTree);

    protected abstract string GetCollapsedText(TNode node);
}
