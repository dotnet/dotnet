// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Collections.Immutable;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Language.Syntax;

internal static class GreenNodeExtensions
{
    internal static InternalSyntax.SyntaxList<T> ToGreenList<T>(this SyntaxNode node) where T : GreenNode
    {
        return node != null ?
            ToGreenList<T>(node.Green) :
            default(InternalSyntax.SyntaxList<T>);
    }

    internal static InternalSyntax.SyntaxList<T> ToGreenList<T>(this GreenNode node) where T : GreenNode
    {
        return new InternalSyntax.SyntaxList<T>(node);
    }

    public static TNode WithAnnotationsGreen<TNode>(this TNode node, params SyntaxAnnotation[] annotations) where TNode : GreenNode
    {
        if (annotations.Length == 0)
        {
            var existingAnnotations = node.GetAnnotations();
            if (existingAnnotations != null && existingAnnotations.Length > 0)
            {
                node = (TNode)node.SetAnnotations(null);
            }

            return node;
        }

        using var newAnnotations = new PooledArrayBuilder<SyntaxAnnotation>(annotations.Length);
        foreach (var candidate in annotations)
        {
            if (!newAnnotations.Contains(candidate))
            {
                newAnnotations.Add(candidate);
            }
        }

        return (TNode)node.SetAnnotations(newAnnotations.ToArray());
    }

    public static TNode WithDiagnosticsGreen<TNode>(this TNode node, RazorDiagnostic[] diagnostics)
        where TNode : GreenNode
    {
        return (TNode)node.SetDiagnostics(diagnostics);
    }

    public static TNode WithDiagnosticsGreen<TNode>(this TNode node, params ImmutableArray<RazorDiagnostic> diagnostics)
        where TNode : GreenNode
    {
        var array = ImmutableCollectionsMarshal.AsArray(diagnostics);
        return node.WithDiagnosticsGreen(array);
    }
}
