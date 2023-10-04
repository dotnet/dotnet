// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.LiveShare.Razor.Test;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LiveShare.Razor.Guest;

public class ProjectSnapshotSynchronizationServiceTest : WorkspaceTestBase
{
    private readonly CollaborationSession _sessionContext;
    private readonly TestProjectSnapshotManager _projectSnapshotManager;
    private readonly ProjectWorkspaceState _projectWorkspaceStateWithTagHelpers;

    public ProjectSnapshotSynchronizationServiceTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _sessionContext = new TestCollaborationSession(isHost: false);

        _projectSnapshotManager = new TestProjectSnapshotManager(Workspace, new TestProjectSnapshotManagerDispatcher());

        _projectWorkspaceStateWithTagHelpers = new ProjectWorkspaceState(ImmutableArray.Create(
            TagHelperDescriptorBuilder.Create("TestTagHelper", "TestAssembly").Build()),
            csharpLanguageVersion: default);
    }

    [Fact]
    public async Task InitializeAsync_RetrievesHostProjectManagerStateAndInitializesGuestManager()
    {
        // Arrange
        var projectHandle = new ProjectSnapshotHandleProxy(
            new Uri("vsls:/path/project.csproj"),
            new Uri("vsls:/path/obj"),
            RazorConfiguration.Default,
            "project",
            _projectWorkspaceStateWithTagHelpers);
        var state = new ProjectSnapshotManagerProxyState(new[] { projectHandle });
        var hostProjectManagerProxy = Mock.Of<IProjectSnapshotManagerProxy>(
            proxy => proxy.GetProjectManagerStateAsync(It.IsAny<CancellationToken>()) == Task.FromResult(state), MockBehavior.Strict);
        var synchronizationService = new ProjectSnapshotSynchronizationService(
            JoinableTaskFactory,
            _sessionContext,
            hostProjectManagerProxy,
            _projectSnapshotManager);

        // Act
        await synchronizationService.InitializeAsync(DisposalToken);

        // Assert
        var project = Assert.Single(_projectSnapshotManager.GetProjects());
        Assert.Equal("/guest/path/project.csproj", project.FilePath);
        Assert.Same(RazorConfiguration.Default, project.Configuration);

        Assert.Equal(_projectWorkspaceStateWithTagHelpers.TagHelpers.Length, project.TagHelpers.Length);
        for (var i = 0; i < _projectWorkspaceStateWithTagHelpers.TagHelpers.Length; i++)
        {
            Assert.Same(_projectWorkspaceStateWithTagHelpers.TagHelpers[i], project.TagHelpers[i]);
        }
    }

    [Fact]
    public void UpdateGuestProjectManager_ProjectAdded()
    {
        // Arrange
        var newHandle = new ProjectSnapshotHandleProxy(
            new Uri("vsls:/path/project.csproj"),
            new Uri("vsls:/path/obj"),
            RazorConfiguration.Default,
            "project",
            _projectWorkspaceStateWithTagHelpers);
        var synchronizationService = new ProjectSnapshotSynchronizationService(
            JoinableTaskFactory,
            _sessionContext,
            Mock.Of<IProjectSnapshotManagerProxy>(MockBehavior.Strict),
            _projectSnapshotManager);
        var args = new ProjectChangeEventProxyArgs(older: null, newHandle, ProjectProxyChangeKind.ProjectAdded);

        // Act
        synchronizationService.UpdateGuestProjectManager(args);

        // Assert
        var project = Assert.Single(_projectSnapshotManager.GetProjects());
        Assert.Equal("/guest/path/project.csproj", project.FilePath);
        Assert.Same(RazorConfiguration.Default, project.Configuration);

        Assert.Equal(_projectWorkspaceStateWithTagHelpers.TagHelpers.Length, project.TagHelpers.Length);
        for (var i = 0; i < _projectWorkspaceStateWithTagHelpers.TagHelpers.Length; i++)
        {
            Assert.Same(_projectWorkspaceStateWithTagHelpers.TagHelpers[i], project.TagHelpers[i]);
        }
    }

    [Fact]
    public void UpdateGuestProjectManager_ProjectRemoved()
    {
        // Arrange
        var olderHandle = new ProjectSnapshotHandleProxy(
            new Uri("vsls:/path/project.csproj"),
            new Uri("vsls:/path/obj"),
            RazorConfiguration.Default,
            "project",
            projectWorkspaceState: null);
        var synchronizationService = new ProjectSnapshotSynchronizationService(
            JoinableTaskFactory,
            _sessionContext,
            Mock.Of<IProjectSnapshotManagerProxy>(MockBehavior.Strict),
            _projectSnapshotManager);
        var hostProject = new HostProject("/guest/path/project.csproj", "/guest/path/obj", RazorConfiguration.Default, "project");
        _projectSnapshotManager.ProjectAdded(hostProject);
        var args = new ProjectChangeEventProxyArgs(olderHandle, newer: null, ProjectProxyChangeKind.ProjectRemoved);

        // Act
        synchronizationService.UpdateGuestProjectManager(args);

        // Assert
        Assert.Empty(_projectSnapshotManager.GetProjects());
    }

    [Fact]
    public void UpdateGuestProjectManager_ProjectChanged_ConfigurationChange()
    {
        // Arrange
        var oldHandle = new ProjectSnapshotHandleProxy(
            new Uri("vsls:/path/project.csproj"),
            new Uri("vsls:/path/obj"),
            RazorConfiguration.Default,
            "project",
            projectWorkspaceState: null);
        var newConfiguration = RazorConfiguration.Create(RazorLanguageVersion.Version_1_0, "Custom-1.0", Enumerable.Empty<RazorExtension>());
        var newHandle = new ProjectSnapshotHandleProxy(
            oldHandle.FilePath,
            oldHandle.IntermediateOutputPath,
            newConfiguration,
            oldHandle.RootNamespace,
            oldHandle.ProjectWorkspaceState);
        var synchronizationService = new ProjectSnapshotSynchronizationService(
            JoinableTaskFactory,
            _sessionContext,
            Mock.Of<IProjectSnapshotManagerProxy>(MockBehavior.Strict),
            _projectSnapshotManager);
        var hostProject = new HostProject("/guest/path/project.csproj", "/guest/path/obj", RazorConfiguration.Default, "project");
        _projectSnapshotManager.ProjectAdded(hostProject);
        _projectSnapshotManager.ProjectConfigurationChanged(hostProject);
        var args = new ProjectChangeEventProxyArgs(oldHandle, newHandle, ProjectProxyChangeKind.ProjectChanged);

        // Act
        synchronizationService.UpdateGuestProjectManager(args);

        // Assert
        var project = Assert.Single(_projectSnapshotManager.GetProjects());
        Assert.Equal("/guest/path/project.csproj", project.FilePath);
        Assert.Same(newConfiguration, project.Configuration);
        Assert.Empty(project.TagHelpers);
    }

    [Fact]
    public void UpdateGuestProjectManager_ProjectChanged_ProjectWorkspaceStateChange()
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
            JoinableTaskFactory,
            _sessionContext,
            Mock.Of<IProjectSnapshotManagerProxy>(MockBehavior.Strict),
            _projectSnapshotManager);
        var hostProject = new HostProject("/guest/path/project.csproj", "/guest/path/obj", RazorConfiguration.Default, "project");
        _projectSnapshotManager.ProjectAdded(hostProject);
        _projectSnapshotManager.ProjectWorkspaceStateChanged(hostProject.Key, oldHandle.ProjectWorkspaceState);
        var args = new ProjectChangeEventProxyArgs(oldHandle, newHandle, ProjectProxyChangeKind.ProjectChanged);

        // Act
        synchronizationService.UpdateGuestProjectManager(args);

        // Assert
        var project = Assert.Single(_projectSnapshotManager.GetProjects());
        Assert.Equal("/guest/path/project.csproj", project.FilePath);
        Assert.Same(RazorConfiguration.Default, project.Configuration);

        Assert.Equal(_projectWorkspaceStateWithTagHelpers.TagHelpers.Length, project.TagHelpers.Length);
        for (var i = 0; i < _projectWorkspaceStateWithTagHelpers.TagHelpers.Length; i++)
        {
            Assert.Same(_projectWorkspaceStateWithTagHelpers.TagHelpers[i], project.TagHelpers[i]);
        }
    }
}
