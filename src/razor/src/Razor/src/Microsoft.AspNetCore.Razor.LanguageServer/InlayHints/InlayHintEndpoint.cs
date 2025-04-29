// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.Threading;
using Microsoft.VisualStudio.LanguageServer.Protocol;

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
