// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
    IDocumentVersionCache documentVersionCache,
    ILoggerFactory loggerFactory)
    : IDocumentContextFactory
{
    private readonly IProjectSnapshotManager _projectManager = projectManager;
    private readonly IDocumentVersionCache _documentVersionCache = documentVersionCache;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<DocumentContextFactory>();

    public bool TryCreate(
        Uri documentUri,
        VSProjectContext? projectContext,
        bool versioned,
        [NotNullWhen(true)] out DocumentContext? context)
    {
        var filePath = documentUri.GetAbsoluteOrUNCPath();

        if (!TryGetDocumentAndVersion(filePath, projectContext, versioned, out var documentAndVersion))
        {
            // Stale request or misbehaving client, see above comment.
            context = null;
            return false;
        }

        var (documentSnapshot, version) = documentAndVersion;
        if (documentSnapshot is null)
        {
            Debug.Fail($"Document snapshot should never be null here for '{filePath}'. This indicates that our acquisition of documents / versions did not behave as expected.");
            context = null;
            return false;
        }

        if (versioned)
        {
            // If we were asked for a versioned document, but have no version info, then we didn't find the document
            if (version is null)
            {
                context = null;
                return false;
            }

            context = new VersionedDocumentContext(documentUri, documentSnapshot, projectContext, version.Value);
            return true;
        }

        context = new DocumentContext(documentUri, documentSnapshot, projectContext);
        return true;
    }

    private bool TryGetDocumentAndVersion(
        string filePath,
        VSProjectContext? projectContext,
        bool versioned,
        [NotNullWhen(true)] out DocumentSnapshotAndVersion? documentAndVersion)
    {
        if (TryResolveDocument(filePath, projectContext, out var documentSnapshot))
        {
            if (!versioned)
            {
                documentAndVersion = new DocumentSnapshotAndVersion(documentSnapshot, Version: null);
                return true;
            }

            if (_documentVersionCache.TryGetDocumentVersion(documentSnapshot, out var version))
            {
                documentAndVersion = new DocumentSnapshotAndVersion(documentSnapshot, version.Value);
                return true;
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
        documentAndVersion = null;
        return false;
    }

    private bool TryResolveDocument(
        string filePath,
        VSProjectContext? projectContext,
        [NotNullWhen(true)] out IDocumentSnapshot? documentSnapshot)
    {
        if (projectContext is null)
        {
            return _projectManager.TryResolveDocumentInAnyProject(filePath, _logger, out documentSnapshot);
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
        var miscellaneousProject = _projectManager.GetMiscellaneousProject();
        var normalizedDocumentPath = FilePathNormalizer.Normalize(filePath);
        if (miscellaneousProject.GetDocument(normalizedDocumentPath) is { } miscDocument)
        {
            _logger.LogDebug($"Found document {filePath} in the misc files project, but was asked for project context {projectContext.Id}");
            documentSnapshot = miscDocument;
            return true;
        }

        documentSnapshot = null;
        return false;
    }

    private record DocumentSnapshotAndVersion(IDocumentSnapshot Snapshot, int? Version);
}
