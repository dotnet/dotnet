// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.InlayHints;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Cohost;

[Shared]
[LanguageServerEndpoint(Methods.InlayHintResolveName)]
[ExportRazorStatelessLspService(typeof(CohostInlayHintResolveEndpoint))]
[method: ImportingConstructor]
internal sealed class CohostInlayHintResolveEndpoint(
    IInlayHintService inlayHintService)
    : AbstractRazorCohostRequestHandler<InlayHint, InlayHint?>
{
    private readonly IInlayHintService _inlayHintService = inlayHintService;

    protected override bool MutatesSolutionState => false;
    protected override bool RequiresLSPSolution => true;

    protected override Task<InlayHint?> HandleRequestAsync(InlayHint request, RazorCohostRequestContext context, CancellationToken cancellationToken)
    {
        // TODO: We can't MEF import IRazorCohostClientLanguageServerManager in the constructor. We can make this work
        //       by having it implement a base class, RazorClientConnectionBase or something, that in turn implements
        //       AbstractRazorLspService (defined in Roslyn) and then move everything from importing IClientConnection
        //       to importing the new base class, so we can continue to share services.
        //
        //       Until then we have to get the service from the request context.
        var clientLanguageServerManager = context.GetRequiredService<IRazorCohostClientLanguageServerManager>();
        var clientConnection = new RazorCohostClientConnection(clientLanguageServerManager);

        return _inlayHintService.ResolveInlayHintAsync(clientConnection, request, cancellationToken);
    }
}
