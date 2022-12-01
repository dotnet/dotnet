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

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentHighlighting;

internal class DocumentHighlightEndpoint : AbstractRazorDelegatingEndpoint<DocumentHighlightParamsBridge, DocumentHighlight[]?>, IDocumentHighlightEndpoint
{
    private readonly RazorDocumentMappingService _documentMappingService;

    public DocumentHighlightEndpoint(
        LanguageServerFeatureOptions languageServerFeatureOptions,
        RazorDocumentMappingService documentMappingService,
        ClientNotifierServiceBase languageServer,
        ILoggerFactory loggerFactory)
        : base(languageServerFeatureOptions, documentMappingService, languageServer, loggerFactory.CreateLogger<DocumentHighlightEndpoint>())
    {
        _documentMappingService = documentMappingService ?? throw new ArgumentNullException(nameof(documentMappingService));
    }

    public RegistrationExtensionResult GetRegistration(VSInternalClientCapabilities clientCapabilities)
    {
        const string ServerCapability = "documentHighlightProvider";
        var options = new SumType<bool, DocumentHighlightOptions>(
            new DocumentHighlightOptions
            {
                WorkDoneProgress = false
            });

        return new RegistrationExtensionResult(ServerCapability, options);
    }

    protected override string CustomMessageTarget => RazorLanguageServerCustomMessageTargets.RazorDocumentHighlightEndpointName;

    protected override Task<DocumentHighlight[]?> TryHandleAsync(DocumentHighlightParamsBridge request, RazorRequestContext requestContext, Projection projection, CancellationToken cancellationToken)
    {
        // We don't handle this in any particular way for Razor, we just delegate
        return Task.FromResult<DocumentHighlight[]?>(null);
    }

    protected override Task<IDelegatedParams?> CreateDelegatedParamsAsync(DocumentHighlightParamsBridge request, RazorRequestContext requestContext, Projection projection, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.GetRequiredDocumentContext();
        return Task.FromResult<IDelegatedParams?>(new DelegatedPositionParams(
                documentContext.Identifier,
                projection.Position,
                projection.LanguageKind));
    }

    protected override async Task<DocumentHighlight[]?> HandleDelegatedResponseAsync(DocumentHighlight[]? response, DocumentHighlightParamsBridge request, RazorRequestContext requestContext, Projection projection, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.GetRequiredDocumentContext();
        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

        if (response is null)
        {
            return null;
        }

        foreach (var highlight in response)
        {
            if (_documentMappingService.TryMapFromProjectedDocumentRange(codeDocument, highlight.Range, out var mappedRange))
            {
                highlight.Range = mappedRange;
            }
        }

        return response;
    }
}
