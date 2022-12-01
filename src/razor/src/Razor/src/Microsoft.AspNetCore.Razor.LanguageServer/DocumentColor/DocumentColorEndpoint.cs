// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentColor;

internal class DocumentColorEndpoint : IDocumentColorEndpoint
{
    private readonly ClientNotifierServiceBase _languageServer;

    public DocumentColorEndpoint(ClientNotifierServiceBase languageServer)
    {
        if (languageServer is null)
        {
            throw new ArgumentNullException(nameof(languageServer));
        }

        _languageServer = languageServer;
    }

    public bool MutatesSolutionState => false;

    public RegistrationExtensionResult GetRegistration(VSInternalClientCapabilities clientCapabilities)
    {
        const string ServerCapabilities = "colorProvider";
        var options = new SumType<bool, DocumentColorOptions>(new DocumentColorOptions());

        return new RegistrationExtensionResult(ServerCapabilities, options);
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(DocumentColorParams request)
    {
        return request.TextDocument;
    }

    public async Task<ColorInformation[]> HandleRequestAsync(DocumentColorParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        var delegatedRequest = new DelegatedDocumentColorParams()
        {
            HostDocumentVersion = requestContext.GetRequiredDocumentContext().Version,
            TextDocument = request.TextDocument
        };

        var documentColors = await _languageServer.SendRequestAsync<DelegatedDocumentColorParams, ColorInformation[]>(
            RazorLanguageServerCustomMessageTargets.RazorProvideHtmlDocumentColorEndpoint,
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
