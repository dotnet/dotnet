// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal static class ProjectSnapshotManagerExtensions
{
    public static ProjectSnapshot? GetProject(this ProjectSnapshotManager projectManager, ProjectKey projectKey)
        => projectManager.TryGetProject(projectKey, out var result)
            ? result
            : null;

    public static ProjectSnapshot GetRequiredProject(this ProjectSnapshotManager projectManager, ProjectKey projectKey)
        => projectManager.GetProject(projectKey).AssumeNotNull();

    public static bool ContainsDocument(this ProjectSnapshotManager projectManager, ProjectKey projectKey, string documentFilePath)
        => projectManager.TryGetProject(projectKey, out var project) &&
           project.ContainsDocument(documentFilePath);

    public static bool TryGetDocument(
        this ProjectSnapshotManager projectManager,
        ProjectKey projectKey,
        string documentFilePath,
        [NotNullWhen(true)] out DocumentSnapshot? document)
    {
        document = projectManager.TryGetProject(projectKey, out var project)
            ? project.GetDocument(documentFilePath)
            : null;

        return document is not null;
    }

    public static DocumentSnapshot? GetDocument(this ProjectSnapshotManager projectManager, ProjectKey projectKey, string documentFilePath)
        => projectManager.TryGetDocument(projectKey, documentFilePath, out var result)
            ? result
            : null;

    public static DocumentSnapshot GetRequiredDocument(this ProjectSnapshotManager projectManager, ProjectKey projectKey, string documentFilePath)
        => projectManager.GetDocument(projectKey, documentFilePath).AssumeNotNull();

    public static bool ContainsDocument(this ProjectSnapshotManager projectManager, DocumentKey documentKey)
        => projectManager.ContainsDocument(documentKey.ProjectKey, documentKey.FilePath);

    public static bool TryGetDocument(
        this ProjectSnapshotManager projectManager,
        DocumentKey documentKey,
        [NotNullWhen(true)] out DocumentSnapshot? document)
        => projectManager.TryGetDocument(documentKey.ProjectKey, documentKey.FilePath, out document);

    public static DocumentSnapshot? GetDocument(this ProjectSnapshotManager projectManager, DocumentKey documentKey)
        => projectManager.GetDocument(documentKey.ProjectKey, documentKey.FilePath);

    public static DocumentSnapshot GetRequiredDocument(this ProjectSnapshotManager projectManager, DocumentKey documentKey)
        => projectManager.GetRequiredDocument(documentKey.ProjectKey, documentKey.FilePath);

    public static ProjectSnapshot? GetProject(this ProjectSnapshotManager.Updater updater, ProjectKey projectKey)
        => updater.TryGetProject(projectKey, out var result)
            ? result
            : null;

    public static ProjectSnapshot GetRequiredProject(this ProjectSnapshotManager.Updater updater, ProjectKey projectKey)
        => updater.GetProject(projectKey).AssumeNotNull();

    public static bool ContainsDocument(this ProjectSnapshotManager.Updater updater, ProjectKey projectKey, string documentFilePath)
        => updater.TryGetProject(projectKey, out var project) &&
           project.ContainsDocument(documentFilePath);

    public static bool TryGetDocument(
        this ProjectSnapshotManager.Updater updater,
        ProjectKey projectKey,
        string documentFilePath,
        [NotNullWhen(true)] out DocumentSnapshot? document)
    {
        document = updater.TryGetProject(projectKey, out var project)
            ? project.GetDocument(documentFilePath)
            : null;

        return document is not null;
    }

    public static DocumentSnapshot? GetDocument(this ProjectSnapshotManager.Updater updater, ProjectKey projectKey, string documentFilePath)
        => updater.TryGetDocument(projectKey, documentFilePath, out var result)
            ? result
            : null;

    public static DocumentSnapshot GetRequiredDocument(this ProjectSnapshotManager.Updater updater, ProjectKey projectKey, string documentFilePath)
        => updater.GetDocument(projectKey, documentFilePath).AssumeNotNull();

    public static bool ContainsDocument(this ProjectSnapshotManager.Updater updater, DocumentKey documentKey)
        => updater.ContainsDocument(documentKey.ProjectKey, documentKey.FilePath);

    public static bool TryGetDocument(
        this ProjectSnapshotManager.Updater updater,
        DocumentKey documentKey,
        [NotNullWhen(true)] out DocumentSnapshot? document)
        => updater.TryGetDocument(documentKey.ProjectKey, documentKey.FilePath, out document);

    public static DocumentSnapshot? GetDocument(this ProjectSnapshotManager.Updater updater, DocumentKey documentKey)
        => updater.GetDocument(documentKey.ProjectKey, documentKey.FilePath);

    public static DocumentSnapshot GetRequiredDocument(this ProjectSnapshotManager.Updater updater, DocumentKey documentKey)
        => updater.GetRequiredDocument(documentKey.ProjectKey, documentKey.FilePath);
}
