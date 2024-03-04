// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LiveShare.Razor.Test;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LiveShare.Razor.Guest;

public class ProjectSnapshotSynchronizationServiceTest : VisualStudioWorkspaceTestBase
{
    private readonly CollaborationSession _sessionContext;
    private readonly IProjectSnapshotManagerAccessor _projectManagerAccessor;
    private readonly ProjectWorkspaceState _projectWorkspaceStateWithTagHelpers;

    public ProjectSnapshotSynchronizationServiceTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _sessionContext = new TestCollaborationSession(isHost: false);

        var projectManager = new TestProjectSnapshotManager(
            ProjectEngineFactoryProvider,
            Dispatcher);

        var projectManagerAccessorMock = new StrictMock<IProjectSnapshotManagerAccessor>();
        projectManagerAccessorMock
            .SetupGet(x => x.Instance)
            .Returns(projectManager);

        _projectManagerAccessor = projectManagerAccessorMock.Object;

        _projectWorkspaceStateWithTagHelpers = ProjectWorkspaceState.Create(ImmutableArray.Create(
            TagHelperDescriptorBuilder.Create("TestTagHelper", "TestAssembly").Build()));
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
            _projectManagerAccessor,
            Dispatcher,
            ErrorReporter,
            JoinableTaskFactory);

        // Act
        await synchronizationService.InitializeAsync(DisposalToken);

        // Assert
        var projects = await Dispatcher.RunAsync(
            _projectManagerAccessor.Instance.GetProjects, DisposalToken);
        var project = Assert.Single(projects);
        Assert.Equal("/guest/path/project.csproj", project.FilePath);
        Assert.Same(RazorConfiguration.Default, project.Configuration);

        var tagHelpers = await project.GetTagHelpersAsync(CancellationToken.None);
        Assert.Equal(_projectWorkspaceStateWithTagHelpers.TagHelpers.Length, tagHelpers.Length);
        for (var i = 0; i < _projectWorkspaceStateWithTagHelpers.TagHelpers.Length; i++)
        {
            Assert.Same(_projectWorkspaceStateWithTagHelpers.TagHelpers[i], tagHelpers[i]);
        }
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
            _projectManagerAccessor,
            Dispatcher,
            ErrorReporter,
            JoinableTaskFactory);
        var args = new ProjectChangeEventProxyArgs(older: null, newHandle, ProjectProxyChangeKind.ProjectAdded);

        // Act
        await synchronizationService.UpdateGuestProjectManagerAsync(args);

        // Assert
        var projects = await Dispatcher.RunAsync(
            _projectManagerAccessor.Instance.GetProjects, DisposalToken);
        var project = Assert.Single(projects);
        Assert.Equal("/guest/path/project.csproj", project.FilePath);
        Assert.Same(RazorConfiguration.Default, project.Configuration);

        var tagHelpers = await project.GetTagHelpersAsync(CancellationToken.None);
        Assert.Equal(_projectWorkspaceStateWithTagHelpers.TagHelpers.Length, tagHelpers.Length);
        for (var i = 0; i < _projectWorkspaceStateWithTagHelpers.TagHelpers.Length; i++)
        {
            Assert.Same(_projectWorkspaceStateWithTagHelpers.TagHelpers[i], tagHelpers[i]);
        }
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
            _projectManagerAccessor,
            Dispatcher,
            ErrorReporter,
            JoinableTaskFactory);
        var hostProject = new HostProject("/guest/path/project.csproj", "/guest/path/obj", RazorConfiguration.Default, "project");

        await Dispatcher.RunAsync(
            () => _projectManagerAccessor.Instance.ProjectAdded(hostProject),
            DisposalToken);
        var args = new ProjectChangeEventProxyArgs(olderHandle, newer: null, ProjectProxyChangeKind.ProjectRemoved);

        // Act
        await synchronizationService.UpdateGuestProjectManagerAsync(args);

        // Assert
        var projects = await Dispatcher.RunAsync(
            _projectManagerAccessor.Instance.GetProjects, DisposalToken);
        Assert.Empty(projects);
    }

    [UIFact]
    public async Task UpdateGuestProjectManager_ProjectChanged_ConfigurationChange()
    {
        // Arrange
        var oldHandle = new ProjectSnapshotHandleProxy(
            new Uri("vsls:/path/project.csproj"),
            new Uri("vsls:/path/obj"),
            RazorConfiguration.Default,
            "project",
            ProjectWorkspaceState.Default);
        var newConfiguration = RazorConfiguration.Create(RazorLanguageVersion.Version_1_0, "Custom-1.0", []);
        var newHandle = new ProjectSnapshotHandleProxy(
            oldHandle.FilePath,
            oldHandle.IntermediateOutputPath,
            newConfiguration,
            oldHandle.RootNamespace,
            oldHandle.ProjectWorkspaceState);
        var synchronizationService = new ProjectSnapshotSynchronizationService(
            _sessionContext,
            StrictMock.Of<IProjectSnapshotManagerProxy>(),
            _projectManagerAccessor,
            Dispatcher,
            ErrorReporter,
            JoinableTaskFactory);
        var hostProject = new HostProject("/guest/path/project.csproj", "/guest/path/obj", RazorConfiguration.Default, "project");
        await Dispatcher.RunAsync(() =>
        {
            var projectManager = _projectManagerAccessor.Instance;
            projectManager.ProjectAdded(hostProject);
            projectManager.ProjectConfigurationChanged(hostProject);
        }, DisposalToken);
        var args = new ProjectChangeEventProxyArgs(oldHandle, newHandle, ProjectProxyChangeKind.ProjectChanged);

        // Act
        await synchronizationService.UpdateGuestProjectManagerAsync(args);

        // Assert
        var projects = await Dispatcher.RunAsync(
            _projectManagerAccessor.Instance.GetProjects, DisposalToken);
        var project = Assert.Single(projects);
        Assert.Equal("/guest/path/project.csproj", project.FilePath);
        Assert.Same(newConfiguration, project.Configuration);
        Assert.Empty(await project.GetTagHelpersAsync(CancellationToken.None));
    }

    [UIFact]
    public async Task UpdateGuestProjectManager_ProjectChanged_ProjectWorkspaceStateChange()
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
            _projectManagerAccessor,
            Dispatcher,
            ErrorReporter,
            JoinableTaskFactory);
        var hostProject = new HostProject("/guest/path/project.csproj", "/guest/path/obj", RazorConfiguration.Default, "project");
        await Dispatcher.RunAsync(() =>
        {
            var projectManager = _projectManagerAccessor.Instance;
            projectManager.ProjectAdded(hostProject);
            projectManager.ProjectWorkspaceStateChanged(hostProject.Key, oldHandle.ProjectWorkspaceState);
        }, DisposalToken);
        var args = new ProjectChangeEventProxyArgs(oldHandle, newHandle, ProjectProxyChangeKind.ProjectChanged);

        // Act
        await synchronizationService.UpdateGuestProjectManagerAsync(args);

        // Assert
        var projects = await Dispatcher.RunAsync(
            _projectManagerAccessor.Instance.GetProjects, DisposalToken);
        var project = Assert.Single(projects);
        Assert.Equal("/guest/path/project.csproj", project.FilePath);
        Assert.Same(RazorConfiguration.Default, project.Configuration);

        var tagHelpers = await project.GetTagHelpersAsync(CancellationToken.None);
        Assert.Equal(_projectWorkspaceStateWithTagHelpers.TagHelpers.Length, tagHelpers.Length);
        for (var i = 0; i < _projectWorkspaceStateWithTagHelpers.TagHelpers.Length; i++)
        {
            Assert.Same(_projectWorkspaceStateWithTagHelpers.TagHelpers[i], tagHelpers[i]);
        }
    }
}
