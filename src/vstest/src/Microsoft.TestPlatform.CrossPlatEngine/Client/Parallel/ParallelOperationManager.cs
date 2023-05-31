// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.Parallel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client;

/// <summary>
/// Manages work that is done on multiple managers (testhosts) in parallel such as parallel discovery or parallel run.
/// </summary>
internal sealed class ParallelOperationManager<TManager, TEventHandler, TWorkload> : IDisposable
{
    private const int PreStart = 2;
    private readonly static int VSTEST_HOSTPRESTART_COUNT =
        int.TryParse(
                Environment.GetEnvironmentVariable(nameof(VSTEST_HOSTPRESTART_COUNT)),
                out int num)
        ? num
        : PreStart;
    private readonly Func<TestRuntimeProviderInfo, TWorkload, TManager> _createNewManager;

    /// <summary>
    /// Default number of Processes
    /// </summary>
    private TEventHandler? _eventHandler;
    private Func<TEventHandler, TManager, TEventHandler>? _getEventHandler;
    private Func<TManager, TEventHandler, TWorkload, Task>? _initializeWorkload;
    private Action<TManager, TEventHandler, TWorkload, bool, Task?>? _runWorkload;
    private bool _acceptMoreWork;
    private readonly List<ProviderSpecificWorkload<TWorkload>> _workloads = new();
    private readonly List<Slot> _managerSlots = new();

    private readonly object _lock = new();

    public int MaxParallelLevel { get; }
    public int OccupiedSlotCount { get; private set; }
    public int AvailableSlotCount { get; private set; }
    public int PreStartCount { get; private set; }

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
        // pre-start only when we don't run in parallel, if we do run in parallel,
        // then pre-starting has no additional value because while one host is starting,
        // another is running tests.
        PreStartCount = MaxParallelLevel == 1 ? VSTEST_HOSTPRESTART_COUNT : 0;
        ClearSlots(acceptMoreWork: true);
    }

    private void ClearSlots(bool acceptMoreWork)
    {
        lock (_lock)
        {
            _acceptMoreWork = acceptMoreWork;
            _managerSlots.Clear();
            _managerSlots.AddRange(Enumerable.Range(0, MaxParallelLevel + PreStartCount).Select(i => new Slot { Index = i }));
            SetOccupiedSlotCount();
        }
    }

    private void SetOccupiedSlotCount()
    {
        AvailableSlotCount = _managerSlots.Count(s => !s.HasWork);
        OccupiedSlotCount = _managerSlots.Count - AvailableSlotCount;
    }

    public void StartWork(
        List<ProviderSpecificWorkload<TWorkload>> workloads,
        TEventHandler eventHandler,
        Func<TEventHandler, TManager, TEventHandler> getEventHandler,
        Func<TManager, TEventHandler, TWorkload, Task> initializeWorkload,
        Action<TManager, TEventHandler, TWorkload, bool, Task?> runWorkload)
    {
        _ = workloads ?? throw new ArgumentNullException(nameof(workloads));
        _eventHandler = eventHandler ?? throw new ArgumentNullException(nameof(eventHandler));
        _getEventHandler = getEventHandler ?? throw new ArgumentNullException(nameof(getEventHandler));
        _initializeWorkload = initializeWorkload ?? throw new ArgumentNullException(nameof(initializeWorkload));
        _runWorkload = runWorkload ?? throw new ArgumentNullException(nameof(runWorkload));

        _workloads.AddRange(workloads);

        ClearSlots(acceptMoreWork: true);
        RunWorkInParallel();
    }

    // This does not do anything in parallel, all the workloads we schedule are offloaded to separate Task in the _runWorkload callback.
    // I did not want to change that, yet but this is the correct place to do that offloading. Not each manager.
    private bool RunWorkInParallel()
    {
        // TODO: Right now we don't re-use shared hosts, but if we did, this is the place
        // where we should find a workload that fits the manager if any of them is shared.
        // Or tear it down, and start a new one.

        if (_eventHandler == null)
            throw new InvalidOperationException($"{nameof(_eventHandler)} was not provided.");

        if (_getEventHandler == null)
            throw new InvalidOperationException($"{nameof(_getEventHandler)} was not provided.");

        if (_runWorkload == null)
            throw new InvalidOperationException($"{nameof(_runWorkload)} was not provided.");

        // Reserve slots and assign them work under the lock so we keep the slots consistent.
        Slot[] slots;
        lock (_lock)
        {
            // When HandlePartialDiscovery or HandlePartialRun are in progress, and we call StopAllManagers,
            // it is possible that we will clear all slots, and have RunWorkInParallel waiting on the lock,
            // so when it is allowed to enter it will try to add more work, but we already cancelled,
            // so we should not start more work.
            if (!_acceptMoreWork)
                return false;

            // We grab all empty slots.
            var availableSlots = _managerSlots.Where(slot => !slot.HasWork).ToImmutableArray();
            var occupiedSlots = MaxParallelLevel - (availableSlots.Length - PreStartCount);
            // We grab all available workloads.
            var availableWorkloads = _workloads.Where(workload => workload != null).ToImmutableArray();
            // We take the amount of workloads to fill all the slots, or just as many workloads
            // as there are if there are less workloads than slots.
            var amount = Math.Min(availableSlots.Length, availableWorkloads.Length);
            var workloadsToAdd = availableWorkloads.Take(amount).ToImmutableArray();

            // We associate each workload to a slot, if we reached the max parallel
            // level, then we will run only initalize step of the given workload.
            for (int i = 0; i < amount; i++)
            {
                var slot = availableSlots[i];
                slot.HasWork = true;
                var workload = workloadsToAdd[i];
                slot.ShouldPreStart = occupiedSlots + i + 1 > MaxParallelLevel;

                var manager = _createNewManager(workload.Provider, workload.Work);
                var eventHandler = _getEventHandler(_eventHandler, manager);
                slot.EventHandler = eventHandler;
                slot.Manager = manager;
                slot.ManagerInfo = workload.Provider;
                slot.Work = workload.Work;

                _workloads.Remove(workload);
            }

            slots = _managerSlots.ToArray();
            SetOccupiedSlotCount();
        }

        // Kick of the work in parallel outside of the lock so if we have more requests to run
        // that come in at the same time we only block them from reserving the same slot at the same time
        // but not from starting their assigned work at the same time.

        // Kick of all pre-started hosts from the ones that had the longest time to initialize.
        //
        // This code should be safe even outside the lock since HasWork is only changed when work is
        // complete and only for the slot that completed work. It is not possible to complete work before
        // starting it (which is what we are trying to do here).
        var startedWork = 0;
        foreach (var slot in slots.Where(s => s.HasWork && !s.IsRunning && s.IsPreStarted).OrderBy(s => s.PreStartTime))
        {
            startedWork++;
            slot.IsRunning = true;
            EqtTrace.Verbose($"ParallelOperationManager.RunWorkInParallel: Running on pre-started host: {(DateTime.Now.TimeOfDay - slot.PreStartTime).TotalMilliseconds}ms {slot.InitTask?.Status}");
            _runWorkload(slot.Manager!, slot.EventHandler!, slot.Work!, slot.IsPreStarted, slot.InitTask);

            // We already started as many as we were allowed, jump out;
            if (startedWork == MaxParallelLevel)
            {
                break;
            }
        }

        // We already started as many pre-started testhosts as we are allowed by the max parallel level
        // skip running more work.
        if (startedWork < MaxParallelLevel)
        {
            foreach (var slot in slots)
            {
                if (slot.HasWork && !slot.IsRunning)
                {
                    if (!slot.ShouldPreStart)
                    {
                        startedWork++;
                        slot.IsRunning = true;
                        EqtTrace.Verbose("ParallelOperationManager.RunWorkInParallel: Started work on a host.");
                        _runWorkload(slot.Manager!, slot.EventHandler!, slot.Work!, slot.IsPreStarted, slot.InitTask);
                    }
                }

                // We already started as many as we were allowed, jump out;
                if (startedWork == MaxParallelLevel)
                {
                    break;
                }
            }
        }

        var preStartedWork = 0;
        foreach (var slot in slots)
        {
            if (slot.HasWork && slot.ShouldPreStart && !slot.IsPreStarted)
            {
                preStartedWork++;
                slot.PreStartTime = DateTime.Now.TimeOfDay;
                slot.IsPreStarted = true;
                EqtTrace.Verbose("ParallelOperationManager.RunWorkInParallel: Pre-starting a host.");
                slot.InitTask = _initializeWorkload!(slot.Manager!, slot.EventHandler!, slot.Work!);
            }
        }

        // Return true when we started more work. Or false, when there was nothing more to do.
        // This will propagate to handling of partial discovery or partial run.
        return preStartedWork + startedWork > 0;
    }

    public bool RunNextWork(TManager completedManager)
    {
        ValidateArg.NotNull(completedManager, nameof(completedManager));
        ClearCompletedSlot(completedManager);
        return RunWorkInParallel();
    }

    private void ClearCompletedSlot(TManager completedManager)
    {
        lock (_lock)
        {
            var completedSlot = _managerSlots.Where(s => ReferenceEquals(completedManager, s.Manager)).ToImmutableArray();
            // When HandlePartialDiscovery or HandlePartialRun are in progress, and we call StopAllManagers,
            // it is possible that we will clear all slots, while ClearCompletedSlot is waiting on the lock,
            // so when it is allowed to enter it will fail to find the respective slot and fail. In this case it is
            // okay that the slot is not found, and we do nothing, because we already stopped all work and cleared the slots.
            if (completedSlot.Length == 0)
            {
                if (_acceptMoreWork)
                {
                    throw new InvalidOperationException("The provided manager was not found in any slot.");
                }
                else
                {
                    return;
                }
            }

            if (completedSlot.Length > 1)
            {
                throw new InvalidOperationException("The provided manager was found in multiple slots.");
            }

            var slot = completedSlot[0];
            slot.PreStartTime = TimeSpan.Zero;
            slot.Work = default(TWorkload);
            slot.HasWork = false;
            slot.ShouldPreStart = false;
            slot.IsPreStarted = false;
            slot.InitTask = null;
            slot.IsRunning = false;
            slot.Manager = default(TManager);
            slot.EventHandler = default(TEventHandler);

            SetOccupiedSlotCount();
        }
    }

    public void DoActionOnAllManagers(Action<TManager> action, bool doActionsInParallel = false)
    {
        // We don't need to lock here, we just grab the current list of
        // slots that are occupied (have managers) and run action on each one of them.
        var managers = _managerSlots.Where(slot => slot.HasWork).Select(slot => slot.Manager).ToImmutableArray();
        int i = 0;
        var actionTasks = new Task[managers.Length];
        foreach (var manager in managers)
        {
            if (manager == null)
                continue;

            // Read the array before firing the task - beware of closures
            if (doActionsInParallel)
            {
                actionTasks[i] = Task.Run(() => action(manager));
                i++;
            }
            else
            {
                DoManagerAction(() => action(manager));
            }
        }

        if (doActionsInParallel)
        {
            DoManagerAction(() => Task.WaitAll(actionTasks));
        }
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
        ClearSlots(acceptMoreWork: false);
    }

    public void Dispose()
    {
        ClearSlots(acceptMoreWork: false);
    }

    private class Slot
    {
        public int Index { get; set; }
        public bool HasWork { get; set; }

        public bool ShouldPreStart { get; set; }

        public Task? InitTask { get; set; }

        public bool IsRunning { get; set; }

        public TManager? Manager { get; set; }

        public TestRuntimeProviderInfo? ManagerInfo { get; set; }

        public TEventHandler? EventHandler { get; set; }

        public TWorkload? Work { get; set; }
        public bool IsPreStarted { get; internal set; }
        public TimeSpan PreStartTime { get; internal set; }

        public override string ToString()
        {
            return $"{Index}: HasWork: {HasWork}, ShouldPreStart: {ShouldPreStart}, IsPreStarted: {IsPreStarted},  IsRunning: {IsRunning}";
        }
    }
}
