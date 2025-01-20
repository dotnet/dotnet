// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Razor.Debugging;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Debugging;

[Export(typeof(IRazorProximityExpressionResolver))]
[method: ImportingConstructor]
internal class RazorProximityExpressionResolver(
    FileUriProvider fileUriProvider,
    LSPDocumentManager documentManager,
    ILSPProximityExpressionsProvider proximityExpressionsProvider,
    LanguageServerFeatureOptions languageServerFeatureOptions,
    IRemoteServiceInvoker remoteServiceInvoker) : IRazorProximityExpressionResolver
{
    private record CohostCacheKey(DocumentId DocumentId, VersionStamp Version, int Line, int Character) : CacheKey;
    private record LspCacheKey(Uri DocumentUri, long? HostDocumentSyncVersion, int Line, int Character) : CacheKey;
    private record CacheKey;

    private readonly FileUriProvider _fileUriProvider = fileUriProvider;
    private readonly LSPDocumentManager _documentManager = documentManager;
    private readonly ILSPProximityExpressionsProvider _proximityExpressionsProvider = proximityExpressionsProvider;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions = languageServerFeatureOptions;
    private readonly IRemoteServiceInvoker _remoteServiceInvoker = remoteServiceInvoker;

    // 10 is a magic number where this effectively represents our ability to cache the last 10 "hit" breakpoint locations
    // corresponding proximity expressions which enables us not to go "async" in those re-hit scenarios.
    private readonly MemoryCache<CacheKey, IReadOnlyList<string>> _cache = new(sizeLimit: 10);

    public Task<IReadOnlyList<string>?> TryResolveProximityExpressionsAsync(ITextBuffer textBuffer, int lineIndex, int characterIndex, CancellationToken cancellationToken)
        => _languageServerFeatureOptions.UseRazorCohostServer
            ? TryResolveProximityExpressionsViaCohostingAsync(textBuffer, lineIndex, characterIndex, cancellationToken)
            : TryResolveProximityExpressionsViaLspAsync(textBuffer, lineIndex, characterIndex, cancellationToken);

    private async Task<IReadOnlyList<string>?> TryResolveProximityExpressionsViaCohostingAsync(ITextBuffer textBuffer, int lineIndex, int characterIndex, CancellationToken cancellationToken)
    {
        if (!textBuffer.TryGetTextDocument(out var razorDocument))
        {
            // Razor document is not in the Roslyn workspace.
            return null;
        }

        if (razorDocument.TryGetTextVersion(out var version))
        {
            version = await razorDocument.GetTextVersionAsync(cancellationToken).ConfigureAwait(false);
        }

        var cacheKey = new CohostCacheKey(razorDocument.Id, version, lineIndex, characterIndex);
        if (_cache.TryGetValue(cacheKey, out var cachedRange))
        {
            // We've seen this request before. Hopefully the TryGetTextVersion call above was successful so this whole path
            // will have been sync, and the cache will have been useful.
            return cachedRange;
        }

        var response = await _remoteServiceInvoker
            .TryInvokeAsync<IRemoteDebugInfoService, string[]?>(
                razorDocument.Project.Solution,
                (service, solutionInfo, cancellationToken) =>
                    service.ResolveProximityExpressionsAsync(solutionInfo, razorDocument.Id, new(lineIndex, characterIndex), cancellationToken),
                cancellationToken)
            .ConfigureAwait(false);

        if (response is null)
        {
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();

        // Cache range so if we're asked again for this document/line/character we don't have to go async.
        _cache.Set(cacheKey, response);

        return response;
    }

    private async Task<IReadOnlyList<string>?> TryResolveProximityExpressionsViaLspAsync(ITextBuffer textBuffer, int lineIndex, int characterIndex, CancellationToken cancellationToken)
    {
        if (!_fileUriProvider.TryGet(textBuffer, out var documentUri))
        {
            // Not an addressable Razor document. Do not allow expression resolution here. In practice this shouldn't happen, just being defensive.
            return null;
        }

        if (!_documentManager.TryGetDocument(documentUri, out var documentSnapshot))
        {
            // No associated Razor document. Do not resolve expressions here. In practice this shouldn't happen, just being defensive.
            return null;
        }

        // TODO: Support multiple C# documents per Razor document.
        if (!documentSnapshot.TryGetVirtualDocument<CSharpVirtualDocumentSnapshot>(out var virtualDocument) ||
            virtualDocument.HostDocumentSyncVersion is not { } hostDocumentSyncVersion)
        {
            Debug.Fail($"Some how there's no C# document associated with the host Razor document {documentUri.OriginalString} when resolving proximity expressions.");
            return null;
        }

        var cacheKey = new LspCacheKey(documentSnapshot.Uri, virtualDocument.HostDocumentSyncVersion, lineIndex, characterIndex);
        if (_cache.TryGetValue(cacheKey, out var cachedExpressions))
        {
            // We've seen this request before, no need to go async.
            return cachedExpressions;
        }

        var position = VsLspFactory.CreatePosition(lineIndex, characterIndex);
        var proximityExpressions = await _proximityExpressionsProvider.GetProximityExpressionsAsync(documentSnapshot, hostDocumentSyncVersion, position, cancellationToken).ConfigureAwait(false);

        // Cache range so if we're asked again for this document/line/character we don't have to go async.
        // Note: If we didn't get any proximity expressions back--likely due to an error--we cache an empty array.
        _cache.Set(cacheKey, proximityExpressions ?? []);

        return proximityExpressions;
    }
}
