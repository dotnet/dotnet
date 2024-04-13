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
