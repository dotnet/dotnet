// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

[Export(typeof(IDocumentContextFactory)), Shared]
[method: ImportingConstructor]
internal sealed class DocumentContextFactory(
    IProjectSnapshotManager projectManager,
    ISnapshotResolver snapshotResolver,
    IDocumentVersionCache documentVersionCache,
    IRazorLoggerFactory loggerFactory)
    : IDocumentContextFactory
{
    private readonly IProjectSnapshotManager _projectManager = projectManager;
    private readonly ISnapshotResolver _snapshotResolver = snapshotResolver;
    private readonly IDocumentVersionCache _documentVersionCache = documentVersionCache;
    private readonly ILogger _logger = loggerFactory.CreateLogger<DocumentContextFactory>();

    public DocumentContext? TryCreate(Uri documentUri, VSProjectContext? projectContext, bool versioned)
    {
        var filePath = documentUri.GetAbsoluteOrUNCPath();
        var documentAndVersion = TryGetDocumentAndVersion(filePath, projectContext, versioned);

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

    private DocumentSnapshotAndVersion? TryGetDocumentAndVersion(string filePath, VSProjectContext? projectContext, bool versioned)
    {
        if (TryResolveDocument(filePath, projectContext, out var documentSnapshot))
        {
            if (!versioned)
            {
                return new DocumentSnapshotAndVersion(documentSnapshot, Version: null);
            }

            if (_documentVersionCache.TryGetDocumentVersion(documentSnapshot, out var version))
            {
                return new DocumentSnapshotAndVersion(documentSnapshot, version.Value);
            }

            _logger.LogWarning("Tried to create context for document {filePath} and project {projectContext} and a document was found, but version didn't match.", filePath, projectContext?.Id);
        }

        // This is super rare, if we get here it could mean many things. Some of which:
        //     1. Stale request:
        //          - Got queued after a "document closed" / "document removed" type action
        //          - Took too long to run and by the time the request needed the document context the
        //            version cache has evicted the entry
        //     2. Client is misbehaving and sending requests for a document that we've never seen before.
        _logger.LogWarning("Tried to create context for document {filePath} and project {projectContext} which was not found.", filePath, projectContext?.Id);
        return null;
    }

    private bool TryResolveDocument(string filePath, VSProjectContext? projectContext, [NotNullWhen(true)] out IDocumentSnapshot? documentSnapshot)
    {
        if (projectContext is null)
        {
            return _snapshotResolver.TryResolveDocumentInAnyProject(filePath, out documentSnapshot);
        }

        if (_projectManager.TryGetLoadedProject(projectContext.ToProjectKey(), out var project) &&
            project.GetDocument(filePath) is { } document)
        {
            documentSnapshot = document;
            return true;
        }

        // Couldn't find the document in a real project. Maybe the language server doesn't yet know about the project
        // that the IDE is asking us about. In that case, we might have the document in our misc files project, and we'll
        // move it to the real project when/if we find out about it.
        var miscellaneousProject = _snapshotResolver.GetMiscellaneousProject();
        var normalizedDocumentPath = FilePathNormalizer.Normalize(filePath);
        if (miscellaneousProject.GetDocument(normalizedDocumentPath) is { } miscDocument)
        {
            _logger.LogDebug("Found document {document} in the misc files project, but was asked for project context {context}", filePath, projectContext);
            documentSnapshot = miscDocument;
            return true;
        }

        documentSnapshot = null;
        return false;
    }

    private record DocumentSnapshotAndVersion(IDocumentSnapshot Snapshot, int? Version);
}
