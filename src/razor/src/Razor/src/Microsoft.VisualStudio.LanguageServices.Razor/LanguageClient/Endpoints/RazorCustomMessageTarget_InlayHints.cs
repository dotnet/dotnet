// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Razor.LanguageClient.Extensions;
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
