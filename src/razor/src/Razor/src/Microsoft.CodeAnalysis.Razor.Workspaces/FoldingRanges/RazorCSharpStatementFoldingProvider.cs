// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;

namespace Microsoft.CodeAnalysis.Razor.FoldingRanges;

internal class RazorCSharpStatementFoldingProvider : AbstractSyntaxNodeFoldingProvider<CSharpStatementSyntax>
{
    protected override string GetCollapsedText(CSharpStatementSyntax node)
    {
        return "@{...}";
    }

    protected override ImmutableArray<CSharpStatementSyntax> GetFoldableNodes(RazorSyntaxTree syntaxTree)
    {
        return syntaxTree.Root
            .DescendantNodes(static node => node is RazorDocumentSyntax or MarkupBlockSyntax or MarkupElementSyntax or CSharpCodeBlockSyntax)
            .OfType<CSharpStatementSyntax>()
            .SelectAsArray(d => d);
    }
}
