// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.VisualStudio.LanguageServer.Protocol;

internal static partial class VsLspExtensions
{
    public static Range GetRange(this SyntaxNode node, RazorSourceDocument source)
    {
        var linePositionSpan = node.GetLinePositionSpan(source);

        return VsLspFactory.CreateRange(linePositionSpan);
    }
}
