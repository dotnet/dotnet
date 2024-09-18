// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.Completion;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.InlineCompletion;

[RazorLanguageServerEndpoint(VSInternalMethods.TextDocumentInlineCompletionName)]
internal sealed class InlineCompletionEndpoint(
    IDocumentMappingService documentMappingService,
    IClientConnection clientConnection,
    IFormattingCodeDocumentProvider formattingCodeDocumentProvider,
    RazorLSPOptionsMonitor optionsMonitor,
    ILoggerFactory loggerFactory)
    : IRazorRequestHandler<VSInternalInlineCompletionRequest, VSInternalInlineCompletionList?>, ICapabilitiesProvider
{
    private static readonly ImmutableHashSet<string> s_cSharpKeywords = ImmutableHashSet.Create(
        "~", "Attribute", "checked", "class", "ctor", "cw", "do", "else", "enum", "equals", "Exception", "for", "foreach", "forr",
        "if", "indexer", "interface", "invoke", "iterator", "iterindex", "lock", "mbox", "namespace", "#if", "#region", "prop",
        "propfull", "propg", "sim", "struct", "svm", "switch", "try", "tryf", "unchecked", "unsafe", "using", "while");

    private readonly IDocumentMappingService _documentMappingService = documentMappingService;
    private readonly IClientConnection _clientConnection = clientConnection;
    private readonly IFormattingCodeDocumentProvider _formattingCodeDocumentProvider = formattingCodeDocumentProvider;
    private readonly RazorLSPOptionsMonitor _optionsMonitor = optionsMonitor;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<InlineCompletionEndpoint>();

    public bool MutatesSolutionState => false;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.InlineCompletionOptions = new VSInternalInlineCompletionOptions()
        {
            Pattern = new Regex(string.Join("|", s_cSharpKeywords))
        };
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(VSInternalInlineCompletionRequest request)
    {
        return request.TextDocument;
    }

    public async Task<VSInternalInlineCompletionList?> HandleRequestAsync(VSInternalInlineCompletionRequest request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        ArgHelper.ThrowIfNull(request);

        _logger.LogInformation($"Starting request for {request.TextDocument.Uri} at {request.Position}.");

        var documentContext = requestContext.DocumentContext;
        if (documentContext is null)
        {
            return null;
        }

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        if (codeDocument.IsUnsupported())
        {
            return null;
        }

        var sourceText = await documentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
        var hostDocumentIndex = sourceText.GetPosition(request.Position);

        var languageKind = codeDocument.GetLanguageKind(hostDocumentIndex, rightAssociative: false);

        // Map to the location in the C# document.
        if (languageKind != RazorLanguageKind.CSharp ||
            !_documentMappingService.TryMapToGeneratedDocumentPosition(codeDocument.GetCSharpDocument(), hostDocumentIndex, out Position? projectedPosition, out _))
        {
            _logger.LogInformation($"Unsupported location for {request.TextDocument.Uri}.");
            return null;
        }

        var razorRequest = new RazorInlineCompletionRequest
        {
            TextDocument = request.TextDocument,
            Context = request.Context,
            Position = projectedPosition,
            Kind = languageKind,
            Options = request.Options,
        };

        request.Position = projectedPosition;
        var list = await _clientConnection.SendRequestAsync<RazorInlineCompletionRequest, VSInternalInlineCompletionList?>(
            CustomMessageNames.RazorInlineCompletionEndpoint,
            razorRequest,
            cancellationToken).ConfigureAwait(false);
        if (list is null || !list.Items.Any())
        {
            _logger.LogInformation($"Did not get any inline completions from delegation.");
            return null;
        }

        using var items = new PooledArrayBuilder<VSInternalInlineCompletionItem>(list.Items.Length);
        foreach (var item in list.Items)
        {
            var range = item.Range ?? projectedPosition.ToZeroWidthRange();

            if (!_documentMappingService.TryMapToHostDocumentRange(codeDocument.GetCSharpDocument(), range, out var rangeInRazorDoc))
            {
                _logger.LogWarning($"Could not remap projected range {range} to razor document");
                continue;
            }

            var options = RazorFormattingOptions.From(request.Options, _optionsMonitor.CurrentValue.CodeBlockBraceOnNextLine);
            var formattingContext = FormattingContext.Create(
                documentContext.Snapshot,
                codeDocument,
                options,
                _formattingCodeDocumentProvider);
            if (!TryGetSnippetWithAdjustedIndentation(formattingContext, item.Text, hostDocumentIndex, out var newSnippetText))
            {
                continue;
            }

            var remappedItem = new VSInternalInlineCompletionItem
            {
                Command = item.Command,
                Range = rangeInRazorDoc,
                Text = newSnippetText.ToString(),
                TextFormat = item.TextFormat,
            };
            items.Add(remappedItem);
        }

        if (items.Count == 0)
        {
            _logger.LogInformation($"Could not format / map the items from delegation.");
            return null;
        }

        _logger.LogInformation($"Returning {items.Count} items.");
        return new VSInternalInlineCompletionList
        {
            Items = items.ToArray()
        };
    }

    private static bool TryGetSnippetWithAdjustedIndentation(FormattingContext formattingContext, string snippetText, int hostDocumentIndex, [NotNullWhen(true)] out string? newSnippetText)
    {
        newSnippetText = null;
        if (!formattingContext.TryGetFormattingSpan(hostDocumentIndex, out var formattingSpan))
        {
            return false;
        }

        // Take the amount of indentation razor and html are adding, then remove the amount of C# indentation that is 'hidden'.
        // This should give us the desired base indentation that must be applied to each line.
        var razorAndHtmlContributionsToIndentation = formattingSpan.RazorIndentationLevel + formattingSpan.HtmlIndentationLevel;
        var amountToAddToCSharpIndentation = razorAndHtmlContributionsToIndentation - formattingSpan.MinCSharpIndentLevel;

        var snippetSourceText = SourceText.From(snippetText);
        List<TextChange> indentationChanges = new();
        // Adjust each line, skipping the first since it must start at the snippet keyword.
        foreach (var line in snippetSourceText.Lines.Skip(1))
        {
            var lineText = snippetSourceText.GetSubText(line.Span);
            if (lineText.Length == 0)
            {
                // We just have an empty line, nothing to do.
                continue;
            }

            // Get the indentation of the line in the C# document based on what options the C# document was generated with.
            var csharpLineIndentationSize = line.GetIndentationSize(formattingContext.Options.TabSize);
            var csharpIndentationLevel = csharpLineIndentationSize / formattingContext.Options.TabSize;

            // Get the new indentation level based on the context in the razor document.
            var newIndentationLevel = csharpIndentationLevel + amountToAddToCSharpIndentation;
            var newIndentationString = formattingContext.GetIndentationLevelString(newIndentationLevel);

            // Replace the current indentation with the new indentation.
            var spanToReplace = new TextSpan(line.Start, line.GetFirstNonWhitespaceOffset() ?? line.Span.End);
            var textChange = new TextChange(spanToReplace, newIndentationString);
            indentationChanges.Add(textChange);
        }

        var newSnippetSourceText = snippetSourceText.WithChanges(indentationChanges);
        newSnippetText = newSnippetSourceText.ToString();
        return true;
    }
}

