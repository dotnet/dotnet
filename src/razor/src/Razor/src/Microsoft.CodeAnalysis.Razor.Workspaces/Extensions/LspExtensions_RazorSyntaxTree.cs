// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Roslyn.LanguageServer.Protocol;

internal static partial class LspExtensions
{
    public static SyntaxNode? FindInnermostNode(
        this RazorSyntaxTree syntaxTree,
        SourceText sourceText,
        Position position,
        bool includeWhitespace = false)
    {
        if (!sourceText.TryGetAbsoluteIndex(position, out var absoluteIndex))
        {
            return null;
        }

        return syntaxTree.Root.FindInnermostNode(absoluteIndex, includeWhitespace);
    }
}
