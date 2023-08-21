// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.LiveShare.Razor.Test;
using Microsoft.VisualStudio.Threading;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LiveShare.Razor.Host;

public class DefaultProjectSnapshotManagerProxyTest : ProjectSnapshotManagerDispatcherTestBase
{
    private readonly Workspace _workspace;
    private readonly IProjectSnapshot _projectSnapshot1;
    private readonly IProjectSnapshot _projectSnapshot2;

    public DefaultProjectSnapshotManagerProxyTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _workspace = TestWorkspace.Create();
        AddDisposable(_workspace);

        var projectWorkspaceState1 = new ProjectWorkspaceState(ImmutableArray.Create(
            TagHelperDescriptorBuilder.Create("test1", "TestAssembly1").Build()),
            csharpLanguageVersion: default);

        _projectSnapshot1 = new ProjectSnapshot(
            ProjectState.Create(
                _workspace.Services,
                new HostProject("/host/path/to/project1.csproj", "/host/path/to/obj", RazorConfiguration.Default, "project1"),
                projectWorkspaceState1));

        var projectWorkspaceState2 = new ProjectWorkspaceState(ImmutableArray.Create(
            TagHelperDescriptorBuilder.Create("test2", "TestAssembly2").Build()),
            csharpLanguageVersion: default);

        _projectSnapshot2 = new ProjectSnapshot(
            ProjectState.Create(
                _workspace.Services,
                new HostProject("/host/path/to/project2.csproj", "/host/path/to/obj", RazorConfiguration.Default, "project2"),
                projectWorkspaceState2));
    }

    [Fact]
    public async Task CalculateUpdatedStateAsync_ReturnsStateForAllProjects()
    {
        // Arrange
        var projectSnapshotManager = new TestProjectSnapshotManager(_projectSnapshot1, _projectSnapshot2);
        using var proxy = new DefaultProjectSnapshotManagerProxy(
            new TestCollaborationSession(true),
            Dispatcher,
            projectSnapshotManager,
            JoinableTaskFactory);

        // Act
        var state = await JoinableTaskFactory.RunAsync(() => proxy.CalculateUpdatedStateAsync(projectSnapshotManager.GetProjects()));

        // Assert
        Assert.Collection(
            state.ProjectHandles,
            handle =>
            {
                Assert.Equal("vsls:/path/to/project1.csproj", handle.FilePath.ToString());
                Assert.Equal(_projectSnapshot1.TagHelpers, handle.ProjectWorkspaceState.TagHelpers, TagHelperDescriptorComparer.Default);
            },
            handle =>
            {
                Assert.Equal("vsls:/path/to/project2.csproj", handle.FilePath.ToString());
                Assert.Equal(_projectSnapshot2.TagHelpers, handle.ProjectWorkspaceState.TagHelpers, TagHelperDescriptorComparer.Default);
            });
    }

    [Fact]
    public async Task Changed_TriggersOnSnapshotManagerChanged()
    {
        // Arrange
        var projectSnapshotManager = new TestProjectSnapshotManager(_projectSnapshot1);
        using var proxy = new DefaultProjectSnapshotManagerProxy(
            new TestCollaborationSession(true),
            Dispatcher,
            projectSnapshotManager,
            JoinableTaskFactory);
        var changedArgs = new ProjectChangeEventArgs(_projectSnapshot1, _projectSnapshot1, ProjectChangeKind.ProjectChanged);
        var called = false;
        proxy.Changed += (sender, args) =>
        {
            called = true;
            Assert.Equal($"vsls:/path/to/project1.csproj", args.ProjectFilePath.ToString());
            Assert.Equal(ProjectProxyChangeKind.ProjectChanged, args.Kind);
            Assert.Equal("vsls:/path/to/project1.csproj", args.Newer.FilePath.ToString());
        };

        // Act
        projectSnapshotManager.TriggerChanged(changedArgs);
        await proxy._processingChangedEventTestTask.JoinAsync();

        // Assert
        Assert.True(called);
    }

    [Fact]
    public void Changed_NoopsIfProxyDisposed()
    {
        // Arrange
        var projectSnapshotManager = new TestProjectSnapshotManager(_projectSnapshot1);
        var proxy = new DefaultProjectSnapshotManagerProxy(
            new TestCollaborationSession(true),
            Dispatcher,
            projectSnapshotManager,
            JoinableTaskFactory);
        var changedArgs = new ProjectChangeEventArgs(_projectSnapshot1, _projectSnapshot1, ProjectChangeKind.ProjectChanged);
        proxy.Changed += (sender, args) => throw new InvalidOperationException("Should not have been called.");
        proxy.Dispose();

        // Act
        projectSnapshotManager.TriggerChanged(changedArgs);

        // Assert
        Assert.Null(proxy._processingChangedEventTestTask);
    }

    [Fact]
    public async Task GetLatestProjectsAsync_ReturnsSnapshotManagerProjects()
    {
        // Arrange
        var projectSnapshotManager = new TestProjectSnapshotManager(_projectSnapshot1);
        using var proxy = new DefaultProjectSnapshotManagerProxy(
            new TestCollaborationSession(true),
            Dispatcher,
            projectSnapshotManager,
            JoinableTaskFactory);

        // Act
        var projects = await proxy.GetLatestProjectsAsync();

        // Assert
        var project = Assert.Single(projects);
        Assert.Same(_projectSnapshot1, project);
    }

    [Fact]
    public async Task GetStateAsync_ReturnsProjectState()
    {
        // Arrange
        var projectSnapshotManager = new TestProjectSnapshotManager(_projectSnapshot1, _projectSnapshot2);
        using var proxy = new DefaultProjectSnapshotManagerProxy(
            new TestCollaborationSession(true),
            Dispatcher,
            projectSnapshotManager,
            JoinableTaskFactory);

        // Act
        var state = await JoinableTaskFactory.RunAsync(() => proxy.GetProjectManagerStateAsync(DisposalToken));

        // Assert
        Assert.Collection(
            state.ProjectHandles,
            handle =>
            {
                Assert.Equal("vsls:/path/to/project1.csproj", handle.FilePath.ToString());
                Assert.Equal(_projectSnapshot1.TagHelpers, handle.ProjectWorkspaceState.TagHelpers, TagHelperDescriptorComparer.Default);
            },
            handle =>
            {
                Assert.Equal("vsls:/path/to/project2.csproj", handle.FilePath.ToString());
                Assert.Equal(_projectSnapshot2.TagHelpers, handle.ProjectWorkspaceState.TagHelpers, TagHelperDescriptorComparer.Default);
            });
    }

    [Fact]
    public async Task GetStateAsync_CachesState()
    {
        // Arrange
        var projectSnapshotManager = new TestProjectSnapshotManager(_projectSnapshot1);
        using var proxy = new DefaultProjectSnapshotManagerProxy(
            new TestCollaborationSession(true),
            Dispatcher,
            projectSnapshotManager,
            JoinableTaskFactory);

        // Act
        var state1 = await JoinableTaskFactory.RunAsync(() => proxy.GetProjectManagerStateAsync(DisposalToken));
        var state2 = await JoinableTaskFactory.RunAsync(() => proxy.GetProjectManagerStateAsync(DisposalToken));

        // Assert
        Assert.Same(state1, state2);
    }

    private class TestProjectSnapshotManager : ProjectSnapshotManager
    {
        private ImmutableArray<IProjectSnapshot> _projects;
        public TestProjectSnapshotManager(params IProjectSnapshot[] projects)
        {
            _projects = projects.ToImmutableArray();
        }

        public override ImmutableArray<IProjectSnapshot> GetProjects() => _projects;

        public override event EventHandler<ProjectChangeEventArgs> Changed;

        public void TriggerChanged(ProjectChangeEventArgs args)
        {
            Changed?.Invoke(this, args);
        }

        public override IProjectSnapshot GetLoadedProject(ProjectKey projectKey)
        {
            throw new NotImplementedException();
        }

        public override ImmutableArray<ProjectKey> GetAllProjectKeys(string projectFileName)
        {
            throw new NotImplementedException();
        }

        public override bool IsDocumentOpen(string documentFilePath)
        {
            throw new NotImplementedException();
        }
    }
}
