// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.Threading;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.GoToDefinition;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using DefinitionResult = Roslyn.LanguageServer.Protocol.SumType<
    Roslyn.LanguageServer.Protocol.Location,
    Roslyn.LanguageServer.Protocol.VSInternalLocation,
    Roslyn.LanguageServer.Protocol.VSInternalLocation[],
    Roslyn.LanguageServer.Protocol.DocumentLink[]>;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Definition;

[RazorLanguageServerEndpoint(Methods.TextDocumentDefinitionName)]
internal sealed class DefinitionEndpoint(
    IRazorComponentDefinitionService componentDefinitionService,
    IDocumentMappingService documentMappingService,
    ProjectSnapshotManager projectManager,
    LanguageServerFeatureOptions languageServerFeatureOptions,
    IClientConnection clientConnection,
    ILoggerFactory loggerFactory)
    : AbstractRazorDelegatingEndpoint<TextDocumentPositionParams, DefinitionResult?>(
        languageServerFeatureOptions,
        documentMappingService,
        clientConnection,
        loggerFactory.GetOrCreateLogger<DefinitionEndpoint>()), ICapabilitiesProvider
{
    private readonly IRazorComponentDefinitionService _componentDefinitionService = componentDefinitionService;
    private readonly IDocumentMappingService _documentMappingService = documentMappingService;
    private readonly ProjectSnapshotManager _projectManager = projectManager;

    protected override bool PreferCSharpOverHtmlIfPossible => true;

    protected override IDocumentPositionInfoStrategy DocumentPositionInfoStrategy => PreferAttributeNameDocumentPositionInfoStrategy.Instance;

    protected override string CustomMessageTarget => CustomMessageNames.RazorDefinitionEndpointName;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.DefinitionProvider = new DefinitionOptions();
    }

    protected async override Task<DefinitionResult?> TryHandleAsync(
        TextDocumentPositionParams request,
        RazorRequestContext requestContext,
        DocumentPositionInfo positionInfo,
        CancellationToken cancellationToken)
    {
        Logger.LogInformation($"Starting go-to-def endpoint request.");

        var documentContext = requestContext.DocumentContext;
        if (documentContext is null)
        {
            return null;
        }

        // If single server support is on, then we ignore attributes, as they are better handled by delegating to Roslyn
        return await _componentDefinitionService
            .GetDefinitionAsync(documentContext.Snapshot, positionInfo, _projectManager.GetQueryOperations(), ignoreAttributes: SingleServerSupport, cancellationToken)
            .ConfigureAwait(false);
    }

    protected override Task<IDelegatedParams?> CreateDelegatedParamsAsync(
        TextDocumentPositionParams request,
        RazorRequestContext requestContext,
        DocumentPositionInfo positionInfo,
        CancellationToken cancellationToken)
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

    protected async override Task<DefinitionResult?> HandleDelegatedResponseAsync(
        DefinitionResult? response,
        TextDocumentPositionParams originalRequest,
        RazorRequestContext requestContext,
        DocumentPositionInfo positionInfo,
        CancellationToken cancellationToken)
    {
        if (response is null)
        {
            return null;
        }

        var result = response.GetValueOrDefault().Value;

        // Not using .TryGetXXX because this does the null check for us too
        if (result is LspLocation location)
        {
            (var uri, location.Range) = await _documentMappingService.MapToHostDocumentUriAndRangeAsync(location.DocumentUri.GetRequiredParsedUri(), location.Range, cancellationToken).ConfigureAwait(false);
            location.DocumentUri = new(uri);
        }
        else if (result is LspLocation[] locations)
        {
            foreach (var loc in locations)
            {
                (var uri, loc.Range) = await _documentMappingService.MapToHostDocumentUriAndRangeAsync(loc.DocumentUri.GetRequiredParsedUri(), loc.Range, cancellationToken).ConfigureAwait(false);
                loc.DocumentUri = new(uri);
            }
        }
        else if (result is DocumentLink[] links)
        {
            foreach (var link in links)
            {
                if (link.DocumentTarget?.ParsedUri is not null)
                {
                    (var uri, link.Range) = await _documentMappingService.MapToHostDocumentUriAndRangeAsync(link.DocumentTarget.ParsedUri, link.Range, cancellationToken).ConfigureAwait(false);
                    link.DocumentTarget = new(uri);
                }
            }
        }

        return response;
    }
}
