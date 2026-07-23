// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.Parallel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client;

/// <summary>
/// Manages work that is done on multiple managers (testhosts) in parallel, such as parallel discovery or parallel
/// run. It is a plain producer/consumer queue: <see cref="StartWork"/> enqueues all the workloads (a single source,
/// multiple sources, or a batch of test cases), and up to <see cref="MaxParallelLevel"/> managers pull from that
/// queue. When a manager finishes its workload the consumer calls <see cref="RunNextWork"/>, which frees the manager
/// and pulls the next workload until the queue is empty.
/// </summary>
/// <remarks>
/// <code>
///     workloads
///         |                StartWork enqueues every workload
///         v
///   +-------------------+
///   | _pendingWorkloads |  FIFO queue of work waiting for a free manager
///   +-------------------+
///         |                RunWorkInParallel dequeues while a slot is free
///         v                (i.e. while _activeManagers holds fewer than MaxParallelLevel managers)
///   +-------+  +-------+  +-------+
///   | mgr 1 |  | mgr 2 |  | mgr 3 |   _activeManagers: at most MaxParallelLevel testhosts
///   +-------+  +-------+  +-------+
///       |          |          |
///       |          |          |      each manager runs one workload on its testhost over IPC;
///       v          v          v      from here on the work is fire-and-forget
///     - - - - - asynchronous - - - - -
///       |          |          |      when a testhost is done, its event handler calls
///       v          v          v      RunNextWork(manager) - that out-of-band call is the "done" signal
///   RunNextWork: remove the finished manager, then RunWorkInParallel pulls the next workload
/// </code>
///
/// Why is this hand-rolled instead of a bounded primitive (SemaphoreSlim, System.Threading.Channels, or TPL
/// Dataflow's ActionBlock with MaxDegreeOfParallelism)? All of those bound concurrency around an awaitable unit of
/// work: the primitive starts a Task and watches that Task complete to release a slot. Our unit of work is not
/// awaitable. A manager is a separate testhost process; it runs its workload over an IPC connection and reports
/// completion much later through an out-of-band event (HandlePartialRunComplete / HandlePartialDiscoveryComplete),
/// which is what calls RunNextWork. To adopt a bounded primitive we would have to bridge every workload to a
/// TaskCompletionSource, complete it from that event, and add the matching cancellation plumbing - which relocates
/// the complexity instead of removing it, and we would still need the queue and the active-manager list anyway.
///
/// We also keep the full active-manager list (not just a counter) because it is the cancellation surface: Abort /
/// Cancel / Close are broadcast to every in-flight manager through <see cref="DoActionOnAllManagers"/>, so we need
/// the live set of managers, not only how many there are.
/// </remarks>
internal sealed class ParallelOperationManager<TManager, TEventHandler, TWorkload> : IDisposable
{
    private readonly Func<TestRuntimeProviderInfo, TWorkload, TManager> _createNewManager;
    private readonly object _lock = new();

    // Workloads that are waiting to be picked up by a manager.
    private readonly Queue<ProviderSpecificWorkload<TWorkload>> _pendingWorkloads = new();

    // Managers that are currently processing a workload. There are never more than MaxParallelLevel of them.
    private readonly List<TManager> _activeManagers = new();

    private TEventHandler? _eventHandler;
    private Func<TEventHandler, TManager, TEventHandler>? _getEventHandler;
    private Action<TManager, TEventHandler, TWorkload>? _runWorkload;
    private bool _acceptMoreWork = true;

    public int MaxParallelLevel { get; }

    /// <summary>Number of managers that are currently running a workload. Exposed mainly for tests and diagnostics.</summary>
    public int OccupiedSlotCount => _activeManagers.Count;

    /// <summary>Number of managers that could still be started before reaching <see cref="MaxParallelLevel"/>. Exposed mainly for tests and diagnostics.</summary>
    public int AvailableSlotCount => MaxParallelLevel - _activeManagers.Count;

    /// <summary>
    /// Creates new instance of ParallelOperationManager.
    /// </summary>
    /// <param name="createNewManager">Creates a new manager that is responsible for running a single part of the overall workload.
    /// A manager is typically a testhost, and the part of workload is discovering or running a single test dll.</param>
    /// <param name="parallelLevel">Determines the maximum amount of parallel managers that can be active at the same time.</param>
    public ParallelOperationManager(Func<TestRuntimeProviderInfo, TWorkload, TManager> createNewManager, int parallelLevel)
    {
        _createNewManager = createNewManager;
        MaxParallelLevel = parallelLevel;
    }

    public void StartWork(
        List<ProviderSpecificWorkload<TWorkload>> workloads,
        TEventHandler eventHandler,
        Func<TEventHandler, TManager, TEventHandler> getEventHandler,
        Action<TManager, TEventHandler, TWorkload> runWorkload)
    {
        _ = workloads ?? throw new ArgumentNullException(nameof(workloads));
        _eventHandler = eventHandler ?? throw new ArgumentNullException(nameof(eventHandler));
        _getEventHandler = getEventHandler ?? throw new ArgumentNullException(nameof(getEventHandler));
        _runWorkload = runWorkload ?? throw new ArgumentNullException(nameof(runWorkload));

        EqtTrace.Verbose($"ParallelOperationManager.StartWork: Enqueuing {workloads.Count} workloads.");

        lock (_lock)
        {
            _acceptMoreWork = true;
            foreach (var workload in workloads)
            {
                _pendingWorkloads.Enqueue(workload);
            }
        }

        RunWorkInParallel();
    }

    /// <summary>
    /// Starts as many pending workloads as there are free manager slots (up to <see cref="MaxParallelLevel"/>).
    /// </summary>
    private void RunWorkInParallel()
    {
        if (_eventHandler == null)
            throw new InvalidOperationException($"{nameof(_eventHandler)} was not provided.");

        if (_getEventHandler == null)
            throw new InvalidOperationException($"{nameof(_getEventHandler)} was not provided.");

        if (_runWorkload == null)
            throw new InvalidOperationException($"{nameof(_runWorkload)} was not provided.");

        // Reserve the managers under the lock so the slots stay consistent, but start their work outside of the
        // lock. That way, if multiple completions come in at the same time, they only block each other while
        // reserving a slot, and not while actually starting their assigned work.
        List<(TManager Manager, TEventHandler EventHandler, TWorkload Work)> reserved = new();
        int activeCount;
        int pendingCount;
        lock (_lock)
        {
            // When HandlePartialDiscovery or HandlePartialRun are in progress and we call StopAllManagers, it is
            // possible that RunWorkInParallel is waiting on the lock. When it is finally allowed in, it should not
            // start any more work, because we already cancelled.
            if (!_acceptMoreWork)
            {
                EqtTrace.Verbose("ParallelOperationManager.RunWorkInParallel: We don't accept more work, doing nothing.");
                return;
            }

            while (_activeManagers.Count < MaxParallelLevel && _pendingWorkloads.Count > 0)
            {
                var workload = _pendingWorkloads.Dequeue();
                var manager = _createNewManager(workload.Provider, workload.Work);
                var eventHandler = _getEventHandler(_eventHandler, manager);
                _activeManagers.Add(manager);
                reserved.Add((manager, eventHandler, workload.Work));
            }

            activeCount = _activeManagers.Count;
            pendingCount = _pendingWorkloads.Count;
        }

        if (EqtTrace.IsVerboseEnabled)
        {
            EqtTrace.Verbose($"ParallelOperationManager.RunWorkInParallel: {activeCount} managers active (max {MaxParallelLevel}), {pendingCount} workloads still pending.");
        }

        foreach (var (manager, eventHandler, work) in reserved)
        {
            EqtTrace.Verbose("ParallelOperationManager.RunWorkInParallel: Starting work on a manager.");
            _runWorkload(manager, eventHandler, work);
        }
    }

    /// <summary>
    /// Frees the manager that just completed its workload and starts the next pending workload if there is any.
    /// </summary>
    public void RunNextWork(TManager completedManager)
    {
        ValidateArg.NotNull(completedManager, nameof(completedManager));

        lock (_lock)
        {
            var removed = TryRemoveActiveManager(completedManager);

            // When HandlePartialDiscovery or HandlePartialRun are in progress and we call StopAllManagers, it is
            // possible that we already cleared all managers while RunNextWork was waiting on the lock. In that case
            // it is okay that the manager is not found, because we already stopped all work.
            if (!removed && _acceptMoreWork)
            {
                throw new InvalidOperationException("The provided manager was not found among the active managers.");
            }
        }

        RunWorkInParallel();
    }

    // Called under _lock. Uses reference equality because each workload gets its own freshly created manager.
    private bool TryRemoveActiveManager(TManager manager)
    {
        for (int i = 0; i < _activeManagers.Count; i++)
        {
            if (ReferenceEquals(_activeManagers[i], manager))
            {
                _activeManagers.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    public void DoActionOnAllManagers(Action<TManager> action, bool doActionsInParallel = false)
    {
        EqtTrace.Verbose("ParallelOperationManager.DoActionOnAllManagers: Calling an action on all managers.");

        TManager[] managers;
        lock (_lock)
        {
            managers = _activeManagers.ToArray();
        }

        if (!doActionsInParallel)
        {
            foreach (var manager in managers)
            {
                DoManagerAction(() => action(manager));
            }

            return;
        }

        var actionTasks = new Task[managers.Length];
        for (int i = 0; i < managers.Length; i++)
        {
            // Read the array before firing the task - beware of closures.
            var manager = managers[i];
            actionTasks[i] = Task.Run(() => action(manager));
        }

        DoManagerAction(() => Task.WaitAll(actionTasks));
    }

    private static void DoManagerAction(Action action)
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            // Exception can occur if we are trying to cancel a test run on an executor where test run is not even fired
            // we can safely ignore that as user is just canceling the test run and we don't care about additional parallel executors
            // as we will be disposing them off soon anyway
            EqtTrace.Warning("ParallelOperationManager.DoManagerAction: Exception while invoking an action on Proxy Manager instance: {0}", ex);
        }
    }

    internal void StopAllManagers()
    {
        EqtTrace.Verbose("ParallelOperationManager.StopAllManagers: Stopping all managers and discarding pending workloads.");
        lock (_lock)
        {
            _acceptMoreWork = false;
            _pendingWorkloads.Clear();
            _activeManagers.Clear();
        }
    }

    public void Dispose()
    {
        EqtTrace.Verbose("ParallelOperationManager.Dispose: Disposing all managers.");
        StopAllManagers();
    }
}
