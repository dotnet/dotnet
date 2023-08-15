// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.DocumentColor;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ColorPresentation;

[LanguageServerEndpoint(ColorPresentationMethodName)]
internal sealed class ColorPresentationEndpoint : IRazorRequestHandler<ColorPresentationParams, ColorPresentation[]>
{
    public const string ColorPresentationMethodName = "textDocument/colorPresentation";

    private readonly ClientNotifierServiceBase _languageServer;

    public ColorPresentationEndpoint(ClientNotifierServiceBase languageServer)
    {
        if (languageServer is null)
        {
            throw new ArgumentNullException(nameof(languageServer));
        }

        _languageServer = languageServer;
    }

    public bool MutatesSolutionState => false;

    public TextDocumentIdentifier GetTextDocumentIdentifier(ColorPresentationParams request)
        => request.TextDocument;

    public async Task<ColorPresentation[]> HandleRequestAsync(ColorPresentationParams request, RazorRequestContext context, CancellationToken cancellationToken)
    {
        var documentContext = context.DocumentContext;
        if (documentContext is null)
        {
            return Array.Empty<ColorPresentation>();
        }

        var delegatedRequest = new DelegatedColorPresentationParams
        {
            RequiredHostDocumentVersion = documentContext.Version,
            Color = request.Color,
            Range = request.Range,
            TextDocument = request.TextDocument
        };

        var colorPresentations = await _languageServer.SendRequestAsync<ColorPresentationParams, ColorPresentation[]>(
            CustomMessageNames.RazorProvideHtmlColorPresentationEndpoint,
            delegatedRequest,
            cancellationToken).ConfigureAwait(false);

        if (colorPresentations is null)
        {
            return Array.Empty<ColorPresentation>();
        }

        // HTML and Razor documents have identical mapping locations. Because of this we can return the result as-is.
        return colorPresentations;
    }
}
