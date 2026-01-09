// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Utilities;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal abstract partial class AbstractRazorProjectInfoDriver : IRazorProjectInfoDriver, IDisposable
{
    private abstract record Work(ProjectKey ProjectKey);
    private sealed record Update(RazorProjectInfo ProjectInfo) : Work(ProjectInfo.ProjectKey);
    private sealed record Remove(ProjectKey ProjectKey) : Work(ProjectKey);

    protected static readonly TimeSpan DefaultDelay = TimeSpan.FromMilliseconds(250);

    protected readonly ILogger Logger;

    private readonly CancellationTokenSource _disposeTokenSource;
    private readonly AsyncBatchingWorkQueue<Work> _workQueue;
    private readonly HashSet<Work> _workerSet;
    private readonly Dictionary<ProjectKey, RazorProjectInfo> _latestProjectInfoMap;
    private ImmutableArray<IRazorProjectInfoListener> _listeners;
    private readonly TaskCompletionSource<bool> _initializationTaskSource;

    protected CancellationToken DisposalToken => _disposeTokenSource.Token;

    protected AbstractRazorProjectInfoDriver(ILoggerFactory loggerFactory, TimeSpan? delay = null)
    {
        Logger = loggerFactory.GetOrCreateLogger(GetType());

        _workerSet = new(Comparer.Instance);
        _disposeTokenSource = new();
        _workQueue = new AsyncBatchingWorkQueue<Work>(delay ?? DefaultDelay, ProcessBatchAsync, _disposeTokenSource.Token);
        _latestProjectInfoMap = [];
        _listeners = [];
        _initializationTaskSource = new();
    }

    public void Dispose()
    {
        if (_disposeTokenSource.IsCancellationRequested)
        {
            return;
        }

        _disposeTokenSource.Cancel();
        _disposeTokenSource.Dispose();

        OnDispose();
    }

    protected virtual void OnDispose() { }

    public Task WaitForInitializationAsync()
    {
#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks
        return _initializationTaskSource.Task;
#pragma warning restore VSTHRD003 // Avoid awaiting foreign Tasks
    }

    /// <summary>
    /// MUST be called in the constructor of any <see cref="AbstractRazorProjectInfoDriver"/> descendent
    /// to kick off initialization.
    /// </summary>
    protected void StartInitialization()
    {
        // Kick off initialization asynchronously and call TrySetResult(true) in the continuation.
        InitializeAsync(_disposeTokenSource.Token)
            .ContinueWith(
                _ =>
                {
                    _initializationTaskSource.TrySetResult(true);
                },
                _disposeTokenSource.Token,
                TaskContinuationOptions.OnlyOnRanToCompletion,
                TaskScheduler.Default)
            .Forget();
    }

    protected abstract Task InitializeAsync(CancellationToken cancellationToken);

    private async ValueTask ProcessBatchAsync(ImmutableArray<Work> items, CancellationToken token)
    {
        _workerSet.Clear();

        foreach (var work in items.GetMostRecentUniqueItems(_workerSet))
        {
            if (token.IsCancellationRequested)
            {
                return;
            }

            // Update our map first
            lock (_latestProjectInfoMap)
            {
                switch (work)
                {
                    case Update(var projectInfo):
                        Logger?.LogTrace($"Sending update for {projectInfo.FilePath} with {projectInfo.ProjectWorkspaceState.TagHelpers.Count} TagHelpers");
                        _latestProjectInfoMap[projectInfo.ProjectKey] = projectInfo;
                        break;

                    case Remove(var projectKey):
                        Logger?.LogTrace($"Removing {projectKey.Id}");
                        _latestProjectInfoMap.Remove(projectKey);
                        break;

                    default:
                        Assumed.Unreachable();
                        break;
                }
            }

            // Now, notify listeners
            foreach (var listener in _listeners)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }

                switch (work)
                {
                    case Update(var projectInfo):
                        await listener.UpdatedAsync(projectInfo, token).ConfigureAwait(false);
                        break;

                    case Remove(var projectKey):
                        await listener.RemovedAsync(projectKey, token).ConfigureAwait(false);
                        break;
                }
            }
        }
    }

    protected void EnqueueUpdate(RazorProjectInfo projectInfo)
    {
        _workQueue.AddWork(new Update(projectInfo));
    }

    protected void EnqueueRemove(ProjectKey projectKey)
    {
        _workQueue.AddWork(new Remove(projectKey));
    }

    public ImmutableArray<RazorProjectInfo> GetLatestProjectInfo()
    {
        if (!_initializationTaskSource.Task.IsCompleted)
        {
            throw new InvalidOperationException($"{nameof(GetLatestProjectInfo)} cannot be called before initialization is complete.");
        }

        lock (_latestProjectInfoMap)
        {
            using var builder = new PooledArrayBuilder<RazorProjectInfo>(capacity: _latestProjectInfoMap.Count);

            foreach (var (_, projectInfo) in _latestProjectInfoMap)
            {
                builder.Add(projectInfo);
            }

            return builder.ToImmutableAndClear();
        }
    }

    public void AddListener(IRazorProjectInfoListener listener)
    {
        if (!_initializationTaskSource.Task.IsCompleted)
        {
            throw new InvalidOperationException($"An {nameof(IRazorProjectInfoListener)} cannot be added before initialization is complete.");
        }

        ImmutableInterlocked.Update(ref _listeners, array => array.Add(listener));
    }
}
