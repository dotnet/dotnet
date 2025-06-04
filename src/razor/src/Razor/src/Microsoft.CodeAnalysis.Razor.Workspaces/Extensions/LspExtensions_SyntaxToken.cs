// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Roslyn.LanguageServer.Protocol;

internal static partial class LspExtensions
{
    public static LspRange GetRange(this SyntaxToken token, RazorSourceDocument source)
    {
        var linePositionSpan = token.GetLinePositionSpan(source);

        return LspFactory.CreateRange(linePositionSpan);
    }
}
