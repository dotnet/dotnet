// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.Language.Syntax;

internal static class RazorSyntaxNodeOrTokenExtensions
{
    public static bool ContainsOnlyWhitespace(this SyntaxNodeOrToken nodeOrToken, bool includingNewLines = true)
        => nodeOrToken.IsToken
            ? nodeOrToken.AsToken().ContainsOnlyWhitespace(includingNewLines)
            : nodeOrToken.AsNode().AssumeNotNull().ContainsOnlyWhitespace(includingNewLines);

    public static LinePositionSpan GetLinePositionSpan(this SyntaxNodeOrToken nodeOrToken, RazorSourceDocument source)
        => nodeOrToken.IsToken
            ? nodeOrToken.AsToken().GetLinePositionSpan(source)
            : nodeOrToken.AsNode().AssumeNotNull().GetLinePositionSpan(source);
}
