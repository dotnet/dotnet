// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Razor.DynamicFiles;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

// These tests are really integration tests. There isn't a good way to unit test this functionality since
// the only thing in here is threading.
public class BackgroundDocumentGeneratorTest(ITestOutputHelper testOutput) : VisualStudioWorkspaceTestBase(testOutput)
{
    private static readonly HostDocument[] s_documents = [TestProjectData.SomeProjectFile1, TestProjectData.AnotherProjectFile1];

    private static readonly HostProject s_hostProject1 = new(
        TestProjectData.SomeProject.FilePath,
        TestProjectData.SomeProject.IntermediateOutputPath,
        FallbackRazorConfiguration.MVC_1_0,
        TestProjectData.SomeProject.RootNamespace);

    private static readonly HostProject s_hostProject2 = new(
        TestProjectData.AnotherProject.FilePath,
        TestProjectData.AnotherProject.IntermediateOutputPath,
        FallbackRazorConfiguration.MVC_1_0,
        TestProjectData.AnotherProject.RootNamespace);

    private readonly TestDynamicFileInfoProvider _dynamicFileInfoProvider = new();

    protected override void ConfigureProjectEngine(RazorProjectEngineBuilder builder)
    {
        builder.SetImportFeature(new TestImportProjectFeature());
    }

    [UIFact]
    public async Task ProcessDocument_LongDocumentParse_DoesNotUpdateAfterSuppress()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(s_hostProject1);
        });

        // We utilize a task completion source here so we can "fake" a document parse taking a significant amount of time
        var tcs = new TaskCompletionSource<TextAndVersion>();
        var textLoader = new StrictMock<TextLoader>();
        textLoader
            .Setup(loader => loader.LoadTextAndVersionAsync(It.IsAny<LoadTextOptions>(), It.IsAny<CancellationToken>()))
            .Returns(tcs.Task);
        var hostDocument = s_documents[0];

        var project = projectManager.GetLoadedProject(s_hostProject1.Key);
        var queue = new BackgroundDocumentGenerator(projectManager, Dispatcher, _dynamicFileInfoProvider, ErrorReporter)
        {
            Delay = TimeSpan.FromMilliseconds(1),
            NotifyBackgroundWorkCompleted = new ManualResetEventSlim(initialState: false),
            NotifyBackgroundCapturedWorkload = new ManualResetEventSlim(initialState: false),
        };

        // We trigger enqueued notifications via adding/opening to the project manager

        // Act & Assert
        await projectManager.UpdateAsync(updater =>
        {
            updater.DocumentAdded(s_hostProject1.Key, hostDocument, textLoader.Object);
        });

        queue.NotifyBackgroundCapturedWorkload.Wait();

        await projectManager.UpdateAsync(updater =>
        {
            updater.DocumentOpened(s_hostProject1.Key, hostDocument.FilePath, SourceText.From(string.Empty));
        });

        // Verify document was suppressed because it was opened
        Assert.Null(_dynamicFileInfoProvider.DynamicDocuments[hostDocument.FilePath]);

        // Unblock document processing
        tcs.SetResult(TextAndVersion.Create(SourceText.From(string.Empty), VersionStamp.Default));

        await Task.Run(() => queue.NotifyBackgroundWorkCompleted.Wait(TimeSpan.FromSeconds(3)));

        // Validate that even though document parsing took a significant amount of time that the dynamic document wasn't "unsuppressed"
        Assert.Null(_dynamicFileInfoProvider.DynamicDocuments[hostDocument.FilePath]);
    }

    [UIFact]
    public async Task ProcessDocument_SwallowsIOExceptions()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(s_hostProject1);
        });

        var textLoader = new StrictMock<TextLoader>();
        textLoader
            .Setup(loader => loader.LoadTextAndVersionAsync(It.IsAny<LoadTextOptions>(), It.IsAny<CancellationToken>()))
            .Throws<FileNotFoundException>();

        await projectManager.UpdateAsync(updater =>
        {
            updater.DocumentAdded(s_hostProject1.Key, s_documents[0], textLoader.Object);
        });

        var project = projectManager.GetLoadedProject(s_hostProject1.Key);

        var queue = new BackgroundDocumentGenerator(projectManager, Dispatcher, _dynamicFileInfoProvider, ErrorReporter)
        {
            Delay = TimeSpan.FromMilliseconds(1),
            NotifyBackgroundWorkCompleted = new ManualResetEventSlim(initialState: false),
            NotifyErrorBeingReported = new ManualResetEventSlim(initialState: false),
        };

        // Act & Assert
        await RunOnDispatcherAsync(() =>
        {
            queue.Enqueue(project, project.GetDocument(s_documents[0].FilePath).AssumeNotNull());
        });

        await Task.Run(() => queue.NotifyBackgroundWorkCompleted.Wait(TimeSpan.FromSeconds(3)));

        Assert.False(queue.NotifyErrorBeingReported.IsSet);
    }

    [UIFact]
    public async Task ProcessDocument_SwallowsUnauthorizedAccessExceptions()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(s_hostProject1);
        });

        var textLoader = new StrictMock<TextLoader>();
        textLoader
            .Setup(loader => loader.LoadTextAndVersionAsync(It.IsAny<LoadTextOptions>(), It.IsAny<CancellationToken>()))
            .Throws<UnauthorizedAccessException>();

        await projectManager.UpdateAsync(updater =>
        {
            updater.DocumentAdded(s_hostProject1.Key, s_documents[0], textLoader.Object);
        });

        var project = projectManager.GetLoadedProject(s_hostProject1.Key);

        var queue = new BackgroundDocumentGenerator(projectManager, Dispatcher, _dynamicFileInfoProvider, ErrorReporter)
        {
            Delay = TimeSpan.FromMilliseconds(1),
            NotifyBackgroundWorkCompleted = new ManualResetEventSlim(initialState: false),
            NotifyErrorBeingReported = new ManualResetEventSlim(initialState: false),
        };

        // Act & Assert
        await RunOnDispatcherAsync(() =>
        {
            queue.Enqueue(project, project.GetDocument(s_documents[0].FilePath).AssumeNotNull());
        });

        await Task.Run(() => queue.NotifyBackgroundWorkCompleted.Wait(TimeSpan.FromSeconds(3)));

        Assert.False(queue.NotifyErrorBeingReported.IsSet);
    }

    [UIFact]
    public async Task Queue_ProcessesNotifications_AndGoesBackToSleep()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(s_hostProject1);
            updater.ProjectAdded(s_hostProject2);
            updater.DocumentAdded(s_hostProject1.Key, s_documents[0], null!);
            updater.DocumentAdded(s_hostProject1.Key, s_documents[1], null!);
        });

        var project = projectManager.GetLoadedProject(s_hostProject1.Key);

        var queue = new BackgroundDocumentGenerator(projectManager, Dispatcher, _dynamicFileInfoProvider, ErrorReporter)
        {
            Delay = TimeSpan.FromMilliseconds(1),
            BlockBackgroundWorkStart = new ManualResetEventSlim(initialState: false),
            NotifyBackgroundWorkStarting = new ManualResetEventSlim(initialState: false),
            BlockBackgroundWorkCompleting = new ManualResetEventSlim(initialState: false),
            NotifyBackgroundWorkCompleted = new ManualResetEventSlim(initialState: false),
        };

        // Act & Assert
        await RunOnDispatcherAsync(() =>
        {
            queue.Enqueue(project, project.GetDocument(s_documents[0].FilePath).AssumeNotNull());
        });

        Assert.True(queue.IsScheduledOrRunning, "Queue should be scheduled during Enqueue");
        Assert.True(queue.HasPendingNotifications, "Queue should have a notification created during Enqueue");

        // Allow the background work to proceed.
        queue.BlockBackgroundWorkStart.Set();
        queue.BlockBackgroundWorkCompleting.Set();

        await Task.Run(() => queue.NotifyBackgroundWorkCompleted.Wait(TimeSpan.FromSeconds(3)));

        Assert.False(queue.IsScheduledOrRunning, "Queue should not have restarted");
        Assert.False(queue.HasPendingNotifications, "Queue should have processed all notifications");
    }

    [UIFact]
    public async Task Queue_ProcessesNotifications_AndRestarts()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(s_hostProject1);
            updater.ProjectAdded(s_hostProject2);
            updater.DocumentAdded(s_hostProject1.Key, s_documents[0], null!);
            updater.DocumentAdded(s_hostProject1.Key, s_documents[1], null!);
        });

        var project = projectManager.GetLoadedProject(s_hostProject1.Key);

        var queue = new BackgroundDocumentGenerator(projectManager, Dispatcher, _dynamicFileInfoProvider, ErrorReporter)
        {
            Delay = TimeSpan.FromMilliseconds(1),
            BlockBackgroundWorkStart = new ManualResetEventSlim(initialState: false),
            NotifyBackgroundWorkStarting = new ManualResetEventSlim(initialState: false),
            NotifyBackgroundCapturedWorkload = new ManualResetEventSlim(initialState: false),
            BlockBackgroundWorkCompleting = new ManualResetEventSlim(initialState: false),
            NotifyBackgroundWorkCompleted = new ManualResetEventSlim(initialState: false),
        };

        // Act & Assert
        await RunOnDispatcherAsync(() =>
        {
            queue.Enqueue(project, project.GetDocument(s_documents[0].FilePath).AssumeNotNull());
        });

        Assert.True(queue.IsScheduledOrRunning, "Queue should be scheduled during Enqueue");
        Assert.True(queue.HasPendingNotifications, "Queue should have a notification created during Enqueue");

        // Allow the background work to start.
        queue.BlockBackgroundWorkStart.Set();

        await Task.Run(() => queue.NotifyBackgroundWorkStarting.Wait(TimeSpan.FromSeconds(1)));

        Assert.True(queue.IsScheduledOrRunning, "Worker should be processing now");

        await Task.Run(() => queue.NotifyBackgroundCapturedWorkload.Wait(TimeSpan.FromSeconds(1)));
        Assert.False(queue.HasPendingNotifications, "Worker should have taken all notifications");

        await RunOnDispatcherAsync(() =>
        {
            queue.Enqueue(project, project.GetDocument(s_documents[1].FilePath).AssumeNotNull());
        });

        Assert.True(queue.HasPendingNotifications); // Now we should see the worker restart when it finishes.

        // Allow work to complete, which should restart the timer.
        queue.BlockBackgroundWorkCompleting.Set();

        await Task.Run(() => queue.NotifyBackgroundWorkCompleted.Wait(TimeSpan.FromSeconds(3)));
        queue.NotifyBackgroundWorkCompleted.Reset();

        // It should start running again right away.
        Assert.True(queue.IsScheduledOrRunning, "Queue should be scheduled during Enqueue");
        Assert.True(queue.HasPendingNotifications, "Queue should have a notification created during Enqueue");

        // Allow the background work to proceed.
        queue.BlockBackgroundWorkStart.Set();

        queue.BlockBackgroundWorkCompleting.Set();
        await Task.Run(() => queue.NotifyBackgroundWorkCompleted.Wait(TimeSpan.FromSeconds(3)));

        Assert.False(queue.IsScheduledOrRunning, "Queue should not have restarted");
        Assert.False(queue.HasPendingNotifications, "Queue should have processed all notifications");
    }

    [UIFact]
    public async Task DocumentChanged_ReparsesRelatedFiles()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        var documents = new[]
        {
            TestProjectData.SomeProjectComponentFile1,
            TestProjectData.SomeProjectImportFile
        };

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(s_hostProject1);
            for (var i = 0; i < documents.Length; i++)
            {
                updater.DocumentAdded(s_hostProject1.Key, documents[i], null!);
            }
        });

        var queue = new BackgroundDocumentGenerator(projectManager, Dispatcher, _dynamicFileInfoProvider, ErrorReporter)
        {
            Delay = TimeSpan.FromMilliseconds(1),
            BlockBackgroundWorkStart = new ManualResetEventSlim(initialState: false),
            NotifyBackgroundWorkStarting = new ManualResetEventSlim(initialState: false),
            NotifyBackgroundCapturedWorkload = new ManualResetEventSlim(initialState: false),
            BlockBackgroundWorkCompleting = new ManualResetEventSlim(initialState: false),
            NotifyBackgroundWorkCompleted = new ManualResetEventSlim(initialState: false),
        };

        var changedSourceText = SourceText.From("@inject DateTime Time");

        // Act & Assert
        await projectManager.UpdateAsync(updater =>
        {
            updater.DocumentChanged(s_hostProject1.Key, TestProjectData.SomeProjectImportFile.FilePath, changedSourceText);
        });

        Assert.True(queue.IsScheduledOrRunning, "Queue should be scheduled during Enqueue");
        Assert.True(queue.HasPendingNotifications, "Queue should have a notification created during Enqueue");

        for (var i = 0; i < documents.Length; i++)
        {
            var key = new DocumentKey(s_hostProject1.Key, documents[i].FilePath);
            Assert.True(queue.Work.ContainsKey(key));
        }

        // Allow the background work to start.
        queue.BlockBackgroundWorkStart.Set();

        await Task.Run(() => queue.NotifyBackgroundWorkStarting.Wait(TimeSpan.FromSeconds(1)));

        Assert.True(queue.IsScheduledOrRunning, "Worker should be processing now");

        await Task.Run(() => queue.NotifyBackgroundCapturedWorkload.Wait(TimeSpan.FromSeconds(1)));
        Assert.False(queue.HasPendingNotifications, "Worker should have taken all notifications");

        // Allow work to complete
        queue.BlockBackgroundWorkCompleting.Set();

        await Task.Run(() => queue.NotifyBackgroundWorkCompleted.Wait(TimeSpan.FromSeconds(3)));

        Assert.False(queue.HasPendingNotifications, "Queue should have processed all notifications");
        Assert.False(queue.IsScheduledOrRunning, "Queue should not have restarted");
    }

    [UIFact]
    public async Task DocumentRemoved_ReparsesRelatedFiles()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(s_hostProject1);
            updater.DocumentAdded(s_hostProject1.Key, TestProjectData.SomeProjectComponentFile1, null!);
            updater.DocumentAdded(s_hostProject1.Key, TestProjectData.SomeProjectImportFile, null!);
        });

        var queue = new BackgroundDocumentGenerator(projectManager, Dispatcher, _dynamicFileInfoProvider, ErrorReporter)
        {
            Delay = TimeSpan.FromMilliseconds(1),
            BlockBackgroundWorkStart = new ManualResetEventSlim(initialState: false),
            NotifyBackgroundWorkStarting = new ManualResetEventSlim(initialState: false),
            NotifyBackgroundCapturedWorkload = new ManualResetEventSlim(initialState: false),
            BlockBackgroundWorkCompleting = new ManualResetEventSlim(initialState: false),
            NotifyBackgroundWorkCompleted = new ManualResetEventSlim(initialState: false),
        };

        // Act & Assert
        await projectManager.UpdateAsync(updater =>
        {
            updater.DocumentRemoved(s_hostProject1.Key, TestProjectData.SomeProjectImportFile);
        });

        Assert.True(queue.IsScheduledOrRunning, "Queue should be scheduled during Enqueue");
        Assert.True(queue.HasPendingNotifications, "Queue should have a notification created during Enqueue");

        var kvp = Assert.Single(queue.Work);
        var expectedKey = new DocumentKey(s_hostProject1.Key, TestProjectData.SomeProjectComponentFile1.FilePath);
        Assert.Equal(expectedKey, kvp.Key);

        // Allow the background work to start.
        queue.BlockBackgroundWorkStart.Set();

        await Task.Run(() => queue.NotifyBackgroundWorkStarting.Wait(TimeSpan.FromSeconds(1)));

        Assert.True(queue.IsScheduledOrRunning, "Worker should be processing now");

        await Task.Run(() => queue.NotifyBackgroundCapturedWorkload.Wait(TimeSpan.FromSeconds(1)));
        Assert.False(queue.HasPendingNotifications, "Worker should have taken all notifications");

        // Allow work to complete
        queue.BlockBackgroundWorkCompleting.Set();

        await Task.Run(() => queue.NotifyBackgroundWorkCompleted.Wait(TimeSpan.FromSeconds(3)));

        Assert.False(queue.HasPendingNotifications, "Queue should have processed all notifications");
        Assert.False(queue.IsScheduledOrRunning, "Queue should not have restarted");
    }

    private class TestDynamicFileInfoProvider : IRazorDynamicFileInfoProviderInternal
    {
        private readonly Dictionary<string, IDynamicDocumentContainer?> _dynamicDocuments;

        public TestDynamicFileInfoProvider()
        {
            _dynamicDocuments = [];
        }

        public IReadOnlyDictionary<string, IDynamicDocumentContainer?> DynamicDocuments => _dynamicDocuments;

        public void SuppressDocument(ProjectKey projectFilePath, string documentFilePath)
        {
            _dynamicDocuments[documentFilePath] = null;
        }

        public void UpdateFileInfo(ProjectKey projectKey, IDynamicDocumentContainer documentContainer)
        {
            _dynamicDocuments[documentContainer.FilePath] = documentContainer;
        }

        public void UpdateLSPFileInfo(Uri documentUri, IDynamicDocumentContainer documentContainer)
        {
            _dynamicDocuments[documentContainer.FilePath] = documentContainer;
        }
    }
}
