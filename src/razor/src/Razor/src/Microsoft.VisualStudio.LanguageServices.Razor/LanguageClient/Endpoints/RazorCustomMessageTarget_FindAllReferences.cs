// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Endpoints;

internal partial class RazorCustomMessageTarget
{
    [JsonRpcMethod(CustomMessageNames.RazorReferencesEndpointName, UseSingleObjectParameterDeserialization = true)]
    public async Task<VSInternalReferenceItem[]?> ReferencesAsync(DelegatedPositionParams request, CancellationToken cancellationToken)
    {
        var delegationDetails = await GetProjectedRequestDetailsAsync(request, cancellationToken).ConfigureAwait(false);
        if (delegationDetails is null)
        {
            return default;
        }

        var referenceParams = new ReferenceParams()
        {
            TextDocument = new VSTextDocumentIdentifier()
            {
                Uri = delegationDetails.Value.ProjectedUri,
                ProjectContext = null,
            },
            Position = request.ProjectedPosition,
            Context = new ReferenceContext(),
        };

        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<ReferenceParams, VSInternalReferenceItem[]?>(
            delegationDetails.Value.TextBuffer,
            Methods.TextDocumentReferencesName,
            delegationDetails.Value.LanguageServerName,
            referenceParams,
            cancellationToken).ConfigureAwait(false);

        if (response is null)
        {
            return default;
        }

        return response.Response;
    }
}
