// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.DocumentMapping;

internal abstract class AbstractEditMappingService(
    IDocumentMappingService documentMappingService,
    ITelemetryReporter telemetryReporter,
    IFilePathService filePathService) : IEditMappingService
{
    private readonly IDocumentMappingService _documentMappingService = documentMappingService;
    private readonly ITelemetryReporter _telemetryReporter = telemetryReporter;
    private readonly IFilePathService _filePathService = filePathService;

    public async Task MapWorkspaceEditAsync(IDocumentSnapshot contextDocumentSnapshot, WorkspaceEdit workspaceEdit, CancellationToken cancellationToken)
    {
        if (workspaceEdit.DocumentChanges is not null)
        {
            using var builder = new PooledArrayBuilder<SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>>();
            foreach (var edit in workspaceEdit.EnumerateEdits())
            {
                if (edit.TryGetFirst(out var textDocumentEdit))
                {
                    await MapTextDocumentEditAsync(contextDocumentSnapshot, textDocumentEdit, cancellationToken).ConfigureAwait(false);
                    if (textDocumentEdit.Edits.Length == 0)
                    {
                        continue;
                    }
                }

                builder.Add(edit);
            }

            workspaceEdit.DocumentChanges = builder.ToArrayAndClear();
        }

        if (workspaceEdit.Changes is { } changeMap)
        {
            workspaceEdit.Changes = await MapDocumentEditsAsync(contextDocumentSnapshot, changeMap, cancellationToken).ConfigureAwait(false);
        }
    }

    private async Task MapTextDocumentEditAsync(IDocumentSnapshot contextDocumentSnapshot, TextDocumentEdit entry, CancellationToken cancellationToken)
    {
        var generatedDocumentUri = entry.TextDocument.DocumentUri.GetRequiredParsedUri();

        // For Html we just map the Uri, the range will be the same
        if (_filePathService.IsVirtualHtmlFile(generatedDocumentUri))
        {
            var razorUri = _filePathService.GetRazorDocumentUri(generatedDocumentUri);
            entry.TextDocument = new OptionalVersionedTextDocumentIdentifier()
            {
                DocumentUri = new(razorUri),
            };
            return;
        }

        // Check if the edit is actually for a generated document, because if not we don't need to do anything
        if (!_filePathService.IsVirtualCSharpFile(generatedDocumentUri))
        {
            // This location doesn't point to a background razor file. No need to map.
            return;
        }

        var razorDocumentUri = await GetRazorDocumentUriAsync(contextDocumentSnapshot, generatedDocumentUri, cancellationToken).ConfigureAwait(false);
        if (razorDocumentUri is null)
        {
            return;
        }

        if (!TryGetDocumentContext(contextDocumentSnapshot, razorDocumentUri, entry.TextDocument.GetProjectContext(), out var documentContext))
        {
            return;
        }

        // entry.Edits is SumType<TextEdit, AnnotatedTextEdit> but AnnotatedTextEdit inherits from TextEdit, so we can just cast
        var mappedEdits = await GetMappedTextEditsAsync(documentContext, [.. entry.Edits.Select(static e => (TextEdit)e)], cancellationToken).ConfigureAwait(false);

        // Update the entry in-place
        entry.TextDocument = new OptionalVersionedTextDocumentIdentifier()
        {
            DocumentUri = new(razorDocumentUri),
        };
        entry.Edits = [.. mappedEdits.Select(static e => new SumType<TextEdit, AnnotatedTextEdit>(e))];
    }

    private async Task<Dictionary<string, TextEdit[]>> MapDocumentEditsAsync(IDocumentSnapshot contextDocumentSnapshot, Dictionary<string, TextEdit[]> changes, CancellationToken cancellationToken)
    {
        var mappedChanges = new Dictionary<string, TextEdit[]>(capacity: changes.Count);

        foreach (var (uriString, edits) in changes)
        {
            var generatedDocumentUri = new Uri(uriString);

            // For Html we just map the Uri, the range will be the same
            if (_filePathService.IsVirtualHtmlFile(generatedDocumentUri))
            {
                var razorUri = _filePathService.GetRazorDocumentUri(generatedDocumentUri);
                mappedChanges[razorUri.AbsoluteUri] = edits;
            }

            // Check if the edit is actually for a generated document, because if not we don't need to do anything
            if (!_filePathService.IsVirtualCSharpFile(generatedDocumentUri))
            {
                mappedChanges[uriString] = edits;
                continue;
            }

            var razorDocumentUri = await GetRazorDocumentUriAsync(contextDocumentSnapshot, generatedDocumentUri, cancellationToken).ConfigureAwait(false);
            if (razorDocumentUri is null)
            {
                continue;
            }

            if (!TryGetDocumentContext(contextDocumentSnapshot, razorDocumentUri, projectContext: null, out var documentContext))
            {
                continue;
            }

            var mappedEdits = await GetMappedTextEditsAsync(documentContext, edits, cancellationToken).ConfigureAwait(false);
            if (mappedEdits.Length == 0)
            {
                // Nothing to do.
                continue;
            }

            mappedChanges[razorDocumentUri.AbsoluteUri] = mappedEdits;
        }

        return mappedChanges;
    }

    private async Task<TextEdit[]> GetMappedTextEditsAsync(DocumentContext documentContext, TextEdit[] edits, CancellationToken cancellationToken)
    {
        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

        var razorSourceText = codeDocument.Source.Text;
        var csharpSourceText = codeDocument.GetCSharpSourceText();
        var textChanges = edits.SelectAsArray(e => new RazorTextChange
        {
            Span = csharpSourceText.GetTextSpan(e.Range).ToRazorTextSpan(),
            NewText = e.NewText,
        });
        var mappedEdits = await RazorEditHelper.MapCSharpEditsAsync(textChanges, documentContext.Snapshot, _documentMappingService, _telemetryReporter, cancellationToken).ConfigureAwait(false);

        return [.. mappedEdits.Select(e => LspFactory.CreateTextEdit(razorSourceText.GetLinePositionSpan(e.Span.ToTextSpan()), e.NewText.AssumeNotNull()))];
    }

    protected abstract bool TryGetDocumentContext(IDocumentSnapshot contextDocumentSnapshot, Uri razorDocumentUri, VSProjectContext? projectContext, [NotNullWhen(true)] out DocumentContext? documentContext);

    protected abstract Task<Uri?> GetRazorDocumentUriAsync(IDocumentSnapshot contextDocumentSnapshot, Uri uri, CancellationToken cancellationToken);
}
