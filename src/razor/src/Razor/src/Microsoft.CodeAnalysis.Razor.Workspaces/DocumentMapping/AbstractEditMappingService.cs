// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.DocumentMapping;

internal abstract class AbstractEditMappingService(
    IDocumentMappingService documentMappingService,
    IFilePathService filePathService) : IEditMappingService
{
    private readonly IDocumentMappingService _documentMappingService = documentMappingService;
    private readonly IFilePathService _filePathService = filePathService;

    public async Task<WorkspaceEdit> RemapWorkspaceEditAsync(IDocumentSnapshot contextDocumentSnapshot, WorkspaceEdit workspaceEdit, CancellationToken cancellationToken)
    {
        if (workspaceEdit.TryGetTextDocumentEdits(out var documentEdits))
        {
            // The LSP spec says, we should prefer `DocumentChanges` property over `Changes` if available.
            var remappedEdits = await RemapTextDocumentEditsAsync(contextDocumentSnapshot, documentEdits, cancellationToken).ConfigureAwait(false);

            return new WorkspaceEdit()
            {
                DocumentChanges = remappedEdits
            };
        }

        if (workspaceEdit.Changes is { } changeMap)
        {
            var remappedEdits = await RemapDocumentEditsAsync(contextDocumentSnapshot, changeMap, cancellationToken).ConfigureAwait(false);

            return new WorkspaceEdit()
            {
                Changes = remappedEdits
            };
        }

        return workspaceEdit;
    }

    private async Task<Dictionary<string, TextEdit[]>> RemapDocumentEditsAsync(IDocumentSnapshot contextDocumentSnapshot, Dictionary<string, TextEdit[]> changes, CancellationToken cancellationToken)
    {
        var remappedChanges = new Dictionary<string, TextEdit[]>(capacity: changes.Count);

        foreach (var (uriString, edits) in changes)
        {
            var uri = new Uri(uriString);

            // Check if the edit is actually for a generated document, because if not we don't need to do anything
            if (!_filePathService.IsVirtualDocumentUri(uri))
            {
                remappedChanges[uriString] = edits;
                continue;
            }

            if (!TryGetDocumentContext(contextDocumentSnapshot, uri, projectContext: null, out var documentContext))
            {
                continue;
            }

            var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
            var remappedEdits = RemapTextEditsCore(uri, codeDocument, edits);
            if (remappedEdits.Length == 0)
            {
                // Nothing to do.
                continue;
            }

            var razorDocumentUri = _filePathService.GetRazorDocumentUri(uri);
            remappedChanges[razorDocumentUri.AbsoluteUri] = remappedEdits;
        }

        return remappedChanges;
    }

    private TextEdit[] RemapTextEditsCore(Uri generatedDocumentUri, RazorCodeDocument codeDocument, TextEdit[] edits)
    {
        if (!codeDocument.TryGetGeneratedDocument(generatedDocumentUri, _filePathService, out var generatedDocument))
        {
            return edits;
        }

        using var remappedEdits = new PooledArrayBuilder<TextEdit>(edits.Length);

        foreach (var edit in edits)
        {
            var generatedRange = edit.Range;
            if (!_documentMappingService.TryMapToHostDocumentRange(generatedDocument, generatedRange, MappingBehavior.Strict, out var hostDocumentRange))
            {
                // Can't map range. Discard this edit.
                continue;
            }

            var remappedEdit = VsLspFactory.CreateTextEdit(hostDocumentRange, edit.NewText);
            remappedEdits.Add(remappedEdit);
        }

        return remappedEdits.ToArray();
    }

    private async Task<TextDocumentEdit[]> RemapTextDocumentEditsAsync(IDocumentSnapshot contextDocumentSnapshot, TextDocumentEdit[] documentEdits, CancellationToken cancellationToken)
    {
        using var remappedDocumentEdits = new PooledArrayBuilder<TextDocumentEdit>(documentEdits.Length);

        foreach (var entry in documentEdits)
        {
            var generatedDocumentUri = entry.TextDocument.Uri;

            // Check if the edit is actually for a generated document, because if not we don't need to do anything
            if (!_filePathService.IsVirtualDocumentUri(generatedDocumentUri))
            {
                // This location doesn't point to a background razor file. No need to remap.
                remappedDocumentEdits.Add(entry);
                continue;
            }

            var razorDocumentUri = _filePathService.GetRazorDocumentUri(generatedDocumentUri);

            if (!TryGetDocumentContext(contextDocumentSnapshot, razorDocumentUri, entry.TextDocument.GetProjectContext(), out var documentContext))
            {
                continue;
            }

            var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

            var remappedEdits = RemapTextEditsCore(generatedDocumentUri, codeDocument, entry.Edits);
            if (remappedEdits.Length == 0)
            {
                // Nothing to do.
                continue;
            }

            remappedDocumentEdits.Add(new()
            {
                TextDocument = new OptionalVersionedTextDocumentIdentifier()
                {
                    Uri = razorDocumentUri,
                },
                Edits = remappedEdits
            });
        }

        return remappedDocumentEdits.ToArray();
    }

    protected abstract bool TryGetDocumentContext(IDocumentSnapshot contextDocumentSnapshot, Uri razorDocumentUri, VSProjectContext? projectContext, [NotNullWhen(true)] out DocumentContext? documentContext);
}
