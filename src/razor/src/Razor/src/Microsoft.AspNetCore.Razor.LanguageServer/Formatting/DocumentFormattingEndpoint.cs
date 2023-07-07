// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

[LanguageServerEndpoint(Methods.TextDocumentFormattingName)]
internal class DocumentFormattingEndpoint : IRazorRequestHandler<DocumentFormattingParams, TextEdit[]?>, ICapabilitiesProvider
{
    private readonly IRazorFormattingService _razorFormattingService;
    private readonly IOptionsMonitor<RazorLSPOptions> _optionsMonitor;

    public DocumentFormattingEndpoint(
        IRazorFormattingService razorFormattingService,
        IOptionsMonitor<RazorLSPOptions> optionsMonitor)
    {
        _razorFormattingService = razorFormattingService ?? throw new ArgumentNullException(nameof(razorFormattingService));
        _optionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
    }

    public bool MutatesSolutionState => false;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.DocumentFormattingProvider = new DocumentFormattingOptions();
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(DocumentFormattingParams request)
    {
        return request.TextDocument;
    }

    public async Task<TextEdit[]?> HandleRequestAsync(DocumentFormattingParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        if (!_optionsMonitor.CurrentValue.EnableFormatting)
        {
            return null;
        }

        var documentContext = requestContext.DocumentContext;
        if (documentContext is null || cancellationToken.IsCancellationRequested)
        {
            return null;
        }

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        if (codeDocument.IsUnsupported())
        {
            return null;
        }

        var edits = await _razorFormattingService.FormatAsync(documentContext, range: null, request.Options, cancellationToken).ConfigureAwait(false);
        return edits;
    }
}
