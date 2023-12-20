// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.AspNetCore.Razor;
using Microsoft.VisualStudio.LanguageServices;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.Properties;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal abstract class WindowsRazorProjectHostBase : OnceInitializedOnceDisposedAsync, IProjectDynamicLoadComponent
{
    private static readonly DataflowLinkOptions s_dataflowLinkOptions = new DataflowLinkOptions() { PropagateCompletion = true };

    private readonly Workspace _workspace;
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private readonly AsyncSemaphore _lock;

    private ProjectSnapshotManagerBase? _projectManager;
    private readonly Dictionary<ProjectConfigurationSlice, IDisposable> _projectSubscriptions = new();
    private readonly List<IDisposable> _disposables = new();
    protected readonly ProjectConfigurationFilePathStore ProjectConfigurationFilePathStore;

    internal const string BaseIntermediateOutputPathPropertyName = "BaseIntermediateOutputPath";
    internal const string IntermediateOutputPathPropertyName = "IntermediateOutputPath";
    internal const string MSBuildProjectDirectoryPropertyName = "MSBuildProjectDirectory";

    internal const string ConfigurationGeneralSchemaName = "ConfigurationGeneral";

    // Internal settable for testing
    // 250ms between publishes to prevent bursts of changes yet still be responsive to changes.
    internal int EnqueueDelay { get; set; } = 250;

    public WindowsRazorProjectHostBase(
        IUnconfiguredProjectCommonServices commonServices,
        [Import(typeof(VisualStudioWorkspace))] Workspace workspace,
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        ProjectConfigurationFilePathStore projectConfigurationFilePathStore)
        : base(commonServices.ThreadingService.JoinableTaskContext)
    {
        if (commonServices is null)
        {
            throw new ArgumentNullException(nameof(commonServices));
        }

        if (workspace is null)
        {
            throw new ArgumentNullException(nameof(workspace));
        }

        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        if (projectConfigurationFilePathStore is null)
        {
            throw new ArgumentNullException(nameof(projectConfigurationFilePathStore));
        }

        CommonServices = commonServices;
        _workspace = workspace;
        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;

        _lock = new AsyncSemaphore(initialCount: 1);
        ProjectConfigurationFilePathStore = projectConfigurationFilePathStore;
    }

    // Internal for testing
    protected WindowsRazorProjectHostBase(
        IUnconfiguredProjectCommonServices commonServices,
        Workspace workspace,
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        ProjectConfigurationFilePathStore projectConfigurationFilePathStore,
        ProjectSnapshotManagerBase projectManager)
        : this(commonServices, workspace, projectSnapshotManagerDispatcher, projectConfigurationFilePathStore)
    {
        if (projectManager is null)
        {
            throw new ArgumentNullException(nameof(projectManager));
        }

        _projectManager = projectManager;
    }

    protected abstract ImmutableHashSet<string> GetRuleNames();

    protected abstract Task HandleProjectChangeAsync(string sliceDimensions, IProjectVersionedValue<IProjectSubscriptionUpdate> update);

    protected IUnconfiguredProjectCommonServices CommonServices { get; }

    internal bool SkipIntermediateOutputPathExistCheck_TestOnly { get; set; }

    // internal for tests. The product will call through the IProjectDynamicLoadComponent interface.
    internal Task LoadAsync()
    {
        return InitializeAsync();
    }

    protected sealed override Task InitializeCoreAsync(CancellationToken cancellationToken)
    {
        CommonServices.UnconfiguredProject.ProjectRenaming += UnconfiguredProject_ProjectRenamingAsync;

        // CPS represents the various target frameworks that a project has in configuration groups, which are called "slices". Each
        // slice represents a variation of a project configuration. So for example, a given multi-targeted project would have:
        //
        // Configuration    | Platform     | Configuration Groups (slices)
        // -----------------------------------------------------
        // Debug            | Any CPU      | net6.0, net7.0
        // Release          | Any CPU      | net6.0, net7.0
        //
        // This subscription hooks to the ActiveConfigurationGroupSubscriptionService which will feed us data whenever a
        // "slice" is added or removed, for the current active Configuration/Platform combination. This is a nice mix between
        // not having too many things loaded (ie, we don't get 4 projects representing the full matrix of configurations) but
        // still having distinct projects per target framework. If the user changes configuration from Debug to Release, we will
        // get updates for the slices indicating that change, but within a specific configuration, both target frameworks will
        // get updates for project changes, which means our data won't be stale if the user changes the active context.
        //
        // CPS also manages the slices themselves, and we get updates as they change. eg the first event we get has one target
        // framework, then we get an update containing both. If the user adds one, we get another event.  Either way
        // the event also gives us a datasource we can subscribe to in order to receive updates about that project. It is
        // important that we maintain our list of subscriptions because if a slice is removed, we are responsible for cleaning
        // up our resources.
        //
        // It's worth noting the events also give us a key, which is a list of "dimensions". We only care about the target framework
        // but they could be strictly anything, and could change at any time. If they do, we'll get new events, so the easiest
        // thing to do is just treat the key as an opaque object. CPS implements IEquatable on it, expressly for this purpose.
        // We should not have any logic that depends on the contents of the key.
        //
        // Somewhat similar to https://github.com/dotnet/project-system/blob/bf4f33ec1843551eb775f73cff515a939aa2f629/src/Microsoft.VisualStudio.ProjectSystem.Managed/ProjectSystem/Tree/Dependencies/Subscriptions/DependenciesSnapshotProvider.cs
        // but a lot simpler.
        _disposables.Add(CommonServices.ActiveConfigurationGroupSubscriptionService.SourceBlock.LinkTo(
            DataflowBlockSlim.CreateActionBlock<IProjectVersionedValue<ConfigurationSubscriptionSources>>(SlicesChanged, nameFormat: "Slice {1}"),
            new DataflowLinkOptions() { PropagateCompletion = true }));

        // Join, in the JTF sense, the ActiveConfigurationGroupSubscriptionService, to help avoid hangs in our OnProjectChangedAsync method
        _disposables.Add(ProjectDataSources.JoinUpstreamDataSources(CommonServices.ThreadingService.JoinableTaskFactory, CommonServices.FaultHandlerService, CommonServices.ActiveConfigurationGroupSubscriptionService));

        return Task.CompletedTask;
    }

    private void SlicesChanged(IProjectVersionedValue<ConfigurationSubscriptionSources> value)
    {
        // Create a new dictionary representing the subscriptions we know about at the start of the update. Data flow ensures
        // this method will not be called in parallel.
        var current = new Dictionary<ProjectConfigurationSlice, IDisposable>(_projectSubscriptions);

        foreach (var (slice, source) in value.Value)
        {
            if (!_projectSubscriptions.TryGetValue(slice, out var dataSource))
            {
                Assumes.False(current.ContainsKey(slice));

                var dimensions = string.Join(";", slice.Dimensions.Values);

                // This is a new slice that we didn't previously know about, either because its a new target framework, or how dimensions
                // are calculated has changed. We simply subscribe to updates for it, and let our action block code handle whether the
                // distinction is important. To put it another way, we may end up having multiple subscriptions and events that would be
                // affect about the same project.razor.bin file, but our event handling code ensures we don't handle them more than
                // necessary.
                var subscription = source.JointRuleSource.SourceBlock.LinkTo(
                    DataflowBlockSlim.CreateActionBlock<IProjectVersionedValue<IProjectSubscriptionUpdate>>(v => OnProjectChangedAsync(dimensions, v), nameFormat: "OnProjectChanged {1}"),
                    initialDataAsNew: true,
                    suppressVersionOnlyUpdates: true,
                    ruleNames: GetRuleNames(),
                    linkOptions: s_dataflowLinkOptions);

                _projectSubscriptions.Add(slice, subscription);
            }
            else
            {
                // We already know about this slice, so remove it from our "current" list, as we have nothing to do for it
                Assumes.True(current.Remove(slice));
            }
        }

        // Anything left in the current list must have been removed, so we dispose it
        foreach (var (slice, subscription) in current)
        {
            Assumes.True(_projectSubscriptions.Remove(slice));

            subscription.Dispose();
        }
    }

    // Internal for testing
    internal Task OnProjectChangedAsync(string sliceDimensions, IProjectVersionedValue<IProjectSubscriptionUpdate> update)
    {
        if (IsDisposing || IsDisposed)
        {
            return Task.CompletedTask;
        }

        return CommonServices.TasksService.LoadedProjectAsync(() => ExecuteWithLockAsync(() =>
        {
            return HandleProjectChangeAsync(sliceDimensions, update);
        }), registerFaultHandler: true).Task;
    }

    protected override async Task DisposeCoreAsync(bool initialized)
    {
        if (initialized)
        {
            CommonServices.UnconfiguredProject.ProjectRenaming -= UnconfiguredProject_ProjectRenamingAsync;

            // If we haven't been initialized, lets not start now
            if (_projectManager is not null)
            {
                await ExecuteWithLockAsync(() => UpdateAsync(() =>
                    {
                        // The Projects property creates a copy, so its okay to iterate through this
                        var projects = _projectManager.GetProjects();
                        foreach (var project in projects)
                        {
                            UninitializeProjectUnsafe(project.Key);
                        }
                    }, CancellationToken.None)).ConfigureAwait(false);
            }

            foreach (var (slice, subscription) in _projectSubscriptions)
            {
                subscription.Dispose();
            }

            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }
    }

    // Internal for tests
    internal Task OnProjectRenamingAsync(string oldProjectFilePath, string newProjectFilePath)
    {
        // When a project gets renamed we expect any rules watched by the derived class to fire.
        //
        // However, the project snapshot manager uses the project Fullpath as the key. We want to just
        // reinitialize the HostProject with the same configuration and settings here, but the updated
        // FilePath.
        return ExecuteWithLockAsync(() => UpdateAsync(() =>
        {
            var projectManager = GetProjectManager();

            var projectKeys = projectManager.GetAllProjectKeys(oldProjectFilePath);
            foreach (var projectKey in projectKeys)
            {
                var current = projectManager.GetLoadedProject(projectKey);
                if (current?.Configuration is not null)
                {
                    UninitializeProjectUnsafe(projectKey);

                    var hostProject = new HostProject(newProjectFilePath, current.IntermediateOutputPath, current.Configuration, current.RootNamespace, current.DisplayName);
                    UpdateProjectUnsafe(hostProject);

                    // This should no-op in the common case, just putting it here for insurance.
                    foreach (var documentFilePath in current.DocumentFilePaths)
                    {
                        var documentSnapshot = current.GetDocument(documentFilePath);
                        Assumes.NotNull(documentSnapshot);
                        // TODO: The creation of the HostProject here is silly
                        var hostDocument = new HostDocument(documentSnapshot.FilePath.AssumeNotNull(), documentSnapshot.TargetPath.AssumeNotNull(), documentSnapshot.FileKind);
                        AddDocumentUnsafe(hostProject.Key, hostDocument);
                    }
                }
            }
        }, CancellationToken.None));
    }

    // Should only be called from the project snapshot manager's specialized thread.
    protected ProjectSnapshotManagerBase GetProjectManager()
    {
        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        _projectManager ??= (ProjectSnapshotManagerBase)_workspace.Services.GetLanguageServices(RazorLanguage.Name).GetRequiredService<ProjectSnapshotManager>();

        return _projectManager;
    }

    protected Task UpdateAsync(Action action, CancellationToken cancellationToken)
        => _projectSnapshotManagerDispatcher.RunOnDispatcherThreadAsync(action, cancellationToken);

    protected void UninitializeProjectUnsafe(ProjectKey projectKey)
    {
        var projectManager = GetProjectManager();
        var current = projectManager.GetLoadedProject(projectKey);
        if (current is not null)
        {
            projectManager.ProjectRemoved(projectKey);
            ProjectConfigurationFilePathStore.Remove(projectKey);
        }
    }

    protected void UpdateProjectUnsafe(HostProject project)
    {
        var projectManager = GetProjectManager();

        var current = projectManager.GetLoadedProject(project.Key);
        if (current is null)
        {
            // Just in case we somehow got in a state where VS didn't tell us that solution close was finished, lets just
            // ensure we're going to actually do something with the new project that we've just been told about.
            // If VS did tell us, then this is a no-op.
            projectManager.SolutionOpened();

            projectManager.ProjectAdded(project);
        }
        else
        {
            projectManager.ProjectConfigurationChanged(project);
        }
    }

    protected void AddDocumentUnsafe(ProjectKey projectKey, HostDocument document)
    {
        var projectManager = GetProjectManager();
        projectManager.DocumentAdded(projectKey, document, new FileTextLoader(document.FilePath, null));
    }

    protected void RemoveDocumentUnsafe(ProjectKey projectKey, HostDocument document)
    {
        var projectManager = GetProjectManager();
        projectManager.DocumentRemoved(projectKey, document);
    }

    private async Task ExecuteWithLockAsync(Func<Task> func)
    {
        using (JoinableCollection.Join())
        {
            using (await _lock.EnterAsync().ConfigureAwait(false))
            {
                var task = JoinableFactory.RunAsync(func);
                await task.Task.ConfigureAwait(false);
            }
        }
    }

    Task IProjectDynamicLoadComponent.LoadAsync()
    {
        return InitializeAsync();
    }

    Task IProjectDynamicLoadComponent.UnloadAsync()
    {
        return DisposeAsync();
    }

    private Task UnconfiguredProject_ProjectRenamingAsync(object? sender, ProjectRenamedEventArgs args)
        => OnProjectRenamingAsync(args.OldFullPath, args.NewFullPath);

    // virtual for testing
    protected virtual bool TryGetIntermediateOutputPath(
        IImmutableDictionary<string, IProjectRuleSnapshot> state,
        [NotNullWhen(returnValue: true)] out string? path)
    {
        if (!state.TryGetValue(ConfigurationGeneralSchemaName, out var rule))
        {
            path = null;
            return false;
        }

        if (!rule.Properties.TryGetValue(BaseIntermediateOutputPathPropertyName, out var baseIntermediateOutputPathValue))
        {
            path = null;
            return false;
        }

        if (!rule.Properties.TryGetValue(IntermediateOutputPathPropertyName, out var intermediateOutputPathValue))
        {
            path = null;
            return false;
        }

        if (string.IsNullOrEmpty(intermediateOutputPathValue) || string.IsNullOrEmpty(baseIntermediateOutputPathValue))
        {
            path = null;
            return false;
        }

        var basePath = new DirectoryInfo(baseIntermediateOutputPathValue).Parent;
        var joinedPath = Path.Combine(basePath.FullName, intermediateOutputPathValue);

        if (!SkipIntermediateOutputPathExistCheck_TestOnly && !Directory.Exists(joinedPath))
        {
            // The directory doesn't exist for the currently executing application.
            // This can occur in Razor class library scenarios because:
            //   1. Razor class libraries base intermediate path is not absolute. Meaning instead of C:/project/obj it returns /obj.
            //   2. Our `new DirectoryInfo(...).Parent` call above is forgiving so if the path passed to it isn't absolute (Razor class library scenario) it utilizes Directory.GetCurrentDirectory where
            //      in this case would be the C:/Windows/System path
            // Because of the above two issues the joinedPath ends up looking like "C:\WINDOWS\system32\obj\Debug\netstandard2.0\" which doesn't actually exist and of course isn't writeable. The end-user effect of this
            // quirk means that you don't get any component completions for Razor class libraries because we're unable to capture their project state information.
            //
            // To workaround these inconsistencies with Razor class libraries we fall back to the MSBuildProjectDirectory and build what we think is the intermediate output path.
            joinedPath = ResolveFallbackIntermediateOutputPath(rule, intermediateOutputPathValue);
            if (joinedPath is null)
            {
                // Still couldn't resolve a valid directory.
                path = null;
                return false;
            }
        }

        path = joinedPath;
        return true;
    }

    private static string? ResolveFallbackIntermediateOutputPath(IProjectRuleSnapshot rule, string intermediateOutputPathValue)
    {
        if (!rule.Properties.TryGetValue(MSBuildProjectDirectoryPropertyName, out var projectDirectory))
        {
            // Can't resolve the project, bail.
            return null;
        }

        var joinedPath = Path.Combine(projectDirectory, intermediateOutputPathValue);
        if (!Directory.Exists(joinedPath))
        {
            return null;
        }

        return joinedPath;
    }
}
