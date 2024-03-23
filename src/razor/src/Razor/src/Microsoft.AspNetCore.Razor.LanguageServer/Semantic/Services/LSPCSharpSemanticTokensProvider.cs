// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic.Models;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.SemanticTokens;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

internal class LSPCSharpSemanticTokensProvider(LanguageServerFeatureOptions languageServerFeatureOptions, IClientConnection clientConnection, IRazorLoggerFactory loggerFactory) : ICSharpSemanticTokensProvider
{
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions = languageServerFeatureOptions;
    private readonly IClientConnection _clientConnection = clientConnection;
    private readonly ILogger _logger = loggerFactory.CreateLogger<LSPCSharpSemanticTokensProvider>();

    public async Task<int[]?> GetCSharpSemanticTokensResponseAsync(
            VersionedDocumentContext documentContext,
            ImmutableArray<LinePositionSpan> csharpSpans,
            Guid correlationId,
            CancellationToken cancellationToken)
    {
        var documentVersion = documentContext.Version;

        using var _ = ListPool<Range>.GetPooledObject(out var csharpRangeList);
        foreach (var span in csharpSpans)
        {
            csharpRangeList.Add(span.ToRange());
        }

        var csharpRanges = csharpRangeList.ToArray();

        var parameter = new ProvideSemanticTokensRangesParams(documentContext.Identifier.TextDocumentIdentifier, documentVersion, csharpRanges, correlationId);
        ProvideSemanticTokensResponse? csharpResponse;
        if (_languageServerFeatureOptions.UsePreciseSemanticTokenRanges)
        {
            csharpResponse = await GetCsharpResponseAsync(_clientConnection, parameter, CustomMessageNames.RazorProvidePreciseRangeSemanticTokensEndpoint, cancellationToken).ConfigureAwait(false);

            // Likely the server doesn't support the new endpoint, fallback to the original one
            if (csharpResponse?.Tokens is null && csharpRanges.Length > 1)
            {
                var minimalRange = new Range
                {
                    Start = csharpRanges[0].Start,
                    End = csharpRanges[^1].End
                };

                var newParams = new ProvideSemanticTokensRangesParams(
                    parameter.TextDocument,
                    parameter.RequiredHostDocumentVersion,
                    [minimalRange],
                    parameter.CorrelationId);

                csharpResponse = await GetCsharpResponseAsync(_clientConnection, newParams, CustomMessageNames.RazorProvideSemanticTokensRangeEndpoint, cancellationToken).ConfigureAwait(false);
            }
        }
        else
        {
            csharpResponse = await GetCsharpResponseAsync(_clientConnection, parameter, CustomMessageNames.RazorProvideSemanticTokensRangeEndpoint, cancellationToken).ConfigureAwait(false);
        }

        if (csharpResponse is null)
        {
            // C# isn't ready yet, don't make Razor wait for it. Once C# is ready they'll send a refresh notification.
            return [];
        }

        var csharpVersion = csharpResponse.HostDocumentSyncVersion;
        if (csharpVersion != documentVersion)
        {
            // No C# response or C# is out of sync with us. Unrecoverable, return null to indicate no change.
            // Once C# syncs up they'll send a refresh notification.
            if (csharpVersion == -1)
            {
                _logger.LogWarning("Didn't get C# tokens because the virtual document wasn't found, or other problem. We were wanting {documentVersion} but C# could not get any version.", documentVersion);
            }
            else if (csharpVersion < documentVersion)
            {
                _logger.LogDebug("Didn't wait for Roslyn to get the C# version we were expecting. We are wanting {documentVersion} but C# is at {csharpVersion}.", documentVersion, csharpVersion);
            }
            else
            {
                _logger.LogWarning("We are behind the C# version which is surprising. Could be an old request that wasn't cancelled, but if not, expect most future requests to fail. We were wanting {documentVersion} but C# is at {csharpVersion}.", documentVersion, csharpVersion);
            }

            return null;
        }

        return csharpResponse.Tokens ?? [];
    }

    private static Task<ProvideSemanticTokensResponse?> GetCsharpResponseAsync(IClientConnection clientConnection, ProvideSemanticTokensRangesParams parameter, string lspMethodName, CancellationToken cancellationToken)
    {
        return clientConnection.SendRequestAsync<ProvideSemanticTokensRangesParams, ProvideSemanticTokensResponse?>(
            lspMethodName,
            parameter,
            cancellationToken);
    }
}
