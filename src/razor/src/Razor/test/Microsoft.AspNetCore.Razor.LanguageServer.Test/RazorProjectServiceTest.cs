// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Moq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class RazorProjectServiceTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    private static readonly SourceText s_emptyText = SourceText.From("");

    // Each of these is initialized by InitializeAsync() below.
#nullable disable
    private TestProjectSnapshotManager _projectManager;
    private TestRazorProjectService _projectService;
    private IRazorProjectInfoListener _projectInfoListener;
#nullable enable

    protected override Task InitializeAsync()
    {
        var optionsMonitor = TestRazorLSPOptionsMonitor.Create();
        var projectEngineFactoryProvider = new LspProjectEngineFactoryProvider(optionsMonitor);
        _projectManager = CreateProjectSnapshotManager(projectEngineFactoryProvider);

        var remoteTextLoaderFactoryMock = new StrictMock<RemoteTextLoaderFactory>();
        remoteTextLoaderFactoryMock
            .Setup(x => x.Create(It.IsAny<string>()))
            .Returns(CreateEmptyTextLoader());

        _projectService = new TestRazorProjectService(
            remoteTextLoaderFactoryMock.Object,
            _projectManager,
            LoggerFactory);

        _projectInfoListener = _projectService;

        return Task.CompletedTask;
    }

    [Fact]
    public async Task IProjectInfoListener_UpdatedAsync_UpdatesProjectWorkspaceState()
    {
        // Arrange
        var hostProject = new HostProject("C:/path/to/project.csproj", "C:/path/to/obj", RazorConfiguration.Default, "TestRootNamespace");

        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(hostProject);
        });

        var projectWorkspaceState = ProjectWorkspaceState.Default;

        // Act
        await _projectInfoListener.UpdatedAsync(new RazorProjectInfo(
            hostProject.Key,
            hostProject.FilePath,
            hostProject.Configuration,
            hostProject.RootNamespace,
            hostProject.DisplayName,
            projectWorkspaceState,
            documents: []),
            DisposalToken);

        // Assert
        var project = _projectManager.GetRequiredProject(hostProject.Key);
        Assert.Same(projectWorkspaceState, project.ProjectWorkspaceState);
    }

    [Fact]
    public async Task IProjectInfoListener_UpdatedAsync_UpdatingDocument_MapsRelativeFilePathToActualDocument()
    {
        // Arrange
        var hostProject = new HostProject("C:/path/to/project.csproj", "C:/path/to/obj", RazorConfiguration.Default, "TestRootNamespace");
        var hostDocument = new HostDocument("C:/path/to/file.cshtml", "file.cshtml", FileKinds.Legacy);

        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(hostProject);
            updater.AddDocument(hostProject.Key, hostDocument, StrictMock.Of<TextLoader>());
        });

        var newDocument = new DocumentSnapshotHandle("file.cshtml", "file.cshtml", FileKinds.Component);

        // Act
        await _projectInfoListener.UpdatedAsync(new RazorProjectInfo(
            hostProject.Key,
            hostProject.FilePath,
            hostProject.Configuration,
            hostProject.RootNamespace,
            hostProject.DisplayName,
            ProjectWorkspaceState.Default,
            [newDocument]),
            DisposalToken);

        // Assert
        var document = _projectManager.GetRequiredDocument(hostProject.Key, hostDocument.FilePath);

        Assert.Equal(FileKinds.Component, document.FileKind);
    }

    [Fact]
    public async Task IProjectInfoListener_UpdatedAsync_AddsNewDocuments()
    {
        // Arrange
        var hostProject = new HostProject("C:/path/to/project.csproj", "C:/path/to/obj", RazorConfiguration.Default, "TestRootNamespace");
        var hostDocument = new HostDocument("C:/path/to/file.cshtml", "file.cshtml", FileKinds.Legacy);

        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(hostProject);
            updater.AddDocument(hostProject.Key, hostDocument, StrictMock.Of<TextLoader>());
        });

        var oldDocument = new DocumentSnapshotHandle(hostDocument.FilePath, hostDocument.TargetPath, hostDocument.FileKind);
        var newDocument = new DocumentSnapshotHandle("C:/path/to/file2.cshtml", "file2.cshtml", FileKinds.Legacy);

        // Act
        await _projectInfoListener.UpdatedAsync(new RazorProjectInfo(
            hostProject.Key,
            hostProject.FilePath,
            hostProject.Configuration,
            hostProject.RootNamespace,
            hostProject.DisplayName,
            ProjectWorkspaceState.Default,
            [oldDocument, newDocument]),
            DisposalToken);

        // Assert
        var project = _projectManager.GetRequiredProject(hostProject.Key);
        var projectFilePaths = project.DocumentFilePaths.OrderBy(path => path);
        Assert.Equal(projectFilePaths, [oldDocument.FilePath, newDocument.FilePath]);
    }

    [Fact]
    public async Task IProjectInfoListener_UpdatedAsync_MovesDocumentsFromMisc()
    {
        // Arrange
        var hostProject = new HostProject("C:/path/to/project.csproj", "C:/path/to/obj", RazorConfiguration.Default, "TestRootNamespace");
        var hostDocument = new HostDocument("C:/path/to/file.cshtml", "file.cshtml", FileKinds.Legacy);

        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(hostProject);
            updater.AddDocument(MiscFilesProject.Key, hostDocument, StrictMock.Of<TextLoader>());
        });

        var addedDocument = new DocumentSnapshotHandle(hostDocument.FilePath, hostDocument.TargetPath, hostDocument.FileKind);

        // Act
        await _projectInfoListener.UpdatedAsync(new RazorProjectInfo(
            hostProject.Key,
            hostProject.FilePath,
            hostProject.Configuration,
            hostProject.RootNamespace,
            hostProject.DisplayName,
            ProjectWorkspaceState.Default,
            [addedDocument]),
            DisposalToken);

        // Assert
        var project = _projectManager.GetRequiredProject(hostProject.Key);
        var projectFilePaths = project.DocumentFilePaths.OrderBy(path => path);
        Assert.Equal(projectFilePaths, [addedDocument.FilePath]);

        var miscProject = _projectManager.GetMiscellaneousProject();
        Assert.Empty(miscProject.DocumentFilePaths);
    }

    [Fact]
    public async Task IProjectInfoListener_UpdatedAsync_MovesDocumentsFromMisc_ViaService()
    {
        // Arrange
        const string DocumentFilePath = "C:/path/to/file.cshtml";
        const string ProjectFilePath = "C:/path/to/project.csproj";
        const string IntermediateOutputPath = "C:/path/to/obj";
        const string RootNamespace = "TestRootNamespace";

        var ownerProjectKey = await _projectService.GetTestAccessor().AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);

        await _projectService.AddDocumentToMiscProjectAsync(DocumentFilePath, DisposalToken);

        var project = _projectManager.GetRequiredProject(ownerProjectKey);

        var addedDocument = new DocumentSnapshotHandle(DocumentFilePath, DocumentFilePath, FileKinds.Legacy);

        // Act
        await _projectInfoListener.UpdatedAsync(new RazorProjectInfo(
            project.Key,
            project.FilePath,
            project.Configuration,
            project.RootNamespace,
            project.DisplayName,
            ProjectWorkspaceState.Default,
            [addedDocument]),
            DisposalToken);

        // Assert
        project = _projectManager.GetRequiredProject(ownerProjectKey);
        var projectFilePaths = project.DocumentFilePaths.OrderBy(path => path);
        Assert.Equal(projectFilePaths, [addedDocument.FilePath]);

        var miscProject = _projectManager.GetMiscellaneousProject();
        Assert.Empty(miscProject.DocumentFilePaths);
    }

    [Fact]
    public async Task IProjectInfoListener_UpdatedAsync_MovesExistingDocumentToMisc()
    {
        // Arrange
        var hostProject = new HostProject("C:/path/to/project.csproj", "C:/path/to/obj", RazorConfiguration.Default, "TestRootNamespace");
        var hostDocument = new HostDocument("C:/path/to/file.cshtml", "file.cshtml", FileKinds.Legacy);

        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(hostProject);
            updater.AddDocument(MiscFilesProject.Key, hostDocument, StrictMock.Of<TextLoader>());
        });

        var newDocument = new DocumentSnapshotHandle("C:/path/to/file2.cshtml", "file2.cshtml", FileKinds.Legacy);

        // Act
        await _projectInfoListener.UpdatedAsync(new RazorProjectInfo(
            hostProject.Key,
            hostProject.FilePath,
            hostProject.Configuration,
            hostProject.RootNamespace,
            hostProject.DisplayName,
            ProjectWorkspaceState.Default,
            [newDocument]),
            DisposalToken);

        // Assert
        var project = _projectManager.GetRequiredProject(hostProject.Key);
        Assert.Equal(project.DocumentFilePaths, [newDocument.FilePath]);

        var miscProject = _projectManager.GetMiscellaneousProject();
        Assert.Equal(miscProject.DocumentFilePaths, [hostDocument.FilePath]);
    }

    [Fact]
    public async Task IProjectInfoListener_UpdatedAsync_KnownDocuments()
    {
        // Arrange
        var hostProject = new HostProject("path/to/project.csproj", "path/to/obj", RazorConfiguration.Default, "TestRootNamespace");
        var document = new HostDocument("path/to/file.cshtml", "file.cshtml", FileKinds.Legacy);

        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(hostProject);
            updater.AddDocument(hostProject.Key, document, StrictMock.Of<TextLoader>());
        });

        var newDocument = new DocumentSnapshotHandle(document.FilePath, document.TargetPath, document.FileKind);

        using var listener = _projectManager.ListenToNotifications();

        // Act & Assert
        await _projectInfoListener.UpdatedAsync(new RazorProjectInfo(
            hostProject.Key,
            hostProject.FilePath,
            hostProject.Configuration,
            hostProject.RootNamespace,
            hostProject.DisplayName,
            ProjectWorkspaceState.Default,
            [newDocument]),
            DisposalToken);

        listener.AssertNoNotifications();
    }

    [Fact]
    public async Task IProjectInfoListener_UpdatedAsync_UpdatesLegacyDocumentsAsComponents()
    {
        // Arrange
        var hostProject = new HostProject("C:/path/to/project.csproj", "C:/path/to/obj", RazorConfiguration.Default, "TestRootNamespace");
        var legacyDocument = new HostDocument("C:/path/to/file.cshtml", "file.cshtml", FileKinds.Legacy);

        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(hostProject);
            updater.AddDocument(hostProject.Key, legacyDocument, StrictMock.Of<TextLoader>());
        });

        var newDocument = new DocumentSnapshotHandle(legacyDocument.FilePath, legacyDocument.TargetPath, FileKinds.Component);

        // Act
        await _projectInfoListener.UpdatedAsync(new RazorProjectInfo(
            hostProject.Key,
            hostProject.FilePath,
            hostProject.Configuration,
            hostProject.RootNamespace,
            hostProject.DisplayName,
            ProjectWorkspaceState.Default,
            [newDocument]),
            DisposalToken);

        // Assert
        var document = _projectManager.GetRequiredDocument(hostProject.Key, newDocument.FilePath);

        Assert.Equal(FileKinds.Component, document.FileKind);
    }

    [Fact]
    public async Task IProjectInfoListener_UpdatedAsync_SameConfigurationDifferentRootNamespace_UpdatesRootNamespace()
    {
        // Arrange
        const string ProjectFilePath = "C:/path/to/project.csproj";
        const string IntermediateOutputPath = "C:/path/to/obj";
        const string NewRootNamespace = "NewRootNamespace";

        var ownerProjectKey = await _projectService.GetTestAccessor().AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath, RazorConfiguration.Default, rootNamespace: null, displayName: null, DisposalToken);

        var ownerProject = _projectManager.GetRequiredProject(ownerProjectKey);

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectInfoListener.UpdatedAsync(new RazorProjectInfo(
            ownerProject.Key,
            ownerProject.FilePath,
            ownerProject.Configuration,
            NewRootNamespace,
            ownerProject.DisplayName,
            ProjectWorkspaceState.Default,
            documents: []),
            DisposalToken);

        var notification = Assert.Single(listener);
        Assert.NotNull(notification.Older);
        Assert.Null(notification.Older.RootNamespace);
        Assert.NotNull(notification.Newer);
        Assert.Equal(NewRootNamespace, notification.Newer.RootNamespace);
    }

    [Fact]
    public async Task IProjectInfoListener_UpdatedAsync_SameConfigurationAndRootNamespaceNoops()
    {
        // Arrange
        const string ProjectFilePath = "C:/path/to/project.csproj";
        const string IntermediateOutputPath = "C:/path/to/obj";
        const string RootNamespace = "TestRootNamespace";

        var ownerProjectKey = await _projectService.GetTestAccessor().AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);

        var ownerProject = _projectManager.GetRequiredProject(ownerProjectKey);

        using var listener = _projectManager.ListenToNotifications();

        // Act & Assert
        await _projectInfoListener.UpdatedAsync(new RazorProjectInfo(
            ownerProject.Key,
            ownerProject.FilePath,
            ownerProject.Configuration,
            ownerProject.RootNamespace,
            displayName: "",
            ProjectWorkspaceState.Default,
            documents: []),
            DisposalToken);

        listener.AssertNoNotifications();
    }

    [Fact]
    public async Task IProjectInfoListener_UpdatedAsync_ChangesProjectToUseProvidedConfiguration()
    {
        // Arrange
        const string ProjectFilePath = "C:/path/to/project.csproj";
        const string IntermediateOutputPath = "C:/path/to/obj";
        const string RootNamespace = "TestRootNamespace";

        var ownerProjectKey = await _projectService.GetTestAccessor().AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);

        var ownerProject = _projectManager.GetRequiredProject(ownerProjectKey);

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectInfoListener.UpdatedAsync(new RazorProjectInfo(
            ownerProject.Key,
            ownerProject.FilePath,
            FallbackRazorConfiguration.MVC_1_1,
            "TestRootNamespace",
            displayName: "",
            ProjectWorkspaceState.Default,
            documents: []),
            DisposalToken);

        // Assert
        var notification = Assert.Single(listener);
        Assert.NotNull(notification.Newer);
        Assert.Same(FallbackRazorConfiguration.MVC_1_1, notification.Newer.Configuration);
    }

    [Fact]
    public async Task CloseDocument_ClosesDocumentInOwnerProject()
    {
        // Arrange
        const string ProjectFilePath = "C:/path/to/project.csproj";
        const string IntermediateOutputPath = "C:/path/to/obj";
        const string RootNamespace = "TestRootNamespace";
        const string DocumentFilePath = "C:/path/to/document.cshtml";

        var ownerProjectKey = await _projectService.GetTestAccessor().AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);
        await _projectService.AddDocumentToPotentialProjectsAsync(DocumentFilePath, DisposalToken);
        await _projectService.OpenDocumentAsync(DocumentFilePath, s_emptyText, DisposalToken);

        Assert.True(_projectManager.IsDocumentOpen(DocumentFilePath));

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.CloseDocumentAsync(DocumentFilePath, DisposalToken);

        // Assert
        listener.AssertNotifications(
            x => x.DocumentChanged(DocumentFilePath, ownerProjectKey));

        Assert.False(_projectManager.IsDocumentOpen(DocumentFilePath));
    }

    [Fact]
    public async Task CloseDocument_ClosesDocumentInAllOwnerProjects()
    {
        // Arrange
        const string ProjectFilePath = "C:/path/to/project.csproj";
        const string IntermediateOutputPath1 = "C:/path/to/obj/net6";
        const string IntermediateOutputPath2 = "C:/path/to/obj/net7";
        const string RootNamespace = "TestRootNamespace";
        const string DocumentFilePath = "C:/path/to/document.cshtml";

        var ownerProjectKey1 = await _projectService.GetTestAccessor().AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath1, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);
        var ownerProjectKey2 = await _projectService.GetTestAccessor().AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath2, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);
        await _projectService.AddDocumentToPotentialProjectsAsync(DocumentFilePath, DisposalToken);
        await _projectService.OpenDocumentAsync(DocumentFilePath, s_emptyText, DisposalToken);

        Assert.True(_projectManager.IsDocumentOpen(DocumentFilePath));

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.CloseDocumentAsync(DocumentFilePath, DisposalToken);

        // Assert
        listener.AssertNotifications(
            x => x.DocumentChanged(DocumentFilePath, ownerProjectKey1),
            x => x.DocumentChanged(DocumentFilePath, ownerProjectKey2));

        Assert.False(_projectManager.IsDocumentOpen(DocumentFilePath));
    }

    [Fact]
    public async Task CloseDocument_ClosesDocumentInMiscellaneousProject()
    {
        // Arrange
        const string DocumentFilePath = "document.cshtml";

        await _projectService.AddDocumentToPotentialProjectsAsync(DocumentFilePath, DisposalToken);
        await _projectService.OpenDocumentAsync(DocumentFilePath, s_emptyText, DisposalToken);

        var miscProject = _projectManager.GetMiscellaneousProject();

        Assert.True(_projectManager.IsDocumentOpen(DocumentFilePath));

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.CloseDocumentAsync(DocumentFilePath, DisposalToken);

        // Assert
        listener.AssertNotifications(
            x => x.DocumentChanged(DocumentFilePath, miscProject.Key));

        Assert.False(_projectManager.IsDocumentOpen(DocumentFilePath));
    }

    [Fact]
    public async Task OpenDocument_OpensAlreadyAddedDocumentInOwnerProject()
    {
        // Arrange
        const string ProjectFilePath = "C:/path/to/project.csproj";
        const string IntermediateOutputPath = "C:/path/to/obj";
        const string RootNamespace = "TestRootNamespace";
        const string DocumentFilePath = "C:/path/to/document.cshtml";

        var ownerProjectKey = await _projectService.GetTestAccessor().AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);
        await _projectService.AddDocumentToPotentialProjectsAsync(DocumentFilePath, DisposalToken);

        Assert.False(_projectManager.IsDocumentOpen(DocumentFilePath));

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.OpenDocumentAsync(DocumentFilePath, SourceText.From("Hello World"), DisposalToken);

        // Assert
        listener.AssertNotifications(
            x => x.DocumentChanged(DocumentFilePath, ownerProjectKey));

        Assert.True(_projectManager.IsDocumentOpen(DocumentFilePath));
    }

    [Fact]
    public async Task OpenDocument_OpensAlreadyAddedDocumentInAllOwnerProjects()
    {
        // Arrange
        const string ProjectFilePath = "C:/path/to/project.csproj";
        const string IntermediateOutputPath1 = "C:/path/to/obj/net6";
        const string IntermediateOutputPath2 = "C:/path/to/obj/net7";
        const string RootNamespace = "TestRootNamespace";
        const string DocumentFilePath = "C:/path/to/document.cshtml";

        var ownerProjectKey1 = await _projectService.GetTestAccessor().AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath1, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);
        var ownerProjectKey2 = await _projectService.GetTestAccessor().AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath2, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);
        await _projectService.AddDocumentToPotentialProjectsAsync(DocumentFilePath, DisposalToken);

        Assert.False(_projectManager.IsDocumentOpen(DocumentFilePath));

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.OpenDocumentAsync(DocumentFilePath, SourceText.From("Hello World"), DisposalToken);

        // Assert
        listener.AssertNotifications(
            x => x.DocumentChanged(DocumentFilePath, ownerProjectKey1),
            x => x.DocumentChanged(DocumentFilePath, ownerProjectKey2));

        Assert.True(_projectManager.IsDocumentOpen(DocumentFilePath));
    }

    [Fact]
    public async Task OpenDocument_OpensAlreadyAddedDocumentInMiscellaneousProject()
    {
        // Arrange
        const string DocumentFilePath = "document.cshtml";

        await _projectService.AddDocumentToMiscProjectAsync(DocumentFilePath, DisposalToken);

        var miscProject = _projectManager.GetMiscellaneousProject();

        Assert.False(_projectManager.IsDocumentOpen(DocumentFilePath));

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.OpenDocumentAsync(DocumentFilePath, s_emptyText, DisposalToken);

        // Assert
        listener.AssertNotifications(
            x => x.DocumentChanged(DocumentFilePath, miscProject.Key));

        Assert.True(_projectManager.IsDocumentOpen(DocumentFilePath));
    }

    [Fact]
    public async Task OpenDocument_OpensAndAddsDocumentToMiscellaneousProject()
    {
        // Arrange
        const string ProjectFilePath = "C:/path/to/project.csproj";
        const string IntermediateOutputPath = "C:/path/to/obj";
        const string RootNamespace = "TestRootNamespace";
        const string DocumentFilePath = "C:/path/to/document.cshtml";

        var ownerProjectKey = await _projectService.GetTestAccessor().AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);

        var miscProject = _projectManager.GetMiscellaneousProject();

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.OpenDocumentAsync(DocumentFilePath, SourceText.From("Hello World"), DisposalToken);

        // Assert
        listener.AssertNotifications(
            x => x.DocumentAdded(DocumentFilePath, miscProject.Key),
            x => x.DocumentChanged(DocumentFilePath, miscProject.Key));

        Assert.True(_projectManager.IsDocumentOpen(DocumentFilePath));
    }

    [Fact]
    public async Task AddDocument_NoopsIfDocumentIsAlreadyAdded()
    {
        // Arrange
        const string DocumentFilePath = "document.cshtml";

        await _projectService.AddDocumentToPotentialProjectsAsync(DocumentFilePath, DisposalToken);

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.AddDocumentToPotentialProjectsAsync(DocumentFilePath, DisposalToken);

        // Assert
        listener.AssertNoNotifications();
    }

    [Fact]
    public async Task AddDocument_AddsDocumentToOwnerProject()
    {
        // Arrange
        const string ProjectFilePath = "C:/path/to/project.csproj";
        const string IntermediateOutputPath = "C:/path/to/obj";
        const string RootNamespace = "TestRootNamespace";
        const string DocumentFilePath = "C:/path/to/document.cshtml";

        var ownerProjectKey = await _projectService.GetTestAccessor().AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);

        var ownerProject = _projectManager.GetRequiredProject(ownerProjectKey);

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.AddDocumentToPotentialProjectsAsync(DocumentFilePath, DisposalToken);

        // Assert
        listener.AssertNotifications(
            x => x.DocumentAdded(DocumentFilePath, ownerProjectKey));

        Assert.False(_projectManager.IsDocumentOpen(DocumentFilePath));
    }

    [Fact]
    public async Task AddDocumentToMiscProjectAsync_AddsDocumentToMiscellaneousProject()
    {
        // Arrange
        const string DocumentFilePath = "document.cshtml";

        var miscProject = _projectManager.GetMiscellaneousProject();

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.AddDocumentToMiscProjectAsync(DocumentFilePath, DisposalToken);

        // Assert
        listener.AssertNotifications(
            x => x.DocumentAdded(DocumentFilePath, miscProject.Key));

        Assert.False(_projectManager.IsDocumentOpen(DocumentFilePath));
    }

    [Fact]
    public async Task AddDocumentToMiscProjectAsync_IgnoresKnownDocument()
    {
        // Arrange
        const string ProjectFilePath = "C:/path/to/project.csproj";
        const string IntermediateOutputPath = "C:/path/to/obj";
        const string RootNamespace = "TestRootNamespace";
        const string DocumentFilePath = "C:/path/to/document.cshtml";

        await _projectService.GetTestAccessor().AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);
        await _projectService.AddDocumentToPotentialProjectsAsync(DocumentFilePath, DisposalToken);

        // Act
        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.AddDocumentToMiscProjectAsync(DocumentFilePath, DisposalToken);

        // Assert
        listener.AssertNoNotifications();
    }

    [Fact]
    public async Task AddDocumentToMiscProjectAsync_IgnoresKnownDocument_InMiscFiles()
    {
        // Arrange
        const string DocumentFilePath = "C:/path/to/document.cshtml";

        await _projectService.AddDocumentToMiscProjectAsync(DocumentFilePath, DisposalToken);

        // Act
        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.AddDocumentToMiscProjectAsync(DocumentFilePath, DisposalToken);

        // Assert
        listener.AssertNoNotifications();
    }

    [Fact]
    public async Task RemoveDocument_RemovesDocumentFromOwnerProject()
    {
        // Arrange
        const string ProjectFilePath = "C:/path/to/project.csproj";
        const string IntermediateOutputPath = "C:/path/to/obj";
        const string RootNamespace = "TestRootNamespace";
        const string DocumentFilePath = "C:/path/to/document.cshtml";

        var ownerProjectKey = await _projectService.GetTestAccessor().AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);
        await _projectService.AddDocumentToPotentialProjectsAsync(DocumentFilePath, DisposalToken);

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.RemoveDocumentAsync(DocumentFilePath, DisposalToken);

        // Assert
        listener.AssertNotifications(
            x => x.DocumentRemoved(DocumentFilePath, ownerProjectKey));

        Assert.False(_projectManager.IsDocumentOpen(DocumentFilePath));
    }

    [Fact]
    public async Task RemoveDocument_RemovesDocumentFromAllOwnerProjects()
    {
        // Arrange
        const string ProjectFilePath = "C:/path/to/project.csproj";
        const string IntermediateOutputPath1 = "C:/path/to/obj/net6";
        const string IntermediateOutputPath2 = "C:/path/to/obj/net7";
        const string RootNamespace = "TestRootNamespace";
        const string DocumentFilePath = "C:/path/to/document.cshtml";

        var ownerProjectKey1 = await _projectService.GetTestAccessor().AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath1, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);
        var ownerProjectKey2 = await _projectService.GetTestAccessor().AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath2, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);
        await _projectService.AddDocumentToPotentialProjectsAsync(DocumentFilePath, DisposalToken);

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.RemoveDocumentAsync(DocumentFilePath, DisposalToken);

        // Assert
        listener.AssertNotifications(
            x => x.DocumentRemoved(DocumentFilePath, ownerProjectKey1),
            x => x.DocumentRemoved(DocumentFilePath, ownerProjectKey2));

        Assert.False(_projectManager.IsDocumentOpen(DocumentFilePath));
    }

    [Fact]
    public async Task RemoveOpenDocument_RemovesDocumentFromOwnerProject_MovesToMiscellaneousProject()
    {
        // Arrange
        const string ProjectFilePath = "C:/path/to/project.csproj";
        const string IntermediateOutputPath = "C:/path/to/obj";
        const string RootNamespace = "TestRootNamespace";
        const string DocumentFilePath = "C:/path/to/document.cshtml";

        var ownerProjectKey = await _projectService.GetTestAccessor().AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);
        await _projectService.AddDocumentToPotentialProjectsAsync(DocumentFilePath, DisposalToken);
        await _projectService.OpenDocumentAsync(DocumentFilePath, s_emptyText, DisposalToken);

        Assert.True(_projectManager.IsDocumentOpen(DocumentFilePath));

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.RemoveDocumentAsync(DocumentFilePath, DisposalToken);

        // Assert
        listener.AssertNotifications(
            x => x.DocumentRemoved(DocumentFilePath, ownerProjectKey),
            x => x.DocumentAdded(DocumentFilePath, MiscFilesProject.Key));

        Assert.True(_projectManager.IsDocumentOpen(DocumentFilePath));
    }

    [Fact]
    public async Task RemoveDocument_RemovesDocumentFromMiscellaneousProject()
    {
        // Arrange
        const string DocumentFilePath = "document.cshtml";

        await _projectService.AddDocumentToMiscProjectAsync(DocumentFilePath, DisposalToken);

        var miscProject = _projectManager.GetMiscellaneousProject();

        Assert.False(_projectManager.IsDocumentOpen(DocumentFilePath));

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.RemoveDocumentAsync(DocumentFilePath, DisposalToken);

        // Assert
        listener.AssertNotifications(
            x => x.DocumentRemoved(DocumentFilePath, miscProject.Key));
    }

    [Fact]
    public async Task RemoveDocument_NoopsIfOwnerProjectDoesNotContainDocument()
    {
        // Arrange
        const string DocumentFilePath = "C:/path/to/document.cshtml";

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.RemoveDocumentAsync(DocumentFilePath, DisposalToken);

        // Assert
        listener.AssertNoNotifications();
    }

    [Fact]
    public async Task RemoveDocument_NoopsIfMiscellaneousProjectDoesNotContainDocument()
    {
        // Arrange
        const string DocumentFilePath = "document.cshtml";

        Assert.False(_projectManager.IsDocumentOpen(DocumentFilePath));

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.RemoveDocumentAsync(DocumentFilePath, DisposalToken);

        // Assert
        listener.AssertNoNotifications();
    }

    [Fact]
    public async Task UpdateDocument_ChangesDocumentInOwnerProject()
    {
        // Arrange
        const string ProjectFilePath = "C:/path/to/project.csproj";
        const string IntermediateOutputPath = "C:/path/to/obj";
        const string RootNamespace = "TestRootNamespace";
        const string DocumentFilePath = "C:/path/to/document.cshtml";

        var ownerProjectKey = await _projectService.GetTestAccessor().AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);
        await _projectService.AddDocumentToPotentialProjectsAsync(DocumentFilePath, DisposalToken);
        await _projectService.OpenDocumentAsync(DocumentFilePath, s_emptyText, DisposalToken);

        Assert.True(_projectManager.IsDocumentOpen(DocumentFilePath));

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.UpdateDocumentAsync(DocumentFilePath, s_emptyText.Replace(0, 0, "Hello World"), DisposalToken);

        // Assert
        listener.AssertNotifications(
            x => x.DocumentChanged(DocumentFilePath, ownerProjectKey));

        Assert.True(_projectManager.IsDocumentOpen(DocumentFilePath));
    }

    [Fact]
    public async Task UpdateDocument_ChangesDocumentInAllOwnerProjects()
    {
        // Arrange
        const string ProjectFilePath = "C:/path/to/project.csproj";
        const string IntermediateOutputPath1 = "C:/path/to/obj/net6";
        const string IntermediateOutputPath2 = "C:/path/to/obj/net7";
        const string RootNamespace = "TestRootNamespace";
        const string DocumentFilePath = "C:/path/to/document.cshtml";

        var ownerProjectKey1 = await _projectService.GetTestAccessor().AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath1, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);
        var ownerProjectKey2 = await _projectService.GetTestAccessor().AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath2, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);
        await _projectService.AddDocumentToPotentialProjectsAsync(DocumentFilePath, DisposalToken);
        await _projectService.OpenDocumentAsync(DocumentFilePath, s_emptyText, DisposalToken);

        Assert.True(_projectManager.IsDocumentOpen(DocumentFilePath));

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.UpdateDocumentAsync(DocumentFilePath, s_emptyText.Replace(0, 0, "Hello World"), DisposalToken);

        // Assert
        listener.AssertNotifications(
            x => x.DocumentChanged(DocumentFilePath, ownerProjectKey1),
            x => x.DocumentChanged(DocumentFilePath, ownerProjectKey2));

        Assert.True(_projectManager.IsDocumentOpen(DocumentFilePath));
    }

    [Fact]
    public async Task UpdateDocument_ChangesDocumentInMiscProject()
    {
        // Arrange
        const string DocumentFilePath = "document.cshtml";

        await _projectService.AddDocumentToPotentialProjectsAsync(DocumentFilePath, DisposalToken);
        await _projectService.OpenDocumentAsync(DocumentFilePath, s_emptyText, DisposalToken);

        var miscProject = _projectManager.GetMiscellaneousProject();

        Assert.True(_projectManager.IsDocumentOpen(DocumentFilePath));

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.UpdateDocumentAsync(DocumentFilePath, s_emptyText.Replace(0, 0, "Hello World"), DisposalToken);

        // Assert
        listener.AssertNotifications(
            x => x.DocumentChanged(DocumentFilePath, miscProject.Key));

        Assert.True(_projectManager.IsDocumentOpen(DocumentFilePath));
    }

    [Fact]
    public async Task UpdateDocument_DocumentVersionUpdated()
    {
        // Arrange
        const string ProjectFilePath = "C:/path/to/project.csproj";
        const string IntermediateOutputPath = "C:/path/to/obj";
        const string RootNamespace = "TestRootNamespace";
        const string DocumentFilePath = "C:/path/to/document.cshtml";

        var ownerProjectKey = await _projectService.GetTestAccessor().AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);
        await _projectService.AddDocumentToPotentialProjectsAsync(DocumentFilePath, DisposalToken);

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.UpdateDocumentAsync(DocumentFilePath, s_emptyText.Replace(0, 0, "Hello World"), DisposalToken);

        // Assert
        listener.AssertNotifications(
            x => x.DocumentChanged(DocumentFilePath, ownerProjectKey));

        var latestVersion = _projectManager.GetRequiredDocument(ownerProjectKey, DocumentFilePath).Version;

        Assert.Equal(2, latestVersion);
    }

    [Fact]
    public async Task IRazorProjectInfoListener_UpdatedAsync_AddsProjectWithSpecifiedConfiguration()
    {
        // Arrange
        const string ProjectFilePath = "C:/path/to/project.csproj";
        const string IntermediateOutputPath = "C:/path/to/obj";
        const string RootNamespace = "My.Root.Namespace";

        var configuration = new RazorConfiguration(RazorLanguageVersion.Version_1_0, "TestName", Extensions: []);
        var projectKey = new ProjectKey(IntermediateOutputPath);

        // Act
        await _projectInfoListener.UpdatedAsync(new RazorProjectInfo(
            projectKey,
            ProjectFilePath,
            configuration,
            RootNamespace,
            "ProjectDisplayName",
            ProjectWorkspaceState.Default,
            documents: []),
            DisposalToken);

        var project = _projectManager.GetRequiredProject(projectKey);

        // Assert
        Assert.Equal(ProjectFilePath, project.FilePath);
        Assert.Same(configuration, project.Configuration);
        Assert.Equal(RootNamespace, project.RootNamespace);
    }

    private static TextLoader CreateTextLoader(SourceText text)
    {
        var textLoaderMock = new StrictMock<TextLoader>();
        textLoaderMock
            .Setup(x => x.LoadTextAndVersionAsync(It.IsAny<LoadTextOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TextAndVersion.Create(text, VersionStamp.Create()));

        return textLoaderMock.Object;
    }

    private static TextLoader CreateEmptyTextLoader()
    {
        return CreateTextLoader(s_emptyText);
    }
}
