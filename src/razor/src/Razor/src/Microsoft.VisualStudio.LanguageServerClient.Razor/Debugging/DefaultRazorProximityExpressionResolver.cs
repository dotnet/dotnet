// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.VisualStudio.Editor.Razor.Debugging;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Debugging;

[Export(typeof(RazorProximityExpressionResolver))]
internal class DefaultRazorProximityExpressionResolver : RazorProximityExpressionResolver
{
    private readonly FileUriProvider _fileUriProvider;
    private readonly LSPDocumentManager _documentManager;
    private readonly LSPProximityExpressionsProvider _proximityExpressionsProvider;
    private readonly MemoryCache<CacheKey, IReadOnlyList<string>> _cache;

    [ImportingConstructor]
    public DefaultRazorProximityExpressionResolver(
        FileUriProvider fileUriProvider,
        LSPDocumentManager documentManager,
        LSPProximityExpressionsProvider proximityExpressionsProvider)
    {
        if (fileUriProvider is null)
        {
            throw new ArgumentNullException(nameof(fileUriProvider));
        }

        if (documentManager is null)
        {
            throw new ArgumentNullException(nameof(documentManager));
        }

        if (proximityExpressionsProvider is null)
        {
            throw new ArgumentNullException(nameof(proximityExpressionsProvider));
        }

        _fileUriProvider = fileUriProvider;
        _documentManager = documentManager;
        _proximityExpressionsProvider = proximityExpressionsProvider;

        // 10 is a magic number where this effectively represents our ability to cache the last 10 "hit" breakpoint locations
        // corresponding proximity expressions which enables us not to go "async" in those re-hit scenarios.
        _cache = new MemoryCache<CacheKey, IReadOnlyList<string>>(sizeLimit: 10);
    }

    public override async Task<IReadOnlyList<string>?> TryResolveProximityExpressionsAsync(ITextBuffer textBuffer, int lineIndex, int characterIndex, CancellationToken cancellationToken)
    {
        if (textBuffer is null)
        {
            throw new ArgumentNullException(nameof(textBuffer));
        }

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
        if (!documentSnapshot.TryGetVirtualDocument<CSharpVirtualDocumentSnapshot>(out var virtualDocument))
        {
            Debug.Fail($"Some how there's no C# document associated with the host Razor document {documentUri.OriginalString} when resolving proximity expressions.");
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
        if (_cache.TryGetValue(cacheKey, out var cachedExpressions))
        {
            // We've seen this request before, no need to go async.
            return cachedExpressions;
        }

        var position = new Position(lineIndex, characterIndex);
        var proximityExpressions = await _proximityExpressionsProvider.GetProximityExpressionsAsync(documentSnapshot, position, cancellationToken).ConfigureAwait(false);

        // Cache range so if we're asked again for this document/line/character we don't have to go async.
        // Note: If we didn't get any proximity expressions back--likely due to an error--we cache an empty array.
        _cache.Set(cacheKey, proximityExpressions ?? Array.Empty<string>());

        return proximityExpressions;
    }

    private record CacheKey(Uri DocumentUri, int DocumentVersion, int Line, int Character);
}
