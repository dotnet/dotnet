// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.DocumentMapping;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Range = Microsoft.VisualStudio.LanguageServer.Protocol.Range;

namespace Microsoft.VisualStudio.Razor.LanguageClient.DocumentMapping;

[Export(typeof(LSPDocumentMappingProvider))]
internal class DefaultLSPDocumentMappingProvider : LSPDocumentMappingProvider
{
    private readonly LSPRequestInvoker _requestInvoker;

    // Lazy loading the document manager to get around circular dependencies
    // The Document Manager is a more "Core Service" it depends on the ChangeTriggers which require the LSPDocumentMappingProvider
    // LSPDocumentManager => LSPDocumentMappingProvider => LSPDocumentManagerChangeTrigger => LSPDocumentManager
    private readonly Lazy<LSPDocumentManager> _lazyDocumentManager;

    [ImportingConstructor]
    public DefaultLSPDocumentMappingProvider(
        LSPRequestInvoker requestInvoker,
        Lazy<LSPDocumentManager> lazyDocumentManager)
    {
        if (requestInvoker is null)
        {
            throw new ArgumentNullException(nameof(requestInvoker));
        }

        if (lazyDocumentManager is null)
        {
            throw new ArgumentNullException(nameof(lazyDocumentManager));
        }

        _requestInvoker = requestInvoker;
        _lazyDocumentManager = lazyDocumentManager;
    }

    public async override Task<RazorMapToDocumentRangesResponse?> MapToDocumentRangesAsync(
        RazorLanguageKind languageKind,
        Uri razorDocumentUri,
        Range[] projectedRanges,
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
            MappingBehavior = MappingBehavior.Strict,
        };

        if (!_lazyDocumentManager.Value.TryGetDocument(razorDocumentUri, out var documentSnapshot))
        {
            return null;
        }

        var documentMappingResponse = await _requestInvoker.ReinvokeRequestOnServerAsync<RazorMapToDocumentRangesParams, RazorMapToDocumentRangesResponse>(
            documentSnapshot.Snapshot.TextBuffer,
            LanguageServerConstants.RazorMapToDocumentRangesEndpoint,
            RazorLSPConstants.RazorLanguageServerName,
            mapToDocumentRangeParams,
            cancellationToken).ConfigureAwait(false);

        return documentMappingResponse?.Response;
    }
}
