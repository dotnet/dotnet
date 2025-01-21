// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

public class IProjectSnapshotManagerExtensionsTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    [Fact]
    public async Task TryResolveDocumentInAnyProject_AsksPotentialParentProjectForDocumentItsTracking_ReturnsTrue()
    {
        // Arrange
        var documentFilePath = @"C:\path\to\document.cshtml";
        var normalizedFilePath = "C:/path/to/document.cshtml";
        var projectManager = await CreateProjectManagerAsync(normalizedFilePath);

        // Act
        Assert.True(projectManager.TryResolveDocumentInAnyProject(documentFilePath, Logger, out var document));

        // Assert
        Assert.Equal(normalizedFilePath, document.FilePath);
    }

    [Fact]
    public async Task TryResolveDocumentInAnyProject_AsksMiscellaneousProjectForDocumentItIsTracking_ReturnsTrue()
    {
        // Arrange
        var documentFilePath = @"C:\path\to\document.cshtml";
        var normalizedFilePath = "C:/path/to/document.cshtml";
        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(updater =>
        {
            var hostProject = MiscFilesProject.HostProject with { Configuration = FallbackRazorConfiguration.Latest };
            var hostDocument = new HostDocument(normalizedFilePath, targetPath: "document.cshtml");

            updater.AddDocument(hostProject.Key, hostDocument, EmptyTextLoader.Instance);
        });

        // Act
        Assert.True(projectManager.TryResolveDocumentInAnyProject(documentFilePath, Logger, out var document));

        // Assert
        Assert.Equal(normalizedFilePath, document.FilePath);
    }

    [Fact]
    public void TryResolveDocumentInAnyProject_AsksPotentialParentProjectForDocumentItsNotTrackingAndMiscellaneousProjectIsNotTrackingEither_ReturnsFalse()
    {
        // Arrange
        var documentFilePath = @"C:\path\to\document.cshtml";
        var projectManager = CreateProjectSnapshotManager();

        // Act
        Assert.False(projectManager.TryResolveDocumentInAnyProject(documentFilePath, Logger, out var document));

        // Assert
        Assert.Null(document);
    }

    [Fact]
    public void TryResolveAllProjects_NoProjects_ReturnsFalse()
    {
        // Arrange
        var documentFilePath = "C:/path/to/document.cshtml";
        var projectManager = CreateProjectSnapshotManager();

        // Act
        Assert.False(projectManager.TryResolveAllProjects(documentFilePath, out _));
    }

    [Fact]
    public void TryResolveAllProjects_OnlyMiscellaneousProjectDoesNotContainDocument_ReturnsFalse()
    {
        // Arrange
        var documentFilePath = "C:/path/to/document.cshtml";
        var projectManager = CreateProjectSnapshotManager();

        // Act
        Assert.False(projectManager.TryResolveAllProjects(documentFilePath, out _));
    }

    [Fact]
    public async Task TryResolveAllProjects_OnlyMiscellaneousProjectContainsDocument_ReturnsTrue()
    {
        // Arrange
        var documentFilePath = Path.Combine(MiscFilesProject.DirectoryPath, "document.cshtml");
        var projectManager = await CreateProjectManagerAsync(documentFilePath, addToMiscellaneous: true);

        // Act
        Assert.True(projectManager.TryResolveAllProjects(documentFilePath, out var projects));

        // Assert
        var miscFilesProject = projectManager.GetMiscellaneousProject();
        Assert.Single(projects, miscFilesProject);
    }

    [Fact]
    public async Task TryResolveAllProjects_UnrelatedProject_ReturnsFalse()
    {
        // Arrange
        var documentFilePath = "C:/path/to/document.cshtml";
        var hostProject = TestHostProject.Create("C:/other/path/to/project.csproj");

        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(hostProject);
        });

        // Act
        Assert.False(projectManager.TryResolveAllProjects(documentFilePath, out _));
    }

    [Fact]
    public async Task TryResolveAllProjects_OwnerProjectWithOthers_ReturnsTrue()
    {
        // Arrange
        var hostProject = TestHostProject.Create("C:/path/to/project.csproj");
        var hostDocument = TestHostDocument.Create(hostProject, "C:/path/to/document.cshtml");
        var otherHostProject = TestHostProject.Create("C:/path/to/other/project.csproj");

        var projectManager = CreateProjectSnapshotManager();

        var expectedProject = await projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(hostProject);
            updater.AddProject(otherHostProject);
            updater.AddDocument(hostProject.Key, hostDocument, EmptyTextLoader.Instance);

            return updater.GetRequiredProject(hostProject.Key);
        });

        // Act
        Assert.True(projectManager.TryResolveAllProjects(hostDocument.FilePath, out var projects));

        // Assert
        var project = Assert.Single(projects);
        AssertSnapshotsEqual(expectedProject, project);
    }

    [Fact]
    public async Task TryResolveAllProjects_MiscellaneousOwnerProjectWithOthers_ReturnsTrue()
    {
        // Arrange
        var miscFilesHostProject = MiscFilesProject.HostProject;
        var documentFilePath = Path.Combine(MiscFilesProject.DirectoryPath, "file.cshtml");
        documentFilePath = FilePathNormalizer.Normalize(documentFilePath);

        var hostDocument = TestHostDocument.Create(miscFilesHostProject, documentFilePath);
        var hostProject = TestHostProject.Create("C:/path/to/project.csproj");

        var projectManager = CreateProjectSnapshotManager();

        var miscProject = await projectManager.UpdateAsync(updater =>
        {
            updater.AddDocument(miscFilesHostProject.Key, hostDocument, EmptyTextLoader.Instance);
            updater.AddProject(hostProject);

            return updater.GetRequiredProject(miscFilesHostProject.Key);
        });

        // Act
        Assert.True(projectManager.TryResolveAllProjects(documentFilePath, out var projects));

        // Assert
        var project = Assert.Single(projects);
        AssertSnapshotsEqual(miscProject, project);
    }

    [ConditionalFact(Is.Windows)]
    public async Task TryResolveAllProjects_OwnerProjectDifferentCasing_ReturnsTrue()
    {
        // Arrange
        var hostProject = TestHostProject.Create("C:/Path/To/project.csproj");
        var hostDocument = TestHostDocument.Create(hostProject, "c:/path/to/document.cshtml");

        var projectManager = CreateProjectSnapshotManager();

        var ownerProject = await projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(hostProject);
            updater.AddDocument(hostProject.Key, hostDocument, EmptyTextLoader.Instance);

            return updater.GetRequiredProject(hostProject.Key);
        });

        // Act
        Assert.True(projectManager.TryResolveAllProjects(hostDocument.FilePath, out var projects));

        // Assert
        var project = Assert.Single(projects);
        AssertSnapshotsEqual(ownerProject, project);
    }

    [Fact]
    public void GetMiscellaneousProject_ProjectLoaded_ReturnsExistingProject()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        // Act
        var project = projectManager.GetMiscellaneousProject();
        var inManager = projectManager.GetRequiredProject(MiscFilesProject.Key);

        // Assert
        Assert.Same(inManager, project);
    }

    [Fact]
    public void GetMiscellaneousProject_ProjectNotLoaded_CreatesProjectAndReturnsCreatedProject()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        // Act
        var project = projectManager.GetMiscellaneousProject();

        // Assert
        Assert.Single(projectManager.GetProjects());
        Assert.Equal(MiscFilesProject.FilePath, project.FilePath);
    }

    private async Task<TestProjectSnapshotManager> CreateProjectManagerAsync(string documentFilePath, bool addToMiscellaneous = false)
    {
        documentFilePath = FilePathNormalizer.Normalize(documentFilePath);

        var projectManager = CreateProjectSnapshotManager();

        HostProject hostProject;

        if (addToMiscellaneous)
        {
            hostProject = MiscFilesProject.HostProject;
        }
        else
        {
            var projectDirectory = FilePathNormalizer.GetNormalizedDirectoryName(documentFilePath);
            hostProject = TestHostProject.Create(Path.Combine(projectDirectory, "proj.csproj"));

            await projectManager.UpdateAsync(updater =>
            {
                updater.AddProject(hostProject);
            });
        }

        var hostDocument = TestHostDocument.Create(hostProject, documentFilePath);

        await projectManager.UpdateAsync(updater =>
        {
            updater.AddDocument(hostProject.Key, hostDocument, EmptyTextLoader.Instance);
        });

        return projectManager;
    }

    private static void AssertSnapshotsEqual(ProjectSnapshot first, ProjectSnapshot second)
    {
        Assert.Equal(first.FilePath, second.FilePath);
        Assert.Equal(first.CSharpLanguageVersion, second.CSharpLanguageVersion);
        Assert.Equal(first.RootNamespace, second.RootNamespace);
    }
}
