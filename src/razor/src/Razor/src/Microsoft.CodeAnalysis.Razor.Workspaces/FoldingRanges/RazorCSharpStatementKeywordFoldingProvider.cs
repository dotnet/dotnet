// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;

namespace Microsoft.CodeAnalysis.Razor.FoldingRanges;

internal class RazorCSharpStatementKeywordFoldingProvider : AbstractSyntaxNodeFoldingProvider<CSharpCodeBlockSyntax>
{
    protected override string GetCollapsedText(CSharpCodeBlockSyntax node)
    {
        if (node.Children is [_, CSharpStatementLiteralSyntax literal, ..] &&
            literal.LiteralTokens is [var keyword, ..])
        {
            return $"@{keyword.Content}";
        }

        return "@{...}";
    }

    protected override ImmutableArray<CSharpCodeBlockSyntax> GetFoldableNodes(RazorSyntaxTree syntaxTree)
    {
        return syntaxTree.Root
            .DescendantNodes(static node => node is RazorDocumentSyntax or MarkupBlockSyntax or MarkupElementSyntax or CSharpCodeBlockSyntax)
            .OfType<CSharpStatementLiteralSyntax>()
            .Where(n => n is
            {
                Parent: CSharpCodeBlockSyntax,
                LiteralTokens: [{ Kind: SyntaxKind.Keyword }, ..]
            })
            .SelectAsArray(d => (CSharpCodeBlockSyntax)d.Parent);
    }
}
