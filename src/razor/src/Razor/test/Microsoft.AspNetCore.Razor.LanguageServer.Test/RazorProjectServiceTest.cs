// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Moq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;
using TestProjectSnapshotManager = Microsoft.AspNetCore.Razor.Test.Common.LanguageServer.TestProjectSnapshotManager;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class RazorProjectServiceTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    [Fact]
    public async Task UpdateProject_UpdatesProjectWorkspaceState()
    {
        // Arrange
        var projectManager = TestProjectSnapshotManager.Create(ErrorReporter, Dispatcher);
        var hostProject = new HostProject("C:/path/to/project.csproj", "C:/path/to/obj", RazorConfiguration.Default, "TestRootNamespace");
        projectManager.ProjectAdded(hostProject);
        var projectService = CreateProjectService(new TestSnapshotResolver(), projectManager);
        var projectWorkspaceState = ProjectWorkspaceState.Create(LanguageVersion.LatestMajor);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            projectService.UpdateProject(
                hostProject.Key,
                hostProject.Configuration,
                hostProject.RootNamespace,
                hostProject.DisplayName,
                projectWorkspaceState,
                ImmutableArray<DocumentSnapshotHandle>.Empty));

        // Assert
        var project = projectManager.GetLoadedProject(hostProject.Key);
        Assert.Same(projectWorkspaceState, project.ProjectWorkspaceState);
    }

    [Fact]
    public async Task UpdateProject_UpdatingDocument_MapsRelativeFilePathToActualDocument()
    {
        // Arrange
        var projectManager = TestProjectSnapshotManager.Create(ErrorReporter, Dispatcher);
        var hostProject = new HostProject("C:/path/to/project.csproj", "C:/path/to/obj", RazorConfiguration.Default, "TestRootNamespace");
        projectManager.ProjectAdded(hostProject);
        var hostDocument = new HostDocument("C:/path/to/file.cshtml", "file.cshtml", FileKinds.Legacy);
        projectManager.DocumentAdded(hostProject.Key, hostDocument, Mock.Of<TextLoader>(MockBehavior.Strict));
        var projectService = CreateProjectService(new TestSnapshotResolver(), projectManager);
        var newDocument = new DocumentSnapshotHandle("file.cshtml", "file.cshtml", FileKinds.Component);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            projectService.UpdateProject(
                hostProject.Key,
                hostProject.Configuration,
                hostProject.RootNamespace,
                hostProject.DisplayName,
                ProjectWorkspaceState.Default,
                ImmutableArray.Create(newDocument)));

        // Assert
        var project = projectManager.GetLoadedProject(hostProject.Key);
        var document = project.GetDocument(hostDocument.FilePath);
        Assert.NotNull(document);
        Assert.Equal(FileKinds.Component, document.FileKind);
    }

    [Fact]
    public async Task UpdateProject_AddsNewDocuments()
    {
        // Arrange
        var projectManager = TestProjectSnapshotManager.Create(ErrorReporter, Dispatcher);
        var hostProject = new HostProject("C:/path/to/project.csproj", "C:/path/to/obj", RazorConfiguration.Default, "TestRootNamespace");
        projectManager.ProjectAdded(hostProject);
        var hostDocument = new HostDocument("C:/path/to/file.cshtml", "file.cshtml", FileKinds.Legacy);
        projectManager.DocumentAdded(hostProject.Key, hostDocument, Mock.Of<TextLoader>(MockBehavior.Strict));
        var projectService = CreateProjectService(new TestSnapshotResolver(), projectManager);
        var oldDocument = new DocumentSnapshotHandle(hostDocument.FilePath, hostDocument.TargetPath, hostDocument.FileKind);
        var newDocument = new DocumentSnapshotHandle("C:/path/to/file2.cshtml", "file2.cshtml", FileKinds.Legacy);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            projectService.UpdateProject(
                hostProject.Key,
                hostProject.Configuration,
                hostProject.RootNamespace,
                hostProject.DisplayName,
                ProjectWorkspaceState.Default,
                ImmutableArray.CreateRange([oldDocument, newDocument])));

        // Assert
        var project = projectManager.GetLoadedProject(hostProject.Key);
        var projectFilePaths = project.DocumentFilePaths.OrderBy(path => path);
        Assert.Equal(projectFilePaths, new[] { oldDocument.FilePath, newDocument.FilePath });
    }

    [Fact]
    public async Task UpdateProject_MovesDocumentsFromMisc()
    {
        // Arrange
        var projectManager = TestProjectSnapshotManager.Create(ErrorReporter, Dispatcher);
        var hostDocument = new HostDocument("C:/path/to/file.cshtml", "file.cshtml", FileKinds.Legacy);
        var miscProject = TestProjectSnapshot.Create("C:/__MISC_PROJECT__");
        projectManager.ProjectAdded(miscProject.HostProject);
        var hostProject = new HostProject("C:/path/to/project.csproj", "C:/path/to/obj", RazorConfiguration.Default, "TestRootNamespace");
        projectManager.ProjectAdded(hostProject);
        projectManager.DocumentAdded(miscProject.Key, hostDocument, Mock.Of<TextLoader>(MockBehavior.Strict));
        var project = projectManager.GetLoadedProject(hostProject.Key);
        var miscProjectSnapshot = projectManager.GetLoadedProject(miscProject.Key);
        var projectResolver = new TestSnapshotResolver(
            new Dictionary<string, IProjectSnapshot>
            {
                [hostDocument.FilePath] = project
            },
            miscProjectSnapshot);
        var projectService = CreateProjectService(projectResolver, projectManager);
        var addedDocument = new DocumentSnapshotHandle("C:/path/to/file.cshtml", "file.cshtml", FileKinds.Legacy);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            projectService.UpdateProject(
                hostProject.Key,
                hostProject.Configuration,
                hostProject.RootNamespace,
                hostProject.DisplayName,
                ProjectWorkspaceState.Default,
                ImmutableArray.Create(addedDocument)));

        // Assert
        project = projectManager.GetLoadedProject(hostProject.Key);
        var projectFilePaths = project.DocumentFilePaths.OrderBy(path => path);
        Assert.Equal(projectFilePaths, new[] { addedDocument.FilePath });
        miscProjectSnapshot = projectManager.GetLoadedProject(miscProject.Key);
        Assert.Empty(miscProjectSnapshot.DocumentFilePaths);
    }

    [Fact]
    public async Task UpdateProject_MovesExistingDocumentToMisc()
    {
        // Arrange
        var projectManager = TestProjectSnapshotManager.Create(ErrorReporter, Dispatcher);
        IProjectSnapshot miscProject = TestProjectSnapshot.Create("C:/__MISC_PROJECT__");
        var miscHostProject = new HostProject(miscProject.FilePath, miscProject.IntermediateOutputPath, RazorConfiguration.Default, "TestRootNamespace");
        projectManager.ProjectAdded(miscHostProject);
        var hostProject = new HostProject("C:/path/to/project.csproj", "C:/path/to/obj", RazorConfiguration.Default, "TestRootNamespace");
        projectManager.ProjectAdded(hostProject);
        var project = projectManager.GetLoadedProject(hostProject.Key);
        var hostDocument = new HostDocument("C:/path/to/file.cshtml", "file.cshtml", FileKinds.Legacy);
        projectManager.DocumentAdded(hostProject.Key, hostDocument, Mock.Of<TextLoader>(MockBehavior.Strict));
        var projectResolver = new TestSnapshotResolver(
            new Dictionary<string, IProjectSnapshot>
            {
                [hostDocument.FilePath] = project
            },
            miscProject);
        var projectService = CreateProjectService(projectResolver, projectManager);
        var newDocument = new DocumentSnapshotHandle("C:/path/to/file2.cshtml", "file2.cshtml", FileKinds.Legacy);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            projectService.UpdateProject(
                hostProject.Key,
                hostProject.Configuration,
                hostProject.RootNamespace,
                hostProject.DisplayName,
                ProjectWorkspaceState.Default,
                ImmutableArray.Create(newDocument)));

        // Assert
        project = projectManager.GetLoadedProject(hostProject.Key);
        Assert.Equal(project.DocumentFilePaths, new[] { newDocument.FilePath });

        miscProject = projectManager.GetLoadedProject(miscProject.Key);
        Assert.Equal(miscProject.DocumentFilePaths, new[] { hostDocument.FilePath });
    }

    [Fact]
    public async Task UpdateProject_KnownDocuments()
    {
        // Arrange
        var projectManager = TestProjectSnapshotManager.Create(ErrorReporter, Dispatcher);
        var hostProject = new HostProject("path/to/project.csproj", "path/to/obj", RazorConfiguration.Default, "TestRootNamespace");
        projectManager.ProjectAdded(hostProject);
        var document = new HostDocument("path/to/file.cshtml", "file.cshtml", FileKinds.Legacy);
        projectManager.DocumentAdded(hostProject.Key, document, Mock.Of<TextLoader>(MockBehavior.Strict));
        var projectService = CreateProjectService(new TestSnapshotResolver(), projectManager);
        var newDocument = new DocumentSnapshotHandle(document.FilePath, document.TargetPath, document.FileKind);
        projectManager.AllowNotifyListeners = true;
        projectManager.Changed += (sender, args) =>
        {
            if (args.Kind == ProjectChangeKind.DocumentRemoved ||
                args.Kind == ProjectChangeKind.DocumentChanged ||
                args.Kind == ProjectChangeKind.DocumentAdded)
            {
                throw new XunitException("Should have nooped");
            }
        };

        // Act & Assert
        await RunOnDispatcherThreadAsync(() =>
            projectService.UpdateProject(
                hostProject.Key,
                hostProject.Configuration,
                hostProject.RootNamespace,
                hostProject.DisplayName,
                ProjectWorkspaceState.Default,
                ImmutableArray.Create(newDocument)));
    }

    [Fact]
    public async Task UpdateProject_UpdatesLegacyDocumentsAsComponents()
    {
        // Arrange
        var projectManager = TestProjectSnapshotManager.Create(ErrorReporter, Dispatcher);
        var hostProject = new HostProject("C:/path/to/project.csproj", "C:/path/to/obj", RazorConfiguration.Default, "TestRootNamespace");
        projectManager.ProjectAdded(hostProject);
        var legacyDocument = new HostDocument("C:/path/to/file.cshtml", "file.cshtml", FileKinds.Legacy);
        projectManager.DocumentAdded(hostProject.Key, legacyDocument, Mock.Of<TextLoader>(MockBehavior.Strict));
        var projectService = CreateProjectService(new TestSnapshotResolver(), projectManager);
        var newDocument = new DocumentSnapshotHandle(legacyDocument.FilePath, legacyDocument.TargetPath, FileKinds.Component);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            projectService.UpdateProject(
                hostProject.Key,
                hostProject.Configuration,
                hostProject.RootNamespace,
                hostProject.DisplayName,
                ProjectWorkspaceState.Default,
                ImmutableArray.Create(newDocument)));

        // Assert
        var project = projectManager.GetLoadedProject(hostProject.Key);
        var document = project.GetDocument(newDocument.FilePath);
        Assert.NotNull(document);
        Assert.Equal(FileKinds.Component, document.FileKind);
    }

    [Fact]
    public async Task UpdateProject_SameConfigurationDifferentRootNamespace_UpdatesRootNamespace()
    {
        // Arrange
        var projectFilePath = "C:/path/to/project.csproj";
        var ownerProject = TestProjectSnapshot.Create(projectFilePath);
        var projectSnapshotManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        var expectedRootNamespace = "NewRootNamespace";
        projectSnapshotManager.Setup(manager => manager.GetLoadedProject(ownerProject.Key))
            .Returns(ownerProject);
        projectSnapshotManager.Setup(manager => manager.ProjectWorkspaceStateChanged(It.IsAny<ProjectKey>(), It.IsAny<ProjectWorkspaceState>()));
        projectSnapshotManager.Setup(manager => manager.ProjectConfigurationChanged(It.IsAny<HostProject>()))
            .Callback<HostProject>((hostProject) => Assert.Equal(expectedRootNamespace, hostProject.RootNamespace));
        var projectService = CreateProjectService(new TestSnapshotResolver(), projectSnapshotManager.Object);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            projectService.UpdateProject(
                ownerProject.Key,
                ownerProject.Configuration,
                expectedRootNamespace,
                ownerProject.DisplayName,
                ProjectWorkspaceState.Default,
                ImmutableArray<DocumentSnapshotHandle>.Empty));

        // Assert
        projectSnapshotManager.VerifyAll();
    }

    [Fact]
    public async Task UpdateProject_SameConfigurationAndRootNamespaceNoops()
    {
        // Arrange
        var projectFilePath = "C:/path/to/project.csproj";
        var ownerProject = TestProjectSnapshot.Create(projectFilePath);
        var projectSnapshotManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        projectSnapshotManager.Setup(manager => manager.GetLoadedProject(ownerProject.Key))
            .Returns(ownerProject);
        projectSnapshotManager.Setup(manager => manager.ProjectWorkspaceStateChanged(It.IsAny<ProjectKey>(), It.IsAny<ProjectWorkspaceState>()));
        projectSnapshotManager.Setup(manager => manager.ProjectConfigurationChanged(It.IsAny<HostProject>()))
            .Throws(new XunitException("Should not have been called."));
        var projectService = CreateProjectService(new TestSnapshotResolver(), projectSnapshotManager.Object);

        // Act & Assert
        await RunOnDispatcherThreadAsync(() =>
            projectService.UpdateProject(
                ownerProject.Key,
                ownerProject.Configuration,
                "TestRootNamespace",
                displayName: "",
                ProjectWorkspaceState.Default,
                ImmutableArray<DocumentSnapshotHandle>.Empty));
    }

    [Fact]
    public async Task UpdateProject_NullConfigurationUsesDefault()
    {
        // Arrange
        var projectFilePath = "C:/path/to/project.csproj";
        var ownerProject = TestProjectSnapshot.Create(projectFilePath);
        var projectSnapshotManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        projectSnapshotManager.Setup(manager => manager.GetLoadedProject(ownerProject.Key))
            .Returns(ownerProject);
        projectSnapshotManager.Setup(manager => manager.ProjectWorkspaceStateChanged(It.IsAny<ProjectKey>(), It.IsAny<ProjectWorkspaceState>()));
        projectSnapshotManager.Setup(manager => manager.ProjectConfigurationChanged(It.IsAny<HostProject>()))
            .Callback<HostProject>((hostProject) =>
            {
                Assert.Same(FallbackRazorConfiguration.Latest, hostProject.Configuration);
                Assert.Equal(projectFilePath, hostProject.FilePath);
            });
        var projectService = CreateProjectService(new TestSnapshotResolver(), projectSnapshotManager.Object);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            projectService.UpdateProject(
                ownerProject.Key,
                configuration: null,
                "TestRootNamespace",
                displayName: "",
                ProjectWorkspaceState.Default,
                ImmutableArray<DocumentSnapshotHandle>.Empty));

        // Assert
        projectSnapshotManager.VerifyAll();
    }

    [Fact]
    public async Task UpdateProject_ChangesProjectToUseProvidedConfiguration()
    {
        // Arrange
        var projectFilePath = "C:/path/to/project.csproj";
        var ownerProject = TestProjectSnapshot.Create(projectFilePath);
        var projectSnapshotManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        projectSnapshotManager.Setup(manager => manager.GetLoadedProject(ownerProject.Key))
            .Returns(ownerProject);
        projectSnapshotManager.Setup(manager => manager.ProjectWorkspaceStateChanged(It.IsAny<ProjectKey>(), It.IsAny<ProjectWorkspaceState>()));
        projectSnapshotManager.Setup(manager => manager.ProjectConfigurationChanged(It.IsAny<HostProject>()))
            .Callback<HostProject>((hostProject) =>
            {
                Assert.Same(FallbackRazorConfiguration.MVC_1_1, hostProject.Configuration);
                Assert.Equal(projectFilePath, hostProject.FilePath);
            });
        var projectService = CreateProjectService(new TestSnapshotResolver(), projectSnapshotManager.Object);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            projectService.UpdateProject(
                ownerProject.Key,
                FallbackRazorConfiguration.MVC_1_1,
                "TestRootNamespace",
                displayName: "",
                ProjectWorkspaceState.Default,
                ImmutableArray<DocumentSnapshotHandle>.Empty));

        // Assert
        projectSnapshotManager.VerifyAll();
    }

    [Fact]
    public async Task UpdateProject_UntrackedProjectNoops()
    {
        // Arrange
        var projectSnapshotManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        var projectKey = TestProjectKey.Create("C:/path/to/obj");
        projectSnapshotManager.Setup(manager => manager.GetLoadedProject(projectKey))
            .Returns<IProjectSnapshot>(null);
        projectSnapshotManager.Setup(manager => manager.ProjectConfigurationChanged(It.IsAny<HostProject>()))
            .Throws(new XunitException("Should not have been called."));
        var projectService = CreateProjectService(new TestSnapshotResolver(), projectSnapshotManager.Object);

        // Act & Assert
        await RunOnDispatcherThreadAsync(() =>
            projectService.UpdateProject(
                projectKey,
                FallbackRazorConfiguration.MVC_1_1,
                "TestRootNamespace",
                displayName: "",
                ProjectWorkspaceState.Default,
                ImmutableArray<DocumentSnapshotHandle>.Empty));
    }

    [Fact]
    public async Task CloseDocument_ClosesDocumentInOwnerProject()
    {
        // Arrange
        var expectedDocumentFilePath = "C:/path/to/document.cshtml";
        var ownerProject = TestProjectSnapshot.Create("C:/path/to/project.csproj", new[] { expectedDocumentFilePath });
        var projectResolver = new TestSnapshotResolver(
            new Dictionary<string, IProjectSnapshot>
            {
                [expectedDocumentFilePath] = ownerProject
            },
            TestProjectSnapshot.Create("C:/__MISC_PROJECT__"));
        var projectSnapshotManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        projectSnapshotManager.Setup(manager => manager.DocumentClosed(It.IsAny<ProjectKey>(), It.IsAny<string>(), It.IsAny<TextLoader>()))
            .Callback<ProjectKey, string, TextLoader>((projectKey, documentFilePath, text) =>
            {
                Assert.Equal(ownerProject.HostProject.Key, projectKey);
                Assert.Equal(expectedDocumentFilePath, documentFilePath);
                Assert.NotNull(text);
            });
        var projectService = CreateProjectService(projectResolver, projectSnapshotManager.Object);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            projectService.CloseDocument(expectedDocumentFilePath));

        // Assert
        projectSnapshotManager.VerifyAll();
    }

    [Fact]
    public async Task CloseDocument_ClosesDocumentInAllOwnerProjects()
    {
        // Arrange
        var expectedDocumentFilePath = "C:/path/to/document.cshtml";
        var project1 = TestProjectSnapshot.Create("C:/path/to/project.csproj", "C:/path.to/obj/net6", new[] { expectedDocumentFilePath }, RazorConfiguration.Default, projectWorkspaceState: null);
        var project2 = TestProjectSnapshot.Create("C:/path/to/project.csproj", "C:/path.to/obj/net7", new[] { expectedDocumentFilePath }, RazorConfiguration.Default, projectWorkspaceState: null);
        var projectResolver = new TestSnapshotResolver(
            new Dictionary<string, IProjectSnapshot[]>
            {
                [expectedDocumentFilePath] = new[] { project1, project2 }
            },
            TestProjectSnapshot.Create("C:/__MISC_PROJECT__"));
        var projectSnapshotManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        projectSnapshotManager.Setup(manager => manager.DocumentClosed(project1.Key, expectedDocumentFilePath, It.IsNotNull<TextLoader>()));
        projectSnapshotManager.Setup(manager => manager.DocumentClosed(project2.Key, expectedDocumentFilePath, It.IsNotNull<TextLoader>()));
        var projectService = CreateProjectService(projectResolver, projectSnapshotManager.Object);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            projectService.CloseDocument(expectedDocumentFilePath));

        // Assert
        projectSnapshotManager.VerifyAll();
    }

    [Fact]
    public async Task CloseDocument_ClosesDocumentInMiscellaneousProject()
    {
        // Arrange
        var expectedDocumentFilePath = "C:/path/to/document.cshtml";
        var miscellaneousProject = TestProjectSnapshot.Create("C:/__MISC_PROJECT__");
        var projectResolver = new TestSnapshotResolver(
            new Dictionary<string, IProjectSnapshot>(),
            miscellaneousProject);
        var projectSnapshotManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        projectSnapshotManager.Setup(manager => manager.DocumentClosed(It.IsAny<ProjectKey>(), It.IsAny<string>(), It.IsAny<TextLoader>()))
            .Callback<ProjectKey, string, TextLoader>((projectKey, documentFilePath, text) =>
            {
                Assert.Equal(miscellaneousProject.Key, projectKey);
                Assert.Equal(expectedDocumentFilePath, documentFilePath);
                Assert.NotNull(text);
            });
        var projectService = CreateProjectService(projectResolver, projectSnapshotManager.Object);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            projectService.CloseDocument(expectedDocumentFilePath));

        // Assert
        projectSnapshotManager.VerifyAll();
    }

    [Fact]
    public async Task OpenDocument_OpensAlreadyAddedDocumentInOwnerProject()
    {
        // Arrange
        var expectedDocumentFilePath = "C:/path/to/document.cshtml";
        var ownerProject = TestProjectSnapshot.Create("C:/path/to/project.csproj", new[] { expectedDocumentFilePath });
        var snapshotResolver = new TestSnapshotResolver(
            new Dictionary<string, IProjectSnapshot>
            {
                [expectedDocumentFilePath] = ownerProject
            },
            TestProjectSnapshot.Create("C:/__MISC_PROJECT__"));
        var projectSnapshotManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        projectSnapshotManager.Setup(manager => manager.DocumentAdded(It.IsAny<ProjectKey>(), It.IsAny<HostDocument>(), It.IsAny<TextLoader>()))
            .Throws(new InvalidOperationException("This shouldn't have been called."));
        projectSnapshotManager.Setup(manager => manager.DocumentOpened(It.IsAny<ProjectKey>(), It.IsAny<string>(), It.IsAny<SourceText>()))
            .Callback<ProjectKey, string, SourceText>((projectKey, documentFilePath, text) =>
            {
                Assert.Equal(ownerProject.HostProject.Key, projectKey);
                Assert.Equal(expectedDocumentFilePath, documentFilePath);
                Assert.NotNull(text);
            });
        var documentSnapshot = Mock.Of<IDocumentSnapshot>(s => s.GetGeneratedOutputAsync() == Task.FromResult<RazorCodeDocument>(null), MockBehavior.Strict);
        var projectService = CreateProjectService(snapshotResolver, projectSnapshotManager.Object);
        var sourceText = SourceText.From("Hello World");

        // Act
        await RunOnDispatcherThreadAsync(() =>
            projectService.OpenDocument(expectedDocumentFilePath, sourceText, 1));

        // Assert
        projectSnapshotManager.Verify(manager => manager.DocumentOpened(It.IsAny<ProjectKey>(), It.IsAny<string>(), It.IsAny<SourceText>()));
    }

    [Fact]
    public async Task OpenDocument_OpensAlreadyAddedDocumentInAllOwnerProjects()
    {
        // Arrange
        var expectedDocumentFilePath = "C:/path/to/document.cshtml";
        var project1 = TestProjectSnapshot.Create("C:/path/to/project.csproj", "C:/path.to/obj/net6", new[] { expectedDocumentFilePath }, RazorConfiguration.Default, projectWorkspaceState: null);
        var project2 = TestProjectSnapshot.Create("C:/path/to/project.csproj", "C:/path.to/obj/net7", new[] { expectedDocumentFilePath }, RazorConfiguration.Default, projectWorkspaceState: null);
        var snapshotResolver = new TestSnapshotResolver(
            new Dictionary<string, IProjectSnapshot[]>
            {
                [expectedDocumentFilePath] = new[] { project1, project2 }
            },
            TestProjectSnapshot.Create("C:/__MISC_PROJECT__"));
        var projectSnapshotManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        projectSnapshotManager.Setup(manager => manager.DocumentAdded(It.IsAny<ProjectKey>(), It.IsAny<HostDocument>(), It.IsAny<TextLoader>()))
            .Throws(new InvalidOperationException("This shouldn't have been called."));
        projectSnapshotManager.Setup(manager => manager.DocumentOpened(project1.Key, expectedDocumentFilePath, It.IsNotNull<SourceText>()));
        projectSnapshotManager.Setup(manager => manager.DocumentOpened(project2.Key, expectedDocumentFilePath, It.IsNotNull<SourceText>()));

        var documentSnapshot = Mock.Of<IDocumentSnapshot>(s => s.GetGeneratedOutputAsync() == Task.FromResult<RazorCodeDocument>(null), MockBehavior.Strict);
        var projectService = CreateProjectService(snapshotResolver, projectSnapshotManager.Object);
        var sourceText = SourceText.From("Hello World");

        // Act
        await RunOnDispatcherThreadAsync(() =>
            projectService.OpenDocument(expectedDocumentFilePath, sourceText, 1));

        // Assert
        projectSnapshotManager.Verify(manager => manager.DocumentOpened(It.IsAny<ProjectKey>(), It.IsAny<string>(), It.IsAny<SourceText>()));
    }

    [Fact]
    public async Task OpenDocument_OpensAlreadyAddedDocumentInMiscellaneousProject()
    {
        // Arrange
        var expectedDocumentFilePath = "C:/path/to/document.cshtml";
        var miscellaneousProject = TestProjectSnapshot.Create("C:/__MISC_PROJECT__", new string[] { expectedDocumentFilePath });
        var snapshotResolver = new TestSnapshotResolver(
            new Dictionary<string, IProjectSnapshot>(),
            miscellaneousProject);
        var projectSnapshotManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        projectSnapshotManager.Setup(manager => manager.DocumentAdded(It.IsAny<ProjectKey>(), It.IsAny<HostDocument>(), It.IsAny<TextLoader>()))
            .Throws(new InvalidOperationException("This shouldn't have been called."));
        projectSnapshotManager.Setup(manager => manager.DocumentOpened(It.IsAny<ProjectKey>(), It.IsAny<string>(), It.IsAny<SourceText>()))
            .Callback<ProjectKey, string, SourceText>((projectKey, documentFilePath, text) =>
            {
                Assert.Equal(miscellaneousProject.Key, projectKey);
                Assert.Equal(expectedDocumentFilePath, documentFilePath);
                Assert.NotNull(text);
            });
        var documentSnapshot = new Mock<IDocumentSnapshot>(MockBehavior.Strict).Object;
        Mock.Get(documentSnapshot)
            .Setup(s => s.GetGeneratedOutputAsync())
            .ReturnsAsync(value: null);
        var projectService = CreateProjectService(snapshotResolver, projectSnapshotManager.Object);
        var sourceText = SourceText.From("Hello World");

        // Act
        await RunOnDispatcherThreadAsync(() =>
            projectService.OpenDocument(expectedDocumentFilePath, sourceText, 1));

        // Assert
        projectSnapshotManager.Verify(manager => manager.DocumentOpened(It.IsAny<ProjectKey>(), It.IsAny<string>(), It.IsAny<SourceText>()));
    }

    [Fact]
    public async Task OpenDocument_OpensAndAddsDocumentToOwnerProject()
    {
        // Arrange
        var expectedDocumentFilePath = "C:/path/to/document.cshtml";
        var ownerProject = TestProjectSnapshot.Create("C:/path/to/project.csproj");
        var projectResolver = new TestSnapshotResolver(
            new Dictionary<string, IProjectSnapshot>
            {
                [expectedDocumentFilePath] = ownerProject
            },
            TestProjectSnapshot.Create("C:/__MISC_PROJECT__"));
        var projectSnapshotManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        projectSnapshotManager.Setup(manager => manager.IsDocumentOpen(It.IsAny<string>())).Returns(false);
        projectSnapshotManager.Setup(manager => manager.DocumentAdded(It.IsAny<ProjectKey>(), It.IsAny<HostDocument>(), It.IsAny<TextLoader>()))
            .Callback<ProjectKey, HostDocument, TextLoader>((projectKey, hostDocument, loader) =>
            {
                Assert.Equal(ownerProject.Key, projectKey);
                Assert.Equal(expectedDocumentFilePath, hostDocument.FilePath);
                Assert.NotNull(loader);

                projectResolver.UpdateProject(expectedDocumentFilePath, ownerProject.State.WithAddedHostDocument(hostDocument, DocumentState.EmptyLoader));
            });
        projectSnapshotManager.Setup(manager => manager.DocumentOpened(It.IsAny<ProjectKey>(), It.IsAny<string>(), It.IsAny<SourceText>()))
            .Callback<ProjectKey, string, SourceText>((projectKey, documentFilePath, text) =>
            {
                Assert.Equal(ownerProject.HostProject.Key, projectKey);
                Assert.Equal(expectedDocumentFilePath, documentFilePath);
                Assert.NotNull(text);
            });
        var projectService = CreateProjectService(projectResolver, projectSnapshotManager.Object);
        var sourceText = SourceText.From("Hello World");

        // Act
        await RunOnDispatcherThreadAsync(() =>
            projectService.OpenDocument(expectedDocumentFilePath, sourceText, 1));

        // Assert
        projectSnapshotManager.VerifyAll();
    }

    [Fact]
    public async Task AddDocument_NoopsIfDocumentIsAlreadyAdded()
    {
        // Arrange
        var documentFilePath = "C:/path/to/document.cshtml";
        var project = new Mock<IProjectSnapshot>(MockBehavior.Strict);
        project.Setup(p => p.Key).Returns(TestProjectKey.Create("C:/path/to/obj"));
        project.Setup(p => p.GetDocument(It.IsAny<string>())).Returns(TestDocumentSnapshot.Create(documentFilePath));
        var alreadyOpenDoc = Mock.Of<IDocumentSnapshot>(MockBehavior.Strict);
        var snapshotResolver = new Mock<ISnapshotResolver>(MockBehavior.Strict);
        snapshotResolver.Setup(resolver => resolver.FindPotentialProjects(It.IsAny<string>())).Returns(new[] { project.Object });
        var projectSnapshotManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        projectSnapshotManager.Setup(manager => manager.DocumentAdded(It.IsAny<ProjectKey>(), It.IsAny<HostDocument>(), It.IsAny<TextLoader>()))
            .Throws(new InvalidOperationException("This should not have been called."));
        var projectService = CreateProjectService(snapshotResolver.Object, projectSnapshotManager.Object);

        // Act & Assert
        await RunOnDispatcherThreadAsync(() =>
            projectService.AddDocument(documentFilePath));
    }

    [Fact]
    public async Task AddDocument_AddsDocumentToOwnerProject()
    {
        // Arrange
        var documentFilePath = "C:/path/to/document.cshtml";
        var ownerProject = TestProjectSnapshot.Create("C:/path/to/project.csproj");
        var projectResolver = new TestSnapshotResolver(
            new Dictionary<string, IProjectSnapshot>
            {
                [documentFilePath] = ownerProject
            },
            TestProjectSnapshot.Create("C:/__MISC_PROJECT__"));
        var projectSnapshotManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        projectSnapshotManager.Setup(manager => manager.IsDocumentOpen(It.IsAny<string>())).Returns(false);
        projectSnapshotManager.Setup(manager => manager.DocumentAdded(It.IsAny<ProjectKey>(), It.IsAny<HostDocument>(), It.IsAny<TextLoader>()))
            .Callback<ProjectKey, HostDocument, TextLoader>((projectKey, hostDocument, loader) =>
            {
                Assert.Equal(ownerProject.Key, projectKey);
                Assert.Equal(documentFilePath, hostDocument.FilePath);
                Assert.NotNull(loader);
            });
        var projectService = CreateProjectService(projectResolver, projectSnapshotManager.Object);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            projectService.AddDocument(documentFilePath));

        // Assert
        projectSnapshotManager.VerifyAll();
    }

    [Fact]
    public async Task AddDocument_AddsDocumentToMiscellaneousProject()
    {
        // Arrange
        var documentFilePath = "C:/path/to/document.cshtml";
        var miscellaneousProject = TestProjectSnapshot.Create("C:/__MISC_PROJECT__");
        var projectResolver = new TestSnapshotResolver(new Dictionary<string, IProjectSnapshot>(), miscellaneousProject);
        var projectSnapshotManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        projectSnapshotManager.Setup(manager => manager.IsDocumentOpen(It.IsAny<string>())).Returns(false);
        projectSnapshotManager.Setup(manager => manager.DocumentAdded(It.IsAny<ProjectKey>(), It.IsAny<HostDocument>(), It.IsAny<TextLoader>()))
            .Callback<ProjectKey, HostDocument, TextLoader>((projectKey, hostDocument, loader) =>
            {
                Assert.Equal(miscellaneousProject.HostProject.Key, projectKey);
                Assert.Equal(documentFilePath, hostDocument.FilePath);
                Assert.NotNull(loader);
            });
        var projectService = CreateProjectService(projectResolver, projectSnapshotManager.Object);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            projectService.AddDocument(documentFilePath));

        // Assert
        projectSnapshotManager.VerifyAll();
    }

    [Fact]
    public async Task RemoveDocument_RemovesDocumentFromOwnerProject()
    {
        // Arrange
        var documentFilePath = "C:/path/to/document.cshtml";
        var ownerProject = TestProjectSnapshot.Create("C:/path/to/project.csproj", new[] { documentFilePath });
        var projectResolver = new TestSnapshotResolver(
            new Dictionary<string, IProjectSnapshot>
            {
                [documentFilePath] = ownerProject
            },
            TestProjectSnapshot.Create("C:/__MISC_PROJECT__"));
        var projectSnapshotManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        projectSnapshotManager.Setup(manager => manager.DocumentRemoved(It.IsAny<ProjectKey>(), It.IsAny<HostDocument>()))
            .Callback<ProjectKey, HostDocument>((projectKey, hostDocument) =>
            {
                Assert.Equal(ownerProject.Key, projectKey);
                Assert.Equal(documentFilePath, hostDocument.FilePath);
            });
        projectSnapshotManager.Setup(manager => manager.IsDocumentOpen(It.IsAny<string>()))
            .Returns<string>((filePath) =>
            {
                Assert.Equal(filePath, documentFilePath);
                return false;
            });

        var projectService = CreateProjectService(projectResolver, projectSnapshotManager.Object);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            projectService.RemoveDocument(documentFilePath));

        // Assert
        projectSnapshotManager.VerifyAll();
    }

    [Fact]
    public async Task RemoveDocument_RemovesDocumentFromAllOwnerProjects()
    {
        // Arrange
        var documentFilePath = "C:/path/to/document.cshtml";

        var project1 = TestProjectSnapshot.Create("C:/path/to/project.csproj", "C:/path.to/obj/net6", new[] { documentFilePath }, RazorConfiguration.Default, projectWorkspaceState: null);
        var project2 = TestProjectSnapshot.Create("C:/path/to/project.csproj", "C:/path.to/obj/net7", new[] { documentFilePath }, RazorConfiguration.Default, projectWorkspaceState: null);
        var projectResolver = new TestSnapshotResolver(
            new Dictionary<string, IProjectSnapshot[]>
            {
                [documentFilePath] = new[] { project1, project2 }
            },
            TestProjectSnapshot.Create("C:/__MISC_PROJECT__"));

        var projectSnapshotManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        projectSnapshotManager.Setup(manager => manager.DocumentRemoved(project1.Key, It.IsAny<HostDocument>()))
            .Callback<ProjectKey, HostDocument>((projectKey, hostDocument) =>
            {
                Assert.Equal(project1.Key, projectKey);
                Assert.Equal(documentFilePath, hostDocument.FilePath);
            });
        projectSnapshotManager.Setup(manager => manager.DocumentRemoved(project2.Key, It.IsAny<HostDocument>()))
            .Callback<ProjectKey, HostDocument>((projectKey, hostDocument) =>
            {
                Assert.Equal(project2.Key, projectKey);
                Assert.Equal(documentFilePath, hostDocument.FilePath);
            });
        projectSnapshotManager.Setup(manager => manager.IsDocumentOpen(It.IsAny<string>()))
            .Returns<string>((filePath) =>
            {
                Assert.Equal(filePath, documentFilePath);
                return false;
            });

        var projectService = CreateProjectService(projectResolver, projectSnapshotManager.Object);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            projectService.RemoveDocument(documentFilePath));

        // Assert
        projectSnapshotManager.VerifyAll();
    }

    [Fact]
    public async Task RemoveOpenDocument_RemovesDocumentFromOwnerProject_MovesToMiscellaneousProject()
    {
        // Arrange
        var documentFilePath = "C:/path/to/document.cshtml";
        var miscellaneousProject = TestProjectSnapshot.Create("C:/__MISC_PROJECT__");
        var ownerProject = TestProjectSnapshot.Create("C:/path/to/project.csproj", new[] { documentFilePath });
        var projectResolver = new TestSnapshotResolver(
            new Dictionary<string, IProjectSnapshot>
            {
                [documentFilePath] = ownerProject
            },
            TestProjectSnapshot.Create("C:/__MISC_PROJECT__"));
        var projectSnapshotManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        projectSnapshotManager.Setup(manager => manager.DocumentRemoved(It.IsAny<ProjectKey>(), It.IsAny<HostDocument>()))
            .Callback<ProjectKey, HostDocument>((projectKey, hostDocument) =>
            {
                Assert.Equal(ownerProject.Key, projectKey);
                Assert.Equal(documentFilePath, hostDocument.FilePath);
            });
        projectSnapshotManager.Setup(manager => manager.DocumentAdded(It.IsAny<ProjectKey>(), It.IsAny<HostDocument>(), It.IsAny<TextLoader>()))
            .Callback<ProjectKey, HostDocument, TextLoader>((projectKey, hostDocument, loader) =>
            {
                Assert.Equal(miscellaneousProject.Key, projectKey);
                Assert.Equal(documentFilePath, hostDocument.FilePath);
                Assert.NotNull(loader);
            });
        projectSnapshotManager.Setup(manager => manager.IsDocumentOpen(It.IsAny<string>()))
            .Returns<string>((filePath) =>
            {
                Assert.Equal(filePath, documentFilePath);
                return true;
            });

        var projectService = CreateProjectService(projectResolver, projectSnapshotManager.Object);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            projectService.RemoveDocument(documentFilePath));

        // Assert
        projectSnapshotManager.VerifyAll();
    }

    [Fact]
    public async Task RemoveDocument_RemovesDocumentFromMiscellaneousProject()
    {
        // Arrange
        var documentFilePath = "C:/path/to/document.cshtml";
        var miscellaneousProject = TestProjectSnapshot.Create("C:/__MISC_PROJECT__", new[] { documentFilePath });
        var projectResolver = new TestSnapshotResolver(
            new Dictionary<string, IProjectSnapshot>(),
            miscellaneousProject);
        var projectSnapshotManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        projectSnapshotManager.Setup(manager => manager.DocumentRemoved(It.IsAny<ProjectKey>(), It.IsAny<HostDocument>()))
            .Callback<ProjectKey, HostDocument>((projectKey, hostDocument) =>
            {
                Assert.Equal(miscellaneousProject.Key, projectKey);
                Assert.Equal(documentFilePath, hostDocument.FilePath);
            });
        projectSnapshotManager.Setup(manager => manager.IsDocumentOpen(It.IsAny<string>()))
            .Returns<string>((filePath) =>
            {
                Assert.Equal(filePath, documentFilePath);
                return false;
            });
        var projectService = CreateProjectService(projectResolver, projectSnapshotManager.Object);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            projectService.RemoveDocument(documentFilePath));

        // Assert
        projectSnapshotManager.VerifyAll();
    }

    [Fact]
    public async Task RemoveDocument_NoopsIfOwnerProjectDoesNotContainDocument()
    {
        // Arrange
        var documentFilePath = "C:/path/to/document.cshtml";
        var ownerProject = TestProjectSnapshot.Create("C:/path/to/project.csproj", Array.Empty<string>());
        var projectResolver = new TestSnapshotResolver(
            new Dictionary<string, IProjectSnapshot>
            {
                [documentFilePath] = ownerProject
            },
            TestProjectSnapshot.Create("C:/__MISC_PROJECT__"));
        var projectSnapshotManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        projectSnapshotManager.Setup(manager => manager.DocumentRemoved(It.IsAny<ProjectKey>(), It.IsAny<HostDocument>()))
            .Throws(new InvalidOperationException("Should not have been called."));
        var projectService = CreateProjectService(projectResolver, projectSnapshotManager.Object);

        // Act & Assert
        await RunOnDispatcherThreadAsync(() =>
            projectService.RemoveDocument(documentFilePath));
    }

    [Fact]
    public async Task RemoveDocument_NoopsIfMiscellaneousProjectDoesNotContainDocument()
    {
        // Arrange
        var documentFilePath = "C:/path/to/document.cshtml";
        var miscellaneousProject = TestProjectSnapshot.Create("C:/__MISC_PROJECT__", Array.Empty<string>());
        var projectResolver = new TestSnapshotResolver(
            new Dictionary<string, IProjectSnapshot>(),
            miscellaneousProject);
        var projectSnapshotManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        projectSnapshotManager.Setup(manager => manager.DocumentRemoved(It.IsAny<ProjectKey>(), It.IsAny<HostDocument>()))
            .Throws(new InvalidOperationException("Should not have been called."));
        var projectService = CreateProjectService(projectResolver, projectSnapshotManager.Object);

        // Act & Assert
        await RunOnDispatcherThreadAsync(() =>
            projectService.RemoveDocument(documentFilePath));
    }

    [Fact]
    public async Task UpdateDocument_ChangesDocumentInOwnerProject()
    {
        // Arrange
        var documentFilePath = "C:/path/to/document.cshtml";
        var ownerProject = TestProjectSnapshot.Create("C:/path/to/project.csproj", new[] { documentFilePath });
        var projectResolver = new TestSnapshotResolver(
            new Dictionary<string, IProjectSnapshot>
            {
                [documentFilePath] = ownerProject
            },
            TestProjectSnapshot.Create("C:/__MISC_PROJECT__"));
        var newText = SourceText.From("Something New");
        var projectSnapshotManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        projectSnapshotManager.Setup(manager => manager.DocumentChanged(ownerProject.Key, documentFilePath, newText))
            .Callback<ProjectKey, string, SourceText>((projectKey, documentPath, sourceText) =>
            {
                Assert.Equal(ownerProject.Key, projectKey);
                Assert.Equal(documentFilePath, documentPath);
                Assert.Same(newText, sourceText);
            });
        var projectService = CreateProjectService(projectResolver, projectSnapshotManager.Object);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            projectService.UpdateDocument(documentFilePath, newText, 1337));

        // Assert
        projectSnapshotManager.VerifyAll();
    }

    [Fact]
    public async Task UpdateDocument_ChangesDocumentInAllOwnerProjects()
    {
        // Arrange
        var documentFilePath = "C:/path/to/document.cshtml";

        var project1 = TestProjectSnapshot.Create("C:/path/to/project.csproj", "C:/path.to/obj/net6", new[] { documentFilePath }, RazorConfiguration.Default, projectWorkspaceState: null);
        var project2 = TestProjectSnapshot.Create("C:/path/to/project.csproj", "C:/path.to/obj/net7", new[] { documentFilePath }, RazorConfiguration.Default, projectWorkspaceState: null);
        var projectResolver = new TestSnapshotResolver(
            new Dictionary<string, IProjectSnapshot[]>
            {
                [documentFilePath] = new[] { project1, project2 }
            },
            TestProjectSnapshot.Create("C:/__MISC_PROJECT__"));

        var newText = SourceText.From("Something New");
        var projectSnapshotManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        projectSnapshotManager.Setup(manager => manager.DocumentChanged(project1.Key, documentFilePath, newText));
        projectSnapshotManager.Setup(manager => manager.DocumentChanged(project2.Key, documentFilePath, newText));
        var projectService = CreateProjectService(projectResolver, projectSnapshotManager.Object);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            projectService.UpdateDocument(documentFilePath, newText, 1337));

        // Assert
        projectSnapshotManager.VerifyAll();
    }

    [Fact]
    public async Task UpdateDocument_ChangesDocumentInMiscProject()
    {
        // Arrange
        var documentFilePath = "C:/path/to/document.cshtml";
        var miscellaneousProject = TestProjectSnapshot.Create("C:/__MISC_PROJECT__", new[] { documentFilePath });
        var projectResolver = new TestSnapshotResolver(
            new Dictionary<string, IProjectSnapshot>(),
            TestProjectSnapshot.Create("C:/__MISC_PROJECT__"));
        var newText = SourceText.From("Something New");
        var projectSnapshotManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        projectSnapshotManager.Setup(manager => manager.DocumentChanged(miscellaneousProject.Key, documentFilePath, newText))
            .Callback<ProjectKey, string, SourceText>((projectKey, documentPath, sourceText) =>
            {
                Assert.Equal(miscellaneousProject.Key, projectKey);
                Assert.Equal(documentFilePath, documentPath);
                Assert.Same(newText, sourceText);
            });
        var projectService = CreateProjectService(projectResolver, projectSnapshotManager.Object);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            projectService.UpdateDocument(documentFilePath, newText, 1337));

        // Assert
        projectSnapshotManager.VerifyAll();
    }

    [Fact]
    public async Task UpdateDocument_TracksKnownDocumentVersion()
    {
        // Arrange
        var documentFilePath = "C:/path/to/document.cshtml";
        var ownerProject = TestProjectSnapshot.Create("C:/path/to/project.csproj", new[] { documentFilePath });
        var snapshotResolver = new TestSnapshotResolver(
            new Dictionary<string, IProjectSnapshot>
            {
                [documentFilePath] = ownerProject
            },
            TestProjectSnapshot.Create("C:/__MISC_PROJECT__"));

        var newText = SourceText.From("Something New");
        var documentVersionCache = new Mock<IDocumentVersionCache>(MockBehavior.Strict);
        documentVersionCache.Setup(cache => cache.TrackDocumentVersion(It.IsAny<IDocumentSnapshot>(), It.IsAny<int>()))
            .Callback<IDocumentSnapshot, int>((snapshot, version) =>
            {
                // We updated the project in the DocumentChanged callback, so we expect to get a new snapshot
                Assert.NotSame(ownerProject.GetDocument(documentFilePath), snapshot);
                Assert.Equal(documentFilePath, snapshot.FilePath);
#pragma warning disable xUnit1031 // Do not use blocking task operations in test method
                Assert.Equal(newText, snapshot.GetTextAsync().Result);
#pragma warning restore xUnit1031
                Assert.Equal(1337, version);
            });
        var projectSnapshotManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        projectSnapshotManager.Setup(m => m.DocumentChanged(It.IsAny<ProjectKey>(), It.IsAny<string>(), It.IsAny<SourceText>()))
            .Callback<ProjectKey, string, SourceText>((projectKey, documentFilePath, sourceText) =>
            {
                Assert.Equal(ownerProject.Key, projectKey);
                var hostDocument = new HostDocument(documentFilePath, documentFilePath);
                var newState = ownerProject.State.WithChangedHostDocument(hostDocument, sourceText, VersionStamp.Create());
                snapshotResolver.UpdateProject(documentFilePath, newState);
            }).Verifiable();
        var projectService = CreateProjectService(
            snapshotResolver,
            projectSnapshotManager.Object,
            documentVersionCache.Object);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            projectService.UpdateDocument(documentFilePath, newText, 1337));

        // Assert
        documentVersionCache.VerifyAll();
    }

    [Fact]
    public async Task UpdateDocument_IgnoresUnknownDocumentVersions()
    {
        // Arrange
        var documentFilePath = "C:/path/to/document.cshtml";
        var ownerProject = TestProjectSnapshot.Create("C:/path/to/project.csproj");
        var projectResolver = new TestSnapshotResolver(
            new Dictionary<string, IProjectSnapshot>
            {
                [documentFilePath] = ownerProject
            },
            TestProjectSnapshot.Create("C:/__MISC_PROJECT__"));
        var documentVersionCache = new Mock<IDocumentVersionCache>(MockBehavior.Strict);
        documentVersionCache.Setup(cache => cache.TrackDocumentVersion(It.IsAny<IDocumentSnapshot>(), It.IsAny<int>()))
            .Throws<Exception>();
        var newText = SourceText.From("Something New");
        var projectSnapshotManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        projectSnapshotManager.Setup(m => m.DocumentChanged(It.IsAny<ProjectKey>(), It.IsAny<string>(), It.IsAny<SourceText>())).Verifiable();
        var projectService = CreateProjectService(
            projectResolver,
            projectSnapshotManager.Object,
            documentVersionCache: documentVersionCache.Object);

        // Act & Assert
        await RunOnDispatcherThreadAsync(() =>
            projectService.UpdateDocument(documentFilePath, newText, 1337));
    }

    [Fact]
    public async Task AddProject_AddsProjectWithDefaultConfiguration()
    {
        // Arrange
        var projectFilePath = "C:/path/to/project.csproj";
        var miscellaneousProject = TestProjectSnapshot.Create("/./__MISC_PROJECT__");
        var projectResolver = new TestSnapshotResolver(new Dictionary<string, IProjectSnapshot>(), miscellaneousProject);
        var projectSnapshotManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        projectSnapshotManager.Setup(manager => manager.ProjectAdded(It.IsAny<HostProject>()))
            .Callback<HostProject>((hostProject) =>
            {
                Assert.Equal(projectFilePath, hostProject.FilePath);
                Assert.Same(FallbackRazorConfiguration.Latest, hostProject.Configuration);
                Assert.Null(hostProject.RootNamespace);
            });
        var projectService = CreateProjectService(projectResolver, projectSnapshotManager.Object);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            projectService.AddProject(projectFilePath, "C:/path/to/obj", configuration: null, rootNamespace: null, displayName: ""));

        // Assert
        projectSnapshotManager.VerifyAll();
    }

    [Fact]
    public async Task AddProject_AddsProjectWithSpecifiedConfiguration()
    {
        // Arrange
        var projectFilePath = "C:/path/to/project.csproj";
        var miscellaneousProject = TestProjectSnapshot.Create("/./__MISC_PROJECT__");
        var projectResolver = new TestSnapshotResolver(new Dictionary<string, IProjectSnapshot>(), miscellaneousProject);

        var configuration = RazorConfiguration.Create(RazorLanguageVersion.Version_1_0, "TestName", Array.Empty<RazorExtension>());

        var projectSnapshotManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        projectSnapshotManager.Setup(manager => manager.ProjectAdded(It.IsAny<HostProject>()))
            .Callback<HostProject>((hostProject) =>
            {
                Assert.Equal(projectFilePath, hostProject.FilePath);
                Assert.Same(configuration, hostProject.Configuration);
                Assert.Equal("My.Root.Namespace", hostProject.RootNamespace);
            });
        var projectService = CreateProjectService(projectResolver, projectSnapshotManager.Object);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            projectService.AddProject(projectFilePath, "C:/path/to/obj", configuration, "My.Root.Namespace", displayName: ""));

        // Assert
        projectSnapshotManager.VerifyAll();
    }

    [Fact]
    public async Task TryMigrateDocumentsFromRemovedProject_MigratesDocumentsToNonMiscProject()
    {
        // Arrange
        var documentFilePath1 = "C:/path/to/some/document1.cshtml";
        var documentFilePath2 = "C:/path/to/some/document2.cshtml";
        var miscellaneousProject = TestProjectSnapshot.Create("C:/__MISC_PROJECT__");
        var removedProject = TestProjectSnapshot.Create("C:/path/to/some/project.csproj", new[] { documentFilePath1, documentFilePath2 });
        var projectToBeMigratedTo = TestProjectSnapshot.Create("C:/path/to/project.csproj");
        var projectResolver = new TestSnapshotResolver(
            new Dictionary<string, IProjectSnapshot>
            {
                [documentFilePath1] = projectToBeMigratedTo,
                [documentFilePath2] = projectToBeMigratedTo,
            },
            miscellaneousProject);
        var migratedDocuments = new List<HostDocument>();
        var projectSnapshotManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        projectSnapshotManager.Setup(manager => manager.DocumentAdded(It.IsAny<ProjectKey>(), It.IsAny<HostDocument>(), It.IsAny<TextLoader>()))
            .Callback<ProjectKey, HostDocument, TextLoader>((projectKey, hostDocument, textLoader) =>
            {
                Assert.Equal(projectToBeMigratedTo.Key, projectKey);
                Assert.NotNull(textLoader);

                migratedDocuments.Add(hostDocument);
            });
        var projectService = CreateProjectService(projectResolver, projectSnapshotManager.Object);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            projectService.TryMigrateDocumentsFromRemovedProject(removedProject));

        // Assert
        Assert.Collection(migratedDocuments.OrderBy(doc => doc.FilePath),
            document => Assert.Equal(documentFilePath1, document.FilePath),
            document => Assert.Equal(documentFilePath2, document.FilePath));
    }

    [Fact]
    public async Task TryMigrateDocumentsFromRemovedProject_MigratesDocumentsToMiscProject()
    {
        // Arrange
        var documentFilePath1 = "C:/path/to/some/document1.cshtml";
        var documentFilePath2 = "C:/path/to/some/document2.cshtml";
        var miscellaneousProject = TestProjectSnapshot.Create("C:/__MISC_PROJECT__");
        var removedProject = TestProjectSnapshot.Create("C:/path/to/some/project.csproj", new[] { documentFilePath1, documentFilePath2 });
        var projectResolver = new TestSnapshotResolver(new Dictionary<string, IProjectSnapshot>(), miscellaneousProject);
        var migratedDocuments = new List<HostDocument>();
        var projectSnapshotManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        projectSnapshotManager.Setup(manager => manager.DocumentAdded(It.IsAny<ProjectKey>(), It.IsAny<HostDocument>(), It.IsAny<TextLoader>()))
            .Callback<ProjectKey, HostDocument, TextLoader>((projectKey, hostDocument, textLoader) =>
            {
                Assert.Equal(miscellaneousProject.Key, projectKey);
                Assert.NotNull(textLoader);

                migratedDocuments.Add(hostDocument);
            });
        var projectService = CreateProjectService(projectResolver, projectSnapshotManager.Object);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            projectService.TryMigrateDocumentsFromRemovedProject(removedProject));

        // Assert
        Assert.Collection(migratedDocuments.OrderBy(doc => doc.FilePath),
            document => Assert.Equal(documentFilePath1, document.FilePath),
            document => Assert.Equal(documentFilePath2, document.FilePath));
    }

    [Fact]
    public async Task TryMigrateMiscellaneousDocumentsToProject_DoesNotMigrateDocumentsIfNoOwnerProject()
    {
        // Arrange
        var documentFilePath1 = "C:/path/to/document1.cshtml";
        var documentFilePath2 = "C:/path/to/document2.cshtml";
        var miscellaneousProject = TestProjectSnapshot.Create("C:/__MISC_PROJECT__", new[] { documentFilePath1, documentFilePath2 });
        var projectResolver = new TestSnapshotResolver(new Dictionary<string, IProjectSnapshot>(), miscellaneousProject);
        var projectSnapshotManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        projectSnapshotManager.Setup(manager => manager.DocumentAdded(It.IsAny<ProjectKey>(), It.IsAny<HostDocument>(), It.IsAny<TextLoader>()))
            .Throws(new InvalidOperationException("Should not have been called."));
        var projectService = CreateProjectService(projectResolver, projectSnapshotManager.Object);

        // Act & Assert
        await RunOnDispatcherThreadAsync(() =>
            projectService.TryMigrateMiscellaneousDocumentsToProject());
    }

    [Fact]
    public async Task TryMigrateMiscellaneousDocumentsToProject_MigratesDocumentsToNewOwnerProject()
    {
        // Arrange
        var documentFilePath1 = "C:/path/to/document1.cshtml";
        var documentFilePath2 = "C:/path/to/document2.cshtml";
        var miscellaneousProject = TestProjectSnapshot.Create("C:/__MISC_PROJECT__", new[] { documentFilePath1, documentFilePath2 });
        var projectToBeMigratedTo = TestProjectSnapshot.Create("C:/path/to/project.csproj");
        var projectResolver = new TestSnapshotResolver(
            new Dictionary<string, IProjectSnapshot>
            {
                [documentFilePath1] = projectToBeMigratedTo,
                [documentFilePath2] = projectToBeMigratedTo,
            },
            miscellaneousProject);
        var migratedDocuments = new List<HostDocument>();
        var projectSnapshotManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        projectSnapshotManager.Setup(manager => manager.DocumentAdded(It.IsAny<ProjectKey>(), It.IsAny<HostDocument>(), It.IsAny<TextLoader>()))
            .Callback<ProjectKey, HostDocument, TextLoader>((projectKey, hostDocument, textLoader) =>
            {
                Assert.Equal(projectToBeMigratedTo.Key, projectKey);
                Assert.NotNull(textLoader);

                migratedDocuments.Add(hostDocument);
            });
        projectSnapshotManager.Setup(manager => manager.DocumentRemoved(It.IsAny<ProjectKey>(), It.IsAny<HostDocument>()))
            .Callback<ProjectKey, HostDocument>((projectKey, hostDocument) =>
            {
                Assert.Equal(miscellaneousProject.Key, projectKey);

                Assert.DoesNotContain(hostDocument, migratedDocuments);
            });
        var projectService = CreateProjectService(projectResolver, projectSnapshotManager.Object);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            projectService.TryMigrateMiscellaneousDocumentsToProject());

        // Assert
        Assert.Collection(migratedDocuments.OrderBy(doc => doc.FilePath),
            document => Assert.Equal(documentFilePath1, document.FilePath),
            document => Assert.Equal(documentFilePath2, document.FilePath));
    }

    private RazorProjectService CreateProjectService(
        ISnapshotResolver snapshotResolver,
        ProjectSnapshotManagerBase projectSnapshotManager,
        IDocumentVersionCache documentVersionCache = null)
    {
        if (documentVersionCache is null)
        {
            documentVersionCache = new Mock<IDocumentVersionCache>(MockBehavior.Strict).Object;
            Mock.Get(documentVersionCache).Setup(c => c.TrackDocumentVersion(It.IsAny<IDocumentSnapshot>(), It.IsAny<int>())).Verifiable();
        }

        var accessor = Mock.Of<IProjectSnapshotManagerAccessor>(a => a.Instance == projectSnapshotManager, MockBehavior.Strict);
        if (snapshotResolver is null)
        {
            snapshotResolver = new Mock<ISnapshotResolver>(MockBehavior.Strict).Object;
            Mock.Get(snapshotResolver).Setup(r => r.TryResolveDocumentInAnyProject(It.IsAny<string>(), out It.Ref<IDocumentSnapshot>.IsAny)).Returns(false);
        }

        var remoteTextLoaderFactory = Mock.Of<RemoteTextLoaderFactory>(factory => factory.Create(It.IsAny<string>()) == Mock.Of<TextLoader>(MockBehavior.Strict), MockBehavior.Strict);
        var projectService = new RazorProjectService(
            Dispatcher,
            remoteTextLoaderFactory,
            snapshotResolver,
            documentVersionCache,
            accessor,
            LoggerFactory);

        return projectService;
    }

    private class TestSnapshotResolver : ISnapshotResolver
    {
        private readonly Dictionary<string, IProjectSnapshot[]> _projectMappings;
        private readonly IProjectSnapshot _miscellaneousProject;

        public TestSnapshotResolver()
            : this(new Dictionary<string, IProjectSnapshot>(), TestProjectSnapshot.Create("C:/__MISC_PROJECT__"))
        {
        }

        public TestSnapshotResolver(IReadOnlyDictionary<string, IProjectSnapshot> projectMappings, IProjectSnapshot miscellaneousProject)
        {
            _projectMappings = projectMappings.ToDictionary(kvp => kvp.Key, kvp => new[] { kvp.Value });
            _miscellaneousProject = miscellaneousProject;
        }

        public TestSnapshotResolver(IReadOnlyDictionary<string, IProjectSnapshot[]> projectMappings, IProjectSnapshot miscellaneousProject)
        {
            _projectMappings = projectMappings.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            _miscellaneousProject = miscellaneousProject;
        }

        public IEnumerable<IProjectSnapshot> FindPotentialProjects(string documentFilePath)
        {
            var currentProjects = _projectMappings.Values.ToArray();

            foreach (var projects in currentProjects)
            {
                foreach (var project in projects)
                {
                    yield return project;
                }
            }
        }

        public IProjectSnapshot GetMiscellaneousProject() => _miscellaneousProject;

        public bool TryResolveDocumentInAnyProject(string documentFilePath, out IDocumentSnapshot documentSnapshot)
        {
            if (_projectMappings.TryGetValue(documentFilePath, out var projects))
            {
                var projectSnapshot = projects.First();
                documentSnapshot = projectSnapshot.GetDocument(documentFilePath);
                return documentSnapshot is not null;
            }

            documentSnapshot = _miscellaneousProject.GetDocument(documentFilePath);
            return documentSnapshot is not null;
        }

        internal void UpdateProject(string expectedDocumentFilePath, ProjectState projectState)
        {
            _projectMappings[expectedDocumentFilePath] = new[] { new ProjectSnapshot(projectState) };
        }
    }
}
