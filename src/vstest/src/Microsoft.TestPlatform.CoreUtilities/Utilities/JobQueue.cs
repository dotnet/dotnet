// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Resources;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.Utilities;

/// <summary>
/// Generic queue for processing jobs on a background thread.
/// </summary>
/// <typeparam name="T">The type of the job that is being processed.</typeparam>
public class JobQueue<T> : IDisposable
{
    /// <summary>
    /// Handler which processes the individual jobs.
    /// </summary>
    private readonly Action<T?> _processJob;

    /// <summary>
    /// Name used when displaying information or reporting errors about this queue.
    /// </summary>
    private readonly string _displayName;

    /// <summary>
    /// The queue of jobs.
    /// </summary>
    private readonly Queue<Job<T>> _jobsQueue;

    /// <summary>
    /// Signaled when a job is added to the queue.  Used to wakeup the background thread.
    /// </summary>
    private readonly ManualResetEvent _jobAdded;

    /// <summary>
    /// The maximum number of jobs the job queue may hold.
    /// </summary>
    private readonly int _maxNumberOfJobsInQueue;

    /// <summary>
    /// The maximum total size of jobs the job queue may hold.
    /// </summary>
    private readonly int _maxBytesQueueCanHold;

    /// <summary>
    /// Gives the approximate total size of objects in the queue.
    /// </summary>
    private int _currentNumberOfBytesQueueIsHolding;

    /// <summary>
    /// Tells whether the queue should be bounded on size and no of events.
    /// </summary>
    private bool _enableBoundsOnQueue;

    /// <summary>
    /// Used to pause and resume processing of the queue.  By default the manual reset event is
    /// set so the queue can continue processing.
    /// </summary>
    private readonly ManualResetEvent _queueProcessing;

    /// <summary>
    /// The background thread which is processing the jobs.  Used when disposing to wait
    /// for the thread to complete.
    /// </summary>
    private readonly Task _backgroundJobProcessor;

    /// <summary>
    /// Keeps track of if we are disposed.
    /// </summary>
    private bool _isDisposed;

    /// <summary>
    /// Logs to this action any exception when processing jobs.
    /// </summary>
    private readonly Action<string> _exceptionLogger;

    /// <summary>
    /// True when the job queue is paused. Don't use this for synchronization,
    /// it is not super thread-safe. Just use it to see if the queue was started already.
    /// </summary>
    public bool IsPaused { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="JobQueue{T}"/> class.
    /// </summary>
    /// <param name="processJob">Action to handle the processing of the job.</param>
    /// <param name="displayName">Name to used when displaying information about this queue.</param>
    /// <param name="maxQueueLength">The max Queue Length.</param>
    /// <param name="maxQueueSize">The max Queue Size.</param>
    /// <param name="enableBounds">The enable Bounds.</param>
    /// <param name="exceptionLogger">The exception Logger.</param>
    public JobQueue(Action<T?> processJob, string displayName, int maxQueueLength, int maxQueueSize, bool enableBounds, Action<string> exceptionLogger)
    {
        _processJob = processJob ?? throw new ArgumentNullException(nameof(processJob));

        if (string.IsNullOrWhiteSpace(displayName))
        {
            throw new ArgumentException(Resources.CannotBeNullOrEmpty, nameof(displayName));
        }

        if (maxQueueLength < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(maxQueueLength));
        }

        if (maxQueueSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(maxQueueSize));
        }

        _maxNumberOfJobsInQueue = maxQueueLength;
        _maxBytesQueueCanHold = maxQueueSize;
        _enableBoundsOnQueue = enableBounds;

        // Initialize defaults.
        _jobsQueue = new Queue<Job<T>>();
        _jobAdded = new ManualResetEvent(false);
        _queueProcessing = new ManualResetEvent(true);
        _currentNumberOfBytesQueueIsHolding = 0;
        _isDisposed = false;

        // Save off the arguments.
        _displayName = displayName;
        _exceptionLogger = exceptionLogger;

        // Setup the background thread to process the jobs.
        _backgroundJobProcessor = new Task(() => BackgroundJobProcessor(_displayName), TaskCreationOptions.LongRunning);
        _backgroundJobProcessor.Start();
    }

    /// <summary>
    /// Adds a job to the queue.
    /// </summary>
    /// <param name="job"> Job to add to the queue. </param>
    /// <param name="jobSize"> The job Size. </param>
    public void QueueJob(T job, int jobSize)
    {
        CheckDisposed();

        TPDebug.Assert(jobSize >= 0, "Job size should never be negative");

        // Add the job and signal that a new job is available.
        InternalQueueJob(new Job<T>(job, jobSize));
    }

    /// <summary>
    /// Pause the processing of queued jobs.
    /// </summary>
    public void Pause()
    {
        CheckDisposed();

        // Do not allow any jobs to be processed.
        IsPaused = true;
        _queueProcessing.Reset();
    }

    /// <summary>
    /// Resume the processing of queued jobs.
    /// </summary>
    public void Resume()
    {
        CheckDisposed();

        // Resume processing of jobs.
        _queueProcessing.Set();
        IsPaused = false;
    }

    /// <summary>
    /// Waits for all current jobs in the queue to be processed and then returns.
    /// </summary>
    public void Flush()
    {
        CheckDisposed();

        // Create the wait job.
        using var waitEvent = new ManualResetEvent(false);
        var waitJob = Job<T>.CreateWaitJob(waitEvent);

        // Queue the wait job and wait for it to be processed.
        InternalQueueJob(waitJob);

        waitEvent.WaitOne();
    }

    /// <summary>
    /// Waits for all pending jobs to complete and shutdown the background thread.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed
            || !disposing)
        {
            return;
        }

        // If the queue is paused, then throw.
        if (!_queueProcessing.WaitOne(0))
        {
            throw new InvalidOperationException(
                string.Format(CultureInfo.CurrentCulture, Resources.QueuePausedDisposeError, _displayName));
        }

        _isDisposed = true;

        // Disable bounds on the queue so that any waiting threads can proceed.
        lock (_jobsQueue)
        {
            _enableBoundsOnQueue = false;
            Monitor.PulseAll(_jobsQueue);
        }

        // Flag the queue as being shutdown and wake up the background thread.
        InternalQueueJob(Job<T>.ShutdownJob);

        // Wait for the background thread to shutdown.
        _backgroundJobProcessor.Wait();

        // Cleanup
        _jobAdded.Dispose();
        _queueProcessing.Dispose();
    }

    /// <summary>
    /// Block the queue call.
    /// A separate protected virtual method had to be made so that it can be over-ridden when writing unit test to check
    /// if bounds on the queue are applied correctly.
    /// </summary>
    /// <returns>True if the queue is empty.</returns>
    protected virtual bool WaitForQueueToGetEmpty()
    {
        EqtTrace.Verbose("blocking on over filled queue.");
        return Monitor.Wait(_jobsQueue);
    }

    /// <summary>
    /// Queue the job and signal the background thread.
    /// </summary>
    /// <param name="job">Job to be queued.</param>
    private void InternalQueueJob(Job<T> job)
    {
        // Add the job and signal that a new job is available.
        lock (_jobsQueue)
        {
            // If the queue is getting over filled wait till the background processor releases the thread.
            while (_enableBoundsOnQueue
                   &&
                   ((_jobsQueue.Count >= _maxNumberOfJobsInQueue)
                    ||
                    (_currentNumberOfBytesQueueIsHolding >= _maxBytesQueueCanHold)))
            {
                WaitForQueueToGetEmpty();
            }

            _jobsQueue.Enqueue(job);
            _currentNumberOfBytesQueueIsHolding += job.Size;
            _jobAdded.Set();
        }
    }

    /// <summary>
    /// Throws wen the queue has been disposed.
    /// </summary>
    private void CheckDisposed()
    {
        if (_isDisposed)
        {
            throw new ObjectDisposedException(
                string.Format(CultureInfo.CurrentCulture, Resources.QueueAlreadyDisposed, _displayName));
        }
    }

    /// <summary>
    /// Method which processes the jobs on the background thread.
    /// </summary>
    private void BackgroundJobProcessor(string threadName)
    {
#if DEBUG && (NETFRAMEWORK || NET || NETSTANDARD2_0_OR_GREATER)
        Thread.CurrentThread.Name = threadName;
#endif
        bool shutdown = false;

        do
        {
            _jobAdded.WaitOne();

            // Pull all of the current jobs out of the queue.
            List<Job<T>> jobs = new();
            lock (_jobsQueue)
            {
                while (_jobsQueue.Count != 0)
                {
                    var job = _jobsQueue.Dequeue();
                    _currentNumberOfBytesQueueIsHolding -= job.Size;

                    // If this is a shutdown job, signal shutdown and stop adding jobs.
                    if (job.Shutdown)
                    {
                        shutdown = true;
                        break;
                    }

                    jobs.Add(job);
                }

                // Reset the manual reset event so we get notified of new jobs that are added.
                _jobAdded.Reset();

                // Releases a thread waiting on the queue to get empty, to continue with the enqueuing process.
                if (_enableBoundsOnQueue)
                {
                    Monitor.PulseAll(_jobsQueue);
                }
            }

            // Process the jobs
            foreach (var job in jobs)
            {
                // Wait for the queue to be open (not paused) and process the job.
                _queueProcessing.WaitOne();

                // If this is a wait job, signal the manual reset event and continue.
                if (job.WaitManualResetEvent != null)
                {
                    job.WaitManualResetEvent.Set();
                }
                else
                {
                    SafeProcessJob(job.Payload);
                }
            }
        }
        while (!shutdown);
    }

    /// <summary>
    /// Executes the process job handler and logs any exceptions which occur.
    /// </summary>
    /// <param name="job">Job to be executed.</param>
    private void SafeProcessJob(T? job)
    {
        try
        {
            _processJob(job);
        }
        catch (Exception e)
        {
            _exceptionLogger(string.Format(CultureInfo.CurrentCulture, Resources.ExceptionFromJobProcessor, _displayName, e));
        }
    }

}
