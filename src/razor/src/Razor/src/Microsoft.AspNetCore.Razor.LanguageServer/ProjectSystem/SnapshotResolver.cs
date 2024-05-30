// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CommonLanguageServerProtocol.Framework;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal sealed class SnapshotResolver : ISnapshotResolver, IOnInitialized
{
    private readonly IProjectSnapshotManager _projectManager;
    private readonly ILogger _logger;

    public SnapshotResolver(IProjectSnapshotManager projectManager, ILoggerFactory loggerFactory)
    {
        _projectManager = projectManager;
        _logger = loggerFactory.GetOrCreateLogger<SnapshotResolver>();
    }

    public Task OnInitializedAsync(ILspServices services, CancellationToken cancellationToken)
    {
        // This is called when the language server is initialized.

        return _projectManager.UpdateAsync(
            (updater, miscHostProject) => updater.ProjectAdded(miscHostProject),
            state: MiscFilesHostProject.Instance,
            cancellationToken);
    }

    /// <inheritdoc/>
    public ImmutableArray<IProjectSnapshot> FindPotentialProjects(string documentFilePath)
    {
        var normalizedDocumentPath = FilePathNormalizer.Normalize(documentFilePath);

        using var projects = new PooledArrayBuilder<IProjectSnapshot>();

        foreach (var project in _projectManager.GetProjects())
        {
            // Always exclude the miscellaneous project.
            if (project.FilePath == MiscFilesHostProject.Instance.FilePath)
            {
                continue;
            }

            var projectDirectory = FilePathNormalizer.GetNormalizedDirectoryName(project.FilePath);
            if (normalizedDocumentPath.StartsWith(projectDirectory, FilePathComparison.Instance))
            {
                projects.Add(project);
            }
        }

        return projects.DrainToImmutable();
    }

    public IProjectSnapshot GetMiscellaneousProject()
    {
        return _projectManager.GetLoadedProject(MiscFilesHostProject.Instance.Key);
    }

    public bool TryResolveDocumentInAnyProject(string documentFilePath, [NotNullWhen(true)] out IDocumentSnapshot? document)
    {
        _logger.LogTrace($"Looking for {documentFilePath}.");

        var normalizedDocumentPath = FilePathNormalizer.Normalize(documentFilePath);
        var potentialProjects = FindPotentialProjects(documentFilePath);

        foreach (var project in potentialProjects)
        {
            if (project.GetDocument(normalizedDocumentPath) is { } projectDocument)
            {
                _logger.LogTrace($"Found {documentFilePath} in {project.FilePath}");
                document = projectDocument;
                return true;
            }
        }

        _logger.LogTrace($"Looking for {documentFilePath} in miscellaneous project.");
        var miscellaneousProject = GetMiscellaneousProject();

        if (miscellaneousProject.GetDocument(normalizedDocumentPath) is { } miscDocument)
        {
            _logger.LogTrace($"Found {documentFilePath} in miscellaneous project.");
            document = miscDocument;
            return true;
        }

        _logger.LogTrace($"{documentFilePath} not found in {string.Join(", ", _projectManager.GetProjects().SelectMany(p => p.DocumentFilePaths))}");

        document = null;
        return false;
    }
}
