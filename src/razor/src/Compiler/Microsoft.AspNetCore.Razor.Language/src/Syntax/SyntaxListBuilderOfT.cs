// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.AspNetCore.Razor.Language.Syntax;

internal readonly struct SyntaxListBuilder<TNode>
    where TNode : SyntaxNode
{
    internal readonly SyntaxListBuilder Builder;

    internal SyntaxListBuilder(SyntaxListBuilder builder)
    {
        Builder = builder;
    }

    public bool IsNull => Builder == null;

    public int Capacity => Builder.Capacity;

    public void SetCapacityIfLarger(int newCapacity)
        => Builder.SetCapacityIfLarger(newCapacity);

    public int Count => Builder.Count;

    public void Clear()
    {
        Builder.Clear();
    }

    internal void ClearInternal()
    {
        Builder.ClearInternal();
    }

    public SyntaxListBuilder<TNode> Add(TNode node)
    {
        Builder.Add(node);
        return this;
    }

    public void AddRange(TNode[] items, int offset, int length)
    {
        Builder.AddRange(items, offset, length);
    }

    public void AddRange(SyntaxList<TNode> nodes)
    {
        Builder.AddRange(nodes);
    }

    public void AddRange(SyntaxList<TNode> nodes, int offset, int length)
    {
        Builder.AddRange(nodes, offset, length);
    }

    public bool Any(SyntaxKind kind)
    {
        return Builder.Any(kind);
    }

    public SyntaxList<TNode> ToList()
    {
        return Builder.ToList();
    }

    public SyntaxList<TNode> ToList(SyntaxNode parent)
    {
        return Builder.ToList(parent);
    }

    public SyntaxList<TNode> Consume()
    {
        var list = ToList();
        Clear();
        return list;
    }

    public static implicit operator SyntaxListBuilder(SyntaxListBuilder<TNode> builder)
    {
        return builder.Builder;
    }

    public static implicit operator SyntaxList<TNode>(SyntaxListBuilder<TNode> builder)
    {
        if (builder.Builder != null)
        {
            return builder.ToList();
        }

        return default;
    }
}
