// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.Threading;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Hover;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Tooltip;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using LspHover = Microsoft.VisualStudio.LanguageServer.Protocol.Hover;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hover;

[RazorLanguageServerEndpoint(Methods.TextDocumentHoverName)]
internal sealed class HoverEndpoint(
    IComponentAvailabilityService componentAvailabilityService,
    IClientCapabilitiesService clientCapabilitiesService,
    LanguageServerFeatureOptions languageServerFeatureOptions,
    IDocumentMappingService documentMappingService,
    IClientConnection clientConnection,
    ILoggerFactory loggerFactory)
    : AbstractRazorDelegatingEndpoint<TextDocumentPositionParams, LspHover?>(
        languageServerFeatureOptions,
        documentMappingService,
        clientConnection,
        loggerFactory.GetOrCreateLogger<HoverEndpoint>()), ICapabilitiesProvider
{
    private readonly IComponentAvailabilityService _componentAvailabilityService = componentAvailabilityService;
    private readonly IClientCapabilitiesService _clientCapabilitiesService = clientCapabilitiesService;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.EnableHoverProvider();
    }

    protected override bool PreferCSharpOverHtmlIfPossible => true;

    protected override IDocumentPositionInfoStrategy DocumentPositionInfoStrategy => PreferAttributeNameDocumentPositionInfoStrategy.Instance;

    protected override string CustomMessageTarget => CustomMessageNames.RazorHoverEndpointName;

    protected override Task<IDelegatedParams?> CreateDelegatedParamsAsync(TextDocumentPositionParams request, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.DocumentContext;
        if (documentContext is null)
        {
            return SpecializedTasks.Null<IDelegatedParams>();
        }

        return Task.FromResult<IDelegatedParams?>(new DelegatedPositionParams(
            documentContext.GetTextDocumentIdentifierAndVersion(),
            positionInfo.Position,
            positionInfo.LanguageKind));
    }

    protected override async Task<LspHover?> TryHandleAsync(TextDocumentPositionParams request, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.DocumentContext;
        if (documentContext is null)
        {
            return null;
        }

        // HTML can still sometimes be handled by razor. For example hovering over
        // a component tag like <Counter /> will still be in an html context
        if (positionInfo.LanguageKind == RazorLanguageKind.CSharp)
        {
            return null;
        }

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

        // Sometimes what looks like a html attribute can actually map to C#, in which case its better to let Roslyn try to handle this.
        // We can only do this if we're in single server mode though, otherwise we won't be delegating to Roslyn at all
        if (SingleServerSupport && DocumentMappingService.TryMapToGeneratedDocumentPosition(codeDocument.GetCSharpDocument(), positionInfo.HostDocumentIndex, out _, out _))
        {
            return null;
        }

        var options = HoverDisplayOptions.From(_clientCapabilitiesService.ClientCapabilities);

        return await HoverFactory.GetHoverAsync(
            codeDocument,
            positionInfo.HostDocumentIndex,
            options,
            _componentAvailabilityService,
            cancellationToken)
            .ConfigureAwait(false);
    }

    protected override async Task<LspHover?> HandleDelegatedResponseAsync(LspHover? response, TextDocumentPositionParams originalRequest, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.DocumentContext;
        if (documentContext is null)
        {
            return null;
        }

        if (response?.Range is null)
        {
            return response;
        }

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

        // If we don't include the originally requested position in our response, the client may not show it, so we extend the range to ensure it is in there.
        // eg for hovering at @bind-Value:af$$ter, we want to show people the hover for the Value property, so Roslyn will return to us the range for just the
        // portion of the attribute that says "Value".
        if (RazorSyntaxFacts.TryGetFullAttributeNameSpan(codeDocument, positionInfo.HostDocumentIndex, out var originalAttributeRange))
        {
            response.Range = codeDocument.Source.Text.GetRange(originalAttributeRange);
        }
        else if (positionInfo.LanguageKind == RazorLanguageKind.CSharp)
        {
            if (DocumentMappingService.TryMapToHostDocumentRange(codeDocument.GetCSharpDocument(), response.Range, out var projectedRange))
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
