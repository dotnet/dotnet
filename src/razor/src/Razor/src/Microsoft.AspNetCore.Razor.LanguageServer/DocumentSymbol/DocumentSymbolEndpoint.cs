// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentSymbol;

[LanguageServerEndpoint(Methods.TextDocumentDocumentSymbolName)]
internal class DocumentSymbolEndpoint : IRazorRequestHandler<DocumentSymbolParams, SymbolInformation[]>, ICapabilitiesProvider
{
    private readonly ClientNotifierServiceBase _languageServer;
    private readonly IRazorDocumentMappingService _documentMappingService;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions;

    public DocumentSymbolEndpoint(
        ClientNotifierServiceBase languageServer,
        IRazorDocumentMappingService documentMappingService,
        LanguageServerFeatureOptions languageServerFeatureOptions)
    {
        _languageServer = languageServer ?? throw new ArgumentNullException(nameof(languageServer));
        _documentMappingService = documentMappingService ?? throw new ArgumentNullException(nameof(documentMappingService));
        _languageServerFeatureOptions = languageServerFeatureOptions;
    }

    public bool MutatesSolutionState => false;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        // TODO: Add an option for this that the client can configure. This turns this off for
        // VS Code but keeps it on for VS by depending on SingleServerSupport signifying the client.
        if (!_languageServerFeatureOptions.SingleServerSupport)
        {
            return;
        }

        // Disabled for 17.8
        //serverCapabilities.DocumentSymbolProvider = new DocumentSymbolOptions()
        //{
        //    WorkDoneProgress = false
        //};
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(DocumentSymbolParams request)
        => request.TextDocument;

    public async Task<SymbolInformation[]> HandleRequestAsync(DocumentSymbolParams request, RazorRequestContext context, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var documentContext = context.GetRequiredDocumentContext();
        var delegatedParams = new DelegatedDocumentSymbolParams(documentContext.Identifier);

        var symbolInformations = await _languageServer.SendRequestAsync<DelegatedDocumentSymbolParams, SymbolInformation[]?>(
            CustomMessageNames.RazorDocumentSymbolEndpoint,
            delegatedParams,
            cancellationToken).ConfigureAwait(false);

        if (symbolInformations is null)
        {
            return Array.Empty<SymbolInformation>();
        }

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        var csharpDocument = codeDocument.GetCSharpDocument();

        using var _ = ListPool<SymbolInformation>.GetPooledObject(out var mappedSymbols);

        foreach (var symbolInformation in symbolInformations)
        {
            if (_documentMappingService.TryMapToHostDocumentRange(csharpDocument, symbolInformation.Location.Range, out var newRange))
            {
                symbolInformation.Location.Range = newRange;
                symbolInformation.Location.Uri = documentContext.Uri;
                mappedSymbols.Add(symbolInformation);
            }
        }

        return mappedSymbols.ToArray();
    }
}
