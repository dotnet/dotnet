// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

[LanguageServerEndpoint(Methods.TextDocumentOnTypeFormattingName)]
internal class DocumentOnTypeFormattingEndpoint(
    IRazorFormattingService razorFormattingService,
    IRazorDocumentMappingService razorDocumentMappingService,
    IOptionsMonitor<RazorLSPOptions> optionsMonitor,
    IRazorLoggerFactory loggerFactory)
    : IRazorRequestHandler<DocumentOnTypeFormattingParams, TextEdit[]?>, ICapabilitiesProvider
{
    private readonly IRazorFormattingService _razorFormattingService = razorFormattingService ?? throw new ArgumentNullException(nameof(razorFormattingService));
    private readonly IRazorDocumentMappingService _razorDocumentMappingService = razorDocumentMappingService ?? throw new ArgumentNullException(nameof(razorDocumentMappingService));
    private readonly IOptionsMonitor<RazorLSPOptions> _optionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
    private readonly ILogger _logger = loggerFactory.CreateLogger<DocumentOnTypeFormattingEndpoint>();

    private static readonly IReadOnlyList<string> s_csharpTriggerCharacters = new[] { "}", ";" };
    private static readonly IReadOnlyList<string> s_htmlTriggerCharacters = new[] { "\n", "{", "}", ";" };
    private static readonly IReadOnlyList<string> s_allTriggerCharacters = s_csharpTriggerCharacters.Concat(s_htmlTriggerCharacters).ToArray();

    public bool MutatesSolutionState => false;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.DocumentOnTypeFormattingProvider = new DocumentOnTypeFormattingOptions
        {
            FirstTriggerCharacter = s_allTriggerCharacters[0],
            MoreTriggerCharacter = s_allTriggerCharacters.Skip(1).ToArray(),
        };
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(DocumentOnTypeFormattingParams request)
    {
        return request.TextDocument;
    }

    public async Task<TextEdit[]?> HandleRequestAsync(DocumentOnTypeFormattingParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting OnTypeFormatting request for {requestTextDocumentUri}.", request.TextDocument.Uri);

        if (!_optionsMonitor.CurrentValue.EnableFormatting)
        {
            _logger.LogInformation("Formatting option disabled.");
            return null;
        }

        if (!_optionsMonitor.CurrentValue.FormatOnType)
        {
            _logger.LogInformation("Formatting on type disabled.");
            return null;
        }

        if (!s_allTriggerCharacters.Contains(request.Character, StringComparer.Ordinal))
        {
            _logger.LogWarning("Unexpected trigger character '{requestCharacter}'.", request.Character);
            return null;
        }

        var documentContext = requestContext.DocumentContext;

        if (documentContext is null)
        {
            _logger.LogWarning("Failed to find document {requestTextDocumentUri}.", request.TextDocument.Uri);
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        if (codeDocument.IsUnsupported())
        {
            _logger.LogWarning("Failed to retrieve generated output for document {requestTextDocumentUri}.", request.TextDocument.Uri);
            return null;
        }

        var sourceText = await documentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
        if (!request.Position.TryGetAbsoluteIndex(sourceText, _logger, out var hostDocumentIndex))
        {
            return null;
        }

        var triggerCharacterKind = _razorDocumentMappingService.GetLanguageKind(codeDocument, hostDocumentIndex, rightAssociative: false);
        if (triggerCharacterKind is not (RazorLanguageKind.CSharp or RazorLanguageKind.Html))
        {
            _logger.LogInformation("Unsupported trigger character language {triggerCharacterKind:G}.", triggerCharacterKind);
            return null;
        }

        if (!IsApplicableTriggerCharacter(request.Character, triggerCharacterKind))
        {
            // We were triggered but the trigger character doesn't make sense for the current cursor position. Bail.
            _logger.LogInformation("Unsupported trigger character location.");
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();

        Debug.Assert(request.Character.Length > 0);

        var formattedEdits = await _razorFormattingService.FormatOnTypeAsync(documentContext, triggerCharacterKind, Array.Empty<TextEdit>(), request.Options, hostDocumentIndex, request.Character[0], cancellationToken).ConfigureAwait(false);
        if (formattedEdits.Length == 0)
        {
            _logger.LogInformation("No formatting changes were necessary");
            return null;
        }

        _logger.LogInformation("Returning {formattingEditsLength} final formatted results.", formattedEdits.Length);
        return formattedEdits;
    }

    private static bool IsApplicableTriggerCharacter(string triggerCharacter, RazorLanguageKind languageKind)
    {
        if (languageKind == RazorLanguageKind.CSharp)
        {
            return s_csharpTriggerCharacters.Contains(triggerCharacter);
        }
        else if (languageKind == RazorLanguageKind.Html)
        {
            return s_htmlTriggerCharacters.Contains(triggerCharacter);
        }

        // Unknown trigger character.
        return false;
    }
}
