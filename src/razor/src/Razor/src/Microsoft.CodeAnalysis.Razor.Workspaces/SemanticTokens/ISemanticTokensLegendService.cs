// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.CodeAnalysis.Razor.SemanticTokens;

internal interface ISemanticTokensLegendService
{
    SemanticTokenModifiers TokenModifiers { get; }
    SemanticTokenTypes TokenTypes { get; }
}
