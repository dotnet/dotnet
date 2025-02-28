// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.Properties;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.Razor.ProjectSystem;

internal abstract partial class WindowsRazorProjectHostBase : OnceInitializedOnceDisposedAsync, IProjectDynamicLoadComponent
{
    // AsyncSemaphore is banned. See https://github.com/dotnet/razor/issues/10390 for more info.
#pragma warning disable RS0030 // Do not use banned APIs

    private static readonly DataflowLinkOptions s_dataflowLinkOptions = new DataflowLinkOptions() { PropagateCompletion = true };

    private readonly IServiceProvider _serviceProvider;
    private readonly ProjectSnapshotManager _projectManager;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions;
    private readonly AsyncSemaphore _lock;

    private readonly Dictionary<ProjectConfigurationSlice, IDisposable> _projectSubscriptions = new();
    private readonly List<IDisposable> _disposables = new();

    internal const string BaseIntermediateOutputPathPropertyName = "BaseIntermediateOutputPath";
    internal const string IntermediateOutputPathPropertyName = "IntermediateOutputPath";
    internal const string MSBuildProjectDirectoryPropertyName = "MSBuildProjectDirectory";

    internal const string ConfigurationGeneralSchemaName = "ConfigurationGeneral";

    private bool _skipDirectoryExistCheck_TestOnly;

    protected WindowsRazorProjectHostBase(
        IUnconfiguredProjectCommonServices commonServices,
        IServiceProvider serviceProvider,
        ProjectSnapshotManager projectManager,
        LanguageServerFeatureOptions languageServerFeatureOptions)
        : base(commonServices.ThreadingService.JoinableTaskContext)
    {
        CommonServices = commonServices;
        _serviceProvider = serviceProvider;
        _projectManager = projectManager;
        _languageServerFeatureOptions = languageServerFeatureOptions;

        _lock = new AsyncSemaphore(initialCount: 1);
    }

    protected abstract ImmutableHashSet<string> GetRuleNames();

    protected abstract Task HandleProjectChangeAsync(string sliceDimensions, IProjectVersionedValue<IProjectSubscriptionUpdate> update);

    protected IUnconfiguredProjectCommonServices CommonServices { get; }

    protected sealed override Task InitializeCoreAsync(CancellationToken cancellationToken)
    {
        if (_languageServerFeatureOptions.UseRazorCohostServer)
        {
            return Task.CompletedTask;
        }

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

    private Task OnProjectChangedAsync(string sliceDimensions, IProjectVersionedValue<IProjectSubscriptionUpdate> update)
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
                await ExecuteWithLockAsync(
                    () => UpdateAsync(updater =>
                    {
                        // The Projects property creates a copy, so its okay to iterate through this
                        var projects = updater.GetProjects();
                        foreach (var project in projects)
                        {
                            RemoveProject(updater, project.Key);
                        }
                    },
                    CancellationToken.None))
                    .ConfigureAwait(false);
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

    private Task OnProjectRenamingAsync(string oldProjectFilePath, string newProjectFilePath)
    {
        // When a project gets renamed we expect any rules watched by the derived class to fire.
        //
        // However, the project snapshot manager uses the project Fullpath as the key. We want to just
        // reinitialize the HostProject with the same configuration and settings here, but the updated
        // FilePath.
        return ExecuteWithLockAsync(() => UpdateAsync(updater =>
        {
            var projectKeys = updater.GetProjectKeysWithFilePath(oldProjectFilePath);
            foreach (var projectKey in projectKeys)
            {
                if (updater.TryGetProject(projectKey, out var project))
                {
                    RemoveProject(updater, projectKey);

                    var hostProject = new HostProject(newProjectFilePath, project.IntermediateOutputPath, project.Configuration, project.RootNamespace);
                    UpdateProject(updater, hostProject);

                    // This should no-op in the common case, just putting it here for insurance.
                    foreach (var documentFilePath in project.DocumentFilePaths)
                    {
                        var documentSnapshot = project.GetRequiredDocument(documentFilePath);

                        var hostDocument = new HostDocument(
                            documentSnapshot.FilePath,
                            documentSnapshot.TargetPath,
                            documentSnapshot.FileKind);
                        updater.AddDocument(projectKey, hostDocument, new FileTextLoader(hostDocument.FilePath, null));
                    }
                }
            }
        }, CancellationToken.None));
    }

    protected ImmutableArray<ProjectKey> GetProjectKeysWithFilePath(string projectFilePath)
        => _projectManager.GetProjectKeysWithFilePath(projectFilePath);

    protected Task UpdateAsync(Action<ProjectSnapshotManager.Updater> action, CancellationToken cancellationToken)
    {
        return _projectManager.UpdateAsync(
            static (updater, state) =>
            {
                var (action, serviceProvider) = state;

                // This is a potential entry point for Razor start up when a project is opened with no open editors.
                // We need to ensure that any Razor start up services are initialized before the project manager is updated.
                RazorStartupInitializer.Initialize(serviceProvider);

                action(updater);
            },
            state: (action, _serviceProvider),
            cancellationToken);
    }

    protected static void UpdateProject(ProjectSnapshotManager.Updater updater, HostProject project)
    {
        if (!updater.ContainsProject(project.Key))
        {
            // Just in case we somehow got in a state where VS didn't tell us that solution close was finished, lets just
            // ensure we're going to actually do something with the new project that we've just been told about.
            // If VS did tell us, then this is a no-op.
            updater.SolutionOpened();
            updater.AddProject(project);
        }
        else
        {
            updater.UpdateProjectConfiguration(project);
        }
    }

    protected void RemoveProject(ProjectSnapshotManager.Updater updater, ProjectKey projectKey)
    {
        updater.RemoveProject(projectKey);
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

    protected bool TryGetBeforeIntermediateOutputPath(IImmutableDictionary<string, IProjectChangeDescription> state,
        [NotNullWhen(returnValue: true)] out string? path)
    {
        if (!state.TryGetValue(ConfigurationGeneralSchemaName, out var rule))
        {
            path = null;
            return false;
        }

        var beforeValues = rule.Before;

        return TryGetIntermediateOutputPathFromProjectRuleSnapshot(beforeValues, out path);
    }

    protected virtual bool TryGetIntermediateOutputPath(
        IImmutableDictionary<string, IProjectRuleSnapshot> state,
        [NotNullWhen(returnValue: true)] out string? path)
    {
        if (!state.TryGetValue(ConfigurationGeneralSchemaName, out var rule))
        {
            path = null;
            return false;
        }

        return TryGetIntermediateOutputPathFromProjectRuleSnapshot(rule, out path);
    }

    private bool TryGetIntermediateOutputPathFromProjectRuleSnapshot(IProjectRuleSnapshot rule, out string? path)
    {
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

        if (!Path.IsPathRooted(baseIntermediateOutputPathValue))
        {
            // For Razor class libraries, the base intermediate path is relative. Meaning instead of C:/project/obj it returns /obj.
            // The `new DirectoryInfo(...).Parent` call above is forgiving so if the path passed to it isn't absolute (Razor class library scenario) it utilizes Directory.GetCurrentDirectory, which
            // could be the C:/Windows/System path, or the solution path, or anything really.
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

        path = Path.GetFullPath(joinedPath);
        return true;
    }

    private string? ResolveFallbackIntermediateOutputPath(IProjectRuleSnapshot rule, string intermediateOutputPathValue)
    {
        if (!rule.Properties.TryGetValue(MSBuildProjectDirectoryPropertyName, out var projectDirectory))
        {
            // Can't resolve the project, bail.
            return null;
        }

        var joinedPath = Path.Combine(projectDirectory, intermediateOutputPathValue);
        if (!_skipDirectoryExistCheck_TestOnly && !Directory.Exists(joinedPath))
        {
            return null;
        }

        return joinedPath;
    }
}
