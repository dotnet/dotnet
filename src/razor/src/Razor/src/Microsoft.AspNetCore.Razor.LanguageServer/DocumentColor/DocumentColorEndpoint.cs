// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentColor;

[RazorLanguageServerEndpoint(Methods.TextDocumentDocumentColorName)]
internal sealed class DocumentColorEndpoint(IDocumentColorService documentColorService, IClientConnection clientConnection) : IRazorRequestHandler<DocumentColorParams, ColorInformation[]>, ICapabilitiesProvider
{
    private readonly IDocumentColorService _documentColorService = documentColorService ?? throw new ArgumentNullException(nameof(documentColorService));
    private readonly IClientConnection _clientConnection = clientConnection ?? throw new ArgumentNullException(nameof(clientConnection));

    public bool MutatesSolutionState => false;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
        => serverCapabilities.EnableDocumentColorProvider();

    public TextDocumentIdentifier GetTextDocumentIdentifier(DocumentColorParams request)
        => request.TextDocument;

    public Task<ColorInformation[]> HandleRequestAsync(DocumentColorParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
        => _documentColorService.GetColorInformationAsync(_clientConnection, request, requestContext.DocumentContext, cancellationToken);
}
