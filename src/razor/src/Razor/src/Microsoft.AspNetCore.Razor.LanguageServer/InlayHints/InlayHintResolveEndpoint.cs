// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;

namespace Microsoft.AspNetCore.Razor.LanguageServer.InlayHints;

[RazorLanguageServerEndpoint(Methods.InlayHintResolveName)]
internal sealed class InlayHintResolveEndpoint(IInlayHintService inlayHintService, IClientConnection clientConnection)
    : IRazorDocumentlessRequestHandler<InlayHint, InlayHint?>
{
    private readonly IInlayHintService _inlayHintService = inlayHintService;
    private readonly IClientConnection _clientConnection = clientConnection;

    public bool MutatesSolutionState => false;

    public Task<InlayHint?> HandleRequestAsync(InlayHint request, RazorRequestContext context, CancellationToken cancellationToken)
    {
        return _inlayHintService.ResolveInlayHintAsync(_clientConnection, request, cancellationToken);
    }
}
