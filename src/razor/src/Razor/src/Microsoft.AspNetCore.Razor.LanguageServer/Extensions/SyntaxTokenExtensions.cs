// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using SyntaxKind = Microsoft.AspNetCore.Razor.Language.SyntaxKind;
using SyntaxToken = Microsoft.AspNetCore.Razor.Language.Syntax.SyntaxToken;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Extensions;

internal static class SyntaxTokenExtensions
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
