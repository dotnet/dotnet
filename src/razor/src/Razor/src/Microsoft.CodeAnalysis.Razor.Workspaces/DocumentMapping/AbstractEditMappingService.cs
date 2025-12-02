// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.CodeAnalysis.Razor.DocumentMapping;

internal abstract class AbstractEditMappingService(
    IDocumentMappingService documentMappingService,
    IFilePathService filePathService) : IEditMappingService
{
    private readonly IDocumentMappingService _documentMappingService = documentMappingService;
    private readonly IFilePathService _filePathService = filePathService;

    public async Task MapWorkspaceEditAsync(IDocumentSnapshot contextDocumentSnapshot, WorkspaceEdit workspaceEdit, CancellationToken cancellationToken)
    {
        // Handle DocumentChanges - iterate through TextDocumentEdits and modify them in-place.
        // This preserves CreateFile, RenameFile, DeleteFile operations automatically since we don't create a new array.
        if (workspaceEdit.DocumentChanges is not null)
        {
            foreach (var textDocumentEdit in workspaceEdit.EnumerateTextDocumentEdits())
            {
                await MapTextDocumentEditAsync(contextDocumentSnapshot, textDocumentEdit, cancellationToken).ConfigureAwait(false);
            }
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

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

        // entry.Edits is SumType<TextEdit, AnnotatedTextEdit> but AnnotatedTextEdit inherits from TextEdit, so we can just cast
        var mappedEdits = GetMappedTextEdits(codeDocument.GetRequiredCSharpDocument(), [.. entry.Edits.Select(static e => (TextEdit)e)]);

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

            var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
            var mappedEdits = GetMappedTextEdits(codeDocument.GetRequiredCSharpDocument(), edits);
            if (mappedEdits.Length == 0)
            {
                // Nothing to do.
                continue;
            }

            mappedChanges[razorDocumentUri.AbsoluteUri] = mappedEdits;
        }

        return mappedChanges;
    }

    private TextEdit[] GetMappedTextEdits(RazorCSharpDocument csharpDocument, TextEdit[] edits)
    {
        using var mappedEdits = new PooledArrayBuilder<TextEdit>(edits.Length);

        foreach (var edit in edits)
        {
            var generatedRange = edit.Range;
            if (!_documentMappingService.TryMapToRazorDocumentRange(csharpDocument, generatedRange, MappingBehavior.Strict, out var hostDocumentRange))
            {
                // Can't map range. Discard this edit.
                continue;
            }

            mappedEdits.Add(LspFactory.CreateTextEdit(hostDocumentRange, edit.NewText));
        }

        return mappedEdits.ToArray();
    }

    protected abstract bool TryGetDocumentContext(IDocumentSnapshot contextDocumentSnapshot, Uri razorDocumentUri, VSProjectContext? projectContext, [NotNullWhen(true)] out DocumentContext? documentContext);

    protected abstract Task<Uri?> GetRazorDocumentUriAsync(IDocumentSnapshot contextDocumentSnapshot, Uri uri, CancellationToken cancellationToken);
}
