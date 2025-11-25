// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Serialization;
using Microsoft.CodeAnalysis.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

/// <summary>
/// Maintains the language server's <see cref="ProjectSnapshotManager"/> with the semantics of Razor's project model.
/// </summary>
/// <remarks>
/// This service implements <see cref="IRazorStartupService"/> to ensure it is created early so it can begin
/// initializing immediately.
/// </remarks>
internal partial class RazorProjectService : IRazorProjectService, IRazorProjectInfoListener, IRazorStartupService, IDisposable
{
    private readonly IRazorProjectInfoDriver _projectInfoDriver;
    private readonly ProjectSnapshotManager _projectManager;
    private readonly RemoteTextLoaderFactory _remoteTextLoaderFactory;
    private readonly ILogger _logger;

    private readonly CancellationTokenSource _disposeTokenSource;
    private readonly Task _initializeTask;

    public RazorProjectService(
        ProjectSnapshotManager projectManager,
        IRazorProjectInfoDriver projectInfoDriver,
        RemoteTextLoaderFactory remoteTextLoaderFactory,
        ILoggerFactory loggerFactory)
    {
        _projectInfoDriver = projectInfoDriver;
        _projectManager = projectManager;
        _remoteTextLoaderFactory = remoteTextLoaderFactory;
        _logger = loggerFactory.GetOrCreateLogger<RazorProjectService>();

        // We kick off initialization immediately to ensure that the IRazorProjectService
        // is hot and ready to go when requests come in.
        _disposeTokenSource = new();
        _initializeTask = InitializeAsync(_disposeTokenSource.Token);
    }

    public void Dispose()
    {
        if (_disposeTokenSource.IsCancellationRequested)
        {
            return;
        }

        _disposeTokenSource.Cancel();
        _disposeTokenSource.Dispose();
    }

    private async Task InitializeAsync(CancellationToken cancellationToken)
    {
        _logger.LogTrace($"Initializing {nameof(RazorProjectService)}...");

        await _projectInfoDriver.WaitForInitializationAsync().ConfigureAwait(false);

        // Register ourselves as a listener to the project driver.
        _projectInfoDriver.AddListener(this);

        // Add all existing projects from the driver.
        foreach (var projectInfo in _projectInfoDriver.GetLatestProjectInfo())
        {
            await AddOrUpdateProjectCoreAsync(
                projectInfo.ProjectKey,
                projectInfo.FilePath,
                projectInfo.Configuration,
                projectInfo.RootNamespace,
                projectInfo.DisplayName,
                projectInfo.ProjectWorkspaceState,
                projectInfo.Documents,
                cancellationToken)
                .ConfigureAwait(false);
        }

        _logger.LogTrace($"{nameof(RazorProjectService)} initialized.");

    }

    // Call to ensure that any public IRazorProjectService methods wait for initialization to complete.
    private ValueTask WaitForInitializationAsync()
        => _initializeTask is { IsCompleted: true }
            ? default
            : new(_initializeTask);

    async Task IRazorProjectInfoListener.UpdatedAsync(RazorProjectInfo projectInfo, CancellationToken cancellationToken)
    {
        // Don't update a project during initialization.
        await WaitForInitializationAsync().ConfigureAwait(false);

        _logger.LogTrace($"{nameof(IRazorProjectInfoListener)} received update for {projectInfo.ProjectKey}");

        await AddOrUpdateProjectCoreAsync(
            projectInfo.ProjectKey,
            projectInfo.FilePath,
            projectInfo.Configuration,
            projectInfo.RootNamespace,
            projectInfo.DisplayName,
            projectInfo.ProjectWorkspaceState,
            projectInfo.Documents,
            cancellationToken)
            .ConfigureAwait(false);
    }

    async Task IRazorProjectInfoListener.RemovedAsync(ProjectKey projectKey, CancellationToken cancellationToken)
    {
        // Don't remove a project during initialization.
        await WaitForInitializationAsync().ConfigureAwait(false);

        _logger.LogTrace($"{nameof(IRazorProjectInfoListener)} received remove for {projectKey}");

        await AddOrUpdateProjectCoreAsync(
            projectKey,
            filePath: null,
            configuration: null,
            rootNamespace: null,
            displayName: "",
            ProjectWorkspaceState.Default,
            documents: [],
            cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task AddDocumentsToMiscProjectAsync(ImmutableArray<string> filePaths, CancellationToken cancellationToken)
    {
        await WaitForInitializationAsync().ConfigureAwait(false);

        await _projectManager
            .UpdateAsync(
                (updater, cancellationToken) =>
                {
                    var projects = _projectManager.GetProjects();

                    // For each file, check to see if it's already in a project.
                    // If it is, we don't want to add it to the misc project.
                    foreach (var filePath in filePaths)
                    {
                        var add = true;

                        foreach (var project in projects)
                        {
                            if (project.ContainsDocument(filePath))
                            {
                                // The file is already in a project, so we shouldn't add it to the misc project.
                                add = false;
                                break;
                            }
                        }

                        if (cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }

                        if (add)
                        {
                            AddDocumentToMiscProjectCore(updater, filePath);
                        }
                    }
                },
                state: cancellationToken,
                cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task AddDocumentToMiscProjectAsync(string filePath, CancellationToken cancellationToken)
    {
        await WaitForInitializationAsync().ConfigureAwait(false);

        await _projectManager
            .UpdateAsync(
                updater =>
                {
                    if (!_projectManager.TryFindContainingProject(filePath, out _))
                    {
                        AddDocumentToMiscProjectCore(updater, filePath);
                    }
                },
                cancellationToken)
            .ConfigureAwait(false);
    }

    private void AddDocumentToMiscProjectCore(ProjectSnapshotManager.Updater updater, string filePath, SourceText? sourceText = null)
    {
        Debug.Assert(
            !_projectManager.TryFindContainingProject(filePath, out _),
            $"File already belongs to a project and can't be added to the misc files project");

        _logger.LogInformation($"Adding document '{filePath}' to miscellaneous files project.");

        var miscFilesProject = _projectManager.GetMiscellaneousProject();
        var textDocumentPath = FilePathNormalizer.Normalize(filePath);

        // Representing all of our host documents with a re-normalized target path to workaround GetRelatedDocument limitations.
        var normalizedTargetFilePath = textDocumentPath.Replace('/', '\\').TrimStart('\\');

        var hostDocument = new HostDocument(textDocumentPath, normalizedTargetFilePath);

        if (sourceText is not null)
        {
            updater.AddDocument(miscFilesProject.Key, hostDocument, sourceText);
        }
        else
        {
            updater.AddDocument(miscFilesProject.Key, hostDocument, _remoteTextLoaderFactory.Create(textDocumentPath));
        }
    }

    public async Task OpenDocumentAsync(string filePath, SourceText sourceText, CancellationToken cancellationToken)
    {
        await WaitForInitializationAsync().ConfigureAwait(false);

        await _projectManager.UpdateAsync(
            updater =>
            {
                // We are okay to use the non-project-key overload of TryResolveDocument here because we really are just checking if the document
                // has been added to _any_ project. AddDocument will take care of adding to all of the necessary ones, and then below we ensure
                // we process them all too
                if (!_projectManager.TryFindContainingProject(filePath, out _))
                {
                    // Document hasn't been added. This usually occurs when VSCode trumps all other initialization
                    // processes and pre-initializes already open documents. We add this to the misc project, and
                    // if/when we get project info from the client, it will be migrated to a real project.
                    AddDocumentToMiscProjectCore(updater, filePath, sourceText);
                }

                ActOnDocumentInMultipleProjects(
                    filePath,
                    (project, textDocumentPath) =>
                    {
                        _logger.LogInformation($"Opening document '{textDocumentPath}' in project '{project.Key}'.");
                        updater.OpenDocument(project.Key, textDocumentPath, sourceText);
                    });
            },
            cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task CloseDocumentAsync(string filePath, CancellationToken cancellationToken)
    {
        await WaitForInitializationAsync().ConfigureAwait(false);

        await _projectManager.UpdateAsync(
            updater =>
            {
                ActOnDocumentInMultipleProjects(
                    filePath,
                    (project, textDocumentPath) =>
                    {
                        var textLoader = _remoteTextLoaderFactory.Create(filePath);
                        _logger.LogInformation($"Closing document '{textDocumentPath}' in project '{project.Key}'.");

                        updater.CloseDocument(project.Key, textDocumentPath, textLoader);
                    });
            },
            cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task RemoveDocumentAsync(string filePath, CancellationToken cancellationToken)
    {
        await WaitForInitializationAsync().ConfigureAwait(false);

        await _projectManager.UpdateAsync(
            updater =>
            {
                ActOnDocumentInMultipleProjects(
                    filePath,
                    (project, textDocumentPath) =>
                    {
                        if (!project.TryGetDocument(textDocumentPath, out var document))
                        {
                            _logger.LogError($"Containing project does not contain document '{textDocumentPath}'");
                            return;
                        }

                        // If the document is open, we can't remove it, because we could still get a request for it, and that
                        // request would fail. Instead we move it to the miscellaneous project, just like if we got notified of
                        // a remove via the project.razor.bin
                        if (_projectManager.IsDocumentOpen(textDocumentPath))
                        {
                            _logger.LogInformation($"Moving document '{textDocumentPath}' from project '{project.Key}' to misc files because it is open.");
                            if (!project.IsMiscellaneousProject())
                            {
                                var miscellaneousProject = _projectManager.GetMiscellaneousProject();
                                MoveDocument(updater, textDocumentPath, fromProject: project, toProject: miscellaneousProject);
                            }
                        }
                        else
                        {
                            _logger.LogInformation($"Removing document '{textDocumentPath}' from project '{project.Key}'.");

                            updater.RemoveDocument(project.Key, document.FilePath);
                        }
                    });
            },
            cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task UpdateDocumentAsync(string filePath, SourceText sourceText, CancellationToken cancellationToken)
    {
        await WaitForInitializationAsync().ConfigureAwait(false);

        await _projectManager.UpdateAsync(
            updater =>
            {
                ActOnDocumentInMultipleProjects(
                    filePath,
                    (project, textDocumentPath) =>
                    {
                        _logger.LogTrace($"Updating document '{textDocumentPath}' in {project.Key}.");

                        updater.UpdateDocumentText(project.Key, textDocumentPath, sourceText);
                    });
            },
            cancellationToken)
            .ConfigureAwait(false);
    }

    private void ActOnDocumentInMultipleProjects(string filePath, Action<ProjectSnapshot, string> action)
    {
        var textDocumentPath = FilePathNormalizer.Normalize(filePath);
        if (!_projectManager.TryResolveAllProjects(textDocumentPath, out var projects))
        {
            var miscFilesProject = _projectManager.GetMiscellaneousProject();
            projects = [miscFilesProject];
        }

        foreach (var project in projects)
        {
            action(project, textDocumentPath);
        }
    }

    private ProjectKey AddProjectCore(ProjectSnapshotManager.Updater updater, string filePath, string intermediateOutputPath, RazorConfiguration? configuration, string? rootNamespace, string? displayName)
    {
        var normalizedPath = FilePathNormalizer.Normalize(filePath);
        var hostProject = new HostProject(
            normalizedPath, intermediateOutputPath, configuration ?? FallbackRazorConfiguration.Latest, rootNamespace, displayName);

        // ProjectAdded will no-op if the project already exists
        updater.AddProject(hostProject);

        _logger.LogInformation($"Added project '{filePath}' with key {hostProject.Key} to project system.");

        return hostProject.Key;
    }

    private Task AddOrUpdateProjectCoreAsync(
        ProjectKey projectKey,
        string? filePath,
        RazorConfiguration? configuration,
        string? rootNamespace,
        string? displayName,
        ProjectWorkspaceState projectWorkspaceState,
        ImmutableArray<DocumentSnapshotHandle> documents,
        CancellationToken cancellationToken)
    {
        // Note: We specifically don't wait for initialization here because this is called *during* initialization.
        // All other callers of this method must await WaitForInitializationAsync().

        return _projectManager.UpdateAsync(
            updater =>
            {
                if (!_projectManager.TryGetProject(projectKey, out var project))
                {
                    if (filePath is null)
                    {
                        // Never tracked the project to begin with, noop.
                        _logger.LogInformation($"Failed to update untracked project '{projectKey}'.");
                        return;
                    }

                    // If we've been given a project file path, then we have enough info to add the project ourselves, because we know
                    // the intermediate output path from the id
                    var intermediateOutputPath = projectKey.Id;

                    var newKey = AddProjectCore(updater, filePath, intermediateOutputPath, configuration, rootNamespace, displayName);
                    Debug.Assert(newKey == projectKey);

                    project = _projectManager.GetRequiredProject(projectKey);
                }

                UpdateProjectDocuments(updater, documents, project.Key);

                if (!projectWorkspaceState.IsDefault)
                {
                    _logger.LogInformation($"Updating project '{project.Key}' TagHelpers ({projectWorkspaceState.TagHelpers.Count}).");
                }

                updater.UpdateProjectWorkspaceState(project.Key, projectWorkspaceState);

                var currentConfiguration = project.Configuration;
                var currentRootNamespace = project.RootNamespace;
                if (currentConfiguration == configuration &&
                    currentRootNamespace == rootNamespace)
                {
                    _logger.LogTrace($"Updating project '{project.Key}'. The project is already using configuration '{configuration.ConfigurationName}' and root namespace '{rootNamespace}' and C# lang version '{configuration.CSharpLanguageVersion}'.");
                    return;
                }

                if (configuration is null)
                {
                    configuration = FallbackRazorConfiguration.Latest;
                    _logger.LogInformation($"Updating project '{project.Key}' to use the latest configuration ('{configuration.ConfigurationName}')'.");
                }
                else
                {
                    _logger.LogInformation($"Updating project '{project.Key}' to Razor configuration '{configuration.ConfigurationName}' with language version '{configuration.LanguageVersion}' and C# lang version '{configuration.CSharpLanguageVersion}'.");
                }

                if (currentRootNamespace != rootNamespace)
                {
                    _logger.LogInformation($"Updating project '{project.Key}''s root namespace to '{rootNamespace}'.");
                }

                var hostProject = new HostProject(project.FilePath, project.IntermediateOutputPath, configuration, rootNamespace, displayName);
                updater.UpdateProjectConfiguration(hostProject);
            },
            cancellationToken);
    }

    private void UpdateProjectDocuments(
        ProjectSnapshotManager.Updater updater,
        ImmutableArray<DocumentSnapshotHandle> documents,
        ProjectKey projectKey)
    {
        _logger.LogDebug($"UpdateProjectDocuments for {projectKey} with {documents.Length} documents: {string.Join(", ", documents.Select(d => d.FilePath))}");

        var project = _projectManager.GetRequiredProject(projectKey);
        var currentProjectKey = project.Key;
        var projectDirectory = FilePathNormalizer.GetNormalizedDirectoryName(project.FilePath);
        var documentMap = documents.ToDictionary(document => EnsureFullPath(document.FilePath, projectDirectory), PathUtilities.OSSpecificPathComparer);
        var miscellaneousProject = _projectManager.GetMiscellaneousProject();

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

        project = _projectManager.GetRequiredProject(projectKey);

        // Update existing documents
        foreach (var documentFilePath in project.DocumentFilePaths)
        {
            if (!documentMap.TryGetValue(documentFilePath, out var documentHandle))
            {
                // Document exists in the project but not in the configured documents. Chances are the project configuration is from a fallback
                // configuration case (< 2.1) or the project isn't fully loaded yet.
                continue;
            }

            if (!project.TryGetDocument(documentFilePath, out var document))
            {
                continue;
            }

            var currentHostDocument = document.HostDocument;
            var newFilePath = EnsureFullPath(documentHandle.FilePath, projectDirectory);
            var newHostDocument = new HostDocument(newFilePath, documentHandle.TargetPath, documentHandle.FileKind);

            if (HostDocumentComparer.Instance.Equals(currentHostDocument, newHostDocument))
            {
                // Current and "new" host documents are equivalent
                continue;
            }

            _logger.LogTrace($"Updating document '{newHostDocument.FilePath}''s file kind to '{newHostDocument.FileKind}' and target path to '{newHostDocument.TargetPath}'.");

            // If the physical file name hasn't changed, we use the current document snapshot as the source of truth for text, in case
            // it has received text change info from LSP. eg, if someone changes the TargetPath of the file while its open in the editor
            // with unsaved changes, we don't want to reload it from disk.
            var textLoader = PathUtilities.OSSpecificPathComparer.Equals(currentHostDocument.FilePath, newHostDocument.FilePath)
                ? new DocumentSnapshotTextLoader(document)
                : _remoteTextLoaderFactory.Create(newFilePath);

            updater.RemoveDocument(currentProjectKey, currentHostDocument.FilePath);
            updater.AddDocument(currentProjectKey, newHostDocument, textLoader);
        }

        project = _projectManager.GetRequiredProject(project.Key);
        miscellaneousProject = _projectManager.GetMiscellaneousProject();

        // Add (or migrate from misc) any new documents
        foreach (var documentKvp in documentMap)
        {
            var documentFilePath = documentKvp.Key;
            if (project.ContainsDocument(documentFilePath))
            {
                // Already know about this document
                continue;
            }

            if (miscellaneousProject.ContainsDocument(documentFilePath))
            {
                MoveDocument(updater, documentFilePath, fromProject: miscellaneousProject, toProject: project);
            }
            else
            {
                var documentHandle = documentKvp.Value;
                var remoteTextLoader = _remoteTextLoaderFactory.Create(documentFilePath);
                var newHostDocument = new HostDocument(documentFilePath, documentHandle.TargetPath, documentHandle.FileKind);

                _logger.LogInformation($"Adding new document '{documentFilePath}' to project '{currentProjectKey}'.");

                updater.AddDocument(currentProjectKey, newHostDocument, remoteTextLoader);
            }
        }
    }

    private void MoveDocument(
        ProjectSnapshotManager.Updater updater,
        string documentFilePath,
        ProjectSnapshot fromProject,
        ProjectSnapshot toProject)
    {
        Debug.Assert(fromProject.ContainsDocument(documentFilePath));
        Debug.Assert(!toProject.ContainsDocument(documentFilePath));

        if (!fromProject.TryGetDocument(documentFilePath, out var document))
        {
            return;
        }

        var currentHostDocument = document.HostDocument;

        var textLoader = new DocumentSnapshotTextLoader(document);

        // If we're moving from the misc files project to a real project, then target path will be the full path to the file
        // and the next update to the project will update it to be a relative path. To save a bunch of busy work if that is
        // the only change necessary, we can proactively do that work here.
        var projectDirectory = FilePathNormalizer.GetNormalizedDirectoryName(toProject.FilePath);
        var newTargetPath = document.TargetPath;
        if (FilePathNormalizer.Normalize(newTargetPath).StartsWith(projectDirectory))
        {
            newTargetPath = newTargetPath[projectDirectory.Length..];
        }

        var newHostDocument = new HostDocument(document.FilePath, newTargetPath, document.FileKind);

        _logger.LogInformation($"Moving '{documentFilePath}' from the '{fromProject.Key}' project to '{toProject.Key}' project.");

        updater.RemoveDocument(fromProject.Key, currentHostDocument.FilePath);
        updater.AddDocument(toProject.Key, newHostDocument, textLoader);
    }

    private static string EnsureFullPath(string filePath, string projectDirectory)
    {
        var normalizedFilePath = FilePathNormalizer.Normalize(filePath);
        if (!normalizedFilePath.StartsWith(projectDirectory, PathUtilities.OSSpecificPathComparison))
        {
            var absolutePath = Path.Combine(projectDirectory, normalizedFilePath);
            normalizedFilePath = FilePathNormalizer.Normalize(absolutePath);
        }

        return normalizedFilePath;
    }
}
