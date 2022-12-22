// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.OmniSharpPlugin;
using Microsoft.Build.Execution;
using Microsoft.Extensions.Logging;
using OmniSharp.MSBuild.Notification;

namespace Microsoft.AspNetCore.Razor.OmnisharpPlugin;

[Shared]
[Export(typeof(IMSBuildEventSink))]
[Export(typeof(IRazorDocumentChangeListener))]
[Export(typeof(IOmniSharpProjectSnapshotManagerChangeTrigger))]
internal class MSBuildProjectManager : IMSBuildEventSink, IOmniSharpProjectSnapshotManagerChangeTrigger, IRazorDocumentChangeListener
{
    // Internal for testing
    internal const string IntermediateOutputPathPropertyName = "IntermediateOutputPath";
    internal const string MSBuildProjectDirectoryPropertyName = "MSBuildProjectDirectory";
    internal const string ProjectCapabilityItemType = "ProjectCapability";

    private const string MSBuildProjectFullPathPropertyName = "MSBuildProjectFullPath";
    private const string DebugRazorOmnisharpPluginPropertyName = "_DebugRazorOmnisharpPlugin_";
    private readonly ILogger _logger;
    private readonly IEnumerable<ProjectConfigurationProvider> _projectConfigurationProviders;
    private readonly ProjectInstanceEvaluator _projectInstanceEvaluator;
    private readonly ProjectChangePublisher _projectConfigurationPublisher;
    private readonly OmniSharpProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private OmniSharpProjectSnapshotManagerBase? _projectManager;

    [ImportingConstructor]
    public MSBuildProjectManager(
        [ImportMany] IEnumerable<ProjectConfigurationProvider> projectConfigurationProviders,
        ProjectInstanceEvaluator projectInstanceEvaluator,
        ProjectChangePublisher projectConfigurationPublisher,
        OmniSharpProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        ILoggerFactory loggerFactory)
    {
        if (projectConfigurationProviders is null)
        {
            throw new ArgumentNullException(nameof(projectConfigurationProviders));
        }

        if (projectInstanceEvaluator is null)
        {
            throw new ArgumentNullException(nameof(projectInstanceEvaluator));
        }

        if (projectConfigurationPublisher is null)
        {
            throw new ArgumentNullException(nameof(projectConfigurationPublisher));
        }

        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        if (loggerFactory is null)
        {
            throw new ArgumentNullException(nameof(loggerFactory));
        }

        _logger = loggerFactory.CreateLogger<MSBuildProjectManager>();
        _projectConfigurationProviders = projectConfigurationProviders;
        _projectInstanceEvaluator = projectInstanceEvaluator;
        _projectConfigurationPublisher = projectConfigurationPublisher;
        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
    }

    public OmniSharpProjectSnapshotManagerBase ProjectManager => _projectManager ?? throw new InvalidOperationException($"{nameof(ProjectManager)} was unexpectedly 'null'. Has {nameof(Initialize)} been called?");

    public void Initialize(OmniSharpProjectSnapshotManagerBase projectManager)
    {
        if (projectManager is null)
        {
            throw new ArgumentNullException(nameof(projectManager));
        }

        _projectManager = projectManager;
    }

    public void ProjectLoaded(ProjectLoadedEventArgs args)
    {
        _ = ProjectLoadedAsync(args, CancellationToken.None);
    }

    public void RazorDocumentChanged(RazorFileChangeEventArgs args)
    {
        if (args.Kind == RazorFileChangeKind.Added ||
            args.Kind == RazorFileChangeKind.Removed)
        {
            // When documents get added or removed we need to refresh project state to properly reflect the host documents in the project.

            var evaluatedProjectInstance = _projectInstanceEvaluator.Evaluate(args.UnevaluatedProjectInstance);
            _ = _projectSnapshotManagerDispatcher.RunOnDispatcherThreadAsync(
                () => UpdateProjectState(evaluatedProjectInstance), CancellationToken.None).ConfigureAwait(false);
        }
    }

    // Internal for testing
    internal async Task ProjectLoadedAsync(ProjectLoadedEventArgs args, CancellationToken cancellationToken)
    {
        try
        {
            var projectInstance = args.ProjectInstance;
            HandleDebug(projectInstance);

            if (!TryResolveConfigurationOutputPath(projectInstance, out var configPath))
            {
                return;
            }

            var projectFilePath = projectInstance.GetPropertyValue(MSBuildProjectFullPathPropertyName);
            if (string.IsNullOrEmpty(projectFilePath))
            {
                // This should never be true but we're being extra careful.
                return;
            }

            _projectConfigurationPublisher.SetPublishFilePath(projectFilePath, configPath);

            // Force project instance evaluation to ensure that all Razor specific targets have run.
            projectInstance = _projectInstanceEvaluator.Evaluate(projectInstance);

            await _projectSnapshotManagerDispatcher.RunOnDispatcherThreadAsync(
                () => UpdateProjectState(projectInstance), cancellationToken).ConfigureAwait(false);

        }
        catch (Exception ex)
        {
            _logger.LogError("Unexpected exception got thrown from the Razor plugin: {exception}", ex);
        }
    }

    private void UpdateProjectState(ProjectInstance projectInstance)
    {
        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        var projectFilePath = projectInstance.GetPropertyValue(MSBuildProjectFullPathPropertyName);
        if (string.IsNullOrEmpty(projectFilePath))
        {
            // This should never be true but we're being extra careful.
            return;
        }

        var projectConfiguration = GetProjectConfiguration(projectInstance, _projectConfigurationProviders);
        if (projectConfiguration is null)
        {
            // Not a Razor project
            return;
        }

        var projectSnapshot = ProjectManager.GetLoadedProject(projectFilePath);
        var hostProject = new OmniSharpHostProject(projectFilePath, projectConfiguration.Configuration, projectConfiguration.RootNamespace);
        if (projectSnapshot is null)
        {
            // Project doesn't exist yet, create it and set it up with all of its host documents.

            ProjectManager.ProjectAdded(hostProject);

            foreach (var hostDocument in projectConfiguration.Documents)
            {
                ProjectManager.DocumentAdded(hostProject, hostDocument);
            }
        }
        else
        {
            // Project already exists (project change). Reconfigure the project and add or remove host documents to synchronize it with the configured host documents.

            ProjectManager.ProjectConfigurationChanged(hostProject);

            SynchronizeDocuments(projectConfiguration.Documents, projectSnapshot, hostProject);
        }
    }

    // Internal for testing
    internal void SynchronizeDocuments(
        IReadOnlyList<OmniSharpHostDocument> configuredHostDocuments,
        OmniSharpProjectSnapshot projectSnapshot,
        OmniSharpHostProject hostProject)
    {
        // Remove any documents that need to be removed
        foreach (var documentFilePath in projectSnapshot.DocumentFilePaths)
        {
            OmniSharpHostDocument? associatedHostDocument = null;
            var currentHostDocument = projectSnapshot.GetDocument(documentFilePath).HostDocument;

            for (var i = 0; i < configuredHostDocuments.Count; i++)
            {
                var configuredHostDocument = configuredHostDocuments[i];
                if (OmniSharpHostDocumentComparer.Instance.Equals(configuredHostDocument, currentHostDocument))
                {
                    associatedHostDocument = configuredHostDocument;
                    break;
                }
            }

            if (associatedHostDocument is null)
            {
                // Document was removed
                ProjectManager.DocumentRemoved(hostProject, currentHostDocument);
            }
        }

        // Refresh the project snapshot to reflect any removed documents.
        projectSnapshot = ProjectManager.GetLoadedProject(projectSnapshot.FilePath);

        // Add any documents that need to be added
        for (var i = 0; i < configuredHostDocuments.Count; i++)
        {
            var hostDocument = configuredHostDocuments[i];
            if (!projectSnapshot.DocumentFilePaths.Contains(hostDocument.FilePath, FilePathComparer.Instance))
            {
                // Document was added.
                ProjectManager.DocumentAdded(hostProject, hostDocument);
            }
        }
    }

    // Internal for testing
    internal static ProjectConfiguration? GetProjectConfiguration(
        ProjectInstance projectInstance,
        IEnumerable<ProjectConfigurationProvider> projectConfigurationProviders)
    {
        if (projectInstance is null)
        {
            throw new ArgumentNullException(nameof(projectInstance));
        }

        if (projectConfigurationProviders is null)
        {
            throw new ArgumentNullException(nameof(projectConfigurationProviders));
        }

        var projectCapabilities = projectInstance
            .GetItems(ProjectCapabilityItemType)
            .Select(capability => capability.EvaluatedInclude)
            .ToList();

        var context = new ProjectConfigurationProviderContext(projectCapabilities, projectInstance);
        foreach (var projectConfigurationProvider in projectConfigurationProviders)
        {
            if (projectConfigurationProvider.TryResolveConfiguration(context, out var configuration))
            {
                return configuration;
            }
        }

        if (FallbackConfigurationProvider.Instance.TryResolveConfiguration(context, out var fallbackConfiguration))
        {
            return fallbackConfiguration;
        }

        return null;
    }

    private static void HandleDebug(ProjectInstance projectInstance)
    {
        var debugPlugin = projectInstance.GetPropertyValue(DebugRazorOmnisharpPluginPropertyName);
        if (!string.IsNullOrEmpty(debugPlugin) && string.Equals(debugPlugin, "true", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine($"Waiting for a debugger to attach to the Razor Plugin. Process id: {Process.GetCurrentProcess().Id}");
            while (!Debugger.IsAttached)
            {
                Thread.Sleep(1000);
            }

            Debugger.Break();
        }
    }

    // Internal for testing
    internal static bool TryResolveConfigurationOutputPath(ProjectInstance projectInstance, out string? path)
    {
        var intermediateOutputPath = projectInstance.GetPropertyValue(IntermediateOutputPathPropertyName);
        if (string.IsNullOrEmpty(intermediateOutputPath))
        {
            path = null;
            return false;
        }

        if (!Path.IsPathRooted(intermediateOutputPath))
        {
            // Relative path, need to convert to absolute.
            var projectDirectory = projectInstance.GetPropertyValue(MSBuildProjectDirectoryPropertyName);
            if (string.IsNullOrEmpty(projectDirectory))
            {
                // This should never be true but we're beign extra careful.
                path = null;
                return false;
            }

            intermediateOutputPath = Path.Combine(projectDirectory, intermediateOutputPath);
        }

        intermediateOutputPath = intermediateOutputPath
            .Replace('\\', Path.DirectorySeparatorChar)
            .Replace('/', Path.DirectorySeparatorChar);
        path = Path.Combine(intermediateOutputPath, LanguageServerConstants.DefaultProjectConfigurationFile);
        return true;
    }
}
