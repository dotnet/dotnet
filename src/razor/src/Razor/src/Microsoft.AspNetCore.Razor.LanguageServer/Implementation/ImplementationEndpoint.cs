// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.Threading;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using ImplementationResult = Roslyn.LanguageServer.Protocol.SumType<
    Roslyn.LanguageServer.Protocol.Location[],
    Roslyn.LanguageServer.Protocol.VSInternalReferenceItem[]>;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Implementation;

[RazorLanguageServerEndpoint(Methods.TextDocumentImplementationName)]
internal sealed class ImplementationEndpoint : AbstractRazorDelegatingEndpoint<TextDocumentPositionParams, ImplementationResult>, ICapabilitiesProvider
{
    private readonly IDocumentMappingService _documentMappingService;

    public ImplementationEndpoint(
        LanguageServerFeatureOptions languageServerFeatureOptions,
        IDocumentMappingService documentMappingService,
        IClientConnection clientConnection,
        ILoggerFactory loggerFactory)
        : base(languageServerFeatureOptions, documentMappingService, clientConnection, loggerFactory.GetOrCreateLogger<ImplementationEndpoint>())
    {
        _documentMappingService = documentMappingService ?? throw new ArgumentNullException(nameof(documentMappingService));
    }

    protected override string CustomMessageTarget => CustomMessageNames.RazorImplementationEndpointName;

    protected override bool PreferCSharpOverHtmlIfPossible => true;

    protected override IDocumentPositionInfoStrategy DocumentPositionInfoStrategy => PreferAttributeNameDocumentPositionInfoStrategy.Instance;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.ImplementationProvider = new ImplementationOptions();
    }

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

    protected async override Task<ImplementationResult> HandleDelegatedResponseAsync(ImplementationResult delegatedResponse, TextDocumentPositionParams request, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
    {
        var result = delegatedResponse.Value;

        // Not using .TryGetXXX because this does the null check for us too
        if (result is LspLocation[] locations)
        {
            foreach (var loc in locations)
            {
                (var uri, loc.Range) = await _documentMappingService.MapToHostDocumentUriAndRangeAsync(loc.DocumentUri.GetRequiredParsedUri(), loc.Range, cancellationToken).ConfigureAwait(false);
                loc.DocumentUri = new(uri);
            }

            return locations;
        }
        else if (result is VSInternalReferenceItem[] referenceItems)
        {
            foreach (var item in referenceItems)
            {
                (var uri, item.Location!.Range) = await _documentMappingService.MapToHostDocumentUriAndRangeAsync(item.Location.DocumentUri.GetRequiredParsedUri(), item.Location.Range, cancellationToken).ConfigureAwait(false);
                item.Location.DocumentUri = new(uri);
            }

            return referenceItems;
        }

        return default;
    }
}
