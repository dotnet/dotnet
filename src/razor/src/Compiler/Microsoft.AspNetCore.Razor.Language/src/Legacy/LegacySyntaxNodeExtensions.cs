// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Razor.Language.Syntax;

namespace Microsoft.AspNetCore.Razor.Language.Legacy;

internal static partial class LegacySyntaxNodeExtensions
{
    private class SpanData
    {
        public SyntaxNode? Previous;
        public bool PreviousComputed;
        public SyntaxNode? Next;
        public bool NextComputed;
    }

    /// <summary>
    ///  Caches previous/next span result for a particular node. A conditional weak table
    ///  is used to avoid adding fields to all syntax nodes.
    /// </summary>
    private static readonly ConditionalWeakTable<SyntaxNode, SpanData> s_spanDataTable = new();

    private static readonly ImmutableHashSet<SyntaxKind> s_transitionSpanKinds = ImmutableHashSet.Create(
        SyntaxKind.CSharpTransition,
        SyntaxKind.MarkupTransition);

    private static readonly ImmutableHashSet<SyntaxKind> s_metaCodeSpanKinds = ImmutableHashSet.Create(
        SyntaxKind.RazorMetaCode);

    private static readonly ImmutableHashSet<SyntaxKind> s_commentSpanKinds = ImmutableHashSet.Create(
        SyntaxKind.RazorCommentTransition,
        SyntaxKind.RazorCommentStar,
        SyntaxKind.RazorCommentLiteral);

    private static readonly ImmutableHashSet<SyntaxKind> s_codeSpanKinds = ImmutableHashSet.Create(
        SyntaxKind.CSharpStatementLiteral,
        SyntaxKind.CSharpExpressionLiteral,
        SyntaxKind.CSharpEphemeralTextLiteral);

    private static readonly ImmutableHashSet<SyntaxKind> s_markupSpanKinds = ImmutableHashSet.Create(
        SyntaxKind.MarkupTextLiteral,
        SyntaxKind.MarkupEphemeralTextLiteral);

    private static readonly ImmutableHashSet<SyntaxKind> s_noneSpanKinds = ImmutableHashSet.Create(
        SyntaxKind.UnclassifiedTextLiteral);

    private static readonly ImmutableHashSet<SyntaxKind> s_allSpanKinds = CreateAllSpanKindsSet();

    private static ImmutableHashSet<SyntaxKind> CreateAllSpanKindsSet()
    {
        var set = ImmutableHashSet<SyntaxKind>.Empty.ToBuilder();

        set.UnionWith(s_transitionSpanKinds);
        set.UnionWith(s_metaCodeSpanKinds);
        set.UnionWith(s_commentSpanKinds);
        set.UnionWith(s_codeSpanKinds);
        set.UnionWith(s_markupSpanKinds);
        set.UnionWith(s_noneSpanKinds);

        return set.ToImmutable();
    }

    internal static ISpanChunkGenerator? GetChunkGenerator(this SyntaxNode node)
     => node switch
        {
            MarkupStartTagSyntax start => start.ChunkGenerator,
            MarkupEndTagSyntax end => end.ChunkGenerator,
            MarkupEphemeralTextLiteralSyntax ephemeral => ephemeral.ChunkGenerator,
            MarkupTagHelperStartTagSyntax start => start.ChunkGenerator,
            MarkupTagHelperEndTagSyntax end => end.ChunkGenerator,
            MarkupTextLiteralSyntax text => text.ChunkGenerator,
            MarkupTransitionSyntax transition => transition.ChunkGenerator,
            CSharpStatementLiteralSyntax csharp => csharp.ChunkGenerator,
            CSharpExpressionLiteralSyntax csharp => csharp.ChunkGenerator,
            CSharpEphemeralTextLiteralSyntax csharp => csharp.ChunkGenerator,
            CSharpTransitionSyntax transition => transition.ChunkGenerator,
            RazorMetaCodeSyntax meta => meta.ChunkGenerator,
            UnclassifiedTextLiteralSyntax unclassified => unclassified.ChunkGenerator,
            _ => null,
        };

    public static SpanEditHandler? GetEditHandler(this SyntaxNode node) => node.GetAnnotationValue(SyntaxConstants.EditHandlerKind) as SpanEditHandler;

    public static TNode WithEditHandler<TNode>(this TNode node, SpanEditHandler? editHandler) where TNode : SyntaxNode
    {
        if (node is null)
        {
            throw new ArgumentNullException(nameof(node));
        }

        if (editHandler is null)
        {
            if (node.ContainsAnnotations)
            {
                List<SyntaxAnnotation>? filteredAnnotations = null;
                foreach (var annotation in node.GetAnnotations())
                {
                    if (annotation.Kind != SyntaxConstants.EditHandlerKind)
                    {
                        (filteredAnnotations ??= new List<SyntaxAnnotation>()).Add(annotation);
                    }
                }

                return node.WithAnnotations(filteredAnnotations?.ToArray() ?? Array.Empty<SyntaxAnnotation>());
            }
            else
            {
                return node;
            }
        }

        var newAnnotation = new SyntaxAnnotation(SyntaxConstants.EditHandlerKind, editHandler);

        List<SyntaxAnnotation>? newAnnotations = null;
        if (node.ContainsAnnotations)
        {
            foreach (var annotation in node.GetAnnotations())
            {
                if (annotation.Kind != newAnnotation.Kind)
                {
                    newAnnotations ??= new List<SyntaxAnnotation>
                    {
                        newAnnotation
                    };

                    newAnnotations.Add(annotation);
                }
            }
        }

        var newAnnotationsArray = newAnnotations is null
            ? new[] { newAnnotation }
            : newAnnotations.ToArray();

        return node.WithAnnotations(newAnnotationsArray);
    }

    [Obsolete("Use FindToken or FindInnermostNode instead", error: false)]
    public static SyntaxNode? LocateOwner(this SyntaxNode node, SourceChange change)
    {
        if (node is null)
        {
            throw new ArgumentNullException(nameof(node));
        }

        if (change.Span.AbsoluteIndex < node.Position)
        {
            // Early escape for cases where changes overlap multiple spans
            // In those cases, the span will return false, and we don't want to search the whole tree
            // So if the current span starts after the change, we know we've searched as far as we need to
            return null;
        }

        if (node.EndPosition < change.Span.AbsoluteIndex)
        {
            // no need to look into this node as it completely precedes the change
            return null;
        }

        if (node.IsSpanKind())
        {
            var editHandler = node.GetEditHandler() ?? SpanEditHandler.CreateDefault(AcceptedCharactersInternal.Any);
            return editHandler.OwnsChange(node, change) ? node : null;
        }

        return node switch
        {
            MarkupStartTagSyntax startTag => LocateOwnerForSyntaxList(startTag.LegacyChildren, change),
            MarkupEndTagSyntax endTag => LocateOwnerForSyntaxList(endTag.LegacyChildren, change),
            MarkupTagHelperStartTagSyntax startTagHelper => LocateOwnerForSyntaxList(startTagHelper.LegacyChildren, change),
            MarkupTagHelperEndTagSyntax endTagHelper => LocateOwnerForSyntaxList(endTagHelper.LegacyChildren, change),
            _ => LocateOwnerForChildSyntaxList(node.ChildNodes(), change)
        };

        static SyntaxNode? LocateOwnerForSyntaxList(in SyntaxList<RazorSyntaxNode> list, SourceChange change)
        {
            foreach (var child in list)
            {
                if (child.LocateOwner(change) is { } owner)
                {
                    return owner;
                }
            }

            return null;
        }

        static SyntaxNode? LocateOwnerForChildSyntaxList(in ChildSyntaxList list, SourceChange change)
        {
            foreach (var child in list)
            {
                if (child.LocateOwner(change) is { } owner)
                {
                    return owner;
                }
            }

            return null;
        }
    }

    public static bool IsTransitionSpanKind(this SyntaxNode node)
    {
        if (node is null)
        {
            throw new ArgumentNullException(nameof(node));
        }

        return s_transitionSpanKinds.Contains(node.Kind);
    }

    public static bool IsMetaCodeSpanKind(this SyntaxNode node)
    {
        if (node is null)
        {
            throw new ArgumentNullException(nameof(node));
        }

        return s_metaCodeSpanKinds.Contains(node.Kind);
    }

    public static bool IsCommentSpanKind(this SyntaxNode node)
    {
        if (node is null)
        {
            throw new ArgumentNullException(nameof(node));
        }

        return s_commentSpanKinds.Contains(node.Kind);
    }

    public static bool IsCodeSpanKind(this SyntaxNode node)
    {
        if (node is null)
        {
            throw new ArgumentNullException(nameof(node));
        }

        return s_codeSpanKinds.Contains(node.Kind);
    }

    public static bool IsMarkupSpanKind(this SyntaxNode node)
    {
        if (node is null)
        {
            throw new ArgumentNullException(nameof(node));
        }

        return s_markupSpanKinds.Contains(node.Kind);
    }

    public static bool IsNoneSpanKind(this SyntaxNode node)
    {
        if (node is null)
        {
            throw new ArgumentNullException(nameof(node));
        }

        return s_noneSpanKinds.Contains(node.Kind);
    }

    public static bool IsSpanKind(this SyntaxNode node)
        => s_allSpanKinds.Contains(node.Kind);

    private static IEnumerable<SyntaxNode> FlattenSpansInReverse(this SyntaxNode node)
    {
        using var stack = new ChildSyntaxListReversedEnumeratorStack(node);

        while (stack.TryGetNextNode(out var nextNode))
        {
            if (nextNode is MarkupStartTagSyntax startTag)
            {
                var children = startTag.LegacyChildren;

                for (var i = children.Count - 1; i >= 0; i--)
                {
                    var tagChild = children[i];
                    if (tagChild.IsSpanKind())
                    {
                        yield return tagChild;
                    }
                }
            }
            else if (nextNode is MarkupEndTagSyntax endTag)
            {
                var children = endTag.LegacyChildren;

                for (var i = children.Count - 1; i >= 0; i--)
                {
                    var tagChild = children[i];
                    if (tagChild.IsSpanKind())
                    {
                        yield return tagChild;
                    }
                }
            }
            else if (nextNode.IsSpanKind())
            {
                yield return nextNode;
            }
        }
    }

    public static IEnumerable<SyntaxNode> FlattenSpans(this SyntaxNode node)
    {
        if (node is null)
        {
            throw new ArgumentNullException(nameof(node));
        }

        foreach (var child in node.DescendantNodes())
        {
            if (child is MarkupStartTagSyntax startTag)
            {
                foreach (var tagChild in startTag.LegacyChildren)
                {
                    if (tagChild.IsSpanKind())
                    {
                        yield return tagChild;
                    }
                }
            }
            else if (child is MarkupEndTagSyntax endTag)
            {
                foreach (var tagChild in endTag.LegacyChildren)
                {
                    if (tagChild.IsSpanKind())
                    {
                        yield return tagChild;
                    }
                }
            }
            else if (child.IsSpanKind())
            {
                yield return child;
            }
        }
    }

    public static SyntaxNode? PreviousSpan(this SyntaxNode node)
    {
        if (node is null)
        {
            throw new ArgumentNullException(nameof(node));
        }

        var spanData = s_spanDataTable.GetOrCreateValue(node);

        lock (spanData)
        {
            if (spanData.PreviousComputed)
            {
                return spanData.Previous;
            }

            var parent = node.Parent;
            while (parent is not null)
            {
                foreach (var span in parent.FlattenSpansInReverse())
                {
                    if (span.EndPosition <= node.Position && span != node)
                    {
                        spanData.PreviousComputed = true;
                        spanData.Previous = span;

                        return span;
                    }
                }

                parent = parent.Parent;
            }

            spanData.PreviousComputed = true;
            spanData.Previous = null;

            return null;
        }
    }

    public static SyntaxNode? NextSpan(this SyntaxNode node)
    {
        if (node is null)
        {
            throw new ArgumentNullException(nameof(node));
        }

        var spanData = s_spanDataTable.GetOrCreateValue(node);

        lock (spanData)
        {
            if (spanData.NextComputed)
            {
                return spanData.Next;
            }

            var parent = node.Parent;
            while (parent is not null)
            {
                foreach (var span in parent.FlattenSpans())
                {
                    if (span.Position >= node.Position && span != node)
                    {
                        spanData.NextComputed = true;
                        spanData.Next = span;

                        return span;
                    }
                }

                parent = parent.Parent;
            }

            spanData.NextComputed = true;
            spanData.Next = null;

            return null;
        }
    }
}
