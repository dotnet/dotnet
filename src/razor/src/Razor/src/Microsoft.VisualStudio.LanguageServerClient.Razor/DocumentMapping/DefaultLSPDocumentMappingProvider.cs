// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.DocumentMapping;

[Shared]
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
            MappingBehavior = LanguageServerMappingBehavior.Strict,
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

    private static bool CheckRazorRangeMappingCapability(JToken token)
    {
        if (!RazorLanguageServerCapability.TryGet(token, out var razorCapability))
        {
            return false;
        }

        return razorCapability.RangeMapping;
    }
}
