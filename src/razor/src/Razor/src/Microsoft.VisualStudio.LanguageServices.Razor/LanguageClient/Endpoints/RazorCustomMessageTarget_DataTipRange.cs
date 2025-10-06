// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Protocol;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Endpoints;

internal partial class RazorCustomMessageTarget
{
    [JsonRpcMethod(CustomMessageNames.RazorDataTipRangeName, UseSingleObjectParameterDeserialization = true)]
    public async Task<VSInternalDataTip?> DataTipRangeAsync(DelegatedPositionParams request, CancellationToken cancellationToken)
    {
        var delegationDetails = await GetProjectedRequestDetailsAsync(request, cancellationToken).ConfigureAwait(false);
        if (delegationDetails is null)
        {
            return default;
        }

        var dataTipRangeParams = new TextDocumentPositionParams
        {
            TextDocument = request.Identifier.TextDocumentIdentifier.WithUri(delegationDetails.Value.ProjectedUri),
            Position = request.ProjectedPosition,
        };

        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<TextDocumentPositionParams, VSInternalDataTip?>(
            delegationDetails.Value.TextBuffer,
            VSInternalMethods.TextDocumentDataTipRangeName,
            delegationDetails.Value.LanguageServerName,
            dataTipRangeParams,
            cancellationToken).ConfigureAwait(false);

        return response?.Response;
    }
}
