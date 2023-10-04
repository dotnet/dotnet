// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

/// <summary>
/// Transports C# semantic token responses from the Razor LS client to the Razor LS.
/// </summary>
internal class ProvideSemanticTokensResponse
{
    public ProvideSemanticTokensResponse(int[]? tokens, long hostDocumentSyncVersion)
    {
        Tokens = tokens;
        HostDocumentSyncVersion = hostDocumentSyncVersion;
    }

    public int[]? Tokens { get; }

    public long HostDocumentSyncVersion { get; }
}
