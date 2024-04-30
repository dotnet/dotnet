// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal class RazorProjectService(
    RemoteTextLoaderFactory remoteTextLoaderFactory,
    ISnapshotResolver snapshotResolver,
    IDocumentVersionCache documentVersionCache,
    IProjectSnapshotManager projectManager,
    ILoggerFactory loggerFactory)
    : IRazorProjectService
{
    private readonly IProjectSnapshotManager _projectManager = projectManager;
    private readonly RemoteTextLoaderFactory _remoteTextLoaderFactory = remoteTextLoaderFactory;
    private readonly ISnapshotResolver _snapshotResolver = snapshotResolver;
    private readonly IDocumentVersionCache _documentVersionCache = documentVersionCache;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<RazorProjectService>();

    public Task AddDocumentAsync(string filePath, CancellationToken cancellationToken)
    {
        return _projectManager.UpdateAsync(
            updater: AddDocumentCore,
            state: filePath,
            cancellationToken);
    }

    private void AddDocumentCore(ProjectSnapshotManager.Updater updater, string filePath)
    {
        var textDocumentPath = FilePathNormalizer.Normalize(filePath);

        var added = false;
        foreach (var projectSnapshot in _snapshotResolver.FindPotentialProjects(textDocumentPath))
        {
            added = true;
            AddDocumentToProject(updater, projectSnapshot, textDocumentPath);
        }

        if (!added)
        {
            var miscFilesProject = _snapshotResolver.GetMiscellaneousProject();
            AddDocumentToProject(updater, miscFilesProject, textDocumentPath);
        }

        void AddDocumentToProject(ProjectSnapshotManager.Updater updater, IProjectSnapshot projectSnapshot, string textDocumentPath)
        {
            if (projectSnapshot.GetDocument(FilePathNormalizer.Normalize(textDocumentPath)) is not null)
            {
                // Document already added. This usually occurs when VSCode has already pre-initialized
                // open documents and then we try to manually add all known razor documents.
                return;
            }

            var targetFilePath = textDocumentPath;
            var projectDirectory = FilePathNormalizer.GetNormalizedDirectoryName(projectSnapshot.FilePath);
            if (targetFilePath.StartsWith(projectDirectory, FilePathComparison.Instance))
            {
                // Make relative
                targetFilePath = textDocumentPath[projectDirectory.Length..];
            }

            // Representing all of our host documents with a re-normalized target path to workaround GetRelatedDocument limitations.
            var normalizedTargetFilePath = targetFilePath.Replace('/', '\\').TrimStart('\\');

            var hostDocument = new HostDocument(textDocumentPath, normalizedTargetFilePath);
            var textLoader = _remoteTextLoaderFactory.Create(textDocumentPath);

            _logger.LogInformation($"Adding document '{filePath}' to project '{projectSnapshot.Key}'.");

            updater.DocumentAdded(projectSnapshot.Key, hostDocument, textLoader);

            // Adding a document to a project could also happen because a target was added to a project, or we're moving a document
            // from Misc Project to a real one, and means the newly added document could actually already be open.
            // If it is, we need to make sure we start generating it so we're ready to handle requests that could start coming in.
            if (_projectManager.IsDocumentOpen(textDocumentPath) &&
                _projectManager.TryGetLoadedProject(projectSnapshot.Key, out var project) &&
                project.GetDocument(textDocumentPath) is { } document)
            {
                document.GetGeneratedOutputAsync().Forget();
            }
        }
    }

    public Task OpenDocumentAsync(string filePath, SourceText sourceText, int version, CancellationToken cancellationToken)
    {
        return _projectManager.UpdateAsync(
            updater =>
            {
                var textDocumentPath = FilePathNormalizer.Normalize(filePath);

                // We are okay to use the non-project-key overload of TryResolveDocument here because we really are just checking if the document
                // has been added to _any_ project. AddDocument will take care of adding to all of the necessary ones, and then below we ensure
                // we process them all too
                if (!_snapshotResolver.TryResolveDocumentInAnyProject(textDocumentPath, out var document))
                {
                    // Document hasn't been added. This usually occurs when VSCode trumps all other initialization
                    // processes and pre-initializes already open documents.
                    AddDocumentCore(updater, filePath);
                }

                ActOnDocumentInMultipleProjects(
                    filePath,
                    (projectSnapshot, textDocumentPath) =>
                    {
                        _logger.LogInformation($"Opening document '{textDocumentPath}' in project '{projectSnapshot.Key}'.");
                        updater.DocumentOpened(projectSnapshot.Key, textDocumentPath, sourceText);
                    });

                // Use a separate loop, as the above call modified out projects, so we have to make sure we're operating on the latest snapshot
                ActOnDocumentInMultipleProjects(
                    filePath,
                    (projectSnapshot, textDocumentPath) =>
                    {
                        TrackDocumentVersion(projectSnapshot, textDocumentPath, version, startGenerating: true);
                    });
            },
            cancellationToken);
    }

    public Task CloseDocumentAsync(string filePath, CancellationToken cancellationToken)
    {
        return _projectManager.UpdateAsync(
            updater =>
            {
                ActOnDocumentInMultipleProjects(
                    filePath,
                    (projectSnapshot, textDocumentPath) =>
                    {
                        var textLoader = _remoteTextLoaderFactory.Create(filePath);
                        _logger.LogInformation($"Closing document '{textDocumentPath}' in project '{projectSnapshot.Key}'.");

                        updater.DocumentClosed(projectSnapshot.Key, textDocumentPath, textLoader);
                    });
            },
            cancellationToken);
    }

    public Task RemoveDocumentAsync(string filePath, CancellationToken cancellationToken)
    {
        return _projectManager.UpdateAsync(
            updater =>
            {
                ActOnDocumentInMultipleProjects(
                    filePath,
                    (projectSnapshot, textDocumentPath) =>
                    {
                        if (!projectSnapshot.DocumentFilePaths.Contains(textDocumentPath, FilePathComparer.Instance))
                        {
                            _logger.LogInformation($"Containing project is not tracking document '{textDocumentPath}'");
                            return;
                        }

                        if (projectSnapshot.GetDocument(textDocumentPath) is not DocumentSnapshot documentSnapshot)
                        {
                            _logger.LogError($"Containing project does not contain document '{textDocumentPath}'");
                            return;
                        }

                        // If the document is open, we can't remove it, because we could still get a request for it, and that
                        // request would fail. Instead we move it to the miscellaneous project, just like if we got notified of
                        // a remove via the project.razor.bin
                        if (_projectManager.IsDocumentOpen(textDocumentPath))
                        {
                            _logger.LogInformation($"Moving document '{textDocumentPath}' from project '{projectSnapshot.Key}' to misc files because it is open.");
                            var miscellaneousProject = _snapshotResolver.GetMiscellaneousProject();
                            if (projectSnapshot != miscellaneousProject)
                            {
                                MoveDocument(updater, textDocumentPath, fromProject: projectSnapshot, toProject: miscellaneousProject);
                            }
                        }
                        else
                        {
                            _logger.LogInformation($"Removing document '{textDocumentPath}' from project '{projectSnapshot.Key}'.");

                            updater.DocumentRemoved(projectSnapshot.Key, documentSnapshot.State.HostDocument);
                        }
                    });
            },
            cancellationToken);
    }

    public Task UpdateDocumentAsync(string filePath, SourceText sourceText, int version, CancellationToken cancellationToken)
    {
        return _projectManager.UpdateAsync(
            updater =>
            {
                ActOnDocumentInMultipleProjects(
                    filePath,
                    (project, textDocumentPath) =>
                    {
                        _logger.LogTrace($"Updating document '{textDocumentPath}' in {project.Key}.");

                        updater.DocumentChanged(project.Key, textDocumentPath, sourceText);
                    });

                // Use a separate loop, as the above call modified out projects, so we have to make sure we're operating on the latest snapshot
                ActOnDocumentInMultipleProjects(
                    filePath,
                    (projectSnapshot, textDocumentPath) =>
                    {
                        TrackDocumentVersion(projectSnapshot, textDocumentPath, version, startGenerating: false);
                    });
            },
            cancellationToken);
    }

    private void ActOnDocumentInMultipleProjects(string filePath, Action<IProjectSnapshot, string> action)
    {
        var textDocumentPath = FilePathNormalizer.Normalize(filePath);
        if (!_snapshotResolver.TryResolveAllProjects(textDocumentPath, out var projects))
        {
            var miscFilesProject = _snapshotResolver.GetMiscellaneousProject();
            projects = [miscFilesProject];
        }

        foreach (var project in projects)
        {
            action(project, textDocumentPath);
        }
    }

    public Task<ProjectKey> AddProjectAsync(
        string filePath,
        string intermediateOutputPath,
        RazorConfiguration? configuration,
        string? rootNamespace,
        string? displayName,
        CancellationToken cancellationToken)
    {
        return _projectManager.UpdateAsync(
            updater =>
            {
                var normalizedPath = FilePathNormalizer.Normalize(filePath);
                var hostProject = new HostProject(
                    normalizedPath, intermediateOutputPath, configuration ?? FallbackRazorConfiguration.Latest, rootNamespace, displayName);

                // ProjectAdded will no-op if the project already exists
                updater.ProjectAdded(hostProject);

                _logger.LogInformation($"Added project '{filePath}' with key {hostProject.Key} to project system.");

                TryMigrateMiscellaneousDocumentsToProject(updater);

                return hostProject.Key;
            },
            cancellationToken);
    }

    public Task UpdateProjectAsync(
        ProjectKey projectKey,
        RazorConfiguration? configuration,
        string? rootNamespace,
        string? displayName,
        ProjectWorkspaceState projectWorkspaceState,
        ImmutableArray<DocumentSnapshotHandle> documents,
        CancellationToken cancellationToken)
    {
        return _projectManager.UpdateAsync(
            updater =>
            {
                if (!_projectManager.TryGetLoadedProject(projectKey, out var project))
                {
                    // Never tracked the project to begin with, noop.
                    _logger.LogInformation($"Failed to update untracked project '{projectKey}'.");
                    return;
                }

                UpdateProjectDocuments(updater, documents, project.Key);

                if (!projectWorkspaceState.Equals(ProjectWorkspaceState.Default))
                {
                    _logger.LogInformation($"Updating project '{project.Key}' TagHelpers ({projectWorkspaceState.TagHelpers.Length}) and C# Language Version ({projectWorkspaceState.CSharpLanguageVersion}).");
                }

                updater.ProjectWorkspaceStateChanged(project.Key, projectWorkspaceState);

                var currentConfiguration = project.Configuration;
                var currentRootNamespace = project.RootNamespace;
                if (currentConfiguration.ConfigurationName == configuration?.ConfigurationName &&
                    currentRootNamespace == rootNamespace)
                {
                    _logger.LogTrace($"Updating project '{project.Key}'. The project is already using configuration '{configuration.ConfigurationName}' and root namespace '{rootNamespace}'.");
                    return;
                }

                if (configuration is null)
                {
                    configuration = FallbackRazorConfiguration.Latest;
                    _logger.LogInformation($"Updating project '{project.Key}' to use the latest configuration ('{configuration.ConfigurationName}')'.");
                }
                else if (currentConfiguration.ConfigurationName != configuration.ConfigurationName)
                {
                    _logger.LogInformation($"Updating project '{project.Key}' to Razor configuration '{configuration.ConfigurationName}' with language version '{configuration.LanguageVersion}'.");
                }

                if (currentRootNamespace != rootNamespace)
                {
                    _logger.LogInformation($"Updating project '{project.Key}''s root namespace to '{rootNamespace}'.");
                }

                var hostProject = new HostProject(project.FilePath, project.IntermediateOutputPath, configuration, rootNamespace, displayName);
                updater.ProjectConfigurationChanged(hostProject);
            },
            cancellationToken);
    }

    private void UpdateProjectDocuments(
        ProjectSnapshotManager.Updater updater,
        ImmutableArray<DocumentSnapshotHandle> documents,
        ProjectKey projectKey)
    {
        _logger.LogDebug($"UpdateProjectDocuments for {projectKey} with {documents.Length} documents: {string.Join(", ", documents.Select(d => d.FilePath))}");

        var project = _projectManager.GetLoadedProject(projectKey);
        var currentProjectKey = project.Key;
        var projectDirectory = FilePathNormalizer.GetNormalizedDirectoryName(project.FilePath);
        var documentMap = documents.ToDictionary(document => EnsureFullPath(document.FilePath, projectDirectory), FilePathComparer.Instance);
        var miscellaneousProject = _snapshotResolver.GetMiscellaneousProject();

        // "Remove" any unnecessary documents by putting them into the misc project
        foreach (var documentFilePath in project.DocumentFilePaths)
        {
            if (documentMap.ContainsKey(documentFilePath))
            {
                // This document still exists in the updated project
                continue;
            }

            _logger.LogDebug($"Document '{documentFilePath}' no longer exists in project '{projectKey}'. Moving to miscellaneous project.");

            MoveDocument(updater, documentFilePath, fromProject: project, toProject: miscellaneousProject);
        }

        project = _projectManager.GetLoadedProject(projectKey);

        // Update existing documents
        foreach (var documentFilePath in project.DocumentFilePaths)
        {
            if (!documentMap.TryGetValue(documentFilePath, out var documentHandle))
            {
                // Document exists in the project but not in the configured documents. Chances are the project configuration is from a fallback
                // configuration case (< 2.1) or the project isn't fully loaded yet.
                continue;
            }

            if (project.GetDocument(documentFilePath) is not DocumentSnapshot documentSnapshot)
            {
                continue;
            }

            var currentHostDocument = documentSnapshot.State.HostDocument;
            var newFilePath = EnsureFullPath(documentHandle.FilePath, projectDirectory);
            var newHostDocument = new HostDocument(newFilePath, documentHandle.TargetPath, documentHandle.FileKind);

            if (HostDocumentComparer.Instance.Equals(currentHostDocument, newHostDocument))
            {
                // Current and "new" host documents are equivalent
                continue;
            }

            _logger.LogTrace($"Updating document '{newHostDocument.FilePath}''s file kind to '{newHostDocument.FileKind}' and target path to '{newHostDocument.TargetPath}'.");

            var remoteTextLoader = _remoteTextLoaderFactory.Create(newFilePath);

            updater.DocumentRemoved(currentProjectKey, currentHostDocument);
            updater.DocumentAdded(currentProjectKey, newHostDocument, remoteTextLoader);
        }

        project = _projectManager.GetLoadedProject(project.Key);
        miscellaneousProject = _snapshotResolver.GetMiscellaneousProject();

        // Add (or migrate from misc) any new documents
        foreach (var documentKvp in documentMap)
        {
            var documentFilePath = documentKvp.Key;
            if (project.DocumentFilePaths.Contains(documentFilePath, FilePathComparer.Instance))
            {
                // Already know about this document
                continue;
            }

            if (miscellaneousProject.DocumentFilePaths.Contains(documentFilePath, FilePathComparer.Instance))
            {
                MoveDocument(updater, documentFilePath, fromProject: miscellaneousProject, toProject: project);
            }
            else
            {
                var documentHandle = documentKvp.Value;
                var remoteTextLoader = _remoteTextLoaderFactory.Create(documentFilePath);
                var newHostDocument = new HostDocument(documentFilePath, documentHandle.TargetPath, documentHandle.FileKind);

                _logger.LogInformation($"Adding new document '{documentFilePath}' to project '{currentProjectKey}'.");

                updater.DocumentAdded(currentProjectKey, newHostDocument, remoteTextLoader);
            }
        }
    }

    private void MoveDocument(
        ProjectSnapshotManager.Updater updater,
        string documentFilePath,
        IProjectSnapshot fromProject,
        IProjectSnapshot toProject)
    {
        Debug.Assert(fromProject.DocumentFilePaths.Contains(documentFilePath, FilePathComparer.Instance));
        Debug.Assert(!toProject.DocumentFilePaths.Contains(documentFilePath, FilePathComparer.Instance));

        if (fromProject.GetDocument(documentFilePath) is not DocumentSnapshot documentSnapshot)
        {
            return;
        }

        var currentHostDocument = documentSnapshot.State.HostDocument;

        var textLoader = new DocumentSnapshotTextLoader(documentSnapshot);
        var newHostDocument = new HostDocument(documentSnapshot.FilePath, documentSnapshot.TargetPath, documentSnapshot.FileKind);

        _logger.LogInformation($"Moving '{documentFilePath}' from the '{fromProject.Key}' project to '{toProject.Key}' project.");

        updater.DocumentRemoved(fromProject.Key, currentHostDocument);
        updater.DocumentAdded(toProject.Key, newHostDocument, textLoader);
    }

    private static string EnsureFullPath(string filePath, string projectDirectory)
    {
        var normalizedFilePath = FilePathNormalizer.Normalize(filePath);
        if (!normalizedFilePath.StartsWith(projectDirectory, FilePathComparison.Instance))
        {
            var absolutePath = Path.Combine(projectDirectory, normalizedFilePath);
            normalizedFilePath = FilePathNormalizer.Normalize(absolutePath);
        }

        return normalizedFilePath;
    }

    private void TryMigrateMiscellaneousDocumentsToProject(ProjectSnapshotManager.Updater updater)
    {
        var miscellaneousProject = _snapshotResolver.GetMiscellaneousProject();

        foreach (var documentFilePath in miscellaneousProject.DocumentFilePaths)
        {
            var projectSnapshot = _snapshotResolver.FindPotentialProjects(documentFilePath).FirstOrDefault();
            if (projectSnapshot is null)
            {
                continue;
            }

            if (miscellaneousProject.GetDocument(documentFilePath) is not DocumentSnapshot documentSnapshot)
            {
                continue;
            }

            // Remove from miscellaneous project
            var defaultMiscProject = miscellaneousProject;

            updater.DocumentRemoved(defaultMiscProject.Key, documentSnapshot.State.HostDocument);

            // Add to new project

            var textLoader = new DocumentSnapshotTextLoader(documentSnapshot);
            var defaultProject = projectSnapshot;
            var newHostDocument = new HostDocument(documentSnapshot.FilePath, documentSnapshot.TargetPath);
            _logger.LogInformation($"Migrating '{documentFilePath}' from the '{miscellaneousProject.Key}' project to '{projectSnapshot.Key}' project.");

            updater.DocumentAdded(defaultProject.Key, newHostDocument, textLoader);
        }
    }

    private void TrackDocumentVersion(IProjectSnapshot projectSnapshot, string textDocumentPath, int version, bool startGenerating)
    {
        if (projectSnapshot.GetDocument(FilePathNormalizer.Normalize(textDocumentPath)) is not { } documentSnapshot)
        {
            return;
        }

        _documentVersionCache.TrackDocumentVersion(documentSnapshot, version);

        if (startGenerating)
        {
            // Start generating the C# for the document so it can immediately be ready for incoming requests.
            documentSnapshot.GetGeneratedOutputAsync().Forget();
        }
    }
}
