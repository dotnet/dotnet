// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.VisualStudio.Editor.Razor;

namespace Microsoft.CodeAnalysis.Razor.Completion;

#pragma warning disable IDE0065 // Misplaced using directive
using SyntaxKind = AspNetCore.Razor.Language.SyntaxKind;
using SyntaxNode = AspNetCore.Razor.Language.Syntax.SyntaxNode;
#pragma warning restore IDE0065 // Misplaced using directive

internal abstract class AbstractRazorCompletionFactsService(ImmutableArray<IRazorCompletionItemProvider> providers) : IRazorCompletionFactsService
{
    private readonly ImmutableArray<IRazorCompletionItemProvider> _providers = providers;

    public ImmutableArray<RazorCompletionItem> GetCompletionItems(RazorCompletionContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (context.TagHelperDocumentContext is null)
        {
            throw new ArgumentNullException(nameof(context.TagHelperDocumentContext));
        }

        using var completions = new PooledArrayBuilder<RazorCompletionItem>();

        foreach (var provider in _providers)
        {
            var items = provider.GetCompletionItems(context);
            completions.AddRange(items);
        }

        return completions.DrainToImmutable();
    }

    // Internal for testing
    [return: NotNullIfNotNull(nameof(originalNode))]
    internal static SyntaxNode? AdjustSyntaxNodeForWordBoundary(SyntaxNode? originalNode, int requestIndex)
    {
        if (originalNode == null)
        {
            return null;
        }

        // If we're on a word boundary, ie: <a hr| />, then with `includeWhitespace`, we'll get back back a whitespace node.
        // For completion purposes, we want to walk one token back in this case. For the scenario like <a hr | />, the start of the
        // node that FindInnermostNode will return is not the request index, so we won't end up walking that back.
        // If we ever move to roslyn-style trivia, where whitespace is attached to the token, we can remove this, and simply check
        // to see whether the absolute index is in the Span of the node in the relevant providers.
        // Note - this also addresses directives, including with cursor at EOF, e.g. @fun|
        if (originalNode.SpanStart == requestIndex
            // allow zero-length tokens for cases when cursor is at EOF,
            // e.g. see https://github.com/dotnet/razor/issues/9955
            && originalNode.GetFirstToken(includeZeroWidth: true) is { } startToken
            && startToken.GetPreviousToken() is { } previousToken)
        {
            Debug.Assert(previousToken.Span.End == requestIndex);
            Debug.Assert(previousToken.Kind != SyntaxKind.Marker);
            return previousToken.Parent;
        }

        // We also want to walk back for cases like <a hr|/>, which do not involve whitespace at all. For this case, we want
        // to see if we're on the closing slash or angle bracket of a start or end tag
        if (HtmlFacts.TryGetElementInfo(originalNode, containingTagNameToken: out _, attributeNodes: out _, closingForwardSlashOrCloseAngleToken: out var closingForwardSlashOrCloseAngleToken)
            && closingForwardSlashOrCloseAngleToken.SpanStart == requestIndex
            && closingForwardSlashOrCloseAngleToken.GetPreviousToken() is { } previousToken2)
        {
            Debug.Assert(previousToken2.Span.End == requestIndex);
            return previousToken2.Parent;
        }

        return originalNode;
    }
}
