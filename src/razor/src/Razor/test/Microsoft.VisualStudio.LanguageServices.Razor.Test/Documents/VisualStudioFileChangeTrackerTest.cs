// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Threading;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using Moq;
using Xunit;
using Xunit.Abstractions;
using Task = System.Threading.Tasks.Task;

namespace Microsoft.VisualStudio.Editor.Razor.Documents;

public class VisualStudioFileChangeTrackerTest : ProjectSnapshotManagerDispatcherTestBase
{
    public VisualStudioFileChangeTrackerTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [UIFact]
    public async Task StartListening_AdvisesForFileChange()
    {
        // Arrange
        var fileChangeService = new Mock<IVsAsyncFileChangeEx>(MockBehavior.Strict);
        fileChangeService
            .Setup(f => f.AdviseFileChangeAsync(It.IsAny<string>(), It.IsAny<_VSFILECHANGEFLAGS>(), It.IsAny<IVsFreeThreadedFileChangeEvents2>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(123u)
            .Verifiable();
        var tracker = new VisualStudioFileChangeTracker(TestProjectData.SomeProjectImportFile.FilePath, ErrorReporter, fileChangeService.Object, Dispatcher, JoinableTaskFactory.Context);

        // Act
        tracker.StartListening();
        await tracker._fileChangeAdviseTask;

        // Assert
        fileChangeService.Verify();
    }

    [UIFact]
    public async Task StartListening_AlreadyListening_DoesNothing()
    {
        // Arrange
        var callCount = 0;
        var fileChangeService = new Mock<IVsAsyncFileChangeEx>(MockBehavior.Strict);
        fileChangeService
            .Setup(f => f.AdviseFileChangeAsync(It.IsAny<string>(), It.IsAny<_VSFILECHANGEFLAGS>(), It.IsAny<IVsFreeThreadedFileChangeEvents2>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(123u)
            .Callback(() => callCount++);
        var tracker = new VisualStudioFileChangeTracker(TestProjectData.SomeProjectImportFile.FilePath, ErrorReporter, fileChangeService.Object, Dispatcher, JoinableTaskFactory.Context);
        tracker.StartListening();

        // Act
        tracker.StartListening();
        await tracker._fileChangeAdviseTask;

        // Assert
        Assert.Equal(1, callCount);
    }

    [UIFact]
    public async Task StopListening_UnadvisesForFileChange()
    {
        // Arrange
        var fileChangeService = new Mock<IVsAsyncFileChangeEx>(MockBehavior.Strict);
        fileChangeService
            .Setup(f => f.AdviseFileChangeAsync(It.IsAny<string>(), It.IsAny<_VSFILECHANGEFLAGS>(), It.IsAny<IVsFreeThreadedFileChangeEvents2>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(123u)
            .Verifiable();
        fileChangeService
            .Setup(f => f.UnadviseFileChangeAsync(123, It.IsAny<CancellationToken>()))
            .Verifiable();
        var tracker = new VisualStudioFileChangeTracker(TestProjectData.SomeProjectImportFile.FilePath, ErrorReporter, fileChangeService.Object, Dispatcher, JoinableTaskFactory.Context);
        tracker.StartListening(); // Start listening for changes.
        await tracker._fileChangeAdviseTask;

        // Act
        tracker.StopListening();
        await tracker._fileChangeUnadviseTask;

        // Assert
        fileChangeService.Verify();
    }

    [UIFact]
    public void StopListening_NotListening_DoesNothing()
    {
        // Arrange
        var fileChangeService = new Mock<IVsAsyncFileChangeEx>(MockBehavior.Strict);
        fileChangeService
            .Setup(f => f.UnadviseFileChangeAsync(123, It.IsAny<CancellationToken>()))
            .Throws(new InvalidOperationException());
        var tracker = new VisualStudioFileChangeTracker(TestProjectData.SomeProjectImportFile.FilePath, ErrorReporter, fileChangeService.Object, Dispatcher, JoinableTaskFactory.Context);

        // Act
        tracker.StopListening();

        // Assert
        Assert.Null(tracker._fileChangeUnadviseTask);
    }

    [UITheory]
    [InlineData((uint)_VSFILECHANGEFLAGS.VSFILECHG_Size, (int)FileChangeKind.Changed)]
    [InlineData((uint)_VSFILECHANGEFLAGS.VSFILECHG_Time, (int)FileChangeKind.Changed)]
    [InlineData((uint)_VSFILECHANGEFLAGS.VSFILECHG_Add, (int)FileChangeKind.Added)]
    [InlineData((uint)_VSFILECHANGEFLAGS.VSFILECHG_Del, (int)FileChangeKind.Removed)]
    public async Task FilesChanged_WithSpecificFlags_InvokesChangedHandler_WithExpectedArguments(uint fileChangeFlag, int expectedKind)
    {
        // Arrange
        var filePath = TestProjectData.SomeProjectImportFile.FilePath;
        var fileChangeService = Mock.Of<IVsAsyncFileChangeEx>(MockBehavior.Strict);
        var tracker = new VisualStudioFileChangeTracker(filePath, ErrorReporter, fileChangeService, Dispatcher, JoinableTaskFactory.Context);

        var called = false;
        tracker.Changed += (sender, args) =>
        {
            called = true;
            Assert.Same(sender, tracker);
            Assert.Equal(filePath, args.FilePath);
            Assert.Equal((FileChangeKind)expectedKind, args.Kind);
        };

        // Act
        tracker.FilesChanged(fileCount: 1, filePaths: new[] { filePath }, fileChangeFlags: new[] { fileChangeFlag });
        await tracker._fileChangedTask;

        // Assert
        Assert.True(called);
    }
}
