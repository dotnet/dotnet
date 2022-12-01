// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;

namespace Microsoft.VisualStudio.Editor.Razor.SyntaxVisualizer;

/// <summary>
/// Wraps a syntax node for projects that don't have IVT to the compiler
/// </summary>
internal class RazorSyntaxNode : IEnumerable<RazorSyntaxNode>
{
    private readonly SyntaxNode _node;

    public int SpanStart => _node.SpanStart;

    public int SpanEnd => _node.Span.End;

    public int SpanLength => _node.Span.Length;

    public string Kind => _node.Kind.ToString();

    public RazorSyntaxNodeList Children { get; }

    public RazorSyntaxNode(SyntaxNode node)
    {
        _node = node;
        Children = new RazorSyntaxNodeList(_node.ChildNodes());
    }

    public RazorSyntaxNode(RazorSyntaxTree tree)
    {
        _node = tree.Root;
        Children = new RazorSyntaxNodeList(_node.ChildNodes());
    }

    public IEnumerator<RazorSyntaxNode> GetEnumerator()
    {
        return Children.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    public override string ToString()
    {
        return _node.ToString();
    }
}
