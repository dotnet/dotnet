// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.ColorPresentation;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentColor;

[RazorLanguageServerEndpoint(Methods.TextDocumentDocumentColorName)]
internal sealed class DocumentColorEndpoint(IClientConnection clientConnection) : IRazorRequestHandler<DocumentColorParams, ColorInformation[]>, ICapabilitiesProvider
{
    private readonly IClientConnection _clientConnection = clientConnection;

    public bool MutatesSolutionState => false;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
        => serverCapabilities.DocumentColorProvider = true;

    public TextDocumentIdentifier GetTextDocumentIdentifier(DocumentColorParams request)
        => request.TextDocument;

    public async Task<ColorInformation[]> HandleRequestAsync(DocumentColorParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.DocumentContext;

        // Workaround for Web Tools bug https://devdiv.visualstudio.com/DevDiv/_workitems/edit/1743689 where they sometimes
        // send requests for filenames that are stale, possibly due to adornment taggers being cached incorrectly (or caching
        // filenames incorrectly)
        if (documentContext is null)
        {
            return [];
        }

        var delegatedRequest = new DelegatedDocumentColorParams()
        {
            HostDocumentVersion = documentContext.Snapshot.Version,
            TextDocument = request.TextDocument
        };

        var documentColors = await _clientConnection.SendRequestAsync<DelegatedDocumentColorParams, ColorInformation[]>(
            CustomMessageNames.RazorProvideHtmlDocumentColorEndpoint,
            delegatedRequest,
            cancellationToken).ConfigureAwait(false);

        if (documentColors is null)
        {
            return [];
        }

        // HTML and Razor documents have identical mapping locations. Because of this we can return the result as-is.
        return documentColors;
    }
}
