// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

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

    public virtual SyntaxList<TNode> VisitList<TNode>(SyntaxList<TNode> list) where TNode : SyntaxNode
    {
        SyntaxListBuilder alternate = null;
        for (int i = 0, n = list.Count; i < n; i++)
        {
            var item = list[i];
            var visited = VisitListElement(item);
            if (item != visited && alternate == null)
            {
                alternate = new SyntaxListBuilder(n);
                alternate.AddRange(list, 0, i);
            }

            if (alternate != null && visited != null)
            {
                alternate.Add(visited);
            }
        }

        if (alternate != null)
        {
            return alternate.ToList();
        }

        return list;
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
