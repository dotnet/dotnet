// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces.Serialization;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal class DefaultRazorProjectService : RazorProjectService
{
    private readonly ProjectSnapshotManagerAccessor _projectSnapshotManagerAccessor;
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private readonly RemoteTextLoaderFactory _remoteTextLoaderFactory;
    private readonly ProjectResolver _projectResolver;
    private readonly DocumentVersionCache _documentVersionCache;
    private readonly DocumentResolver _documentResolver;
    private readonly ILogger _logger;

    public DefaultRazorProjectService(
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        RemoteTextLoaderFactory remoteTextLoaderFactory,
        DocumentResolver documentResolver,
        ProjectResolver projectResolver,
        DocumentVersionCache documentVersionCache,
        ProjectSnapshotManagerAccessor projectSnapshotManagerAccessor,
        ILoggerFactory loggerFactory)
    {
        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        if (remoteTextLoaderFactory is null)
        {
            throw new ArgumentNullException(nameof(remoteTextLoaderFactory));
        }

        if (documentResolver is null)
        {
            throw new ArgumentNullException(nameof(documentResolver));
        }

        if (projectResolver is null)
        {
            throw new ArgumentNullException(nameof(projectResolver));
        }

        if (documentVersionCache is null)
        {
            throw new ArgumentNullException(nameof(documentVersionCache));
        }

        if (projectSnapshotManagerAccessor is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerAccessor));
        }

        if (loggerFactory is null)
        {
            throw new ArgumentNullException(nameof(loggerFactory));
        }

        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        _remoteTextLoaderFactory = remoteTextLoaderFactory;
        _documentResolver = documentResolver;
        _projectResolver = projectResolver;
        _documentVersionCache = documentVersionCache;
        _projectSnapshotManagerAccessor = projectSnapshotManagerAccessor;
        _logger = loggerFactory.CreateLogger<DefaultRazorProjectService>();
    }

    public override void AddDocument(string filePath)
    {
        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        var textDocumentPath = FilePathNormalizer.Normalize(filePath);
        if (_documentResolver.TryResolveDocument(textDocumentPath, out var _))
        {
            // Document already added. This usually occurs when VSCode has already pre-initialized
            // open documents and then we try to manually add all known razor documents.
            return;
        }

        if (!_projectResolver.TryResolveProject(textDocumentPath, out var projectSnapshot, enforceDocumentInProject: false))
        {
            projectSnapshot = _projectResolver.GetMiscellaneousProject();
        }

        var targetFilePath = textDocumentPath;
        var projectDirectory = FilePathNormalizer.GetDirectory(projectSnapshot.FilePath);
        if (targetFilePath.StartsWith(projectDirectory, FilePathComparison.Instance))
        {
            // Make relative
            targetFilePath = textDocumentPath[projectDirectory.Length..];
        }

        // Representing all of our host documents with a re-normalized target path to workaround GetRelatedDocument limitations.
        var normalizedTargetFilePath = targetFilePath.Replace('/', '\\').TrimStart('\\');

        var hostDocument = new HostDocument(textDocumentPath, normalizedTargetFilePath);
        var defaultProject = (DefaultProjectSnapshot)projectSnapshot;
        var textLoader = _remoteTextLoaderFactory.Create(textDocumentPath);

        _logger.LogInformation("Adding document '{filePath}' to project '{projectSnapshotFilePath}'.", filePath, projectSnapshot.FilePath);
        _projectSnapshotManagerAccessor.Instance.DocumentAdded(defaultProject.HostProject, hostDocument, textLoader);
    }

    public override void OpenDocument(string filePath, SourceText sourceText, int version)
    {
        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        var textDocumentPath = FilePathNormalizer.Normalize(filePath);
        if (!_documentResolver.TryResolveDocument(textDocumentPath, out _))
        {
            // Document hasn't been added. This usually occurs when VSCode trumps all other initialization
            // processes and pre-initializes already open documents.
            AddDocument(filePath);
        }

        if (!_projectResolver.TryResolveProject(textDocumentPath, out var projectSnapshot))
        {
            projectSnapshot = _projectResolver.GetMiscellaneousProject();
        }

        var defaultProject = (DefaultProjectSnapshot)projectSnapshot;

        _logger.LogInformation("Opening document '{textDocumentPath}' in project '{projectSnapshotFilePath}'.", textDocumentPath, projectSnapshot.FilePath);
        _projectSnapshotManagerAccessor.Instance.DocumentOpened(defaultProject.HostProject.FilePath, textDocumentPath, sourceText);

        TrackDocumentVersion(textDocumentPath, version);

        if (_documentResolver.TryResolveDocument(textDocumentPath, out var documentSnapshot))
        {
            // Start generating the C# for the document so it can immediately be ready for incoming requests.
            _ = documentSnapshot.GetGeneratedOutputAsync();
        }
    }

    public override void CloseDocument(string filePath)
    {
        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        var textDocumentPath = FilePathNormalizer.Normalize(filePath);
        if (!_projectResolver.TryResolveProject(textDocumentPath, out var projectSnapshot))
        {
            projectSnapshot = _projectResolver.GetMiscellaneousProject();
        }

        var textLoader = _remoteTextLoaderFactory.Create(filePath);
        var defaultProject = (DefaultProjectSnapshot)projectSnapshot;
        _logger.LogInformation("Closing document '{textDocumentPath}' in project '{projectSnapshotFilePath}'.", textDocumentPath, projectSnapshot.FilePath);
        _projectSnapshotManagerAccessor.Instance.DocumentClosed(defaultProject.HostProject.FilePath, textDocumentPath, textLoader);
    }

    public override void RemoveDocument(string filePath)
    {
        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        var textDocumentPath = FilePathNormalizer.Normalize(filePath);
        if (!_projectResolver.TryResolveProject(textDocumentPath, out var projectSnapshot))
        {
            projectSnapshot = _projectResolver.GetMiscellaneousProject();
        }

        if (!projectSnapshot.DocumentFilePaths.Contains(textDocumentPath, FilePathComparer.Instance))
        {
            _logger.LogInformation("Containing project is not tracking document '{filePath}", filePath);
            return;
        }

        var document = (DefaultDocumentSnapshot)projectSnapshot.GetDocument(textDocumentPath);
        var defaultProject = (DefaultProjectSnapshot)projectSnapshot;
        _logger.LogInformation("Removing document '{textDocumentPath}' from project '{projectSnapshotFilePath}'.", textDocumentPath, projectSnapshot.FilePath);
        _projectSnapshotManagerAccessor.Instance.DocumentRemoved(defaultProject.HostProject, document.State.HostDocument);
    }

    public override void UpdateDocument(string filePath, SourceText sourceText, int version)
    {
        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        var textDocumentPath = FilePathNormalizer.Normalize(filePath);
        if (!_projectResolver.TryResolveProject(textDocumentPath, out var projectSnapshot))
        {
            projectSnapshot = _projectResolver.GetMiscellaneousProject();
        }

        var defaultProject = (DefaultProjectSnapshot)projectSnapshot;
        _logger.LogTrace("Updating document '{textDocumentPath}'.", textDocumentPath);
        _projectSnapshotManagerAccessor.Instance.DocumentChanged(defaultProject.HostProject.FilePath, textDocumentPath, sourceText);

        TrackDocumentVersion(textDocumentPath, version);
    }

    public override void AddProject(string filePath)
    {
        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        var normalizedPath = FilePathNormalizer.Normalize(filePath);

        var project = _projectSnapshotManagerAccessor.Instance.GetLoadedProject(normalizedPath);

        if (project != null)
        {
            // Project already exists, noop.
            return;
        }

        var hostProject = new HostProject(normalizedPath, RazorDefaults.Configuration, RazorDefaults.RootNamespace);
        _projectSnapshotManagerAccessor.Instance.ProjectAdded(hostProject);
        _logger.LogInformation("Added project '{filePath}' to project system.", filePath);

        TryMigrateMiscellaneousDocumentsToProject();
    }

    public override void RemoveProject(string filePath)
    {
        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        var normalizedPath = FilePathNormalizer.Normalize(filePath);
        var project = (DefaultProjectSnapshot)_projectSnapshotManagerAccessor.Instance.GetLoadedProject(normalizedPath);

        if (project is null)
        {
            // Never tracked the project to begin with, noop.
            return;
        }

        _logger.LogInformation("Removing project '{filePath}' from project system.", filePath);
        _projectSnapshotManagerAccessor.Instance.ProjectRemoved(project.HostProject);

        TryMigrateDocumentsFromRemovedProject(project);
    }

    public override void UpdateProject(
        string filePath,
        RazorConfiguration? configuration,
        string? rootNamespace,
        ProjectWorkspaceState projectWorkspaceState,
        IReadOnlyList<DocumentSnapshotHandle> documents)
    {
        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        var normalizedPath = FilePathNormalizer.Normalize(filePath);
        var project = (DefaultProjectSnapshot)_projectSnapshotManagerAccessor.Instance.GetLoadedProject(normalizedPath);

        if (project is null)
        {
            // Never tracked the project to begin with, noop.
            _logger.LogInformation("Failed to update untracked project '{filePath}'.", filePath);
            return;
        }

        UpdateProjectDocuments(documents, project.FilePath);

        if (!projectWorkspaceState.Equals(ProjectWorkspaceState.Default))
        {
            _logger.LogInformation("Updating project '{filePath}' TagHelpers ({projectWorkspaceState.TagHelpers.Count}) and C# Language Version ({projectWorkspaceState.CSharpLanguageVersion}).",
                filePath, projectWorkspaceState.TagHelpers.Count, projectWorkspaceState.CSharpLanguageVersion);
        }

        _projectSnapshotManagerAccessor.Instance.ProjectWorkspaceStateChanged(project.FilePath, projectWorkspaceState);

        var currentHostProject = project.HostProject;
        var currentConfiguration = currentHostProject.Configuration;
        if (currentConfiguration.ConfigurationName == configuration?.ConfigurationName &&
            currentHostProject.RootNamespace == rootNamespace)
        {
            _logger.LogTrace("Updating project '{filePath}'. The project is already using configuration '{configuration.ConfigurationName}' and root namespace '{rootNamespace}'.",
                filePath, configuration.ConfigurationName, rootNamespace);
            return;
        }

        if (configuration is null)
        {
            configuration = RazorDefaults.Configuration;
            _logger.LogInformation("Updating project '{filePath}' to use Razor's default configuration ('{configuration.ConfigurationName}')'.", filePath, configuration.ConfigurationName);
        }
        else if (currentConfiguration.ConfigurationName != configuration.ConfigurationName)
        {
            _logger.LogInformation("Updating project '{filePath}' to Razor configuration '{configuration.ConfigurationName}' with language version '{configuration.LanguageVersion}'.",
                filePath, configuration.ConfigurationName, configuration.LanguageVersion);
        }

        if (currentHostProject.RootNamespace != rootNamespace)
        {
            _logger.LogInformation("Updating project '{filePath}''s root namespace to '{rootNamespace}'.", filePath, rootNamespace);
        }

        var hostProject = new HostProject(project.FilePath, configuration, rootNamespace);
        _projectSnapshotManagerAccessor.Instance.ProjectConfigurationChanged(hostProject);
    }

    private void UpdateProjectDocuments(IReadOnlyList<DocumentSnapshotHandle> documents, string projectFilePath)
    {
        var project = (DefaultProjectSnapshot)_projectSnapshotManagerAccessor.Instance.GetLoadedProject(projectFilePath);
        var currentHostProject = project.HostProject;
        var projectDirectory = FilePathNormalizer.GetDirectory(project.FilePath);
        var documentMap = documents.ToDictionary(document => EnsureFullPath(document.FilePath, projectDirectory), FilePathComparer.Instance);
        var miscellaneousProject = (DefaultProjectSnapshot)_projectResolver.GetMiscellaneousProject();

        // "Remove" any unnecessary documents by putting them into the misc project
        foreach (var documentFilePath in project.DocumentFilePaths)
        {
            if (documentMap.ContainsKey(documentFilePath))
            {
                // This document still exists in the updated project
                continue;
            }

            MoveDocument(documentFilePath, project, miscellaneousProject);
        }

        project = (DefaultProjectSnapshot)_projectSnapshotManagerAccessor.Instance.GetLoadedProject(projectFilePath);

        // Update existing documents
        foreach (var documentFilePath in project.DocumentFilePaths)
        {
            if (!documentMap.TryGetValue(documentFilePath, out var documentHandle))
            {
                // Document exists in the project but not in the configured documents. Chances are the project configuration is from a fallback
                // configuration case (< 2.1) or the project isn't fully loaded yet.
                continue;
            }

            var documentSnapshot = (DefaultDocumentSnapshot)project.GetDocument(documentFilePath);
            var currentHostDocument = documentSnapshot.State.HostDocument;
            var newFilePath = EnsureFullPath(documentHandle.FilePath, projectDirectory);
            var newHostDocument = new HostDocument(newFilePath, documentHandle.TargetPath, documentHandle.FileKind);

            if (HostDocumentComparer.Instance.Equals(currentHostDocument, newHostDocument))
            {
                // Current and "new" host documents are equivalent
                continue;
            }

            _logger.LogTrace("Updating document '{newHostDocument.FilePath}''s file kind to '{newHostDocument.FileKind}' and target path to '{newHostDocument.TargetPath}'.",
                newHostDocument.FilePath, newHostDocument.FileKind, newHostDocument.TargetPath);

            _projectSnapshotManagerAccessor.Instance.DocumentRemoved(currentHostProject, currentHostDocument);

            var remoteTextLoader = _remoteTextLoaderFactory.Create(newFilePath);
            _projectSnapshotManagerAccessor.Instance.DocumentAdded(currentHostProject, newHostDocument, remoteTextLoader);
        }

        project = (DefaultProjectSnapshot)_projectSnapshotManagerAccessor.Instance.GetLoadedProject(project.FilePath);
        miscellaneousProject = (DefaultProjectSnapshot)_projectResolver.GetMiscellaneousProject();

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
                MoveDocument(documentFilePath, miscellaneousProject, project);
            }
            else
            {
                var documentHandle = documentKvp.Value;
                var remoteTextLoader = _remoteTextLoaderFactory.Create(documentFilePath);
                var newHostDocument = new HostDocument(documentFilePath, documentHandle.TargetPath, documentHandle.FileKind);

                _logger.LogInformation("Adding new document '{documentFilePath}' to project '{projectFilePath}'.", documentFilePath, projectFilePath);
                _projectSnapshotManagerAccessor.Instance.DocumentAdded(currentHostProject, newHostDocument, remoteTextLoader);
            }
        }
    }

    private void MoveDocument(string documentFilePath, DefaultProjectSnapshot fromProject, DefaultProjectSnapshot toProject)
    {
        Debug.Assert(fromProject.DocumentFilePaths.Contains(documentFilePath, FilePathComparer.Instance));
        Debug.Assert(!toProject.DocumentFilePaths.Contains(documentFilePath, FilePathComparer.Instance));

        var documentSnapshot = (DefaultDocumentSnapshot)fromProject.GetDocument(documentFilePath);
        var currentHostDocument = documentSnapshot.State.HostDocument;

        var textLoader = new DocumentSnapshotTextLoader(documentSnapshot);
        var newHostDocument = new HostDocument(documentSnapshot.FilePath, documentSnapshot.TargetPath, documentSnapshot.FileKind);

        _logger.LogInformation("Moving '{documentFilePath}' from the '{fromProject.FilePath}' project to '{toProject.FilePath}' project.",
            documentFilePath, fromProject.FilePath, toProject.FilePath);
        _projectSnapshotManagerAccessor.Instance.DocumentRemoved(fromProject.HostProject, currentHostDocument);
        _projectSnapshotManagerAccessor.Instance.DocumentAdded(toProject.HostProject, newHostDocument, textLoader);
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

    // Internal for testing
    internal void TryMigrateDocumentsFromRemovedProject(ProjectSnapshot project)
    {
        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        var miscellaneousProject = _projectResolver.GetMiscellaneousProject();

        foreach (var documentFilePath in project.DocumentFilePaths)
        {
            var documentSnapshot = (DefaultDocumentSnapshot)project.GetDocument(documentFilePath);

            if (!_projectResolver.TryResolveProject(documentFilePath, out var toProject, enforceDocumentInProject: false))
            {
                // This is the common case. It'd be rare for a project to be nested but we need to protect against it anyhow.
                toProject = miscellaneousProject;
            }

            var textLoader = new DocumentSnapshotTextLoader(documentSnapshot);
            var defaultToProject = (DefaultProjectSnapshot)toProject;
            var newHostDocument = new HostDocument(documentSnapshot.FilePath, documentSnapshot.TargetPath, documentSnapshot.FileKind);

            _logger.LogInformation("Migrating '{documentFilePath}' from the '{project.FilePath}' project to '{toProject.FilePath}' project.",
                documentFilePath, project.FilePath, toProject.FilePath);
            _projectSnapshotManagerAccessor.Instance.DocumentAdded(defaultToProject.HostProject, newHostDocument, textLoader);
        }
    }

    // Internal for testing
    internal void TryMigrateMiscellaneousDocumentsToProject()
    {
        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        var miscellaneousProject = _projectResolver.GetMiscellaneousProject();

        foreach (var documentFilePath in miscellaneousProject.DocumentFilePaths)
        {
            if (!_projectResolver.TryResolveProject(documentFilePath, out var projectSnapshot, enforceDocumentInProject: false))
            {
                continue;
            }

            var documentSnapshot = (DefaultDocumentSnapshot)miscellaneousProject.GetDocument(documentFilePath);

            // Remove from miscellaneous project
            var defaultMiscProject = (DefaultProjectSnapshot)miscellaneousProject;
            _projectSnapshotManagerAccessor.Instance.DocumentRemoved(defaultMiscProject.HostProject, documentSnapshot.State.HostDocument);

            // Add to new project

            var textLoader = new DocumentSnapshotTextLoader(documentSnapshot);
            var defaultProject = (DefaultProjectSnapshot)projectSnapshot;
            var newHostDocument = new HostDocument(documentSnapshot.FilePath, documentSnapshot.TargetPath);
            _logger.LogInformation("Migrating '{documentFilePath}' from the '{miscellaneousProject.FilePath}' project to '{projectSnapshot.FilePath}' project.",
                documentFilePath, miscellaneousProject.FilePath, projectSnapshot.FilePath);
            _projectSnapshotManagerAccessor.Instance.DocumentAdded(defaultProject.HostProject, newHostDocument, textLoader);
        }
    }

    private void TrackDocumentVersion(string textDocumentPath, int version)
    {
        if (!_documentResolver.TryResolveDocument(textDocumentPath, out var documentSnapshot))
        {
            return;
        }

        _documentVersionCache.TrackDocumentVersion(documentSnapshot, version);
    }

    private class DelegatingTextLoader : TextLoader
    {
        private readonly DocumentSnapshot _fromDocument;
        public DelegatingTextLoader(DocumentSnapshot fromDocument)
        {
            _fromDocument = fromDocument ?? throw new ArgumentNullException(nameof(fromDocument));
        }
        public override async Task<TextAndVersion> LoadTextAndVersionAsync(
           LoadTextOptions options,
           CancellationToken cancellationToken)
        {
            var sourceText = await _fromDocument.GetTextAsync();
            var version = await _fromDocument.GetTextVersionAsync();
            var textAndVersion = TextAndVersion.Create(sourceText, version.GetNewerVersion());
            return textAndVersion;
        }
    }
}
