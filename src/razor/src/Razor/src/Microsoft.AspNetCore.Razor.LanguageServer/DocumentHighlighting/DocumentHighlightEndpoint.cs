// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentHighlighting;

[LanguageServerEndpoint(Methods.TextDocumentDocumentHighlightName)]
internal class DocumentHighlightEndpoint : AbstractRazorDelegatingEndpoint<DocumentHighlightParams, DocumentHighlight[]?>, ICapabilitiesProvider
{
    private readonly IRazorDocumentMappingService _documentMappingService;

    public DocumentHighlightEndpoint(
        LanguageServerFeatureOptions languageServerFeatureOptions,
        IRazorDocumentMappingService documentMappingService,
        ClientNotifierServiceBase languageServer,
        ILoggerFactory loggerFactory)
        : base(languageServerFeatureOptions, documentMappingService, languageServer, loggerFactory.CreateLogger<DocumentHighlightEndpoint>())
    {
        _documentMappingService = documentMappingService ?? throw new ArgumentNullException(nameof(documentMappingService));
    }

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.DocumentHighlightProvider = new DocumentHighlightOptions
        {
            WorkDoneProgress = false
        };
    }

    protected override string CustomMessageTarget => CustomMessageNames.RazorDocumentHighlightEndpointName;

    protected override Task<DocumentHighlight[]?> TryHandleAsync(DocumentHighlightParams request, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
    {
        // We don't handle this in any particular way for Razor, we just delegate
        return Task.FromResult<DocumentHighlight[]?>(null);
    }

    protected override Task<IDelegatedParams?> CreateDelegatedParamsAsync(DocumentHighlightParams request, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.GetRequiredDocumentContext();
        return Task.FromResult<IDelegatedParams?>(new DelegatedPositionParams(
                documentContext.Identifier,
                positionInfo.Position,
                positionInfo.LanguageKind));
    }

    protected override async Task<DocumentHighlight[]?> HandleDelegatedResponseAsync(DocumentHighlight[]? response, DocumentHighlightParams request, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
    {
        if (response is null)
        {
            return null;
        }

        var documentContext = requestContext.GetRequiredDocumentContext();
        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

        var generatedDocument = codeDocument.GetGeneratedDocument(positionInfo.LanguageKind);
        foreach (var highlight in response)
        {
            if (_documentMappingService.TryMapToHostDocumentRange(generatedDocument, highlight.Range, out var mappedRange))
            {
                highlight.Range = mappedRange;
            }
        }

        return response;
    }
}
