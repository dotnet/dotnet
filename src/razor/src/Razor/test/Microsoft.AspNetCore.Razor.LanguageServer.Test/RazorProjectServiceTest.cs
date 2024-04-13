// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
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
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Moq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class RazorProjectServiceTest : LanguageServerTestBase
{
    private static readonly SourceText s_emptyText = SourceText.From("");

    private readonly TestProjectSnapshotManager _projectManager;
    private readonly SnapshotResolver _snapshotResolver;
    private readonly DocumentVersionCache _documentVersionCache;
    private readonly RazorProjectService _projectService;

    public RazorProjectServiceTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _projectManager = CreateProjectSnapshotManager();
        _snapshotResolver = new SnapshotResolver(_projectManager, LoggerFactory);
        _documentVersionCache = new DocumentVersionCache(_projectManager);

        var remoteTextLoaderFactoryMock = new StrictMock<RemoteTextLoaderFactory>();
        remoteTextLoaderFactoryMock
            .Setup(x => x.Create(It.IsAny<string>()))
            .Returns(CreateEmptyTextLoader());

        _projectService = new RazorProjectService(
            remoteTextLoaderFactoryMock.Object,
            _snapshotResolver,
            _documentVersionCache,
            _projectManager,
            LoggerFactory);
    }

    [Fact]
    public async Task UpdateProject_UpdatesProjectWorkspaceState()
    {
        // Arrange
        var hostProject = new HostProject("C:/path/to/project.csproj", "C:/path/to/obj", RazorConfiguration.Default, "TestRootNamespace");

        await _projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(hostProject);
        });

        var projectWorkspaceState = ProjectWorkspaceState.Create(LanguageVersion.LatestMajor);

        // Act
        await _projectService.UpdateProjectAsync(
            hostProject.Key,
            hostProject.Configuration,
            hostProject.RootNamespace,
            hostProject.DisplayName,
            projectWorkspaceState,
            documents: [],
            DisposalToken);

        // Assert
        var project = _projectManager.GetLoadedProject(hostProject.Key);
        Assert.Same(projectWorkspaceState, project.ProjectWorkspaceState);
    }

    [Fact]
    public async Task UpdateProject_UpdatingDocument_MapsRelativeFilePathToActualDocument()
    {
        // Arrange
        var hostProject = new HostProject("C:/path/to/project.csproj", "C:/path/to/obj", RazorConfiguration.Default, "TestRootNamespace");
        var hostDocument = new HostDocument("C:/path/to/file.cshtml", "file.cshtml", FileKinds.Legacy);

        await _projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(hostProject);
            updater.DocumentAdded(hostProject.Key, hostDocument, StrictMock.Of<TextLoader>());
        });

        var newDocument = new DocumentSnapshotHandle("file.cshtml", "file.cshtml", FileKinds.Component);

        // Act
        await _projectService.UpdateProjectAsync(
            hostProject.Key,
            hostProject.Configuration,
            hostProject.RootNamespace,
            hostProject.DisplayName,
            ProjectWorkspaceState.Default,
            [newDocument],
            DisposalToken);

        // Assert
        var project = _projectManager.GetLoadedProject(hostProject.Key);
        var document = project.GetDocument(hostDocument.FilePath);
        Assert.NotNull(document);
        Assert.Equal(FileKinds.Component, document.FileKind);
    }

    [Fact]
    public async Task UpdateProject_AddsNewDocuments()
    {
        // Arrange
        var hostProject = new HostProject("C:/path/to/project.csproj", "C:/path/to/obj", RazorConfiguration.Default, "TestRootNamespace");
        var hostDocument = new HostDocument("C:/path/to/file.cshtml", "file.cshtml", FileKinds.Legacy);

        await _projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(hostProject);
            updater.DocumentAdded(hostProject.Key, hostDocument, StrictMock.Of<TextLoader>());
        });

        var oldDocument = new DocumentSnapshotHandle(hostDocument.FilePath, hostDocument.TargetPath, hostDocument.FileKind);
        var newDocument = new DocumentSnapshotHandle("C:/path/to/file2.cshtml", "file2.cshtml", FileKinds.Legacy);

        // Act
        await _projectService.UpdateProjectAsync(
            hostProject.Key,
            hostProject.Configuration,
            hostProject.RootNamespace,
            hostProject.DisplayName,
            ProjectWorkspaceState.Default,
            [oldDocument, newDocument],
            DisposalToken);

        // Assert
        var project = _projectManager.GetLoadedProject(hostProject.Key);
        var projectFilePaths = project.DocumentFilePaths.OrderBy(path => path);
        Assert.Equal(projectFilePaths, [oldDocument.FilePath, newDocument.FilePath]);
    }

    [Fact]
    public async Task UpdateProject_MovesDocumentsFromMisc()
    {
        // Arrange
        var hostProject = new HostProject("C:/path/to/project.csproj", "C:/path/to/obj", RazorConfiguration.Default, "TestRootNamespace");
        var hostDocument = new HostDocument("C:/path/to/file.cshtml", "file.cshtml", FileKinds.Legacy);

        var miscProject = await _snapshotResolver.GetMiscellaneousProjectAsync(DisposalToken);

        await _projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(hostProject);
            updater.DocumentAdded(miscProject.Key, hostDocument, StrictMock.Of<TextLoader>());
        });

        var project = _projectManager.GetLoadedProject(hostProject.Key);

        var addedDocument = new DocumentSnapshotHandle(hostDocument.FilePath, hostDocument.TargetPath, hostDocument.FileKind);

        // Act
        await _projectService.UpdateProjectAsync(
            hostProject.Key,
            hostProject.Configuration,
            hostProject.RootNamespace,
            hostProject.DisplayName,
            ProjectWorkspaceState.Default,
            [addedDocument],
            DisposalToken);

        // Assert
        project = _projectManager.GetLoadedProject(hostProject.Key);
        var projectFilePaths = project.DocumentFilePaths.OrderBy(path => path);
        Assert.Equal(projectFilePaths, [addedDocument.FilePath]);
        miscProject = _projectManager.GetLoadedProject(miscProject.Key);
        Assert.Empty(miscProject.DocumentFilePaths);
    }

    [Fact]
    public async Task UpdateProject_MovesExistingDocumentToMisc()
    {
        // Arrange
        var hostProject = new HostProject("C:/path/to/project.csproj", "C:/path/to/obj", RazorConfiguration.Default, "TestRootNamespace");
        var hostDocument = new HostDocument("C:/path/to/file.cshtml", "file.cshtml", FileKinds.Legacy);

        var miscProject = await _snapshotResolver.GetMiscellaneousProjectAsync(DisposalToken);

        var project = await _projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(hostProject);

            var project = updater.GetLoadedProject(hostProject.Key);
            updater.DocumentAdded(hostProject.Key, hostDocument, StrictMock.Of<TextLoader>());

            return project;
        });

        var newDocument = new DocumentSnapshotHandle("C:/path/to/file2.cshtml", "file2.cshtml", FileKinds.Legacy);

        // Act
        await _projectService.UpdateProjectAsync(
            hostProject.Key,
            hostProject.Configuration,
            hostProject.RootNamespace,
            hostProject.DisplayName,
            ProjectWorkspaceState.Default,
            [newDocument],
            DisposalToken);

        // Assert
        project = _projectManager.GetLoadedProject(hostProject.Key);
        Assert.Equal(project.DocumentFilePaths, [newDocument.FilePath]);

        miscProject = _projectManager.GetLoadedProject(miscProject.Key);
        Assert.Equal(miscProject.DocumentFilePaths, [hostDocument.FilePath]);
    }

    [Fact]
    public async Task UpdateProject_KnownDocuments()
    {
        // Arrange
        var hostProject = new HostProject("path/to/project.csproj", "path/to/obj", RazorConfiguration.Default, "TestRootNamespace");
        var document = new HostDocument("path/to/file.cshtml", "file.cshtml", FileKinds.Legacy);

        // Note: We acquire the miscellaneous project here to avoid a spurious 'ProjectAdded'
        // notification when it would get created by UpdateProject(...) below.
        _ = await _snapshotResolver.GetMiscellaneousProjectAsync(DisposalToken);

        await _projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(hostProject);
            updater.DocumentAdded(hostProject.Key, document, StrictMock.Of<TextLoader>());
        });

        var newDocument = new DocumentSnapshotHandle(document.FilePath, document.TargetPath, document.FileKind);

        using var listener = _projectManager.ListenToNotifications();

        // Act & Assert
        await _projectService.UpdateProjectAsync(
            hostProject.Key,
            hostProject.Configuration,
            hostProject.RootNamespace,
            hostProject.DisplayName,
            ProjectWorkspaceState.Default,
            [newDocument],
            DisposalToken);

        listener.AssertNoNotifications();
    }

    [Fact]
    public async Task UpdateProject_UpdatesLegacyDocumentsAsComponents()
    {
        // Arrange
        var hostProject = new HostProject("C:/path/to/project.csproj", "C:/path/to/obj", RazorConfiguration.Default, "TestRootNamespace");
        var legacyDocument = new HostDocument("C:/path/to/file.cshtml", "file.cshtml", FileKinds.Legacy);

        await _projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(hostProject);
            updater.DocumentAdded(hostProject.Key, legacyDocument, StrictMock.Of<TextLoader>());
        });

        var newDocument = new DocumentSnapshotHandle(legacyDocument.FilePath, legacyDocument.TargetPath, FileKinds.Component);

        // Act
        await _projectService.UpdateProjectAsync(
            hostProject.Key,
            hostProject.Configuration,
            hostProject.RootNamespace,
            hostProject.DisplayName,
            ProjectWorkspaceState.Default,
            [newDocument],
            DisposalToken);

        // Assert
        var project = _projectManager.GetLoadedProject(hostProject.Key);
        var document = project.GetDocument(newDocument.FilePath);
        Assert.NotNull(document);
        Assert.Equal(FileKinds.Component, document.FileKind);
    }

    [Fact]
    public async Task UpdateProject_SameConfigurationDifferentRootNamespace_UpdatesRootNamespace()
    {
        // Arrange
        const string ProjectFilePath = "C:/path/to/project.csproj";
        const string IntermediateOutputPath = "C:/path/to/obj";
        const string NewRootNamespace = "NewRootNamespace";

        var ownerProjectKey = await _projectService.AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath, RazorConfiguration.Default, rootNamespace: null, displayName: null, DisposalToken);

        var ownerProject = _projectManager.GetLoadedProject(ownerProjectKey);

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.UpdateProjectAsync(
            ownerProject.Key,
            ownerProject.Configuration,
            NewRootNamespace,
            ownerProject.DisplayName,
            ProjectWorkspaceState.Default,
            documents: [],
            DisposalToken);

        var notification = Assert.Single(listener);
        Assert.NotNull(notification.Older);
        Assert.Null(notification.Older.RootNamespace);
        Assert.NotNull(notification.Newer);
        Assert.Equal(NewRootNamespace, notification.Newer.RootNamespace);
    }

    [Fact]
    public async Task UpdateProject_SameConfigurationAndRootNamespaceNoops()
    {
        // Arrange
        const string ProjectFilePath = "C:/path/to/project.csproj";
        const string IntermediateOutputPath = "C:/path/to/obj";
        const string RootNamespace = "TestRootNamespace";

        var ownerProjectKey = await _projectService.AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);

        var ownerProject = _projectManager.GetLoadedProject(ownerProjectKey);

        using var listener = _projectManager.ListenToNotifications();

        // Act & Assert
        await _projectService.UpdateProjectAsync(
            ownerProject.Key,
            ownerProject.Configuration,
            ownerProject.RootNamespace,
            displayName: "",
            ProjectWorkspaceState.Default,
            documents: [],
            DisposalToken);

        listener.AssertNoNotifications();
    }

    [Fact]
    public async Task UpdateProject_NullConfigurationUsesDefault()
    {
        // Arrange
        const string ProjectFilePath = "C:/path/to/project.csproj";
        const string IntermediateOutputPath = "C:/path/to/obj";
        const string RootNamespace = "TestRootNamespace";

        var ownerProjectKey = await _projectService.AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);

        var ownerProject = _projectManager.GetLoadedProject(ownerProjectKey);

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.UpdateProjectAsync(
            ownerProject.Key,
            configuration: null,
            "TestRootNamespace",
            displayName: "",
            ProjectWorkspaceState.Default,
            documents: [],
            DisposalToken);

        // Assert
        var notification = Assert.Single(listener);
        Assert.NotNull(notification.Newer);
        Assert.Same(FallbackRazorConfiguration.Latest, notification.Newer.Configuration);
    }

    [Fact]
    public async Task UpdateProject_ChangesProjectToUseProvidedConfiguration()
    {
        // Arrange
        const string ProjectFilePath = "C:/path/to/project.csproj";
        const string IntermediateOutputPath = "C:/path/to/obj";
        const string RootNamespace = "TestRootNamespace";

        var ownerProjectKey = await _projectService.AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);

        var ownerProject = _projectManager.GetLoadedProject(ownerProjectKey);

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.UpdateProjectAsync(
            ownerProject.Key,
            FallbackRazorConfiguration.MVC_1_1,
            "TestRootNamespace",
            displayName: "",
            ProjectWorkspaceState.Default,
            documents: [],
            DisposalToken);

        // Assert
        var notification = Assert.Single(listener);
        Assert.NotNull(notification.Newer);
        Assert.Same(FallbackRazorConfiguration.MVC_1_1, notification.Newer.Configuration);
    }

    [Fact]
    public async Task UpdateProject_UntrackedProjectNoops()
    {
        // Arrange
        using var listener = _projectManager.ListenToNotifications();

        // Act & Assert
        await _projectService.UpdateProjectAsync(
            TestProjectKey.Create("C:/path/to/obj"),
            FallbackRazorConfiguration.MVC_1_1,
            "TestRootNamespace",
            displayName: "",
            ProjectWorkspaceState.Default,
            documents: [],
            DisposalToken);

        listener.AssertNoNotifications();
    }

    [Fact]
    public async Task CloseDocument_ClosesDocumentInOwnerProject()
    {
        // Arrange
        const string ProjectFilePath = "C:/path/to/project.csproj";
        const string IntermediateOutputPath = "C:/path/to/obj";
        const string RootNamespace = "TestRootNamespace";
        const string DocumentFilePath = "C:/path/to/document.cshtml";

        var ownerProjectKey = await _projectService.AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);
        await _projectService.AddDocumentAsync(DocumentFilePath, DisposalToken);
        await _projectService.OpenDocumentAsync(DocumentFilePath, s_emptyText, version: 42, DisposalToken);

        var ownerProject = _projectManager.GetLoadedProject(ownerProjectKey);

        Assert.True(_projectManager.IsDocumentOpen(DocumentFilePath));

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.CloseDocumentAsync(DocumentFilePath, DisposalToken);

        // Assert
        listener.AssertNotifications(
            x => x.DocumentChanged(DocumentFilePath, ownerProject.Key));

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

        var ownerProjectKey1 = await _projectService.AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath1, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);
        var ownerProjectKey2 = await _projectService.AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath2, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);
        await _projectService.AddDocumentAsync(DocumentFilePath, DisposalToken);
        await _projectService.OpenDocumentAsync(DocumentFilePath, s_emptyText, version: 42, DisposalToken);

        var ownerProject1 = _projectManager.GetLoadedProject(ownerProjectKey1);
        var ownerProject2 = _projectManager.GetLoadedProject(ownerProjectKey2);

        Assert.True(_projectManager.IsDocumentOpen(DocumentFilePath));

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.CloseDocumentAsync(DocumentFilePath, DisposalToken);

        // Assert
        listener.AssertNotifications(
            x => x.DocumentChanged(DocumentFilePath, ownerProject1.Key),
            x => x.DocumentChanged(DocumentFilePath, ownerProject2.Key));

        Assert.False(_projectManager.IsDocumentOpen(DocumentFilePath));
    }

    [Fact]
    public async Task CloseDocument_ClosesDocumentInMiscellaneousProject()
    {
        // Arrange
        const string DocumentFilePath = "document.cshtml";

        await _projectService.AddDocumentAsync(DocumentFilePath, DisposalToken);
        await _projectService.OpenDocumentAsync(DocumentFilePath, s_emptyText, version: 42, DisposalToken);

        var miscProject = await _snapshotResolver.GetMiscellaneousProjectAsync(DisposalToken);

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

        var ownerProjectKey = await _projectService.AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);
        await _projectService.AddDocumentAsync(DocumentFilePath, DisposalToken);

        var ownerProject = _projectManager.GetLoadedProject(ownerProjectKey);

        Assert.False(_projectManager.IsDocumentOpen(DocumentFilePath));

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.OpenDocumentAsync(DocumentFilePath, SourceText.From("Hello World"), version: 42, DisposalToken);

        // Assert
        listener.AssertNotifications(
            x => x.DocumentChanged(DocumentFilePath, ownerProject.Key));

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

        var ownerProjectKey1 = await _projectService.AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath1, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);
        var ownerProjectKey2 = await _projectService.AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath2, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);
        await _projectService.AddDocumentAsync(DocumentFilePath, DisposalToken);

        var ownerProject1 = _projectManager.GetLoadedProject(ownerProjectKey1);
        var ownerProject2 = _projectManager.GetLoadedProject(ownerProjectKey2);

        Assert.False(_projectManager.IsDocumentOpen(DocumentFilePath));

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.OpenDocumentAsync(DocumentFilePath, SourceText.From("Hello World"), version: 42, DisposalToken);

        // Assert
        listener.AssertNotifications(
            x => x.DocumentChanged(DocumentFilePath, ownerProject1.Key),
            x => x.DocumentChanged(DocumentFilePath, ownerProject2.Key));

        Assert.True(_projectManager.IsDocumentOpen(DocumentFilePath));
    }

    [Fact]
    public async Task OpenDocument_OpensAlreadyAddedDocumentInMiscellaneousProject()
    {
        // Arrange
        const string DocumentFilePath = "document.cshtml";

        await _projectService.AddDocumentAsync(DocumentFilePath, DisposalToken);

        var miscProject = await _snapshotResolver.GetMiscellaneousProjectAsync(DisposalToken);

        Assert.False(_projectManager.IsDocumentOpen(DocumentFilePath));

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.OpenDocumentAsync(DocumentFilePath, s_emptyText, version: 42, DisposalToken);

        // Assert
        listener.AssertNotifications(
            x => x.DocumentChanged(DocumentFilePath, miscProject.Key));

        Assert.True(_projectManager.IsDocumentOpen(DocumentFilePath));
    }

    [Fact]
    public async Task OpenDocument_OpensAndAddsDocumentToOwnerProject()
    {
        // Arrange
        const string ProjectFilePath = "C:/path/to/project.csproj";
        const string IntermediateOutputPath = "C:/path/to/obj";
        const string RootNamespace = "TestRootNamespace";
        const string DocumentFilePath = "C:/path/to/document.cshtml";

        var ownerProjectKey = await _projectService.AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);

        var ownerProject = _projectManager.GetLoadedProject(ownerProjectKey);

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.OpenDocumentAsync(DocumentFilePath, SourceText.From("Hello World"), version: 42, DisposalToken);

        // Assert
        listener.AssertNotifications(
            x => x.DocumentAdded(DocumentFilePath, ownerProject.Key),
            x => x.DocumentChanged(DocumentFilePath, ownerProject.Key));

        Assert.True(_projectManager.IsDocumentOpen(DocumentFilePath));
    }

    [Fact]
    public async Task AddDocument_NoopsIfDocumentIsAlreadyAdded()
    {
        // Arrange
        const string DocumentFilePath = "document.cshtml";

        await _projectService.AddDocumentAsync(DocumentFilePath, DisposalToken);

        var miscProject = await _snapshotResolver.GetMiscellaneousProjectAsync(DisposalToken);

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.AddDocumentAsync(DocumentFilePath, DisposalToken);

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

        var ownerProjectKey = await _projectService.AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);

        var ownerProject = _projectManager.GetLoadedProject(ownerProjectKey);

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.AddDocumentAsync(DocumentFilePath, DisposalToken);

        // Assert
        listener.AssertNotifications(
            x => x.DocumentAdded(DocumentFilePath, ownerProject.Key));

        Assert.False(_projectManager.IsDocumentOpen(DocumentFilePath));
    }

    [Fact]
    public async Task AddDocument_AddsDocumentToMiscellaneousProject()
    {
        // Arrange
        const string DocumentFilePath = "document.cshtml";

        var miscProject = await _snapshotResolver.GetMiscellaneousProjectAsync(DisposalToken);

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.AddDocumentAsync(DocumentFilePath, DisposalToken);

        // Assert
        listener.AssertNotifications(
            x => x.DocumentAdded(DocumentFilePath, miscProject.Key));

        Assert.False(_projectManager.IsDocumentOpen(DocumentFilePath));
    }

    [Fact]
    public async Task RemoveDocument_RemovesDocumentFromOwnerProject()
    {
        // Arrange
        const string ProjectFilePath = "C:/path/to/project.csproj";
        const string IntermediateOutputPath = "C:/path/to/obj";
        const string RootNamespace = "TestRootNamespace";
        const string DocumentFilePath = "C:/path/to/document.cshtml";

        var ownerProjectKey = await _projectService.AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);
        await _projectService.AddDocumentAsync(DocumentFilePath, DisposalToken);

        var ownerProject = _projectManager.GetLoadedProject(ownerProjectKey);

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.RemoveDocumentAsync(DocumentFilePath, DisposalToken);

        // Assert
        listener.AssertNotifications(
            x => x.DocumentRemoved(DocumentFilePath, ownerProject.Key));

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

        var ownerProjectKey1 = await _projectService.AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath1, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);
        var ownerProjectKey2 = await _projectService.AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath2, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);
        await _projectService.AddDocumentAsync(DocumentFilePath, DisposalToken);

        var ownerProject1 = _projectManager.GetLoadedProject(ownerProjectKey1);
        var ownerProject2 = _projectManager.GetLoadedProject(ownerProjectKey2);

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.RemoveDocumentAsync(DocumentFilePath, DisposalToken);

        // Assert
        listener.AssertNotifications(
            x => x.DocumentRemoved(DocumentFilePath, ownerProject1.Key),
            x => x.DocumentRemoved(DocumentFilePath, ownerProject2.Key));

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

        var ownerProjectKey = await _projectService.AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);
        await _projectService.AddDocumentAsync(DocumentFilePath, DisposalToken);
        await _projectService.OpenDocumentAsync(DocumentFilePath, s_emptyText, version: 42, DisposalToken);

        var ownerProject = _projectManager.GetLoadedProject(ownerProjectKey);
        var miscProject = await _snapshotResolver.GetMiscellaneousProjectAsync(DisposalToken);

        Assert.True(_projectManager.IsDocumentOpen(DocumentFilePath));

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.RemoveDocumentAsync(DocumentFilePath, DisposalToken);

        // Assert
        listener.AssertNotifications(
            x => x.DocumentRemoved(DocumentFilePath, ownerProject.Key),
            x => x.DocumentAdded(DocumentFilePath, miscProject.Key));

        Assert.True(_projectManager.IsDocumentOpen(DocumentFilePath));
    }

    [Fact]
    public async Task RemoveDocument_RemovesDocumentFromMiscellaneousProject()
    {
        // Arrange
        const string DocumentFilePath = "document.cshtml";

        await _projectService.AddDocumentAsync(DocumentFilePath, DisposalToken);

        var miscProject = await _snapshotResolver.GetMiscellaneousProjectAsync(DisposalToken);

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
        const string ProjectFilePath = "C:/path/to/project.csproj";
        const string IntermediateOutputPath = "C:/path/to/obj";
        const string RootNamespace = "TestRootNamespace";
        const string DocumentFilePath = "C:/path/to/document.cshtml";

        var ownerProjectKey = await _projectService.AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);

        var ownerProject = _projectManager.GetLoadedProject(ownerProjectKey);

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

        var miscProject = await _snapshotResolver.GetMiscellaneousProjectAsync(DisposalToken);

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

        var ownerProjectKey = await _projectService.AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);
        await _projectService.AddDocumentAsync(DocumentFilePath, DisposalToken);
        await _projectService.OpenDocumentAsync(DocumentFilePath, s_emptyText, version: 42, DisposalToken);

        var ownerProject = _projectManager.GetLoadedProject(ownerProjectKey);

        Assert.True(_projectManager.IsDocumentOpen(DocumentFilePath));

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.UpdateDocumentAsync(DocumentFilePath, s_emptyText.Replace(0, 0, "Hello World"), version: 43, DisposalToken);

        // Assert
        listener.AssertNotifications(
            x => x.DocumentChanged(DocumentFilePath, ownerProject.Key));

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

        var ownerProjectKey1 = await _projectService.AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath1, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);
        var ownerProjectKey2 = await _projectService.AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath2, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);
        await _projectService.AddDocumentAsync(DocumentFilePath, DisposalToken);
        await _projectService.OpenDocumentAsync(DocumentFilePath, s_emptyText, version: 42, DisposalToken);

        var ownerProject1 = _projectManager.GetLoadedProject(ownerProjectKey1);
        var ownerProject2 = _projectManager.GetLoadedProject(ownerProjectKey2);

        Assert.True(_projectManager.IsDocumentOpen(DocumentFilePath));

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.UpdateDocumentAsync(DocumentFilePath, s_emptyText.Replace(0, 0, "Hello World"), version: 43, DisposalToken);

        // Assert
        listener.AssertNotifications(
            x => x.DocumentChanged(DocumentFilePath, ownerProject1.Key),
            x => x.DocumentChanged(DocumentFilePath, ownerProject2.Key));

        Assert.True(_projectManager.IsDocumentOpen(DocumentFilePath));
    }

    [Fact]
    public async Task UpdateDocument_ChangesDocumentInMiscProject()
    {
        // Arrange
        const string DocumentFilePath = "document.cshtml";

        await _projectService.AddDocumentAsync(DocumentFilePath, DisposalToken);
        await _projectService.OpenDocumentAsync(DocumentFilePath, s_emptyText, version: 42, DisposalToken);

        var miscProject = await _snapshotResolver.GetMiscellaneousProjectAsync(DisposalToken);

        Assert.True(_projectManager.IsDocumentOpen(DocumentFilePath));

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.UpdateDocumentAsync(DocumentFilePath, s_emptyText.Replace(0, 0, "Hello World"), version: 43, DisposalToken);

        // Assert
        listener.AssertNotifications(
            x => x.DocumentChanged(DocumentFilePath, miscProject.Key));

        Assert.True(_projectManager.IsDocumentOpen(DocumentFilePath));
    }

    [Fact]
    public async Task UpdateDocument_TracksKnownDocumentVersion()
    {
        // Arrange
        const string ProjectFilePath = "C:/path/to/project.csproj";
        const string IntermediateOutputPath = "C:/path/to/obj";
        const string RootNamespace = "TestRootNamespace";
        const string DocumentFilePath = "C:/path/to/document.cshtml";

        var ownerProjectKey = await _projectService.AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);
        await _projectService.AddDocumentAsync(DocumentFilePath, DisposalToken);

        var ownerProject = _projectManager.GetLoadedProject(ownerProjectKey);

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectService.UpdateDocumentAsync(DocumentFilePath, s_emptyText.Replace(0, 0, "Hello World"), version: 43, DisposalToken);

        // Assert
        listener.AssertNotifications(
            x => x.DocumentChanged(DocumentFilePath, ownerProject.Key));

        var latestVersion = _documentVersionCache.GetLatestDocumentVersion(DocumentFilePath);
        Assert.Equal(43, latestVersion);
    }

    [Fact]
    public async Task UpdateDocument_ThrowsForUnknownDocument()
    {
        // Arrange
        const string ProjectFilePath = "C:/path/to/project.csproj";
        const string IntermediateOutputPath = "C:/path/to/obj";
        const string RootNamespace = "TestRootNamespace";
        const string DocumentFilePath = "C:/path/to/document.cshtml";

        await _projectService.AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath, RazorConfiguration.Default, RootNamespace, displayName: null, DisposalToken);

        // Act
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() =>
        {
            return _projectService.UpdateDocumentAsync(DocumentFilePath, s_emptyText.Replace(0, 0, "Hello World"), version: 43, DisposalToken);
        });
    }

    [Fact]
    public async Task AddProject_AddsProjectWithDefaultConfiguration()
    {
        // Arrange
        const string ProjectFilePath = "C:/path/to/project.csproj";
        const string IntermediateOutputPath = "C:/path/to/obj";

        // Act
        var projectKey = await _projectService.AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath, configuration: null, rootNamespace: null, displayName: null, DisposalToken);

        var project = _projectManager.GetLoadedProject(projectKey);

        // Assert
        Assert.Equal(ProjectFilePath, project.FilePath);
        Assert.Same(FallbackRazorConfiguration.Latest, project.Configuration);
        Assert.Null(project.RootNamespace);
    }

    [Fact]
    public async Task AddProject_AddsProjectWithSpecifiedConfiguration()
    {
        // Arrange
        const string ProjectFilePath = "C:/path/to/project.csproj";
        const string IntermediateOutputPath = "C:/path/to/obj";
        const string RootNamespace = "My.Root.Namespace";

        var configuration = new RazorConfiguration(RazorLanguageVersion.Version_1_0, "TestName", Extensions: []);

        // Act
        var projectKey = await _projectService.AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath, configuration, RootNamespace, displayName: null, DisposalToken);

        var project = _projectManager.GetLoadedProject(projectKey);

        // Assert
        Assert.Equal(ProjectFilePath, project.FilePath);
        Assert.Same(configuration, project.Configuration);
        Assert.Equal(RootNamespace, project.RootNamespace);
    }

    [Fact]
    public async Task AddProject_DoesNotMigrateMiscellaneousDocumentIfNewProjectNotACandidate()
    {
        // Arrange
        const string ProjectFilePath = "C:/other-path/to/project.csproj";
        const string IntermediateOutputPath = "C:/other-path/to/obj";
        const string DocumentFilePath1 = "C:/path/to/document1.cshtml";
        const string DocumentFilePath2 = "C:/path/to/document2.cshtml";

        var miscProject = await _snapshotResolver.GetMiscellaneousProjectAsync(DisposalToken);

        await _projectManager.UpdateAsync(updater =>
        {
            updater.DocumentAdded(miscProject.Key,
                new HostDocument(DocumentFilePath1, "document1.cshtml"), CreateEmptyTextLoader());
            updater.DocumentAdded(miscProject.Key,
                new HostDocument(DocumentFilePath2, "document2.cshtml"), CreateEmptyTextLoader());
        });

        using var listener = _projectManager.ListenToNotifications();

        // Act
        var newProjectKey = await _projectService.AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath, RazorConfiguration.Default, rootNamespace: null, displayName: null, DisposalToken);

        // Assert
        listener.AssertNotifications(
            x => x.ProjectAdded(ProjectFilePath, newProjectKey));
    }

    [Fact]
    public async Task AddProject_MigratesMiscellaneousDocumentsToNewOwnerProject()
    {
        // Arrange
        const string ProjectFilePath = "C:/path/to/project.csproj";
        const string IntermediateOutputPath = "C:/path/to/obj";
        const string DocumentFilePath1 = "C:/path/to/document1.cshtml";
        const string DocumentFilePath2 = "C:/path/to/document2.cshtml";

        var miscProject = await _snapshotResolver.GetMiscellaneousProjectAsync(DisposalToken);

        await _projectManager.UpdateAsync(updater =>
        {
            updater.DocumentAdded(miscProject.Key,
                new HostDocument(DocumentFilePath1, "document1.cshtml"), CreateEmptyTextLoader());
            updater.DocumentAdded(miscProject.Key,
                new HostDocument(DocumentFilePath2, "document2.cshtml"), CreateEmptyTextLoader());
        });

        using var listener = _projectManager.ListenToNotifications();

        // Act
        var newProjectKey = await _projectService.AddProjectAsync(
            ProjectFilePath, IntermediateOutputPath, RazorConfiguration.Default, rootNamespace: null, displayName: null, DisposalToken);

        // Assert

        // AddProject iterates through a dictionary to migrate documents, so the order of the documents is not deterministic. In the real world
        // the adds and removes are interleaved per document
        listener.OrderBy(e => e.Kind).ThenBy(e => e.DocumentFilePath).AssertNotifications(
            x => x.ProjectAdded(ProjectFilePath, newProjectKey),
            x => x.DocumentAdded(DocumentFilePath1, newProjectKey),
            x => x.DocumentAdded(DocumentFilePath2, newProjectKey),
            x => x.DocumentRemoved(DocumentFilePath1, miscProject.Key),
            x => x.DocumentRemoved(DocumentFilePath2, miscProject.Key));
    }

    private static TextLoader CreateEmptyTextLoader()
    {
        var textLoaderMock = new StrictMock<TextLoader>();
        textLoaderMock
            .Setup(x => x.LoadTextAndVersionAsync(It.IsAny<LoadTextOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TextAndVersion.Create(s_emptyText, VersionStamp.Create()));

        return textLoaderMock.Object;
    }
}
