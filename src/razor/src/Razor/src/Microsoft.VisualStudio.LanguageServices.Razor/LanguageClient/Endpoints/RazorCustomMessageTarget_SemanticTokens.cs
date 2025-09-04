// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor.Workspaces.Protocol.SemanticTokens;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Endpoints;

internal partial class RazorCustomMessageTarget
{
    [JsonRpcMethod(CustomMessageNames.RazorProvidePreciseRangeSemanticTokensEndpoint, UseSingleObjectParameterDeserialization = true)]
    public async Task<ProvideSemanticTokensResponse?> ProvidePreciseRangeSemanticTokensAsync(
        ProvideSemanticTokensRangesParams semanticTokensParams,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Semantic tokens request for {semanticTokensParams.Ranges.Max(static r => r.End.Line)} max line number, host version {semanticTokensParams.RequiredHostDocumentVersion}, correlation ID {semanticTokensParams.CorrelationId}");

        var lspMethodName = RazorLSPConstants.RoslynSemanticTokenRangesEndpointName;
        var requestParams = new SemanticTokensRangesParams()
        {
            TextDocument = semanticTokensParams.TextDocument,
            Ranges = semanticTokensParams.Ranges
        };

        var (synchronized, csharpDoc) = await TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(
            semanticTokensParams.RequiredHostDocumentVersion,
            semanticTokensParams.TextDocument,
            cancellationToken);

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

        requestParams.TextDocument.DocumentUri = new(csharpDoc.Uri);
        var textBuffer = csharpDoc.Snapshot.TextBuffer;

        _logger.LogDebug($"Requesting semantic tokens for {csharpDoc.Uri}, for buffer version {textBuffer.CurrentSnapshot.Version.VersionNumber} and snapshot version {csharpDoc.Snapshot.Version.VersionNumber}, host version {semanticTokensParams.RequiredHostDocumentVersion}, correlation ID {semanticTokensParams.CorrelationId}");

        cancellationToken.ThrowIfCancellationRequested();
        var languageServerName = RazorLSPConstants.RazorCSharpLanguageServerName;

        SemanticTokens? response;
        using (var disposable = _telemetryReporter.TrackLspRequest(lspMethodName, languageServerName, TelemetryThresholds.SemanticTokensSubLSPTelemetryThreshold, semanticTokensParams.CorrelationId))
        {
            try
            {
                var result = await _requestInvoker.ReinvokeRequestOnServerAsync<SemanticTokensRangeParams, SemanticTokens?>(
                    textBuffer,
                    lspMethodName,
                    languageServerName,
                    requestParams,
                    cancellationToken).ConfigureAwait(false);

                response = result?.Response;
            }
            catch
            {
                _logger.LogWarning($"Error getting semantic tokens from Roslyn for host version {semanticTokensParams.RequiredHostDocumentVersion}, correlation ID {semanticTokensParams.CorrelationId}");
                throw;
            }
        }

        if (response?.Data is null)
        {
            _logger.LogDebug($"Made one semantic token request to Roslyn for {semanticTokensParams.Ranges.Length} ranges but got null result back, due to sync issues");
            // Weren't able to re-invoke C# semantic tokens but we have to indicate it's due to out of sync by providing the old version
            return new ProvideSemanticTokensResponse(tokens: null, hostDocumentSyncVersion: csharpDoc.HostDocumentSyncVersion ?? -1);
        }

        _logger.LogDebug($"Made one semantic token requests to Roslyn for {semanticTokensParams.Ranges.Length} ranges");
        return new ProvideSemanticTokensResponse(response.Data, semanticTokensParams.RequiredHostDocumentVersion);
    }
}
