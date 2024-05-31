// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test;

public class SnapshotResolverTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    [Fact]
    public async Task TryResolveDocumentInAnyProject_AsksPotentialParentProjectForDocumentItsTracking_ReturnsTrue()
    {
        // Arrange
        var documentFilePath = @"C:\path\to\document.cshtml";
        var normalizedFilePath = "C:/path/to/document.cshtml";
        var snapshotResolver = await CreateSnapshotResolverAsync(normalizedFilePath);

        // Act
        Assert.True(snapshotResolver.TryResolveDocumentInAnyProject(documentFilePath, out var document));

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
        var snapshotResolver = new SnapshotResolver(projectManager, LoggerFactory);
        await snapshotResolver.OnInitializedAsync(StrictMock.Of<ILspServices>(), DisposalToken);

        await projectManager.UpdateAsync(updater =>
        {
            var miscProject = snapshotResolver.GetMiscellaneousProject();
            var hostProject = new HostProject(miscProject.FilePath, miscProject.IntermediateOutputPath, FallbackRazorConfiguration.Latest, miscProject.RootNamespace);
            updater.DocumentAdded(
                hostProject.Key,
                new HostDocument(normalizedFilePath, "document.cshtml"),
                new EmptyTextLoader(normalizedFilePath));
        });

        // Act
        Assert.True(snapshotResolver.TryResolveDocumentInAnyProject(documentFilePath, out var document));

        // Assert
        Assert.Equal(normalizedFilePath, document.FilePath);
    }

    [Fact]
    public async Task TryResolveDocumentInAnyProject_AsksPotentialParentProjectForDocumentItsNotTrackingAndMiscellaneousProjectIsNotTrackingEither_ReturnsFalse()
    {
        // Arrange
        var documentFilePath = @"C:\path\to\document.cshtml";
        var projectManager = CreateProjectSnapshotManager();
        var snapshotResolver = new SnapshotResolver(projectManager, LoggerFactory);
        await snapshotResolver.OnInitializedAsync(StrictMock.Of<ILspServices>(), DisposalToken);

        // Act
        Assert.False(snapshotResolver.TryResolveDocumentInAnyProject(documentFilePath, out var document));

        // Assert
        Assert.Null(document);
    }

    [Fact]
    public async Task TryResolveAllProjects_NoProjects_ReturnsFalse()
    {
        // Arrange
        var documentFilePath = "C:/path/to/document.cshtml";
        var projectManager = CreateProjectSnapshotManager();
        var snapshotResolver = new SnapshotResolver(projectManager, LoggerFactory);
        await snapshotResolver.OnInitializedAsync(StrictMock.Of<ILspServices>(), DisposalToken);

        // Act
        Assert.False(snapshotResolver.TryResolveAllProjects(documentFilePath, out _));
    }

    [Fact]
    public async Task TryResolveAllProjects_OnlyMiscellaneousProjectDoesNotContainDocument_ReturnsFalse()
    {
        // Arrange
        var documentFilePath = "C:/path/to/document.cshtml";
        var projectManager = CreateProjectSnapshotManager();
        var snapshotResolver = new SnapshotResolver(projectManager, LoggerFactory);
        await snapshotResolver.OnInitializedAsync(StrictMock.Of<ILspServices>(), DisposalToken);

        // Act
        Assert.False(snapshotResolver.TryResolveAllProjects(documentFilePath, out _));
    }

    [Fact]
    public async Task TryResolveAllProjects_OnlyMiscellaneousProjectContainsDocument_ReturnsTrue()
    {
        // Arrange
        var documentFilePath = Path.Combine(MiscFilesHostProject.Instance.DirectoryPath, "document.cshtml");
        var snapshotResolver = await CreateSnapshotResolverAsync(documentFilePath, addToMiscellaneous: true);
        await snapshotResolver.OnInitializedAsync(StrictMock.Of<ILspServices>(), DisposalToken);

        // Act
        Assert.True(snapshotResolver.TryResolveAllProjects(documentFilePath, out var projects));

        // Assert
        var miscFilesProject = snapshotResolver.GetMiscellaneousProject();
        Assert.Single(projects, miscFilesProject);
    }

    [Fact]
    public async Task TryResolveAllProjects_UnrelatedProject_ReturnsFalse()
    {
        // Arrange
        var documentFilePath = "C:/path/to/document.cshtml";
        var projectManager = CreateProjectSnapshotManager();
        var snapshotResolver = new SnapshotResolver(projectManager, LoggerFactory);
        await snapshotResolver.OnInitializedAsync(StrictMock.Of<ILspServices>(), DisposalToken);

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(TestProjectSnapshot.Create("C:/other/path/to/project.csproj").HostProject);
        });

        // Act
        Assert.False(snapshotResolver.TryResolveAllProjects(documentFilePath, out _));
    }

    [Fact]
    public async Task TryResolveAllProjects_OwnerProjectWithOthers_ReturnsTrue()
    {
        // Arrange
        var documentFilePath = "C:/path/to/document.cshtml";
        var projectManager = CreateProjectSnapshotManager();

        var snapshotResolver = new SnapshotResolver(projectManager, LoggerFactory);
        await snapshotResolver.OnInitializedAsync(StrictMock.Of<ILspServices>(), DisposalToken);

        var expectedProject = await projectManager.UpdateAsync(updater =>
        {
            var expectedProject = updater.CreateAndAddProject("C:/path/to/project.csproj");
            updater.CreateAndAddProject("C:/path/to/other/project.csproj");
            updater.CreateAndAddDocument(expectedProject, documentFilePath);

            return expectedProject;
        });

        // Act
        Assert.True(snapshotResolver.TryResolveAllProjects(documentFilePath, out var projects));

        // Assert
        var project = Assert.Single(projects);
        AssertSnapshotsEqual(expectedProject, project);
    }

    [Fact]
    public async Task TryResolveAllProjects_MiscellaneousOwnerProjectWithOthers_ReturnsTrue()
    {
        // Arrange
        var documentFilePath = Path.Combine(MiscFilesHostProject.Instance.DirectoryPath, "file.cshtml");
        documentFilePath = FilePathNormalizer.Normalize(documentFilePath);

        var projectManager = CreateProjectSnapshotManager();
        var snapshotResolver = new SnapshotResolver(projectManager, LoggerFactory);
        await snapshotResolver.OnInitializedAsync(StrictMock.Of<ILspServices>(), DisposalToken);

        var miscProject = await projectManager.UpdateAsync(updater =>
        {
            var miscProject = (ProjectSnapshot)snapshotResolver.GetMiscellaneousProject();
            updater.CreateAndAddDocument(miscProject, documentFilePath);
            updater.CreateAndAddProject("C:/path/to/project.csproj");

            return miscProject;
        });

        // Act
        Assert.True(snapshotResolver.TryResolveAllProjects(documentFilePath, out var projects));

        // Assert
        var project = Assert.Single(projects);
        AssertSnapshotsEqual(miscProject, project);
    }

    [OSSkipConditionFact(["OSX", "Linux"])]
    public async Task TryResolveAllProjects_OwnerProjectDifferentCasing_ReturnsTrue()
    {
        // Arrange
        var documentFilePath = "c:/path/to/document.cshtml";
        var projectManager = CreateProjectSnapshotManager();
        var snapshotResolver = new SnapshotResolver(projectManager, LoggerFactory);
        await snapshotResolver.OnInitializedAsync(StrictMock.Of<ILspServices>(), DisposalToken);

        var ownerProject = await projectManager.UpdateAsync(updater =>
        {
            var ownerProject = updater.CreateAndAddProject("C:/Path/To/project.csproj");
            updater.CreateAndAddDocument(ownerProject, documentFilePath);

            return ownerProject;
        });

        // Act
        Assert.True(snapshotResolver.TryResolveAllProjects(documentFilePath, out var projects));

        // Assert
        var project = Assert.Single(projects);
        AssertSnapshotsEqual(ownerProject, project);
    }

    [Fact]
    public async Task GetMiscellaneousProject_ProjectLoaded_ReturnsExistingProject()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var snapshotResolver = new SnapshotResolver(projectManager, LoggerFactory);
        await snapshotResolver.OnInitializedAsync(StrictMock.Of<ILspServices>(), DisposalToken);

        // Act
        var project = snapshotResolver.GetMiscellaneousProject();
        var inManager = projectManager.GetLoadedProject(MiscFilesHostProject.Instance.Key);

        // Assert
        Assert.Same(inManager, project);
    }

    [Fact]
    public async Task GetMiscellaneousProject_ProjectNotLoaded_CreatesProjectAndReturnsCreatedProject()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var snapshotResolver = new SnapshotResolver(projectManager, LoggerFactory);
        await snapshotResolver.OnInitializedAsync(StrictMock.Of<ILspServices>(), DisposalToken);

        // Act
        var project = snapshotResolver.GetMiscellaneousProject();

        // Assert
        Assert.Single(projectManager.GetProjects());
        Assert.Equal(MiscFilesHostProject.Instance.FilePath, project.FilePath);
    }

    private async Task<SnapshotResolver> CreateSnapshotResolverAsync(string filePath, bool addToMiscellaneous = false)
    {
        filePath = FilePathNormalizer.Normalize(filePath);

        var projectManager = CreateProjectSnapshotManager();
        var snapshotResolver = new SnapshotResolver(projectManager, LoggerFactory);
        await snapshotResolver.OnInitializedAsync(StrictMock.Of<ILspServices>(), DisposalToken);

        if (addToMiscellaneous)
        {
            await projectManager.UpdateAsync(updater =>
            {
                var miscProject = (ProjectSnapshot)snapshotResolver.GetMiscellaneousProject();
                updater.CreateAndAddDocument(miscProject, filePath);
            });
        }
        else
        {
            var projectDirectory = FilePathNormalizer.GetNormalizedDirectoryName(filePath);
            var projectSnapshot = TestProjectSnapshot.Create(Path.Combine(projectDirectory, "proj.csproj"));

            await projectManager.UpdateAsync(updater =>
            {
                updater.ProjectAdded(projectSnapshot.HostProject);
                updater.CreateAndAddDocument(projectSnapshot, filePath);
            });
        }

        return snapshotResolver;
    }

    private static void AssertSnapshotsEqual(IProjectSnapshot first, IProjectSnapshot second)
    {
        Assert.Equal(first.FilePath, second.FilePath);
        Assert.Equal(first.CSharpLanguageVersion, second.CSharpLanguageVersion);
        Assert.Equal(first.RootNamespace, second.RootNamespace);
    }
}
