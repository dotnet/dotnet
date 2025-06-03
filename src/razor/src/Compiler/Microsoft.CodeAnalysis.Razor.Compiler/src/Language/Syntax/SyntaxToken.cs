// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Razor.Language.Syntax;

internal class SyntaxToken : SyntaxNode
{
    internal static readonly Func<SyntaxToken, bool> NonZeroWidth = t => t.Width > 0;
    internal static readonly Func<SyntaxToken, bool> Any = t => true;

    internal SyntaxToken(GreenNode green, SyntaxNode parent, int position)
        : base(green, parent, position)
    {
    }

    internal override string SerializedValue => Serializer.Serialize(this);

    internal new InternalSyntax.SyntaxToken Green => (InternalSyntax.SyntaxToken)base.Green;

    public string Content => Green.Content;

    internal sealed override SyntaxNode GetCachedSlot(int index)
    {
        throw new InvalidOperationException("Tokens can't have slots.");
    }

    internal sealed override SyntaxNode GetNodeSlot(int slot)
    {
        throw new InvalidOperationException("Tokens can't have slots.");
    }

    protected internal override SyntaxNode ReplaceCore<TNode>(
        IEnumerable<TNode>? nodes = null,
        Func<TNode, TNode, SyntaxNode>? computeReplacementNode = null,
        IEnumerable<SyntaxToken>? tokens = null,
        Func<SyntaxToken, SyntaxToken, SyntaxToken>? computeReplacementToken = null)
        => Assumed.Unreachable<SyntaxNode>();

    protected internal override SyntaxNode ReplaceNodeInListCore(SyntaxNode originalNode, IEnumerable<SyntaxNode> replacementNodes)
        => Assumed.Unreachable<SyntaxNode>();

    protected internal override SyntaxNode InsertNodesInListCore(SyntaxNode nodeInList, IEnumerable<SyntaxNode> nodesToInsert, bool insertBefore)
        => Assumed.Unreachable<SyntaxNode>();

    protected internal override SyntaxNode ReplaceTokenInListCore(SyntaxToken originalToken, IEnumerable<SyntaxToken> newTokens)
        => Assumed.Unreachable<SyntaxNode>();

    protected internal override SyntaxNode InsertTokensInListCore(SyntaxToken originalToken, IEnumerable<SyntaxToken> newTokens, bool insertBefore)
        => Assumed.Unreachable<SyntaxNode>();

    /// <summary>
    /// Gets the token that follows this token in the syntax tree.
    /// </summary>
    /// <returns>The token that follows this token in the syntax tree.</returns>
    public SyntaxToken? GetNextToken(bool includeZeroWidth = false)
    {
        return SyntaxNavigator.GetNextToken(this, includeZeroWidth);
    }

    /// <summary>
    /// Gets the token that precedes this token in the syntax tree.
    /// </summary>
    /// <returns>The previous token that precedes this token in the syntax tree.</returns>
    public SyntaxToken? GetPreviousToken(bool includeZeroWidth = false)
    {
        return SyntaxNavigator.GetPreviousToken(this, includeZeroWidth);
    }

    public override string ToString()
    {
        return Content;
    }
}
