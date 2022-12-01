// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Composition;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Extensions;
using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;

[Shared]
[Export(typeof(LSPDocumentMappingProvider))]
internal class DefaultLSPDocumentMappingProvider : LSPDocumentMappingProvider
{
    private static readonly TextEdit[] s_emptyEdits = Array.Empty<TextEdit>();

    private readonly LSPRequestInvoker _requestInvoker;

    // Lazy loading the document manager to get around circular dependencies
    // The Document Manager is a more "Core Service" it depends on the ChangeTriggers which require the LSPDocumentMappingProvider
    // LSPDocumentManager => LSPDocumentMappingProvider => LSPDocumentManagerChangeTrigger => LSPDocumentManager
    private readonly Lazy<LSPDocumentManager> _lazyDocumentManager;
    private readonly RazorLSPConventions _razorConventions;

    [ImportingConstructor]
    public DefaultLSPDocumentMappingProvider(
        LSPRequestInvoker requestInvoker,
        Lazy<LSPDocumentManager> lazyDocumentManager,
        RazorLSPConventions razorConventions)
    {
        if (requestInvoker is null)
        {
            throw new ArgumentNullException(nameof(requestInvoker));
        }

        if (lazyDocumentManager is null)
        {
            throw new ArgumentNullException(nameof(lazyDocumentManager));
        }

        if (razorConventions is null)
        {
            throw new ArgumentNullException(nameof(razorConventions));
        }

        _requestInvoker = requestInvoker;
        _lazyDocumentManager = lazyDocumentManager;
        _razorConventions = razorConventions;
    }

    public override Task<RazorMapToDocumentRangesResponse?> MapToDocumentRangesAsync(RazorLanguageKind languageKind, Uri razorDocumentUri, Range[] projectedRanges, CancellationToken cancellationToken)
        => MapToDocumentRangesAsync(languageKind, razorDocumentUri, projectedRanges, LanguageServerMappingBehavior.Strict, cancellationToken);

    public async override Task<RazorMapToDocumentRangesResponse?> MapToDocumentRangesAsync(
        RazorLanguageKind languageKind,
        Uri razorDocumentUri,
        Range[] projectedRanges,
        LanguageServerMappingBehavior mappingBehavior,
        CancellationToken cancellationToken)
    {
        if (razorDocumentUri is null)
        {
            throw new ArgumentNullException(nameof(razorDocumentUri));
        }

        if (projectedRanges is null)
        {
            throw new ArgumentNullException(nameof(projectedRanges));
        }

        var mapToDocumentRangeParams = new RazorMapToDocumentRangesParams()
        {
            Kind = languageKind,
            RazorDocumentUri = razorDocumentUri,
            ProjectedRanges = projectedRanges,
            MappingBehavior = mappingBehavior,
        };

        if (!_lazyDocumentManager.Value.TryGetDocument(razorDocumentUri, out var documentSnapshot))
        {
            return null;
        }

        var documentMappingResponse = await _requestInvoker.ReinvokeRequestOnServerAsync<RazorMapToDocumentRangesParams, RazorMapToDocumentRangesResponse>(
            documentSnapshot.Snapshot.TextBuffer,
            LanguageServerConstants.RazorMapToDocumentRangesEndpoint,
            RazorLSPConstants.RazorLanguageServerName,
            CheckRazorRangeMappingCapability,
            mapToDocumentRangeParams,
            cancellationToken).ConfigureAwait(false);

        return documentMappingResponse?.Response;
    }

    public async override Task<Location[]> RemapLocationsAsync(Location[] locations, CancellationToken cancellationToken)
    {
        if (locations is null)
        {
            throw new ArgumentNullException(nameof(locations));
        }

        var remappedLocations = new List<Location>();
        foreach (var location in locations)
        {
            var uri = location.Uri;
            RazorLanguageKind languageKind;
            if (_razorConventions.IsVirtualCSharpFile(uri))
            {
                languageKind = RazorLanguageKind.CSharp;
            }
            else if (_razorConventions.IsVirtualHtmlFile(uri))
            {
                languageKind = RazorLanguageKind.Html;
            }
            else
            {
                // This location doesn't point to a virtual razor file. No need to remap.
                remappedLocations.Add(location);
                continue;
            }

            var razorDocumentUri = _razorConventions.GetRazorDocumentUri(uri);

            var mappingResult = await MapToDocumentRangesAsync(
                languageKind,
                razorDocumentUri,
                new[] { location.Range },
                cancellationToken).ConfigureAwait(false);

            cancellationToken.ThrowIfCancellationRequested();

            if (mappingResult is null ||
                (_lazyDocumentManager.Value.TryGetDocument(razorDocumentUri, out var documentSnapshot) &&
                mappingResult.HostDocumentVersion != documentSnapshot.Version))
            {
                // Couldn't remap the location or the document changed in the meantime. Discard these ranges.
                continue;
            }

            var remappedRange = mappingResult.Ranges[0];
            if (remappedRange.IsUndefined())
            {
                // Couldn't remap the range correctly. Discard this range.
                continue;
            }

            // We update the deserialized object, rather than create a new one, in case one of the JSON converters
            // gave us a different type that is a superset of Location
            location.Uri = razorDocumentUri;
            location.Range = remappedRange;

            remappedLocations.Add(location);
        }

        return remappedLocations.ToArray();
    }

    public async override Task<TextEdit[]> RemapTextEditsAsync(Uri uri, TextEdit[] edits, CancellationToken cancellationToken)
    {
        if (uri is null)
        {
            throw new ArgumentNullException(nameof(uri));
        }

        if (edits is null)
        {
            throw new ArgumentNullException(nameof(edits));
        }

        if (!_razorConventions.IsVirtualCSharpFile(uri) && !_razorConventions.IsVirtualHtmlFile(uri))
        {
            // This is not a virtual razor file. No need to remap.
            return edits;
        }

        var (_, remappedEdits) = await RemapTextEditsCoreAsync(uri, edits, TextEditKind.Default, cancellationToken).ConfigureAwait(false);
        return remappedEdits;
    }

    public async override Task<TextEdit[]> RemapFormattedTextEditsAsync(Uri uri, TextEdit[] edits, FormattingOptions options, bool containsSnippet, CancellationToken cancellationToken)
    {
        if (uri is null)
        {
            throw new ArgumentNullException(nameof(uri));
        }

        if (edits is null)
        {
            throw new ArgumentNullException(nameof(edits));
        }

        if (!_razorConventions.IsVirtualCSharpFile(uri) && !_razorConventions.IsVirtualHtmlFile(uri))
        {
            // This is not a virtual razor file. No need to remap.
            return edits;
        }

        var textEditKind = containsSnippet ? TextEditKind.Snippet : TextEditKind.FormatOnType;
        var (_, remappedEdits) = await RemapTextEditsCoreAsync(uri, edits, textEditKind, cancellationToken, formattingOptions: options).ConfigureAwait(false);
        return remappedEdits;
    }

    public async override Task<WorkspaceEdit> RemapWorkspaceEditAsync(WorkspaceEdit workspaceEdit, CancellationToken cancellationToken)
    {
        if (TryGetDocumentChanges(workspaceEdit, out var documentChanges))
        {
            // The LSP spec says, we should prefer `DocumentChanges` property over `Changes` if available.
            var remappedEdits = await RemapVersionedDocumentEditsAsync(documentChanges, cancellationToken).ConfigureAwait(false);
            return new WorkspaceEdit()
            {
                DocumentChanges = remappedEdits
            };
        }
        else if (workspaceEdit.Changes != null)
        {
            var remappedEdits = await RemapDocumentEditsAsync(workspaceEdit.Changes, cancellationToken).ConfigureAwait(false);
            return new WorkspaceEdit()
            {
                Changes = remappedEdits
            };
        }

        return workspaceEdit;
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

    private async Task<TextDocumentEdit[]> RemapVersionedDocumentEditsAsync(TextDocumentEdit[] documentEdits, CancellationToken cancellationToken)
    {
        var remappedDocumentEdits = new List<TextDocumentEdit>();
        foreach (var entry in documentEdits)
        {
            var uri = entry.TextDocument.Uri;
            if (!CanRemap(uri))
            {
                // This location doesn't point to a background razor file. No need to remap.
                remappedDocumentEdits.Add(entry);

                continue;
            }

            var edits = entry.Edits;
            var (documentSnapshot, remappedEdits) = await RemapTextEditsCoreAsync(uri, edits, TextEditKind.Default, cancellationToken).ConfigureAwait(false);
            if (remappedEdits is null || remappedEdits.Length == 0)
            {
                // Nothing to do.
                continue;
            }

            var razorDocumentUri = _razorConventions.GetRazorDocumentUri(uri);
            remappedDocumentEdits.Add(new TextDocumentEdit()
            {
                TextDocument = new OptionalVersionedTextDocumentIdentifier()
                {
                    Uri = razorDocumentUri,
                    Version = documentSnapshot?.Version
                },
                Edits = remappedEdits
            });
        }

        return remappedDocumentEdits.ToArray();
    }

    private async Task<Dictionary<string, TextEdit[]>> RemapDocumentEditsAsync(Dictionary<string, TextEdit[]> changes, CancellationToken cancellationToken)
    {
        var remappedChanges = new Dictionary<string, TextEdit[]>();
        foreach (var entry in changes)
        {
            var uri = new Uri(entry.Key);
            var edits = entry.Value;

            if (!CanRemap(uri))
            {
                // This location doesn't point to a background razor file. No need to remap.
                remappedChanges[entry.Key] = entry.Value;
                continue;
            }

            var (_, remappedEdits) = await RemapTextEditsCoreAsync(uri, edits, TextEditKind.Default, cancellationToken).ConfigureAwait(false);
            if (remappedEdits is null || remappedEdits.Length == 0)
            {
                // Nothing to do.
                continue;
            }

            var razorDocumentUri = _razorConventions.GetRazorDocumentUri(uri);
            remappedChanges[razorDocumentUri.AbsoluteUri] = remappedEdits;
        }

        return remappedChanges;
    }

    private async Task<(LSPDocumentSnapshot?, TextEdit[])> RemapTextEditsCoreAsync(
        Uri uri,
        TextEdit[] edits,
        TextEditKind textEditKind,
        CancellationToken cancellationToken,
        FormattingOptions? formattingOptions = null)
    {
        var languageKind = RazorLanguageKind.Razor;
        if (_razorConventions.IsVirtualCSharpFile(uri))
        {
            languageKind = RazorLanguageKind.CSharp;
        }
        else if (_razorConventions.IsVirtualHtmlFile(uri))
        {
            languageKind = RazorLanguageKind.Html;
        }
        else
        {
            Debug.Fail("This method should only be called for Razor background files.");
        }

        var razorDocumentUri = _razorConventions.GetRazorDocumentUri(uri);

        var mapToDocumentEditsParams = new RazorMapToDocumentEditsParams()
        {
            Kind = languageKind,
            RazorDocumentUri = razorDocumentUri,
            ProjectedTextEdits = edits,
            TextEditKind = textEditKind,
            FormattingOptions = formattingOptions
        };

        if (!_lazyDocumentManager.Value.TryGetDocument(razorDocumentUri, out var documentSnapshot))
        {
            return (null, s_emptyEdits);
        }

        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<RazorMapToDocumentEditsParams, RazorMapToDocumentEditsResponse>(
            documentSnapshot.Snapshot.TextBuffer,
            LanguageServerConstants.RazorMapToDocumentEditsEndpoint,
            RazorLSPConstants.RazorLanguageServerName,
            CheckRazorEditMappingCapability,
            mapToDocumentEditsParams,
            cancellationToken).ConfigureAwait(false);
        var mappingResult = response?.Response;

        if (mappingResult is null ||
            (_lazyDocumentManager.Value.TryGetDocument(razorDocumentUri, out documentSnapshot) &&
                mappingResult.HostDocumentVersion != documentSnapshot.Version))
        {
            // Couldn't remap the location or the document changed in the meantime. Discard these ranges.
            return (null, s_emptyEdits);
        }

        return (documentSnapshot, mappingResult.TextEdits);
    }

    private bool CanRemap(Uri uri)
    {
        return _razorConventions.IsVirtualCSharpFile(uri) || _razorConventions.IsVirtualHtmlFile(uri);
    }

    private static bool CheckRazorRangeMappingCapability(JToken token)
    {
        if (!RazorLanguageServerCapability.TryGet(token, out var razorCapability))
        {
            return false;
        }

        return razorCapability.RangeMapping;
    }

    private static bool CheckRazorEditMappingCapability(JToken token)
    {
        if (!RazorLanguageServerCapability.TryGet(token, out var razorCapability))
        {
            return false;
        }

        return razorCapability.EditMapping;
    }
}
