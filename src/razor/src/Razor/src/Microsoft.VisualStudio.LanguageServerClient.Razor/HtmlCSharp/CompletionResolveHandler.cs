// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Logging;
using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;

[Shared]
[ExportLspMethod(Methods.TextDocumentCompletionResolveName)]
internal class CompletionResolveHandler : IRequestHandler<CompletionItem, CompletionItem>
{
    private readonly LSPRequestInvoker _requestInvoker;
    private readonly LSPDocumentManager _documentManager;
    private readonly LSPDocumentMappingProvider _documentMappingProvider;
    private readonly FormattingOptionsProvider _formattingOptionsProvider;
    private readonly CompletionRequestContextCache _completionRequestContextCache;
    private readonly ILogger _logger;

    [ImportingConstructor]
    public CompletionResolveHandler(
        LSPRequestInvoker requestInvoker,
        LSPDocumentManager documentManager,
        LSPDocumentMappingProvider documentMappingProvider,
        FormattingOptionsProvider formattingOptionsProvider,
        CompletionRequestContextCache completionRequestContextCache,
        HTMLCSharpLanguageServerLogHubLoggerProvider loggerProvider)
    {
        if (requestInvoker is null)
        {
            throw new ArgumentNullException(nameof(requestInvoker));
        }

        if (documentManager is null)
        {
            throw new ArgumentNullException(nameof(documentManager));
        }

        if (documentMappingProvider is null)
        {
            throw new ArgumentNullException(nameof(documentMappingProvider));
        }

        if (formattingOptionsProvider is null)
        {
            throw new ArgumentNullException(nameof(formattingOptionsProvider));
        }

        if (completionRequestContextCache is null)
        {
            throw new ArgumentNullException(nameof(completionRequestContextCache));
        }

        if (loggerProvider is null)
        {
            throw new ArgumentNullException(nameof(loggerProvider));
        }

        _requestInvoker = requestInvoker;
        _documentManager = documentManager;
        _documentMappingProvider = documentMappingProvider;
        _formattingOptionsProvider = formattingOptionsProvider;
        _completionRequestContextCache = completionRequestContextCache;

        _logger = loggerProvider.CreateLogger(nameof(CompletionResolveHandler));
    }

    public async Task<CompletionItem?> HandleRequestAsync(CompletionItem request, ClientCapabilities clientCapabilities, CancellationToken cancellationToken)
    {
        if (request.Data is null)
        {
            _logger.LogInformation("Received no completion resolve data.");
            return request;
        }

        _logger.LogInformation("Starting request to resolve completion.");

        var resolveData = request.Data is CompletionResolveData data ? data : ((JToken)request.Data).ToObject<CompletionResolveData>();

        if (resolveData is null)
        {
            _logger.LogInformation("CompletionResolveData failed to serialize.");
            return request;
        }

        // Set the original resolve data back so the language server deserializes it correctly.
        request.Data = resolveData.OriginalData;

        if (!_completionRequestContextCache.TryGet(resolveData.ResultId, out var requestContext))
        {
            _logger.LogInformation("Could not find the associated request context.");
            return request;
        }

        if (!_documentManager.TryGetDocument(requestContext.HostDocumentUri, out var documentSnapshot))
        {
            _logger.LogError("Could not find the associated host document for completion resolve: {hostDocumentUri}.", requestContext.HostDocumentUri);
            return request;
        }

        var serverKind = requestContext.LanguageServerKind;
        var languageServerName = serverKind.ToLanguageServerName();
        var textBuffer = serverKind.GetTextBuffer(documentSnapshot);
        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<CompletionItem, CompletionItem>(
            textBuffer,
            Methods.TextDocumentCompletionResolveName,
            languageServerName,
            request,
            cancellationToken).ConfigureAwait(false);

        if (!ReinvocationResponseHelper.TryExtractResultOrLog(response, _logger, RazorLSPConstants.RazorCSharpLanguageServerName, out var result))
        {
            return request;
        }

        _logger.LogInformation("Received result, post-processing.");

        var postProcessedResult = await PostProcessCompletionItemAsync(request, result, requestContext, documentSnapshot, cancellationToken).ConfigureAwait(false);
        _logger.LogInformation("Returning resolved completion.");
        return postProcessedResult;
    }

    private async Task<CompletionItem> PostProcessCompletionItemAsync(
        CompletionItem preResolveCompletionItem,
        CompletionItem resolvedCompletionItem,
        CompletionRequestContext requestContext,
        LSPDocumentSnapshot documentSnapshot,
        CancellationToken cancellationToken)
    {
        // This is a special contract between the Visual Studio LSP platform and language servers where if insert text and text edit's are not present
        // then the "resolve" endpoint is guaranteed to run prior to a completion item's content being comitted. This gives language servers the
        // opportunity to lazily evaluate text edits which in turn we need to remap. Given text edits generated through this mechanism tend to be
        // more extensive we do a full remapping gesture which includes formatting of said text-edits.
        var shouldRemapTextEdits = preResolveCompletionItem.InsertText is null && preResolveCompletionItem.TextEdit is null;
        if (!shouldRemapTextEdits)
        {
            _logger.LogInformation("No TextEdit remap required.");
            return resolvedCompletionItem;
        }

        _logger.LogInformation("Start formatting text edit.");

        var formattingOptions = _formattingOptionsProvider.GetOptions(documentSnapshot.Uri);
        if (formattingOptions is null)
        {
            return resolvedCompletionItem;
        }

        if (resolvedCompletionItem.TextEdit != null)
        {
            var containsSnippet = resolvedCompletionItem.InsertTextFormat == InsertTextFormat.Snippet;
            var remappedEdits = await _documentMappingProvider.RemapFormattedTextEditsAsync(
                requestContext.ProjectedDocumentUri,
                new[] { resolvedCompletionItem.TextEdit },
                formattingOptions,
                containsSnippet,
                cancellationToken).ConfigureAwait(false);

            // We only passed in a single edit to be remapped
            var remappedEdit = remappedEdits.Single();
            resolvedCompletionItem.TextEdit = remappedEdit;

            _logger.LogInformation("Formatted text edit.");
        }

        if (resolvedCompletionItem.AdditionalTextEdits != null)
        {
            var remappedEdits = await _documentMappingProvider.RemapFormattedTextEditsAsync(
                requestContext.ProjectedDocumentUri,
                resolvedCompletionItem.AdditionalTextEdits,
                formattingOptions,
                containsSnippet: false, // Additional text edits can't contain snippets
                cancellationToken).ConfigureAwait(false);

            resolvedCompletionItem.AdditionalTextEdits = remappedEdits;

            _logger.LogInformation("Formatted additional text edit.");
        }

        return resolvedCompletionItem;
    }
}
