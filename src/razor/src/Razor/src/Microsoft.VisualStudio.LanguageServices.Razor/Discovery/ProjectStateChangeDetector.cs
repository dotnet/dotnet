// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.VisualStudio.Razor.Discovery;

[Export(typeof(IRazorStartupService))]
internal partial class ProjectStateChangeDetector : IRazorStartupService, IDisposable
{
    private readonly record struct Work(ProjectKey Key, ProjectId? Id)
    {
        public bool Equals(Work other)
            => Key.Equals(other.Key);

        public override int GetHashCode()
            => Key.GetHashCode();
    }

    private static readonly TimeSpan s_delay = TimeSpan.FromSeconds(1);

    private readonly IProjectStateUpdater _updater;
    private readonly ProjectSnapshotManager _projectManager;
    private readonly LanguageServerFeatureOptions _options;
    private readonly CodeAnalysis.Workspace _workspace;

    private readonly CancellationTokenSource _disposeTokenSource;
    private readonly AsyncBatchingWorkQueue<Work> _workQueue;
    private readonly HashSet<Work> _workerSet;

    /// <summary>
    ///  A map of assembly path strings to ProjectKeys. This will be cleared when the solution is closed.
    /// </summary>
    private readonly Dictionary<string, ProjectKey> _assemblyPathToProjectKeyMap = new(FilePathComparer.Instance);

    private WorkspaceChangedListener? _workspaceChangedListener;

    [ImportingConstructor]
    public ProjectStateChangeDetector(
        IProjectStateUpdater generator,
        ProjectSnapshotManager projectManager,
        LanguageServerFeatureOptions options,
        IWorkspaceProvider workspaceProvider)
        : this(generator, projectManager, options, workspaceProvider, s_delay)
    {
    }

    public ProjectStateChangeDetector(
        IProjectStateUpdater updater,
        ProjectSnapshotManager projectManager,
        LanguageServerFeatureOptions options,
        IWorkspaceProvider workspaceProvider,
        TimeSpan delay)
    {
        _updater = updater;
        _projectManager = projectManager;
        _options = options;

        _workerSet = [];
        _disposeTokenSource = new();
        _workQueue = new AsyncBatchingWorkQueue<Work>(
            delay,
            ProcessBatchAsync,
            _disposeTokenSource.Token);

        _projectManager.Changed += ProjectManager_Changed;

        _workspace = workspaceProvider.GetWorkspace();
        _workspace.WorkspaceChanged += Workspace_WorkspaceChanged;

        // This will usually no-op, in the case that another project snapshot change trigger
        // immediately adds projects we want to be able to handle those projects.
        UpdateSolutionProjects(_workspace.CurrentSolution);
    }

    public void Dispose()
    {
        if (_disposeTokenSource.IsCancellationRequested)
        {
            return;
        }

        _projectManager.Changed -= ProjectManager_Changed;
        _workspace.WorkspaceChanged -= Workspace_WorkspaceChanged;

        _disposeTokenSource.Cancel();
        _disposeTokenSource.Dispose();
    }

    private ValueTask ProcessBatchAsync(ImmutableArray<Work> items, CancellationToken token)
    {
        _workerSet.Clear();

        foreach (var (projectKey, projectId) in items.GetMostRecentUniqueItems(_workerSet))
        {
            if (token.IsCancellationRequested)
            {
                return default;
            }

            _updater.EnqueueUpdate(projectKey, projectId);
        }

        return default;
    }

    private void Workspace_WorkspaceChanged(object? sender, WorkspaceChangeEventArgs e)
    {
        switch (e.Kind)
        {
            case WorkspaceChangeKind.ProjectAdded:
            case WorkspaceChangeKind.ProjectChanged:
            case WorkspaceChangeKind.ProjectReloaded:
                {
                    var projectId = e.ProjectId.AssumeNotNull();
                    var newSolution = e.NewSolution;

                    var project = newSolution.GetRequiredProject(projectId);

                    UpdateProject(project);
                    UpdateProjectDependents(projectId, newSolution);
                }

                break;

            case WorkspaceChangeKind.ProjectRemoved:
                {
                    var projectId = e.ProjectId.AssumeNotNull();
                    var oldSolution = e.OldSolution;

                    var project = oldSolution.GetRequiredProject(projectId);

                    RemoveProject(project);
                    UpdateProjectDependents(projectId, oldSolution);
                }

                break;

            case WorkspaceChangeKind.DocumentAdded:
                {
                    var projectId = e.ProjectId.AssumeNotNull();
                    var documentId = e.DocumentId.AssumeNotNull();
                    var newSolution = e.NewSolution;

                    var newDocument = newSolution.GetRequiredDocument(documentId);
                    UpdateProjectAndDependentsIfNecessary(newDocument, projectId, newSolution);
                }

                break;

            case WorkspaceChangeKind.DocumentRemoved:
                {
                    var projectId = e.ProjectId.AssumeNotNull();
                    var documentId = e.DocumentId.AssumeNotNull();
                    var oldSolution = e.OldSolution;
                    var newSolution = e.NewSolution;

                    var removedDocument = oldSolution.GetRequiredDocument(documentId);
                    UpdateProjectAndDependentsIfNecessary(removedDocument, projectId, newSolution);
                }

                break;

            case WorkspaceChangeKind.DocumentChanged:
            case WorkspaceChangeKind.DocumentReloaded:
                {
                    var projectId = e.ProjectId.AssumeNotNull();
                    var documentId = e.DocumentId.AssumeNotNull();
                    var newSolution = e.OldSolution;

                    var changedDocument = newSolution.GetRequiredDocument(documentId);
                    UpdateProjectAndDependentsIfNecessary(changedDocument, projectId, newSolution);
                }

                break;

            case WorkspaceChangeKind.SolutionAdded:
            case WorkspaceChangeKind.SolutionChanged:
            case WorkspaceChangeKind.SolutionCleared:
            case WorkspaceChangeKind.SolutionReloaded:
            case WorkspaceChangeKind.SolutionRemoved:
                RemoveSolutionProjects(e.OldSolution);
                UpdateSolutionProjects(e.NewSolution);
                break;
        }

        _workspaceChangedListener?.WorkspaceChanged(e.Kind);
    }

    private void ProjectManager_Changed(object? sender, ProjectChangeEventArgs e)
    {
        if (e.IsSolutionClosing)
        {
            // When the solution is closing...

            // 1. Cancel any remaining work.
            _workQueue.CancelExistingWork();

            // 2. Clear out the assembly path to ProjectKey map.
            lock (_assemblyPathToProjectKeyMap)
            {
                _assemblyPathToProjectKeyMap.Clear();
            }

            // 3. Return rather than adding more work.
            return;
        }

        switch (e.Kind)
        {
            case ProjectChangeKind.ProjectAdded:
            case ProjectChangeKind.DocumentRemoved:
            case ProjectChangeKind.DocumentAdded:
                var solution = _workspace.CurrentSolution;

                if (solution.TryGetProject(e.ProjectKey, out var project))
                {
                    UpdateProject(e.ProjectKey, project);
                    UpdateProjectDependents(project.Id, solution);
                }

                break;
        }
    }

    private bool IsRazorOrRazorVirtualFile(Document document)
    {
        if (document.FilePath is not { } filePath)
        {
            return false;
        }

        // Using EndsWith because Path.GetExtension will ignore everything before .cs
        return filePath.EndsWith(_options.CSharpVirtualDocumentSuffix, FilePathComparison.Instance) ||
               // Still have .cshtml.g.cs and .razor.g.cs for Razor.VSCode scenarios.
               filePath.EndsWith(".cshtml.g.cs", FilePathComparison.Instance) ||
               filePath.EndsWith(".razor.g.cs", FilePathComparison.Instance) ||
               filePath.EndsWith(".razor", FilePathComparison.Instance) ||

               // VSCode's background C# document
               // Using Ordinal because the SDK generates these filenames.
               filePath.EndsWith("__bg__virtual.cs", StringComparison.Ordinal);
    }

    // Internal for testing
    internal static bool ContainsPartialComponentClass(Document document)
    {
        // These will occasionally return false resulting in us not refreshing TagHelpers
        // for component partial classes. This means there are situations when a user's
        // TagHelper definitions will not immediately update but we should eventually catch up.

        if (!document.TryGetSyntaxRoot(out var syntaxRoot) ||
            !document.TryGetSemanticModel(out var semanticModel))
        {
            return false;
        }

        using var classDeclarations = new PooledArrayBuilder<ClassDeclarationSyntax>();

        foreach (var descendentNode in syntaxRoot.DescendantNodes())
        {
            if (descendentNode is ClassDeclarationSyntax classDeclaration)
            {
                classDeclarations.Add(classDeclaration);
            }
        }

        if (classDeclarations.Count == 0)
        {
            return false;
        }

        var componentType = semanticModel.Compilation.GetTypeByMetadataName(ComponentsApi.IComponent.MetadataName);
        if (componentType is null)
        {
            // IComponent is not available in the compilation.
            return false;
        }

        foreach (var classDeclaration in classDeclarations)
        {
            if (semanticModel.GetDeclaredSymbol(classDeclaration) is INamedTypeSymbol classSymbol &&
                ComponentDetectionConventions.IsComponent(classSymbol, componentType))
            {
                return true;
            }
        }

        return false;
    }

    private void RemoveSolutionProjects(Solution solution)
    {
        foreach (var project in solution.Projects)
        {
            if (TryGetProjectKey(project, out var projectKey))
            {
                RemoveProject(projectKey);
            }
        }
    }

    private void UpdateSolutionProjects(Solution solution)
    {
        foreach (var project in solution.Projects)
        {
            UpdateProject(project);
        }
    }

    /// <summary>
    ///  Enqueues updates for the specified project and its dependents if the given <see cref="Document"/>
    ///  represents a file that impacts Razor.
    /// </summary>
    private void UpdateProjectAndDependentsIfNecessary(Document document, ProjectId projectId, Solution solution)
    {
        // A file impacts Razor if it has one of the known file extensions or contains
        // a class declaration that matches a Razor component declaration.
        if (IsRazorOrRazorVirtualFile(document) || ContainsPartialComponentClass(document))
        {
            UpdateProject(document.Project);
            UpdateProjectDependents(projectId, solution);
        }
    }

    private void UpdateProjectDependents(ProjectId projectId, Solution solution)
    {
        var dependencyGraph = solution.GetProjectDependencyGraph();
        var dependentProjectIds = dependencyGraph.GetProjectsThatTransitivelyDependOnThisProject(projectId);

        foreach (var dependentProjectId in dependentProjectIds)
        {
            if (solution.TryGetProject(dependentProjectId, out var dependentProject))
            {
                UpdateProject(dependentProject);
            }
        }
    }

    private void UpdateProject(Project project)
    {
        if (TryGetProjectKey(project, out var projectKey))
        {
            UpdateProject(projectKey, project);
        }
    }

    private void RemoveProject(Project project)
    {
        if (TryGetProjectKey(project, out var projectKey))
        {
            RemoveProject(projectKey);
        }
    }

    private void UpdateProject(ProjectKey projectKey, Project project)
    {
        _workQueue.AddWork(new Work(projectKey, project.Id));
    }

    private void RemoveProject(ProjectKey projectKey)
    {
        _workQueue.AddWork(new Work(projectKey, Id: null));
    }

    private bool TryGetProjectKey(Project project, out ProjectKey projectKey)
    {
        if (project.CompilationOutputInfo.AssemblyPath is not string assemblyPath)
        {
            projectKey = default;
            return false;
        }

        lock (_assemblyPathToProjectKeyMap)
        {
            projectKey = _assemblyPathToProjectKeyMap.GetOrAdd(assemblyPath, _ => project.ToProjectKey());
        }

        return true;
    }
}
