// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.InlineCompletion;

[RazorLanguageServerEndpoint(VSInternalMethods.TextDocumentInlineCompletionName)]
internal sealed class InlineCompletionEndpoint(
    IDocumentMappingService documentMappingService,
    IClientConnection clientConnection,
    RazorLSPOptionsMonitor optionsMonitor,
    ILoggerFactory loggerFactory)
    : IRazorRequestHandler<VSInternalInlineCompletionRequest, VSInternalInlineCompletionList?>, ICapabilitiesProvider
{
    private readonly IDocumentMappingService _documentMappingService = documentMappingService;
    private readonly IClientConnection _clientConnection = clientConnection;
    private readonly RazorLSPOptionsMonitor _optionsMonitor = optionsMonitor;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<InlineCompletionEndpoint>();

    public bool MutatesSolutionState => false;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.InlineCompletionOptions = new VSInternalInlineCompletionOptions().EnableInlineCompletion();
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
                // I don't think this makes a difference for inline completions, but it's better to be safe.
                useNewFormattingEngine: false);
            if (!SnippetFormatter.TryGetSnippetWithAdjustedIndentation(formattingContext, item.Text, hostDocumentIndex, out var newSnippetText))
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
}

