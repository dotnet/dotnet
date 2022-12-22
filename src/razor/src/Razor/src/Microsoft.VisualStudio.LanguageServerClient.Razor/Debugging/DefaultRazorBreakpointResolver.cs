// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.Editor.Razor.Debugging;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Debugging;

[Export(typeof(RazorBreakpointResolver))]
internal class DefaultRazorBreakpointResolver : RazorBreakpointResolver
{
    private readonly FileUriProvider _fileUriProvider;
    private readonly LSPDocumentManager _documentManager;
    private readonly LSPBreakpointSpanProvider _breakpointSpanProvider;
    private readonly MemoryCache<CacheKey, Range> _cache;

    [ImportingConstructor]
    public DefaultRazorBreakpointResolver(
        FileUriProvider fileUriProvider,
        LSPDocumentManager documentManager,
        LSPBreakpointSpanProvider breakpointSpanProvider)
    {
        if (fileUriProvider is null)
        {
            throw new ArgumentNullException(nameof(fileUriProvider));
        }

        if (documentManager is null)
        {
            throw new ArgumentNullException(nameof(documentManager));
        }

        if (breakpointSpanProvider is null)
        {
            throw new ArgumentNullException(nameof(breakpointSpanProvider));
        }

        _fileUriProvider = fileUriProvider;
        _documentManager = documentManager;
        _breakpointSpanProvider = breakpointSpanProvider;

        // 4 is a magic number that was determined based on the functionality of VisualStudio. Currently when you set or edit a breakpoint
        // we get called with two different locations for the same breakpoint. Because of this 2 time call our size must be at least 2,
        // we grow it to 4 just to be safe for lesser known scenarios.
        _cache = new MemoryCache<CacheKey, Range>(sizeLimit: 4);
    }

    public override async Task<Range?> TryResolveBreakpointRangeAsync(ITextBuffer textBuffer, int lineIndex, int characterIndex, CancellationToken cancellationToken)
    {
        if (textBuffer is null)
        {
            throw new ArgumentNullException(nameof(textBuffer));
        }

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

        if (!documentSnapshot.TryGetVirtualDocument<CSharpVirtualDocumentSnapshot>(out var virtualDocument))
        {
            Debug.Fail($"Some how there's no C# document associated with the host Razor document {documentUri.OriginalString} when validating breakpoint locations.");
            return null;
        }

        if (virtualDocument.HostDocumentSyncVersion != documentSnapshot.Version)
        {
            // C# document isn't up-to-date with the Razor document. Because VS' debugging tech is synchronous on the UI thread we have to bail. Ideally we'd wait
            // for the C# document to become "updated"; however, that'd require the UI thread to see that the C# buffer is updated. Because this call path blocks
            // the UI thread the C# document will never update until this path has exited. This means as a user types around the point of interest data may get stale
            // but will re-adjust later.
            return null;
        }

        var cacheKey = new CacheKey(documentSnapshot.Uri, documentSnapshot.Version, lineIndex, characterIndex);
        if (_cache.TryGetValue(cacheKey, out var cachedRange))
        {
            // We've seen this request before, no need to go async.
            return cachedRange;
        }

        var lspPosition = new Position(lineIndex, characterIndex);
        var hostDocumentRange = await _breakpointSpanProvider.GetBreakpointSpanAsync(documentSnapshot, lspPosition, cancellationToken).ConfigureAwait(false);
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

    private record CacheKey(Uri DocumentUri, int DocumentVersion, int Line, int Character);
}
