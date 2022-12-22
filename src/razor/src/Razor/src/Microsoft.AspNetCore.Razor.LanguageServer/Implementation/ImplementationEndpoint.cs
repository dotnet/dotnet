// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using ImplementationResult = Microsoft.VisualStudio.LanguageServer.Protocol.SumType<
    Microsoft.VisualStudio.LanguageServer.Protocol.Location[],
    Microsoft.VisualStudio.LanguageServer.Protocol.VSInternalReferenceItem[]>;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Implementation;

internal class ImplementationEndpoint : AbstractRazorDelegatingEndpoint<TextDocumentPositionParamsBridge, ImplementationResult>, IImplementationEndpoint
{
    private readonly RazorDocumentMappingService _documentMappingService;

    public ImplementationEndpoint(
        LanguageServerFeatureOptions languageServerFeatureOptions,
        RazorDocumentMappingService documentMappingService,
        ClientNotifierServiceBase languageServer,
        ILoggerFactory loggerFactory)
        : base(languageServerFeatureOptions, documentMappingService, languageServer, loggerFactory.CreateLogger<ImplementationEndpoint>())
    {
        _documentMappingService = documentMappingService ?? throw new ArgumentNullException(nameof(documentMappingService));
    }

    protected override string CustomMessageTarget => RazorLanguageServerCustomMessageTargets.RazorImplementationEndpointName;

    public RegistrationExtensionResult GetRegistration(VSInternalClientCapabilities clientCapabilities)
    {
        const string ServerCapability = "implementationProvider";
        var option = new SumType<bool, ImplementationOptions>(new ImplementationOptions());

        return new RegistrationExtensionResult(ServerCapability, option);
    }

    protected override Task<IDelegatedParams?> CreateDelegatedParamsAsync(TextDocumentPositionParamsBridge request, RazorRequestContext requestContext, Projection projection, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.GetRequiredDocumentContext();
        return Task.FromResult<IDelegatedParams?>(new DelegatedPositionParams(
                documentContext.Identifier,
                projection.Position,
                projection.LanguageKind));
    }

    protected async override Task<ImplementationResult> HandleDelegatedResponseAsync(ImplementationResult delegatedResponse, TextDocumentPositionParamsBridge request, RazorRequestContext requestContext, Projection projection, CancellationToken cancellationToken)
    {
        // Not using .TryGetXXX because this does the null check for us too
        if (delegatedResponse.Value is Location[] locations)
        {
            foreach (var loc in locations)
            {
                (loc.Uri, loc.Range) = await _documentMappingService.MapFromProjectedDocumentRangeAsync(loc.Uri, loc.Range, cancellationToken).ConfigureAwait(false);
            }

            return locations;
        }
        else if (delegatedResponse.Value is VSInternalReferenceItem[] referenceItems)
        {
            foreach (var item in referenceItems)
            {
                (item.Location.Uri, item.Location.Range) = await _documentMappingService.MapFromProjectedDocumentRangeAsync(item.Location.Uri, item.Location.Range, cancellationToken).ConfigureAwait(false);
            }

            return referenceItems;
        }

        return default;
    }
}
