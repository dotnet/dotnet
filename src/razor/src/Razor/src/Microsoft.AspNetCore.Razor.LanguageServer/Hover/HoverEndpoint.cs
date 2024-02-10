// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hover;

[RazorLanguageServerEndpoint(Methods.TextDocumentHoverName)]
internal sealed class HoverEndpoint : AbstractRazorDelegatingEndpoint<TextDocumentPositionParams, VSInternalHover?>, ICapabilitiesProvider
{
    private readonly IHoverService _hoverService;
    private VSInternalClientCapabilities? _clientCapabilities;

    public HoverEndpoint(
        IHoverService hoverService,
        LanguageServerFeatureOptions languageServerFeatureOptions,
        IRazorDocumentMappingService documentMappingService,
        IClientConnection clientConnection,
        IRazorLoggerFactory loggerFactory)
        : base(languageServerFeatureOptions, documentMappingService, clientConnection, loggerFactory.CreateLogger<HoverEndpoint>())
    {
        _hoverService = hoverService ?? throw new ArgumentNullException(nameof(hoverService));
        Debug.Assert(languageServerFeatureOptions.UseRazorCohostServer == false, "This endpoint should not be registered when using the Razor Cohost server.");
    }

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        _clientCapabilities = clientCapabilities;
        serverCapabilities.EnableHoverProvider();
    }

    protected override bool PreferCSharpOverHtmlIfPossible => true;

    protected override IDocumentPositionInfoStrategy DocumentPositionInfoStrategy => PreferAttributeNameDocumentPositionInfoStrategy.Instance;

    protected override string CustomMessageTarget => CustomMessageNames.RazorHoverEndpointName;

    protected override Task<IDelegatedParams?> CreateDelegatedParamsAsync(TextDocumentPositionParams request, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.GetRequiredDocumentContext();
        return Task.FromResult<IDelegatedParams?>(new DelegatedPositionParams(
                documentContext.Identifier,
                positionInfo.Position,
                positionInfo.LanguageKind));
    }

    protected override Task<VSInternalHover?> TryHandleAsync(TextDocumentPositionParams request, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
        => _hoverService.GetRazorHoverInfoAsync(
            requestContext.GetRequiredDocumentContext(),
            positionInfo,
            request.Position,
            _clientCapabilities,
            cancellationToken);

    protected override Task<VSInternalHover?> HandleDelegatedResponseAsync(VSInternalHover? response, TextDocumentPositionParams originalRequest, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
        => _hoverService.TranslateDelegatedResponseAsync(
            response,
            requestContext.GetRequiredDocumentContext(),
            positionInfo,
            cancellationToken);
}
