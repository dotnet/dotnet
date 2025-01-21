// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Razor.Debugging;
using Microsoft.VisualStudio.Text;
using Range = Microsoft.VisualStudio.LanguageServer.Protocol.Range;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Debugging;

[Export(typeof(IRazorBreakpointResolver))]
[method: ImportingConstructor]
internal class RazorBreakpointResolver(
    FileUriProvider fileUriProvider,
    LSPDocumentManager documentManager,
    ILSPBreakpointSpanProvider breakpointSpanProvider,
    LanguageServerFeatureOptions languageServerFeatureOptions,
    IRemoteServiceInvoker remoteServiceInvoker) : IRazorBreakpointResolver
{
    private record CohostCacheKey(DocumentId DocumentId, VersionStamp Version, int Line, int Character) : CacheKey;
    private record LspCacheKey(Uri DocumentUri, long? HostDocumentSyncVersion, int Line, int Character) : CacheKey;
    private record CacheKey;

    private readonly FileUriProvider _fileUriProvider = fileUriProvider;
    private readonly LSPDocumentManager _documentManager = documentManager;
    private readonly ILSPBreakpointSpanProvider _breakpointSpanProvider = breakpointSpanProvider;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions = languageServerFeatureOptions;
    private readonly IRemoteServiceInvoker _remoteServiceInvoker = remoteServiceInvoker;

    // 4 is a magic number that was determined based on the functionality of VisualStudio. Currently when you set or edit a breakpoint
    // we get called with two different locations for the same breakpoint. Because of this 2 time call our size must be at least 2,
    // we grow it to 4 just to be safe for lesser known scenarios.
    private readonly MemoryCache<CacheKey, Range> _cache = new(sizeLimit: 4);

    public Task<Range?> TryResolveBreakpointRangeAsync(ITextBuffer textBuffer, int lineIndex, int characterIndex, CancellationToken cancellationToken)
        => _languageServerFeatureOptions.UseRazorCohostServer
            ? TryResolveBreakpointRangeViaCohostingAsync(textBuffer, lineIndex, characterIndex, cancellationToken)
            : TryResolveBreakpointRangeViaLspAsync(textBuffer, lineIndex, characterIndex, cancellationToken);

    private async Task<Range?> TryResolveBreakpointRangeViaCohostingAsync(ITextBuffer textBuffer, int lineIndex, int characterIndex, CancellationToken cancellationToken)
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
            .TryInvokeAsync<IRemoteDebugInfoService, LinePositionSpan?>(
                razorDocument.Project.Solution,
                (service, solutionInfo, cancellationToken) =>
                    service.ResolveBreakpointRangeAsync(solutionInfo, razorDocument.Id, new(lineIndex, characterIndex), cancellationToken),
                cancellationToken)
            .ConfigureAwait(false);

        if (response is not { } responseSpan)
        {
            // can't map the position, invalid breakpoint location.
            return null;
        }

        var hostDocumentRange = responseSpan.ToRange();
        cancellationToken.ThrowIfCancellationRequested();

        // Cache range so if we're asked again for this document/line/character we don't have to go async.
        _cache.Set(cacheKey, hostDocumentRange);

        return hostDocumentRange;
    }

    private async Task<Range?> TryResolveBreakpointRangeViaLspAsync(ITextBuffer textBuffer, int lineIndex, int characterIndex, CancellationToken cancellationToken)
    {
        if (!_fileUriProvider.TryGet(textBuffer, out var documentUri))
        {
            // Not an addressable Razor document. Do not allow a breakpoint here. In practice this shouldn't happen, just being defensive.
            return null;
        }

        if (!_documentManager.TryGetDocument(documentUri, out var documentSnapshot))
        {
            // No associated Razor document. Do not allow a breakpoint here. In practice this shouldn't happen, just being defensive.
            return null;
        }

        // TODO: Support multiple C# documents per Razor document.
        if (!documentSnapshot.TryGetVirtualDocument<CSharpVirtualDocumentSnapshot>(out var virtualDocument) ||
            virtualDocument.HostDocumentSyncVersion is not { } hostDocumentSyncVersion)
        {
            Debug.Fail($"Some how there's no C# document associated with the host Razor document {documentUri.OriginalString} when validating breakpoint locations.");
            return null;
        }

        var cacheKey = new LspCacheKey(documentSnapshot.Uri, virtualDocument.HostDocumentSyncVersion, lineIndex, characterIndex);
        if (_cache.TryGetValue(cacheKey, out var cachedRange))
        {
            // We've seen this request before, no need to go async.
            return cachedRange;
        }

        var position = VsLspFactory.CreatePosition(lineIndex, characterIndex);
        var hostDocumentRange = await _breakpointSpanProvider.GetBreakpointSpanAsync(documentSnapshot, hostDocumentSyncVersion, position, cancellationToken).ConfigureAwait(false);
        if (hostDocumentRange is null)
        {
            // can't map the position, invalid breakpoint location.
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();

        // Cache range so if we're asked again for this document/line/character we don't have to go async.
        _cache.Set(cacheKey, hostDocumentRange);

        return hostDocumentRange;
    }
}
