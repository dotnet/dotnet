// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;

namespace Microsoft.VisualStudio.Razor.SyntaxVisualizer;

/// <summary>
/// Wraps a syntax node for projects that don't have IVT to the compiler
/// </summary>
internal class RazorSyntaxNode : IEnumerable<RazorSyntaxNode>
{
    private readonly SyntaxNodeOrToken _nodeOrToken;

    public int SpanStart => _nodeOrToken.SpanStart;

    public int SpanEnd => _nodeOrToken.Span.End;

    public int SpanLength => _nodeOrToken.Span.Length;

    public string Kind => _nodeOrToken.Kind.ToString();

    public RazorSyntaxNodeList Children { get; }

    public RazorSyntaxNode(SyntaxNodeOrToken node)
    {
        _nodeOrToken = node;
        Children = new RazorSyntaxNodeList(_nodeOrToken.ChildNodesAndTokens());
    }

    public RazorSyntaxNode(RazorSyntaxTree tree)
    {
        _nodeOrToken = tree.Root;
        Children = new RazorSyntaxNodeList(_nodeOrToken.ChildNodesAndTokens());
    }

    public IEnumerator<RazorSyntaxNode> GetEnumerator()
    {
        return Children.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public override string ToString()
    {
        return _nodeOrToken.ToString();
    }
}
