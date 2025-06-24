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
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.DocumentSymbols;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentSymbols;

[RazorLanguageServerEndpoint(Methods.TextDocumentDocumentSymbolName)]
internal class DocumentSymbolEndpoint(
    IClientConnection clientConnection,
    IDocumentSymbolService documentSymbolService,
    LanguageServerFeatureOptions languageServerFeatureOptions) : IRazorRequestHandler<DocumentSymbolParams, SumType<DocumentSymbol[], SymbolInformation[]>?>, ICapabilitiesProvider
{
    private readonly IClientConnection _clientConnection = clientConnection;
    private readonly IDocumentSymbolService _documentSymbolService = documentSymbolService;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions = languageServerFeatureOptions;

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
        var csharpDocument = codeDocument.GetRequiredCSharpDocument();
        return _documentSymbolService.GetDocumentSymbols(documentContext.Uri, csharpDocument, symbols);
    }
}
