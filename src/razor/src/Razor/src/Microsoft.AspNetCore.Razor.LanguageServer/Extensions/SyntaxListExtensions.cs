// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language.Syntax;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Extensions;

internal static class SyntaxListExtensions
{
    internal static SyntaxNode PreviousSiblingOrSelf(this SyntaxList<RazorSyntaxNode> syntaxList, RazorSyntaxNode syntaxNode)
    {
        var index = syntaxList.IndexOf(syntaxNode);

        if (index == 0)
        {
            return syntaxNode;
        }
        else if (index == -1)
        {
            throw new ArgumentException("The provided node was not in the SyntaxList");
        }
        else
        {
            return syntaxList[index - 1];
        }
    }

    internal static SyntaxNode NextSiblingOrSelf(this SyntaxList<RazorSyntaxNode> syntaxList, RazorSyntaxNode syntaxNode)
    {
        var index = syntaxList.IndexOf(syntaxNode);

        if (index == syntaxList.Count - 1)
        {
            return syntaxNode;
        }
        else if (index == -1)
        {
            throw new ArgumentException("The provided node was not in the SyntaxList");
        }
        else
        {
            return syntaxList[index + 1];
        }
    }

    internal static bool TryGetOpenBraceNode(this SyntaxList<RazorSyntaxNode> children, [NotNullWhen(true)] out RazorMetaCodeSyntax? brace)
    {
        // If there is no whitespace between the directive and the brace then there will only be
        // three children and the brace should be the first child
        brace = null;
        if (children.FirstOrDefault(c => c.Kind == Language.SyntaxKind.RazorMetaCode) is RazorMetaCodeSyntax metaCode)
        {
            var token = metaCode.MetaCode.SingleOrDefault(m => m.Kind == Language.SyntaxKind.LeftBrace);
            if (token != null)
            {
                brace = metaCode;
            }
        }

        return brace != null;
    }
    internal static bool TryGetCloseBraceNode(this SyntaxList<RazorSyntaxNode> children, [NotNullWhen(true)] out RazorMetaCodeSyntax? brace)
    {
        // If there is no whitespace between the directive and the brace then there will only be
        // three children and the brace should be the last child
        brace = null;
        if (children.LastOrDefault(c => c.Kind == Language.SyntaxKind.RazorMetaCode) is RazorMetaCodeSyntax metaCode)
        {
            var token = metaCode.MetaCode.SingleOrDefault(m => m.Kind == Language.SyntaxKind.RightBrace);
            if (token != null)
            {
                brace = metaCode;
            }
        }

        return brace != null;
    }
    internal static bool TryGetOpenBraceToken(this SyntaxList<RazorSyntaxNode> children, [NotNullWhen(true)] out SyntaxToken? brace)
    {
        brace = null;
        if (children.TryGetOpenBraceNode(out var metacode))
        {
            var token = metacode.MetaCode.SingleOrDefault(m => m.Kind == Language.SyntaxKind.LeftBrace);
            if (token != null)
            {
                brace = token;
            }
        }

        return brace != null;
    }

    internal static bool TryGetCloseBraceToken(this SyntaxList<RazorSyntaxNode> children, [NotNullWhen(true)] out SyntaxToken? brace)
    {
        brace = null;
        if (children.TryGetCloseBraceNode(out var metacode))
        {
            var token = metacode.MetaCode.SingleOrDefault(m => m.Kind == Language.SyntaxKind.RightBrace);
            if (token != null)
            {
                brace = token;
            }
        }

        return brace != null;
    }
}
