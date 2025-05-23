// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
