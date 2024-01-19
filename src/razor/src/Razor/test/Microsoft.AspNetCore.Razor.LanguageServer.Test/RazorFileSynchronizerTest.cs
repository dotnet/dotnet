// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class RazorFileSynchronizerTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    [Fact]
    public async Task RazorFileChanged_Added_AddsRazorDocument()
    {
        // Arrange
        var filePath = "/path/to/file.razor";
        var projectService = new Mock<IRazorProjectService>(MockBehavior.Strict);
        projectService.Setup(service => service.AddDocument(filePath)).Verifiable();
        var synchronizer = new RazorFileSynchronizer(Dispatcher, projectService.Object);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            synchronizer.RazorFileChanged(filePath, RazorFileChangeKind.Added));

        // Assert
        projectService.VerifyAll();
    }

    [Fact]
    public async Task RazorFileChanged_Added_AddsCSHTMLDocument()
    {
        // Arrange
        var filePath = "/path/to/file.cshtml";
        var projectService = new Mock<IRazorProjectService>(MockBehavior.Strict);
        projectService.Setup(service => service.AddDocument(filePath)).Verifiable();
        var synchronizer = new RazorFileSynchronizer(Dispatcher, projectService.Object);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            synchronizer.RazorFileChanged(filePath, RazorFileChangeKind.Added));

        // Assert
        projectService.VerifyAll();
    }

    [Fact]
    public async Task RazorFileChanged_Removed_RemovesRazorDocument()
    {
        // Arrange
        var filePath = "/path/to/file.razor";
        var projectService = new Mock<IRazorProjectService>(MockBehavior.Strict);
        projectService.Setup(service => service.RemoveDocument(filePath)).Verifiable();
        var synchronizer = new RazorFileSynchronizer(Dispatcher, projectService.Object);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            synchronizer.RazorFileChanged(filePath, RazorFileChangeKind.Removed));

        // Assert
        projectService.VerifyAll();
    }

    [Fact]
    public async Task RazorFileChanged_Removed_RemovesCSHTMLDocument()
    {
        // Arrange
        var filePath = "/path/to/file.cshtml";
        var projectService = new Mock<IRazorProjectService>(MockBehavior.Strict);
        projectService.Setup(service => service.RemoveDocument(filePath)).Verifiable();
        var synchronizer = new RazorFileSynchronizer(Dispatcher, projectService.Object);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            synchronizer.RazorFileChanged(filePath, RazorFileChangeKind.Removed));

        // Assert
        projectService.VerifyAll();
    }
}
