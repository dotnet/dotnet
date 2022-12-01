// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class ProjectFileSynchronizerTest : LanguageServerTestBase
{
    public ProjectFileSynchronizerTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public void ProjectFileChanged_Added_AddsProject()
    {
        // Arrange
        var projectPath = "/path/to/project.csproj";
        var projectService = new Mock<RazorProjectService>(MockBehavior.Strict);
        projectService.Setup(service => service.AddProject(projectPath)).Verifiable();
        var synchronizer = new ProjectFileSynchronizer(LegacyDispatcher, projectService.Object);

        // Act
        synchronizer.ProjectFileChanged(projectPath, RazorFileChangeKind.Added);

        // Assert
        projectService.VerifyAll();
    }

    [Fact]
    public void ProjectFileChanged_Removed_RemovesProject()
    {
        // Arrange
        var projectPath = "/path/to/project.csproj";
        var projectService = new Mock<RazorProjectService>(MockBehavior.Strict);
        projectService.Setup(service => service.RemoveProject(projectPath)).Verifiable();
        var synchronizer = new ProjectFileSynchronizer(LegacyDispatcher, projectService.Object);

        // Act
        synchronizer.ProjectFileChanged(projectPath, RazorFileChangeKind.Removed);

        // Assert
        projectService.VerifyAll();
    }
}
