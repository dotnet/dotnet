// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Language;

internal static class RazorSyntaxTreeExtensions
{
    public static ImmutableArray<RazorDirectiveSyntax> GetSectionDirectives(this RazorSyntaxTree syntaxTree)
        => GetDirectives(syntaxTree, static d => d.DirectiveDescriptor?.Directive == SectionDirective.Directive.Directive);

    public static ImmutableArray<RazorDirectiveSyntax> GetCodeBlockDirectives(this RazorSyntaxTree syntaxTree)
        => GetDirectives(syntaxTree, static d => d.DirectiveDescriptor?.Kind == DirectiveKind.CodeBlock);

    private static ImmutableArray<RazorDirectiveSyntax> GetDirectives(RazorSyntaxTree syntaxTree, Func<RazorDirectiveSyntax, bool> predicate)
    {
        using var builder = new PooledArrayBuilder<RazorDirectiveSyntax>();

        foreach (var node in syntaxTree.Root.DescendantNodes(ShouldDescendIntoChildren))
        {
            if (node is RazorDirectiveSyntax directive && predicate(directive))
            {
                builder.Add(directive);
            }
        }

        return builder.ToImmutable();

        static bool ShouldDescendIntoChildren(SyntaxNode node)
        {
            return node is RazorDocumentSyntax or MarkupBlockSyntax or CSharpCodeBlockSyntax;
        }
    }
}
