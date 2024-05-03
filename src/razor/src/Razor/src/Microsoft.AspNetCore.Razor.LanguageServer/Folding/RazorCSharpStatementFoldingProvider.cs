// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Folding;

internal sealed class RazorCSharpStatementFoldingProvider : AbstractSyntaxNodeFoldingProvider<CSharpStatementSyntax>
{
    protected override string GetCollapsedText(CSharpStatementSyntax node)
    {
        return "@{...}";
    }

    protected override ImmutableArray<CSharpStatementSyntax> GetFoldableNodes(RazorSyntaxTree syntaxTree)
    {
        return syntaxTree.Root
            .DescendantNodes(node => node is RazorDocumentSyntax or MarkupBlockSyntax or MarkupElementSyntax or CSharpCodeBlockSyntax)
            .OfType<CSharpStatementSyntax>()
            .SelectAsArray(d => d);
    }
}
