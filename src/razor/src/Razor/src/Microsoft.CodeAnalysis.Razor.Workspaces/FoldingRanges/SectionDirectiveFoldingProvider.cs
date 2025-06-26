// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;

namespace Microsoft.CodeAnalysis.Razor.FoldingRanges;

internal class SectionDirectiveFoldingProvider : AbstractSyntaxNodeFoldingProvider<RazorDirectiveSyntax>
{
    protected override string GetCollapsedText(RazorDirectiveSyntax node)
    {
        return $"@{node.DirectiveDescriptor.Directive}{GetSectionName(node)}";

        static string GetSectionName(RazorDirectiveSyntax node)
        {
            if (node.Body is RazorDirectiveBodySyntax { CSharpCode.Children: [_, { } name, ..] })
            {
                return $" {name.GetContent()}";
            }

            return "";
        }
    }

    protected override ImmutableArray<RazorDirectiveSyntax> GetFoldableNodes(RazorSyntaxTree syntaxTree)
    {
        return syntaxTree.GetSectionDirectives();
    }
}
