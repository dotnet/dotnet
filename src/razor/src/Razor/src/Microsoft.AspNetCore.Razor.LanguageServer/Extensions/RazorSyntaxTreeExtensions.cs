// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Legacy;
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

    public static SyntaxNode? GetOwner(this RazorSyntaxTree syntaxTree, int absoluteIndex)
    {
        if (syntaxTree is null)
        {
            throw new ArgumentNullException(nameof(syntaxTree));
        }

        var change = new SourceChange(absoluteIndex, 0, string.Empty);
        var owner = syntaxTree.Root.LocateOwner(change);
        return owner;
    }

    public static SyntaxNode? GetOwner(
        this RazorSyntaxTree syntaxTree,
        SourceText sourceText,
        Position position,
        ILogger logger) 
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

        return GetOwner(syntaxTree, absoluteIndex);
    }

    public static SyntaxNode? GetOwner(
        this RazorSyntaxTree syntaxTree,
        SourceText sourceText,
        Range range,
        ILogger logger)
    {
        if (syntaxTree is null)
        {
            throw new ArgumentNullException(nameof(syntaxTree));
        }

        if (sourceText is null)
        {
            throw new ArgumentNullException(nameof(sourceText));
        }

        if (range is null)
        {
            throw new ArgumentNullException(nameof(range));
        }

        var startInSync = range.Start.TryGetAbsoluteIndex(sourceText, logger, out var absoluteStartIndex);
        var endInSync = range.End.TryGetAbsoluteIndex(sourceText, logger, out var absoluteEndIndex);
        if (startInSync is false || endInSync is false)
        {
            return default;
        }

        var length = absoluteEndIndex - absoluteStartIndex;
        var change = new SourceChange(absoluteStartIndex, length, string.Empty);
        var owner = syntaxTree.Root.LocateOwner(change);
        return owner;
    }
}
