// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class DefaultDocumentContextFactory : DocumentContextFactory
{
    private readonly ISnapshotResolver _snapshotResolver;
    private readonly DocumentVersionCache _documentVersionCache;
    private readonly ILogger<DefaultDocumentContextFactory> _logger;

    public DefaultDocumentContextFactory(
        ISnapshotResolver snapshotResolver,
        DocumentVersionCache documentVersionCache,
        ILoggerFactory loggerFactory)
    {
        _snapshotResolver = snapshotResolver;
        _documentVersionCache = documentVersionCache;
        _logger = loggerFactory.CreateLogger<DefaultDocumentContextFactory>();
    }

    protected override DocumentContext? TryCreateCore(Uri documentUri, VSProjectContext? projectContext, bool versioned)
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

        documentSnapshot = null;
        if (!_snapshotResolver.TryResolveAllProjects(filePath, out var projectSnapshots))
        {
            return false;
        }

        foreach (var project in projectSnapshots)
        {
            if (project.Key.Equals(projectContext.ToProjectKey()))
            {
                documentSnapshot = project.GetDocument(filePath);
                return documentSnapshot is not null;
            }
        }

        return false;
    }

    private record DocumentSnapshotAndVersion(IDocumentSnapshot Snapshot, int? Version);
}
