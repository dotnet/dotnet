// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal sealed class DocumentContextFactory(
    IProjectSnapshotManager projectManager,
    ISnapshotResolver snapshotResolver,
    IDocumentVersionCache documentVersionCache,
    ILoggerFactory loggerFactory)
    : IDocumentContextFactory
{
    private readonly IProjectSnapshotManager _projectManager = projectManager;
    private readonly ISnapshotResolver _snapshotResolver = snapshotResolver;
    private readonly IDocumentVersionCache _documentVersionCache = documentVersionCache;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<DocumentContextFactory>();

    public async Task<DocumentContext?> TryCreateAsync(Uri documentUri, VSProjectContext? projectContext, bool versioned, CancellationToken cancellationToken)
    {
        var filePath = documentUri.GetAbsoluteOrUNCPath();
        var documentAndVersion = await TryGetDocumentAndVersionAsync(filePath, projectContext, versioned, cancellationToken).ConfigureAwait(false);

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

        if (versioned)
        {
            // If we were asked for a versioned document, but have no version info, then we didn't find the document
            if (version is null)
            {
                return null;
            }

            return new VersionedDocumentContext(documentUri, documentSnapshot, projectContext, version.Value);
        }

        return new DocumentContext(documentUri, documentSnapshot, projectContext);
    }

    private async Task<DocumentSnapshotAndVersion?> TryGetDocumentAndVersionAsync(string filePath, VSProjectContext? projectContext, bool versioned, CancellationToken cancellationToken)
    {
        var documentSnapshot = await TryResolveDocumentAsync(filePath, projectContext, cancellationToken).ConfigureAwait(false);

        if (documentSnapshot is not null)
        {
            if (!versioned)
            {
                return new DocumentSnapshotAndVersion(documentSnapshot, Version: null);
            }

            if (_documentVersionCache.TryGetDocumentVersion(documentSnapshot, out var version))
            {
                return new DocumentSnapshotAndVersion(documentSnapshot, version.Value);
            }

            _logger.LogWarning($"Tried to create context for document {filePath} and project {projectContext?.Id} and a document was found, but version didn't match.");
        }

        // This is super rare, if we get here it could mean many things. Some of which:
        //     1. Stale request:
        //          - Got queued after a "document closed" / "document removed" type action
        //          - Took too long to run and by the time the request needed the document context the
        //            version cache has evicted the entry
        //     2. Client is misbehaving and sending requests for a document that we've never seen before.
        _logger.LogWarning($"Tried to create context for document {filePath} and project {projectContext?.Id} which was not found.");
        return null;
    }

    private async Task<IDocumentSnapshot?> TryResolveDocumentAsync(string filePath, VSProjectContext? projectContext, CancellationToken cancellationToken)
    {
        if (projectContext is null)
        {
            return await _snapshotResolver
                .ResolveDocumentInAnyProjectAsync(filePath, cancellationToken)
                .ConfigureAwait(false);
        }

        if (_projectManager.TryGetLoadedProject(projectContext.ToProjectKey(), out var project) &&
            project.GetDocument(filePath) is { } document)
        {
            return document;
        }

        // Couldn't find the document in a real project. Maybe the language server doesn't yet know about the project
        // that the IDE is asking us about. In that case, we might have the document in our misc files project, and we'll
        // move it to the real project when/if we find out about it.
        var miscellaneousProject = await _snapshotResolver.GetMiscellaneousProjectAsync(cancellationToken).ConfigureAwait(false);
        var normalizedDocumentPath = FilePathNormalizer.Normalize(filePath);
        if (miscellaneousProject.GetDocument(normalizedDocumentPath) is { } miscDocument)
        {
            _logger.LogDebug($"Found document {filePath} in the misc files project, but was asked for project context {projectContext}");
            return miscDocument;
        }

        return null;
    }

    private record DocumentSnapshotAndVersion(IDocumentSnapshot Snapshot, int? Version);
}
