// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

using Microsoft.AspNetCore.Razor.Language.Syntax;

internal static class RazorSyntaxTreeExtensions
{
    public static ImmutableArray<RazorDirectiveSyntax> GetSectionDirectives(this RazorSyntaxTree syntaxTree)
    {
        return GetDirectives(syntaxTree, directive => directive.DirectiveDescriptor?.Directive == SectionDirective.Directive.Directive);
    }

    public static ImmutableArray<RazorDirectiveSyntax> GetCodeBlockDirectives(this RazorSyntaxTree syntaxTree)
    {
        return GetDirectives(syntaxTree, directive => directive.DirectiveDescriptor?.Kind == DirectiveKind.CodeBlock);
    }

    private static ImmutableArray<RazorDirectiveSyntax> GetDirectives(RazorSyntaxTree syntaxTree, Func<RazorDirectiveSyntax, bool> predicate)
    {
        return syntaxTree.Root
            .DescendantNodes(node => node is RazorDocumentSyntax or MarkupBlockSyntax or CSharpCodeBlockSyntax)
            .OfType<RazorDirectiveSyntax>()
            .Where(predicate)
            .SelectAsArray(d => d);
    }

    public static IReadOnlyList<CSharpStatementSyntax> GetCSharpStatements(this RazorSyntaxTree syntaxTree)
    {
        // We want all nodes that represent Razor C# statements, @{ ... }.
        var statements = syntaxTree.Root.DescendantNodes().OfType<CSharpStatementSyntax>().ToList();
        return statements;
    }

    public static SyntaxNode? FindInnermostNode(
        this RazorSyntaxTree syntaxTree,
        SourceText sourceText,
        Position position,
        ILogger logger,
        bool includeWhitespace = false)
    {
        if (!position.TryGetAbsoluteIndex(sourceText, logger, out var absoluteIndex))
        {
            return default;
        }

        return syntaxTree.Root.FindInnermostNode(absoluteIndex, includeWhitespace);
    }
}
