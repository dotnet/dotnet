// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text.Json.Serialization;

namespace Microsoft.CodeAnalysis.Razor.Workspaces.Protocol.SemanticTokens;

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

    [JsonPropertyName("tokens")]
    public int[]? Tokens { get; }

    [JsonPropertyName("hostDocumentSyncVersion")]
    public long HostDocumentSyncVersion { get; }
}
