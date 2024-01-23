// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Editor;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

// These tests are really integration tests. There isn't a good way to unit test this functionality since
// the only thing in here is threading.
public class BackgroundDocumentGeneratorTest : ProjectSnapshotManagerDispatcherWorkspaceTestBase
{
    private readonly HostDocument[] _documents;
    private readonly HostProject _hostProject1;
    private readonly HostProject _hostProject2;
    private readonly TestDynamicFileInfoProvider _dynamicFileInfoProvider;

    public BackgroundDocumentGeneratorTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _documents =
        [
            TestProjectData.SomeProjectFile1,
            TestProjectData.AnotherProjectFile1,
        ];

        _hostProject1 = new HostProject(TestProjectData.SomeProject.FilePath, TestProjectData.SomeProject.IntermediateOutputPath, FallbackRazorConfiguration.MVC_1_0, TestProjectData.SomeProject.RootNamespace);
        _hostProject2 = new HostProject(TestProjectData.AnotherProject.FilePath, TestProjectData.AnotherProject.IntermediateOutputPath, FallbackRazorConfiguration.MVC_1_0, TestProjectData.AnotherProject.RootNamespace);

        _dynamicFileInfoProvider = new TestDynamicFileInfoProvider();
    }

    protected override void ConfigureProjectEngine(RazorProjectEngineBuilder builder)
    {
        builder.SetImportFeature(new TestImportProjectFeature());
    }

    [UIFact]
    public async Task ProcessDocument_LongDocumentParse_DoesNotUpdateAfterSuppress()
    {
        // Arrange
        var projectManager = new TestProjectSnapshotManager(Workspace, ProjectEngineFactoryProvider, Dispatcher);
        projectManager.ProjectAdded(_hostProject1);

        // We utilize a task completion source here so we can "fake" a document parse taking a significant amount of time
        var tcs = new TaskCompletionSource<TextAndVersion>();
        var textLoader = new Mock<TextLoader>(MockBehavior.Strict);
        textLoader.Setup(loader => loader.LoadTextAndVersionAsync(It.IsAny<LoadTextOptions>(), It.IsAny<CancellationToken>()))
            .Returns(tcs.Task);
        var hostDocument = _documents[0];

        var project = projectManager.GetLoadedProject(_hostProject1.Key);
        var queue = new BackgroundDocumentGenerator(Dispatcher, _dynamicFileInfoProvider)
        {
            Delay = TimeSpan.FromMilliseconds(1),
            NotifyBackgroundWorkCompleted = new ManualResetEventSlim(initialState: false),
            NotifyBackgroundCapturedWorkload = new ManualResetEventSlim(initialState: false),
        };

        queue.Initialize(projectManager);

        // We trigger enqueued notifications via adding/opening to the project manager
        projectManager.AllowNotifyListeners = true;

        // Act & Assert
        projectManager.DocumentAdded(_hostProject1.Key, hostDocument, textLoader.Object);

        queue.NotifyBackgroundCapturedWorkload.Wait();

        projectManager.DocumentOpened(_hostProject1.Key, hostDocument.FilePath, SourceText.From(string.Empty));

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
        var projectManager = new TestProjectSnapshotManager(Workspace, ProjectEngineFactoryProvider, Dispatcher);
        projectManager.ProjectAdded(_hostProject1);

        var textLoader = new Mock<TextLoader>(MockBehavior.Strict);
        textLoader.Setup(loader => loader.LoadTextAndVersionAsync(It.IsAny<LoadTextOptions>(), It.IsAny<CancellationToken>()))
            .Throws<FileNotFoundException>();
        projectManager.DocumentAdded(_hostProject1.Key, _documents[0], textLoader.Object);

        var project = projectManager.GetLoadedProject(_hostProject1.Key);

        var queue = new BackgroundDocumentGenerator(Dispatcher, _dynamicFileInfoProvider)
        {
            Delay = TimeSpan.FromMilliseconds(1),
            NotifyBackgroundWorkCompleted = new ManualResetEventSlim(initialState: false),
            NotifyErrorBeingReported = new ManualResetEventSlim(initialState: false),
        };

        queue.Initialize(projectManager);

        // Act & Assert
        queue.Enqueue(project, project.GetDocument(_documents[0].FilePath));

        await Task.Run(() => queue.NotifyBackgroundWorkCompleted.Wait(TimeSpan.FromSeconds(3)));

        Assert.False(queue.NotifyErrorBeingReported.IsSet);
    }

    [UIFact]
    public async Task ProcessDocument_SwallowsUnauthorizedAccessExceptions()
    {
        // Arrange
        var projectManager = new TestProjectSnapshotManager(Workspace, ProjectEngineFactoryProvider, Dispatcher);
        projectManager.ProjectAdded(_hostProject1);

        var textLoader = new Mock<TextLoader>(MockBehavior.Strict);
        textLoader.Setup(loader => loader.LoadTextAndVersionAsync(It.IsAny<LoadTextOptions>(), It.IsAny<CancellationToken>()))
            .Throws<UnauthorizedAccessException>();
        projectManager.DocumentAdded(_hostProject1.Key, _documents[0], textLoader.Object);

        var project = projectManager.GetLoadedProject(_hostProject1.Key);

        var queue = new BackgroundDocumentGenerator(Dispatcher, _dynamicFileInfoProvider)
        {
            Delay = TimeSpan.FromMilliseconds(1),
            NotifyBackgroundWorkCompleted = new ManualResetEventSlim(initialState: false),
            NotifyErrorBeingReported = new ManualResetEventSlim(initialState: false),
        };

        queue.Initialize(projectManager);

        // Act & Assert
        queue.Enqueue(project, project.GetDocument(_documents[0].FilePath));

        await Task.Run(() => queue.NotifyBackgroundWorkCompleted.Wait(TimeSpan.FromSeconds(3)));

        Assert.False(queue.NotifyErrorBeingReported.IsSet);
    }

    [UIFact]
    public async Task Queue_ProcessesNotifications_AndGoesBackToSleep()
    {
        // Arrange
        var projectManager = new TestProjectSnapshotManager(Workspace, ProjectEngineFactoryProvider, Dispatcher);
        projectManager.ProjectAdded(_hostProject1);
        projectManager.ProjectAdded(_hostProject2);
        projectManager.DocumentAdded(_hostProject1.Key, _documents[0], null);
        projectManager.DocumentAdded(_hostProject1.Key, _documents[1], null);

        var project = projectManager.GetLoadedProject(_hostProject1.Key);

        var queue = new BackgroundDocumentGenerator(Dispatcher, _dynamicFileInfoProvider)
        {
            Delay = TimeSpan.FromMilliseconds(1),
            BlockBackgroundWorkStart = new ManualResetEventSlim(initialState: false),
            NotifyBackgroundWorkStarting = new ManualResetEventSlim(initialState: false),
            BlockBackgroundWorkCompleting = new ManualResetEventSlim(initialState: false),
            NotifyBackgroundWorkCompleted = new ManualResetEventSlim(initialState: false),
        };

        queue.Initialize(projectManager);

        // Act & Assert
        queue.Enqueue(project, project.GetDocument(_documents[0].FilePath));

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
        var projectManager = new TestProjectSnapshotManager(Workspace, ProjectEngineFactoryProvider, Dispatcher);
        projectManager.ProjectAdded(_hostProject1);
        projectManager.ProjectAdded(_hostProject2);
        projectManager.DocumentAdded(_hostProject1.Key, _documents[0], null);
        projectManager.DocumentAdded(_hostProject1.Key, _documents[1], null);

        var project = projectManager.GetLoadedProject(_hostProject1.Key);

        var queue = new BackgroundDocumentGenerator(Dispatcher, _dynamicFileInfoProvider)
        {
            Delay = TimeSpan.FromMilliseconds(1),
            BlockBackgroundWorkStart = new ManualResetEventSlim(initialState: false),
            NotifyBackgroundWorkStarting = new ManualResetEventSlim(initialState: false),
            NotifyBackgroundCapturedWorkload = new ManualResetEventSlim(initialState: false),
            BlockBackgroundWorkCompleting = new ManualResetEventSlim(initialState: false),
            NotifyBackgroundWorkCompleted = new ManualResetEventSlim(initialState: false),
        };

        queue.Initialize(projectManager);

        // Act & Assert
        queue.Enqueue(project, project.GetDocument(_documents[0].FilePath));

        Assert.True(queue.IsScheduledOrRunning, "Queue should be scheduled during Enqueue");
        Assert.True(queue.HasPendingNotifications, "Queue should have a notification created during Enqueue");

        // Allow the background work to start.
        queue.BlockBackgroundWorkStart.Set();

        await Task.Run(() => queue.NotifyBackgroundWorkStarting.Wait(TimeSpan.FromSeconds(1)));

        Assert.True(queue.IsScheduledOrRunning, "Worker should be processing now");

        await Task.Run(() => queue.NotifyBackgroundCapturedWorkload.Wait(TimeSpan.FromSeconds(1)));
        Assert.False(queue.HasPendingNotifications, "Worker should have taken all notifications");

        queue.Enqueue(project, project.GetDocument(_documents[1].FilePath));
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

    [UIFact(Skip = "https://github.com/dotnet/aspnetcore/issues/14805")]
    public async Task DocumentChanged_ReparsesRelatedFiles()
    {
        // Arrange
        var projectManager = new TestProjectSnapshotManager(Workspace, ProjectEngineFactoryProvider, Dispatcher)
        {
            AllowNotifyListeners = true,
        };
        var documents = new[]
        {
            TestProjectData.SomeProjectComponentFile1,
            TestProjectData.SomeProjectImportFile
        };
        projectManager.ProjectAdded(_hostProject1);
        for (var i = 0; i < documents.Length; i++)
        {
            projectManager.DocumentAdded(_hostProject1.Key, documents[i], null);
        }

        var queue = new BackgroundDocumentGenerator(Dispatcher, _dynamicFileInfoProvider)
        {
            Delay = TimeSpan.FromMilliseconds(1),
            BlockBackgroundWorkStart = new ManualResetEventSlim(initialState: false),
            NotifyBackgroundWorkStarting = new ManualResetEventSlim(initialState: false),
            NotifyBackgroundCapturedWorkload = new ManualResetEventSlim(initialState: false),
            BlockBackgroundWorkCompleting = new ManualResetEventSlim(initialState: false),
            NotifyBackgroundWorkCompleted = new ManualResetEventSlim(initialState: false),
        };

        var changedSourceText = SourceText.From("@inject DateTime Time");
        queue.Initialize(projectManager);

        // Act & Assert
        projectManager.DocumentChanged(_hostProject1.Key, TestProjectData.SomeProjectImportFile.FilePath, changedSourceText);

        Assert.True(queue.IsScheduledOrRunning, "Queue should be scheduled during Enqueue");
        Assert.True(queue.HasPendingNotifications, "Queue should have a notification created during Enqueue");

        for (var i = 0; i < documents.Length; i++)
        {
            var key = new DocumentKey(_hostProject1.Key, documents[i].FilePath);
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
        var projectManager = new TestProjectSnapshotManager(Workspace, ProjectEngineFactoryProvider, Dispatcher)
        {
            AllowNotifyListeners = true,
        };
        projectManager.ProjectAdded(_hostProject1);
        projectManager.DocumentAdded(_hostProject1.Key, TestProjectData.SomeProjectComponentFile1, null);
        projectManager.DocumentAdded(_hostProject1.Key, TestProjectData.SomeProjectImportFile, null);

        var queue = new BackgroundDocumentGenerator(Dispatcher, _dynamicFileInfoProvider)
        {
            Delay = TimeSpan.FromMilliseconds(1),
            BlockBackgroundWorkStart = new ManualResetEventSlim(initialState: false),
            NotifyBackgroundWorkStarting = new ManualResetEventSlim(initialState: false),
            NotifyBackgroundCapturedWorkload = new ManualResetEventSlim(initialState: false),
            BlockBackgroundWorkCompleting = new ManualResetEventSlim(initialState: false),
            NotifyBackgroundWorkCompleted = new ManualResetEventSlim(initialState: false),
        };

        queue.Initialize(projectManager);

        // Act & Assert
        projectManager.DocumentRemoved(_hostProject1.Key, TestProjectData.SomeProjectImportFile);

        Assert.True(queue.IsScheduledOrRunning, "Queue should be scheduled during Enqueue");
        Assert.True(queue.HasPendingNotifications, "Queue should have a notification created during Enqueue");

        var kvp = Assert.Single(queue.Work);
        var expectedKey = new DocumentKey(_hostProject1.Key, TestProjectData.SomeProjectComponentFile1.FilePath);
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

    private class TestDynamicFileInfoProvider : RazorDynamicFileInfoProvider
    {
        private readonly Dictionary<string, DynamicDocumentContainer> _dynamicDocuments;

        public TestDynamicFileInfoProvider()
        {
            _dynamicDocuments = new Dictionary<string, DynamicDocumentContainer>();
        }

        public IReadOnlyDictionary<string, DynamicDocumentContainer> DynamicDocuments => _dynamicDocuments;

        public override void Initialize(ProjectSnapshotManagerBase projectManager)
        {
        }

        public override void SuppressDocument(ProjectKey projectFilePath, string documentFilePath)
        {
            _dynamicDocuments[documentFilePath] = null;
        }

        public override void UpdateFileInfo(ProjectKey projectKey, DynamicDocumentContainer documentContainer)
        {
            _dynamicDocuments[documentContainer.FilePath] = documentContainer;
        }

        public override void UpdateLSPFileInfo(Uri documentUri, DynamicDocumentContainer documentContainer)
        {
            _dynamicDocuments[documentContainer.FilePath] = documentContainer;
        }
    }
}
