// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Razor.LanguageServer.Protocol;

/// <summary>
/// Interface for delegated params that enables sharing of code in RazorCustomMessageTarget
/// </summary>
internal interface IDelegatedParams
{
    public TextDocumentIdentifierAndVersion Identifier { get; }
    public RazorLanguageKind ProjectedKind { get; }
}
