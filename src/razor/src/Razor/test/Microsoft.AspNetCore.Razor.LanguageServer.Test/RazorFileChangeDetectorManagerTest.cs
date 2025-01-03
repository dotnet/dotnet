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

        var initializeParams = new InitializeParams()
        {
            RootUri = VsLspFactory.CreateFilePathUri(initialWorkspaceDirectory),
        };

        var capabilitiesManager = new CapabilitiesManager(StrictMock.Of<ILspServices>());
        capabilitiesManager.SetInitializeParams(initializeParams);

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

        using (var detectorManager = new RazorFileChangeDetectorManager(capabilitiesManager, [detectorMock1.Object, detectorMock2.Object]))
        {
            // Act
            await detectorManager.OnInitializedAsync(StrictMock.Of<ILspServices>(), DisposalToken);
        }

        // Assert
        detectorMock1.VerifyAll();
        detectorMock2.VerifyAll();
    }

    [Fact]
    public async Task InitializedAsync_Disposed_ReStopsFileChangeDetectors()
    {
        // Arrange
        var expectedWorkspaceDirectory = "\\\\testpath";
        var initializeParams = new InitializeParams()
        {
            RootUri = new Uri(expectedWorkspaceDirectory),
        };

        var capabilitiesManager = new CapabilitiesManager(StrictMock.Of<ILspServices>());
        capabilitiesManager.SetInitializeParams(initializeParams);

        var detectorMock = new StrictMock<IFileChangeDetector>();
        var cts = new TaskCompletionSource<bool>();
        detectorMock
            .Setup(d => d.StartAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(cts.Task);
        var stopCount = 0;
        detectorMock
            .Setup(d => d.Stop())
            .Callback(() => stopCount++);

        using var detectorManager = new RazorFileChangeDetectorManager(capabilitiesManager, [detectorMock.Object]);

        // Act
        var initializeTask = detectorManager.OnInitializedAsync(StrictMock.Of<ILspServices>(), DisposalToken);
        detectorManager.Dispose();

        // Unblock the detector start
        cts.SetResult(true);
        await initializeTask;

        // Assert
        Assert.Equal(2, stopCount);
    }
}
