// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Hover;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Cohost;

[Shared]
[LanguageServerEndpoint(Methods.TextDocumentHoverName)]
[ExportRazorStatelessLspService(typeof(CohostHoverEndpoint))]
[Export(typeof(ICapabilitiesProvider))]
[method: ImportingConstructor]
internal sealed class CohostHoverEndpoint(
    IHoverService hoverInfoService,
    IRazorDocumentMappingService documentMappingService,
    IRazorLoggerFactory loggerFactory)
    : AbstractCohostDelegatingEndpoint<TextDocumentPositionParams, VSInternalHover?>(documentMappingService, loggerFactory.CreateLogger<CohostHoverEndpoint>()),
      ICapabilitiesProvider
{
    private readonly IHoverService _hoverInfoService = hoverInfoService;
    private readonly IRazorDocumentMappingService _documentMappingService = documentMappingService;
    private VSInternalClientCapabilities? _clientCapabilities;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        _clientCapabilities = clientCapabilities;
        serverCapabilities.EnableHoverProvider();
    }

    protected override bool PreferCSharpOverHtmlIfPossible => true;

    protected override IDocumentPositionInfoStrategy DocumentPositionInfoStrategy => PreferAttributeNameDocumentPositionInfoStrategy.Instance;

    protected override string CustomMessageTarget => CustomMessageNames.RazorHoverEndpointName;

    protected override bool RequiresLSPSolution => true;

    protected override Task<IDelegatedParams?> CreateDelegatedParamsAsync(TextDocumentPositionParams request, RazorCohostRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.GetRequiredDocumentContext();
        return Task.FromResult<IDelegatedParams?>(new DelegatedPositionParams(
                documentContext.Identifier,
                positionInfo.Position,
                positionInfo.LanguageKind));
    }

    protected override Task<VSInternalHover?> TryHandleAsync(TextDocumentPositionParams request, RazorCohostRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
        => _hoverInfoService.GetRazorHoverInfoAsync(
            requestContext.GetRequiredDocumentContext(),
            positionInfo,
            request.Position,
            _clientCapabilities,
            cancellationToken);

    protected override Task<VSInternalHover?> HandleDelegatedResponseAsync(VSInternalHover? response, TextDocumentPositionParams originalRequest, RazorCohostRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
        => _hoverInfoService.TranslateDelegatedResponseAsync(
            response,
            requestContext.GetRequiredDocumentContext(),
            positionInfo,
            cancellationToken);
}
