// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

internal static class CSharpSyntaxNodeExtensions
{
    extension(SyntaxNode node)
    {
        internal bool IsStringLiteral(bool multilineOnly = false)
        {
            if (node is not (InterpolatedStringTextSyntax or LiteralExpressionSyntax
                {
                    RawKind: (int)SyntaxKind.StringLiteralExpression or (int)SyntaxKind.Utf8StringLiteralExpression
                }))
            {
                return false;
            }

            if (!multilineOnly)
            {
                return true;
            }

            var sourceText = node.SyntaxTree.GetText();

            return sourceText.GetLinePositionSpan(node.Span).SpansMultipleLines();
        }
    }
}
