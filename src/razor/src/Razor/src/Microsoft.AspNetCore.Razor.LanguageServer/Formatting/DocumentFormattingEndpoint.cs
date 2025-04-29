// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

[RazorLanguageServerEndpoint(Methods.TextDocumentFormattingName)]
internal class DocumentFormattingEndpoint(
    IRazorFormattingService razorFormattingService,
    IHtmlFormatter htmlFormatter,
    RazorLSPOptionsMonitor optionsMonitor) : IRazorRequestHandler<DocumentFormattingParams, TextEdit[]?>, ICapabilitiesProvider
{
    private readonly IRazorFormattingService _razorFormattingService = razorFormattingService;
    private readonly RazorLSPOptionsMonitor _optionsMonitor = optionsMonitor;
    private readonly IHtmlFormatter _htmlFormatter = htmlFormatter;

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
        if (!_optionsMonitor.CurrentValue.Formatting.IsEnabled())
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

        var options = RazorFormattingOptions.From(request.Options, _optionsMonitor.CurrentValue.CodeBlockBraceOnNextLine);

        if (await _htmlFormatter.GetDocumentFormattingEditsAsync(
            documentContext.Snapshot,
            documentContext.Uri,
            request.Options,
            cancellationToken).ConfigureAwait(false) is not { } htmlChanges)
        {
            return null;
        }

        var changes = await _razorFormattingService.GetDocumentFormattingChangesAsync(documentContext, htmlChanges, span: null, options, cancellationToken).ConfigureAwait(false);

        return [.. changes.Select(codeDocument.Source.Text.GetTextEdit)];
    }
}
