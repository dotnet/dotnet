// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.LiveShare;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LiveShare.Guest;

public class ProjectSnapshotSynchronizationServiceTest : VisualStudioWorkspaceTestBase
{
    private readonly CollaborationSession _sessionContext;
    private readonly TestProjectSnapshotManager _projectManager;
    private readonly ProjectWorkspaceState _projectWorkspaceStateWithTagHelpers;

    public ProjectSnapshotSynchronizationServiceTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _sessionContext = new TestCollaborationSession(isHost: false);

        _projectManager = CreateProjectSnapshotManager();

        _projectWorkspaceStateWithTagHelpers = ProjectWorkspaceState.Create(
            tagHelpers: [TagHelperDescriptorBuilder.CreateTagHelper("TestTagHelper", "TestAssembly").Build()]);
    }

    [UIFact]
    public async Task InitializeAsync_RetrievesHostProjectManagerStateAndInitializesGuestManager()
    {
        // Arrange
        var projectHandle = new ProjectSnapshotHandleProxy(
            new Uri("vsls:/path/project.csproj"),
            new Uri("vsls:/path/obj"),
            RazorConfiguration.Default,
            "project",
            _projectWorkspaceStateWithTagHelpers);
        var state = new ProjectSnapshotManagerProxyState([projectHandle]);
        var hostProjectManagerProxyMock = new StrictMock<IProjectSnapshotManagerProxy>();
        hostProjectManagerProxyMock
            .Setup(x => x.GetProjectManagerStateAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(state);

        var synchronizationService = new ProjectSnapshotSynchronizationService(
            _sessionContext,
            hostProjectManagerProxyMock.Object,
            _projectManager,
            LoggerFactory,
            JoinableTaskFactory);

        // Act
        await synchronizationService.InitializeAsync(DisposalToken);

        // Assert
        var projects = _projectManager.GetProjects();
        var project = Assert.Single(projects);
        Assert.Equal("/guest/path/project.csproj", project.FilePath);
        Assert.Same(RazorConfiguration.Default, project.Configuration);

        var tagHelpers = await project.GetTagHelpersAsync(DisposalToken);
        Assert.SameItems(_projectWorkspaceStateWithTagHelpers.TagHelpers, tagHelpers);
    }

    [UIFact]
    public async Task UpdateGuestProjectManager_ProjectAdded()
    {
        // Arrange
        var newHandle = new ProjectSnapshotHandleProxy(
            new Uri("vsls:/path/project.csproj"),
            new Uri("vsls:/path/obj"),
            RazorConfiguration.Default,
            "project",
            _projectWorkspaceStateWithTagHelpers);
        var synchronizationService = new ProjectSnapshotSynchronizationService(
            _sessionContext,
            StrictMock.Of<IProjectSnapshotManagerProxy>(),
            _projectManager,
            LoggerFactory,
            JoinableTaskFactory);
        var args = new ProjectChangeEventProxyArgs(older: null, newHandle, ProjectProxyChangeKind.ProjectAdded);

        // Act
        await synchronizationService.UpdateGuestProjectManagerAsync(args);

        // Assert
        var projects = _projectManager.GetProjects();
        var project = Assert.Single(projects);
        Assert.Equal("/guest/path/project.csproj", project.FilePath);
        Assert.Same(RazorConfiguration.Default, project.Configuration);

        var tagHelpers = await project.GetTagHelpersAsync(DisposalToken);
        Assert.SameItems(_projectWorkspaceStateWithTagHelpers.TagHelpers, tagHelpers);
    }

    [UIFact]
    public async Task UpdateGuestProjectManager_ProjectRemoved()
    {
        // Arrange
        var olderHandle = new ProjectSnapshotHandleProxy(
            new Uri("vsls:/path/project.csproj"),
            new Uri("vsls:/path/obj"),
            RazorConfiguration.Default,
            "project",
            ProjectWorkspaceState.Default);
        var synchronizationService = new ProjectSnapshotSynchronizationService(
            _sessionContext,
            StrictMock.Of<IProjectSnapshotManagerProxy>(),
            _projectManager,
            LoggerFactory,
            JoinableTaskFactory);
        var hostProject = new HostProject("/guest/path/project.csproj", "/guest/path/obj", RazorConfiguration.Default, "project");

        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(hostProject);
        });

        var args = new ProjectChangeEventProxyArgs(olderHandle, newer: null, ProjectProxyChangeKind.ProjectRemoved);

        // Act
        await synchronizationService.UpdateGuestProjectManagerAsync(args);

        // Assert
        var projects = _projectManager.GetProjects();
        Assert.Empty(projects);
    }

    [UIFact]
    public async Task UpdateGuestProjectManager_ProjectChanged_UpdateProjectConfiguration()
    {
        // Arrange
        var oldHandle = new ProjectSnapshotHandleProxy(
            new Uri("vsls:/path/project.csproj"),
            new Uri("vsls:/path/obj"),
            RazorConfiguration.Default,
            "project",
            ProjectWorkspaceState.Default);
        var newConfiguration = new RazorConfiguration(RazorLanguageVersion.Version_1_0, "Custom-1.0", Extensions: []);
        var newHandle = new ProjectSnapshotHandleProxy(
            oldHandle.FilePath,
            oldHandle.IntermediateOutputPath,
            newConfiguration,
            oldHandle.RootNamespace,
            oldHandle.ProjectWorkspaceState);
        var synchronizationService = new ProjectSnapshotSynchronizationService(
            _sessionContext,
            StrictMock.Of<IProjectSnapshotManagerProxy>(),
            _projectManager,
            LoggerFactory,
            JoinableTaskFactory);
        var hostProject = new HostProject("/guest/path/project.csproj", "/guest/path/obj", RazorConfiguration.Default, "project");

        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(hostProject);
            updater.UpdateProjectConfiguration(hostProject);
        });

        var args = new ProjectChangeEventProxyArgs(oldHandle, newHandle, ProjectProxyChangeKind.ProjectChanged);

        // Act
        await synchronizationService.UpdateGuestProjectManagerAsync(args);

        // Assert
        var projects = _projectManager.GetProjects();
        var project = Assert.Single(projects);
        Assert.Equal("/guest/path/project.csproj", project.FilePath);
        Assert.Same(newConfiguration, project.Configuration);
        Assert.Empty(await project.GetTagHelpersAsync(DisposalToken));
    }

    [UIFact]
    public async Task UpdateGuestProjectManager_ProjectChanged_UpdateProjectWorkspaceState()
    {
        // Arrange
        var oldHandle = new ProjectSnapshotHandleProxy(
            new Uri("vsls:/path/project.csproj"),
            new Uri("vsls:/path/obj"),
            RazorConfiguration.Default,
            "project",
            ProjectWorkspaceState.Default);
        var newProjectWorkspaceState = _projectWorkspaceStateWithTagHelpers;
        var newHandle = new ProjectSnapshotHandleProxy(
            oldHandle.FilePath,
            oldHandle.IntermediateOutputPath,
            oldHandle.Configuration,
            oldHandle.RootNamespace,
            newProjectWorkspaceState);
        var synchronizationService = new ProjectSnapshotSynchronizationService(
            _sessionContext,
            StrictMock.Of<IProjectSnapshotManagerProxy>(),
            _projectManager,
            LoggerFactory,
            JoinableTaskFactory);
        var hostProject = new HostProject("/guest/path/project.csproj", "/guest/path/obj", RazorConfiguration.Default, "project");

        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(hostProject);
            updater.UpdateProjectWorkspaceState(hostProject.Key, oldHandle.ProjectWorkspaceState);
        });

        var args = new ProjectChangeEventProxyArgs(oldHandle, newHandle, ProjectProxyChangeKind.ProjectChanged);

        // Act
        await synchronizationService.UpdateGuestProjectManagerAsync(args);

        // Assert
        var projects = _projectManager.GetProjects();
        var project = Assert.Single(projects);
        Assert.Equal("/guest/path/project.csproj", project.FilePath);
        Assert.Same(RazorConfiguration.Default, project.Configuration);

        var tagHelpers = await project.GetTagHelpersAsync(DisposalToken);
        Assert.SameItems(_projectWorkspaceStateWithTagHelpers.TagHelpers, tagHelpers);
    }
}
