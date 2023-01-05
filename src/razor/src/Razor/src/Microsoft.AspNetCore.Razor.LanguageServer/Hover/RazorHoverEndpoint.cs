// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hover;

internal class RazorHoverEndpoint : AbstractRazorDelegatingEndpoint<TextDocumentPositionParamsBridge, VSInternalHover?>, IVSHoverEndpoint
{
    private readonly RazorHoverInfoService _hoverInfoService;
    private readonly RazorDocumentMappingService _documentMappingService;
    private VSInternalClientCapabilities? _clientCapabilities;

    public RazorHoverEndpoint(
        RazorHoverInfoService hoverInfoService,
        LanguageServerFeatureOptions languageServerFeatureOptions,
        RazorDocumentMappingService documentMappingService,
        ClientNotifierServiceBase languageServer,
        ILoggerFactory loggerFactory)
        : base(languageServerFeatureOptions, documentMappingService, languageServer, loggerFactory.CreateLogger<RazorHoverEndpoint>())
    {
        _hoverInfoService = hoverInfoService ?? throw new ArgumentNullException(nameof(hoverInfoService));
        _documentMappingService = documentMappingService ?? throw new ArgumentNullException(nameof(documentMappingService));
    }

    public RegistrationExtensionResult GetRegistration(VSInternalClientCapabilities clientCapabilities)
    {
        const string AssociatedServerCapability = "hoverProvider";
        _clientCapabilities = clientCapabilities;

        var registrationOptions = new HoverOptions()
        {
            WorkDoneProgress = false,
        };

        return new RegistrationExtensionResult(AssociatedServerCapability, new SumType<bool, HoverOptions>(registrationOptions));
    }

    protected override bool PreferCSharpOverHtmlIfPossible => true;

    protected override string CustomMessageTarget => RazorLanguageServerCustomMessageTargets.RazorHoverEndpointName;

    protected override Task<IDelegatedParams?> CreateDelegatedParamsAsync(TextDocumentPositionParamsBridge request, RazorRequestContext requestContext, Projection projection, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.GetRequiredDocumentContext();
        return Task.FromResult<IDelegatedParams?>(new DelegatedPositionParams(
                documentContext.Identifier,
                projection.Position,
                projection.LanguageKind));
    }

    protected override async Task<VSInternalHover?> TryHandleAsync(TextDocumentPositionParamsBridge request, RazorRequestContext requestContext, Projection projection, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.GetRequiredDocumentContext();
        // HTML can still sometimes be handled by razor. For example hovering over
        // a component tag like <Counter /> will still be in an html context
        if (projection.LanguageKind == RazorLanguageKind.CSharp)
        {
            return null;
        }

        // Sometimes what looks like a html attribute can actually map to C#, in which case its better to let Roslyn try to handle this.
        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        if (_documentMappingService.TryMapToProjectedDocumentPosition(codeDocument, projection.AbsoluteIndex, out _, out _))
        {
            return null;
        }

        var location = new SourceLocation(projection.AbsoluteIndex, request.Position.Line, request.Position.Character);
        return _hoverInfoService.GetHoverInfo(codeDocument, location, _clientCapabilities!);
    }

    protected override async Task<VSInternalHover?> HandleDelegatedResponseAsync(VSInternalHover? response, TextDocumentPositionParamsBridge originalRequest, RazorRequestContext requestContext, Projection projection, CancellationToken cancellationToken)
    {
        if (response?.Range is null)
        {
            return response;
        }

        var documentContext = requestContext.GetRequiredDocumentContext();
        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

        if (_documentMappingService.TryMapFromProjectedDocumentRange(codeDocument, response.Range, out var projectedRange))
        {
            response.Range = projectedRange;
        }

        return response;
    }
}
