// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.InlayHints;

[LanguageServerEndpoint(Methods.InlayHintResolveName)]
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
