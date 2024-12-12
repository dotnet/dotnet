// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal static class IProjectSnapshotManagerExtensions
{
    public static IProjectSnapshot? GetProject(this IProjectSnapshotManager projectManager, ProjectKey projectKey)
        => projectManager.TryGetProject(projectKey, out var result)
            ? result
            : null;

    public static IProjectSnapshot GetRequiredProject(this IProjectSnapshotManager projectManager, ProjectKey projectKey)
        => projectManager.GetProject(projectKey).AssumeNotNull();

    public static bool ContainsDocument(this IProjectSnapshotManager projectManager, ProjectKey projectKey, string documentFilePath)
        => projectManager.TryGetProject(projectKey, out var project) &&
           project.ContainsDocument(documentFilePath);

    public static bool TryGetDocument(
        this IProjectSnapshotManager projectManager,
        ProjectKey projectKey,
        string documentFilePath,
        [NotNullWhen(true)] out IDocumentSnapshot? document)
    {
        document = projectManager.TryGetProject(projectKey, out var project)
            ? project.GetDocument(documentFilePath)
            : null;

        return document is not null;
    }

    public static IDocumentSnapshot? GetDocument(this IProjectSnapshotManager projectManager, ProjectKey projectKey, string documentFilePath)
        => projectManager.TryGetDocument(projectKey, documentFilePath, out var result)
            ? result
            : null;

    public static IDocumentSnapshot GetRequiredDocument(this IProjectSnapshotManager projectManager, ProjectKey projectKey, string documentFilePath)
        => projectManager.GetDocument(projectKey, documentFilePath).AssumeNotNull();

    public static IProjectSnapshot? GetProject(this ProjectSnapshotManager.Updater updater, ProjectKey projectKey)
        => updater.TryGetProject(projectKey, out var result)
            ? result
            : null;

    public static IProjectSnapshot GetRequiredProject(this ProjectSnapshotManager.Updater updater, ProjectKey projectKey)
        => updater.GetProject(projectKey).AssumeNotNull();

    public static bool ContainsDocument(this ProjectSnapshotManager.Updater updater, ProjectKey projectKey, string documentFilePath)
        => updater.TryGetProject(projectKey, out var project) &&
           project.ContainsDocument(documentFilePath);

    public static bool TryGetDocument(
        this ProjectSnapshotManager.Updater updater,
        ProjectKey projectKey,
        string documentFilePath,
        [NotNullWhen(true)] out IDocumentSnapshot? document)
    {
        document = updater.TryGetProject(projectKey, out var project)
            ? project.GetDocument(documentFilePath)
            : null;

        return document is not null;
    }

    public static IDocumentSnapshot? GetDocument(this ProjectSnapshotManager.Updater updater, ProjectKey projectKey, string documentFilePath)
        => updater.TryGetDocument(projectKey, documentFilePath, out var result)
            ? result
            : null;

    public static IDocumentSnapshot GetRequiredDocument(this ProjectSnapshotManager.Updater updater, ProjectKey projectKey, string documentFilePath)
        => updater.GetDocument(projectKey, documentFilePath).AssumeNotNull();
}
