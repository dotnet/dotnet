// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.CodeAnalysis.Razor.SemanticTokens;

namespace Microsoft.CodeAnalysis.Remote.Razor.SemanticTokens;

[Shared]
[Export(typeof(ISemanticTokensLegendService))]
[Export(typeof(RemoteSemanticTokensLegendService))]
internal sealed class RemoteSemanticTokensLegendService : ISemanticTokensLegendService
{
    private SemanticTokenModifiers _tokenModifiers = null!;
    private SemanticTokenTypes _tokenTypes = null!;

    public SemanticTokenModifiers TokenModifiers => _tokenModifiers;

    public SemanticTokenTypes TokenTypes => _tokenTypes;

    public void SetLegend(string[] tokenTypes, string[] tokenModifiers)
    {
        _tokenTypes = new SemanticTokenTypes(tokenTypes);
        _tokenModifiers = new SemanticTokenModifiers(tokenModifiers);
    }
}
