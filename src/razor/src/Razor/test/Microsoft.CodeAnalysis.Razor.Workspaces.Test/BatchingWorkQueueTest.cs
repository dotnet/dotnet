// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

public class BatchingWorkQueueTest : TestBase
{
    private readonly BatchingWorkQueue _workQueue;
    private readonly BatchingWorkQueue.TestAccessor _testAccessor;
    private readonly TestErrorReporter _errorReporter;

    public BatchingWorkQueueTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _errorReporter = new TestErrorReporter();
        _workQueue = new BatchingWorkQueue(TimeSpan.FromMilliseconds(1), StringComparer.Ordinal, _errorReporter);
        _testAccessor = _workQueue.GetTestAccessor();
        _testAccessor.NotifyBackgroundWorkCompleted = new ManualResetEventSlim(initialState: false);

        AddDisposable(_workQueue);
    }

    [Fact]
    public void Enqueue_ProcessesNotifications_AndGoesBackToSleep()
    {
        // Arrange
        var workItem = new TestBatchableWorkItem();
        _testAccessor.BlockBackgroundWorkStart = new ManualResetEventSlim(initialState: false);
        _testAccessor.BlockBackgroundWorkCompleting = new ManualResetEventSlim(initialState: false);

        // Act
        _workQueue.Enqueue("key", workItem);

        // Assert
        Assert.True(_testAccessor.IsScheduledOrRunning, "Queue should be scheduled during Enqueue");
        Assert.NotEmpty(_testAccessor.Work);

        // Allow the background work to proceed.
        _testAccessor.BlockBackgroundWorkStart.Set();
        _testAccessor.BlockBackgroundWorkCompleting.Set();

        _testAccessor.NotifyBackgroundWorkCompleted.Wait(TimeSpan.FromSeconds(3));

        Assert.False(_testAccessor.IsScheduledOrRunning, "Queue should not have restarted");
        Assert.Empty(_testAccessor.Work);
        Assert.True(workItem.Processed);
        Assert.Empty(_errorReporter.ReportedExceptions);
    }

    [Fact]
    public void Enqueue_BatchesNotificationsByKey_ProcessesLast()
    {
        // Arrange
        var originalWorkItem = new ThrowingBatchableWorkItem();
        var newestWorkItem = new TestBatchableWorkItem();
        _testAccessor.BlockBackgroundWorkStart = new ManualResetEventSlim(initialState: false);

        // Act
        _workQueue.Enqueue("key", originalWorkItem);
        _workQueue.Enqueue("key", newestWorkItem);

        // Assert
        Assert.True(_testAccessor.IsScheduledOrRunning, "Queue should be scheduled during Enqueue");
        Assert.NotEmpty(_testAccessor.Work);

        // Allow the background work to start.
        _testAccessor.BlockBackgroundWorkStart.Set();
        _testAccessor.NotifyBackgroundWorkCompleted.Wait(TimeSpan.FromSeconds(3));

        Assert.Empty(_testAccessor.Work);

        Assert.False(originalWorkItem.Processed);
        Assert.True(newestWorkItem.Processed);
        Assert.Empty(_errorReporter.ReportedExceptions);
    }

    [Fact]
    public void Enqueue_ProcessesNotifications_AndRestarts()
    {
        // Arrange
        var initialWorkItem = new TestBatchableWorkItem();
        var workItemToCauseRestart = new TestBatchableWorkItem();
        _testAccessor.BlockBackgroundWorkStart = new ManualResetEventSlim(initialState: false);
        _testAccessor.NotifyBackgroundWorkStarting = new ManualResetEventSlim(initialState: false);
        _testAccessor.NotifyBackgroundCapturedWorkload = new ManualResetEventSlim(initialState: false);
        _testAccessor.BlockBackgroundWorkCompleting = new ManualResetEventSlim(initialState: false);
        _testAccessor.NotifyBackgroundWorkCompleted = new ManualResetEventSlim(initialState: false);

        // Act & Assert
        _workQueue.Enqueue("key", initialWorkItem);

        Assert.True(_testAccessor.IsScheduledOrRunning, "Queue should be scheduled during Enqueue");
        Assert.NotEmpty(_testAccessor.Work);

        // Allow the background work to start.
        _testAccessor.BlockBackgroundWorkStart.Set();

        _testAccessor.NotifyBackgroundWorkStarting.Wait(TimeSpan.FromSeconds(3));

        Assert.True(_testAccessor.IsScheduledOrRunning, "Worker should be processing now");

        _testAccessor.NotifyBackgroundCapturedWorkload.Wait(TimeSpan.FromSeconds(3));
        Assert.Empty(_testAccessor.Work);

        _workQueue.Enqueue("key", workItemToCauseRestart);
        Assert.NotEmpty(_testAccessor.Work); // Now we should see the worker restart when it finishes.

        // Allow work to complete, which should restart the timer.
        _testAccessor.BlockBackgroundWorkCompleting.Set();

        _testAccessor.NotifyBackgroundWorkCompleted.Wait(TimeSpan.FromSeconds(3));
        _testAccessor.NotifyBackgroundWorkCompleted.Reset();

        // It should start running again right away.
        Assert.True(_testAccessor.IsScheduledOrRunning, "Queue should be scheduled during Enqueue");
        Assert.NotEmpty(_testAccessor.Work);

        // Allow the background work to proceed.
        _testAccessor.BlockBackgroundWorkStart.Set();

        _testAccessor.BlockBackgroundWorkCompleting.Set();
        _testAccessor.NotifyBackgroundWorkCompleted.Wait(TimeSpan.FromSeconds(3));

        Assert.False(_testAccessor.IsScheduledOrRunning, "Queue should not have restarted");
        Assert.Empty(_testAccessor.Work);
        Assert.True(initialWorkItem.Processed);
        Assert.True(workItemToCauseRestart.Processed);
        Assert.Empty(_errorReporter.ReportedExceptions);
    }

    [Fact]
    public void Enqueue_ThrowingWorkItem_DoesNotPreventProcessingSubsequentItems()
    {
        // Arrange
        var throwingWorkItem = new ThrowingBatchableWorkItem();
        var validWorkItem = new TestBatchableWorkItem();
        _testAccessor.BlockBackgroundWorkStart = new ManualResetEventSlim(initialState: false);

        // Act
        _workQueue.Enqueue("key", throwingWorkItem);
        _workQueue.Enqueue("key2", validWorkItem);

        // Assert
        Assert.True(_testAccessor.IsScheduledOrRunning, "Queue should be scheduled during Enqueue");
        Assert.NotEmpty(_testAccessor.Work);

        // Allow the background work to start.
        _testAccessor.BlockBackgroundWorkStart.Set();
        _testAccessor.NotifyBackgroundWorkCompleted.Wait(TimeSpan.FromSeconds(3));

        Assert.Empty(_testAccessor.Work);

        Assert.True(throwingWorkItem.Processed);
        Assert.True(validWorkItem.Processed);
        Assert.Single(_errorReporter.ReportedExceptions);
    }

    [Fact]
    public void Enqueue_DisposedPreventsRestart()
    {
        // Arrange
        var initialWorkItem = new TestBatchableWorkItem();
        var workItemToCauseRestart = new TestBatchableWorkItem();
        _testAccessor.BlockBackgroundWorkStart = new ManualResetEventSlim(initialState: false);
        _testAccessor.NotifyBackgroundWorkStarting = new ManualResetEventSlim(initialState: false);
        _testAccessor.NotifyBackgroundCapturedWorkload = new ManualResetEventSlim(initialState: false);
        _testAccessor.NotifyBackgroundWorkCompleting = new ManualResetEventSlim(initialState: false);
        _testAccessor.BlockBackgroundWorkCompleting = new ManualResetEventSlim(initialState: false);
        _testAccessor.NotifyBackgroundWorkCompleted = new ManualResetEventSlim(initialState: false);

        // Act & Assert
        _workQueue.Enqueue("key", initialWorkItem);

        Assert.True(_testAccessor.IsScheduledOrRunning, "Queue should be scheduled during Enqueue");
        Assert.NotEmpty(_testAccessor.Work);

        // Allow the background work to start.
        _testAccessor.BlockBackgroundWorkStart.Set();

        _testAccessor.NotifyBackgroundWorkStarting.Wait(TimeSpan.FromSeconds(3));

        Assert.True(_testAccessor.IsScheduledOrRunning, "Worker should be processing now");

        _testAccessor.NotifyBackgroundCapturedWorkload.Wait(TimeSpan.FromSeconds(3));
        Assert.Empty(_testAccessor.Work);

        // Wait for the background workload to complete
        _testAccessor.NotifyBackgroundWorkCompleting.Wait(TimeSpan.FromSeconds(5));

        _workQueue.Enqueue("key", workItemToCauseRestart);
        Assert.NotEmpty(_testAccessor.Work);

        // Disposing before the queue has a chance to restart;
        _workQueue.Dispose();

        // Allow work to complete, which should restart the timer.
        _testAccessor.BlockBackgroundWorkCompleting.Set();

        _testAccessor.NotifyBackgroundWorkCompleted.Wait(TimeSpan.FromSeconds(3));
        _testAccessor.NotifyBackgroundWorkCompleted.Reset();

        // It should start running again right away.
        Assert.False(_testAccessor.IsScheduledOrRunning, "Queue should be scheduled during Enqueue");

        // Dispose clears the work queue
        Assert.Empty(_testAccessor.Work);

        Assert.True(initialWorkItem.Processed);
        Assert.False(workItemToCauseRestart.Processed);
        Assert.Empty(_errorReporter.ReportedExceptions);
    }

    private class TestBatchableWorkItem : BatchableWorkItem
    {
        public bool Processed { get; private set; }

        public override ValueTask ProcessAsync(CancellationToken cancellationToken)
        {
            Processed = true;
            return new ValueTask();
        }
    }

    private class ThrowingBatchableWorkItem : TestBatchableWorkItem
    {
        public override ValueTask ProcessAsync(CancellationToken cancellationToken)
        {
            _ = base.ProcessAsync(cancellationToken);
            throw new InvalidOperationException();
        }
    }

    private class TestErrorReporter : IErrorReporter
    {
        private readonly List<Exception> _reportedExceptions = new();

        public IReadOnlyList<Exception> ReportedExceptions => _reportedExceptions;

        public void ReportError(Exception exception) => _reportedExceptions.Add(exception);

        public void ReportError(Exception exception, IProjectSnapshot project) => _reportedExceptions.Add(exception);

        public void ReportError(Exception exception, Project workspaceProject) => _reportedExceptions.Add(exception);
    }
}
