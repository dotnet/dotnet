// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
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
    private readonly Func<TestRuntimeProviderInfo, TManager> _createNewManager;

    /// <summary>
    /// Default number of Processes
    /// </summary>
    private TEventHandler? _eventHandler;
    private Func<TEventHandler, TManager, TEventHandler>? _getEventHandler;
    private Action<TManager, TEventHandler, TWorkload>? _runWorkload;
    private bool _acceptMoreWork;
    private readonly List<ProviderSpecificWorkload<TWorkload>> _workloads = new();
    private readonly List<Slot> _managerSlots = new();

    private readonly object _lock = new();

    public int MaxParallelLevel { get; }
    public int OccupiedSlotCount { get; private set; }
    public int AvailableSlotCount { get; private set; }

    /// <summary>
    /// Creates new instance of ParallelOperationManager.
    /// </summary>
    /// <param name="createNewManager">Creates a new manager that is responsible for running a single part of the overall workload.
    /// A manager is typically a testhost, and the part of workload is discovering or running a single test dll.</param>
    /// <param name="parallelLevel">Determines the maximum amount of parallel managers that can be active at the same time.</param>
    public ParallelOperationManager(Func<TestRuntimeProviderInfo, TManager> createNewManager, int parallelLevel)
    {
        _createNewManager = createNewManager;
        MaxParallelLevel = parallelLevel;
        ClearSlots(acceptMoreWork: true);
    }

    private void ClearSlots(bool acceptMoreWork)
    {
        lock (_lock)
        {
            _acceptMoreWork = acceptMoreWork;
            _managerSlots.Clear();
            _managerSlots.AddRange(Enumerable.Range(0, MaxParallelLevel).Select(_ => new Slot()));
            SetOccupiedSlotCount();
        }
    }

    private void SetOccupiedSlotCount()
    {
        AvailableSlotCount = _managerSlots.Count(s => s.IsAvailable);
        OccupiedSlotCount = _managerSlots.Count - AvailableSlotCount;
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

        _workloads.AddRange(workloads);

        // This creates as many slots as possible even though we might not use them when we get less workloads to process,
        // this is not a big issue, and not worth optimizing, because the parallel level is determined by the logical CPU count,
        // so it is a small number.
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

        // Reserve slots and assign them work under the lock so we keep
        // the slots consistent.
        List<SlotWorkloadPair> workToRun = new();
        lock (_lock)
        {
            if (_workloads.Count == 0)
                return false;

            // When HandlePartialDiscovery or HandlePartialRun are in progress, and we call StopAllManagers,
            // it is possible that we will clear all slots, and have RunWorkInParallel waiting on the lock,
            // so when it is allowed to enter it will try to add more work, but we already cancelled,
            // so we should not start more work.
            if (!_acceptMoreWork)
                return false;

            var availableSlots = _managerSlots.Where(slot => slot.IsAvailable).ToList();
            var availableWorkloads = _workloads.Where(workload => workload != null).ToList();
            var amount = Math.Min(availableSlots.Count, availableWorkloads.Count);
            var workloadsToRun = availableWorkloads.Take(amount).ToList();

            for (int i = 0; i < amount; i++)
            {
                var slot = availableSlots[i];
                slot.IsAvailable = false;
                var workload = workloadsToRun[i];
                workToRun.Add(new SlotWorkloadPair(slot, workload));
                _workloads.Remove(workload);
            }

            SetOccupiedSlotCount();

            foreach (var pair in workToRun)
            {
                var manager = _createNewManager(pair.Workload.Provider);
                var eventHandler = _getEventHandler(_eventHandler, manager);
                pair.Slot.EventHandler = eventHandler;
                pair.Slot.Manager = manager;
                pair.Slot.ManagerInfo = pair.Workload.Provider;
                pair.Slot.Work = pair.Workload.Work;
            }
        }

        // Kick of the work in parallel outside of the lock so if we have more requests to run
        // that come in at the same time we only block them from reserving the same slot at the same time
        // but not from starting their assigned work at the same time.
        foreach (var pair in workToRun)
        {
            try
            {
                _runWorkload(pair.Slot.Manager!, pair.Slot.EventHandler!, pair.Workload.Work!);
            }
            finally
            {
                // clean the slot or something, to make sure we don't keep it reserved.
            }
        }

        // Return true when we started more work. Or false, when there was nothing more to do.
        // This will propagate to handling of partial discovery or partial run.
        return workToRun.Count > 0;
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
            var completedSlot = _managerSlots.Where(s => ReferenceEquals(completedManager, s.Manager)).ToList();
            // When HandlePartialDiscovery or HandlePartialRun are in progress, and we call StopAllManagers,
            // it is possible that we will clear all slots, while ClearCompletedSlot is waiting on the lock,
            // so when it is allowed to enter it will fail to find the respective slot and fail. In this case it is
            // okay that the slot is not found, and we do nothing, because we already stopped all work and cleared the slots.
            if (completedSlot.Count == 0)
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

            if (completedSlot.Count > 1)
            {
                throw new InvalidOperationException("The provided manager was found in multiple slots.");
            }

            var slot = completedSlot[0];
            slot.IsAvailable = true;

            SetOccupiedSlotCount();
        }
    }

    public void DoActionOnAllManagers(Action<TManager> action, bool doActionsInParallel = false)
    {
        // We don't need to lock here, we just grab the current list of
        // slots that are occupied (have managers) and run action on each one of them.
        var managers = _managerSlots.Where(slot => !slot.IsAvailable).Select(slot => slot.Manager).ToList();
        int i = 0;
        var actionTasks = new Task[managers.Count];
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
        public bool IsAvailable { get; set; } = true;

        public TManager? Manager { get; set; }

        public TestRuntimeProviderInfo? ManagerInfo { get; set; }

        public TEventHandler? EventHandler { get; set; }

        public TWorkload? Work { get; set; }
    }

    private class SlotWorkloadPair
    {
        public SlotWorkloadPair(Slot slot, ProviderSpecificWorkload<TWorkload> workload)
        {
            Slot = slot;
            Workload = workload;
        }
        public Slot Slot { get; }
        public ProviderSpecificWorkload<TWorkload> Workload { get; }
    }
}
