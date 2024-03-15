// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
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
        IDocumentSnapshot? document = null;
        var result = await RunOnDispatcherAsync(() =>
        {
            return snapshotResolver.TryResolveDocumentInAnyProject(documentFilePath, out document);
        });

        // Assert
        Assert.True(result);
        Assert.NotNull(document);
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

        await RunOnDispatcherAsync(() =>
        {
            var miscProject = snapshotResolver.GetMiscellaneousProject();
            var hostProject = new HostProject(miscProject.FilePath, miscProject.IntermediateOutputPath, FallbackRazorConfiguration.Latest, miscProject.RootNamespace);
            projectManager.DocumentAdded(
                hostProject.Key,
                new HostDocument(normalizedFilePath, "document.cshtml"),
                new EmptyTextLoader(normalizedFilePath));
        });

        // Act
        IDocumentSnapshot? document = null;
        var result = await RunOnDispatcherAsync(() =>
        {
            return snapshotResolver.TryResolveDocumentInAnyProject(documentFilePath, out document);
        });

        // Assert
        Assert.True(result);
        Assert.NotNull(document);
        Assert.Equal(normalizedFilePath, document.FilePath);
    }

    [Fact]
    public async Task TryResolveDocumentInAnyProject_AsksPotentialParentProjectForDocumentItsNotTrackingAndMiscellaneousProjectIsNotTrackingEither_ReturnsFalse()
    {
        // Arrange
        var documentFilePath = @"C:\path\to\document.cshtml";
        var projectManager = CreateProjectSnapshotManager();
        var snapshotResolver = new SnapshotResolver(projectManager, LoggerFactory);

        // Act
        IDocumentSnapshot? document = null;
        var result = await RunOnDispatcherAsync(() =>
        {
            return snapshotResolver.TryResolveDocumentInAnyProject(documentFilePath, out document);
        });

        // Assert
        Assert.False(result);
        Assert.Null(document);
    }

    [Fact]
    public async Task TryResolveAllProjects_NoProjects_ReturnsFalse()
    {
        // Arrange
        var documentFilePath = "C:/path/to/document.cshtml";
        var projectManager = CreateProjectSnapshotManager();
        var snapshotResolver = new SnapshotResolver(projectManager, LoggerFactory);

        // Act
        IProjectSnapshot[]? projects = null;
        var result = await RunOnDispatcherAsync(() =>
        {
            return snapshotResolver.TryResolveAllProjects(documentFilePath, out projects);
        });

        // Assert
        Assert.False(result);
        Assert.NotNull(projects);
        Assert.Empty(projects);
    }

    [Fact]
    public async Task TryResolveAllProjects_OnlyMiscellaneousProjectDoesNotContainDocument_ReturnsFalse()
    {
        // Arrange
        var documentFilePath = "C:/path/to/document.cshtml";
        var projectManager = CreateProjectSnapshotManager();
        var snapshotResolver = new SnapshotResolver(projectManager, LoggerFactory);

        await RunOnDispatcherAsync(() =>
        {
            _ = snapshotResolver.GetMiscellaneousProject();
        });

        // Act
        IProjectSnapshot[]? projects = null;
        var result = await RunOnDispatcherAsync(() =>
        {
            return snapshotResolver.TryResolveAllProjects(documentFilePath, out projects);
        });

        // Assert
        Assert.False(result);
        Assert.NotNull(projects);
        Assert.Empty(projects);
    }

    [Fact]
    public async Task TryResolveAllProjects_OnlyMiscellaneousProjectContainsDocument_ReturnsTrue()
    {
        // Arrange
        var documentFilePath = Path.Combine(TempDirectory.Instance.DirectoryPath, "document.cshtml");
        var snapshotResolver = await CreateSnapshotResolverAsync(documentFilePath, addToMiscellaneous: true);

        // Act
        IProjectSnapshot[]? projects = null;
        var result = await RunOnDispatcherAsync(() =>
        {
            return snapshotResolver.TryResolveAllProjects(documentFilePath, out projects);
        });

        // Assert
        Assert.True(result);
        Assert.NotNull(projects);
        Assert.Single(projects, snapshotResolver.GetMiscellaneousProject());
    }

    [Fact]
    public async Task TryResolveAllProjects_UnrelatedProject_ReturnsFalse()
    {
        // Arrange
        var documentFilePath = "C:/path/to/document.cshtml";
        var projectManager = CreateProjectSnapshotManager();
        var snapshotResolver = new SnapshotResolver(projectManager, LoggerFactory);

        await RunOnDispatcherAsync(() =>
        {
            projectManager.ProjectAdded(TestProjectSnapshot.Create("C:/other/path/to/project.csproj").HostProject);
        });

        // Act
        IProjectSnapshot[]? projects = null;
        var result = await RunOnDispatcherAsync(() =>
        {
            return snapshotResolver.TryResolveAllProjects(documentFilePath, out projects);
        });

        // Assert
        Assert.False(result);
        Assert.NotNull(projects);
        Assert.Empty(projects);
    }

    [Fact]
    public async Task TryResolveAllProjects_OwnerProjectWithOthers_ReturnsTrue()
    {
        // Arrange
        var documentFilePath = "C:/path/to/document.cshtml";
        var projectManager = CreateProjectSnapshotManager();

        var expectedProject = await RunOnDispatcherAsync(() =>
        {
            var expectedProject = projectManager.CreateAndAddProject("C:/path/to/project.csproj");
            projectManager.CreateAndAddProject("C:/path/to/other/project.csproj");
            projectManager.CreateAndAddDocument(expectedProject, documentFilePath);

            return expectedProject;
        });

        var snapshotResolver = new SnapshotResolver(projectManager, LoggerFactory);

        // Act
        IProjectSnapshot[]? projects = null;
        var result = await RunOnDispatcherAsync(() =>
        {
            return snapshotResolver.TryResolveAllProjects(documentFilePath, out projects);
        });

        // Assert
        Assert.True(result);
        Assert.NotNull(projects);
        var project = Assert.Single(projects);
        AssertSnapshotsEqual(expectedProject, project);
    }

    [Fact]
    public async Task TryResolveAllProjects_MiscellaneousOwnerProjectWithOthers_ReturnsTrue()
    {
        // Arrange
        var documentFilePath = Path.Combine(TempDirectory.Instance.DirectoryPath, "file.cshtml");
        documentFilePath = FilePathNormalizer.Normalize(documentFilePath);

        var projectManager = CreateProjectSnapshotManager();
        var snapshotResolver = new SnapshotResolver(projectManager, LoggerFactory);

        var miscProject = await RunOnDispatcherAsync(() =>
        {
            var miscProject = (ProjectSnapshot)snapshotResolver.GetMiscellaneousProject();
            projectManager.CreateAndAddDocument(miscProject, documentFilePath);
            projectManager.CreateAndAddProject("C:/path/to/project.csproj");

            return miscProject;
        });

        // Act
        IProjectSnapshot[]? projects = null;
        var result = await RunOnDispatcherAsync(() =>
        {
            return snapshotResolver.TryResolveAllProjects(documentFilePath, out projects);
        });

        // Assert
        Assert.True(result);
        Assert.NotNull(projects);
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

        var ownerProject = await RunOnDispatcherAsync(() =>
        {
            var ownerProject = projectManager.CreateAndAddProject("C:/Path/To/project.csproj");
            projectManager.CreateAndAddDocument(ownerProject, documentFilePath);

            return ownerProject;
        });

        // Act
        IProjectSnapshot[]? projects = null;
        var result = await RunOnDispatcherAsync(() =>
        {
            return snapshotResolver.TryResolveAllProjects(documentFilePath, out projects);
        });

        // Assert
        Assert.True(result);
        Assert.NotNull(projects);
        var project = Assert.Single(projects);
        AssertSnapshotsEqual(ownerProject, project);
    }

    [Fact]
    public async Task GetMiscellaneousProject_ProjectLoaded_ReturnsExistingProject()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var snapshotResolver = new SnapshotResolver(projectManager, LoggerFactory);

        // Act
        var project = await RunOnDispatcherAsync(snapshotResolver.GetMiscellaneousProject);
        var inManager = projectManager.GetLoadedProject(snapshotResolver.MiscellaneousHostProject.Key);

        // Assert
        Assert.Same(inManager, project);
    }

    [Fact]
    public async Task GetMiscellaneousProject_ProjectNotLoaded_CreatesProjectAndReturnsCreatedProject()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var snapshotResolver = new SnapshotResolver(projectManager, LoggerFactory);

        // Act
        var project = await RunOnDispatcherAsync(snapshotResolver.GetMiscellaneousProject);

        // Assert
        Assert.Single(projectManager.GetProjects());
        Assert.Equal(snapshotResolver.MiscellaneousHostProject.FilePath, project.FilePath);
    }

    private async Task<SnapshotResolver> CreateSnapshotResolverAsync(string filePath, bool addToMiscellaneous = false)
    {
        filePath = FilePathNormalizer.Normalize(filePath);

        var projectManager = CreateProjectSnapshotManager();
        var snapshotResolver = new SnapshotResolver(projectManager, LoggerFactory);

        if (addToMiscellaneous)
        {
            await RunOnDispatcherAsync(() =>
            {
                var miscProject = (ProjectSnapshot)snapshotResolver.GetMiscellaneousProject();
                projectManager.CreateAndAddDocument(miscProject, filePath);
            });
        }
        else
        {
            var projectDirectory = FilePathNormalizer.GetNormalizedDirectoryName(filePath);
            var projectSnapshot = TestProjectSnapshot.Create(Path.Combine(projectDirectory, "proj.csproj"));

            await RunOnDispatcherAsync(() =>
            {
                projectManager.ProjectAdded(projectSnapshot.HostProject);
                projectManager.CreateAndAddDocument(projectSnapshot, filePath);
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
