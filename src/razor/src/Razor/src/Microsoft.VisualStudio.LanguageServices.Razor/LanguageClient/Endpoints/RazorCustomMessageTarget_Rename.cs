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
