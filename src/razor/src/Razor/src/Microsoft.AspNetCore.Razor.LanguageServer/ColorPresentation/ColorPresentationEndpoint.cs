// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.ColorPresentation;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using LspColorPresentation = Microsoft.CodeAnalysis.Razor.Protocol.ColorPresentation.ColorPresentation;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ColorPresentation;

[RazorLanguageServerEndpoint(CustomMessageNames.ColorPresentationMethodName)]
internal sealed class ColorPresentationEndpoint(IClientConnection clientConnection) : IRazorRequestHandler<ColorPresentationParams, LspColorPresentation[]>
{
    private readonly IClientConnection _clientConnection = clientConnection;

    public bool MutatesSolutionState => false;

    public TextDocumentIdentifier GetTextDocumentIdentifier(ColorPresentationParams request)
        => request.TextDocument;

    public async Task<LspColorPresentation[]> HandleRequestAsync(ColorPresentationParams request, RazorRequestContext context, CancellationToken cancellationToken)
    {
        var documentContext = context.DocumentContext;
        if (documentContext is null)
        {
            return [];
        }

        var delegatedRequest = new DelegatedColorPresentationParams
        {
            RequiredHostDocumentVersion = documentContext.Snapshot.Version,
            Color = request.Color,
            Range = request.Range,
            TextDocument = request.TextDocument
        };

        var colorPresentations = await _clientConnection.SendRequestAsync<ColorPresentationParams, LspColorPresentation[]>(
            CustomMessageNames.RazorProvideHtmlColorPresentationEndpoint,
            delegatedRequest,
            cancellationToken).ConfigureAwait(false);

        if (colorPresentations is null)
        {
            return [];
        }

        // HTML and Razor documents have identical mapping locations. Because of this we can return the result as-is.
        return colorPresentations;
    }
}
