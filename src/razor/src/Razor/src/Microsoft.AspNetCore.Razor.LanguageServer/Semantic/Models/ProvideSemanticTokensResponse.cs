// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

/// <summary>
/// Transports C# semantic token responses from the Razor LS client to the Razor LS.
/// </summary>
internal class ProvideSemanticTokensResponse
{
    public ProvideSemanticTokensResponse(int[]? tokens, long? hostDocumentSyncVersion)
    {
        Tokens = tokens;
        HostDocumentSyncVersion = hostDocumentSyncVersion;
    }

    public int[]? Tokens { get; }

    public long? HostDocumentSyncVersion { get; }

    public override bool Equals(object? obj)
    {
        if (obj is not ProvideSemanticTokensResponse other ||
            HostDocumentSyncVersion != other.HostDocumentSyncVersion)
        {
            return false;
        }

        return Tokens == other.Tokens ||
              (Tokens is not null && other.Tokens is not null && Tokens.SequenceEqual(other.Tokens));
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }
}
