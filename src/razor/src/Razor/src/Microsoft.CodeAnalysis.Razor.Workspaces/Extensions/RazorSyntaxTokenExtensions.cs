// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Razor.Language.Syntax;

internal static class RazorSyntaxTokenExtensions
{
    public static bool IsWhitespace(this SyntaxToken token)
        => token.Kind is SyntaxKind.Whitespace or SyntaxKind.NewLine;

    public static bool IsSpace(this SyntaxToken token)
        => token.Kind == SyntaxKind.Whitespace && token.Content == " ";

    public static bool IsTab(this SyntaxToken token)
        => token.Kind == SyntaxKind.Whitespace && token.Content == "\t";
}
