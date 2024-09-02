// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

[RazorLanguageServerEndpoint(Methods.TextDocumentRangeFormattingName)]
internal class DocumentRangeFormattingEndpoint(
    IRazorFormattingService razorFormattingService,
    IHtmlFormatter htmlFormatter,
    RazorLSPOptionsMonitor optionsMonitor) : IRazorRequestHandler<DocumentRangeFormattingParams, TextEdit[]?>, ICapabilitiesProvider
{
    private readonly IRazorFormattingService _razorFormattingService = razorFormattingService;
    private readonly RazorLSPOptionsMonitor _optionsMonitor = optionsMonitor;
    private readonly IHtmlFormatter _htmlFormatter = htmlFormatter;

    public bool MutatesSolutionState => false;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.DocumentRangeFormattingProvider = new DocumentRangeFormattingOptions();
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(DocumentRangeFormattingParams request)
    {
        return request.TextDocument;
    }

    public async Task<TextEdit[]?> HandleRequestAsync(DocumentRangeFormattingParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
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

        var options = RazorFormattingOptions.From(request.Options, _optionsMonitor.CurrentValue.CodeBlockBraceOnNextLine);

        var htmlEdits = await _htmlFormatter.GetDocumentFormattingEditsAsync(documentContext.Snapshot, documentContext.Uri, request.Options, cancellationToken).ConfigureAwait(false);
        var edits = await _razorFormattingService.GetDocumentFormattingEditsAsync(documentContext, htmlEdits, request.Range, options, cancellationToken).ConfigureAwait(false);

        return edits;
    }
}
