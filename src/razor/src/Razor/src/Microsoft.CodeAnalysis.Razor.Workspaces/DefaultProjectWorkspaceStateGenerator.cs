// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Composition;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.CodeAnalysis.Razor;

[Shared]
[Export(typeof(ProjectWorkspaceStateGenerator))]
[Export(typeof(IProjectSnapshotChangeTrigger))]
internal class DefaultProjectWorkspaceStateGenerator : ProjectWorkspaceStateGenerator, IDisposable
{
    // Internal for testing
    internal readonly Dictionary<ProjectKey, UpdateItem> Updates;

    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private readonly SemaphoreSlim _semaphore;
    private ProjectSnapshotManagerBase _projectManager;
    private ITagHelperResolver _tagHelperResolver;
    private bool _disposed;

    [ImportingConstructor]
    public DefaultProjectWorkspaceStateGenerator(ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher)
    {
        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;

        _semaphore = new SemaphoreSlim(initialCount: 1);
        Updates = new Dictionary<ProjectKey, UpdateItem>();
    }

    // Used in unit tests to ensure we can control when background work starts.
    public ManualResetEventSlim BlockBackgroundWorkStart { get; set; }

    // Used in unit tests to ensure we can know when background work finishes.
    public ManualResetEventSlim NotifyBackgroundWorkCompleted { get; set; }

    public override void Initialize(ProjectSnapshotManagerBase projectManager)
    {
        if (projectManager is null)
        {
            throw new ArgumentNullException(nameof(projectManager));
        }

        _projectManager = projectManager;

        _tagHelperResolver = _projectManager.Workspace.Services.GetRequiredService<ITagHelperResolver>();
    }

    public override void Update(Project workspaceProject, IProjectSnapshot projectSnapshot, CancellationToken cancellationToken)
    {
        if (projectSnapshot is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshot));
        }

        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        if (_disposed)
        {
            return;
        }

        if (Updates.TryGetValue(projectSnapshot.Key, out var updateItem) &&
            !updateItem.Task.IsCompleted &&
            !updateItem.Cts.IsCancellationRequested)
        {
            updateItem.Cts.Cancel();
        }

        if (updateItem?.Cts.IsCancellationRequested == false)
        {
            updateItem?.Cts.Dispose();
        }

        var lcts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var updateTask = Task.Factory.StartNew(
            () => UpdateWorkspaceStateAsync(workspaceProject, projectSnapshot, lcts.Token),
            lcts.Token,
            TaskCreationOptions.None,
            TaskScheduler.Default).Unwrap();
        updateItem = new UpdateItem(updateTask, lcts);
        Updates[projectSnapshot.Key] = updateItem;
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        foreach (var update in Updates)
        {
            if (!update.Value.Task.IsCompleted &&
                !update.Value.Cts.IsCancellationRequested)
            {
                update.Value.Cts.Cancel();
            }
        }

        // Release before dispose to ensure we don't throw exceptions from the background thread trying to release
        // while we're disposing. Multiple releases are fine, and if we release and it lets something passed the lock
        // our cancellation token check will mean its a no-op.
        _semaphore.Release();
        _semaphore.Dispose();

        BlockBackgroundWorkStart?.Set();
    }

    private async Task UpdateWorkspaceStateAsync(Project workspaceProject, IProjectSnapshot projectSnapshot, CancellationToken cancellationToken)
    {
        // We fire this up on a background thread so we could have been disposed already, and if so, waiting on our semaphore
        // throws an exception.
        if (_disposed)
        {
            return;
        }

        try
        {
            // Only allow a single TagHelper resolver request to process at a time in order to reduce Visual Studio memory pressure. Typically a TagHelper resolution result can be upwards of 10mb+.
            // So if we now do multiple requests to resolve TagHelpers simultaneously it results in only a single one executing at a time so that we don't have N number of requests in flight with these
            // 10mb payloads waiting to be processed.
            await _semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception)
        {
            // Object disposed or task cancelled exceptions should be swallowed/no-op'd
            return;
        }

        try
        {
            OnStartingBackgroundWork();

            if (cancellationToken.IsCancellationRequested)
            {
                // Silently cancel, we're the only ones creating these tasks.
                return;
            }

            var workspaceState = ProjectWorkspaceState.Default;
            try
            {
                if (workspaceProject != null)
                {
                    var csharpLanguageVersion = LanguageVersion.Default;
                    var csharpParseOptions = (CSharpParseOptions)workspaceProject.ParseOptions;
                    if (csharpParseOptions is null)
                    {
                        Debug.Fail("Workspace project should always have CSharp parse options.");
                    }
                    else
                    {
                        csharpLanguageVersion = csharpParseOptions.LanguageVersion;
                    }

                    var tagHelpers = await _tagHelperResolver.GetTagHelpersAsync(workspaceProject, projectSnapshot, cancellationToken).ConfigureAwait(false);
                    workspaceState = new ProjectWorkspaceState(tagHelpers, csharpLanguageVersion);
                }
            }
            catch (OperationCanceledException)
            {
                // Abort work if we get a task cancelled exception
                return;
            }
            catch (Exception ex)
            {
                await _projectSnapshotManagerDispatcher.RunOnDispatcherThreadAsync(
                   () => _projectManager.ReportError(ex, projectSnapshot),
                   // Don't allow errors to be cancelled
                   CancellationToken.None).ConfigureAwait(false);
                return;
            }

            if (cancellationToken.IsCancellationRequested)
            {
                // Silently cancel, we're the only ones creating these tasks.
                return;
            }

            await _projectSnapshotManagerDispatcher.RunOnDispatcherThreadAsync(
                () =>
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }

                    ReportWorkspaceStateChange(projectSnapshot.Key, workspaceState);
                },
                cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            // Abort work if we get a task canceled exception
            return;
        }
        catch (Exception ex)
        {
            // This is something totally unexpected, let's just send it over to the project manager.
            await _projectSnapshotManagerDispatcher.RunOnDispatcherThreadAsync(
                () => _projectManager.ReportError(ex),
                // Don't allow errors to be cancelled
                CancellationToken.None).ConfigureAwait(false);
        }
        finally
        {
            try
            {
                // Prevent ObjectDisposedException if we've disposed before we got here. The dispose method will release
                // anyway, so we're all good.
                if (!_disposed)
                {
                    _semaphore.Release();
                }
            }
            catch
            {
                // Swallow exceptions that happen from releasing the semaphore.
            }
        }

        OnBackgroundWorkCompleted();
    }

    private void ReportWorkspaceStateChange(ProjectKey projectKey, ProjectWorkspaceState workspaceStateChange)
    {
        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        _projectManager.ProjectWorkspaceStateChanged(projectKey, workspaceStateChange);
    }

    private void OnStartingBackgroundWork()
    {
        if (BlockBackgroundWorkStart != null)
        {
            BlockBackgroundWorkStart.Wait();
            BlockBackgroundWorkStart.Reset();
        }
    }

    private void OnBackgroundWorkCompleted()
    {
        if (NotifyBackgroundWorkCompleted != null)
        {
            NotifyBackgroundWorkCompleted.Set();
        }
    }

    // Internal for testing
    internal class UpdateItem
    {
        public UpdateItem(Task task, CancellationTokenSource cts)
        {
            if (task is null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            if (cts is null)
            {
                throw new ArgumentNullException(nameof(cts));
            }

            Task = task;
            Cts = cts;
        }

        public Task Task { get; }

        public CancellationTokenSource Cts { get; }
    }
}
