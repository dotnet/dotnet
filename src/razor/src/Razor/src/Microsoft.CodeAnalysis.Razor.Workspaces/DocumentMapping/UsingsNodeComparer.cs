// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language.Syntax;

namespace Microsoft.CodeAnalysis.Razor.DocumentMapping;

internal sealed class UsingsNodeComparer : IComparer<RazorDirectiveSyntax>
{
    public static readonly UsingsNodeComparer Instance = new();

    public int Compare(RazorDirectiveSyntax? x, RazorDirectiveSyntax? y)
    {
        if (x is null)
        {
            return y is null ? 0 : -1;
        }

        if (y is null)
        {
            return 1;
        }

        RazorSyntaxFacts.TryGetNamespaceFromDirective(x, out var xNamespace);
        RazorSyntaxFacts.TryGetNamespaceFromDirective(y, out var yNamespace);

        return UsingsStringComparer.Instance.Compare(xNamespace, yNamespace);
    }
}
