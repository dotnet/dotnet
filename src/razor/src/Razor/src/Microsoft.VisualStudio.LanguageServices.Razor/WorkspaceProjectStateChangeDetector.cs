// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.VisualStudio.Razor;

[Export(typeof(IRazorStartupService))]
internal partial class WorkspaceProjectStateChangeDetector : IRazorStartupService, IDisposable
{
    private static readonly TimeSpan s_delay = TimeSpan.FromSeconds(1);

    private readonly IProjectWorkspaceStateGenerator _generator;
    private readonly IProjectSnapshotManager _projectManager;
    private readonly LanguageServerFeatureOptions _options;
    private readonly CodeAnalysis.Workspace _workspace;

    private readonly CancellationTokenSource _disposeTokenSource;
    private readonly AsyncBatchingWorkQueue<(Project?, IProjectSnapshot)> _workQueue;

    private WorkspaceChangedListener? _workspaceChangedListener;

    [ImportingConstructor]
    public WorkspaceProjectStateChangeDetector(
        IProjectWorkspaceStateGenerator generator,
        IProjectSnapshotManager projectManager,
        LanguageServerFeatureOptions options,
        IWorkspaceProvider workspaceProvider)
        : this(generator, projectManager, options, workspaceProvider, s_delay)
    {
    }

    public WorkspaceProjectStateChangeDetector(
        IProjectWorkspaceStateGenerator generator,
        IProjectSnapshotManager projectManager,
        LanguageServerFeatureOptions options,
        IWorkspaceProvider workspaceProvider,
        TimeSpan delay)
    {
        _generator = generator;
        _projectManager = projectManager;
        _options = options;

        _disposeTokenSource = new();
        _workQueue = new AsyncBatchingWorkQueue<(Project?, IProjectSnapshot)>(
            delay,
            ProcessBatchAsync,
            _disposeTokenSource.Token);

        _projectManager.Changed += ProjectManager_Changed;

        _workspace = workspaceProvider.GetWorkspace();
        _workspace.WorkspaceChanged += Workspace_WorkspaceChanged;

        // This will usually no-op, in the case that another project snapshot change trigger
        // immediately adds projects we want to be able to handle those projects.
        InitializeSolution(_workspace.CurrentSolution);
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

    private ValueTask ProcessBatchAsync(ImmutableArray<(Project? Project, IProjectSnapshot ProjectSnapshot)> items, CancellationToken token)
    {
        foreach (var (project, projectSnapshot) in items.GetMostRecentUniqueItems(Comparer.Instance))
        {
            if (token.IsCancellationRequested)
            {
                return default;
            }

            _generator.EnqueueUpdate(project, projectSnapshot);
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
                    EnqueueUpdateOnProjectAndDependencies(project, newSolution);
                }

                break;

            case WorkspaceChangeKind.ProjectRemoved:
                {
                    var projectId = e.ProjectId.AssumeNotNull();
                    var oldSolution = e.OldSolution;

                    var project = oldSolution.GetRequiredProject(projectId);

                    if (TryGetProjectSnapshot(project, out var projectSnapshot))
                    {
                        EnqueueUpdateOnProjectAndDependencies(projectId, project: null, oldSolution, projectSnapshot);
                    }
                }

                break;

            case WorkspaceChangeKind.DocumentAdded:
                {
                    var projectId = e.ProjectId.AssumeNotNull();
                    var documentId = e.DocumentId.AssumeNotNull();
                    var newSolution = e.NewSolution;

                    // This is the case when a component declaration file changes on disk. We have an MSBuild
                    // generator configured by the SDK that will poke these files on disk when a component
                    // is saved, or loses focus in the editor.
                    var project = newSolution.GetRequiredProject(projectId);

                    if (project.GetDocument(documentId) is not Document newDocument ||
                        newDocument.FilePath is null)
                    {
                        return;
                    }

                    if (IsRazorFileOrRazorVirtual(newDocument))
                    {
                        EnqueueUpdateOnProjectAndDependencies(project, newSolution);
                        return;
                    }

                    // We now know we're not operating directly on a Razor file. However, it's possible the user
                    // is operating on a partial class that is associated with a Razor file.
                    if (IsPartialComponentClass(newDocument))
                    {
                        EnqueueUpdateOnProjectAndDependencies(project, newSolution);
                    }
                }

                break;

            case WorkspaceChangeKind.DocumentRemoved:
                {
                    var projectId = e.ProjectId.AssumeNotNull();
                    var documentId = e.DocumentId.AssumeNotNull();
                    var oldSolution = e.OldSolution;
                    var newSolution = e.NewSolution;

                    var project = oldSolution.GetRequiredProject(projectId);
                    var removedDocument = project.GetRequiredDocument(documentId);

                    if (removedDocument.FilePath is null)
                    {
                        return;
                    }

                    if (IsRazorFileOrRazorVirtual(removedDocument))
                    {
                        EnqueueUpdateOnProjectAndDependencies(project, newSolution);
                        return;
                    }

                    // We now know we're not operating directly on a Razor file. However, it's possible the user
                    // is operating on a partial class that is associated with a Razor file.

                    if (IsPartialComponentClass(removedDocument))
                    {
                        EnqueueUpdateOnProjectAndDependencies(project, newSolution);
                    }
                }

                break;

            case WorkspaceChangeKind.DocumentChanged:
            case WorkspaceChangeKind.DocumentReloaded:
                {
                    var projectId = e.ProjectId.AssumeNotNull();
                    var documentId = e.DocumentId.AssumeNotNull();
                    var oldSolution = e.OldSolution;
                    var newSolution = e.NewSolution;

                    // This is the case when a component declaration file changes on disk. We have an MSBuild
                    // generator configured by the SDK that will poke these files on disk when a component
                    // is saved, or loses focus in the editor.
                    var project = oldSolution.GetRequiredProject(projectId);
                    var document = project.GetRequiredDocument(documentId);

                    if (document.FilePath is null)
                    {
                        return;
                    }

                    if (IsRazorFileOrRazorVirtual(document))
                    {
                        var newProject = newSolution.GetRequiredProject(projectId);
                        EnqueueUpdateOnProjectAndDependencies(newProject, newSolution);
                        return;
                    }

                    // We now know we're not operating directly on a Razor file. However, it's possible the user
                    // is operating on a partial class that is associated with a Razor file.
                    if (IsPartialComponentClass(document))
                    {
                        var newProject = newSolution.GetRequiredProject(projectId);
                        EnqueueUpdateOnProjectAndDependencies(newProject, newSolution);
                    }
                }

                break;

            case WorkspaceChangeKind.SolutionAdded:
            case WorkspaceChangeKind.SolutionChanged:
            case WorkspaceChangeKind.SolutionCleared:
            case WorkspaceChangeKind.SolutionReloaded:
            case WorkspaceChangeKind.SolutionRemoved:
                {
                    var oldSolution = e.OldSolution;
                    var newSolution = e.NewSolution;

                    foreach (var project in oldSolution.Projects)
                    {
                        if (TryGetProjectSnapshot(project, out var projectSnapshot))
                        {
                            EnqueueUpdate(project: null, projectSnapshot);
                        }
                    }

                    InitializeSolution(newSolution);
                }

                break;
        }

        _workspaceChangedListener?.WorkspaceChanged(e.Kind);
    }

    private bool IsRazorFileOrRazorVirtual(Document document)
    {
        if (document.FilePath is not { } filePath)
            return false;

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
    internal static bool IsPartialComponentClass(Document document)
    {
        if (!document.TryGetSyntaxRoot(out var root))
        {
            return false;
        }

        var classDeclarations = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
        if (!classDeclarations.Any())
        {
            return false;
        }

        if (!document.TryGetSemanticModel(out var semanticModel))
        {
            // This will occasionally return false resulting in us not refreshing TagHelpers
            // for component partial classes. This means there are situations when a user's
            // TagHelper definitions will not immediately update but we will eventually achieve omniscience.
            return false;
        }

        var icomponentType = semanticModel.Compilation.GetTypeByMetadataName(ComponentsApi.IComponent.MetadataName);
        if (icomponentType is null)
        {
            // IComponent is not available in the compilation.
            return false;
        }

        foreach (var classDeclaration in classDeclarations)
        {
            if (semanticModel.GetDeclaredSymbol(classDeclaration) is not INamedTypeSymbol classSymbol)
            {
                continue;
            }

            if (ComponentDetectionConventions.IsComponent(classSymbol, icomponentType))
            {
                return true;
            }
        }

        return false;
    }

    private void InitializeSolution(Solution solution)
    {
        foreach (var project in solution.Projects)
        {
            if (TryGetProjectSnapshot(project, out var projectSnapshot))
            {
                EnqueueUpdate(project, projectSnapshot);
            }
        }
    }

    private void ProjectManager_Changed(object? sender, ProjectChangeEventArgs e)
    {
        // Don't do any work if the solution is closing. Any work in the queue will be cancelled on disposal
        if (e.SolutionIsClosing)
        {
            _workQueue.CancelExistingWork();
            return;
        }

        switch (e.Kind)
        {
            case ProjectChangeKind.ProjectAdded:
            case ProjectChangeKind.DocumentRemoved:
            case ProjectChangeKind.DocumentAdded:
                var currentSolution = _workspace.CurrentSolution;
                var associatedWorkspaceProject = currentSolution.Projects
                    .FirstOrDefault(project => e.ProjectKey == project.ToProjectKey());

                if (associatedWorkspaceProject is not null)
                {
                    var projectSnapshot = e.Newer.AssumeNotNull();
                    EnqueueUpdateOnProjectAndDependencies(
                        associatedWorkspaceProject.Id,
                        associatedWorkspaceProject,
                        associatedWorkspaceProject.Solution,
                        projectSnapshot);
                }

                break;

            case ProjectChangeKind.ProjectRemoved:
                // No-op. We don't need to recompute tag helpers if the project is being removed
                break;
        }
    }

    private void EnqueueUpdateOnProjectAndDependencies(Project project, Solution solution)
    {
        if (TryGetProjectSnapshot(project, out var projectSnapshot))
        {
            EnqueueUpdateOnProjectAndDependencies(project.Id, project, solution, projectSnapshot);
        }
    }

    private void EnqueueUpdateOnProjectAndDependencies(ProjectId projectId, Project? project, Solution solution, IProjectSnapshot projectSnapshot)
    {
        EnqueueUpdate(project, projectSnapshot);

        var dependencyGraph = solution.GetProjectDependencyGraph();
        var dependentProjectIds = dependencyGraph.GetProjectsThatTransitivelyDependOnThisProject(projectId);

        foreach (var dependentProjectId in dependentProjectIds)
        {
            if (solution.GetProject(dependentProjectId) is { } dependentProject &&
                TryGetProjectSnapshot(dependentProject, out var dependentProjectSnapshot))
            {
                EnqueueUpdate(dependentProject, dependentProjectSnapshot);
            }
        }
    }

    private void EnqueueUpdate(Project? project, IProjectSnapshot projectSnapshot)
    {
        _workQueue.AddWork((project, projectSnapshot));
    }

    private bool TryGetProjectSnapshot(Project? project, [NotNullWhen(true)] out IProjectSnapshot? projectSnapshot)
    {
        if (project?.CompilationOutputInfo.AssemblyPath is null)
        {
            projectSnapshot = null;
            return false;
        }

        var projectKey = project.ToProjectKey();

        return _projectManager.TryGetProject(projectKey, out projectSnapshot);
    }

    internal TestAccessor GetTestAccessor() => new(this);

    internal sealed class TestAccessor(WorkspaceProjectStateChangeDetector instance)
    {
        public void CancelExistingWork()
        {
            instance._workQueue.CancelExistingWork();
        }

        public async Task WaitUntilCurrentBatchCompletesAsync()
        {
            await instance._workQueue.WaitUntilCurrentBatchCompletesAsync();
        }

        public Task ListenForWorkspaceChangesAsync(params WorkspaceChangeKind[] kinds)
        {
            if (instance._workspaceChangedListener is not null)
            {
                throw new InvalidOperationException($"There's already a {nameof(WorkspaceChangedListener)} installed.");
            }

            var listener = new WorkspaceChangedListener(kinds.ToImmutableArray());
            instance._workspaceChangedListener = listener;

            return listener.Task;
        }

        public void WorkspaceChanged(WorkspaceChangeEventArgs e)
        {
            instance.Workspace_WorkspaceChanged(instance, e);
        }
    }

    private class WorkspaceChangedListener(ImmutableArray<WorkspaceChangeKind> kinds)
    {
        private readonly ImmutableArray<WorkspaceChangeKind> _kinds = kinds;
        private readonly TaskCompletionSource<bool> _completionSource = new();
        private int _index;

        public Task Task => _completionSource.Task;

        public void WorkspaceChanged(WorkspaceChangeKind kind)
        {
            if (_index == _kinds.Length)
            {
                throw new InvalidOperationException($"Expected {_kinds.Length} WorkspaceChanged events but received another {kind}.");
            }

            if (_kinds[_index] != kind)
            {
                throw new InvalidOperationException($"Expected WorkspaceChanged event #{_index + 1} to be {_kinds[_index]} but it was {kind}.");
            }

            _index++;

            if (_index == _kinds.Length)
            {
                _completionSource.TrySetResult(true);
            }
        }
    }
}
