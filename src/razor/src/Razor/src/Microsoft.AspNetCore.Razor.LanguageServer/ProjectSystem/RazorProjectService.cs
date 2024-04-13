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
    : IRazorProjectService, IDisposable
{
    private readonly IProjectSnapshotManager _projectManager = projectManager;
    private readonly RemoteTextLoaderFactory _remoteTextLoaderFactory = remoteTextLoaderFactory;
    private readonly ISnapshotResolver _snapshotResolver = snapshotResolver;
    private readonly IDocumentVersionCache _documentVersionCache = documentVersionCache;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<RazorProjectService>();

    // This lock is used to ensure that the public entry points to the project service,
    // i.e. AddDocumentAsync, OpenDocumentAsync, etc., cannot interleave.
    private readonly AsyncSemaphore _gate = new(initialCount: 1);

    public void Dispose()
    {
        _gate.Dispose();
    }

    public async Task AddDocumentAsync(string filePath, CancellationToken cancellationToken)
    {
        using var _ = await _gate.EnterAsync(cancellationToken).ConfigureAwait(false);

        await AddDocumentNeedsLocksAsync(filePath, cancellationToken).ConfigureAwait(false);
    }

    private async Task AddDocumentNeedsLocksAsync(string filePath, CancellationToken cancellationToken)
    {
        var textDocumentPath = FilePathNormalizer.Normalize(filePath);

        var added = false;
        foreach (var projectSnapshot in _snapshotResolver.FindPotentialProjects(textDocumentPath))
        {
            added = true;
            await AddDocumentToProjectAsync(projectSnapshot, textDocumentPath, cancellationToken).ConfigureAwait(false);
        }

        if (!added)
        {
            var miscFilesProject = await _snapshotResolver.GetMiscellaneousProjectAsync(cancellationToken).ConfigureAwait(false);
            await AddDocumentToProjectAsync(miscFilesProject, textDocumentPath, cancellationToken).ConfigureAwait(false);
        }

        async Task AddDocumentToProjectAsync(IProjectSnapshot projectSnapshot, string textDocumentPath, CancellationToken cancellationToken)
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

            await _projectManager
                .UpdateAsync(
                    static (updater, state) => updater.DocumentAdded(state.key, state.hostDocument, state.textLoader),
                    state: (key: projectSnapshot.Key, hostDocument, textLoader),
                    cancellationToken)
                .ConfigureAwait(false);

            // Adding a document to a project could also happen because a target was added to a project, or we're moving a document
            // from Misc Project to a real one, and means the newly added document could actually already be open.
            // If it is, we need to make sure we start generating it so we're ready to handle requests that could start coming in.
            if (_projectManager.IsDocumentOpen(textDocumentPath) &&
                _projectManager.TryGetLoadedProject(projectSnapshot.Key, out var project) &&
                project.GetDocument(textDocumentPath) is { } document)
            {
                _ = document.GetGeneratedOutputAsync();
            }
        }
    }

    public async Task OpenDocumentAsync(string filePath, SourceText sourceText, int version, CancellationToken cancellationToken)
    {
        using var _ = await _gate.EnterAsync(cancellationToken).ConfigureAwait(false);

        var textDocumentPath = FilePathNormalizer.Normalize(filePath);

        // We are okay to use the non-project-key overload of TryResolveDocument here because we really are just checking if the document
        // has been added to _any_ project. AddDocument will take care of adding to all of the necessary ones, and then below we ensure
        // we process them all too
        var document = await _snapshotResolver
            .ResolveDocumentInAnyProjectAsync(textDocumentPath, cancellationToken)
            .ConfigureAwait(false);

        if (document is null)
        {
            // Document hasn't been added. This usually occurs when VSCode trumps all other initialization
            // processes and pre-initializes already open documents.
            await AddDocumentNeedsLocksAsync(filePath, cancellationToken).ConfigureAwait(false);
        }

        await ActOnDocumentInMultipleProjectsAsync(
            filePath,
            async (projectSnapshot, textDocumentPath, cancellationToken) =>
            {
                _logger.LogInformation($"Opening document '{textDocumentPath}' in project '{projectSnapshot.Key}'.");

                await _projectManager
                    .UpdateAsync(
                        static (updater, state) => updater.DocumentOpened(state.key, state.textDocumentPath, state.sourceText),
                        state: (key: projectSnapshot.Key, textDocumentPath, sourceText),
                        cancellationToken)
                    .ConfigureAwait(false);
            },
            cancellationToken).ConfigureAwait(false);

        // Use a separate loop, as the above call modified out projects, so we have to make sure we're operating on the latest snapshot
        await ActOnDocumentInMultipleProjectsAsync(
            filePath,
            (projectSnapshot, textDocumentPath, cancellationToken) =>
            {
                TrackDocumentVersion(projectSnapshot, textDocumentPath, version, startGenerating: true);
                return Task.CompletedTask;
            },
            cancellationToken).ConfigureAwait(false);
    }

    public async Task CloseDocumentAsync(string filePath, CancellationToken cancellationToken)
    {
        using var _ = await _gate.EnterAsync(cancellationToken).ConfigureAwait(false);

        await ActOnDocumentInMultipleProjectsAsync(
            filePath,
            (projectSnapshot, textDocumentPath, cancellationToken) =>
            {
                var textLoader = _remoteTextLoaderFactory.Create(filePath);
                _logger.LogInformation($"Closing document '{textDocumentPath}' in project '{projectSnapshot.Key}'.");

                return _projectManager.UpdateAsync(
                    static (updater, state) => updater.DocumentClosed(state.key, state.textDocumentPath, state.textLoader),
                    state: (key: projectSnapshot.Key, textDocumentPath, textLoader),
                    cancellationToken);
            },
            cancellationToken).ConfigureAwait(false);
    }

    public async Task RemoveDocumentAsync(string filePath, CancellationToken cancellationToken)
    {
        using var _ = await _gate.EnterAsync(cancellationToken).ConfigureAwait(false);

        await ActOnDocumentInMultipleProjectsAsync(filePath, async (projectSnapshot, textDocumentPath, cancellationToken) =>
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
                var miscellaneousProject = await _snapshotResolver.GetMiscellaneousProjectAsync(cancellationToken).ConfigureAwait(false);
                if (projectSnapshot != miscellaneousProject)
                {
                    await MoveDocumentAsync(textDocumentPath, projectSnapshot, miscellaneousProject, cancellationToken).ConfigureAwait(false);
                }
            }
            else
            {
                _logger.LogInformation($"Removing document '{textDocumentPath}' from project '{projectSnapshot.Key}'.");

                await _projectManager
                    .UpdateAsync(
                        static (updater, state) => updater.DocumentRemoved(state.Key, state.HostDocument),
                        state: (projectSnapshot.Key, documentSnapshot.State.HostDocument),
                        cancellationToken)
                    .ConfigureAwait(false);
            }
        },
        cancellationToken).ConfigureAwait(false);
    }

    public async Task UpdateDocumentAsync(string filePath, SourceText sourceText, int version, CancellationToken cancellationToken)
    {
        using var _ = await _gate.EnterAsync(cancellationToken).ConfigureAwait(false);

        await ActOnDocumentInMultipleProjectsAsync(
            filePath,
            (project, textDocumentPath, cancellationToken) =>
            {
                _logger.LogTrace($"Updating document '{textDocumentPath}' in {project.Key}.");

                return _projectManager.UpdateAsync(
                    static (updater, state) => updater.DocumentChanged(state.key, state.textDocumentPath, state.sourceText),
                    state: (key: project.Key, textDocumentPath, sourceText),
                    cancellationToken);
            },
            cancellationToken).ConfigureAwait(false);

        // Use a separate loop, as the above call modified out projects, so we have to make sure we're operating on the latest snapshot
        await ActOnDocumentInMultipleProjectsAsync(
            filePath,
            (projectSnapshot, textDocumentPath, cancellationToken) =>
            {
                TrackDocumentVersion(projectSnapshot, textDocumentPath, version, startGenerating: false);
                return Task.CompletedTask;
            },
            cancellationToken).ConfigureAwait(false);
    }

    private async Task ActOnDocumentInMultipleProjectsAsync(
        string filePath,
        Func<IProjectSnapshot, string, CancellationToken, Task> func,
        CancellationToken cancellationToken)
    {
        var textDocumentPath = FilePathNormalizer.Normalize(filePath);
        var projects = await _snapshotResolver.TryResolveAllProjectsAsync(textDocumentPath, cancellationToken).ConfigureAwait(false);
        if (projects.IsEmpty)
        {
            var miscFilesProject = await _snapshotResolver.GetMiscellaneousProjectAsync(cancellationToken).ConfigureAwait(false);
            projects = [miscFilesProject];
        }

        foreach (var project in projects)
        {
            await func(project, textDocumentPath, cancellationToken).ConfigureAwait(false);
        }
    }

    public async Task<ProjectKey> AddProjectAsync(
        string filePath,
        string intermediateOutputPath,
        RazorConfiguration? configuration,
        string? rootNamespace,
        string? displayName,
        CancellationToken cancellationToken)
    {
        using var _ = await _gate.EnterAsync(cancellationToken).ConfigureAwait(false);

        var normalizedPath = FilePathNormalizer.Normalize(filePath);
        var hostProject = new HostProject(
            normalizedPath, intermediateOutputPath, configuration ?? FallbackRazorConfiguration.Latest, rootNamespace, displayName);

        // ProjectAdded will no-op if the project already exists
        await _projectManager
            .UpdateAsync(
                static (updater, hostProject) => updater.ProjectAdded(hostProject),
                state: hostProject,
                cancellationToken)
            .ConfigureAwait(false);

        _logger.LogInformation($"Added project '{filePath}' with key {hostProject.Key} to project system.");

        await TryMigrateMiscellaneousDocumentsToProjectAsync(cancellationToken).ConfigureAwait(false);

        return hostProject.Key;
    }

    public async Task UpdateProjectAsync(
        ProjectKey projectKey,
        RazorConfiguration? configuration,
        string? rootNamespace,
        string? displayName,
        ProjectWorkspaceState projectWorkspaceState,
        ImmutableArray<DocumentSnapshotHandle> documents,
        CancellationToken cancellationToken)
    {
        using var _ = await _gate.EnterAsync(cancellationToken).ConfigureAwait(false);

        if (!_projectManager.TryGetLoadedProject(projectKey, out var project))
        {
            // Never tracked the project to begin with, noop.
            _logger.LogInformation($"Failed to update untracked project '{projectKey}'.");
            return;
        }

        await UpdateProjectDocumentsAsync(documents, project.Key, cancellationToken).ConfigureAwait(false);

        if (!projectWorkspaceState.Equals(ProjectWorkspaceState.Default))
        {
            _logger.LogInformation($"Updating project '{project.Key}' TagHelpers ({projectWorkspaceState.TagHelpers.Length}) and C# Language Version ({projectWorkspaceState.CSharpLanguageVersion}).");
        }

        await _projectManager
            .UpdateAsync(
                static (updater, state) => updater.ProjectWorkspaceStateChanged(state.key, state.projectWorkspaceState),
                state: (key: project.Key, projectWorkspaceState),
                cancellationToken)
            .ConfigureAwait(false);

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
        await _projectManager
            .UpdateAsync(
                static (updater, hostProject) => updater.ProjectConfigurationChanged(hostProject),
                state: hostProject,
                cancellationToken)
            .ConfigureAwait(false);
    }

    private async Task UpdateProjectDocumentsAsync(ImmutableArray<DocumentSnapshotHandle> documents, ProjectKey projectKey, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"UpdateProjectDocuments for {projectKey} with {documents.Length} documents.");

        var project = _projectManager.GetLoadedProject(projectKey);
        var currentProjectKey = project.Key;
        var projectDirectory = FilePathNormalizer.GetNormalizedDirectoryName(project.FilePath);
        var documentMap = documents.ToDictionary(document => EnsureFullPath(document.FilePath, projectDirectory), FilePathComparer.Instance);
        var miscellaneousProject = await _snapshotResolver.GetMiscellaneousProjectAsync(cancellationToken).ConfigureAwait(false);

        // "Remove" any unnecessary documents by putting them into the misc project
        foreach (var documentFilePath in project.DocumentFilePaths)
        {
            if (documentMap.ContainsKey(documentFilePath))
            {
                // This document still exists in the updated project
                continue;
            }

            _logger.LogDebug($"Document '{documentFilePath}' no longer exists in project '{projectKey}'. Moving to miscellaneous project.");

            await MoveDocumentAsync(documentFilePath, project, miscellaneousProject, cancellationToken).ConfigureAwait(false);
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

            await _projectManager
                .UpdateAsync(
                    static (updater, state) =>
                    {
                        updater.DocumentRemoved(state.currentProjectKey, state.currentHostDocument);
                        updater.DocumentAdded(state.currentProjectKey, state.newHostDocument, state.remoteTextLoader);
                    },
                    state: (currentProjectKey, currentHostDocument, newHostDocument, remoteTextLoader),
                    cancellationToken)
                .ConfigureAwait(false);
        }

        project = _projectManager.GetLoadedProject(project.Key);
        miscellaneousProject = await _snapshotResolver.GetMiscellaneousProjectAsync(cancellationToken).ConfigureAwait(false);

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
                await MoveDocumentAsync(documentFilePath, miscellaneousProject, project, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                var documentHandle = documentKvp.Value;
                var remoteTextLoader = _remoteTextLoaderFactory.Create(documentFilePath);
                var newHostDocument = new HostDocument(documentFilePath, documentHandle.TargetPath, documentHandle.FileKind);

                _logger.LogInformation($"Adding new document '{documentFilePath}' to project '{currentProjectKey}'.");

                await _projectManager
                    .UpdateAsync(
                        static (updater, state) => updater.DocumentAdded(state.currentProjectKey, state.newHostDocument, state.remoteTextLoader),
                        state: (currentProjectKey, newHostDocument, remoteTextLoader),
                        cancellationToken)
                    .ConfigureAwait(false);
            }
        }
    }

    private Task MoveDocumentAsync(string documentFilePath, IProjectSnapshot fromProject, IProjectSnapshot toProject, CancellationToken cancellationToken)
    {
        Debug.Assert(fromProject.DocumentFilePaths.Contains(documentFilePath, FilePathComparer.Instance));
        Debug.Assert(!toProject.DocumentFilePaths.Contains(documentFilePath, FilePathComparer.Instance));

        if (fromProject.GetDocument(documentFilePath) is not DocumentSnapshot documentSnapshot)
        {
            return Task.CompletedTask;
        }

        var currentHostDocument = documentSnapshot.State.HostDocument;

        var textLoader = new DocumentSnapshotTextLoader(documentSnapshot);
        var newHostDocument = new HostDocument(documentSnapshot.FilePath, documentSnapshot.TargetPath, documentSnapshot.FileKind);

        _logger.LogInformation($"Moving '{documentFilePath}' from the '{fromProject.Key}' project to '{toProject.Key}' project.");

        return _projectManager.UpdateAsync(
            static (updater, state) =>
            {
                updater.DocumentRemoved(state.fromProject.Key, state.currentHostDocument);
                updater.DocumentAdded(state.toProject.Key, state.newHostDocument, state.textLoader);
            },
            state: (fromProject, currentHostDocument, toProject, newHostDocument, textLoader),
            cancellationToken);
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

    private async Task TryMigrateMiscellaneousDocumentsToProjectAsync(CancellationToken cancellationToken)
    {
        var miscellaneousProject = await _snapshotResolver.GetMiscellaneousProjectAsync(cancellationToken).ConfigureAwait(false);

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

            await _projectManager
                .UpdateAsync(
                    static (updater, state) => updater.DocumentRemoved(state.Key, state.HostDocument),
                    state: (defaultMiscProject.Key, documentSnapshot.State.HostDocument),
                    cancellationToken)
                .ConfigureAwait(false);

            // Add to new project

            var textLoader = new DocumentSnapshotTextLoader(documentSnapshot);
            var defaultProject = projectSnapshot;
            var newHostDocument = new HostDocument(documentSnapshot.FilePath, documentSnapshot.TargetPath);
            _logger.LogInformation($"Migrating '{documentFilePath}' from the '{miscellaneousProject.Key}' project to '{projectSnapshot.Key}' project.");

            await _projectManager
                .UpdateAsync(
                    static (updater, state) => updater.DocumentAdded(state.key, state.newHostDocument, state.textLoader),
                    state: (key: defaultProject.Key, newHostDocument, textLoader),
                    cancellationToken)
                .ConfigureAwait(false);
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
            _ = documentSnapshot.GetGeneratedOutputAsync();
        }
    }

    private class DelegatingTextLoader : TextLoader
    {
        private readonly IDocumentSnapshot _fromDocument;
        public DelegatingTextLoader(IDocumentSnapshot fromDocument)
        {
            _fromDocument = fromDocument ?? throw new ArgumentNullException(nameof(fromDocument));
        }
        public override async Task<TextAndVersion> LoadTextAndVersionAsync(
           LoadTextOptions options,
           CancellationToken cancellationToken)
        {
            var sourceText = await _fromDocument.GetTextAsync().ConfigureAwait(false);
            var version = await _fromDocument.GetTextVersionAsync().ConfigureAwait(false);
            var textAndVersion = TextAndVersion.Create(sourceText, version.GetNewerVersion());
            return textAndVersion;
        }
    }
}
