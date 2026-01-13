// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.DocumentPresentation;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentPresentation;

internal abstract class AbstractTextDocumentPresentationEndpointBase<TParams>(
    IDocumentMappingService documentMappingService,
    IClientConnection clientConnection,
    IFilePathService filePathService,
    ILogger logger) : IRazorRequestHandler<TParams, WorkspaceEdit?>, ICapabilitiesProvider
        where TParams : IPresentationParams
{
    private readonly IDocumentMappingService _documentMappingService = documentMappingService;
    private readonly IClientConnection _clientConnection = clientConnection;
    private readonly IFilePathService _filePathService = filePathService;
    private readonly ILogger _logger = logger;

    public abstract string EndpointName { get; }

    protected ILogger Logger => _logger;

    public bool MutatesSolutionState => false;

    public abstract void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities);

    protected abstract IRazorPresentationParams CreateRazorRequestParameters(TParams request);

    protected abstract Task<WorkspaceEdit?> TryGetRazorWorkspaceEditAsync(RazorLanguageKind languageKind, TParams request, CancellationToken cancellationToken);

    public abstract TextDocumentIdentifier GetTextDocumentIdentifier(TParams request);

    public async Task<WorkspaceEdit?> HandleRequestAsync(TParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.DocumentContext;
        if (documentContext is null)
        {
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        var sourceText = codeDocument.Source.Text;

        if (!sourceText.TryGetAbsoluteIndex(request.Range.Start, out var hostDocumentIndex))
        {
            return null;
        }

        var languageKind = codeDocument.GetLanguageKind(hostDocumentIndex, rightAssociative: false);
        // See if we can handle this directly in Razor. If not, we'll let things flow to the below delegated handling.
        var result = await TryGetRazorWorkspaceEditAsync(languageKind, request, cancellationToken).ConfigureAwait(false);
        if (result is not null)
        {
            return result;
        }

        if (languageKind is RazorLanguageKind.CSharp)
        {
            // Roslyn does not support Uri or Text presentation, so to prevent unnecessary LSP calls and misleading telemetry
            // reports, we just return null here.
            return null;
        }
        else if (languageKind is not RazorLanguageKind.Html)
        {
            _logger.LogInformation($"Unsupported language {languageKind}.");
            return null;
        }

        var requestParams = CreateRazorRequestParameters(request);

        requestParams.HostDocumentVersion = documentContext.Snapshot.Version;
        requestParams.Kind = languageKind;

        // For CSharp we need to map the range to the generated document
        if (languageKind == RazorLanguageKind.CSharp)
        {
            if (!_documentMappingService.TryMapToCSharpDocumentRange(codeDocument.GetRequiredCSharpDocument(), request.Range, out var projectedRange))
            {
                return null;
            }

            requestParams.Range = projectedRange;
        }

        var response = await _clientConnection.SendRequestAsync<IRazorPresentationParams, WorkspaceEdit?>(EndpointName, requestParams, cancellationToken).ConfigureAwait(false);
        if (response is null)
        {
            return null;
        }

        // The responses we get back will be for virtual documents, so we have to map them back to the real
        // document, and in the case of C#, map the returned ranges too
        MapWorkspaceEdit(response, mapRanges: languageKind == RazorLanguageKind.CSharp, codeDocument);

        return response;
    }

    private Dictionary<string, TextEdit[]> MapDocumentEdits(Dictionary<string, TextEdit[]> changes, bool mapRanges, RazorCodeDocument codeDocument)
    {
        var mappedChanges = new Dictionary<string, TextEdit[]>();
        foreach (var entry in changes)
        {
            var uri = new Uri(entry.Key);
            var edits = entry.Value;

            if (!_filePathService.IsVirtualDocumentUri(uri))
            {
                // This location doesn't point to a background razor file. No need to map.
                mappedChanges[entry.Key] = entry.Value;
                continue;
            }

            var mappedEdits = GetMappedTextEdits(mapRanges, codeDocument, edits.Select(e => (SumType<TextEdit, AnnotatedTextEdit>)e));
            if (mappedEdits.Length == 0)
            {
                // Nothing to do.
                continue;
            }

            var razorDocumentUri = _filePathService.GetRazorDocumentUri(uri);
            mappedChanges[razorDocumentUri.AbsoluteUri] = mappedEdits;
        }

        return mappedChanges;
    }

    private TextEdit[] GetMappedTextEdits(bool mapRanges, RazorCodeDocument codeDocument, IEnumerable<SumType<TextEdit, AnnotatedTextEdit>> edits)
    {
        using var mappedEdits = new PooledArrayBuilder<TextEdit>();
        if (!mapRanges)
        {
            mappedEdits.AddRange(edits.Select(e => (TextEdit)e));
        }
        else
        {
            foreach (var edit in edits)
            {
                if (!_documentMappingService.TryMapToRazorDocumentRange(codeDocument.GetRequiredCSharpDocument(), ((TextEdit)edit).Range, out var newRange))
                {
                    return [];
                }

                var newEdit = LspFactory.CreateTextEdit(newRange, ((TextEdit)edit).NewText);
                mappedEdits.Add(newEdit);
            }
        }

        return mappedEdits.ToArray();
    }

    private void MapWorkspaceEdit(WorkspaceEdit workspaceEdit, bool mapRanges, RazorCodeDocument codeDocument)
    {
        // Handle DocumentChanges - iterate through TextDocumentEdits and modify them in-place.
        // This preserves CreateFile, RenameFile, DeleteFile operations automatically since we don't create a new array.
        if (workspaceEdit.DocumentChanges is not null)
        {
            foreach (var textDocumentEdit in workspaceEdit.EnumerateTextDocumentEdits())
            {
                MapTextDocumentEditInPlace(textDocumentEdit, mapRanges, codeDocument);
            }
        }

        if (workspaceEdit.Changes is not null)
        {
            workspaceEdit.Changes = MapDocumentEdits(workspaceEdit.Changes, mapRanges, codeDocument);
        }
    }

    private void MapTextDocumentEditInPlace(TextDocumentEdit entry, bool mapRanges, RazorCodeDocument codeDocument)
    {
        var uri = entry.TextDocument.DocumentUri.GetRequiredParsedUri();
        if (!_filePathService.IsVirtualDocumentUri(uri))
        {
            // This location doesn't point to a background razor file. No need to map.
            return;
        }

        var edits = entry.Edits;
        var mappedEdits = GetMappedTextEdits(mapRanges, codeDocument, edits);

        var razorDocumentUri = _filePathService.GetRazorDocumentUri(uri);

        // Update the entry in-place
        entry.TextDocument = new OptionalVersionedTextDocumentIdentifier()
        {
            DocumentUri = new(razorDocumentUri),
        };
        entry.Edits = [.. mappedEdits];
    }

    protected record DocumentSnapshotAndVersion(IDocumentSnapshot Snapshot, int Version);
}
