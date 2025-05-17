// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Language.Syntax;

[DebuggerDisplay("{GetDebuggerDisplay(), nq}")]
internal abstract class GreenNode
{
    private static readonly RazorDiagnostic[] EmptyDiagnostics = Array.Empty<RazorDiagnostic>();
    private static readonly SyntaxAnnotation[] EmptyAnnotations = Array.Empty<SyntaxAnnotation>();
    private static readonly ConditionalWeakTable<GreenNode, RazorDiagnostic[]> DiagnosticsTable =
        new ConditionalWeakTable<GreenNode, RazorDiagnostic[]>();
    private static readonly ConditionalWeakTable<GreenNode, SyntaxAnnotation[]> AnnotationsTable =
        new ConditionalWeakTable<GreenNode, SyntaxAnnotation[]>();

    private int _width;
    private byte _slotCount;

    protected GreenNode(SyntaxKind kind)
    {
        Kind = kind;
    }

    protected GreenNode(SyntaxKind kind, int width)
        : this(kind)
    {
        _width = width;
    }

    protected GreenNode(SyntaxKind kind, RazorDiagnostic[] diagnostics, SyntaxAnnotation[] annotations)
        : this(kind, 0, diagnostics, annotations)
    {
    }

    protected GreenNode(SyntaxKind kind, int width, RazorDiagnostic[] diagnostics, SyntaxAnnotation[] annotations)
        : this(kind, width)
    {
        if (diagnostics?.Length > 0)
        {
            Flags |= NodeFlags.ContainsDiagnostics;
            DiagnosticsTable.Add(this, diagnostics);
        }

        if (annotations?.Length > 0)
        {
            foreach (var annotation in annotations)
            {
                if (annotation == null)
                {
                    throw new ArgumentException("Annotation cannot be null", nameof(annotations));
                }
            }

            Flags |= NodeFlags.ContainsAnnotations;
            AnnotationsTable.Add(this, annotations);
        }
    }

    protected void AdjustFlagsAndWidth(GreenNode node)
    {
        if (node == null)
        {
            return;
        }

        Flags |= (node.Flags & NodeFlags.InheritMask);
        _width += node.Width;
    }

    #region Kind
    internal SyntaxKind Kind { get; }

    internal virtual bool IsList => false;

    internal virtual bool IsToken => false;
    #endregion

    public int Width => _width;

    #region Slots
    public int SlotCount
    {
        get
        {
            int count = _slotCount;
            if (count == byte.MaxValue)
            {
                count = GetSlotCount();
            }

            return count;
        }

        protected set
        {
            _slotCount = (byte)value;
        }
    }

    internal abstract GreenNode GetSlot(int index);

    // for slot counts >= byte.MaxValue
    protected virtual int GetSlotCount()
    {
        return _slotCount;
    }

    public virtual int GetSlotOffset(int index)
    {
        var offset = 0;
        for (var i = 0; i < index; i++)
        {
            var child = GetSlot(i);
            if (child != null)
            {
                offset += child.Width;
            }
        }

        return offset;
    }

    public virtual int FindSlotIndexContainingOffset(int offset)
    {
        Debug.Assert(0 <= offset && offset < Width);

        int i;
        var accumulatedWidth = 0;
        for (i = 0; ; i++)
        {
            Debug.Assert(i < SlotCount);
            var child = GetSlot(i);
            if (child != null)
            {
                accumulatedWidth += child.Width;
                if (offset < accumulatedWidth)
                {
                    break;
                }
            }
        }

        return i;
    }
    #endregion

    #region Flags
    public NodeFlags Flags { get; protected set; }

    internal void SetFlags(NodeFlags flags)
    {
        Flags |= flags;
    }

    internal void ClearFlags(NodeFlags flags)
    {
        Flags &= ~flags;
    }

    internal virtual bool IsMissing => (Flags & NodeFlags.IsMissing) != 0;

    public bool ContainsDiagnostics
    {
        get
        {
            return (Flags & NodeFlags.ContainsDiagnostics) != 0;
        }
    }

    public bool ContainsAnnotations
    {
        get
        {
            return (Flags & NodeFlags.ContainsAnnotations) != 0;
        }
    }
    #endregion

    #region Diagnostics
    internal abstract GreenNode SetDiagnostics(RazorDiagnostic[] diagnostics);

    internal RazorDiagnostic[] GetDiagnostics()
    {
        if (ContainsDiagnostics)
        {
            if (DiagnosticsTable.TryGetValue(this, out var diagnostics))
            {
                return diagnostics;
            }
        }

        return EmptyDiagnostics;
    }
    #endregion

    #region Annotations
    internal abstract GreenNode SetAnnotations(SyntaxAnnotation[] annotations);

    internal SyntaxAnnotation[] GetAnnotations()
    {
        if (ContainsAnnotations)
        {
            if (AnnotationsTable.TryGetValue(this, out var annotations))
            {
                Debug.Assert(annotations.Length != 0, "There cannot be an empty annotation entry.");
                return annotations;
            }
        }

        return EmptyAnnotations;
    }
    #endregion

    #region Text
    private string GetDebuggerDisplay()
    {
        using var _ = StringBuilderPool.GetPooledObject(out var builder);
        builder.Append(GetType().Name);
        builder.Append('<');
        builder.Append(Kind.ToString());
        builder.Append('>');

        return builder.ToString();
    }

    public override string ToString()
    {
        using var _ = StringBuilderPool.GetPooledObject(out var builder);
        using var writer = new StringWriter(builder, CultureInfo.InvariantCulture);
        WriteTo(writer);
        return builder.ToString();
    }

    public void WriteTo(TextWriter writer)
    {
        // Use an actual Stack so we can write out deeply recursive structures without overflowing.
        using var stack = new PooledArrayBuilder<GreenNode>();

        stack.Push(this);

        while (stack.Count > 0)
        {
            var node = stack.Pop();

            if (node.IsToken)
            {
                node.WriteTokenTo(writer);
                continue;
            }

            var slotCount = node.SlotCount;

            for (var i = slotCount - 1; i >= 0; i--)
            {
                if (node.GetSlot(i) is GreenNode child)
                {
                    stack.Push(child);
                }
            }
        }
    }

    protected virtual void WriteTokenTo(TextWriter writer)
    {
        throw new NotImplementedException();
    }
    #endregion

    #region Equivalence
    public virtual bool IsEquivalentTo(GreenNode other)
    {
        if (this == other)
        {
            return true;
        }

        if (other == null)
        {
            return false;
        }

        return EquivalentToInternal(this, other);
    }

    private static bool EquivalentToInternal(GreenNode node1, GreenNode node2)
    {
        if (node1.Kind != node2.Kind)
        {
            // A single-element list is usually represented as just a single node,
            // but can be represented as a List node with one child. Move to that
            // child if necessary.
            if (node1.IsList && node1.SlotCount == 1)
            {
                node1 = node1.GetSlot(0);
            }

            if (node2.IsList && node2.SlotCount == 1)
            {
                node2 = node2.GetSlot(0);
            }

            if (node1.Kind != node2.Kind)
            {
                return false;
            }
        }

        if (node1.Width != node2.Width)
        {
            return false;
        }

        var n = node1.SlotCount;
        if (n != node2.SlotCount)
        {
            return false;
        }

        for (var i = 0; i < n; i++)
        {
            var node1Child = node1.GetSlot(i);
            var node2Child = node2.GetSlot(i);
            if (node1Child != null && node2Child != null && !node1Child.IsEquivalentTo(node2Child))
            {
                return false;
            }
        }

        return true;
    }
    #endregion

    #region Factories
    public virtual GreenNode CreateList(IEnumerable<GreenNode> nodes, bool alwaysCreateListNode = false)
    {
        if (nodes == null)
        {
            return null;
        }

        var list = nodes.ToArray();

        switch (list.Length)
        {
            case 0:
                return null;
            case 1:
                if (alwaysCreateListNode)
                {
                    goto default;
                }
                else
                {
                    return list[0];
                }
            case 2:
                return InternalSyntax.SyntaxList.List(list[0], list[1]);
            case 3:
                return InternalSyntax.SyntaxList.List(list[0], list[1], list[2]);
            default:
                return InternalSyntax.SyntaxList.List(list);
        }
    }

    public SyntaxNode CreateRed()
    {
        return CreateRed(null, 0);
    }

    internal abstract SyntaxNode CreateRed(SyntaxNode parent, int position);
    #endregion

    public abstract TResult Accept<TResult>(InternalSyntax.SyntaxVisitor<TResult> visitor);

    public abstract void Accept(InternalSyntax.SyntaxVisitor visitor);
}
