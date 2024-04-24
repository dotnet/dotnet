// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Folding;

internal sealed class RazorCodeBlockFoldingProvider : AbstractSyntaxNodeFoldingProvider<RazorDirectiveSyntax>
{
    protected override string GetCollapsedText(RazorDirectiveSyntax node)
    {
        return "@" + node.DirectiveDescriptor.Directive;
    }

    protected override ImmutableArray<RazorDirectiveSyntax> GetFoldableNodes(RazorSyntaxTree syntaxTree)
    {
        return syntaxTree.GetCodeBlockDirectives();
    }
}
