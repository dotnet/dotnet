// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;

#pragma warning disable IDE0065 // Misplaced using directive
using SyntaxKind = AspNetCore.Razor.Language.SyntaxKind;
using SyntaxToken = AspNetCore.Razor.Language.Syntax.SyntaxToken;
#pragma warning restore IDE0065 // Misplaced using directive

internal static class RazorSyntaxTokenExtensions
{
    public static bool IsWhitespace(this SyntaxToken token)
    {
        return token.Kind == SyntaxKind.Whitespace || token.Kind == SyntaxKind.NewLine;
    }

    public static bool IsSpace(this SyntaxToken token)
    {
        return token.Kind == SyntaxKind.Whitespace && token.Content.Equals(" ", StringComparison.Ordinal);
    }

    public static bool IsTab(this SyntaxToken token)
    {
        return token.Kind == SyntaxKind.Whitespace && token.Content.Equals("\t", StringComparison.Ordinal);
    }
}
