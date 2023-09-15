// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic.Models;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

internal partial class RazorCustomMessageTarget
{
    // Called by the Razor Language Server to provide ranged semantic tokens from the platform.
    [JsonRpcMethod(CustomMessageNames.RazorProvideSemanticTokensRangeEndpoint, UseSingleObjectParameterDeserialization = true)]
    public async Task<ProvideSemanticTokensResponse?> ProvideSemanticTokensAsync(
        ProvideSemanticTokensRangesParams semanticTokensParams,
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

        using var _ = ListPool<Task<ReinvocationResponse<VSSemanticTokensResponse>?>>.GetPooledObject(out var requestTasks);

        semanticTokensParams.TextDocument.Uri = csharpDoc.Uri;
        var textBuffer = csharpDoc.Snapshot.TextBuffer;

        cancellationToken.ThrowIfCancellationRequested();

        foreach (var range in semanticTokensParams.Ranges)
        {
            var task = Task.Run(async () =>
            {
                var newParams = new SemanticTokensRangeParams
                {
                    TextDocument = semanticTokensParams.TextDocument,
                    Range = range,
                };

                var languageServerName = RazorLSPConstants.RazorCSharpLanguageServerName;
                var lspMethodName = Methods.TextDocumentSemanticTokensRangeName;
                using (var disposable = _telemetryReporter.TrackLspRequest(lspMethodName, languageServerName, semanticTokensParams.CorrelationId))
                {
                    return await _requestInvoker.ReinvokeRequestOnServerAsync<SemanticTokensRangeParams, VSSemanticTokensResponse>(
                        textBuffer,
                        lspMethodName,
                        languageServerName,
                        newParams,
                        cancellationToken);
                }
            }, cancellationToken);

            requestTasks.Add(task);
        }

        var results = await Task.WhenAll(requestTasks).ConfigureAwait(false);
        var nonEmptyResults = results.Select(r => r?.Response?.Data).WithoutNull().ToArray();
        if (nonEmptyResults.Length != semanticTokensParams.Ranges.Length)
        {
            _logger?.LogDebug("Made {count} semantic tokens requests to Roslyn but only got {nonEmpty} results back", semanticTokensParams.Ranges.Length, nonEmptyResults.Length);
            // Weren't able to re-invoke C# semantic tokens but we have to indicate it's due to out of sync by providing the old version
            return new ProvideSemanticTokensResponse(tokens: null, hostDocumentSyncVersion: csharpDoc.HostDocumentSyncVersion ?? -1);
        }

        _logger?.LogDebug("Made {count} semantic tokens requests to Roslyn", semanticTokensParams.Ranges.Length);
        return new ProvideSemanticTokensResponse(nonEmptyResults, semanticTokensParams.RequiredHostDocumentVersion);
    }
}
