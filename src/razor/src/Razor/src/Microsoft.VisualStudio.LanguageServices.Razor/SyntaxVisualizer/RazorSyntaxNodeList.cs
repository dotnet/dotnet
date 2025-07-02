// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language.Syntax;

namespace Microsoft.VisualStudio.Razor.SyntaxVisualizer;

internal class RazorSyntaxNodeList(ChildSyntaxList childSyntaxList) : IEnumerable<RazorSyntaxNode>
{
    private readonly ChildSyntaxList _childSyntaxList = childSyntaxList;

    public IEnumerator<RazorSyntaxNode> GetEnumerator()
    {
        foreach (var nodeOrToken in _childSyntaxList)
        {
            yield return new RazorSyntaxNode(nodeOrToken);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
