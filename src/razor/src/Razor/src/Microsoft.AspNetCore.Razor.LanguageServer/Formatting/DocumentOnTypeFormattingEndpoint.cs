// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

[RazorLanguageServerEndpoint(Methods.TextDocumentOnTypeFormattingName)]
internal class DocumentOnTypeFormattingEndpoint(
    IRazorFormattingService razorFormattingService,
    IHtmlFormatter htmlFormatter,
    IDocumentMappingService documentMappingService,
    RazorLSPOptionsMonitor optionsMonitor,
    ILoggerFactory loggerFactory)
    : IRazorRequestHandler<DocumentOnTypeFormattingParams, TextEdit[]?>, ICapabilitiesProvider
{
    private readonly IRazorFormattingService _razorFormattingService = razorFormattingService;
    private readonly IDocumentMappingService _documentMappingService = documentMappingService;
    private readonly RazorLSPOptionsMonitor _optionsMonitor = optionsMonitor;
    private readonly IHtmlFormatter _htmlFormatter = htmlFormatter;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<DocumentOnTypeFormattingEndpoint>();

    private static readonly ImmutableArray<string> s_allTriggerCharacters = ["}", ";", "\n", "{"];

    private static readonly FrozenSet<string> s_csharpTriggerCharacterSet = FrozenSet.ToFrozenSet(["}", ";"], StringComparer.Ordinal);
    private static readonly FrozenSet<string> s_htmlTriggerCharacterSet = FrozenSet.ToFrozenSet(["\n", "{", "}", ";"], StringComparer.Ordinal);
    private static readonly FrozenSet<string> s_allTriggerCharacterSet = s_allTriggerCharacters.ToFrozenSet(StringComparer.Ordinal);

    public bool MutatesSolutionState => false;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.DocumentOnTypeFormattingProvider = new DocumentOnTypeFormattingOptions
        {
            FirstTriggerCharacter = s_allTriggerCharacters[0],
            MoreTriggerCharacter = s_allTriggerCharacters.AsSpan()[1..].ToArray(),
        };
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(DocumentOnTypeFormattingParams request)
    {
        return request.TextDocument;
    }

    public async Task<TextEdit[]?> HandleRequestAsync(DocumentOnTypeFormattingParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Starting OnTypeFormatting request for {request.TextDocument.Uri}.");

        if (!_optionsMonitor.CurrentValue.EnableFormatting)
        {
            _logger.LogInformation($"Formatting option disabled.");
            return null;
        }

        if (!_optionsMonitor.CurrentValue.FormatOnType)
        {
            _logger.LogInformation($"Formatting on type disabled.");
            return null;
        }

        if (!s_allTriggerCharacterSet.Contains(request.Character))
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

        var triggerCharacterKind = _documentMappingService.GetLanguageKind(codeDocument, hostDocumentIndex, rightAssociative: false);
        if (triggerCharacterKind is not (RazorLanguageKind.CSharp or RazorLanguageKind.Html))
        {
            _logger.LogInformation($"Unsupported trigger character language {triggerCharacterKind:G}.");
            return null;
        }

        if (!IsApplicableTriggerCharacter(request.Character, triggerCharacterKind))
        {
            // We were triggered but the trigger character doesn't make sense for the current cursor position. Bail.
            _logger.LogInformation($"Unsupported trigger character location.");
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();

        Debug.Assert(request.Character.Length > 0);

        var options = RazorFormattingOptions.From(request.Options, _optionsMonitor.CurrentValue.CodeBlockBraceOnNextLine);

        TextEdit[] formattedEdits;
        if (triggerCharacterKind == RazorLanguageKind.CSharp)
        {
            formattedEdits = await _razorFormattingService.GetCSharpOnTypeFormattingEditsAsync(documentContext, options, hostDocumentIndex, request.Character[0], cancellationToken).ConfigureAwait(false);
        }
        else if (triggerCharacterKind == RazorLanguageKind.Html)
        {
            var htmlEdits = await _htmlFormatter.GetOnTypeFormattingEditsAsync(documentContext.Snapshot, documentContext.Uri, request.Position, request.Character, request.Options, cancellationToken).ConfigureAwait(false);
            formattedEdits = await _razorFormattingService.GetHtmlOnTypeFormattingEditsAsync(documentContext, htmlEdits, options, hostDocumentIndex, request.Character[0], cancellationToken).ConfigureAwait(false);
        }
        else
        {
            Assumed.Unreachable();
            return null;
        }

        if (formattedEdits.Length == 0)
        {
            _logger.LogInformation($"No formatting changes were necessary");
            return null;
        }

        _logger.LogInformation($"Returning {formattedEdits.Length} final formatted results.");
        return formattedEdits;
    }

    private static bool IsApplicableTriggerCharacter(string triggerCharacter, RazorLanguageKind languageKind)
    {
        if (languageKind == RazorLanguageKind.CSharp)
        {
            return s_csharpTriggerCharacterSet.Contains(triggerCharacter);
        }
        else if (languageKind == RazorLanguageKind.Html)
        {
            return s_htmlTriggerCharacterSet.Contains(triggerCharacter);
        }

        // Unknown trigger character.
        return false;
    }

    internal static class TestAccessor
    {
        public static ImmutableArray<string> GetAllTriggerCharacters() => s_allTriggerCharacters;
        public static FrozenSet<string> GetCSharpTriggerCharacterSet() => s_csharpTriggerCharacterSet;
        public static FrozenSet<string> GetHtmlTriggerCharacterSet() => s_htmlTriggerCharacterSet;
    }
}
