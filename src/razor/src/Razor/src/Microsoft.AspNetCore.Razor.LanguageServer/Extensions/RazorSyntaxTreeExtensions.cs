// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Extensions;

internal static class RazorSyntaxTreeExtensions
{
    public static IReadOnlyList<FormattingSpan> GetFormattingSpans(this RazorSyntaxTree syntaxTree)
    {
        if (syntaxTree is null)
        {
            throw new ArgumentNullException(nameof(syntaxTree));
        }

        var visitor = new FormattingVisitor();
        visitor.Visit(syntaxTree.Root);

        return visitor.FormattingSpans;
    }

    public static IReadOnlyList<RazorDirectiveSyntax> GetCodeBlockDirectives(this RazorSyntaxTree syntaxTree)
    {
        if (syntaxTree is null)
        {
            throw new ArgumentNullException(nameof(syntaxTree));
        }

        // We want all nodes of type RazorDirectiveSyntax which will contain code.
        // Since code block directives occur at the top-level, we don't need to dive deeper into unrelated nodes.
        var codeBlockDirectives = syntaxTree.Root
            .DescendantNodes(node => node is RazorDocumentSyntax || node is MarkupBlockSyntax || node is CSharpCodeBlockSyntax)
            .OfType<RazorDirectiveSyntax>()
            .Where(directive => directive.DirectiveDescriptor?.Kind == DirectiveKind.CodeBlock)
            .ToList();

        return codeBlockDirectives;
    }

    public static IReadOnlyList<CSharpStatementSyntax> GetCSharpStatements(this RazorSyntaxTree syntaxTree)
    {
        if (syntaxTree is null)
        {
            throw new ArgumentNullException(nameof(syntaxTree));
        }

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
        if (syntaxTree is null)
        {
            throw new ArgumentNullException(nameof(syntaxTree));
        }

        if (sourceText is null)
        {
            throw new ArgumentNullException(nameof(sourceText));
        }

        if (position is null)
        {
            throw new ArgumentNullException(nameof(position));
        }

        if (logger is null)
        {
            throw new ArgumentNullException(nameof(logger));
        }

        if (!position.TryGetAbsoluteIndex(sourceText, logger, out var absoluteIndex))
        {
            return default;
        }

        return syntaxTree.Root.FindInnermostNode(absoluteIndex, includeWhitespace);
    }
}
