// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class DefaultDocumentContextFactory : DocumentContextFactory
{
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private readonly DocumentResolver _documentResolver;
    private readonly DocumentVersionCache _documentVersionCache;
    private readonly ILogger<DefaultDocumentContextFactory> _logger;

    public DefaultDocumentContextFactory(
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        DocumentResolver documentResolver,
        DocumentVersionCache documentVersionCache,
        ILoggerFactory loggerFactory)
    {
        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        _documentResolver = documentResolver;
        _documentVersionCache = documentVersionCache;
        _logger = loggerFactory.CreateLogger<DefaultDocumentContextFactory>();
    }

    public override async Task<DocumentContext?> TryCreateAsync(Uri documentUri, CancellationToken cancellationToken)
    {
        var filePath = documentUri.GetAbsoluteOrUNCPath();

        var documentAndVersion = await _projectSnapshotManagerDispatcher.RunOnDispatcherThreadAsync(() =>
        {
            if (_documentResolver.TryResolveDocument(filePath, out var documentSnapshot))
            {
                if (_documentVersionCache.TryGetDocumentVersion(documentSnapshot, out var version))
                {
                    return new DocumentSnapshotAndVersion(documentSnapshot, version.Value);
                }
            }

            // This is super rare, if we get here it could mean many things. Some of which:
            //     1. Stale request:
            //          - Got queued after a "document closed" / "document removed" type action
            //          - Took too long to run and by the time the request needed the document context the
            //            version cache has evicted the entry
            //     2. Client is misbehaving and sending requests for a document that we've never seen before.
            _logger.LogWarning("Tried to create context for document {documentUri} which was not found.", documentUri);
            return null;
        }, cancellationToken).ConfigureAwait(false);

        if (documentAndVersion is null)
        {
            // Stale request or misbehaving client, see above comment.
            return null;
        }

        var (documentSnapshot, version) = documentAndVersion;
        if (documentSnapshot is null)
        {
            Debug.Fail($"Document snapshot should never be null here for '{filePath}'. This indicates that our acquisition of documents / versions did not behave as expected.");
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();

        var context = new DocumentContext(documentUri, documentSnapshot, version);
        return context;
    }

    private record DocumentSnapshotAndVersion(DocumentSnapshot Snapshot, int Version);
}
