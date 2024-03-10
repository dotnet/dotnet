// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

using Microsoft.AspNetCore.Razor.Language.Syntax;

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
