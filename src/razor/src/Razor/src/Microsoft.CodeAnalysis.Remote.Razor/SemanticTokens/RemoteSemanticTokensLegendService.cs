// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.CodeAnalysis.Razor.SemanticTokens;

namespace Microsoft.CodeAnalysis.Remote.Razor.SemanticTokens;

[Export(typeof(ISemanticTokensLegendService)), Shared]
internal sealed class RemoteSemanticTokensLegendService : ISemanticTokensLegendService
{
    private static SemanticTokenModifiers s_tokenModifiers = null!;
    private static SemanticTokenTypes s_tokenTypes = null!;

    public SemanticTokenModifiers TokenModifiers => s_tokenModifiers;

    public SemanticTokenTypes TokenTypes => s_tokenTypes;

    public static void SetLegend(string[] tokenTypes, string[] tokenModifiers)
    {
        if (s_tokenTypes is null)
        {
            s_tokenTypes = new SemanticTokenTypes(tokenTypes);
            s_tokenModifiers = new SemanticTokenModifiers(tokenModifiers);
        }
    }
}
