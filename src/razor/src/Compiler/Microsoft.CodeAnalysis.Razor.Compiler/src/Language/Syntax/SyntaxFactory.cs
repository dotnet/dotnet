// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language.Legacy;

namespace Microsoft.AspNetCore.Razor.Language.Syntax;

internal static partial class SyntaxFactory
{
    public static SyntaxToken Token(SyntaxKind kind, params RazorDiagnostic[] diagnostics)
    {
        return Token(kind, content: string.Empty, diagnostics: diagnostics);
    }

    public static SyntaxToken Token(SyntaxKind kind, string content, params RazorDiagnostic[] diagnostics)
    {
        return new SyntaxToken(parent: null, InternalSyntax.SyntaxFactory.Token(kind, content, diagnostics), position: 0, index: 0);
    }

    internal static SyntaxToken MissingToken(SyntaxKind kind, params RazorDiagnostic[] diagnostics)
    {
        return new SyntaxToken(parent: null, InternalSyntax.SyntaxFactory.MissingToken(kind, diagnostics), position: 0, index: 0);
    }

    public static SyntaxTokenList TokenList()
        => default;

    public static SyntaxTokenList TokenList(SyntaxToken token)
        => new(token);

    public static SyntaxTokenList TokenList(params ReadOnlySpan<SyntaxToken> tokens)
        => new(tokens);

    public static SyntaxTokenList TokenList(IEnumerable<SyntaxToken> tokens)
        => new(tokens);

    public static MarkupTextLiteralSyntax MarkupTextLiteral(SyntaxToken token, ISpanChunkGenerator? chunkGenerator)
        => MarkupTextLiteral(new SyntaxTokenList(token), chunkGenerator);

    public static RazorMetaCodeSyntax RazorMetaCode(SyntaxToken token, ISpanChunkGenerator? chunkGenerator)
        => RazorMetaCode(new SyntaxTokenList(token), chunkGenerator);
}
