// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.Language.Syntax;

internal interface IStartTagSyntaxNode
{
    SyntaxList<RazorSyntaxNode> Attributes { get; }
    SyntaxToken Name { get; }
    TextSpan Span { get; }
    int SpanStart { get; }
    SyntaxNode? Parent { get; }
}
