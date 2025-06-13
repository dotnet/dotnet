// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Language.Intermediate;

public static class IntermediateNodeExtensions
{
    public static ImmutableArray<RazorDiagnostic> GetAllDiagnostics(this IntermediateNode node)
    {
        ArgHelper.ThrowIfNull(node);

        var diagnostics = new PooledHashSet<RazorDiagnostic>();
        try
        {
            CollectDiagnostics(node, ref diagnostics);

            return diagnostics.OrderByAsArray(static d => d.Span.AbsoluteIndex);
        }
        finally
        {
            diagnostics.ClearAndFree();
        }

        static void CollectDiagnostics(IntermediateNode node, ref PooledHashSet<RazorDiagnostic> diagnostics)
        {
            if (node.HasDiagnostics)
            {
                diagnostics.UnionWith(node.Diagnostics);
            }

            foreach (var childNode in node.Children)
            {
                CollectDiagnostics(childNode, ref diagnostics);
            }
        }
    }

    public static IReadOnlyList<TNode> FindDescendantNodes<TNode>(this IntermediateNode node)
        where TNode : IntermediateNode
    {
        var visitor = new Visitor<TNode>();
        visitor.Visit(node);

        if (visitor.Results.Count > 0 && visitor.Results[0] == node)
        {
            // Don't put the node itself in the results
            visitor.Results.Remove((TNode)node);
        }

        return visitor.Results;
    }

    private class Visitor<TNode> : IntermediateNodeWalker where TNode : IntermediateNode
    {
        public List<TNode> Results { get; } = new List<TNode>();

        public override void VisitDefault(IntermediateNode node)
        {
            if (node is TNode match)
            {
                Results.Add(match);
            }

            base.VisitDefault(node);
        }
    }
}
