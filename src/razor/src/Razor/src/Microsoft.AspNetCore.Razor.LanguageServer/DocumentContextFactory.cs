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
    ILoggerFactory loggerFactory)
    : IDocumentContextFactory
{
    private readonly IProjectSnapshotManager _projectManager = projectManager;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<DocumentContextFactory>();

    public bool TryCreate(
        Uri documentUri,
        VSProjectContext? projectContext,
        [NotNullWhen(true)] out DocumentContext? context)
    {
        var filePath = documentUri.GetAbsoluteOrUNCPath();

        if (!TryResolveDocument(filePath, projectContext, out var documentSnapshot))
        {
            // Stale request or misbehaving client, see above comment.
            context = null;
            return false;
        }

        if (documentSnapshot is null)
        {
            Debug.Fail($"Document snapshot should never be null here for '{filePath}'. This indicates that our acquisition of documents / versions did not behave as expected.");
            context = null;
            return false;
        }

        context = new DocumentContext(documentUri, documentSnapshot, projectContext);
        return true;
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
}
