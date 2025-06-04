// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Razor.Language.Syntax;

/// <summary>
/// Represents a read-only list of <see cref="SyntaxToken"/>.
/// </summary>
[CollectionBuilder(typeof(SyntaxTokenList), methodName: "Create")]
internal readonly partial struct SyntaxTokenList : IEquatable<SyntaxTokenList>, IReadOnlyList<SyntaxToken>
{
    public static SyntaxTokenList Empty => default;

    internal GreenNode? Node { get; }
    internal int Position { get; }

    private readonly SyntaxNode? _parent;
    private readonly int _index;

    internal SyntaxTokenList(SyntaxNode? parent, GreenNode? tokenOrList, int position, int index)
    {
        Debug.Assert(tokenOrList != null || (position == 0 && index == 0 && parent == null));
        Debug.Assert(position >= 0);
        Debug.Assert(tokenOrList == null || tokenOrList.IsToken || tokenOrList.IsList);

        _parent = parent;
        Node = tokenOrList;
        Position = position;
        _index = index;
    }

    public SyntaxTokenList(SyntaxToken token)
    {
        _parent = token.Parent;
        Node = token.Node;
        Position = token.Position;
        _index = 0;
    }

    public SyntaxTokenList(params ReadOnlySpan<SyntaxToken> tokens)
        : this(parent: null, CreateNodeFromSpan(tokens), position: 0, index: 0)
    {
    }

    public SyntaxTokenList(IEnumerable<SyntaxToken> tokens)
        : this(parent: null, CreateNode(tokens), position: 0, index: 0)
    {
    }

    public static SyntaxTokenList Create(ReadOnlySpan<SyntaxToken> tokens)
    {
        return tokens.Length == 0
            ? default
            : new(parent: null, CreateNodeFromSpan(tokens), position: 0, index: 0);
    }

    private static GreenNode? CreateNodeFromSpan(ReadOnlySpan<SyntaxToken> tokens)
    {
        return tokens.Length switch
        {
            0 => null,
            1 => tokens[0].Node,
            2 => InternalSyntax.SyntaxList.List(tokens[0].Node, tokens[1].Node),
            3 => InternalSyntax.SyntaxList.List(tokens[0].Node, tokens[1].Node, tokens[2].Node),
            _ => BuildAsArray(tokens)
        };

        static GreenNode BuildAsArray(ReadOnlySpan<SyntaxToken> tokens)
        {
            var copy = new ArrayElement<GreenNode>[tokens.Length];

            for (var i = 0; i < tokens.Length; i++)
            {
                copy[i].Value = tokens[i].RequiredNode;
            }

            return InternalSyntax.SyntaxList.List(copy);
        }
    }

    private static GreenNode? CreateNode(IEnumerable<SyntaxToken> tokens)
    {
        if (tokens == null)
        {
            return null;
        }

        using var builder = new PooledArrayBuilder<SyntaxToken>();
        builder.AddRange(tokens);

        return builder.ToList().Node;
    }
    /// <summary>
    /// The number of nodes in the list.
    /// </summary>
    public int Count
        => Node == null ? 0 : (Node.IsList ? Node.SlotCount : 1);

    /// <summary>
    /// Gets the node at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the node to get or set.</param>
    /// <returns>The node at the specified index.</returns>
    public SyntaxToken this[int index]
    {
        get
        {
            if (Node != null)
            {
                if (Node.IsList)
                {
                    if (unchecked((uint)index < (uint)Node.SlotCount))
                    {
                        return new SyntaxToken(_parent, Node.GetSlot(index), Position + Node.GetSlotOffset(index), _index + index);
                    }
                }
                else if (index == 0)
                {
                    return new SyntaxToken(_parent, Node, Position, _index);
                }
            }

            throw new ArgumentOutOfRangeException(nameof(index));
        }
    }

    public TextSpan Span
       => Node == null ? default : TextSpan.FromBounds(Position, Position + Node.Width);

    public override string ToString()
        => Node != null ? Node.ToString() : string.Empty;

    public bool Any() => Node != null;
    public SyntaxToken First() => Any() ? this[0] : throw new InvalidOperationException();
    public SyntaxToken Last() => Any() ? this[^1] : throw new InvalidOperationException();

    private static GreenNode? GetGreenNodeAt(GreenNode node, int index)
    {
        Debug.Assert(node.IsList || index == 0);

        return node.IsList ? node.GetSlot(index) : node;
    }

    public int IndexOf(SyntaxToken tokenInList)
    {
        for (int i = 0, count = Count; i < count; i++)
        {
            if (this[i] == tokenInList)
            {
                return i;
            }
        }

        return -1;
    }

    internal int IndexOf(SyntaxKind kind)
    {
        for (int i = 0, count = Count; i < count; i++)
        {
            if (this[i].Kind == kind)
            {
                return i;
            }
        }

        return -1;
    }

    public SyntaxTokenList Add(SyntaxToken token)
        => Insert(Count, token);

    public SyntaxTokenList AddRange(ReadOnlySpan<SyntaxToken> tokens)
        => InsertRange(Count, tokens);

    public SyntaxTokenList AddRange(IEnumerable<SyntaxToken> tokens)
        => InsertRange(Count, tokens);

    public SyntaxTokenList Insert(int index, SyntaxToken token)
    {
        if (token == default)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(token));
        }

        return InsertRange(index, [token]);
    }

    public SyntaxTokenList InsertRange(int index, ReadOnlySpan<SyntaxToken> tokens)
    {
        var count = Count;

        ArgHelper.ThrowIfNegative(index);
        ArgHelper.ThrowIfGreaterThan(index, count);

        if (tokens.Length == 0)
        {
            return this;
        }

        var array = new ArrayElement<GreenNode>[count + tokens.Length];

        // Add current tokens up to 'index'
        int i;
        for (i = 0; i < index; i++)
        {
            array[i].Value = this[i].RequiredNode;
        }

        // Add new tokens
        for (var j = 0; j < tokens.Length; i++, j++)
        {
            array[i].Value = tokens[j].RequiredNode;
        }

        // Add remaining tokens starting from 'index'
        for (var j = index; j < count; i++, j++)
        {
            array[i].Value = this[j].RequiredNode;
        }

        Debug.Assert(i == array.Length);

        return new(parent: null, InternalSyntax.SyntaxList.List(array), position: 0, index: 0);
    }

    public SyntaxTokenList InsertRange(int index, IEnumerable<SyntaxToken> tokens)
    {
        var count = Count;

        ArgHelper.ThrowIfNegative(index);
        ArgHelper.ThrowIfGreaterThan(index, count);
        ArgHelper.ThrowIfNull(tokens);

        if (tokens.TryGetCount(out var tokenCount))
        {
            return InsertRangeWithCount(index, tokens, tokenCount);
        }

        using var builder = new PooledArrayBuilder<SyntaxToken>(count);

        // Add current tokens up to 'index'
        for (var i = 0; i < index; i++)
        {
            builder.Add(this[i]);
        }

        var oldCount = builder.Count;

        // Add new tokens
        foreach (var token in tokens)
        {
            builder.Add(token);
        }

        // If builder.Count == oldCount, there weren't any tokens added.
        // So, there's no need to continue.
        if (builder.Count == oldCount)
        {
            return this;
        }

        // Add remaining tokens starting from 'index'
        for (var i = index; i < count; i++)
        {
            builder.Add(this[i]);
        }

        return new(parent: null, builder.ToGreenListNode(), position: 0, index: 0);
    }

    private SyntaxTokenList InsertRangeWithCount(int index, IEnumerable<SyntaxToken> tokens, int tokenCount)
    {
        if (tokenCount == 0)
        {
            return this;
        }

        var count = Count;
        var array = new ArrayElement<GreenNode>[count + tokenCount];

        // Add current tokens up to 'index'
        int i;
        for (i = 0; i < index; i++)
        {
            array[i].Value = this[i].RequiredNode;
        }

        // Add new tokens
        foreach (var token in tokens)
        {
            array[i++].Value = token.RequiredNode;
        }

        Debug.Assert(i == index + tokenCount);

        // Add remaining tokens starting from 'index'
        for (var j = index; j < count; i++, j++)
        {
            array[i].Value = this[j].RequiredNode;
        }

        Debug.Assert(i == array.Length);

        return new(parent: null, InternalSyntax.SyntaxList.List(array), position: 0, index: 0);
    }

    public SyntaxTokenList RemoveAt(int index)
    {
        var count = Count;

        ArgHelper.ThrowIfNegative(index);
        ArgHelper.ThrowIfGreaterThanOrEqual(index, count);

        // count - 1 because we're removing an item.
        var array = new ArrayElement<GreenNode>[count - 1];

        // Add current tokens up to 'index'
        int i;
        for (i = 0; i < index; i++)
        {
            array[i].Value = this[i].RequiredNode;
        }

        // Add remaining tokens starting *after* 'index'
        for (var j = index + 1; j < count; i++, j++)
        {
            array[i].Value = this[j].RequiredNode;
        }

        return new(parent: null, InternalSyntax.SyntaxList.List(array), position: 0, index: 0);
    }

    public SyntaxTokenList Remove(SyntaxToken tokenInList)
    {
        var index = IndexOf(tokenInList);
        return index >= 0 ? RemoveAt(index) : this;
    }

    public SyntaxTokenList Replace(SyntaxToken tokenInList, SyntaxToken newToken)
    {
        if (newToken == default)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(newToken));
        }

        return ReplaceRange(tokenInList, [newToken]);
    }

    public SyntaxTokenList ReplaceRange(SyntaxToken tokenInList, ReadOnlySpan<SyntaxToken> tokens)
    {
        var index = IndexOf(tokenInList);

        if (index < 0)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(tokenInList));
        }

        if (tokens.Length == 0)
        {
            return RemoveAt(index);
        }

        var count = Count;

        // The length of the new array is -1 because an element will be replaced.
        var array = new ArrayElement<GreenNode>[count + tokens.Length - 1];

        // Add current tokens up to 'index'
        int i;
        for (i = 0; i < index; i++)
        {
            array[i].Value = this[i].RequiredNode;
        }

        // Add new tokens
        for (var j = 0; j < tokens.Length; i++, j++)
        {
            array[i].Value = tokens[j].RequiredNode;
        }

        // Add remaining tokens starting *after* 'index'
        for (var j = index + 1; j < count; i++, j++)
        {
            array[i].Value = this[j].RequiredNode;
        }

        Debug.Assert(i == array.Length);

        return new(parent: null, InternalSyntax.SyntaxList.List(array), position: 0, index: 0);
    }

    public SyntaxTokenList ReplaceRange(SyntaxToken tokenInList, IEnumerable<SyntaxToken> tokens)
    {
        var index = IndexOf(tokenInList);

        if (index < 0)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(tokenInList));
        }

        ArgHelper.ThrowIfNull(tokens);

        if (tokens.TryGetCount(out var tokenCount))
        {
            return ReplaceRangeWithCount(index, tokens, tokenCount);
        }

        var count = Count;
        using var builder = new PooledArrayBuilder<SyntaxToken>(count);

        // Add current tokens up to 'index'
        for (var i = 0; i < index; i++)
        {
            builder.Add(this[i]);
        }

        // Add new tokens
        foreach (var token in tokens)
        {
            builder.Add(token);
        }

        // Add remaining tokens starting *after* 'index'
        for (var i = index + 1; i < count; i++)
        {
            builder.Add(this[i]);
        }

        return new(parent: null, builder.ToGreenListNode(), position: 0, index: 0);
    }

    private SyntaxTokenList ReplaceRangeWithCount(int index, IEnumerable<SyntaxToken> tokens, int tokenCount)
    {
        if (tokenCount == 0)
        {
            return RemoveAt(index);
        }

        var count = Count;

        // The length of the new array is -1 because an element will be replaced.
        var array = new ArrayElement<GreenNode>[count + tokenCount - 1];

        // Add current tokens up to 'index'
        int i;
        for (i = 0; i < index; i++)
        {
            array[i].Value = this[i].RequiredNode;
        }

        // Add new tokens
        foreach (var token in tokens)
        {
            array[i++].Value = token.RequiredNode;
        }

        Debug.Assert(i == index + tokenCount);

        // Add remaining tokens starting *after* 'index'
        for (var j = index + 1; j < count; i++, j++)
        {
            array[i].Value = this[j].RequiredNode;
        }

        Debug.Assert(i == array.Length);

        return new(parent: null, InternalSyntax.SyntaxList.List(array), position: 0, index: 0);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is SyntaxTokenList list && Equals(list);

    public bool Equals(SyntaxTokenList other)
        => Node == other.Node &&
           _parent == other._parent &&
           _index == other._index;

    public override int GetHashCode()
    {
        // Not call GHC on parent as it's expensive
        var hash = HashCodeCombiner.Start();
        hash.Add(Node);
        hash.Add(_index);

        return hash.CombinedHash;
    }

    public static bool operator ==(SyntaxTokenList left, SyntaxTokenList right)
        => left.Equals(right);

    public static bool operator !=(SyntaxTokenList left, SyntaxTokenList right)
        => !left.Equals(right);

    public Enumerator GetEnumerator()
        => new(in this);

    IEnumerator<SyntaxToken> IEnumerable<SyntaxToken>.GetEnumerator()
        => Node == null
            ? SpecializedCollections.EmptyEnumerator<SyntaxToken>()
            : new EnumeratorImpl(in this);

    IEnumerator IEnumerable.GetEnumerator()
        => Node == null
            ? SpecializedCollections.EmptyEnumerator<SyntaxToken>()
            : (IEnumerator)new EnumeratorImpl(in this);

    public Reversed Reverse()
        => new(this);
}
