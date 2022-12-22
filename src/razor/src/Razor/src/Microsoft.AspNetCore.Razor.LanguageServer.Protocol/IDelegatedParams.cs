// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Protocol;

/// <summary>
/// Interface for delegated params that enables sharing of code in DefaultRazorLanguageServerCustomMessageTarget
/// </summary>
internal interface IDelegatedParams
{
    public VersionedTextDocumentIdentifier HostDocument { get; }
    public RazorLanguageKind ProjectedKind { get; }
}
