// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.Parallel;

using FluentAssertions;

namespace TestPlatform.CrossPlatEngine.UnitTests.Client;

[TestClass]
public class ParallelOperationManagerTests
{
    [TestMethod]
    public void OperationManagerShouldRunOnlyMaximumParallelLevelOfWorkInParallelEvenWhenThereAreMoreWorkloads()
    {
        // Arrange
        Func<TestRuntimeProviderInfo, SampleWorkload, SampleManager> createNewManager = (_, _2) => new SampleManager();
        var maxParallelLevel = 3;
        var parallelOperationManager = new ParallelOperationManager<SampleManager, SampleHandler, SampleWorkload>(createNewManager, maxParallelLevel);

        // Create more workloads than our parallel level so we can observe that the maximum parallel level is reached but not more
        var workloads = Enumerable.Range(1, maxParallelLevel + 2)
            .Select(i => new ProviderSpecificWorkload<SampleWorkload>(new SampleWorkload { Id = i }, provider: null!))
            .ToList();
        var eventHandler = new SampleHandler();

        List<int> workerCounts = new();

        Func<SampleHandler, SampleManager, SampleHandler> getEventHandler = (handler, _) => handler;
        Action<SampleManager, SampleHandler, SampleWorkload, bool, Task?> runWorkload = (manager, _, _, _, _) =>
        {
            // Every time we run a workload check how many slots are occupied,
            // we should see 3 slots at max, because that is our max parallel level, we should NOT see 4 or more:
            // This is what the data should be:
            // - At the start we schedule as much work as we can, workloads 1, 2, 3
            //   are started and grab a slot.
            //   We only update the slot count after scheduling all the work up to the max parallel level,
            //   so when we reach this method, all the slots are already occupied, so for workloads 1, 2, 3 we record 3, 3, 3.
            // - Workload 1 finishes and leaves the slot, 4 starts and grabs a slot, 2, 3, 4 are now running we record 3.
            // - workload 2 finishes and leaves the slot, 5 starts and grabs a slot, 3, 4, 5 are now running we record 3.
            // - workload 2 finishes and leaves the slot, 5 starts and grabs a slot, 3, 4, 5 are now running we record 3.
            // - workload 3 finishes and leaves the slot, there is no more work to do so we don't grab any additional slot. Just 4, 5 are now running we record 2.
            // - workload 4 finishes and leaves the slot, there is no more work to do so we don't grab any additional slot. Just 5 is now running we record 1.

            workerCounts.Add(parallelOperationManager.OccupiedSlotCount);

            System.Threading.Thread.Sleep(100);

            // Tell the operation manager that we are done, and it should move to the next piece of work.
            // Normally the operation manager would get this notification via the handler because the work we do
            // is asynchronous, but here we know that we are already done so we just tell the operation manager directly
            // and pass on the current manager that is done.
            parallelOperationManager.RunNextWork(manager);
        };
        Func<SampleManager, SampleHandler, SampleWorkload, Task> initializeWorkload = (_, _, _) =>
            Task.Run(() => System.Threading.Thread.Sleep(100));

        // Act
        parallelOperationManager.StartWork(workloads, eventHandler, getEventHandler, initializeWorkload, runWorkload);

        // Assert
        workerCounts.Should().BeEquivalentTo(new[] { 3, 3, 3, 2, 1 });
    }

    [TestMethod]
    public void OperationManagerShouldCreateOnlyAsManyParallelWorkersAsThereAreWorkloadsWhenTheAmountOfWorkloadsIsSmallerThanMaxParallelLevel()
    {
        // Arrange
        Func<TestRuntimeProviderInfo, SampleWorkload, SampleManager> createNewManager = (_, _2) => new SampleManager();
        var maxParallelLevel = 10;
        var parallelOperationManager = new ParallelOperationManager<SampleManager, SampleHandler, SampleWorkload>(createNewManager, maxParallelLevel);

        // Create less workloads than our parallel level so we can observe that only as many slots are created as there are workloads.
        var workloads = Enumerable.Range(1, 2)
            .Select(i => new ProviderSpecificWorkload<SampleWorkload>(new SampleWorkload { Id = i }, provider: null!))
            .ToList();
        var eventHandler = new SampleHandler();

        List<int> workerCounts = new();

        Func<SampleHandler, SampleManager, SampleHandler> getEventHandler = (handler, _) => handler;
        Action<SampleManager, SampleHandler, SampleWorkload, bool, Task?> runWorkload = (manager, _, _, _, _) =>
        {
            // See comments in test above for explanation.
            workerCounts.Add(parallelOperationManager.OccupiedSlotCount);
            System.Threading.Thread.Sleep(100);

            parallelOperationManager.RunNextWork(manager);
        };
        Func<SampleManager, SampleHandler, SampleWorkload, Task> initializeWorkload = (_, _, _) =>
            Task.Run(() => System.Threading.Thread.Sleep(100));

        // Act
        parallelOperationManager.StartWork(workloads, eventHandler, getEventHandler, initializeWorkload, runWorkload);

        // Assert
        workerCounts.Should().BeEquivalentTo(new[] { 2, 1 });
    }


    [TestMethod]
    public void OperationManagerShouldCreateAsManyMaxParallelLevel()
    {
        // Arrange
        Func<TestRuntimeProviderInfo, SampleWorkload, SampleManager> createNewManager = (_, _2) => new SampleManager();
        var maxParallelLevel = 10;
        var parallelOperationManager = new ParallelOperationManager<SampleManager, SampleHandler, SampleWorkload>(createNewManager, maxParallelLevel);

        // Create less workloads than our parallel level so we can observe that only as many slots are created as there are workloads.
        var workloads = Enumerable.Range(1, 2)
            .Select(i => new ProviderSpecificWorkload<SampleWorkload>(new SampleWorkload { Id = i }, provider: null!))
            .ToList();
        var eventHandler = new SampleHandler();

        List<int> workerCounts = new();
        List<int> availableWorkerCounts = new();

        Func<SampleHandler, SampleManager, SampleHandler> getEventHandler = (handler, _) => handler;
        Action<SampleManager, SampleHandler, SampleWorkload, bool, Task?> runWorkload = (manager, _, _, _, _) =>
        {
            // See comments in test above for explanation.
            workerCounts.Add(parallelOperationManager.OccupiedSlotCount);
            availableWorkerCounts.Add(parallelOperationManager.AvailableSlotCount);
            System.Threading.Thread.Sleep(100);

            parallelOperationManager.RunNextWork(manager);
        };
        Func<SampleManager, SampleHandler, SampleWorkload, Task> initializeWorkload = (_, _, _) =>
            Task.Run(() => System.Threading.Thread.Sleep(100));

        // Act
        parallelOperationManager.StartWork(workloads, eventHandler, getEventHandler, initializeWorkload, runWorkload);

        // Assert
        workerCounts.Should().BeEquivalentTo(new[] { 2, 1 });
        // We create 10 slots, because that is the max parallel level, when we observe, there are 2 workloads running,
        // and then 1 workload running, so we see 8 and 9 (10 - 2, and 10 - 1).
        availableWorkerCounts.Should().BeEquivalentTo(new[] { 8, 9 });
    }

    [TestMethod]
    public void OperationManagerMovesToTheNextWorkloadOnlyWhenRunNextWorkIsCalled()
    {
        // Arrange
        Func<TestRuntimeProviderInfo, SampleWorkload, SampleManager> createNewManager = (_, _2) => new SampleManager();
        var maxParallelLevel = 2;
        var parallelOperationManager = new ParallelOperationManager<SampleManager, SampleHandler, SampleWorkload>(createNewManager, maxParallelLevel);

        // Create more workloads than our parallel level so we can observe that when one workload is finished, calling RunNextWork will move on
        // to the next workload.
        var workloads = Enumerable.Range(1, maxParallelLevel + 3)
            .Select(i => new ProviderSpecificWorkload<SampleWorkload>(new SampleWorkload { Id = i }, provider: null!))
            .ToList();
        var eventHandler = new SampleHandler();

        List<int> workloadsProcessed = new();

        Func<SampleHandler, SampleManager, SampleHandler> getEventHandler = (handler, _) => handler;
        Action<SampleManager, SampleHandler, SampleWorkload, bool, Task?> runWorkload = (manager, _, workload, _, _) =>
        {
            // See comments in test above for explanation.
            System.Threading.Thread.Sleep(100);

            workloadsProcessed.Add(workload.Id);
            // Only move to next when we run the first workload. Meaning we process 1, 2, and then 3, but not 4 and 5.
            if (workload.Id == 1)
            {
                parallelOperationManager.RunNextWork(manager);
            }
        };
        Func<SampleManager, SampleHandler, SampleWorkload, Task> initializeWorkload = (_, _, _) =>
            Task.Run(() => System.Threading.Thread.Sleep(100));

        // Act
        parallelOperationManager.StartWork(workloads, eventHandler, getEventHandler, initializeWorkload, runWorkload);

        // Assert
        // We start by scheduling 2 workloads (1 and 2) becuase that is the max parallel level.
        // Then we call next and go to 3. After that, we don't call next anymore which means we are done,
        // even though we did not process workloads 4 and 5.
        // (e.g. In real life Abort was called so the handler won't call RunNextWork, because we don't want to run the remaining sources,
        // and all the sources that are currently running be aborted by calling Abort on each manager via DoActionOnAllManagers.)
        workloadsProcessed.Should().BeEquivalentTo(new[] { 1, 2, 3 });
    }

    [TestMethod]
    public void OperationManagerRunsAnOperationOnAllActiveManagersWhenDoActionOnAllManagersIsCalled()
    {
        // Arrange
        var createdManagers = new List<SampleManager>();
        // Store the managers we created so we can inspect them later and see if Abort was called on them.
        Func<TestRuntimeProviderInfo, SampleWorkload, SampleManager> createNewManager = (_, _2) =>
        {
            var manager = new SampleManager();
            createdManagers.Add(manager);
            return manager;
        };

        var maxParallelLevel = 2;
        // Create more workloads than the parallel level so we can go past max parallel level of active workers and simulate that we
        // are aborting in the middle of a run.
        var workloads = Enumerable.Range(1, maxParallelLevel + 3)
            .Select(i => new ProviderSpecificWorkload<SampleWorkload>(new SampleWorkload { Id = i }, provider: null!))
            .ToList();

        var parallelOperationManager = new ParallelOperationManager<SampleManager, SampleHandler, SampleWorkload>(createNewManager, maxParallelLevel);
        var eventHandler = new SampleHandler();

        Func<SampleHandler, SampleManager, SampleHandler> getEventHandler = (handler, _) => handler;
        Action<SampleManager, SampleHandler, SampleWorkload, bool, Task?> runWorkload = (manager, _, workload, _, _) =>
        {
            // See comments in test above for explanation.

            // Make workload 1 fast, we want to put this in state where 2 and 3 are running and we call abort on them.
            if (workload.Id != 1)
            {
                System.Threading.Thread.Sleep(100);
            }

            // Only move to next when we run the first workload. Meaning we process 1, 2, and then 3, but not 4 and 5.
            if (workload.Id == 1)
            {
                parallelOperationManager.RunNextWork(manager);
            }
        };
        Func<SampleManager, SampleHandler, SampleWorkload, Task> initializeWorkload = (_, _, _) =>
            Task.Run(() => System.Threading.Thread.Sleep(100));

        // Start the work, so we process workload 1 and then move to 2.
        parallelOperationManager.StartWork(workloads, eventHandler, getEventHandler, initializeWorkload, runWorkload);

        // Act
        parallelOperationManager.DoActionOnAllManagers(manager => manager.Abort(), doActionsInParallel: true);

        // Assert
        // When we aborted workload 1 was already processed, and 2, and 3 were active.
        // We should see that the first manager did not call abort, but second and third called abort,
        // and there were no more managers created because we stopped calling next after 1 was done.
        createdManagers.Select(manager => manager.AbortCalled).Should().BeEquivalentTo(new[] { false, true, true });
    }

    /// <summary>
    /// Represents a manager that is responsible for processing a single given workload. Normally this would be a testhost.
    /// </summary>
    private class SampleManager
    {
        public bool AbortCalled { get; private set; }

        public void Abort()
        {
            AbortCalled = true;
        }
    }

    /// <summary>
    /// Represents a handler, in our tests it does nothing, because we are not running any "async" work
    /// so we don't need a handler to call us back when processing one workload is done and we can progress to next
    /// workload.
    /// </summary>
    private class SampleHandler
    {

    }

    // Represents a workload, normally this would be a test dll, or a collection of testcases from a single dll that
    // are supposed to run on 1 testhost.
    private class SampleWorkload
    {
        public int Id { get; set; }
    }
}
