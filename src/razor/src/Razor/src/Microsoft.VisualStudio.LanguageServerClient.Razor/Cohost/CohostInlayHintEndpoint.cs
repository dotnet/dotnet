// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.InlayHints;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Extensions;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Cohost;

[Shared]
[LanguageServerEndpoint(Methods.TextDocumentInlayHintName)]
[ExportRazorStatelessLspService(typeof(CohostInlayHintEndpoint))]
[Export(typeof(ICapabilitiesProvider))]
[method: ImportingConstructor]
internal sealed class CohostInlayHintEndpoint(
    IInlayHintService inlayHintService,
    IRazorLoggerFactory loggerFactory)
    : AbstractRazorCohostDocumentRequestHandler<InlayHintParams, InlayHint[]?>, ICapabilitiesProvider
{
    private readonly IInlayHintService _inlayHintService = inlayHintService;
    private readonly ILogger _logger = loggerFactory.CreateLogger<CohostInlayHintEndpoint>();

    protected override bool MutatesSolutionState => false;
    protected override bool RequiresLSPSolution => true;

    protected override RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(InlayHintParams request)
        => request.TextDocument.ToRazorTextDocumentIdentifier();

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
        => serverCapabilities.EnableInlayHints();

    protected override Task<InlayHint[]?> HandleRequestAsync(InlayHintParams request, RazorCohostRequestContext context, CancellationToken cancellationToken)
    {
        var documentContext = context.GetRequiredDocumentContext();

        _logger.LogDebug("[Cohost] Received inlay hint request for {requestPath} and got document {documentPath}", request.TextDocument.Uri, documentContext.FilePath);

        // TODO: We can't MEF import IRazorCohostClientLanguageServerManager in the constructor. We can make this work
        //       by having it implement a base class, RazorClientConnectionBase or something, that in turn implements
        //       AbstractRazorLspService (defined in Roslyn) and then move everything from importing IClientConnection
        //       to importing the new base class, so we can continue to share services.
        //
        //       Until then we have to get the service from the request context.
        var clientLanguageServerManager = context.GetRequiredService<IRazorCohostClientLanguageServerManager>();
        var clientConnection = new RazorCohostClientConnection(clientLanguageServerManager);

        return _inlayHintService.GetInlayHintsAsync(clientConnection, documentContext, request.Range, cancellationToken);
    }
}
