// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Extensions;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Cohost;

[LanguageServerEndpoint(Methods.TextDocumentSemanticTokensRangeName)]
[ExportRazorStatelessLspService(typeof(CohostSemanticTokensRangeEndpoint))]
[Export(typeof(ICapabilitiesProvider))]
[method: ImportingConstructor]
internal sealed class CohostSemanticTokensRangeEndpoint(
    IRazorSemanticTokensInfoService semanticTokensInfoService,
    IClientSettingsManager clientSettingsManager,
    IDocumentContextFactory documentContextFactory,
    IRazorLoggerFactory loggerFactory)
    : AbstractRazorCohostDocumentRequestHandler<SemanticTokensRangeParams, SemanticTokens?>, ICapabilitiesProvider
{
    private readonly IRazorSemanticTokensInfoService _semanticTokensInfoService = semanticTokensInfoService;
    private readonly IClientSettingsManager _clientSettingsManager = clientSettingsManager;
    private readonly IDocumentContextFactory _documentContextFactory = documentContextFactory;
    private readonly ILogger _logger = loggerFactory.CreateLogger<CohostSemanticTokensRangeEndpoint>();

    protected override bool MutatesSolutionState => false;
    protected override bool RequiresLSPSolution => true;

    protected override RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(SemanticTokensRangeParams request)
        => request.TextDocument.ToRazorTextDocumentIdentifier();

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
        => _semanticTokensInfoService.ApplyCapabilities(serverCapabilities, clientCapabilities);

    protected override Task<SemanticTokens?> HandleRequestAsync(SemanticTokensRangeParams request, RazorCohostRequestContext context, CancellationToken cancellationToken)
    {
        // TODO: Create document context from request.TextDocument, by looking at request.Solution instead of our project snapshots
        var documentContext = _documentContextFactory.TryCreateForOpenDocument(request.TextDocument);

        _logger.LogDebug("[Cohost] Received semantic range request for {requestPath} and got document {documentPath}", request.TextDocument.Uri, documentContext?.FilePath);

        // TODO: We can't MEF import IRazorCohostClientLanguageServerManager in the constructor. We can make this work
        //       by having it implement a base class, RazorClientConnectionBase or something, that in turn implements
        //       AbstractRazorLspService (defined in Roslyn) and then move everything from importing IClientConnection
        //       to importing the new base class, so we can continue to share services.
        //
        //       Until then we have to get the service from the request context.
        var clientLanguageServerManager = context.GetRequiredService<IRazorCohostClientLanguageServerManager>();
        var clientConnection = new RazorCohostClientConnection(clientLanguageServerManager);

        // TODO: This is currently using the "VS" client settings manager, since that's where we are running. In future
        //       we should create a hook into Roslyn's LSP options infra so we get the option values from the LSP client
        var colorBackground = _clientSettingsManager.GetClientSettings().AdvancedSettings.ColorBackground;

        return _semanticTokensInfoService.GetSemanticTokensAsync(clientConnection, request.TextDocument, request.Range, documentContext.AssumeNotNull(), colorBackground, cancellationToken);
    }
}
