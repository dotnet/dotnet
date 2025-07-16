// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CommonLanguageServerProtocol.Framework;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentSynchronization;

[RazorLanguageServerEndpoint(Methods.TextDocumentDidChangeName)]
internal class DocumentDidChangeEndpoint(
    IRazorProjectService razorProjectService,
    ILoggerFactory loggerFactory)
    : IRazorNotificationHandler<DidChangeTextDocumentParams>, ITextDocumentIdentifierHandler<DidChangeTextDocumentParams, TextDocumentIdentifier>, ICapabilitiesProvider
{
    public bool MutatesSolutionState => true;

    private readonly IRazorProjectService _projectService = razorProjectService;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<DocumentDidChangeEndpoint>();

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.TextDocumentSync = new TextDocumentSyncOptions()
        {
            Change = TextDocumentSyncKind.Incremental,
            OpenClose = true,
            Save = new SaveOptions()
            {
                IncludeText = true,
            },
            WillSave = false,
            WillSaveWaitUntil = false,
        };
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(DidChangeTextDocumentParams request)
    {
        return request.TextDocument;
    }

    public async Task HandleNotificationAsync(DidChangeTextDocumentParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.DocumentContext;
        if (documentContext is null)
        {
            _logger.LogWarning($"Could not find a document context for didChange on '{request.TextDocument.DocumentUri}'");
            Debug.Fail($"Could not find a document context for didChange on '{request.TextDocument.DocumentUri}'");
            return;
        }

        var sourceText = await documentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
        sourceText = ApplyContentChanges(request.ContentChanges, sourceText);

        await _projectService
            .UpdateDocumentAsync(documentContext.FilePath, sourceText, cancellationToken)
            .ConfigureAwait(false);
    }

    // Internal for testing
    internal SourceText ApplyContentChanges(IEnumerable<TextDocumentContentChangeEvent> contentChanges, SourceText sourceText)
    {
        foreach (var change in contentChanges)
        {
            var range = change.Range.AssumeNotNull();

            if (!sourceText.TryGetAbsoluteIndex(range.Start, out var startPosition))
            {
                continue;
            }

            if (!sourceText.TryGetAbsoluteIndex(range.End, out var endPosition))
            {
                continue;
            }

            var textSpan = new TextSpan(startPosition, change.RangeLength ?? endPosition - startPosition);
            var textChange = new TextChange(textSpan, change.Text);

            _logger.LogInformation($"Applying {textChange}");

            // If there happens to be multiple text changes we generate a new source text for each one. Due to the
            // differences in VSCode and Roslyn's representation we can't pass in all changes simultaneously because
            // ordering may differ.
            sourceText = sourceText.WithChanges(textChange);
        }

        return sourceText;
    }
}
