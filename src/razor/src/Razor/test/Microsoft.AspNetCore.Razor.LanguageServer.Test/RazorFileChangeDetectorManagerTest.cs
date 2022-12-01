// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class RazorFileChangeDetectorManagerTest
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
        var languageServer = new Mock<IInitializeManager<InitializeParams, InitializeResult>>(MockBehavior.Strict);
        languageServer.Setup(s => s.GetInitializeParams())
            .Returns(clientSettings);
        var detector1 = new Mock<IFileChangeDetector>(MockBehavior.Strict);
        var expectedWorkspaceDirectory = $"\\\\{initialWorkspaceDirectory}";
        detector1
            .Setup(detector => detector.StartAsync(expectedWorkspaceDirectory, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();
        var detector2 = new Mock<IFileChangeDetector>(MockBehavior.Strict);
        detector2
            .Setup(detector => detector.StartAsync(expectedWorkspaceDirectory, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();
        var workspaceDirectoryPathResolver = new DefaultWorkspaceDirectoryPathResolver(languageServer.Object);
        var detectorManager = new RazorFileChangeDetectorManager(workspaceDirectoryPathResolver, new[] { detector1.Object, detector2.Object });

        // Act
        await detectorManager.InitializedAsync();

        // Assert
        detector1.VerifyAll();
        detector2.VerifyAll();
        languageServer.VerifyAll();
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
        var languageServer = new Mock<IInitializeManager<InitializeParams, InitializeResult>>(MockBehavior.Strict);
        languageServer
            .Setup(s => s.GetInitializeParams())
            .Returns(clientSettings);

        var detector = new Mock<IFileChangeDetector>(MockBehavior.Strict);
        var cts = new TaskCompletionSource<bool>();
        detector.Setup(d => d.StartAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(cts.Task);
        var stopCount = 0;
        detector.Setup(d => d.Stop()).Callback(() => stopCount++);
        var workspaceDirectoryPathResolver = new DefaultWorkspaceDirectoryPathResolver(languageServer.Object);
        var detectorManager = new RazorFileChangeDetectorManager(workspaceDirectoryPathResolver, new[] { detector.Object });

        // Act
        var initializeTask = detectorManager.InitializedAsync();
        detectorManager.Dispose();

        // Unblock the detector start
        cts.SetResult(true);
        await initializeTask;

        // Assert
        Assert.Equal(2, stopCount);

        languageServer.VerifyAll();
    }
}
