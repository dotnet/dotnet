// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Protocol;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Endpoints;

internal partial class RazorCustomMessageTarget
{
    // These handlers do different jobs, but all take a  DelegatedPositionParams and in turn request a TextDocumentPositionParams

    [JsonRpcMethod(CustomMessageNames.RazorDefinitionEndpointName, UseSingleObjectParameterDeserialization = true)]
    public Task<LspLocation[]?> DefinitionAsync(DelegatedPositionParams request, CancellationToken cancellationToken)
        => DelegateTextDocumentPositionAndProjectContextAsync<LspLocation[]>(request, Methods.TextDocumentDefinitionName, cancellationToken);

    [JsonRpcMethod(CustomMessageNames.RazorDocumentHighlightEndpointName, UseSingleObjectParameterDeserialization = true)]
    public Task<DocumentHighlight[]?> DocumentHighlightAsync(DelegatedPositionParams request, CancellationToken cancellationToken)
        => DelegateTextDocumentPositionAndProjectContextAsync<DocumentHighlight[]>(request, Methods.TextDocumentDocumentHighlightName, cancellationToken);

    [JsonRpcMethod(CustomMessageNames.RazorHoverEndpointName, UseSingleObjectParameterDeserialization = true)]
    public Task<VSInternalHover?> HoverAsync(DelegatedPositionParams request, CancellationToken cancellationToken)
        => DelegateTextDocumentPositionAndProjectContextAsync<VSInternalHover>(request, Methods.TextDocumentHoverName, cancellationToken);

    [JsonRpcMethod(CustomMessageNames.RazorImplementationEndpointName, UseSingleObjectParameterDeserialization = true)]
    public Task<SumType<LspLocation[], VSInternalReferenceItem[]>> ImplementationAsync(DelegatedPositionParams request, CancellationToken cancellationToken)
        => DelegateTextDocumentPositionAndProjectContextAsync<SumType<LspLocation[], VSInternalReferenceItem[]>>(request, Methods.TextDocumentImplementationName, cancellationToken);

    [JsonRpcMethod(CustomMessageNames.RazorSignatureHelpEndpointName, UseSingleObjectParameterDeserialization = true)]
    public Task<LspSignatureHelp?> SignatureHelpAsync(DelegatedPositionParams request, CancellationToken cancellationToken)
        => DelegateTextDocumentPositionAndProjectContextAsync<LspSignatureHelp>(request, Methods.TextDocumentSignatureHelpName, cancellationToken);

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
                DocumentUri = new(delegationDetails.Value.ProjectedUri),
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
