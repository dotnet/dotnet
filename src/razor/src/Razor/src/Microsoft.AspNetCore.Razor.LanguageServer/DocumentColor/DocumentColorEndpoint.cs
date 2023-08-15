// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentColor;

[LanguageServerEndpoint(Methods.TextDocumentDocumentColorName)]
internal sealed class DocumentColorEndpoint : IRazorRequestHandler<DocumentColorParams, ColorInformation[]>, ICapabilitiesProvider
{
    private readonly ClientNotifierServiceBase _languageServer;

    public DocumentColorEndpoint(ClientNotifierServiceBase languageServer)
    {
        _languageServer = languageServer ?? throw new ArgumentNullException(nameof(languageServer));
    }

    public bool MutatesSolutionState => false;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.DocumentColorProvider = new DocumentColorOptions();
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(DocumentColorParams request)
        => request.TextDocument;

    public async Task<ColorInformation[]> HandleRequestAsync(DocumentColorParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        // Workaround for Web Tools bug https://devdiv.visualstudio.com/DevDiv/_workitems/edit/1743689 where they sometimes
        // send requests for filenames that are stale, possibly due to adornment taggers being cached incorrectly (or caching
        // filenames incorrectly)
        if (requestContext.DocumentContext is null)
        {
            return Array.Empty<ColorInformation>();
        }

        var delegatedRequest = new DelegatedDocumentColorParams()
        {
            HostDocumentVersion = requestContext.GetRequiredDocumentContext().Version,
            TextDocument = request.TextDocument
        };

        var documentColors = await _languageServer.SendRequestAsync<DelegatedDocumentColorParams, ColorInformation[]>(
            CustomMessageNames.RazorProvideHtmlDocumentColorEndpoint,
            delegatedRequest,
            cancellationToken).ConfigureAwait(false);

        if (documentColors is null)
        {
            return Array.Empty<ColorInformation>();
        }

        // HTML and Razor documents have identical mapping locations. Because of this we can return the result as-is.
        return documentColors;
    }
}
