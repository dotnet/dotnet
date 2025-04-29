// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class WorkspaceRootPathWatcherTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    [Fact]
    public async Task InitializedAsync_CallsStartAsync()
    {
        // Arrange
        var initialWorkspaceDirectory = "testpath";

        var initializeParams = new InitializeParams()
        {
            RootUri = VsLspFactory.CreateFilePathUri(initialWorkspaceDirectory),
        };

        var capabilitiesManager = new CapabilitiesManager(StrictMock.Of<ILspServices>());
        capabilitiesManager.SetInitializeParams(initializeParams);

        var expectedWorkspaceDirectory = $"\\\\{initialWorkspaceDirectory}";

        var started = false;

        using var watcher = new TestWorkspaceRootPathWatcher(
            capabilitiesManager, StrictMock.Of<IRazorProjectService>(), StrictMock.Of<IFileSystem>(), LoggerFactory,
            onStartAsync: (workspaceDirectory, _) =>
            {
                started = true;
                Assert.Equal(expectedWorkspaceDirectory, workspaceDirectory);
                return Task.CompletedTask;
            });

        // Act
        await watcher.OnInitializedAsync(StrictMock.Of<ILspServices>(), DisposalToken);

        // Assert
        Assert.True(started);
    }

    [Fact]
    public async Task StartAsync_NotifiesProjectServiceOfExistingRazorFiles()
    {
        // Arrange
        var actual = new List<string>();

        var projectServiceMock = new StrictMock<IRazorProjectService>();
        projectServiceMock
            .Setup(x => x.AddDocumentsToMiscProjectAsync(It.IsAny<ImmutableArray<string>>(), It.IsAny<CancellationToken>()))
            .Callback((ImmutableArray<string> filePaths, CancellationToken _) => actual.AddRange(filePaths))
            .Returns(Task.CompletedTask);

        var workspaceRootPathProviderMock = new StrictMock<IWorkspaceRootPathProvider>();
        workspaceRootPathProviderMock
            .Setup(x => x.GetRootPathAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync("/some/workspacedirectory");

        ImmutableArray<string> existingRazorFiles = ["c:/path/to/index.razor", "c:/other/path/_Host.cshtml"];

        using var watcher = new TestWorkspaceRootPathWatcher(
            workspaceRootPathProviderMock.Object,
            projectServiceMock.Object,
            StrictMock.Of<IFileSystem>(),
            LoggerFactory,
            existingRazorFiles);

        // Act
        await watcher.OnInitializedAsync(StrictMock.Of<ILspServices>(), DisposalToken);

        // Assert
        Assert.Equal(existingRazorFiles, actual);
    }

    [Theory]
    [MemberData(nameof(NotificationBehaviorData))]
    internal async Task TestNotificationBehavior((string, RazorFileChangeKind)[] work, (string, RazorFileChangeKind)[] expected)
    {
        var actual = new List<(string, RazorFileChangeKind)>();

        var projectServiceMock = new StrictMock<IRazorProjectService>();
        projectServiceMock
            .Setup(x => x.AddDocumentToMiscProjectAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback((string filePath, CancellationToken _) => actual.Add((filePath, RazorFileChangeKind.Added)))
            .Returns(Task.CompletedTask);
        projectServiceMock
            .Setup(x => x.RemoveDocumentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback((string filePath, CancellationToken _) => actual.Add((filePath, RazorFileChangeKind.Removed)))
            .Returns(Task.CompletedTask);

        var workspaceRootPathProviderMock = new StrictMock<IWorkspaceRootPathProvider>();
        workspaceRootPathProviderMock
            .Setup(x => x.GetRootPathAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync("/some/workspacedirectory");

        using var watcher = new TestWorkspaceRootPathWatcher(
            workspaceRootPathProviderMock.Object,
            projectServiceMock.Object,
            StrictMock.Of<IFileSystem>(),
            LoggerFactory);

        var watcherAccessor = watcher.GetTestAccessor();

        watcherAccessor.AddWork(work);

        await watcherAccessor.WaitUntilCurrentBatchCompletesAsync();

        Assert.Equal(expected, actual);
    }

    public static TheoryData NotificationBehaviorData
    {
        get
        {
            const string File1 = "C:/path/to/file1.razor";
            const string File2 = "C:/path/to/file2.razor";

            const RazorFileChangeKind Add = RazorFileChangeKind.Added;
            const RazorFileChangeKind Remove = RazorFileChangeKind.Removed;

            return new TheoryData<(string, RazorFileChangeKind)[], (string, RazorFileChangeKind)[]>
            {
                { [(File1, Add)], [(File1, Add)] },
                { [(File1, Add), (File1, Remove)], [] },
                { [(File1, Remove), (File1, Add)], [] },
                { [(File1, Add), (File1, Remove), (File1, Add)], [(File1, Add)] },
                { [(File1, Remove), (File1, Add), (File1, Remove)], [(File1, Remove)] },
                { [(File1, Add), (File2, Remove)], [(File1, Add), (File2, Remove)] },
                { [(File1, Add), (File1, Remove), (File2, Remove)], [(File2, Remove)] },
            };
        }
    }

    private class TestWorkspaceRootPathWatcher(
        IWorkspaceRootPathProvider workspaceRootPathProvider,
        IRazorProjectService projectService,
        IFileSystem fileSystem,
        ILoggerFactory loggerFactory,
        ImmutableArray<string> existingRazorFiles = default,
        Func<string, CancellationToken, Task>? onStartAsync = null)
        : WorkspaceRootPathWatcher(workspaceRootPathProvider, projectService, TestLanguageServerFeatureOptions.Instance, fileSystem, loggerFactory, delay: TimeSpan.Zero)
    {
        protected override Task StartAsync(string workspaceDirectory, CancellationToken cancellationToken)
        {
            return onStartAsync is not null
                ? onStartAsync.Invoke(workspaceDirectory, cancellationToken)
                : base.StartAsync(workspaceDirectory, cancellationToken);
        }

        protected override bool InitializeFileWatchers => false;

        protected override ImmutableArray<string> GetExistingRazorFiles(string workspaceDirectory)
            => existingRazorFiles.NullToEmpty();
    }
}
