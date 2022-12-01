// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Common;

// These tests are really integration tests. There isn't a good way to unit test this functionality since
// the only thing in here is threading.
public class BackgroundDocumentGeneratorTest : LanguageServerTestBase
{
    private readonly HostDocument[] _documents;
    private readonly HostProject _hostProject1;
    private readonly HostProject _hostProject2;

    public BackgroundDocumentGeneratorTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _documents = new HostDocument[]
        {
            new HostDocument("c:/Test1/Index.cshtml", "Index.cshtml"),
            new HostDocument("c:/Test1/Components/Counter.cshtml", "Components/Counter.cshtml"),
        };

        _hostProject1 = new HostProject("c:/Test1/Test1.csproj", RazorConfiguration.Default, "TestRootNamespace");
        _hostProject2 = new HostProject("c:/Test2/Test2.csproj", RazorConfiguration.Default, "TestRootNamespace");
    }

    [Fact]
    public async Task Queue_ProcessesNotifications_AndGoesBackToSleep()
    {
        // Arrange
        var projectManager = TestProjectSnapshotManager.Create(Dispatcher);
        await Dispatcher.RunOnDispatcherThreadAsync(() =>
        {
            projectManager.ProjectAdded(_hostProject1);
            projectManager.ProjectAdded(_hostProject2);
            projectManager.DocumentAdded(_hostProject1, _documents[0], null);
            projectManager.DocumentAdded(_hostProject1, _documents[1], null);
        }, DisposalToken);

        var project = await Dispatcher.RunOnDispatcherThreadAsync(
            () => projectManager.GetLoadedProject(_hostProject1.FilePath), DisposalToken);

        var queue = new TestBackgroundDocumentGenerator(Dispatcher)
        {
            Delay = TimeSpan.FromMilliseconds(1),
            BlockBackgroundWorkStart = new ManualResetEventSlim(initialState: false),
            NotifyBackgroundWorkStarting = new ManualResetEventSlim(initialState: false),
            BlockBackgroundWorkCompleting = new ManualResetEventSlim(initialState: false),
            NotifyBackgroundWorkCompleted = new ManualResetEventSlim(initialState: false),
        };

        // Act & Assert
        await Dispatcher.RunOnDispatcherThreadAsync(
            () => queue.Enqueue(project.GetDocument(_documents[0].FilePath)), DisposalToken);

        Assert.True(queue.IsScheduledOrRunning, "Queue should be scheduled during Enqueue");
        Assert.True(queue.HasPendingNotifications, "Queue should have a notification created during Enqueue");

        // Allow the background work to proceed.
        queue.BlockBackgroundWorkStart.Set();
        queue.BlockBackgroundWorkCompleting.Set();

        queue.NotifyBackgroundWorkCompleted.Wait(TimeSpan.FromSeconds(3));

        Assert.False(queue.IsScheduledOrRunning, "Queue should not have restarted");
        Assert.False(queue.HasPendingNotifications, "Queue should have processed all notifications");
    }

    [Fact]
    public async Task Queue_ProcessesNotifications_AndRestarts()
    {
        // Arrange
        var projectManager = TestProjectSnapshotManager.Create(Dispatcher);
        await Dispatcher.RunOnDispatcherThreadAsync(() =>
        {
            projectManager.ProjectAdded(_hostProject1);
            projectManager.ProjectAdded(_hostProject2);
            projectManager.DocumentAdded(_hostProject1, _documents[0], null);
            projectManager.DocumentAdded(_hostProject1, _documents[1], null);
        }, DisposalToken);

        var project = await Dispatcher.RunOnDispatcherThreadAsync(
            () => projectManager.GetLoadedProject(_hostProject1.FilePath), DisposalToken);

        var queue = new TestBackgroundDocumentGenerator(Dispatcher)
        {
            Delay = TimeSpan.FromMilliseconds(1),
            BlockBackgroundWorkStart = new ManualResetEventSlim(initialState: false),
            NotifyBackgroundWorkStarting = new ManualResetEventSlim(initialState: false),
            NotifyBackgroundCapturedWorkload = new ManualResetEventSlim(initialState: false),
            BlockBackgroundWorkCompleting = new ManualResetEventSlim(initialState: false),
            NotifyBackgroundWorkCompleted = new ManualResetEventSlim(initialState: false),
        };

        // Act & Assert
        await Dispatcher.RunOnDispatcherThreadAsync(
            () => queue.Enqueue(project.GetDocument(_documents[0].FilePath)), DisposalToken);

        Assert.True(queue.IsScheduledOrRunning, "Queue should be scheduled during Enqueue");
        Assert.True(queue.HasPendingNotifications, "Queue should have a notification created during Enqueue");

        // Allow the background work to start.
        queue.BlockBackgroundWorkStart.Set();

        queue.NotifyBackgroundWorkStarting.Wait(TimeSpan.FromSeconds(1));
        Assert.True(queue.IsScheduledOrRunning, "Worker should be processing now");

        queue.NotifyBackgroundCapturedWorkload.Wait(TimeSpan.FromSeconds(1));
        Assert.False(queue.HasPendingNotifications, "Worker should have taken all notifications");

        await Dispatcher.RunOnDispatcherThreadAsync(
            () => queue.Enqueue(project.GetDocument(_documents[1].FilePath)), DisposalToken);
        Assert.True(queue.HasPendingNotifications); // Now we should see the worker restart when it finishes.

        // Allow work to complete, which should restart the timer.
        queue.BlockBackgroundWorkCompleting.Set();

        queue.NotifyBackgroundWorkCompleted.Wait(TimeSpan.FromSeconds(3));
        queue.NotifyBackgroundWorkCompleted.Reset();

        // It should start running again right away.
        Assert.True(queue.IsScheduledOrRunning, "Queue should be scheduled during Enqueue");
        Assert.True(queue.HasPendingNotifications, "Queue should have a notification created during Enqueue");

        // Allow the background work to proceed.
        queue.BlockBackgroundWorkStart.Set();

        queue.BlockBackgroundWorkCompleting.Set();
        queue.NotifyBackgroundWorkCompleted.Wait(TimeSpan.FromSeconds(3));

        Assert.False(queue.IsScheduledOrRunning, "Queue should not have restarted");
        Assert.False(queue.HasPendingNotifications, "Queue should have processed all notifications");
    }

    private class TestBackgroundDocumentGenerator : BackgroundDocumentGenerator
    {
        public TestBackgroundDocumentGenerator(ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher)
            : base(projectSnapshotManagerDispatcher)
        {
        }
    }
}
