// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Language.Syntax;

internal class SyntaxListBuilder(int initialCapacity)
{
    private ArrayElement<GreenNode>[] _nodes = new ArrayElement<GreenNode>[initialCapacity];

    public int Capacity => _nodes.Length;

    public void SetCapacityIfLarger(int newCapacity)
    {
        if (newCapacity > _nodes.Length)
        {
            Array.Resize(ref _nodes, newCapacity);
        }
    }

    public int Count { get; private set; }

    public void Clear()
    {
        Array.Clear(_nodes, 0, Count);
        Count = 0;
    }

    internal void ClearInternal()
    {
        if (_nodes.Length > DefaultPool.MaximumObjectSize)
        {
            Array.Resize(ref _nodes, DefaultPool.MaximumObjectSize);
            Count = DefaultPool.MaximumObjectSize;
        }

        Clear();
    }

    public void Add(SyntaxNode item)
    {
        AddInternal(item.Green);
    }

    internal void AddInternal(GreenNode item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        if (Count >= _nodes.Length)
        {
            Grow(Count == 0 ? 8 : _nodes.Length * 2);
        }

        _nodes[Count++].Value = item;
    }

    public void AddRange(SyntaxNode[] items)
    {
        AddRange(items, 0, items.Length);
    }

    public void AddRange(SyntaxNode[] items, int offset, int length)
    {
        if (Count + length > _nodes.Length)
        {
            Grow(Count + length);
        }

        for (int i = offset, j = Count; i < offset + length; ++i, ++j)
        {
            _nodes[j].Value = items[i].Green;
        }

        var start = Count;
        Count += length;
        Validate(start, Count);
    }

    [Conditional("DEBUG")]
    private void Validate(int start, int end)
    {
        for (var i = start; i < end; i++)
        {
            if (_nodes[i].Value == null)
            {
                throw new ArgumentException("Cannot add a null node.");
            }
        }
    }

    public void AddRange(SyntaxList<SyntaxNode> list)
    {
        AddRange(list, 0, list.Count);
    }

    public void AddRange(SyntaxList<SyntaxNode> list, int offset, int count)
    {
        if (Count + count > _nodes.Length)
        {
            Grow(Count + count);
        }

        var dst = Count;
        for (int i = offset, limit = offset + count; i < limit; i++)
        {
            _nodes[dst].Value = list.ItemInternal(i)!.Green;
            dst++;
        }

        var start = Count;
        Count += count;
        Validate(start, Count);
    }

    public void AddRange<TNode>(SyntaxList<TNode> list) where TNode : SyntaxNode
    {
        AddRange(list, 0, list.Count);
    }

    public void AddRange<TNode>(SyntaxList<TNode> list, int offset, int count) where TNode : SyntaxNode
    {
        AddRange(new SyntaxList<SyntaxNode>(list.Node), offset, count);
    }

    private void Grow(int newSize)
    {
        Array.Resize(ref _nodes, newSize);
    }

    public bool Any(SyntaxKind kind)
    {
        for (var i = 0; i < Count; i++)
        {
            if (_nodes[i].Value.Kind == kind)
            {
                return true;
            }
        }

        return false;
    }

    internal GreenNode? ToListNode()
    {
        switch (Count)
        {
            case 0:
                return null;
            case 1:
                return _nodes[0].Value;
            case 2:
                return InternalSyntax.SyntaxList.List(_nodes[0].Value, _nodes[1].Value);
            case 3:
                return InternalSyntax.SyntaxList.List(_nodes[0].Value, _nodes[1].Value, _nodes[2].Value);
            default:
                var tmp = new ArrayElement<GreenNode>[Count];
                for (var i = 0; i < Count; i++)
                {
                    tmp[i].Value = _nodes[i].Value;
                }

                return InternalSyntax.SyntaxList.List(tmp);
        }
    }

    public static implicit operator SyntaxList<SyntaxNode>(SyntaxListBuilder builder)
    {
        if (builder == null)
        {
            return default;
        }

        return builder.ToList();
    }

    internal void RemoveLast()
    {
        Count -= 1;
        _nodes[Count] = default;
    }
}
