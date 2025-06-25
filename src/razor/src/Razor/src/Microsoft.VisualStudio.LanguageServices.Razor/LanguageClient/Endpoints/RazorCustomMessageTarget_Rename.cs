// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Protocol;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Endpoints;

internal partial class RazorCustomMessageTarget
{
    [JsonRpcMethod(CustomMessageNames.RazorRenameEndpointName, UseSingleObjectParameterDeserialization = true)]
    public async Task<WorkspaceEdit?> RenameAsync(DelegatedRenameParams request, CancellationToken cancellationToken)
    {
        var delegationDetails = await GetProjectedRequestDetailsAsync(request, cancellationToken).ConfigureAwait(false);
        if (delegationDetails is null)
        {
            return null;
        }

        var renameParams = new RenameParams()
        {
            TextDocument = request.Identifier.TextDocumentIdentifier.WithUri(delegationDetails.Value.ProjectedUri),
            Position = request.ProjectedPosition,
            NewName = request.NewName,
        };

        var textBuffer = delegationDetails.Value.TextBuffer;
        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<RenameParams, WorkspaceEdit?>(
            textBuffer,
            Methods.TextDocumentRenameName,
            delegationDetails.Value.LanguageServerName,
            renameParams,
            cancellationToken).ConfigureAwait(false);
        return response?.Response;
    }
}
