// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using ImplementationResult = Microsoft.VisualStudio.LanguageServer.Protocol.SumType<
    Microsoft.VisualStudio.LanguageServer.Protocol.Location[],
    Microsoft.VisualStudio.LanguageServer.Protocol.VSInternalReferenceItem[]>;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Implementation;

[LanguageServerEndpoint(Methods.TextDocumentImplementationName)]
internal sealed class ImplementationEndpoint : AbstractRazorDelegatingEndpoint<TextDocumentPositionParams, ImplementationResult>, ICapabilitiesProvider
{
    private readonly IRazorDocumentMappingService _documentMappingService;

    public ImplementationEndpoint(
        LanguageServerFeatureOptions languageServerFeatureOptions,
        IRazorDocumentMappingService documentMappingService,
        ClientNotifierServiceBase languageServer,
        ILoggerFactory loggerFactory)
        : base(languageServerFeatureOptions, documentMappingService, languageServer, loggerFactory.CreateLogger<ImplementationEndpoint>())
    {
        _documentMappingService = documentMappingService ?? throw new ArgumentNullException(nameof(documentMappingService));
    }

    protected override string CustomMessageTarget => CustomMessageNames.RazorImplementationEndpointName;

    protected override bool PreferCSharpOverHtmlIfPossible => true;

    protected override IDocumentPositionInfoStrategy DocumentPositionInfoStrategy => PreferAttributeNameDocumentPositionInfoStrategy.Instance;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.ImplementationProvider= new ImplementationOptions();
    }

    protected override Task<IDelegatedParams?> CreateDelegatedParamsAsync(TextDocumentPositionParams request, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.GetRequiredDocumentContext();
        return Task.FromResult<IDelegatedParams?>(new DelegatedPositionParams(
                documentContext.Identifier,
                positionInfo.Position,
                positionInfo.LanguageKind));
    }

    protected async override Task<ImplementationResult> HandleDelegatedResponseAsync(ImplementationResult delegatedResponse, TextDocumentPositionParams request, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
    {
        // Not using .TryGetXXX because this does the null check for us too
        if (delegatedResponse.Value is Location[] locations)
        {
            foreach (var loc in locations)
            {
                (loc.Uri, loc.Range) = await _documentMappingService.MapToHostDocumentUriAndRangeAsync(loc.Uri, loc.Range, cancellationToken).ConfigureAwait(false);
            }

            return locations;
        }
        else if (delegatedResponse.Value is VSInternalReferenceItem[] referenceItems)
        {
            foreach (var item in referenceItems)
            {
                (item.Location.Uri, item.Location.Range) = await _documentMappingService.MapToHostDocumentUriAndRangeAsync(item.Location.Uri, item.Location.Range, cancellationToken).ConfigureAwait(false);
            }

            return referenceItems;
        }

        return default;
    }
}
