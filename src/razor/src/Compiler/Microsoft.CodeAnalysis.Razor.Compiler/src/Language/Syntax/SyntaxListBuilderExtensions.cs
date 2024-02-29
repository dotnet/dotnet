// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.AspNetCore.Razor.Language.Syntax;

internal static class SyntaxListBuilderExtensions
{
    public static SyntaxList<SyntaxNode> ToList(this SyntaxListBuilder builder)
    {
        if (builder == null || builder.Count == 0)
        {
            return default;
        }

        return new SyntaxList<SyntaxNode>(builder.ToListNode().AssumeNotNull().CreateRed());
    }

    public static SyntaxList<SyntaxNode> ToList(this SyntaxListBuilder builder, SyntaxNode parent)
    {
        if (builder == null || builder.Count == 0)
        {
            return default;
        }

        return new SyntaxList<SyntaxNode>(builder.ToListNode().AssumeNotNull().CreateRed(parent, parent.Position));
    }

    public static SyntaxList<TNode> ToList<TNode>(this SyntaxListBuilder builder)
        where TNode : SyntaxNode
    {
        if (builder == null || builder.Count == 0)
        {
            return default;
        }

        return new SyntaxList<TNode>(builder.ToListNode().AssumeNotNull().CreateRed());
    }

    public static SyntaxList<TNode> ToList<TNode>(this SyntaxListBuilder builder, SyntaxNode parent)
        where TNode : SyntaxNode
    {
        if (builder == null || builder.Count == 0)
        {
            return default;
        }

        return new SyntaxList<TNode>(builder.ToListNode().AssumeNotNull().CreateRed(parent, parent.Position));
    }
}
