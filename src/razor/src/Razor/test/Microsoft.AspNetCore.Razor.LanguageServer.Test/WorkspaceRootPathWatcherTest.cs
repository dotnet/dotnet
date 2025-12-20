// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
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

#pragma warning disable CS0618 // Type or member is obsolete
        var initializeParams = new InitializeParams()
        {
            RootUri = LspFactory.CreateFilePathUri(initialWorkspaceDirectory),
        };
#pragma warning restore CS0618 // Type or member is obsolete

        var capabilitiesManager = new CapabilitiesManager(LspServices.Empty);
        capabilitiesManager.SetInitializeParams(initializeParams);

        var expectedWorkspaceDirectory = $"\\\\{initialWorkspaceDirectory}";

        var started = false;

        using var watcher = new TestWorkspaceRootPathWatcher(
            capabilitiesManager, StrictMock.Of<IRazorProjectService>(),
            onStartAsync: (workspaceDirectory, _) =>
            {
                started = true;
                Assert.Equal(expectedWorkspaceDirectory, workspaceDirectory);
                return Task.CompletedTask;
            });

        // Act
        await watcher.OnInitializedAsync(DisposalToken);

        // Assert
        Assert.True(started);
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
            projectServiceMock.Object);

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
        Func<string, CancellationToken, Task>? onStartAsync = null)
        : WorkspaceRootPathWatcher(workspaceRootPathProvider, projectService, delay: TimeSpan.Zero)
    {
        protected override Task StartAsync(string workspaceDirectory, CancellationToken cancellationToken)
        {
            return onStartAsync is not null
                ? onStartAsync.Invoke(workspaceDirectory, cancellationToken)
                : base.StartAsync(workspaceDirectory, cancellationToken);
        }

        protected override bool InitializeFileWatchers => false;
    }
}
