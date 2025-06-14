// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Language;

internal static class RazorSyntaxTreeExtensions
{
    public static ImmutableArray<RazorDirectiveSyntax> GetSectionDirectives(this RazorSyntaxTree syntaxTree)
        => GetDirectives(syntaxTree, static d => d.IsSectionDirective());

    public static ImmutableArray<RazorDirectiveSyntax> GetCodeBlockDirectives(this RazorSyntaxTree syntaxTree)
        => GetDirectives(syntaxTree, static d => d.IsCodeBlockDirective());

    public static ImmutableArray<RazorDirectiveSyntax> GetUsingDirectives(this RazorSyntaxTree syntaxTree)
        => GetDirectives(syntaxTree, static d => d.IsUsingDirective());

    public static ImmutableArray<RazorDirectiveSyntax> GetDirectives(
        this RazorSyntaxTree syntaxTree, Func<RazorDirectiveSyntax, bool>? predicate = null)
    {
        using var builder = new PooledArrayBuilder<RazorDirectiveSyntax>();
        builder.AddRange(EnumerateDirectives(syntaxTree, predicate));

        return builder.ToImmutable();
    }

    public static IEnumerable<RazorDirectiveSyntax> EnumerateSectionDirectives(this RazorSyntaxTree syntaxTree)
        => EnumerateDirectives(syntaxTree, static d => d.IsSectionDirective());

    public static IEnumerable<RazorDirectiveSyntax> EnumerateCodeBlockDirectives(this RazorSyntaxTree syntaxTree)
        => EnumerateDirectives(syntaxTree, static d => d.IsCodeBlockDirective());

    public static IEnumerable<RazorDirectiveSyntax> EnumerateUsingDirectives(this RazorSyntaxTree syntaxTree)
        => EnumerateDirectives(syntaxTree, static d => d.IsUsingDirective());

    public static IEnumerable<RazorDirectiveSyntax> EnumerateDirectives(
        this RazorSyntaxTree syntaxTree, Func<RazorDirectiveSyntax, bool>? predicate = null)
    {
        foreach (var node in syntaxTree.Root.DescendantNodes(MayContainDirectives))
        {
            if (node is RazorDirectiveSyntax directive && (predicate == null || predicate(directive)))
            {
                yield return directive;
            }
        }

        static bool MayContainDirectives(SyntaxNode node)
        {
            return node is RazorDocumentSyntax or MarkupBlockSyntax or CSharpCodeBlockSyntax;
        }
    }
}
