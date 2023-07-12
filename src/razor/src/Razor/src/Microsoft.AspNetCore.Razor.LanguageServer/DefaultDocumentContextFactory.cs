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
    private readonly ISnapshotResolver _snapshotResolver;
    private readonly DocumentVersionCache _documentVersionCache;
    private readonly ILogger<DefaultDocumentContextFactory> _logger;

    public DefaultDocumentContextFactory(
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        ISnapshotResolver snapshotResolver,
        DocumentVersionCache documentVersionCache,
        ILoggerFactory loggerFactory)
    {
        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        _snapshotResolver = snapshotResolver;
        _documentVersionCache = documentVersionCache;
        _logger = loggerFactory.CreateLogger<DefaultDocumentContextFactory>();
    }

    public override Task<DocumentContext?> TryCreateAsync(Uri documentUri, CancellationToken cancellationToken)
        => TryCreateCoreAsync(documentUri, versioned: false, cancellationToken);

    public async override Task<VersionedDocumentContext?> TryCreateForOpenDocumentAsync(Uri documentUri, CancellationToken cancellationToken)
        => (VersionedDocumentContext?)await TryCreateCoreAsync(documentUri, versioned: true, cancellationToken).ConfigureAwait(false);

    private async Task<DocumentContext?> TryCreateCoreAsync(Uri documentUri, bool versioned, CancellationToken cancellationToken)
    {
        var filePath = documentUri.GetAbsoluteOrUNCPath();

        var documentAndVersion = await _projectSnapshotManagerDispatcher.RunOnDispatcherThreadAsync(() =>
        {
            if (_snapshotResolver.TryResolveDocument(filePath,  out var documentSnapshot))
            {
                if (!versioned)
                {
                    return new DocumentSnapshotAndVersion(documentSnapshot, Version: null);
                }

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

        if (versioned)
        {
            // If we were asked for a versioned document, but have no version info, then we didn't find the document
            if (version is null)
            {
                return null;
            }

            return new VersionedDocumentContext(documentUri, documentSnapshot, version.Value);
        }

        return new DocumentContext(documentUri, documentSnapshot);
    }

    private record DocumentSnapshotAndVersion(IDocumentSnapshot Snapshot, int? Version);
}
