// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.Razor.ProjectSystem;

/// <summary>
/// This class is responsible for maintaining project information for projects that don't
/// use the Razor or Web SDK, or otherwise don't get picked up by our CPS bits, but have
/// .razor or .cshtml files regardless.
/// </summary>
[Export(typeof(FallbackProjectManager))]
[Export(typeof(IFallbackProjectManager))]
internal sealed class FallbackProjectManager : IFallbackProjectManager
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ProjectSnapshotManager _projectManager;
    private readonly IWorkspaceProvider _workspaceProvider;
    private readonly ITelemetryReporter _telemetryReporter;

    // Tracks project keys that are known to be fallback projects.
    private ImmutableHashSet<ProjectKey> _fallbackProjects = [];

    [ImportingConstructor]
    public FallbackProjectManager(
        [Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider,
        ProjectSnapshotManager projectManager,
        IWorkspaceProvider workspaceProvider,
        ITelemetryReporter telemetryReporter)
    {
        _serviceProvider = serviceProvider;
        _projectManager = projectManager;
        _workspaceProvider = workspaceProvider;
        _telemetryReporter = telemetryReporter;

        // Use PriorityChanged to ensure that project changes or removes update _fallbackProjects
        // before IProjectSnapshotManager.Changed listeners are notified.
        _projectManager.PriorityChanged += ProjectManager_Changed;
    }

    private void ProjectManager_Changed(object sender, ProjectChangeEventArgs e)
    {
        // If a project is changed, we know that this is no longer a fallback project because
        // one of two things has happened:
        //
        // 1. The project system has updated the project's configuration or root namespace.
        // 2. The project's ProjectWorkspaceState has been updated.
        //
        // In either of these two cases, we assume that something else (Roslyn or CPS) is properly
        // tracking the project and we no longer need to treat it as a fallback project.
        //
        // In addition, if a project is removed, we can stop tracking it as a fallback project.
        if (e.Kind is ProjectChangeKind.ProjectChanged or ProjectChangeKind.ProjectRemoved)
        {
            ImmutableInterlocked.Update(ref _fallbackProjects, set => set.Remove(e.ProjectKey));
        }
    }

    public bool IsFallbackProject(ProjectKey projectKey)
        => _fallbackProjects.Contains(projectKey);

    internal void DynamicFileAdded(
        ProjectId projectId,
        ProjectKey razorProjectKey,
        string projectFilePath,
        string filePath,
        CancellationToken cancellationToken)
    {
        try
        {
            if (_projectManager.TryGetProject(razorProjectKey, out var project))
            {
                if (IsFallbackProject(razorProjectKey))
                {
                    // If this is a fallback project, then Roslyn may not track documents in the project, so these dynamic file notifications
                    // are the only way to know about files in the project.
                    AddFallbackDocument(razorProjectKey, filePath, projectFilePath, cancellationToken);
                }
            }
            else
            {
                // We have been asked to provide dynamic file info, which means there is a .razor or .cshtml file in the project
                // but for some reason our project system doesn't know about the project. In these cases (often when people don't
                // use the Razor or Web SDK) we spin up a fallback experience for them
                AddFallbackProject(projectId, filePath, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _telemetryReporter.ReportFault(ex, "Error while trying to add fallback document to project");
        }
    }

    internal void DynamicFileRemoved(
        ProjectId projectId,
        ProjectKey razorProjectKey,
        string projectFilePath,
        string filePath,
        CancellationToken cancellationToken)
    {
        try
        {
            if (IsFallbackProject(razorProjectKey) &&
                _projectManager.TryGetProject(razorProjectKey, out var project))
            {
                // If this is a fallback project, then Roslyn may not track documents in the project, so these dynamic file notifications
                // are the only way to know about files in the project.
                RemoveFallbackDocument(projectId, filePath, projectFilePath, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _telemetryReporter.ReportFault(ex, "Error while trying to remove fallback document from project");
        }
    }

    private void AddFallbackProject(ProjectId projectId, string filePath, CancellationToken cancellationToken)
    {
        var project = TryFindProjectForProjectId(projectId);
        if (project?.FilePath is null)
        {
            return;
        }

        var intermediateOutputPath = Path.GetDirectoryName(project.CompilationOutputInfo.AssemblyPath);
        if (intermediateOutputPath is null)
        {
            return;
        }

        var rootNamespace = project.DefaultNamespace;

        var configuration = FallbackRazorConfiguration.Latest;

        // We create this as a fallback project so that other parts of the system can reason about them - eg we don't do code
        // generation for closed files for documents in these projects. If these projects become "real", either because capabilities
        // change or simply a timing difference between Roslyn and our CPS components, we'll receive a priority notification from
        // the IProjectSnapshotManager and remove this project's key from the_fallbackProjects set.
        var hostProject = new HostProject(project.FilePath, intermediateOutputPath, configuration, rootNamespace, project.Name);

        ImmutableInterlocked.Update(ref _fallbackProjects, set => set.Add(hostProject.Key));

        EnqueueProjectManagerUpdate(
            updater => updater.AddProject(hostProject),
            cancellationToken);

        AddFallbackDocument(hostProject.Key, filePath, project.FilePath, cancellationToken);
    }

    private void AddFallbackDocument(ProjectKey projectKey, string filePath, string projectFilePath, CancellationToken cancellationToken)
    {
        var hostDocument = CreateHostDocument(filePath, projectFilePath);
        if (hostDocument is null)
        {
            return;
        }

        var textLoader = new FileTextLoader(filePath, defaultEncoding: null);

        EnqueueProjectManagerUpdate(
            updater => updater.AddDocument(projectKey, hostDocument, textLoader),
            cancellationToken);
    }

    private static HostDocument? CreateHostDocument(string filePath, string projectFilePath)
    {
        // The compiler only supports paths that are relative to the project root, so filter our files
        // that don't match
        var projectPath = FilePathNormalizer.GetNormalizedDirectoryName(projectFilePath);
        var normalizedFilePath = FilePathNormalizer.Normalize(filePath);
        if (!normalizedFilePath.StartsWith(projectPath, FilePathComparison.Instance))
        {
            return null;
        }

        var targetPath = filePath[projectPath.Length..];
        var hostDocument = new HostDocument(filePath, targetPath);
        return hostDocument;
    }

    private void RemoveFallbackDocument(ProjectId projectId, string filePath, string projectFilePath, CancellationToken cancellationToken)
    {
        var project = TryFindProjectForProjectId(projectId);
        if (project is null)
        {
            return;
        }

        var projectKey = project.ToProjectKey();

        var hostDocument = CreateHostDocument(filePath, projectFilePath);
        if (hostDocument is null)
        {
            return;
        }

        EnqueueProjectManagerUpdate(
            updater => updater.RemoveDocument(projectKey, hostDocument.FilePath),
            cancellationToken);
    }

    private void EnqueueProjectManagerUpdate(Action<ProjectSnapshotManager.Updater> action, CancellationToken cancellationToken)
    {
        _projectManager
            .UpdateAsync(
                static (updater, state) =>
                {
                    var (serviceProvider, action) = state;
                    RazorStartupInitializer.Initialize(serviceProvider);

                    action(updater);
                },
                state: (_serviceProvider, action),
                cancellationToken)
            .Forget();
    }

    private Project? TryFindProjectForProjectId(ProjectId projectId)
    {
        var workspace = _workspaceProvider.GetWorkspace();

        var project = workspace.CurrentSolution.GetProject(projectId);
        if (project is null ||
            project.Language != LanguageNames.CSharp)
        {
            return null;
        }

        return project;
    }
}
