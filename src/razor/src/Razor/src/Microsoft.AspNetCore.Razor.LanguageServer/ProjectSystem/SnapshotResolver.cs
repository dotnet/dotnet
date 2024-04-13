// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal sealed class SnapshotResolver : ISnapshotResolver
{
    private readonly IProjectSnapshotManager _projectManager;
    private readonly ILogger _logger;

    // Internal for testing
    internal readonly HostProject MiscellaneousHostProject;

    public SnapshotResolver(IProjectSnapshotManager projectManager, ILoggerFactory loggerFactory)
    {
        _projectManager = projectManager;
        _logger = loggerFactory.GetOrCreateLogger<SnapshotResolver>();

        var miscellaneousProjectPath = Path.Combine(TempDirectory.Instance.DirectoryPath, "__MISC_RAZOR_PROJECT__");
        var normalizedPath = FilePathNormalizer.Normalize(miscellaneousProjectPath);
        MiscellaneousHostProject = new HostProject(normalizedPath, normalizedPath, FallbackRazorConfiguration.Latest, rootNamespace: null, "Miscellaneous Files");
    }

    /// <inheritdoc/>
    public ImmutableArray<IProjectSnapshot> FindPotentialProjects(string documentFilePath)
    {
        if (documentFilePath is null)
        {
            throw new ArgumentNullException(nameof(documentFilePath));
        }

        var normalizedDocumentPath = FilePathNormalizer.Normalize(documentFilePath);

        using var projects = new PooledArrayBuilder<IProjectSnapshot>();

        foreach (var project in _projectManager.GetProjects())
        {
            // Always exclude the miscellaneous project.
            if (project.FilePath == MiscellaneousHostProject.FilePath)
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

    public async Task<IProjectSnapshot> GetMiscellaneousProjectAsync(CancellationToken cancellationToken)
    {
        if (!_projectManager.TryGetLoadedProject(MiscellaneousHostProject.Key, out var miscellaneousProject))
        {
            await _projectManager
                .UpdateAsync(
                    static (updater, miscHostProject) => updater.ProjectAdded(miscHostProject),
                    state: MiscellaneousHostProject,
                    cancellationToken)
                .ConfigureAwait(false);

            miscellaneousProject = _projectManager.GetLoadedProject(MiscellaneousHostProject.Key);
        }

        return miscellaneousProject;
    }

    public async Task<IDocumentSnapshot?> ResolveDocumentInAnyProjectAsync(string documentFilePath, CancellationToken cancellationToken)
    {
        _logger.LogTrace($"Looking for {documentFilePath}.");

        if (documentFilePath is null)
        {
            throw new ArgumentNullException(nameof(documentFilePath));
        }

        var normalizedDocumentPath = FilePathNormalizer.Normalize(documentFilePath);
        var potentialProjects = FindPotentialProjects(documentFilePath);

        foreach (var project in potentialProjects)
        {
            if (project.GetDocument(normalizedDocumentPath) is { } document)
            {
                _logger.LogTrace($"Found {documentFilePath} in {project.FilePath}");
                return document;
            }
        }

        _logger.LogTrace($"Looking for {documentFilePath} in miscellaneous project.");
        var miscellaneousProject = await GetMiscellaneousProjectAsync(cancellationToken).ConfigureAwait(false);

        if (miscellaneousProject.GetDocument(normalizedDocumentPath) is { } miscDocument)
        {
            _logger.LogTrace($"Found {documentFilePath} in miscellaneous project.");
            return miscDocument;
        }

        _logger.LogTrace($"{documentFilePath} not found in {_projectManager.GetProjects().SelectMany(p => p.DocumentFilePaths)}");

        return null;
    }
}
