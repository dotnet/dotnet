// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Language.Syntax;

[DebuggerDisplay("{GetDebuggerDisplay(), nq}")]
internal abstract partial class SyntaxNode
{
    public SyntaxNode(GreenNode green, SyntaxNode parent, int position)
    {
        Green = green;
        Parent = parent;
        Position = position;
    }

    internal GreenNode Green { get; }

    public SyntaxNode Parent { get; }

    public int Position { get; }

    public int EndPosition => Position + FullWidth;

    public SyntaxKind Kind => Green.Kind;

    public int Width => Green.Width;

    public int FullWidth => Green.FullWidth;

    public int SpanStart => Position + Green.GetLeadingTriviaWidth();

    public TextSpan FullSpan => new TextSpan(Position, Green.FullWidth);

    public TextSpan Span
    {
        get
        {
            // Start with the full span.
            var start = Position;
            var width = Green.FullWidth;

            // adjust for preceding trivia (avoid calling this twice, do not call Green.Width)
            var precedingWidth = Green.GetLeadingTriviaWidth();
            start += precedingWidth;
            width -= precedingWidth;

            // adjust for following trivia width
            width -= Green.GetTrailingTriviaWidth();

            Debug.Assert(width >= 0);
            return new TextSpan(start, width);
        }
    }

    internal int SlotCount => Green.SlotCount;

    public bool IsList => Green.IsList;

    public bool IsMissing => Green.IsMissing;

    public bool IsToken => Green.IsToken;

    public bool IsTrivia => Green.IsTrivia;

    public bool HasLeadingTrivia
    {
        get
        {
            return GetLeadingTrivia().Count > 0;
        }
    }

    public bool HasTrailingTrivia
    {
        get
        {
            return GetTrailingTrivia().Count > 0;
        }
    }

    public bool ContainsDiagnostics => Green.ContainsDiagnostics;

    public bool ContainsAnnotations => Green.ContainsAnnotations;

    internal string SerializedValue => SyntaxSerializer.Serialize(this);

    public abstract TResult Accept<TResult>(SyntaxVisitor<TResult> visitor);

    public abstract void Accept(SyntaxVisitor visitor);

    internal abstract SyntaxNode GetNodeSlot(int index);

    internal abstract SyntaxNode GetCachedSlot(int index);

    internal SyntaxNode GetRed(ref SyntaxNode field, int slot)
    {
        var result = field;

        if (result == null)
        {
            var green = Green.GetSlot(slot);
            if (green != null)
            {
                Interlocked.CompareExchange(ref field, green.CreateRed(this, GetChildPosition(slot)), null);
                result = field;
            }
        }

        return result;
    }

    // Special case of above function where slot = 0, does not need GetChildPosition
    internal SyntaxNode GetRedAtZero(ref SyntaxNode field)
    {
        var result = field;

        if (result == null)
        {
            var green = Green.GetSlot(0);
            if (green != null)
            {
                Interlocked.CompareExchange(ref field, green.CreateRed(this, Position), null);
                result = field;
            }
        }

        return result;
    }

    protected T GetRed<T>(ref T field, int slot) where T : SyntaxNode
    {
        var result = field;

        if (result == null)
        {
            var green = Green.GetSlot(slot);
            if (green != null)
            {
                Interlocked.CompareExchange(ref field, (T)green.CreateRed(this, this.GetChildPosition(slot)), null);
                result = field;
            }
        }

        return result;
    }

    // special case of above function where slot = 0, does not need GetChildPosition
    protected T GetRedAtZero<T>(ref T field) where T : SyntaxNode
    {
        var result = field;

        if (result == null)
        {
            var green = Green.GetSlot(0);
            if (green != null)
            {
                Interlocked.CompareExchange(ref field, (T)green.CreateRed(this, Position), null);
                result = field;
            }
        }

        return result;
    }

    internal SyntaxNode GetRedElement(ref SyntaxNode element, int slot)
    {
        Debug.Assert(IsList);

        var result = element;

        if (result == null)
        {
            var green = Green.GetSlot(slot);
            // passing list's parent
            Interlocked.CompareExchange(ref element, green.CreateRed(Parent, GetChildPosition(slot)), null);
            result = element;
        }

        return result;
    }

    internal virtual int GetChildPosition(int index)
    {
        var offset = 0;
        var green = Green;
        while (index > 0)
        {
            index--;
            var prevSibling = GetCachedSlot(index);
            if (prevSibling != null)
            {
                return prevSibling.EndPosition + offset;
            }
            var greenChild = green.GetSlot(index);
            if (greenChild != null)
            {
                offset += greenChild.FullWidth;
            }
        }

        return Position + offset;
    }

    public virtual SyntaxTriviaList GetLeadingTrivia()
    {
        var firstToken = GetFirstToken();
        return firstToken != null ? firstToken.GetLeadingTrivia() : default(SyntaxTriviaList);
    }

    public virtual SyntaxTriviaList GetTrailingTrivia()
    {
        var lastToken = GetLastToken();
        return lastToken != null ? lastToken.GetTrailingTrivia() : default(SyntaxTriviaList);
    }

    internal SyntaxToken GetFirstToken()
    {
        return ((SyntaxToken)GetFirstTerminal());
    }

    internal SyntaxList<SyntaxToken> GetTokens()
    {
        var tokens = SyntaxListBuilder<SyntaxToken>.Create();

        AddTokens(this, tokens);

        return tokens;

        static void AddTokens(SyntaxNode current, SyntaxListBuilder<SyntaxToken> tokens)
        {
            if (current.SlotCount == 0 && current is SyntaxToken token)
            {
                // Token
                tokens.Add(token);
                return;
            }

            for (var i = 0; i < current.SlotCount; i++)
            {
                var child = current.GetNodeSlot(i);

                if (child != null)
                {
                    AddTokens(child, tokens);
                }
            }
        }
    }

    internal SyntaxToken GetLastToken()
    {
        return ((SyntaxToken)GetLastTerminal());
    }

    public SyntaxNode GetFirstTerminal()
    {
        var node = this;

        do
        {
            var foundChild = false;
            for (int i = 0, n = node.SlotCount; i < n; i++)
            {
                var child = node.GetNodeSlot(i);
                if (child != null)
                {
                    node = child;
                    foundChild = true;
                    break;
                }
            }

            if (!foundChild)
            {
                return null;
            }
        }
        while (node.SlotCount != 0);

        return node == this ? this : node;
    }

    public SyntaxNode GetLastTerminal()
    {
        var node = this;

        do
        {
            SyntaxNode lastChild = null;
            for (var i = node.SlotCount - 1; i >= 0; i--)
            {
                var child = node.GetNodeSlot(i);
                if (child != null && child.FullWidth > 0)
                {
                    lastChild = child;
                    break;
                }
            }
            node = lastChild;
        } while (node?.SlotCount > 0);

        return node;
    }

    /// <summary>
    /// The list of child nodes of this node, where each element is a SyntaxNode instance.
    /// </summary>
    public ChildSyntaxList ChildNodes()
    {
        return new ChildSyntaxList(this);
    }

    /// <summary>
    /// Gets a list of ancestor nodes
    /// </summary>
    public IEnumerable<SyntaxNode> Ancestors()
    {
        return Parent?
            .AncestorsAndSelf() ??
            Array.Empty<SyntaxNode>();
    }

    /// <summary>
    /// Gets a list of ancestor nodes (including this node)
    /// </summary>
    public IEnumerable<SyntaxNode> AncestorsAndSelf()
    {
        for (var node = this; node != null; node = node.Parent)
        {
            yield return node;
        }
    }

#nullable enable
    /// <summary>
    /// Gets the first node of type TNode that matches the predicate.
    /// </summary>
    public TNode? FirstAncestorOrSelf<TNode>(Func<TNode, bool>? predicate = null)
        where TNode : SyntaxNode
    {
        for (var node = this; node != null; node = node.Parent)
        {
            if (node is TNode tnode && (predicate == null || predicate(tnode)))
            {
                return tnode;
            }
        }

        return default;
    }
#nullable disable

    /// <summary>
    /// Gets a list of descendant nodes in prefix document order.
    /// </summary>
    /// <param name="descendIntoChildren">An optional function that determines if the search descends into the argument node's children.</param>
    public IEnumerable<SyntaxNode> DescendantNodes(Func<SyntaxNode, bool> descendIntoChildren = null)
    {
        return DescendantNodesImpl(FullSpan, descendIntoChildren, includeSelf: false);
    }

    /// <summary>
    /// Gets a list of descendant nodes (including this node) in prefix document order.
    /// </summary>
    /// <param name="descendIntoChildren">An optional function that determines if the search descends into the argument node's children.</param>
    public IEnumerable<SyntaxNode> DescendantNodesAndSelf(Func<SyntaxNode, bool> descendIntoChildren = null)
    {
        return DescendantNodesImpl(FullSpan, descendIntoChildren, includeSelf: true);
    }

    protected internal SyntaxNode ReplaceCore<TNode>(
        IEnumerable<TNode> nodes = null,
        Func<TNode, TNode, SyntaxNode> computeReplacementNode = null)
        where TNode : SyntaxNode
    {
        return SyntaxReplacer.Replace(this, nodes, computeReplacementNode);
    }

    protected internal SyntaxNode ReplaceNodeInListCore(SyntaxNode originalNode, IEnumerable<SyntaxNode> replacementNodes)
    {
        return SyntaxReplacer.ReplaceNodeInList(this, originalNode, replacementNodes);
    }

    protected internal SyntaxNode InsertNodesInListCore(SyntaxNode nodeInList, IEnumerable<SyntaxNode> nodesToInsert, bool insertBefore)
    {
        return SyntaxReplacer.InsertNodeInList(this, nodeInList, nodesToInsert, insertBefore);
    }

    public RazorDiagnostic[] GetDiagnostics()
    {
        return Green.GetDiagnostics();
    }

    public SyntaxAnnotation[] GetAnnotations()
    {
        return Green.GetAnnotations();
    }

    public bool IsEquivalentTo(SyntaxNode other)
    {
        if (this == other)
        {
            return true;
        }

        if (other == null)
        {
            return false;
        }

        return Green.IsEquivalentTo(other.Green);
    }

#nullable enable
    /// <summary>
    /// Finds a descendant token of this node whose span includes the supplied position.
    /// </summary>
    /// <param name="position">The character position of the token relative to the beginning of the file.</param>
    /// <param name="includeWhitespace">
    /// True to return whitespace or newline tokens. If false, finds the closest non-whitespace, non-newline token that matches the following algorithm:
    /// <list type="number">
    /// <item>
    /// <description>
    /// Scan backwards until a non-whitespace token is found. If a newline is found, continue to the next step. Otherwise, return the found token.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Scan forwards until a non-whitespace, non-newline token is found. Return the found token.
    /// </description>
    /// </item>
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when requested position is out of range of the root token requested. This includes the whitespace scanning: calling FindToken(0, false)
    /// on a whitespace token will throw.
    /// </exception>
    public SyntaxToken FindToken(int position, bool includeWhitespace = false)
    {
        if (position == EndPosition && this is RazorDocumentSyntax document)
        {
            return document.EndOfFile;
        }

        if (!FullSpan.Contains(position))
        {
            throw new ArgumentOutOfRangeException(nameof(position));
        }

        // Stack for walking efficiently back up the tree. Only used when includeWhitespace is false.
        using var stack = new PooledArrayBuilder<(SyntaxNode node, int nodeIndexInParent)>();
        SyntaxNode curNode = this;

        while (true)
        {
            Debug.Assert(curNode.Kind is < SyntaxKind.FirstAvailableTokenKind and >= 0);
            Debug.Assert(curNode.FullSpan.Contains(position));

            if (!curNode.IsToken)
            {
                curNode = ChildSyntaxList.ChildThatContainsPosition(curNode, position, out var nodeIndexInParent);
                if (!includeWhitespace)
                {
                    stack.Push((curNode, nodeIndexInParent));
                }
            }
            else
            {
                // Once we've found the token that covers the exact position, we potentially need to account for whitespace to
                // partially emulate Roslyn's behavior. The rule is pretty simple:
                //
                //  After a non-whitespace token, all whitespace up to and including the next newline is considered part of the previous token.
                //  All whitespace after it is considered part of the next token.
                //
                // Roslyn, of course, includes all trivia in this rule, and also uses trivia to represent comments. Razor does neither of these things,
                // and we only want to skip past whitespace. Therefore, the algorithm we implement is:
                //
                //  Walk backwards until we find a non-whitespace token. If we find something that isn't a newline, that is the node requested.
                //  If we find a newline, we need to walk forwards until we find the first non-whitespace or newline token. That is the node requested.
                var foundToken = (SyntaxToken)curNode;
                if (includeWhitespace || foundToken.Kind is not (SyntaxKind.Whitespace or SyntaxKind.NewLine))
                {
                    return foundToken;
                }

                // Walk backwards until we find a non-whitespace token. We accomplish this by looking up the stack and walking nodes backwards from where we
                // were located.
                if (tryWalkBackwards(ref stack.AsRef(), out foundToken))
                {
                    return foundToken;
                }

                // Encountered a newline while backtracking, so we need to walk forward instead.
                return walkForward(ref stack.AsRef());
            }

            bool tryWalkBackwards(ref PooledArrayBuilder<(SyntaxNode node, int nodeIndexInParent)> stack, [NotNullWhen(true)] out SyntaxToken? foundToken)
            {
                // Can't just pop the stack, we may need to rewalk from the start to find the next node if this fails
                for (var originalStackPosition = stack.Count - 1; originalStackPosition >= 0; originalStackPosition--)
                {
                    var (node, nodeIndexInParent) = stack[originalStackPosition];

                    switch (walkNodeChildren(node.Parent, nodeIndexInParent, walkBackwards: true, stopOnNewline: true, out foundToken))
                    {
                        case true:
                            return true;
                        case false:
                            return false;
                        case null:
                            // Didn't find anything in this node, keep walking
                            continue;
                    }
                }

                // Walked all the way back to the end of the node that was requested and did not find either a newline or non-whitespace token. The user requested
                // something out of range.
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            SyntaxToken walkForward(ref PooledArrayBuilder<(SyntaxNode node, int nodeIndexInParent)> stack)
            {
                while (stack.TryPop(out var entry))
                {
                    var (node, nodeIndexInParent) = entry;

                    switch (walkNodeChildren(node.Parent, nodeIndexInParent, walkBackwards: false, stopOnNewline: false, out var foundToken))
                    {
                        case true:
                            return foundToken;
                        case null:
                            // Didn't find anything in this node, keep walking
                            continue;
                        case false:
                            // False is only returned when stopOnNewline is true
                            return Assumed.Unreachable<SyntaxToken>();
                    }
                }

                // Walked all the way forward to the end of the root that was requested and did not find any non-whitespace tokens. The user requested
                // something out of range.
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            static bool? walkNodeChildren(SyntaxNode parent, int startIndex, bool walkBackwards, bool stopOnNewline, [NotNullWhen(true)] out SyntaxToken? foundToken)
            {
                Debug.Assert(parent != null, "Node should have been out of range of the document");

                var (indexIncrement, endIndex) = walkBackwards
                    ? (-1, -1)
                    : (1, ChildSyntaxList.CountNodes(parent!.Green));

                for (int currentIndex = startIndex + indexIncrement; currentIndex != endIndex; currentIndex += indexIncrement)
                {
                    var currentChild = ChildSyntaxList.ItemInternal(parent, currentIndex);
                    switch (currentChild.Kind)
                    {
                        case SyntaxKind.NewLine when stopOnNewline:
                            // We found a newline, we need to walk forwards until we find the first non-whitespace or newline token.
                            foundToken = null;
                            return false;
                        case SyntaxKind.Whitespace:
                            // We found whitespace, keep walking
                            continue;
                        default:
                            if (currentChild.IsToken)
                            {
                                // This is the node we're looking for
                                foundToken = (SyntaxToken)currentChild;
                                return true;
                            }
                            else
                            {
                                // The previous node is something complex. Walk its children to find a desired token.
                                // If this ever becomes a stack overflow concern, we could make it iterative, but this is much
                                // simpler for now.
                                switch (walkNodeChildren(parent: currentChild, startIndex: walkBackwards ? ChildSyntaxList.CountNodes(currentChild.Green) : -1, walkBackwards, stopOnNewline, out foundToken))
                                {
                                    case true:
                                        return true;
                                    case false:
                                        return false;
                                    case null:
                                        // Couldn't find a desired node in the child, keep walking backwards. This is believed to be impossible,
                                        // but isn't an inherent issue in itself, so if it's encountered we should understand the scenario and
                                        // cover with a test if it's not decided to be a bug.
                                        Debug.Fail("This is believed to be impossible. If this fails, add a test for the case.");
                                        continue;
                                }
                            }
                    }
                }

                foundToken = null;

                // The start of the document is the only special case:
                //  - If we're walking backwards and hit the start of the document, we need to treat this as if we should walk forward. There's no
                //    previous node to attach the token to, so walking forward is the only option.
                if (walkBackwards && parent!.SpanStart == 0)
                {
                    return false;
                }

                // Got to the end of the node without finding a desired token. Pop up the stack and try again
                return null;
            }
        }
    }
#nullable disable

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.Append(Green.ToString());
        builder.AppendFormat(CultureInfo.InvariantCulture, " at {0}::{1}", Position, FullWidth);

        return builder.ToString();
    }

    public virtual string ToFullString()
    {
        return Green.ToFullString();
    }

    protected virtual string GetDebuggerDisplay()
    {
        if (IsToken)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0};[{1}]", Kind, ToFullString());
        }

        return string.Format(CultureInfo.InvariantCulture, "{0} [{1}..{2})", Kind, Position, EndPosition);
    }
}
