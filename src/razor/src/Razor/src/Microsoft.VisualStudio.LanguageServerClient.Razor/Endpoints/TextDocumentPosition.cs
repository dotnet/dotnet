// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

internal partial class RazorCustomMessageTarget
{
    // These handlers do different jobs, but all take a  DelegatedPositionParams and in turn request a TextDocumentPositionParams

    [JsonRpcMethod(CustomMessageNames.RazorDefinitionEndpointName, UseSingleObjectParameterDeserialization = true)]
    public Task<Location[]?> DefinitionAsync(DelegatedPositionParams request, CancellationToken cancellationToken)
        => DelegateTextDocumentPositionAndProjectContextAsync<Location[]>(request, Methods.TextDocumentDefinitionName, cancellationToken);

    [JsonRpcMethod(CustomMessageNames.RazorDocumentHighlightEndpointName, UseSingleObjectParameterDeserialization = true)]
    public Task<DocumentHighlight[]?> DocumentHighlightAsync(DelegatedPositionParams request, CancellationToken cancellationToken)
        => DelegateTextDocumentPositionAndProjectContextAsync<DocumentHighlight[]>(request, Methods.TextDocumentDocumentHighlightName, cancellationToken);

    [JsonRpcMethod(CustomMessageNames.RazorHoverEndpointName, UseSingleObjectParameterDeserialization = true)]
    public Task<VSInternalHover?> HoverAsync(DelegatedPositionParams request, CancellationToken cancellationToken)
        => DelegateTextDocumentPositionAndProjectContextAsync<VSInternalHover>(request, Methods.TextDocumentHoverName, cancellationToken);

    [JsonRpcMethod(CustomMessageNames.RazorImplementationEndpointName, UseSingleObjectParameterDeserialization = true)]
    public Task<SumType<Location[], VSInternalReferenceItem[]>> ImplementationAsync(DelegatedPositionParams request, CancellationToken cancellationToken)
        => DelegateTextDocumentPositionAndProjectContextAsync<SumType<Location[], VSInternalReferenceItem[]>>(request, Methods.TextDocumentImplementationName, cancellationToken);

    [JsonRpcMethod(CustomMessageNames.RazorReferencesEndpointName, UseSingleObjectParameterDeserialization = true)]
    public Task<VSInternalReferenceItem[]?> ReferencesAsync(DelegatedPositionParams request, CancellationToken cancellationToken)
        => DelegateTextDocumentPositionAndProjectContextAsync<VSInternalReferenceItem[]>(request, Methods.TextDocumentReferencesName, cancellationToken);

    [JsonRpcMethod(CustomMessageNames.RazorSignatureHelpEndpointName, UseSingleObjectParameterDeserialization = true)]
    public Task<SignatureHelp?> SignatureHelpAsync(DelegatedPositionParams request, CancellationToken cancellationToken)
        => DelegateTextDocumentPositionAndProjectContextAsync<SignatureHelp>(request, Methods.TextDocumentSignatureHelpName, cancellationToken);

    private async Task<TResult?> DelegateTextDocumentPositionAndProjectContextAsync<TResult>(DelegatedPositionParams request, string methodName, CancellationToken cancellationToken)
    {
        var delegationDetails = await GetProjectedRequestDetailsAsync(request, cancellationToken).ConfigureAwait(false);
        if (delegationDetails is null)
        {
            return default;
        }

        var positionParams = new TextDocumentPositionParams()
        {
            TextDocument = new VSTextDocumentIdentifier()
            {
                Uri = delegationDetails.Value.ProjectedUri,
                ProjectContext = null,
            },
            Position = request.ProjectedPosition,
        };

        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<TextDocumentPositionParams, TResult?>(
            delegationDetails.Value.TextBuffer,
            methodName,
            delegationDetails.Value.LanguageServerName,
            positionParams,
            cancellationToken).ConfigureAwait(false);

        if (response is null)
        {
            return default;
        }

        return response.Response;
    }
}
