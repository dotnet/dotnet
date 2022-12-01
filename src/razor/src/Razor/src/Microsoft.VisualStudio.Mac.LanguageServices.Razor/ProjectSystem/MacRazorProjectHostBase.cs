// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.Threading;
using MonoDevelop.Projects;
using MonoDevelop.Projects.MSBuild;

namespace Microsoft.VisualStudio.Mac.LanguageServices.Razor.ProjectSystem;

internal abstract class MacRazorProjectHostBase
{
    // References changes are always triggered when project changes happen.
    private const string ProjectChangedHint = "References";
    private const string IntermediateOutputPathPropertyName = "IntermediateOutputPath";
    private const string MSBuildProjectDirectoryPropertyName = "MSBuildProjectDirectory";

    private bool _batchingProjectChanges;
    protected readonly ProjectConfigurationFilePathStore ProjectConfigurationFilePathStore;
    private readonly ProjectSnapshotManagerBase _projectSnapshotManager;
    private readonly AsyncSemaphore _onProjectChangedInnerSemaphore;
    private readonly AsyncSemaphore _projectChangedSemaphore;
    private readonly Dictionary<string, HostDocument> _currentDocuments;

    public MacRazorProjectHostBase(
        DotNetProject project,
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        ProjectSnapshotManagerBase projectSnapshotManager,
        ProjectConfigurationFilePathStore projectConfigurationFilePathStore)
    {
        if (project is null)
        {
            throw new ArgumentNullException(nameof(project));
        }

        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        if (projectSnapshotManager is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManager));
        }

        if (projectConfigurationFilePathStore is null)
        {
            throw new ArgumentNullException(nameof(projectConfigurationFilePathStore));
        }

        DotNetProject = project;
        ProjectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        _projectSnapshotManager = projectSnapshotManager;
        ProjectConfigurationFilePathStore = projectConfigurationFilePathStore;
        _onProjectChangedInnerSemaphore = new AsyncSemaphore(initialCount: 1);
        _projectChangedSemaphore = new AsyncSemaphore(initialCount: 1);
        _currentDocuments = new Dictionary<string, HostDocument>(FilePathComparer.Instance);

        AttachToProject();
    }

    public DotNetProject DotNetProject { get; }

    public HostProject? HostProject { get; private set; }

    protected ProjectSnapshotManagerDispatcher ProjectSnapshotManagerDispatcher { get; }

    public void Detach()
    {
        ProjectSnapshotManagerDispatcher.AssertDispatcherThread();

        DotNetProject.Modified -= DotNetProject_Modified;

        UpdateHostProjectProjectSnapshotManagerDispatcher(null);
    }

    protected abstract Task OnProjectChangedAsync();

    // Protected virtual for testing
    protected virtual void AttachToProject()
    {
        ProjectSnapshotManagerDispatcher.AssertDispatcherThread();

        DotNetProject.Modified += DotNetProject_Modified;

        // Trigger the initial update to the project.
        _batchingProjectChanges = true;
        _ = Task.Factory.StartNew(ProjectChangedBackgroundAsync, null, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
    }

    // Must be called inside the lock.
    protected async Task UpdateHostProjectUnsafeAsync(HostProject? newHostProject)
    {
        await ProjectSnapshotManagerDispatcher.RunOnDispatcherThreadAsync(
            () => UpdateHostProjectProjectSnapshotManagerDispatcher(newHostProject), CancellationToken.None).ConfigureAwait(false);
    }

    protected async Task ExecuteWithLockAsync(Func<Task> func)
    {
        using (await _projectChangedSemaphore.EnterAsync().ConfigureAwait(false))
        {
            await func().ConfigureAwait(false);
        }
    }

    private async Task ProjectChangedBackgroundAsync(object state)
    {
        _batchingProjectChanges = false;

        // Ensure ordering, typically we'll only have 1 background thread in flight at a time. However,
        // between this line and the one prior another background thread could have also entered this
        // method. This is here to protect against us changing the order of project changed events.
        using (await _onProjectChangedInnerSemaphore.EnterAsync().ConfigureAwait(false))
        {
            await OnProjectChangedAsync();
        }
    }

    private void DotNetProject_Modified(object sender, SolutionItemModifiedEventArgs args)
    {
        if (args is null)
        {
            throw new ArgumentNullException(nameof(args));
        }

        _ = ProjectSnapshotManagerDispatcher.RunOnDispatcherThreadAsync((args, ct) =>
        {
            if (_batchingProjectChanges)
            {
                // Already waiting to recompute host project, no need to do any more work to determine if we're dirty.
                return;
            }

            var projectChanged = args.Any(arg => string.Equals(arg.Hint, ProjectChangedHint, StringComparison.Ordinal));
            if (projectChanged)
            {
                // This method can be spammed for tons of project change events but all we really care about is "are we dirty?".
                // Therefore, we re-dispatch here to allow any remaining project change events to fire and to then only have 1 host
                // project change trigger; this way we don't spam our own system with re-configure calls.
                _batchingProjectChanges = true;
                _ = Task.Factory.StartNew(ProjectChangedBackgroundAsync, null, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
            }
        }, args, CancellationToken.None);
    }

    private void UpdateHostProjectProjectSnapshotManagerDispatcher(object? state)
    {
        ProjectSnapshotManagerDispatcher.AssertDispatcherThread();

        var newHostProject = (HostProject?)state;

        if (HostProject is null && newHostProject is null)
        {
            // This is a no-op. This project isn't using Razor.
        }
        else if (HostProject is null && newHostProject != null)
        {
            _projectSnapshotManager.ProjectAdded(newHostProject);
        }
        else if (HostProject != null && newHostProject is null)
        {
            _projectSnapshotManager.ProjectRemoved(HostProject);
            ProjectConfigurationFilePathStore.Remove(HostProject.FilePath);
        }
        else
        {
            _projectSnapshotManager.ProjectConfigurationChanged(newHostProject);
        }

        HostProject = newHostProject;
    }

    protected void AddDocument(HostProject hostProject, string filePath, string relativeFilePath)
    {
        ProjectSnapshotManagerDispatcher.AssertDispatcherThread();

        if (_currentDocuments.ContainsKey(filePath))
        {
            return;
        }

        var hostDocument = new HostDocument(filePath, relativeFilePath);
        _projectSnapshotManager.DocumentAdded(hostProject, hostDocument, new FileTextLoader(filePath, defaultEncoding: null));

        _currentDocuments[filePath] = hostDocument;
    }

    protected void RemoveDocument(HostProject hostProject, string filePath)
    {
        if (_currentDocuments.TryGetValue(filePath, out var hostDocument))
        {
            _projectSnapshotManager.DocumentRemoved(hostProject, hostDocument);
            _currentDocuments.Remove(filePath);
        }
    }

    protected bool TryGetIntermediateOutputPath(
        IMSBuildEvaluatedPropertyCollection projectProperties,
        [NotNullWhen(returnValue: true)] out string? path)
    {
        if (!projectProperties.HasProperty(IntermediateOutputPathPropertyName))
        {
            path = null;
            return false;
        }

        var baseIntermediateOutputPathValue = DotNetProject.BaseIntermediateOutputPath.ToString();
        if (string.IsNullOrEmpty(baseIntermediateOutputPathValue))
        {
            path = null;
            return false;
        }

        var intermediateOutputPathValue = projectProperties.GetValue(IntermediateOutputPathPropertyName);
        if (string.IsNullOrEmpty(intermediateOutputPathValue))
        {
            path = null;
            return false;
        }

        var normalizedBaseIntermediateOutputPath = ToMacFilePath(baseIntermediateOutputPathValue);
        var basePath = new DirectoryInfo(normalizedBaseIntermediateOutputPath).Parent;
        var normalizedIntermediateOutputPathValue = ToMacFilePath(intermediateOutputPathValue);
        var joinedPath = Path.Combine(basePath.FullName, normalizedIntermediateOutputPathValue);

        if (!Directory.Exists(joinedPath))
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
            joinedPath = ResolveFallbackIntermediateOutputPath(projectProperties, normalizedIntermediateOutputPathValue);
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

    private string? ResolveFallbackIntermediateOutputPath(IMSBuildEvaluatedPropertyCollection projectProperties, string intermediateOutputPathValue)
    {
        var projectDirectory = projectProperties.HasProperty(MSBuildProjectDirectoryPropertyName)
            ? projectProperties.GetValue(MSBuildProjectDirectoryPropertyName)
            : DotNetProject.BaseDirectory.ToString();

        var normalizedProjectDirectory = ToMacFilePath(projectDirectory);
        var joinedPath = Path.Combine(normalizedProjectDirectory, intermediateOutputPathValue);
        if (!Directory.Exists(joinedPath))
        {
            return null;
        }

        return joinedPath;
    }

    /// <summary>
    /// Project system file paths get returned in windows based foramts. Meaning they typically have `\` as their path separator. Because of this in order to
    /// interoperate with local file system APIs (all of the Path / Directory etc. APIs) we need to convert the paths to use `/` as the path separator.
    /// </summary>
    /// <param name="filePath">A project system based file path.</param>
    /// <returns>A file path that can be used with File system APIs</returns>
    private static string ToMacFilePath(string filePath) => filePath.Replace('\\', Path.DirectorySeparatorChar);
}
