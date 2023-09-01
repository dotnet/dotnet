// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic.Models;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

internal partial class RazorCustomMessageTarget
{
    // Called by the Razor Language Server to provide ranged semantic tokens from the platform.
    [JsonRpcMethod(CustomMessageNames.RazorProvideSemanticTokensRangeEndpoint, UseSingleObjectParameterDeserialization = true)]
    public async Task<ProvideSemanticTokensResponse?> ProvideSemanticTokensRangeAsync(
        ProvideSemanticTokensRangeParams semanticTokensParams,
        CancellationToken cancellationToken)
    {
        if (semanticTokensParams is null)
        {
            throw new ArgumentNullException(nameof(semanticTokensParams));
        }

        if (semanticTokensParams.Range is null)
        {
            throw new ArgumentNullException(nameof(semanticTokensParams.Range));
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
            return new ProvideSemanticTokensResponse(tokens: null, hostDocumentSyncVersion: -1);
        }

        semanticTokensParams.TextDocument.Uri = csharpDoc.Uri;

        var newParams = new SemanticTokensRangeParams
        {
            TextDocument = semanticTokensParams.TextDocument,
            PartialResultToken = semanticTokensParams.PartialResultToken,
            Range = semanticTokensParams.Range,
        };

        var textBuffer = csharpDoc.Snapshot.TextBuffer;
        var languageServerName = RazorLSPConstants.RazorCSharpLanguageServerName;
        var lspMethodName = Methods.TextDocumentSemanticTokensRangeName;
        using var _ = _telemetryReporter.TrackLspRequest(lspMethodName, languageServerName, semanticTokensParams.CorrelationId);
        var csharpResults = await _requestInvoker.ReinvokeRequestOnServerAsync<SemanticTokensRangeParams, VSSemanticTokensResponse>(
            textBuffer,
            lspMethodName,
            languageServerName,
            newParams,
            cancellationToken).ConfigureAwait(false);

        var result = csharpResults?.Response;
        if (result is null)
        {
            // Weren't able to re-invoke C# semantic tokens but we have to indicate it's due to out of sync by providing the old version
            return new ProvideSemanticTokensResponse(tokens: null, hostDocumentSyncVersion: csharpDoc.HostDocumentSyncVersion);
        }

        var response = new ProvideSemanticTokensResponse(result.Data, semanticTokensParams.RequiredHostDocumentVersion);

        return response;
    }
}
