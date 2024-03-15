// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.VisualStudio.LanguageServices.Razor;

[Export(typeof(IProjectWorkspaceStateGenerator))]
[method: ImportingConstructor]
internal sealed class ProjectWorkspaceStateGenerator(
    ProjectSnapshotManagerBase projectManager,
    ITagHelperResolver tagHelperResolver,
    ProjectSnapshotManagerDispatcher dispatcher,
    IErrorReporter errorReporter,
    ITelemetryReporter telemetryReporter)
    : IProjectWorkspaceStateGenerator, IDisposable
{
    // Internal for testing
    internal readonly Dictionary<ProjectKey, UpdateItem> Updates = new();

    private readonly ProjectSnapshotManagerBase _projectManager = projectManager;
    private readonly ITagHelperResolver _tagHelperResolver = tagHelperResolver;
    private readonly ProjectSnapshotManagerDispatcher _dispatcher = dispatcher;
    private readonly IErrorReporter _errorReporter = errorReporter;
    private readonly ITelemetryReporter _telemetryReporter = telemetryReporter;
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(initialCount: 1);

    private bool _disposed;

    // Used in unit tests to ensure we can control when background work starts.
    public ManualResetEventSlim? BlockBackgroundWorkStart { get; set; }

    // Used in unit tests to ensure we can know when background work finishes.
    public ManualResetEventSlim? NotifyBackgroundWorkCompleted { get; set; }

    public void Update(Project? workspaceProject, IProjectSnapshot projectSnapshot, CancellationToken cancellationToken)
    {
        if (projectSnapshot is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshot));
        }

        _dispatcher.AssertRunningOnDispatcher();

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

    private async Task UpdateWorkspaceStateAsync(Project? workspaceProject, IProjectSnapshot projectSnapshot, CancellationToken cancellationToken)
    {
        // We fire this up on a background thread so we could have been disposed already, and if so, waiting on our semaphore
        // throws an exception.
        if (_disposed)
        {
            return;
        }

        // Specifically not using BeginBlock because we want to capture cases where tag helper discovery never finishes.
        var telemetryId = Guid.NewGuid();
        _telemetryReporter.ReportEvent("taghelperresolve/begin", Severity.Normal,
            new Property("id", telemetryId),
            new Property("tagHelperCount", projectSnapshot.ProjectWorkspaceState.TagHelpers.Length));

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
                    var csharpParseOptions = workspaceProject.ParseOptions as CSharpParseOptions;
                    if (csharpParseOptions is null)
                    {
                        Debug.Fail("Workspace project should always have CSharp parse options.");
                    }
                    else
                    {
                        csharpLanguageVersion = csharpParseOptions.LanguageVersion;
                    }

                    using var _ = StopwatchPool.GetPooledObject(out var watch);

                    watch.Restart();
                    var tagHelpers = await _tagHelperResolver.GetTagHelpersAsync(workspaceProject, projectSnapshot, cancellationToken).ConfigureAwait(false);
                    watch.Stop();

                    // don't report success if the work was cancelled
                    cancellationToken.ThrowIfCancellationRequested();

                    _telemetryReporter.ReportEvent("taghelperresolve/end", Severity.Normal,
                        new Property("id", telemetryId),
                        new Property("ellapsedms", watch.ElapsedMilliseconds),
                        new Property("result", "success"),
                        new Property("tagHelperCount", tagHelpers.Length));

                    workspaceState = ProjectWorkspaceState.Create(tagHelpers, csharpLanguageVersion);
                }
            }
            catch (OperationCanceledException)
            {
                // Abort work if we get a task cancelled exception
                _telemetryReporter.ReportEvent("taghelperresolve/end", Severity.Normal,
                    new Property("id", telemetryId),
                    new Property("result", "cancel"));
                return;
            }
            catch (Exception ex)
            {
                _telemetryReporter.ReportEvent("taghelperresolve/end", Severity.Normal,
                    new Property("id", telemetryId),
                    new Property("result", "error"));

                await _dispatcher.RunAsync(
                   () => _errorReporter.ReportError(ex, projectSnapshot),
                   // Don't allow errors to be cancelled
                   CancellationToken.None).ConfigureAwait(false);
                return;
            }

            if (cancellationToken.IsCancellationRequested)
            {
                // Silently cancel, we're the only ones creating these tasks.
                return;
            }

            await _dispatcher.RunAsync(
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
            await _dispatcher.RunAsync(
                () => _errorReporter.ReportError(ex),
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
        _dispatcher.AssertRunningOnDispatcher();

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
    internal class UpdateItem(Task task, CancellationTokenSource cts)
    {
        public Task Task { get; } = task;

        public CancellationTokenSource Cts { get; } = cts;
    }
}
