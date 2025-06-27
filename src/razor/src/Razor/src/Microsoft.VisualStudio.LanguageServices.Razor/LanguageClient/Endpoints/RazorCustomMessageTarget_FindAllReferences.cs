// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Protocol;
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
                DocumentUri = new(delegationDetails.Value.ProjectedUri),
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
