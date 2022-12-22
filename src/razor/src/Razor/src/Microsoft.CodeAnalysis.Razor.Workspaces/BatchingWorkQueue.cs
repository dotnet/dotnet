// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Internal;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

internal sealed class BatchingWorkQueue : IDisposable
{
    // Interactions with this collection should always take a lock on the collection and
    // be careful about interactions it may have with the on-going timer. The reasons
    // stem from the transactional manner that we use to modify the collection. For instance
    // we'll capture workloads and then after processing a lot of work items we'll leave open
    // the opportunity to re-start our processing loop to ensure things get processed at an
    // efficient pace.
    private readonly Dictionary<string, BatchableWorkItem> _work;
    private readonly TimeSpan _batchingTimeSpan;
    private readonly ErrorReporter _errorReporter;
    private readonly CancellationTokenSource _disposalCts;
    private Timer? _timer;
    private bool _disposed;

    public BatchingWorkQueue(
        TimeSpan batchingTimeSpan,
        StringComparer keyComparer,
        ErrorReporter errorReporter)
    {
        if (keyComparer is null)
        {
            throw new ArgumentNullException(nameof(keyComparer));
        }

        if (errorReporter is null)
        {
            throw new ArgumentNullException(nameof(errorReporter));
        }

        _batchingTimeSpan = batchingTimeSpan;
        _errorReporter = errorReporter;
        _disposalCts = new CancellationTokenSource();
        _work = new Dictionary<string, BatchableWorkItem>(keyComparer);
    }

    private bool IsScheduledOrRunning => _timer != null;

    // Used in unit tests to ensure we can control when background work starts.
    private ManualResetEventSlim? BlockBackgroundWorkStart { get; set; }

    // Used in unit tests to ensure we can know when background work finishes.
    private ManualResetEventSlim? NotifyBackgroundWorkStarting { get; set; }

    // Used in unit tests to ensure we can know when background has captured its current workload.
    private ManualResetEventSlim? NotifyBackgroundCapturedWorkload { get; set; }

    // Used in unit tests to ensure we can control when background work completes.
    private ManualResetEventSlim? BlockBackgroundWorkCompleting { get; set; }

    // Used in unit tests to ensure we can know when background work finishes.
    private ManualResetEventSlim? NotifyBackgroundWorkCompleted { get; set; }

    // Used in unit tests to ensure we can know when errors are reported
    private ManualResetEventSlim? NotifyErrorBeingReported { get; set; }

    // Used in unit tests to ensure we can know when background workloads are completing
    private ManualResetEventSlim? NotifyBackgroundWorkCompleting { get; set; }

    /// <summary>
    /// Adds the provided <paramref name="workItem"/> to a work queue under the specified <paramref name="key"/>.
    /// Multiple enqueues under the same <paramref name="key"/> will use the last enqueued <paramref name="workItem"/>.
    /// </summary>
    /// <param name="key">An identifier used to track <paramref name="workItem"/>'s.</param>
    /// <param name="workItem">An item to process</param>
    public void Enqueue(string key, BatchableWorkItem workItem)
    {
        lock (_work)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(BatchingWorkQueue));
            }

            // We only want to store the last 'seen' work item. That way when we pick one to process it's
            // always the latest version to use.
            _work[key] = workItem;

            StartWorker();
        }
    }

    public void Dispose()
    {
        lock (_work)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _timer?.Dispose();
            _timer = null;
            _work.Clear();
            _disposalCts.Cancel();
            _disposalCts.Dispose();
        }
    }

    private void StartWorker()
    {
        // Access to the timer is protected by the lock in Enqueue and in Timer_TickAsync
        // Timer will fire after a fixed delay, but only once.
        _timer ??= NonCapturingTimer.Create(
            state =>
            {
                Assumes.NotNull(state);
                ((BatchingWorkQueue)state).Timer_TickAsync().Forget();
            },
            this,
            _batchingTimeSpan,
            Timeout.InfiniteTimeSpan);
    }

    private async Task Timer_TickAsync()
    {
        try
        {
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);

            OnStartingBackgroundWork();

            KeyValuePair<string, BatchableWorkItem>[] work;
            lock (_work)
            {
                work = _work.ToArray();
                _work.Clear();
            }

            OnBackgroundCapturedWorkload();

            for (var i = 0; i < work.Length; i++)
            {
                var workItem = work[i].Value;
                try
                {
                    await workItem.ProcessAsync(_disposalCts.Token).ConfigureAwait(false);
                }
                catch (OperationCanceledException) when (_disposalCts.IsCancellationRequested)
                {
                    // Expected shutdown case, lets not log an error.
                }
                catch (Exception ex)
                {
                    // Work item failed to process, allow the other process events to continue.
                    _errorReporter.ReportError(ex);
                }
            }

            OnCompletingBackgroundWork();

            lock (_work)
            {
                // Suppress analyzer that suggests using DisposeAsync().
#pragma warning disable VSTHRD103 // Call async methods when in an async method

                // Resetting the timer allows another batch of work to start.
                _timer?.Dispose();
                _timer = null;

#pragma warning restore VSTHRD103

                // If more work came in while we were running start the worker again if we're still alive
                if (_work.Count > 0 && !_disposed)
                {
                    StartWorker();
                }
            }

            OnCompletedBackgroundWork();
        }
        catch (Exception ex)
        {
            // This is something totally unexpected
            Debug.Fail("Batching work queue failed unexpectedly");
            _errorReporter.ReportError(ex);
        }
    }

    private void OnStartingBackgroundWork()
    {
        NotifyBackgroundWorkStarting?.Set();

        if (BlockBackgroundWorkStart != null)
        {
            BlockBackgroundWorkStart.Wait();
            BlockBackgroundWorkStart.Reset();
        }
    }

    private void OnCompletingBackgroundWork()
    {
        NotifyBackgroundWorkCompleting?.Set();

        if (BlockBackgroundWorkCompleting != null)
        {
            BlockBackgroundWorkCompleting.Wait();
            BlockBackgroundWorkCompleting.Reset();
        }
    }

    private void OnCompletedBackgroundWork()
    {
        NotifyBackgroundWorkCompleted?.Set();
    }

    private void OnBackgroundCapturedWorkload()
    {
        NotifyBackgroundCapturedWorkload?.Set();
    }

    internal TestAccessor GetTestAccessor()
        => new(this);

    internal class TestAccessor
    {
        private readonly BatchingWorkQueue _queue;

        public TestAccessor(BatchingWorkQueue queue)
        {
            _queue = queue;
        }

        public bool IsScheduledOrRunning => _queue.IsScheduledOrRunning;

        public Dictionary<string, BatchableWorkItem> Work => _queue._work;

        public ManualResetEventSlim? BlockBackgroundWorkStart
        {
            get => _queue.BlockBackgroundWorkStart;
            set => _queue.BlockBackgroundWorkStart = value;
        }

        public ManualResetEventSlim? NotifyBackgroundWorkStarting
        {
            get => _queue.NotifyBackgroundWorkStarting;
            set => _queue.NotifyBackgroundWorkStarting = value;
        }

        public ManualResetEventSlim? NotifyBackgroundCapturedWorkload
        {
            get => _queue.NotifyBackgroundCapturedWorkload;
            set => _queue.NotifyBackgroundCapturedWorkload = value;
        }

        public ManualResetEventSlim? BlockBackgroundWorkCompleting
        {
            get => _queue.BlockBackgroundWorkCompleting;
            set => _queue.BlockBackgroundWorkCompleting = value;
        }

        public ManualResetEventSlim? NotifyBackgroundWorkCompleted
        {
            get => _queue.NotifyBackgroundWorkCompleted;
            set => _queue.NotifyBackgroundWorkCompleted = value;
        }

        public ManualResetEventSlim? NotifyErrorBeingReported
        {
            get => _queue.NotifyErrorBeingReported;
            set => _queue.NotifyErrorBeingReported = value;
        }

        public ManualResetEventSlim? NotifyBackgroundWorkCompleting
        {
            get => _queue.NotifyBackgroundWorkCompleting;
            set => _queue.NotifyBackgroundWorkCompleting = value;
        }
    }
}
