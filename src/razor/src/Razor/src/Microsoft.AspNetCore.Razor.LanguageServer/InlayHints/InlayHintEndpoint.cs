// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.Threading;

namespace Microsoft.AspNetCore.Razor.LanguageServer.InlayHints;

[RazorLanguageServerEndpoint(Methods.TextDocumentInlayHintName)]
internal sealed class InlayHintEndpoint(IInlayHintService inlayHintService, IClientConnection clientConnection)
    : IRazorRequestHandler<InlayHintParams, InlayHint[]?>, ICapabilitiesProvider
{
    private readonly IInlayHintService _inlayHintService = inlayHintService;
    private readonly IClientConnection _clientConnection = clientConnection;

    public bool MutatesSolutionState => false;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.EnableInlayHints();
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(InlayHintParams request)
        => request.TextDocument;

    public Task<InlayHint[]?> HandleRequestAsync(InlayHintParams request, RazorRequestContext context, CancellationToken cancellationToken)
    {
        var documentContext = context.DocumentContext;
        if (documentContext is null)
        {
            return SpecializedTasks.Null<InlayHint[]>();
        }

        return _inlayHintService.GetInlayHintsAsync(_clientConnection, documentContext, request.Range, cancellationToken);
    }
}
