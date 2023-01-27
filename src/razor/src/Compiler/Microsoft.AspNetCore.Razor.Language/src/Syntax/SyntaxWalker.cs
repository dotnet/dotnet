// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

namespace Microsoft.AspNetCore.Razor.Language.Syntax;

/// <summary>
/// Represents a <see cref="SyntaxVisitor"/> that descends an entire <see cref="SyntaxNode"/> graph
/// visiting each SyntaxNode and its child SyntaxNodes and <see cref="SyntaxToken"/>s in depth-first order.
/// An optional range can be passed in which reduces the <see cref="SyntaxNode"/>s and <see cref="SyntaxToken"/>s
/// visited to those overlapping with the given range.
/// </summary>
internal abstract class SyntaxWalker : SyntaxVisitor
{
    private int _recursionDepth;
    private readonly TextSpan? _range;

    public SyntaxWalker(TextSpan? range = null)
    {
        _range = range;
    }

    public override void Visit(SyntaxNode node)
    {
        if (node != null && (_range is null || _range.Value.OverlapsWith(node.Span)))
        {
            _recursionDepth++;
            StackGuard.EnsureSufficientExecutionStack(_recursionDepth);

            node.Accept(this);

            _recursionDepth--;
        }
    }

    public override void DefaultVisit(SyntaxNode node)
    {
        var children = node.ChildNodes();
        for (var i = 0; i < children.Count; i++)
        {
            var child = children[i];

            if (_range is null || _range.Value.OverlapsWith(node.Span))
            {
                Visit(child);
            }
        }
    }

    public override void VisitToken(SyntaxToken token)
    {
        if (_range is null || _range.Value.OverlapsWith(token.Span))
        {
            VisitLeadingTrivia(token);
            VisitTrailingTrivia(token);
        }
    }

    public virtual void VisitLeadingTrivia(SyntaxToken token)
    {
        if (token.HasLeadingTrivia)
        {
            foreach (var trivia in token.GetLeadingTrivia())
            {
                if (_range is null || _range.Value.OverlapsWith(token.Span))
                {
                    VisitTrivia(trivia);
                }
            }
        }
    }

    public virtual void VisitTrailingTrivia(SyntaxToken token)
    {
        if (token.HasTrailingTrivia)
        {
            foreach (var trivia in token.GetTrailingTrivia())
            {
                if (_range is null || _range.Value.OverlapsWith(token.Span))
                {
                    VisitTrivia(trivia);
                }
            }
        }
    }
}
