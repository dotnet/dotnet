// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentSynchronization;

[LanguageServerEndpoint(Methods.TextDocumentDidChangeName)]
internal class DocumentDidChangeEndpoint(
    ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
    RazorProjectService razorProjectService,
    IRazorLoggerFactory loggerFactory)
    : IRazorNotificationHandler<DidChangeTextDocumentParams>, ITextDocumentIdentifierHandler<DidChangeTextDocumentParams, TextDocumentIdentifier>, ICapabilitiesProvider
{
    public bool MutatesSolutionState => true;

    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
    private readonly RazorProjectService _projectService = razorProjectService;
    private readonly ILogger _logger = loggerFactory.CreateLogger<DocumentDidChangeEndpoint>();

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
        var documentContext = requestContext.GetRequiredDocumentContext();

        var sourceText = await documentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
        sourceText = ApplyContentChanges(request.ContentChanges, sourceText);

        await _projectSnapshotManagerDispatcher.RunOnDispatcherThreadAsync(
            () => _projectService.UpdateDocument(documentContext.FilePath, sourceText, request.TextDocument.Version),
            cancellationToken).ConfigureAwait(false);
    }

    // Internal for testing
    internal SourceText ApplyContentChanges(IEnumerable<TextDocumentContentChangeEvent> contentChanges, SourceText sourceText)
    {
        foreach (var change in contentChanges)
        {
            if (change.Range is null)
            {
                throw new ArgumentNullException(nameof(change.Range), "Range of change should not be null.");
            }

            if (!change.Range.Start.TryGetAbsoluteIndex(sourceText, _logger, out var startPosition))
            {
                continue;
            }

            if (!change.Range.End.TryGetAbsoluteIndex(sourceText, _logger, out var endPosition))
            {
                continue;
            }

            var textSpan = new TextSpan(startPosition, change.RangeLength ?? endPosition - startPosition);
            var textChange = new TextChange(textSpan, change.Text);

            _logger.LogInformation("Applying {textChange}", textChange);

            // If there happens to be multiple text changes we generate a new source text for each one. Due to the
            // differences in VSCode and Roslyn's representation we can't pass in all changes simultaneously because
            // ordering may differ.
            sourceText = sourceText.WithChanges(textChange);
        }

        return sourceText;
    }
}
