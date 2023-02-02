// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentPresentation;

internal abstract class AbstractTextDocumentPresentationEndpointBase<TParams> : IRazorRequestHandler<TParams, WorkspaceEdit?>, IRegistrationExtension
    where TParams : IPresentationParams
{
    private readonly RazorDocumentMappingService _razorDocumentMappingService;
    private readonly ClientNotifierServiceBase _languageServer;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions;

    protected AbstractTextDocumentPresentationEndpointBase(
        RazorDocumentMappingService razorDocumentMappingService,
        ClientNotifierServiceBase languageServer,
        LanguageServerFeatureOptions languageServerFeatureOptions)
    {
        if (razorDocumentMappingService is null)
        {
            throw new ArgumentNullException(nameof(razorDocumentMappingService));
        }

        if (languageServer is null)
        {
            throw new ArgumentNullException(nameof(languageServer));
        }

        if (languageServerFeatureOptions is null)
        {
            throw new ArgumentNullException(nameof(languageServerFeatureOptions));
        }

        _razorDocumentMappingService = razorDocumentMappingService;
        _languageServer = languageServer;
        _languageServerFeatureOptions = languageServerFeatureOptions;
    }

    public abstract string EndpointName { get; }

    public bool MutatesSolutionState => false;

    public abstract RegistrationExtensionResult GetRegistration(VSInternalClientCapabilities clientCapabilities);

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
        if (codeDocument.IsUnsupported())
        {
            requestContext.Logger.LogWarning("Failed to retrieve generated output for document {request.TextDocument.Uri}.", request.TextDocument.Uri);
            return null;
        }

        var sourceText = await documentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
        if (request.Range.Start.TryGetAbsoluteIndex(sourceText, requestContext.Logger, out var hostDocumentIndex) != true)
        {
            return null;
        }

        var languageKind = _razorDocumentMappingService.GetLanguageKind(codeDocument, hostDocumentIndex, rightAssociative: false);
        // See if we can handle this directly in Razor. If not, we'll let things flow to the below delegated handling.
        var result = await TryGetRazorWorkspaceEditAsync(languageKind, request, cancellationToken).ConfigureAwait(false);
        if (result is not null)
        {
            return result;
        }

        if (languageKind is not (RazorLanguageKind.CSharp or RazorLanguageKind.Html))
        {
            requestContext.Logger.LogInformation("Unsupported language {languageKind}.", languageKind);
            return null;
        }

        var requestParams = CreateRazorRequestParameters(request);

        requestParams.HostDocumentVersion = documentContext.Version;
        requestParams.Kind = languageKind;

        // For CSharp we need to map the range to the generated document
        if (languageKind == RazorLanguageKind.CSharp)
        {
            if (!_razorDocumentMappingService.TryMapToProjectedDocumentRange(codeDocument.GetCSharpDocument(), request.Range, out var projectedRange))
            {
                return null;
            }

            requestParams.Range = projectedRange;
        }

        var response = await _languageServer.SendRequestAsync<IRazorPresentationParams, WorkspaceEdit?>(EndpointName, requestParams, cancellationToken).ConfigureAwait(false);
        if (response is null)
        {
            return null;
        }

        // The responses we get back will be for virtual documents, so we have to map them back to the real
        // document, and in the case of C#, map the returned ranges too
        var edit = MapWorkspaceEdit(response, mapRanges: languageKind == RazorLanguageKind.CSharp, codeDocument, documentContext.Version);

        return edit;
    }

    private static bool TryGetDocumentChanges(WorkspaceEdit workspaceEdit, [NotNullWhen(true)] out TextDocumentEdit[]? documentChanges)
    {
        if (workspaceEdit.DocumentChanges?.Value is TextDocumentEdit[] documentEdits)
        {
            documentChanges = documentEdits;
            return true;
        }

        if (workspaceEdit.DocumentChanges?.Value is SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>[] sumTypeArray)
        {
            var documentEditList = new List<TextDocumentEdit>();
            foreach (var sumType in sumTypeArray)
            {
                if (sumType.Value is TextDocumentEdit textDocumentEdit)
                {
                    documentEditList.Add(textDocumentEdit);
                }
            }

            if (documentEditList.Count > 0)
            {
                documentChanges = documentEditList.ToArray();
                return true;
            }
        }

        documentChanges = null;
        return false;
    }

    private Dictionary<string, TextEdit[]> MapChanges(Dictionary<string, TextEdit[]> changes, bool mapRanges, RazorCodeDocument codeDocument)
    {
        var remappedChanges = new Dictionary<string, TextEdit[]>();
        foreach (var entry in changes)
        {
            var uri = new Uri(entry.Key);
            var edits = entry.Value;

            if (!_languageServerFeatureOptions.IsVirtualDocumentUri(uri))
            {
                // This location doesn't point to a background razor file. No need to remap.
                remappedChanges[entry.Key] = entry.Value;
                continue;
            }

            var remappedEdits = MapTextEdits(mapRanges, codeDocument, edits);
            if (remappedEdits is null || remappedEdits.Length == 0)
            {
                // Nothing to do.
                continue;
            }

            var razorDocumentUri = _languageServerFeatureOptions.GetRazorDocumentUri(uri);
            remappedChanges[razorDocumentUri.AbsoluteUri] = remappedEdits;
        }

        return remappedChanges;
    }

    private TextDocumentEdit[] MapDocumentChanges(TextDocumentEdit[] documentEdits, bool mapRanges, RazorCodeDocument codeDocument, int hostDocumentVersion)
    {
        var remappedDocumentEdits = new List<TextDocumentEdit>();
        foreach (var entry in documentEdits)
        {
            var uri = entry.TextDocument.Uri;
            if (!_languageServerFeatureOptions.IsVirtualDocumentUri(uri))
            {
                // This location doesn't point to a background razor file. No need to remap.
                remappedDocumentEdits.Add(entry);

                continue;
            }

            var edits = entry.Edits;
            var remappedEdits = MapTextEdits(mapRanges, codeDocument, edits);
            if (remappedEdits is null || remappedEdits.Length == 0)
            {
                // Nothing to do.
                continue;
            }

            var razorDocumentUri = _languageServerFeatureOptions.GetRazorDocumentUri(uri);
            remappedDocumentEdits.Add(new TextDocumentEdit()
            {
                TextDocument = new OptionalVersionedTextDocumentIdentifier()
                {
                    Uri = razorDocumentUri,
                    Version = hostDocumentVersion
                },
                Edits = remappedEdits
            });
        }

        return remappedDocumentEdits.ToArray();
    }

    private TextEdit[]? MapTextEdits(bool mapRanges, RazorCodeDocument codeDocument, IEnumerable<TextEdit> edits)
    {
        if (!mapRanges)
        {
            return edits.ToArray();
        }

        var mappedEdits = new List<TextEdit>();
        foreach (var edit in edits)
        {
            if (!_razorDocumentMappingService.TryMapFromProjectedDocumentRange(codeDocument.GetCSharpDocument(), edit.Range, out var newRange))
            {
                return null;
            }

            var newEdit = new TextEdit()
            {
                NewText = edit.NewText,
                Range = newRange
            };
            mappedEdits.Add(newEdit);
        }

        return mappedEdits.ToArray();
    }

    private WorkspaceEdit? MapWorkspaceEdit(WorkspaceEdit workspaceEdit, bool mapRanges, RazorCodeDocument codeDocument, int hostDocumentVersion)
    {
        if (TryGetDocumentChanges(workspaceEdit, out var documentChanges))
        {
            // The LSP spec says, we should prefer `DocumentChanges` property over `Changes` if available.
            var remappedEdits = MapDocumentChanges(documentChanges, mapRanges, codeDocument, hostDocumentVersion);
            return new WorkspaceEdit()
            {
                DocumentChanges = remappedEdits
            };
        }
        else if (workspaceEdit.Changes != null)
        {
            var remappedEdits = MapChanges(workspaceEdit.Changes, mapRanges, codeDocument);
            return new WorkspaceEdit()
            {
                Changes = remappedEdits
            };
        }

        return workspaceEdit;
    }

    protected record DocumentSnapshotAndVersion(DocumentSnapshot Snapshot, int Version);
}
