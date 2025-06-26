// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Protocol;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Endpoints;

internal partial class RazorCustomMessageTarget
{
    [JsonRpcMethod(CustomMessageNames.RazorInlayHintEndpoint, UseSingleObjectParameterDeserialization = true)]
    public async Task<InlayHint[]?> ProvideInlayHintsAsync(DelegatedInlayHintParams request, CancellationToken cancellationToken)
    {
        var delegationDetails = await GetProjectedRequestDetailsAsync(request, cancellationToken).ConfigureAwait(false);
        if (delegationDetails is null)
        {
            return default;
        }

        var inlayHintParams = new InlayHintParams
        {
            TextDocument = request.Identifier.TextDocumentIdentifier.WithUri(delegationDetails.Value.ProjectedUri),
            Range = request.ProjectedRange
        };

        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<InlayHintParams, InlayHint[]?>(
            delegationDetails.Value.TextBuffer,
            Methods.TextDocumentInlayHintName,
            delegationDetails.Value.LanguageServerName,
            inlayHintParams,
            cancellationToken).ConfigureAwait(false);
        return response?.Response;
    }

    [JsonRpcMethod(CustomMessageNames.RazorInlayHintResolveEndpoint, UseSingleObjectParameterDeserialization = true)]
    public async Task<InlayHint?> ProvideInlayHintsResolveAsync(DelegatedInlayHintResolveParams request, CancellationToken cancellationToken)
    {
        var delegationDetails = await GetProjectedRequestDetailsAsync(request, cancellationToken).ConfigureAwait(false);
        if (delegationDetails is null)
        {
            return default;
        }

        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<InlayHint, InlayHint>(
            delegationDetails.Value.TextBuffer,
            Methods.TextDocumentInlayHintName,
            delegationDetails.Value.LanguageServerName,
            request.InlayHint,
            cancellationToken).ConfigureAwait(false);
        return response?.Response;
    }
}
