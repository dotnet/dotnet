// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class RazorFileChangeDetectorManagerTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    [Fact]
    public async Task InitializedAsync_StartsFileChangeDetectors()
    {
        // Arrange
        var initialWorkspaceDirectory = "testpath";

        var uriBuilder = new UriBuilder
        {
            Scheme = "file",
            Host = null,
            Path = initialWorkspaceDirectory
        };

        var clientSettings = new InitializeParams()
        {
            RootUri = uriBuilder.Uri,
        };

        var languageServerMock = new StrictMock<IInitializeManager<InitializeParams, InitializeResult>>();
        languageServerMock
            .Setup(s => s.GetInitializeParams())
            .Returns(clientSettings);

        var expectedWorkspaceDirectory = $"\\\\{initialWorkspaceDirectory}";

        var detectorMock1 = new StrictMock<IFileChangeDetector>();
        detectorMock1
            .Setup(detector => detector.StartAsync(expectedWorkspaceDirectory, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        detectorMock1.Setup(x => x.Stop());

        var detectorMock2 = new StrictMock<IFileChangeDetector>();
        detectorMock2
            .Setup(detector => detector.StartAsync(expectedWorkspaceDirectory, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        detectorMock2.Setup(x => x.Stop());

        var workspaceDirectoryPathResolver = new DefaultWorkspaceDirectoryPathResolver(languageServerMock.Object);
        using (var detectorManager = new RazorFileChangeDetectorManager(workspaceDirectoryPathResolver, [detectorMock1.Object, detectorMock2.Object]))
        {
            // Act
            await detectorManager.OnInitializedAsync(StrictMock.Of<ILspServices>(), DisposalToken);
        }

        // Assert
        detectorMock1.VerifyAll();
        detectorMock2.VerifyAll();
        languageServerMock.VerifyAll();
    }

    [Fact]
    public async Task InitializedAsync_Disposed_ReStopsFileChangeDetectors()
    {
        // Arrange
        var expectedWorkspaceDirectory = "\\\\testpath";
        var clientSettings = new InitializeParams()
        {
            RootUri = new Uri(expectedWorkspaceDirectory),
        };

        var languageServerMock = new StrictMock<IInitializeManager<InitializeParams, InitializeResult>>();
        languageServerMock
            .Setup(s => s.GetInitializeParams())
            .Returns(clientSettings);

        var detectorMock = new StrictMock<IFileChangeDetector>();
        var cts = new TaskCompletionSource<bool>();
        detectorMock
            .Setup(d => d.StartAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(cts.Task);
        var stopCount = 0;
        detectorMock
            .Setup(d => d.Stop())
            .Callback(() => stopCount++);

        var workspaceDirectoryPathResolver = new DefaultWorkspaceDirectoryPathResolver(languageServerMock.Object);
        using var detectorManager = new RazorFileChangeDetectorManager(workspaceDirectoryPathResolver, [detectorMock.Object]);

        // Act
        var initializeTask = detectorManager.OnInitializedAsync(StrictMock.Of<ILspServices>(), DisposalToken);
        detectorManager.Dispose();

        // Unblock the detector start
        cts.SetResult(true);
        await initializeTask;

        // Assert
        Assert.Equal(2, stopCount);

        languageServerMock.VerifyAll();
    }
}
