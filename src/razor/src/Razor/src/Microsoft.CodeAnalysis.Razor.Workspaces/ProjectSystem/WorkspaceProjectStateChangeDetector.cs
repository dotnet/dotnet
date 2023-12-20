// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

[Shared]
[Export(typeof(IProjectSnapshotChangeTrigger))]
internal class WorkspaceProjectStateChangeDetector : IProjectSnapshotChangeTrigger, IDisposable
{
    private static readonly TimeSpan s_batchingDelay = TimeSpan.FromSeconds(1);
    private readonly object _disposedLock = new();
    private readonly object _workQueueAccessLock = new();
    private readonly ProjectWorkspaceStateGenerator _workspaceStateGenerator;
    private readonly ProjectSnapshotManagerDispatcher _dispatcher;
    private readonly LanguageServerFeatureOptions _options;
    private BatchingWorkQueue? _workQueue;
    private ProjectSnapshotManagerBase? _projectManager;
    private bool _disposed;

    private ProjectSnapshotManagerBase ProjectSnapshotManager
        => _projectManager ?? throw new InvalidOperationException($"ProjectManager was accessed before Initialize was called");

    [ImportingConstructor]
    public WorkspaceProjectStateChangeDetector(
        ProjectWorkspaceStateGenerator workspaceStateGenerator,
        ProjectSnapshotManagerDispatcher dispatcher,
        LanguageServerFeatureOptions options)
    {
        _workspaceStateGenerator = workspaceStateGenerator ?? throw new ArgumentNullException(nameof(workspaceStateGenerator));
        _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    // Internal for testing
    internal WorkspaceProjectStateChangeDetector(
        ProjectWorkspaceStateGenerator workspaceStateGenerator,
        ProjectSnapshotManagerDispatcher dispatcher,
        LanguageServerFeatureOptions options,
        BatchingWorkQueue workQueue)
    {
        _workspaceStateGenerator = workspaceStateGenerator;
        _dispatcher = dispatcher;
        _options = options;
        _workQueue = workQueue;
    }

    public ManualResetEventSlim? NotifyWorkspaceChangedEventComplete { get; set; }

    public void Initialize(ProjectSnapshotManagerBase projectManager)
    {
        _projectManager = projectManager;

        EnsureWorkQueue();

        projectManager.Changed += ProjectManager_Changed;
        projectManager.Workspace.WorkspaceChanged += Workspace_WorkspaceChanged;

        // This will usually no-op, in the case that another project snapshot change trigger
        // immediately adds projects we want to be able to handle those projects.
        InitializeSolution(projectManager.Workspace.CurrentSolution);
    }

    private void EnsureWorkQueue()
    {
        lock (_disposedLock)
        {
            lock (_workQueueAccessLock)
            {
                if (_projectManager is not { } projectManager || _disposed)
                {
                    return;
                }

                _workQueue ??= new BatchingWorkQueue(
                    s_batchingDelay,
                    FilePathComparer.Instance,
                    projectManager.ErrorReporter);
            }
        }
    }

    // Internal for testing, virtual for temporary VSCode workaround
    internal virtual void Workspace_WorkspaceChanged(object? sender, WorkspaceChangeEventArgs e)
    {
        _ = Workspace_WorkspaceChangedAsync(e, CancellationToken.None);
    }

    private async Task Workspace_WorkspaceChangedAsync(WorkspaceChangeEventArgs e, CancellationToken cancellationToken)
    {
        try
        {
            // Method needs to be run on the project snapshot manager's specialized thread
            // due to project snapshot manager access.
            switch (e.Kind)
            {
                case WorkspaceChangeKind.ProjectAdded:
                    await _dispatcher
                        .RunOnDispatcherThreadAsync(
                            static (arg, _) =>
                            {
                                var (@this, eventArgs) = arg;
                                var projectId = eventArgs.ProjectId.AssumeNotNull();
                                var newSolution = eventArgs.NewSolution;

                                var project = newSolution.GetRequiredProject(projectId);
                                @this.EnqueueUpdateOnProjectAndDependencies(project, newSolution);
                            },
                            (self: this, eventArgs: e),
                            cancellationToken)
                        .ConfigureAwait(false);
                    break;

                case WorkspaceChangeKind.ProjectChanged:
                case WorkspaceChangeKind.ProjectReloaded:
                    await _dispatcher
                        .RunOnDispatcherThreadAsync(
                            static (arg, _) =>
                            {
                                var (@this, eventArgs) = arg;
                                var projectId = eventArgs.ProjectId.AssumeNotNull();
                                var newSolution = eventArgs.NewSolution;

                                var project = newSolution.GetRequiredProject(projectId);
                                @this.EnqueueUpdateOnProjectAndDependencies(project, newSolution);
                            },
                            (self: this, eventArgs: e),
                            cancellationToken)
                        .ConfigureAwait(false);
                    break;

                case WorkspaceChangeKind.ProjectRemoved:
                    await _dispatcher
                        .RunOnDispatcherThreadAsync(
                            static (arg, _) =>
                            {
                                var (@this, eventArgs) = arg;
                                var projectId = eventArgs.ProjectId.AssumeNotNull();
                                var oldSolution = eventArgs.OldSolution;

                                var project = oldSolution.GetRequiredProject(projectId);
                                if (@this.TryGetProjectSnapshot(project, out var projectSnapshot))
                                {
                                    @this.EnqueueUpdateOnProjectAndDependencies(projectId, project: null, oldSolution, projectSnapshot);
                                }
                            },
                            (self: this, eventArgs: e),
                            cancellationToken)
                        .ConfigureAwait(false);
                    break;

                case WorkspaceChangeKind.DocumentAdded:
                    await _dispatcher
                        .RunOnDispatcherThreadAsync(
                            static (arg, _) =>
                            {
                                var (@this, eventArgs) = arg;
                                var projectId = eventArgs.ProjectId.AssumeNotNull();
                                var documentId = eventArgs.DocumentId.AssumeNotNull();
                                var newSolution = eventArgs.NewSolution;

                                // This is the case when a component declaration file changes on disk. We have an MSBuild
                                // generator configured by the SDK that will poke these files on disk when a component
                                // is saved, or loses focus in the editor.
                                var project = newSolution.GetRequiredProject(projectId);
                                var newDocument = project.GetDocument(documentId);

                                if (newDocument?.FilePath is null)
                                {
                                    return;
                                }

                                if (@this.IsRazorFileOrRazorVirtual(newDocument))
                                {
                                    @this.EnqueueUpdateOnProjectAndDependencies(project, newSolution);
                                    return;
                                }

                                // We now know we're not operating directly on a Razor file. However, it's possible the user
                                // is operating on a partial class that is associated with a Razor file.
                                if (IsPartialComponentClass(newDocument))
                                {
                                    @this.EnqueueUpdateOnProjectAndDependencies(project, newSolution);
                                }
                            },
                            (self: this, eventArgs: e),
                            cancellationToken)
                        .ConfigureAwait(false);
                    break;

                case WorkspaceChangeKind.DocumentRemoved:
                    await _dispatcher
                        .RunOnDispatcherThreadAsync(
                            static (arg, _) =>
                            {
                                var (@this, eventArgs) = arg;
                                var projectId = eventArgs.ProjectId.AssumeNotNull();
                                var documentId = eventArgs.DocumentId.AssumeNotNull();
                                var oldSolution = eventArgs.OldSolution;
                                var newSolution = eventArgs.NewSolution;

                                var project = oldSolution.GetRequiredProject(projectId);
                                var removedDocument = project.GetRequiredDocument(documentId);

                                if (removedDocument.FilePath is null)
                                {
                                    return;
                                }

                                if (@this.IsRazorFileOrRazorVirtual(removedDocument))
                                {
                                    @this.EnqueueUpdateOnProjectAndDependencies(project, newSolution);
                                    return;
                                }

                                // We now know we're not operating directly on a Razor file. However, it's possible the user
                                // is operating on a partial class that is associated with a Razor file.

                                if (IsPartialComponentClass(removedDocument))
                                {
                                    @this.EnqueueUpdateOnProjectAndDependencies(project, newSolution);
                                }
                            },
                            (self: this, eventArgs: e),
                            cancellationToken)
                        .ConfigureAwait(false);
                    break;

                case WorkspaceChangeKind.DocumentChanged:
                case WorkspaceChangeKind.DocumentReloaded:
                    await _dispatcher
                        .RunOnDispatcherThreadAsync(
                            static (arg, _) =>
                            {
                                var (@this, eventArgs) = arg;
                                var projectId = eventArgs.ProjectId.AssumeNotNull();
                                var documentId = eventArgs.DocumentId.AssumeNotNull();
                                var oldSolution = eventArgs.OldSolution;
                                var newSolution = eventArgs.NewSolution;

                                // This is the case when a component declaration file changes on disk. We have an MSBuild
                                // generator configured by the SDK that will poke these files on disk when a component
                                // is saved, or loses focus in the editor.
                                var project = oldSolution.GetRequiredProject(projectId);
                                var document = project.GetRequiredDocument(documentId);

                                if (document.FilePath is null)
                                {
                                    return;
                                }

                                if (@this.IsRazorFileOrRazorVirtual(document))
                                {
                                    var newProject = newSolution.GetRequiredProject(projectId);
                                    @this.EnqueueUpdateOnProjectAndDependencies(newProject, newSolution);
                                    return;
                                }

                                // We now know we're not operating directly on a Razor file. However, it's possible the user is operating on a partial class that is associated with a Razor file.

                                if (IsPartialComponentClass(document))
                                {
                                    var newProject = newSolution.GetRequiredProject(projectId);
                                    @this.EnqueueUpdateOnProjectAndDependencies(newProject, newSolution);
                                }
                            },
                            (self: this, eventArgs: e),
                            cancellationToken)
                        .ConfigureAwait(false);
                    break;

                case WorkspaceChangeKind.SolutionAdded:
                case WorkspaceChangeKind.SolutionChanged:
                case WorkspaceChangeKind.SolutionCleared:
                case WorkspaceChangeKind.SolutionReloaded:
                case WorkspaceChangeKind.SolutionRemoved:
                    await _dispatcher
                        .RunOnDispatcherThreadAsync(
                            static (arg, _) =>
                            {
                                var (@this, eventArgs) = arg;
                                var oldSolution = eventArgs.OldSolution;
                                var newSolution = eventArgs.NewSolution;

                                foreach (var project in oldSolution.Projects)
                                {
                                    if (@this.TryGetProjectSnapshot(project, out var projectSnapshot))
                                    {
                                        @this.EnqueueUpdate(project: null, projectSnapshot);
                                    }
                                }

                                @this.InitializeSolution(newSolution);
                            },
                            (self: this, eventArgs: e),
                            cancellationToken)
                        .ConfigureAwait(false);
                    break;
            }

            // Let tests know that this event has completed
            NotifyWorkspaceChangedEventComplete?.Set();
        }
        catch (Exception ex)
        {
            Debug.Fail($"""
                WorkspaceProjectStateChangeDetector.Workspace_WorkspaceChanged threw exception:
                {ex.Message}
                Stack trace:
                {ex.StackTrace}
                """);
        }
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

    // Virtual for temporary VSCode workaround
    protected virtual void InitializeSolution(Solution solution)
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
            ClearWorkQueue();
            return;
        }

        switch (e.Kind)
        {
            case ProjectChangeKind.ProjectAdded:
            case ProjectChangeKind.DocumentRemoved:
            case ProjectChangeKind.DocumentAdded:
                var currentSolution = ProjectSnapshotManager.Workspace.CurrentSolution;
                var associatedWorkspaceProject = currentSolution.Projects
                    .FirstOrDefault(project => e.ProjectKey == ProjectKey.From(project));

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
        var workItem = new UpdateWorkspaceWorkItem(project, projectSnapshot, _workspaceStateGenerator, _dispatcher);

        EnsureWorkQueue();

        _workQueue?.Enqueue(projectSnapshot.Key.Id, workItem);
    }

    private bool TryGetProjectSnapshot(Project? project, [NotNullWhen(true)] out IProjectSnapshot? projectSnapshot)
    {
        if (project is null)
        {
            projectSnapshot = null;
            return false;
        }

        // ProjectKey could be null, if Roslyn doesn't know the IntermediateOutputPath for the project
        if (ProjectKey.From(project) is not { } projectKey)
        {
            projectSnapshot = null;
            return false;
        }

        projectSnapshot = ProjectSnapshotManager.GetLoadedProject(projectKey);
        return projectSnapshot is not null;
    }

    public void Dispose()
    {
        lock (_disposedLock)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            ClearWorkQueue();
        }
    }

    private void ClearWorkQueue()
    {
        lock (_workQueueAccessLock)
        {
            _workQueue?.Dispose();
            _workQueue = null;
        }
    }

    private class UpdateWorkspaceWorkItem : BatchableWorkItem
    {
        private readonly Project? _workspaceProject;
        private readonly IProjectSnapshot _projectSnapshot;
        private readonly ProjectWorkspaceStateGenerator _workspaceStateGenerator;
        private readonly ProjectSnapshotManagerDispatcher _dispatcher;

        public UpdateWorkspaceWorkItem(
            Project? workspaceProject,
            IProjectSnapshot projectSnapshot,
            ProjectWorkspaceStateGenerator workspaceStateGenerator,
            ProjectSnapshotManagerDispatcher dispatcher)
        {
            _workspaceProject = workspaceProject;
            _projectSnapshot = projectSnapshot ?? throw new ArgumentNullException(nameof(projectSnapshot));
            _workspaceStateGenerator = workspaceStateGenerator ?? throw new ArgumentNullException(nameof(workspaceStateGenerator));
            _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        }

        public override ValueTask ProcessAsync(CancellationToken cancellationToken)
        {
            var task = _dispatcher.RunOnDispatcherThreadAsync(
                static (arg, ct) =>
                {
                    var @this = arg;
                    @this._workspaceStateGenerator.Update(@this._workspaceProject, @this._projectSnapshot, ct);
                },
                arg: this,
                cancellationToken);

            return new ValueTask(task);
        }
    }
}
