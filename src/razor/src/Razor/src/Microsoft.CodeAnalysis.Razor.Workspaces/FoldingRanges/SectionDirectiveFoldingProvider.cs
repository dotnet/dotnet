// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
