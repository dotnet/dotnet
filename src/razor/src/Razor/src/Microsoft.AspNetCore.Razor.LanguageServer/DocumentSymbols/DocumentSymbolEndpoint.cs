// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Text.Tagging;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentSymbols;

[RazorLanguageServerEndpoint(Methods.TextDocumentDocumentSymbolName)]
internal class DocumentSymbolEndpoint : IRazorRequestHandler<DocumentSymbolParams, SumType<DocumentSymbol[], SymbolInformation[]>?>, ICapabilitiesProvider
{
    private readonly IClientConnection _clientConnection;
    private readonly IDocumentMappingService _documentMappingService;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions;

    public DocumentSymbolEndpoint(
        IClientConnection clientConnection,
        IDocumentMappingService documentMappingService,
        LanguageServerFeatureOptions languageServerFeatureOptions)
    {
        _clientConnection = clientConnection ?? throw new ArgumentNullException(nameof(clientConnection));
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

        serverCapabilities.DocumentSymbolProvider = new DocumentSymbolOptions()
        {
            WorkDoneProgress = false
        };
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(DocumentSymbolParams request)
        => request.TextDocument;

    public async Task<SumType<DocumentSymbol[], SymbolInformation[]>?> HandleRequestAsync(DocumentSymbolParams request, RazorRequestContext context, CancellationToken cancellationToken)
    {
        var documentContext = context.DocumentContext;
        if (documentContext is null)
        {
            return null;
        }

        var delegatedParams = new DelegatedDocumentSymbolParams(documentContext.GetTextDocumentIdentifierAndVersion());

        var result = await _clientConnection.SendRequestAsync<DelegatedDocumentSymbolParams, SumType<DocumentSymbol[], SymbolInformation[]>?>(
            CustomMessageNames.RazorDocumentSymbolEndpoint,
            delegatedParams,
            cancellationToken).ConfigureAwait(false);

        if (result is not { } symbols)
        {
            return Array.Empty<SymbolInformation>();
        }

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        var csharpDocument = codeDocument.GetCSharpDocument();

        if (symbols.TryGetFirst(out var documentSymbols))
        {
            return RemapDocumentSymbols(csharpDocument, documentSymbols);
        }
        else if (symbols.TryGetSecond(out var symbolInformations))
        {
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
        else
        {
            Debug.Fail("Unsupported response type");
            throw new InvalidOperationException();
        }
    }

    private DocumentSymbol[]? RemapDocumentSymbols(RazorCSharpDocument csharpDocument, DocumentSymbol[]? documentSymbols)
    {
        if (documentSymbols is null)
        {
            return null;
        }

        using var _ = ListPool<DocumentSymbol>.GetPooledObject(out var mappedSymbols);

        foreach (var documentSymbol in documentSymbols)
        {
            if (TryRemapRanges(csharpDocument, documentSymbol))
            {
                documentSymbol.Children = RemapDocumentSymbols(csharpDocument, documentSymbol.Children);

                mappedSymbols.Add(documentSymbol);
            }
            else if (documentSymbol.Children is [_, ..] &&
                RemapDocumentSymbols(csharpDocument, documentSymbol.Children) is [_, ..] mappedChildren)
            {
                // This range didn't map, but some/all of its children did, so we promote them to this level so we don't
                // lose any information.
                mappedSymbols.AddRange(mappedChildren);
            }
        }

        return mappedSymbols.ToArray();

        bool TryRemapRanges(RazorCSharpDocument csharpDocument, DocumentSymbol documentSymbol)
        {
            if (_documentMappingService.TryMapToHostDocumentRange(csharpDocument, documentSymbol.Range, out var newRange) &&
                _documentMappingService.TryMapToHostDocumentRange(csharpDocument, documentSymbol.SelectionRange, out var newSelectionRange))
            {
                documentSymbol.Range = newRange;
                documentSymbol.SelectionRange = newSelectionRange;

                return true;
            }

            return false;
        }
    }
}
