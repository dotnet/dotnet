// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hover;

[LanguageServerEndpoint(Methods.TextDocumentHoverName)]
internal sealed class HoverEndpoint : AbstractRazorDelegatingEndpoint<TextDocumentPositionParams, VSInternalHover?>, ICapabilitiesProvider
{
    private readonly IHoverInfoService _hoverInfoService;
    private readonly IRazorDocumentMappingService _documentMappingService;
    private VSInternalClientCapabilities? _clientCapabilities;

    public HoverEndpoint(
        IHoverInfoService hoverInfoService,
        LanguageServerFeatureOptions languageServerFeatureOptions,
        IRazorDocumentMappingService documentMappingService,
        ClientNotifierServiceBase languageServer,
        ILoggerFactory loggerFactory)
        : base(languageServerFeatureOptions, documentMappingService, languageServer, loggerFactory.CreateLogger<HoverEndpoint>())
    {
        _hoverInfoService = hoverInfoService ?? throw new ArgumentNullException(nameof(hoverInfoService));
        _documentMappingService = documentMappingService ?? throw new ArgumentNullException(nameof(documentMappingService));
    }

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        _clientCapabilities = clientCapabilities;

        serverCapabilities.HoverProvider = new HoverOptions()
        {
            WorkDoneProgress = false,
        };
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

    protected override async Task<VSInternalHover?> TryHandleAsync(TextDocumentPositionParams request, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.GetRequiredDocumentContext();
        // HTML can still sometimes be handled by razor. For example hovering over
        // a component tag like <Counter /> will still be in an html context
        if (positionInfo.LanguageKind == RazorLanguageKind.CSharp)
        {
            return null;
        }

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

        // Sometimes what looks like a html attribute can actually map to C#, in which case its better to let Roslyn try to handle this.
        // We can only do this if we're in single server mode though, otherwise we won't be delegating to Roslyn at all
        if (SingleServerSupport && _documentMappingService.TryMapToGeneratedDocumentPosition(codeDocument.GetCSharpDocument(), positionInfo.HostDocumentIndex, out _, out _))
        {
            return null;
        }

        var location = new SourceLocation(positionInfo.HostDocumentIndex, request.Position.Line, request.Position.Character);
        return _hoverInfoService.GetHoverInfo(codeDocument, location, _clientCapabilities!);
    }

    protected override async Task<VSInternalHover?> HandleDelegatedResponseAsync(VSInternalHover? response, TextDocumentPositionParams originalRequest, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
    {
        if (response?.Range is null)
        {
            return response;
        }

        var documentContext = requestContext.GetRequiredDocumentContext();
        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

        // If we don't include the originally requested position in our response, the client may not show it, so we extend the range to ensure it is in there.
        // eg for hovering at @bind-Value:af$$ter, we want to show people the hover for the Value property, so Roslyn will return to us the range for just the
        // portion of the attribute that says "Value".
        if (RazorSyntaxFacts.TryGetFullAttributeNameSpan(codeDocument, positionInfo.HostDocumentIndex, out var originalAttributeRange))
        {
            var sourceText = await documentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
            response.Range = originalAttributeRange.ToRange(sourceText);
        }
        else if (positionInfo.LanguageKind == RazorLanguageKind.CSharp)
        {
            if (_documentMappingService.TryMapToHostDocumentRange(codeDocument.GetCSharpDocument(), response.Range, out var projectedRange))
            {
                response.Range = projectedRange;
            }
            else
            {
                // We couldn't remap the range back from Roslyn, but we have to do something with it, because it definitely won't
                // be correct, and if the Razor document is small, will be completely outside the valid range for the file, which
                // would cause the client to error.
                // Returning null here will still show the hover, just there won't be any extra visual indication, like
                // a background color, applied by the client.
                response.Range = null;
            }
        }

        return response;
    }
}
