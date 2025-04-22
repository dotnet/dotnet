// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Utilities;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal static partial class ProjectSnapshotManagerExtensions
{
    /// <summary>
    /// Finds all the projects where the document path starts with the path of the folder that contains the project file.
    /// </summary>
    public static ImmutableArray<ProjectSnapshot> FindPotentialProjects(this ProjectSnapshotManager projectManager, string documentFilePath)
    {
        var normalizedDocumentPath = FilePathNormalizer.Normalize(documentFilePath);

        using var projects = new PooledArrayBuilder<ProjectSnapshot>();

        foreach (var project in projectManager.GetProjects())
        {
            // Always exclude the miscellaneous project.
            if (project.IsMiscellaneousProject())
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

    public static bool TryResolveAllProjects(
        this ProjectSnapshotManager projectManager,
        string documentFilePath,
        out ImmutableArray<ProjectSnapshot> projects)
    {
        var potentialProjects = projectManager.FindPotentialProjects(documentFilePath);

        using var builder = new PooledArrayBuilder<ProjectSnapshot>(capacity: potentialProjects.Length);

        foreach (var project in potentialProjects)
        {
            if (project.ContainsDocument(documentFilePath))
            {
                builder.Add(project);
            }
        }

        var normalizedDocumentPath = FilePathNormalizer.Normalize(documentFilePath);
        var miscProject = projectManager.GetMiscellaneousProject();
        if (miscProject.ContainsDocument(normalizedDocumentPath))
        {
            builder.Add(miscProject);
        }

        projects = builder.DrainToImmutable();
        return projects.Length > 0;
    }

    public static bool TryFindContainingProject(this ProjectSnapshotManager projectManager, string documentFilePath, out ProjectKey projectKey)
    {
        foreach (var project in projectManager.GetProjects())
        {
            if (project.ContainsDocument(documentFilePath))
            {
                projectKey = project.Key;
                return true;
            }
        }

        projectKey = default;
        return false;
    }

    public static bool TryResolveDocumentInAnyProject(
        this ProjectSnapshotManager projectManager,
        string documentFilePath,
        ILogger logger,
        [NotNullWhen(true)] out DocumentSnapshot? document)
    {
        logger.LogTrace($"Looking for {documentFilePath}.");

        var normalizedDocumentPath = FilePathNormalizer.Normalize(documentFilePath);
        var potentialProjects = projectManager.FindPotentialProjects(documentFilePath);

        foreach (var project in potentialProjects)
        {
            if (project.TryGetDocument(normalizedDocumentPath, out document))
            {
                logger.LogTrace($"Found {documentFilePath} in {project.FilePath}");
                return true;
            }
        }

        logger.LogTrace($"Looking for {documentFilePath} in miscellaneous project.");
        var miscellaneousProject = projectManager.GetMiscellaneousProject();

        if (miscellaneousProject.TryGetDocument(normalizedDocumentPath, out document))
        {
            logger.LogTrace($"Found {documentFilePath} in miscellaneous project.");
            return true;
        }

        logger.LogTrace($"{documentFilePath} not found in {string.Join(", ", projectManager.GetProjects().SelectMany(p => p.DocumentFilePaths))}");

        document = null;
        return false;
    }

    public static ISolutionQueryOperations GetQueryOperations(this ProjectSnapshotManager projectManager)
        => new SolutionQueryOperations(projectManager);
}
