// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;

using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPlatform.CoreUtilities.UnitTests;

[TestClass]
public class JobQueueTests
{
    [TestMethod]
    public void ConstructorThrowsWhenNullProcessHandlerIsProvided()
    {
        JobQueue<string>? jobQueue = null;
        Assert.ThrowsException<ArgumentNullException>(() => jobQueue = new JobQueue<string>(null!, "dp", int.MaxValue, int.MaxValue, false, (message) => { }));

        if (jobQueue != null)
        {
            jobQueue.Dispose();
        }
    }

    [TestMethod]
    public void ThrowsWhenNullEmptyOrWhiteSpaceDisplayNameIsProvided()
    {
        JobQueue<string>? jobQueue = null;
        Assert.ThrowsException<ArgumentException>(() => jobQueue = new JobQueue<string>(GetEmptyProcessHandler<string>(), null!, int.MaxValue, int.MaxValue, false, (message) => { }));
        Assert.ThrowsException<ArgumentException>(() => jobQueue = new JobQueue<string>(GetEmptyProcessHandler<string>(), "", int.MaxValue, int.MaxValue, false, (message) => { }));
        Assert.ThrowsException<ArgumentException>(() => jobQueue = new JobQueue<string>(GetEmptyProcessHandler<string>(), "    ", int.MaxValue, int.MaxValue, false, (message) => { }));

        if (jobQueue != null)
        {
            jobQueue.Dispose();
        }
    }

    [TestMethod]
    public void JobsCanBeAddedToTheQueueAndAreProcessedInTheOrderReceived()
    {
        // Setup the job process handler to keep track of the jobs.
        var jobsProcessed = new List<int>();
        Action<int> processHandler = (job) => jobsProcessed.Add(job);

        // Setup Test Data.
        var job1 = 1;
        var job2 = 2;
        var job3 = 3;

        // Queue the jobs and verify they are processed in the order added.
        using (var queue = new JobQueue<int>(processHandler, "dp", int.MaxValue, int.MaxValue, false, (message) => { }))
        {
            queue.QueueJob(job1, 0);
            queue.QueueJob(job2, 0);
            queue.QueueJob(job3, 0);
        }

        Assert.AreEqual(job1, jobsProcessed[0]);
        Assert.AreEqual(job2, jobsProcessed[1]);
        Assert.AreEqual(job3, jobsProcessed[2]);
    }

    [TestMethod]
    public void JobsAreProcessedOnABackgroundThread()
    {
        // Setup the job process handler to keep track of the jobs.
        var jobsProcessed = new List<int>();
        Action<string?> processHandler = job => jobsProcessed.Add(Environment.CurrentManagedThreadId);

        // Queue the jobs and verify they are processed on a background thread.
        using (var queue = new JobQueue<string>(processHandler, "dp", int.MaxValue, int.MaxValue, false, (message) => { }))
        {
            queue.QueueJob("dp", 0);
        }

        Assert.AreNotEqual(Environment.CurrentManagedThreadId, jobsProcessed[0]);
    }

    [TestMethod]
    public void ThrowsWhenQueuingAfterDisposed()
    {
        var queue = new JobQueue<string>(GetEmptyProcessHandler<string>(), "dp", int.MaxValue, int.MaxValue, false, (message) => { });
        queue.Dispose();

        Assert.ThrowsException<ObjectDisposedException>(() => queue.QueueJob("dp", 0));
    }

    [TestMethod]
    public void ThrowsWhenResumingAfterDisposed()
    {
        var queue = new JobQueue<string>(GetEmptyProcessHandler<string>(), "dp", int.MaxValue, int.MaxValue, false, (message) => { });
        queue.Dispose();

        Assert.ThrowsException<ObjectDisposedException>(() => queue.Resume());
    }

    [TestMethod]
    public void ThrowsWhenPausingAfterDisposed()
    {
        var queue = new JobQueue<string>(GetEmptyProcessHandler<string>(), "dp", int.MaxValue, int.MaxValue, false, (message) => { });
        queue.Dispose();

        Assert.ThrowsException<ObjectDisposedException>(() => queue.Pause());
    }

    [TestMethod]
    public void ThrowsWhenFlushingAfterDisposed()
    {
        var queue = new JobQueue<string>(GetEmptyProcessHandler<string>(), "dp", int.MaxValue, int.MaxValue, false, (message) => { });
        queue.Dispose();

        Assert.ThrowsException<ObjectDisposedException>(() => queue.Flush());
    }

    [TestMethod]
    public void DisposeDoesNotThrowWhenCalledTwice()
    {
        var queue = new JobQueue<string>(GetEmptyProcessHandler<string>(), "dp", int.MaxValue, int.MaxValue, false, (message) => { });
        queue.Dispose();
        queue.Dispose();
    }

    [TestMethod]
    public void OncePausedNoFurtherJobsAreProcessedUntilResumeIsCalled()
    {
        // Setup the job process handler to keep track of the jobs it is called with.
        List<string?> processedJobs = new();
        Action<string?> processHandler = job => processedJobs.Add(job);

        // Queue the jobs after paused and verify they are not processed until resumed.
        using (var queue = new JobQueue<string>(processHandler, "dp", int.MaxValue, int.MaxValue, false, (message) => { }))
        {
            queue.Pause();
            queue.QueueJob("dp", 0);
            queue.QueueJob("dp", 0);
            queue.QueueJob("dp", 0);

            // Allow other threads to execute and verify no jobs processed because the queue is paused.
            Thread.Sleep(0);
            Assert.AreEqual(0, processedJobs.Count);

            queue.Resume();
        }

        Assert.AreEqual(3, processedJobs.Count);
    }

    [TestMethod]
    public void ThrowsWhenBeingDisposedWhileQueueIsPaused()
    {
        using var queue = new JobQueue<string>(GetEmptyProcessHandler<string>(), "dp", int.MaxValue, int.MaxValue, false, (message) => { });
        queue.Pause();

        Assert.ThrowsException<InvalidOperationException>(() => queue.Dispose());

        queue.Resume();
    }

    [TestMethod]
    public void FlushMethodWaitsForAllJobsToBeProcessedBeforeReturning()
    {
        // Setup the job process handler to keep track of the jobs it has processed.
        var jobsProcessed = 0;
        Action<string?> processHandler = job => jobsProcessed++;

        // Queue several jobs and verify they have been processed when wait returns.
        using var queue = new JobQueue<string>(processHandler, "dp", int.MaxValue, int.MaxValue, false, (message) => { });
        queue.QueueJob("dp", 0);
        queue.QueueJob("dp", 0);
        queue.QueueJob("dp", 0);

        queue.Flush();

        Assert.AreEqual(3, jobsProcessed);
    }

    [TestMethod]
    public void TestBlockAtEnqueueDueToLength()
    {
        ManualResetEvent allowJobProcessingHandlerToProceed = new(false);
        AutoResetEvent jobProcessed = new(false);

        // process handler for the jobs in queue. It blocks on a job till the queue gets full and the handler sets the
        // event allowHandlerToProceed.
        Action<string?> processHandler = job =>
        {
            allowJobProcessingHandlerToProceed.WaitOne();
            if (string.Equals(job, "job11", StringComparison.OrdinalIgnoreCase))
            {
                jobProcessed.Set();
            }
        };

        using JobQueueWrapper queue = new(processHandler, 5, int.MaxValue, true, allowJobProcessingHandlerToProceed);
        // run the same thing multiple times to ensure that the queue isn't in a erroneous state after being blocked.
        for (int i = 0; i < 10; i++)
        {
            queue.QueueJob("job1", 0);
            queue.QueueJob("job2", 0);
            queue.QueueJob("job3", 0);
            queue.QueueJob("job4", 0);
            queue.QueueJob("job5", 0);

            // At this point only 5 jobs have been queued. Even if all are still in queue, still the need to block shouldn't have
            // risen. So queue.enteredBlockingMethod would be false.
            Assert.IsFalse(queue.IsEnqueueBlocked, "Entered the over-ridden blocking method at a wrong time.");

            queue.QueueJob("job6", 0);
            queue.QueueJob("job7", 0);
            queue.QueueJob("job8", 0);
            queue.QueueJob("job9", 0);
            queue.QueueJob("job10", 0);
            queue.QueueJob("job11", 0);

            // By this point surely the queue would have blocked at least once, hence setting queue.enteredBlockingMethod true.
            Assert.IsTrue(queue.IsEnqueueBlocked, "Did not enter the over-ridden blocking method");

            // We wait till all jobs are finished, so that for the next iteration the queue is in a deterministic state.
            jobProcessed.WaitOne();

            // queue.enteredBlockingMethod is set to false to check it again in next iteration. Also
            // allowJobProcessingHandlerToProceed is reset to block the handler again in next iteration.
            queue.IsEnqueueBlocked = false;
            allowJobProcessingHandlerToProceed.Reset();

            // if we reach here it means that the queue was successfully blocked at some point in between job6 and job11
            // and subsequently unblocked.
        }
    }

    [TestMethod]
    public void TestBlockAtEnqueueDueToSize()
    {
        ManualResetEvent allowJobProcessingHandlerToProceed = new(false);
        AutoResetEvent jobProcessed = new(false);

        // process handler for the jobs in queue. It blocks on a job till the queue gets full and the handler sets the
        // event allowHandlerToProceed.
        Action<string?> processHandler = job =>
        {
            allowJobProcessingHandlerToProceed.WaitOne();
            if (string.Equals(job, "job11", StringComparison.OrdinalIgnoreCase))
            {
                jobProcessed.Set();
            }
        };

        using JobQueueWrapper queue = new(processHandler, int.MaxValue, 40, true, allowJobProcessingHandlerToProceed);
        // run the same thing multiple times to ensure that the queue isn't in a erroneous state after being blocked.
        for (int i = 0; i < 10; i++)
        {
            queue.QueueJob("job1", 8);
            queue.QueueJob("job2", 8);
            queue.QueueJob("job3", 8);
            queue.QueueJob("job4", 8);
            queue.QueueJob("job5", 8);

            // At this point exactly 80 bytes have been queued. Even if all are still in queue, still the need to block shouldn't
            // have risen. So queue.enteredBlockingMethod would be false.
            Assert.IsFalse(queue.IsEnqueueBlocked, "Entered the over-ridden blocking method at a wrong time.");

            queue.QueueJob("job6", 8);
            queue.QueueJob("job7", 8);
            queue.QueueJob("job8", 8);
            queue.QueueJob("job9", 8);
            queue.QueueJob("job10", 10);
            queue.QueueJob("job11", 10);

            // By this point surely the queue would have blocked at least once, hence setting queue.enteredBlockingMethod true.
            Assert.IsTrue(queue.IsEnqueueBlocked, "Did not enter the over-ridden blocking method");

            // We wait till all jobs are finished, so that for the next iteration the queue is in a deterministic state.
            jobProcessed.WaitOne();

            // queue.enteredBlockingMethod is set to false to check it again in next iteration. Also
            // allowJobProcessingHandlerToProceed is reset to block the handler again in next iteration.
            queue.IsEnqueueBlocked = false;
            allowJobProcessingHandlerToProceed.Reset();

            // if we reach here it means that the queue was successfully blocked at some point in between job6 and job11
            // and subsequently unblocked.
        }
    }

    [TestMethod]
    public void TestBlockingDisabled()
    {
        ManualResetEvent allowJobProcessingHandlerToProceed = new(false);
        AutoResetEvent jobProcessed = new(false);

        // process handler for the jobs in queue. It blocks on a job till the test method sets the
        // event allowHandlerToProceed.
        Action<string?> processHandler = job =>
        {
            allowJobProcessingHandlerToProceed.WaitOne();
            if (string.Equals(job, "job5", StringComparison.OrdinalIgnoreCase))
            {
                jobProcessed.Set();
            }
        };

        using JobQueueWrapper queue = new(processHandler, 2, int.MaxValue, false, allowJobProcessingHandlerToProceed);
        // run the same thing multiple times to ensure that the queue isn't in a erroneous state after first run.
        for (int i = 0; i < 10; i++)
        {
            queue.QueueJob("job1", 0);
            queue.QueueJob("job2", 0);

            // At this point only 2 jobs have been queued. Even if all are still in queue, still the need to block shouldn't have
            // risen. So queue.enteredBlockingMethod would be false regardless of the blocking disabled or not.
            Assert.IsFalse(queue.IsEnqueueBlocked, "Entered the over-ridden blocking method at a wrong time.");

            queue.QueueJob("job3", 0);
            queue.QueueJob("job4", 0);
            queue.QueueJob("job5", 0);

            // queue.enteredBlockingMethod should still be false as the queue should not have blocked.
            Assert.IsFalse(queue.IsEnqueueBlocked, "Entered the over-ridden blocking method though blocking is disabled.");

            // allow handlers to proceed.
            allowJobProcessingHandlerToProceed.Set();

            // We wait till all jobs are finished, so that for the next iteration the queue is in a deterministic state.
            jobProcessed.WaitOne();

            // queue.enteredBlockingMethod is set to false to check it again in next iteration. Also
            // allowJobProcessingHandlerToProceed is reset to allow blocking the handler again in next iteration.
            queue.IsEnqueueBlocked = false;
            allowJobProcessingHandlerToProceed.Reset();

            // if we reach here it means that the queue was never blocked.
        }
    }

    [TestMethod]
    public void TestLargeTestResultCanBeLoadedWithBlockingEnabled()
    {
        var jobProcessed = new AutoResetEvent(false);

        // process handler for the jobs in queue.
        Action<string?> processHandler = (job) => jobProcessed.Set();

        using JobQueueNonBlocking queue = new(processHandler);
        // run the same thing multiple times to ensure that the queue isn't in a erroneous state after first run.
        for (var i = 0; i < 10; i++)
        {
            // we try to enqueue a job of size greater than bound on the queue. It should be queued without blocking as
            // we check whether or not the queue size has exceeded the limit before actually queuing.
            queue.QueueJob("job1", 8);

            // if queue.EnteredBlockingMethod is true, the enqueuing entered the overridden blocking method. This was not
            // intended.
            Assert.IsFalse(queue.EnteredBlockingMethod, "Entered the over-ridden blocking method.");
            jobProcessed.WaitOne();
        }
    }

    [TestMethod]
    [Timeout(60000)]
    public void TestDisposeUnblocksBlockedThreads()
    {
        var allowJobProcessingHandlerToProceed = new ManualResetEvent(false);

        using var gotBlocked = new ManualResetEvent(false);
        var job1Running = new ManualResetEvent(false);

        // process handler for the jobs in queue. It blocks on a job till the test method sets the
        // event allowHandlerToProceed.
        Action<string?> processHandler = job =>
        {
            if (string.Equals(job, "job1", StringComparison.OrdinalIgnoreCase))
                job1Running.Set();

            allowJobProcessingHandlerToProceed.WaitOne();
        };

        var jobQueue = new JobQueueWrapper(processHandler, 1, int.MaxValue, true, gotBlocked);

        var queueThread = new Thread(
            source =>
            {
                jobQueue.QueueJob("job1", 0);
                job1Running.WaitOne();
                jobQueue.QueueJob("job2", 0);
                jobQueue.QueueJob("job3", 0);
                allowJobProcessingHandlerToProceed.Set();
            });
        queueThread.Start();

        gotBlocked.WaitOne();
        jobQueue.Dispose();
        queueThread.Join();
    }

    #region Implementation

    /// <summary>
    /// a class that inherits from job queue and over rides the WaitForQueueToEmpty to allow for checking that blocking and
    /// unblocking work as expected.
    /// </summary>
    internal class JobQueueWrapper : JobQueue<string>
    {
        public JobQueueWrapper(Action<string?> processJob,
            int maxNoOfStringsQueueCanHold,
            int maxNoOfBytesQueueCanHold,
            bool isBoundsEnabled,
            ManualResetEvent queueGotBlocked)
            : base(processJob, "foo", maxNoOfStringsQueueCanHold, maxNoOfBytesQueueCanHold, isBoundsEnabled, (message) => { })
        {
            IsEnqueueBlocked = false;
            _queueGotBlocked = queueGotBlocked;
        }

        protected override bool WaitForQueueToGetEmpty()
        {
            IsEnqueueBlocked = true;
            _queueGotBlocked.Set();
            return base.WaitForQueueToGetEmpty();
        }

        /// <summary>
        /// Specifies whether enQueue was blocked or not.
        /// </summary>
        public bool IsEnqueueBlocked
        {
            get;
            set;
        }

        private readonly ManualResetEvent _queueGotBlocked;
    }

    /// <summary>
    /// a class that inherits from job queue and over rides the WaitForQueueToEmpty to simply setting a boolean to tell
    /// whether or not the queue entered the blocking method during the enqueue process.
    /// </summary>
    internal class JobQueueNonBlocking : JobQueue<string>
    {
        public JobQueueNonBlocking(Action<string?> processHandler)
            : base(processHandler, "foo", 1, 5, true, (message) => { })
        {
            EnteredBlockingMethod = false;
        }

        public bool EnteredBlockingMethod { get; private set; }

        protected override bool WaitForQueueToGetEmpty()
        {
            EnteredBlockingMethod = true;
            return true;
        }
    }

    #endregion

    #region Utility Methods

    /// <summary>
    /// Returns a job processing handler which does nothing.
    /// </summary>
    /// <typeparam name="T">Type of job the handler processes.</typeparam>
    /// <returns>Job processing handler which does nothing.</returns>
    private static Action<T?> GetEmptyProcessHandler<T>()
    {
        Action<T?> handler = (job) =>
        {
        };

        return handler;
    }

    #endregion
}
