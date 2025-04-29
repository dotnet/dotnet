// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

[RazorLanguageServerEndpoint(Methods.TextDocumentOnTypeFormattingName)]
internal class DocumentOnTypeFormattingEndpoint(
    IRazorFormattingService razorFormattingService,
    IHtmlFormatter htmlFormatter,
    RazorLSPOptionsMonitor optionsMonitor,
    ILoggerFactory loggerFactory)
    : IRazorRequestHandler<DocumentOnTypeFormattingParams, TextEdit[]?>, ICapabilitiesProvider
{
    private readonly IRazorFormattingService _razorFormattingService = razorFormattingService;
    private readonly RazorLSPOptionsMonitor _optionsMonitor = optionsMonitor;
    private readonly IHtmlFormatter _htmlFormatter = htmlFormatter;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<DocumentOnTypeFormattingEndpoint>();

    public bool MutatesSolutionState => false;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.DocumentOnTypeFormattingProvider = new DocumentOnTypeFormattingOptions().EnableOnTypeFormattingTriggerCharacters();
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(DocumentOnTypeFormattingParams request)
    {
        return request.TextDocument;
    }

    public async Task<TextEdit[]?> HandleRequestAsync(DocumentOnTypeFormattingParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Starting OnTypeFormatting request for {request.TextDocument.Uri}.");

        if (!_optionsMonitor.CurrentValue.Formatting.IsEnabled())
        {
            _logger.LogInformation($"Formatting option disabled.");
            return null;
        }

        if (!_optionsMonitor.CurrentValue.Formatting.IsOnTypeEnabled())
        {
            _logger.LogInformation($"Formatting on type disabled.");
            return null;
        }

        if (!RazorFormattingService.AllTriggerCharacterSet.Contains(request.Character))
        {
            _logger.LogWarning($"Unexpected trigger character '{request.Character}'.");
            return null;
        }

        var documentContext = requestContext.DocumentContext;
        if (documentContext is null)
        {
            _logger.LogWarning($"Failed to find document {request.TextDocument.Uri}.");
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        if (codeDocument.IsUnsupported())
        {
            _logger.LogWarning($"Failed to retrieve generated output for document {request.TextDocument.Uri}.");
            return null;
        }

        var sourceText = await documentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
        if (!sourceText.TryGetAbsoluteIndex(request.Position, out var hostDocumentIndex))
        {
            return null;
        }

        if (!_razorFormattingService.TryGetOnTypeFormattingTriggerKind(codeDocument, hostDocumentIndex, request.Character, out var triggerCharacterKind) ||
            triggerCharacterKind == RazorLanguageKind.Razor)
        {
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();

        var options = RazorFormattingOptions.From(request.Options, _optionsMonitor.CurrentValue.CodeBlockBraceOnNextLine);

        ImmutableArray<TextChange> formattedChanges;
        if (triggerCharacterKind == RazorLanguageKind.CSharp)
        {
            formattedChanges = await _razorFormattingService.GetCSharpOnTypeFormattingChangesAsync(documentContext, options, hostDocumentIndex, request.Character[0], cancellationToken).ConfigureAwait(false);
        }
        else if (triggerCharacterKind == RazorLanguageKind.Html)
        {
            if (await _htmlFormatter.GetOnTypeFormattingEditsAsync(
                documentContext.Snapshot,
                documentContext.Uri,
                request.Position,
                request.Character,
                request.Options,
                cancellationToken).ConfigureAwait(false) is not { } htmlChanges)
            {
                return null;
            }

            formattedChanges = await _razorFormattingService.GetHtmlOnTypeFormattingChangesAsync(documentContext, htmlChanges, options, hostDocumentIndex, request.Character[0], cancellationToken).ConfigureAwait(false);
        }
        else
        {
            Assumed.Unreachable();
            return null;
        }

        if (formattedChanges.Length == 0)
        {
            _logger.LogInformation($"No formatting changes were necessary");
            return null;
        }

        _logger.LogInformation($"Returning {formattedChanges.Length} final formatted results.");
        return [.. formattedChanges.Select(sourceText.GetTextEdit)];
    }
}
