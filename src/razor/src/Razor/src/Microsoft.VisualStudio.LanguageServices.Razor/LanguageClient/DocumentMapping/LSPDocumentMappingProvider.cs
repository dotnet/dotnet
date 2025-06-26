// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.DocumentMapping;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.Razor.LanguageClient.DocumentMapping;

[Export(typeof(LSPDocumentMappingProvider))]
[method: ImportingConstructor]
internal class LSPDocumentMappingProvider(
    LSPRequestInvoker requestInvoker,
    Lazy<LSPDocumentManager> lazyDocumentManager)
{
    private readonly LSPRequestInvoker _requestInvoker = requestInvoker;

    // Lazy loading the document manager to get around circular dependencies
    // The Document Manager is a more "Core Service" it depends on the ChangeTriggers which require the LSPDocumentMappingProvider
    // LSPDocumentManager => LSPDocumentMappingProvider => LSPDocumentManagerChangeTrigger => LSPDocumentManager
    private readonly Lazy<LSPDocumentManager> _lazyDocumentManager = lazyDocumentManager;

    public async Task<RazorMapToDocumentRangesResponse?> MapToDocumentRangesAsync(
        RazorLanguageKind languageKind,
        Uri razorDocumentUri,
        LspRange[] projectedRanges,
        CancellationToken cancellationToken)
    {
        if (!TryGetTextBuffer(razorDocumentUri, out var textBuffer))
        {
            return null;
        }

        var mapToDocumentRangeParams = new RazorMapToDocumentRangesParams()
        {
            Kind = languageKind,
            RazorDocumentUri = razorDocumentUri,
            ProjectedRanges = projectedRanges,
            MappingBehavior = MappingBehavior.Strict,
        };

        var documentMappingResponse = await _requestInvoker.ReinvokeRequestOnServerAsync<RazorMapToDocumentRangesParams, RazorMapToDocumentRangesResponse>(
            textBuffer,
            LanguageServerConstants.RazorMapToDocumentRangesEndpoint,
            RazorLSPConstants.RazorLanguageServerName,
            mapToDocumentRangeParams,
            cancellationToken).ConfigureAwait(false);

        return documentMappingResponse?.Response;
    }

    public async Task<RazorMapToDocumentEditsResponse?> MapToDocumentEditsAsync(
        RazorLanguageKind languageKind,
        Uri razorDocumentUri,
        TextChange[] textEdits,
        CancellationToken cancellationToken)
    {
        if (!TryGetTextBuffer(razorDocumentUri, out var textBuffer))
        {
            return null;
        }

        var mapToDocumentEditsParams = new RazorMapToDocumentEditsParams()
        {
            Kind = languageKind,
            RazorDocumentUri = razorDocumentUri,
            TextChanges = textEdits.Select(ConvertToRazorCSharpTextChange).ToArray(),
        };

        var documentMappingResponse = await _requestInvoker.ReinvokeRequestOnServerAsync<RazorMapToDocumentEditsParams, RazorMapToDocumentEditsResponse>(
            textBuffer,
            LanguageServerConstants.RazorMapToDocumentEditsEndpoint,
            RazorLSPConstants.RazorLanguageServerName,
            mapToDocumentEditsParams,
            cancellationToken).ConfigureAwait(false);

        return documentMappingResponse?.Response;
    }

    private RazorTextChange ConvertToRazorCSharpTextChange(TextChange change)
        => change.ToRazorTextChange();

    private bool TryGetTextBuffer(Uri razorDocumentUri, [NotNullWhen(true)] out ITextBuffer? textBuffer)
    {
        if (!_lazyDocumentManager.Value.TryGetDocument(razorDocumentUri, out var documentSnapshot))
        {
            textBuffer = null;
            return false;
        }

        textBuffer = documentSnapshot.Snapshot.TextBuffer;
        return true;
    }
}
