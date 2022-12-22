// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.Editor.Razor;
using MonoDevelop.Ide;
using MonoDevelop.Projects;

namespace Microsoft.VisualStudio.Mac.LanguageServices.Razor;

[Export(typeof(ProjectSnapshotChangeTrigger))]
internal class ProjectBuildChangeTrigger : ProjectSnapshotChangeTrigger
{
    private readonly TextBufferProjectService _projectService;
    private readonly ProjectWorkspaceStateGenerator _workspaceStateGenerator;
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private ProjectSnapshotManagerBase? _projectManager;

    [ImportingConstructor]
    public ProjectBuildChangeTrigger(
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        TextBufferProjectService projectService,
        ProjectWorkspaceStateGenerator workspaceStateGenerator)
    {
        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        if (projectService is null)
        {
            throw new ArgumentNullException(nameof(projectService));
        }

        if (workspaceStateGenerator is null)
        {
            throw new ArgumentNullException(nameof(workspaceStateGenerator));
        }

        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        _projectService = projectService;
        _workspaceStateGenerator = workspaceStateGenerator;
    }

    // Internal for testing
    internal ProjectBuildChangeTrigger(
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        TextBufferProjectService projectService,
        ProjectWorkspaceStateGenerator workspaceStateGenerator,
        ProjectSnapshotManagerBase projectManager)
    {
        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        if (projectService is null)
        {
            throw new ArgumentNullException(nameof(projectService));
        }

        if (workspaceStateGenerator is null)
        {
            throw new ArgumentNullException(nameof(workspaceStateGenerator));
        }

        if (projectManager is null)
        {
            throw new ArgumentNullException(nameof(projectManager));
        }

        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        _projectService = projectService;
        _projectManager = projectManager;
        _workspaceStateGenerator = workspaceStateGenerator;
    }

    public override void Initialize(ProjectSnapshotManagerBase projectManager)
    {
        if (projectManager is null)
        {
            throw new ArgumentNullException(nameof(projectManager));
        }

        _projectManager = projectManager;

        if (IdeApp.ProjectOperations is not null)
        {
            IdeApp.ProjectOperations.EndBuild += ProjectOperations_EndBuild;
        }
    }

    private void ProjectOperations_EndBuild(object sender, BuildEventArgs args)
    {
        if (args is null)
        {
            throw new ArgumentNullException(nameof(args));
        }

        _ = HandleEndBuildAsync(args);
    }

    // Internal for testing
    internal Task HandleEndBuildAsync(BuildEventArgs args)
    {
        if (!args.Success)
        {
            // Build failed
            return Task.CompletedTask;
        }

        return _projectSnapshotManagerDispatcher.RunOnDispatcherThreadAsync((projectItem, ct) =>
               {
                   if (!_projectService.IsSupportedProject(projectItem))
                   {
                       // We're hooked into all build events, it's possible to get called with an unsupported project item type.
                       return;
                   }

                   var projectPath = _projectService.GetProjectPath(projectItem);
                   var projectSnapshot = _projectManager?.GetLoadedProject(projectPath);
                   if (projectSnapshot is not null)
                   {
                       var workspaceProject = _projectManager?.Workspace.CurrentSolution?.Projects.FirstOrDefault(
                           project => FilePathComparer.Instance.Equals(project.FilePath, projectSnapshot.FilePath));
                       if (workspaceProject is not null)
                       {
                           // Trigger a tag helper update by forcing the project manager to see the workspace Project
                           // from the current solution.
                           _workspaceStateGenerator.Update(workspaceProject, projectSnapshot, CancellationToken.None);
                       }
                   }
               }, args.SolutionItem, CancellationToken.None);
    }
}
