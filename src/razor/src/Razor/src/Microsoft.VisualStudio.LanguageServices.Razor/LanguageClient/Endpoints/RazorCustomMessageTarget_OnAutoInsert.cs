// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Protocol;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Endpoints;

internal partial class RazorCustomMessageTarget
{
    [JsonRpcMethod(CustomMessageNames.RazorOnAutoInsertEndpointName, UseSingleObjectParameterDeserialization = true)]
    public async Task<VSInternalDocumentOnAutoInsertResponseItem?> OnAutoInsertAsync(DelegatedOnAutoInsertParams request, CancellationToken cancellationToken)
    {
        var delegationDetails = await GetProjectedRequestDetailsAsync(request, cancellationToken).ConfigureAwait(false);
        if (delegationDetails is null)
        {
            return default;
        }

        var onAutoInsertParams = new VSInternalDocumentOnAutoInsertParams
        {
            TextDocument = request.Identifier.TextDocumentIdentifier.WithUri(delegationDetails.Value.ProjectedUri),
            Position = request.ProjectedPosition,
            Character = request.Character,
            Options = request.Options
        };

        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<VSInternalDocumentOnAutoInsertParams, VSInternalDocumentOnAutoInsertResponseItem?>(
           delegationDetails.Value.TextBuffer,
           VSInternalMethods.OnAutoInsertName,
           delegationDetails.Value.LanguageServerName,
           onAutoInsertParams,
           cancellationToken).ConfigureAwait(false);
        return response?.Response;
    }
}
