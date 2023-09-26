// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic.Models;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json.Linq;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

internal partial class RazorCustomMessageTarget
{
    // Called by the Razor Language Server to provide ranged semantic tokens from the platform.
    [JsonRpcMethod(CustomMessageNames.RazorProvideSemanticTokensRangeEndpoint, UseSingleObjectParameterDeserialization = true)]
    public async Task<ProvideSemanticTokensResponse?> ProvideMinimalRangeSemanticTokensAsync(ProvideSemanticTokensRangesParams inputParams, CancellationToken cancellationToken)
    {
        Debug.Assert(inputParams.Ranges.Length == 1);

        return await ProvideSemanticTokensAsync(
            semanticTokensParams: inputParams,
            lspMethodName: Methods.TextDocumentSemanticTokensRangeName,
            capabilitiesFilter: _ => true,
            requestParams: new SemanticTokensRangeParams
            {
                TextDocument = inputParams.TextDocument,
                Range = inputParams.Ranges[0],
            },
            cancellationToken).ConfigureAwait(false);
    }

    [JsonRpcMethod(CustomMessageNames.RazorProvidePreciseRangeSemanticTokensEndpoint, UseSingleObjectParameterDeserialization = true)]
    public async Task<ProvideSemanticTokensResponse?> ProvidePreciseRangeSemanticTokensAsync(ProvideSemanticTokensRangesParams inputParams, CancellationToken cancellationToken)
    {
        return await ProvideSemanticTokensAsync(
            semanticTokensParams: inputParams,
            lspMethodName: RazorLSPConstants.RoslynSemanticTokenRangesEndpointName,
            capabilitiesFilter: SupportsPreciseRanges,
            requestParams: new SemanticTokensRangesParams()
            {
                TextDocument = inputParams.TextDocument,
                Ranges = inputParams.Ranges
            },
            cancellationToken).ConfigureAwait(false);
    }

    public async Task<ProvideSemanticTokensResponse?> ProvideSemanticTokensAsync(
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

        var (synchronized, csharpDoc) = await TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>((int)semanticTokensParams.RequiredHostDocumentVersion, semanticTokensParams.TextDocument, cancellationToken);

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

        VSSemanticTokensResponse? response;
        using (var disposable = _telemetryReporter.TrackLspRequest(lspMethodName, languageServerName, semanticTokensParams.CorrelationId))
        {
            var result = await _requestInvoker.ReinvokeRequestOnServerAsync<SemanticTokensParams, VSSemanticTokensResponse?>(
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
            _logger?.LogDebug("Made one semantic token request to Roslyn for {count} ranges but got null result back, due to sync issues", semanticTokensParams.Ranges.Length);
            // Weren't able to re-invoke C# semantic tokens but we have to indicate it's due to out of sync by providing the old version
            return new ProvideSemanticTokensResponse(tokens: null, hostDocumentSyncVersion: csharpDoc.HostDocumentSyncVersion ?? -1);
        }

        _logger?.LogDebug("Made one semantic token requests to Roslyn for {count} ranges", semanticTokensParams.Ranges.Length);
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
