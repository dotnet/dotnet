// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Language.Syntax;

internal abstract partial class SyntaxRewriter : SyntaxVisitor<SyntaxNode>
{
    private int _recursionDepth;

    public override SyntaxNode Visit(SyntaxNode node)
    {
        if (node != null)
        {
            _recursionDepth++;
            StackGuard.EnsureSufficientExecutionStack(_recursionDepth);

            var result = node.Accept(this);

            _recursionDepth--;
            return result;
        }
        else
        {
            return null;
        }
    }

    public virtual SyntaxList<TNode> VisitList<TNode>(SyntaxList<TNode> list)
        where TNode : SyntaxNode
    {
        var count = list.Count;
        if (count == 0)
        {
            return list;
        }

        using PooledArrayBuilder<TNode> builder = [];

        var isUpdating = false;

        for (var i = 0; i < count; i++)
        {
            var item = list[i];

            var visited = VisitListElement(item);

            if (item != visited && !isUpdating)
            {
                // The list is being updated, so we need to initialize the builder
                // add the items we've seen so far.
                builder.SetCapacityIfLarger(count);

                builder.AddRange(list, index: 0, count: i);

                isUpdating = true;
            }

            if (isUpdating && visited != null)
            {
                builder.Add(visited);
            }
        }

        return isUpdating
            ? builder.ToList()
            : list;
    }

    public override SyntaxNode VisitToken(SyntaxToken token)
    {
        return token;
    }

    public virtual TNode VisitListElement<TNode>(TNode node) where TNode : SyntaxNode
    {
        return (TNode)(SyntaxNode)Visit(node);
    }
}
