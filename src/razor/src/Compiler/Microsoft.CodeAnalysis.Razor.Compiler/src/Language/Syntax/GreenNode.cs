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

internal abstract class GreenNode
{
    private static readonly RazorDiagnostic[] EmptyDiagnostics = Array.Empty<RazorDiagnostic>();
    private static readonly SyntaxAnnotation[] EmptyAnnotations = Array.Empty<SyntaxAnnotation>();
    private static readonly ConditionalWeakTable<GreenNode, RazorDiagnostic[]> DiagnosticsTable =
        new ConditionalWeakTable<GreenNode, RazorDiagnostic[]>();
    private static readonly ConditionalWeakTable<GreenNode, SyntaxAnnotation[]> AnnotationsTable =
        new ConditionalWeakTable<GreenNode, SyntaxAnnotation[]>();
    private byte _slotCount;

    protected GreenNode(SyntaxKind kind)
    {
        Kind = kind;
    }

    protected GreenNode(SyntaxKind kind, int fullWidth)
        : this(kind)
    {
        FullWidth = fullWidth;
    }

    protected GreenNode(SyntaxKind kind, RazorDiagnostic[] diagnostics, SyntaxAnnotation[] annotations)
        : this(kind, 0, diagnostics, annotations)
    {
    }

    protected GreenNode(SyntaxKind kind, int fullWidth, RazorDiagnostic[] diagnostics, SyntaxAnnotation[] annotations)
        : this(kind, fullWidth)
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
        FullWidth += node.FullWidth;
    }

    #region Kind
    internal SyntaxKind Kind { get; }

    internal virtual bool IsList => false;

    internal virtual bool IsToken => false;

    internal virtual bool IsTrivia => false;
    #endregion

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
                offset += child.FullWidth;
            }
        }

        return offset;
    }

    public virtual int FindSlotIndexContainingOffset(int offset)
    {
        Debug.Assert(0 <= offset && offset < FullWidth);

        int i;
        var accumulatedWidth = 0;
        for (i = 0; ; i++)
        {
            Debug.Assert(i < SlotCount);
            var child = GetSlot(i);
            if (child != null)
            {
                accumulatedWidth += child.FullWidth;
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

    #region Spans
    internal int FullWidth { get; private set; }

    public virtual int Width
    {
        get
        {
            return FullWidth - GetLeadingTriviaWidth() - GetTrailingTriviaWidth();
        }
    }

    public virtual int GetLeadingTriviaWidth()
    {
        return FullWidth != 0 ? GetFirstTerminal().GetLeadingTriviaWidth() : 0;
    }

    public virtual int GetTrailingTriviaWidth()
    {
        return FullWidth != 0 ? GetLastTerminal().GetTrailingTriviaWidth() : 0;
    }

    public bool HasLeadingTrivia
    {
        get
        {
            return GetLeadingTriviaWidth() != 0;
        }
    }

    public bool HasTrailingTrivia
    {
        get
        {
            return GetTrailingTriviaWidth() != 0;
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
    public override string ToString()
    {
        using var _ = StringBuilderPool.GetPooledObject(out var builder);
        builder.Append(GetType().Name);
        builder.Append('<');
        builder.Append(Kind.ToString()); // Use ToString() to avoid boxing the enum.
        builder.Append('>');

        return builder.ToString();
    }

    public virtual string ToFullString()
    {
        using var _ = StringBuilderPool.GetPooledObject(out var builder);
        using var writer = new StringWriter(builder, CultureInfo.InvariantCulture);
        WriteTo(writer);
        return builder.ToString();
    }

    public virtual void WriteTo(TextWriter writer)
    {
        WriteTo(writer, includeLeadingTrivia: true, includeTrailingTrivia: true);
    }

    protected internal void WriteTo(TextWriter writer, bool includeLeadingTrivia, bool includeTrailingTrivia)
    {
        // Use an actual Stack so we can write out deeply recursive structures without overflowing.
        using var stack = new PooledArrayBuilder<StackEntry>();

        stack.Push(new(Node: this, includeLeadingTrivia, includeTrailingTrivia));

        // Separated out stack processing logic so that it does not unintentionally refer to
        // "this", "leading" or "trailing.
        ProcessStack(writer, ref stack.AsRef());

        static void ProcessStack(TextWriter writer, ref PooledArrayBuilder<StackEntry> stack)
        {
            while (stack.Count > 0)
            {
                var (node, includeLeadingTrivia, includeTrailingTrivia) = stack.Pop();

                if (node.IsToken)
                {
                    node.WriteTokenTo(writer, includeLeadingTrivia, includeTrailingTrivia);
                    continue;
                }

                if (node.IsTrivia)
                {
                    node.WriteTriviaTo(writer);
                    continue;
                }

                var firstIndex = GetFirstNonNullChildIndex(node);
                var lastIndex = GetLastNonNullChildIndex(node);

                for (var i = lastIndex; i >= firstIndex; i--)
                {
                    if (node.GetSlot(i) is GreenNode child)
                    {
                        var isFirst = i == firstIndex;
                        var isLast = i == lastIndex;

                        stack.Push(new(
                            child,
                            IncludeLeadingTrivia: includeLeadingTrivia | !isFirst,
                            IncludeTrailingTrivia: includeTrailingTrivia | !isLast));
                    }
                }
            }

            static int GetFirstNonNullChildIndex(GreenNode node)
            {
                var slotCount = node.SlotCount;
                var firstIndex = 0;

                for (; firstIndex < slotCount; firstIndex++)
                {
                    if (node.GetSlot(firstIndex) is not null)
                    {
                        break;
                    }
                }

                return firstIndex;
            }

            static int GetLastNonNullChildIndex(GreenNode node)
            {
                var slotCount = node.SlotCount;
                var lastIndex = slotCount - 1;

                for (; lastIndex >= 0; lastIndex--)
                {
                    if (node.GetSlot(lastIndex) is not null)
                    {
                        break;
                    }
                }

                return lastIndex;
            }
        }
    }

    private readonly record struct StackEntry(
        GreenNode Node,
        bool IncludeLeadingTrivia,
        bool IncludeTrailingTrivia);

    protected virtual void WriteTriviaTo(TextWriter writer)
    {
        throw new NotImplementedException();
    }

    protected virtual void WriteTokenTo(TextWriter writer, bool includeLeadingTrivia, bool includeTrailingTrivia)
    {
        throw new NotImplementedException();
    }
    #endregion

    #region Tokens

    public virtual object GetValue()
    {
        return null;
    }

    public virtual string GetValueText()
    {
        return string.Empty;
    }

    public virtual GreenNode GetLeadingTrivia()
    {
        return null;
    }

    public virtual GreenNode GetTrailingTrivia()
    {
        return null;
    }

    public virtual GreenNode WithLeadingTrivia(GreenNode trivia)
    {
        return this;
    }

    public virtual GreenNode WithTrailingTrivia(GreenNode trivia)
    {
        return this;
    }

    public InternalSyntax.SyntaxToken GetFirstToken()
    {
        return (InternalSyntax.SyntaxToken)GetFirstTerminal();
    }

    public InternalSyntax.SyntaxToken GetLastToken()
    {
        return (InternalSyntax.SyntaxToken)GetLastTerminal();
    }

    internal GreenNode GetFirstTerminal()
    {
        var node = this;

        do
        {
            GreenNode firstChild = null;
            for (int i = 0, n = node.SlotCount; i < n; i++)
            {
                var child = node.GetSlot(i);
                if (child != null && child.FullWidth > 0)
                {
                    firstChild = child;
                    break;
                }
            }
            node = firstChild;
        } while (node?._slotCount > 0);

        return node;
    }

    internal GreenNode GetLastTerminal()
    {
        var node = this;

        do
        {
            GreenNode lastChild = null;
            for (var i = node.SlotCount - 1; i >= 0; i--)
            {
                var child = node.GetSlot(i);
                if (child != null && child.FullWidth > 0)
                {
                    lastChild = child;
                    break;
                }
            }
            node = lastChild;
        } while (node?._slotCount > 0);

        return node;
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

        if (node1.FullWidth != node2.FullWidth)
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
