// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language.Syntax;

namespace Microsoft.VisualStudio.Editor.Razor.SyntaxVisualizer;

internal class RazorSyntaxNodeList : IEnumerable<RazorSyntaxNode>
{
    private readonly ChildSyntaxList _childSyntaxList;

    public RazorSyntaxNodeList(ChildSyntaxList childSyntaxList)
    {
        _childSyntaxList = childSyntaxList;
    }

    public IEnumerator<RazorSyntaxNode> GetEnumerator()
    {
        foreach (var node in _childSyntaxList)
        {
            yield return new RazorSyntaxNode(node);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
