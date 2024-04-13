// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces.Protocol.SemanticTokens;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json.Linq;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Endpoints;

internal partial class RazorCustomMessageTarget
{
    // Called by the Razor Language Server to provide ranged semantic tokens from the platform.
    [JsonRpcMethod(CustomMessageNames.RazorProvideSemanticTokensRangeEndpoint, UseSingleObjectParameterDeserialization = true)]
    public Task<ProvideSemanticTokensResponse?> ProvideMinimalRangeSemanticTokensAsync(
        ProvideSemanticTokensRangesParams inputParams,
        CancellationToken cancellationToken)
    {
        Debug.Assert(inputParams.Ranges.Length == 1);

        return ProvideSemanticTokensAsync(
            semanticTokensParams: inputParams,
            lspMethodName: Methods.TextDocumentSemanticTokensRangeName,
            capabilitiesFilter: _ => true,
            requestParams: new SemanticTokensRangeParams
            {
                TextDocument = inputParams.TextDocument,
                Range = inputParams.Ranges[0],
            },
            cancellationToken);
    }

    [JsonRpcMethod(CustomMessageNames.RazorProvidePreciseRangeSemanticTokensEndpoint, UseSingleObjectParameterDeserialization = true)]
    public Task<ProvideSemanticTokensResponse?> ProvidePreciseRangeSemanticTokensAsync(
        ProvideSemanticTokensRangesParams inputParams,
        CancellationToken cancellationToken)
    {
        return ProvideSemanticTokensAsync(
            semanticTokensParams: inputParams,
            lspMethodName: RazorLSPConstants.RoslynSemanticTokenRangesEndpointName,
            capabilitiesFilter: SupportsPreciseRanges,
            requestParams: new SemanticTokensRangesParams()
            {
                TextDocument = inputParams.TextDocument,
                Ranges = inputParams.Ranges
            },
            cancellationToken);
    }

    private async Task<ProvideSemanticTokensResponse?> ProvideSemanticTokensAsync(
        ProvideSemanticTokensRangesParams semanticTokensParams,
        string lspMethodName,
        Func<JToken, bool> capabilitiesFilter,
        SemanticTokensParams requestParams,
        CancellationToken cancellationToken)
    {
        if (semanticTokensParams is null)
        {
            throw new ArgumentNullException(nameof(semanticTokensParams));
        }

        if (semanticTokensParams.Ranges is null)
        {
            throw new ArgumentNullException(nameof(semanticTokensParams.Ranges));
        }

        var (synchronized, csharpDoc) = await TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(
            (int)semanticTokensParams.RequiredHostDocumentVersion,
            semanticTokensParams.TextDocument,
            cancellationToken);

        if (synchronized && csharpDoc.HostDocumentSyncVersion == 1)
        {
            // HACK: Workaround for https://github.com/dotnet/razor/issues/9197 to stop Roslyn NFWs
            // Sometimes we get asked for semantic tokens on v1, and we have sent a v1 to Roslyn, but its the wrong v1.
            // To prevent Roslyn throwing, let's validate the range we're asking about with the generated document they
            // would have seen.
            var lastGeneratedDocumentLine = requestParams switch
            {
                SemanticTokensRangeParams range => range.Range.End.Line,
                SemanticTokensRangesParams ranges => ranges.Ranges[^1].End.Line,
                _ => Assumed.Unreachable<int>()
            };

            if (csharpDoc.Snapshot.LineCount < lastGeneratedDocumentLine)
            {
                // We report this as a fail to synchronize, as that's essentially what it is: We were asked for v1, with X lines
                // and whilst we have v1, we don't have X lines, so we need to wait for a future update to arrive and give us
                // more content.
                return new ProvideSemanticTokensResponse(tokens: null, -1);
            }
        }

        if (csharpDoc is null)
        {
            return null;
        }

        if (!synchronized)
        {
            // If we're unable to synchronize we won't produce useful results, but we have to indicate
            // it's due to out of sync by providing the old version
            return new ProvideSemanticTokensResponse(tokens: null, hostDocumentSyncVersion: csharpDoc.HostDocumentSyncVersion ?? -1);
        }

        semanticTokensParams.TextDocument.Uri = csharpDoc.Uri;
        var textBuffer = csharpDoc.Snapshot.TextBuffer;

        cancellationToken.ThrowIfCancellationRequested();
        var languageServerName = RazorLSPConstants.RazorCSharpLanguageServerName;

        SemanticTokens? response;
        using (var disposable = _telemetryReporter.TrackLspRequest(lspMethodName, languageServerName, semanticTokensParams.CorrelationId))
        {
            var result = await _requestInvoker.ReinvokeRequestOnServerAsync<SemanticTokensParams, SemanticTokens?>(
                textBuffer,
                lspMethodName,
                languageServerName,
                capabilitiesFilter,
                requestParams,
                cancellationToken).ConfigureAwait(false);

            response = result?.Response;
        }

        if (response?.Data is null)
        {
            _logger?.LogDebug($"Made one semantic token request to Roslyn for {semanticTokensParams.Ranges.Length} ranges but got null result back, due to sync issues");
            // Weren't able to re-invoke C# semantic tokens but we have to indicate it's due to out of sync by providing the old version
            return new ProvideSemanticTokensResponse(tokens: null, hostDocumentSyncVersion: csharpDoc.HostDocumentSyncVersion ?? -1);
        }

        _logger?.LogDebug($"Made one semantic token requests to Roslyn for {semanticTokensParams.Ranges.Length} ranges");
        return new ProvideSemanticTokensResponse(response.Data, semanticTokensParams.RequiredHostDocumentVersion);
    }

    private static bool SupportsPreciseRanges(JToken token)
    {
        var serverCapabilities = token.ToObject<VSInternalServerCapabilities>();

        return serverCapabilities?.Experimental is JObject experimental
            && experimental.TryGetValue(RazorLSPConstants.RoslynSemanticTokenRangesEndpointName, out var supportsPreciseRanges)
            && supportsPreciseRanges.ToObject<bool>();
    }
}
